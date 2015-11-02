namespace Contests.App.Areas.Admin.Models.ViewModels
{
    using Infrastructure.Mapping;
    using Contests.Models;

    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}