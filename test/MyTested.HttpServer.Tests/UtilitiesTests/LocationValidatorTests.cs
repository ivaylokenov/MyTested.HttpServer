namespace MyTested.HttpServer.Tests.UtilitiesTests
{
    using Setups;
    using System;
    using Utilities;
    using Xunit;

    public class LocationValidatorTests
    {
        [Fact]
        public void ValidateAndGetWellFormedUriStringShouldReturnProperUriWithCorrectString()
        {
            string uriAsString = "http://somehost.com/someuri/1?query=Test";

            var uri = LocationValidator.ValidateAndGetWellFormedUriString(
                uriAsString,
                TestObjectFactory.GetFailingValidationAction());

            Assert.NotNull(uri);
            Assert.Equal(uriAsString, uri.OriginalString);
        }

        [Fact]
        public void ValidateAndGetWellFormedUriStringShouldThrowExceptionWithIncorrectString()
        {
            string uriAsString = "http://somehost!@#?Query==true";

            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var uri = LocationValidator.ValidateAndGetWellFormedUriString(
                    uriAsString,
                    TestObjectFactory.GetFailingValidationAction());
            });

            Assert.Equal("location to be URI valid instead received http://somehost!@#?Query==true", exception.Message);
        }
    }
}
