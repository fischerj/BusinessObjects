using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessObjectHelper;

namespace StudentInformationApplication
{
    public partial class frmTest : Form
    {
        StudentList students;
        public frmTest()
        {
            students = new StudentList();
            students.evtIsSavable += new Event.IsSavableHandler(students_evtIsSavable);
            InitializeComponent();
            this.dgvStudent.DataSource = students.List;
            this.dgvStudent.KeyDown += new KeyEventHandler(dgvStudent_KeyDown);
        }


        void dgvStudent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (dgvStudent.SelectedRows != null)
                {
                    foreach (DataGridViewRow dgvr in dgvStudent.SelectedRows)
                    {
                        Student s = (Student)dgvr.DataBoundItem;
                        s.Deleted = true;
                        s.Save();
                        students.List.Remove(s);
                    }
                }
            }

        }


        void students_evtIsSavable(bool savable)
        {
            this.btnSave.Enabled = savable;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            students = students.GetAll();
            this.dgvStudent.DataSource = students.List;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            students=students.Save();
            this.dgvStudent.Refresh();
            this.btnSave.Enabled = false;
        }

    }
}
