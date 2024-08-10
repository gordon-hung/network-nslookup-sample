using System.Net;

using System.Net.Sockets;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace NsLookupSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InfoController : ControllerBase
{
	/// <summary>
	/// Gets the asynchronous.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	/// <param name="_hostingEnvironment">The hosting environment.</param>
	/// <returns></returns>
	[HttpGet]
	public async Task<object> GetAsync(
		[FromServices] IConfiguration configuration,
		[FromServices] IWebHostEnvironment webHostEnvironment)
	{
		var hostName = Dns.GetHostName();
		var hostEntry = await Dns.GetHostEntryAsync(hostName).ConfigureAwait(false);
		var hostIp = Array.Find(hostEntry.AddressList,
			x => x.AddressFamily == AddressFamily.InterNetwork);
		return new
		{
			Data = new
			{
				Environment.MachineName,
				HostName = hostName,
				HostIp = hostIp?.ToString() ?? string.Empty,
				Environment = webHostEnvironment.EnvironmentName,
				OsVersion = $"{Environment.OSVersion}",
				Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
				ProcessCount = Environment.ProcessorCount
			}
		};
	}
}