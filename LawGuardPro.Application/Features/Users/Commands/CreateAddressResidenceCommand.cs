using MediatR;
using AutoMapper;
using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Common;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Interfaces;
using System.Diagnostics.Metrics;

namespace LawGuardPro.Application.Features.Users.Commands;

public class CreateAddressResidenceCommand : IRequest<IResult<Guid>>
{
    public AddressType AddressType { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }
}

public class CreateAddressResidenceCommandHandler : IRequestHandler<CreateAddressResidenceCommand, IResult<Guid>>
{
    private readonly IRepository<Address> _repository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    public CreateAddressResidenceCommandHandler(
        IRepository<Address> repository,
        IMapper mapper,
        IUserContext userContext)
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IResult<Guid>> Handle(CreateAddressResidenceCommand request, CancellationToken cancellationToken)
    {
        var UserId = _userContext.UserId;
        
        var curAddress = await _repository.GetFirstAsync(address => address.UserId == UserId &&   address.AddressType == AddressType.Residence);

        var address = new Address
        {
            AddressType = AddressType.Residence,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            Town = request.Town,
            PostalCode = request.PostalCode,
            Country = request.Country,
            UserId = (Guid)UserId!,
        };
        if (curAddress == null)
        {
            await _repository.AddAsync(address);
        }
        else
        {
            curAddress.AddressLine1 = request.AddressLine1;
            //curAddress.AddressType = AddressType.Residence;
            curAddress.AddressLine2 = request.AddressLine2;
            curAddress.Town = request.Town;
            curAddress.PostalCode = request.PostalCode;
            curAddress.Country = request.Country;
            await _repository.UpdateAsync(curAddress);
        }
       
        return Result<Guid>.Success(address.Id);
    }
}