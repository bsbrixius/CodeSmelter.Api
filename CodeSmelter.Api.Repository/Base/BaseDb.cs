using CodeSmelter.Api.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CodeSmelter.Api.Repository.Base
{
    public abstract class BaseDb : DbContext, IUnitOfWork
    {
        public BaseDb(DbContextOptions options) : base(options)
        {

        }
        public async Task<bool> CommitAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
