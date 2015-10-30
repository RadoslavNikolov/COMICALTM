using Contests.App.Infrastructure.Mapping;
using Contests.Models;

namespace Contests.App.Areas.Admin.Models.ViewModels
{
    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}