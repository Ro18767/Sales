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
using System.Windows.Shapes;
using WpfApp12.Entities;

namespace WpfApp12.CRUD
{
    /// <summary>
    /// Interaction logic for CRUDProduct.xaml
    /// </summary>
    public partial class CRUDProduct : Window
    {
        public Entities.Product? Product { get; set; }

        public CRUDProduct()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Product is null)  // режим "C" - добавление отдела
            {
                Product = new()
                {
                    Id = Guid.NewGuid(),
                };
                ButtonDelete.IsEnabled = false;
            }
            else  // Режимы "UD" - есть переданный отдел для изменения/удаления
            {
                ButtonDelete.IsEnabled = true;
            }
            ProductId.Text = Product.Id.ToString();
            ProductName.Text = Product.Name;
            ProductPrice.Text = Product.Price.ToString();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы потвержаете удаление отдела из БД?", "Удалиение из БД", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }
            this.Product = null;
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (Product is null) return;

            if (ProductName.Text == String.Empty)
            {
                MessageBox.Show("Ведите Название Отдела");
                ProductName.Focus();
                return;
            }

            if (ProductPrice.Text == String.Empty)
            {
                MessageBox.Show("Ведите Цену");
                ProductPrice.Focus();
                return;
            }
            try
            {
                Convert.ToDouble(ProductPrice.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ведите Число");
                ProductPrice.Focus();
                return;
            }

            Product.Name = ProductName.Text;
            Product.Price = Convert.ToDouble(ProductPrice.Text);
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}