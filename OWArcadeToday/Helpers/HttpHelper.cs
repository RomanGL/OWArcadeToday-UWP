using JetBrains.Annotations;
using Microsoft.Toolkit.Uwp.Helpers;

namespace OWArcadeToday.Helpers
{
    /// <summary>
    /// Represents a HTTP helper.
    /// </summary>
    internal static class HttpHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets a User-Agent HTTP header.
        /// </summary>
        [NotNull]
        public static string GetUserAgent()
        {
            var ver = SystemInformation.ApplicationVersion;
            var osVer = SystemInformation.OperatingSystemVersion;
            var arch = SystemInformation.OperatingSystemArchitecture;
            return $"OWArcadeToday/{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision} (Windows {osVer.Major}.{osVer.Minor}.{osVer.Build}; {arch}; UWP)";
        }

        #endregion
    }
}