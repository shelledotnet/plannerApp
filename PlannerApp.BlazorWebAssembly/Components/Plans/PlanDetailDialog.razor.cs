using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class PlanDetailDialog
    {


        [CascadingParameter] 
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public string PlanId { get; set; }

        [Inject]
        public IPlannerService PlannerService { get; set; }

        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system

        private PlanDetail _plan;
        private bool _isBusy;
        private string _errorMessage = string.Empty;

        protected override void OnParametersSet()
        {
            if (PlanId == null)
                throw new ArgumentNullException(nameof(PlanId));
            base.OnParametersSet();
        }

        protected async override Task OnInitializedAsync()
        {
            await FetchPlanAsync();
        }
        private void Close()
        {
            MudDialog.Cancel();
        }

        private async Task FetchPlanAsync()
        {
            _isBusy = true;
            try
            {
                var result = await PlannerService.GetPlannsByIdAsync(PlanId);
                _plan = result.Value;
                StateHasChanged();
            }
            catch (ApiException apiException)
            {

                await JSRunTime.InvokeVoidAsync("console.log", "Exception", apiException);
                _errorMessage = $"Error occured at {apiException.ApiErrorResponse.Message}";

            }
            catch (Exception ex)
            {


                await JSRunTime.InvokeVoidAsync("console.log", "Exception", ex.Message);
                _errorMessage = "Error fetching  employee record ";
            }
            _isBusy = false;
        }
       
    }
}
