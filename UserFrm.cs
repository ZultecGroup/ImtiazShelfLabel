using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Imtiaz_AlArbani
{
    public partial class UserFrm : Form
    {
        public UserFrm()
        {
            InitializeComponent();
          
        }
        String token;
        String ID;
        string IDstore;
        public static string SetValueForToken = "";
        string str = ConfigurationManager.AppSettings["str"];
        int useraccess;
        int userid;
        int userRoleId;
        private void StoreList_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
        }

        private void CreateTab_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
        }

        private void UserFrm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;
            bunifuVScrollBar1.Visible = false;
            ID = Dashboard.ID;
            loadAllUsers();
            bunifuVScrollBar1.Visible = false;
        }

        private void loadAllUsers()
        {
            
            using (var client = new HttpClient())
            {
                try
                {
                    Cursor = Cursors.WaitCursor;    
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getallstoresClass lgn = new RequestParameter.getallstoresClass { get = 1 };
                    var response = client.PostAsJsonAsync("api/User/GetAllUsers", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable account = JsonConvert.DeserializeObject<DataTable>(json);
                    

                   // account.Columns.Remove("password");
                    account.Columns.Remove("rowNo");
                    
                    account.Columns.Remove("roleID");
                    usergrid.DataSource = account;

                    usergrid.Columns["password"].Visible = false;
                    usergrid.Columns["userAccess"].Visible = false;
                  
                    usergrid.Columns["ID"].Visible = false;
                    usergrid.Columns["loginName"].HeaderText = "Login Name";
                    usergrid.Columns["userName"].HeaderText = "Username";
                    usergrid.Columns["storeCode"].HeaderText = "Store Code";
                    usergrid.Columns["role"].HeaderText = "Role";
                    usergrid.Columns["emailAddress"].HeaderText = "Email";
                    Cursor = Cursors.Arrow;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void UserFrm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {

                e.SuppressKeyPress = true;
                Dashboard dash = new Dashboard();
                this.Close();
                dash.ShowDialog();
            }
        }

        private void activateBtn_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel5_Click(object sender, EventArgs e)
        {

        }

        private void storegrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bunifuLabel6_Click(object sender, EventArgs e)
        {

        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
         
            if (bunifuPages1.PageIndex == 0)
            {
                SetValueForToken = token;

                Dashboard dash = new Dashboard();
                this.Hide();
                dash.ShowDialog();

                this.Close();
            }
            else
            {
                bunifuPages1.PageIndex = 0;
            }

          
        }

        private void usergrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            Cursor = Cursors.WaitCursor;
            bunifuPages1.PageIndex = 1;
            loadallstores();
            loginNameTxt.Text = usergrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            userNameTxt.Text = usergrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            //PasswordTxt.Text = usergrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            //role.Text = usergrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            EmailTxt.Text = usergrid.Rows[e.RowIndex].Cells[5].Value.ToString();
            userid = int.Parse(usergrid.Rows[e.RowIndex].Cells[7].Value.ToString());
            PasswordTxt.Text = usergrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            conpassword.Text= usergrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            storeDropdown.Text = usergrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            roleCombo.Text = usergrid.Rows[e.RowIndex].Cells["Role"].Value.ToString();
            if(usergrid.Rows[e.RowIndex].Cells["userAccess"].Value.ToString()=="1")
            {
                bunifuRadioButton1.Checked= true;
                bunifuRadioButton2.Checked= false;
                bunifuRadioButton3.Checked= false;
            }
            if(usergrid.Rows[e.RowIndex].Cells["userAccess"].Value.ToString()=="2")
            {
                bunifuRadioButton2.Checked = true;
                bunifuRadioButton1.Checked = false;
                bunifuRadioButton3.Checked = false;
            }
            if(usergrid.Rows[e.RowIndex].Cells["userAccess"].Value.ToString()=="3")
            {
                bunifuRadioButton3.Checked = true;
                bunifuRadioButton2.Checked = false;
                bunifuRadioButton1.Checked = false;
            }
            saveBtn.Text = "Update";
            Cursor = Cursors.Arrow;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (loginNameTxt.Text != "" && userNameTxt.Text != "" && PasswordTxt.Text !="" && storeDropdown.SelectedText.ToString()!="Select" && EmailTxt.Text!="")
            {

                if (PasswordTxt.Text == conpassword.Text)
                {
                    checkUserAccess();
                    checkUserRole();
                    if (saveBtn.Text == "Save")
                    {
                        Cursor = Cursors.WaitCursor;
                        using (var client = new HttpClient())
                        {
                            try
                            {

                                client.BaseAddress = new Uri(str);
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                RequestParameter.createuserClass cls = new RequestParameter.createuserClass { add = 1, loginName = loginNameTxt.Text, userName = userNameTxt.Text, password = PasswordTxt.Text, storeCode = storeDropdown.Text, emailAddress = EmailTxt.Text, roleID = userRoleId, userAccess = useraccess, id = userid };
                                var response = client.PostAsJsonAsync("api/User/InsertUser", cls).Result;

                                var a = response.Content.ReadAsStringAsync();
                                string json = a.Result;
                                RequestParameter.createstoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.createstoresClass>(json);
                                // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                                bunifuSnackbar1.Show(this, Reply.message,
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                                //MessageBox.Show(Reply.message);
                                loginNameTxt.Text = "";
                                loginNameTxt.Text = "";
                                PasswordTxt.Text = "";
                                storeDropdown.Text = "";
                                EmailTxt.Text = "";
                                loadAllUsers();
                                bunifuPages1.PageIndex = 0;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString());
                            }
                            Cursor = Cursors.Arrow; // change cursor to normal type
                        }
                     
                    }

                    if (saveBtn.Text == "Update")
                    {
                        Cursor = Cursors.WaitCursor;
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                    
                                client.BaseAddress = new Uri(str);
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                RequestParameter.createuserClass cls = new RequestParameter.createuserClass { update = 1, loginName = loginNameTxt.Text, userName = userNameTxt.Text, password = PasswordTxt.Text, storeCode = storeDropdown.Text, emailAddress = EmailTxt.Text, roleID = userRoleId, userAccess = useraccess, id = userid };
                                var response = client.PostAsJsonAsync("api/User/UpdateUser", cls).Result;

                                var a = response.Content.ReadAsStringAsync();
                                string json = a.Result;
                                RequestParameter.createstoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.createstoresClass>(json);
                                // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                                bunifuSnackbar1.Show(this, Reply.message,
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                                //MessageBox.Show(Reply.message);
                                loginNameTxt.Text = "";
                                loginNameTxt.Text = "";
                                PasswordTxt.Text = "";
                                storeDropdown.Text = "";
                                EmailTxt.Text = "";
                                conpassword.Text = "";
                                loadAllUsers();
                                bunifuPages1.PageIndex = 0;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString());
                            }
                            Cursor = Cursors.Arrow; // change cursor to normal type
                        }
                      
                    }
                }
                else
                {
                    bunifuSnackbar1.Show(this, "Password not match",
                                    Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000, "",
                                    Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                    Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                }

                

            }
             else
            {
                bunifuSnackbar1.Show(this, "Fields cannot be empty",
                Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000, "",
                Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void checkUserRole()
        {
            if(roleCombo.Text=="Admin")
            {
                userRoleId = 1;
            }
            if (roleCombo.Text == "User")
            {
                userRoleId = 2;
            }
        }

        private void checkUserAccess()
        {
            if (bunifuRadioButton1.Checked == true)
            {
                useraccess = 1;

            }
            if (bunifuRadioButton2.Checked == true)
            {
                useraccess = 2;
            }
            if (bunifuRadioButton3.Checked == true)
            {
                useraccess = 3;
            }

        }

        private void CreateTab_Click_1(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
            saveBtn.Text = "Save";
            loginNameTxt.Text = "";
            userNameTxt.Text = "";
            PasswordTxt.Text = "";
            EmailTxt.Text = "";
            loadallstores();
            storeDropdown.Text = "Select";
        }

        private void loadallstores()
        {
            storeDropdown.Items.Clear();
            Cursor = Cursors.WaitCursor;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getallstoresClass lgn = new RequestParameter.getallstoresClass { get = 1 };
                    var response = client.PostAsJsonAsync("api/Stores/GetAllStores", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable account = JsonConvert.DeserializeObject<DataTable>(json);
                    account.Columns.Remove("isDeleted");
                    account.Columns.Remove("createdBy");
                    account.Columns.Remove("createdOn");
                    account.Columns.Remove("lastUpdatedBy");
                    account.Columns.Remove("lastUpdatedOn");
                    for (int i = 0; i < account.Rows.Count; i++)
                    
                        {
                            string theValue = account.Rows[i].ItemArray[1].ToString();
                            storeDropdown.Items.Add(theValue);

                        }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            Cursor = Cursors.Arrow; // change cursor to normal type
        }

        private void usergrid_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Delete)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this User?",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(str);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            //userid = storegrid[0, storegrid.CurrentCell.RowIndex].Value.ToString();
                            userid = int.Parse(usergrid[7, usergrid.CurrentCell.RowIndex].Value.ToString());
                           
                            RequestParameter.deleteuserClass lgn = new RequestParameter.deleteuserClass { id = userid, delete = 1, update = 1};
                            var response = client.PostAsJsonAsync("api/User/DeleteUser", lgn).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.deleteuserClass Reply = JsonConvert.DeserializeObject<RequestParameter.deleteuserClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            bunifuSnackbar1.Show(this, Reply.message,
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            loadAllUsers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                }
                else
                {
                    // If 'No', do something here.
                }
                loadAllUsers();
                Cursor = Cursors.Arrow; // change cursor to normal type
            }
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuLabel9_Click(object sender, EventArgs e)
        {

        }

        private void bunifuRadioButton3_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {

        }

        private void bunifuLabel12_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel10_Click(object sender, EventArgs e)
        {

        }

        private void bunifuRadioButton2_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {

        }

        private void bunifuRadioButton1_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {

        }

        private void StoreList_Click_1(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
            saveBtn.Text = "Save";
            loadallstores();
        }

        private void PasswordTxt_OnIconRightClick(object sender, EventArgs e)
        {
            PasswordTxt.PasswordChar = '\0';
        }

        private void conpassword_OnIconRightClick(object sender, EventArgs e)
        {
            conpassword.PasswordChar = '\0';
        }
    }
}
