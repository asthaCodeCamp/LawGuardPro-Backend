using MimeKit;
using System;

namespace LawGuardPro.Application.Services;

public class EmailBuilder
{
    private string _fromEmail = string.Empty;
    private string _toEmail = string.Empty;
    private string _fromName = string.Empty;
    private string _toName = string.Empty;
    private MimeEntity? _body;
    public string _subject = string.Empty;

    public EmailBuilder SetFromName(string name)
    {
        _fromName = name;
        return this;
    }

    public EmailBuilder SetToName(string name)
    {
        _toName = name;
        return this;
    }

    public EmailBuilder SetFromEmail(string email)
    {
        _fromEmail = email;
        return this;
    }

    public EmailBuilder SetToEmail(string email)
    {
        _toEmail = email;
        return this;
    }

    public EmailBuilder SetSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public EmailBuilder SetHtmlBody(string htmlBody, bool isResetPassLink, string textBody = "")
    {
        var bodyBuilder = new BodyBuilder();
        if (isResetPassLink)
        {
            bodyBuilder.HtmlBody = $"<p>Please <a href=\"{htmlBody}\" style=\"text-decoration: none; color: #0000EE;\">click here</a> to reset your password.</p>";
        }
        else 
        {
            bodyBuilder.HtmlBody = htmlBody;
        }
        bodyBuilder.TextBody = textBody;
        _body = bodyBuilder.ToMessageBody();

        return this;
    }

    public EmailBuilder SetTextBody(string textBody = "")
    {
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = textBody;
        _body = bodyBuilder.ToMessageBody();

        return this;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_fromName, _fromEmail));
        email.To.Add(new MailboxAddress(_toName, _toEmail));
        email.Subject = _subject;
        email.Body = _body;

        return email;
    }
}

