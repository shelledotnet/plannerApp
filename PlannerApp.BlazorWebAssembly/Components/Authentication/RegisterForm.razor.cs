﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Authentication
{
    public partial class RegisterForm : ComponentBase
    {
        #region Depency injection service container which u have to register at the Service in Program.cs
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        #endregion

        #region Variable for the form
        private readonly RegisterRequest _model = new();

        private bool _isBusy = false;

        private string _errorMessage = string.Empty;
        #endregion
        public async Task RegisterUserAsync()
        {

            try
            {

                _isBusy = true;
                _errorMessage = string.Empty;

                #region replace with a Iservice
                //var response = await HttpClient.PostAsJsonAsync("/api/v2/auth/Login", _model);//  PostAsJsonAsync trnsalate an object in this case _model to application json content b4 posting the request
                //_model.Password = _model.Password.Replace(_model.Password.Substring(2), "****");
                //if (response.IsSuccessStatusCode)
                //{
                //    var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResult>>();

                //    log the info
                //    await JSRunTime.InvokeVoidAsync("console.log", "raw", response);
                //    await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                //    await JSRunTime.InvokeVoidAsync("console.log", "response", new { result.IsSuccess, result.Message, result.Value });
                //    store it in local storage
                //    await Storage.SetItemAsStringAsync("access_token", result.Value.Token);
                //    await Storage.SetItemAsync<DateTime>("expiry_date", result.Value.ExpiryDate);
                //    await AuthenticationStateProvider.GetAuthenticationStateAsync();  //now the application knows about the user claims

                //    Navigation.NavigateTo("/");
                //}
                //else
                //{

                //    var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                //    log the info

                //    await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                //    await JSRunTime.InvokeVoidAsync("console.log", "response", new { errorResult.IsSuccess, errorResult.Errors, errorResult.Message });

                //    _errorMessage = errorResult.Message.ToLower() /*" issue completing request "*/;
                //} 
                #endregion

                await AuthenticationService.RegisterUserAsync(_model);
                Navigation.NavigateTo("/authentication/login");
                _isBusy = false;

            }
            catch (ApiException apiException)
            {
                _errorMessage = apiException.ApiErrorResponse.Message;

            }
            catch (Exception ex)
            {
                //log error
                //return StatusCode(500, ex.Message);
                // return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new employee records " + ex.Message);
                //log the info
                _model.Password = _model.Password.Replace(_model.Password.Substring(2), "****");
                //await JSRunTime.InvokeVoidAsync("console.log", "request", _model);
                //await JSRunTime.InvokeVoidAsync("console.log", "response", "Error retreiving data from the server at authentication/login " + ex.Message);

                _errorMessage = "Error fetching  employee record ";
            }
        }
        private  void RedirectToLogin()
        {
            Navigation.NavigateTo("/authentication/login");
        }
    }
}
