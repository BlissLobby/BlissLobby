using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<UpdateUserCommand, List<IdentityResult>>
{
    public async Task<List<IdentityResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return [IdentityResult.Failed(new IdentityError { Description = $"User with ID '{request.Id}' not found." })];
        }

        List<IdentityResult> results = [];

        // update username
        if (!string.IsNullOrEmpty(request.Username) && request.Username != user.UserName)
        {
            // check if new username exists to update the username
            var existingUser = await userManager.FindByNameAsync(request.Username);
            if (existingUser != null && existingUser.Id != request.Id)
            {
                results.Add(IdentityResult.Failed(new IdentityError { Description = $"Username '{request.Username}' is already taken." }));
            }
            else
            {
                var res = await userManager.SetUserNameAsync(user, request.Username);
                results.Add(res);
            }
        }

        // update display name
        if (!string.IsNullOrEmpty(request.DisplayName) && request.DisplayName != user.DisplayName)
        {
            user.DisplayName = request.DisplayName;
            var res = await userManager.UpdateAsync(user);
            results.Add(res);
        }

        // update email
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != request.Id)
            {
                results.Add(IdentityResult.Failed(new IdentityError { Description = $"Email '{request.Email}' is already taken." }));
            }
            else
            {
                var res = await userManager.SetEmailAsync(user, request.Email);
                results.Add(res);
            }
        }

        // update phone number
        if (!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
        {
            var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.Id != request.Id, cancellationToken: cancellationToken);
            if (existingUser != null)
            {
                results.Add(IdentityResult.Failed(new IdentityError { Description = $"Phone number '{request.PhoneNumber}' is already taken." }));
            }
            else
            {
                var res = await userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                results.Add(res);
            }
        }

        // update password
        if (!string.IsNullOrEmpty(request.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var res = await userManager.ResetPasswordAsync(user, token, request.Password);
            results.Add(res);
        }

        // update user role
        if (!string.IsNullOrEmpty(request.UserRole))
        {
            var currentRoles = await userManager.GetRolesAsync(user) ?? [];
            if (!currentRoles.Contains(request.UserRole))
            {
                var res = await userManager.RemoveFromRolesAsync(user, currentRoles);
                results.Add(res);
                res = await userManager.AddToRoleAsync(user, request.UserRole);
                results.Add(res);
            }
        }

        // update building id
        if (user.BuildingId != request.BuildingId)
        {
            user.BuildingId = string.IsNullOrWhiteSpace(request.BuildingId) ? null : request.BuildingId;
            var res = await userManager.UpdateAsync(user);
            results.Add(res);
        }

        // update two factor enabled
        if (user.TwoFactorEnabled != request.IsTwoFactorEnabled)
        {
            user.TwoFactorEnabled = request.IsTwoFactorEnabled;
            var res = await userManager.UpdateAsync(user);
            results.Add(res);
        }

        // update email confirmed
        if (user.EmailConfirmed != request.IsEmailConfirmed)
        {
            user.EmailConfirmed = request.IsEmailConfirmed;
            var res = await userManager.UpdateAsync(user);
            results.Add(res);
        }

        // update phone confirmed
        if (user.PhoneNumberConfirmed != request.IsPhoneConfirmed)
        {
            user.PhoneNumberConfirmed = request.IsPhoneConfirmed;
            var res = await userManager.UpdateAsync(user);
            results.Add(res);
        }

        return results;
    }
}