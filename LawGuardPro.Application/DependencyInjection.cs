using System.Reflection;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Features.Attachments.Commands;
using LawGuardPro.Application.Features.Attachments.Queries;
using LawGuardPro.Application.Features.Quotes.Commands;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LawGuardPro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(option =>
            option.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserContext, UserContext>();         
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IResetPassword, ResetPassword>();
        services.AddHttpContextAccessor();
        services.AddScoped<IRequestHandler<SaveAttachmentCommand, IResult<string>>, SaveAttachmentCommandHandler>();
        services.AddScoped<IRequestHandler<GetAttachmentListByCaseIdQuery, IResult<List<AttachmentDto>>>, GetAttachmentListByCaseIdQueryHandler>();
        services.AddTransient<IPdfService, PdfService>();
        services.AddTransient<IRequestHandler<GenerateQuoteInvoiceCommand, IResult<byte[]>>, GenerateQuoteInvoiceCommandHandler>();

        return services;
    }
}