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
                    Username = "admin",
                    Password = "e84477f9b95117589a0c8142cab2211f3c421fab5b85f3393784bc7f254545bc", //admin
                }
            };
        }
    }
}
