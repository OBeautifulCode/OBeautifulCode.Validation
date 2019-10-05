﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Assertion.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes
{
    /// <summary>
    /// Some delegates.
    /// </summary>
#if !OBeautifulCodeAssertionRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Assertion.Recipes", "See package version number")]
    internal
#else
    public
#endif
        static class Delegates
    {
        /// <summary>
        /// Executes a <see cref="Verification"/>.
        /// </summary>
        /// <param name="assertionTracker">The assertion tracker.</param>
        /// <param name="verification">The verification.</param>
        public delegate void VerificationHandler(
            AssertionTracker assertionTracker,
            Verification verification);

        /// <summary>
        /// Executes a <see cref="TypeValidation"/>.
        /// </summary>
        /// <param name="verification">The verification.</param>
        /// <param name="typeValidation">The type validation.</param>
        public delegate void TypeValidationHandler(
            Verification verification,
            TypeValidation typeValidation);
    }
}
