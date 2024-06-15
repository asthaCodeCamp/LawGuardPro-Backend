namespace LawGuardPro.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICaseRepository CaseRepository { get; }
    ILawyerRepository LawyerRepository { get; }
    Task<int> CommitAsync();
}
