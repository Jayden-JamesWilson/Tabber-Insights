using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tabber_Insights.Properties;

namespace Tabber_Insights.Tabber_Goals.Goals
{
    public partial class Goals_Pop_Up : UserControl
    {
        public Goals_Pop_Up()
        {
            InitializeComponent();
            GoalTitle1.Text = Settings.Default.TabberGoalTitles[0];
            GoalTitle2.Text = Settings.Default.TabberGoalTitles[1];
            GoalTitle3.Text = Settings.Default.TabberGoalTitles[2];
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Settings.Default.TabberGoalTitles[0] = GoalTitle1.Text.ToString();
            Settings.Default.TabberGoalTitles[1] = GoalTitle2.Text.ToString();
            Settings.Default.TabberGoalTitles[2] = GoalTitle3.Text.ToString();

            Properties.Settings.Default.Save();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Goals")
                {
                    f.Controls.Remove(this);
                    f.MinimizeBox = true;
                    f.MaximizeBox = true;
                    f.Text = "Tabber Insights";

                    f.WindowState = FormWindowState.Maximized;
                    f.Size = new Size(1268, 645);
                    f.FormBorderStyle = FormBorderStyle.Sizable;
                    f.Location = new Point(100, 100);
                    f.TopMost = false;
                    f.BringToFront();
                    f.Icon = Properties.Resources.Tabber_Insights;

                    TabberGoalsPro tabber_Goals_Pro = new TabberGoalsPro();
                    f.Controls.Add(tabber_Goals_Pro);
                    tabber_Goals_Pro.Dock = DockStyle.Fill;

                    f.Refresh();
                    break;
                }
            }

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.TabberGoalTitles[0] = GoalTitle1.Text.ToString();
            Settings.Default.TabberGoalTitles[1] = GoalTitle2.Text.ToString();
            Settings.Default.TabberGoalTitles[2] = GoalTitle3.Text.ToString();

            Properties.Settings.Default.Save();
        }
    }
}
