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

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for LinqWindow.xaml
    /// </summary>
    public partial class LinqWindow : Window
    {
        private LinqContext.DataContext context;
        public LinqWindow()
        {
            InitializeComponent();
            try
            {
                context = new(App.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => p.Price);
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void ByName_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => p.Name);
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void ByPrice_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => -p.Price);
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void Less200_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.Where(p => p.Price < 200);
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void ByPrice_Less200_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products
                .Where(p => p.Price < 200)
                .OrderBy(p => p.Price);
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void WithG_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products
                .Where(p => p.Name.Contains("Г"));
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void WithOV_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products
                .Where(p => p.Name.Contains("ов"));
            //.OrderBy(p => p.Price);
            textBlock1.Text = "";
            
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void ManagerJoinDepartment_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Managers.Join(
                context.Departments,          
                m => m.Id_main_dep,           
                d => d.Id,                    
                (m, d) =>                     
                    new { Manager = m.Surname + " " + m.Name, Department = d.Name });

            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Manager + " - " + item.Department + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void ManagerJoinChief_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Managers.Join(
                context.Managers,
                m => m.Id_chief,
                c => c.Id,
                (m, c) =>
                    new { Manager = m.Surname + " " + m.Name, Chief = c.Surname + " " + c.Name });

            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Manager + " - " + item.Chief + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
        private void ManagerJoinDepartmentJoinChief_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Managers
                           .Join(
                           context.Departments,
                           m => m.Id_main_dep,
                           d => d.Id,
                           (m, d) =>
                               new { Manager = m, Department = d })
                           .Join(
                           context.Managers,
                           m => m.Manager.Id_chief,
                           c => c.Id,
                           (m, c) =>
                               new {
                                   Manager = m.Manager.Surname + " " + m.Manager.Name,
                                   Department = m.Department.Name,
                                   Chief = c.Surname + " " + c.Name
                               });

            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Manager + " - " + item.Department + " - " + item.Chief + "\n";
            }
            textBlock1.Text += "\n" + query.Count() + " Total";
        }
    }
}

/* Задания:
* Вывести товары по цене (сделано)
* Вывести товары по названию
* Вывести товары по убывающей цене
* Вывести товары, которые дешевле 200 грн
*/