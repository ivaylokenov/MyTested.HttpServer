namespace MyTested.HttpServer.Contracts
{
    using System;

    /// <summary>
    /// Used for testing HTTP response message with response time measurements.
    /// </summary>
    public interface IHttpHandlerResponseMessageWithTimeTestBuilder : IHttpHandlerResponseMessageTestBuilder
    {
        /// <summary>
        /// Tests whether the measured response time passes given assertions.
        /// </summary>
        /// <param name="assertions">Action containing all assertions on the measured response time.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpHandlerResponseMessageWithTimeTestBuilder WithResponseTime(Action<TimeSpan> assertions);

        /// <summary>
        /// Tests whether the measured response time passes given predicate.
        /// </summary>
        /// <param name="predicate">Predicate testing the measured response time.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        IAndHttpHandlerResponseMessageWithTimeTestBuilder WithResponseTime(Func<TimeSpan, bool> predicate);

        /// <summary>
        /// Gets the response time measured in the testing.
        /// </summary>
        /// <returns>Instance of TimeSpan.</returns>
        TimeSpan AndProvideTheResponseTime();
    }
}
