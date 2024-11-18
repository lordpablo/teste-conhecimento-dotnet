using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SampleTest.Application.Mapper;
using System.Diagnostics.CodeAnalysis;

namespace SampleTest.Application.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            services.AddSingleton(p => new MapperConfiguration(c =>
            {
                c.AddProfile(new ClientMapper());

            }).CreateMapper());
        }
    }
}
