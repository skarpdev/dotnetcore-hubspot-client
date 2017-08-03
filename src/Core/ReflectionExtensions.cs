using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Skarp.HubSpotClient.Core
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns the name of a given property either by name of <see cref="DataMemberAttribute"/>
        /// </summary>
        /// <remarks>
        /// If the <see cref="DataMemberAttribute"/> is defined it will try to use the prop name defined there. If no name is explictily defined in the attribute the 
        /// name of the actual property will be returned.
        /// </remarks>
        /// <param name="prop"></param>
        /// <returns></returns>
        internal static string GetPropSerializedName(this PropertyInfo prop)
        {
            if (prop == null) return null;

            var propName = prop.Name;

            var dataMemberAttr = prop.GetCustomAttribute<DataMemberAttribute>();
            if (dataMemberAttr == null) return propName;
            if (string.IsNullOrWhiteSpace(dataMemberAttr.Name)) return propName;

            return dataMemberAttr.Name;
        }

        /// <summary>
        /// Finds the method recursively by searching in the given type and all implemented interfaces.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="name">The name.</param>
        /// <param name="typeArgs">The type arguments.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static MethodInfo FindMethodRecursively(this Type prop, string name, params Type[] typeArgs)
        {
            if (prop == null) return null;

            var method = prop.GetMethod(name, typeArgs);
            if (method != null) return method;


            foreach (var iface in prop.GetInterfaces())
            {
                method = iface.FindMethodRecursively(name, typeArgs);
                if (method != null) return method;
            }
            // base types
            var baseType = prop.GetTypeInfo().BaseType;
            if (baseType != null)
            {
                method = baseType.FindMethodRecursively(name, typeArgs);
                if (method != null) return method;
            }

            // TODO better bailout exception?
            throw new ArgumentException($"Unable to locate method with name: {name}");
        }
    }
}
