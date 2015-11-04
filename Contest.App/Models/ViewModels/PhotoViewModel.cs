namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class PhotoViewModel : IMapFrom<Photo>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public int VotesCount { get; set; }

        public bool IsWinner { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Photo, PhotoViewModel>()
                .ForMember(src => src.VotesCount, opt => opt.MapFrom(v => v.Votes.Any() ? v.Votes.Count : 0));
        }
    }
}