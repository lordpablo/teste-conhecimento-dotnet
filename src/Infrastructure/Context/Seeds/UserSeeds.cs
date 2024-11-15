using SampleTest.Domain.Models;

namespace SampleTest.Infrastructure.Context.Seeds
{
    public static class UserSeeds
    {
        public static List<UserModel> NewSeeds()
        {
            return new List<UserModel>
            {
                new() {
                    Id = 1,
                    ClientId = 1,
                    Username = "admin",
                    Password = "admin",
                }
            };
        }
    }
}
