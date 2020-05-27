﻿using System.Linq;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Cosmos.Reflection
{
    public static partial class ReflectionExtensions
    {
        /// <summary>
        /// Get full name of method including type name and method name
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetFullName(this MethodInfo method)
        {
            var result = new StringBuilder();
            var type = method.DeclaringType;
            if (type != null)
                result.Append(type.FullName).Append('.');

            result.Append(method.Name);
            return result.ToString();
        }

        /// <summary>
        /// To compute signature
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string ToComputeSignature(this MethodInfo method)
        {
            var sb = new StringBuilder();
            sb.Append(method.ReturnType.ToComputeSignature());
            sb.Append(" ");
            sb.Append(method.Name);
            if (method.IsGenericMethod)
            {
                sb.Append("[");
                var genericTypes = method.GetGenericArguments().ToTypeInfo().ToList();
                for (var i = 0; i < genericTypes.Count; i++)
                {
                    sb.Append(genericTypes[i].ToComputeSignature());
                    if (i != genericTypes.Count - 1)
                        sb.Append(", ");
                }

                sb.Append("]");
            }

            sb.Append("(");
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i].ParameterType.ToComputeSignature());
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}