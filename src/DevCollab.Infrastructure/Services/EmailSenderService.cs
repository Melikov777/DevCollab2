using DevCollab.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace DevCollab.Infrastructure.Services;

public class EmailSenderService : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSenderService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Email:From"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["Email:Host"], int.Parse(_config["Email:Port"]!), SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"], cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}
