using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using WorkersSolution.Exceptions;
using WorkersSolution.Models;
using WorkersSolution.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WorkersSolution.ViewModels
{
    public class WorkersViewModel : BaseViewModel
    {
        #region Initialization
        public ObservableCollection<WorkerModel> Workers { get; set; }
        public Command LoadWorkersCommand { get; set; }
        public Command ExpanderTappedCommand { get; private set; }

        bool isBusyLoadingList;
        public bool IsBusyLoadingList
        {
            get => isBusyLoadingList;
            set => SetProperty(ref isBusyLoadingList, value);
        }

        public WorkersViewModel(IRepository<Worker> workerRepository = null)
        {
            WorkerRepository = workerRepository;
            Title = "Workers";
            Workers = new ObservableCollection<WorkerModel>();
            LoadWorkersCommand = new Command(async () => await ExecuteLoadWorkersCommand());
            ExpanderTappedCommand = new Command(async (worker) => await ExecuteExpanderTapCommand(worker));
        }
        #endregion

        #region Commands
        async Task ExecuteLoadWorkersCommand()
        {
            if (IsBusy || IsBusyLoadingList)
                return;

            IsBusy = true;

            //Using this only for list loading activity indicator, so it doesn't show in expanders loading
            IsBusyLoadingList = true;

            try
            {
                //First we clear the collection
                ClearWorkersList();
                
                //Here we get all workers from api
                var getWorkers = await GetWorkersAsync();

                if (getWorkers == null || !getWorkers.Any())
                    return;

                //Here we populate the collection
                PopulateWorkersList(getWorkers);
            }
            catch (NoInternetException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.NoInternetMessage, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsBusyLoadingList = false;
            }
        }

        async Task ExecuteExpanderTapCommand(object getWorker)
        {
            //Get the selected item in the collection
            WorkerModel getSelectedWorker = (WorkerModel)getWorker;

            if (IsBusy)
                return;

            //If it is the same item, we collapse it
            if (!getSelectedWorker.IsExpanded)
                return;

            //Collapse all the items, and expand the one selected
            HandleExpanding(getSelectedWorker);

            IsBusy = true;

            try
            {
                //Get the picture from api for the selected worked
                await GetPictureAsync(getSelectedWorker);

                //Get all the locations from api for the selected worker
                await GetLocationsAsync(getSelectedWorker);
            }
            catch (NoInternetException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.NoInternetMessage, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "System error, please contact the support.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region API Calls
        private async Task<IEnumerable<Worker>> GetWorkersAsync()
        {
            var response = await WorkerRepository.GetAllWorkersAsync();

            string responseJson = (string)response.Result;

            if (!response.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
                return null;
            }

            return JsonConvert.DeserializeObject<IEnumerable<Worker>>(responseJson);
        }

        private async Task GetPictureAsync(WorkerModel getSelectedWorker)
        {
            var response = await WorkerRepository.GetPictureByWorkerIdAsync(getSelectedWorker.Id);

            if (!response.Success)
            {
                //If worker has no picture uploaded, the api returns not found, so we set a default profile picture
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    getSelectedWorker.Picture = GetImageFromResource("WorkersSolution.Assets.noprofile.jpg");
                    return;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
                    return;
                }
            }

            getSelectedWorker.Picture = (byte[])response.Result;
        }

        private async Task GetLocationsAsync(WorkerModel getSelectedWorker)
        {
            var response = await WorkerRepository.GetLocationsByWorkerIdAsync(getSelectedWorker.Id);

            string responseJson = (string)response.Result;

            if (!response.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
                return;
            }

            var locations = JsonConvert.DeserializeObject<IEnumerable<WorkerLocation>>(responseJson);

            if (locations != null && locations.Any())
                getSelectedWorker.LocationName = string.Join(",", locations.Select(x => x.Name));
            else
                getSelectedWorker.LocationName = "No locations found for this worker.";
        }
        #endregion

        #region Private Methods
        private void HandleExpanding(WorkerModel getSelectedWorker)
        {
            Workers.ForEach(x => x.IsExpanded = false);

            getSelectedWorker.IsExpanded = true;
        }

        private void ClearWorkersList()
        {
            Workers.Clear();
        }

        private void PopulateWorkersList(IEnumerable<Worker> getWorkers)
        {
            foreach (var worker in getWorkers)
            {
                WorkerModel model = new WorkerModel()
                {
                    Id = worker.Id,
                    FirstName = worker.FirstName,
                    LastName = worker.LastName,
                    CreatedOn = worker.CreatedOn,
                    IsExpanded = false
                };

                Workers.Add(model);
            }
        }

        private byte[] GetImageFromResource(string resourcePath)
        {
            byte[] imageByteArray = null;

            // Get the assembly where the resource is located
            Assembly assembly = typeof(WorkersViewModel).GetTypeInfo().Assembly;

            // Read the embedded resource as a stream
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    // Convert the stream to a byte array
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        imageByteArray = memoryStream.ToArray();
                    }
                }
            }

            return imageByteArray;
        }
        #endregion
    }
}
