using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services.Interfaces
{
    public interface IToDoItemsService
    {
   

        Task<ApiResponse<ToDoItemDetail>> CreateAsync(string description,string planId);

        Task<ApiResponse<ToDoItemDetail>> EditAsync(string id,string newDescription, string planId);

        Task<ApiResponse> ToggleAsync(string id);

        Task<ApiResponse> DeleteAsync(string id);
    }
}
