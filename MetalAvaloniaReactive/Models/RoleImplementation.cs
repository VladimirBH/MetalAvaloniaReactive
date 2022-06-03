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

namespace MetalAvaloniaReactive.Models;

public class RoleImplementation
{
    public static async Task<List<Role>> GetAllRoles()
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
        var taskResponse = httpClient.GetAsync(UrlAddress.MainUrl + "/Role/Get");
        var response = taskResponse.Result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var text = await response.Content.ReadAsStringAsync();
            List<Role> roles = JsonSerializer.Deserialize<List<Role>>(text);
            if (roles != null)
            {
                return roles;
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
    
    
        public static async Task AddRole(Role role)
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
        var taskResponse = httpClient.PostAsJsonAsync(UrlAddress.MainUrl + "/Role/CreateRole", role, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }

    public static async Task UpdateRole(Role role)
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
        var taskResponse = httpClient.PutAsJsonAsync(UrlAddress.MainUrl + $"/Role/put", role, options);
        var response = taskResponse.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new AuthenticationException(response.ToString());
        }
    }

    
    public static async Task DeleteRole(int id)
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