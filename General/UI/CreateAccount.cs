using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.XtraPrinting.Native;
using General.Logic;
using General.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace General.UI
{
    public partial class CreateAccount : Form
    {
        DataAcess.DataAccess da = new DataAcess.DataAccess();
        static string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True";
        SqlConnection connection = new SqlConnection(context);
        Validate txtValidator = new Validate();
        person user1 = new person();
        List<User> loadUser;
        public CreateAccount()
        {
            InitializeComponent();
            idOnFormLoad();
            //display_data();
        }
        //public void display_data()
        //{
            
        //    connection.Open();
        //    SqlCommand cmd = connection.CreateCommand();
        //    cmd.CommandType= CommandType.Text;
        //    cmd.CommandText = "select * from general.vw_userList";
        //    cmd.ExecuteNonQuery();
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter dataadp = new SqlDataAdapter(cmd);
        //    dataadp.Fill(dt);
        //    connection.Close();

        //}
        public void idOnFormLoad()
        {
            string labelValue = "";
            SqlConnection conn = new SqlConnection(context);
            conn.Open();
            string sql = "select MAX(current_value) from general.id_value";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()){
                labelValue=reader.GetString(0);
            }
            labelValue = labelValue.Substring(2, 5);
            int val = int.Parse(labelValue);
            val++;
            labelValue = string.Format("H-{0:00000}-2", val);
            textId.Text = labelValue;
          
        }
        public void idGen()
        {
            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True";
            idGeneratorStructure id = new idGeneratorStructure();

            SqlConnection conn = new SqlConnection(context);
            conn.Open();

            var query = "select length,prefix,prefix_separator,suffix_separator,suffix from general.id_defination where id=@id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", 1);
            SqlDataReader reader = cmd.ExecuteReader();
            int length = 0;


            while (reader.Read())
            {
                length = reader.GetInt32(0);
                id.prefix = reader.GetString(1);
                id.prefix_separator = reader.GetString(2);
                id.suffix_separator = reader.GetString(3);
                id.suffix = reader.GetString(4);
            }
            conn.Close();
            int[] idLen = new int[length];
            id.prefix = id.prefix.Trim() + id.prefix_separator.Trim();

            for (int i = 0; i < length; i++)
            {
                idLen[i] = 0;
                id.prefix = id.prefix + idLen[i];
            }

            id.prefix = id.prefix + id.suffix_separator.Trim() + id.suffix.Trim();
            textId.Text = id.prefix;
            int defNo = 1;
            conn.Open();
            string sql = "insert into general.id_value(defination,current_value) values('" + defNo + "','" + id.prefix + "')";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void idNo()
        {
            string maxId = "";
            try
            {
                string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True";

                SqlConnection conn1 = new SqlConnection(context);
                conn1.Open();
                var query = "select MAX(current_value) from general.id_value";
                SqlCommand cmd = new SqlCommand(query, conn1);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    maxId = reader.GetString(0);
                }
                conn1.Close();


                if (maxId == "")
                {
                    idGen();
                }
                else
                {
                    int defNo = 1;
                    string subString = maxId;
                    subString = subString.Substring(2, 5);
                    int intVal = int.Parse(subString);
                    intVal = intVal + 1;
                    subString = string.Format("H-{0:00000}.2", intVal);
                    conn1.Open();
                    string sql = "insert into general.id_value(defination,current_value) values('" + defNo + "','" + subString + "')";
                    cmd = new SqlCommand(sql, conn1);
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                    textId.Text = subString;

                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }


        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public int getID(string description)
        {
            int id_type = 0;
            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";

            List<user> userLists = null;
            SqlConnection conn1 = new SqlConnection(context);
            conn1.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from general.person_type";
            cmd.Connection = conn1;

            var reader = cmd.ExecuteReader();
            userLists = GetList<user>(reader);

            for (int i = 0; i < userLists.Count; i++)
            {
                if (userLists[i].description == description)
                {
                    id_type = i; break;
                }
            }

            return id_type;
        }
        public void connect()
        {
            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";

            List<user> userLists = null;
            SqlConnection conn1 = new SqlConnection(context);
            conn1.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from general.person_type";
            cmd.Connection = conn1;

            var reader = cmd.ExecuteReader();
            userLists = GetList<user>(reader);
            txtProfession.DataSource = userLists;
            txtProfession.DisplayMember = "description";
            txtProfession.ValueMember = "id";


        }
        public List<T> GetList<T>(IDataReader reader)
        {

            List<T> list = new List<T>();

            while (reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    var proptype = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(reader[prop.Name].ToString(), proptype));


                }
                list.Add(obj);
            }
            return list;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            
            string F_name = txtFirstName.Text.Trim();
            if (!txtValidator.ValidateField(F_name))
            {
                txtFirstName.BackColor = Color.LightPink;
                txtFirstName.Focus();
                return;
            }
            string M_name = txtMiddleName.Text.Trim();
            if (!txtValidator.ValidateField(M_name))
            {
                txtMiddleName.BackColor = Color.LightPink;
                txtMiddleName.Focus();
                return;
            }
            string L_name = txtLastName.Text.Trim();
            if (!txtValidator.ValidateField(L_name))
            {
                txtLastName.BackColor = Color.LightPink;
                txtLastName.Focus();
                return;
            }
            string Gender = txtGender.Text.Trim();
            if (!txtValidator.ValidateField(Gender))
            {
                txtGender.BackColor = Color.LightPink;
                txtGender.Focus();
                return;
            }
            string age = txtAge.Text.Trim();
            if (!txtValidator.ValidateField(age))
            {
                txtAge.BackColor = Color.LightPink;
                txtAge.Focus();
                return;
            }
            string phone_number = txtPhoneNumber.Text.Trim();
            if (!txtValidator.ValidateField(phone_number))
            {
                txtPhoneNumber.BackColor = Color.LightPink;
                txtPhoneNumber.Focus();
                return;
            }
            string User_name = txtUserName.Text.Trim();
            if (!txtValidator.ValidateField(User_name))
            {
                txtUserName.BackColor = Color.LightPink;
                txtUserName.Focus();
                return;
            }
            //IEnumerable<User> check= loadUser.Where(member => member.user_name.Contains(User_name));
            //if (check.Any()==true)
            // {
            //    txtUserName.BackColor = Color.LightPink;
            //    txtUserName.Focus();
            //    MessageBox.Show("UserName is already Exist","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);    
            //    return;
            //}
            for(int i=0;i<loadUser.Count;i++ )
            {
                if (User_name == loadUser[i].user_name)
                {
                    txtUserName.BackColor = Color.LightPink;
                    txtUserName.Focus();
                    MessageBox.Show("UserName is already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }
            string password = txtPassword.Text.Trim();
            if (!txtValidator.ValidateField(password))
            {
                txtPassword.BackColor = Color.LightPink;
                txtPassword.Focus();
                return;
            }
            string confirmPassword= txtConfirmPassword.Text.Trim();
            if(!txtValidator.ValidateField(confirmPassword) )
            {
                txtConfirmPass.BackColor = Color.LightPink;
                txtPassword.BackColor = Color.LightPink;
                txtPassword.Focus();
                return;
            }
            if (password != confirmPassword)
            {

                txtConfirmPassword.BackColor = Color.LightPink;
                txtPassword.BackColor = Color.LightPink;
                txtPassword.Focus();
                return;

            }

            DateTime currentDateTime = DateTime.Now;
          

            //function that returns the person type value;
            string desc = txtProfession.Text.Trim();
            int id_type = getID(desc);


            User user=new User();   


            user selectedItem = (user)txtProfession.SelectedItem;
            user.typeId = selectedItem.id;
            //var value = selectedItem.description;
            user.first_name = txtFirstName.Text.Trim();
            user.last_name = txtLastName.Text.Trim();
            user.middile_name = txtMiddleName.Text.Trim();
            user.gender = txtGender.Text.Trim();
            user.age = int.Parse(txtAge.Text.Trim());
            user.phone = int.Parse(txtPhoneNumber.Text.Trim());
            user.active = true;
            user.person_id = textId.Text;
            user.date_registered = DateTime.Now;




            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";
            SqlConnection conn = new SqlConnection(context);
            conn.Open();


            string sql = "insert into general.person values(@id,@first_name,@middile_name,@last_name,@gender,@age,@date_registered,@type_id,@phone,@active,@remark)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", user.person_id);
            cmd.Parameters.AddWithValue("@first_name", user.first_name);
            cmd.Parameters.AddWithValue("@middile_name", user.middile_name);
            cmd.Parameters.AddWithValue("@last_name", user.last_name);
            cmd.Parameters.AddWithValue("@gender", user.gender);
            cmd.Parameters.AddWithValue("@age", user.age);
            cmd.Parameters.AddWithValue("@date_registered", user.date_registered);
            cmd.Parameters.AddWithValue("@type_id", user.typeId);
            cmd.Parameters.AddWithValue("@phone", user.phone);
            cmd.Parameters.AddWithValue("@active", user.active);
            cmd.Parameters.AddWithValue("@remark", "registered");


            int row=cmd.ExecuteNonQuery();
            conn.Close();
          
         
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            txtGender.Text = "";
            txtAge.Text = "";
            txtPhoneNumber.Text = "";

            conn.Open();

            user.user_name = txtUserName.Text.Trim();
           
              
            
            
                user.password = txtPassword.Text.Trim();
                bool active1 = true;
                string sql1 = "insert into general.user_account ([person_id],[user_name],[password],[active]) values (@person_id,@user_name,@password,@active1)";
                SqlCommand cmd2 = new SqlCommand(sql1, conn);
                cmd2.Parameters.AddWithValue("@person_id", user.person_id);
                cmd2.Parameters.AddWithValue("@user_name", user.user_name);
                cmd2.Parameters.AddWithValue("@password", user.password);
                cmd2.Parameters.AddWithValue("@active1", user.active);
                int row1=cmd2.ExecuteNonQuery();
                conn.Close();
            if (row >0 && row1>0) {
                loadUser.Add(user);
                MessageBox.Show("User Account record inserted sucessfully");
                idNo();
                idOnFormLoad();

            }

        }

        private void txtFirstName_EditValueChanged(object sender, EventArgs e)
        {
            txtFirstName.BackColor = SystemColors.Window;
        }

        private void txtMiddleName_EditValueChanged(object sender, EventArgs e)
        {
            txtMiddleName.BackColor = SystemColors.Window;
        }

        private void txtLastName_EditValueChanged(object sender, EventArgs e)
        {
            txtLastName.BackColor = SystemColors.Window;
        }

        private void txtGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtAge_EditValueChanged(object sender, EventArgs e)
        {
            txtAge.BackColor = SystemColors.Window;
        }

        private void txtPhoneNumber_EditValueChanged(object sender, EventArgs e)
        {
            txtPhoneNumber.BackColor = SystemColors.Window;
        }

        private void txtUserName_EditValueChanged(object sender, EventArgs e)
        {
            txtUserName.BackColor = SystemColors.Window;
        }

        private void txtPassword_EditValueChanged(object sender, EventArgs e)
        {
            txtPassword.BackColor = SystemColors.Window;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            connect();
            loadUser = da.getUsers();
        }

        private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtGender_Leave(object sender, EventArgs e)
        {

        }

        private void txtGender_DisplayMemberChanged(object sender, EventArgs e)
        {
            txtGender.BackColor = SystemColors.Window;
        }

        private void txtGender_SelectedValueChanged(object sender, EventArgs e)
        {
            txtGender.BackColor = SystemColors.Window;
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtMiddleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            bool check = false;

            connection.Open();
            string searched= txtSearched.Text.Trim();
            string query = "select general.person.id,general.user_account.person_id,general.person.first_name,general.person.middile_name,general.person.last_name,general.person.gender,general.person.age,general.person.date_registered,general.person.type_id,general.person.phone,general.user_account.user_name,general.user_account.password,general.user_account.active from general.person INNER JOIN general.user_account ON general.person.id= general.user_account.person_id  where general.user_account.active=1 and user_name='" + searched+"'";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                check = true;
                txtFounded.Enabled = true;

                user1.id= reader.GetString(0);
                user1.person_id=reader.GetString(1);
                user1.first_name=reader.GetString(2);
                user1.middile_name=reader.GetString(3);
                user1.last_name=reader.GetString(4);
                user1.gender=reader.GetString(5);
                user1.age = reader.GetInt32(6);
                user1.date_registered=reader.GetDateTime(7);
                user1.typeid = reader.GetInt32(8);
                user1.phone = reader.GetInt32(9);
                user1.user_name=reader.GetString(10);
                user1.password=reader.GetString(11);
                user1.active = reader.GetBoolean(12);


            }
            connection.Close();
            if (check)
            {
                txtFounded.Text = user1.user_name;
            }
            else {
                txtFounded.Text = "user doesn't exist";
                txtFounded.Enabled = false; 
            }

        }


        private void simpleButton5_Click(object sender, EventArgs e)
        {


            using (SqlConnection conn = new SqlConnection(context))
            {
                conn.Open();
                try
                    
                {

                    string deleteQuery = "update general.user_account set active=0 where person_id= @person_id";
                    using(SqlCommand command = new SqlCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@person_id", user1.person_id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            MessageBox.Show("record deleted sucessfully");
                        }
                        else
                        {
                            MessageBox.Show("recored has not been deleted");
                        }

                    }
                }
                catch
                {
                   
                }

            }
            connection.Close() ;
        }

        private void txtFounded_Click(object sender, EventArgs e)
        {
            textId.Text = user1.person_id;
            txtFirstName.Text = user1.first_name;
            txtMiddleName.Text = user1.middile_name;
            txtLastName.Text = user1.last_name;
            txtGender.Text =user1.gender ;
            txtAge.Text = user1.age.ToString();
            txtPhoneNumber.Text=user1.phone.ToString();
            txtUserName.Text = user1.user_name;
            txtPassword.Text= user1.password;
            txtProfession.SelectedIndex = user1.typeid;

   

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(context))
            {
                connection.Open();

                
                try
                {
                    string person_id = textId.Text;
                    string f_name=txtFirstName.Text;
                    string m_name=txtMiddleName.Text;
                    string l_name=txtLastName.Text;
                    string gender=txtGender.Text;
                    int age = int.Parse(txtAge.Text);
                    int type_id = txtProfession.SelectedIndex;
                    int phone_number = int.Parse(txtPhoneNumber.Text);
                    

                    
                    string updateParentQuery = "UPDATE general.person SET first_name=@first_name, middile_name=@middile_name, last_name=@last_name, gender=@gender, age=@age, type_id=@type_id, phone=@phone WHERE Id = @id";
                    using (SqlCommand command1 = new SqlCommand(updateParentQuery, connection))
                    {
                        command1.Parameters.AddWithValue("@first_name", f_name); 
                        command1.Parameters.AddWithValue("@middile_name", m_name); 
                        command1.Parameters.AddWithValue("@last_name",l_name);
                        command1.Parameters.AddWithValue("@gender",gender);
                        command1.Parameters.AddWithValue("@age",age);
                        command1.Parameters.AddWithValue("@type_id",type_id);
                        command1.Parameters.AddWithValue("@phone",phone_number);
                        command1.Parameters.AddWithValue("@id",person_id);
                        int rowsAffected1 = command1.ExecuteNonQuery();

                        if (rowsAffected1 > 0)
                        {
                            MessageBox.Show("ParentTable updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No matching records found in ParentTable for update.");
                        }
                    }

                    string u_name = txtUserName.Text;
                    string password = txtPassword.Text;
                    
                    string updateChildQuery = "UPDATE general.user_account SET user_name = @user_name, password=@password WHERE person_id = @id";
                    using (SqlCommand command2 = new SqlCommand(updateChildQuery, connection))
                    {
                        command2.Parameters.AddWithValue("@user_name", u_name); 
                        command2.Parameters.AddWithValue("@password", password); 
                        command2.Parameters.AddWithValue("@id", person_id);
                        int rowsAffected2 = command2.ExecuteNonQuery();

                        if (rowsAffected2 > 0)
                        {
                            MessageBox.Show(" updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No matching records found for update.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

            }

        }

        private void txtConfirmPassword_EditValueChanged(object sender, EventArgs e)
        {
            txtConfirmPassword.BackColor = SystemColors.Window;
            txtPassword.BackColor = SystemColors.Window;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void labelControl7_Click(object sender, EventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
