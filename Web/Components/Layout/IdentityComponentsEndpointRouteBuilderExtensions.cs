using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Services;

namespace Web.Components.Layout
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var accountGroup = endpoints.MapGroup("/Account");

            accountGroup.MapPost("/Logout", async (
                ClaimsPrincipal user,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                [FromServices] StatusMessageService statusMessageService,
                [FromForm] string returnUrl) =>
            {
                await signInManager.SignOutAsync();
                statusMessageService.SetStatusMessage("You have been logged out.", Radzen.AlertStyle.Info);
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            return accountGroup;
        }
    }
}
