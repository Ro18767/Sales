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

using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;
using WpfApp12.Entities;

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window


    {
        private List<Entities.Department> _departments;

        private List<Entities.Product> _products;

        private List<Entities.Manager> _managers;

        private SqlConnection _connection;
        public MainWindow()
        {
            InitializeComponent();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\cslearn\WpfApp12\Database1.mdf;Integrated Security=True";
            _connection = new(connectionString);
        }
        private void ShowDepartmentsCount()
        {
            String sql = "SELECT COUNT(*) FROM Departments";
            using var cmd = new SqlCommand(sql, _connection);
            
            MonitorDepartments.Content= Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowProductsCount()
        {
            String sql = "SELECT COUNT(*) FROM Products";
            using var cmd = new SqlCommand(sql, _connection);

            MonitorProducts.Content= Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowManagersCount()
        {
            String sql = "SELECT COUNT(*) FROM Managers";
            using var cmd = new SqlCommand(sql, _connection);

            MonitorManagers.Content= Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowSalesCount()
        {
            String sql = "SELECT COUNT(*) FROM Sales";
            using var cmd = new SqlCommand(sql, _connection);

            MonitorSales.Content= Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowStatTotalMoney()
        {
            String sql = "SELECT SUM(Sales.Cnt * Products.Price) FROM Sales LEFT JOIN Products ON Products.ID = Sales.ID_product WHERE CAST(Moment AS DATE) = CAST(GETDATE() AS DATE)";
            using var cmd = new SqlCommand(sql, _connection);

            StatTotalMoney.Content = Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowStatTotalProducts()
        {
            String sql = "SELECT SUM(Cnt) FROM Sales WHERE CAST(Moment AS DATE) = CAST(GETDATE() AS DATE)";
            using var cmd = new SqlCommand(sql, _connection);

            StatTotalProducts.Content = Convert.ToString(cmd.ExecuteScalar());

        }
        private void ShowStatTotalSales()
        {
            String sql = "SELECT COUNT(*) FROM Sales WHERE CAST(Moment AS DATE) = CAST(GETDATE() AS DATE)";
            using var cmd = new SqlCommand(sql, _connection);

            StatTotalSales.Content = Convert.ToString(cmd.ExecuteScalar());

        }

        private void ShowDepartmens()
        {
            String sql = "SELECT * FROM Departments";
            using var cmd = new SqlCommand(sql, _connection);

            using var reader = cmd.ExecuteReader();
                while (reader.Read())
            {
                Guid id = reader.GetGuid("id");
                string name = reader.GetString(1);

                DepartmensCell.Content += $"{id} {name}\n"; 
            }
        }

        private void ShowDepartmensOrm()
        {
            if (_departments is not null) return;

            String sql = "SELECT D.id, D.name FROM Departments D";
            using var cmd = new SqlCommand(sql, _connection);

            
            try
            {
                using var reader = cmd.ExecuteReader();
                _departments = new();
                while (reader.Read())
                {
                    _departments.Add(new()
                    {
                        Id = reader.GetGuid("id"),
                        Name = reader.GetString("name"),
                    });
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            DepartmensCell.Content = "";
            
            foreach (var department in _departments)
            {
                    DepartmensCell.Content += department.GetShortString() + "\n";
            }

        }

        private void ShowProductsOrm()
        {
            if (_products is not null) return;

            String sql = "SELECT P.Id, P.Name, P.Price FROM Products P";
            using var cmd = new SqlCommand(sql, _connection);


            try
            {
                using var reader = cmd.ExecuteReader();
                _products = new();
                while (reader.Read())
                {
                    _products.Add(new()
                    {
                        Id = reader.GetGuid("Id"),
                        Name = reader.GetString("Name"),
                        Price = reader.GetDouble("Price"),
                    });
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            ProductsCell.Text = "";

            foreach (var product in _products)
            {
                ProductsCell.Text += product.GetShortString() + "\n";
            }

        }

        private void ShowManagersOrm()
        {
            if (_managers is not null) return;

            String sql = "SELECT M.Id, M.Name, M.Surname, M.Secname FROM Managers M";
            using var cmd = new SqlCommand(sql, _connection);


            try
            {
                using var reader = cmd.ExecuteReader();
                _managers = new();
                while (reader.Read())
                {
                    _managers.Add(new()
                    {
                        Id = reader.GetGuid("Id"),
                        Name = reader.GetString("Name"),
                        Surname = reader.GetString("Surname"),
                        Secname = reader.GetString("Secname"),
                    });
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            ManagersCell.Text = "";

            foreach (var product in _managers)
            {
                ManagersCell.Text += product.GetShortString() + "\n";
            }

        }
        private void ShowStatBestManager()
        {
            if (_managers is not null) return;

            String sql = "SELECT \r\n    Managers.Id\r\n    ,MAX(Managers.Name) AS Name\r\n    ,MAX(Managers.Surname) AS Surname\r\n    ,MAX(Managers.Secname) AS Secname\r\nFROM Sales\r\n    LEFT JOIN Managers ON Managers.Id = Sales.ID_manager\r\n    LEFT JOIN Products ON Products.Id = Sales.ID_product\r\nWHERE CAST(GETDATE() AS DATE) = CAST(Sales.Moment AS DATE)\r\nGROUP BY Managers.Id\r\nORDER BY SUM(Sales.Cnt * Products.Price) DESC";
            using var cmd = new SqlCommand(sql, _connection);
            StatBestManager.Content = "";
            try
            {
                using var reader = cmd.ExecuteReader();

                if (!reader.Read()) return;
                Entities.Manager manager = new()
                {
                    Id = reader.GetGuid("Id"),
                    Name = reader.GetString("Name"),
                    Surname = reader.GetString("Surname"),
                    Secname = reader.GetString("Secname"),
                };

                StatBestManager.Content = $"{manager.Name} {manager.Surname}";
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void ShowStatBestDepartment()
        {
            if (_managers is not null) return;

            String sql = "SELECT TOP (1)\r\n    Departments.Id\r\n    ,MAX(Departments.Name) AS Name\r\nFROM Sales\r\n    LEFT JOIN Managers ON Managers.Id = Sales.ID_manager\r\n    LEFT JOIN Departments ON Departments.Id = Managers.Id_main_dep\r\nGROUP BY Departments.Id\r\nORDER BY SUM(Sales.Cnt) DESC";
            using var cmd = new SqlCommand(sql, _connection);
            StatBestDepartment.Content = "";
            try
            {
                using var reader = cmd.ExecuteReader();

                if (!reader.Read()) return;
                Entities.Department department = new()
                {
                    Id = reader.GetGuid("Id"),
                    Name = reader.GetString("Name"),
                };

                StatBestDepartment.Content = department.Name;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void ShowStatBestProduct()
        {
            if (_managers is not null) return;

            String sql = "SELECT TOP (1)\r\n    Products.Id\r\n    ,MAX(Products.Name) AS Name\r\n    ,MAX(Products.Price) AS Price\r\nFROM Sales\r\n    LEFT JOIN Products ON Products.Id = Sales.ID_product\r\nWHERE CAST(GETDATE() AS DATE) = CAST(Sales.Moment AS DATE)\r\nGROUP BY Products.Id\r\nORDER BY COUNT(*) DESC";
            using var cmd = new SqlCommand(sql, _connection);
            StatBestProduct.Content = "";
            try
            {
                using var reader = cmd.ExecuteReader();

                if (!reader.Read()) return;
                Entities.Product product = new()
                {
                    Id = reader.GetGuid("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDouble("Price"),
                };

                StatBestProduct.Content = product.Name;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                MonitorConnection.Content = "Установлено";
                MonitorConnection.Foreground = Brushes.Green;

                ShowDepartmentsCount();
                ShowProductsCount();
                ShowManagersCount();
                ShowSalesCount();
                
                
                ShowStatTotalSales();
                ShowStatTotalProducts();
                ShowStatTotalMoney();
                ShowStatBestManager();
                ShowStatBestDepartment();
                ShowStatBestProduct();

                ShowDepartmensOrm();
                ShowProductsOrm();
                ShowManagersOrm();
            } catch(Exception ex) {
                MonitorConnection.Content = "Закрыто";
                MonitorConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }
       
    }
}
