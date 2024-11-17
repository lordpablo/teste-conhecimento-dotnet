using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleTest.Domain.Interfaces;
using SampleTest.Domain.Models;
using SampleTest.Infrastructure.Repositories.Shared;

namespace SampleTest.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(DbContext context, IConfiguration configuration) : base(context, configuration) { }


        public async Task<UserModel> GetByUsername(string username)
        {
            return await _context.Set<UserModel>()
                .AsNoTracking()
                .Where(x => x.Username == username)
                .FirstAsync();
        }
    }
}
