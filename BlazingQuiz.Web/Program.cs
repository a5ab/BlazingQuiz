using BlazingQuiz.Web;
using BlazingQuiz.Web.Apis;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//injject the refit client configration
ConfigrationRefit(builder.Services);

await builder.Build().RunAsync();

static void ConfigrationRefit(IServiceCollection services)
{
    string? ApiBaseUrl = "https://localhost:7063";
    services.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(ApiBaseUrl));

};