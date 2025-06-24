using RestSharp;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ApiTests.Services
{
    public abstract class BaseApiClient<T> where T : class
    {
        private readonly Func<RestClient> _clientFactory;
        protected readonly string _endpoint;
        private readonly int _maxRetries;
        private readonly int _retryDelay;
        private readonly string _apiKey;

        protected BaseApiClient(Func<RestClient> clientFactory, string apiKey, string endpoint, int maxRetries = 3, int retryDelay = 1000)
        {
            _clientFactory = clientFactory;
            _endpoint = endpoint;
            _maxRetries = maxRetries;
            _retryDelay = retryDelay;
            _apiKey = apiKey;
        }

        protected RestClient CreateClient()
        {
            var client = _clientFactory();
            client.AddDefaultHeader("x-api-key", _apiKey);
            return client;
        }
        
        public string GetEndpointUrl() => $"{_clientFactory().Options.BaseUrl}/{_endpoint}".TrimEnd('/');

        protected async Task<ApiResponse<TResult>> ExecuteWithRetryAsync<TResult>(RestRequest request) where TResult : class
        {
            var stopwatch = Stopwatch.StartNew();
            RestResponse? lastResponse = null;
            Exception? lastException = null;

            for (var attempt = 0; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    if (attempt > 0)
                    {
                        await Task.Delay(_retryDelay * attempt);
                    }

                    var client = CreateClient();
                    lastResponse = await client.ExecuteAsync(request);
                    if (IsTransientError(lastResponse) && attempt < _maxRetries)
                    {
                        continue;
                    }
                    stopwatch.Stop();
                    return CreateApiResponse<TResult>(lastResponse, stopwatch.Elapsed);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (attempt == _maxRetries)
                    {
                        throw new ApiException($"Request failed after {_maxRetries + 1} attempts", ex);
                    }
                }
            }
            stopwatch.Stop();
            if (lastResponse != null)
            {
                return CreateApiResponse<TResult>(lastResponse, stopwatch.Elapsed);
            }
            throw new ApiException($"Request failed after {_maxRetries + 1} attempts", lastException);
        }

        protected bool IsTransientError(RestResponse response)
        {
            return (int)response.StatusCode >= 500 || response.StatusCode == 0;
        }

        protected ApiResponse<TResult> CreateApiResponse<TResult>(RestResponse response, TimeSpan elapsed) where TResult : class
        {
            var apiResponse = new ApiResponse<TResult>
            {
                StatusCode = (int)response.StatusCode,
                IsSuccess = response.IsSuccessful,
                ErrorMessage = response.ErrorMessage,
                ResponseTime = elapsed,
                RawContent = response.Content,
                Headers = response.Headers?.ToDictionary(h => h.Name ?? "", h => h.Value?.ToString() ?? "")
            };
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<TResult>(response.Content);
                }
                catch (JsonException ex)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessage = $"Failed to deserialize response: {ex.Message}";
                }
            }
            else if (!response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                    apiResponse.ErrorMessage = errorResponse?.Error ?? response.ErrorMessage;
                }
                catch
                {
                    apiResponse.ErrorMessage = response.Content ?? response.ErrorMessage;
                }
            }
            return apiResponse;
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; init; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public TimeSpan ResponseTime { get; init; }
        public string? RawContent { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
    }

    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
        public ApiException(string message, Exception? innerException) : base(message, innerException) { }
    }

    public class ErrorResponse
    {
        public string Error { get; } = string.Empty;
    }
} 