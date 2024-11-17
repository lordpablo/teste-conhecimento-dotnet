using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleTest.Application.Features.Authentication.Login;
using SampleTeste.API.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace SampleTeste.API.Controllers
{
    [SwaggerTag("SampleModel Service")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation("Receber token", "Receba um token para entrar no sistema com entradas de nome de usuário e senha")]
        [SwaggerResponse(StatusCodes.Status200OK, "A autenticação foi bem-sucedida", typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado", typeof(void))]
        public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return ApiResult(result);
        }
    }
}
