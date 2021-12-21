using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services.Interfaces
{
   public interface IPlannerService
    {
        
       
       Task<ApiResponse<PagedList<PlanSummary>>> GetPlannsAsync(string query= null, int pageNumber = 1, int pageSize = 10);
    }
}
