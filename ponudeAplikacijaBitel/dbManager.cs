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
    public partial class dbManager : Form
    {
        public DataTable searchTable = new DataTable();
        public static string KonekcijaBaza = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Sven\\source\\repos\\ponudeAplikacijaBitel\\ponudeAplikacijaBitel\\bazaPonude.mdf; Integrated Security = True";
        public dbManager()
        {
            InitializeComponent();
            //dt.Columns.Add("id", typeof(int));
            searchTable.Columns.Add("sifra", typeof(string));
            searchTable.Columns.Add("nazivProizvoda", typeof(string));
            searchTable.Columns.Add("opisProizvoda", typeof(string));
            searchTable.Columns.Add("cijena", typeof(float));
        }
        public bool izmjenaDb = false;
       
        private void DbManager_Load(object sender, EventArgs e)
        {
            textBox3.Multiline = true;
            textBox3.WordWrap = true;
            string Query = null;
            var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            string queryString = "SELECT * FROM dbo.proizvodi";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            con.Open();

            SqlCommand comm = new SqlCommand(queryString, con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(comm);
           
            da.Fill(dt);

            Query = comm.ExecuteScalar() as string;



            dataGridView1.ColumnCount = 4;

            dataGridView1.Columns[0].Name = "sifra";
            dataGridView1.Columns[1].Name = "naziv";
            dataGridView1.Columns[2].Name = "opis";
            dataGridView1.Columns[3].Name = "cijena";

            foreach (DataRow row in dt.Rows)
            {
                string cijenaToFloat = "";
                string cijenaString = row[4].ToString();
              
                for (int i = 0; i < cijenaString.Length - 3; i++)
                {
                    cijenaToFloat += cijenaString[i];
                }
             
                dataGridView1.Rows.Add(row[1], row[2], row[3], float.Parse(cijenaToFloat));
            }

          
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                searchTable.Rows.Add(row.Cells["sifra"].Value, row.Cells["naziv"].Value, row.Cells["opis"].Value, row.Cells["cijena"].Value);
            }
        }
     

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string cleanNaziv = "";
                int countPrazno = 0;
                string nazivCell= row.Cells[1].Value.ToString();
                for (int i = 0; i < nazivCell.Count(); i++)
                {
                    if (char.IsWhiteSpace(nazivCell[i]))
                    {
                        countPrazno++;
                    }
                    else
                    {
                        countPrazno = 0;
                    }
                    if (countPrazno < 3)
                    {
                        cleanNaziv += nazivCell[i];
                    }

                }
                string cleanOpis = "";
                int countPraznoOpis = 0;
                string opisCell = row.Cells[2].Value.ToString();
                for (int i = 0; i < opisCell.Count(); i++)
                {
                    if (char.IsWhiteSpace(opisCell[i]))
                    {
                        countPraznoOpis++;
                    }
                    else
                    {
                        countPraznoOpis = 0;
                    }
                    if (countPraznoOpis < 3)
                    {
                        cleanOpis += opisCell[i];
                    }

                }

                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = cleanNaziv;
                textBox3.Text = cleanOpis;
                textBox4.Text = row.Cells[3].Value.ToString();
                izmjenaDb = true;
            }
            izmjenaDb = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\bazaPonude.mdf;Integrated Security=True");
            SqlConnection con = new SqlConnection(KonekcijaBaza);

            // var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            SqlCommand com = new SqlCommand();
            con.Open();


            com.Connection = con;
            if (izmjenaDb == true) {
                string sifra = textBox1.Text;
                com.CommandText = "UPDATE dbo.proizvodi SET sifra = @sifraEdit, nazivProizvoda = @nazivEdit, opisProizvoda = @opisEdit, cijena=@cijenaEdit WHERE sifra = @sifraEdit";
                com.Parameters.AddWithValue("@sifraEdit", textBox1.Text);

                com.Parameters.AddWithValue("@nazivEdit", textBox2.Text);

                com.Parameters.AddWithValue("@opisEdit", textBox3.Text);

                com.Parameters.AddWithValue("@cijenaEdit", textBox4.Text + ",00 kn");

                com.ExecuteNonQuery();

                izmjenaDb = false;
            }
            else { 
           
                com.CommandText = "INSERT INTO dbo.proizvodi (sifra, nazivProizvoda, opisProizvoda, cijena) VALUES (@sifraEdit, @nazivEdit, @opisEdit, @cijenaEdit)";
                com.Parameters.AddWithValue("@sifraEdit", textBox1.Text);
         
                com.Parameters.AddWithValue("@nazivEdit", textBox2.Text);
         
                com.Parameters.AddWithValue("@opisEdit", textBox3.Text);

                com.Parameters.AddWithValue("@cijenaEdit", textBox4.Text + ",00 kn");

                com.ExecuteNonQuery();
                izmjenaDb = false;
            }

            string cleanNaziv = "";
            int countPrazno = 0;
            for (int i = 0; i < textBox2.Text.Count(); i++)
            {
                if (char.IsWhiteSpace(textBox2.Text[i]))
                {
                    countPrazno++;
                }
                else
                {
                    countPrazno = 0;
                }
                if (countPrazno < 3)
                {
                    cleanNaziv += textBox2.Text[i];
                }

            }
            string cleanOpis = "";
            int countPraznoOpis = 0;
            for (int i = 0; i < textBox3.Text.Count(); i++)
            {
                if (char.IsWhiteSpace(textBox3.Text[i]))
                {
                    countPraznoOpis++;
                }
                else
                {
                    countPraznoOpis = 0;
                }
                if (countPraznoOpis < 3)
                {
                    cleanOpis += textBox3.Text[i];
                }

            }

            dataGridView1.Rows.Add(textBox1.Text, cleanNaziv, cleanOpis, float.Parse(textBox4.Text));
            con.Close();
            MessageBox.Show("Izmjene spremljene");

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if(izmjenaDb == true) { 
            SqlConnection con = new SqlConnection(KonekcijaBaza);
            
            SqlCommand com = new SqlCommand();
            con.Open();

            com.Connection = con;
           
            com.CommandText = "DELETE FROM dbo.proizvodi WHERE sifra = @sifra";
            com.Parameters.AddWithValue("@sifra", textBox1.Text);

            com.ExecuteNonQuery();

           
            con.Close();

                int oznacenIndex = dataGridView1.SelectedRows[0].Index;
                dataGridView1.Rows.RemoveAt(oznacenIndex);

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(searchBox.Text))
            {
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                DbManager_Load(sender, e);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                string kategorija = "opisProizvoda";
                searchTable.DefaultView.RowFilter = string.Format("" + kategorija + " like '%{0}%'", searchBox.Text.Trim().Replace("'", "''"));
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = searchTable;
            }
            catch (Exception) { }
        }
    }
}
