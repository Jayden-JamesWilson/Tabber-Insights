using System.Runtime.InteropServices;
using System.Xml;
using Tabber_Insights.Tabber_Focus;

namespace Tabber_Insights
{
    public partial class TabberInsights : Form
    {
        public TabberInsights()
        {
            InitializeComponent();

            TabberGoalsPro Tabber_Goals_Pro = new TabberGoalsPro();
            this.Controls.Add(Tabber_Goals_Pro);
            Tabber_Goals_Pro.Dock = DockStyle.Fill;

            
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.Refresh();
            Properties.Settings.Default.Upgrade();
        }


     


        private void TabberInsights_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}