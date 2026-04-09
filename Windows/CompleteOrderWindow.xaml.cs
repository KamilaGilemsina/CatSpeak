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
    /// Логика взаимодействия для CompleteOrderWindow.xaml
    /// </summary>
    public partial class CompleteOrderWindow : Window
    {
        private Orders order;
        public CompleteOrderWindow(Orders order)
        {
            InitializeComponent();
            this.order = order;
            tbOrderId.Text = order.Id.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            order.FinalTranslation = tbTranslation.Text;
            order.Status = "Переведено";
            DBClass.connect.SaveChanges();
            MessageBox.Show("Перевод сохранён!");
            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
