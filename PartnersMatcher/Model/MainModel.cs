using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace PartnersMatcher.Model
{
    public class MainModel : INotifyPropertyChanged
    {
        private string dbPath = @"PartnersMatcherDB.accdb";
        /// Default value in topics set
        private const string DEFAULT_VALUE = "(All)";

        #region Properties
        private User m_CurrentUser;
        public User CurrentUser
        {
            get { return m_CurrentUser; }
            set
            {
                m_CurrentUser = value;
                NotifyPropertyChanged("CurrentUser");
            }
        }

        private HashSet<string> m_Topics = new HashSet<string>();
        public HashSet<string> Topics
        {
            get { return m_Topics; }
            set
            {
                m_Topics = value;
                NotifyPropertyChanged("Topics");
            }
        }

        private DataTable m_SearchResults;
        public DataTable SearchResults
        {
            get { return m_SearchResults; }
            set
            {
                m_SearchResults = value;
                NotifyPropertyChanged("SearchResults");
            }
        }
        #endregion

        public MainModel()
        {
            LoadTopics();
        }

        public void SignUp(string name, string email, string phone, int age, string password)
        {
            DataTable dt = ExecuteReadQuery(string.Format("Select UserName From Users Where Email = '{0}'", email));
            if (dt.Rows.Count > 0)
                throw new Exception("Email address already exists");
            ExecuteWriteQuery(string.Format("Insert Into Users\nValues('{0}','{1}','{2}',{3},'{4}')", email, name, phone, age, password));
            CurrentUser = new User(name, email, phone, password, age);
            new Thread(() => { SendConfirmationMail(); }).Start();
        }

        public void LogIn(string email, string password)
        {
            DataTable dt = ExecuteReadQuery(string.Format("Select * from Users where Email = '{0}'", email));
            if (dt.Rows.Count == 0)
                throw new Exception("Email address not found");
            if (dt.Rows[0]["Password"].ToString() != password)
                throw new Exception("Incorrect Password");
            CurrentUser = new User(dt.Rows[0]);
        }

        public void Search(string topic, string location)
        {
            string query = "Select * From Ads ";
            if (topic != DEFAULT_VALUE)
            {
                if (location != string.Empty)
                    query += string.Format("Where Topic = '{0}' And Location = '{1}'", topic, location);
                else
                    query += string.Format("Where Topic = '{0}'", topic);
            }
            else if (location != string.Empty)
                query += string.Format("Where Location = '{0}'", location);

            SearchResults = ExecuteReadQuery(query);
        }

        private void SendConfirmationMail()
        {
            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("partnersmatcher2017@gmail.com", "PartnersPassword"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("no-reply@PartnersMatcher.com", "PartnersMatcher");
                message.To.Add(new MailAddress(CurrentUser.Email));
                message.Body = string.Format("Hello, {0}!\nThank you for joining us and welcome to PartnersMatcher!\nLogin now to find your partner for anything!", CurrentUser.Name);
                message.Subject = string.Format("Hi {0}, welcome to PartnersMatcher!",CurrentUser.Name);
                client.Send(message);
            }
        }

        private void LoadTopics()
        {
            Topics.Add(DEFAULT_VALUE);
            DataTable dt = ExecuteReadQuery("Select * From Topics");
            foreach (DataRow row in dt.Rows)
                Topics.Add(row["TopicName"].ToString());
        }

        private DataTable ExecuteReadQuery(string query)
        {
            DataTable queryResults = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        queryResults.Load(reader);
                    }
                }
            }
            return queryResults;
        }

        private void ExecuteWriteQuery(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
