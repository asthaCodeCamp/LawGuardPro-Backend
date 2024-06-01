using AutoMapper;
using LawGuardPro.Application.Common;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.Features.Users.Commands;

public class CreateAddressBillingCommand : IRequest<Result<int>>
{
    public int? AddressType { get; set; }
    public string? BillingName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public int PostalCode { get; set; }
    public string Country { get; set; }

    // Foreign key property
    public int UserId { get; set; }
}
public class CreateAddressBillingCommandHandler : IRequestHandler<CreateAddressBillingCommand, Result<int>>
{
    private readonly IRepository<AddressUser> _repository;
    private readonly IMapper _mapper;

    public CreateAddressBillingCommandHandler(IRepository<AddressUser> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateAddressBillingCommand request, CancellationToken cancellationToken)
    {
        var address = _mapper.Map<AddressUser>(request);

        await _repository.AddAsync(address);

        return Result<int>.Success(address.AddressId);
    }
}