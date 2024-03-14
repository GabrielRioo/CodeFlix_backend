﻿using FCCodeflix.Catalog.Application.Interfaces;
using FCCodeflix.Catalog.Domain.Entity;
using FCCodeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = FCCodeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FCCodeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
	[Fact(DisplayName = nameof(CreateCategory))]
	[Trait("Application", "CreateCategory - Use Cases")]
	public async void CreateCategory()
	{
		var repositoryMock = new Mock<ICategoryRepository>();
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

		var input = new UseCases.CreateCategoryInput("Category Name", "Category Description", true);

		var output = await useCase.Handle(input, CancellationToken.None);

		repositoryMock.Verify(
			repository => repository.Insert(
				It.IsAny<Category>(), 
				It.IsAny<CancellationToken>()
			),
			Times.Once
		);

		unitOfWorkMock.Verify(
			wow => wow.Commit(It.IsAny<CancellationToken>()),
			Times.Once
		);

		output.Should().NotBeNull();
		output.Name.Should().Be("Category Name");
		output.Description.Should().Be("Category Description");
		output.IsActive.Should().BeTrue();
		output.Id.Should().NotBeEmpty();
		output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
	}
}
