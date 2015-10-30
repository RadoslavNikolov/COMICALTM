namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using AutoMapper;
    using Contests.Models;
    using Helpers;
    using Infrastructure.Mapping;

    public class UserInfoViewModel : IMapFrom<User>
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

        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<User, UserInfoViewModel>()
        //        //.BeforeMap((src, dest) => dest.ProfileImageUrl = Dropbox.Download(src.ProfileImage.Path))
        //        //.BeforeMap((src, dest) => dest.ThumbProfileUrl = Dropbox.Download(src.ProfileImage.ThumbnailPath))

        //        .ForMember(n => n.ProfileImageUrl, opt => opt.ResolveUsing(n => Dropbox.Download(n.ProfileImage.Path)))
        //        .ForMember(n => n.ThumbProfileUrl, opt => opt.ResolveUsing(n => Dropbox.Download(n.ProfileImage.ThumbnailPath)))
        //        .ReverseMap();
        //}

    }
}