Add a new claim for email in identity framework through custom UserClaimsPrincipalFactory and registering it in the DI container. This allows you to map the email claim to the standard ClaimTypes.Email and access it in your razor components. Now both the username and email claims are available for use in your application.

* Create custom UserClaimsPrincipalFactory

```csharp
public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public CustomUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions optionsAccessor)
        : base(userManager, optionsAccessor)
    {}

    protected override async Task GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        
        // Map Username to the standard Name claim
        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? ""));
        
        // Map Email to a custom Email claim
        if (!string.IsNullOrEmpty(user.Email))
        {
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        }

        return identity;
    }
}
```

* Register the custom UserClaimsPrincipalFactory in the DI container
```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>(); // Registered here
```

* Use in razor components

```csharp
<p>Email: @context.User.FindFirst(ClaimTypes.Email)</p>
<p>Username: @context.User.Identity?.Name</p>
```