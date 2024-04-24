using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using EllieApp.CustomPopUp;
using EllieApp.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace EllieApp.Views;

public partial class PointShopPage : ContentPage
{
    List<Item> addedItems = new List<Item>();
    ObservableCollection<Item> items = new ObservableCollection<Item>() { 
        new Item { Name = "Chips", Picture = "ellie.png", Price = "200" },
        new Item { Name = "Toy", Picture = "ellie.png", Price = "2200" },
        new Item { Name = "Movie", Picture = "ellie.png", Price = "2200" },
        new Item { Name = "Game", Picture = "ellie.png", Price = "5200" },
        new Item { Name = "Console", Picture = "ellie.png", Price = "10000" },
        new Item { Name = "Comic", Picture = "ellie.png", Price = "700" }
    };
    public PointShopPage()
    {
        this.Loaded += Page_Loaded;
        BindingContext = this;
        InitializeComponent();
        collectionView.ItemsSource = items;
    }

    private void Page_Loaded(object sender, EventArgs e)
    {
        var points = Preferences.Get("points", defaultValue: "1337");
        pointsLabel.Text = points;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        ShopPopUp popup = new ShopPopUp(addedItems);
        var closedData = await this.ShowPopupAsync(popup, CancellationToken.None);
        if (closedData != null) 
        {
            double finalCost = 0;
            foreach (var item in addedItems)
            {
                finalCost += Convert.ToDouble(item.Price);
            }
            double userPoints = Convert.ToDouble(Preferences.Get("points", defaultValue: "1337"));
            userPoints = userPoints - finalCost;
            var quickObject = "{ \"points\":" + userPoints + "}";
            HttpClient httpClient = new HttpClient();
            var id = Preferences.Get("id", defaultValue: 1337);
            HttpResponseMessage response = await httpClient.PutAsync("https://totally-helpful-krill.ngrok-free.app/User?id=" + id, new StringContent(quickObject, null, "application/json"));
            Preferences.Set("points", userPoints.ToString());
            addedItems.Clear();
            List<Item> tempItems = new List<Item>();
            foreach (var item in items)
            {
                tempItems.Add(item);
            }
            items.Clear();
            foreach ( var item in tempItems)
            {
                items.Add(item);
            }
        }
    }

    private void Item_Added(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (Item)button.BindingContext;
        addedItems.Add(item);
        var cost = Convert.ToInt32(item.Price);
        var points = Convert.ToInt32(pointsLabel.Text);
        pointsLabel.Text = (points - cost).ToString();
        if (button.Parent is Grid parentGrid)
        {
            var amountLabel = parentGrid.FindByName<Label>("amountLabel");
            var amount = Convert.ToInt32(amountLabel.Text);
            amountLabel.Text = (amount + 1).ToString();
        }
    }

    private void Item_Removed(object sender, EventArgs e)
    {
        if (addedItems.Count != 0) { 
            var button = (Button)sender;
            var item = (Item)button.BindingContext;
            addedItems.Remove(item);
            var cost = Convert.ToInt32(item.Price);
            var points = Convert.ToInt32(pointsLabel.Text);
            pointsLabel.Text = (points + cost).ToString();
            if(button.Parent is Grid parentGrid)
            {
                var amountLabel = parentGrid.FindByName<Label>("amountLabel");
                var amount = Convert.ToInt32(amountLabel.Text);
                amountLabel.Text = (amount - 1).ToString();
            }
        }
    }
}