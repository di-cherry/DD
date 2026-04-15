using Microsoft.Win32;
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
    /// Логика взаимодействия для EditAddProductPage.xaml
    /// </summary>
    public partial class EditAddProductPage : Page
    {
        public Products ChoiceProduct { get; set; }
        public List<Categories> Categories { get; set; }

        private bool isNew = false;

        public EditAddProductPage(Products product)
        {
            InitializeComponent();

            Categories = Core.Context.Categories.ToList();

            if (product != null)
            {
                ChoiceProduct = product;
                isNew = false;
            }
            else
            {
                ChoiceProduct = new Products();
                isNew = true;
            }
            DataContext = ChoiceProduct;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ChoiceProduct.Name)
                || ChoiceProduct.Price < 0)
            {
                MessageBox.Show("Заполните корректно данные");
                return;
            }

            if (isNew)
            {
                Core.Context.Products.Add(ChoiceProduct);
            }

            Core.Context.SaveChanges();

            MessageBox.Show("Сохранено");

            NavigationService.GoBack();
        }

        private bool Check()
        {
            if (string.IsNullOrWhiteSpace(ChoiceProduct.Name)
                || ChoiceProduct.Price < 0
                || ChoiceProduct.Quantity < 0
                || ChoiceProduct.Categories == null
                || ChoiceProduct.Discount < 0
                || ChoiceProduct.Discount > 100)
            {
                MessageBox.Show("Проверьте правильность заполнения данных",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files|*.png;*.jpg";

            if (dlg.ShowDialog() == true)
            {
                ChoiceProduct.ImagePath = dlg.FileName;
            }
        }
    }
}
