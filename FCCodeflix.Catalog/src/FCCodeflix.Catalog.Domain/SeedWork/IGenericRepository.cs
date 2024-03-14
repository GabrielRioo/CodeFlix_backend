using FCCodeflix.Catalog.Domain.Entity;

namespace FCCodeflix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAggregate> : IRepository
{
	public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);

}
