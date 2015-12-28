namespace MyTested.HttpServer.Tests.UtilitiesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Utilities;
    
    public class ReflectionTests
    {
        [Fact]
        public void AreAssignableShouldReturnTrueWithTheSameTypes()
        {
            var first = typeof(int);
            var second = typeof(int);

            Assert.True(Reflection.AreAssignable(first, second));
            Assert.False(Reflection.AreNotAssignable(first, second));
        }

        [Fact]
        public void AreAssignableShouldReturnTrueWithInheritedTypes()
        {
            var baseType = typeof(IEnumerable<int>);
            var inheritedType = typeof(IList<int>);

            Assert.True(Reflection.AreAssignable(baseType, inheritedType));
            Assert.False(Reflection.AreNotAssignable(baseType, inheritedType));
        }

        [Fact]
        public void AreAssignableShouldReturnTrueWithInvertedInheritedTypes()
        {
            var baseType = typeof(IList<int>);
            var inheritedType = typeof(IEnumerable<int>);

            Assert.False(Reflection.AreAssignable(baseType, inheritedType));
            Assert.True(Reflection.AreNotAssignable(baseType, inheritedType));
        }

        [Fact]
        public void AreAssignableShouldReturnFalseWithGenericTypeDefinitions()
        {
            var baseType = typeof(IEnumerable<>);
            var inheritedType = typeof(IList<>);

            Assert.False(Reflection.AreAssignable(baseType, inheritedType));
            Assert.True(Reflection.AreNotAssignable(baseType, inheritedType));
        }

        [Fact]
        public void AreAssignableShouldReturnFalseWithOneGenericTypeDefinition()
        {
            var baseType = typeof(IEnumerable<>);
            var inheritedType = typeof(IList<int>);

            Assert.False(Reflection.AreAssignable(baseType, inheritedType));
            Assert.True(Reflection.AreNotAssignable(baseType, inheritedType));
        }

        [Fact]
        public void IsGenericShouldReturnTrueWithGenericType()
        {
            var type = typeof(IEnumerable<int>);

            Assert.True(Reflection.IsGeneric(type));
        }

        [Fact]
        public void IsGenericShouldReturnTrueWithGenericTypeDefinition()
        {
            var type = typeof(IEnumerable<>);

            Assert.True(Reflection.IsGeneric(type));
        }

        [Fact]
        public void IsGenericShouldReturnFalseWithNonGenericType()
        {
            var type = typeof(object);

            Assert.False(Reflection.IsGeneric(type));
        }
        
        [Fact]
        public void ToFriendlyTypeNameShouldReturnTheOriginalNameWhenTypeIsNotGeneric()
        {
            var name = typeof(object).ToFriendlyTypeName();
            Assert.Equal("Object", name);
        }

        [Fact]
        public void ToFriendlyTypeNameShouldReturnProperNameWhenTypeIsGenericWithoutArguments()
        {
            var name = typeof(List<>).ToFriendlyTypeName();
            Assert.Equal("List<>", name);
        }

        [Fact]
        public void ToFriendlyTypeNameShouldReturnProperNameWhenTypeIsGenericWithoutMoreThanOneArguments()
        {
            var name = typeof(Dictionary<,>).ToFriendlyTypeName();
            Assert.Equal("Dictionary<>", name);
        }

        [Fact]
        public void ToFriendlyTypeNameShouldReturnProperNameWhenTypeIsGenericWithOneArgument()
        {
            var name = typeof(List<int>).ToFriendlyTypeName();
            Assert.Equal("List<Int32>", name);
        }

        [Fact]
        public void ToFriendlyTypeNameShouldReturnProperNameWhenTypeIsGenericWithMoreThanOneArguments()
        {
            var name = typeof(Dictionary<string, int>).ToFriendlyTypeName();
            Assert.Equal("Dictionary<String, Int32>", name);
        }
    }
}
