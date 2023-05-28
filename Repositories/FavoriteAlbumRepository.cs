using Microsoft.EntityFrameworkCore;
using spotifyApi.Model;
using SpotifyApi.DbContext;

namespace SpotifyApi.Repository;

public interface IFavoriteAlbumRepository
{
    Task<IReadOnlyList<FavoriteAlbums>> GetAllFavorites();
    Task DeleteFavorite(string id);
    Task AddFavorite(string albumId);
}

public class FavoriteAlbumsRepository : IFavoriteAlbumRepository
{

    private readonly SpotifyContext _context;
    public FavoriteAlbumsRepository(SpotifyContext context)
    {
        _context = context;

    }
    public async Task AddFavorite(string albumId)
    {
        await _context.FavoriteAlbums.AddAsync(new FavoriteAlbums { AlbumId = albumId });
    }

    public Task DeleteFavorite(string id)
    {
        var albumIdToDelete = _context.FavoriteAlbums.FirstOrDefault(x => x.AlbumId == id);
        if (albumIdToDelete != null)
            _context.FavoriteAlbums.Remove(albumIdToDelete);

        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<FavoriteAlbums>> GetAllFavorites()
    {
        return await _context.FavoriteAlbums.ToListAsync();
    }
}