﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cosmos.Reflection
{
    /// <summary>
    /// Property access options
    /// </summary>
    public enum PropertyAccessOptions
    {
        /// <summary>
        /// Getters
        /// </summary>
        Getters,

        /// <summary>
        /// Setters
        /// </summary>
        Setters,

        /// <summary>
        /// Both
        /// </summary>
        Both,
    }

    internal static class PropertyReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithPublicInstanceSetters(Type type)
        {
            return type.GetRuntimeProperties().Where(p => p.SetMethod != null && !p.SetMethod.IsStatic && p.SetMethod.IsPublic);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithPublicInstanceGetters(Type type)
        {
            return type.GetRuntimeProperties().Where(p => p.GetMethod != null && !p.GetMethod.IsStatic && p.GetMethod.IsPublic);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithPublicInstance(Type type)
        {
            return type.GetRuntimeProperties().Where(p =>
                p.GetMethod != null && !p.GetMethod.IsStatic && p.GetMethod.IsPublic
             && p.SetMethod != null && !p.SetMethod.IsStatic && p.SetMethod.IsPublic);
        }

        public static void CheckPropertyAccess(PropertyInfo propertyInfo, PropertyAccessOptions accessOptions)
        {
            ArgumentException CreatePropertyNotMatchAccessException() => new($"The property ({propertyInfo.Name}) does not match accessibility restrictions.");

            switch (accessOptions)
            {
                case PropertyAccessOptions.Getters:
                {
                    if (!(propertyInfo.GetMethod != null && !propertyInfo.GetMethod.IsStatic && propertyInfo.GetMethod.IsPublic))
                        throw CreatePropertyNotMatchAccessException();
                    break;
                }

                case PropertyAccessOptions.Setters:
                {
                    if (!(propertyInfo.SetMethod != null && !propertyInfo.SetMethod.IsStatic && propertyInfo.SetMethod.IsPublic))
                        throw CreatePropertyNotMatchAccessException();
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Type visit, an advanced TypeReflections utility.
    /// </summary>
    public static partial class TypeVisit
    {
        /// <summary>
        /// Get all properties from the given Type.<br />
        /// 从给定的 Type 中获得所有属性。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="accessOptions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<PropertyInfo> GetProperties(Type type, PropertyAccessOptions accessOptions)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return accessOptions switch
            {
                PropertyAccessOptions.Getters => PropertyReflectionHelper.GetPropertiesWithPublicInstanceGetters(type),
                PropertyAccessOptions.Setters => PropertyReflectionHelper.GetPropertiesWithPublicInstanceSetters(type),
                PropertyAccessOptions.Both => PropertyReflectionHelper.GetPropertiesWithPublicInstance(type),
                _ => throw new InvalidOperationException("Invalid operation for unknown access type")
            };
        }

        /// <summary>
        /// Get all properties from the given Type.<br />
        /// 从给定的 Type 中获得所有属性。
        /// </summary>
        /// <param name="accessOptions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties<T>(PropertyAccessOptions accessOptions)
        {
            return GetProperties(typeof(T), accessOptions);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(PropertyAccessOptions accessOptions, params Expression<Func<T, object>>[] propertySelectors)
        {
            return GetProperties(propertySelectors, accessOptions);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(IEnumerable<Expression<Func<T, object>>> propertySelectors, PropertyAccessOptions accessOptions = PropertyAccessOptions.Both)
        {
            if (propertySelectors is null)
                throw new ArgumentNullException(nameof(propertySelectors));
            return propertySelectors.Select(p => GetProperty(p, accessOptions));
        }

        public static PropertyInfo GetProperty<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, PropertyAccessOptions accessOptions = PropertyAccessOptions.Both)
        {
            if (propertySelector is null)
                throw new ArgumentNullException(nameof(propertySelector));

            var member = propertySelector.Body as MemberExpression;

            ArgumentException CreateExpressionNotPropertyException() => new($"The expression parameter ({nameof(propertySelector)}) is not a property expression.");

            if (member is null
             && propertySelector.Body.NodeType == ExpressionType.Convert
             && propertySelector.Body is UnaryExpression unary)
                member = unary.Operand as MemberExpression;

            if (member?.Member is not PropertyInfo propertyInfo)
                throw CreateExpressionNotPropertyException();

            PropertyReflectionHelper.CheckPropertyAccess(propertyInfo, accessOptions);

            return propertyInfo;
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="shape"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(IEnumerable<PropertyInfo> properties, T shape, params Expression<Func<T, object>>[] expressions)
        {
            return Exclude(properties, shape, (IEnumerable<Expression<Func<T, object>>>)expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="shape"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(IEnumerable<PropertyInfo> properties, T shape, IEnumerable<Expression<Func<T, object>>> expressions)
        {
            return Exclude(properties, expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(IEnumerable<PropertyInfo> properties, params Expression<Func<T, object>>[] expressions)
        {
            return Exclude(properties, (IEnumerable<Expression<Func<T, object>>>)expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<PropertyInfo> Exclude<T>(IEnumerable<PropertyInfo> properties, IEnumerable<Expression<Func<T, object>>> expressions)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (expressions is null)
                throw new ArgumentNullException(nameof(expressions));

            ISet<PropertyInfo> excluded = new HashSet<PropertyInfo>(TypeVisit.GetProperties(expressions));

            return properties.Where(p => !excluded.Contains(p));
        }

        /// <summary>
        /// Determine whether PropertyInfo is Visible and Virtual.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsVisibleAndVirtual(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return (property.CanRead && property.GetMethod.IsVisibleAndVirtual()) ||
                   (property.CanWrite && property.GetMethod.IsVisibleAndVirtual());
        }
    }

    public static partial class TypeVisitExtensions
    {
        public static PropertyInfo GetProperty<T, TProperty>(this T x, Expression<Func<T, TProperty>> propertySelector, PropertyAccessOptions accessOptions = PropertyAccessOptions.Both)
        {
            if (x is null) throw new ArgumentNullException(nameof(x));
            if (propertySelector is null) throw new ArgumentNullException(nameof(propertySelector));
            return TypeVisit.GetProperty(propertySelector, accessOptions);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(this T x, params Expression<Func<T, object>>[] propertySelectors)
        {
            if (x is null) throw new ArgumentNullException(nameof(x));
            return TypeVisit.GetProperties(PropertyAccessOptions.Both, propertySelectors);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(this T x, PropertyAccessOptions accessOptions, params Expression<Func<T, object>>[] propertySelectors)
        {
            if (x is null) throw new ArgumentNullException(nameof(x));
            return TypeVisit.GetProperties(accessOptions, propertySelectors);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(this T x, PropertyAccessOptions accessOptions = PropertyAccessOptions.Both)
        {
            return TypeVisit.GetProperties(typeof(T), accessOptions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="shape"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(this IEnumerable<PropertyInfo> properties, T shape, params Expression<Func<T, object>>[] expressions)
        {
            return TypeVisit.Exclude(properties, shape, expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="shape"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(this IEnumerable<PropertyInfo> properties, T shape, IEnumerable<Expression<Func<T, object>>> expressions)
        {
            return TypeVisit.Exclude(properties, shape, expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> Exclude<T>(this IEnumerable<PropertyInfo> properties, params Expression<Func<T, object>>[] expressions)
        {
            return TypeVisit.Exclude(properties, expressions);
        }

        /// <summary>
        /// Exclude all PropertyInfos that meet the given conditions from the PropertyInfo list,
        /// and return the remaining PropertyInfo.<br />
        /// 从 PropertyInfo 列表中排除所有满足给定条件的 PropertyInfo，并返回其余 PropertyInfo。
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<PropertyInfo> Exclude<T>(this IEnumerable<PropertyInfo> properties, IEnumerable<Expression<Func<T, object>>> expressions)
        {
            return TypeVisit.Exclude(properties, expressions);
        }
    }

    /// <summary>
    /// Type metadata visit, a meta information access entry for TypeReflections and TypeVisit.
    /// </summary>
    public static partial class TypeMetaVisitExtensions
    {
        /// <summary>
        /// Determine whether the specified property is a virtual property.<br />
        /// 判断指定属性是否是虚属性。
        /// </summary>
        public static bool IsVirtual(this PropertyInfo property)
        {
            var accessor = property.GetAccessors().FirstOrDefault();
            return accessor is not null && accessor.IsVirtual && !accessor.IsFinal;
        }

        /// <summary>
        /// Determine whether the specified property is an abstract property.<br />
        /// 判断指定属性是否是虚属性。
        /// </summary>
        public static bool IsAbstract(this PropertyInfo property)
        {
            var accessor = property.GetAccessors().FirstOrDefault();
            return accessor is not null && accessor.IsAbstract && !accessor.IsFinal;
        }

        /// <summary>
        /// Determine whether PropertyInfo is Visible and Virtual.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsVisibleAndVirtual(this PropertyInfo property)
        {
            return TypeVisit.IsVisibleAndVirtual(property);
        }
    }
}