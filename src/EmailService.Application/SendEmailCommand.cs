using MediatR;
using EmailService.Contracts;

namespace EmailService.Application.Emails;

public record SendEmailCommand(EmailRequest Request) : IRequest<Guid>;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Guid>
{
    public Task<Guid> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        // TODO: persistir + enfileirar
        return Task.FromResult(Guid.NewGuid());
    }
}


