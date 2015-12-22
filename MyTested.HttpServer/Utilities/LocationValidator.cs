namespace MyTested.HttpServer.Utilities
{
    using System;

    /// <summary>
    /// Validator class containing URI validation logic.
    /// </summary>
    public static class LocationValidator
    {
        /// <summary>
        /// Validates an URI provided as string.
        /// </summary>
        /// <param name="location">Expected URI as string.</param>
        /// <param name="failedValidationAction">Action to call in case of failed validation.</param>
        /// <returns>Valid Uri created from the provided string.</returns>
        public static Uri ValidateAndGetWellFormedUriString(
            string location,
            Action<string, string, string> failedValidationAction)
        {
            if (!Uri.IsWellFormedUriString(location, UriKind.RelativeOrAbsolute))
            {
                failedValidationAction(
                    "location",
                    "to be URI valid",
                    string.Format("instead received {0}", location));
            }

            return new Uri(location, UriKind.RelativeOrAbsolute);
        }
    }
}
