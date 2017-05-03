using BusinessSpecial.Model;
using BusinessSpecial.Models;

namespace BusinessSpecial.ViewModel
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Advert Advert { get; set; }
        public ItemDetailViewModel(Advert advert = null)
        {
            if (advert != null)
            {
                Title = advert.SpecialName;
                Advert = advert;
            }
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}