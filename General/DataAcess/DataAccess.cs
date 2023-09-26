using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.Common;
using DevExpress.Printing.Core.PdfExport.Metafile;
using General.Model;
using DevExpress.Data.Async;
using DevExpress.Utils.Commands;

namespace General.DataAcess
{
    public class DataAccess
    {
        string connection= @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True";
        List<Functionality> userRoles = new List<Functionality>();
        List<User> Users=new List<User>();
        List<userNameList> Usernames= new List<userNameList>();
        
        public List<User> getUsers()
        {
            
            SqlConnection conn= new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd= conn.CreateCommand();
            cmd.CommandText = "select  general.person.id,general.person.first_name,general.person.middile_name,general.person.last_name,general.person.gender,general.person.age,general.person.date_registered,general.person.type_id,general.person.phone,general.user_account.user_name,general.user_account.password,general.user_account.active from general.person INNER JOIN general.user_account ON general.person.id= general.user_account.person_id";
            cmd.Connection= conn;
            var reader= cmd.ExecuteReader();
            while (reader.Read())
            {
                User users = new User
                {
                    person_id = reader["id"].ToString(),
                    first_name = reader["first_name"].ToString(),
                    middile_name = reader["middile_name"].ToString(),
                    last_name = reader["last_name"].ToString(),
                    age = int.Parse(reader["age"].ToString()),
                    gender = reader["gender"].ToString(),
                    date_registered = DateTime.Parse(reader["date_registered"].ToString()),
                    typeId = int.Parse(reader["type_id"].ToString()),
                    user_name = reader["user_name"].ToString(),
                    password = reader["password"].ToString(),   
                    phone = int.Parse(reader["phone"].ToString()),
                    active =reader.GetBoolean(11)
                };  
                Users.Add(users);

            }
                  

            return Users ;

        }
        public List<userNameList> getAllusers() { 
            
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd= conn.CreateCommand();
                cmd.CommandText = "select id,user_name from general.user_account";
                cmd.Connection= conn;
                var reader= cmd.ExecuteReader();
                while (reader.Read())
                {
                    userNameList userNameList = new userNameList
                    {
                        id=reader.GetInt32(0),
                        user_name=reader.GetString(1)
                    };
                    Usernames.Add(userNameList);

                }
            }
            return Usernames;
        }
       public  List<Functionality>getCheckBox()
        {
            using(SqlConnection conn= new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from general.functionalty";
                cmd.Connection = conn;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Functionality functionality = new Functionality
                    {
                        id = int.Parse(reader["id"].ToString()),
                        description= reader["description"].ToString(),
                        category= reader["category"].ToString(),


                    };
                    userRoles.Add(functionality);
                }

            }
            return userRoles;
        }
    }
}
