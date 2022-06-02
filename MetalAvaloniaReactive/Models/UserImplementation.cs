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
        var taskResponse = httpClient.PostAsJsonAsync(UrlAddress.MainUrl + "/User/signin", dataAuth, options);
        var response = taskResponse.Result;
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
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/Token/RefreshAccess");
        var response = taskResponse.Result;
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

    public static async Task<List<User>> GetAllUsers()
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
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/User/Get");
        var response = taskResponse.Result;
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
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/User/GetCurrentUserInfo");
        var response = taskResponse.Result;
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

    public static async Task<User> GetUserById(int id)
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var urlString = UrlAddress.MainUrl + $"/user/get/{id}";
        var taskResponse = httpClient.GetAsync(urlString);
        var response = taskResponse.Result;
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

    public static async Task AddUser(User user)
    {
        if (!PreparedLocalStorage.CheckValidTokenInLocalStorage())
        {
            TokenPair tokenPair = await RefreshTokenPair(PreparedLocalStorage.GetTokenPairFromLocalStorage().RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair);
            PreparedLocalStorage.SaveLocalStorage();
        }
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PreparedLocalStorage.GetTokenPairFromLocalStorage().AccessToken);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var taskResponse = httpClient.PostAsJsonAsync(UrlAddress.MainUrl + "/User/CreateUser", user, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException("Ошибка доступа");
        }
    }

}