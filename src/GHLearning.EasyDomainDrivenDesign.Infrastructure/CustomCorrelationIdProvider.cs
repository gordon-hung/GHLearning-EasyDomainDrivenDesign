using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure;

internal sealed class CustomCorrelationIdProvider() : ICorrelationIdProvider
{
	public string GenerateCorrelationId(HttpContext context)
		=> context.Request.Headers[CorrelationIdOptions.DefaultHeader].FirstOrDefault()
		?? context.Items[CorrelationIdOptions.DefaultHeader]?.ToString()
		?? SequentialGuid.SequentialGuidGenerator.Instance.NewGuid().ToString();
}