using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using PlannerApp.BlazorWebAssembly.Components.Auth;
using PlannerApp.BlazorWebAssembly.Components.Plans;
using PlannerApp.BlazorWebAssembly.Components.Layout;
using PlannerApp.BlazorWebAssembly.Components.Dialog;
using PlannerApp.BlazorWebAssembly.Components.ToDoItems;
using PlannerApp.BlazorWebAssembly.Shared;
using PlannerApp.BlazorWebAssembly;
using System.Security.Claims;
using Blazored.FluentValidation;

namespace PlannerApp.BlazorWebAssembly.Shared
{
    public partial class Error
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        //ERROR METHOD THAT WILL BE AVAILABLE FOR ALL CHILDREN OF THIS PARENT
        //TO INJECT THE HandleError METHOD WE GO TO THE BASE COMPONENT APP.RAZOR AND SURROUND THE ERROR COMPONENT WITH TAGS FOR IT TO CASACADE ALL OTHER CHILD COMPONENT
        public void HandleError(Exception ex)
        {
            Snackbar.Configuration.SnackbarVariant =Variant.Filled;
            Snackbar.Add("Error completing task , Please try again later",Severity.Error);
            Console.WriteLine($"{ex.Message} at {DateTime.Now}");
        }
    }
}