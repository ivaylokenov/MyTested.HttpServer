namespace MyTested.HttpServer.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class for validating reflection checks.
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Checks whether two types are assignable.
        /// </summary>
        /// <param name="baseType">Base type to be checked.</param>
        /// <param name="inheritedType">Inherited type to be checked.</param>
        /// <returns>True or false.</returns>
        public static bool AreAssignable(Type baseType, Type inheritedType)
        {
            return baseType.GetTypeInfo().IsAssignableFrom(inheritedType.GetTypeInfo());
        }

        /// <summary>
        /// Checks whether two types are not assignable.
        /// </summary>
        /// <param name="baseType">Base type to be checked.</param>
        /// <param name="inheritedType">Inherited type to be checked.</param>
        /// <returns>True or false.</returns>
        public static bool AreNotAssignable(Type baseType, Type inheritedType)
        {
            return !AreAssignable(baseType, inheritedType);
        }

        /// <summary>
        /// Checks whether a type is generic.
        /// </summary>
        /// <param name="type">Type to be checked.</param>
        /// <returns>True or false.</returns>
        public static bool IsGeneric(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        
        /// <summary>
        /// Transforms generic type name to friendly one, showing generic type arguments.
        /// </summary>
        /// <param name="type">Type which name will be transformed.</param>
        /// <returns>Transformed name as string.</returns>
        public static string ToFriendlyTypeName(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return type.Name;
            }

            var genericArgumentNames = type.GenericTypeArguments.Select(ga => ga.ToFriendlyTypeName());
            var friendlyGenericName = type.Name.Split('`')[0];
            var joinedGenericArgumentNames = string.Join(", ", genericArgumentNames);

            return string.Format("{0}<{1}>", friendlyGenericName, joinedGenericArgumentNames);
        }
        
        /// <summary>
        /// Checks whether two objects are deeply equal by reflecting all their public properties recursively. Resolves successfully value and reference types, overridden Equals method, custom == operator, IComparable, nested objects and collection properties.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <returns>True or false.</returns>
        /// <remarks>This method is used for the route testing. Since the ASP.NET Web API model binder creates new instances, circular references are not checked.</remarks>
        public static bool AreDeeplyEqual(object expected, object actual)
        {
            if (expected == null && actual == null)
            {
                return true;
            }

            if (expected == null || actual == null)
            {
                return false;
            }

            var expectedType = expected.GetType();
            var actualType = actual.GetType();
            var objectType = typeof(object);

            if ((expectedType == objectType && actualType != objectType)
                || (actualType == objectType && expectedType != objectType))
            {
                return false;
            }

            if (expected is IEnumerable)
            {
                if (CollectionsAreDeeplyEqual(expected, actual))
                {
                    return true;
                }

                return false;
            }

            if (expectedType != actualType
                && !expectedType.GetTypeInfo().IsAssignableFrom(actualType.GetTypeInfo())
                && !actualType.GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo()))
            {
                return false;
            }

            if (expectedType.GetTypeInfo().IsPrimitive && actualType.GetTypeInfo().IsPrimitive)
            {
                return expected.ToString() == actual.ToString();
            }

            var equalsOperator = expectedType.GetTypeInfo().DeclaredMethods.FirstOrDefault(m => m.Name == "op_Equality");
            if (equalsOperator != null)
            {
                return (bool)equalsOperator.Invoke(null, new[] { expected, actual });
            }

            if (expectedType != objectType)
            {
                var equalsMethod = expectedType.GetTypeInfo().DeclaredMethods.FirstOrDefault(m => m.Name == "Equals" && m.DeclaringType == expectedType);
                if (equalsMethod != null)
                {
                    return (bool)equalsMethod.Invoke(expected, new[] { actual });
                }
            }

            if (ComparablesAreDeeplyEqual(expected, actual))
            {
                return true;
            }

            if (!ObjectPropertiesAreDeeplyEqual(expected, actual))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether two objects are not deeply equal by reflecting all their public properties recursively. Resolves successfully value and reference types, overridden Equals method, custom == operator, IComparable, nested objects and collection properties.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <returns>True or false.</returns>
        /// <remarks>This method is used for the route testing. Since the ASP.NET Web API model binder creates new instances, circular references are not checked.</remarks>
        public static bool AreNotDeeplyEqual(object expected, object actual)
        {
            return !AreDeeplyEqual(expected, actual);
        }
        
        private static bool CollectionsAreDeeplyEqual(object expected, object actual)
        {
            var expectedAsEnumerable = (IEnumerable)expected;
            var actualAsEnumerable = actual as IEnumerable;
            if (actualAsEnumerable == null)
            {
                return false;
            }

            var listOfExpectedValues = expectedAsEnumerable.Cast<object>().ToList();
            var listOfActualValues = actualAsEnumerable.Cast<object>().ToList();

            if (listOfExpectedValues.Count != listOfActualValues.Count)
            {
                return false;
            }

            var collectionIsNotEqual = listOfExpectedValues
                .Where((t, i) => AreNotDeeplyEqual(t, listOfActualValues[i]))
                .Any();

            if (collectionIsNotEqual)
            {
                return false;
            }

            return true;
        }

        private static bool ComparablesAreDeeplyEqual(object expected, object actual)
        {
            var expectedAsIComparable = expected as IComparable;
            if (expectedAsIComparable != null)
            {
                if (expectedAsIComparable.CompareTo(actual) == 0)
                {
                    return true;
                }
            }

            if (ObjectImplementsIComparable(expected) && ObjectImplementsIComparable(actual))
            {
                var method = expected.GetType().GetTypeInfo().GetDeclaredMethod("CompareTo");
                if (method != null)
                {
                    return (int)method.Invoke(expected, new[] { actual }) == 0;
                }
            }

            return false;
        }

        private static bool ObjectImplementsIComparable(object obj)
        {
            return obj.GetType()
                .GetTypeInfo()
                .ImplementedInterfaces
                .FirstOrDefault(i => i.Name.StartsWith("IComparable")) != null;
        }

        private static bool ObjectPropertiesAreDeeplyEqual(object expected, object actual)
        {
            var properties = expected.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                var expectedPropertyValue = property.GetValue(expected);
                var actualPropertyValue = property.GetValue(actual);

                if (expectedPropertyValue is IEnumerable)
                {
                    if (!CollectionsAreDeeplyEqual(expectedPropertyValue, actualPropertyValue))
                    {
                        return false;
                    }
                }

                var propertiesAreDifferent = AreNotDeeplyEqual(expectedPropertyValue, actualPropertyValue);
                if (propertiesAreDifferent)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
