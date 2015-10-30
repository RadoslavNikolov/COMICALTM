namespace Contests.App.Areas.Admin.Models.ViewModels
{
    using Infrastructure.Mapping;
    using Contests.Models;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}