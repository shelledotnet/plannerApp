using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PlannerApp.BlazorWebAssembly.Components.ToDoItems;
using PlannerApp.BlazorWebAssembly.Shared;
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

        [CascadingParameter]  //this show case a casecading parent in the child with this you can call all methods in this casecading class e,g Error componemnt
        public Error Error { get; set; }

        private PlanDetail _plan;
        private bool _isBusy;
        private string _errorMessage = string.Empty;
        private List<ToDoItemDetail> _items = new ();
        private bool _descriptionStyle => _isBusy=false;



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
                
                //throw new ArgumentException("Invald data");
                var result = await PlannerService.GetPlannsByIdAsync(PlanId);
                _plan = result.Value;
                _items = _plan.ToDoItems;
                StateHasChanged();
            }
            catch (ApiException apiException)
            {

                await JSRunTime.InvokeVoidAsync("console.log", "Exception", apiException);
                _errorMessage = $"Error occured at {apiException.ApiErrorResponse.Message}";

            }
            catch (Exception ex)
            {


                await JSRunTime.InvokeVoidAsync("console.log", "Error", new { ex.Message, date = DateTime.Now });
                Error.HandleError(ex);
            }
            _isBusy = false;
        }
       
        private void OnToDoItemAddedCallBack(ToDoItemDetail toDoItemDetail)
        {
            _items.Add(toDoItemDetail);
        }
        private void OnItemDeletedCallBack(ToDoItemDetail toDoItemDetail)
        {
            _items.Remove(toDoItemDetail);
        }
        private void OnItemEditCallBack(ToDoItemDetail toDoItemDetail)
        {
            var editedItem=_items.SingleOrDefault(i=>i.Id== toDoItemDetail.Id);
            editedItem.Description= toDoItemDetail.Description;
            editedItem.IsDone= toDoItemDetail.IsDone;
           
        }
    }
}
