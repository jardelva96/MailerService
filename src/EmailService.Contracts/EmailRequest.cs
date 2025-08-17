namespace EmailService.Contracts;

public record EmailAddress(string Name, string Address);
public record AttachmentDto(string FileName, string ContentType, byte[] Content);

public record EmailRequest(
    EmailAddress From,
    List<EmailAddress> To,
    string Subject,
    string HtmlBody,
    string? TextBody = null,
    List<EmailAddress>? Cc = null,
    List<EmailAddress>? Bcc = null,
    List<AttachmentDto>? Attachments = null,
    string? TenantId = null,
    string? MessageIdempotencyKey = null
);
