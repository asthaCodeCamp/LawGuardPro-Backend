using LawGuardPro.Application.Interfaces;
using LawGuardPro.Domain.Entities;
using Microsoft.Extensions.Configuration;
using OtpNet;
using SimpleBase;
using System.Security.Cryptography;
using System.Text;

namespace LawGuardPro.Infrastructure.Services;

public class OtpService: IOtpService
{
    private readonly byte[] secretKey;
    private readonly IRepository<UserOTP> _otpRepository;

    public OtpService(IConfiguration configuration,
        IRepository<UserOTP> otpRepository)
    {
        var jwtKey = configuration.GetValue<string>("Jwt:Key");
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ArgumentException("Jwt:Key configuration value is missing.");
        }
        using (var sha256 = SHA256.Create())
        {
            secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
        }
        _otpRepository = otpRepository;
    }

    public async Task<string> GenerateAndSaveTotp(string email, string uid)
    {
        try
        {
            var totp = new Totp(secretKey);
            var otp = totp.ComputeTotp();
            var userOTP = new UserOTP
            {
                Email = email,
                UId = uid,
                OTP = otp,
                CreatedTime = DateTime.UtcNow
            };

            await _otpRepository.AddAsync(userOTP);
            return otp;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error generating or saving TOTP.", ex);
        }
    }

    public async Task<bool> ValidateAndMarkAsUsed(string email, string givenOTP, int validityInMinutes)
    {
        var totp = new Totp(secretKey);
        int timeSteps = (validityInMinutes * 60) / totp.Step;
        var verificationWindow = new VerificationWindow(previous: timeSteps, future: timeSteps);
        bool isValid = totp.VerifyTotp(givenOTP, out _, verificationWindow);

        if (isValid)
        {
            var userOTP = await _otpRepository.GetFirstAsync(o => o.Email == email && o.OTP == givenOTP && !o.IsUsed && o.CreatedTime.AddMinutes(validityInMinutes) >= DateTime.UtcNow);

            if (userOTP != null)
            {
                userOTP.IsUsed = true;
                await _otpRepository.UpdateAsync(userOTP);
            }
            else
            {
                isValid = false;
            }
        }

        return isValid;
    }
}

