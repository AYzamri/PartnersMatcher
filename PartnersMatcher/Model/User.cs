using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnersMatcher.Model
{
    public class User
    {
        private string m_Name,
                       m_Email,
                       m_Phone,
                       m_Password;
        private int m_Age;

        #region Properties
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string Email { get { return m_Email; } set { m_Email = value; } }
        public string Phone { get { return m_Phone; } set { m_Phone = value; } }
        public string Password { get { return m_Password; } set { m_Password = value; } }
        public int Age { get { return m_Age; } set { m_Age = value; } }
        #endregion

        public User(string name, string email, string phone, string password, int age)
        {
            m_Name = name;
            m_Email = email;
            m_Phone = phone;
            m_Password = password;
            m_Age = age;
        }

        public User(DataRow dataRow)
        {
            m_Name = dataRow["UserName"].ToString();
            m_Email = dataRow["Email"].ToString();
            m_Phone = dataRow["Phone"].ToString();
            m_Password = dataRow["Password"].ToString();
            m_Age = ((int)dataRow["Age"]);
        }
    }
}
