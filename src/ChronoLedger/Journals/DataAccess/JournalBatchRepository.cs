using ChronoLedger.Common.Database;
using ChronoLedger.Common.Repositories;
using ChronoLedger.Schema.Journals;
using Dapper;

namespace ChronoLedger.Journals.DataAccess;

internal interface IJournalBatchRepository : IRepository
{
    Task CreateAsync(JournalBatchDto journalBatch);
}

internal class JournalBatchRepository(IDatabaseContextProvider contextProvider) : IJournalBatchRepository
{
    public async Task CreateAsync(JournalBatchDto journalBatch)
    {
        var sql = @"
            INSERT INTO journal_batches
            (created_by_user_id)
            VALUES (
                @CreatedByUserId
            )
            RETURNING journal_batch_id;
        ";

        var dbContext = await contextProvider.GetContextAsync().ConfigureAwait(false);
        
        journalBatch.JournalBatchId = await dbContext.Connection
            .ExecuteScalarAsync<long>(sql, new
            {
                journalBatch.CreatedByUserId
            }).ConfigureAwait(false);
    }
}