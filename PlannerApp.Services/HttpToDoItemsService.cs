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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services
{
    
    public class HttpToDoItemsService : IToDoItemsService
    {

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _JSRunTime;

        public HttpToDoItemsService(HttpClient httpClient, IJSRuntime JSRunTime)
        {
            _httpClient = httpClient;
            _JSRunTime = JSRunTime;
        }

        #region Implementation of services
        public async Task<ApiResponse<ToDoItemDetail>> CreateAsync(string description, string planId)
        {
          
            //its not a form so we use PostAsJsonAsync which is json
            var response = await _httpClient.PostAsJsonAsync("api/v2/todos",new { PlanId=planId,Description=description});
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ToDoItemDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planId, description });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planId, description });
                await _JSRunTime.InvokeVoidAsync("console.log", "response",new { errorReponse, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse> DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v2/todos/{id}");
            var request = $"Delete/api/v2/todos/{id}";
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { request });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { request });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { errorReponse, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse<ToDoItemDetail>> EditAsync(string id, string newDescription, string planId)
        {
            //this has object to send in hte PUT request
            var response = await _httpClient.PutAsJsonAsync("api/v2/todos", new { PlanId = planId, Description = newDescription,Id=id });
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ToDoItemDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { Edit = "put", planId, newDescription,id });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planId, newDescription, id });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { errorReponse, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse> ToggleAsync(string id)
        {
            //this has no object to send in the PUT request so we put NULL(its easier to use GET method verb here but the endpoint we are consuming uses PUT)
            var response = await _httpClient.PutAsJsonAsync<object>($"api/v2/todos/toggle/{id}", null);
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { Edit = "put",id });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { id });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { errorReponse, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        #endregion
        #region Calling Vendor
     
        #endregion

    }
}
