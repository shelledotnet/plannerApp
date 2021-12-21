using Microsoft.JSInterop;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services
{
    public class HttpPlannerService : IPlannerService
    {

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _JSRunTime;

        public HttpPlannerService(HttpClient httpClient, IJSRuntime JSRunTime)
        {
            _httpClient = httpClient;
            _JSRunTime = JSRunTime;
        }

        public async Task<ApiResponse<PagedList<PlanSummary>>> GetPlannsAsync(string query= null, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"/api/v2/plans?query={query}&pageNumber={pageNumber}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedList<PlanSummary>>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { query,pageNumber,pageSize});
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", result);


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { query, pageNumber, pageSize });
                await _JSRunTime.InvokeVoidAsync("console.log", "response",errorReponse);

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }
    }
}
