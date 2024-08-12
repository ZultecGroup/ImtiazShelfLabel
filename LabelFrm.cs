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
    public partial class LabelFrm : Form
    {
       
        public LabelFrm()
        {
           
            InitializeComponent();

           
        }
        String token;
        public static string ID;
        public static string SetValueForToken = "";
        string str = ConfigurationManager.AppSettings["str"];
        int def;
        string storesID;
        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void StoreList_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
        }

        private void CreateTab_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
            labelCodeTxt.Text = "";
            labelNameTxt.Text = "";
            labelTextTxt.Text = "";
            bunifuCheckBox1.Checked = false;
            saveBtn.Text = "Save";
        }

        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {

        }
        public class getalllabelsClass
        {
            public int get { get; set; }
        }
        public class createlabelClass
        {
            public int add { get; set; }
            public int update { get; set; }
            public int labelID { get; set; }
            public string labelCode { get; set; }
            public string labelName { get; set; }
            public string labelText { get; set; }
            public string message { get; set; }
            public int createdBy { get; set; }
            public int lastUpdatedBy { get; set; }
            public int isDefault { get; set; }
        }
        public class deletelabelClass
        {

            public int update { get; set; }
            public int delete { get; set; }
            public string message { get; set; }
            public int labelID { get; set; }
            public int lastUpdatedBy { get; set; }
        }
        private void LabelFrm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;

            ID = Dashboard.ID;
            loadAllLabels();
           // bunifuVScrollBar1.Visible = false;
        }

        private void loadAllLabels()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    getalllabelsClass cls = new getalllabelsClass { get = 1 };
                    var response = client.PostAsJsonAsync("api/Labels/GetAllLabels", cls).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                   
                    dt.Columns.Remove("createdBy");
                    dt.Columns.Remove("lastUpdatedBy");
                   
                    labelgrid.DataSource = dt;
                    labelgrid.Columns["labelID"].HeaderText = "Label ID";
                    labelgrid.Columns["labelCode"].HeaderText = "Label Code";
                    labelgrid.Columns["labelName"].HeaderText = "Label Name";
                    
                    
                    labelgrid.Columns["labelText"].Visible = false;
                    labelgrid.Columns["isDefault"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuPictureBox2_Click_1(object sender, EventArgs e)
        {
            if (bunifuPages1.PageIndex == 0)
            {
                SetValueForToken = token;

                Dashboard dash = new Dashboard();
                this.Hide();
                dash.ShowDialog();
            }
            else
            {
                bunifuPages1.PageIndex = 0;
            }
          
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (labelCodeTxt.Text != "" || labelNameTxt.Text != "" || labelTextTxt.Text != "")
            {
                checkdefault();
                if (saveBtn.Text == "Save")
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(str);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            createlabelClass cls = new createlabelClass { add = 1, labelCode = labelCodeTxt.Text, labelName = labelNameTxt.Text, labelText = labelTextTxt.Text, lastUpdatedBy = int.Parse(ID), createdBy = int.Parse(ID), isDefault = def };
                            var response = client.PostAsJsonAsync("api/Labels/InsertLabel", cls).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            createlabelClass Reply = JsonConvert.DeserializeObject<createlabelClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            bunifuSnackbar1.Show(this, Reply.message.ToString(),
                                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                        Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                        Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            labelCodeTxt.Text = "";
                            labelNameTxt.Text = "";
                            labelTextTxt.Text = "";
                            loadAllLabels();
                            bunifuPages1.PageIndex = 0;
                            bunifuCheckBox1.Checked = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                }

                if (saveBtn.Text == "Update")
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(str);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            createlabelClass cls = new createlabelClass { update = 1, labelCode = labelCodeTxt.Text, labelName = labelNameTxt.Text, labelText = labelTextTxt.Text, lastUpdatedBy = int.Parse(ID), labelID = int.Parse(storesID), isDefault = def };
                            var response = client.PostAsJsonAsync("api/Labels/UpdateLabel", cls).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            createlabelClass Reply = JsonConvert.DeserializeObject<createlabelClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            bunifuSnackbar1.Show(this, Reply.message.ToString(),
                                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                        Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                        Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            labelCodeTxt.Text = "";
                            labelNameTxt.Text = "";
                            labelTextTxt.Text = "";
                            loadAllLabels();
                            bunifuPages1.PageIndex = 0;
                            bunifuCheckBox1.Checked = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                }
            }
            else
            {
                bunifuSnackbar1.Show(this, "Label Code / Label Name / Label Text cant be empty",
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void checkdefault()
        {
            if(bunifuCheckBox1.Checked== true)
                    {
                def = 1;
                }
            else
            {
                def = 0;
            }
        }

        private void labelgrid_DoubleClick(object sender, EventArgs e)
        {
        }

        private void labelgrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            bunifuPages1.PageIndex = 1;
            storesID = labelgrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            labelCodeTxt.Text = labelgrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            labelNameTxt.Text = labelgrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            labelTextTxt.Text = labelgrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            int defaultch = int.Parse(labelgrid.Rows[e.RowIndex].Cells["isDefault"].Value.ToString());
            

            if (defaultch == 1)
            {
                bunifuCheckBox1.Checked = true;
            }
            else
            {
                bunifuCheckBox1.Checked = false;
            }
            saveBtn.Text = "Update";
        }

        private void labelgrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Label?",
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

                            storesID = labelgrid[0, labelgrid.CurrentCell.RowIndex].Value.ToString();
                            deletelabelClass lgn = new deletelabelClass { labelID = int.Parse(storesID), delete = 1, update = 1, lastUpdatedBy = int.Parse(ID) };
                            var response = client.PostAsJsonAsync("api/Labels/DeleteLabel", lgn).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            deletelabelClass Reply = JsonConvert.DeserializeObject<deletelabelClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            bunifuSnackbar1.Show(this, Reply.message.ToString(),
                                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                        Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                        Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            loadAllLabels();
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
               
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
