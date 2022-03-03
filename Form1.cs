using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FridgeDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        

        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionString["FridgeDB.Properties.Settings.FoodsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateFoodsTable();
        }

        private void PopulateFoodsTable()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM FoodType", connection))
            {
                DataTable.foodTable = new DataTable();
                adapter.Fill(foodTable);

                listFoods.DisplayMember = "FoodTypeName";
                listFoods.ValueMember = "Id";
                listFoods.DataSource = foodTable;
            }
        }

        private void listFoods_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFoodNames();
        }

        private void PopulateFoodNames()
        {
            string query = "SELECT Food.Name, FoodType.TypeName FROM FoodType INNER JOIN Food ON Food.TypeId = FoodType.Id = @TyepeId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@TypeId", listFoods.SelectedValue);
                DataTable foodNameTable = new DataTable();
                adapter.Fill(foodNameTable);

                listFoods.DisplayMember = "Name";
                listFoods.ValueMember = "Id";
                listFoods.DataSource = foodNameTable;
            }
        }
    }
}
