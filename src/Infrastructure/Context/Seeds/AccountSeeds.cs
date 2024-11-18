using SampleTest.Domain.Models;

namespace SampleTest.Infrastructure.Context.Seeds
{
    public static class AccountSeeds
    {
        public static List<AccountModel> NewSeeds()
        {
            return new List<AccountModel>
            {
                new() {
                    Id = 1,
                    ClientId = 1,
                    Agency = "0001",
                    Balance = 0,
                    Overdraft = 10000*0.3D, //30% do Salário para cheque especial.
                    CreateUserId = null,
                    CreatedAt = new DateTime(2024,11,15,19,0,0,0),
                    IsDeleted = false,
                }
            };
        }
    }
}
