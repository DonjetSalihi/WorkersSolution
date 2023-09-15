using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WorkersSolution.Constants;
using WorkersSolution.Exceptions;
using WorkersSolution.Models;
using Xamarin.Essentials;

namespace WorkersSolution.Services
{
    public class WorkerService : IRepository<Worker>
    {
        HttpClient client;
        ILogger<WorkerService> logger;

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public WorkerService(ILogger<WorkerService> logger = null, IHttpClientFactory httpClientFactory = null)
        {
            this.logger = logger;
            client = httpClientFactory == null ? new HttpClient() : httpClientFactory.CreateClient("WorkersApi");

            if (httpClientFactory == null)
            {
                client.BaseAddress = new Uri($"{AppConfig.WorkersApiBaseUrl}/");
            }
        }

        /// <summary>
        /// Retrieves a list of workers from the API.
        /// </summary>
        /// <returns>A list of workers.</returns>
        public async Task<ApiResponse> GetAllWorkersAsync()
        {
            logger?.LogInformation($"Inside: {nameof(GetAllWorkersAsync)}");

            GiveAuthorization();

            if (IsConnected)
            {
                try
                {
                    logger?.LogInformation($"Calling api: {nameof(GetAllWorkersAsync)}");

                    var response = await client.GetAsync($"workers");

                    if (!response.IsSuccessStatusCode)
                    {
                        return await HandleErrorResponse(response);
                    }

                    logger?.LogInformation($"Success calling api: {nameof(GetAllWorkersAsync)}");

                    var getResponseContent = await response.Content.ReadAsStringAsync();

                    return new ApiResponse(true, getResponseContent, string.Empty, response.StatusCode);
                }
                catch (Exception ex)
                {
                    logger?.LogInformation($"General error: {nameof(GetAllWorkersAsync)}, Exception: {ex.Message}");
                    throw ex;
                }
            }
            else
            {
                logger?.LogInformation($"No Internet: {nameof(GetAllWorkersAsync)}");

                throw new NoInternetException() { NoInternetMessage = Message.NO_INTERNET };
            }
        }

        /// <summary>
        /// Retrieves a list of locations from the API based on a specific worker id.
        /// </summary>
        /// <param name="workerId">The workerId for which to retrieve locations.</param>
        /// <returns>A list of locations where the worker is assigned to.</returns>
        public async Task<ApiResponse> GetLocationsByWorkerIdAsync(string workerId)
        {
            logger?.LogInformation($"Inside: {nameof(GetLocationsByWorkerIdAsync)}");

            GiveAuthorization();

            if (IsConnected)
            {
                try
                {
                    logger?.LogInformation($"Calling api: {nameof(GetLocationsByWorkerIdAsync)}");

                    var response = await client.GetAsync($"workers/{workerId}/locations");

                    if (!response.IsSuccessStatusCode)
                    {
                        return await HandleErrorResponse(response);
                    }

                    logger?.LogInformation($"Success calling api: {nameof(GetLocationsByWorkerIdAsync)}");

                    var getResponseContent = await response.Content.ReadAsStringAsync();

                    return new ApiResponse(true, getResponseContent, string.Empty, response.StatusCode);
                }
                catch (Exception ex)
                {
                    logger?.LogInformation($"General error: {nameof(GetLocationsByWorkerIdAsync)}, Exception: {ex.Message}");
                    throw ex;
                }
            }
            else
            {
                logger?.LogInformation($"No Internet: {nameof(GetLocationsByWorkerIdAsync)}");

                throw new NoInternetException() { NoInternetMessage = Message.NO_INTERNET };
            }
        }

        /// <summary>
        /// Retrieves worker picture from API based on a specific worker id.
        /// </summary>
        /// <param name="workerId">The workerId for which to retrieve the worker picture.</param>
        /// <returns>A workers picture.</returns>
        public async Task<ApiResponse> GetPictureByWorkerIdAsync(string workerId)
        {
            logger?.LogInformation($"Inside: {nameof(GetPictureByWorkerIdAsync)}");

            GiveAuthorization();

            if (IsConnected)
            {
                try
                {
                    logger?.LogInformation($"Calling api: {nameof(GetPictureByWorkerIdAsync)}");

                    var response = await client.GetAsync($"workers/photo/{workerId}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return await HandleErrorResponse(response);
                    }

                    logger?.LogInformation($"Success calling api: {nameof(GetPictureByWorkerIdAsync)}");

                    var getResponseContent = await response.Content.ReadAsByteArrayAsync();

                    return new ApiResponse(true, getResponseContent, string.Empty, response.StatusCode);
                }
                catch (Exception ex)
                {
                    logger?.LogInformation($"General error: {nameof(GetPictureByWorkerIdAsync)}, Exception: {ex.Message}");
                    throw ex;
                }
            }
            else
            {
                logger?.LogInformation($"No Internet: {nameof(GetPictureByWorkerIdAsync)}");

                throw new NoInternetException() { NoInternetMessage = Message.NO_INTERNET };
            }
        }

        private void GiveAuthorization()
        {
            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", AppConfig.WorkersApiAuthorizationToken);
            }
        }

        private async Task<ApiResponse> HandleErrorResponse(HttpResponseMessage response)
        {
            var getResponseContent = await response.Content.ReadAsStringAsync();

            logger?.LogInformation($"Error Response: {getResponseContent}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResponse(false, getResponseContent, Message.UNAUTHORIZED, response.StatusCode);

            else if (response.StatusCode == HttpStatusCode.NotFound)
                return new ApiResponse(false, getResponseContent, Message.NOT_FOUND, response.StatusCode);

            else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                return new ApiResponse(false, getResponseContent, Message.TOO_MANY_REQUESTS, response.StatusCode);

            else if (response.StatusCode == HttpStatusCode.InternalServerError)
                return new ApiResponse(false, getResponseContent, Message.INTERNAL_SERVER_ERROR, response.StatusCode);

            else
                return new ApiResponse(false, getResponseContent, Message.GENERAL_API_ERROR, response.StatusCode);
        }
    }
}
