namespace MyTested.HttpServer.Utilities
{
    using System;
    using System.Linq;
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

            var genericArgumentNames = type.GetTypeInfo().GenericTypeParameters.Select(ga => ga.ToFriendlyTypeName());
            var friendlyGenericName = type.Name.Split('`')[0];
            var joinedGenericArgumentNames = string.Join(", ", genericArgumentNames);

            return string.Format("{0}<{1}>", friendlyGenericName, joinedGenericArgumentNames);
        }
        

        private static bool ObjectImplementsIComparable(object obj)
        {
            return obj.GetType()
                .GetTypeInfo()
                .ImplementedInterfaces
                .FirstOrDefault(i => i.Name.StartsWith("IComparable")) != null;
        }
    }
}
