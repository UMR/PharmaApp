﻿using AutoMapper;
using Pharmacy.Application.Features.User.Dtos;

namespace Pharmacy.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.User, UserInfoDto>().ReverseMap();
        }
    }
}
