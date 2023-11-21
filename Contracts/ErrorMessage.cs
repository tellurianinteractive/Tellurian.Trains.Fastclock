namespace Fastclock.Contracts;

public record ErrorMessage(HttpStatusCode StatusCode, string DocumentationLink, string ErrorCode, params string[] Messages);
