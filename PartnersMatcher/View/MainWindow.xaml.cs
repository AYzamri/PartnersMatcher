using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using PartnersMatcher.Model;
using System.Windows.Controls;

namespace PartnersMatcher.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainModel m_Model = new MainModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = m_Model;
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(m_Model).Show();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LogInWindow(m_Model).Show();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (m_Model.CurrentUser == null)
                System.Windows.Forms.MessageBox.Show("Please log in or sign up to search for partners");
            else
            {
                m_Model.Search(TopicsComboBox.SelectionBoxItem.ToString(), LocationText.Text);
                SearchResults.Columns[0].Visibility = Visibility.Hidden;
            } 
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search_Click(sender, e);
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }
    }
}
