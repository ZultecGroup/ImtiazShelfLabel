using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;

namespace Imtiaz_AlArbani
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }
        public static string SetValueForToken = "";
        public static string userId = "";
        public bool isloginSuccess = false;
        string str = ConfigurationManager.AppSettings["str"];
        private void LoginFrm_Load(object sender, EventArgs e)
        {

        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            login();
        }
       
        public void login()
        {
            Cursor = Cursors.WaitCursor;
            if (txtusername.Text.ToString().Trim() != "" && txtpassword.Text.ToString().Trim() != "")
            {
                using (var client = new HttpClient())
                {
                    try
                    {


                        client.BaseAddress = new Uri(str);
                        RequestParameter.LoginClass lgn = new RequestParameter.LoginClass { loginName = txtusername.Text.ToString(), Password = txtpassword.Text.ToString() };
                        var response = client.PostAsJsonAsync("api/User/Login", lgn).Result;
                        var a = response.Content.ReadAsStringAsync();
                        string json = a.Result;
                        RequestParameter.LoginClass account = JsonConvert.DeserializeObject<RequestParameter.LoginClass>(json);
                        if (account.status.ToString().Trim() == "401")
                        {

                            bunifuSnackbar1.Show(this, "Invalid login credentials.",
                                          Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "",
                                          Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                          Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                        }
                        else
                        {

                            // MessageBox.Show(a.Result.ToString());
                            lblErrorMessage.Text = "Loggedin successfully.";
                            lblErrorMessage.ForeColor = Color.Green;
                            SetValueForToken = account.token;
                            userId = account.userID;
                            isloginSuccess = true;

                            bunifuSnackbar1.Show(this, "Log in successfull",
                                       Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, "",
                                       Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                       Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            Dashboard dash = new Dashboard();
                            this.Hide();

                            dash.ShowDialog();

                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }

            }
            else
            {
                bunifuSnackbar1.Show(this, "Login Name and Password cannot be empty",
               Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "",
               Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
               Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
            }
            Cursor = Cursors.Arrow; // change cursor to normal type
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            ForgotPassword str = new ForgotPassword();
            this.Hide();
            str.ShowDialog();
            this.Close();
        }

        private void LoginFrm_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }
    }
}
