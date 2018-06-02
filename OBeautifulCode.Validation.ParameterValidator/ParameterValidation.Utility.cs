﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.Utility.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Validation source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Validation.Recipes
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using static System.FormattableString;

    /// <summary>
    /// Contains all validations that can be applied to a <see cref="Parameter"/>.
    /// </summary>
#if !OBeautifulCodeValidationRecipesProject
    internal
#else
    public
#endif
        static partial class ParameterValidation
    {
#pragma warning disable SA1201

        private static readonly MethodInfo GetDefaultValueOpenGenericMethodInfo = ((Func<object>)GetDefaultValue<object>).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> GetDefaultValueTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly MethodInfo EqualsUsingDefaultEqualityComparerOpenGenericMethodInfo = ((Func<object, object, bool>)EqualsUsingDefaultEqualityComparer).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly MethodInfo CompareUsingDefaultComparerOpenGenericMethodInfo = ((Func<object, object, CompareOutcome>)CompareUsingDefaultComparer).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> CompareUsingDefaultComparerTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static void Validate(
            this Parameter parameter,
            IReadOnlyCollection<TypeValidation> typeValidations,
            ValueValidation valueValidation)
        {
            ParameterValidator.ThrowImproperUseOfFrameworkIfDetected(parameter, ParameterShould.BeMusted);

            valueValidation.ParameterName = parameter.Name;
            valueValidation.IsElementInEnumerable = parameter.HasBeenEached;

            if (parameter.HasBeenEached)
            {
                // check that the parameter is an IEnumerable and not null
                ThrowIfNotOfType(nameof(ParameterValidator.Each), false, parameter.ValueType, new[] { EnumerableType }, null);
                NotBeNullInternal(new ValueValidation { ParameterName = parameter.Name, ValidationName = nameof(ParameterValidator.Each), Value = parameter.Value, ValueType = parameter.ValueType, IsElementInEnumerable = false });

                var valueAsEnumerable = (IEnumerable)parameter.Value;
                var enumerableType = GetEnumerableGenericType(parameter.ValueType);
                foreach (var typeValidation in typeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.TypeValidationHandler(valueValidation.ValidationName, valueValidation.IsElementInEnumerable, enumerableType, typeValidation.ReferenceTypes, valueValidation.ValidationParameters);
                }

                valueValidation.ValueType = enumerableType;

                foreach (var element in valueAsEnumerable)
                {
                    valueValidation.Value = element;

                    valueValidation.ValueValidationHandler(valueValidation);
                }
            }
            else
            {
                foreach (var typeValidation in typeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.TypeValidationHandler(valueValidation.ValidationName, valueValidation.IsElementInEnumerable, parameter.ValueType, typeValidation.ReferenceTypes, valueValidation.ValidationParameters);
                }

                valueValidation.Value = parameter.Value;
                valueValidation.ValueType = parameter.ValueType;

                valueValidation.ValueValidationHandler(valueValidation);
            }

            parameter.HasBeenValidated = true;
        }

        private static Type GetEnumerableGenericType(
            Type enumerableType)
        {
            // adapted from: https://stackoverflow.com/a/17713382/356790
            Type result;
            if (enumerableType.IsArray)
            {
                // type is array, shortcut
                result = enumerableType.GetElementType();
            }
            else if (enumerableType.IsGenericType && (enumerableType.GetGenericTypeDefinition() == UnboundGenericEnumerableType))
            {
                // type is IEnumerable<T>
                result = enumerableType.GetGenericArguments()[0];
            }
            else
            {
                // type implements IEnumerable<T> or is a subclass (sub-sub-class, ...)
                // of a type that implements IEnumerable<T>
                // note that we are grabing the first implementation.  it is possible, but
                // highly unlikely, for a type to have multiple implementations of IEnumerable<T>
                result = enumerableType
                    .GetInterfaces()
                    .Where(_ => _.IsGenericType && (_.GetGenericTypeDefinition() == UnboundGenericEnumerableType))
                    .Select(_ => _.GenericTypeArguments[0])
                    .FirstOrDefault();

                if (result == null)
                {
                    // here we just assume it's an IEnumerable and return typeof(object),
                    // however, for completeness, we should recurse through all interface implementations
                    // and check whether those are IEnumerable<T>.
                    // see: https://stackoverflow.com/questions/5461295/using-isassignablefrom-with-open-generic-types
                    result = ObjectType;
                }
            }

            return result;
        }

        private static void ThrowIfMalformedRange(
            ValidationParameter[] validationParameters)
        {
            // the public BeInRange/NotBeInRange is generic and guarantees that minimum and maximum are of the same type
            var rangeIsMalformed = CompareUsingDefaultComparer(validationParameters[0].ValueType, validationParameters[0].Value, validationParameters[1].Value) == CompareOutcome.Value1GreaterThanValue2;
            if (rangeIsMalformed)
            {
                var malformedRangeExceptionMessage = string.Format(CultureInfo.InvariantCulture, MalformedRangeExceptionMessage, validationParameters[0].Name, validationParameters[1].Name, validationParameters[0].Value?.ToString() ?? NullValueToString, validationParameters[1].Value?.ToString() ?? NullValueToString);
                ParameterValidator.ThrowImproperUseOfFramework(malformedRangeExceptionMessage);
            }
        }

        private static string BuildArgumentExceptionMessage(
            ValueValidation valueValidation,
            string exceptionMessageSuffix,
            Include include = Include.None,
            Type genericTypeOverride = null)
        {
            if (valueValidation.Because != null)
            {
                return valueValidation.Because;
            }

            var parameterNameQualifier = valueValidation.ParameterName == null ? string.Empty : Invariant($" '{valueValidation.ParameterName}'");
            var enumerableQualifier = valueValidation.IsElementInEnumerable ? " contains an element that" : string.Empty;
            var genericTypeQualifier = include.HasFlag(Include.GenericType) ? ", where T: " + (genericTypeOverride?.GetFriendlyTypeName() ?? valueValidation.ValueType.GetFriendlyTypeName()) : string.Empty;
            var failingValueQualifier = include.HasFlag(Include.FailingValue) ? (valueValidation.IsElementInEnumerable ? "  Element value" : "  Parameter value") + Invariant($" is '{valueValidation.Value?.ToString() ?? NullValueToString}'.") : string.Empty;
            var validationParameterQualifiers = valueValidation.ValidationParameters == null || !valueValidation.ValidationParameters.Any() ? string.Empty : valueValidation.ValidationParameters.Select(_ => Invariant($"  Specified '{_.Name}' is '{_.Value ?? NullValueToString}'.")).Aggregate((running, current) => running + current);
            var result = Invariant($"Parameter{parameterNameQualifier}{enumerableQualifier} {exceptionMessageSuffix}{genericTypeQualifier}.{failingValueQualifier}{validationParameterQualifiers}");
            return result;
        }

        private static T GetDefaultValue<T>()
        {
            var result = default(T);
            return result;
        }

        private static object GetDefaultValue(
            Type type)
        {
            if (!GetDefaultValueTypeToMethodInfoMap.ContainsKey(type))
            {
                GetDefaultValueTypeToMethodInfoMap.TryAdd(type, GetDefaultValueOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            var result = GetDefaultValueTypeToMethodInfoMap[type].Invoke(null, null);
            return result;
        }

        private static bool EqualsUsingDefaultEqualityComparer<T>(
            T value1,
            T value2)
        {
            var result = EqualityComparer<T>.Default.Equals(value1, value2);
            return result;
        }

        private static bool EqualUsingDefaultEqualityComparer(
            Type type,
            object value1,
            object value2)
        {
            if (!EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap.ContainsKey(type))
            {
                EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap.TryAdd(type, EqualsUsingDefaultEqualityComparerOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            var result = (bool)EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap[type].Invoke(null, new[] { value1, value2 });
            return result;
        }

        private static CompareOutcome CompareUsingDefaultComparer<T>(
            T x,
            T y)
        {
            var comparison = Comparer<T>.Default.Compare(x, y);
            CompareOutcome result;
            if (comparison < 0)
            {
                result = CompareOutcome.Value1LessThanValue2;
            }
            else if (comparison == 0)
            {
                result = CompareOutcome.Value1EqualsValue2;
            }
            else
            {
                result = CompareOutcome.Value1GreaterThanValue2;
            }

            return result;
        }

        private static CompareOutcome CompareUsingDefaultComparer(
            Type type,
            object value1,
            object value2)
        {
            if (!CompareUsingDefaultComparerTypeToMethodInfoMap.ContainsKey(type))
            {
                CompareUsingDefaultComparerTypeToMethodInfoMap.TryAdd(type, CompareUsingDefaultComparerOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            // note that the call is ultimately, via reflection, to Compare(T, T)
            // as such, reflection will throw an ArgumentException if the types of value1 and value2 are
            // not "convertible" to the specified type.  It's a pretty complicated heuristic:
            // https://stackoverflow.com/questions/34433043/check-whether-propertyinfo-setvalue-will-throw-an-argumentexception
            // Instead of relying on this heuristic, we just check upfront that value2's type == the specified type
            // (value1's type will always be the specified type).  This constrains our capabilities - for example, we
            // can't compare an integer to a decimal.  That said, we feel like this is a good constraint in a parameter
            // validation framework.  We'd rather be forced to make the types align than get a false negative
            // (a validation passes when it should fail).

            // otherwise, if reflection is able to call Compare(T, T), then ArgumentException can be thrown if
            // Type T does not implement either the System.IComparable<T> generic interface or the System.IComparable interface
            // However we already check for this upfront in ThrowIfNotComparable
            var result = (CompareOutcome)CompareUsingDefaultComparerTypeToMethodInfoMap[type].Invoke(null, new[] { value1, value2 });
            return result;
        }

        private enum CompareOutcome
        {
            Value1LessThanValue2,

            Value1EqualsValue2,

            Value1GreaterThanValue2,
        }

        private class ValueValidation
        {
            public string ValidationName { get; set; }

            public string Because { get; set; }

            public ApplyBecause ApplyBecause { get; set; }

            public ValueValidationHandler ValueValidationHandler { get; set; }

            public ValidationParameter[] ValidationParameters { get; set; }

            public string ParameterName { get; set; }

            public object Value { get; set; }

            public Type ValueType { get; set; }

            public bool IsElementInEnumerable { get; set; }
        }

        private class ValidationParameter
        {
            public string Name { get; set; }

            public object Value { get; set; }

            public Type ValueType { get; set; }
        }

        [Flags]
        private enum Include
        {
            None = 0,

            FailingValue = 1,

            GenericType = 2,
        }

#pragma warning restore SA1201
    }
}
