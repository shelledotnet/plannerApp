using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Pages.Plans
{
    public partial class CreateEditPlan
    {
        [Parameter]
        public string Id { get; set; } //this is how we received the template parameter from the route


        public List<BreadcrumbItem> _breadcrumbItems = new()
        {
            new BreadcrumbItem("Home", "/index"),
            new BreadcrumbItem("Plans", "/plans"),
             new BreadcrumbItem("Create", "/plans/form", true)
        };
    }
}
