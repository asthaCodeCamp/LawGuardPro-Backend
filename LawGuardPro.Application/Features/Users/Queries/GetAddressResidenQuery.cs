using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LawGuardPro.Application.Features.Users.Queries;

public class GetAddressResidenQuery : IRequest<AddressResponseResidencDTO>
{
    public Guid UserId { get; set; }
}

public class GetAddressResidenQueriesHandler
    : IRequestHandler<GetAddressResidenQuery, AddressResponseResidencDTO>
{

    private readonly IAddressRepository _repository;
    private readonly IMapper _mapper;

    public GetAddressResidenQueriesHandler(IAddressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseResidencDTO> Handle(GetAddressResidenQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository
                            .FilterBy(
                                user => user.UserId == request.UserId,
                                user => user.AddressType == AddressType.Residence)
                            .FirstOrDefaultAsync();

        var address = _mapper.Map<AddressResponseResidencDTO>(details);
        return address;
    }
}