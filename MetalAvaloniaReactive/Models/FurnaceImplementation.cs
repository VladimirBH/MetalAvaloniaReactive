using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using WebServer.DataAccess.Implementations.Entities;

namespace MetalAvaloniaReactive.Models;

public class FurnaceImplementation
{
    public static async Task<List<Furnace>> GetAllFurnaces()
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var accessToken = PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/Furnace/Get");
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK) throw new AuthenticationException("Ошибка доступа");
        var text = await response.Content.ReadAsStringAsync();
        List<Furnace> furnaces = JsonSerializer.Deserialize<List<Furnace>>(text);
        if (furnaces != null)
        {
            return furnaces;
        }

        throw new JsonException("Произошла ошибка");

    }
    
    
    public static async Task AddFurnace(Furnace furnace)
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var taskResponse = httpClient.PostAsJsonAsync(UrlAddress.MainUrl + "/furnace/Create", furnace, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }

    public static async Task UpdateFurnace(Furnace furnace)
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var taskResponse = httpClient.PutAsJsonAsync(UrlAddress.MainUrl + $"/furnace/put", furnace, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }

    
    public static async Task DeleteFurnace(int id)
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var taskResponse = httpClient.DeleteAsync(UrlAddress.MainUrl + $"/Role/Delete/{id}");
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }
}