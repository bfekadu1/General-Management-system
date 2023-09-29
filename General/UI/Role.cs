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
        List<Functionality> roles = new List<Functionality>();
        List<UserRole> userRoles = new List<UserRole>();
        List<string> selectedValues = new List<string>();
        List<int>description_id=new List<int>();
        List<CheckBox>ListOfCheckBox= new List<CheckBox>();
        userNameList userList = new userNameList();


        public Role()
        { 
            InitializeComponent();
            userRoles = da.getAllUserRoles();
            loadUsernames = da.getAllusers();
            functionalities = da.getCheckBox();
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
        
        }

        // the above code draw the check box for every user that is selected it will change every time a user is changed
        private void txtUserNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(CheckBox checkbox in ListOfCheckBox)
            {
                panelControl3.Controls.Remove(checkbox);
                checkbox.Dispose();     
            }
            ListOfCheckBox.Clear();
            userNameList selectedUser = (userNameList)txtUserNames.SelectedItem;
            userList.id = selectedUser.id;
            userList.user_name = selectedUser.user_name;
            userRoles.Clear();
            userRoles = da.getAllUserRoles();
            var users = userRoles.Where(userRoles => userRoles.useri_id == userList.id);
            int x = 155;
            int y = 150;

            for (var i = 0; i < functionalities.Count; i++)
            {
                
                CheckBox checkBox = new CheckBox();
                checkBox.Text = functionalities[i].description;
                checkBox.Size = new Size(124, 27);
                //checkBox.Name = "txt" + functionalities[i].description;
                checkBox.Location = new Point(x, y);
                
                foreach (var use in users)
                {
                    if (use.functionalty_id == functionalities[i].id)
                    {
                        checkBox.Checked = true;
                    }
                    
                }
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                y = y + 50;
                panelControl3.Controls.Add(checkBox);
                ListOfCheckBox.Add(checkBox);
            }
        }

       private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //CheckBox checkBox = (CheckBox)sender; 
         }
        

        public int getid(string text)
        {
            for (int i = 0; i < functionalities.Count; i++)
            {
                if (functionalities[i].description == text)
                {
                    
                    return functionalities[i].id;
                    break;
                   
                }  
            }
            return 0;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {

            userNameList selectedUser = (userNameList)txtUserNames.SelectedItem;
            userList.id = selectedUser.id;
            userList.user_name = selectedUser.user_name;
            foreach (var checkbox in ListOfCheckBox)
            {
                if (checkbox.Checked)
                {

                    description_id.Add(getid(checkbox.Text));
                }
                else
                {
                    selectedValues.Remove(checkbox.Text);
                }
            }


            da.removeUserRole(userList.id);
            da.addUserRole(userList.id, description_id);

        }

        
    }
}
