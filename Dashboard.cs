using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imtiaz_AlArbani
{
    public partial class Dashboard : Form
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        //TODO: Don't forget to include using System.Runtime.InteropServices.

        internal static class NativeWinAPI
        {
            internal static readonly int GWL_EXSTYLE = -20;
            internal static readonly int WS_EX_COMPOSITED = 0x02000000;

            [DllImport("user32")]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32")]
            internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }
       
        public Dashboard()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;
            //int style = NativeWinAPI.GetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE);
            //style |= NativeWinAPI.WS_EX_COMPOSITED;
            //NativeWinAPI.SetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE, style);
            enableDoubleBuff(panel1);
            enableDoubleBuff(panel2);
            enableDoubleBuff(panel3);
            enableDoubleBuff(panel4);
            enableDoubleBuff(bunifuGradientPanel1);
            enableDoubleBuff(flowLayoutPanel1);
          
         
        }
        String token;
        String userID;
        public static string SetValueForToken = "";
        public static string ID = "";
        public static void enableDoubleBuff(System.Windows.Forms.Control cont)
        {
            System.Reflection.PropertyInfo DemoProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            DemoProp.SetValue(cont, true, null);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
        
            token = LoginFrm.SetValueForToken;
            userID = LoginFrm.userId;
            bunifuVScrollBar1.Visible = false;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }

        private void bunifuTileButton6_Click(object sender, EventArgs e)
        {
            SettingFrm dash = new SettingFrm();
           
            dash.ShowDialog();
            this.Close();
        }

        private void bunifuTileButton5_Click(object sender, EventArgs e)
        {
            DeviceInit dash = new DeviceInit();
      
            this.Hide();

            dash.ShowDialog();

            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuTileButton5_Click_1(object sender, EventArgs e)
        {

            SetValueForToken = token;
            ID = userID;
            DeviceInit us = new DeviceInit();
            this.Hide();

            us.ShowDialog();
            this.Close();

        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            UserFrm us = new UserFrm();
            this.Hide();
           
            us.ShowDialog();
            this.Close();
        }

        private void Store_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            StoreFrm str = new StoreFrm();
            this.Hide();
            str.ShowDialog();
            this.Close();
        }

        private void Brand_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            BrandFrm dash = new BrandFrm();
            this.Hide();
            dash.ShowDialog();
            this.Close();
        }

        private void CompanyInfo_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            CompanyInformationFrm dash = new CompanyInformationFrm();
            this.Hide();
            dash.ShowDialog();
            this.Close();
        }

        private void setting_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            SettingFrm dash = new SettingFrm();
            this.Hide();
            dash.ShowDialog();
            this.Close();

        }

        private void label_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            LabelFrm dash = new LabelFrm();
            this.Hide();
            dash.ShowDialog();
            this.Close();
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void importData_Click(object sender, EventArgs e)
        {
            SetValueForToken = token;
            ID = userID;
            ImportData dash = new ImportData();
            this.Hide();
            dash.ShowDialog();
            this.Close();
        }

        private void bunifuTileButton3_Click(object sender, EventArgs e)
        {
            LoginFrm logg = new LoginFrm();
            this.Hide();
            logg.ShowDialog();
            this.Close();
        }
    }
}
