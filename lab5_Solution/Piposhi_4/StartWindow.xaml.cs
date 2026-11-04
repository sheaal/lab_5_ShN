using System.Windows;

namespace lab_4_ShN
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            EditorWindow editor = new EditorWindow();
            editor.Show();
            this.Hide();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GameWindow game = new GameWindow();
                game.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запуске игры:\n" + ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}