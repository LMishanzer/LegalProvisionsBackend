using System.Net.Http.Headers;
using System.Text;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.Logging.Models;
using LegalProvisionsLib.Settings;

using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace LegalProvisionsLib.Logging.Persistence.Elastic;

public class ElasticsearchPersistence : ILogPersistence
{
    private readonly ElasticSettings _settings;

    public ElasticsearchPersistence(ElasticSettings settings)
    {
        _settings = settings;
    }
    
    public async Task SaveLogAsync(LogItem item)
    {
        var indexBase = item switch
        {
            ErrorLog => "error",
            RequestLog => "http",
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
        var indexName = $"{indexBase}-{DateTime.UtcNow:yyyy-MM-dd}";
        
        if (!string.IsNullOrWhiteSpace(_settings.Url))
        {
            var address = $"{_settings.Url}/{indexName}/log/{Guid.NewGuid().ToString().ToLower()}";
        
            HttpResponseMessage response;
            try
            {
                var jsonInString = JsonConvert.SerializeObject(item);
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
                using var client = new HttpClient(handler);

                var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.Timeout = new TimeSpan(hours: 1, minutes: 0, seconds: 0);
        
                response = await client.PostAsync(requestUri: address, content: content);
            }
            catch (Exception e)
            {
                throw new ElasticException(message: $"Logging to ELK failed: {e.Message}", innerException: e);
            }
        
            if (response is {IsSuccessStatusCode: false})
            {
                string error;
                try
                {
                    error = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    error = $"response.Content.ReadAsStringAsync error: {ex.Message}";
                }
                throw new ElasticException(message: $"Logging to ELK failed: {error}", statusCode: (int) response.StatusCode);
            }
        }
    }
}