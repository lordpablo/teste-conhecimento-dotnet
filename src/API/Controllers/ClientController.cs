using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleTest.Application.Features.Client.Commands;
using SampleTeste.API.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace SampleTeste.API.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly IMediator _mediator;
        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "Cliente cadastrado com sucesso", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Falha ao inserir o Cliente", typeof(void))]
        public async Task<IActionResult> Create(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return ApiResult(result);
        }
    }
}
