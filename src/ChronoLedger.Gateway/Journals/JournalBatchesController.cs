using ChronoLedger.Gateway.Journals.Contracts;
using ChronoLedger.Gateway.Journals.Extensions;
using ChronoLedger.Gateway.Routing;
using ChronoLedger.Journals;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLedger.Gateway.Journals;

[ApiController]
[Route(ApiRoutes.Base + "/[controller]")]
public class JournalBatchesController(IJournalBatchFacade journalBatchFacade) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddJournalBatch(AddJournalBatchRequest request)
    {
        await journalBatchFacade.AddBatch(request.ToCommand());

        return Ok();
    }
}