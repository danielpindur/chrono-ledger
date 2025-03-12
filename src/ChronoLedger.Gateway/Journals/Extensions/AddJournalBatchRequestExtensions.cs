using ChronoLedger.Gateway.Journals.Contracts;
using ChronoLedger.Journals.Commands;

namespace ChronoLedger.Gateway.Journals.Extensions;

public static class AddJournalBatchRequestExtensions
{
    public static AddJournalBatchCommand ToCommand(this AddJournalBatchRequest request)
    {
        return new AddJournalBatchCommand()
        {
            ExternalUserId = request.ExternalUserId,
            LedgerDate = new DateTime(),
            JournalEntries = new List<AddJournalBatchCommand.JournalEntry>()
        };
    }
}