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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudentManagementApp
{
    public partial class StudentsForm : Form
    {
        private String connectionString;
        public StudentsForm()
        {
            InitializeComponent();
            connectionString =
ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblStudents (StudentName,DateOfBirth, City, Age, YearOfEnroll, Major, GPA) VALUES(@StudentName, @DateOfBirth,@City, @Age, @YearOfEnroll, @Major, @GPA)", connection);
                cmd.Parameters.AddWithValue("@StudentName", "New Student");
                cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Now);
                cmd.Parameters.AddWithValue("@City", "City");
                cmd.Parameters.AddWithValue("@Age", 20);
                cmd.Parameters.AddWithValue("@YearOfEnroll", 2021);
                cmd.Parameters.AddWithValue("@Major", "Major");
                cmd.Parameters.AddWithValue("@GPA", 4.0);
                cmd.ExecuteNonQuery();
                LoadData();
            }
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tblStudents", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblStudents WHERE StudentID = @StudentID", connection);
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.ExecuteNonQuery();
                    LoadData();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);

                // Retrieve values from the selected row
                string studentName = dataGridView1.SelectedRows[0].Cells["StudentName"].Value.ToString();
                DateTime dateOfBirth = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["DateOfBirth"].Value);
                string city = dataGridView1.SelectedRows[0].Cells["City"].Value.ToString();
                int age = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Age"].Value);
                int yearOfEnroll = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["YearOfEnroll"].Value);
                string major = dataGridView1.SelectedRows[0].Cells["Major"].Value.ToString();
                double gpa = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells["GPA"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE tblStudents SET StudentName = @StudentName, DateOfBirth = @DateOfBirth, City = @City, Age = @Age, YearOfEnroll = @YearOfEnroll, Major = @Major, GPA = @GPA WHERE StudentID = @StudentID", connection);

                    cmd.Parameters.AddWithValue("@StudentID", studentID); // Still used in WHERE clause
                    cmd.Parameters.AddWithValue("@StudentName", studentName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@YearOfEnroll", yearOfEnroll);
                    cmd.Parameters.AddWithValue("@Major", major);
                    cmd.Parameters.AddWithValue("@GPA", gpa);

                    cmd.ExecuteNonQuery();
                    LoadData(); // Refresh the DataGridView
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }
    }
}
