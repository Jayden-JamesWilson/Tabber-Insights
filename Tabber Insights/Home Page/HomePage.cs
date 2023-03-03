
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
using System.Xml;
using Tabber_Insights.Tabber_Focus;
using Tabber_Insights.Tabber_Reflect;
using Tabber_Insights.Tabber_Tasks;

namespace Tabber_Insights.Home_Page
{
    public partial class HomePage : UserControl
    {
        #region Constructor
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

            Score.Text = Properties.Settings.Default.ReflectionScore.ToString();

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

            #region Goals
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

        #region Overrides
        private void HomePage_Resize(object sender, EventArgs e)
        {
            this.Refresh();
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

        #region Tabber Focus 
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
        #endregion

        #region Tabber Reflect
        private void ClearFocusMinutesButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TotalFocusSeconds = 0;
            Properties.Settings.Default.Save();

            TotalFocusMinutes.Text = "0";
            ClearFocusMinutesButton.Enabled = false;
        }
        #endregion

        #region Save/Load XML Data
        private void SaveValues()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Tabber Insights Data File (*.TabberInsights)|*.TabberInsights";
            saveFileDialog.Title = "Open Tabber Insighst Data";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xmlDocument = new XmlDocument();

                #region Tabber Goals
                #region Goal Title
                XmlElement root = xmlDocument.CreateElement("Root");
                xmlDocument.AppendChild(root);

                XmlElement GoalTitleOne = xmlDocument.CreateElement("GoalTitleOne");
                GoalTitleOne.InnerText = Properties.Settings.Default.TabberGoalTitles[0].ToString();
                root.AppendChild(GoalTitleOne);

                XmlElement GoalTitleTwo = xmlDocument.CreateElement("GoalTitleTwo");
                GoalTitleTwo.InnerText = Properties.Settings.Default.TabberGoalTitles[1].ToString();
                root.AppendChild(GoalTitleTwo);

                XmlElement GoalTitleThree = xmlDocument.CreateElement("GoalTitleThree");
                GoalTitleThree.InnerText = Properties.Settings.Default.TabberGoalTitles[2].ToString();
                root.AppendChild(GoalTitleThree);
                #endregion

                #region Goal Descriptions 
                XmlElement GoalDescriptionOne = xmlDocument.CreateElement("GoalDescriptionOne");
                GoalDescriptionOne.InnerText = Properties.Settings.Default.TabberGoalDescriptions[0].ToString();
                root.AppendChild(GoalDescriptionOne);

                XmlElement GoalDescriptionTwo = xmlDocument.CreateElement("GoalDescriptionTwo");
                GoalDescriptionTwo.InnerText = Properties.Settings.Default.TabberGoalDescriptions[1].ToString();
                root.AppendChild(GoalDescriptionTwo);

                XmlElement GoalDescriptionThree = xmlDocument.CreateElement("GoalDescriptionThree");
                GoalDescriptionThree.InnerText = Properties.Settings.Default.TabberGoalDescriptions[2].ToString();
                root.AppendChild(GoalDescriptionThree);
                #endregion

                #region Goal Priorities 
                XmlElement GoalPrirotyOne = xmlDocument.CreateElement("GoalPrirotyOne");
                GoalPrirotyOne.InnerText = Properties.Settings.Default.TabberGoalPriorities[0].ToString();
                root.AppendChild(GoalPrirotyOne);

                XmlElement GoalPrirotyTwo = xmlDocument.CreateElement("GoalPrirotyTwo");
                GoalPrirotyTwo.InnerText = Properties.Settings.Default.TabberGoalPriorities[1].ToString();
                root.AppendChild(GoalPrirotyTwo);

                XmlElement GoalPrirotyThree = xmlDocument.CreateElement("GoalPrirotyThree");
                GoalPrirotyThree.InnerText = Properties.Settings.Default.TabberGoalPriorities[2].ToString();
                root.AppendChild(GoalPrirotyThree);
                #endregion

                #region Goal Priorities 
                XmlElement GoalDatesOne = xmlDocument.CreateElement("GoalDatesOne");
                GoalDatesOne.InnerText = Properties.Settings.Default.TabberGoalDates[0].ToString();
                root.AppendChild(GoalDatesOne);

                XmlElement GoalDatesTwo = xmlDocument.CreateElement("GoalDatesTwo");
                GoalDatesTwo.InnerText = Properties.Settings.Default.TabberGoalDates[1].ToString();
                root.AppendChild(GoalDatesTwo);

                XmlElement GoalDatesThree = xmlDocument.CreateElement("GoalDatesThree");
                GoalDatesThree.InnerText = Properties.Settings.Default.TabberGoalDates[2].ToString();
                root.AppendChild(GoalDatesThree);
                #endregion
                #endregion

                #region Tabber Focus
                XmlElement FocusPeriodMinutes = xmlDocument.CreateElement("FocusPeriodMinutes");
                FocusPeriodMinutes.InnerText = Properties.Settings.Default.FocusPeriodMinutes.ToString();
                root.AppendChild(FocusPeriodMinutes);

                XmlElement BreakPeriodMinutes = xmlDocument.CreateElement("BreakPeriodMinutes");
                BreakPeriodMinutes.InnerText = Properties.Settings.Default.BreakPeriodMinutes.ToString();
                root.AppendChild(BreakPeriodMinutes);

                XmlElement TotalFocusSeconds = xmlDocument.CreateElement("TotalFocusSeconds");
                TotalFocusSeconds.InnerText = Properties.Settings.Default.TotalFocusSeconds.ToString();
                root.AppendChild(TotalFocusSeconds);
                #endregion

                #region Tabber Reflect
                XmlElement LastReflectionLog = xmlDocument.CreateElement("LastReflectionLog");
                LastReflectionLog.InnerText = Properties.Settings.Default.LastReflectionLog.ToString();
                root.AppendChild(LastReflectionLog);

                XmlElement ReflectionScore = xmlDocument.CreateElement("ReflectionScore");
                ReflectionScore.InnerText = Properties.Settings.Default.ReflectionScore.ToString();
                root.AppendChild(ReflectionScore);

                XmlElement ReflectionLogs = xmlDocument.CreateElement("ReflectionLogs");
                ReflectionLogs.InnerText = Properties.Settings.Default.ReflectionLogs.ToString();
                root.AppendChild(ReflectionLogs);
                #endregion

                xmlDocument.Save(saveFileDialog.FileName);
            }

        }
        private void LoadValues()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tabber Insights Files (*.TabberInsights)|*.TabberInsights";
            openFileDialog.Title = "Open Tabber Insights Data";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    xmlDocument.Load(openFileDialog.FileName);

                    #region Tabber Goals

                    #region Goal Titles
                    XmlNode TabberGoalOne = xmlDocument.SelectSingleNode("/Root/GoalTitleOne");
                    if (TabberGoalOne != null)
                    {
                        Properties.Settings.Default.TabberGoalTitles[0] = TabberGoalOne.InnerText;
                    }

                    XmlNode TabberGoalTwo = xmlDocument.SelectSingleNode("/Root/GoalTitleTwo");
                    if (TabberGoalTwo != null)
                    {
                        Properties.Settings.Default.TabberGoalTitles[1] = TabberGoalTwo.InnerText;
                    }

                    XmlNode TabberGoalThree = xmlDocument.SelectSingleNode("/Root/GoalTitleThree");
                    if (TabberGoalThree != null)
                    {
                        Properties.Settings.Default.TabberGoalTitles[2] = TabberGoalThree.InnerText;
                    }
                    #endregion

                    #region Goal Descriptions
                    XmlNode GoalDescriptionOne = xmlDocument.SelectSingleNode("/Root/GoalDescriptionOne");
                    if (GoalDescriptionOne != null)
                    {
                        Properties.Settings.Default.TabberGoalDescriptions[0] = GoalDescriptionOne.InnerText;
                    }

                    XmlNode GoalDescriptionTwo = xmlDocument.SelectSingleNode("/Root/GoalDescriptionTwo");
                    if (GoalDescriptionTwo != null)
                    {
                        Properties.Settings.Default.TabberGoalDescriptions[1] = GoalDescriptionTwo.InnerText;
                    }

                    XmlNode GoalDescriptionThree = xmlDocument.SelectSingleNode("/Root/GoalDescriptionThree");
                    if (GoalDescriptionThree != null)
                    {
                        Properties.Settings.Default.TabberGoalDescriptions[2] = GoalDescriptionThree.InnerText;
                    }
                    #endregion

                    #region Goal Priorities
                    XmlNode GoalPrioritiesOne = xmlDocument.SelectSingleNode("/Root/GoalPrioritiesOne");
                    if (GoalPrioritiesOne != null)
                    {
                        Properties.Settings.Default.TabberGoalPriorities[0] = GoalPrioritiesOne.InnerText;
                    }

                    XmlNode GoalPrioritiesTwo = xmlDocument.SelectSingleNode("/Root/GoalPrioritiesTwo");
                    if (GoalPrioritiesTwo != null)
                    {
                        Properties.Settings.Default.TabberGoalPriorities[1] = GoalPrioritiesTwo.InnerText;
                    }

                    XmlNode GoalPrioritiesThree = xmlDocument.SelectSingleNode("/Root/GoalPrioritiesThree");
                    if (GoalPrioritiesThree != null)
                    {
                        Properties.Settings.Default.TabberGoalPriorities[2] = GoalPrioritiesThree.InnerText;
                    }
                    #endregion

                    #region Goal Dates
                    XmlNode GoalDatesOne = xmlDocument.SelectSingleNode("/Root/GoalDatesOne");
                    if (GoalDatesOne != null)
                    {
                        Properties.Settings.Default.TabberGoalDates[0] = GoalDatesOne.InnerText;
                    }

                    XmlNode GoalDatesTwo = xmlDocument.SelectSingleNode("/Root/GoalDatesTwo");
                    if (GoalDatesTwo != null)
                    {
                        Properties.Settings.Default.TabberGoalDates[1] = GoalDatesTwo.InnerText;
                    }

                    XmlNode GoalDatesThree = xmlDocument.SelectSingleNode("/Root/GoalDatesThree");
                    if (GoalDatesThree != null)
                    {
                        Properties.Settings.Default.TabberGoalDates[2] = GoalDatesThree.InnerText;
                    }
                    #endregion

                    #endregion

                    #region Tabber Focus 
                    XmlNode FocusPeriodMinutes = xmlDocument.SelectSingleNode("/Root/FocusPeriodMinutes");
                    if (FocusPeriodMinutes != null)
                    {
                        Properties.Settings.Default.FocusPeriodMinutes = Convert.ToInt32(FocusPeriodMinutes.InnerText);
                    }

                    XmlNode BreakPeriodMinutes = xmlDocument.SelectSingleNode("/Root/BreakPeriodMinutes");
                    if (BreakPeriodMinutes != null)
                    {
                        Properties.Settings.Default.BreakPeriodMinutes = Convert.ToInt32(BreakPeriodMinutes.InnerText);
                    }

                    XmlNode TotalFocusSeconds = xmlDocument.SelectSingleNode("/Root/TotalFocusSeconds");
                    if (BreakPeriodMinutes != null)
                    {
                        Properties.Settings.Default.TotalFocusSeconds = Convert.ToInt32(TotalFocusSeconds.InnerText);
                    }
                    #endregion

                    #region Tabber Reflect
                    XmlNode LastReflectionLog = xmlDocument.SelectSingleNode("/Root/LastReflectionLog");
                    if (LastReflectionLog != null)
                    {
                        Properties.Settings.Default.LastReflectionLog = LastReflectionLog.InnerText;
                    }
                    XmlNode ReflectionScore = xmlDocument.SelectSingleNode("/Root/ReflectionScore");
                    if (ReflectionScore != null)
                    {
                        Properties.Settings.Default.ReflectionScore = Convert.ToInt32(ReflectionScore.InnerText);
                    }
                    XmlNode ReflectionLogs = xmlDocument.SelectSingleNode("/Root/ReflectionLogs");
                    if (ReflectionLogs != null)
                    {
                        Properties.Settings.Default.ReflectionLogs = ReflectionLogs.InnerText;
                    }
                    #endregion
                }
            }
        }

        private void OpenDataButton_Click(object sender, EventArgs e)
        {
            LoadValues();
        }

        #endregion

        private void OpenButton_Click(object sender, EventArgs e)
        {
            LoadValues();

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

            #region Goals
            GoalTitle1.Text = Properties.Settings.Default.TabberGoalTitles[0];
            GoalTitle2.Text = Properties.Settings.Default.TabberGoalTitles[1];
            GoalTitle3.Text = Properties.Settings.Default.TabberGoalTitles[2];

            if (GoalTitle1.Text == "")
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
            #endregion

            #region Focus Lifetime Minutes
            int TotalMinutes = Properties.Settings.Default.TotalFocusSeconds / 60;
            this.TotalFocusMinutes.Text = $"{TotalMinutes} Minutes";

            if (TotalMinutes > 0)
            {
                ClearFocusMinutesButton.Enabled = true;
            }
            else
            {
                ClearFocusMinutesButton.Enabled = false;
            }
            #endregion
        }
    }
}
