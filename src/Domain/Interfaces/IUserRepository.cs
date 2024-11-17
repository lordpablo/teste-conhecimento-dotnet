using SampleTest.Domain.Models;

namespace SampleTest.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        Task<UserModel> GetByUsername(string username);
    }
}
