namespace App.Users.Queries.GetUserForDisplay;

public class UserDto
{
    public required string Id { get; set; }

    public required string UserName { get; set; }

    public required string DisplayName { get; set; }

    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }

    public required string Phone { get; set; }
    public required bool PhoneConfirmed { get; set; }

    public required bool TwoFactorEnabled { get; set; }

    public string? BuildingId { get; set; }

    public List<string> Roles { get; set; } = [];
}