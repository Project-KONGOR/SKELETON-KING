namespace ZORGATH;

[ApiController]
[Route("/rap/report2.php")]
[Consumes("application/x-www-form-urlencoded")]
public class IncidentReportController : ControllerBase
{
    private readonly BountyContext _bountyContext;

    public IncidentReportController(BountyContext bountyContext)
    {
        _bountyContext = bountyContext;
    }

    [HttpPost(Name = "Report Incident")]
    public async Task<IActionResult> ReportIncident([FromForm] ReportAPlayer2FormData formData)
    {
        int? reporterAccountId = await _bountyContext.Accounts.Where(account => account.Cookie == formData.Cookie && account.AccountId == formData.AccountId).Select(account => account.AccountId).FirstOrDefaultAsync();
        int? accusedAccountId = await _bountyContext.Accounts.Where(account => account.Name == formData.OffenderName).Select(account => account.AccountId).FirstOrDefaultAsync();
        if (reporterAccountId is null || accusedAccountId is null || reporterAccountId == accusedAccountId)
        {
            string[] errorResponse = new string[]
            {
                // true = success, false = failure. On failure, the second parameter is ignored.
                "false",
            };
            return Ok(PHP.Serialize(errorResponse));
        }

        IncidentReport report = new()
        {
            ReporterId = reporterAccountId,
            AccusedId = accusedAccountId,
            MatchId = Convert.ToInt32(formData.MatchId),
            IncidentType = formData.Type,
            Description = formData.Body,
            OffenseOccurrenceTime = formData.Time
        };

        await _bountyContext.IncidentReports.AddAsync(report);
        await _bountyContext.SaveChangesAsync();

        string[] response = new string[2]
        {
            "true", // true = success, false = failure. On failure, the second parameter is ignored.
            "1",    // 1 = success, 2 = failure, 3 = disabled.
        };
        return Ok(PHP.Serialize(response));
    }
}
