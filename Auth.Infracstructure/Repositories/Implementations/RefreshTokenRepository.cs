using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;

namespace Auth.Infrastructure.Repositories.Implementations;

public class RefreshTokenRepository : GenericRepository<RefreshToken, YoloAuthContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(YoloAuthContext context) : base(context)
    {
    }

    public Task<RefreshToken?> FindByTokenAsync(string token)
    {
        return _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public Task<RefreshToken?> FindByTokenIncludeAccountAsync(string token)
    {
        return _context.RefreshTokens.Include(rt => rt.Account).ThenInclude(acc => acc.Role)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }
}