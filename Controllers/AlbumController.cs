
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using spotifyApi.Repositories;

namespace SpotifyApi.Controllers;
[ApiController]
[Route("api/spotify")]

public class AlbumController : ControllerBase
{
    private readonly ILogger<AlbumController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AlbumController(ILogger<AlbumController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetAllFavorites()
    {
        var albums = await _unitOfWork.FavoriteAlbums.GetAllFavorites();
        return Ok(albums);
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] FavoriteAlbumRequest album)
    {
        await _unitOfWork.FavoriteAlbums.AddFavorite(album.AlbumId);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("favorites/{id}")]
    public async Task<IActionResult> DeleteFavorite(string id)
    {
        await _unitOfWork.FavoriteAlbums.DeleteFavorite(id);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("NewReleases")]
    public async Task<IActionResult> NewReleases()
    {
        var newReleaseEndpoint = "https://api.spotify.com/v1/browse/new-releases";

        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, newReleaseEndpoint);
        var token = await _unitOfWork.SpotifyTokens.Get(x => x.AccessToken != null);
        var existingToken = token.OrderByDescending(x => x.Id).FirstOrDefault();
        string accessToken = existingToken.AccessToken;
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        var response = await client.GetAsync(newReleaseEndpoint);
        var responseContent = await response.Content.ReadAsStringAsync();

        var parsedResponse = JsonSerializer.Deserialize<SpotifyAlbum>(responseContent);

        return Ok(parsedResponse);
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<IActionResult> Search(string searchTerm)
    {
        var searchEndpoint = $"https://api.spotify.com/v1/search?q={searchTerm}&type=album";

        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, searchEndpoint);
        var token = await _unitOfWork.SpotifyTokens.Get(x => x.AccessToken != null);
        var existingToken = token.OrderByDescending(x => x.Id).FirstOrDefault();
        string accessToken = existingToken.AccessToken;
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        var response = await client.GetAsync(searchEndpoint);
        var responseContent = await response.Content.ReadAsStringAsync();

        var parsedResponse = JsonSerializer.Deserialize<SpotifyAlbum>(responseContent);

        return Ok(parsedResponse);
    }

    [HttpGet("favorites/all")]
    public async Task<IActionResult> AllFavorites()
    {

        var favoritesIds = await _unitOfWork.FavoriteAlbums.GetAllFavorites();


        var ids = favoritesIds.Select(x => x.AlbumId).Aggregate((x, y) => x + "%2C" + y);

        var albumEndpoint = $"https://api.spotify.com/v1/albums?ids={ids}";

        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, albumEndpoint);
        var token = await _unitOfWork.SpotifyTokens.Get(x => x.AccessToken != null);
        var existingToken = token.OrderByDescending(x => x.Id).FirstOrDefault();
        string accessToken = existingToken.AccessToken;
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        var response = await client.GetAsync(albumEndpoint);
        var responseContent = await response.Content.ReadAsStringAsync();

        var parsedResponse = JsonSerializer.Deserialize<FavoriteAlbums>(responseContent);

        return Ok(parsedResponse);
    }

}

public class FavoriteAlbumRequest
{
    public string? AlbumId { get; set; }
}

public class FavoriteAlbums
{
    [JsonPropertyName("albums")]
    public List<Album>? Albums { get; set; }
}

