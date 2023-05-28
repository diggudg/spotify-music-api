using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SpotifyApi.DbContext;
using SpotifyApi.Model;

namespace SpotifyApi.Repository;


public interface ISpotifyTokenRepository
{
    Task<SpotifyToken> GetById(int id);
    Task Add(SpotifyToken token);
    Task Update(SpotifyToken token);
    Task Delete(SpotifyToken token);
    Task<SpotifyToken> GetByExpression(Expression<Func<SpotifyToken, bool>> expression);
    Task<IEnumerable<SpotifyToken>> Get(
        Expression<Func<SpotifyToken, bool>> filter = null,
        Func<IQueryable<SpotifyToken>, IOrderedQueryable<SpotifyToken>> orderBy = null,
        string includeProperties = ""
    );

}


public class SpotifyTokenRepository : ISpotifyTokenRepository
{
    private readonly SpotifyContext _context;

    public SpotifyTokenRepository(SpotifyContext context)
    {
        _context = context;
    }

    public async Task<SpotifyToken> GetById(int id)
    {
        return await _context.Set<SpotifyToken>().FindAsync(id);
    }

    public async Task Add(SpotifyToken token)
    {
        await _context.Set<SpotifyToken>().AddAsync(token);
    }

    public async Task Update(SpotifyToken token)
    {
        _context.Set<SpotifyToken>().Update(token);
    }

    public async Task Delete(SpotifyToken token)
    {
        _context.Set<SpotifyToken>().Remove(token);
    }

    public async Task<IEnumerable<SpotifyToken>> Get(Expression<Func<SpotifyToken, bool>> filter = null, Func<IQueryable<SpotifyToken>, IOrderedQueryable<SpotifyToken>> orderBy = null, string includeProperties = "")
    {
        IQueryable<SpotifyToken> query = _context.Set<SpotifyToken>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return orderBy != null ? orderBy(query).ToList() : query.ToList();
    }

    public async Task<SpotifyToken> GetByExpression(Expression<Func<SpotifyToken, bool>> expression)
    {
        return await _context.Set<SpotifyToken>().FirstOrDefaultAsync(expression);
    }

}
