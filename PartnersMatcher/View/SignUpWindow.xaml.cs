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
using System.Text.RegularExpressions;

namespace PartnersMatcher.View
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private MainModel m_Model;

        public SignUpWindow(MainModel model)
        {
            InitializeComponent();
            m_Model = model;
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            int ageAsNum;
            string name = NameText.Text;
            string email = EmailText.Text;
            string phone = PhoneText.Text;
            string age = AgeText.Text;
            string password = PasswordText.Password;

            // Inputs validation
            if (string.IsNullOrEmpty(name))
                System.Windows.Forms.MessageBox.Show("Name cannot be empty. Please re-enter");
            else if (string.IsNullOrEmpty(email))
                System.Windows.Forms.MessageBox.Show("Email cannot be empty. Please re-enter");
            else if (string.IsNullOrEmpty(phone))
                System.Windows.Forms.MessageBox.Show("Phone cannot be empty. Please re-enter");
            else if (string.IsNullOrEmpty(age))
                System.Windows.Forms.MessageBox.Show("Age cannot be empty. Please re-enter");
            else if (string.IsNullOrEmpty(password))
                System.Windows.Forms.MessageBox.Show("Password cannot be empty. Please re-enter");

            else if (!int.TryParse(age, out ageAsNum))
                System.Windows.Forms.MessageBox.Show("Invalid age");
            else if (!IsValidEmail(email))
                System.Windows.Forms.MessageBox.Show("Invalid e-mail address");
            else if (!Regex.IsMatch(phone, @"^[0-9-]+$"))
                System.Windows.Forms.MessageBox.Show("Invalid phone number");
            else if (password.Length < 8)
                System.Windows.Forms.MessageBox.Show("Password must be at least 8 characters long");

            // All inputs are valid
            else
            {
                phone = phone.Replace("-", "");
                ageAsNum = int.Parse(age);
                try
                {
                    m_Model.SignUp(name, email, phone, ageAsNum, password);
                }
                catch (Exception error)
                {
                    System.Windows.Forms.MessageBox.Show(error.Message);
                }
                System.Windows.Forms.MessageBox.Show(string.Format("Signed Up successfully.\nWelcome to PartnersMatcher, {0}!", name));
                Close();
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
                SignUp_Click(sender, e);
        }
    }
}
