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
    public partial class StoreFrm : Form
    {
       
        public StoreFrm()
        {
            InitializeComponent();

           
        }
        String token;
        String ID;
        string IDstore;
        public static string SetValueForToken = "";
        string str = ConfigurationManager.AppSettings["str"];
        private void StoreFrm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;
           
            ID = Dashboard.ID;
            loadAllStores();
        }

        private void loadAllStores()
        {
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
                    //account.Columns["ID"].Visible = false;
                  
                    
                    storegrid.DataSource = account;
                    storegrid.Columns["storeID"].Visible = false;
                    storegrid.Columns["storeCode"].HeaderText = "Store Code";
                    storegrid.Columns["storeName"].HeaderText = "Store Name";
                    storegrid.Columns["brandName"].HeaderText = "Brand";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
       
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

     
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void CreateTab_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
            saveBtn.Text = "Save";
            storeCode.Text = "";
            storeName.Text = "";
        }

        private void StoreList_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
            loadAllStores();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void storegrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bunifuPages1.PageIndex = 1;
            IDstore= storegrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            storeCode.Text= storegrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            storeName.Text= storegrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            saveBtn.Text = "Update";
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if(storeCode.Text!="" && storeName.Text!="")
            {

            
            if(saveBtn.Text=="Update")
                {
                    Cursor = Cursors.WaitCursor;
                    using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(str);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        RequestParameter.createstoresClass lgn = new RequestParameter.createstoresClass { update = 1, storeCode = storeCode.Text, storeName = storeName.Text, lastUpdatedBy = int.Parse(ID), storeID = int.Parse(IDstore) };
                        var response = client.PostAsJsonAsync("api/Stores/UpdateStore", lgn).Result;

                        var a = response.Content.ReadAsStringAsync();
                        string json = a.Result;
                            RequestParameter.createstoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.createstoresClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            bunifuSnackbar1.Show(this, Reply.message,
                                          Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                          Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                          Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            //MessageBox.Show(Reply.message);
                        storeCode.Text = "";
                        storeName.Text = "";
                        loadAllStores();
                        bunifuPages1.PageIndex = 0;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                        Cursor = Cursors.Arrow; // change cursor to normal type
                    }
                
            }
            if (saveBtn.Text == "Save")
                {
                    Cursor = Cursors.WaitCursor;
                    using (var client = new HttpClient())
                {
                    try
                        {
                           
                            client.BaseAddress = new Uri(str);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            RequestParameter.createstoresClass lgn = new RequestParameter.createstoresClass { add = 1, storeCode = storeCode.Text, storeName = storeName.Text, lastUpdatedBy = int.Parse(ID), createdBy = int.Parse(ID) };
                        var response = client.PostAsJsonAsync("api/Stores/InsertStore", lgn).Result;

                        var a = response.Content.ReadAsStringAsync();
                        string json = a.Result;
                            RequestParameter.createstoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.createstoresClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            bunifuSnackbar1.Show(this, Reply.message,
                                                                     Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                                                     Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                                                     Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            //MessageBox.Show(Reply.message);

                        }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                        Cursor = Cursors.Arrow; // change cursor to normal type
                    }
                storeCode.Text = "";
                storeName.Text = "";
                loadAllStores();
                bunifuPages1.PageIndex = 0;
            }

            }
            else
            {
              bunifuSnackbar1.Show(this, "Store Code and Store Name cannot be empty",
              Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "",
              Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
              Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
            }
        }

        private void storegrid_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Delete)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Store?",
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
                           
                            IDstore = storegrid[0, storegrid.CurrentCell.RowIndex].Value.ToString() ;
                            RequestParameter.deletestoresClass lgn = new RequestParameter.deletestoresClass { storeID = int.Parse(IDstore), delete = 1, update = 1, lastUpdatedBy = int.Parse(ID) };
                            var response = client.PostAsJsonAsync("api/Stores/DeleteStore", lgn).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.deletestoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.deletestoresClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            bunifuSnackbar1.Show(this, Reply.message,
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
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
                loadAllStores();
                Cursor = Cursors.Arrow; // change cursor to normal type
            }
        }

        private void storegrid_DoubleClick(object sender, EventArgs e)
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
    }
}
