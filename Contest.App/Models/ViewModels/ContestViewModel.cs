namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using Contests.Models;
    using Contests.Models.Enums;
    using Infrastructure.Mapping;
    using Microsoft.AspNet.Identity;
    using RestSharp.Extensions;

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

        public string OrganozatorId { get; set; }

        public string Category { get; set; }

        public bool CanParticipate { get; set; }

        public bool CanVote { get; set; }

        public bool HasVoted { get; set; }

        public IEnumerable<PhotoViewModel> Photos { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                .ForMember(n => n.OrganizatorName, opt => opt.MapFrom(n => n.Organizator.FullName))
                .ForMember(n => n.OrganozatorId, opt => opt.MapFrom(n => n.OrganizatorId))
                .ForMember(n => n.Category, opt => opt.MapFrom(n => n.Category.Name))
                .ForMember(n => n.CanParticipate, opt => opt.MapFrom(src =>
                    (
                    (src.ParticipationType == ParticipationType.Open &&
                    HttpContext.Current.User.Identity.IsAuthenticated &&
                    src.Organizator.UserName != HttpContext.Current.User.Identity.Name)
                    ||
                    (src.ParticipationType == ParticipationType.Closed &&
                    src.Participants.Any(p => p.UserName == HttpContext.Current.User.Identity.Name)))
                    &&
                    ((src.DeadlineType == DeadlineType.ByParticipants &&
                    src.Photos.Where(p => !p.IsDeleted).GroupBy(p => p.OwnerId).Count() <= src.ParticipantsNumberDeadline)
                    ||
                    (src.DeadlineType == DeadlineType.ByTime &&
                    src.DeadLine >= DateTime.Now))
                    ))
                .ForMember(n => n.CanVote, opt => opt.MapFrom(src => (src.VotingType == VotingType.Open && 
                    HttpContext.Current.User.Identity.IsAuthenticated)  || 
                    (src.VotingType == VotingType.Closed &&
                    HttpContext.Current.User.Identity.IsAuthenticated
                    && src.Voters.Any(v => v.UserName == HttpContext.Current.User.Identity.Name))))
                .ForMember(n => n.Photos, opt => opt.MapFrom(src => src.Photos.Where(p => p.IsDeleted == false)))
                .ForMember(n => n.HasVoted, opt => opt.MapFrom(src => src.Photos.Any(p => !p.IsDeleted && p.Votes.Any(v => v.User.UserName == HttpContext.Current.User.Identity.Name))))
                .ReverseMap();
        }
    }
}