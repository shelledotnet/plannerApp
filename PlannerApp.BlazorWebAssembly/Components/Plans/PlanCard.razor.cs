using Microsoft.AspNetCore.Components;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class PlanCard 
    {
        [Parameter]
        public PlanSummary PlanSummary { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnViewClicked { get; set; }

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnDeleteClicked { get; set; }

        //Event call back actions
        [Parameter]
        public EventCallback<PlanSummary> OnEditClicked { get; set; }
    }
}
