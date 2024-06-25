using System;
using System.Text;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using MimeKit;

namespace LawGuardPro.Application.Services;

public class EmailBuilder
{
    private string _fromEmail = string.Empty;
    private string _toEmail = string.Empty;
    private string _fromName = string.Empty;
    private string _toName = string.Empty;
    private readonly BodyBuilder _bodyBuilder;
    public string _subject = string.Empty;

    public EmailBuilder()
    {
        _bodyBuilder = new BodyBuilder();
    }

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

    public EmailBuilder SetHtmlBody(string htmlBody, string textBody = "")
    {
        _bodyBuilder.HtmlBody = htmlBody;
        _bodyBuilder.TextBody = textBody;

        return this;
    }

    public EmailBuilder AddAttachmentsIfHas(IList<EmailAttachmentDto> attachments)
    {
        if (attachments is null || !attachments.Any()) return this;

        var content = File.ReadAllBytes(@"C:\Users\Public\New folder\repos\New folder (2)\LawGuardPro-Backend\LawGuardPro.API\wwwroot\files\resume.pdf");

        foreach (var attachment in attachments)
        {
            _bodyBuilder
                .Attachments
                .Add(attachment.FileName,
                        content,
                        ContentType.Parse(attachment.ContentType));
        }

        return this;
    }

    public EmailBuilder SetTextBody(string textBody = "")
    {
        _bodyBuilder.TextBody = textBody;
        return this;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_fromName, _fromEmail));
        email.To.Add(new MailboxAddress(_toName, _toEmail));
        email.Subject = _subject;
        email.Body = _bodyBuilder.ToMessageBody();

        return email;
    }
}