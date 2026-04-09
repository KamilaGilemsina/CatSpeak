using CatSpeak.DB;
using CatSpeak.Windows;
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

namespace CatSpeak.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private List<Cats> allCats;

        public CatalogPage()
        {
            InitializeComponent();
            LoadCats();
        }

        private void LoadCats()
        {
            allCats = DBClass.connect.Cats.ToList();
            icCats.ItemsSource = allCats;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in icCats.Items)
            {
                var container = icCats.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                if (container == null) continue;

                var btnOrder = FindChild<Button>(container, "btnOrder");
                var btnDetails = FindChild<Button>(container, "btnDetails");
                var btnEdit = FindChild<Button>(container, "btnEdit");
                var btnDelete = FindChild<Button>(container, "btnDelete");
                var btnManageStatus = FindChild<Button>(container, "btnManageStatus");

                string role = App.CurrentUser?.Role;

                if (btnOrder != null) btnOrder.Visibility = (role == "Client") ? Visibility.Visible : Visibility.Collapsed;
                if (btnDetails != null) btnDetails.Visibility = Visibility.Visible; 
                if (btnEdit != null) btnEdit.Visibility = (role == "Admin") ? Visibility.Visible : Visibility.Collapsed;
                if (btnDelete != null) btnDelete.Visibility = (role == "Admin") ? Visibility.Visible : Visibility.Collapsed;
                if (btnManageStatus != null) btnManageStatus.Visibility = (role == "Linguist") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private T FindChild<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t && t.Name == name) return t;
                var result = FindChild<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = tbSearch.Text?.ToLower();
            if (string.IsNullOrEmpty(filter))
                icCats.ItemsSource = allCats;
            else
                icCats.ItemsSource = allCats.Where(c => c.Name.ToLower().Contains(filter)).ToList();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            NavigationService.Navigate(new CatDetailsPage(id));
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role != "Client")
            {
                MessageBox.Show("Только клиенты могут заказывать перевод.");
                return;
            }
            int catId = (int)((Button)sender).Tag;
            new CreateOrderWindow(catId).ShowDialog();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role != "Admin") return;
            int id = (int)((Button)sender).Tag;
            var cat = DBClass.connect.Cats.Find(id);
            new EditCatWindow(cat).ShowDialog();
            LoadCats(); 
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role != "Admin") return;
            int id = (int)((Button)sender).Tag;
            if (MessageBox.Show("Удалить кошку? Все связанные заказы будут удалены.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var cat = DBClass.connect.Cats.Find(id);
                var orders = DBClass.connect.Orders.Where(o => o.CatId == id).ToList();
                DBClass.connect.Orders.RemoveRange(orders);
                DBClass.connect.Cats.Remove(cat);
                DBClass.connect.SaveChanges();
                LoadCats();
            }
        }

        private void BtnManageStatus_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role != "Linguist") return;
            int catId = (int)((Button)sender).Tag;
            var cat = DBClass.connect.Cats.Find(catId);
            MessageBox.Show($"Статистика по диалекту '{cat.Dialect}':\n" +
                            $"Количество фраз: {DBClass.connect.CatPhrases.Count(p => p.CatDialect == cat.Dialect)}\n" +
                            $"Средний уровень опасности: ...",
                            "Диалект статистика");
        }
    }
}
