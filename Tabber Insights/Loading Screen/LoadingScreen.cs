using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tabber_Insights.Loading_Screen
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            TabberInsights TabberInsights = new TabberInsights();
            TabberInsights.Show();

            this.Text = "Loaded";
            this.Hide();
        }
    }
}
