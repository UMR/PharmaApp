namespace Pharmacy.Application.Contracts.Infrastructure;

public interface ITokenGenerationService
{
    ValueTask<string> GenerateTokenAsync(Guid pharmacyId, Guid customerId);
    ValueTask<bool> VerifyTokenAsync(string encryptedString, Guid pharmacyId, Guid customerId);

}

