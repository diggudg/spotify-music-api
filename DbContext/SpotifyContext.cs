namespace SpotifyApi.DbContext;

using Microsoft.EntityFrameworkCore;
using spotifyApi.Model;
using SpotifyApi.Model;

public class SpotifyContext : DbContext
{
    public SpotifyContext(DbContextOptions<SpotifyContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Artist> Artists { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<SpotifyToken> SpotifyToken { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<FavoriteAlbums> FavoriteAlbums { get; set; }
}