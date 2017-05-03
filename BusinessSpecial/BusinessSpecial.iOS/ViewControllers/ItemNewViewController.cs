using System;
using BusinessSpecial.Helpers;
using BusinessSpecial.Model;
using BusinessSpecial.ViewModel;
using UIKit;
using BusinessSpecial.Models;

namespace BusinessSpecial.iOS
{
	public partial class ItemNewViewController : UIViewController
    {
        public Item Item { get; set; }
        public ItemsViewModel ViewModel { get; set; }

		public ItemNewViewController(IntPtr handle) : base(handle)
		{

        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            btnSaveItem.TouchUpInside += async (sender, e) =>
			{
				var _advert = new Advert();
                _advert.SpecialName = txtTitle.Text;
                _advert.User.BusinessName = txtDesc.Text;

                await ViewModel.AddItem(_advert);
                NavigationController.PopViewController(true);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

