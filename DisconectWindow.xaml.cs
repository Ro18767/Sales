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
                    String sql = "SELECT id, surname, name, secname, Id_main_dep, Id_sec_dep, Id_chief  FROM Managers";
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
                            Id_main_dep = reader.GetGuid("Id_main_dep"),
                            Id_sec_dep = reader[5] == DBNull.Value ? null : reader.GetGuid("Id_sec_dep"),
                            Id_chief = reader[6] == DBNull.Value ? null : reader.GetGuid("Id_chief"),
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

            if (sender is ListViewItem item)
            {
                if (item.Content is Entities.Department department)
                {
                    //MessageBox.Show(department.GetShortString());

                    this.Hide();
                    var window = new CRUD.CRUDDepartment()
                    {
                        Department = department
                    };
                    var index = Departments.IndexOf(department);

                    Departments.Remove(department);
                    if (window.ShowDialog().GetValueOrDefault())
                    {
                        if(window.Department is null)
                        {
                            SqlConnection connection;
                            connection = new(App.ConnectionString);
                            try
                            {

                                connection.Open();

                                String sql = $"DELETE FROM Departments WHERE Id = @id";
                                using var cmd = new SqlCommand(sql, connection);
                                cmd.Parameters.AddWithValue("@id", department.Id);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {


                            SqlConnection connection;
                            connection = new(App.ConnectionString);
                            try
                            {

                                connection.Open();
                                String sql = $"UPDATE Departments SET Name = @name WHERE Id = @id";
                                using var cmd = new SqlCommand(sql, connection);
                                cmd.Parameters.AddWithValue("@id", department.Id);
                                cmd.Parameters.AddWithValue("@name", window.Department.Name);
                                cmd.ExecuteNonQuery();

                                Departments.Insert(index, window.Department);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }


                    }
                    else
                    {
                        Departments.Insert(index, window.Department);
                    }
                    this.Show();
                }
            }
        }
        private void ListViewItem_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {

            {
                if (sender is ListViewItem item)
                {
                    if (item.Content is Entities.Product product)
                    {
                        MessageBox.Show(product.GetShortString());
                    }
                }
            }
            {
                if (sender is ListViewItem item)
                {
                    if (item.Content is Entities.Product product)
                    {
                        //MessageBox.Show(product.GetShortString());

                        this.Hide();
                        var window = new CRUD.CRUDProduct()
                        {
                            Product = product
                        };
                        var index = Products.IndexOf(product);

                        Products.Remove(product);
                        if (window.ShowDialog().GetValueOrDefault())
                        {
                            if (window.Product is null)
                            {
                                SqlConnection connection;
                                connection = new(App.ConnectionString);
                                try
                                {

                                    connection.Open();

                                    String sql = $"DELETE FROM Products WHERE Id = @id";
                                    using var cmd = new SqlCommand(sql, connection);
                                    cmd.Parameters.AddWithValue("@id", product.Id);
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else
                            {


                                SqlConnection connection;
                                connection = new(App.ConnectionString);
                                try
                                {

                                    connection.Open();
                                    String sql = $"UPDATE Products SET Name = @name, Price = @price WHERE Id = @id";
                                    using var cmd = new SqlCommand(sql, connection);
                                    cmd.Parameters.AddWithValue("@id", product.Id);
                                    cmd.Parameters.AddWithValue("@name", window.Product.Name);
                                    cmd.Parameters.AddWithValue("@price", window.Product.Price);
                                    cmd.ExecuteNonQuery();

                                    Products.Insert(index, window.Product);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }


                        }
                        else
                        {
                            Products.Insert(index, window.Product);
                        }
                        this.Show();
                    }
                }
            }
        }
        private void ListViewItem_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entities.Manager manager)
                {
                    //MessageBox.Show(manager.GetShortString());

                    this.Hide();
                    var window = new CRUD.CRUDManager()
                    {
                        Departments = Departments,
                        Managers = Managers,
                        Manager = manager,
                    };
                    var index = Managers.IndexOf(manager);

                    Managers.Remove(manager);
                    if (window.ShowDialog().GetValueOrDefault())
                    {
                        if (window.Manager is null)
                        {
                            SqlConnection connection;
                            connection = new(App.ConnectionString);
                            try
                            {

                                connection.Open();

                                String sql = $"DELETE FROM Managers WHERE Id = @id";
                                using var cmd = new SqlCommand(sql, connection);
                                cmd.Parameters.AddWithValue("@id", manager.Id);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {


                            SqlConnection connection;
                            connection = new(App.ConnectionString);
                            try
                            {

                                connection.Open();
                                String sql = $"UPDATE Managers SET Surname = @surname, Name = @name, Secname = @secname, Id_main_dep = @department, Id_sec_dep = @secondary_department, Id_chief = @chief WHERE Id = @id";
                                using var cmd = new SqlCommand(sql, connection);
                                cmd.Parameters.AddWithValue("@id", manager.Id);
                                cmd.Parameters.AddWithValue("@surname", window.Manager.Surname);
                                cmd.Parameters.AddWithValue("@name", window.Manager.Name);
                                cmd.Parameters.AddWithValue("@secname", window.Manager.Secname);
                                cmd.Parameters.AddWithValue("@department", window.Manager.Id_main_dep);
                                    
                                if (window.Manager.Id_sec_dep is null)
                                {
                                    cmd.Parameters.AddWithValue("@secondary_department", DBNull.Value);
                                } else
                                {
                                    cmd.Parameters.AddWithValue("@secondary_department", window.Manager.Id_sec_dep);
                                }

                                if (window.Manager.Id_chief is null)
                                {
                                    cmd.Parameters.AddWithValue("@chief", DBNull.Value);
                                } else
                                {
                                    cmd.Parameters.AddWithValue("@chief", window.Manager.Id_chief);
                                }

                                cmd.ExecuteNonQuery();

                                Managers.Insert(index, window.Manager);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }


                    }
                    else
                    {
                        Managers.Insert(index, window.Manager);
                    }
                    this.Show();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var window = new CRUD.CRUDDepartment();
            if (window.ShowDialog().GetValueOrDefault())
            {
                SqlConnection connection;
                connection = new(App.ConnectionString);
                try
                {

                    connection.Open();


                    String sql = $"INSERT INTO Departments (Id, Name) VALUES (@id, @name)";
                    using var cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", window.Department.Id);
                    cmd.Parameters.AddWithValue("@name", window.Department.Name);
                    cmd.ExecuteNonQuery();
                    Departments.Add(window.Department);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            /* 
             else
            {
                MessageBox.Show("cancel");
            }
             */
            this.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var window = new CRUD.CRUDDepartment();
            if (window.ShowDialog().GetValueOrDefault())
            {
                SqlConnection connection;
                connection = new(App.ConnectionString);
                try
                {

                    connection.Open();


                    String sql = $"INSERT INTO Departments (Id, Name) VALUES (@id, @name)";
                    using var cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", window.Department.Id);
                    cmd.Parameters.AddWithValue("@name", window.Department.Name);
                    cmd.ExecuteNonQuery();
                    Departments.Add(window.Department);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            /* 
             else
            {
                MessageBox.Show("cancel");
            }
             */
            this.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var window = new CRUD.CRUDManager()
            {
                Departments = Departments,
                Managers = Managers,
            };
            if (window.ShowDialog().GetValueOrDefault())
            {
                SqlConnection connection;
                connection = new(App.ConnectionString);
                try
                {

                    connection.Open();


                    String sql = $"INSERT INTO Managers (Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief) VALUES (@id, @surname, @name, @secname, @department, @secondary_department, @chief)";
                    using var cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", window.Manager.Id);
                    cmd.Parameters.AddWithValue("@surname", window.Manager.Surname);
                    cmd.Parameters.AddWithValue("@name", window.Manager.Name);
                    cmd.Parameters.AddWithValue("@secname", window.Manager.Secname);
                    cmd.Parameters.AddWithValue("@department", window.Manager.Id_main_dep);

                    if (window.Manager.Id_sec_dep is null)
                    {
                        cmd.Parameters.AddWithValue("@secondary_department", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@secondary_department", window.Manager.Id_sec_dep);
                    }

                    if (window.Manager.Id_chief is null)
                    {
                        cmd.Parameters.AddWithValue("@chief", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@chief", window.Manager.Id_chief);
                    }

                    cmd.ExecuteNonQuery();
                    Managers.Add(window.Manager);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Guid id = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546");
            var query = from d in Departments
                        select d.Name;
            var query2 = Departments
                .Where(d => d.Id == id)
                .Select(d => d.Name);

            textBlock1.Text = "";
            foreach (String item in query)
            {
                textBlock1.Text += item + "\n";
            }
            textBlock1.Text +=  "-----------------------\n";

            foreach (String item in query2)
            {
                textBlock1.Text += item + "\n";
            }
        }
    }
}
