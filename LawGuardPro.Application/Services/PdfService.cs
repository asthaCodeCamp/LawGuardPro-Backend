using LawGuardPro.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LawGuardPro.Application.Services;

public interface IPdfService
{
    byte[] GenerateQuoteInvoice(Quote quote);
}

public class PdfService : IPdfService
{
    public byte[] GenerateQuoteInvoice(Quote quote)
    {
        using (var stream = new MemoryStream())
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Invoice")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text($"Quote Number: {quote.QuoteNumber}").FontSize(20);
                            x.Item().Text($"Value: {quote.Value}").FontSize(20);
                            x.Item().Text($"Total Value: {quote.TotalValue}").FontSize(20);
                            x.Item().Text($"Created On: {quote.CreatedOn:yyyy-MM-dd}").FontSize(20);
                            x.Item().Text($"Status: {quote.Status}").FontSize(20);
                            x.Item().Text($"Payment Method: {quote.PaymentMethod}").FontSize(20);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(stream);

            return stream.ToArray();
        }
    }
}