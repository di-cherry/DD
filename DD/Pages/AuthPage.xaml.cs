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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void Auth_Click(object sender, RoutedEventArgs e)
        {

            Users AppUser = Core.Context.Users.
                FirstOrDefault(u => u.Login.ToLower() == LoginTxtBx.Text.ToLower()
                && u.Password == PasswPassBx.Password); 

            if (AppUser == null)
                return; 

            Core.AppUser = AppUser;
            NavigationService.Navigate(new Pages.ProductPage());
        }

        private void Guest_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new Pages.ProductPage());
    }
}
