using Pharmacy.Application.Features.PackageFeature.Dtos;

namespace Pharmacy.Application.Features.PackageFeature.Services;

public class PackageService : IPackageService
{
    #region Fields
    
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    #endregion

    #region Ctro
    
    public PackageService(IPackageRepository packageRepository, IMapper mapper)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }

    #endregion

    #region Methods
    
    public async Task<PackageDto> GetLatestAsync()
    {
        var package = await _packageRepository.GetAsync();
        var result = _mapper.Map<PackageDto>(package);

        return result;
    }

    public async Task<PackageDto> GetAsync(Guid packageId)
    {
        var package = await _packageRepository.GetAsync(packageId);
        var result = _mapper.Map<PackageDto>(package);

        return result;
    }

    #endregion
}
