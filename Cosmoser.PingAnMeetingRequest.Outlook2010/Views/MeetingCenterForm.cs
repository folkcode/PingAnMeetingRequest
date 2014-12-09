using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public partial class MeetingCenterForm : Form
    {
        public MeetingData MeetingData
        {
            get;
            set;
        }

        private string currentMeetingId;

        public MeetingCenterForm()
        {
            InitializeComponent();
        }

        private void MeetingCenterForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.SetDataSource();
        }

        private void SetDataSource()
        {
            var list = this.MeetingData.Values.ToList();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = list;
            //this.dataGridView1.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Id"].Value = list[i].Id;
                dataGridView1.Rows[i].Cells["checkbox"].Value = SelectedStatus.NoSelected;
                dataGridView1.Rows[i].Cells["MeetingName"].Value = list[i].Name;
                dataGridView1.Rows[i].Cells["StartTime"].Value = list[i].StartTime;
                dataGridView1.Rows[i].Cells["EndTime"].Value = list[i].EndTime;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                if (column is DataGridViewCheckBoxColumn)
                {
                    DataGridViewDisableCheckBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewDisableCheckBoxCell;
                    if (!cell.Enabled)
                    {
                        return;
                    }
                    if ((SelectedStatus)cell.Value == SelectedStatus.NoSelected)
                    {
                        cell.Value = SelectedStatus.Selected;
                        SetRadioButtonValue(cell);//if radiobutton, uncomment this code line.
                        currentMeetingId = this.dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    }
                    else
                    {
                        cell.Value = SelectedStatus.NoSelected;
                    }
                }
            }
        }

        private void SetRadioButtonValue(DataGridViewDisableCheckBoxCell cell)
        {
            SelectedStatus status = (SelectedStatus)cell.Value;
            if (status == SelectedStatus.Selected)
            {
                status = SelectedStatus.NoSelected;
            }
            else
            {
                status = SelectedStatus.Selected;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewDisableCheckBoxCell cel = dataGridView1.Rows[i].Cells["checkbox"] as DataGridViewDisableCheckBoxCell;
                if (!cel.Equals(cell))
                {
                    cel.Value = status;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Display();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Delete();
                this.SetDataSource();
            }
        }

        private void MeetingCenterForm_Activated(object sender, EventArgs e)
        {
            this.SetDataSource();
        }
    }
}
