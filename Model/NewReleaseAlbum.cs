
using System.Text.Json.Serialization;

public class Album
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }
    [JsonPropertyName("items")]
    public List<Item>? items { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    // public string? next { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    // public object? previous { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
}

public class Image
{
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
}

public class Item
{
    public string? id { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("total_tracks")]
    public int TotalTrack { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}

public class SpotifyAlbum
{
    [JsonPropertyName("albums")]
    public Album? Albums { get; set; }
}

