using ChronoLedger.Common.Cash;
using ChronoLedger.Common.Extensions;
using ChronoLedger.Common.Facades;
using ChronoLedger.Journals.Commands;
using ChronoLedger.Journals.DataAccess;
using ChronoLedger.Schema.Cash;
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

        if (command.LedgerDate == DateTime.MinValue)
        {
            throw new ArgumentException("Invalid journal batch ledgerDate.");
        }

        if (command.JournalEntries.IsNullOrEmpty())
        {
            throw new ArgumentException("Cannot add journal batch without journal entries.");
        }
        
        var amountByCurrency = new Dictionary<CurrencyCode, decimal>();

        foreach (var journalEntry in command.JournalEntries)
        {
            if (journalEntry.ExternalAccountId.IsNullOrEmpty())
            {
                throw new ArgumentException("Invalid journal entry externalUserId.");
            }
            
            if (journalEntry.Amount.CurrencyCode == CurrencyCode.NotSpecified)
            {
                throw new ArgumentException("Currency not specified for journal entry.");
            }
            
            if (journalEntry.Amount.Value != decimal.Zero)
            {
                throw new ArgumentException("Invalid amount for journal entry.");
            }
            
            amountByCurrency.TryAdd(journalEntry.Amount.CurrencyCode, decimal.Zero);
            amountByCurrency[journalEntry.Amount.CurrencyCode] += journalEntry.Amount.Value;
        }

        foreach (var kvp in amountByCurrency)
        {
            if (kvp.Value != decimal.Zero)
            {
                throw new ArgumentException($"Journal entries do not balance to zero (balance: {kvp.Value}, currency: {kvp.Key})");
            }
        }
    }
}