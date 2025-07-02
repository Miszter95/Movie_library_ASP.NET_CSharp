using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MovieLibrary.Server.Data;
using MovieLibrary.Server.GraphQL;
using MovieLibrary.Server.Interfaces;
using MovieLibrary.Server.Services;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddPooledDbContextFactory<AppDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<AppDataContext>(DbContextKind.Pooled)
    .AddQueryType<MovieQueryResolver>()
    .AddQueryType<ActorQueryResolver>();

//builder.Services.AddAuthorization(options =>
//options.AddPolicy("Admin", policy =>
//policy.RequireClaim(
//"http://schemas.microsoft.com/identity/claims/objectidentifier"
//, "feae5ebd-c5a4-4fd8-b004-fed319793df8"
//, "7bfb30d8-01ac-4b2f-bfce-0e103f664cae"))
//);

builder.Services.AddOpenApiDocument(document =>
{
    document.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.OAuth2,
        Description = "B2C authentication",
        Flow = OpenApiOAuth2Flow.Implicit,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                Scopes = new Dictionary<string, string>
                        {
                            { "https://movielibrary.onmicrosoft.com/3655e4d8-bfea-4cd4-a4e3-307bc34b96c6/Api.ReadWrite", "Access the api as the signed-in user" },
                        },
                AuthorizationUrl = "https://movielibrary.b2clogin.com/movielibrary.onmicrosoft.com/B2C_1_signupsignin1/oauth2/v2.0/authorize",
                TokenUrl = "https://movielibrary.b2clogin.com/movielibrary.onmicrosoft.com/B2C_1_signupsignin1/oauth2/v2.0/token"
            },
        }
    });

    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseOpenApi();
    app.UseSwaggerUi3(settings =>
    {
        settings.OAuth2Client = new OAuth2ClientSettings
        {
            ClientId = "9b52ff54-1b10-4400-bd32-49a69932f38c",
            AppName = "MovieLibrary Blazor Client AAD B2C"
        };
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });

app.Run();
