using BlazingQuiz.Web;
using BlazingQuiz.Web.Apis;
using BlazingQuiz.Web.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<QuizAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<QuizAuthStateProvider>());
builder.Services.AddAuthorizationCore();


//injject the refit client configration
ConfigrationRefit(builder.Services);

await builder.Build().RunAsync();

static void ConfigrationRefit(IServiceCollection services)
{
    string? ApiBaseUrl = "https://localhost:7063";
    services.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(ApiBaseUrl));

};