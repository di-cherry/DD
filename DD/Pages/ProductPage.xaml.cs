using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DD.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private List<Products> _products;

        public ProductPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            _products = Core.Context.Products
            .Include("Categories")
            .ToList();
            ProductListBox.ItemsSource = _products;

            var categories = Core.Context.Categories.Select(c => c.Name).ToList();
            categories.Insert(0, "Все категории");

            FiltrCmbBx.ItemsSource = categories;
            FiltrCmbBx.SelectedIndex = 0;

            ApplyRole();
        }

        void ApplyRole()
        {
            var role = Core.AppUser?.RoleId;

            bool isAdmin = role == 1;
            bool isManager = role == 2;
            bool isClient = role == 3;
            bool isGuest = role == null;

            if (isGuest)
            {
                CartBtn.Visibility = Visibility.Collapsed;
                OrdersBtn.Visibility = Visibility.Collapsed;

                AdminStPanl.Visibility = Visibility.Collapsed;
            }

            else if (isClient)
            {
                CartBtn.Visibility = Visibility.Visible;
                OrdersBtn.Visibility = Visibility.Collapsed;

                AdminStPanl.Visibility = Visibility.Collapsed;
            }

            else if (isManager)
            {
                CartBtn.Visibility = Visibility.Visible;
                OrdersBtn.Visibility = Visibility.Visible;

                AdminStPanl.Visibility = Visibility.Visible;

                DeleteBtn.Visibility = Visibility.Collapsed;
            }

            else if (isAdmin)
            {
                CartBtn.Visibility = Visibility.Visible;
                OrdersBtn.Visibility = Visibility.Visible;

                AdminStPanl.Visibility = Visibility.Visible;

                DeleteBtn.Visibility = Visibility.Visible;
            }
        }

        private void Update(object sender, System.EventArgs e)
        {
            if (_products == null) return;

            var result = _products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTxtBx.Text))
            {
                string text = SearchTxtBx.Text.ToLower();

                result = result.Where(p =>
                    p.Name.ToLower().Contains(text) ||
                    p.Description.ToLower().Contains(text) ||
                    p.Categories.Name.ToLower().Contains(text));
            }

            if (FiltrCmbBx.SelectedIndex > 0)
            {
                string category = FiltrCmbBx.SelectedItem.ToString();
                result = result.Where(p => p.Categories.Name == category);
            }

            switch (SortCmbBx.SelectedIndex)
            {
                case 1:
                    result = result.OrderBy(p => p.Price);
                    break;
                case 2:
                    result = result.OrderByDescending(p => p.Price);
                    break;
            }

            ProductListBox.ItemsSource = result.ToList();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).Tag as Products;
            CartPage.Cart.Add(product);
            MessageBox.Show("Добавлено в корзину");
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAddProductPage(null));
        }

        private void RedProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductListBox.SelectedItem as Products;

            if (product == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            NavigationService.Navigate(new EditAddProductPage(product));
        }
        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
        }
        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            if (Core.AppUser == null ||
                (Core.AppUser.RoleId != 1 && Core.AppUser.RoleId != 2))
            {
                MessageBox.Show("Нет доступа к заказам");
                return;
            }

            NavigationService.Navigate(new OrdersPage());
        }

        private void DelProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductListBox.SelectedItem as Products;

            if (product == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            if (MessageBox.Show("Удалить товар?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    Core.Context.Products.Remove(product);
                    Core.Context.SaveChanges();
                    LoadData();
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления");
                }
            }
        }
    }
}
