using System.Text.RegularExpressions;

namespace App.Utils.FluentValidation;

public class UserDisplayNameValidator : AbstractValidator<string>
{
    // Regex breakdown:
    // ^[^\p{C}\p{So}\p{Sk}]+$ 
    // \p{C}  - Blocks Control characters (null bytes, hidden formatting)
    // \p{So} - Blocks Other Symbols (Emojis, checkmarks, web icons, wingdings)
    // \p{Sk} - Blocks Modifier Symbols (Standalone accent marks/skin tone modifiers used out of context)
    private static readonly Regex SafeNameRegex = new(@"^[^\p{C}\p{So}\p{Sk}]+$", RegexOptions.Compiled);

    public UserDisplayNameValidator()
    {
        RuleFor(name => name)
            // 1. Handle Null/Empty check
            .NotEmpty().WithMessage("Display name is required.")

            // 2. Length Constraints (Accommodates long international/compound names)
            .Length(3, 50).WithMessage("Display name must be between 1 and 50 characters.")

            // 3. Security & Spoofing Filter (Blocks Emojis, Control characters, and Symbols)
            .Matches(SafeNameRegex).WithMessage("Display name contains invalid symbols or emojis.")

            // 4. Practical Layout Constraints
            .Must(NotStartOrEndWithWhitespace).WithMessage("Display name cannot start or end with spaces.")
            .Must(NotContainConsecutiveSpaces).WithMessage("Display name cannot contain consecutive spaces.");
    }

    private bool NotStartOrEndWithWhitespace(string name)
    {
        if (string.IsNullOrEmpty(name)) return true;
        return name.Length == name.Trim().Length;
    }

    private bool NotContainConsecutiveSpaces(string name)
    {
        if (string.IsNullOrEmpty(name)) return true;
        return !name.Contains("  ");
    }
}