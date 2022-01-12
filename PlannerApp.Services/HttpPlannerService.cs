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

        #region Implementation of services
        public async Task<ApiResponse<PlanDetail>> CreateAsync(PlanDetail planDetail, FormFile formFile)
        {
            var form = PreparePlanForm(planDetail, formFile, false);

            //its a form that is why we are using PostAsync not PostAsJsonAsync which is json
            var response = await _httpClient.PostAsync("api/v2/plans", form);
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PlanDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planDetail, formFile });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planDetail, formFile });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", errorReponse);

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse> DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v2/plans/{id}");
            var request = $"Delete/api/v2/plans/{id}";
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PlanDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new {request });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response",new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { request });
                await _JSRunTime.InvokeVoidAsync("console.log", "response",new { errorReponse, response.StatusCode });

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse<PlanDetail>> EditAsync(PlanDetail planDetail, FormFile formFile)
        {
            var form = PreparePlanForm(planDetail, formFile, true);

            //its a form that is why we are using PutAsync not PostAsJsonAsync which is json
            var response = await _httpClient.PutAsync("api/v2/plans", form);
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PlanDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { Edit="put", planDetail, formFile });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { planDetail, formFile });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", errorReponse);

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }


        public async Task<ApiResponse<PagedList<PlanSummary>>> GetPlannsAsync(string query = null, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"/api/v2/plans?query={query}&pageNumber={pageNumber}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedList<PlanSummary>>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { query, pageNumber, pageSize });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { query, pageNumber, pageSize });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", errorReponse);

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        public async Task<ApiResponse<PlanDetail>> GetPlannsByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v2/plans/{id}");
            var request = $"Get/api/v2/plans/{id}";
            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PlanDetail>>();
                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new {  request });
                await _JSRunTime.InvokeVoidAsync("console.log", "raw-response", response);
                await _JSRunTime.InvokeVoidAsync("console.log", "response", new { result, response.StatusCode });


                return result;
            }
            else
            {
                var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                //log the info
                await _JSRunTime.InvokeVoidAsync("console.log", "request", new { request });
                await _JSRunTime.InvokeVoidAsync("console.log", "response", errorReponse);

                throw new ApiException(errorReponse, response.StatusCode);
            }
        }

        #endregion
        #region Calling Vendor
        //i need to send a form that why i need the below
        private HttpContent PreparePlanForm(PlanDetail model, FormFile coverFile, bool isUpdate)
        {
                //for contentent-type-> multipart/form-data request to upload the file to Web API use below
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(model.Title), nameof(PlanDetail.Title));
                if (!string.IsNullOrWhiteSpace(model.Description))
                    form.Add(new StringContent(model.Description), nameof(PlanDetail.Description));
                if (isUpdate)
                    form.Add(new StringContent(model.Id), nameof(PlanDetail.Id));
                if (coverFile != null)
                    form.Add(new StreamContent(coverFile.FileStream), nameof(PlanDetail.CoverFile), coverFile.FileName);

                return form;
            
         
        } 
        #endregion

    }
}
