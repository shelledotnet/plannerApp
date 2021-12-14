using Microsoft.JSInterop;
using PlannerApp.Services.Exceptions;
using Blazored.LocalStorage;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components.Authorization;

namespace PlannerApp.Services
{

    //Always rember to register the service at the program.cs
    public class HttpAuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _JSRunTime;
       private readonly ILocalStorageService _Storage; //is use to store access token  and expiry date after the response from the server
        private AuthenticationStateProvider _AuthenticationStateProvider;  //tellus about login user
        public HttpAuthenticationService(HttpClient httpClient, IJSRuntime JSRunTime, ILocalStorageService storage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _JSRunTime = JSRunTime;
            _Storage = storage;
            _AuthenticationStateProvider = authenticationStateProvider;
        }
        public async Task<ApiResponse> RegisterUserAsync(RegisterRequest model)
        {
           var response= await _httpClient.PostAsJsonAsync("/api/v2/auth/register", model);
            model.Password = model.Password.Replace(model.Password.Substring(2), "****");
            model.ConfirmPassword = model.Password.Replace(model.ConfirmPassword.Substring(2), "****");
            if (response.IsSuccessStatusCode)
            {
               
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", model);
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result.IsSuccess, result.Message,response.StatusCode});


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", model);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { errorReponse.IsSuccess, errorReponse.Errors, errorReponse.Message , response.StatusCode});

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }
        public async Task<ApiResponse> LoginUserAsync(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v2/auth/Login", model);
            model.Password = model.Password.Replace(model.Password.Substring(2), "****");
          
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResult>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", model);
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result.IsSuccess, result.Message,result.Value, response.StatusCode });

                //store it in local storage
                await _Storage.SetItemAsStringAsync("access_token", result.Value.Token);
                await _Storage.SetItemAsync<DateTime>("expiry_date", result.Value.ExpiryDate);
                await _AuthenticationStateProvider.GetAuthenticationStateAsync();  //now the application knows about the user claims


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
               

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", model);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { errorReponse.IsSuccess, errorReponse.Errors, errorReponse.Message, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }
    }
}
