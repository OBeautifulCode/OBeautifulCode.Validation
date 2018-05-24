﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.cs" company="OBeautifulCode">
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
    using System.Linq;
    using System.Reflection;

    using static System.FormattableString;

    /// <summary>
    /// Contains all validations that can be applied to a <see cref="Parameter"/>.
    /// </summary>
#if !OBeautifulCodeValidationRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Validation", "See package version number")]
    internal
#else
    public
#endif
        static class ParameterValidation
    {
        private static readonly MethodInfo GetDefaultValueOpenGenericMethodInfo;

        private static readonly ConcurrentDictionary<Type, MethodInfo> GetDefaultValueTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly MethodInfo IsEqualUsingDefaultEqualityComparerOpenGenericMethodInfo;

        private static readonly ConcurrentDictionary<Type, MethodInfo> IsEqualUsingDefaultEqualityComparerTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        static ParameterValidation()
        {
            GetDefaultValueOpenGenericMethodInfo = ((Func<object>)GetDefaultValue<object>).Method.GetGenericMethodDefinition();
            IsEqualUsingDefaultEqualityComparerOpenGenericMethodInfo = ((Func<object, object, bool>)IsEqualUsingDefaultEqualityComparer).Method.GetGenericMethodDefinition();
        }

        private delegate void TypeValidation(string validationName, bool isElementInEnumerable, Type valueType, params Type[] referenceTypes);

        private delegate void ValueValidation(object value, Type valueType, string parameterName, string because, bool isElementInEnumerable);

        /// <summary>
        /// Validates that the reference type or nullable parameter is null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeNull(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfTypeCannotBeNull,
            };

            parameter.Validate(BeNull, nameof(BeNull), because, typeValidations);
            return parameter;
        }

        /// <summary>
        /// Validates that the reference type or nullable parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeNull(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfTypeCannotBeNull,
            };

            parameter.Validate(NotBeNull, nameof(NotBeNull), because, typeValidations);
            return parameter;
        }

        /// <summary>
        /// Validates that the bool or bool? parameter is true.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeTrue(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType
            };

            parameter.Validate(BeTrue, nameof(BeTrue), because, typeValidations, typeof(bool), typeof(bool?));
            return parameter;
        }

        /// <summary>
        /// Validates that the bool or bool? parameter is not true.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeTrue(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(NotBeTrue, nameof(NotBeTrue), because, typeValidations, typeof(bool), typeof(bool?));
            return parameter;
        }

        /// <summary>
        /// Validates that the bool or bool? parameter is false.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeFalse(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(BeFalse, nameof(BeFalse), because, typeValidations, typeof(bool), typeof(bool?));
            return parameter;
        }

        /// <summary>
        /// Validates that the bool? or bool? parameter is not false.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeFalse(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(NotBeFalse, nameof(NotBeFalse), because, typeValidations, typeof(bool), typeof(bool?));
            return parameter;
        }

        /// <summary>
        /// Validates that the string parameter is neither null nor whitespace.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeNullNorWhiteSpace(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(NotBeNullNorWhiteSpace, nameof(NotBeNullNorWhiteSpace), because, typeValidations, typeof(string));
            return parameter;
        }

        /// <summary>
        /// Validates that the guid or guid? is empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeEmptyGuid(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(BeEmptyGuid, nameof(BeEmptyGuid), because, typeValidations, typeof(Guid), typeof(Guid?));
            return parameter;
        }

        /// <summary>
        /// Validates that the string parameter is empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeEmptyString(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(BeEmptyString, nameof(BeEmptyString), because, typeValidations, typeof(string));
            return parameter;
        }

        /// <summary>
        /// Validates that the IEnumerable parameter is empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeEmptyEnumerable(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(BeEmptyEnumerable, nameof(BeEmptyEnumerable), because, typeValidations, typeof(IEnumerable));
            return parameter;
        }

        /// <summary>
        /// Validates that the guid or guid? parameter is not empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeEmptyGuid(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType
            };

            parameter.Validate(NotBeEmptyGuid, nameof(NotBeEmptyGuid), because, typeValidations, typeof(Guid), typeof(Guid?));
            return parameter;
        }

        /// <summary>
        /// Validates that the string parameter is not empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeEmptyString(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(NotBeEmptyString, nameof(NotBeEmptyString), because, typeValidations, typeof(string));
            return parameter;
        }

        /// <summary>
        /// Validates that the IEnumerable parameter is not empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeEmptyEnumerable(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
            };

            parameter.Validate(NotBeEmptyEnumerable, nameof(NotBeEmptyEnumerable), because, typeValidations, typeof(IEnumerable));
            return parameter;
        }

        /// <summary>
        /// Validates that the IEnumerable parameter contains at least one null element.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter ContainSomeNulls(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
                ThrowIfEnumerableTypeCannotBeNull,
            };

            parameter.Validate(ContainSomeNulls, nameof(ContainSomeNulls), because, typeValidations, typeof(IEnumerable));
            return parameter;
        }

        /// <summary>
        /// Validates that the IEnumerable parameter does not contain any null elements.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotContainAnyNulls(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
                ThrowIfEnumerableTypeCannotBeNull,
            };

            parameter.Validate(NotContainAnyNulls, nameof(NotContainAnyNulls), because, typeValidations, typeof(IEnumerable));
            return parameter;
        }

        /// <summary>
        /// Validates that the IEnumerable parameter is not null nor empty nor contains any null elements.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeNullNorEmptyNorContainAnyNulls(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfNotOfType,
                ThrowIfEnumerableTypeCannotBeNull,
            };

            var validationName = nameof(NotBeNullNorEmptyNorContainAnyNulls);

            parameter.Validate(NotBeEmptyEnumerable, validationName, because, typeValidations, typeof(IEnumerable));
            parameter.Validate(NotContainAnyNulls, validationName, because, new TypeValidation[] { });

            return parameter;
        }

        /// <summary>
        /// Validates that the parameter is equal to default(T).
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeDefault(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
            };

            parameter.Validate(BeDefault, nameof(BeDefault), because, typeValidations);
            return parameter;
        }

        /// <summary>
        /// Validates that the parameter is not equal to default(T).
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeDefault(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
            };

            parameter.Validate(NotBeDefault, nameof(NotBeDefault), because, typeValidations);
            return parameter;
        }

        /// <summary>
        /// Always throws.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeOfTypeThatDoesNotExist(
            [ValidatedNotNull] this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                Throw,
            };

            parameter.Validate(null, nameof(BeOfTypeThatDoesNotExist), because, typeValidations);
            return parameter;
        }

        private static void Validate(
            this Parameter parameter,
            ValueValidation valueValidation,
            string validationName,
            string because,
            IReadOnlyCollection<TypeValidation> typeValidations,
            params Type[] referenceTypes)
        {
            ParameterValidator.ThrowOnImproperUseOfFrameworkIfDetected(parameter, ParameterShould.BeMusted);

            if (parameter.HasBeenEached)
            {
                if (parameter.Value is IEnumerable valueAsEnumerable)
                {
                    var enumerableType = GetEnumerableGenericType(parameter.ValueType);

                    foreach (var typeValidation in typeValidations)
                    {
                        typeValidation(validationName, true, enumerableType, referenceTypes);
                    }

                    foreach (var element in valueAsEnumerable)
                    {
                        valueValidation(element, enumerableType, parameter.Name, because, isElementInEnumerable: true);
                    }
                }
                else
                {
                    // Each() calls:
                    // - ThrowOnImproperUseOfFramework when the parameter value is null
                    // - ThrowOnUnexpectedType when the parameter value is not an Enumerable
                    // so if we get here, the caller is trying to hack the framework
                    ParameterValidator.ThrowOnImproperUseOfFramework();
                }
            }
            else
            {
                foreach (var typeValidation in typeValidations)
                {
                    typeValidation(validationName, false, parameter.ValueType, referenceTypes);
                }

                valueValidation(parameter.Value, parameter.ValueType, parameter.Name, because, isElementInEnumerable: false);
            }

            parameter.HasBeenValidated = true;
        }

        private static Type GetEnumerableGenericType(
            Type type)
        {
            // adapted from: https://stackoverflow.com/a/17713382/356790
            Type result;
            if (type.IsArray)
            {
                // type is array, shortcut
                result = type.GetElementType();
            }
            else if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                // type is IEnumerable<T>
                result = type.GetGenericArguments()[0];
            }
            else
            {
                // type implements/extends IEnumerable<T>
                result = type
                    .GetInterfaces()
                    .Where(_ => _.IsGenericType && _.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(_ => _.GenericTypeArguments[0])
                    .FirstOrDefault();

                if (result == null)
                {
                    result = typeof(object);
                }
            }

            return result;
        }

        private static void Throw(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            params Type[] referenceTypes)
        {
            var parameterValueTypeName = valueType.GetFriendlyTypeName();
            throw new InvalidCastException(Invariant($"validationName: {validationName}, isElementInEnumerable: {isElementInEnumerable}, parameterValueTypeName: {parameterValueTypeName}"));
        }

        private static void ThrowIfTypeCannotBeNull(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            params Type[] referenceTypes)
        {
            if (valueType.IsValueType && (Nullable.GetUnderlyingType(valueType) == null))
            {
                ParameterValidator.ThrowOnUnexpectedTypes(validationName, isElementInEnumerable, "Any Reference Type", "Nullable<T>");
            }
        }

        private static void ThrowIfEnumerableTypeCannotBeNull(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            params Type[] referenceTypes)
        {
            var enumerableType = GetEnumerableGenericType(valueType);

            if (enumerableType.IsValueType && (Nullable.GetUnderlyingType(enumerableType) == null))
            {
                ParameterValidator.ThrowOnUnexpectedTypes(validationName, isElementInEnumerable, "IEnumerable", "IEnumerable<Any Reference Type>", "IEnumerable<Nullable<T>>");
            }
        }

        private static void ThrowIfNotOfType(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            params Type[] validTypes)
        {
            if ((!validTypes.Contains(valueType)) && (!validTypes.Any(_ => _.IsAssignableFrom(valueType))))
            {
                ParameterValidator.ThrowOnUnexpectedTypes(validationName, isElementInEnumerable, validTypes);
            }
        }

        private static string BuildExceptionMessage(
            string parameterName,
            string because,
            bool isElementInEnumerable,
            string exceptionMessageSuffix)
        {
            if (because != null)
            {
                return because;
            }

            var parameterNameQualifier = parameterName == null ? string.Empty : Invariant($" '{parameterName}'");
            var enumerableQualifier = isElementInEnumerable ? " contains an element that" : string.Empty;
            var result = Invariant($"parameter{parameterNameQualifier}{enumerableQualifier} {exceptionMessageSuffix}");
            return result;
        }

        private static void BeNull(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (!ReferenceEquals(value, null))
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not null");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeNull(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (ReferenceEquals(value, null))
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is null");
                if (isElementInEnumerable)
                {
                    throw new ArgumentException(exceptionMessage);
                }
                else
                {
                    throw new ArgumentNullException(null, exceptionMessage);
                }
            }
        }

        private static void BeTrue(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = ReferenceEquals(value, null) || ((bool)value != true);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not true");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeTrue(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldNotThrow = ReferenceEquals(value, null) || ((bool)value == false);
            if (!shouldNotThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is true");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void BeFalse(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = ReferenceEquals(value, null) || (bool)value;
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not false");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeFalse(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldNotThrow = ReferenceEquals(value, null) || (bool)value;
            if (!shouldNotThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is false");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeNullNorWhiteSpace(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            NotBeNull(value, valueType, parameterName, because, isElementInEnumerable);

            var shouldThrow = string.IsNullOrWhiteSpace((string)value);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is white space");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void BeEmptyGuid(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = ReferenceEquals(value, null) || ((Guid)value != Guid.Empty);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not an empty guid");
                throw new ArgumentException(exceptionMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength", Justification = "string.IsNullOrEmpty does not work here")]
        private static void BeEmptyString(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = (string)value != string.Empty;

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not an empty string");
                throw new ArgumentException(exceptionMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unused", Justification = "Cannot iterate without a local")]
        private static void BeEmptyEnumerable(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            NotBeNull(value, valueType, parameterName, because, isElementInEnumerable);

            var valueAsEnumerable = value as IEnumerable;
            var shouldThrow = false;

            // ReSharper disable once PossibleNullReferenceException
            foreach (var unused in valueAsEnumerable)
            {
                shouldThrow = true;
                break;
            }

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not an empty enumerable");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeEmptyGuid(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = (!ReferenceEquals(value, null)) && ((Guid)value == Guid.Empty);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is an empty guid");
                throw new ArgumentException(exceptionMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength", Justification = "string.IsNullOrEmpty does not work here")]
        private static void NotBeEmptyString(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = (string)value == string.Empty;

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is an empty string");
                throw new ArgumentException(exceptionMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unused", Justification = "Cannot iterate without a local")]
        private static void NotBeEmptyEnumerable(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            NotBeNull(value, valueType, parameterName, because, isElementInEnumerable);

            var valueAsEnumerable = value as IEnumerable;
            var shouldThrow = true;

            // ReSharper disable once PossibleNullReferenceException
            foreach (var unused in valueAsEnumerable)
            {
                shouldThrow = false;
                break;
            }

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is an empty enumerable");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void ContainSomeNulls(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            NotBeNull(value, valueType, parameterName, because, isElementInEnumerable);

            var valueAsEnumerable = value as IEnumerable;
            var shouldThrow = true;

            // ReSharper disable once PossibleNullReferenceException
            foreach (var unused in valueAsEnumerable)
            {
                if (ReferenceEquals(unused, null))
                {
                    shouldThrow = false;
                    break;
                }
            }

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "contains no null elements");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotContainAnyNulls(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            NotBeNull(value, valueType, parameterName, because, isElementInEnumerable);

            var valueAsEnumerable = value as IEnumerable;
            var shouldThrow = false;

            // ReSharper disable once PossibleNullReferenceException
            foreach (var unused in valueAsEnumerable)
            {
                if (ReferenceEquals(unused, null))
                {
                    shouldThrow = true;
                    break;
                }
            }

            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "contains at least one null element");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void BeDefault(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var defaultValue = GetDefaultValue(valueType);
            var shouldThrow = !IsEqualUsingDefaultEqualityComparer(valueType, value, defaultValue);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not equal to default(T)");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeDefault(
            object value,
            Type valueType,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var defaultValue = GetDefaultValue(valueType);
            var shouldThrow = IsEqualUsingDefaultEqualityComparer(valueType, value, defaultValue);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is equal to default(T)");
                throw new ArgumentException(exceptionMessage);
            }
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

        private static bool IsEqualUsingDefaultEqualityComparer<T>(
            T x,
            T y)
        {
            var result = EqualityComparer<T>.Default.Equals(x, y);
            return result;
        }

        private static bool IsEqualUsingDefaultEqualityComparer(
            Type type,
            object value1,
            object value2)
        {
            if (!IsEqualUsingDefaultEqualityComparerTypeToMethodInfoMap.ContainsKey(type))
            {
                IsEqualUsingDefaultEqualityComparerTypeToMethodInfoMap.TryAdd(type, IsEqualUsingDefaultEqualityComparerOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            var result = (bool)IsEqualUsingDefaultEqualityComparerTypeToMethodInfoMap[type].Invoke(null, new[] { value1, value2 });
            return result;
        }        
    }
}
