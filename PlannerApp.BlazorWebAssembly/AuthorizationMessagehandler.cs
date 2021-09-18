using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly
{
    public class AuthorizationMessagehandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthorizationMessagehandler(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (await _localStorageService.ContainKeyAsync("access_token"))
            {
                var token = await _localStorageService.GetItemAsStringAsync("access_token");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);

            }
            //Console.WriteLine("Authorization message handler called");
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
