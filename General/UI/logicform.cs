using General.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace General
{
    public partial class Login : Form
    {
        Validate txtValidator = new Validate();
        public Login()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string name = txtUserName.Text.Trim();
            if (!txtValidator.ValidateField(name))
            {
                txtUserName.BackColor = Color.LightPink;
                txtUserName.Focus();
                return;
            }


            string password = txtPassword.Text.Trim();
            if (!txtValidator.ValidateField(password))
            {
                txtPassword.BackColor = Color.LightPink;
                txtPassword.Focus();
                return;

            }

            string userName = txtUserName.Text.Trim();
            string passWord = txtPassword.Text.Trim();

            if (authenticate(userName, passWord))
            {
                MessageBox.Show($"welcome {userName}", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("not logged", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {

        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {

        }





        private void txtPassword_EditValueChanged(object sender, EventArgs e)
        {
            txtPassword.BackColor = SystemColors.Window;
        }

        private void txtUserName_EditValueChanged(object sender, EventArgs e)
        {
            txtUserName.BackColor = SystemColors.Window;
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
        public bool authenticate(string name, string password)
        {
            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";
            List<userList> userLists = null;
            SqlConnection conn1 = new SqlConnection(context);
            conn1.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from general.vw_userList where active=1";
            cmd.Connection = conn1;
            var reader = cmd.ExecuteReader();
            userLists = GetList<userList>(reader);

            for (int i = 0; i < userLists.Count; i++)
            {
                if (name == userLists[i].user_name && password == userLists[i].password)
                {
                    return true;
                    break;
                }
                
            }

            return false;

        }

        public void connect()
    {
        string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";

        List<userList> userLists = null;
        SqlConnection conn1 = new SqlConnection(context);
        conn1.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from general.vw_userList where active=1";
        cmd.Connection = conn1;

        var reader = cmd.ExecuteReader();
        userLists = GetList<userList>(reader);
        if (userLists != null)
        {
            //dataGridView1.DataSource = userLists;
        }

        txtUserName.DataSource = userLists;
        txtUserName.DisplayMember = "user_name";


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
        private void Login_Load(object sender, EventArgs e)
        {
            connect();


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void panelControl8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUserName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_Click(object sender, EventArgs e)
        {
            txtUserName.BackColor = SystemColors.Window;
        }

        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }
    }


}
