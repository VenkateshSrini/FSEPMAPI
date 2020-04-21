
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoMapper;
using PMO.API.DomainModel;
using PMO.API.Messages;

namespace PMO.API.DomainService.MapProfile
{
    [ExcludeFromCodeCoverage]
    public class ProjectMapProfile:Profile
    {
        public ProjectMapProfile()
        {
            CreateMap<Project, ProjectAdd>()
                .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.End))
                .ForMember(dest => dest.PMUsrId, options => options.MapFrom(src => src.PMId))
                .ForMember(dest => dest.ProjectTitle, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.StartDate, options => options.MapFrom(src => src.Start)).ReverseMap();

            CreateMap<Project, ProjectMod>()
                .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.End))
                .ForMember(dest => dest.PMUsrId, options => options.MapFrom(src => src.PMId))
                .ForMember(dest => dest.ProjectTitle, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.StartDate, options => options.MapFrom(src => src.Start))
                .ForMember(dest => dest.ProjId, options => options.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<ProjectUserVO, ProjectListing>()
                .ForMember(dest => dest.CompletedTaskCount, options =>
                  options.MapFrom(src => src.Projects.ProjectTasks.Count(tsk => tsk.Status == -1)))
                 .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.Projects.End))
                .ForMember(dest => dest.PMUsrId, options => options.MapFrom(src => src.Projects.PMId))
                .ForMember(dest => dest.ProjectTitle, 
                options => options.MapFrom(src => src.Projects.Title))
                .ForMember(dest => dest.StartDate, options => options.MapFrom(src => src.Projects.Start))
                .ForMember(dest => dest.ProjId, options => options.MapFrom(src => src.Projects.Id))
                .ForMember(dest => dest.PMUsrName, options => options.MapFrom(src => src.UserName))
                .ForMember(dest => dest.TotalTaskCount,
                options => options.MapFrom(src => src.Projects.MaxTaskCount));



        }
    }
}
