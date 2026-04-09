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
    /// Логика взаимодействия для EditPhraseWindow.xaml
    /// </summary>
    public partial class EditPhraseWindow : Window
    {
        private CatPhrases phrase;
        private bool isNew;

        public EditPhraseWindow(CatPhrases existingPhrase = null)
        {
            InitializeComponent();
            if (existingPhrase == null)
            {
                isNew = true;
                phrase = new CatPhrases();
            }
            else
            {
                isNew = false;
                phrase = existingPhrase;
                LoadPhraseData();
            }
        }

        private void LoadPhraseData()
        {
            foreach (ComboBoxItem item in cmbDialect.Items)
                if (item.Content.ToString() == phrase.CatDialect)
                    cmbDialect.SelectedItem = item;

            tbMeowSound.Text = phrase.MeowSound;
            tbHumanTranslation.Text = phrase.HumanTranslation;

            if (phrase.DangerLevel.HasValue)
            {
                string level = phrase.DangerLevel.Value.ToString();
                foreach (ComboBoxItem item in cmbDangerLevel.Items)
                    if (item.Content.ToString() == level)
                        cmbDangerLevel.SelectedItem = item;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            phrase.CatDialect = (cmbDialect.SelectedItem as ComboBoxItem)?.Content.ToString();
            phrase.MeowSound = tbMeowSound.Text.Trim();
            phrase.HumanTranslation = tbHumanTranslation.Text.Trim();

            if (int.TryParse((cmbDangerLevel.SelectedItem as ComboBoxItem)?.Content.ToString(), out int danger))
                phrase.DangerLevel = danger;

            if (isNew)
                DBClass.connect.CatPhrases.Add(phrase);

            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
