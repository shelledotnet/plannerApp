using AKSoftware.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PlannerApp.BlazorWebAssembly.Components.Dialog;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class Plans : ComponentBase
    {
        #region Depency injection service container which u have to register at the Service in Program.cs
        [Inject]
        public IPlannerService PlannerService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system

        //[Inject]
        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }  //tellus about login user


        //[Inject]
        //public ILocalStorageService Storage { get; set; }//is use to store access token  and expiry date after the response from the server

        #endregion

        #region Variable for the PlansList

        private  bool _isBusy = false;

        private  string _errorMessage = string.Empty;
        private  int _pageSize = 10;
        private  int _pageNumber = 1;
        private int __totalPages= 1;
        private  string _query = string.Empty;
        private List<PlanSummary> _plans=new();

        #endregion

        public async Task<PagedList<PlanSummary>> GetPlansAsync(string query="",int pageNumber=1,int pageSize=10)
        {
            try
            {

                _isBusy = true;
               
                var result = await PlannerService.GetPlannsAsync(query, pageNumber, pageSize);
                _plans = result.Value.Records.ToList();
                _pageNumber = result.Value.Page;
                _pageSize = result.Value.PageSize;
                __totalPages = result.Value.TotalPages;

                return result.Value;
               

            }
            catch (ApiException apiException)
            {

                await JSRunTime.InvokeVoidAsync("console.log", "Exception", apiException);
                _errorMessage =$"Error occured at {apiException.ApiErrorResponse.Message}";

            }
            catch (Exception ex)
            {


                await JSRunTime.InvokeVoidAsync("console.log", "Exception", ex.Message);
                _errorMessage = "Error fetching  employee record ";
            }

            _isBusy = false;
            return null;
           
        }

        #region View Toggler
        private bool _isCardViewEnabled = true;
        private void SetCardsView()
        {
            _isCardViewEnabled = true;
        }
        private void SetTableView()
        {
            _isCardViewEnabled = false;
        }
        #endregion

        #region Edit_in_Parent_Component_to_cascade_on_grid_and_table_childcomponent
        private void EditPlan(PlanSummary planSummary)
        {
            Navigation.NavigateTo($"/plans/form/{planSummary.Id}");
        }
        #endregion

        #region Delete_in_Parent_Component_to_cascade_on_grid_and_table_childcomponent
        private async Task DeletePlanAsync(PlanSummary plan)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", $"Do you really want to delete the plan '{plan.Title}'? ");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialogue = DialogService.Show<ConfirmationDialog>("Delete", parameters, options);
            var confirmationResult = await dialogue.Result;
            if (!confirmationResult.Cancelled)
            {
                try
                {
                    //for counsiming a service pls use try catch block
                    await PlannerService.DeleteAsync(plan.Id);

                    //the parent after delete above send a message about the deleted plan to the children component to reload
                    //this(an instatcne of this class that is sending the message),"deleted mesage","deleted plan"
                    MessagingCenter.Send(this, "plan_deleted", plan);
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

            }
        }
        #endregion

        #region View_in_Parent_Component_to_cascade_on_grid_and_table_childcomponent
        private void ViewPlan(PlanSummary plan)
        {
            var parameters = new DialogParameters();
            parameters.Add("PlanId",plan.Id);
       

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

            var dialogue = DialogService.Show<PlanDetailDialog>("Details", parameters, options);
         
        }
        #endregion
    }
}
