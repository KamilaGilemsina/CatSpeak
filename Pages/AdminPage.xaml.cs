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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CatSpeak.Windows;

namespace CatSpeak.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            if (App.CurrentUser?.Role != "Admin")
            {
                MessageBox.Show("Нет прав доступа");
                NavigationService.GoBack();
                return;
            }
            RefreshUsers();
        }

        private void RefreshUsers() => lvUsers.ItemsSource = DBClass.connect.Users.ToList();

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            new EditUserWindow().ShowDialog();
            RefreshUsers();
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var user = DBClass.connect.Users.Find(id);
            new EditUserWindow(user).ShowDialog();
            RefreshUsers();
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var user = DBClass.connect.Users.Find(id);
            if (user == null) return;

            if (MessageBox.Show($"Удалить пользователя {user.Login}? Все его заказы и логи также будут удалены.",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            var orders = DBClass.connect.Orders.Where(o => o.ClientId == id).ToList();
            DBClass.connect.Orders.RemoveRange(orders);

            var logs = DBClass.connect.ActionLogs.Where(l => l.UserId == id).ToList();
            DBClass.connect.ActionLogs.RemoveRange(logs);

            var translator = DBClass.connect.Translators.FirstOrDefault(t => t.UserId == id);
            if (translator != null)
                DBClass.connect.Translators.Remove(translator);

            DBClass.connect.Users.Remove(user);

            DBClass.connect.SaveChanges();

            RefreshUsers();
            MessageBox.Show("Пользователь успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
