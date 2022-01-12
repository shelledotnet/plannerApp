using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PlannerApp.Services.Exceptions;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.BlazorWebAssembly.Components.Plans
{
    public partial class Planform
    {

        #region Depency injection service container which u have to register at the Service in Program.cs
        [Inject]
        public IPlannerService PlannerService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }


        [Inject]
        public IJSRuntime JSRunTime { get; set; }//login system

        //[Inject]
        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }  //tellus about login user


        //[Inject]
        //public ILocalStorageService Storage { get; set; }//is use to store access token  and expiry date after the response from the server

        #endregion

        #region Variable for the PlansList

        private PlanDetail _model = new PlanDetail();

        [Parameter]
        public string Id { get; set; }  //tempalte paramter for editing
        public bool _isEditMode => Id != null;
        private bool _isBusy = false;

        //this represent the stream of the file
        private Stream _stream = null; 

        //incase the file is selected
        private string _fileName = string.Empty;
        private string _errorMessage = string.Empty;
        #endregion

        private async Task SubmitFormAsync()
        {
            _isBusy = true;
            try
            {

                FormFile formFile = null;
                if (_stream !=null)
                    formFile = new FormFile(_stream, _fileName);
                if(_isEditMode)
                    await PlannerService.EditAsync(_model, formFile);
                else
                    await PlannerService.CreateAsync(_model, formFile);
                Navigation.NavigateTo("/plans");
                
            }
            catch (ApiException apiException)
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Error", apiException);
                _errorMessage = apiException.ApiErrorResponse.Message;


            }
            catch (Exception ex)
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Error", ex.Message);
                _errorMessage = "Error fetching  employee record ";
            }
            _isBusy = false;
        }

        //this will be use when choosing a file
        private async Task OnChooseFileAsync(InputFileChangeEventArgs e)
        {
            _errorMessage = string.Empty;
            var file = e.File;
            if (file != null)
            {
                if (file.Size >= 2097152) //the character lenght of the file must be less than or equal to 2MB
                {
                    _errorMessage = "the file must be less than or equal to  2MB";
                    return;
                }
                string[] allowExtensions = new[] { ".jpg", ".png", ".bmp", ".svg" };
                string extension = Path.GetExtension(file.Name).ToLower();

                if (!allowExtensions.Contains(extension))
                {
                    _errorMessage = "Plaese choose a valid image file extension";
                    return;
                }

                //file is ready to upload
                using (var stream=file.OpenReadStream(2097152))//is not going to read any file bigger than 2MB
                {
                    //byte array is going to be the charascter lenght of the file
                    var buffer = new byte[file.Size];


                    //read the file and put it in the stream variable memory
                    await stream.ReadAsync(buffer,0,(int)file.Size);
                    _stream = new MemoryStream(buffer)
                    {
                        Position = 0  //start reading from o character of the file
                    };
                    _fileName = file.Name;
                }
            }
        }

        protected  override async Task OnInitializedAsync()
        {
            if (_isEditMode)
                await FetchPlanById();
        }

        private async Task FetchPlanById()
        {
            _isBusy = true;
            try
            {
                var result = await PlannerService.GetPlannsByIdAsync(Id);
                _model = result.Value;
            }
            catch (ApiException apiException) //execption from the api that we have anticipitated
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Error", apiException);
                _errorMessage = apiException.ApiErrorResponse.Message;


            }
            catch (Exception ex) //exception that we do not anticaipated
            {
                await JSRunTime.InvokeVoidAsync("console.log", "Error", ex.Message);
                _errorMessage = "Error fetching  employee record ";
            }
            _isBusy = false;
        }
    }
}
