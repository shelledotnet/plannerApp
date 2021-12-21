using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PlannerApp.Services;
using PlannerApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //the system registered httpclient that has base address below is actually pointing to the project address which in our case we are consuming api from a vendor so we dont need it
            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            
            //using Microsoft.Extensions.http
            builder.Services.AddHttpClient("PlannerApp.Api",client=> 
            {
                client.BaseAddress = new Uri("https://plannerapp-api.azurewebsites.net/" );
            
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
        }
    }
}
