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
        private delegate void CheckParameter(object parameterValue, string parameterName, string because, bool isElementInEnumerable);

        /// <summary>
        /// Validates that the parameter is null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeNull(
            this Parameter parameter,
            string because = null)
        {
            parameter.Validate(BeNull, because);
            return parameter;
        }

        /// <summary>
        /// Validates that the parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeNull(
            this Parameter parameter,
            string because = null)
        {
            parameter.Validate(NotBeNull, because);
            return parameter;
        }

        private static void Validate(
            this Parameter parameter,
            CheckParameter checkParameter,
            string because)
        {
            ParameterValidator.ThrowOnImproperUseOfFramework(parameter, ParameterShould.BeMusted);

            if (parameter.HasBeenEached)
            {
                if (parameter.Value is IEnumerable valueAsEnumerable)
                {
                    foreach (var element in valueAsEnumerable)
                    {
                        checkParameter(element, parameter.Name, because, isElementInEnumerable: true);
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
                checkParameter(parameter.Value, parameter.Name, because, isElementInEnumerable: false);
            }

            parameter.HasBeenValidated = true;
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
            object parameterValue,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (!ReferenceEquals(parameterValue, null))
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not null");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeNull(
            object parameterValue,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (ReferenceEquals(parameterValue, null))
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
    }
}
