using System;
using UIKit;

using BusinessSpecial.ViewModel;

namespace BusinessSpecial.iOS
{
    public partial class BrowseItemDetailViewController : UIViewController
    {
		public ItemDetailViewModel ViewModel { get; set; }
		public BrowseItemDetailViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = ViewModel.Title;
			ItemNameLabel.Text = ViewModel.Advert.SpecialName;
			ItemDescriptionLabel.Text = ViewModel.Advert.User.BusinessName;

		}


    }
}