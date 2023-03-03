
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tabber_Insights.Home_Page;
using Tabber_Insights.Tabber_Goals.Goals;
using Tabber_Insights.Tabber_Reflect;
using Tabber_Insights.Tabber_Tasks;

namespace Tabber_Insights.Tabber_Focus
{
    public partial class TabberFocusPro : UserControl
    {
        #region Constructor
        private int TotalFocusSeconds;
        private int TotalBreakSeconds;

        int FocusMinutes = 30;
        int FocusSeconds = 0;

        int BreakMinutes = 5;
        int BreakSeconds = 0;

        int FocusSession = 0;
        int MaxFocusSessions = 1;

        bool PopOut = false;

        public string? FocusMode = "";

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

        public TabberFocusPro()
        {
            InitializeComponent();

            ClearModesButton.BackColor = Color.FromArgb(229, 229, 229);
            ClearGoalsButton.BackColor = Color.FromArgb(229, 229, 229);

            GoalTitle1.Text = Properties.Settings.Default.TabberGoalTitles[0];
            GoalTitle2.Text = Properties.Settings.Default.TabberGoalTitles[1];
            GoalTitle3.Text = Properties.Settings.Default.TabberGoalTitles[2];

            if(GoalTitle1.Text == "")
            {
                GoalTitle1.Text = "Goal Title 1";
            }
            if (GoalTitle2.Text == "")
            {
                GoalTitle2.Text = "Goal Title 2";
            }
            if (GoalTitle3.Text == "")
            {
                GoalTitle3.Text = "Goal Title 3";
            }

            Properties.Settings.Default.FocusPeriodMinutes = 30;
            Properties.Settings.Default.BreakPeriodMinutes = 0;
            Properties.Settings.Default.Save();

            UpNextLabel.Text = "Away To Start Focus Session";
            FocusTimeLabel.Text = "30:00";

        }
        #endregion

        #region Overrides
        const int WM_PARENTNOTIFY = 0x210;
        const int WM_LBUTTONDOWN = 0x201;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || (m.Msg == WM_PARENTNOTIFY && (int)m.WParam == WM_LBUTTONDOWN))
            {
                if (!SettingsPanel.ClientRectangle.Contains(SettingsPanel.PointToClient(Cursor.Position)))
                {
                    SettingsPanel.Hide();
                }
            }
            base.WndProc(ref m);
        }

        private void TabberFocus_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
        #endregion

        #region Tabber Goals 
        private void GoalButton1_Click(object sender, EventArgs e)
        {
            FocusedGoal.Text = GoalTitle1.Text;
            GoalButton1.Image = Properties.Resources.Complete_Active_24px;
            GoalButton2.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton3.Image = Properties.Resources.Complete_In_Active_24px;

            FocusedGoalTitle.Visible = true;

            ClearGoalsButton.BackColor = Color.White;
        }

        private void GoalButton2_Click(object sender, EventArgs e)
        {
            FocusedGoal.Text = GoalTitle2.Text;
            GoalButton1.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton2.Image = Properties.Resources.Complete_Active_24px;
            GoalButton3.Image = Properties.Resources.Complete_In_Active_24px;

            FocusedGoalTitle.Visible = true;

            ClearGoalsButton.BackColor = Color.White;
        }

        private void GoalButton3_Click(object sender, EventArgs e)
        {
            FocusedGoal.Text = GoalTitle3.Text;
            GoalButton1.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton2.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton3.Image = Properties.Resources.Complete_Active_24px;

            FocusedGoalTitle.Visible = true;

            ClearGoalsButton.BackColor = Color.White;
        }
        #endregion

        #region Timer
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            StopButton.Enabled = true;
            SettingsButton.Enabled = false;
            FocusModesPanel.Enabled = false;

            TotalFocusSeconds = (Properties.Settings.Default.FocusPeriodMinutes * 60) + FocusSeconds;
            TotalBreakSeconds = (Properties.Settings.Default.BreakPeriodMinutes * 60) + BreakSeconds;

            FocusTimer.Start();

            GoalsPanel.Enabled = false;
            SidePanel.Enabled = false;
            StatusPanel.BackColor= Color.FromArgb(148,148,148);
            StatusPanel.BorderColor = Color.FromArgb(148, 148, 148);


            KeepScreenActive();

            if(FocusMode == "Work")
            {
                Process.Start(new ProcessStartInfo("https://www.office.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("http://www.outlook.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("https://teams.microsoft.com") { UseShellExecute = true });
            }
            if(FocusMode == "Education")
            {
                Process.Start(new ProcessStartInfo("https://www.office.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("https://teams.microsoft.com") { UseShellExecute = true });
            }
            if(FocusMode == "Personal")
            {
                Process.Start(new ProcessStartInfo("https://www.instagram.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("https://discord.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("https://www.youtube.com") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo("https://www.facebook.com") { UseShellExecute = true });
            }
            if (FocusMode == "None")
            {
                
            }
        }

        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            UpNextLabel.Text = "Focusing";

            int FOCUS_SESSION2 = FocusSession + 1;
            string FOCUS_SESSION = FOCUS_SESSION2.ToString();
            string MAX_FOCUS_SESSION = MaxFocusSessions.ToString();
            CurrentFocusPeriodPanel.Text = $"Focus Session {FOCUS_SESSION} of {MAX_FOCUS_SESSION}";

            if (TotalFocusSeconds > 0)
            {
                
                TotalFocusSeconds--;
                int minutes = TotalFocusSeconds / 60;
                int seconds = TotalFocusSeconds - (minutes * 60);
                FocusTimeLabel.Text = $"{minutes:00}:{seconds:00}";

                Properties.Settings.Default.TotalFocusSeconds = Properties.Settings.Default.TotalFocusSeconds + 1;
                Properties.Settings.Default.Save();

                int TotalMinutes = 27 / 60; 
            }
            else
            {
                
                TotalFocusSeconds = (27 * 60) + FocusSeconds; 
                TotalBreakSeconds = (5 * 60) + BreakSeconds; 

                BreakMinutes = 27;
                BreakSeconds = 0;

                FocusSession++;
                this.FocusTimer.Stop();
                this.BreakTimer.Start();

                if (FocusSession >= MaxFocusSessions)
                {
                    this.BreakTimer.Stop();
                    this.FocusTimer.Stop();
                    MaxFocusSessions = 0;
                    FocusSession= 0;
                    CurrentFocusPeriodPanel.Text = $"Focus Session 0 of 0";

                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "Tabber Insights")
                        {
                            f.TopMost = true;
                            f.TopMost = false;
                            f.Focus();

                            GoalsPanel.Enabled = true;
                            SidePanel.Enabled = true;
                            StartButton.Enabled = true;
                            StopButton.Enabled = false;
                            MaxFocusSessions = 1;
                            StatusPanel.BackColor = Color.FromArgb(92, 92, 92);
                            StatusPanel.BorderColor = Color.FromArgb(92, 92, 92);
                            MessageBox.Show($"You have completed your focus session", "Great Job!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "Tabber Insights")
                        {
                            f.TopMost = true;
                            f.TopMost = false;
                            f.Focus();

                            MessageBox.Show($"You have completed your focus period. Your {Properties.Settings.Default.BreakPeriodMinutes} break starts now.", "Great Job!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void BreakTimer_Tick(object sender, EventArgs e)
        {
            UpNextLabel.Text = "Taking a Break";

            if (TotalBreakSeconds > 0)
            {
                TotalBreakSeconds--;
                int minutes = TotalBreakSeconds / 60;
                int seconds = TotalBreakSeconds - (minutes * 60);
                FocusTimeLabel.Text = $"{minutes:00}:{seconds:00}";

            }
            else
            {
                FocusMinutes = 27;
                FocusSeconds = 0;

                TotalFocusSeconds = (FocusMinutes * 60) + FocusSeconds;
                TotalBreakSeconds = (BreakMinutes * 60) + BreakSeconds;

                this.BreakTimer.Stop();
                this.FocusTimer.Start();

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Tabber Insights")
                    {
                        f.TopMost = true;
                        f.TopMost = false;
                        f.Focus();

                        MessageBox.Show($"Your {Properties.Settings.Default.FocusPeriodMinutes} minute focus period starts now.", "Time To Focus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.BreakTimer.Stop();
            this.FocusTimer.Stop();

            StartButton.Enabled = true;
            StopButton.Enabled = false;
            SettingsButton.Enabled = true;
            FocusModesPanel.Enabled = true;

            TotalBreakSeconds = 0;
            TotalFocusSeconds = 0;
            MaxFocusSessions = 1;
            FocusSession = 0;

            UpNextLabel.Text = "Not In a Focus Session";
            CurrentFocusPeriodPanel.Text = $"Focus Session 0 of 0";

            GoalsPanel.Enabled = true;
            SidePanel.Enabled = true;
            StatusPanel.BackColor = Color.FromArgb(89, 97, 195);
            StatusPanel.BorderColor = Color.FromArgb(89, 97, 195);

            Properties.Settings.Default.FocusPeriodMinutes = 30;
            Properties.Settings.Default.BreakPeriodMinutes = 0;
            Properties.Settings.Default.Save();

            UpNextLabel.Text = "Away Start a Focus Session";
            FocusTimeLabel.Text = "30:00";

            AllowSleep();

            if (ClearModesButton.Enabled == true)
            {
                FocusMode = "None";
                ClearModesButton.BackColor = Color.FromArgb(229, 229, 229);

                HomeFocusButton.BackColor = Color.White;
                WorkFocusButton.BackColor = Color.White;
                PersonalFocusButton.BackColor = Color.White;
            }
            else
            {
                FocusMode = "None";
            }
        }
        #endregion

        #region Pop Out 
        private void PopOutButton_Click(object sender, EventArgs e)
        {
            if (PopOut == false)
            {
                GoalsPanel.Visible = false;
                TitleBar.Visible = false;
                SidePanel.Visible = false;

                this.Size = new Size(414, 592);
                this.MinimumSize = new Size(0, 0);
                FocusPanel.Location = new Point(1070, 70);
                FocusPanel.Dock = DockStyle.Fill;
                PopOutButton.Image = Properties.Resources.Expand_Arrow_24px;

                PopOut= true;

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Tabber Insights")
                    {
                        f.MinimizeBox = false;
                        f.MaximizeBox = false;
                        f.Text = "Tabber Focus";

                        f.MinimumSize = new Size(0, 0);
                        f.WindowState = FormWindowState.Normal;
                        f.Size = new Size(414, 592);
                        f.FormBorderStyle = FormBorderStyle.FixedSingle;
                        f.TopMost = true;
                        f.Icon = Properties.Resources.Tabber_Focus;


                        Screen rightmost = Screen.AllScreens[0];
                        foreach (Screen screen in Screen.AllScreens)
                        {
                            if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                                rightmost = screen;
                        }

                        f.Left = rightmost.WorkingArea.Right - f.Width;
                        f.Top = rightmost.WorkingArea.Bottom - f.Height;

                        break;
                    }
                }
            }
            else
            {
                GoalsPanel.Visible = true;
                TitleBar.Visible = true;
                SidePanel.Visible = true;

                this.Size = new Size(1268, 645);
                FocusPanel.Size = new Size(414, 592);
                FocusPanel.Location = new Point(115, 103);
                FocusPanel.Dock = DockStyle.None;
                PopOutButton.Image = Properties.Resources.Expand_Arrow_24px;

                PopOut = false;

                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Tabber Focus")
                    {
                        f.MinimizeBox = true;
                        f.MaximizeBox = true;
                        f.Text = "Tabber Insights";

                        f.WindowState = FormWindowState.Maximized;
                        f.Size = new Size(1268, 645);
                        f.MinimumSize = new Size(1070, 700);
                        f.FormBorderStyle = FormBorderStyle.Sizable;
                        f.Location = new Point(100, 100);
                        f.TopMost = false;
                        f.BringToFront();
                        f.Icon = Properties.Resources.Tabber_Insights;


                        f.Refresh();
                        break;
                    }
                }
            }
        }
        #endregion

        #region Timer Settings
        private void PeriodsButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FocusPeriodMinutes = 30;
            Properties.Settings.Default.BreakPeriodMinutes = 0;
            Properties.Settings.Default.Save();

            MaxFocusSessions = 1;

            UpNextLabel.Text = "Away To Start a Focus Session";
            FocusTimeLabel.Text = "30:00";

            SettingsPanel.Hide();
        }

        private void PeriodsButton2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FocusPeriodMinutes = 27;
            Properties.Settings.Default.BreakPeriodMinutes = 5;
            Properties.Settings.Default.Save();

            MaxFocusSessions = 2;

            UpNextLabel.Text = "Away To Start a Fhocus Session";
            FocusTimeLabel.Text = "27:00";

            SettingsPanel.Hide();
        }

        private void PeriodsButton3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FocusPeriodMinutes = 27;
            Properties.Settings.Default.BreakPeriodMinutes = 5;
            Properties.Settings.Default.Save();

            MaxFocusSessions = 3;

            UpNextLabel.Text = "Away To Start a Focus Session";
            FocusTimeLabel.Text = "27:00";

            SettingsPanel.Hide();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsPanel.BringToFront();
            SettingsPanel.Location = new System.Drawing.Point(179, 406);
            SettingsPanel.Visible = true;
        }
        #endregion

        #region Side Panel
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
        private void TabberGoalsButton_Click(object sender, EventArgs e)
        {
            TabberGoalsPro Tabber_Goals_Pro = new TabberGoalsPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(Tabber_Goals_Pro);
                    Tabber_Goals_Pro.Dock = DockStyle.Fill;
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
        #endregion

        #region Focus Modes

        private void FocusModeButtons_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            FocusMode = button.Tag.ToString();

            foreach (Control c in FocusModesPanel.Controls)
            {
                if (c is RoundFluentButton)
                {
                    ((RoundFluentButton)c).BackColor = Color.White;
                }
            }

            button.BackColor = Color.FromArgb(229, 229, 229);
        }
        #endregion

        private void ClearGoalsButton_Click(object sender, EventArgs e)
        {
            FocusedGoalTitle.Visible = false;
            ClearGoalsButton.BackColor = Color.FromArgb(229, 229, 229);

            GoalButton1.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton2.Image = Properties.Resources.Complete_In_Active_24px;
            GoalButton3.Image = Properties.Resources.Complete_In_Active_24px;
        }
    }
}
