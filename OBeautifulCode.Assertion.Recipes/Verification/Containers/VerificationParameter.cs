﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationParameter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Assertion.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes
{
    using System;

    /// <summary>
    /// A parameter of the verification (e.g. the comparison value when verifying that a subject is greater than some comparison value)"/>
    /// </summary>
#if !OBeautifulCodeAssertionRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Assertion.Recipes", "See package version number")]
    internal
#else
    public
#endif
        class VerificationParameter
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets a func that can render the value as a string.
        /// </summary>
        public Func<string> ValueToStringFunc { get; set; }
    }
}
