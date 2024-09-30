using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using MBS_AUTHORIZATION.Domain.Abstractions.Repositories;
using MBS_AUTHORIZATION.Domain.Entities;

namespace MBS_AUTHORIZATION.Application.UserCases.Commands.Identity;

public class RegisterCommandHandler : ICommandHandler<Command.RegisterCommand>
{
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public RegisterCommandHandler(IRepositoryBase<User, Guid> userRepository, IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result> Handle(Command.RegisterCommand request, CancellationToken cancellationToken)
    {
        var userExisted =
            await _userRepository.FindSingleAsync(x =>
                x.Email.Equals(request.Email), cancellationToken);
        
        if (userExisted is not null)
        {
            throw new Exception("User Existed !");
        }

        var hashingPassword = _passwordHasherService.HashPassword(request.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FirstName + request.LastName,
            Role = request.Role,
            
            Password = hashingPassword,
           
            Points = 100,
            Status = 0
        };
        
        _userRepository.Add(user);

        return Result.Success(user);
    }
}