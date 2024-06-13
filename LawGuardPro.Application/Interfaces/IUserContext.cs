namespace LawGuardPro.Application.Interfaces;

public interface IUserContext
{
    public string? Email { get; }
    public Guid? UserId { get; }
}