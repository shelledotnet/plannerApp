using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PlannerApp.Shared.Models;

namespace PlannerApp.BlazorWebAssembly.Components.ToDoItems
{
    public partial class ToDoItem
    {
        [Parameter]
        public ToDoItemDetail Item { get; set; }

        private bool _isChecked = true;


    }
}