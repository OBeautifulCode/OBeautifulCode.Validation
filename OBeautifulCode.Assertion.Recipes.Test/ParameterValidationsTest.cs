﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidationsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text.RegularExpressions;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class ParameterValidationsTest
    {
        private static readonly ParameterEqualityComparer ParameterComparer = new ParameterEqualityComparer();

        private delegate AssertionTracker Validation(AssertionTracker assertionTracker, string because = null, ApplyBecause applyBecause = ApplyBecause.PrefixedToDefaultMessage, IDictionary data = null);

        [Fact]
        public static void GetEnumerableGenericType___Gets_the_correct_generic_type___When_called_with_various_flavors_of_IEnumerable()
        {
            // Arrange
            var values1 = new[] { string.Empty };
            var values2 = new List<string> { string.Empty };
            var values3 = new ArrayList();
            var values4 = new Dictionary<string, object>();
            IEnumerable<string> values5 = new List<string>();
            IReadOnlyCollection<string> values6 = new List<string>();

            var expectedStringMessage = "validationName: BeOfTypeThatDoesNotExist, isElementInEnumerable: True, parameterValueTypeName: string";
            var expectedObjectMessage = "validationName: BeOfTypeThatDoesNotExist, isElementInEnumerable: True, parameterValueTypeName: object";
            var expectedKvpMessage = "validationName: BeOfTypeThatDoesNotExist, isElementInEnumerable: True, parameterValueTypeName: KeyValuePair<string, object>";

            // Act
            // note: GetEnumerableGenericType is not public, so we're using BeOfNonExistentType which
            // always throws and checking that parameterValueTypeName is the expected type
            var actual1 = Record.Exception(() => values1.Must().Each().BeOfTypeThatDoesNotExist());
            var actual2 = Record.Exception(() => values2.Must().Each().BeOfTypeThatDoesNotExist());
            var actual3 = Record.Exception(() => values3.Must().Each().BeOfTypeThatDoesNotExist());
            var actual4 = Record.Exception(() => values4.Must().Each().BeOfTypeThatDoesNotExist());
            var actual5 = Record.Exception(() => values5.Must().Each().BeOfTypeThatDoesNotExist());
            var actual6 = Record.Exception(() => values6.Must().Each().BeOfTypeThatDoesNotExist());

            // Assert
            actual1.Should().BeOfType<InvalidCastException>();
            actual1.Message.Should().Be(expectedStringMessage);

            actual2.Should().BeOfType<InvalidCastException>();
            actual2.Message.Should().Be(expectedStringMessage);

            actual3.Should().BeOfType<InvalidCastException>();
            actual3.Message.Should().Be(expectedObjectMessage);

            actual4.Should().BeOfType<InvalidCastException>();
            actual4.Message.Should().Be(expectedKvpMessage);

            actual5.Should().BeOfType<InvalidCastException>();
            actual5.Message.Should().Be(expectedStringMessage);

            actual6.Should().BeOfType<InvalidCastException>();
            actual6.Message.Should().Be(expectedStringMessage);
        }

        [Fact]
        public static void Validations_with_validation_parameter___Should_throw_InvalidCastException_with_expected_Exception_message___When_validation_parameter_is_not_of_the_expected_type()
        {
            // Arrange
            var testParameter1 = A.Dummy<string>();
            var expected1 = "Called BeLessThan(comparisonValue:) where 'comparisonValue' is of type decimal, which is not one of the following expected type(s): string.";

            var testParameter2 = Some.ReadOnlyDummies<string>();
            var expected2 = "Called BeLessThan(comparisonValue:) where 'comparisonValue' is of type decimal, which is not one of the following expected type(s): string.";

            var testParameter3 = Some.ReadOnlyDummies<string>();
            var expected3 = "Called Contain(itemToSearchFor:) where 'itemToSearchFor' is of type decimal, which is not one of the following expected type(s): string.";

            var testParameter4 = new[] { Some.ReadOnlyDummies<string>(), Some.ReadOnlyDummies<string>() };
            var expected4 = "Called Contain(itemToSearchFor:) where 'itemToSearchFor' is of type decimal, which is not one of the following expected type(s): string.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeLessThan(A.Dummy<decimal>()));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeLessThan(A.Dummy<decimal>()));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().Contain(A.Dummy<decimal>()));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().Contain(A.Dummy<decimal>()));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void ApplyBecause___Should_not_alter_default_exception_message___When_ApplyBecause_is_PrefixedToDefaultMessage_and_because_is_null_or_white_space()
        {
            // Arrange
            Guid? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not an empty guid.  Parameter value is '<null>'.";

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty guid.  Element value is '6d062b50-03c1-4fa4-af8c-097b711214e7'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: null, applyBecause: ApplyBecause.PrefixedToDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: "  \r\n ", applyBecause: ApplyBecause.PrefixedToDefaultMessage));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ApplyBecause___Should_not_alter_default_exception_message___When_ApplyBecause_is_SuffixedToDefaultMessage_and_because_is_null_or_white_space()
        {
            // Arrange
            Guid? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not an empty guid.  Parameter value is '<null>'.";

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty guid.  Element value is '6d062b50-03c1-4fa4-af8c-097b711214e7'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: null, applyBecause: ApplyBecause.SuffixedToDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: "  \r\n ", applyBecause: ApplyBecause.SuffixedToDefaultMessage));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ApplyBecause___Should_prefix_default_exception_message_with_because___When_ApplyBecause_is_PrefixedToDefaultMessage_and_because_is_not_null_and_not_white_space()
        {
            // Arrange
            var because = A.Dummy<string>();

            Guid? testParameter1 = null;
            var expected1 = because + "  Parameter 'testParameter1' is not an empty guid.  Parameter value is '<null>'.";

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };
            var expected2 = because + "  Parameter 'testParameter2' contains an element that is not an empty guid.  Element value is '6d062b50-03c1-4fa4-af8c-097b711214e7'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: because, applyBecause: ApplyBecause.PrefixedToDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: because, applyBecause: ApplyBecause.PrefixedToDefaultMessage));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ApplyBecause___Should_suffix_default_exception_message_with_because___When_ApplyBecause_is_SuffixedToDefaultMessage_and_because_is_not_null_and_not_white_space()
        {
            // Arrange
            var because = A.Dummy<string>();

            Guid? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not an empty guid.  Parameter value is '<null>'.  " + because;

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty guid.  Element value is '6d062b50-03c1-4fa4-af8c-097b711214e7'.  " + because;

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: because, applyBecause: ApplyBecause.SuffixedToDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: because, applyBecause: ApplyBecause.SuffixedToDefaultMessage));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ApplyBecause___Should_replace_default_exception_message_with_empty_string___When_ApplyBecause_is_InLieuOfDefaultMessage_and_because_is_null()
        {
            // Arrange
            Guid? testParameter1 = null;

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: null, applyBecause: ApplyBecause.InLieuOfDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: null, applyBecause: ApplyBecause.InLieuOfDefaultMessage));

            // Assert
            actual1.Message.Should().Be(string.Empty);
            actual2.Message.Should().Be(string.Empty);
        }

        [Fact]
        public static void ApplyBecause___Should_replace_default_exception_message_with_because___When_ApplyBecause_is_InLieuOfDefaultMessage_and_because_is_not_null()
        {
            // Arrange
            var because = A.Dummy<string>();

            Guid? testParameter1 = null;

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid(because: string.Empty, applyBecause: ApplyBecause.InLieuOfDefaultMessage));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid(because: because, applyBecause: ApplyBecause.InLieuOfDefaultMessage));

            // Assert
            actual1.Message.Should().Be(string.Empty);
            actual2.Message.Should().Be(because);
        }

        [Fact]
        public static void BeNull___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeNull,
                ValidationName = nameof(Verifications.BeNull),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "Any Reference Type, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { null, null },
                },
                MustFailingValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { null, Guid.NewGuid(), null },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null, null },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                },
                MustEachFailingValues = new IEnumerable<string>[]
                {
                    new string[] { null, string.Empty, null },
                    new string[] { null, " \r\n ", null },
                    new string[] { null, A.Dummy<string>(), null },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { null, null },
                },
                MustFailingValues = new object[]
                {
                    A.Dummy<object>(),
                },
                MustEachFailingValues = new IEnumerable<object>[]
                {
                    new object[] { null, A.Dummy<object>(), null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void BeNull___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            decimal? testParameter1 = 5;
            var expected1 = "Parameter 'testParameter1' is not null.  Parameter value is '5'.";

            var testParameter2 = new decimal?[] { null, -6, null };
            var expected2 = "Parameter 'testParameter2' contains an element that is not null.  Element value is '-6'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeNull());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeNull());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeNull___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeNull,
                ValidationName = nameof(Verifications.NotBeNull),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "Any Reference Type, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                },
                MustFailingValues = new Guid?[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<string>[]
                {
                    new string[] { string.Empty, null, " \r\n  " },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[]
                {
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachPassingValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                },
                MustFailingValues = new object[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<object>[]
                {
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void NotBeNull___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            decimal? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is null.";

            var testParameter2 = new decimal?[] { -6, null, -5 };
            var expected2 = "Parameter 'testParameter2' contains an element that is null.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeNull());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeNull());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeTrue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeTrue,
                ValidationName = nameof(Verifications.BeTrue),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeTrueExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "bool, bool?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<bool>, IEnumerable<bool?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                    new string[] { string.Empty, null, " \r\n  " },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    A.Dummy<object>(),
                    new List<string> { null },
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[]
                {
                    true,
                },
                MustEachPassingValues = new[]
                {
                    new bool[] { },
                    new bool[] { true, true },
                },
                MustFailingValues = new[]
                {
                    false,
                },
                MustEachFailingValues = new[]
                {
                    new bool[] { false, false },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[]
                {
                    true,
                },
                MustEachPassingValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true, true },
                },
                MustFailingValues = new bool?[]
                {
                    false,
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new bool?[] { false, null },
                    new bool?[] { null, false },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void BeTrue___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            bool? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not true.  Parameter value is '<null>'.";

            var testParameter2 = new[] { true, false, true };
            var expected2 = "Parameter 'testParameter2' contains an element that is not true.  Element value is 'False'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeTrue());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeTrue());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeTrue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeTrue,
                ValidationName = nameof(Verifications.NotBeTrue),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeTrueExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "bool, bool?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<bool>, IEnumerable<bool?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                    new string[] { string.Empty, null, " \r\n  " },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[]
                {
                    false,
                },
                MustEachPassingValues = new[]
                {
                    new bool[] { },
                    new bool[] { false, false },
                },
                MustFailingValues = new[]
                {
                    true,
                },
                MustEachFailingValues = new[]
                {
                    new[] { true, true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[]
                {
                    false,
                    null,
                },
                MustEachPassingValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { false, null },
                    new bool?[] { null, false },
                },
                MustFailingValues = new bool?[]
                {
                    true,
                },
                MustEachFailingValues = new[]
                {
                    new bool?[] { true, true },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeTrue___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            bool? testParameter1 = true;
            var expected1 = "Parameter 'testParameter1' is true.";

            var testParameter2 = new[] { false, true, false };
            var expected2 = "Parameter 'testParameter2' contains an element that is true.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeTrue());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeTrue());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeFalse___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeFalse,
                ValidationName = nameof(Verifications.BeFalse),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeFalseExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "bool, bool?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<bool>, IEnumerable<bool?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                    new string[] { string.Empty, null, " \r\n  " },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[]
                {
                    false,
                },
                MustEachPassingValues = new[]
                {
                    new bool[] { },
                    new bool[] { false, false },
                },
                MustFailingValues = new[]
                {
                    true,
                },
                MustEachFailingValues = new[]
                {
                    new bool[] { true, true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[]
                {
                    false,
                },
                MustEachPassingValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { false },
                },
                MustFailingValues = new bool?[]
                {
                    true,
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new bool?[] { true, null },
                    new bool?[] { null, true },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void BeFalse___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            bool? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not false.  Parameter value is '<null>'.";

            var testParameter2 = new[] { false, true, false };
            var expected2 = "Parameter 'testParameter2' contains an element that is not false.  Element value is 'True'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeFalse());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeFalse());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeFalse___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeFalse,
                ValidationName = nameof(Verifications.NotBeFalse),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeFalseExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "bool, bool?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<bool>, IEnumerable<bool?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    string.Empty,
                    " \r\n  ",
                    A.Dummy<string>(),
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                    new string[] { string.Empty, null, " \r\n  " },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[]
                {
                    true,
                },
                MustEachPassingValues = new[]
                {
                    new bool[] { },
                    new bool[] { true, true },
                },
                MustFailingValues = new[]
                {
                    false,
                },
                MustEachFailingValues = new[]
                {
                    new bool[] { false, false },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[]
                {
                    true,
                    null,
                },
                MustEachPassingValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true, null },
                    new bool?[] { null, true },
                },
                MustFailingValues = new bool?[]
                {
                    false,
                },
                MustEachFailingValues = new[]
                {
                    new bool?[] { false, false },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeFalse___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            bool? testParameter1 = false;
            var expected1 = "Parameter 'testParameter1' is false.";

            var testParameter2 = new[] { true, false, true };
            var expected2 = "Parameter 'testParameter2' contains an element that is false.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeFalse());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeFalse());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeNullNorWhiteSpace___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest1 = new ValidationTest
            {
                Validation = Verifications.NotBeNullNorWhiteSpace,
                ValidationName = nameof(Verifications.NotBeNullNorWhiteSpace),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = Verifications.NotBeNullNorWhiteSpace,
                ValidationName = nameof(Verifications.NotBeNullNorWhiteSpace),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullNorWhiteSpaceExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), null, A.Dummy<string>() },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    "    ",
                    " \r\n  ",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), "    ", A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), " \r\n  ", A.Dummy<string>() },
                },
            };

            // Act, Assert
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(stringTestValues1);

            validationTest2.Run(guidTestValues);
            validationTest2.Run(nullableGuidTestValues);
            validationTest2.Run(objectTestValues);
            validationTest2.Run(stringTestValues2);
        }

        [Fact]
        public static void NotBeNullNorWhiteSpace___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = "\r\n";
            var expected1 = Invariant($"Parameter 'testParameter1' is white space.  Parameter value is '{Environment.NewLine}'.");

            var testParameter2 = new[] { A.Dummy<string>(), "    ", A.Dummy<string>() };
            var expected2 = "Parameter 'testParameter2' contains an element that is white space.  Element value is '    '.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeNullNorWhiteSpace());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeNullNorWhiteSpace());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeNullOrNotWhiteSpace___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeNullOrNotWhiteSpace,
                ValidationName = nameof(Verifications.BeNullOrNotWhiteSpace),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeNullOrNotWhiteSpaceExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    null,
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), null, A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    "  \r\n  ",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), " \r\n ", A.Dummy<string>() },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(stringTestValues);
        }

        [Fact]
        public static void BeNullOrNotWhiteSpace___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = "\r\n";
            var expected1 = Invariant($"Parameter 'testParameter1' is not null and is white space.  Parameter value is '{Environment.NewLine}'.");

            var testParameter2 = new[] { A.Dummy<string>(), "    ", A.Dummy<string>() };
            var expected2 = "Parameter 'testParameter2' contains an element that is not null and is white space.  Element value is '    '.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeNullOrNotWhiteSpace());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeNullOrNotWhiteSpace());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeEmptyGuid___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeEmptyGuid,
                ValidationName = nameof(Verifications.BeEmptyGuid),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeEmptyGuidExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "Guid, Guid?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<Guid>, IEnumerable<Guid?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustPassingValues = new[]
                {
                    Guid.Empty,
                },
                MustEachPassingValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                },
                MustFailingValues = new[]
                {
                    Guid.NewGuid(),
                },
                MustEachFailingValues = new[]
                {
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    Guid.Empty,
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                },
                MustFailingValues = new Guid?[]
                {
                    null,
                    Guid.NewGuid(),
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    "   ",
                    "   \r\n ",
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new[] { string.Empty },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string>(),
                    new List<string> { string.Empty },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(enumerableTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void BeEmptyGuid___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            Guid? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not an empty guid.  Parameter value is '<null>'.";

            var testParameter2 = new Guid[] { Guid.Empty, Guid.Parse("6d062b50-03c1-4fa4-af8c-097b711214e7"), Guid.Empty };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty guid.  Element value is '6d062b50-03c1-4fa4-af8c-097b711214e7'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyGuid());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyGuid());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeEmptyGuid___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeEmptyGuid,
                ValidationName = nameof(Verifications.NotBeEmptyGuid),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyGuidExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "Guid, Guid?",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<Guid>, IEnumerable<Guid?>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustPassingValues = new[]
                {
                    Guid.NewGuid(),
                },
                MustEachPassingValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
                },
                MustFailingValues = new[]
                {
                    Guid.Empty,
                },
                MustEachFailingValues = new[]
                {
                    new Guid[] { Guid.NewGuid(), Guid.Empty, Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    null,
                    Guid.NewGuid(),
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid() },
                    new Guid?[] { null },
                },
                MustFailingValues = new Guid?[]
                {
                    Guid.Empty,
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { Guid.NewGuid(), Guid.Empty, Guid.NewGuid() },
                    new Guid?[] { null, Guid.Empty, null },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    "   ",
                    "   \r\n ",
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new[] { string.Empty },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string>(),
                    new List<string> { string.Empty },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(enumerableTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeEmptyGuid___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            Guid? testParameter1 = Guid.Empty;
            var expected1 = "Parameter 'testParameter1' is an empty guid.";

            var testParameter2 = new Guid[] { Guid.NewGuid(), Guid.Empty, Guid.NewGuid() };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty guid.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeEmptyGuid());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeEmptyGuid());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeEmptyString___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeEmptyString,
                ValidationName = nameof(Verifications.BeEmptyString),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeEmptyStringExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty },
                },
                MustFailingValues = new[]
                {
                    null,
                    "   ",
                    "   \r\n ",
                    A.Dummy<string>(),
                },
                MustEachFailingValues = new[]
                {
                    new string[] { null, A.Dummy<string>() },
                    new string[] { "    ", A.Dummy<string>() },
                    new string[] { "  \r\n  ", A.Dummy<string>() },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string>(),
                    new List<string> { string.Empty },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(enumerableTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void BeEmptyString___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is not an empty string.  Parameter value is '<null>'.";

            var testParameter2 = new[] { string.Empty, "abcd", string.Empty };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty string.  Element value is 'abcd'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyString());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyString());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeEmptyString___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeEmptyString,
                ValidationName = nameof(Verifications.NotBeEmptyString),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyStringExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    null,
                    "   ",
                    "   \r\n ",
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { null, A.Dummy<string>() },
                    new string[] { "    ", A.Dummy<string>() },
                    new string[] { "  \r\n  ", A.Dummy<string>() },
                },
                MustFailingValues = new[]
                {
                    string.Empty,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string>(),
                    new List<string> { string.Empty },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(enumerableTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeEmptyString___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = string.Empty;
            var expected1 = "Parameter 'testParameter1' is an empty string.";

            var testParameter2 = new[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty string.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeEmptyString());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeEmptyString());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeEmptyEnumerable___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest1 = new ValidationTest
            {
                Validation = Verifications.BeEmptyEnumerable,
                ValidationName = nameof(Verifications.BeEmptyEnumerable),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = Verifications.BeEmptyEnumerable,
                ValidationName = nameof(Verifications.BeEmptyEnumerable),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeEmptyEnumerableExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, null, string.Empty },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty },
                },
                MustFailingValues = new[]
                {
                    "   ",
                    "   \r\n ",
                    A.Dummy<string>(),
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "    ", string.Empty },
                    new string[] { string.Empty, "  \r\n  ", string.Empty },
                    new string[] { string.Empty, A.Dummy<string>(), string.Empty },
                },
            };

            var enumerableTestValues1 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { }, null, new string[] { } },
                },
            };

            var enumerableTestValues2A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { }, new List<string> { null }, new string[] { } },
                },
            };

            var enumerableTestValues2B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new List<string>(), new string[] { } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new string[] { }, new List<string> { null }, new string[] { } },
                },
            };

            var enumerableTestValues2C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string>(), new List<string>() },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string>() { string.Empty },
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string> { }, new List<string> { null }, new List<string> { } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues1);

            validationTest2.Run(stringTestValues2);
            validationTest2.Run(enumerableTestValues2A);
            validationTest2.Run(enumerableTestValues2B);
            validationTest2.Run(enumerableTestValues2C);
        }

        [Fact]
        public static void BeEmptyEnumerable___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new[] { A.Dummy<object>() };
            var expected1 = "Parameter 'testParameter1' is not an empty enumerable.";

            var testParameter2 = new[] { new object[] { }, new[] { A.Dummy<object>() }, new object[] { } };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty enumerable.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyEnumerable());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyEnumerable());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeEmptyEnumerable___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest1 = new ValidationTest
            {
                Validation = Verifications.NotBeEmptyEnumerable,
                ValidationName = nameof(Verifications.NotBeEmptyEnumerable),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = Verifications.NotBeEmptyEnumerable,
                ValidationName = nameof(Verifications.NotBeEmptyEnumerable),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyEnumerableExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), null, A.Dummy<string>() },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "   ",
                    "   \r\n ",
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "    ", "    ", "  \r\n ", A.Dummy<string>() },
                },
                MustFailingValues = new[]
                {
                    string.Empty,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
            };

            var enumerableTestValues1 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { A.Dummy<string>() } },
                },
            };

            var enumerableTestValues2A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, new List<string> { null }, new string[] { string.Empty } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues2B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new string[] { A.Dummy<string>() }, new List<string> { null }, new string[] { string.Empty } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues2C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string>() { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string> { A.Dummy<string>() }, new List<string> { null }, new List<string> { string.Empty } },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string>(),
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string>(), new List<string>() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues1);

            validationTest2.Run(stringTestValues2);
            validationTest2.Run(enumerableTestValues2A);
            validationTest2.Run(enumerableTestValues2B);
            validationTest2.Run(enumerableTestValues2C);
        }

        [Fact]
        public static void NotBeEmptyEnumerable___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new object[] { };
            var expected1 = "Parameter 'testParameter1' is an empty enumerable.";

            var testParameter2 = new[] { new[] { A.Dummy<object>() }, new object[] { }, new[] { A.Dummy<object>() } };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty enumerable.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeEmptyEnumerable());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeEmptyEnumerable());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeEmptyDictionary___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.BeEmptyDictionary;
            var validationName = nameof(Verifications.BeEmptyDictionary);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>(), null, new Dictionary<string, string>() },
                },
            };

            validationTest2.Run(dictionaryTest);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeEmptyDictionaryExceptionMessageSuffix,
            };

            var dictionaryTest3A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary(),
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary[] { },
                    new IDictionary[] { new ListDictionary(), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary(),
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary(),
                    },
                },
            };

            var dictionaryTest3B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[] { new Dictionary<string, string>(), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                    },
                },
            };

            var dictionaryTest3C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[] { new Dictionary<string, string>(), new Dictionary<string, string>(new Dictionary<string, string>()) },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    new Dictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                    },
                },
            };

            var dictionaryTest3D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[] { new Dictionary<string, string>(), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                    },
                },
            };

            var dictionaryTest3E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[] { new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()) },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                    },
                },
            };

            validationTest3.Run(dictionaryTest3A);
            validationTest3.Run(dictionaryTest3B);
            validationTest3.Run(dictionaryTest3C);
            validationTest3.Run(dictionaryTest3D);
            validationTest3.Run(dictionaryTest3E);
        }

        [Fact]
        public static void BeEmptyDictionary___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } };
            var expected1 = "Parameter 'testParameter1' is not an empty dictionary.";

            var testParameter2 = new IReadOnlyDictionary<string, string>[]
            {
                new Dictionary<string, string>(),
                new Dictionary<string, string>(), new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
            };
            var expected2 = "Parameter 'testParameter2' contains an element that is not an empty dictionary.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEmptyDictionary());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeEmptyDictionary());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeEmptyDictionary___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotBeEmptyDictionary;
            var validationName = nameof(Verifications.NotBeEmptyDictionary);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, null, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
            };

            validationTest2.Run(dictionaryTest);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyDictionaryExceptionMessageSuffix,
            };

            var dictionaryTest3A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary[] { },
                    new IDictionary[] { new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary(),
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>(),
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[] { new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
            };

            validationTest3.Run(dictionaryTest3A);
            validationTest3.Run(dictionaryTest3B);
            validationTest3.Run(dictionaryTest3C);
            validationTest3.Run(dictionaryTest3D);
            validationTest3.Run(dictionaryTest3E);
        }

        [Fact]
        public static void NotBeEmptyDictionary___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new Dictionary<string, string>();
            var expected1 = "Parameter 'testParameter1' is an empty dictionary.";

            var testParameter2 = new IReadOnlyDictionary<string, string>[]
            {
                new Dictionary<string, string>(), new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                new Dictionary<string, string>(),
            };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty dictionary.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeEmptyDictionary());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeEmptyDictionary());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ContainSomeNullElements___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.ContainSomeNullElements;
            var validationName = nameof(Verifications.ContainSomeNullElements);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>, IEnumerable when not IEnumerable<Any Value Type>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable<Any Reference Type>>, IEnumerable<IEnumerable<Nullable<T>>>, IEnumerable<IEnumerable when not IEnumerable<Any Value Type>>",
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { null, A.Dummy<string>() },
                    new string[] { string.Empty, null },
                    new string[] { A.Dummy<string>() },
                },
            };

            var enumerableTestValues2 = new TestValues<IEnumerable<bool>>
            {
                MustParameterInvalidTypeValues = new IEnumerable<bool>[]
                {
                    new bool[] { },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable<bool>[] { new bool[] { }, },
                },
            };

            validationTest2.Run(enumerableTestValues2);
            validationTest2.Run(stringTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var enumerableTestValues3 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>(), null }, null, new string[] { A.Dummy<string>(), null } },
                },
            };

            validationTest3.Run(enumerableTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.ContainSomeNullElementsExceptionMessageSuffix,
            };

            var enumerableTestValues4A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string> { A.Dummy<string>(), null },
                    new string[] { A.Dummy<string>(), null },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string> { null }, new string[] { A.Dummy<string>(), null } },
                    new IEnumerable[] { new List<string> { A.Dummy<string>(), null } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { null }, new List<string> { A.Dummy<string>(), null }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            var enumerableTestValues4B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string> { A.Dummy<string>(), null },
                    new string[] { A.Dummy<string>(), null },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new List<string> { null }, new string[] { A.Dummy<string>(), null } },
                    new IList[] { new List<string> { A.Dummy<string>(), null } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new string[] { null }, new List<string> { A.Dummy<string>(), null }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            var enumerableTestValues4C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string> { A.Dummy<string>(), null },
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string> { null }, new List<string> { A.Dummy<string>(), null } },
                    new List<string>[] { new List<string> { A.Dummy<string>(), null } },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new List<string> { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string> { null }, new List<string> { A.Dummy<string>(), null }, new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            validationTest4.Run(enumerableTestValues4A);
            validationTest4.Run(enumerableTestValues4B);
            validationTest4.Run(enumerableTestValues4C);
        }

        [Fact]
        public static void ContainSomeNullElements___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new[] { A.Dummy<object>() };
            var expected1 = "Parameter 'testParameter1' contains no null elements.";

            var testParameter2 = new[] { new object[] { }, new object[] { }, new object[] { } };
            var expected2 = "Parameter 'testParameter2' contains an element that contains no null elements.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().ContainSomeNullElements());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().ContainSomeNullElements());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotContainAnyNullElements___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotContainAnyNullElements;
            var validationName = nameof(Verifications.NotContainAnyNullElements);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>, IEnumerable when not IEnumerable<Any Value Type>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable<Any Reference Type>>, IEnumerable<IEnumerable<Nullable<T>>>, IEnumerable<IEnumerable when not IEnumerable<Any Value Type>>",
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { null, A.Dummy<string>() },
                    new string[] { string.Empty, null },
                    new string[] { A.Dummy<string>() },
                },
            };

            var enumerableTestValues2 = new TestValues<IEnumerable<bool>>
            {
                MustParameterInvalidTypeValues = new IEnumerable<bool>[]
                {
                    new bool[] { },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable<bool>[] { new bool[] { }, },
                },
            };

            validationTest2.Run(enumerableTestValues2);
            validationTest2.Run(stringTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var enumerableTestValues3 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>(), A.Dummy<string>() }, null, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            validationTest3.Run(enumerableTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotContainAnyNullElementsExceptionMessageSuffix,
            };

            var enumerableTestValues4A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string> { },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string> { }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                    new IEnumerable[] { new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new string[] { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            var enumerableTestValues4B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string> { },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new List<string> { }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                    new IList[] { new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new string[] { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new string[] { }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            var enumerableTestValues4C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string> { },
                    new List<string> { A.Dummy<string>(), A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string> { }, new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                    new List<string>[] { new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new List<string> { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string> { }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new List<string> { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            validationTest4.Run(enumerableTestValues4A);
            validationTest4.Run(enumerableTestValues4B);
            validationTest4.Run(enumerableTestValues4C);
        }

        [Fact]
        public static void NotContainAnyNullElements___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new[] { A.Dummy<object>(), null, A.Dummy<object>() };
            var expected1 = "Parameter 'testParameter1' contains at least one null element.";

            var testParameter2 = new[] { new object[] { }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() }, new object[] { } };
            var expected2 = "Parameter 'testParameter2' contains an element that contains at least one null element.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotContainAnyNullElements());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotContainAnyNullElements());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void ContainSomeKeyValuePairsWithNullValue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.ContainSomeKeyValuePairsWithNullValue;
            var validationName = nameof(Verifications.ContainSomeKeyValuePairsWithNullValue);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey,Any Reference Type>, IDictionary<TKey,Nullable<T>>, IReadOnlyDictionary<TKey,Any Reference Type>, IReadOnlyDictionary<TKey,Nullable<T>>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey,Any Reference Type>>, IEnumerable<IDictionary<TKey,Nullable<T>>>, IEnumerable<IReadOnlyDictionary<TKey,Any Reference Type>>, IEnumerable<IReadOnlyDictionary<TKey,Nullable<T>>>",
            };

            var dictionaryTest2A = new TestValues<IDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2B = new TestValues<Dictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new Dictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2C = new TestValues<IReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IReadOnlyDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2D = new TestValues<ReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new ReadOnlyDictionary<string, bool>[]
                {
                    new ReadOnlyDictionary<string, bool>(new Dictionary<string, bool>()),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new ReadOnlyDictionary<string, bool>[] { },
                },
            };

            validationTest2.Run(dictionaryTest2A);
            validationTest2.Run(dictionaryTest2B);
            validationTest2.Run(dictionaryTest2C);
            validationTest2.Run(dictionaryTest2D);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest3 = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>() { { A.Dummy<string>(), null } }, null, new Dictionary<string, string>() { { A.Dummy<string>(), null } } },
                },
            };

            validationTest3.Run(dictionaryTest3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.ContainSomeKeyValuePairsWithNullValueExceptionMessageSuffix,
            };

            var dictionaryTest4A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), null } },
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachPassingValues = new[]
                {
                    new ListDictionary[] { },
                    new ListDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), null } },
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                    },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary(),
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), null } },
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary() { { A.Dummy<string>(), null } },
                    },
                },
            };

            var dictionaryTest4B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                    },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                    },
                },
            };

            var dictionaryTest4C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                    },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                    },
                },
            };

            var dictionaryTest4D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                    },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                    },
                },
            };

            var dictionaryTest4E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), null } }),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), null } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } }),
                    },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), null } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), null } }),
                    },
                },
            };

            validationTest4.Run(dictionaryTest4A);
            validationTest4.Run(dictionaryTest4B);
            validationTest4.Run(dictionaryTest4C);
            validationTest4.Run(dictionaryTest4D);
            validationTest4.Run(dictionaryTest4E);
        }

        [Fact]
        public static void ContainSomeKeyValuePairsWithNullValue___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new Dictionary<string, string>();
            var expected1 = "Parameter 'testParameter1' contains no key-value pairs with a null value.";

            var testParameter2 = new[] { new Dictionary<string, string>() };
            var expected2 = "Parameter 'testParameter2' contains an element that contains no key-value pairs with a null value.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().ContainSomeKeyValuePairsWithNullValue());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().ContainSomeKeyValuePairsWithNullValue());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotContainAnyKeyValuePairsWithNullValue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotContainAnyKeyValuePairsWithNullValue;
            var validationName = nameof(Verifications.NotContainAnyKeyValuePairsWithNullValue);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey,Any Reference Type>, IDictionary<TKey,Nullable<T>>, IReadOnlyDictionary<TKey,Any Reference Type>, IReadOnlyDictionary<TKey,Nullable<T>>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey,Any Reference Type>>, IEnumerable<IDictionary<TKey,Nullable<T>>>, IEnumerable<IReadOnlyDictionary<TKey,Any Reference Type>>, IEnumerable<IReadOnlyDictionary<TKey,Nullable<T>>>",
            };

            var dictionaryTest2A = new TestValues<IDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2B = new TestValues<Dictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new Dictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2C = new TestValues<IReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IReadOnlyDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2D = new TestValues<ReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new ReadOnlyDictionary<string, bool>[]
                {
                    new ReadOnlyDictionary<string, bool>(new Dictionary<string, bool>()),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new ReadOnlyDictionary<string, bool>[] { },
                },
            };

            validationTest2.Run(dictionaryTest2A);
            validationTest2.Run(dictionaryTest2B);
            validationTest2.Run(dictionaryTest2C);
            validationTest2.Run(dictionaryTest2D);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest3 = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, null, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
            };

            validationTest3.Run(dictionaryTest3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotContainAnyKeyValuePairsWithNullValueExceptionMessageSuffix,
            };

            var dictionaryTest4A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary(),
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new ListDictionary[] { },
                    new ListDictionary[]
                    {
                        new ListDictionary(),
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), null } },
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary() { { A.Dummy<string>(), null } },
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string>(),
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), null } }),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } }),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), null } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
            };

            validationTest4.Run(dictionaryTest4A);
            validationTest4.Run(dictionaryTest4B);
            validationTest4.Run(dictionaryTest4C);
            validationTest4.Run(dictionaryTest4D);
            validationTest4.Run(dictionaryTest4E);
        }

        [Fact]
        public static void NotContainAnyKeyValuePairsWithNullValue___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new Dictionary<string, string>() { { A.Dummy<string>(), null } };
            var expected1 = "Parameter 'testParameter1' contains at least one key-value pair with a null value.";

            var testParameter2 = new[] { new Dictionary<string, string>() { { A.Dummy<string>(), null } } };
            var expected2 = "Parameter 'testParameter2' contains an element that contains at least one key-value pair with a null value.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotContainAnyKeyValuePairsWithNullValue());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotContainAnyKeyValuePairsWithNullValue());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeNullNorEmptyEnumerable___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest1 = new ValidationTest
            {
                Validation = Verifications.NotBeNullNorEmptyEnumerable,
                ValidationName = nameof(Verifications.NotBeNullNorEmptyEnumerable),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = Verifications.NotBeNullNorEmptyEnumerable,
                ValidationName = nameof(Verifications.NotBeNullNorEmptyEnumerable),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyEnumerableExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), null, A.Dummy<string>() },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "   ",
                    "   \r\n ",
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "    ", "    ", "  \r\n ", A.Dummy<string>() },
                },
                MustFailingValues = new[]
                {
                    string.Empty,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
            };

            var enumerableTestValues1 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { A.Dummy<string>() } },
                },
            };

            var enumerableTestValues2A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, new List<string> { null }, new string[] { string.Empty } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues2B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string> { string.Empty },
                    new string[] { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new string[] { A.Dummy<string>() }, new List<string> { null }, new string[] { string.Empty } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues2C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string>() { string.Empty },
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string> { A.Dummy<string>() }, new List<string> { null }, new List<string> { string.Empty } },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string>(),
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string>(), new List<string>() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues1);

            validationTest2.Run(stringTestValues2);
            validationTest2.Run(enumerableTestValues2A);
            validationTest2.Run(enumerableTestValues2B);
            validationTest2.Run(enumerableTestValues2C);
        }

        [Fact]
        public static void NotBeNullNorEmptyEnumerable___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new object[] { };
            var expected1 = "Parameter 'testParameter1' is an empty enumerable.";

            var testParameter2 = new[] { new[] { A.Dummy<object>() }, new object[] { }, new[] { A.Dummy<object>() } };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty enumerable.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeNullNorEmptyEnumerable());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeNullNorEmptyEnumerable());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeNullNorEmptyDictionary___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotBeNullNorEmptyDictionary;
            var validationName = nameof(Verifications.NotBeNullNorEmptyDictionary);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, null, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
            };

            validationTest2.Run(dictionaryTest);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyDictionaryExceptionMessageSuffix,
            };

            var dictionaryTest3A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary[] { },
                    new IDictionary[] { new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary(),
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>(),
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest3E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[] { new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
            };

            validationTest3.Run(dictionaryTest3A);
            validationTest3.Run(dictionaryTest3B);
            validationTest3.Run(dictionaryTest3C);
            validationTest3.Run(dictionaryTest3D);
            validationTest3.Run(dictionaryTest3E);
        }

        [Fact]
        public static void NotBeNullNorEmptyDictionary___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new Dictionary<string, string>();
            var expected1 = "Parameter 'testParameter1' is an empty dictionary.";

            var testParameter2 = new IReadOnlyDictionary<string, string>[]
            {
                new Dictionary<string, string>(), new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                new Dictionary<string, string>(),
            };
            var expected2 = "Parameter 'testParameter2' contains an element that is an empty dictionary.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeNullNorEmptyDictionary());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeNullNorEmptyDictionary());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeNullNorEmptyEnumerableNorContainAnyNulls___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotBeNullNorEmptyEnumerableNorContainAnyNulls;
            var validationName = nameof(Verifications.NotBeNullNorEmptyEnumerableNorContainAnyNulls);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>, IEnumerable when not IEnumerable<Any Value Type>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable<Any Reference Type>>, IEnumerable<IEnumerable<Nullable<T>>>, IEnumerable<IEnumerable when not IEnumerable<Any Value Type>>",
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { null, A.Dummy<string>() },
                    new string[] { string.Empty, null },
                    new string[] { A.Dummy<string>() },
                },
            };

            var enumerableTestValues2 = new TestValues<IEnumerable<bool>>
            {
                MustParameterInvalidTypeValues = new IEnumerable<bool>[]
                {
                    new bool[] { },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable<bool>[] { new bool[] { }, },
                },
            };

            validationTest2.Run(enumerableTestValues2);
            validationTest2.Run(stringTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var enumerableTestValues3 = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>(), A.Dummy<string>() }, null, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            validationTest3.Run(enumerableTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyEnumerableExceptionMessageSuffix,
            };

            var enumerableTestValues4A = new TestValues<IEnumerable>
            {
                MustFailingValues = new IEnumerable[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues4B = new TestValues<IList>
            {
                MustFailingValues = new IList[]
                {
                    new List<string>(),
                    new string[] { },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new List<string>(), new string[] { } },
                },
            };

            var enumerableTestValues4C = new TestValues<List<string>>
            {
                MustFailingValues = new List<string>[]
                {
                    new List<string>(),
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string>(), new List<string>() },
                },
            };

            validationTest4.Run(enumerableTestValues4A);
            validationTest4.Run(enumerableTestValues4B);
            validationTest4.Run(enumerableTestValues4C);

            var validationTest5 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotContainAnyNullElementsExceptionMessageSuffix,
            };

            var enumerableTestValues5A = new TestValues<IEnumerable>
            {
                MustPassingValues = new IEnumerable[]
                {
                    new List<string> { A.Dummy<string>(), },
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new IEnumerable[] { },
                    new IEnumerable[] { new List<string> { string.Empty }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new IEnumerable[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new string[] { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable[] { new string[] { string.Empty }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            var enumerableTestValues5B = new TestValues<IList>
            {
                MustPassingValues = new IList[]
                {
                    new List<string> { A.Dummy<string>(), },
                    new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new IList[] { },
                    new IList[] { new List<string> { string.Empty }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new IList[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new string[] { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new IList[] { new string[] { string.Empty }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            var enumerableTestValues5C = new TestValues<List<string>>
            {
                MustPassingValues = new List<string>[]
                {
                    new List<string> { A.Dummy<string>(), },
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                },
                MustEachPassingValues = new[]
                {
                    new List<string>[] { },
                    new List<string>[] { new List<string> { string.Empty }, new List<string> { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustFailingValues = new List<string>[]
                {
                    new List<string> { A.Dummy<string>(), null, A.Dummy<string>() },
                    new List<string> { null, A.Dummy<string>() },
                },
                MustEachFailingValues = new[]
                {
                    new List<string>[] { new List<string> { string.Empty }, new List<string> { A.Dummy<string>(), A.Dummy<string>() }, new List<string> { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            validationTest5.Run(enumerableTestValues5A);
            validationTest5.Run(enumerableTestValues5B);
            validationTest5.Run(enumerableTestValues5C);
        }

        [Fact]
        public static void NotBeNullNorEmptyDictionaryNorContainAnyNullValues___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation validation = Verifications.NotBeNullNorEmptyDictionaryNorContainAnyNullValues;
            var validationName = nameof(Verifications.NotBeNullNorEmptyDictionaryNorContainAnyNullValues);

            var validationTest1 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey, TValue>>, IEnumerable<IReadOnlyDictionary<TKey, TValue>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustParameterInvalidTypeValues = new string[]
                {
                    A.Dummy<string>(),
                    string.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new string[] { },
                    new string[] { A.Dummy<string>(), string.Empty, null },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                    new List<string> { A.Dummy<string>() },
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new string[] { A.Dummy<string>() }, null, new string[] { } },
                },
            };

            validationTest1.Run(stringTestValues);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(enumerableTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IDictionary, IDictionary<TKey,Any Reference Type>, IDictionary<TKey,Nullable<T>>, IReadOnlyDictionary<TKey,Any Reference Type>, IReadOnlyDictionary<TKey,Nullable<T>>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IDictionary>, IEnumerable<IDictionary<TKey,Any Reference Type>>, IEnumerable<IDictionary<TKey,Nullable<T>>>, IEnumerable<IReadOnlyDictionary<TKey,Any Reference Type>>, IEnumerable<IReadOnlyDictionary<TKey,Nullable<T>>>",
            };

            var dictionaryTest2A = new TestValues<IDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2B = new TestValues<Dictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new Dictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2C = new TestValues<IReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new IReadOnlyDictionary<string, bool>[]
                {
                    new Dictionary<string, bool>(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Dictionary<string, bool>[] { },
                },
            };

            var dictionaryTest2D = new TestValues<ReadOnlyDictionary<string, bool>>
            {
                MustParameterInvalidTypeValues = new ReadOnlyDictionary<string, bool>[]
                {
                    new ReadOnlyDictionary<string, bool>(new Dictionary<string, bool>()),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new ReadOnlyDictionary<string, bool>[] { },
                },
            };

            validationTest2.Run(dictionaryTest2A);
            validationTest2.Run(dictionaryTest2B);
            validationTest2.Run(dictionaryTest2C);
            validationTest2.Run(dictionaryTest2D);

            var validationTest3 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var dictionaryTest3 = new TestValues<IDictionary>
            {
                MustFailingValues = new IDictionary[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, null, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
            };

            validationTest3.Run(dictionaryTest3);

            var validationTest4 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEmptyDictionaryExceptionMessageSuffix,
            };

            var dictionaryTest4A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary[] { },
                    new IDictionary[] { new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary(),
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } } },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>(),
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[] { new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>(),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest4E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[] { new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }), new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() } }) },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
            };

            validationTest4.Run(dictionaryTest4A);
            validationTest4.Run(dictionaryTest4B);
            validationTest4.Run(dictionaryTest4C);
            validationTest4.Run(dictionaryTest4D);
            validationTest4.Run(dictionaryTest4E);

            var validationTest5 = new ValidationTest
            {
                Validation = validation,
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotContainAnyKeyValuePairsWithNullValueExceptionMessageSuffix,
            };

            var dictionaryTest5A = new TestValues<IDictionary>
            {
                MustPassingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new ListDictionary[] { },
                    new ListDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IDictionary[]
                {
                    new ListDictionary() { { A.Dummy<string>(), null } },
                    new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary[]
                    {
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                        new ListDictionary() { { A.Dummy<string>(), null } },
                        new ListDictionary() { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest5B = new TestValues<IDictionary<string, string>>
            {
                MustPassingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IDictionary<string, string>[] { },
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest5C = new TestValues<Dictionary<string, string>>
            {
                MustPassingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new Dictionary<string, string>[] { },
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new Dictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new Dictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest5D = new TestValues<IReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                },
                MustEachPassingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[] { },
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
                MustFailingValues = new IReadOnlyDictionary<string, string>[]
                {
                    new Dictionary<string, string>() { { A.Dummy<string>(), null } },
                    new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } },
                },
                MustEachFailingValues = new[]
                {
                    new IReadOnlyDictionary<string, string>[]
                    {
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                        new Dictionary<string, string> { { A.Dummy<string>(), null } },
                        new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } },
                    },
                },
            };

            var dictionaryTest5E = new TestValues<ReadOnlyDictionary<string, string>>
            {
                MustPassingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } }),
                },
                MustEachPassingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[] { },
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
                MustFailingValues = new ReadOnlyDictionary<string, string>[]
                {
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), null } }),
                    new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() { { A.Dummy<string>(), A.Dummy<string>() }, { A.Dummy<string>(), null } }),
                },
                MustEachFailingValues = new[]
                {
                    new ReadOnlyDictionary<string, string>[]
                    {
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), null } }),
                        new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { A.Dummy<string>(), A.Dummy<string>() } }),
                    },
                },
            };

            validationTest5.Run(dictionaryTest5A);
            validationTest5.Run(dictionaryTest5B);
            validationTest5.Run(dictionaryTest5C);
            validationTest5.Run(dictionaryTest5D);
            validationTest5.Run(dictionaryTest5E);
        }

        [Fact]
        public static void BeDefault___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.BeDefault,
                ValidationName = nameof(Verifications.BeDefault),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeDefaultExceptionMessageSuffix,
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustPassingValues = new[]
                {
                    Guid.Empty,
                },
                MustEachPassingValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                },
                MustFailingValues = new[]
                {
                    Guid.NewGuid(),
                },
                MustEachFailingValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { null, null },
                },
                MustFailingValues = new Guid?[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { null, Guid.Empty, null },
                    new Guid?[] { null, Guid.NewGuid(), null },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    "  \r\n  ",
                    A.Dummy<string>(),
                },
                MustEachFailingValues = new IEnumerable<string>[]
                {
                    new string[] { null, string.Empty, null },
                },
            };

            var dateTimeTestValues = new TestValues<DateTime>
            {
                MustPassingValues = new[]
                {
                    DateTime.MinValue,
                },
                MustEachPassingValues = new IEnumerable<DateTime>[]
                {
                    new DateTime[] { },
                    new DateTime[] { DateTime.MinValue, DateTime.MinValue },
                },
                MustFailingValues = new[]
                {
                    DateTime.MaxValue,
                    DateTime.Now,
                },
                MustEachFailingValues = new IEnumerable<DateTime>[]
                {
                    new DateTime[] { DateTime.MinValue, DateTime.Now, DateTime.MinValue },
                    new DateTime[] { DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue },
                },
            };

            var decimalTestValues = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    0m,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { 0m, 0m },
                },
                MustFailingValues = new[]
                {
                    decimal.MaxValue,
                    decimal.MinValue,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { 0m, decimal.MaxValue, 0m },
                    new decimal[] { 0m, decimal.MinValue, 0m },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { null },
                },
                MustFailingValues = new object[]
                {
                    A.Dummy<object>(),
                },
                MustEachFailingValues = new IEnumerable<object>[]
                {
                    new object[] { null, A.Dummy<object>(), null },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(dateTimeTestValues);
            validationTest.Run(decimalTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void BeDefault___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int testParameter1 = 5;
            var expected1 = "Parameter 'testParameter1' is not equal to default(T) using EqualityComparer<T>.Default, where T: int.  Parameter value is '5'.";

            var testParameter2 = new[] { 0, 1, 0 };
            var expected2 = "Parameter 'testParameter2' contains an element that is not equal to default(T) using EqualityComparer<T>.Default, where T: int.  Element value is '1'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeDefault());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeDefault());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeDefault___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = Verifications.NotBeDefault,
                ValidationName = nameof(Verifications.NotBeDefault),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeDefaultExceptionMessageSuffix,
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustPassingValues = new[]
                {
                    Guid.NewGuid(),
                },
                MustEachPassingValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
                },
                MustFailingValues = new[]
                {
                    Guid.Empty,
                },
                MustEachFailingValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { Guid.NewGuid(), Guid.Empty, Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachPassingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.NewGuid() },
                },
                MustFailingValues = new Guid?[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { Guid.Empty, null, Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.Empty },
                },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "  \r\n  ",
                    A.Dummy<string>(),
                },
                MustEachPassingValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { string.Empty, "  \r\n ", A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<string>[]
                {
                    new string[] { string.Empty, null, string.Empty },
                    new string[] { A.Dummy<string>(), null, A.Dummy<string>() },
                },
            };

            var dateTimeTestValues = new TestValues<DateTime>
            {
                MustPassingValues = new[]
                {
                    DateTime.MaxValue,
                    DateTime.Now,
                },
                MustEachPassingValues = new IEnumerable<DateTime>[]
                {
                    new DateTime[] { },
                    new DateTime[] { DateTime.Now, DateTime.MaxValue },
                },
                MustFailingValues = new[]
                {
                    DateTime.MinValue,
                },
                MustEachFailingValues = new IEnumerable<DateTime>[]
                {
                    new DateTime[] { DateTime.Now, DateTime.MinValue, DateTime.Now },
                    new DateTime[] { DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue },
                },
            };

            var decimalTestValues = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    decimal.MaxValue,
                    decimal.MinValue,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { decimal.MaxValue, decimal.MinValue },
                },
                MustFailingValues = new[]
                {
                    0m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { decimal.MinValue, 0m, decimal.MinValue },
                    new decimal[] { decimal.MaxValue, 0m, decimal.MaxValue },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[]
                {
                    A.Dummy<object>(),
                },
                MustEachPassingValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                },
                MustFailingValues = new object[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<object>[]
                {
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(dateTimeTestValues);
            validationTest.Run(decimalTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void NotBeDefault___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            var expected1 = "Parameter 'testParameter1' is equal to default(T) using EqualityComparer<T>.Default, where T: int?.";

            var testParameter2 = new[] { 1, 0, 1 };
            var expected2 = "Parameter 'testParameter2' contains an element that is equal to default(T) using EqualityComparer<T>.Default, where T: int.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeDefault());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeDefault());

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void BeLessThan___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.BeLessThan(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.BeLessThan);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    null,
                    comparisonValue2 - .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, comparisonValue2 - .0000001m },
                },
                MustFailingValues = new[]
                {
                    comparisonValue2,
                    comparisonValue2 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2 - .0000001m, comparisonValue2, comparisonValue2 - .0000001m },
                    new decimal?[] { comparisonValue2 - .0000001m, comparisonValue2 + .00000001m, comparisonValue2 - .0000001m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5 - .0000001m,
                    comparisonValue5 - Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5 - Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5 },
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5 + .00000001m, comparisonValue5 - .0000001m },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                    A.Dummy<decimal>(),
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null },
                    new decimal?[] { A.Dummy<decimal>() },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void BeLessThan___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = null;
            var expected1 = "Parameter 'testParameter1' is not less than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            int? testParameter2 = 10;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is not less than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '10'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 10;
            int comparisonValue3 = 5;
            var expected3 = "Parameter 'testParameter3' is not less than the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '10'.  Specified 'comparisonValue' is '5'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = null;
            var expected4 = "Parameter 'testParameter4' contains an element that is not less than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter5 = new int?[] { 10 };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is not less than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '10'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 10 };
            int comparisonValue6 = 5;
            var expected6 = "Parameter 'testParameter6' contains an element that is not less than the comparison value using Comparer<T>.Default, where T: int.  Element value is '10'.  Specified 'comparisonValue' is '5'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeLessThan(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeLessThan(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeLessThan(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeLessThan(comparisonValue4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().BeLessThan(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeLessThan(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeLessThan___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeLessThan(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotBeLessThan);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    comparisonValue2,
                    comparisonValue2 + .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { comparisonValue2, comparisonValue2 + .0000001m },
                },
                MustFailingValues = new[]
                {
                    null,
                    comparisonValue2 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2, null, comparisonValue2 },
                    new decimal?[] { comparisonValue2, comparisonValue2 - .0000001m, comparisonValue2 },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 + .0000001m,
                    comparisonValue5 + Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5, comparisonValue5 },
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5 + Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5, comparisonValue5 - .0000001m, comparisonValue5 },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                    A.Dummy<decimal>(),
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, A.Dummy<decimal>() },
                },
                MustFailingValues = new decimal?[]
                {
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void NotBeLessThan___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = 10;
            var expected1 = "Parameter 'testParameter1' is less than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '10'.";

            int testParameter3 = 10;
            int comparisonValue3 = 20;
            var expected3 = "Parameter 'testParameter3' is less than the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '10'.  Specified 'comparisonValue' is '20'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is less than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '10'.";

            var testParameter6 = new int[] { 10 };
            int comparisonValue6 = 20;
            var expected6 = "Parameter 'testParameter6' contains an element that is less than the comparison value using Comparer<T>.Default, where T: int.  Element value is '10'.  Specified 'comparisonValue' is '20'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeLessThan(comparisonValue1));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().NotBeLessThan(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().NotBeLessThan(comparisonValue4));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().NotBeLessThan(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void BeGreaterThan___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.BeGreaterThan(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.BeGreaterThan);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            decimal? comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    comparisonValue2 + .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { comparisonValue2 + .0000001m },
                },
                MustFailingValues = new[]
                {
                    null,
                    comparisonValue2,
                    comparisonValue2 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2 + .0000001m, null, comparisonValue2 + .0000001m },
                    new decimal?[] { comparisonValue2 + .0000001m, comparisonValue2, comparisonValue2 + .0000001m },
                    new decimal?[] { comparisonValue2 + .0000001m, comparisonValue2 - .00000001m, comparisonValue2 + .0000001m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5 + .0000001m,
                    comparisonValue5 + Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5 + Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5 },
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5 - .00000001m, comparisonValue5 + .0000001m },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    A.Dummy<decimal>(),
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { A.Dummy<decimal>() },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { A.Dummy<decimal>(), null, A.Dummy<decimal>() },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void BeGreaterThan___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = 10;
            var expected1 = "Parameter 'testParameter1' is not greater than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '10'.";

            int? testParameter2 = null;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is not greater than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 5;
            int comparisonValue3 = 10;
            var expected3 = "Parameter 'testParameter3' is not greater than the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '5'.  Specified 'comparisonValue' is '10'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is not greater than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '10'.";

            var testParameter5 = new int?[] { null };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is not greater than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 5 };
            int comparisonValue6 = 10;
            var expected6 = "Parameter 'testParameter6' contains an element that is not greater than the comparison value using Comparer<T>.Default, where T: int.  Element value is '5'.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeGreaterThan(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeGreaterThan(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeGreaterThan(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeGreaterThan(comparisonValue4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().BeGreaterThan(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeGreaterThan(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeGreaterThan___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeGreaterThan(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotBeGreaterThan);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    null,
                    comparisonValue2,
                    comparisonValue2 - .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, comparisonValue2, comparisonValue2 - .0000001m },
                },
                MustFailingValues = new[]
                {
                    comparisonValue2 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2, comparisonValue2 + .0000001m, comparisonValue2 },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 - .0000001m,
                    comparisonValue5 - Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5, comparisonValue5 },
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5 - Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5, comparisonValue5 + .0000001m, comparisonValue5 },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null },
                },
                MustFailingValues = new decimal?[]
                {
                    A.Dummy<decimal>(),
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null, A.Dummy<decimal>(), null },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void NotBeGreaterThan___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter2 = 10;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is greater than the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '10'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 10;
            int comparisonValue3 = 5;
            var expected3 = "Parameter 'testParameter3' is greater than the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '10'.  Specified 'comparisonValue' is '5'.";

            var testParameter5 = new int?[] { 10 };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is greater than the comparison value using Comparer<T>.Default, where T: int?.  Element value is '10'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 10 };
            int comparisonValue6 = 5;
            var expected6 = "Parameter 'testParameter6' contains an element that is greater than the comparison value using Comparer<T>.Default, where T: int.  Element value is '10'.  Specified 'comparisonValue' is '5'.";

            // Act
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().NotBeGreaterThan(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().NotBeGreaterThan(comparisonValue3));

            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().NotBeGreaterThan(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().NotBeGreaterThan(comparisonValue6));

            // Assert
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void BeLessThanOrEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.BeLessThanOrEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.BeLessThanOrEqualTo);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    null,
                    comparisonValue2,
                    comparisonValue2 - .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, comparisonValue2, comparisonValue2 - .0000001m },
                },
                MustFailingValues = new[]
                {
                    comparisonValue2 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2, comparisonValue2 + .00000001m, comparisonValue2 },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanOrEqualToExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 - .0000001m,
                    comparisonValue5 - Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5, comparisonValue5 - .0000001m, comparisonValue5 - Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5 + .0000001m,
                    comparisonValue5 + Math.Abs(comparisonValue5),
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5, comparisonValue5 + .00000001m, comparisonValue5 },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeLessThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null },
                },
                MustFailingValues = new decimal?[]
                {
                    A.Dummy<decimal>(),
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null, A.Dummy<decimal>(), null },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void BeLessThanOrEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter2 = 10;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is not less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '10'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 20;
            int comparisonValue3 = 10;
            var expected3 = "Parameter 'testParameter3' is not less than or equal to the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '20'.  Specified 'comparisonValue' is '10'.";

            var testParameter5 = new int?[] { 10 };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is not less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '10'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 20 };
            int comparisonValue6 = 10;
            var expected6 = "Parameter 'testParameter6' contains an element that is not less than or equal to the comparison value using Comparer<T>.Default, where T: int.  Element value is '20'.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeLessThanOrEqualTo(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeLessThanOrEqualTo(comparisonValue3));

            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().BeLessThanOrEqualTo(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeLessThanOrEqualTo(comparisonValue6));

            // Assert
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeLessThanOrEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeLessThanOrEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotBeLessThanOrEqualTo);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    comparisonValue2 + .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { comparisonValue2 + .0000001m },
                },
                MustFailingValues = new[]
                {
                    null,
                    comparisonValue2,
                    comparisonValue2 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2 + .0000001m, null, comparisonValue2 + .0000001m },
                    new decimal?[] { comparisonValue2 + .0000001m, comparisonValue2, comparisonValue2 + .0000001m },
                    new decimal?[] { comparisonValue2 + .0000001m, comparisonValue2 - .0000001m, comparisonValue2 + .0000001m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanOrEqualToExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5 + .0000001m,
                    comparisonValue5 + Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5 + Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5, comparisonValue5 + .0000001m },
                    new decimal[] { comparisonValue5 + .0000001m, comparisonValue5 - .0000001m, comparisonValue5 + .0000001m },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeLessThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    A.Dummy<decimal>(),
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { A.Dummy<decimal>() },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { A.Dummy<decimal>(), null,  A.Dummy<decimal>() },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void NotBeLessThanOrEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = 10;
            var expected1 = "Parameter 'testParameter1' is less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '10'.";

            int? testParameter2 = null;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 5;
            int comparisonValue3 = 10;
            var expected3 = "Parameter 'testParameter3' is less than or equal to the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '5'.  Specified 'comparisonValue' is '10'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '10'.";

            var testParameter5 = new int?[] { null };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is less than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 5 };
            int comparisonValue6 = 10;
            var expected6 = "Parameter 'testParameter6' contains an element that is less than or equal to the comparison value using Comparer<T>.Default, where T: int.  Element value is '5'.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeLessThanOrEqualTo(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().NotBeLessThanOrEqualTo(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().NotBeLessThanOrEqualTo(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().NotBeLessThanOrEqualTo(comparisonValue4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().NotBeLessThanOrEqualTo(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().NotBeLessThanOrEqualTo(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void BeGreaterThanOrEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.BeGreaterThanOrEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.BeGreaterThanOrEqualTo);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    comparisonValue2,
                    comparisonValue2 + .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { comparisonValue2, comparisonValue2 + .0000001m },
                },
                MustFailingValues = new[]
                {
                    null,
                    comparisonValue2 - .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2, null, comparisonValue2 },
                    new decimal?[] { comparisonValue2, comparisonValue2 - .00000001m, comparisonValue2 },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 + .0000001m,
                    comparisonValue5 + Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5, comparisonValue5 + .0000001m, comparisonValue5 + Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5 - .0000001m,
                    comparisonValue5 - Math.Abs(comparisonValue5),
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5, comparisonValue5 - .00000001m, comparisonValue5 },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                    A.Dummy<decimal>(),
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, A.Dummy<decimal>() },
                },
                MustFailingValues = new decimal?[]
                {
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void BeGreaterThanOrEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = 10;
            var expected1 = "Parameter 'testParameter1' is not greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '10'.";

            int testParameter3 = 5;
            int comparisonValue3 = 10;
            var expected3 = "Parameter 'testParameter3' is not greater than or equal to the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '5'.  Specified 'comparisonValue' is '10'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is not greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '10'.";

            var testParameter6 = new int[] { 5 };
            int comparisonValue6 = 10;
            var expected6 = "Parameter 'testParameter6' contains an element that is not greater than or equal to the comparison value using Comparer<T>.Default, where T: int.  Element value is '5'.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeGreaterThanOrEqualTo(comparisonValue1));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeGreaterThanOrEqualTo(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeGreaterThanOrEqualTo(comparisonValue4));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeGreaterThanOrEqualTo(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeGreaterThanOrEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeGreaterThanOrEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotBeGreaterThanOrEqualTo);

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            var comparisonValue2 = A.Dummy<decimal?>();
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new[]
                {
                    null,
                    comparisonValue2 - .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, comparisonValue2 - .0000001m },
                },
                MustFailingValues = new[]
                {
                    comparisonValue2,
                    comparisonValue2 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { comparisonValue2 - .0000001m, comparisonValue2, comparisonValue2 - .0000001m },
                    new decimal?[] { comparisonValue2 - .0000001m, comparisonValue2 + .0000001m, comparisonValue2 - .0000001m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var comparisonValue5 = A.Dummy<decimal>();
            var validationTest5 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue5),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var decimalTestValues5 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue5 - .0000001m,
                    comparisonValue5 - Math.Abs(comparisonValue5),
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5 - Math.Abs(comparisonValue5) },
                },
                MustFailingValues = new[]
                {
                    comparisonValue5,
                    comparisonValue5 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5, comparisonValue5 - .0000001m },
                    new decimal[] { comparisonValue5 - .0000001m, comparisonValue5 + .0000001m, comparisonValue5 - .0000001m },
                },
            };

            validationTest5.Run(decimalTestValues5);

            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation((decimal?)null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeGreaterThanOrEqualToExceptionMessageSuffix,
            };

            var nullableDecimalTestValues6 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                    A.Dummy<decimal>(),
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null },
                    new decimal?[] { A.Dummy<decimal>() },
                },
            };

            validationTest6.Run(nullableDecimalTestValues6);
        }

        [Fact]
        public static void NotBeGreaterThanOrEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = null;
            var expected1 = "Parameter 'testParameter1' is greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            int? testParameter2 = 10;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Parameter value is '10'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 20;
            int comparisonValue3 = 10;
            var expected3 = "Parameter 'testParameter3' is greater than or equal to the comparison value using Comparer<T>.Default, where T: int.  Parameter value is '20'.  Specified 'comparisonValue' is '10'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = null;
            var expected4 = "Parameter 'testParameter4' contains an element that is greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter5 = new int?[] { 10 };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is greater than or equal to the comparison value using Comparer<T>.Default, where T: int?.  Element value is '10'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 20 };
            int comparisonValue6 = 10;
            var expected6 = "Parameter 'testParameter6' contains an element that is greater than or equal to the comparison value using Comparer<T>.Default, where T: int.  Element value is '20'.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeGreaterThanOrEqualTo(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().NotBeGreaterThanOrEqualTo(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().NotBeGreaterThanOrEqualTo(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().NotBeGreaterThanOrEqualTo(comparisonValue4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().NotBeGreaterThanOrEqualTo(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().NotBeGreaterThanOrEqualTo(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void BeEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.BeEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.BeEqualTo);

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest1.Run(stringTestValues1);

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues2 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest2.Run(decimalTestValues2);

            var comparisonValue3 = A.Dummy<decimal>();
            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue3),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeEqualToExceptionMessageSuffix,
            };

            var decimalTestValues3 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue3,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue3, comparisonValue3 },
                },
                MustFailingValues = new[]
                {
                    comparisonValue3 - .0000001m,
                    comparisonValue3 + .0000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue3, comparisonValue3 - .0000001m, comparisonValue3 },
                    new decimal[] { comparisonValue3, comparisonValue3 + .0000001m, comparisonValue3 },
                },
            };

            validationTest3.Run(decimalTestValues3);
        }

        [Fact]
        public static void BeEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = 10;
            var expected1 = "Parameter 'testParameter1' is not equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'comparisonValue' is '10'.";

            int? testParameter2 = 10;
            int? comparisonValue2 = null;
            var expected2 = "Parameter 'testParameter2' is not equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Parameter value is '10'.  Specified 'comparisonValue' is '<null>'.";

            int testParameter3 = 10;
            int comparisonValue3 = 20;
            var expected3 = "Parameter 'testParameter3' is not equal to the comparison value using EqualityComparer<T>.Default, where T: int.  Parameter value is '10'.  Specified 'comparisonValue' is '20'.";

            var testParameter4 = new int?[] { null };
            int? comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is not equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'comparisonValue' is '10'.";

            var testParameter5 = new int?[] { 10 };
            int? comparisonValue5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is not equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Element value is '10'.  Specified 'comparisonValue' is '<null>'.";

            var testParameter6 = new int[] { 10 };
            int comparisonValue6 = 20;
            var expected6 = "Parameter 'testParameter6' contains an element that is not equal to the comparison value using EqualityComparer<T>.Default, where T: int.  Element value is '10'.  Specified 'comparisonValue' is '20'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeEqualTo(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeEqualTo(comparisonValue2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeEqualTo(comparisonValue3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeEqualTo(comparisonValue4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().BeEqualTo(comparisonValue5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeEqualTo(comparisonValue6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeEqualTo___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T comparisonValue)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeEqualTo(comparisonValue, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotBeEqualTo);

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest1.Run(stringTestValues1);

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "comparisonValue",
            };

            var decimalTestValues2 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest2.Run(decimalTestValues2);

            var comparisonValue3 = A.Dummy<decimal>();
            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue3),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeEqualToExceptionMessageSuffix,
            };

            var decimalTestValues3 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue3 - .0000001m,
                    comparisonValue3 + .0000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue3 - .0000001m, comparisonValue3 + .0000001m },
                },
                MustFailingValues = new[]
                {
                    comparisonValue3,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue3 - .0000001m, comparisonValue3, comparisonValue3 + .0000001m },
                },
            };

            validationTest3.Run(decimalTestValues3);
        }

        [Fact]
        public static void NotBeEqualTo___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? comparisonValue1 = null;
            var expected1 = "Parameter 'testParameter1' is equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Specified 'comparisonValue' is '<null>'.";

            int testParameter2 = 10;
            int comparisonValue2 = 10;
            var expected2 = "Parameter 'testParameter2' is equal to the comparison value using EqualityComparer<T>.Default, where T: int.  Specified 'comparisonValue' is '10'.";

            var testParameter3 = new int?[] { null };
            int? comparisonValue3 = null;
            var expected3 = "Parameter 'testParameter3' contains an element that is equal to the comparison value using EqualityComparer<T>.Default, where T: int?.  Specified 'comparisonValue' is '<null>'.";

            var testParameter4 = new int[] { 10 };
            int comparisonValue4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that is equal to the comparison value using EqualityComparer<T>.Default, where T: int.  Specified 'comparisonValue' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeEqualTo(comparisonValue1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().NotBeEqualTo(comparisonValue2));

            var actual3 = Record.Exception(() => new { testParameter3 }.Must().Each().NotBeEqualTo(comparisonValue3));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().NotBeEqualTo(comparisonValue4));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);

            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void BeInRange___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T minimum, T maximum)
            {
                return (parameter, because, applyBecause, data) => parameter.BeInRange(minimum, maximum, because: because, applyBecause: applyBecause, data: data);
            }

            var validationName = nameof(Verifications.BeInRange);

            var ex1 = Record.Exception(() => A.Dummy<object>().Must().BeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.IncludesMinimumAndExcludesMaximum));
            var ex2 = Record.Exception(() => A.Dummy<object>().Must().BeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.ExcludesMinimumAndIncludesMaximum));
            var ex3 = Record.Exception(() => A.Dummy<object>().Must().BeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.ExcludesMinimumAndMaximum));
            ex1.Should().BeOfType<NotImplementedException>();
            ex2.Should().BeOfType<NotImplementedException>();
            ex3.Should().BeOfType<NotImplementedException>();

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>(), A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            decimal? minimum2 = 10m;
            decimal? maximum2 = 20m;
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(minimum2, maximum2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    10m,
                    16m,
                    20m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { 10m, 16m, 20m },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                    decimal.MinValue,
                    decimal.MaxValue,
                    9.999999999m,
                    20.000000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { 16m, null, 16m },
                    new decimal?[] { 16m, decimal.MinValue, 16m },
                    new decimal?[] { 16m, decimal.MaxValue, 16m },
                    new decimal?[] { 16m, 9.999999999m, 16m },
                    new decimal?[] { 16m, 20.000000001m, 16m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>(), A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "minimum",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>(), A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "minimum",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var minimum5 = 5m;
            var maximum5 = 4.5m;
            var validationTest5Actual = Record.Exception(() => A.Dummy<decimal>().Must().BeInRange(minimum5, maximum5, because: A.Dummy<string>()));
            validationTest5Actual.Should().BeOfType<ImproperUseOfAssertionFrameworkException>();
            validationTest5Actual.Message.Should().Be("The specified range is invalid because 'maximum' is less than 'minimum'.  Specified 'minimum' is '5'.  Specified 'maximum' is '4.5'.  " + Verifications.ImproperUseOfFrameworkExceptionMessage);

            var minimum6 = 10m;
            var maximum6 = 20m;
            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation(minimum6, maximum6),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeInRangeExceptionMessageSuffix,
            };

            var decimalTestValues6 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    10m,
                    16m,
                    20m,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { 10m, 16m, 20m },
                },
                MustFailingValues = new[]
                {
                    decimal.MinValue,
                    decimal.MaxValue,
                    9.999999999m,
                    20.000000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { 16m, decimal.MinValue, 16m },
                    new decimal[] { 16m, decimal.MaxValue, 16m },
                    new decimal[] { 16m, 9.999999999m, 16m },
                    new decimal[] { 16m, 20.000000001m, 16m },
                },
            };

            validationTest6.Run(decimalTestValues6);

            var comparisonValue7 = A.Dummy<decimal>();
            var validationTest7 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue7, comparisonValue7),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeInRangeExceptionMessageSuffix,
            };

            var decimalTestValues7 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue7,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue7, comparisonValue7 },
                },
                MustFailingValues = new decimal[]
                {
                    comparisonValue7 + .000000001m,
                    comparisonValue7 - .000000001m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue7, comparisonValue7 + .000000001m, comparisonValue7 },
                    new decimal[] { comparisonValue7, comparisonValue7 - .000000001m, comparisonValue7 },
                },
            };

            validationTest7.Run(decimalTestValues7);

            var validationTest8Actual = Record.Exception(() => A.Dummy<decimal?>().Must().BeInRange(10m, (decimal?)null, because: A.Dummy<string>()));
            validationTest8Actual.Should().BeOfType<ImproperUseOfAssertionFrameworkException>();
            validationTest8Actual.Message.Should().Be("The specified range is invalid because 'maximum' is less than 'minimum'.  Specified 'minimum' is '10'.  Specified 'maximum' is '<null>'.  " + Verifications.ImproperUseOfFrameworkExceptionMessage);

            decimal? maximum9 = 20m;
            var validationTest9 = new ValidationTest
            {
                Validation = GetValidation(null, maximum9),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues9 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                    decimal.MinValue,
                    20m,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, decimal.MinValue, 20m },
                },
                MustFailingValues = new decimal?[]
                {
                    20.000000001m,
                    decimal.MaxValue,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null, 20.000000001m, null },
                    new decimal?[] { null, decimal.MaxValue, null },
                },
            };

            validationTest9.Run(nullableDecimalTestValues9);

            var validationTest10 = new ValidationTest
            {
                Validation = GetValidation<decimal?>(null, null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues10 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null },
                },
                MustFailingValues = new decimal?[]
                {
                    decimal.MinValue,
                    decimal.MaxValue,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { null, decimal.MinValue, null },
                    new decimal?[] { null, decimal.MaxValue, null },
                },
            };

            validationTest10.Run(nullableDecimalTestValues10);
        }

        [Fact]
        public static void BeInRange___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            int? testParameter1 = null;
            int? minimum1 = 10;
            int? maximum1 = 20;
            var expected1 = "Parameter 'testParameter1' is not within the specified range using Comparer<T>.Default, where T: int?.  Parameter value is '<null>'.  Specified 'minimum' is '10'.  Specified 'maximum' is '20'.";

            int? testParameter2 = 5;
            int? minimum2 = null;
            int? maximum2 = null;
            var expected2 = "Parameter 'testParameter2' is not within the specified range using Comparer<T>.Default, where T: int?.  Parameter value is '5'.  Specified 'minimum' is '<null>'.  Specified 'maximum' is '<null>'.";

            int testParameter3 = 5;
            int minimum3 = 10;
            int maximum3 = 20;
            var expected3 = "Parameter 'testParameter3' is not within the specified range using Comparer<T>.Default, where T: int.  Parameter value is '5'.  Specified 'minimum' is '10'.  Specified 'maximum' is '20'.";

            var testParameter4 = new int?[] { null };
            int? minimum4 = 10;
            int? maximum4 = 20;
            var expected4 = "Parameter 'testParameter4' contains an element that is not within the specified range using Comparer<T>.Default, where T: int?.  Element value is '<null>'.  Specified 'minimum' is '10'.  Specified 'maximum' is '20'.";

            var testParameter5 = new int?[] { 5 };
            int? minimum5 = null;
            int? maximum5 = null;
            var expected5 = "Parameter 'testParameter5' contains an element that is not within the specified range using Comparer<T>.Default, where T: int?.  Element value is '5'.  Specified 'minimum' is '<null>'.  Specified 'maximum' is '<null>'.";

            var testParameter6 = new int[] { 5 };
            int minimum6 = 10;
            int maximum6 = 20;
            var expected6 = "Parameter 'testParameter6' contains an element that is not within the specified range using Comparer<T>.Default, where T: int.  Element value is '5'.  Specified 'minimum' is '10'.  Specified 'maximum' is '20'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeInRange(minimum1, maximum1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeInRange(minimum2, maximum2));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeInRange(minimum3, maximum3));

            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeInRange(minimum4, maximum4));
            var actual5 = Record.Exception(() => new { testParameter5 }.Must().Each().BeInRange(minimum5, maximum5));
            var actual6 = Record.Exception(() => new { testParameter6 }.Must().Each().BeInRange(minimum6, maximum6));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);

            actual4.Message.Should().Be(expected4);
            actual5.Message.Should().Be(expected5);
            actual6.Message.Should().Be(expected6);
        }

        [Fact]
        public static void NotBeInRange___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T minimum, T maximum)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeInRange(minimum, maximum, because: because, applyBecause: applyBecause, data: data);
            }

            var validationName = nameof(Verifications.NotBeInRange);

            var ex1 = Record.Exception(() => A.Dummy<object>().Must().NotBeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.IncludesMinimumAndExcludesMaximum));
            var ex2 = Record.Exception(() => A.Dummy<object>().Must().NotBeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.ExcludesMinimumAndIncludesMaximum));
            var ex3 = Record.Exception(() => A.Dummy<object>().Must().NotBeInRange(A.Dummy<object>(), A.Dummy<object>(), Range.ExcludesMinimumAndMaximum));
            ex1.Should().BeOfType<NotImplementedException>();
            ex2.Should().BeOfType<NotImplementedException>();
            ex3.Should().BeOfType<NotImplementedException>();

            // here the comparisonValue type doesn't match the parameter type, but
            // that shouldn't matter because it first fails on TestClass not being comparable
            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>(), A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IComparable, IComparable<T>, Nullable<T>",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IComparable>, IEnumerable<IComparable<T>>, IEnumerable<Nullable<T>>",
            };

            var customClassTestValues1 = new TestValues<TestClass>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    null,
                    new TestClass(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<TestClass>[]
                {
                    new TestClass[] { },
                    new TestClass[] { null },
                    new TestClass[] { new TestClass() },
                },
            };

            validationTest1.Run(customClassTestValues1);

            decimal? minimum2 = 10m;
            decimal? maximum2 = 20m;
            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(minimum2, maximum2),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues2 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    null,
                    9.9999999999m,
                    20.00000001m,
                    decimal.MinValue,
                    decimal.MaxValue,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { null, decimal.MinValue, 9.9999999999m, 20.00000001m, decimal.MaxValue },
                },
                MustFailingValues = new decimal?[]
                {
                    10m,
                    15m,
                    20m,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { 9.9999999999m, 10m, 20.00000001m },
                    new decimal?[] { 9.9999999999m, 15m, 20.00000001m },
                    new decimal?[] { 9.9999999999m, 20m, 20.00000001m },
                },
            };

            validationTest2.Run(nullableDecimalTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>(), A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "minimum",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    null,
                    string.Empty,
                    A.Dummy<string>(),
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new string[] { },
                    new string[] { null },
                    new string[] { A.Dummy<string>() },
                },
            };

            validationTest3.Run(stringTestValues3);

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<int>(), A.Dummy<int>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "decimal",
                ValidationParameterInvalidCastParameterName = "minimum",
            };

            var decimalTestValues4 = new TestValues<decimal>
            {
                MustValidationParameterInvalidTypeValues = new[]
                {
                    A.Dummy<decimal>(),
                    decimal.MaxValue,
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { A.Dummy<decimal>() },
                },
            };

            validationTest4.Run(decimalTestValues4);

            var minimum5 = 5m;
            var maximum5 = 4.5m;
            var validationTest5Actual = Record.Exception(() => A.Dummy<decimal>().Must().NotBeInRange(minimum5, maximum5, because: A.Dummy<string>()));
            validationTest5Actual.Should().BeOfType<ImproperUseOfAssertionFrameworkException>();
            validationTest5Actual.Message.Should().Be("The specified range is invalid because 'maximum' is less than 'minimum'.  Specified 'minimum' is '5'.  Specified 'maximum' is '4.5'.  " + Verifications.ImproperUseOfFrameworkExceptionMessage);

            var minimum6 = 10m;
            var maximum6 = 20m;
            var validationTest6 = new ValidationTest
            {
                Validation = GetValidation(minimum6, maximum6),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeInRangeExceptionMessageSuffix,
            };

            var decimalTestValues6 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    9.9999999999m,
                    20.00000001m,
                    decimal.MinValue,
                    decimal.MaxValue,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { decimal.MinValue, 9.9999999999m, 20.00000001m, decimal.MaxValue },
                },
                MustFailingValues = new[]
                {
                    10m,
                    15m,
                    20m,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { 9.9999999999m, 10m, 20.00000001m },
                    new decimal[] { 9.9999999999m, 15m, 20.00000001m },
                    new decimal[] { 9.9999999999m, 20m, 20.00000001m },
                },
            };

            validationTest6.Run(decimalTestValues6);

            var comparisonValue7 = A.Dummy<decimal>();
            var validationTest7 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue7, comparisonValue7),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeInRangeExceptionMessageSuffix,
            };

            var decimalTestValues7 = new TestValues<decimal>
            {
                MustPassingValues = new[]
                {
                    comparisonValue7 - .000000001m,
                    comparisonValue7 + .000000001m,
                },
                MustEachPassingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new decimal[] { comparisonValue7 - .000000001m, comparisonValue7 + .000000001m },
                },
                MustFailingValues = new decimal[]
                {
                    comparisonValue7,
                },
                MustEachFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { comparisonValue7 - .000000001m, comparisonValue7, comparisonValue7 + .000000001m },
                },
            };

            validationTest7.Run(decimalTestValues7);

            var validationTest8Actual = Record.Exception(() => A.Dummy<decimal?>().Must().NotBeInRange(10m, (decimal?)null, because: A.Dummy<string>()));
            validationTest8Actual.Should().BeOfType<ImproperUseOfAssertionFrameworkException>();
            validationTest8Actual.Message.Should().Be("The specified range is invalid because 'maximum' is less than 'minimum'.  Specified 'minimum' is '10'.  Specified 'maximum' is '<null>'.  " + Verifications.ImproperUseOfFrameworkExceptionMessage);

            decimal? maximum9 = 20m;
            var validationTest9 = new ValidationTest
            {
                Validation = GetValidation(null, maximum9),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues9 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    20.00000000001m,
                    decimal.MaxValue,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { },
                    new decimal?[] { 20.00000000001m, decimal.MaxValue },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                    20m,
                    decimal.MinValue,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { decimal.MaxValue, null, decimal.MaxValue },
                    new decimal?[] { decimal.MaxValue, 20m, decimal.MaxValue },
                    new decimal?[] { decimal.MaxValue, decimal.MinValue, decimal.MaxValue },
                },
            };

            validationTest9.Run(nullableDecimalTestValues9);

            var validationTest10 = new ValidationTest
            {
                Validation = GetValidation<decimal?>(null, null),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentOutOfRangeException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeInRangeExceptionMessageSuffix,
            };

            var nullableDecimalTestValues10 = new TestValues<decimal?>
            {
                MustPassingValues = new decimal?[]
                {
                    decimal.MinValue,
                    decimal.MaxValue,
                },
                MustEachPassingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { decimal.MinValue },
                    new decimal?[] { decimal.MaxValue },
                },
                MustFailingValues = new decimal?[]
                {
                    null,
                },
                MustEachFailingValues = new IEnumerable<decimal?>[]
                {
                    new decimal?[] { decimal.MinValue, null, decimal.MinValue },
                },
            };

            validationTest10.Run(nullableDecimalTestValues10);
        }

        [Fact]
        public static void Contain___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T item)
            {
                return (parameter, because, applyBecause, data) => parameter.Contain(item, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.Contain);

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "itemToSearchFor",
            };

            var stringTestValues2 = new TestValues<IEnumerable<string>>
            {
                MustValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                    new List<string> { A.Dummy<string>(), null },
                    new string[] { A.Dummy<string>(), null },
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<IEnumerable<string>>[]
                {
                    new IEnumerable<string>[] { },
                    new IEnumerable<string>[] { new List<string> { null }, new string[] { A.Dummy<string>(), null } },
                    new IEnumerable<string>[] { new List<string> { A.Dummy<string>(), null } },
                    new IEnumerable<string>[] { new string[] { null }, new List<string> { A.Dummy<string>(), null }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            validationTest2.Run(stringTestValues2);

            var comparisonValue3 = A.Dummy<string>();
            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue3),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var enumerableTestValues3 = new TestValues<IEnumerable<string>>
            {
                MustFailingValues = new IEnumerable<string>[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable<string>[] { new string[] { A.Dummy<string>(), null, comparisonValue3 }, null, new string[] { A.Dummy<string>(), null, comparisonValue3 } },
                },
            };

            validationTest3.Run(enumerableTestValues3);

            var comparisonValue4 = 10m;
            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue4),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.ContainExceptionMessageSuffix,
            };

            var decimalTestValues4 = new TestValues<IEnumerable<decimal>>
            {
                MustPassingValues = new[]
                {
                    new[] { comparisonValue4 },
                    new[] { 5m, comparisonValue4, 15m },
                },
                MustEachPassingValues = new IEnumerable<IEnumerable<decimal>>[]
                {
                    new IEnumerable<decimal>[] { },
                    new[] { new[] { comparisonValue4 }, new[] { 5m, comparisonValue4, 15m } },
                },
                MustFailingValues = new IEnumerable<decimal>[]
                {
                    new decimal[] { },
                    new[] { 5m, 9.9999999m, 10.000001m, 15m },
                },
                MustEachFailingValues = new IEnumerable<IEnumerable<decimal>>[]
                {
                    new[] { new[] { comparisonValue4 }, new[] { 5m, 9.9999999m, 10.000001m, 15m }, new[] { comparisonValue4 } },
                },
            };

            validationTest4.Run(decimalTestValues4);
        }

        [Fact]
        public static void Contain___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new int?[] { 1, 2, 3 };
            int? itemToSearchFor1 = null;
            var expected1 = "Parameter 'testParameter1' does not contain the item to search for using EqualityComparer<T>.Default, where T: int?.  Specified 'itemToSearchFor' is '<null>'.";

            var testParameter2 = new int[] { 1, 2, 3 };
            int itemToSearchFor2 = 10;
            var expected2 = "Parameter 'testParameter2' does not contain the item to search for using EqualityComparer<T>.Default, where T: int.  Specified 'itemToSearchFor' is '10'.";

            var testParameter3 = new int?[][] { new int?[] { 1, 2, 3 } };
            int? itemToSearchFor3 = null;
            var expected3 = "Parameter 'testParameter3' contains an element that does not contain the item to search for using EqualityComparer<T>.Default, where T: int?.  Specified 'itemToSearchFor' is '<null>'.";

            var testParameter4 = new int[][] { new int[] { 1, 2, 3 } };
            int itemToSearchFor4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that does not contain the item to search for using EqualityComparer<T>.Default, where T: int.  Specified 'itemToSearchFor' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().Contain(itemToSearchFor1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Contain(itemToSearchFor2));

            var actual3 = Record.Exception(() => new { testParameter3 }.Must().Each().Contain(itemToSearchFor3));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().Contain(itemToSearchFor4));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);

            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void NotContain___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange, Act, Assert
            Validation GetValidation<T>(T item)
            {
                return (parameter, because, applyBecause, data) => parameter.NotContain(item, because, applyBecause, data);
            }

            var validationName = nameof(Verifications.NotContain);

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<object>()),
                ValidationName = validationName,
                ParameterInvalidCastExpectedTypes = "IEnumerable",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<IEnumerable>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<decimal>()),
                ValidationName = validationName,
                ValidationParameterInvalidCastExpectedTypes = "string",
                ValidationParameterInvalidCastParameterName = "itemToSearchFor",
            };

            var stringTestValues2 = new TestValues<IEnumerable<string>>
            {
                MustValidationParameterInvalidTypeValues = new IEnumerable<string>[]
                {
                    new List<string> { A.Dummy<string>(), string.Empty, A.Dummy<string>() },
                    new string[] { A.Dummy<string>(), A.Dummy<string>() },
                    new List<string> { A.Dummy<string>(), null },
                    new string[] { A.Dummy<string>(), null },
                },
                MustEachValidationParameterInvalidTypeValues = new IEnumerable<IEnumerable<string>>[]
                {
                    new IEnumerable<string>[] { },
                    new IEnumerable<string>[] { new List<string> { null }, new string[] { A.Dummy<string>(), null } },
                    new IEnumerable<string>[] { new List<string> { A.Dummy<string>(), null } },
                    new IEnumerable<string>[] { new string[] { null }, new List<string> { A.Dummy<string>(), null }, new string[] { A.Dummy<string>(), A.Dummy<string>() } },
                },
            };

            validationTest2.Run(stringTestValues2);

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<string>()),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
            };

            var enumerableTestValues3 = new TestValues<IEnumerable<string>>
            {
                MustFailingValues = new IEnumerable<string>[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new IEnumerable<string>[] { new string[] { A.Dummy<string>(), null, A.Dummy<string>() }, null, new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
                },
            };

            validationTest3.Run(enumerableTestValues3);

            var comparisonValue4 = 10m;
            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(comparisonValue4),
                ValidationName = validationName,
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotContainExceptionMessageSuffix,
            };

            var decimalTestValues4 = new TestValues<IEnumerable<decimal>>
            {
                MustPassingValues = new[]
                {
                    new decimal[] { },
                    new[] { 5m, 9.9999999m, 10.000001m, 15m },
                },
                MustEachPassingValues = new IEnumerable<IEnumerable<decimal>>[]
                {
                    new IEnumerable<decimal>[] { },
                    new[] { new[] { A.Dummy<decimal>() }, new[] { 5m, 9.9999999m, 10.000001m, 15m } },
                },
                MustFailingValues = new IEnumerable<decimal>[]
                {
                    new[] { comparisonValue4 },
                    new[] { 5m, comparisonValue4, 15m },
                },
                MustEachFailingValues = new IEnumerable<IEnumerable<decimal>>[]
                {
                    new[] { new[] { 5m, comparisonValue4, 15m }, new[] { A.Dummy<decimal>() } },
                    new[] { new[] { A.Dummy<decimal>() }, new[] { 5m, comparisonValue4, 15m } },
                },
            };

            validationTest4.Run(decimalTestValues4);
        }

        [Fact]
        public static void NotContain___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = new int?[] { 1, null, 3 };
            int? itemToSearchFor1 = null;
            var expected1 = "Parameter 'testParameter1' contains the item to search for using EqualityComparer<T>.Default, where T: int?.  Specified 'itemToSearchFor' is '<null>'.";

            var testParameter2 = new int[] { 1, 10, 3 };
            int itemToSearchFor2 = 10;
            var expected2 = "Parameter 'testParameter2' contains the item to search for using EqualityComparer<T>.Default, where T: int.  Specified 'itemToSearchFor' is '10'.";

            var testParameter3 = new int?[][] { new int?[] { 1, null, 3 } };
            int? itemToSearchFor3 = null;
            var expected3 = "Parameter 'testParameter3' contains an element that contains the item to search for using EqualityComparer<T>.Default, where T: int?.  Specified 'itemToSearchFor' is '<null>'.";

            var testParameter4 = new int[][] { new int[] { 1, 10, 3 } };
            int itemToSearchFor4 = 10;
            var expected4 = "Parameter 'testParameter4' contains an element that contains the item to search for using EqualityComparer<T>.Default, where T: int.  Specified 'itemToSearchFor' is '10'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotContain(itemToSearchFor1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().NotContain(itemToSearchFor2));

            var actual3 = Record.Exception(() => new { testParameter3 }.Must().Each().NotContain(itemToSearchFor3));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().NotContain(itemToSearchFor4));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);

            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void BeAlphabetic___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(char[] otherAllowedCharacters)
            {
                return (parameter, because, applyBecause, data) => parameter.BeAlphabetic(otherAllowedCharacters, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<char[]>()),
                ValidationName = nameof(Verifications.BeAlphabetic),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "isAlphabetic", null, "isAlphabetic" },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new List<string> { A.Dummy<string>() } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(null),
                ValidationName = nameof(Verifications.BeAlphabetic),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphabeticExceptionMessageSuffix,
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                },
                MustFailingValues = new[]
                {
                    " ",
                    "\r\n",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyz5ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0", string.Empty },
                },
            };

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(new char[0]),
                ValidationName = nameof(Verifications.BeAlphabetic),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphabeticExceptionMessageSuffix,
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                },
                MustFailingValues = new[]
                {
                    " ",
                    "\r\n",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyz5ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0", string.Empty },
                },
            };

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(new[] { 'b', '-', '_', '^', '\\', '/', '(', 'b', ' ' }),
                ValidationName = nameof(Verifications.BeAlphabetic),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphabeticExceptionMessageSuffix,
            };

            var stringTestValues4 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    @"abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ-_^\/(",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, @"abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ-_^\/(" },
                },
                MustFailingValues = new[]
                {
                    "\r\n",
                    "9abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    "&abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ)",
                    "abcdefghijklmnopqrstuvwxyz$ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ)", string.Empty },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues);

            validationTest2.Run(stringTestValues2);

            validationTest3.Run(stringTestValues3);

            validationTest4.Run(stringTestValues4);
        }

        [Fact]
        public static void BeAlphabetic___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = "abc-def";
            var expected1 = "Parameter 'testParameter1' is not alphabetic.  Parameter value is 'abc-def'.  Specified 'otherAllowedCharacters' is <null>.";

            var testParameter2 = "abc-def";
            var expected2 = "Parameter 'testParameter2' is not alphabetic.  Parameter value is 'abc-def'.  Specified 'otherAllowedCharacters' is [<empty>].";

            var testParameter3 = "abc4def";
            var expected3 = "Parameter 'testParameter3' is not alphabetic.  Parameter value is 'abc4def'.  Specified 'otherAllowedCharacters' is ['-'].";

            var testParameter4 = new[] { "a-c", "d7f", "g*i" };
            var expected4 = "Parameter 'testParameter4' contains an element that is not alphabetic.  Element value is 'd7f'.  Specified 'otherAllowedCharacters' is ['-', '*'].";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeAlphabetic());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeAlphabetic(otherAllowedCharacters: new char[0]));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeAlphabetic(otherAllowedCharacters: new[] { '-' }));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeAlphabetic(otherAllowedCharacters: new[] { '-', '*' }));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void BeAlphanumeric___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(char[] otherAllowedCharacters)
            {
                return (parameter, because, applyBecause, data) => parameter.BeAlphanumeric(otherAllowedCharacters, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(A.Dummy<char[]>()),
                ValidationName = nameof(Verifications.BeAlphanumeric),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "isalphanumeric1", null, "isalphanumeric2" },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new List<string> { A.Dummy<string>() } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(null),
                ValidationName = nameof(Verifications.BeAlphanumeric),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphanumericExceptionMessageSuffix,
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" },
                },
                MustFailingValues = new[]
                {
                    " ",
                    "\r\n",
                    "-abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*",
                    "abcdefghijklmnopqrstuvwxyz%ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*", string.Empty },
                },
            };

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(new char[0]),
                ValidationName = nameof(Verifications.BeAlphanumeric),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphanumericExceptionMessageSuffix,
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" },
                },
                MustFailingValues = new[]
                {
                    " ",
                    "\r\n",
                    "-abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*",
                    "abcdefghijklmnopqrstuvwxyz%ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*", string.Empty },
                },
            };

            var validationTest4 = new ValidationTest
            {
                Validation = GetValidation(new[] { '0', '-', '_', '^', '\\', '/', '(', '0', ' ' }),
                ValidationName = nameof(Verifications.BeAlphanumeric),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAlphanumericExceptionMessageSuffix,
            };

            var stringTestValues4 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    @"abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ-_^\/(1234567890",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, @"abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ-_^\/(1234567890" },
                },
                MustFailingValues = new[]
                {
                    "\r\n",
                    "&abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890)",
                    "abcdefghijklmnopqrstuvwxyz$ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890)", string.Empty },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues);

            validationTest2.Run(stringTestValues2);

            validationTest3.Run(stringTestValues3);

            validationTest4.Run(stringTestValues4);
        }

        [Fact]
        public static void BeAlphanumeric___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = "abc-def";
            var expected1 = "Parameter 'testParameter1' is not alphanumeric.  Parameter value is 'abc-def'.  Specified 'otherAllowedCharacters' is <null>.";

            var testParameter2 = "abc-def";
            var expected2 = "Parameter 'testParameter2' is not alphanumeric.  Parameter value is 'abc-def'.  Specified 'otherAllowedCharacters' is [<empty>].";

            var testParameter3 = "abc*def";
            var expected3 = "Parameter 'testParameter3' is not alphanumeric.  Parameter value is 'abc*def'.  Specified 'otherAllowedCharacters' is ['-'].";

            var testParameter4 = new[] { "a-c", "d f", "g*i" };
            var expected4 = "Parameter 'testParameter4' contains an element that is not alphanumeric.  Element value is 'd f'.  Specified 'otherAllowedCharacters' is ['-', '*'].";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeAlphanumeric());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeAlphanumeric(otherAllowedCharacters: new char[0]));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().BeAlphanumeric(otherAllowedCharacters: new[] { '-' }));
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeAlphanumeric(otherAllowedCharacters: new[] { '-', '*' }));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void BeAsciiPrintable___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(bool treatNewlineAsPrintable)
            {
                return (parameter, because, applyBecause, data) => parameter.BeAsciiPrintable(treatNewlineAsPrintable, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(false),
                ValidationName = nameof(Verifications.BeAsciiPrintable),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "isalphanumeric1", null, "isalphanumeric2" },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new List<string> { A.Dummy<string>() } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(false),
                ValidationName = nameof(Verifications.BeAsciiPrintable),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAsciiPrintableExceptionMessageSuffix,
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+,-./0123456789:;<=>?@[\]^_`{|}~",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+,-./0123456789:;<=>?@[\]^_`{|}~" },
                },
                MustFailingValues = new[]
                {
                    "\r\n",
                    $@"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+{Environment.NewLine},-./0123456789:;<=>?@[\]^_`{{|}}~",
                    Convert.ToChar(31).ToString(),
                    Convert.ToChar(127).ToString(),
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, "\r\n", string.Empty },
                    new string[] { string.Empty, $@"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+{Environment.NewLine},-./0123456789:;<=>?@[\]^_`{{|}}~", string.Empty },
                    new string[] { string.Empty, Convert.ToChar(31).ToString(), string.Empty },
                    new string[] { string.Empty, Convert.ToChar(127).ToString(), string.Empty },
                },
            };

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation(true),
                ValidationName = nameof(Verifications.BeAsciiPrintable),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeAsciiPrintableExceptionMessageSuffix,
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    $@"abcdefghijklmnopqrstuvwxyz{Environment.NewLine}ABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+{Environment.NewLine},-./0123456789:;<=>?@[\]^_`{{|}}~",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, $@"abcdefghijklmnopqrstuvwxyz{Environment.NewLine}ABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+{Environment.NewLine},-./0123456789:;<=>?@[\]^_`{{|}}~" },
                },
                MustFailingValues = new[]
                {
                    Convert.ToChar(31).ToString(),
                    Convert.ToChar(127).ToString(),
                },
                MustEachFailingValues = new[]
                {
                    new string[] { string.Empty, Convert.ToChar(31).ToString(), string.Empty },
                    new string[] { string.Empty, Convert.ToChar(127).ToString(), string.Empty },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues);

            validationTest2.Run(stringTestValues2);

            validationTest3.Run(stringTestValues3);
        }

        [Fact]
        public static void BeAsciiPrintable___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = $"abc{Environment.NewLine}def";
            var expected1 = $"Parameter 'testParameter1' is not ASCII Printable.  Parameter value is 'abc{Environment.NewLine}def'.  Specified 'treatNewLineAsPrintable' is 'False'.";

            var testParameter2 = $"abc{Environment.NewLine}def" + Convert.ToChar(30);
            var expected2 = $"Parameter 'testParameter2' is not ASCII Printable.  Parameter value is 'abc{Environment.NewLine}def{Convert.ToChar(30)}'.  Specified 'treatNewLineAsPrintable' is 'True'.";

            var testParameter3 = new[] { "a-c", $"d{Environment.NewLine}f", "g*i" };
            var expected3 = $"Parameter 'testParameter3' contains an element that is not ASCII Printable.  Element value is 'd{Environment.NewLine}f'.  Specified 'treatNewLineAsPrintable' is 'False'.";

            var testParameter4 = new[] { "a-c", $"d{Environment.NewLine}f" + Convert.ToChar(30), "g*i" };
            var expected4 = $"Parameter 'testParameter4' contains an element that is not ASCII Printable.  Element value is 'd{Environment.NewLine}f{Convert.ToChar(30)}'.  Specified 'treatNewLineAsPrintable' is 'True'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeAsciiPrintable());
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().BeAsciiPrintable(true));
            var actual3 = Record.Exception(() => new { testParameter3 }.Must().Each().BeAsciiPrintable());
            var actual4 = Record.Exception(() => new { testParameter4 }.Must().Each().BeAsciiPrintable(true));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
            actual3.Message.Should().Be(expected3);
            actual4.Message.Should().Be(expected4);
        }

        [Fact]
        public static void BeMatchedByRegex___Should_throw_ArgumentNullException___When_parameter_regex_is_null()
        {
            // Arrange, Act
            var testParameter = A.Dummy<string>();
            var actual = Record.Exception(() => new { testParameter }.Must().BeMatchedByRegex(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("regex");
        }

        [Fact]
        public static void BeMatchedByRegex___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(Regex regex)
            {
                return (parameter, because, applyBecause, data) => parameter.BeMatchedByRegex(regex, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(new Regex("abc")),
                ValidationName = nameof(Verifications.BeMatchedByRegex),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "abc", null, "abc" },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new List<string> { A.Dummy<string>() } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(new Regex("abc")),
                ValidationName = nameof(Verifications.BeMatchedByRegex),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.BeMatchedByRegexExceptionMessageSuffix,
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "abc",
                    "def-abc-def",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "abc", "def-abc-def" },
                },
                MustFailingValues = new[]
                {
                    string.Empty,
                    "\r\n",
                    "a-b-c",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "abc", "a-b-c", "abc" },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues);

            validationTest2.Run(stringTestValues2);
        }

        [Fact]
        public static void BeMatchedByRegex___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = "abc-def";
            var regex1 = new Regex("^abc$");
            var expected1 = "Parameter 'testParameter1' is not matched by the specified regex.  Parameter value is 'abc-def'.  Specified 'regex' is ^abc$.";

            var testParameter2 = new[] { "abc", "abc-def", "abc" };
            var regex2 = new Regex("^abc$");
            var expected2 = "Parameter 'testParameter2' contains an element that is not matched by the specified regex.  Element value is 'abc-def'.  Specified 'regex' is ^abc$.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().BeMatchedByRegex(regex1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().BeMatchedByRegex(regex2));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotBeMatchedByRegex___Should_throw_ArgumentNullException___When_parameter_regex_is_null()
        {
            // Arrange, Act
            var testParameter = A.Dummy<string>();
            var actual = Record.Exception(() => new { testParameter }.Must().NotBeMatchedByRegex(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("regex");
        }

        [Fact]
        public static void NotBeMatchedByRegex___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(Regex regex)
            {
                return (parameter, because, applyBecause, data) => parameter.NotBeMatchedByRegex(regex, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation(new Regex("abc")),
                ValidationName = nameof(Verifications.NotBeMatchedByRegex),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.Empty, Guid.Empty },
                    new Guid[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    Guid.Empty,
                    null,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { }, new Guid?[] { Guid.Empty, Guid.Empty },
                    new Guid?[] { Guid.Empty, null, Guid.Empty },
                    new Guid?[] { Guid.Empty, Guid.NewGuid(), Guid.Empty },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "def", null, "def" },
                },
            };

            var enumerableTestValues = new TestValues<IEnumerable>
            {
                MustParameterInvalidTypeValues = new IEnumerable[]
                {
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new IEnumerable[] { new List<string> { A.Dummy<string>() } },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string>() { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustParameterInvalidTypeValues = new bool[]
                {
                    true,
                    false,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool[] { },
                    new bool[] { true },
                },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustParameterInvalidTypeValues = new bool?[]
                {
                    true,
                    false,
                    null,
                },
                MustEachParameterInvalidTypeValues = new[]
                {
                    new bool?[] { },
                    new bool?[] { true },
                    new bool?[] { null },
                },
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation(new Regex("abc")),
                ValidationName = nameof(Verifications.NotBeMatchedByRegex),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeMatchedByRegexExceptionMessageSuffix,
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    string.Empty,
                    "def",
                    "a-b-c",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { string.Empty, "def", "a-b-c" },
                },
                MustFailingValues = new[]
                {
                    "abc",
                    "def-abc-def",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "def", "abc", "def" },
                },
            };

            // Act, Assert
            validationTest1.Run(stringTestValues1);
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(boolTestValues);
            validationTest1.Run(nullableBoolTestValues);
            validationTest1.Run(enumerableTestValues);

            validationTest2.Run(stringTestValues2);
        }

        [Fact]
        public static void NotBeMatchedByRegex___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            var testParameter1 = "def-abc-def";
            var regex1 = new Regex("abc");
            var expected1 = "Parameter 'testParameter1' is matched by the specified regex.  Parameter value is 'def-abc-def'.  Specified 'regex' is abc.";

            var testParameter2 = new[] { "def", "def-abc-def", "def" };
            var regex2 = new Regex("abc");
            var expected2 = "Parameter 'testParameter2' contains an element that is matched by the specified regex.  Element value is 'def-abc-def'.  Specified 'regex' is abc.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotBeMatchedByRegex(regex1));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotBeMatchedByRegex(regex2));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void StartWith___Should_throw_ArgumentNullException___When_parameter_comparisonValue_is_null()
        {
            // Arrange, Act
            var testParameter = A.Dummy<string>();
            var actual = Record.Exception(() => new { testParameter }.Must().StartWith(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("comparisonValue");
        }

        [Fact]
        public static void StartWith___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(string comparisonValue, StringComparison? comparisonType)
            {
                return (parameter, because, applyBecause, data) => parameter.StartWith(comparisonValue, comparisonType, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation("starter", A.Dummy<StringComparison>()),
                ValidationName = nameof(Verifications.StartWith),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation("starter", null),
                ValidationName = nameof(Verifications.StartWith),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.StartWithExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "starter", null, "starter" },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "starter",
                    "starter" + A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "starter", "starter" + A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    "Astarter",
                    "Somestarter",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "starter", string.Empty, "starter" + A.Dummy<string>() },
                },
            };

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation("staRTer", StringComparison.OrdinalIgnoreCase),
                ValidationName = nameof(Verifications.StartWith),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.StartWithExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "starter",
                    "starteR" + A.Dummy<string>(),
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "stArter", "STarter" + A.Dummy<string>() },
                },
                MustFailingValues = new string[]
                {
                    string.Empty,
                    "AstaRTer",
                    "SomestaRTer",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "staRTer", string.Empty, "staRTer" + A.Dummy<string>() },
                },
            };

            // Act, Assert
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(stringTestValues1);

            validationTest2.Run(guidTestValues);
            validationTest2.Run(nullableGuidTestValues);
            validationTest2.Run(objectTestValues);
            validationTest2.Run(stringTestValues2);

            validationTest3.Run(stringTestValues3);
        }

        [Fact]
        public static void StartWith___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = "some-string";
            var expected1 = Invariant($"Parameter 'testParameter1' does not start with the specified comparison value.  Parameter value is 'some-string'.  Specified 'comparisonValue' is 'starter'.  Specified 'comparisonType' is '<null>'.");

            var testParameter2 = new[] { "starter", "some-string", "starter" };
            var expected2 = "Parameter 'testParameter2' contains an element that does not start with the specified comparison value.  Element value is 'some-string'.  Specified 'comparisonValue' is 'starter'.  Specified 'comparisonType' is 'OrdinalIgnoreCase'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().StartWith("starter"));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().StartWith("starter", StringComparison.OrdinalIgnoreCase));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        [Fact]
        public static void NotStartWith___Should_throw_ArgumentNullException___When_parameter_comparisonValue_is_null()
        {
            // Arrange, Act
            var testParameter = A.Dummy<string>();
            var actual = Record.Exception(() => new { testParameter }.Must().NotStartWith(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("comparisonValue");
        }

        [Fact]
        public static void NotStartWith___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            Validation GetValidation(string comparisonValue, StringComparison? comparisonType)
            {
                return (parameter, because, applyBecause, data) => parameter.NotStartWith(comparisonValue, comparisonType, because, applyBecause, data);
            }

            var validationTest1 = new ValidationTest
            {
                Validation = GetValidation("starter", A.Dummy<StringComparison>()),
                ValidationName = nameof(Verifications.NotStartWith),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotBeNullExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = GetValidation("starter", null),
                ValidationName = nameof(Verifications.NotStartWith),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotStartWithExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustParameterInvalidTypeValues = new[]
                {
                    Guid.Empty,
                    Guid.NewGuid(),
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid>[]
                {
                    new Guid[] { },
                    new Guid[] { Guid.NewGuid() },
                },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustParameterInvalidTypeValues = new Guid?[]
                {
                    A.Dummy<Guid>(),
                    Guid.Empty,
                    null,
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<Guid?>[]
                {
                    new Guid?[] { },
                    new Guid?[] { Guid.NewGuid(), Guid.NewGuid() },
                    new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() },
                },
            };

            var objectTestValues = new TestValues<object>
            {
                MustParameterInvalidTypeValues = new object[]
                {
                    null,
                    A.Dummy<object>(),
                    new List<string> { null },
                },
                MustEachParameterInvalidTypeValues = new IEnumerable<object>[]
                {
                    new object[] { },
                    new object[] { A.Dummy<object>(), A.Dummy<object>() },
                    new object[] { A.Dummy<object>(), null, A.Dummy<object>() },
                },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustFailingValues = new string[]
                {
                    null,
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "something", null, "something" },
                },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "something",
                    "something-starter",
                    "Starter",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "something", "something-starter", "Starter" },
                },
                MustFailingValues = new string[]
                {
                    "starter",
                    "starter-something",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "something", "starter", "something" },
                },
            };

            var validationTest3 = new ValidationTest
            {
                Validation = GetValidation("STarter", StringComparison.OrdinalIgnoreCase),
                ValidationName = nameof(Verifications.NotStartWith),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = Verifications.NotStartWithExceptionMessageSuffix,
                ParameterInvalidCastExpectedTypes = "string",
                ParameterInvalidCastExpectedEnumerableTypes = "IEnumerable<string>",
            };

            var stringTestValues3 = new TestValues<string>
            {
                MustPassingValues = new string[]
                {
                    "something",
                    "something-STarter",
                },
                MustEachPassingValues = new[]
                {
                    new string[] { },
                    new string[] { "something", "something-STarter" },
                },
                MustFailingValues = new string[]
                {
                    "STarter",
                    "STarter-something",
                    "starter",
                    "starter-something",
                },
                MustEachFailingValues = new[]
                {
                    new string[] { "something", "starter", "something" },
                },
            };

            // Act, Assert
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(stringTestValues1);

            validationTest2.Run(guidTestValues);
            validationTest2.Run(nullableGuidTestValues);
            validationTest2.Run(objectTestValues);
            validationTest2.Run(stringTestValues2);

            validationTest3.Run(stringTestValues3);
        }

        [Fact]
        public static void NotStartWith___Should_throw_with_expected_Exception_message___When_called()
        {
            // Arrange
            string testParameter1 = "starter-something";
            var expected1 = Invariant($"Parameter 'testParameter1' starts with the specified comparison value.  Parameter value is 'starter-something'.  Specified 'comparisonValue' is 'starter'.  Specified 'comparisonType' is '<null>'.");

            var testParameter2 = new[] { "something", "STARTER-something", "something" };
            var expected2 = "Parameter 'testParameter2' contains an element that starts with the specified comparison value.  Element value is 'STARTER-something'.  Specified 'comparisonValue' is 'starter'.  Specified 'comparisonType' is 'OrdinalIgnoreCase'.";

            // Act
            var actual1 = Record.Exception(() => new { testParameter1 }.Must().NotStartWith("starter"));
            var actual2 = Record.Exception(() => new { testParameter2 }.Must().Each().NotStartWith("starter", StringComparison.OrdinalIgnoreCase));

            // Assert
            actual1.Message.Should().Be(expected1);
            actual2.Message.Should().Be(expected2);
        }

        private static void Run<T>(
            this ValidationTest validationTest,
            TestValues<T> testValues)
        {
            var parameterNames = new[] { null, A.Dummy<string>() };

            var userData = new[] { null, A.Dummy<Dictionary<string, string>>() };

            foreach (var parameterName in parameterNames)
            {
                foreach (var data in userData)
                {
                    RunPassingScenarios(validationTest, testValues, parameterName, data);

                    RunMustFailingScenarios(validationTest, testValues, parameterName, data);

                    RunMustEachImproperUseOfFrameworkScenarios<T>(validationTest, parameterName, data);

                    RunMustEachFailingScenarios(validationTest, testValues, parameterName, data);

                    RunMustInvalidParameterTypeScenarios(validationTest, testValues, parameterName, data);

                    RunMustEachInvalidParameterTypeScenarios(validationTest, testValues, parameterName, data);

                    RunInvalidValidationParameterTypeScenarios(validationTest, testValues, parameterName, data);
                }
            }
        }

        private static void RunPassingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            var mustParameters = testValues.MustPassingValues.Select(_ => _.Named(parameterName).Must());
            var mustEachParameters = testValues.MustEachPassingValues.Select(_ => _.Named(parameterName).Must().Each());
            var parameters = mustParameters.Concat(mustEachParameters).ToList();

            foreach (var parameter in parameters)
            {
                // Arrange
                var expected = parameter.CloneWithActionVerifiedAtLeastOnce();

                // Act
                var actual = validationTest.Validation(parameter, data: data);

                // Assert
                ParameterComparer.Equals(actual, expected).Should().BeTrue();
            }
        }

        private static void RunMustFailingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            foreach (var failingValue in testValues.MustFailingValues)
            {
                // Arrange
                var parameter = failingValue.Named(parameterName).Must();
                string expectedExceptionMessage;
                if (parameterName == null)
                {
                    expectedExceptionMessage = "Parameter " + validationTest.ExceptionMessageSuffix;
                }
                else
                {
                    expectedExceptionMessage = "Parameter '" + parameterName + "' " + validationTest.ExceptionMessageSuffix;
                }

                var expectedData = data == null ? new Hashtable() : data;

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, data: data));

                // Assert
                actual.Should().BeOfType(validationTest.ExceptionType);
                actual.Message.Should().StartWith(expectedExceptionMessage);
                actual.Data.Keys.Should().BeEquivalentTo(expectedData.Keys);
                foreach (var dataKey in actual.Data.Keys)
                {
                    actual.Data[dataKey].Should().Be(expectedData[dataKey]);
                }
            }
        }

        private static void RunMustEachFailingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            foreach (var eachFailingValue in testValues.MustEachFailingValues)
            {
                // Arrange
                var parameter = eachFailingValue.Named(parameterName).Must().Each();
                string expectedExceptionMessage;
                if (parameterName == null)
                {
                    expectedExceptionMessage = "Parameter contains an element that " + validationTest.ExceptionMessageSuffix;
                }
                else
                {
                    expectedExceptionMessage = "Parameter '" + parameterName + "' contains an element that " + validationTest.ExceptionMessageSuffix;
                }

                var expectedData = data == null ? new Hashtable() : data;

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, data: data));

                // Assert
                actual.Should().BeOfType(validationTest.EachExceptionType);
                actual.Message.Should().StartWith(expectedExceptionMessage);
                actual.Data.Keys.Should().BeEquivalentTo(expectedData.Keys);
                foreach (var dataKey in actual.Data.Keys)
                {
                    actual.Data[dataKey].Should().Be(expectedData[dataKey]);
                }
            }
        }

        private static void RunMustInvalidParameterTypeScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            foreach (var invalidTypeValue in testValues.MustParameterInvalidTypeValues)
            {
                // Arrange
                var valueTypeName = testValues.MustParameterInvalidTypeValues.GetType().GetEnumerableGenericType().ToStringReadable();
                var parameter = invalidTypeValue.Named(parameterName).Must();
                var expectedMessage = Invariant($"Called {validationTest.ValidationName}() on a value of type {valueTypeName}, which is not one of the following expected type(s): {validationTest.ParameterInvalidCastExpectedTypes}.");

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, data: data));

                // Assert
                actual.Should().BeOfType<InvalidCastException>();
                actual.Message.Should().Be(expectedMessage);
            }
        }

        private static void RunMustEachInvalidParameterTypeScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            foreach (var invalidTypeValue in testValues.MustEachParameterInvalidTypeValues)
            {
                // Arrange
                var valueTypeName = testValues.MustParameterInvalidTypeValues.GetType().GetEnumerableGenericType().ToStringReadable();
                var parameter = invalidTypeValue.Named(parameterName).Must().Each();
                var expectedMessage = Invariant($"Called {validationTest.ValidationName}() on a value of type IEnumerable<{valueTypeName}>, which is not one of the following expected type(s): {validationTest.ParameterInvalidCastExpectedEnumerableTypes}.");

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, data: data));

                // Assert
                actual.Should().BeOfType<InvalidCastException>();
                actual.Message.Should().Be(expectedMessage);
            }
        }

        private static void RunMustEachImproperUseOfFrameworkScenarios<T>(
            ValidationTest validationTest,
            string parameterName,
            IDictionary data)
        {
            // Arrange
            // calling Each() on IEnumerable that is not IEnumerable OR a value that's null
            object notEnumerable = new object();
            var parameter1 = notEnumerable.Named(parameterName).Must();
            parameter1.Actions |= Actions.Eached;
            var expectedExceptionMessage1 = Invariant($"Called Each() on a value of type object, which is not one of the following expected type(s): IEnumerable.");

            IEnumerable<string> nullEnumerable = null;
            var parameter2 = nullEnumerable.Named(parameterName).Must();
            parameter2.Actions |= Actions.Eached;
            string expectedExceptionMessage2;
            if (parameterName == null)
            {
                expectedExceptionMessage2 = "Parameter " + Verifications.NotBeNullExceptionMessageSuffix + ".";
            }
            else
            {
                expectedExceptionMessage2 = "Parameter '" + parameterName + "' " + Verifications.NotBeNullExceptionMessageSuffix + ".";
            }

            // Act
            var actual1 = Record.Exception(() => validationTest.Validation(parameter1, data: data));
            var actual2 = Record.Exception(() => validationTest.Validation(parameter2, data: data));

            // Assert
            actual1.Should().BeOfType<InvalidCastException>();
            actual1.Message.Should().Be(expectedExceptionMessage1);

            actual2.Should().BeOfType<ArgumentNullException>();
            actual2.Message.Should().Be(expectedExceptionMessage2);
        }

        private static void RunInvalidValidationParameterTypeScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            IDictionary data)
        {
            var mustParameters = testValues.MustValidationParameterInvalidTypeValues.Select(_ => _.Named(parameterName).Must());
            var mustEachParameters = testValues.MustEachValidationParameterInvalidTypeValues.Select(_ => _.Named(parameterName).Must().Each());
            var parameters = mustParameters.Concat(mustEachParameters).ToList();

            foreach (var parameter in parameters)
            {
                // Arrange
                testValues.GetType().GetGenericArguments().First().ToStringReadable();
                var expectedStartOfMessage = Invariant($"Called {validationTest.ValidationName}({validationTest.ValidationParameterInvalidCastParameterName}:) where '{validationTest.ValidationParameterInvalidCastParameterName}' is of type");
                var expectedEndOfMessage = Invariant($"which is not one of the following expected type(s): {validationTest.ValidationParameterInvalidCastExpectedTypes}.");

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, data: data));

                // Assert
                actual.Should().BeOfType<InvalidCastException>();
                actual.Message.Should().StartWith(expectedStartOfMessage);
                actual.Message.Should().EndWith(expectedEndOfMessage);
            }
        }

        private static Type GetEnumerableGenericType(
            this Type enumerableType)
        {
            // adapted from: https://stackoverflow.com/a/17713382/356790
            Type result;
            if (enumerableType.IsArray)
            {
                // type is array, shortcut
                result = enumerableType.GetElementType();
            }
            else if (enumerableType.IsGenericType && (enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
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
                    .Where(_ => _.IsGenericType && (_.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    .Select(_ => _.GenericTypeArguments[0])
                    .FirstOrDefault();

                if (result == null)
                {
                    // here we just assume it's an IEnumerable and return typeof(object),
                    // however, for completeness, we should recurse through all interface implementations
                    // and check whether those are IEnumerable<T>.
                    // see: https://stackoverflow.com/questions/5461295/using-isassignablefrom-with-open-generic-types
                    result = typeof(object);
                }
            }

            return result;
        }

        private class ValidationTest
        {
            public Validation Validation { get; set; }

            public Type ExceptionType { get; set; }

            public Type EachExceptionType { get; set; }

            public string ExceptionMessageSuffix { get; set; }

            public string ParameterInvalidCastExpectedTypes { get; set; }

            public string ParameterInvalidCastExpectedEnumerableTypes { get; set; }

            public string ValidationParameterInvalidCastExpectedTypes { get; set; }

            public string ValidationParameterInvalidCastParameterName { get; set; }

            public string ValidationName { get; set; }
        }

        private class TestValues<T>
        {
            public IReadOnlyCollection<T> MustParameterInvalidTypeValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachParameterInvalidTypeValues { get; set; } = new List<List<T>>();

            public IReadOnlyCollection<T> MustValidationParameterInvalidTypeValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachValidationParameterInvalidTypeValues { get; set; } = new List<List<T>>();

            public IReadOnlyCollection<T> MustPassingValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachPassingValues { get; set; } = new List<List<T>>();

            public IReadOnlyCollection<T> MustFailingValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachFailingValues { get; set; } = new List<List<T>>();
        }

        private class TestClass
        {
        }
    }
}