using AutoMapper;
using Pharmacy.Application.Features.Customer.Dtos;
using Pharmacy.Application.Features.PackageFeature.Dtos;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.User, UserInfoDto>().ReverseMap();
            CreateMap<Domain.Pharmacy, PharmacyDetailsDto>().ReverseMap();
            CreateMap<Domain.Customer, CustomerResDto>().ReverseMap();
            CreateMap<Domain.Package, PackageDto>().ReverseMap();
                    CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }
}
