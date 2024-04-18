using EllieApp.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace EllieApp.Views;

public partial class PointShopPage : ContentPage
{
    ObservableCollection<Item> items = new ObservableCollection<Item>() { 
        new Item { Name = "Chips", Picture = "ellie.png", Price = "200" },
        new Item { Name = "Extra ipad time", Picture = "ellie.png", Price = "1200" },
        new Item { Name = "Toy", Picture = "ellie.png", Price = "2200" },
        new Item { Name = "Movie", Picture = "ellie.png", Price = "2200" },
        new Item { Name = "Game", Picture = "ellie.png", Price = "5200" },
        new Item { Name = "Console", Picture = "ellie.png", Price = "10000" },
        new Item { Name = "Comic", Picture = "ellie.png", Price = "700" }
    };
    public PointShopPage()
	{
        BindingContext = this;
        InitializeComponent();
        collectionView.ItemsSource = items;
    }
}