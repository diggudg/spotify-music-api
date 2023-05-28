
using SpotifyApi.DbContext;
using SpotifyApi.Repository;

namespace spotifyApi.Repositories;
public interface IUnitOfWork : IDisposable
{
    ISpotifyTokenRepository SpotifyTokens { get; }
    IUserRepository Users { get; }

    IFavoriteAlbumRepository FavoriteAlbums { get; }
    Task SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly SpotifyContext _context;
    private ISpotifyTokenRepository _spotifyTokenRepository;
    private IUserRepository _userRepository;

    private IFavoriteAlbumRepository _favoriteAlbumRepository;

    public UnitOfWork(SpotifyContext context)
    {
        _context = context;
    }

    public ISpotifyTokenRepository SpotifyTokens
    {
        get
        {
            if (_spotifyTokenRepository == null)
            {
                _spotifyTokenRepository = new SpotifyTokenRepository(_context);
            }
            return _spotifyTokenRepository;
        }
    }

    public IUserRepository Users
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
            }
            return _userRepository;

        }
    }

    public IFavoriteAlbumRepository FavoriteAlbums
    {
        get
        {
            if (_favoriteAlbumRepository == null)
            {
                _favoriteAlbumRepository = new FavoriteAlbumsRepository(_context);
            }
            return _favoriteAlbumRepository;

        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
