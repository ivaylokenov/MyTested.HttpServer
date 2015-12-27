namespace MyTested.HttpServer.Tests.UtilitiesTests
{
    using System;
    using Setups;
    using Utilities;
    using Xunit;

    public class VersionValidatorTests
    {
        [Fact]
        public void TryParseShouldReturnCorrectVersion()
        {
            var version = VersionValidator.TryParse("1.1", TestObjectFactory.GetFailingValidationAction());

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
        }

        [Fact]
        public void TryParseShouldInvokeFailedActionIfStringIsNotInCorrectFormat()
        {
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                VersionValidator.TryParse("test", TestObjectFactory.GetFailingValidationAction());
            });

            Assert.Equal("version valid version string invalid one", exception.Message);
        }
    }
}
