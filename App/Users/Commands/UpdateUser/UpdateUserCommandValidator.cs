using App.Common.Interfaces;
using App.Utils;
using App.Utils.FluentValidation;

namespace App.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateUserCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(20).WithMessage("Username must be at most 20 characters long");

        RuleFor(v => v.DisplayName)
            .SetValidator(new UserDisplayNameValidator());

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(v => v.PhoneNumber)
            .Must(FluentValidationUtils.BeAValidPhoneNumber).WithMessage("Invalid phone number format");

        RuleFor(v => v.UserRole)
            .Must(FluentValidationUtils.BeAValidRole).WithMessage("Invalid role provided");

        RuleFor(v => v.BuildingId)
            .MustAsync(BeAValidBuildingId).WithMessage("Building Id is not present in the database");

        RuleFor(v => v.Password)
                .Equal(v => v.ConfirmPassword).WithMessage("Passwords do not match");

    }

    private async Task<bool> BeAValidBuildingId(string? buildingId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(buildingId)) return true;

        return await _dbContext.Buildings.AsNoTracking()
                            .AnyAsync(b => b.Id == buildingId, cancellationToken);
    }
}