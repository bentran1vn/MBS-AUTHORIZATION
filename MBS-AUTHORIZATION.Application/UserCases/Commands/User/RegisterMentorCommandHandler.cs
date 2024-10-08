using MBS_AUTHORIZATION.Contract.Abstractions.Messages;
using MBS_AUTHORIZATION.Contract.Abstractions.Shared;
using MBS_AUTHORIZATION.Contract.Services.Users;
using MBS_AUTHORIZATION.Persistence.Repositories;

namespace MBS_AUTHORIZATION.Application.UserCases.Commands.User;

public class RegisterMentorCommandHandler : ICommandHandler<Command.RegisterMentor>
{
    public async Task<Result> Handle(Command.RegisterMentor request, CancellationToken cancellationToken)
    {
       // var U = await userRepository.FindSingleAsync(x => x.Id.Equals(request.MentorId), cancellationToken);
        //if (U is null)
        //{
        //    return Result.Failure(new Error("404", "Mentor Not Existed !"));
        //}
        throw new NotImplementedException();
    }
}
