namespace Contests.App.Models.ViewModels
{
    using System;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class PhotoViewModel : IMapFrom<Photo>
    {
        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}