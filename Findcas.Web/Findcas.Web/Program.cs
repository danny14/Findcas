using Findcas.Application;
using Findcas.Application.Interfaces;
using Findcas.Infrastructure.Data;
using Findcas.Infrastructure.Services;
using Findcas.Web.Client.Pages;
using Findcas.Web.Client.Services;
using Findcas.Web.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 10 * 1024 * 1024; 
    })
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Usamos el operador ! para prometerle al compilador que la llave existirá en el appsettings
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "FindcasSuperSecretKeyParaProtegerLosTokens123456789!";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// CONTROLADOR
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHttpContextAccessor();

// SERVICIOS
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IImageService, CloudinaryService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<IReservationService, ReservationService>();


Action<IServiceProvider, HttpClient> configureHttpClient = (sp, client) =>
{
    var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var request = contextAccessor.HttpContext?.Request;
    var baseUri = request != null ? $"{request.Scheme}://{request.Host}/" : "http://localhost:5013/";

    client.BaseAddress = new Uri(baseUri);
};

builder.Services.AddHttpClient<AuthHttpClient>(configureHttpClient);
builder.Services.AddHttpClient<ReservationHttpClient>(configureHttpClient);

//builder.Services.AddHttpClient<AuthHttpClient>(client =>
//{
//    client.BaseAddress = new Uri("http://localhost:5013/");
//});

builder.Services.AddLocalization();

var supportedCultures = new[] { "es", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCascadingAuthenticationState();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseRequestLocalization(localizationOptions);


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Findcas.Web.Client._Imports).Assembly);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
