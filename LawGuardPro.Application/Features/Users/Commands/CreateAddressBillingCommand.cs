using MediatR;
using AutoMapper;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Services;

namespace LawGuardPro.Application.Features.Users.Commands;

public class CreateAddressBillingCommand : IRequest<Result<Guid>>
{
    public AddressType? AddressType { get; set; }
    public string? BillingName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }
}

public class CreateAddressBillingCommandHandler : IRequestHandler<CreateAddressBillingCommand, Result<Guid>>
{
    private readonly IRepository<Address> _repository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    public CreateAddressBillingCommandHandler(IRepository<Address> repository, IMapper mapper, IUserContext userContext)
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
    }



    public async Task<Result<Guid>> Handle(CreateAddressBillingCommand request, CancellationToken cancellationToken)
    {

        var UserId =  _userContext.UserId;
        
        var address = new Address
        {
            AddressType = AddressType.Billing,
            BillingName = request.BillingName,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            Town = request.Town,
            PostalCode = request.PostalCode,
            Country = request.Country,
            UserId = (Guid)UserId
        };

        await _repository.AddAsync(address);

        return Result<Guid>.Success(address.Id);
    }
}