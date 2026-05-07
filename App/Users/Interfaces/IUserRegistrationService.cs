using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Interfaces;

public interface IUserRegistrationService
{
    Task<IdentityResult> RegisterAsync(ApplicationUser user, string password, string role,
        bool isEmailConfirmed, bool isPhoneConfirmed, CancellationToken cancellationToken = default);
}
