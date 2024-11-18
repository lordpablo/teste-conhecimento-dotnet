using SampleTest.Domain.Models;

namespace SampleTest.Infrastructure.Context.Seeds
{
    public static class ClientSeeds
    {
        public static List<ClientModel> NewSeeds()
        {
            return new List<ClientModel>
            {
                new() {
                    Id = 1,
                    CPF = "57289157010", //FAKE CPF
                    Email = "pablohmsfa@gmail.com",
                    BirthDate = new DateTime(1990,3,3),
                    Gender = Domain.Enums.GenderEnum.Male,
                    Name = "Pablo Alvim",
                    CreateUserId = null,
                    CreatedAt = new DateTime(2024,11,15,19,0,0,0),
                    IsDeleted = false,
                    MonthRemuneration = 10000,
                }
            };
        }
    }
}
