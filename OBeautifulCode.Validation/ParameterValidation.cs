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
        static partial class ParameterValidation
    {
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfTypeCannotBeNull,
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeNull,
                ValidationName = nameof(BeNull),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfTypeCannotBeNull,
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeNull,
                ValidationName = nameof(NotBeNull),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(bool), typeof(bool?) },
                }                
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeTrue,
                ValidationName = nameof(BeTrue),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(bool), typeof(bool?) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeTrue,
                ValidationName = nameof(NotBeTrue),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(bool), typeof(bool?) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeFalse,
                ValidationName = nameof(BeFalse),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(bool), typeof(bool?) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeFalse,
                ValidationName = nameof(NotBeFalse),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(string) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeNullNorWhiteSpace,
                ValidationName = nameof(NotBeNullNorWhiteSpace),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(Guid), typeof(Guid?) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeEmptyGuid,
                ValidationName = nameof(BeEmptyGuid),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(Guid), typeof(Guid?) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeEmptyGuid,
                ValidationName = nameof(NotBeEmptyGuid),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(string) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeEmptyString,
                ValidationName = nameof(BeEmptyString),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(string) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeEmptyString,
                ValidationName = nameof(NotBeEmptyString),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(IEnumerable) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeEmptyEnumerable,
                ValidationName = nameof(BeEmptyEnumerable),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(IEnumerable) },
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeEmptyEnumerable,
                ValidationName = nameof(NotBeEmptyEnumerable),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(IEnumerable) },
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfEnumerableTypeCannotBeNull,
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = ContainSomeNulls,
                ValidationName = nameof(ContainSomeNulls),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(IEnumerable) },
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfEnumerableTypeCannotBeNull,
                }
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotContainAnyNulls,
                ValidationName = nameof(NotContainAnyNulls),
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotOfType,
                    ReferenceTypes = new[] { typeof(IEnumerable) },
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfEnumerableTypeCannotBeNull,
                }
            };

            var validationName = nameof(NotBeNullNorEmptyNorContainAnyNulls);

            var valueValidation1 = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeEmptyEnumerable,
                ValidationName = validationName,
            };

            parameter.Validate(typeValidations, valueValidation1);

            var valueValidation2 = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotContainAnyNulls,
                ValidationName = validationName,
            };

            parameter.Validate(null, valueValidation2);

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
            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeDefault,
                ValidationName = nameof(BeDefault),
            };

            parameter.Validate(null, valueValidation);
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
            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeDefault,
                ValidationName = nameof(NotBeDefault),
            };

            parameter.Validate(null, valueValidation);
            return parameter;
        }

        /// <summary>
        /// Validates that the IComparable or IComparable{T} type is less than some specified value.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="otherValue">The value to compare the parameter value to.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeLessThan<T>(
            [ValidatedNotNull] this Parameter parameter,
            T otherValue,
            string because = null)
        {
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotComparable,
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
                },
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeLessThan,
                ValidationName = nameof(BeLessThan),
                ValidationParameters = new[]
                {
                    new ValidationParameter
                    {
                        Name = nameof(otherValue),
                        Value = otherValue,
                        ValueType = typeof(T),
                    }
                }
            };
            
            parameter.Validate(typeValidations, valueValidation);
            return parameter;
        }

        /// <summary>
        /// Validates that the IComparable or IComparable{T} type is not less than some specified value.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="otherValue">The value to compare the parameter value to.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeLessThan<T>(
            [ValidatedNotNull] this Parameter parameter,
            T otherValue,
            string because = null)
        {
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotComparable,
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
                },
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeLessThan,
                ValidationName = nameof(NotBeLessThan),
                ValidationParameters = new[]
                {
                    new ValidationParameter
                    {
                        Name = nameof(otherValue),
                        Value = otherValue,
                        ValueType = typeof(T),
                    }
                }
            };

            parameter.Validate(typeValidations, valueValidation);
            return parameter;
        }

        /// <summary>
        /// Validates that the IComparable or IComparable{T} type is greater than than some specified value.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="otherValue">The value to compare the parameter value to.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeGreaterThan<T>(
            [ValidatedNotNull] this Parameter parameter,
            T otherValue,
            string because = null)
        {
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotComparable,
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
                },
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = BeGreaterThan,
                ValidationName = nameof(BeGreaterThan),
                ValidationParameters = new[]
                {
                    new ValidationParameter
                    {
                        Name = nameof(otherValue),
                        Value = otherValue,
                        ValueType = typeof(T),
                    }
                }
            };

            parameter.Validate(typeValidations, valueValidation);
            return parameter;
        }

        /// <summary>
        /// Validates that the IComparable or IComparable{T} type is not greater than than some specified value.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="otherValue">The value to compare the parameter value to.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeGreaterThan<T>(
            [ValidatedNotNull] this Parameter parameter,
            T otherValue,
            string because = null)
        {
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfNotComparable,
                },
                new TypeValidation
                {
                    TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
                },
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = NotBeGreaterThan,
                ValidationName = nameof(NotBeGreaterThan),
                ValidationParameters = new[]
                {
                    new ValidationParameter
                    {
                        Name = nameof(otherValue),
                        Value = otherValue,
                        ValueType = typeof(T),
                    }
                }
            };

            parameter.Validate(typeValidations, valueValidation);
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
            var typeValidations = new[]
            {
                new TypeValidation
                {
                    TypeValidationHandler = Throw,
                },
            };

            var valueValidation = new ValueValidation
            {
                Because = because,
                ValueValidationHandler = null,
                ValidationName = nameof(BeOfTypeThatDoesNotExist),                
            };

            parameter.Validate(typeValidations, valueValidation);
            return parameter;
        }
    }
}
