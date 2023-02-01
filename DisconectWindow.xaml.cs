using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for DisconectWindow.xaml
    /// </summary>
    public partial class DisconectWindow : Window
    {
        public ObservableCollection<Entities.Department> Departments { get; set; }
        public ObservableCollection<Entities.Product> Products { get; set; }
        public ObservableCollection<Entities.Manager> Managers { get; set; }
        public DisconectWindow()
        {
            InitializeComponent();
            DataContext = this;
            SqlConnection connection;
            connection = new(App.ConnectionString);
            try
            {

                connection.Open();

                {
                    Departments = new();
                    String sql = "SELECT id, name FROM Departments";
                    using var cmd = new SqlCommand(sql, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Departments.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString("name"),
                        });
                    }
                    reader.Close();
                }

                {
                    Products = new();
                    String sql = "SELECT id, name, price FROM Products";
                    using var cmd = new SqlCommand(sql, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Products.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString("name"),
                            Price = reader.GetDouble("price"),
                        });
                    }
                    reader.Close();
                }
                
                {
                    Managers = new();
                    String sql = "SELECT id, surname, name, secname FROM Managers";
                    using var cmd = new SqlCommand(sql, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Managers.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Surname = reader.GetString("surname"),
                            Name = reader.GetString("name"),
                            Secname = reader.GetString("secname"),
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if(sender is ListViewItem item)
            {
                if(item.Content is Entities.Product department)
                {
                    MessageBox.Show(department.GetShortString());
                }
            }
        }
        private void ListViewItem_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListViewItem item)
            {
                if (item.Content is Entities.Product product)
                {
                    MessageBox.Show(product.GetShortString());
                }
            }
        }
        private void ListViewItem_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListViewItem item)
            {
                if (item.Content is Entities.Manager manager)
                {
                    MessageBox.Show(manager.GetShortString());
                }
            }
        }
    }
}
