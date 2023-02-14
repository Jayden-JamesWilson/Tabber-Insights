using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tabber_Insights.Home_Page;
using Tabber_Insights.Tabber_Focus;
using Tabber_Insights.Tabber_Reflect;

namespace Tabber_Insights.Tabber_Tasks
{
    public partial class TabberListsPro : UserControl
    {
        private DataTable dataTable;

        #region Constructor
        public TabberListsPro()
        {
            InitializeComponent();
            dataTable = new DataTable();

            DataGridViewCheckBoxColumn Complete = new DataGridViewCheckBoxColumn();
            Complete.HeaderText = "Complete";
            Complete.Width = 20;
            List.Columns.Add(Complete);

            List.Columns.Add("Task", "Task");
            List.Columns.Add("Description", "Description");

            DataGridViewComboBoxColumn Priority = new DataGridViewComboBoxColumn();
            Priority.HeaderText = "Priority";
            Priority.Items.Add("Low");
            Priority.Items.Add("Medium");
            Priority.Items.Add("High");
            Priority.Items.Add("Critical");
            List.Columns.Add(Priority);
            

            Thread.Sleep(100);
            DataGridViewComboBoxColumn Status = new DataGridViewComboBoxColumn();
            Status.HeaderText = "Status";
            Status.Items.Add("Not Started");
            Status.Items.Add("In Progress");
            Status.Items.Add("Completed");
            Status.Items.Add("Behind");
            List.Columns.Add(Status);

            List.Columns.Add("Due Date", "Due Date");
        }
        #endregion

        #region Overrides
        private void TabberListsPro_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
        #endregion

        #region Table Controls
        private void ClearButton_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to clear the table. You cannot undo this action";
            string title = "Clear Table";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                List.Rows.Clear();
                SaveDataGridViewData();
            }
            else { }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Tabber List";
            saveFileDialog.DefaultExt = "List";
            saveFileDialog.Filter = "Tabber List Files (*.List)|*.List";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog.FileName, FileMode.Create)))
                {
                    bw.Write(List.Columns.Count);
                    bw.Write(List.Rows.Count);
                    foreach (DataGridViewRow dgvR in List.Rows)
                    {
                        for (int j = 0; j < List.Columns.Count; ++j)
                        {
                            object val = dgvR.Cells[j].Value;
                            if (val == null)
                            {
                                bw.Write(false);
                                bw.Write(false);
                            }
                            else
                            {
                                bw.Write(true);
                                bw.Write(val.ToString());
                            }
                        }
                    }
                }

                ListNameLabel.Text = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + " ";
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            List.AllowUserToAddRows = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Tabber List";
            openFileDialog.DefaultExt = "List";
            openFileDialog.Filter = "Tabber List Files (*.List)|*.List";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List.Rows.Clear();

                using (BinaryReader bw = new BinaryReader(File.Open(openFileDialog.FileName, FileMode.Open)))
                {
                    int n = bw.ReadInt32();
                    int m = bw.ReadInt32();
                    for (int i = 0; i < m; ++i)
                    {
                        List.Rows.Add();
                        for (int j = 0; j < n; ++j)
                        {
                            try
                            {
                                if (bw.ReadBoolean())
                                {
                                    try
                                    {
                                        List.Rows[i].Cells[j].Value = bw.ReadString();
                                    }
                                    catch
                                    {

                                    }
                                }
                                else bw.ReadBoolean();
                            }
                            catch
                            {

                            }
                        }
                    }
                }

                ListNameLabel.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) + " ";

                if (List.Rows.Count > 0)
                {
                    DataGridViewRow lastRow = List.Rows[List.Rows.Count - 1];
                    bool isLastRowEmpty = true;
                    for (int i = 0; i < lastRow.Cells.Count; i++)
                    {
                        if (lastRow.Cells[i].Value != null)
                        {
                            isLastRowEmpty = false;
                            break;
                        }
                    }

                    // Remove the last row if it is empty
                    if (isLastRowEmpty)
                    {
                        List.Rows.RemoveAt(List.Rows.Count - 1);
                    }
                }

                List.AllowUserToAddRows = true;
            }
        }

        #endregion

        #region Side Panel
        private void TabberGoals_Click(object sender, EventArgs e)
        {
            SaveDataGridViewData();

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

        private void TabberFocus_Click(object sender, EventArgs e)
        {
            SaveDataGridViewData();

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

        private void TabberReflect_Click(object sender, EventArgs e)
        {
            SaveDataGridViewData();

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
        private void HomeButton_Click(object sender, EventArgs e)
        {
            SaveDataGridViewData();

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


        private void SaveDataGridViewData()
        {
            string fileName = "TabberListData.List";
            string filePath = Path.Combine(Application.StartupPath, fileName);

            if (File.Exists(filePath))
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    bw.Write(List.Columns.Count);
                    bw.Write(List.Rows.Count);
                    foreach (DataGridViewRow dgvR in List.Rows)
                    {
                        for (int j = 0; j < List.Columns.Count; ++j)
                        {
                            object val = dgvR.Cells[j].Value;
                            if (val == null)
                            {
                                bw.Write(false);
                                bw.Write(false);
                            }
                            else
                            {
                                bw.Write(true);
                                bw.Write(val.ToString());
                            }
                        }
                    }
                }

                ListNameLabel.Text = System.IO.Path.GetFileNameWithoutExtension(filePath) + " ";
            }
            else
            {
                File.Create(filePath).Close();
            }
        }



        private void TabberListsPro_Load(object sender, EventArgs e)
        {
            List.AllowUserToAddRows = false;
            
            string fileName = "TabberListData.List";
            string filePath = Path.Combine(Application.StartupPath, fileName);

            if (File.Exists(filePath))
            {
                if (new FileInfo(filePath).Length == 0)
                {
                    Console.WriteLine("The file is empty.");
                }
                else 
                {
                    using (BinaryReader bw = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {
                        int n = bw.ReadInt32();
                        int m = bw.ReadInt32();
                        for (int i = 0; i < m; ++i)
                        {
                            List.Rows.Add();
                            for (int j = 0; j < n; ++j)
                            {
                                try
                                {
                                    if (bw.ReadBoolean())
                                    {
                                        try
                                        {
                                            List.Rows[i].Cells[j].Value = bw.ReadString();
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    else bw.ReadBoolean();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }

                // Commit the new row
                List.EndEdit();
            }
            else
            {
                File.Create(filePath).Close();
                SaveDataGridViewData();
                
            }
            if (List.Rows.Count > 0)
            {
                DataGridViewRow lastRow = List.Rows[List.Rows.Count - 1];
                bool isLastRowEmpty = true;
                for (int i = 0; i < lastRow.Cells.Count; i++)
                {
                    if (lastRow.Cells[i].Value != null)
                    {
                        isLastRowEmpty = false;
                        break;
                    }
                }

                // Remove the last row if it is empty
                if (isLastRowEmpty)
                {
                    List.Rows.RemoveAt(List.Rows.Count - 1);
                }
            }

            List.AllowUserToAddRows = true;
        }
    }
}
