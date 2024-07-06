using HashGo.Domain.ViewModels;
﻿using HashGo.Domain.ViewModels.Base;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Views.Pages
{
    /// <summary>
    /// Interaction logic for ItemAddedToCartPage.xaml
    /// </summary>
    public partial class ItemAddedToCartPage : Page
    {
        public ItemAddedToCartPage(ItemAddedToCartViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
