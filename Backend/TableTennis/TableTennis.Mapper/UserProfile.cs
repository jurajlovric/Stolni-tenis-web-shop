using AutoMapper;
using TableTennis.Model;
using DTO.UserModel;

namespace TableTennis.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterPost, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); 

            CreateMap<User, TokenData>();
        }
    }
}
