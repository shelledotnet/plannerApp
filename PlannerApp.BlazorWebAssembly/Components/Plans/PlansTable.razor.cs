using AKSoftware.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class PlansTable
    {
        #region Depency injection service container which u have to register at the Service in Program.cs
        [Inject]
        public IPlannerService PlannerService { get; set; }

        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system

        //[Inject]
        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }  //tellus about login user


        //[Inject]
        //public ILocalStorageService Storage { get; set; }//is use to store access token  and expiry date after the response from the server

        #endregion

        #region Variable for the PlansList

        private string _query = string.Empty;
        private MudTable<PlanSummary> _table;

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnViewClicked { get; set; }
        //this event wil be called OnViewClicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline



        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnDeleteClicked { get; set; }
        //this event wil be called OnDeleteClicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnEditClicked { get; set; }
        //this event wil be called onEditclicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline


        #endregion

        protected override void OnInitialized()
        {
            //becos their is a send message from the patrent and this child component want to subscribe to that
            MessagingCenter.Subscribe<Plans, PlanSummary>(this, "plan_deleted", async (sender, args) =>
             {
                //the action we are doing here is a result of retrieving the data from ServerReloadAsync() below
                //and it will called the ServerReloadAsync() agian
                await _table.ReloadServerData();
                 StateHasChanged();//becos is been called from diferent tread and it will force this component to re-render itself
            });
        }


        private async Task<TableData<PlanSummary>> ServerReloadAsync(TableState state)
        {
          var result=  await PlannerService.GetPlannsAsync(_query, state.Page, state.PageSize);

            return new TableData<PlanSummary>
            {
                Items = result.Value.Records,
                TotalItems = result.Value.ItemsCount
            };
        }

        private void OnSearch(string query)
        {
            _query = query;
            _table.ReloadServerData();
        }

    }
}
