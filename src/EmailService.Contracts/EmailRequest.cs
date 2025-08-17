using System;
using System.Collections.Generic;

namespace EmailService.Contracts;

public sealed record EmailAddress(string? Name, string Address);

public sealed record AttachmentDto(string FileName, string ContentType, ReadOnlyMemory<byte> Content);

public sealed record EmailRequest(
    EmailAddress From,
    IReadOnlyList<EmailAddress> To,
    string Subject,
    string HtmlBody,
    string? TextBody = null,
    IReadOnlyList<EmailAddress>? Cc = null,
    IReadOnlyList<EmailAddress>? Bcc = null,
    IReadOnlyList<AttachmentDto>? Attachments = null,
    string? TenantId = null,
    string? MessageIdempotencyKey = null
);
