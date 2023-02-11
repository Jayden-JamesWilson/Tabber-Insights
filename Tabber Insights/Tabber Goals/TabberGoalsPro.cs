using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Tabber_Insights.Home_Page;
using Tabber_Insights.Properties;
using Tabber_Insights.Tabber_Focus;
using Tabber_Insights.Tabber_Goals.Goals;
using Tabber_Insights.Tabber_Reflect;
using Tabber_Insights.Tabber_Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace Tabber_Insights
{
    public partial class TabberGoalsPro : UserControl
    {
        #region Constructor 

        int GoalNumber = 1;
        MonthCalendar monthCalendar = new MonthCalendar();
        ListBox listBox = new ListBox();

        public TabberGoalsPro()
        {
            InitializeComponent();

            monthCalendar.MaxSelectionCount = 1;

            listBox.Items.AddRange(new object[] { "0%", "25%", "50%", "75%", "100%" });
            listBox.DrawItem += Priority_DrawItem;
            listBox.DrawMode= DrawMode.OwnerDrawFixed;
            listBox.Width = 100;
            listBox.Height = 150;

            TitleTextBox1.Text = Settings.Default.TabberGoalTitles[0];
            TitleTextBox2.Text = Settings.Default.TabberGoalTitles[1];
            TitleTextBox3.Text = Settings.Default.TabberGoalTitles[2];

            DateTextBox1.Text = Settings.Default.TabberGoalDates[0];
            DateTextBox2.Text = Settings.Default.TabberGoalDates[1];
            DateTextBox3.Text = Settings.Default.TabberGoalDates[2];

            PriorityTextBox1.Text = Settings.Default.TabberGoalPriorities[0];
            PriorityTextBox2.Text = Settings.Default.TabberGoalPriorities[1];
            PriorityTextBox3.Text = Settings.Default.TabberGoalPriorities[2];

            DescriptionTextBox1.Text = Settings.Default.TabberGoalDescriptions[0];
            DescriptionTextBox2.Text = Settings.Default.TabberGoalDescriptions[1];
            DescriptionTextBox3.Text = Settings.Default.TabberGoalDescriptions[2];

            Properties.Settings.Default.Save();
        }

        private void Priority_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;


            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(229, 229, 229)), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
            }

            if (e.Index == -1) return;

            e.Graphics.DrawString(listBox.Items[e.Index].ToString(),
                                  new Font(listBox.Items[e.Index].ToString(), 10),
                                  new SolidBrush(Color.Black),
                                  e.Bounds.X,
                                  e.Bounds.Y);

            listBox.ItemHeight = 25;
        }

        #endregion

        #region Overrides

        const int WM_PARENTNOTIFY = 0x210;
        const int WM_LBUTTONDOWN = 0x201;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || (m.Msg == WM_PARENTNOTIFY && (int)m.WParam == WM_LBUTTONDOWN))
            {
                if (!monthCalendar.ClientRectangle.Contains(monthCalendar.PointToClient(Cursor.Position)))
                {
                    monthCalendar.Hide();
                }
                if (!listBox.ClientRectangle.Contains(listBox.PointToClient(Cursor.Position)))
                {
                    listBox.Hide();
                }
            }
            base.WndProc(ref m);
        }

        private void Tabber_Goals_Pro_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        #endregion

        #region Calendar Drop Down

        private void DateDropDownButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = (System.Windows.Forms.Button)sender;

            if (button.Name == "DateDropDownButton1")
            {
                monthCalendar.Show();
                Goal1.Controls.Add(monthCalendar);
                monthCalendar.BringToFront();
                monthCalendar.Location = new System.Drawing.Point(13, 121);
                monthCalendar.DateSelected += Calendar_DateSelected;
                monthCalendar.SelectionStart = DateTime.Parse(DateTextBox1.Text);

                GoalNumber = 1;
            }

            else if (button.Name == "DateDropDownButton2")
            {
                monthCalendar.Show();
                Goal2.Controls.Add(monthCalendar);
                monthCalendar.BringToFront();
                monthCalendar.Location = new System.Drawing.Point(13, 121);
                monthCalendar.DateSelected += Calendar_DateSelected;
                monthCalendar.SelectionStart = DateTime.Parse(DateTextBox2.Text);

                GoalNumber = 2;
            }

            else if (button.Name == "DateDropDownButton3")
            {
                monthCalendar.Show();
                Goal3.Controls.Add(monthCalendar);
                monthCalendar.BringToFront();
                monthCalendar.Location = new System.Drawing.Point(13, 121);
                monthCalendar.DateSelected += Calendar_DateSelected;
                monthCalendar.SelectionStart = DateTime.Parse(DateTextBox3.Text);

                GoalNumber = 3;
            }
        }
        
        private void Calendar_DateSelected(object? sender, DateRangeEventArgs e)
        {

            if (GoalNumber == 1)
            {
                DateTextBox1.Text = monthCalendar.SelectionStart.ToLongDateString();
                monthCalendar.Visible = false;
                DateTextBox1.Focus();
            }

            else if (GoalNumber == 2)
            {

                DateTextBox2.Text = monthCalendar.SelectionStart.ToLongDateString();
                monthCalendar.Visible = false;
                DateTextBox2.Focus();
            }

            else if (GoalNumber == 3)
            {
                DateTextBox3.Text = monthCalendar.SelectionStart.ToLongDateString();
                monthCalendar.Visible = false;
                DateTextBox3.Focus();
            }
        }

        #endregion

        #region Priority Drop Down
        private void PriorityDropDownButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = (System.Windows.Forms.Button)sender;

            if (button.Name == "PriorityDropDownButton1")
            {
                listBox.Show();
                Goal1.Controls.Add(listBox);
                listBox.BringToFront();
                listBox.Location = new System.Drawing.Point(15, 169);
                listBox.SelectedIndexChanged += Priority_SelectedIndexChanged;

                GoalNumber = 1;
            }

            else if (button.Name == "PriorityDropDownButton2")
            {
                listBox.Show();
                Goal2.Controls.Add(listBox);
                listBox.BringToFront();
                listBox.Location = new System.Drawing.Point(15, 169);
                listBox.SelectedIndexChanged += Priority_SelectedIndexChanged;

                GoalNumber = 2;
            }

            else if (button.Name == "PriorityDropDownButton3")
            {
                listBox.Show();
                Goal3.Controls.Add(listBox);
                listBox.BringToFront();
                listBox.Location = new System.Drawing.Point(15, 169);
                listBox.SelectedIndexChanged += Priority_SelectedIndexChanged;

                GoalNumber = 3;
            }
        }

        private void Priority_SelectedIndexChanged(object? sender, EventArgs e)
        {

            if (GoalNumber == 1)
            {
                PriorityTextBox1.Text = listBox.SelectedItem.ToString();
                listBox.Hide();
                PriorityTextBox1.Focus();
            }

            else if (GoalNumber == 2)
            {

                PriorityTextBox2.Text = listBox.SelectedItem.ToString();
                listBox.Hide();
                PriorityTextBox2.Focus();
            }

            else if (GoalNumber == 3)
            {
                PriorityTextBox3.Text = listBox.SelectedItem.ToString();
                listBox.Hide();
                PriorityTextBox3.Focus();
            }
        }

        #endregion

        #region Save Goals
        private void SaveGoals()
        {
            Settings.Default.TabberGoalTitles[0] = TitleTextBox1.Text.ToString();
            Settings.Default.TabberGoalTitles[1] = TitleTextBox2.Text.ToString();
            Settings.Default.TabberGoalTitles[2] = TitleTextBox3.Text.ToString();

            Settings.Default.TabberGoalDates[0] = DateTextBox1.Text.ToString();
            Settings.Default.TabberGoalDates[1] = DateTextBox2.Text.ToString();
            Settings.Default.TabberGoalDates[2] = DateTextBox3.Text.ToString();

            Settings.Default.TabberGoalPriorities[0] = PriorityTextBox1.Text.ToString();
            Settings.Default.TabberGoalPriorities[1] = PriorityTextBox2.Text.ToString();
            Settings.Default.TabberGoalPriorities[2] = PriorityTextBox3.Text.ToString();

            Settings.Default.TabberGoalDescriptions[0] = DescriptionTextBox1.Text.ToString();
            Settings.Default.TabberGoalDescriptions[1] = DescriptionTextBox2.Text.ToString();
            Settings.Default.TabberGoalDescriptions[2] = DescriptionTextBox3.Text.ToString();

            Settings.Default.Save();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveGoals();
        }
        #endregion

        #region Pop Out
        private void ExpandButton_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Remove(this);
                    f.MinimizeBox = false;
                    f.MaximizeBox = false;
                    f.Text = "Tabber Goals";

                    f.WindowState = FormWindowState.Normal;
                    f.Size = new Size(315, 215);
                    f.FormBorderStyle = FormBorderStyle.FixedSingle;
                    f.TopMost = true;
                    f.Icon = Properties.Resources.Tabber_Goals;


                    Goals_Pop_Up goals_Pop_Up = new Goals_Pop_Up();
                    f.Controls.Add(goals_Pop_Up);
                    goals_Pop_Up.Dock = DockStyle.Fill;

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

            //if (Isopen == false)
            //{
            //    TabberInsights tabberInsights = new Tabber Insights();
            //    tabberInsights.Show();
            //}


            //Determine "rightmost" screen
        }
        #endregion

        #region Side Panel
        private void TabberFocusButton_Click(object sender, EventArgs e)
        {
            SaveGoals();

            TabberFocusPro Tabber_Focus_Pro = new TabberFocusPro();

            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Tabber Insights")
                {
                    f.Controls.Add(Tabber_Focus_Pro);
                    Tabber_Focus_Pro.Dock = DockStyle.Fill;
                    f.Controls.Remove(this);
                }
            }
        }

        private void TabberReflectButton_Click(object sender, EventArgs e)
        {
            SaveGoals();

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

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            SaveGoals();
        }
    }
}
