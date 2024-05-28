using Azure.Messaging.ServiceBus;
using BlazorApp.Client.Pages;
using BlazorApp.Components;
using BlazorApp.Components.Account;
using BlazorApp.Data;
using BlazorApp.Hubs;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddScoped<SignOutSessionStateManager>();


builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	options.User.RequireUniqueEmail = true;
	options.Password.RequiredLength = 8;
})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(x =>
{
	x.Cookie.HttpOnly = true;
	x.LoginPath = "/login";
	x.LogoutPath = "/signout";
	x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
	x.SlidingExpiration = true;
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<CourseService>();

var serviceBusConnectionString = builder.Configuration.GetConnectionString("ServiceBusConnection") ?? throw new InvalidOperationException("Connection string 'ServiceBusConnection' not found.");
builder.Services.AddSingleton(sp => new ServiceBusClient(serviceBusConnectionString));
builder.Services.AddScoped<MessageService>();
builder.Services.AddSignalR();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting(); 

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapHub<ChatHub>("/chathub"); 


app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);



app.Run();
