using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components
{
    public partial class LoginForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }  //tellus about login user

        [Inject]
        public IJSRuntime JSRunTime { get; set; }

        [Inject]
        public  ILocalStorageService Storage{ get; set; }//is use to store access token after the response from the server

        private LoginRequest _model = new LoginRequest();

        private bool _isBusy = false;

        private string _errorMessage = string.Empty;
        public async Task LoginUserAsync()
        {

            try
            {

                _isBusy = true;
                _errorMessage = string.Empty;

                var response =await HttpClient.PostAsJsonAsync("/api/v2/auth/Login",_model);//trnsalate an object to application json content

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResult>>();

                    //log the info
                    _model.Password = _model.Password.Replace(_model.Password.Substring(2), "****");
                    await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                    await JSRunTime.InvokeVoidAsync("console.log", "response", new { result.IsSuccess, result.Message, result.Value });
                //store it in local storage
                await Storage.SetItemAsStringAsync("access_token", result.Value.Token);
                await Storage.SetItemAsync<DateTime>("expiry_date", result.Value.ExpiryDate);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();  //now the application knows about the user claims

                Navigation.NavigateTo("/");
                }
            else
            {
                
                var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                    //log the info
                    _model.Password = _model.Password.Replace(_model.Password.Substring(2), "****");
                    await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                    await JSRunTime.InvokeVoidAsync("console.log","response", new { errorResult.IsSuccess,errorResult.Errors,errorResult.Message});
           
               _errorMessage = errorResult.Message;
            }
            
            
            
            
            _isBusy = false;

            }
            catch (Exception ex)
            {
                //log error
                //return StatusCode(500, ex.Message);
                // return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new employee records " + ex.Message);
                //log the info
                _model.Password = _model.Password.Replace(_model.Password.Substring(2), "****");
                await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                await JSRunTime.InvokeVoidAsync("console.log", "response", "Error retreiving data from the server " + ex.Message);
             
                _errorMessage = "Error fetching  employee record ";
            }
        }

    }
}
