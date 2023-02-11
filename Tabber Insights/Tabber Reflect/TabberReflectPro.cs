using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tabber_Insights.Home_Page;
using Tabber_Insights.Properties;
using Tabber_Insights.Tabber_Focus;
using Tabber_Insights.Tabber_Tasks;

namespace Tabber_Insights.Tabber_Reflect
{
    public partial class TabberReflectPro : UserControl
    {
        
        #region Constructor 
        public string? Feeling = "";
        public int Counter = Settings.Default.ReflectionScore;

        public TabberReflectPro()
        {
            InitializeComponent();

            LogReflectionButton.Enabled = false;
            LogReflectionButton.BackColor = Color.FromArgb(249, 249, 249);

            ReflectionLogs.Text = Settings.Default.ReflectionLogs;
            Settings.Default.Save();

            if (ReflectionLogs.Text == "")
            {
                ClearLogsButton.Enabled = false;
                ClearLogsButton.BackColor = Color.FromArgb(249, 249, 249);
            }
            else
            {
                ClearLogsButton.Enabled = true;
                ClearLogsButton.BackColor = Color.White;

                #region Counter Validation
                if (Counter >= 0)
                {
                    if (Counter <= 5 && Counter >= 1)
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter - 3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter - 2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter + 0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                    else
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter -3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter -2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter +0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                }
                else
                {
                    Counter = 0;
                }
                #endregion
            }
        }
        #endregion

        #region Overrides
        private void TabberReflectPro_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void ClearFeelingSelecton()
        {
            foreach (Control controls in FeelingPanel.Controls)
            {
                if (controls is RoundFluentButton)
                {
                    ((RoundFluentButton)controls).BackColor = Color.White;
                }
            }
        }

        #endregion

        #region Save Reflections
        public void SaveReflection()
        {
            Settings.Default.ReflectionLogs = ReflectionLogs.Text;
            Settings.Default.ReflectionScore = Counter;
            Settings.Default.Save();
        }
        #endregion

        #region Add Reflection
        private void LogReflectionButton_Click(object sender, EventArgs e)
        {
            Settings.Default.LastReflectionLog = "";
            Settings.Default.LastReflectionLog = $"{Feeling?.ToString()} - {DateTime.Now.ToString("dd/MM/yyyy")}";

            if(Feeling != "" && ReflectionNoteTextBox.Text != "")
            {
                #region Counter Validation
                if (Counter >= 0)
                {
                    if (Counter <= 5 && Counter >= 1)
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter - 3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter - 2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter + 0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                    else
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter - 3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter - 2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter + 0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                }
                else
                {
                    Counter = 0;
                }
                #endregion

                ReflectionLogs.Text = ReflectionLogs.Text.Insert(0, $"{DateTime.Now}\n{ReflectionNoteTextBox.Text}\n{Feeling}\n\n");
                Feeling = "";

                ReflectionNoteTextBox.Text = "";
                LogReflectionButton.Enabled = false;
                LogReflectionButton.BackColor = Color.FromArgb(249, 249, 249);

                ClearLogsButton.Enabled = true;
                ClearLogsButton.BackColor = Color.White;

                ClearFeelingSelecton();
                SaveReflection();
            }
            else if (Feeling != "" && ReflectionNoteTextBox.Text == "")
            {
                #region Counter Validation
                if (Counter >= 0)
                {
                    if (Counter <= 5 && Counter >= 1)
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter - 3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter - 2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter + 0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                    else
                    {
                        TipsPane.Visible = false;
                        if (Feeling == "Sad")
                        {
                            Counter = Counter - 3;
                        }
                        if (Feeling == "Unhappy")
                        {
                            Counter = Counter - 2;
                        }
                        if (Feeling == "Neutral")
                        {
                            Counter = Counter + 0;
                        }
                        if (Feeling == "Happy")
                        {
                            Counter = Counter + 1;
                        }
                        if (Feeling == "Very Happy")
                        {
                            Counter = Counter + 2;
                        }
                    }
                }
                else
                {
                    Counter = 0;
                }
                #endregion

                ReflectionLogs.Text = ReflectionLogs.Text.Insert(0, $"{DateTime.Now}\n{Feeling}\n\n");
                Feeling = "";

                ReflectionNoteTextBox.Text = "";
                LogReflectionButton.Enabled = false;
                LogReflectionButton.BackColor = Color.FromArgb(249, 249, 249);

                ClearLogsButton.Enabled = true;
                ClearLogsButton.BackColor = Color.White;

                ClearFeelingSelecton();
                SaveReflection();
            }
            else
            {

            }
        }
        #endregion

        #region Clear Reflections
        private void ClearLogsButton_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to clear all reflection logs. You can't undo this action.";
            string title = "Clear Reflection Logs";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ReflectionLogs.Clear();
                Settings.Default.ReflectionScore = 25;
                Settings.Default.ReflectionLogs = "";
                Settings.Default.Save();

                ClearLogsButton.Enabled = false;
                ClearLogsButton.BackColor = Color.FromArgb(249, 249, 249);
                TipsPane.Visible = false;
            }
            else
            {

            }
        }
        #endregion

        #region Feeling Scale
        private void Feeling_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Feeling = button.Tag.ToString();

            foreach (Control c in FeelingPanel.Controls)
            {
                if (c is RoundFluentButton)
                {
                    ((RoundFluentButton)c).BackColor = Color.White;
                }
            }

            button.BackColor = Color.FromArgb(229, 229, 229);

            LogReflectionButton.Enabled = true;
            LogReflectionButton.BackColor = Color.White;
        }
        #endregion

        #region Side Panel
        private void TabberGoalsButton_Click(object sender, EventArgs e)
        {
            SaveReflection();

            TabberGoalsPro tabberGoalsPro = new TabberGoalsPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(tabberGoalsPro);
                    tabberGoalsPro.Dock = DockStyle.Fill;
                    f.Controls.Remove(this);
                }
            }
        }

        private void TabberFocusButton_Click(object sender, EventArgs e)
        {
            SaveReflection();

            TabberFocusPro tabberFocusPro = new TabberFocusPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(tabberFocusPro);
                    tabberFocusPro.Dock = DockStyle.Fill;
                    f.Controls.Remove(this);
                }
            }
        }
        private void TabberListsButton_Click(object sender, EventArgs e)
        {
            TabberListsPro tabberListsPro = new TabberListsPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(tabberListsPro);
                    tabberListsPro.Dock = DockStyle.Fill;
                    f.Controls.Remove(this);
                }
            }
        }
        private void HomeButton_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(home);
                    home.Dock = DockStyle.Fill;
                    f.Controls.Remove(this);
                }
            }
        }
        #endregion
    }
}
