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
using Newtonsoft.Json;
using Bunifu.UI.WinForms;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Imtiaz_AlArbani
{
    public partial class BrandFrm : Form
    {
       
        public BrandFrm()
        {
            InitializeComponent();

           

        }
        String token;
        String ID;
        string IDstore;
        string[] values;
        string[] storeIDwithoutdash;
        public static string SetValueForToken = "";
        public static string IDD = "";
        string brandcode;
        string brandid;
        string str = ConfigurationManager.AppSettings["str"];
        private void CreateTab_Click(object sender, EventArgs e)
        {
            saveBtn.Text = "Save";
            //BunifuTransition transition = new BunifuTransition();
            //transition.ShowSync(bunifuPages1, false,Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.HorizSlideAndRotate);
            loadAllUncheckedBrands();
            bunifuPages1.PageIndex = 1;
        }

        private void BrandList_Click(object sender, EventArgs e)
        {   
            bunifuPages1.PageIndex = 0;
        }

        private void BrandFrm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;

            ID = Dashboard.ID;
            loadAllBrands();
          
        }

        private void loadAllUncheckedBrands()   
        {
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                checkedListBox1.Items.Remove(checkedListBox1.Items[i]);
                
            }
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getuncheckClass lgn = new RequestParameter.getuncheckClass {  };
                    var response = client.PostAsJsonAsync("api/Brands/GetAllUncheckedStores", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable uncheckedstores = JsonConvert.DeserializeObject<DataTable>(json);
                   

                    //brandgird.DataSource = forgrid;
                    for(int i=0;i<uncheckedstores.Rows.Count;i++)
                    {
                        checkedListBox1.Items.Add(uncheckedstores.Rows[i][1]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void loadAllBrands()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getallbrandClass lgn = new RequestParameter.getallbrandClass { get = 1 };
                    var response = client.PostAsJsonAsync("api/Brands/GetAllBrands", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable forgrid = JsonConvert.DeserializeObject<DataTable>(json);
                    forgrid.Columns.Remove("createdBy");
                    forgrid.Columns.Remove("createdOn"); 
                    forgrid.Columns.Remove("brand"); 
                    forgrid.Columns.Remove("lastUpdatedBy");
                    forgrid.Columns.Remove("lastUpdatedOn");
                    
                   if(forgrid.Rows.Count>0)
                    {
                        brandgird.DataSource = forgrid;
                        brandgird.Columns["brandID"].Visible = false;
                        brandgird.Columns["brandCode"].HeaderText = "Brand Code";
                        brandgird.Columns["brandName"].HeaderText = "Brand Name";
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        #region rough
        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        #endregion
       
      
        private void saveBtn_Click(object sender, EventArgs e)
        {
            List<RequestParameter.storess> data = new List<RequestParameter.storess>();
            var texts = this.checkedListBox1.CheckedItems.Cast<object>()
          .Select(x => this.checkedListBox1.GetItemText(x));

            string a = string.Join(",", texts);

            values = a.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                string tt = values[i];
                storeIDwithoutdash = tt.ToString().Split('-');

                data.Add(new RequestParameter.storess
                {
                    StoreCode = storeIDwithoutdash[0]
                });
            }


            var json2 = JsonConvert.SerializeObject(new
            {
                data
            });

            if (saveBtn.Text == "Update")
            {
                Cursor = Cursors.WaitCursor;
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(str);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        RequestParameter.createbrandClass cls = new RequestParameter.createbrandClass { update = 1,brandID=int.Parse(brandid), brandCode=brandcode, brandName = brandNameTextBox.Text, lastUpdatedBy = int.Parse(ID), storeBrandList = data };
                        var response = client.PostAsJsonAsync("api/Brands/UpdateBrand", cls).Result;

                        var aa = response.Content.ReadAsStringAsync();
                        string json = aa.Result;
                        RequestParameter.createbrandClass Reply = JsonConvert.DeserializeObject<RequestParameter.createbrandClass>(json);
                        // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                        bunifuSnackbar1.Show(this, Reply.message,
                                           Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                           Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                           Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                        brandNameTextBox.Text = "";

                        loadAllBrands();
                        loadAllUncheckedBrands();
                        bunifuPages1.PageIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                Cursor = Cursors.Arrow; // change cursor to normal type

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
                        RequestParameter.createbrandClass cls = new RequestParameter.createbrandClass { add = 1, brandName = brandNameTextBox.Text, brandCode = brandCodde.Text, lastUpdatedBy = int.Parse(ID), createdBy = int.Parse(ID), storeBrandList= data };
                        var response = client.PostAsJsonAsync("api/Brands/InsertBrand", cls).Result;

                        var aa = response.Content.ReadAsStringAsync();
                        string json = aa.Result;
                        RequestParameter.createbrandClass Reply = JsonConvert.DeserializeObject<RequestParameter.createbrandClass>(json);
                        // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                        bunifuSnackbar1.Show(this, Reply.message,
                                           Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                           Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                           Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                        brandNameTextBox.Text = "";
                        brandCodde.Text = "";

                        loadAllBrands();
                        loadAllUncheckedBrands();
                        bunifuPages1.PageIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                Cursor = Cursors.Arrow; // change cursor to normal type

            }
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            if (bunifuPages1.PageIndex == 0)
            {
                SetValueForToken = token;
                IDD = ID;
                Dashboard str = new Dashboard();
                this.Hide();
                str.ShowDialog();
                this.Close();
            }
            else
            {
                bunifuPages1.PageIndex = 0;
            }
          

         
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }


        private void loadAllUncheckedBrandsalongwithBrand()
        {
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                checkedListBox1.Items.Remove(checkedListBox1.Items[i]);

            }

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.brand br = new RequestParameter.brand { brandCode= brandcode };
                    var response = client.PostAsJsonAsync("api/Brands/GetStoresAgainstBrandCode", br).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable checkedstores = JsonConvert.DeserializeObject<DataTable>(json);

                  
                    //brandgird.DataSource = forgrid;
                    for (int i = 0; i < checkedstores.Rows.Count; i++)
                    {
                        checkedListBox1.Items.Add(checkedstores.Rows[i][1]);
                        checkedListBox1.SetItemChecked(i, true);
                    }
                   
                }
                    catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }



            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.getuncheckClass lgn = new RequestParameter.getuncheckClass { };
                    var response = client.PostAsJsonAsync("api/Brands/GetAllUncheckedStores", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable uncheckedstores = JsonConvert.DeserializeObject<DataTable>(json);


                    //brandgird.DataSource = forgrid;
                    for (int i = 0; i < uncheckedstores.Rows.Count; i++)
                    {
                        checkedListBox1.Items.Add(uncheckedstores.Rows[i][1]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        [STAThread()]
        private void brandgird_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

           
            brandid = brandgird.Rows[e.RowIndex].Cells["brandID"].Value.ToString();
            brandcode = brandgird.Rows[e.RowIndex].Cells["brandCode"].Value.ToString();
            brandCodde.Text = brandgird.Rows[e.RowIndex].Cells["brandCode"].Value.ToString();
            brandNameTextBox.Text = brandgird.Rows[e.RowIndex].Cells["brandName"].Value.ToString();
            //instantiate Bunifu Loader using Bunifu.UI.WinForms

            loadAllUncheckedBrandsalongwithBrand();
            


            saveBtn.Text = "Update";
            bunifuPages1.PageIndex = 1;
            Cursor = Cursors.Arrow; // change cursor to normal type
        }
       
        private void bunifuTextBox2_TextChange(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(bunifuPictureBox2.Text))
            //{
            //    checkedListBox1.DataSource = doctors;
            //}
            //else
            //{
            //    var filteredDoctors =
            //        new BindingList<CheckedListBoxItem<Doctor>>
            //        (doctors.Where(x => x.DataBoundItem.Name.StartsWith(searchTextBox.Text))
            //        .ToList());
            //    doctorsCheckedListBox.DataSource = filteredDoctors;
            //}
            //for (var i = 0; i < doctorsCheckedListBox.Items.Count; i++)
            //{
            //    doctorsCheckedListBox.SetItemCheckState(i,
            //        ((CheckedListBoxItem<Doctor>)doctorsCheckedListBox.Items[i]).CheckState);
            //}
            //var dv = checkedListBox1.DataSource as DataView;
            //var filter = bunifuTextBox2.Text.Trim().Length > 0
            //    ? $"Item LIKE '{checkedListBox1.Text}*'"
            //    : null;

            //dv.RowFilter = filter;

            //for (var i = 0; i < checkedListBox1.Items.Count; i++)
            //{
            //    var drv = checkedListBox1.Items[i] as DataRowView;
            //    var chk = Convert.ToBoolean(drv["Checked"]);
            //    checkedListBox1.SetItemChecked(i, chk);
            //}
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var dv = checkedListBox1.DataSource as DataView;
            var drv = dv[e.Index];
            drv["Checked"] = e.NewValue == CheckState.Checked ? true : false;
        }

        private void brandgrid_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Delete)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Brand?",
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

                            brandid = brandgird[0, brandgird.CurrentCell.RowIndex].Value.ToString();
                            RequestParameter.deletebrandClass lgn = new RequestParameter.deletebrandClass { brandID = int.Parse(brandid), delete = 1, update = 1, lastUpdatedBy = int.Parse(ID) };
                            var response = client.PostAsJsonAsync("api/Brands/DeleteBrand", lgn).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.deletestoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.deletestoresClass>(json);
                            // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                            //MessageBox.Show(Reply.message);
                            if(Reply.status=="404")
                            {
                                bunifuSnackbar1.Show(this, Reply.message,
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                            }
                            else { 
                            bunifuSnackbar1.Show(this, Reply.message,
                                         Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                         Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                         Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
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
                    // If 'No', do something here.
                }
                loadAllBrands();
                Cursor = Cursors.Arrow; // change cursor to normal type
            }
        }
    }
}
