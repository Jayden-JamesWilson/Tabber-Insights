using PowerArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tabber_Insights.Tabber_Focus;
using Tabber_Insights.Tabber_Reflect;
using Tabber_Insights.Tabber_Tasks;
using Windows.UI.Notifications;

namespace Tabber_Insights.Home_Page
{
    public partial class HomePage : UserControl
    {
        #region Constructor
        public int? Feeling = Properties.Settings.Default.ReflectionScore;
        public string? LastLog = Properties.Settings.Default.LastReflectionLog.ToString();

        private int TotalFocusSeconds;
        private int TotalBreakSeconds;

        int FocusMinutes = 30;
        int FocusSeconds = 0;

        int BreakMinutes = 5;
        int BreakSeconds = 0;

        Color CustomRed = Color.FromArgb(187, 39, 61);
        Color CustomOrange = Color.FromArgb(211, 125, 46);
        Color CustomGreen = Color.FromArgb(24, 136, 79);

        public static void KeepScreenActive()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        [DllImport("kernel32.dll")]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        [FlagsAttribute]
        enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        public HomePage()
        {
            InitializeComponent();

            #region Calculations
            int FeelingRounded = (int)Feeling * 6;
            #endregion

            #region Last Reflection Log
            if (LastLog != "")
            {
                ReflectionEntry.Text = LastLog;
            }
            else
            {
                ReflectionEntry.Text = "No Entries Added";
            }
            #endregion

            #region Reflection Score
            if (FeelingRounded < 1)
            {
                FeelingScore.Width = 6;
                FeelingScore.BackColor = CustomRed;
            }
            else if (FeelingRounded >= 1 && FeelingRounded <= 100)
            {
                FeelingScore.Width = FeelingRounded;
                FeelingScore.BackColor = CustomRed;
            }
            else if (FeelingRounded > 100 && FeelingRounded <= 200)
            {
                FeelingScore.Width = FeelingRounded;
                FeelingScore.BackColor = CustomOrange;
            }
            else if (FeelingRounded > 200 && FeelingRounded <= 300)
            {
                FeelingScore.Width = FeelingRounded;
                FeelingScore.BackColor = CustomGreen;
            }
            else
            {
                FeelingScore.Width = FeelingRounded;
                FeelingScore.BackColor = CustomGreen;
            }
            #endregion

            #region Goals
            GoalTitle1.Text = Properties.Settings.Default.TabberGoalTitles[0];
            GoalTitle2.Text = Properties.Settings.Default.TabberGoalTitles[1];
            GoalTitle3.Text = Properties.Settings.Default.TabberGoalTitles[2];
            #endregion

            #region Focus Lifetime Minutes
            int TotalMinutes = Properties.Settings.Default.TotalFocusSeconds / 60;
            this.TotalFocusMinutes.Text = $"{TotalMinutes} Minutes";

            if(TotalMinutes > 0) 
            { 
                ClearFocusMinutesButton.Enabled = true;
            }
            else
            {
                ClearFocusMinutesButton.Enabled = false;
            }
            #endregion
        }
        #endregion

        #region Side Panel
        private void TabberGoalsButton_Click(object sender, EventArgs e)
        {
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

        private void TabberReflectButton_Click(object sender, EventArgs e)
        {
            TabberReflectPro tabberReflectPro = new TabberReflectPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(tabberReflectPro);
                    tabberReflectPro.Dock = DockStyle.Fill;
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
        private void TabberFocusButton_Click(object sender, EventArgs e)
        {
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
        #endregion

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            StopButton.Enabled = true;

            TotalFocusSeconds = (30 * 60) + FocusSeconds;
            TotalBreakSeconds = (5 * 60) + BreakSeconds;

            FocusTimer.Start();

            GoalsPanel.Enabled = false;
            SidePanel.Enabled = false;
            StatusPanel.BackColor = Color.FromArgb(148, 148, 148);
            StatusPanel.BorderColor = Color.FromArgb(148, 148, 148);

            KeepScreenActive();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.BreakTimer.Stop();
            this.FocusTimer.Stop();

            StartButton.Enabled = true;
            StopButton.Enabled = false;

            TotalBreakSeconds = 0;
            TotalFocusSeconds = 0;

            FocusTimeLabel.Text = "30:00";
            UpNextLabel.Text = $"Up Next: 5 Minute Break";

            GoalsPanel.Enabled = true;
            SidePanel.Enabled = true;
            StatusPanel.BackColor = Color.FromArgb(89, 97, 195);
            StatusPanel.BorderColor = Color.FromArgb(89, 97, 195);

            AllowSleep();
        }

        private void BreakTimer_Tick(object sender, EventArgs e)
        {
            if (TotalBreakSeconds > 0)
            {
                TotalBreakSeconds--;
                int minutes = TotalBreakSeconds / 60;
                int seconds = TotalBreakSeconds - (minutes * 60);
                this.FocusTimeLabel.Text = minutes.ToString() + ":" + seconds.ToString();
                this.UpNextLabel.Text = $"Up Next: 30 Minute Focus Period";
            }
            else
            {
                TotalFocusSeconds = (FocusMinutes * 60) + FocusSeconds;
                TotalBreakSeconds = (BreakMinutes * 60) + BreakSeconds;

                FocusMinutes = 30;
                FocusSeconds = 0;

                this.BreakTimer.Stop();
                this.FocusTimer.Start();

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Tabber Insights")
                    {
                        f.TopMost = true;
                        f.TopMost = false;
                        f.Focus();

                        MessageBox.Show($"Your 30 minute focus period starts now.", "Time To Focus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            if (TotalFocusSeconds > 0)
            {
                TotalFocusSeconds--;
                int minutes = TotalFocusSeconds / 60;
                int seconds = TotalFocusSeconds - (minutes * 60);
                this.FocusTimeLabel.Text = minutes.ToString() + ":" + seconds.ToString();
                this.UpNextLabel.Text = $"Up Next: 5 Minute Break";

                Properties.Settings.Default.TotalFocusSeconds = Properties.Settings.Default.TotalFocusSeconds + 1;
                Properties.Settings.Default.Save();

                int TotalMinutes = Properties.Settings.Default.TotalFocusSeconds / 60;
                this.TotalFocusMinutes.Text = $"{TotalMinutes.ToString()} Minutes"; 
            }
            else
            {
                TotalFocusSeconds = (30 * 60) + FocusSeconds;
                TotalBreakSeconds = (5 * 60) + BreakSeconds;

                BreakMinutes = Properties.Settings.Default.BreakPeriodMinutes;
                BreakSeconds = 0;

                this.FocusTimer.Stop();
                this.BreakTimer.Start();

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Tabber Insights")
                    {
                        f.TopMost = true;
                        f.TopMost = false;
                        f.Focus();

                        MessageBox.Show($"You have completed your focus period. Your 5 break starts now.", "Great Job!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ClearFocusMinutesButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TotalFocusSeconds = 0;
            Properties.Settings.Default.Save();

            TotalFocusMinutes.Text = "0";
            ClearFocusMinutesButton.Enabled = false;
        }

        private void HomePage_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
