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
    public partial class CoursesForm : Form
    {
        private String connectionString;

        public CoursesForm()
        {
            InitializeComponent();
            connectionString =
ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        }

        private void CoursesForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tblCourses", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblCourses (CourseName, Credits, DepartmentID, InstructorID) VALUES(@CourseName, @Credits, @DepartmentID, @InstructorID)", connection);
                cmd.Parameters.AddWithValue("@CourseName", "New Course");
                cmd.Parameters.AddWithValue("@Credits", 3);
                cmd.Parameters.AddWithValue("@DepartmentID", 1);
                cmd.Parameters.AddWithValue("@InstructorID", 1);
                cmd.ExecuteNonQuery();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int courseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CourseID"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblCourses WHERE CourseID = @CourseID", connection);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int courseID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CourseID"].Value);

                // Retrieve values from the selected row
                string courseName = dataGridView1.SelectedRows[0].Cells["CourseName"].Value.ToString();
                int credits = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Credits"].Value);
                int departmentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DepartmentID"].Value);
                int instructorID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["InstructorID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE tblCourses SET CourseName = @CourseName, Credits = @Credits, DepartmentID = @DepartmentID, InstructorID = @InstructorID WHERE CourseID = @CourseID", connection);

                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.Parameters.AddWithValue("@CourseName", courseName);
                    cmd.Parameters.AddWithValue("@Credits", credits);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.Parameters.AddWithValue("@InstructorID", instructorID);

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
