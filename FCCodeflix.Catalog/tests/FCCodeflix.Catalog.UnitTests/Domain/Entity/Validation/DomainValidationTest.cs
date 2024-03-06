﻿using Bogus;
using Xunit;
using FCCodeflix.Catalog.Domain.Validation;
using FluentAssertions;
using FCCodeflix.Catalog.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace FCCodeflix.Catalog.UnitTests.Domain.Entity.Validation;
public class DomainValidationTest
{
	public Faker Faker { get; set; } = new Faker();

	[Fact(DisplayName = nameof(NotNullOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void NotNullOk()
	{
		var value = Faker.Commerce.ProductName();

		Action action = () => DomainValidation.NotNull(value, "Value");

		action.Should().NotThrow();
	}

	[Fact(DisplayName = nameof(NotNullThrowWhenNull))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void NotNullThrowWhenNull()
	{
		string? value = null;

		Action action = () => DomainValidation.NotNull(value, "FieldName");

		action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
	}

	[Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
	[Trait("Domain", "DomainValidation - Validation")]
	[InlineData("")]
	[InlineData("   ")]
	[InlineData(null)]
	public void NotNullOrEmptyThrowWhenEmpty(string? target)
	{
		Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

		action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null or empty");
	}

	[Fact(DisplayName = nameof(NotNullOrEmptyThrowWhenEmptyOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void NotNullOrEmptyThrowWhenEmptyOk()
	{
		var target = Faker.Commerce.ProductName();

		Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

		action.Should().NotThrow();
	}

	[Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
	[Trait("Domain", "DomainValidation - Validation")]
	[MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
	public void MinLengthThrowWhenLess(string target, int minLength)
	{
		Action action = () => DomainValidation.MinLength(target, minLength,  "FieldName");

		action.Should().Throw<EntityValidationException>().WithMessage($"FieldName should not be less than {minLength} characters long");
	}

	public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
	{
		yield return new object[] { "123456", 10 };
		var faker = new Faker();
		for (int i = 0; i < (numberOfTests -1); i++)
		{
			var example = faker.Commerce.ProductName();
			var minLength = example.Length + (new Random()).Next(1, 20);

			yield return new object[] { example, minLength };
		}
	}

	[Theory(DisplayName = nameof(MinLengthOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	[MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
	public void MinLengthOk(string target, int minLength)
	{
		Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");

		action.Should().NotThrow();
	}

	public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
	{
		yield return new object[] { "123456", 6 };
		var faker = new Faker();
		for (int i = 0; i < (numberOfTests - 1); i++)
		{
			var example = faker.Commerce.ProductName();
			var minLength = example.Length - (new Random()).Next(1, 5);

			yield return new object[] { example, minLength };
		}
	}
}
