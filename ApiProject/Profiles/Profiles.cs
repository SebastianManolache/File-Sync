using Data.Models;
using Data.Models.Dtos.File;
using AutoMapper;

namespace ApiProject.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<File, FileGet>();
            CreateMap<File, FilePost>();
        }
    }
}
