using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AvaloniaClientMVVM.Models;

namespace AvaloniaClientMetal.Models;

public class UserImplementation
{
    public static async Task<TokenPair> UserAuthorization(DataAuth dataAuth)
    {
        var httpClient = new HttpClient();
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var response = await httpClient.PostAsJsonAsync("https://localhost:7019/api/User/signin", dataAuth, options);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var text = await response.Content.ReadAsStringAsync();
            TokenPair tokenPair = JsonSerializer.Deserialize<TokenPair>(text);
            if (tokenPair != null)
            {
                return tokenPair;
            }
            else
            {
                throw new JsonException("Произошла ошибка");
            }
        }
        else
        {
            throw new AuthenticationException("Неверный логин/пароль");
        }
    }
    
    public static async Task<TokenPair> RefreshTokenPair(string refreshToken)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
        var response = await httpClient.GetAsync("https://localhost:7019/api/Token/RefreshAccess");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var text = await response.Content.ReadAsStringAsync();
            TokenPair tokenPair = JsonSerializer.Deserialize<TokenPair>(text);
            if (tokenPair != null)
            {
                return tokenPair;
            }
            else
            {
                throw new JsonException("Произошла ошибка");
            }
        }
        else
        {
            throw new AuthenticationException("Ошибка доступа");
        }
    }

    public static async Task<IEnumerable<User>> GetAllUsers()
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var accessToken = PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetAsync("https://localhost:7019/api/User/Get");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var text = await response.Content.ReadAsStringAsync();
            List<User> users = JsonSerializer.Deserialize<List<User>>(text);
            if (users != null)
            {
                return users;
            }
            else
            {
                throw new JsonException("Произошла ошибка");
            }
        }
        else
        {
            throw new AuthenticationException("Ошибка доступа");
        } 
    }

    public static async Task<User> GetCurrentUserInfo()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
        var response = await httpClient.GetAsync("https://localhost:7019/api/User/GetCurrentUserInfo");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var text = await response.Content.ReadAsStringAsync();
            User user = JsonSerializer.Deserialize<User>(text);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new JsonException("Произошла ошибка");
            }
        }
        else
        {
            throw new AuthenticationException("Ошибка доступа");
        }
    }
    
    

}