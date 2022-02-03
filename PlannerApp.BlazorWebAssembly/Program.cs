using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PlannerApp.BlazorWebAssembly;
using PlannerApp.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

//this is how to refere to the header tag after means anything within header tag like title tag in html pages
builder.RootComponents.Add<HeadOutlet>("head::after");//HeadOutlet enable <PageTitle> in the pages

//the system registered httpclient that has base address below is actually pointing to the project address which in our case we are consuming api from a vendor so we dont need it
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


//using Microsoft.Extensions.http
builder.Services.AddHttpClient("PlannerApp.Api", client =>
 {
     client.BaseAddress = new Uri("https://plannerapp-api.azurewebsites.net/");

 }).AddHttpMessageHandler<AuthorizationMessagehandler>();
//the esence of the message handler above is to force the request(create ,read,update,delete)andresponse to go through the message handler function sendAsync() in other to set access token on the header of every request

//this register httpclient service whose name is PlannerApp.Api
builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("PlannerApp.Api"));

builder.Services.AddTransient<AuthorizationMessagehandler>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();//this will allow us to use Authorize attribute
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddHttpClientService();
builder.Services.AddMudServices();
await builder.Build().RunAsync();
