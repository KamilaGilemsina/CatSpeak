using CatSpeak.DB;
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

namespace CatSpeak.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private Users user;
        private bool isNew;
        public EditUserWindow(Users existing = null)
        {
            InitializeComponent();
            if (existing == null)
            {
                isNew = true;
                user = new Users();
            }
            else
            {
                isNew = false;
                user = existing;
                tbLogin.Text = user.Login;
                tbFullName.Text = user.FullName;
                cmbRole.SelectedItem = cmbRole.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == user.Role);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            user.Login = tbLogin.Text;
            user.FullName = tbFullName.Text;
            user.Role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(pbPassword.Password))
                user.PasswordHash = pbPassword.Password;

            if (isNew)
                DBClass.connect.Users.Add(user);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
