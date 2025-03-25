using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementApp
{
    public partial class DepartmentsForm : Form
    {
        private String connectionString;
        public DepartmentsForm()
        {
            InitializeComponent();
            connectionString =
ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;

        }

        private void DepartmentsForm_Load(object sender, EventArgs e)
        {

            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tblDepartments", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblDepartments (DepartmentName) VALUES(@DepartmentName)", connection);
                cmd.Parameters.AddWithValue("@DepartmentName", "New Department");
                cmd.ExecuteNonQuery();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int departmentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DepartmentID"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblDepartments WHERE DepartmentID = @DepartmentID", connection);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int departmentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DepartmentID"].Value);
                string departmentName = dataGridView1.SelectedRows[0].Cells["DepartmentName"].Value.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE tblDepartments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID", connection);

                    // Use Add() and specify data types
                    cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID;
                    cmd.Parameters.Add("@DepartmentName", SqlDbType.NVarChar, 255).Value = departmentName; //Adjust 255 to your column size

                    cmd.ExecuteNonQuery();
                    LoadData(); // Refresh the DataGridView
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
