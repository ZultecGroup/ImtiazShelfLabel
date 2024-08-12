using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Imtiaz_AlArbani
{
    public partial class DeviceInit : Form
    {
      
        public DeviceInit()
        {
            InitializeComponent();

          
        }
        String token;
        String ID;
        string deviceId;
        public static string SetValueForToken = "";
        string str = ConfigurationManager.AppSettings["str"];
        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void DeviceInit_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;
            ID = Dashboard.ID;
            loadAllDevices();
            
        }

        private void loadAllDevices()
        {
            using (var client = new HttpClient())
            {
                try
                {


                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                   RequestParameter.getalldeviceClass lgn = new RequestParameter.getalldeviceClass { get = 1 };
                    var response = client.PostAsJsonAsync("api/DeviceConfiguration/GetAllDevices", lgn).Result;
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    DataTable account = JsonConvert.DeserializeObject<DataTable>(json);


                    bunifuDataGridView1.DataSource = account;
                    bunifuDataGridView1.Columns["deviceID"].HeaderText = "Device ID";
                    bunifuDataGridView1.Columns["deviceDesc"].HeaderText = "Device Description";
                    bunifuDataGridView1.Columns["status"].HeaderText = "Status";
                    bunifuDataGridView1.Columns["hardwareID"].HeaderText = "Hardware ID";
                    bunifuDataGridView1.Columns["licKey"].HeaderText = "License Key";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }



        //grid double click
        private void bunifuDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            deviceId = bunifuDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            name.Text = bunifuDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string status = bunifuDataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            hardware.Text= bunifuDataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            lickey.Text= bunifuDataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
          

            if(status=="Registered")
            {
                activateBtn.Enabled = false;
                activateBtn.IdleFillColor = Color.Green;
            }
            else
            {
                activateBtn.Enabled = true;
                activateBtn.IdleFillColor = Color.DodgerBlue;
            }
        }

        private void activateBtn_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.ActivatedeviceClass lgn = new RequestParameter.ActivatedeviceClass { deviceSerialNo = hardware.Text.ToString(), deviceLicKey = lickey.Text.ToString() };
                    var response = client.PostAsJsonAsync("api/DeviceConfiguration/ActivateDevice", lgn).Result;
                  
                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    RequestParameter.ActivatedeviceClass Reply = JsonConvert.DeserializeObject<RequestParameter.ActivatedeviceClass>(json);
                    // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                    bunifuSnackbar1.Show(this, Reply.message,
                                           Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000, "",
                                           Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter,
                                           Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner);
                    loadAllDevices();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
       
        //this is delete thing
        private void bunifuDataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
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
                         RequestParameter.DeletedeviceClass lgn = new RequestParameter.DeletedeviceClass { deviceID = deviceId, delete = 1, update=1 , LastUpdatedBy=int.Parse(ID)};
                            var response = client.PostAsJsonAsync("api/DeviceConfiguration/DeleteDevice", lgn).Result;

                            var a = response.Content.ReadAsStringAsync();
                            string json = a.Result;
                            RequestParameter.DeletedeviceClass Reply = JsonConvert.DeserializeObject<RequestParameter.DeletedeviceClass>(json);
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
                loadAllDevices();
            }
        }


        //save button for details
        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.SaveedeviceClass lgn = new RequestParameter.SaveedeviceClass { deviceID = int.Parse(deviceId), DeviceDesc = name.Text, update=1,LastUpdatedBy=int.Parse(ID) };
                    var response = client.PostAsJsonAsync("api/DeviceConfiguration/UpdateDevice", lgn).Result;

                    var a = response.Content.ReadAsStringAsync();
                    string json = a.Result;
                    RequestParameter.SaveedeviceClass Reply = JsonConvert.DeserializeObject<RequestParameter.SaveedeviceClass>(json);
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
            loadAllDevices();
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            Dashboard str = new Dashboard();
            this.Hide();
            str.ShowDialog();
            this.Close();

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
