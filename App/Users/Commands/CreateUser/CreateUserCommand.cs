using App.Common.Security;
using App.Users.Queries.GetUsersForDisplay;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Commands.CreateUser;

[Authorize(Roles = Roles.Administrator)]
public class CreateUserCommand : IRequest<IdentityResult>
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string UserRole { get; set; } = Roles.Resident;
    public string? BuildingId { get; set; }
    public bool IsTwoFactorEnabled { get; set; } = true;
    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsPhoneConfirmed { get; set; } = false;
}
