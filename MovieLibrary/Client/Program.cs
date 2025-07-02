using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MovieLibrary.Client;
using MovieLibrary.Client.Interface;
using MovieLibrary.Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("MovieLibrary.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MovieLibrary.ServerAPI"));

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IActorService, ActorService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://movielibrary.onmicrosoft.com/3655e4d8-bfea-4cd4-a4e3-307bc34b96c6/Api.ReadWrite");
});

await builder.Build().RunAsync();
