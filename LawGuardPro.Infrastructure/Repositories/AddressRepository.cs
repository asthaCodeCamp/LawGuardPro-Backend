using LawGuardPro.Domain.Entities;
using LawGuardPro.Application.Interfaces;
using LawGuardPro.Infrastructure.Persistence.Context;

namespace LawGuardPro.Infrastructure.Repositories;

public class AddressRepository(ApplicationDbContext context)
    : Repository<Address>(context), IAddressRepository
{

}
