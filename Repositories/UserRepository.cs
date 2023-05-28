
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SpotifyApi.DbContext;
using SpotifyApi.Model;

namespace SpotifyApi.Repository;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync();

    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    void DeleteAsync(int id);
    Task<User> GetByExpression(Expression<Func<User, bool>> expression);
    Task<IEnumerable<User>> Get(
       Expression<Func<User, bool>> filter = null,
       Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
       string includeProperties = ""
   );
}

public class UserRepository : IUserRepository
{
    private readonly SpotifyContext _context;


    public UserRepository(SpotifyContext context)
    {
        _context = context;

    }

    public async Task AddAsync(User entity)
    {
        await _context.AddAsync(entity);

    }

    public void DeleteAsync(int id)
    {
        var user = _context.Users.Find(id);
        _context.Users.Remove(user);

    }

    public async Task<IEnumerable<User>> Get(Expression<Func<User, bool>> filter = null, Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, string includeProperties = "")
    {

        IQueryable<User> query = _context.Set<User>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var result = orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        return result ?? Enumerable.Empty<User>();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByExpression(Expression<Func<User, bool>> expression)
    {
        return await _context.Users.FirstOrDefaultAsync(expression);
    }

    public Task UpdateAsync(User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}