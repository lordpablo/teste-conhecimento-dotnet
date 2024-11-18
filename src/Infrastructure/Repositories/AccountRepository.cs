using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleTest.Domain.Interfaces;
using SampleTest.Domain.Models;
using SampleTest.Infrastructure.Repositories.Shared;

namespace SampleTest.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<AccountModel>, IAccountRepository
    {
        public AccountRepository(DbContext context, IConfiguration configuration) : base(context, configuration) { }

    }
}
