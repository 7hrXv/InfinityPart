using System.Net.Http.Json;

namespace InfinityPart.UI.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }

    public async Task<HttpResponseMessage> PostAsync<T>(
        string endpoint,
        T dados)
    {
        return await _httpClient.PostAsJsonAsync(endpoint, dados);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(
        string endpoint,
        T dados)
    {
        return await _httpClient.PutAsJsonAsync(endpoint, dados);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        return await _httpClient.DeleteAsync(endpoint);
    }
}