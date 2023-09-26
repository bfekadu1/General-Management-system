using General.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace General.UI
{
    public partial class Role : Form
    {
        DataAcess.DataAccess da = new DataAcess.DataAccess();
        List<userNameList> loadUsernames = new List<userNameList>();
        List<Functionality> functionalities = new List<Functionality>();   
        List<RoleModel> checkBoxList = new List<RoleModel>();
        List<Functionality> roles = new List<Functionality>();
        List<string> selectedValues = new List<string>();
        List<int>description_id=new List<int>();

        public Role()
        { 
            InitializeComponent();
            loadUsernames = da.getAllusers();
            txtUserNames.Items.Add("Select user");
            txtUserNames.DataSource= loadUsernames;
            txtUserNames.DisplayMember = "user_name";
            txtUserNames.ValueMember = "id";
           

        }
        




        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Role_Load(object sender, EventArgs e)
        {
            int x = 155;
            int y = 150;
            roles = da.getCheckBox();
            for (var i=0;i<roles.Count;i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = roles[i].description;
                checkBox.Size = new Size(124, 27);
                //checkBox.Name = "checkBox_" + i;
                checkBox.Location = new Point(x, y);
                checkBox.CheckedChanged += CheckBox_CheckedChanged;

                y = y + 50;
                panelControl3.Controls.Add(checkBox);
                RoleModel model = new RoleModel();  
                model.RoleCheckBox=checkBox;
                checkBoxList.Add(model);

            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                
                description_id.Add(getid(checkBox.Text));
            }
            else
            {
                selectedValues.Remove(checkBox.Text); 
            }



            //throw new NotImplementedException();
            //another
        }
        public int x = 0;

        public int getid(string text)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].description == text)
                {
                    //trying
                    return roles[i].id;
                    break;
                    // here
                    //lhaskjlnc
                    //opopoopo
                }
                
            }

            return 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            userNameList userList= new userNameList();
            userNameList selectedUser = (userNameList)txtUserNames.SelectedItem;
            userList.id = selectedUser.id;
            userList.user_name= selectedUser.user_name;


            string context = @"Data Source=DESKTOP-A2IS30E\SQLEXPRESS;Initial Catalog=HAHC;Integrated Security=True ";
            using (SqlConnection connection = new SqlConnection(context))
            {
                connection.Open();
                string sql = "select * from general.role where usri_d='" + userList.id + "'";
                //trying to do
            }
            using (SqlConnection connection = new SqlConnection(context))
                {
                int rowcount = 0;
                    connection.Open();
                    foreach (int i in description_id)
                    {
                        string sql = "INSERT INTO general.role (useri_d,functionalty_id) VALUES (@user_id,@functionality_id)";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@user_id", userList.id);
                            cmd.Parameters.AddWithValue("@functionality_id", i);
                            rowcount=cmd.ExecuteNonQuery();
                        }
                    }
                if (rowcount < 0)
                {
                    description_id = null;
                }
                }

        }
    }
}
