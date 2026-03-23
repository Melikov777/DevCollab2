using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Domain.Entities;

namespace DevCollab.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Project mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        CreateMap<CreateProjectDto, Project>();

        // Review mappings
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer.UserName));
        CreateMap<CreateReviewDto, Review>();
    }
}
