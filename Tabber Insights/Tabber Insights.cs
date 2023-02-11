using System.Runtime.InteropServices;
using Tabber_Insights.Tabber_Focus;

namespace Tabber_Insights
{
    public partial class TabberInsights : Form
    {
        public TabberInsights()
        {
            InitializeComponent();

            // Copy user settings from previous application version if necessary
            if (Properties.Settings.Default.UpdateSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }

            TabberGoalsPro Tabber_Goals_Pro = new TabberGoalsPro();
            this.Controls.Add(Tabber_Goals_Pro);
            Tabber_Goals_Pro.Dock = DockStyle.Fill;
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}