using MediatR;
using AutoMapper;
using LawGuardPro.Application.DTO;
using Microsoft.EntityFrameworkCore;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Features.Users.Queries;

public class GetAddressBillingQuery : IRequest<AddressResponseBillingDTO>
{
    public Guid UserId { get; set; }
}

public class GetAddressBillingQueriesHandler : IRequestHandler<GetAddressBillingQuery, AddressResponseBillingDTO>
{
    private readonly IAddressRepository _repository;
    private readonly IMapper _mapper;

    public GetAddressBillingQueriesHandler(IAddressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseBillingDTO> Handle(GetAddressBillingQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository
            .FilterBy(
               address => address.UserId == request.UserId,
               address => address.AddressType == AddressType.Billing)
            .FirstOrDefaultAsync();

        var address = _mapper.Map<AddressResponseBillingDTO>(details);
        return address;
    }
}