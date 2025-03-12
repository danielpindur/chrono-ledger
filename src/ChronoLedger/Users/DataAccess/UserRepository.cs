using ChronoLedger.Common.Database;
using ChronoLedger.Common.Repositories;
using Dapper;

namespace ChronoLedger.Users.DataAccess;

internal interface IUserRepository : IRepository
{
    Task<Guid> ResolveUserIdAsync(string externalUserId);
}

internal class UserRepository(IDatabaseContext dbContext) : IUserRepository
{
    public async Task<Guid> ResolveUserIdAsync(string externalUserId)
    {
        var sql = @"
            WITH new_users AS (
                INSERT INTO users (external_user_id)
                VALUES (@ExternalUserId)
                ON CONFLICT (external_user_id) DO NOTHING
                RETURNING user_id
            )
            SELECT user_id FROM new_users
            UNION ALL
            SELECT user_id FROM users WHERE external_user_id = @ExternalUserId
            LIMIT 1;";

        return await dbContext.Connection
            .QuerySingleOrDefaultAsync<Guid>(sql, new
            {
                ExternalUserId = externalUserId
            });
    }
}