using AutoMapper;

namespace authService.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Authlib.Models.Db.User, Model.Business.User>();
            CreateMap<Model.Business.User, Authlib.Models.Db.User>();

            CreateMap<Model.Api.CreateUser, Model.Business.User>();
            CreateMap<Model.Business.User, Model.Api.User>();
        }
    }
}    