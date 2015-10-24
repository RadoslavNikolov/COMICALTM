namespace Contests.App
{
    using AutoMapper;
    using Contests.Models;
    using Models.ViewModels;

    public static class MapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.CreateMap<User, UserInfoViewModel>();
        }
    }
}