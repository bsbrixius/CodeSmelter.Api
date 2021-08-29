using System.Threading.Tasks;

namespace CodeSmelter.Api.Repository.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
