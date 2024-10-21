using MyApp.Models;

namespace MyApp.Models.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        public IEnumerable<Category> categories { get; set; } = new List<Category>();
    }
}
