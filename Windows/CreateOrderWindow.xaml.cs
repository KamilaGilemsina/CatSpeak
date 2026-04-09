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
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private int catId;

        public CreateOrderWindow(int catId)
        {
            InitializeComponent();
            this.catId = catId;
            var cat = DBClass.connect.Cats.Find(catId);
            tbCatName.Text = cat.Name;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var order = new Orders
            {
                ClientId = App.CurrentUser.Id,
                CatId = catId,
                OrderDate = DateTime.Now,
                Status = "Новый",          
                Price = 500m               
            };
            DBClass.connect.Orders.Add(order);
            DBClass.connect.SaveChanges();
            MessageBox.Show("Заказ создан!");
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
