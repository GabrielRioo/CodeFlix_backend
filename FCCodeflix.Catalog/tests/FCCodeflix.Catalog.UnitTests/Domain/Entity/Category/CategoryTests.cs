﻿using FCCodeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainEntity = FCCodeflix.Catalog.Domain.Entity;

namespace FCCodeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTests
{
	private readonly CategoryTestFixture _categoryTestFixture = new CategoryTestFixture();

	public CategoryTests(CategoryTestFixture categoryTestFixture)
	{
		_categoryTestFixture = categoryTestFixture;
	}

	[Fact(DisplayName = nameof(Instantiete))]
	[Trait("Domain", "Category - Aggregates")]
	public void Instantiete()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();

		var datetimeBefore = DateTime.Now;
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
		var datetimeAfter = DateTime.Now;

		(category).Should().NotBeNull();
		(category.Name).Should().Be(validCategory.Name);
		(category.Description).Should().Be(validCategory.Description);
		(category.Id).Should().NotBeEmpty();
		(category.CreatedAt).Should().NotBeSameDateAs(default(DateTime));
		(category.CreatedAt > datetimeBefore).Should().BeTrue();
		(category.CreatedAt < datetimeAfter).Should().BeTrue();
		(category.IsActive).Should().BeTrue();
	}

	[Theory(DisplayName = nameof(InstantieteWithIsActive))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData(true)]
	[InlineData(false)]
	public void InstantieteWithIsActive(bool isActive)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();

		var datetimeBefore = DateTime.Now;
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
		var datetimeAfter = DateTime.Now;

		(category).Should().NotBeNull();
		(category.Name).Should().Be(validCategory.Name);
		(category.Description).Should().Be(validCategory.Description);
		(category.Id).Should().NotBeEmpty();
		(category.CreatedAt).Should().NotBeSameDateAs(default(DateTime));
		(category.CreatedAt > datetimeBefore).Should().BeTrue();
		(category.CreatedAt < datetimeAfter).Should().BeTrue();
		(category.IsActive).Should().Be(isActive);
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	public void InstantiateErrorWhenNameIsEmpty(string? name)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();

		Action action = () => new DomainEntity.Category(name!, validCategory.Description);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should not be empty or null");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsNull()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();

		Action action = () => new DomainEntity.Category(validCategory.Name, null!);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should not be empty or null");
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("1")]
	[InlineData("12")]
	[InlineData("a")]
	[InlineData("ga")]
	public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();

		Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be at least 3 characters long");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenNameIsGreaterThan255Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(x => "a").ToArray());

		Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be less or equal 255 characters long");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(x => "a").ToArray());

		Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription );

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should be less or equal 10.000 characters long");
	}

	[Fact(DisplayName = nameof(Activate))]
	[Trait("Domain", "Category - Aggregates")]
	public void Activate()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

		category.Activate();

		category.IsActive.Should().BeTrue();
	}

	[Fact(DisplayName = nameof(Deactivate))]
	[Trait("Domain", "Category - Aggregates")]
	public void Deactivate()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

		category.Deactivate();

		category.IsActive.Should().BeFalse();
	}

	[Fact(DisplayName = nameof(Update))]
	[Trait("Domain", "Category - Aggregates")]
	public void Update()
	{
		var category = _categoryTestFixture.GetValidCategory();
		var newValues = new { Name = "New name", Description = "New Description" };

		category.Update(newValues.Name, newValues.Description);

		category.Name.Should().Be(newValues.Name);
		category.Description.Should().Be(newValues.Description);
	}

	[Fact(DisplayName = nameof(UpdateOnlyName))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateOnlyName()
	{
		var category = _categoryTestFixture.GetValidCategory();
		var newValues = new { Name = "New name" };
		var currentDescription = category.Description;

		category.Update(newValues.Name);

		category.Name.Should().Be(newValues.Name);
		category.Description.Should().Be(currentDescription);
	}

	[Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	public void UpdateErrorWhenNameIsEmpty(string? name)
	{
		var category = _categoryTestFixture.GetValidCategory();

		Action action = () => category.Update(name!);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should not be empty or null");
	}

	[Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("1")]
	[InlineData("12")]
	[InlineData("a")]
	[InlineData("ga")]
	public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
	{
		var category = _categoryTestFixture.GetValidCategory();

		Action action = () => category.Update(invalidName);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be at least 3 characters long");
	}

	[Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateErrorWhenNameIsGreaterThan255Characters()
	{
		var category = _categoryTestFixture.GetValidCategory();

		var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(x => "a").ToArray());
		Action action = () => category.Update(invalidName);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be less or equal 255 characters long");
	}

	[Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
	{
		var category = _categoryTestFixture.GetValidCategory();

		var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(x => "a").ToArray());
		Action action = () => category.Update("Category New Name", invalidDescription);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should be less or equal 10.000 characters long");
	}
}
