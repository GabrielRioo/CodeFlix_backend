using Xunit;
using DomainEntity = FCCodeflix.Catalog.Domain.Entity;

namespace FCCodeflix.Catalog.UnitTests.Domain.Entity.Category;
public class CategoryTestFixture
{
	public DomainEntity.Category GetValidCategory() => new ("Category Name", "Category Description");
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> { }