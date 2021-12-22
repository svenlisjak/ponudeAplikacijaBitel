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

namespace ponudeAplikacijaBitel
{
    public partial class generalijeForm : Form
    {
        public static string narucitelj;
        public static string adresa;
        public static string brojPonude;
        public static string datumStr;
        public static string vrstaPosla;
        public static string opis;
        public bool podaciSuIspravni = false;
        public static bool podaciSuSpremljenjiUbazu = false;
        public generalijeForm()
        {
            InitializeComponent();
            //svi proizvodi s kamir.hr
            comboBox1.Items.Add("Videonadzor");
            comboBox1.Items.Add("Vatrodojava");
            comboBox1.Items.Add("Protuprovala");
            comboBox1.Items.Add("Kontrola pristupa i radnog vremena");
            comboBox1.Items.Add("Interfonija");
            textBox2.Multiline=true;
            if (brojPonude != "")
            {
                textBox3.Text = brojPonude;
            }
            if (narucitelj != "")
            {
                textBox1.Text = narucitelj;
            }
         
            if (vrstaPosla != "")
            {
                comboBox1.SelectedItem = vrstaPosla;
            }
            if (adresa != "")
            {
                textBox4.Text = adresa;
            }
            if (opis != "")
            {
                textBox2.Text = opis;
            }



            var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            string queryString = "SELECT brojPonude FROM dbo.popisPonuda ORDER BY Id DESC";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            con.Open();
           

            SqlCommand command = new SqlCommand(queryString, con);
            SqlDataReader sReader;

            sReader = command.ExecuteReader();
            string SljedecaPonudaBroj="";
          
            List<string> brojeviPonudaIndexed = new List<string>();
            while (sReader.Read())
            {
                brojeviPonudaIndexed.Add(sReader["brojPonude"].ToString());
            }
            string TrenutniBrojPonude="";
            string ponudaBrojBaza = brojeviPonudaIndexed.First();
            for(int i = 1; i< ponudaBrojBaza.Length; i++)
            {
               
                if (ponudaBrojBaza[i].Equals('/'))
                {
                    break;
                }
                TrenutniBrojPonude += ponudaBrojBaza[i];
            }
            int brojika = (Int32.Parse(TrenutniBrojPonude) + 1);
            SljedecaPonudaBroj = "0" + brojika + "/" + DateTime.Now.Year.ToString();


            textBox3.Text = SljedecaPonudaBroj;
            brojPonude = SljedecaPonudaBroj;
           
            con.Close();
        }


        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            //SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\bazaPonude.mdf;Integrated Security=True");
            //SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Sven\\source\\repos\\ponudeAplikacijaBitel\\ponudeAplikacijaBitel\\bazaPonude.mdf; Integrated Security = True");
            SqlConnection con = new SqlConnection(dbManager.KonekcijaBaza);
            // var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
           SqlCommand com = new SqlCommand();
             con.Open();


             com.Connection = con;
             com.CommandText = "INSERT INTO dbo.popisPonuda (brojPonude, datum, narucitelj, vrstaPosla) VALUES (@brojPonude, @datum, @narucitelj, @vrstaPosla)";
             com.Parameters.AddWithValue("@brojPonude", textBox3.Text);
            brojPonude = textBox3.Text;
               DateTime datum = DateTime.Today;
             com.Parameters.AddWithValue("@datum", datum.ToString("dd.MM.yyyy."));
            datumStr = datum.ToString("dd.MM.yyyy.");
             com.Parameters.AddWithValue("@narucitelj", textBox1.Text);
            narucitelj = textBox1.Text;

            adresa = textBox4.Text;
            opis = textBox2.Text;
            ispravnostPodataka();
            if (podaciSuIspravni == false)
            {
                MessageBox.Show("Nisu svi podaci upisani");
                return;
            }

            com.Parameters.AddWithValue("@vrstaPosla", comboBox1.Text);
            vrstaPosla = comboBox1.Text;

            if (podaciSuSpremljenjiUbazu == false)
            {
                com.ExecuteNonQuery();
                podaciSuSpremljenjiUbazu = true;
            }
           
           
           
            
            con.Close();
            MessageBox.Show("Generalije spremljene");

        }
        public void ispravnostPodataka()
        {
           if(!string.IsNullOrEmpty(brojPonude) && !string.IsNullOrEmpty(opis) && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(adresa)  && !string.IsNullOrEmpty(narucitelj)) { 
                podaciSuIspravni = true;
           }
        }


        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.KeyCode == Keys.Up)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox2_KeyUp(object sender, KeyEventArgs e)
        {

            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.KeyCode == Keys.Up)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                Button1_Click(sender, e);
                this.SelectNextControl((Control)sender, false, true, true, true);

            }
            if (e.KeyCode == Keys.Up)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

    }
}
