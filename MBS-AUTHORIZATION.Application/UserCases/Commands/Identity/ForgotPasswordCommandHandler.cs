using MBS_AUTHORIZATION.Application.Abstractions;
using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Identity;
using MBS_AUTHORIZATION.Domain.Abstractions.Repositories;
using MBS_AUTHORIZATION.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace MBS_AUTHORIZATION.Application.UserCases.Commands.Identity;

public class ForgotPasswordCommandHandler : ICommandHandler<Command.ForgotPasswordCommand>
{
    // private readonly IMailService _mailService;
    private readonly ICacheService _cacheService;
    private readonly IRepositoryBase<User, Guid> _userRepository;

    public ForgotPasswordCommandHandler(ICacheService cacheService, IRepositoryBase<User, Guid> userRepository)
    {
        // _mailService = mailService;
        _cacheService = cacheService;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user =
            await _userRepository.FindSingleAsync(x =>
                x.Email.Equals(request.Email), cancellationToken);
        
        if (user is null)
        {
            throw new Exception("User Not Existed !");
        }
        
        Random random = new Random();
        var randomNumber = random.Next(0, 100000).ToString("D5");
        
        var slidingExpiration = 30;
        var absoluteExpiration = 30;
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(absoluteExpiration));
        
        await _cacheService.SetAsync($"{nameof(Command.ForgotPasswordCommand)}-UserAccount:{user.Email}", randomNumber, options, cancellationToken);
        
        // await _mailService.SendMail(EmailExtensions.ForgotPasswordBody(randomNumber, $"{user.Firstname} {user.Lastname}", request.Email));
        
        return Result.Success("Send Mail Successfully !");
    }
}