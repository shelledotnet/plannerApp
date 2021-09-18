using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (await _localStorageService.ContainKeyAsync("access_token"))
            {
                //the user is login
                var tokenAsString = await _localStorageService.GetItemAsStringAsync("access_token");//this get the accesstoken
                var tokenHandler = new JwtSecurityTokenHandler(); //this derypt the access token for us to ge the cliams
                var token = tokenHandler.ReadJwtToken(tokenAsString);//this read the token
                var identity = new ClaimsIdentity(token.Claims,"Bearer");
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(authState));  //its an event to notify component that state as changed for login user

                return authState;
            }
            return new AuthenticationState(new ClaimsPrincipal());//Empty claims principal means no identity and user is logged in
        }
    }
}
