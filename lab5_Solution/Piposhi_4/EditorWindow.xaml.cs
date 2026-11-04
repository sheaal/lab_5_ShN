using lab_4_ShN.Data;
using lab_4_ShN.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace lab_4_ShN
{
    public partial class EditorWindow : Window
    {
        private CEnemyTemplateList _templates = new CEnemyTemplateList();
        private List<EnemyIcon> _icons = new List<EnemyIcon>();

        public EditorWindow()
        {
            InitializeComponent();
            LoadIcons();
        }

        private void LoadIcons()
        {
            _icons.Clear();

            string imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img");
            if (!Directory.Exists(imgDir))
            {
                imgDir = "img";
            }

            if (Directory.Exists(imgDir))
            {
                foreach (var file in Directory.GetFiles(imgDir))
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                    {
                        _icons.Add(new EnemyIcon(Path.GetFileName(file), file));
                    }
                }
            }

            IconsListBox.ItemsSource = null;
            IconsListBox.ItemsSource = _icons;
        }

        private void LoadIconsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadIcons();
            if (_icons.Count == 0)
            {
                MessageBox.Show($"Иконки не найдены! Я искал тут: {Path.GetFullPath("img")}\n\nУбедитесь, что папка 'img' с картинками лежит в папке с проектом.");
            }
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IconsListBox.SelectedItem is EnemyIcon icon && File.Exists(icon.ImagePath))
            {
                EnemyIconImage.Source = new BitmapImage(new Uri(Path.GetFullPath(icon.ImagePath)));
            }
        }

        private void EnemyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArmorLabel == null || HealLabel == null) return;

            var selectedItem = (ComboBoxItem)EnemyTypeComboBox.SelectedItem;
            if (selectedItem == null) return;
            string tag = selectedItem.Tag.ToString();

            ArmorLabel.Visibility = Visibility.Collapsed;
            EnemyArmorTextBox.Visibility = Visibility.Collapsed;
            HealLabel.Visibility = Visibility.Collapsed;
            EnemyHealTextBox.Visibility = Visibility.Collapsed;

            if (tag == "Armored")
            {
                ArmorLabel.Visibility = Visibility.Visible;
                EnemyArmorTextBox.Visibility = Visibility.Visible;
            }
            else if (tag == "Regenerating")
            {
                HealLabel.Visibility = Visibility.Visible;
                EnemyHealTextBox.Visibility = Visibility.Visible;
            }
        }

        private void AddEnemyInOC(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EnemyNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EnemyHPTextBox.Text) ||
                    IconsListBox.SelectedItem == null)
                {
                    MessageBox.Show("Заполните имя, HP и выберите иконку!");
                    return;
                }

                var icon = (EnemyIcon)IconsListBox.SelectedItem;
                int level = int.Parse(EnemyLevelTextBox.Text);
                var hp = new BigNumber(EnemyHPTextBox.Text);
                var damage = new BigNumber(EnemyDamageTextBox.Text);
                var gold = new BigNumber(EnemyGoldTextBox.Text);

                var selectedItem = (ComboBoxItem)EnemyTypeComboBox.SelectedItem;
                if (selectedItem == null) { MessageBox.Show("Выберите тип!"); return; }
                string type = selectedItem.Tag.ToString();

                CEnemyTemplate newTemplate = null;

                if (type == "Normal")
                {
                    newTemplate = new CNormalEnemyTemplate(EnemyNameTextBox.Text, icon, level, hp, damage, gold, 1.0, 1.0, 1.0);
                }
                else if (type == "Armored")
                {
                    var armor = new BigNumber(EnemyArmorTextBox.Text);
                    newTemplate = new CArmoredEnemyTemplate(EnemyNameTextBox.Text, icon, level, hp, damage, gold, 1.0, 1.0, 1.0, armor);
                }
                else if (type == "Regenerating")
                {
                    int heal = int.Parse(EnemyHealTextBox.Text);
                    newTemplate = new CRegeneratingEnemyTemplate(EnemyNameTextBox.Text, icon, level, hp, damage, gold, 1.0, 1.0, 1.0, heal);
                }

                if (newTemplate != null)
                {
                    _templates.AddEnemy(newTemplate);
                    UpdateListUI();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления (проверьте, что введены числа): " + ex.Message);
            }
        }

        private void DeleteEnemyButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnemyListBox.SelectedItem is CEnemyTemplate selected)
            {
                _templates.DeleteEnemyByName(selected.Name);
                UpdateListUI();
            }
        }

        private void EnemyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnemyListBox.SelectedItem is CEnemyTemplate selected)
            {
                EnemyNameTextBox.Text = selected.Name;
                EnemyLevelTextBox.Text = selected.Level.ToString();
                EnemyHPTextBox.Text = selected.HP.ToString();
                EnemyDamageTextBox.Text = selected.Damage.ToString();
                EnemyGoldTextBox.Text = selected.Gold.ToString();

                string type = selected.TypeName;
                foreach (ComboBoxItem item in EnemyTypeComboBox.Items)
                {
                    if (item.Tag.ToString() == type)
                    {
                        EnemyTypeComboBox.SelectedItem = item;
                        break;
                    }
                }

                if (selected is CArmoredEnemyTemplate armored)
                    EnemyArmorTextBox.Text = armored.Armor.ToString();
                else if (selected is CRegeneratingEnemyTemplate regen)
                    EnemyHealTextBox.Text = regen.HealPercent.ToString();

                if (selected.Icon != null)
                {
                    IconsListBox.SelectedItem = _icons.Find(i => i.ImagePath == selected.Icon.ImagePath);
                }
            }
        }

        private void ClearForm()
        {
            EnemyNameTextBox.Clear();
            EnemyLevelTextBox.Clear();
            EnemyHPTextBox.Clear();
            EnemyDamageTextBox.Clear();
            EnemyGoldTextBox.Clear();
            EnemyArmorTextBox.Clear();
            EnemyHealTextBox.Clear();
        }

        private void UpdateListUI()
        {
            EnemyListBox.ItemsSource = null;
            EnemyListBox.ItemsSource = _templates.Enemies;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _templates.SaveAll("enemies.json");
            MessageBox.Show("Сохранено в enemies.json");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _templates.LoadAll("enemies.json");
            UpdateListUI();
            MessageBox.Show($"Загружено {_templates.Enemies.Count} врагов.");
        }
    }
}