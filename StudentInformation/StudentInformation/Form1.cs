using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace StudentInformation
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            int id = 1;
            txtStudent_Id.Text = id.ToString();
            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");

            con1.Open();
            string str1 = @"select * from student";
            SqlCommand cmd1 = new SqlCommand(str1, con1);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                id = Convert.ToInt32(dr1[0]);
                id += 1;
                txtStudent_Id.Text = id.ToString();

            }
            Show();
        }

        public void Show()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM student", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }


        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtContact.Text, "[^0-9]"))
            {
                MessageBox.Show("Please Enter Only Number for Contact.");
                txtContact.Text = txtContact.Text.Remove(txtContact.Text.Length - 1);

            }
            else if (txtContact.Text.Length > 11)
            {
                MessageBox.Show("Your Contact Number is greater than 11.");
                txtContact.Text = txtContact.Text.Remove(txtContact.Text.Length - 1);

                txtContact.Focus();

            }

        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                dataGridView1.CurrentRow.Selected = true;
                txtStudent_Id.Text = dataGridView1.Rows[e.RowIndex].Cells["student_Id"].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells["address"].Value.ToString();
                txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["email"].Value.ToString();
                txtContact.Text = dataGridView1.Rows[e.RowIndex].Cells["contact"].Value.ToString();
                dateOfBirth.Text = dataGridView1.Rows[e.RowIndex].Cells["dob"].Value.ToString();



                if (dataGridView1.Rows[e.RowIndex].Cells["gender"].Value.ToString() == "Male")
                {
                    radioMale.Checked = true;
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells["gender"].Value.ToString() == "Female")
                {
                    radioFemale.Checked = true;
                }
            }


        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Regex rexEmail = new Regex(@"[a-zA-Z]+@[a-zA-Z]+.com$");
            Regex rexPhone = new Regex(@"^(09)[0-9]{9}$");

            dateOfBirth.Format = DateTimePickerFormat.Custom;
            dateOfBirth.CustomFormat = "MM-dd-yyyy";

            string gender = "";

            if (!(IsValid(rexEmail, txtEmail.Text)))
            {
                MessageBox.Show("The Email You Enter is Invalid");
            }

            if (!(IsValid(rexPhone, txtContact.Text)))
            {
                MessageBox.Show("Phone Number You Enter is Invalid");
            }

            if (radioMale.Checked)
            {
                gender = "Male";
            }
            else if (radioFemale.Checked)
            {
                gender = "Female";
            }


            if (txtName.Text != "" && gender != "" && txtAddress.Text != "" && dateOfBirth.Text != "")
            {


                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");
                con.Open();
                try
                {
                    string str = @"Insert into student (name, email, address, gender, dob, contact) values ('" + txtName.Text + "', '" + txtEmail.Text + "', '" + txtAddress.Text + "', '" + gender + "', '" + dateOfBirth.Text + "', '" + txtContact.Text + "' ) ";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insert Sucessfully");
                    Show();


                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    con.Close();

                }

            }
            else
            {
                MessageBox.Show("Please check your data");
            }



        }

        public Boolean IsValid(Regex rex, String data)
        {

            return rex.IsMatch(data);

        }

        public void clear()
        {
            int id = 1;
            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");

            con1.Open();
            string str1 = @"select * from student";
            SqlCommand cmd1 = new SqlCommand(str1, con1);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                id = Convert.ToInt32(dr1[0]);
                id += 1;
                txtStudent_Id.Text = id.ToString();
            }
            txtName.Text = "";
            txtEmail.Text = "";
            radioMale.Checked = false;
            radioFemale.Checked = false;
            txtContact.Text = "";
            txtAddress.Text = "";
            dateOfBirth.Text = "";
            con1.Close();

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Regex rexEmail = new Regex(@"[a-zA-Z]+@[a-zA-Z]+.com$");
            Regex rexPhone = new Regex(@"^(09)[0-9]{9}$");

            dateOfBirth.Format = DateTimePickerFormat.Custom;
            dateOfBirth.CustomFormat = "MM-dd-yyyy";

            string gender = "";
            Boolean isEmailValid = IsValid(rexEmail, txtEmail.Text);
            if (!(isEmailValid))
            {
                MessageBox.Show("The Email You Enter is Invalid");
            }

            Boolean isPhoneValid = IsValid(rexPhone, txtContact.Text);
            if (!(isPhoneValid))
            {
                MessageBox.Show("Phone Number You Enter is Invalid");
            }



            if (radioMale.Checked)
            {
                gender = "Male";
            }
            else if (radioFemale.Checked)
            {
                gender = "Female";
            }


            if (isEmailValid && isPhoneValid  && txtName.Text != "" && gender != "" && txtAddress.Text != "" && dateOfBirth.Text != "")
            {

                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");
                con.Open();
                try
                {

                    String str = @"Update student set name ='" + txtName.Text + "', email='" + txtEmail.Text + "', address = '" + txtAddress.Text + "',gender = '" + gender + "',dob = '" + dateOfBirth.Text + "',contact = '" + txtContact.Text + "'where student_id = '" + txtStudent_Id.Text + "';";

                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update Sucessfully");
                  


                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    con.Close();
                    Show();
                    clear();

                }

            }
            else
            {
                MessageBox.Show("Please check your data");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\OneDrive - University of Computer Studies (Maubin)\Documents\Student.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();
            try
            {

                String str = @"Delete student where student_id = '" + txtStudent_Id.Text + "';";

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Delete Sucessfully");
                
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                con.Close();
                Show();
                clear();
            }


        }

        private void btnExit_Click(object sender, EventArgs e)
        {
           if(MessageBox.Show("Are you sure? Do you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes ) {
             Application.Exit();
           }
        }




    }
}
