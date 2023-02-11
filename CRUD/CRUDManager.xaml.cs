using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Interaction logic for CRUDManager.xaml
    /// </summary>
    public partial class CRUDManager : Window
    {
        public Entities.Manager? Manager { get; set; }

        public ObservableCollection<Entities.Department>? Departments { get; set; }
        public ObservableCollection<Entities.Manager>? Managers { get; set; }
        public CRUDManager()
        {
            InitializeComponent();
            DataContext = this;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Manager is null)  // режим "C" - добавление отдела
            {
                Manager = new()
                {
                    Id = Guid.NewGuid(),
                };
                ButtonDelete.IsEnabled = false;
            }
            else  // Режимы "UD" - есть переданный отдел для изменения/удаления
            {
                ButtonDelete.IsEnabled = true;
            }

            ManagerId.Text = Manager.Id.ToString();
            ManagerSurname.Text = Manager.Surname;
            ManagerName.Text = Manager.Name;
            ManagerSecname.Text = Manager.Secname;

            if (Departments == null)
            {
                Departments = new();
            }
            if (Managers == null)
            {
                Managers = new();
            }

            foreach (var department in Departments)
            {
                if (department.Id != Manager.Id_main_dep) continue;
                ManagerDepartment.SelectedValue = department;
                break;
            }

            foreach (var department in Departments)
            {
                if (department.Id != Manager.Id_sec_dep) continue;
                ManagerSecondaryDepartment.SelectedValue = department;
                break;
            }
            foreach (var manager in Managers)
            {
                if (manager.Id != Manager.Id_chief) continue;
                ManagerChief.SelectedValue = manager;
                break;
            }

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы потвержаете удаление сотрудника из БД?", "Удалиение из БД", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }
            this.Manager = null;
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (Manager is null) return;

            if (ManagerName.Text == String.Empty)
            {
                MessageBox.Show("Ведите имя сотрудника");
                ManagerName.Focus();
                return;
            }

            var department = ManagerDepartment.SelectedValue as Entities.Department;

            if (department is null)
            {
                MessageBox.Show("Выберете Департамент");
                ManagerName.Focus();
                return;
            }

            Manager.Id_main_dep = department.Id;

            var secondaryDepartment = ManagerSecondaryDepartment.SelectedValue as Entities.Department;

            Manager.Id_sec_dep = secondaryDepartment?.Id;

            var chief = ManagerChief.SelectedValue as Entities.Department;

            Manager.Id_chief = chief?.Id;

            Manager.Surname = ManagerSurname.Text;
            Manager.Name = ManagerName.Text;
            Manager.Secname = ManagerSecname.Text;
            
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerDepartment.SelectedValue = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ManagerSecondaryDepartment.SelectedValue = null;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ManagerChief.SelectedValue = null;
        }
    }
}
