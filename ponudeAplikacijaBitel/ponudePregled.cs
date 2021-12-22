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
using System.Configuration;
using System.IO;

using OfficeOpenXml;

namespace ponudeAplikacijaBitel
{
    public partial class ponudePregled : Form
    {
        public ponudePregled()
        {
            InitializeComponent();
        }

        private void PonudePregled_Load(object sender, EventArgs e)
        {
       
            string Query = null;
            var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            string queryString = "SELECT * FROM dbo.popisPonuda";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            con.Open();

            SqlCommand comm = new SqlCommand(queryString, con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dt);

            Query = comm.ExecuteScalar() as string;



            dataGridView1.ColumnCount = 4;

            dataGridView1.Columns[0].Name = "brojPonude";
            dataGridView1.Columns[1].Name = "narucitelj";
            dataGridView1.Columns[2].Name = "vrstaPosla";
            dataGridView1.Columns[3].Name = "datum";

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[4], row[1], row[3], row[2]);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            string brojPonudeValue = "";
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                brojPonudeValue = Convert.ToString(selectedRow.Cells["brojPonude"].Value);
            }
            SqlConnection con = new SqlConnection(dbManager.KonekcijaBaza);

            SqlCommand com = new SqlCommand();
            con.Open();

            com.Connection = con;

            com.CommandText = "DELETE FROM dbo.popisPonuda WHERE brojPonude = @brPon";
            com.Parameters.AddWithValue("@brPon", brojPonudeValue);

            com.ExecuteNonQuery();


            con.Close();

            int oznacenIndex = dataGridView1.SelectedRows[0].Index;
            //dataGridView1.Rows.RemoveAt(oznacenIndex);
            dataGridView1.Rows.Clear();
            PonudePregled_Load(sender, e);
        }
         
    }
}
