using DnsClient;

using Microsoft.AspNetCore.Mvc;

namespace NsLookupSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NslookupController : ControllerBase
{
	/// <summary>
	/// Gets the nslookup asynchronous.
	/// </summary>
	/// <param name="lookupClient">The lookup client.</param>
	/// <param name="domain">The domain.</param>
	/// <param name="queryType">Type of the query.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public async IAsyncEnumerable<string> GetNslookupAsync(
		[FromServices] ILookupClient lookupClient,
		string domain = "google.com",
		[FromQuery] string queryType = "A")
	{
		var result = await lookupClient.QueryAsync(
			query: domain,
			queryType: (QueryType)Enum.Parse(typeof(QueryType), queryType),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
		if (result.Answers.ARecords().Any())
		{
			foreach (var aRecord in result.Answers.ARecords())
			{
				yield return aRecord.ToString();
			}
		}
		else
		{
			yield return $"UnKnown 找不到 {domain}: Non-existent domain";
		}
	}
}