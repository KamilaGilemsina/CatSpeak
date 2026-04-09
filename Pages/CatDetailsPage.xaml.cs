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

namespace CatSpeak.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatDetailsPage.xaml
    /// </summary>
    public partial class CatDetailsPage : Page
    {
        private int catId;
        public CatDetailsPage(int id)
        {
            InitializeComponent();
            catId = id;
            LoadData();
        }

        private void LoadData()
        {
            var cat = DBClass.connect.Cats.Find(catId);
            if (cat == null) return;

            tbName.Text = cat.Name;
            tbBreed.Text = $"Порода: {cat.Breed}";
            tbDialect.Text = $"Диалект: {cat.Dialect}";
            tbMood.Text = $"Настроение: {cat.Mood}";
            tbOwner.Text = $"Владелец: {cat.OwnerName}";

            var orders = DBClass.connect.Orders.Where(o => o.CatId == catId).OrderByDescending(o => o.OrderDate).ToList();
            lvOrders.ItemsSource = orders;
        }
    }
}
