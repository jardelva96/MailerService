namespace EmailService.Domain;

public class EmailMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string ToJson { get; set; } = "[]"; // lista serializada
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Queued"; // Queued/Sent/Failed
    public string? TenantId { get; set; }
    public string? IdempotencyKey { get; set; }
}
