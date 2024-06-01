using AutoMapper;
using LawGuardPro.Application.DTO;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;

namespace LawGuardPro.Application.Features.Users.Queries;

public class GetAddressResidenQueries : IRequest<AddressResponseResidencDTO>
{
    public int UserId { get; set; }
}

public class GetAddressResidenQueriesHandler : IRequestHandler<GetAddressResidenQueries, AddressResponseResidencDTO>
{

    private readonly IRepository<AddressUser> _repository;
    private readonly IMapper _mapper;

    public GetAddressResidenQueriesHandler(IRepository<AddressUser> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseResidencDTO> Handle(GetAddressResidenQueries request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetByIdAsync(request.UserId);
        var address = _mapper.Map<AddressResponseResidencDTO>(details);
        return address;
    }

}
