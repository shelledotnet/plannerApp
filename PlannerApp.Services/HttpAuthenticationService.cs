using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PlannerApp.Services
{
    public class HttpAuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        public HttpAuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse> RegisterUserAsync(RegisterRequest model)
        {
           var response= await _httpClient.PostAsJsonAsync("/api/v2/auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                throw new ApiException(errorReponse, response.StatusCode);
            }
        }
    }
}
