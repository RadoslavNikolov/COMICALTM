namespace Contests.App.Models.ViewModels
{
    using Contests.Models;
    using Infrastructure.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}