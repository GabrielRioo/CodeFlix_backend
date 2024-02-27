using FCCodeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = FCCodeflix.Catalog.Domain.Entity;

namespace FCCodeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTests
{
	[Fact(DisplayName = nameof(Instantiete))]
	[Trait("Domain", "Category - Aggregates")]
	public void Instantiete()
	{
		var validData = new
		{
			Name = "category name",
			Description = "category description",
		};

		var datetimeBefore = DateTime.Now;
		var category = new DomainEntity.Category(validData.Name, validData.Description);
		var datetimeAfter = DateTime.Now;

		Assert.NotNull(category);
		Assert.Equal(validData.Name, category.Name);
		Assert.Equal(validData.Description, category.Description);
		Assert.NotEqual(default(Guid), category.Id);
		Assert.NotEqual(default(DateTime), category.CreatedAt);
		Assert.True(category.CreatedAt > datetimeBefore);
		Assert.True(category.CreatedAt < datetimeAfter);
		Assert.True(category.IsActive);
	}

	[Theory(DisplayName = nameof(InstantieteWithIsActive))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData(true)]
	[InlineData(false)]
	public void InstantieteWithIsActive(bool isActive)
	{
		var validData = new
		{
			Name = "category name",
			Description = "category description",
		};

		var datetimeBefore = DateTime.Now;
		var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
		var datetimeAfter = DateTime.Now;

		Assert.NotNull(category);
		Assert.Equal(validData.Name, category.Name);
		Assert.Equal(validData.Description, category.Description);
		Assert.NotEqual(default(Guid), category.Id);
		Assert.NotEqual(default(DateTime), category.CreatedAt);
		Assert.True(category.CreatedAt > datetimeBefore);
		Assert.True(category.CreatedAt < datetimeAfter);
		Assert.Equal(isActive, category.IsActive);
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	public void InstantiateErrorWhenNameIsEmpty(string? name)
	{
		Action action = () => new DomainEntity.Category(name!, "Category Description");

		var exception = Assert.Throws<EntityValidationException>(action);
		Assert.Equal("Name should not be empty or null", exception.Message);
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsNull()
	{
		Action action = () => new DomainEntity.Category("Category Name", null!);

		var exception = Assert.Throws<EntityValidationException>(action);
		Assert.Equal("Description should not be empty or null", exception.Message);
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("1")]
	[InlineData("12")]
	[InlineData("a")]
	[InlineData("ga")]
	public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
	{
		Action action = () => new DomainEntity.Category(invalidName, "Category Ok Description");

		var exception = Assert.Throws<EntityValidationException>(action);
		Assert.Equal("Name should be at least 3 characters long", exception.Message);
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenNameIsGreaterThan255Characters()
	{
		var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(x => "a").ToArray());
		Action action = () => new DomainEntity.Category(invalidName, "Category Ok Description");

		var exception = Assert.Throws<EntityValidationException>(action);
		Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
	{
		var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(x => "a").ToArray());
		Action action = () => new DomainEntity.Category("Category Name", invalidDescription );

		var exception = Assert.Throws<EntityValidationException>(action);
		Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
	}
}
