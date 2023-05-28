// create api for spotify authentication

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using spotifyApi.Repositories;
using SpotifyApi.Model;
using SpotifyApi.Repository;

namespace SpotifyApi.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyAuthentication : ControllerBase
{
    private readonly ILogger<SpotifyAuthentication> _logger;
    private readonly IConfiguration _configuration;

    private readonly ISpotifyTokenRepository _spotifyTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    private string _clientId;
    private string _clientSecret;

    public SpotifyAuthentication(ILogger<SpotifyAuthentication> logger, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _configuration = configuration;
        _clientId = _configuration["Spotify:clientId"] ?? throw new ArgumentNullException(nameof(configuration), "Spotify:clientId is null");
        _clientSecret = _configuration["Spotify:clientSecret"] ?? throw new ArgumentNullException(nameof(configuration), "Spotify:clientSecret is null");

        _unitOfWork = unitOfWork;
    }

    private static readonly Random random = new Random();
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private static string RandomString(int length)
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        var state = RandomString(16);
        var scope = "user-read-private user-read-email";
        var redirect_uri = "https://localhost:7293/api/spotify/authorized";

        var loginUrl = "https://accounts.spotify.com/authorize";

        var httpClient = new HttpClient();

        var body = new Dictionary<string, string>
        {
            {"client_id", _clientId},
            {"response_type", "code"},
            {"redirect_uri", redirect_uri},
            {"state", state},
            {"scope", scope}
        };

        var content = new FormUrlEncodedContent(body);

        var redirectUri = $"{loginUrl}?{content.ReadAsStringAsync().Result}";

        return Ok(redirectUri);

    }

    [HttpGet]
    [Route("authorized")]
    public async Task<IActionResult> SpotifyAuthorizedAsync()
    {
        var code = Request.Query["code"].ToString();

        var access_token = await GetAccessTokenAsync(code, "https://localhost:7293/api/spotify/authorized", _clientId, _clientSecret);

        var data = JsonSerializer.Deserialize<SpotifyToken>(access_token);
        string? tokenExpiryTime = null;

        if (data?.AccessToken != null)
        {
            var existingToken = await _unitOfWork.SpotifyTokens.Get(x => x.AccessToken != null);
            if (existingToken.Count() > 0)
            {
                var existingTokenList = existingToken.OrderByDescending(x => x.Id).FirstOrDefault();

                existingTokenList.AccessToken = data.AccessToken;
                existingTokenList.RefreshToken = data.RefreshToken;
                existingTokenList.ExpiresIn = data.ExpiresIn;
                existingTokenList.TokenType = data.TokenType;
                existingTokenList.Scope = data.Scope;
                existingTokenList.ExpiryTime = DateTime.Now.AddSeconds(data.ExpiresIn);
                tokenExpiryTime = existingTokenList.ExpiryTime.ToString();
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                await _unitOfWork.SpotifyTokens.Add(data);
                await _unitOfWork.SaveChangesAsync();
            }

            var redirectUrl = "http://localhost:3000?token=" + tokenExpiryTime;
            return Redirect(redirectUrl);
        }

        return BadRequest("Something went wrong");
    }

    [HttpGet]
    [Route("me")]
    public async Task<IActionResult> GetMeAsync(string email)
    {

        var existingMe = await _unitOfWork.Users.GetByExpression(x => x.Email == email); // There should be one user in the database
        if (existingMe == null)
        {
            var token = await _unitOfWork.SpotifyTokens.Get(x => x.AccessToken != null);
            if (token != null)
            {
                var existingToken = token.OrderByDescending(x => x.Id).FirstOrDefault();
                string accessToken = existingToken.AccessToken;
                var me = await GetAboutMe(accessToken);
                await _unitOfWork.Users.AddAsync(me);
                await _unitOfWork.SaveChangesAsync();
                return Ok(me);
            }

            return BadRequest("No token found");

        }
        else
        {
            return Ok(existingMe);
        }
    }

    [HttpGet]
    [Route("isAuthenticated")]
    public async Task<IActionResult> IsAuthenticatedAsync()
    {
        var token = await _unitOfWork.SpotifyTokens.GetByExpression(x => x.ExpiryTime > DateTime.Now);
        if (token == null)
        {
            return BadRequest("No user found");
        }
        else
        {
            return Ok(token.ExpiryTime);
        }
    }

    private async Task<User> GetAboutMe(string accessToken)
    {
        var data = new User();
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        var response = await httpClient.GetAsync("https://api.spotify.com/v1/me");
        var responseContent = await response.Content.ReadAsStringAsync();


        try
        {
            data = JsonSerializer.Deserialize<User>(responseContent);
        }
        catch (Exception e)
        {
            throw e;
        }
        return data;
    }

    private static async Task<string> GetAccessTokenAsync(string authorizationCode, string redirectUri, string clientId, string clientSecret)
    {
        using (var httpClient = new HttpClient())
        {
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", redirectUri }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);

            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)));
            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", encodedContent);

            var responseContent = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            else
            {
                throw new Exception($"Failed to get access token: {responseContent}");
            }
        }
    }

    private static async Task<string> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret)
    {
        using (var httpClient = new HttpClient())
        {
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);

            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)));
            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", encodedContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            else
            {
                throw new Exception($"Failed to refresh access token: {responseContent}");
            }
        }
    }


}


