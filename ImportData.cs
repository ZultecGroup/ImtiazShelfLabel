using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Imtiaz_AlArbani
{
    public partial class ImportData : Form
    {
      
        public ImportData()
        {
            InitializeComponent();

        }
        ExcelApp.Application excelApp = new ExcelApp.Application();
        DataRow myNewRow;
        DataTable myTable;
        DataTable dt = new DataTable();
        public static string SetValueForToken = "";
        String token;
        String ID;
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        string str = ConfigurationManager.AppSettings["str"];
        private void Browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            path.Text = openFileDialog1.FileName;
            path.Enabled = false;
           
            if(path.Text!="openFileDialog1" || path.Text == "")
            {
                Process.Enabled = false;
            }
            if (path.Text == "openFileDialog1")
            {
                path.Text = "";
            }
             else
            {
                Process.Enabled = true;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            openFileDialog1.Dispose();
            Cursor = Cursors.WaitCursor;
            ProcessAssetCreationFile();
            Cursor = Cursors.Arrow;
            //backgroundWorker1.RunWorkerAsync();
            //importGrid.DataSource = dt;
        }
        System.Data.OleDb.OleDbConnection MyConnection;
        DataTable Sheetdt = new DataTable();
        DataTable Sheetdt2 = new DataTable();
        private void ProcessAssetCreationFile()
            {

            Application.DoEvents();
            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string header = "YES";
            string conStr, sheetName;

            conStr = string.Empty;
            switch (extension)
            {

                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03ConString, filePath, header);
                    break;

                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07ConString, filePath, header);
                    break;
            }
            MyConnection = new System.Data.OleDb.OleDbConnection(conStr);
            

            try
            {
                importGrid.DataSource = null;
                MyConnection.Open();
                DataTable dtSheet = MyConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                //string SheetName = dtSheet.Rows[dtSheet.Rows.Count-1]["TABLE_NAME"].ToString();
                string SheetName = string.Empty;
                string SheetName2 = string.Empty;

                if (dtSheet.Rows.Count > 0)
                {
                    SheetName = dtSheet.Rows[0]["TABLE_NAME"].ToString();
                    // Clean up special characters if needed (replace $ and single quotes)
                 //   SheetName = SheetName.Replace("$", "").Replace("'", "");
                }
                
                if (!string.IsNullOrEmpty(SheetName))
                {
                //    SheetName = Regex.Replace(SheetName, @"\s", ""); // Remove all whitespace characters

                    System.Data.OleDb.OleDbDataAdapter MyCommand;
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter($"SELECT * FROM [{SheetName}]", MyConnection);
                    MyCommand.TableMappings.Add("Table", "TestTable");
               
                    MyCommand.Fill(Sheetdt);
                   
                    foreach (DataColumn c in Sheetdt.Columns)
                    { 
                        c.ColumnName = String.Join("", c.ColumnName.Split());
                        //if(c.ColumnName != "ItemCode" && c.ColumnName != "Reference" && c.ColumnName != "Description" && c.ColumnName != "Size" && c.ColumnName != "Colour" && c.ColumnName != "BarCode" && c.ColumnName != "StoreID" && c.ColumnName != "BrandID" && c.ColumnName != "RegularPrice" && c.ColumnName != "SalePrice" && c.ColumnName != "SalesStartDate" && c.ColumnName != "SalesEndDate")
                        if(c.ColumnName.ToLower() != "itemcode" && c.ColumnName.ToLower() != "reference" && c.ColumnName.ToLower() != "itemdesc" && c.ColumnName.ToLower() != "size" && c.ColumnName.ToLower() != "colour" && c.ColumnName != "barcode" && c.ColumnName.ToLower() != "regularpricecode" && c.ColumnName.ToLower() != "salespricecode" && c.ColumnName.ToLower() != "brandcode" && c.ColumnName.ToLower() != "brandname" && c.ColumnName.ToLower() != "regularprice" && c.ColumnName.ToLower() != "saleprice" && c.ColumnName.ToLower() != "salesstartdate" && c.ColumnName.ToLower() != "salesenddate")
                        {
                            MessageBox.Show("File Not Correct, " +c.ColumnName.ToString() +" not found");
                            importBtn.Visible = false;
                            MyConnection.Close();
                            return;
                        }
                    }
                   
                    MyConnection.Close();

                    importGrid.ColumnHeadersVisible = false;
                    importGrid.DataSource = Sheetdt;
                    importGrid.ColumnHeadersVisible = true;
                    importGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    importGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    importBtn.Visible = true;
                }
                else
                {
                    MessageBox.Show("Please select sheet from the list first.");
                    // return false;
                }

                if (dtSheet.Rows.Count > 1)
                {
                    SheetName2 = dtSheet.Rows[dtSheet.Rows.Count - 1]["TABLE_NAME"].ToString();
                    if (!string.IsNullOrEmpty(SheetName2))
                    {
                        //    SheetName = Regex.Replace(SheetName, @"\s", ""); // Remove all whitespace characters

                        System.Data.OleDb.OleDbDataAdapter MyCommand;
                        MyCommand = new System.Data.OleDb.OleDbDataAdapter($"SELECT * FROM [{SheetName2}]", MyConnection);
                        MyCommand.TableMappings.Add("Table", "TestTable");

                        MyCommand.Fill(Sheetdt2);

                        //foreach (DataColumn c in Sheetdt.Columns)
                        //{
                        //    c.ColumnName = String.Join("", c.ColumnName.Split());
                        //    //if(c.ColumnName != "ItemCode" && c.ColumnName != "Reference" && c.ColumnName != "Description" && c.ColumnName != "Size" && c.ColumnName != "Colour" && c.ColumnName != "BarCode" && c.ColumnName != "StoreID" && c.ColumnName != "BrandID" && c.ColumnName != "RegularPrice" && c.ColumnName != "SalePrice" && c.ColumnName != "SalesStartDate" && c.ColumnName != "SalesEndDate")
                        //    if (c.ColumnName != "ItemCode" && c.ColumnName != "Reference" && c.ColumnName != "ItemDesc" && c.ColumnName != "Size" && c.ColumnName != "Colour" && c.ColumnName != "Barcode" && c.ColumnName != "RegularPriceCode" && c.ColumnName != "SalesPriceCode" && c.ColumnName != "BrandCode" && c.ColumnName != "BrandName" && c.ColumnName != "RegularPrice" && c.ColumnName != "SalePrice" && c.ColumnName != "SalesStartDate" && c.ColumnName != "SalesEndDate")
                        //    {
                        //        MessageBox.Show("File Not Correct, " + c.ColumnName.ToString() + " not found");
                        //        importBtn.Visible = false;
                        //        MyConnection.Close();
                        //        return;
                        //    }
                        //}

                        MyConnection.Close();

                        //importGrid.ColumnHeadersVisible = false;
                        //importGrid.DataSource = Sheetdt2;
                        //importGrid.ColumnHeadersVisible = true;
                        //importGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        //importGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        //importBtn.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select sheet from the list first.");
                        // return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //  return false;
            }
            
          
            

        }


    


        private async void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //usergrid.DataSource = dt;
            await Task.Delay(1000);
            
            MessageBox.Show("hellloo");

            Cursor = Cursors.Arrow;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //usergrid.PopulateWithSampleData();


            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string header = "YES";
            string conStr, sheetName;

            conStr = string.Empty;
            switch (extension)
            {

                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03ConString, filePath, header);
                    break;

                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel07ConString, filePath, header);
                    break;
            }

            //Get the name of the First Sheet.
            //using (OleDbConnection con = new OleDbConnection(conStr))
            //{
            //    using (OleDbCommand cmd = new OleDbCommand())
            //    {
            //        cmd.Connection = con;
            //        con.Open();
            //        DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //        sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            //        con.Close();
            //    }
            //}
            // Get the names of all sheets.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();
                DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dtExcelSchema != null)
                {
                    foreach (DataRow row in dtExcelSchema.Rows)
                    {
                        sheetName = row["TABLE_NAME"].ToString();
                        // The sheet name may include a dollar sign ($) and single quotes, so you may need to clean it up.
                        sheetName = sheetName.Replace("$", "").Replace("'", "");
                       // MessageBox.Show(sheetName);
                    }
                }

                con.Close();
            }
            //Read Data from the First Sheet.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {

                        cmd.CommandText = "SELECT * From [Sheet1$]";
                        cmd.Connection = con;
                        con.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con.Close();
                        importGrid.DataSource = dt;
                        //Populate DataGridView.
                        //usergrid.DataSource = dt;

                    }
                }
            }
            try {
                //importGrid.BeginInvoke(new InvokeDelegate(AttachData));
               

              //  MessageBox.Show("gellllllllllo");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void AttachData()
        {
            //dataGridView1.Table.... Bind data here (this method would be executed on UI thread)
        }
        private void ImportData_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            token = Dashboard.SetValueForToken;

            ID = Dashboard.ID;
        }

        private void CreateTab_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProcessAssetCreationFile();
            Process.Enabled = false;
            
            Cursor = Cursors.Arrow;

        }

        private void importBtn_Click(object sender, EventArgs e)
        {

            //List<String> data = new List<String>();
            //string JSONString = string.Empty;
            List<RequestParameter.importFileList> data = new List<RequestParameter.importFileList>();

            for (int i = 0; i < Sheetdt.Rows.Count; i++)
            {

                data.Add(new RequestParameter.importFileList
                {
                    
                    ItemCode = Sheetdt.Rows[i]["ItemCode"].ToString(),
                    Reference = Sheetdt.Rows[i]["Reference"].ToString(),
                    ItemDesc = Sheetdt.Rows[i]["ItemDesc"].ToString(),
                    Size = Sheetdt.Rows[i]["Size"].ToString(),
                    Colour = Sheetdt.Rows[i]["Colour"].ToString(),
                    BarCode = Sheetdt.Rows[i]["Barcode"].ToString(),
                    RegularPriceCode = Sheetdt.Rows[i]["RegularPriceCode"].ToString(),
                    SalesPriceCode = Sheetdt.Rows[i]["SalesPriceCode"].ToString(),
                    
                    BrandID = Sheetdt.Rows[i]["BrandCode"].ToString(),
                    BrandName = Sheetdt.Rows[i]["BrandName"].ToString(),
                  
                    RegularPrice = Sheetdt.Rows[i]["RegularPrice"].ToString(),
                    SalePrice = Sheetdt.Rows[i]["SalePrice"].ToString(),
                    SalesStartDate = Sheetdt.Rows[i]["SalesStartDate"].ToString(),
                    SalesEndDate = Sheetdt.Rows[i]["SalesEndDate"].ToString()
                });
            }


            var json2 = JsonConvert.SerializeObject(new
            {
                data
            });

            Cursor = Cursors.WaitCursor;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(str);
                    //client.Timeout = TimeSpan.FromMinutes(5);
                    client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    RequestParameter.importClass cls = new RequestParameter.importClass { importData = data };
                    var response = client.PostAsJsonAsync("api/ImportData/ImportData", cls).Result;

                    var aa = response.Content.ReadAsStringAsync();
                    string json = aa.Result;
                    RequestParameter.importClass Reply = JsonConvert.DeserializeObject<RequestParameter.importClass>(json);
                    // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                    MessageBox.Show(Reply.message);
                    importGrid.DataSource = null;
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            Cursor = Cursors.Arrow; // change cursor to normal type

            //for second sheet

            List<RequestParameter.importFileLists> data2 = new List<RequestParameter.importFileLists>();
            if(Sheetdt2.Rows.Count>1)
            { 
            for (int i = 0; i < Sheetdt2.Rows.Count; i++)
            {

                data2.Add(new RequestParameter.importFileLists
                {

                   
                    RegularPriceCode = Sheetdt2.Rows[i]["RegularPriceCode"].ToString(),
                    StoreCode = Sheetdt2.Rows[i]["StoreCode"].ToString(),
                    SalesPriceCode = Sheetdt2.Rows[i]["SalesPriceCode"].ToString(),
                  
                });
            }


            var json3 = JsonConvert.SerializeObject(new
            {
                data2
            });

            Cursor = Cursors.WaitCursor;
            using (var client = new HttpClient())
            {
                try
                {
                        client.BaseAddress = new Uri(str);
                        //client.Timeout = TimeSpan.FromMinutes(5);
                        client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        RequestParameter.importStoresClass cls = new RequestParameter.importStoresClass { importStoresPrices = data2 };
                        var response = client.PostAsJsonAsync("api/ImportData/ImportStoresPrices", cls).Result;

                        var aa = response.Content.ReadAsStringAsync();
                        string json = aa.Result;
                        RequestParameter.importStoresClass Reply = JsonConvert.DeserializeObject<RequestParameter.importStoresClass>(json);
                        // LoginClass account = JsonConvert.DeserializeObject<LoginClass>(json);
                        MessageBox.Show(Reply.message);
                        //   importGrid.DataSource = null;

                    }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            }
            Cursor = Cursors.Arrow; // change cursor to normal type

        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;

            Dashboard dash = new Dashboard();
            this.Hide();
            dash.ShowDialog();

            this.Close();
        }
        //  var JSONString = JsonConvert.SerializeObject(Sheetdt);

        // dbContext db = new dbContext();
        // List<string> jsonList = (List<string>)JsonConvert.DeserializeObject(JSONString, typeof(List<string>));

        //List<RequestParameter.importFileList> models = JsonConvert.DeserializeObject<List<RequestParameter.importFileList>>(JSONString);
        //List<RequestParameter.importFileList> models = JsonConvert.SerializeObject(Sheetdt, Newtonsoft.Json.Formatting.Indented);


    }
    }

