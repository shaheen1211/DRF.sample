using System.Threading.Tasks;
using System;
using System.Text;
using Flurl.Http;
using DRF.sample;

public class ApiClient
{
    private readonly HttpClient _client;

    public ApiClient(string baseAddress)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };        
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        HttpResponseMessage response = await _client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var httpResponse = await response.Content.ReadAsStringAsync();

        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(httpResponse, new Newtonsoft.Json.JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        });

        return result;
    }

    public async Task<T> PutAsync<T>(string endpoint, string requestBody)
    {
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var httpResponse = await _client.PutAsync(endpoint, content);
        httpResponse.EnsureSuccessStatusCode();

        var respone = await httpResponse.Content.ReadAsStringAsync();
       
        var updatedEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respone, new Newtonsoft.Json.JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        });

        return updatedEntity;
    }

    public async Task<string> PostAsync(string endpoint, string requestBody)
    {
        
        
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> AuthPostAsync(string endpoint, string requestBody, string bearerToken)
    {
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        HttpResponseMessage response = await _client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> AuthGetAsync(string endpoint,  string bearerToken)
    {
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        HttpResponseMessage response = await _client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }



    //private static async Task CaptureHttpRequest(HttpCall call)
    //{
    //    // Do something with the captured request
    //    Console.WriteLine(call.Request.Url);
    //    Console.WriteLine(call.Request.Method);
    //    Console.WriteLine(call.Request.Headers);
    //    Console.WriteLine(await call.RequestBody.ReadAsStringAsync());
    //}
}
