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
    /// Логика взаимодействия для LinguistPage.xaml
    /// </summary>
    public partial class LinguistPage : Page
    {
        public LinguistPage()
        {
            InitializeComponent();
            if (App.CurrentUser?.Role != "Linguist")
            {
                MessageBox.Show("Нет прав доступа");
                NavigationService.GoBack();
                return;
            }
            RefreshMyOrders();
            RefreshAvailableOrders();
            RefreshPhrases();
        }

        private int GetTranslatorId()
        {
            var trans = DBClass.connect.Translators.FirstOrDefault(t => t.UserId == App.CurrentUser.Id);
            return trans?.Id ?? 0;
        }

        private void RefreshMyOrders()
        {
            int tid = GetTranslatorId();
            lvMyOrders.ItemsSource = DBClass.connect.Orders.Where(o => o.TranslatorId == tid).ToList();
        }

        private void RefreshAvailableOrders()
        {
            lvAvailableOrders.ItemsSource = DBClass.connect.Orders.Where(o => o.TranslatorId == null && o.Status == "Новый").ToList();
        }

        private void RefreshPhrases() => lvPhrases.ItemsSource = DBClass.connect.CatPhrases.ToList();

        private void BtnAcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var order = DBClass.connect.Orders.Find(id);
            order.TranslatorId = GetTranslatorId();
            order.Status = "В работе";
            DBClass.connect.SaveChanges();
            RefreshMyOrders();
            RefreshAvailableOrders();
        }

        private void BtnCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var order = DBClass.connect.Orders.Find(id);
            new CompleteOrderWindow(order).ShowDialog();
            RefreshMyOrders();
        }

        private void BtnAddPhrase_Click(object sender, RoutedEventArgs e)
        {
            new EditPhraseWindow().ShowDialog();
            RefreshPhrases();
        }
    }
}
