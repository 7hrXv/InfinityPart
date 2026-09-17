using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InfinityPart.Desktop.Services
{
    /// <summary>
    /// Wrapper único de HttpClient para toda a aplicação desktop.
    /// Endpoint base configurado a partir de InfinityPart.API/Properties/launchSettings.json
    /// (perfil "http": http://localhost:5022).
    /// </summary>
    public static class ApiClient
    {
        // Ajuste aqui caso a API rode em outra porta/host.
        public const string BaseUrl = "http://localhost:5022/api";

        private static readonly HttpClient _http = new()
        {
            BaseAddress = new Uri(BaseUrl + "/"),
            Timeout = TimeSpan.FromSeconds(15)
        };

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        static ApiClient()
        {
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            var response = await _http.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }

        public static async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _http.GetAsync(endpoint);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        public static async Task<TResponse?> PostAsync<TResponse, TBody>(string endpoint, TBody body)
        {
            var content = ToJsonContent(body);
            var response = await _http.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
        }

        public static async Task<TResponse?> PutAsync<TResponse, TBody>(string endpoint, TBody body)
        {
            var content = ToJsonContent(body);
            var response = await _http.PutAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
        }

        /// <summary>
        /// POST que NÃO lança exceção em respostas de erro (ex.: 401 no login).
        /// Devolve o status e o corpo desserializado, quando houver.
        /// </summary>
        public static async Task<ApiResposta<TResponse>> PostComRespostaAsync<TResponse, TBody>(string endpoint, TBody body)
        {
            var content = ToJsonContent(body);
            var response = await _http.PostAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();

            TResponse? dados = default;

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    dados = JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
                }
                catch (JsonException)
                {
                    dados = default;
                }
            }

            return new ApiResposta<TResponse>(response.IsSuccessStatusCode, response.StatusCode, dados);
        }

        public static async Task DeleteAsync(string endpoint)
        {
            var response = await _http.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }

        private static StringContent ToJsonContent<T>(T body)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }

    /// <summary>
    /// Resultado cru de uma chamada HTTP, para quando o status importa
    /// (login inválido retorna 401 com mensagem no corpo).
    /// </summary>
    public record ApiResposta<T>(bool Sucesso, System.Net.HttpStatusCode Status, T? Dados);

}
