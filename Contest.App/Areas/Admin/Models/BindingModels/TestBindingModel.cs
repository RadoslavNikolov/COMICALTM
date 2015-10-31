namespace Contests.App.Areas.Admin.Models.BindingModels
{
    using System.Collections.Generic;

    public class TestBindingModel
    {
        public ICollection<int> Elements { get; set; }
    }
}