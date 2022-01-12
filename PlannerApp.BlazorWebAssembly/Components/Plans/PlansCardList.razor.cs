using AKSoftware.Blazor.Utilities;
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

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Parameter]
        public EventCallback<PlanSummary> OnViewClicked { get; set; }
        //this event wil be called onViewclicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline



        [Parameter]
        public EventCallback<PlanSummary> OnEditClicked { get; set; }
        //this event wil be called onEditclicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnDeleteClicked { get; set; }
        //this event wil be called OnDeleteClicked when clicked and it will go an call a function in the paraent component
        //therefore the child component are just displaying the edit action why the actual logic is rendering in the parant component
        //hence allowing for code reusablity at every request pipeline

        protected override void OnInitialized()
        {
            //becos their is a send message from the patrent and this child component want to subscribe to that
            MessagingCenter.Subscribe<Plans, PlanSummary>(this, "plan_deleted", async (sender, args) =>
            {

                await GetPlanAsync(_pageNumber);
                StateHasChanged();//becos is been called from diferent tread and it will force this component to re-render itself
            });
        }


        protected async override Task OnInitializedAsync()
        {
            await GetPlanAsync();
        }

       
        private async Task GetPlanAsync(int pageNumber=1)
        {
            _pageNumber = pageNumber;
            _isBusy = true;
            _result = await FetchPlans?.Invoke(_query, _pageNumber, _pageSize); // ?  if its null it will not throw an exception
            _isBusy = false;
        }

        
    }
}
