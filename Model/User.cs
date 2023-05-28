using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SpotifyApi.Model;

public class User
{
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }


    [JsonPropertyName("href")]
    public string? Href { get; set; }
    public int Id { get; set; }
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }


    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

public class ExternalUrls
{
    public int Id { get; set; }
    [JsonPropertyName("spotify")]
    public string? Spotify { get; set; }
}

public class Image
{
    public int Id { get; set; }
    [JsonPropertyName("height")]
    public int? Height { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    [JsonPropertyName("width")]
    public int? Width { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
}