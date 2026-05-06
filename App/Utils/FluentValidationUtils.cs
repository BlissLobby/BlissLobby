using Domain.Constants;
using System.Text.RegularExpressions;

namespace App.Utils;

public static class FluentValidationUtils
{
    /*
    Custom validation for phone number
    Reference: https://www.geeksforgeeks.org/dsa/validate-phone-numbers-with-country-code-extension-using-regular-expression/
    
    Rules for the valid phone numbers are: 
    * The numbers should start with a plus sign ( + )
    * It should be followed by Country code and National number.
    * It may contain white spaces or a hyphen ( - ).
    * The length of phone numbers may vary from 7 digits to 15 digits
        
    Examples - +91 (976) 006-4000 , +403 58 59594
    */
    public static bool BeAValidPhoneNumber(string? phoneNumber)
    {
        // Allow null phone number, use NotEmpty rule to enforce if needed
        if (phoneNumber == null) return true;

        // Regex to check valid phone number.
        string pattern = @"^[+]{1}(?:[0-9\-\(\)\/\.]\s?){6, 15}[0-9]{1}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }

    // custom rule to check the role
    public static bool BeAValidRole(string role)
    {
        return Roles.GetRoles().Contains(role);
    }
}
