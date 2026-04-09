using CatSpeak.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CatSpeak.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private Users currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            this.Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("Не авторизован");
                if (NavigationService != null)
                    NavigationService.Navigate(new LoginPage());
                else
                    (Application.Current.MainWindow as MainWindow)?.MainFrame.Navigate(new LoginPage());
                return;
            }

            currentUser = DBClass.connect.Users.Find(App.CurrentUser.Id);
            if (currentUser == null) return;

            tbLogin.Text = currentUser.Login;
            tbFullName.Text = currentUser.FullName;

            if (!string.IsNullOrEmpty(currentUser.AvatarPath) && File.Exists(currentUser.AvatarPath))
                imgAvatar.Source = new BitmapImage(new Uri(currentUser.AvatarPath));
            else
                imgAvatar.Source = new BitmapImage(new Uri("/Images/iconProfile.png", UriKind.Relative));

            var orders = DBClass.connect.Orders.Where(o => o.ClientId == currentUser.Id).OrderByDescending(o => o.OrderDate).ToList();
            lvOrders.ItemsSource = orders;
        }

        private void BtnSelectAvatar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Image files|*.jpg;*.png;*.bmp" };
            if (dialog.ShowDialog() == true)
            {
                string avatarsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "avatars");
                if (!Directory.Exists(avatarsDir)) Directory.CreateDirectory(avatarsDir);

                string fileName = $"{currentUser.Id}_{Guid.NewGuid()}{System.IO.Path.GetExtension(dialog.FileName)}";
                string destPath = System.IO.Path.Combine(avatarsDir, fileName);
                File.Copy(dialog.FileName, destPath, true);

                if (!string.IsNullOrEmpty(currentUser.AvatarPath) && File.Exists(currentUser.AvatarPath))
                    File.Delete(currentUser.AvatarPath);

                currentUser.AvatarPath = destPath;
                DBClass.connect.SaveChanges();
                imgAvatar.Source = new BitmapImage(new Uri(destPath));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            currentUser.FullName = tbFullName.Text.Trim();
            if (!string.IsNullOrEmpty(pbPassword.Password))
                currentUser.PasswordHash = pbPassword.Password;

            DBClass.connect.SaveChanges();
            MessageBox.Show("Профиль обновлён", "Успех");
        }
    }
}
