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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            LoadData();
            CheckRole();
        }

        void LoadData()
        {
            OrdersListView.ItemsSource = Core.Context.Orders.ToList();
        }

        void CheckRole()
        {
            if (Core.AppUser == null || Core.AppUser.RoleId != 1)
            {
                AdminPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var order = OrdersListView.SelectedItem as Orders;

            if (order == null)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            if (MessageBox.Show("Удалить заказ?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Core.Context.Orders.Remove(order);
                Core.Context.SaveChanges();
                LoadData();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
