using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlannerApp.BlazorWebAssembly.Shared;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;

namespace PlannerApp.BlazorWebAssembly.Components.ToDoItems
{
    public partial class CreateToDoItemForm
    {
        #region Add this to Dependency injection service container at Programm.cs
        [Inject]
        public IToDoItemsService ToDoItemsService { get; set; }
        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system 
        #endregion

        [Parameter]
        public string PlannId { get; set; }

        //we need an event call baack to notify the parent about a new item added to plan
        [Parameter]
        public EventCallback<ToDoItemDetail> OnToDoItemAdded { get; set; }

        [CascadingParameter]  //this show case a casecading parent in the child with this you can call all methods in this casecading class e,g Error componemnt
        public Error Error { get; set; }

        private bool _isBusy = false;
        private string _description { get; set; }
        private string _errorMessage = string.Empty;
        private async Task AddToItemAsync()
        {
            _errorMessage = string.Empty;
            _isBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(_description))
                {
                    _errorMessage = "description is required";
                    return;
                }



                //call the api to add item
                var result = await ToDoItemsService.CreateAsync(_description, PlannId);
                _description = String.Empty;
                await OnToDoItemAdded.InvokeAsync(result.Value); //notify the parent plandetaildialog about the newly addedd item  
            }
            catch (ApiException apiException)
            {
                //error responses from the endpoint am calling
                await JSRunTime.InvokeVoidAsync("console.log", "Exception", apiException);
                _errorMessage = $"Error occured at {apiException.ApiErrorResponse.Message}";

            }
            catch (Exception ex)
            {

                //Error responses from me e.g intenet server went down , server issues
                await JSRunTime.InvokeVoidAsync("console.log", "Error", new { ex.Message, date = DateTime.Now });
                Error.HandleError(ex);
            }
            _isBusy = false;
        }
    }
}
