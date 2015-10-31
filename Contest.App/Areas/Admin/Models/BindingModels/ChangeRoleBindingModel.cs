namespace Contests.App.Areas.Admin.Models.BindingModels
{
    using Contests.Models;
    using Infrastructure.Mapping;

    public class ChangeRoleBindingModel : IMapFrom<User>
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}