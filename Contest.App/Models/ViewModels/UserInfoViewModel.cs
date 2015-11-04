namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using Contests.Models;
    using Helpers;
    using Infrastructure.Mapping;

    public class UserInfoViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "User name: ")]
        public string UserName { get; set; }

        [Display(Name = "Full name: ")]
        public string FullName { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Display(Name = "Phone number: ")]
        public string PhoneNumber { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime RegisteredOn { get; set; }

        public IEnumerable<ContestViewModel> Contest { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserInfoViewModel>()
                .ForMember(u => u.Contest, opt => opt.MapFrom(u => u.ContestsCreated.Where(c => c.IsActive)))
                .ReverseMap();
        }
    }
}