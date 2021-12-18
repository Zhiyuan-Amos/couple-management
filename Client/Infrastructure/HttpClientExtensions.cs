using System.Text;
using System.Text.Json;

namespace Couple.Client.Infrastructure;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient,
        string requestUri,
        T data) =>
        httpClient.SendAsync(new(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

    private static HttpContent Serialize(object data) =>
        new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
}
