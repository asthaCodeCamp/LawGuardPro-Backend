namespace LawGuardPro.Application.Interfaces;

public interface IOtpService
{
    Task<string> GenerateAndSaveTotp(string email, string uid);
    Task<bool> ValidateAndMarkAsUsed(string email, string givenOTP, int validityInMinutes);
}


