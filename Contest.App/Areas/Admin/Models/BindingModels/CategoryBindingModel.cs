namespace Contests.App.Areas.Admin.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class CategoryBindingModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}