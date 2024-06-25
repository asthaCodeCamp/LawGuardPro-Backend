using LawGuardPro.Domain.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;

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
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 20, XFontStyle.Bold);

            gfx.DrawString("Invoice", font, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.Center);

            // Draw quote details
            font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString($"Quote Number: {quote.QuoteNumber}", font, XBrushes.Black, new XRect(40, 100, page.Width, 20));
            gfx.DrawString($"Value: {quote.Value}", font, XBrushes.Black, new XRect(40, 130, page.Width, 20));
            gfx.DrawString($"Total Value: {quote.TotalValue}", font, XBrushes.Black, new XRect(40, 160, page.Width, 20));
            gfx.DrawString($"Created On: {quote.CreatedOn}", font, XBrushes.Black, new XRect(40, 190, page.Width, 20));
            gfx.DrawString($"Status: {quote.Status}", font, XBrushes.Black, new XRect(40, 220, page.Width, 20));
            gfx.DrawString($"Payment Method: {quote.PaymentMethod}", font, XBrushes.Black, new XRect(40, 250, page.Width, 20));

            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}
