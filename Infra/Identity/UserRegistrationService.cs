using App.Common.Interfaces;
using App.Users.Commands.CreateUser;
using App.Users.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Infra.Identity;

public class UserRegistrationService(UserManager<ApplicationUser> userManager, ILogger<CreateUserCommandHandler> logger, IEmailSender emailSender, ISmsSender smsSender) : IUserRegistrationService
{
    public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password, string role, bool isEmailConfirmed, bool isPhoneConfirmed, CancellationToken cancellationToken = default)
    {
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            logger.LogInformation($"Created new account for {user.UserName} with id {user.Id}");

            // add user to role
            await userManager.AddToRoleAsync(user, role);
            logger.LogInformation($"{role} role assigned to new user {user.UserName} with id {user.Id}");

            // send confirmation email to user if required
            var emailVerifyCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
            if (isEmailConfirmed)
            {
                IdentityResult emaiVerifiedResult = await userManager.ConfirmEmailAsync(user, emailVerifyCode);
                if (emaiVerifiedResult.Succeeded)
                {
                    logger.LogInformation($"Email verified for new user {user.UserName} with id {user.Id} and email {user.Email}");
                }
                else
                {
                    logger.LogError($"Email verify failed for {user.UserName} with id {user.Id} and email {user.Email} due to errors {emaiVerifiedResult.Errors}");
                }
            }
            else
            {
                //var callbackUrl = QueryHelpers.AddQueryString(request.EmailVerifyBaseUrl, queryString: new Dictionary<string, string?>() {
                //                                                                            { "emailVerifyCode", emailVerifyCode }, { "userId", user.Id }
                //                                                                        });
                try
                {
                    // TODO: explore using background job to send email instead of doing it in the request pipeline. can move email sending logic to a domain event handler that listens to user created event
                    // TODO: can keep application name as config driven
                    await emailSender.SendEmailAsync(
                    email: user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
                    subject: "Please confirm your email for BlissLobby app",
                    htmlMessage: $"Please confirm your account of BlissLobby app by using the code {emailVerifyCode} in the app.");

                    logger.LogInformation($"Email address Confirmation mail sent to {user.UserName}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while sending email confirmation to {UserName}", user.UserName);
                }
            }

            // verify user phone if we dont want users to do it themselves
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                // verify phone number
                string phoneVerifyCode = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                if (isPhoneConfirmed)
                {
                    IdentityResult phoneVeifyResult = await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, phoneVerifyCode);
                    logger.LogInformation($"Phone verified new user {user.UserName} with id {user.Id} and phone {user.PhoneNumber} = {phoneVeifyResult.Succeeded}");
                }
                else
                {
                    //var callbackUrl = QueryHelpers.AddQueryString(request.PhoneVerifyBaseUrl,
                    //                                                queryString: new Dictionary<string, string?>() {
                    //                                                                    { "emailVerifyCode", phoneVerifyCode },
                    //                                                                    { "userId", user.Id },
                    //                                                                    { "phoneNumber", user.PhoneNumber }
                    //                                                                });
                    try
                    {
                        await smsSender.SendSmsAsync(user.PhoneNumber, $"Please confirm your phone number for BlissLobby app by using the code {phoneVerifyCode} in the app.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error occurred while sending phone confirmation to {UserName}", user.UserName);
                    }
                }
            }

        }
        else
        {
            logger.LogError($"Failed to create account for {user.UserName} with id {user.Id} due to errors {result.Errors}");
        }
        return result;
    }
}