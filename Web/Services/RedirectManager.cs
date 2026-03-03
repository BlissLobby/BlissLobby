using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Web.Services
{
    internal sealed class RedirectManager(NavigationManager navigationManager, StatusMessageService statusMessage)
    {
        public void RedirectTo(string? uri, bool forceLoad = false)
        {
            uri ??= "";

            // Prevent open redirects.
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = navigationManager.ToBaseRelativePath(uri);
            }

            navigationManager.NavigateTo(uri, forceLoad: forceLoad);
        }

        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters, bool forceLoad = false)
        {
            var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
            RedirectTo(newUri, forceLoad);
        }

        public void RedirectToWithStatus(string uri, string message, Radzen.AlertStyle severity = Radzen.AlertStyle.Info)
        {
            statusMessage.SetStatusMessage(message, severity);
            RedirectTo(uri, true);
        }

        private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

        public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

        public void RedirectToCurrentPageWithStatus(string message, Radzen.AlertStyle severity = Radzen.AlertStyle.Info)
            => RedirectToWithStatus(CurrentPath, message, severity);

        public void RedirectToInvalidUser(UserManager<ApplicationUser> userManager, HttpContext context)
            => RedirectToWithStatus("Account/InvalidUser", $"Unable to load user with ID '{userManager.GetUserId(context.User)}'.", Radzen.AlertStyle.Danger);
    }
}
