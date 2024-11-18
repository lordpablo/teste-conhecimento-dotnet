using AutoMapper;
using SampleTest.Application.Features.Client.Commands;
using SampleTest.Domain.Models;

namespace SampleTest.Application.Mapper
{
    public partial class ClientMapper : Profile
    {
        public ClientMapper()
        {
            CreateMap<ClientModel, CreateClientCommand>().ReverseMap();
        }
    }
}
