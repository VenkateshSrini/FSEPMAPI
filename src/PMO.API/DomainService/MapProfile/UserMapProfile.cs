using AutoMapper;
using PMO.API.DomainModel;
using PMO.API.Messages;

using System.Diagnostics.CodeAnalysis;


namespace PMO.API.DomainService.MapProfile
{
    [ExcludeFromCodeCoverage]
    public class UserMapProfile: Profile
    {
        public UserMapProfile()
        {

            CreateMap<PMOUser, UserAddMsg>()
                .ForMember(dest => dest.EmployeeId, options =>
                  options.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.FirstName, options =>
                  options.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, options =>
                  options.MapFrom(dest => dest.LastName))
                .ReverseMap();

            CreateMap<PMOUser, UserModMsg>()
               .ForMember(dest => dest.EmployeeId, options =>
                 options.MapFrom(src => src.EmployeeId))
               .ForMember(dest => dest.FirstName, options =>
                 options.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, options =>
                 options.MapFrom(dest => dest.LastName))
               .ForMember(dest=>dest.Id, options=>
               options.MapFrom(dest=>dest.Id))
               .ReverseMap();
        }

    }
}
