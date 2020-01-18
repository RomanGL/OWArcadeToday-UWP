using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace OWArcadeToday.Core.JsonConverters
{
    public sealed class TolerantEnumConverter : JsonConverter
    {
        #region Fields

        [ThreadStatic]
        private Dictionary<Type, Dictionary<string, object>> fromValueMap; // string representation to Enum value map

        [ThreadStatic]
        private Dictionary<Type, Dictionary<object, string>> toValueMap; // Enum value to string map

        #endregion

        #region Properties

        public string UnknownValue { get; set; } = "Unknown";

        #endregion

        #region Overrides of JsonConverter

        public override bool CanConvert(Type objectType)
        {
            var type = IsNullableType(objectType)
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            return type?.IsEnum == true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isNullable = IsNullableType(objectType);
            var enumType = isNullable
                ? Nullable.GetUnderlyingType(objectType) ?? objectType
                : objectType;

            InitMap(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                var enumText = reader.Value.ToString();

                var val = FromValue(enumType, enumText);
                if (val != null)
                    return val;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                var enumVal = Convert.ToInt32(reader.Value);
                var values = (int[])Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                var names = Enum.GetNames(enumType);
                var unknownName = names.FirstOrDefault(n => string.Equals(n, UnknownValue, StringComparison.OrdinalIgnoreCase));
                if (unknownName == null)
                {
                    throw new JsonSerializationException($"Unable to parse '{reader.Value}' to enum {enumType}. Consider adding Unknown as fail-back value.");
                }

                return Enum.Parse(enumType, unknownName);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumType = value.GetType();

            InitMap(enumType);

            var val = ToValue(enumType, value);

            writer.WriteValue(val);
        }

        #endregion

        #region Private methods

        private bool IsNullableType(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);

        private void InitMap(Type enumType)
        {
            if (fromValueMap == null)
            {
                fromValueMap = new Dictionary<Type, Dictionary<string, object>>();
            }

            if (toValueMap == null)
            {
                toValueMap = new Dictionary<Type, Dictionary<object, string>>();
            }

            if (fromValueMap.ContainsKey(enumType))
                return; // already initialized

            var fromMap = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            var toMap = new Dictionary<object, string>();

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                var name = field.Name;
                var enumValue = Enum.Parse(enumType, name);

                // use EnumMember attribute if exists
                var enumMemberAttrbiute = field.GetCustomAttribute<EnumMemberAttribute>();

                if (enumMemberAttrbiute != null)
                {
                    var enumMemberValue = enumMemberAttrbiute.Value;

                    fromMap[enumMemberValue] = enumValue;
                    toMap[enumValue] = enumMemberValue;
                }
                else
                {
                    toMap[enumValue] = name;
                }

                fromMap[name] = enumValue;
            }

            fromValueMap[enumType] = fromMap;
            toValueMap[enumType] = toMap;
        }

        private string ToValue(Type enumType, object obj)
        {
            var map = toValueMap[enumType];
            return map[obj];
        }

        private object FromValue(Type enumType, string value)
        {
            var map = fromValueMap[enumType];
            return map.ContainsKey(value)
                ? map[value]
                : null;
        }

        #endregion
    }
}
