using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imtiaz_AlArbani
{
    public partial class CompanyInformationFrm : Form
    {
       
        public CompanyInformationFrm()
        {
            InitializeComponent();

        
        }
        String token;
        String ID;
        public static string SetValueForToken = "";
        string str = ConfigurationManager.AppSettings["str"];
        private void activateBtn_Click(object sender, EventArgs e)
        {
            string emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            string emailAddress = email.Text;
            if (Regex.IsMatch(emailAddress, emailPattern))
            {
                if (activateBtn.Text == "Save")
                {
                    //string base64String = "";
                    //if (bunifuPictureBox1.Image != null)
                    //{
                    //    base64String = ImageToBase64(bunifuPictureBox1.Image);
                    //    // You can now use the 'base64String' variable for your purposes.
                    //}

                    Cursor = Cursors.WaitCursor;
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(str);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            RequestParameter.saveCompanyClass cls = new RequestParameter.saveCompanyClass { add = 1, companyName = compname.Text, companyAddress = address.Text, country = country.Text, city = city.Text, state = state.Text, email = email.Text, contactNo = contact.Text, imageBase64 = "" };
                            var response = client.PostAsJsonAsync("api/Company/InsertCompanyDetails", cls).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.replyClass Reply = JsonConvert.DeserializeObject<RequestParameter.replyClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);


                            bunifuSnackbar1.Show(this, Reply.message.ToString(),
                                      Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, "",
                                      Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                      Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                        Cursor = Cursors.Arrow;
                    }
                }
                if (activateBtn.Text == "Update")
                {
                    string base64String2 = "";
                    //if (bunifuPictureBox1.Image != null)
                    //{
                    //    base64String2 = ImageToBase64(bunifuPictureBox1.Image);
                    //    // You can now use the 'base64String' variable for your purposes.
                    //}

                    Cursor = Cursors.WaitCursor;
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(str);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            RequestParameter.saveCompanyClass cls = new RequestParameter.saveCompanyClass { update = 1, companyName = compname.Text, companyAddress = address.Text, country = country.Text, city = city.Text, state = state.Text, email = email.Text, contactNo = contact.Text, imageBase64 = "1122" };
                            var response = client.PostAsJsonAsync("api/Company/UpdateCompanyDetails", cls).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.saveCompanyClass Reply = JsonConvert.DeserializeObject<RequestParameter.saveCompanyClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);


                            bunifuSnackbar1.Show(this, Reply.message.ToString(),
                                      Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, "",
                                      Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                      Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    Cursor = Cursors.Arrow;
                }
            }
            else
            {
                // Invalid email address, display an error message or take appropriate action.
                bunifuSnackbar1.Show(this, "Invalid email address.Please enter a valid email.",
                                    Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "",
                                    Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                    Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                // MessageBox.Show("Invalid email address. Please enter a valid email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                email.Clear(); // Clear the TextBox to correct the input.
            }

        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuPictureBox3_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;

            Dashboard dash = new Dashboard();
            this.Hide();
            dash.ShowDialog();

            this.Close();
        }

        private void CompanyInformationFrm_Load(object sender, EventArgs e)
        {
            //bunifuVScrollBar1.Visible = false;
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;

            ID = Dashboard.ID;
            loadAllDetails();
        }

        public void loadAllDetails()
        {
            using (var client = new HttpClient())
            {
                try
                {


                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getallsetting lgn = new RequestParameter.getallsetting { get = 1 };
                    var response = client.PostAsJsonAsync("api/Company/GetCompanyDetails", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);


                    if (dt.Rows.Count > 0)
                    {
                        activateBtn.Text = "Update";
                        compname.Text = dt.Rows[0]["companyName"].ToString();
                        address.Text = dt.Rows[0]["companyAddress"].ToString();
                        country.Text = dt.Rows[0]["country"].ToString();
                        state.Text = dt.Rows[0]["state"].ToString();
                        city.Text = dt.Rows[0]["city"].ToString();
                        email.Text = dt.Rows[0]["email"].ToString();
                        contact.Text = dt.Rows[0]["contactNo"].ToString();
                        //if (dt.Rows[0]["imageBase64"].ToString() != "")
                        //{
                        //    Image image = Base64ToImage(dt.Rows[0]["imageBase64"].ToString());
                        //    bunifuPictureBox1.Image = image;
                        //    bunifuPictureBox2.Image = image;
                        //    activateBtn.Text = "Update";
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
