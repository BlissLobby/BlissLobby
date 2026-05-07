using App.Users.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRegistrationService registrationService) : IRequestHandler<CreateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            UserName = request.Username,
            DisplayName = request.DisplayName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            TwoFactorEnabled = request.IsTwoFactorEnabled,
            BuildingId = request.BuildingId
        };

        return await registrationService.RegisterAsync(user, request.Password, request.UserRole,
            request.IsEmailConfirmed, request.IsPhoneConfirmed, cancellationToken);
    }
}