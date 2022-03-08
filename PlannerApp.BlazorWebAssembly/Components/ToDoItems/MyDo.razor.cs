using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlannerApp.BlazorWebAssembly.Shared;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;

namespace PlannerApp.BlazorWebAssembly.Components.ToDoItems
{
    public partial class MyDo
    {


        #region Add this to Dependency injection service container at Programm.cs
        [Inject]
        public IToDoItemsService ToDoItemsService { get; set; }
        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system 
        #endregion

        [Parameter]
        public ToDoItemDetail Item { get; set; }
      
        //we need an event call baack to notify the parent about a new item added to plan
        [Parameter]
        public EventCallback<ToDoItemDetail> OnItemDeleted { get; set; }

        //we need an event call baack to notify the parent about a new item added to plan
        [Parameter]
        public EventCallback<ToDoItemDetail> OnItemEdit { get; set; }

        private bool _isChecked = true;
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;
        private bool _isEditMode = false;
        private string _description = String.Empty;
        private string _descriptionStyle => $"cursor:pointer;{(!_isChecked ? "" : "text-decoration: line-through")}";

        [CascadingParameter]  //this show case a casecading parent in the child with this you can call all methods in this casecading class e,g Error componemnt
        public Error Error { get; set; }

        //not always good to initialised _isChecked = true becos true means isdone
        protected override void OnInitialized()
        {
            //this indicat is done
            _isChecked = Item.IsDone;
        }
        private void ToggleEditMode(bool isCancle)
        {
            if (_isEditMode)
            {
                _isEditMode= false;
                _description = isCancle ? Item.Description : _description;
            }
            else
            {
                _isEditMode= true;
                _description=Item.Description;
            }
        }
        private async Task RemoveItemAsync()
        {
           
            
            try
            {
                _isBusy = true;
                //call the api to add item
                await ToDoItemsService.DeleteAsync(Item.Id);
                
                await OnItemDeleted.InvokeAsync(Item); //notify the parent plandetaildialog about the newly addedd item  
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
        private async Task EditItemAsync()
        {
            _errorMessage = String.Empty;

            try
            {

                if (string.IsNullOrWhiteSpace(_description))
                {
                    _errorMessage = "description is required";
                    return;
                }
                _isBusy = true;
                //call the api to add item
                var result= await ToDoItemsService.EditAsync(Item.Id,_description,Item.PlanId);
                ToggleEditMode(false);
                await OnItemEdit.InvokeAsync(result.Value); //notify the parent plandetaildialog about the newly Edited item  
                
            }
            catch (ApiException apiException)
            {
                //error responses from the endpoint am calling
                await JSRunTime.InvokeVoidAsync("console.log", "Exception",new  { apiException , DateTime.Now });
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

        //toggle the item to indicate the ones completed and cheked it
        private async Task ToggleItemAsync(bool value)
        {
            _errorMessage = String.Empty;

            try
            {

                _isBusy = true;
                //call the api to add item
                var result = await ToDoItemsService.ToggleAsync(Item.Id);
                Item.IsDone= !Item.IsDone;
                _isChecked= Item.IsDone;
                await OnItemEdit.InvokeAsync(Item); //notify the parent plandetaildialog about the newly Edited item  

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
