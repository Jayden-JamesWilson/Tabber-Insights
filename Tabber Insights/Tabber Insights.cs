using System.Runtime.InteropServices;
using Tabber_Insights.Tabber_Focus;

namespace Tabber_Insights
{
    public partial class TabberInsights : Form
    {
        public TabberInsights()
        {
            string localAppData =
            Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);
            string userFilePath
              = Path.Combine(localAppData, "Tabber");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);

            //if it's not already there, 
            //copy the file from the deployment location to the folder
            string sourceFilePath = Path.Combine(
              System.Windows.Forms.Application.StartupPath, "TabberInsightsData.txt");
            string destFilePath = Path.Combine(userFilePath, "TabberInsightsData.txt");
            if (!File.Exists(destFilePath))
                File.Copy(sourceFilePath, destFilePath);

            InitializeComponent();

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