using FluentEmail.Core;
using URLshortner.Domain.Exceptions;
using URLshortner.Services.Interfaces;

namespace URLshortner.Services
;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var response = await fluentEmail
            .To(toEmail)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync();

        if (!response.Successful)
        {
            throw new FailedToSendEmailException("Email sending failed.");
        }
    }
}