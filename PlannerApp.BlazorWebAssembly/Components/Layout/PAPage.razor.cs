using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Layout
{
    public partial class PAPage
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();  // to void object refernce not set to an intance of an Object
    }
}
