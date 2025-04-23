using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaekwondoRanking.Views
{
    public class FilterViewModel
    {
        public string SelectedCategory { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
