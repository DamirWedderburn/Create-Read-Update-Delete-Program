using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Function allows for a search
            if (e.KeyChar == (char)13)
            {
                //Search is Null or Empty
                if (string.IsNullOrEmpty(txtSearch.Text))
                {

                    this.employeesTableAdapter.Fill(this.appData.Employees);
                    employeesBindingSource.DataSource = this.appData.Employees;
                    //dataGridView.DataSource = employeesBindingSource;
                }
                else
                {
                    var query = from o in this.appData.Employees
                                where o.FullName.Contains(txtSearch.Text) || o.PhoneNumber == txtSearch.Text || o.Email == txtSearch.Text || o.Address.Contains(txtSearch.Text)
                                select o;
                    employeesBindingSource.DataSource = query.ToList();
                    //dataGridView.DataSource = query.ToList();
                }
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                //Function allows for deletion of records
                if (MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    employeesBindingSource.RemoveCurrent();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {   //Function allows for the browsing of JPG files
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG |*.jpg", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        pictureBox.Image = Image.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                //New typed up record that will be added to Employees from appData database. Focus is set on FullName.
                panel.Enabled = true;
                txtFullName.Focus();
                this.appData.Employees.AddEmployeesRow(this.appData.Employees.NewEmployeesRow());
                employeesBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                employeesBindingSource.ResetBindings(false);

            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Allows edit proccess. Puts focus on FullName field.
            panel.Enabled = true;
            txtFullName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Allows cancel process.
            panel.Enabled = false;
            employeesBindingSource.ResetBindings(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Allows save process. Saves and updates to Employees in appData database.
                employeesBindingSource.EndEdit();
                employeesTableAdapter.Update(this.appData.Employees);
                panel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                employeesBindingSource.ResetBindings(false);


            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.Employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.appData.Employees);
            employeesBindingSource.DataSource = this.appData.Employees;

        }
    }
}
