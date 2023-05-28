using SpotifyApi.Model;
namespace SpotifyApi.Interface;

public interface ISpotifyAuthentication
{
    public Task<SpotifyToken> GetAccessToken(string clientId, string clientSecret, string code, string redirectUri);
    public Task<SpotifyToken> RefreshAccessToken(string clientId, string clientSecret, string refreshToken);
    public Task<SpotifyToken> GetAccessTokenFromCode(string clientId, string clientSecret, string code, string redirectUri);
}
