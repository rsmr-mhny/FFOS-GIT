﻿using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Collections;


namespace FFOSproj
{
    public partial class Cashier_Formmmm : Form
    {
        private SqlConnection connection = new SqlConnection("Data Source=LAPTOP-IJH96CPD;Initial Catalog=purchased_items;Integrated Security=True");
        private DataTable dataTable = new DataTable();
        private string connectionString = ("Data Source=LAPTOP-IJH96CPD;Initial Catalog=purchased_items;Integrated Security=True");
        private SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        private BindingSource bindingSource = new BindingSource();
        private DataSet dataSet = new DataSet();
        private DataTable table = new DataTable();
        int ID = 0;
        private Timer timer;


        public Cashier_Formmmm()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000;  
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Cashier_Formmm_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            dateToday.Text = DateTime.Now.ToString();
        }

        private void LoadDataIntoDataGridView()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM purchased_items";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView4.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }
        private void LoadDataPizza()
        {

            string connectionString = "Data Source=LAPTOP-IJH96CPD;Initial Catalog=pizza_table;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Name, Size, Price FROM pizza_table";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

        }

        private void LoadDataBeverages()
        {
            string connectionString = "Data Source=LAPTOP-IJH96CPD;Initial Catalog=beverage_table;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Name, Size, Price FROM beverage_table";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView2.DataSource = table;
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            LoadDataPizza();
        }

        private void refresh2_btn_Click(object sender, EventArgs e)
        {
            LoadDataBeverages();
        }


        private void saveToDb_Click(object sender, EventArgs e)
        {

        }

        private void dateToday_Click(object sender, EventArgs e)
        {
            dateToday.Text = DateTime.Now.ToString();
        }



        private void btn1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string itemName = row.Cells["Name"].Value.ToString();
                string itemSize = row.Cells["Size"].Value.ToString();
                string itemPrice = row.Cells["Price"].Value.ToString();

                nameTXT.Text = itemName;
                sizeTXT.Text = itemSize;
                priceTXT.Text = itemPrice;
            }


        }

        private void btn2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                string itemName = row.Cells["Name"].Value.ToString();
                string itemSize = row.Cells["Size"].Value.ToString();
                string itemPrice = row.Cells["Price"].Value.ToString();

                nameTXT.Text = itemName;
                sizeTXT.Text = itemSize;
                priceTXT.Text = itemPrice;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addBTN_Click(object sender, EventArgs e)
        {
            try
            {
                string MyConnection2 = "Data Source=LAPTOP-IJH96CPD;Initial Catalog=purchased_items;Integrated Security=True";
                string Query = "insert into purchased_items(Name, Size, Price) values(@Name, @Size, @Price)";
                using (SqlConnection MyConn2 = new SqlConnection(MyConnection2))
                {
                    using (SqlCommand MyCommand2 = new SqlCommand(Query, MyConn2))
                    {
                        MyCommand2.Parameters.AddWithValue("@Name", this.nameTXT.Text);
                        MyCommand2.Parameters.AddWithValue("@Size", this.sizeTXT.Text);
                        MyCommand2.Parameters.AddWithValue("@Price", this.priceTXT.Text);

                        MyConn2.Open();
                        MyCommand2.ExecuteNonQuery();
                        MessageBox.Show("Successfully Saved!");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void displayBTN_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void clearBTN_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow item in this.dataGridView4.SelectedRows)
            {
                using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-IJH96CPD;Initial Catalog=purchased_items;Integrated Security=True"))
                {
                    SqlCommand cmd = con.CreateCommand();
                    int id = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells[0].Value);
                    cmd.CommandText = "DELETE FROM purchased_items WHERE id='" + id + "'";

                    dataGridView4.Rows.RemoveAt(this.dataGridView4.SelectedRows[0].Index);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }






        private void totalllll_Click(object sender, EventArgs e)
        {
            decimal Total = 0;
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                Total += Convert.ToDecimal(dataGridView4.Rows[i].Cells["Price"].Value);
            }
            totalLabel.Text = Total.ToString();
        }


        string tableName = "total_sum_saved";
        private void saveCurrent_Click(object sender, EventArgs e)
        {
            try
            {
                string textBoxValue = totalLabel.Text;
                DateTime currentTime = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string insertQuery = $"INSERT INTO {tableName} (TotalSum, DateTime) VALUES (@sum, @time)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@sum", textBoxValue);
                    command.Parameters.AddWithValue("@time", currentTime);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Data saved to the database successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving data to the database: " + ex.Message);
            }
        }

        private void SaveDataToDatabase(string labelValue, string dateTimeString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string MyConnection2 = "datasource=localhost;port=3306;username=root;password=RteCh_0C#@11";
                    string Query = "insert into pizza_db.total_sum_saved(TotalSum) values('" + this.totalLabel.Text + "'); ";
                    MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                    MySqlDataReader MyReader2;
                    MyConn2.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    MessageBox.Show("Successfully Saved!");
                    while (MyReader2.Read())
                    {
                    }
                    MyConn2.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchItem(searchTerm);
        }

        private void SearchItem(string searchTerm)
        {
            dataGridView1.ClearSelection();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Contains(searchTerm))
                    {
                        row.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        return;
                    }
                }
            }

            MessageBox.Show("Item not found.");
        }

        private void btnSearchBev_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchBev.Text.Trim();
            SearchItemBev(searchTerm);
        }

        private void SearchItemBev(string searchTerm)
        {
            dataGridView2.ClearSelection();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Contains(searchTerm))
                    {
                        row.Selected = true;
                        dataGridView2.FirstDisplayedScrollingRowIndex = row.Index;
                        return;
                    }
                }
            }

            MessageBox.Show("Item not found.");
        }

       
    }
    }




