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
    public partial class frmEmailType : Form
    {
        EmailTypeList EmailTypes;
        public frmEmailType()
        {
            EmailTypes = new EmailTypeList();
            EmailTypes.evtIsSavable += new Event.IsSavableHandler(EmailTypes_evtIsSavable);
            InitializeComponent();
            this.dgvEmailType.DataSource = EmailTypes.List;
            this.dgvEmailType.KeyDown += new KeyEventHandler(dgvEmailType_KeyDown);
        }

        void dgvEmailType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (dgvEmailType.SelectedRows != null)
                {
                    foreach (DataGridViewRow dgvr in dgvEmailType.SelectedRows)
                    {
                        EmailType et = (EmailType)dgvr.DataBoundItem;
                        et.Deleted = true;
                        et.Save();
                        EmailTypes.List.Remove(et);
                    }
                }
            } 
        }

        void EmailTypes_evtIsSavable(bool savable)
        {
            this.btnSave.Enabled = savable;
        }
        private void btnGet_Click(object sender, EventArgs e)
        {
            EmailTypes = EmailTypes.GetAll();
            this.dgvEmailType.DataSource = EmailTypes.List;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EmailTypes = EmailTypes.Save();
            this.dgvEmailType.Refresh();
            this.btnSave.Enabled = false;
        }
    }
}
