using DataAccess.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Server;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
   builder.Configuration.GetConnectionString("DefaultConnection")
));

// Registering AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSession();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddIdentityLayer(builder.Configuration);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", options =>
    {
        options.AccessDeniedPath = "/Errors/Error403";
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5443";

        options.ClientId = "customer";
        options.ClientSecret = "customer";
        options.ResponseType = "code";
        options.UsePkce = true;

        options.Scope.Add("profile");
        options.Scope.Add("offline_access");

        options.Scope.Add("roles");
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.TokenValidationParameters.RoleClaimType = "role";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    })
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
