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
using WebServer.Classes;
using WebServer.DataAccess.Implementations.Entities;

namespace MetalAvaloniaReactive.Models;

public static class CalculationHistoryImplementation
{
    public static async Task<List<CalculationHistory>> GetAllHistoryRecords()
    {
        if (PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var accessToken = PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/CalculationHistory/Get");
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK) throw new AuthenticationException("Ошибка доступа");
        var text = await response.Content.ReadAsStringAsync();
        List<CalculationHistory> calculationHistories = JsonSerializer.Deserialize<List<CalculationHistory>>(text);
        if (calculationHistories != null)
        {
            return calculationHistories;
        }

        throw new JsonException("Произошла ошибка");

    }
    
    public static async Task AddCalculationHistory(CalculationHistory calculationHistory)
    {
        if (PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await UserImplementation.RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var taskResponse = httpClient.PostAsJsonAsync(UrlAddress.MainUrl + "/CalculationHistory/Create", calculationHistory, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }
}