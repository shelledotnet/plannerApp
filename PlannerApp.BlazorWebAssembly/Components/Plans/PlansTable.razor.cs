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

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnDeleteClicked { get; set; }

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnEditClicked { get; set; }


        #endregion
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
