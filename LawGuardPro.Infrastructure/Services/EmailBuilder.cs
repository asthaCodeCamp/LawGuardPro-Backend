
using MimeKit;

namespace LawGuardPro.Infrastructure.Services;

public class EmailBuilder
{
    private MimeMessage _email;
    private BodyBuilder _bodyBuilder;

    public EmailBuilder()
    {
        _email = new MimeMessage();
        _bodyBuilder = new BodyBuilder();
    }

    public EmailBuilder SetFrom(string name, string email)
    {
        _email.From.Add(new MailboxAddress(name, email));
        return this;
    }

    public EmailBuilder SetTo(string toEmail)
    {
        _email.To.Add(new MailboxAddress("", toEmail));
        return this;
    }

    public EmailBuilder SetSubject(string subject)
    {
        _email.Subject = subject;
        return this;
    }

    public EmailBuilder SetBody(string body)
    {
        _bodyBuilder.HtmlBody = body;
        _bodyBuilder.TextBody = body;
        return this;
    }

    public MimeMessage Build()
    {
        _email.Body = _bodyBuilder.ToMessageBody();
        return _email;
    }
}

