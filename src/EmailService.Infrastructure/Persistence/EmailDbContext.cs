using Microsoft.EntityFrameworkCore;
using EmailService.Domain;

namespace EmailService.Infrastructure.Persistence;

public class EmailDbContext : DbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }
    public DbSet<EmailMessage> Emails => Set<EmailMessage>();
}
