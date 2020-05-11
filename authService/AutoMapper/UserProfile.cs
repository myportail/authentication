using authService.Models.Api;
using AutoMapper;

namespace authService.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Authlib.Models.Db.User, Models.Business.User>();
            CreateMap<Models.Business.User, Authlib.Models.Db.User>();

            CreateMap<CreateUser, Models.Business.User>();
            CreateMap<Models.Business.User, User>();
        }
    }
}    