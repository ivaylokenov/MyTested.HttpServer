namespace MyTested.HttpServer.Utilities
{
    using System;

    /// <summary>
    /// Validator class containing Version validation logic.
    /// </summary>
    public static class VersionValidator
    {
        /// <summary>
        /// Tries to parse version from string.
        /// </summary>
        /// <param name="version">Provided version string.</param>
        /// <param name="failedValidationAction">Action to call in case of failed validation.</param>
        /// <returns>Valid Version from the provided string.</returns>
        public static Version TryParse(string version, Action<string, string, string> failedValidationAction)
        {
            Version parsedVersion;
            if (!Version.TryParse(version, out parsedVersion))
            {
                failedValidationAction("version", "valid version string", "invalid one");
            }

            return parsedVersion;
        }
    }
}
