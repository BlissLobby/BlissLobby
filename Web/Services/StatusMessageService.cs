namespace Web.Services;

public class StatusMessageService(IHttpContextAccessor contextAccessor)
{
    public const string StatusCookieMessage = "StatusMessage";
    public const string StatusCookieSeverity = "StatusSeverity";

    private static readonly CookieBuilder StatusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5)
    };

    public void SetStatusMessage(string message, Radzen.AlertStyle severity)
    {
        var context = contextAccessor.HttpContext;
        context?.Response.Cookies.Append(StatusCookieMessage, message, StatusCookieBuilder.Build(context));
        context?.Response.Cookies.Append(StatusCookieSeverity, severity.ToString(), StatusCookieBuilder.Build(context));
    }
}
