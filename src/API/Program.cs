using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleTest.Application.Configuration;
using SampleTest.Infrastructure.Context;
using SampleTest.Resources.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("cnnBaseString").ToString();
var key = builder.Configuration["ApiSecretKey"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<DefaultContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<DbContext, DefaultContext>();

// Base configurantion
ConfigurationHelper.ConfigureJwt(builder.Services, key);
ConfigurationHelper.ConfigureSwaggerGen(builder.Services, "SampleTestAPI", "v1");
ConfigurationHelper.ConfigureScrutor(builder.Services);
ConfigurationHelper.ConfigureAuthenticateUser(builder.Services);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddAutoMapperConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("pt-BR"));
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
