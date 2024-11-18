
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleTest.Application.Shared;
using SampleTest.Domain.Interfaces;
using SampleTest.Domain.Models;
using SampleTest.Resources;
using SampleTest.Resources.Exceptions;
using SampleTest.Resources.Features;
using SampleTest.Resources.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleTest.Application.Features.Authentication.Login;

public class LoginCommandHandler : ICommandQueryHandler<LoginCommand, string>
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IConfiguration configuration,
                               IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new Result<string>();

        var user = await _userRepository.GetByUsername(request.UserName) ?? throw new NotFoundException(Messages.NotFound);

        var passwordHash = FunctionsUtil.GenerateSHA256Hash(request.Password, _configuration);

        if (user != null && user.Password != passwordHash)
            throw new ForbiddenException(Messages.InvalidPassword);

        var token = string.Empty;
        await Task.Run(() =>
        {
            token = GenerateToken(user);
        }, cancellationToken);

        result.AddValue(token);
        result.OK();
        return result;
    }


    private string GenerateToken(UserModel user)
    {
        var secKey = _configuration.GetValue<string>("ApiSecretKey");
        var appOrigin = _configuration.GetValue<string>("ApplicationOrigin");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
        {
            new("user.id", user.Id.ToString()),
            new("user.name", user.Username),
            new("application.origin", appOrigin)
        }),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
