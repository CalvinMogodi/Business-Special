using BusinessSpecial.Models;
using BusinessSpecial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        public List<FAQ> FAQs { get; set; }
        public HelpViewModel()
        {
            Title = "Help";
        }
        public async Task GetFAQsAsync()
        {
            FAQs = await DataStore.GetFAQAsync();

            IsComplete = FAQs != null ? true : false;
        }
    }
}
