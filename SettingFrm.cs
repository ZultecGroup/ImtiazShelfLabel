using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Imtiaz_AlArbani
{
    public partial class SettingFrm : Form
    {
       
        public SettingFrm()
        {
            InitializeComponent();

           
        }
        String token;
        String ID;
        int Pmode;
        int tol;
        public static string SetValueForToken = "";
       
        string str = ConfigurationManager.AppSettings["str"];
        private void SettingFrm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;
            bunifuRadioButton1.Checked = false;
            bunifuRadioButton2.Checked = false;
            bunifuRadioButton3.Checked = false;
            bunifuRadioButton4.Checked = false;
            bunifuRadioButton5.Checked = false;
            

            ID = Dashboard.ID;
            loadAllsetting();
           
        }

        private void loadAllsetting()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                   RequestParameter.getallsetting cls = new RequestParameter.getallsetting { get = 1 };
                    var response = client.PostAsJsonAsync("api/Settings/GetAdminSettings", cls).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable forgrid = JsonConvert.DeserializeObject<DataTable>(json);

                    if (forgrid.Rows.Count > 0)
                    {
                        saveBtn.Text = "Update";
                        if (forgrid.Rows[0][1].ToString() == "1")
                        {

                            bunifuRadioButton1.Checked = true;
                        }
                        if (forgrid.Rows[0][1].ToString() == "2")
                        {
                            bunifuRadioButton2.Checked = true;
                        }

                        if (forgrid.Rows[0][2].ToString() == "1")
                        {

                            bunifuRadioButton4.Checked = true;
                        }
                        if (forgrid.Rows[0][2].ToString() == "2")
                        {
                            bunifuRadioButton3.Checked = true;
                        }
                        if (forgrid.Rows[0][2].ToString() == "3")
                        {
                            bunifuRadioButton5.Checked = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            setRadios();
            if(saveBtn.Text=="Update")
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(str);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        RequestParameter.createsettingClass cls = new RequestParameter.createsettingClass { update = 1, id = 1, printingMode = Pmode, typeofLabel=tol, lastUpdatedBy = int.Parse(ID) };
                        var response = client.PostAsJsonAsync("api/Settings/UpdateAdminSettings", cls).Result;

                        var a = response.Content.ReadAsStringAsync();
                        string json = a.Result;
                        RequestParameter.createsettingClass Reply = JsonConvert.DeserializeObject<RequestParameter.createsettingClass>(json);
                        // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                        //MessageBox.Show(Reply.message);
                        bunifuSnackbar1.Show(this, Reply.message,
                                           Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, "",
                                           Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                           Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            if (saveBtn.Text == "Save")
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(str);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        RequestParameter.createsettingClass cls = new RequestParameter.createsettingClass { add = 1, id = 1, printingMode = Pmode, typeofLabel = tol, createdBy = int.Parse(ID), lastUpdatedBy=int.Parse(ID) };
                        var response = client.PostAsJsonAsync("api/Settings/InsertAdminSettings", cls).Result;

                        var a = response.Content.ReadAsStringAsync();
                        string json = a.Result;
                        RequestParameter.createsettingClass Reply = JsonConvert.DeserializeObject<RequestParameter.createsettingClass>(json);
                        // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                        //MessageBox.Show(Reply.message);
                        bunifuSnackbar1.Show(this, Reply.message,
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void setRadios()
        {
            if (bunifuRadioButton1.Checked == true)
            {
                Pmode = 1;
                
            }
            if (bunifuRadioButton2.Checked == true)
            {
                Pmode = 2;
            }

            if (bunifuRadioButton4.Checked == true)
            {
                tol = 1;
               
            }
            if (bunifuRadioButton3.Checked == true)
            {
                tol = 2;
            }
            if (bunifuRadioButton5.Checked == true)
            {
                tol = 3;
            }
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;

            Dashboard dash = new Dashboard();
            this.Hide();
            dash.ShowDialog();

            this.Close();
        }
    }
}
