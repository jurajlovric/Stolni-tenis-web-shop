using AutoMapper;
using TableTennis.Model;
using DTO.UserModel;

namespace TableTennis.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapiranje iz RegisterPost DTO-a u User model
            CreateMap<RegisterPost, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // ID se generira u servisu
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Datum se postavlja u servisu
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)); // Osigurava da se password mapira ispravno

            // Mapiranje iz User modela u TokenData DTO
            CreateMap<User, TokenData>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()); // Uloga se popunjava dodatno

            // Mapiranje iz LoginPost DTO-a u UserLogin ili sličan model (ako postoji)
            // Ako imate poseban model za login podatke, možete prilagoditi mapiranje prema potrebi
            CreateMap<LoginPost, User>();
        }
    }
}
