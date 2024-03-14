using FCCodeflix.Catalog.Application.Interfaces;
using FCCodeflix.Catalog.Domain.Repository;
using DomainEntity = FCCodeflix.Catalog.Domain.Entity;

namespace FCCodeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
	{
		var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

		await _categoryRepository.Insert(category, cancellationToken);
		await _unitOfWork.Commit(cancellationToken);

		return new CreateCategoryOutput(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt) ;
	}
}
