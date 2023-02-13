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

            LoadValues();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.Refresh();
            Properties.Settings.Default.Upgrade();
        }

        private void SaveValues()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog.Title = "Save XML Document";

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
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.Title = "Open User Settings";

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


        private void TabberInsights_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveValues();
        }
    }
}