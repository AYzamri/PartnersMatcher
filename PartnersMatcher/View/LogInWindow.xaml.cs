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
using PartnersMatcher.Model;

namespace PartnersMatcher.View
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private MainModel m_Model;

        public LogInWindow(MainModel model)
        {
            InitializeComponent();
            m_Model = model;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailText.Text;
            string password = PasswordText.Password;
            if (string.IsNullOrEmpty(email))
                System.Windows.Forms.MessageBox.Show("Email cannot be empty. Please re-enter");
            else if (string.IsNullOrEmpty(email))
                System.Windows.Forms.MessageBox.Show("Email cannot be empty. Please re-enter");
            else if (!IsValidEmail(email))
                System.Windows.Forms.MessageBox.Show("Invalid e-mail address");
            else
                try
                {
                    m_Model.LogIn(email, password);
                    System.Windows.Forms.MessageBox.Show("Logged In Successfully");
                    Close();
                }
                catch (Exception error)
                {
                    System.Windows.Forms.MessageBox.Show(error.Message);
                }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login_Click(sender, e);
        }
    }
}
