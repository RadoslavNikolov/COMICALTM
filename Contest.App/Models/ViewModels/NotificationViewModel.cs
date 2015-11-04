namespace Contests.App.Models.ViewModels
{
    using System;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime Date { get; set; }
    }
}