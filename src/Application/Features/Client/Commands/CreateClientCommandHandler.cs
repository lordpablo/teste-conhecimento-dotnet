using AutoMapper;
using FluentValidation;
using SampleTest.Application.Shared;
using SampleTest.Domain.Interfaces;
using SampleTest.Domain.Models;
using SampleTest.Resources;
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
        private readonly IValidator<CreateClientCommand> _clientCreateValidator;

        public CreateClientCommandHandler(
            IAuthenticateUser authenticateUser,
            IClientRepository clientRepository,
            IAccountRepository accountRepository,
            IValidator<CreateClientCommand> clientCreateValidator,
            IMapper mapper
            ) : base(authenticateUser)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _clientCreateValidator = clientCreateValidator;
        }
        public async Task<Result<string>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var errorsDict = new Dictionary<string, string[]>();

            await _clientCreateValidator.ValidateAndThrowAsync(request, cancellationToken);
            var businessValidations = await ValidateCreateAsync(request);

            if (businessValidations != null && businessValidations.Count > 0)
            {
                errorsDict.Add(Messages.BadRequest, businessValidations.ToArray());
                throw new BadRequestException(Messages.BadRequest, errorsDict);
            }

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

        public async Task<List<string>> ValidateCreateAsync(CreateClientCommand request)
        {
            var errors = new List<string>();
            var client = await _clientRepository.FindFirstOrDefaultAsync(x => x.CPF == request.CPF || x.Email == request.Email);
            if (client is not null && client.CPF == request.CPF)
                errors.Add("CPF já cadastrado na base de dados.");

            if (client is not null && client.Email == request.Email)
                errors.Add("Email já cadastrado na base de dados.");

            return errors;

        }
    }
}
