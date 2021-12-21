using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Auth
{
    public partial class LoginForm : ComponentBase
    {
        #region Depency injection service container which u have to register at the Service in Program.cs
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }


        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system

        //[Inject]
        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }  //tellus about login user


        //[Inject]
        //public ILocalStorageService Storage { get; set; }//is use to store access token  and expiry date after the response from the server

        #endregion

        #region Variable for the form
        private readonly LoginRequest _model = new();

        private bool _isBusy = false;

        private string _errorMessage = string.Empty; 
        #endregion
        private async Task LoginUserAsync()
        {

            try
            {

                _isBusy = true;
                _errorMessage = string.Empty;

             

                await AuthenticationService.LoginUserAsync(_model);
                Navigation.NavigateTo("/");
                _isBusy = false;

            }
            catch (ApiException apiException)
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Exception", apiException);
                _errorMessage = apiException.ApiErrorResponse.Message;


            }
            catch (Exception ex)
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Exception", ex);
                _errorMessage = "Error fetching  employee record ";
            }
            _isBusy = false;
        }
        private void RedirectToRegister()
        {
            Navigation.NavigateTo("/auth/register");
        }
    }
}
