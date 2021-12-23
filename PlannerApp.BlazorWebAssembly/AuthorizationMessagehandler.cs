using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly
{
    //i aslo need to reegister the messagehandler
    public class AuthorizationMessagehandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;  //u need to register this service in the  service container
        private readonly IJSRuntime _jSRuntime;

        public AuthorizationMessagehandler(ILocalStorageService localStorageService, IJSRuntime jSRuntime)
        {
            this._localStorageService = localStorageService;
            this._jSRuntime = jSRuntime;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (await _localStorageService.ContainKeyAsync("access_token"))
            {
                //if the localstorage contain acces token then store it on the header
                var token = await _localStorageService.GetItemAsStringAsync("access_token");// this store the key value pair of the acces_token
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
                
            }
            //Console.WriteLine($"Authorization message handler called: {request.Headers.Authorization}");
            await _jSRuntime.InvokeVoidAsync("console.log", "Header-parameter", new { request.Headers.Authorization });
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
