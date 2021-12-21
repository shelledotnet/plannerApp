using Microsoft.AspNetCore.Components;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class PlansCardList
    {
        private bool _isBusy { get; set; }
        private int _pageSize = 10;
        private int _pageNumber = 1;
        private string _query = string.Empty;
        private PagedList<PlanSummary> _result = new();

        //Delegate is an action pointer
        [Parameter]
        public Func<string,int,int,Task<PagedList<PlanSummary>>> FetchPlans { get; set; }

        
        protected async override Task OnInitializedAsync()
        {
            _isBusy = true;
            _result =await FetchPlans?.Invoke(_query, _pageNumber, _pageSize); // ?  if its null it will not throw an exception
            _isBusy = false;
        }
    }
}
