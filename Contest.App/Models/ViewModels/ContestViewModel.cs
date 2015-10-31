namespace Contests.App.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class ContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string WallpaperUrl { get; set; }

        public string WallpaperThumbUrl { get; set; }

        [Display(Name = "Organizator`s Name")]
        public string OrganizatorName { get; set; }

        public string Category { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
 
            configuration.CreateMap<Contest, ContestViewModel>()
                .ForMember(n => n.OrganizatorName, opt => opt.MapFrom(n => n.Organizator.FullName))
                .ForMember(n => n.Category, opt => opt.MapFrom(n => n.Category.Name))
                .ReverseMap();
        }
    }
}