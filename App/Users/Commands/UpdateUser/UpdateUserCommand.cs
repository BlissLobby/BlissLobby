using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<List<IdentityResult>>
{
    public string Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string UserRole { get; set; } = Roles.Resident;
    public string? BuildingId { get; set; }
    public bool IsTwoFactorEnabled { get; set; } = true;
    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsPhoneConfirmed { get; set; } = false;
}
