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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public static List<Products> Cart = new List<Products>();
        public CartPage()
        {
            InitializeComponent();
            LoadCart();
        }

        void LoadCart()
        {
            CartListView.ItemsSource = null;
            CartListView.ItemsSource = Cart;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var product = CartListView.SelectedItem as Products;

            if (product == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            Cart.Remove(product);
            LoadCart();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            if (string.IsNullOrWhiteSpace(AddressTxt.Text))
            {
                MessageBox.Show("Введите адрес доставки");
                return;
            }

            Orders order = new Orders
            {
                UserId = Core.AppUser?.Id ?? 0,
                Date = System.DateTime.Now,
                Address = AddressTxt.Text
            };

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            foreach (var p in Cart)
            {
                Core.Context.OrderItems.Add(new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = p.Id,
                    Quantity = 1
                });
            }

            Core.Context.SaveChanges();

            Cart.Clear();

            MessageBox.Show("Заказ оформлен!");

            LoadCart();
        }
    }
}