using AutoMapper;
using TableTennis.Model;
using DTO.UserModel;

namespace TableTennis.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapiranje iz RegisterPost u User
            CreateMap<RegisterPost, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid())) // Ako UserId nije postavljen na klijentskoj strani
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // Ako želite postaviti datum prilikom registracije

            // Mapiranje iz User u TokenData ili druge DTO-ove ako su potrebni
            CreateMap<User, TokenData>();
        }
    }
}
