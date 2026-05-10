using App.Common.Interfaces;
using App.Users.Commands.CreateUser;
using FluentValidation;
using Radzen;
using Web.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }
        builder.Services.AddScoped<StatusMessageService>();
        
        builder.Services.AddScoped<RedirectManager>();

        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Add services to the container.
        builder.Services.AddRazorComponents();
               //.AddInteractiveWebAssemblyComponents();
        builder.Services.AddRadzenComponents();

        builder.Services.AddValidatorsFromAssemblyContaining<IUser>();
    }
}
