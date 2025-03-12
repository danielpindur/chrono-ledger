using ChronoLedger.Common.Extensions;
using ChronoLedger.Common.Facades;
using ChronoLedger.Journals.Commands;
using ChronoLedger.Journals.DataAccess;
using ChronoLedger.Schema.Journals;
using ChronoLedger.Users.DataAccess;

namespace ChronoLedger.Journals;

/// <summary>
/// Facade for managing journal batches.
/// </summary>
public interface IJournalBatchFacade : IFacade
{
    /// <summary>
    /// Adds a new journal batch.
    /// </summary>
    Task AddBatch(AddJournalBatchCommand command);
}

internal class JournalBatchFacade(
    IJournalBatchRepository journalBatchRepository,
    IUserRepository userRepository) : IJournalBatchFacade
{
    public async Task AddBatch(AddJournalBatchCommand command)
    {
        ValidateAddJournalBatch(command);
        
        var userId = await userRepository.ResolveUserIdAsync(command.ExternalUserId).ConfigureAwait(false);

        var journalBatch = new JournalBatchDto()
        {
            CreatedByUserId = userId,
        };

        await journalBatchRepository.CreateAsync(journalBatch).ConfigureAwait(false);
    }

    // TODO: Figure out how to do this better
    private static void ValidateAddJournalBatch(AddJournalBatchCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.ExternalUserId.IsNullOrEmpty())
        {
            throw new ArgumentException("Invalid journal batch externalUserId.");
        }
    }
}