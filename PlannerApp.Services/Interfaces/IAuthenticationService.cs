using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services.Interfaces
{
   public   interface IAuthenticationService
    {
        Task<ApiResponse> RegisterUserAsync(RegisterRequest model);
        Task<ApiResponse<LoginResult>> LoginUserAsync(LoginRequest model);
    }
}
