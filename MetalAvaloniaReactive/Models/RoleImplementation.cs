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
    
}