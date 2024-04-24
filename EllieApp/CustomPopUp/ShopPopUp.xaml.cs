using CommunityToolkit.Maui.Views;
using EllieApp.Models;
using System.Collections.ObjectModel;

namespace EllieApp.CustomPopUp;

public partial class ShopPopUp : Popup
{
	ObservableCollection<Item> pendingItems = new ObservableCollection<Item>();
	public ShopPopUp(List<Item>items)
	{
#if ANDROID
		foreach (var item in items)
		{
            pendingItems.Add(item);
        }
        InitializeComponent();
		collectionView.ItemsSource = pendingItems;
		if(pendingItems.Count == 0)
		{
            itemStatus.Text = "You have not chosen anything yet";
            redeemButton.IsVisible = false;
        } else
        {
            redeemButton.IsVisible = true;
        }
#endif
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        pendingItems.Clear();
        await CloseAsync(true);
    }
}