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
    /// Логика взаимодействия для EditCatWindow.xaml
    /// </summary>
    public partial class EditCatWindow : Window
    {
        private Cats cat;
        private bool isNew;

        public EditCatWindow(Cats existingCat = null)
        {
            InitializeComponent();
            if (existingCat == null)
            {
                isNew = true;
                cat = new Cats();
            }
            else
            {
                isNew = false;
                cat = existingCat;
                LoadCatData();
            }
        }

        private void LoadCatData()
        {
            tbName.Text = cat.Name;
            tbBreed.Text = cat.Breed;
            tbOwnerName.Text = cat.OwnerName;
            tbPhotoUrl.Text = cat.PhotoUrl;

            foreach (ComboBoxItem item in cmbDialect.Items)
                if (item.Content.ToString() == cat.Dialect)
                    cmbDialect.SelectedItem = item;

            foreach (ComboBoxItem item in cmbMood.Items)
                if (item.Content.ToString() == cat.Mood)
                    cmbMood.SelectedItem = item;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            cat.Name = tbName.Text.Trim();
            cat.Breed = tbBreed.Text.Trim();
            cat.Dialect = (cmbDialect.SelectedItem as ComboBoxItem)?.Content.ToString();
            cat.Mood = (cmbMood.SelectedItem as ComboBoxItem)?.Content.ToString();
            cat.OwnerName = tbOwnerName.Text.Trim();
            cat.PhotoUrl = tbPhotoUrl.Text.Trim();

            if (isNew)
                DBClass.connect.Cats.Add(cat);

            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
