namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class WinningContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string WallpaperUrl { get; set; }

        public string WallpaperThumbUrl { get; set; }

        //[Display(Name = "Organizator`s Name")]
        //public string OrganizatorName { get; set; }

        //public string OrganozatorId { get; set; }

        //public string Category { get; set; }

        public IEnumerable<PhotoViewModel> Photos { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, WinningContestViewModel>()
                //.ForMember(n => n.OrganizatorName, opt => opt.MapFrom(n => n.Organizator.FullName))
                //.ForMember(n => n.OrganozatorId, opt => opt.MapFrom(n => n.Organizator.Id))
                //.ForMember(n => n.Category, opt => opt.MapFrom(n => n.Category.Name))
                .ForMember(n => n.Photos, opt => opt.MapFrom(src => src.Photos.Where(p => !p.IsDeleted)))
                .ReverseMap();
        } 
    }
}