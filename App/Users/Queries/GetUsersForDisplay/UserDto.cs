namespace App.Users.Queries.GetUsersForDisplay;

public class UserDto
{
    public required string Id { get; set; }

    public required string UserName { get; set; }

    public required string DisplayName { get; set; }

    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }

    public List<string> Roles { get; set; } = [];
}