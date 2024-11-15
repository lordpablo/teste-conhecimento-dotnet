using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SampleTest.Resources.Configuration.Impl;
using SampleTest.Resources.Configuration.Interfaces;
using Scrutor;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace SampleTest.Resources.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationHelper
    {
        [ExcludeFromCodeCoverage]
        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();
        }
        [ExcludeFromCodeCoverage]
        public static void ConfigureSwaggerGen(IServiceCollection services, string apiName, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"Sample Test - {apiName}",
                    Version = version,
                    Description = $"Sample Test {apiName}",
                    Contact = new OpenApiContact
                    {
                        Name = "Sample Test - Information System",
                        Email = "pablohmsfa@gmail.com"
                    }
                });
                c.ResolveConflictingActions(apiDesc => apiDesc.First());

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Add o JWT token com o Texto: 'Bearer '",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });
        }

        /// <summary>
        /// This methoda Configures UserAuth
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAuthenticateUser(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticateUser, AuthenticateUser>();
        }

        /// <summary>
        /// This method configure inject of dependences automaticly, by scopedNameSpaces, ByDefault: AlvimSolutions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="scopedNamespaces"></param>
        [ExcludeFromCodeCoverage]
        public static void ConfigureScrutor(IServiceCollection services)
        {
            var scopedNameSpaces = new string[] { "SampleTest" };
            services.Scan(
                scan => scan
                .FromApplicationDependencies()
                    .AddClasses(classes => classes.InNamespaces(scopedNameSpaces))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());
        }
        /// <summary>
        /// This method configureJwt on application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="secret"></param>
        [ExcludeFromCodeCoverage]
        public static void ConfigureJwt(IServiceCollection services, string secret)
        {
            var keyByte = Encoding.Unicode.GetBytes(secret);
            var key64 = Convert.ToBase64String(keyByte);

            services
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "DefaultSchemeAuthentication";
                    options.DefaultChallengeScheme = "DefaultSchemeAuthentication";
                })
                .AddJwtBearer("DefaultSchemeAuthentication", x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key64)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        LifetimeValidator = LifetimeValidator,
                        ClockSkew = TimeSpan.Zero
                    };
                    x.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = _ => Task.CompletedTask
                    };
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="token"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                var result = expires > DateTime.UtcNow;

                return result;
            }
            return false;
        }
    }
}
