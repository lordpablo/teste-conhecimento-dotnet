using AutoMapper;
using SampleTest.Application.Shared;
using SampleTest.Domain.Interfaces;
using SampleTest.Domain.Models;
using SampleTest.Resources.Configuration.Interfaces;
using SampleTest.Resources.Exceptions;
using SampleTest.Resources.Features;

namespace SampleTest.Application.Features.Client.Commands
{
    public class CreateClientCommandHandler : BaseCommandHandler, ICommandQueryHandler<CreateClientCommand, string>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public CreateClientCommandHandler(
            IAuthenticateUser authenticateUser,
            IClientRepository clientRepository,
            IAccountRepository accountRepository,
            IMapper mapper
            ) : base(authenticateUser)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new Result<string>();
                var client = _mapper.Map<ClientModel>(request);
                client.Created(AuthenticateUser.GetUserId());

                await _clientRepository.SaveAsync(client);

                var account = new AccountModel
                {
                    Balance = 0,
                    ClientId = client.Id,
                    Agency = "0001",
                    Overdraft = Math.Round(client.MonthRemuneration * 0.3D, 2), //Apenas 2 casas decimais.
                };
                account.Created(AuthenticateUser.GetUserId());
                await _accountRepository.SaveAsync(account);

                result.AddValue($"Cliente criado com sucesso. Agencia: 0001 Conta: {account.Id}");
                result.OK();
                return result;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message, new Dictionary<string, string[]>());
            }

        }
    }
}
