using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;

namespace LawGuardPro.Application.Features.Users.Queries;

public class GetAddressBillingQueries : IRequest<AddressResponseBillingDTO>
{
    public int UserId { get; set; }
}

public class GetAddressBillingQueriesHandler : IRequestHandler<GetAddressBillingQueries, AddressResponseBillingDTO>
{

    private readonly IRepository<AddressUser> _repository;
    private readonly IMapper _mapper;

    public GetAddressBillingQueriesHandler(IRepository<AddressUser> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseBillingDTO> Handle(GetAddressBillingQueries request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetByIdAsync(request.UserId);
        var address = _mapper.Map<AddressResponseBillingDTO>(details);
        return address;
    }

}
