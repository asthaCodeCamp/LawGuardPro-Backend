using MediatR;
using AutoMapper;
using LawGuardPro.Application.DTO;
using Microsoft.EntityFrameworkCore;
using LawGuardPro.Domain.Common.Enums;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Features.Users.Queries;

public class GetAddressResidenceQuery : IRequest<AddressResponseResidencDTO>
{
    public Guid UserId { get; set; }
}

public class GetAddressResidenceQueryHandler
    : IRequestHandler<GetAddressResidenceQuery, AddressResponseResidencDTO>
{
    private readonly IAddressRepository _repository;
    private readonly IMapper _mapper;

    public GetAddressResidenceQueryHandler(IAddressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseResidencDTO> Handle(GetAddressResidenceQuery request, CancellationToken cancellationToken)
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