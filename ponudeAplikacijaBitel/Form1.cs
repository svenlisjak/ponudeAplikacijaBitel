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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ponudeAplikacijaBitel
{
    
    public partial class Form1 : Form
    {
        public static float ukupnaCijenaPonude = 0;
        public static string popustNaCijenu = "";
        public float ukupnaCijenaBezPopusta = 0;
        public bool txtboxVis = false;
        public bool SviPodaciUpisani = true;

        public Form1()
        {
            InitializeComponent();
            textBox2.Visible = false;
            label5.Visible = false;
            nazivPopust.Visible = false;
            label8.Visible = false;
            unosOpis.Multiline = true;
        }
    

        private void Form1_Load(object sender, EventArgs e)
        {
            this.tableTableAdapter.Fill(this.bazaPonudeDataSet.Table);
        }
        int counterProizvoda = 0;
        bool izmjena = false;
        
        private void Button1_Click(object sender, EventArgs e)
        {
            string sifra = textBox1.Text;

            string Query = null;
            var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            string queryString = "SELECT * FROM dbo.proizvodi WHERE sifra =" + sifra;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            con.Open();

            SqlCommand comm = new SqlCommand(queryString, con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dt);

            Query = comm.ExecuteScalar() as string;


        
            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].Name = "sifra";
            dataGridView1.Columns[1].Name = "naziv";
            dataGridView1.Columns[2].Name = "opis";
            dataGridView1.Columns[3].Name = "cijena";
            dataGridView1.Columns[4].Name = "kolicina";
            dataGridView1.Columns[5].Name = "popust";
            dataGridView1.Columns[6].Name = "ukupna cijena";

            string kolicina = "1";
            float popust = 0;
            float CijenaProv = 0;
            float ukupnaCijena = 0;

            string naziv = "";
            string opis = "";
            string cijena = "";



            if (izmjena)
            {
                foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                {
                    if (dgvR.Cells["sifra"].Value.ToString() == sifra)
                    {
                        kolicina = textBox3.Text;
                        popust = Int32.Parse(textBox2.Text);
                   
                        break;
                    }
                }
            }
            else {
                if (textBox3.Text == "")
                {
                    kolicina = "1";
                }
                else if(textBox3.Text== "paušal")
                {
                    kolicina = "1";
                }
                else if (textBox3.Text == "komplet")
                {
                    kolicina = "1";
                }
                else
                {
                    kolicina = textBox3.Text;
                }
                if (textBox2.Text == "")
                {
                    popust = 0;
                }
                else
                {
                    popust = Int32.Parse(textBox2.Text);
                }
               

            }

            string queryUkupnaCijena = "SELECT cijena FROM dbo.proizvodi WHERE sifra =" + sifra;
            SqlCommand samoCijena = new SqlCommand(queryUkupnaCijena, con);
            SqlDataReader readerCijena = samoCijena.ExecuteReader();



            foreach (DataRow row in dt.Rows)
            {
                naziv = row[2].ToString();
                opis = row[3].ToString();
                string cijenaToFloat = "";
                string cijenaString = row[4].ToString();
                for (int i = 0; i < cijenaString.Length - 3; i++)
                {
                    cijenaToFloat += cijenaString[i];
                }
                cijena = cijenaToFloat;
              
            }

            
            if (popust != 0)
            {
                while (readerCijena.Read())
                {
                    string cijenaToFloat = "";
                    string cijenaString = readerCijena.GetString(0);
                    for (int i = 0; i < cijenaString.Length - 3; i++)
                    {
                        cijenaToFloat += cijenaString[i];
                    }

                    CijenaProv = float.Parse(cijenaToFloat);
                    float brojPopust = 1 - (popust / 100);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv * brojPopust;
                }
            }
            else
            {
                while (readerCijena.Read())
                {
                    string cijenaToFloat = "";
                    string cijenaString = readerCijena.GetString(0);
                    for (int i = 0; i < cijenaString.Length - 3; i++)
                    {
                        cijenaToFloat += cijenaString[i];
                    }

                    CijenaProv = float.Parse(cijenaToFloat);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv;
                }
            }


            if (izmjena)
            {
              
                foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                {
                    if (dgvR.Cells["sifra"].Value.ToString() == sifra)
                    {
                        int oznacenIndex = dataGridView1.SelectedRows[0].Index;
                        //dataGridView1.Rows[oznacenIndex].SetValues(new object[] { sifra, naziv, opis, cijena, kolicina, popustVrijednost, ukupnaCijena });
                        dataGridView1.Rows[oznacenIndex].SetValues(new object[] { sifra, naziv, opis, cijena, kolicina, popust, ukupnaCijena });
                        izmjena = false;
                        break;
                    }
                }
            }
            else
            {
                ukupnaCijenaPonude += ukupnaCijena;
                ukupnaCijenaBezPopusta += ukupnaCijena;
                //dataGridView1.Rows.Add(sifra, naziv, opis, cijena, kolicina, popustVrijednost, ukupnaCijena);
                dataGridView1.Rows.Add(sifra, naziv, opis, cijena, kolicina, popust, ukupnaCijena);
            }

            if (textBox4.Text != "")
            {
                float obradaUkupniPopust = ukupnaCijenaPonude * ( 1 - (float.Parse(textBox4.Text)/100));
           
                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn   Sa popustom = " + obradaUkupniPopust.ToString() + " kn";

                popustNaCijenu = (1 - (float.Parse(textBox4.Text) / 100)).ToString();
            }
            else
            {
                popustNaCijenu = "1";
                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn";
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
           

            counterProizvoda++;
            con.Close();
        }
        public static string opisZaEditForm;
        public static int dataIndexZaEditForm;
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string cleanOpis = "";
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
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
              
            }
            dataIndexZaEditForm = dataGridView1.SelectedRows[0].Index;
            opisZaEditForm = cleanOpis;
            editOpis editanje = new editOpis();
          
            DialogResult dr = editanje.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                /*
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Index == dataIndexZaEditForm)
                    {
                        row.Cells["opis"].Value = editOpis.returnEdit();
                    }
                }
                editanje.Close();*/
            }
            else if (dr == DialogResult.OK)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Index == dataIndexZaEditForm)
                    {
                        row.Cells["opis"].Value = editOpis.returnEdit();
                    }
                }
                //editanje.Close();
                editanje.Dispose();
            }
          

        }
      
     
        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text)) {
                string Query = null;
                var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
                string queryString = "SELECT * FROM dbo.proizvodi WHERE sifra =" + textBox1.Text;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
                con.Open();

                SqlCommand comm = new SqlCommand(queryString, con);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);



                Query = comm.ExecuteScalar() as string;

                foreach (DataRow row in dt.Rows)
                {
                    string brojZnakovaRow2 = row[2].ToString();
                    string cleanNaziv = "";
                    int countPrazno = 0;
                    for (int i = 0; i < brojZnakovaRow2.Count(); i++)
                    {
                        if (char.IsWhiteSpace(brojZnakovaRow2[i]))
                        {
                            countPrazno++;
                        }
                        else
                        {
                            countPrazno = 0;
                        }
                        if (countPrazno < 3)
                        {
                            cleanNaziv += brojZnakovaRow2[i];
                        }

                    }//veroatno pretjerano komplicirano

                    string labelFill = cleanNaziv + "\n" + row[3].ToString();
                    proizvodPrikaz.Text = labelFill;

                    //prikazivanje slika ako ima šifre
                    var placeholder = Directory.GetFiles("slikeProizvoda", "default.png");
                    Image slikaProizvoda = Image.FromFile(placeholder[0]);
                   
                    var slika = Directory.GetFiles("slikeProizvoda", textBox1.Text + ".jpg");
                    if (slika!=null && slika.Length>0)
                    {
                        slikaProizvoda = Image.FromFile(slika[0]);

                    }
                    else
                    {
                        slika = Directory.GetFiles("slikeProizvoda", textBox1.Text + ".png");
                        slikaProizvoda = Image.FromFile(slika[0]);
                    }

                    // proizvodPrikaz.Image = slikaProizvoda;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    
                    pictureBox1.ClientSize = new Size(300, 225);
                    pictureBox1.Image = (Image)slikaProizvoda;
                }



                con.Close();
            }
            return;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            izmjena = true;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                string cellValueSifra = Convert.ToString(selectedRow.Cells["sifra"].Value);
                string cellValueNaziv = Convert.ToString(selectedRow.Cells["naziv"].Value);
                string cellValueKolicina = Convert.ToString(selectedRow.Cells["kolicina"].Value);
                string cellValuePopust = Convert.ToString(selectedRow.Cells["popust"].Value);
                if(unosVrijednosti.SelectedTab == tabPage1)
                {
                    textBox1.Text = cellValueSifra;
                    textBox2.Text = cellValuePopust;
                    textBox3.Text = cellValueKolicina;
                }
                else if (unosVrijednosti.SelectedTab == tabPage2)
                {
                    nazivPretraga.Text = cellValueNaziv;
                    nazivPopust.Text = cellValuePopust;
                    nazivKolicina.Text = cellValueKolicina;
                }

            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int oznacenIndex = dataGridView1.SelectedRows[0].Index;
            dataGridView1.Rows.RemoveAt(oznacenIndex);
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
        bool reset = false;

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
                reset = true;
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

        private void TextBox2_Enter(object sender, EventArgs e)
        {
            if (reset)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
                reset = false;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            generalijeForm generalije = new generalijeForm();
            generalije.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            provjeraSvihUpisanihInformacije();
            if (SviPodaciUpisani == false)
            {
                MessageBox.Show("Nisu svi podaci upisani");
                return;
            }

           var fi = new FileInfo(@"C:\excelTemplate\PonudaBitelTemplate.xlsx");
           // var fi = new FileInfo(Directory.GetFiles("excelTemplate", "PonudaBitelTemplate.xlsx").ToString());
            using (ExcelPackage excel = new ExcelPackage(fi))
            {
               
                var worksheet = excel.Workbook.Worksheets["Ponuda"];

                worksheet.Cells[7, 4].Value = generalijeForm.narucitelj;
                worksheet.Cells[7, 4].Style.Font.Bold = true;

                worksheet.Cells[8, 4].Value = generalijeForm.adresa;
                worksheet.Cells[8, 4].Style.Font.Bold = true;

                worksheet.Cells[10, 1].Value ="PONUDA broj: " + generalijeForm.brojPonude;
                worksheet.Cells[10, 1].Style.Font.Bold = true;

                
                worksheet.Cells[13, 1].Value = generalijeForm.opis;
                worksheet.Cells[13, 1].Style.WrapText = true;

                worksheet.Cells[17, 1].Value = "SPECIFIKACIJA PREDVIĐENE OPREME I RADOVA "+ generalijeForm.vrstaPosla.ToUpper();
                worksheet.Cells[17, 1].Style.Font.Bold = true;
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                
                DataTable data = (DataTable)(dataGridView1.DataSource);

                    DataTable dtSource = new DataTable();
                //////create columns
                int counterReda = 0;
                
                dtSource.Columns.Add("opis", typeof(string));
                dtSource.Columns.Add("kolicina", typeof(string));
                dtSource.Columns.Add("cijena", typeof(string));
                dtSource.Columns.Add("ukupna cijena", typeof(float));
                
                List<object> listSifre = new List<object>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                    
                    counterReda++;
                    if (counterReda == dataGridView1.Rows.Count)
                    {
                        break;
                    }
                   
                    DataRow proizvod = dtSource.NewRow();
                    proizvod["opis"] = counterReda.ToString() + ". Dobava i montaža " + row.Cells["opis"].Value;
                         proizvod["kolicina"] = row.Cells["kolicina"].Value;
                         proizvod["cijena"] = row.Cells["cijena"].Value;
                    //string brojCijena = (row.Cells["ukupna cijena"].Value).ToString();
                    //proizvod["ukupna cijena"] = float.Parse(brojCijena);
                    proizvod["ukupna cijena"] = row.Cells["ukupna cijena"].Value;
                    dtSource.Rows.Add(proizvod);
                    listSifre.Add(row.Cells["sifra"].Value);
                }
                    
                
                worksheet.InsertRow(19, counterReda+1);
               

                string dataRange = "A19:D25";
                worksheet.Cells[dataRange].LoadFromDataTable(dtSource);
                
                for(int i=1; i<counterReda; i++)
                {
                    worksheet.Row(18+i).Height = 150;
                }
                int brojReda = 18;
                foreach (var sifra in listSifre) {
                    
                    //prikazivanje slika ako ima šifre
                    var placeholder = Directory.GetFiles("slikeProizvoda", "default.png");
                    Image slikaProizvoda = Image.FromFile(placeholder[0]);

                    if (sifra.ToString().Length > 1) { 
                        var slika = Directory.GetFiles("slikeProizvoda", sifra.ToString() + ".jpg");
                        if (slika != null && slika.Length > 0)
                        {
                            slikaProizvoda = Image.FromFile(slika[0]);

                        }
                        else
                        {
                            slika = Directory.GetFiles("slikeProizvoda", sifra.ToString() + ".png");
                            slikaProizvoda = Image.FromFile(slika[0]);
                        }
                    }
                    else
                    {
                        var slika = Directory.GetFiles("tempSlike", sifra.ToString() + ".jpg");
                        if (slika != null && slika.Length > 0)
                        {
                            slikaProizvoda = Image.FromFile(slika[0]);

                        }
                        else
                        {
                            slika = Directory.GetFiles("tempSlike", sifra.ToString() + ".png");
                            slikaProizvoda = Image.FromFile(slika[0]);
                        }

                    }


                    var picture = worksheet.Drawings.AddPicture("a"+sifra, ResizeImage(slikaProizvoda, 130, 130));
                    picture.SetPosition(brojReda, 15, 1, 15);
                    brojReda++;
                }

                worksheet.Column(2).Width = 10;

               // worksheet.Cells[dataRange].Style.WrapText = true;
                
                worksheet.Cells[dataRange].Style.Font.Size = 10;
                worksheet.Cells[dataRange].Style.Font.Name = "Arial";
                worksheet.Cells[dataRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                worksheet.Cells["A19:A"+ (19+counterReda)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                

                worksheet.Cells[22 + counterReda, 4].Formula = "=SUM(D18:D"+(19+counterReda)+")";
                if (popustNaCijenu == "0")
                {
                    worksheet.Cells[23 + counterReda, 3].Value = "POPUST(0%)";
                    worksheet.Cells[23 + counterReda, 8].Value = "0";
                }
                else { 
                worksheet.Cells[23 + counterReda, 3].Value = "POPUST(" + popustNaCijenu + "%)";
                worksheet.Cells[23 + counterReda, 8].Value = popustNaCijenu;
                }
                worksheet.Cells[23 + counterReda, 3].Style.Font.Bold = true;
                worksheet.Cells[23 + counterReda, 4].Style.Font.Bold = true;
                worksheet.Cells[23 + counterReda, 5].Style.Font.Bold = true;
                DateTime datum = DateTime.Today;
                
                worksheet.Cells[38+counterReda, 1].Value = "Valjanost ponude: " + datum.AddMonths(1).ToString("dd.MM.yyyy.");
                worksheet.Cells[43+counterReda, 1].Value = "U Čakovcu, " + datum.ToString("dd.MM.yyyy.");
                // excel.Save();
                FileInfo excelFile = new FileInfo(@"C:\Users\Sven\Desktop\test.xlsx");
                excel.SaveAs(excelFile);
                MessageBox.Show("Excel file spreman");
            }

        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            dbManager dbManage = new dbManager();
            dbManage.Refresh();
            dbManage.Show();
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            popustNaCijenu = textBox4.Text;
            float obradaUkupniPopust = ukupnaCijenaPonude * (1 - (float.Parse(textBox4.Text) / 100));
            label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn   Sa popustom = " + obradaUkupniPopust.ToString() + " kn";

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            ponudePregled ponudePreglednik = new ponudePregled();
            ponudePreglednik.Refresh();
            ponudePreglednik.Show();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void CheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (txtboxVis == false)
            {
                textBox2.Visible = true;
                label5.Visible = true;
                nazivPopust.Visible = true;
                label8.Visible = true;
                txtboxVis = true;
                return;
            }

            if (txtboxVis == true)
            {
                textBox2.Visible = false;
                label5.Visible = false;
                nazivPopust.Visible = false;
                label8.Visible = false;
                txtboxVis = false;
                return;
            }
        }

        private void provjeraSvihUpisanihInformacije()
        {
       
            if (string.IsNullOrEmpty(generalijeForm.narucitelj))
            {
                SviPodaciUpisani = false;
                return;
            }
           else if (string.IsNullOrEmpty(generalijeForm.opis))
            {
                SviPodaciUpisani = false;
                return;
            }
           else if (string.IsNullOrEmpty(generalijeForm.vrstaPosla))
            {
                SviPodaciUpisani = false;
                return;
            }
           else if (string.IsNullOrEmpty(generalijeForm.adresa))
            {
                SviPodaciUpisani = false;
                return;
            }
          else if (string.IsNullOrEmpty(generalijeForm.brojPonude))
            {
                SviPodaciUpisani = false;
                return;
            }
            else
            {
                SviPodaciUpisani = true;
            }

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            postavke postavke1 = new postavke();
            postavke1.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string nazivPure = nazivPretraga.Text;
            string sifra = nazivPretraga.Text;
            for(int i = nazivPure.Length; i<51; i++)
            {
                sifra += " ";
            }

            string Query = null;
            var connectionString = ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString;
            string queryString = "SELECT * FROM dbo.proizvodi WHERE nazivProizvoda = '" + sifra + "'";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["newConnectionString"].ConnectionString);
            con.Open();

            SqlCommand comm = new SqlCommand(queryString, con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dt);

            Query = comm.ExecuteScalar() as string;



            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].Name = "sifra";
            dataGridView1.Columns[1].Name = "naziv";
            dataGridView1.Columns[2].Name = "opis";
            dataGridView1.Columns[3].Name = "cijena";
            dataGridView1.Columns[4].Name = "kolicina";
            dataGridView1.Columns[5].Name = "popust";
            dataGridView1.Columns[6].Name = "ukupna cijena";

            string kolicina = "1";
            float popust = 0;
            float CijenaProv = 0;
            float ukupnaCijena = 0;

            string naziv = "";
            string opis = "";
            string cijena = "";



            if (izmjena)
            {
                foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                {
                    if (dgvR.Cells["sifra"].Value.ToString() == sifra)
                    {
                        kolicina = nazivKolicina.Text;
                        popust = Int32.Parse(nazivPopust.Text);

                        break;
                    }
                }
            }
            else
            {
                if (nazivKolicina.Text == "")
                {
                    kolicina = "1";
                }
                else if (nazivKolicina.Text == "paušal")
                {
                    kolicina = "1";
                }
                else if (nazivKolicina.Text == "komplet")
                {
                    kolicina = "1";
                }
                else
                {
                    kolicina = nazivKolicina.Text;
                }
                if (nazivPopust.Text == "")
                {
                    popust = 0;
                }
                else
                {
                    popust = Int32.Parse(nazivPopust.Text);
                }


            }

            string queryUkupnaCijena = "SELECT cijena FROM dbo.proizvodi WHERE nazivProizvoda = '" + sifra+"'";
            SqlCommand samoCijena = new SqlCommand(queryUkupnaCijena, con);
            SqlDataReader readerCijena = samoCijena.ExecuteReader();



            foreach (DataRow row in dt.Rows)
            {
                naziv = row[2].ToString();
                opis = row[3].ToString();
                string cijenaToFloat = "";
                string cijenaString = row[4].ToString();
                for (int i = 0; i < cijenaString.Length - 3; i++)
                {
                    cijenaToFloat += cijenaString[i];
                }
                cijena = cijenaToFloat;

            }


            if (popust != 0)
            {
                while (readerCijena.Read())
                {
                    string cijenaToFloat = "";
                    string cijenaString = readerCijena.GetString(0);
                    for (int i = 0; i < cijenaString.Length - 3; i++)
                    {
                        cijenaToFloat += cijenaString[i];
                    }

                    CijenaProv = float.Parse(cijenaToFloat);
                    float brojPopust = 1 - (popust / 100);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv * brojPopust;
                }
            }
            else
            {
                while (readerCijena.Read())
                {
                    string cijenaToFloat = "";
                    string cijenaString = readerCijena.GetString(0);
                    for (int i = 0; i < cijenaString.Length - 3; i++)
                    {
                        cijenaToFloat += cijenaString[i];
                    }

                    CijenaProv = float.Parse(cijenaToFloat);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv;
                }
            }


            if (izmjena)
            {

                foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                {
                    if (dgvR.Cells["sifra"].Value.ToString() == sifra)
                    {
                        int oznacenIndex = dataGridView1.SelectedRows[0].Index;
                        //dataGridView1.Rows[oznacenIndex].SetValues(new object[] { sifra, naziv, opis, cijena, kolicina, popustVrijednost, ukupnaCijena });
                        dataGridView1.Rows[oznacenIndex].SetValues(new object[] { sifra, naziv, opis, cijena, kolicina, popust, ukupnaCijena });
                        izmjena = false;
                        break;
                    }
                }
            }
            else
            {
                ukupnaCijenaPonude += ukupnaCijena;
                ukupnaCijenaBezPopusta += ukupnaCijena;
                //dataGridView1.Rows.Add(sifra, naziv, opis, cijena, kolicina, popustVrijednost, ukupnaCijena);
                dataGridView1.Rows.Add(sifra, naziv, opis, cijena, kolicina, popust, ukupnaCijena);
            }

            if (textBox4.Text != "")
            {
                float obradaUkupniPopust = ukupnaCijenaPonude * (1 - (float.Parse(textBox4.Text) / 100));

                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn   Sa popustom = " + obradaUkupniPopust.ToString() + " kn";

                popustNaCijenu = (1 - (float.Parse(textBox4.Text) / 100)).ToString();
            }
            else
            {
                popustNaCijenu = "1";
                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn";
            }
            nazivPretraga.Clear();
            nazivPopust.Clear();
            nazivKolicina.Clear();


            counterProizvoda++;
            con.Close();
        }

        public static int sifraManualniUnos = 0;
        private void predajManualno_Click(object sender, EventArgs e)
        {
           
            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].Name = "sifra";
            dataGridView1.Columns[1].Name = "naziv";
            dataGridView1.Columns[2].Name = "opis";
            dataGridView1.Columns[3].Name = "cijena";
            dataGridView1.Columns[4].Name = "kolicina";
            dataGridView1.Columns[5].Name = "popust";
            dataGridView1.Columns[6].Name = "ukupna cijena";

            string kolicina = "1";
            float popust = 0;
            float CijenaProv = 0;
            float ukupnaCijena = 0;

            string naziv = unosNaziv.Text;
            string opis = unosOpis.Text;
            string cijena = unosCijena.Text;

                if (unosKolicina.Text == "")
                {
                    kolicina = "1";
                }
                else if (unosKolicina.Text == "paušal")
                {
                    kolicina = "1";
                }
                else if (unosKolicina.Text == "komplet")
                {
                    kolicina = "1";
                }
                else
                {
                    kolicina = unosKolicina.Text;
                }
                if (unosPopust.Text == "")
                {
                    popust = 0;
                }
                else
                {
                    popust = Int32.Parse(unosPopust.Text);
                }  


            if (popust != 0)
            {
                    CijenaProv = float.Parse(unosCijena.Text);
                    float brojPopust = 1 - (popust / 100);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv * brojPopust;    
            }
            else
            {
                    CijenaProv = float.Parse(unosCijena.Text);
                    ukupnaCijena = Int32.Parse(kolicina) * CijenaProv;   
            }


                ukupnaCijenaPonude += ukupnaCijena;
                ukupnaCijenaBezPopusta += ukupnaCijena;
                //dataGridView1.Rows.Add(sifra, naziv, opis, cijena, kolicina, popustVrijednost, ukupnaCijena);
                dataGridView1.Rows.Add(sifraManualniUnos, naziv, opis, cijena, kolicina, popust, ukupnaCijena);
            

            if (textBox4.Text != "")
            {
                float obradaUkupniPopust = ukupnaCijenaPonude * (1 - (float.Parse(textBox4.Text) / 100));

                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn   Sa popustom = " + obradaUkupniPopust.ToString() + " kn";

                popustNaCijenu = (1 - (float.Parse(textBox4.Text) / 100)).ToString();
            }
            else
            {
                popustNaCijenu = "1";
                label4.Text = "Bez popusta = " + ukupnaCijenaBezPopusta.ToString() + " kn";
            }
            unosNaziv.Clear();
            unosPopust.Clear();
            unosOpis.Clear();
            unosKolicina.Clear();
            unosCijena.Clear();

            sifraManualniUnos++;
            counterProizvoda++;
           
        }

        private void unosPopust_TextChanged(object sender, EventArgs e)
        {
            string kolicina = "1";
            float popust = 0;
            float CijenaProv = 0;
            float ukupnaCijena = 0;

            string cijena = unosCijena.Text;

            if (unosKolicina.Text == "")
            {
                kolicina = "1";
            }
            else if (unosKolicina.Text == "paušal")
            {
                kolicina = "1";
            }
            else if (unosKolicina.Text == "komplet")
            {
                kolicina = "1";
            }
            else
            {
                kolicina = unosKolicina.Text;
            }
            if (unosPopust.Text == "")
            {
                popust = 0;
            }
            else
            {
                popust = Int32.Parse(unosPopust.Text);
            }


            if (popust != 0)
            {
                CijenaProv = float.Parse(unosCijena.Text);
                float brojPopust = 1 - (popust / 100);
                ukupnaCijena = Int32.Parse(kolicina) * CijenaProv * brojPopust;

            }
            else
            {
                CijenaProv = float.Parse(unosCijena.Text);
                ukupnaCijena = Int32.Parse(kolicina) * CijenaProv;
            }

            manulanoUkupnaCijena.Text = ukupnaCijena.ToString() + " kn";
        }

        private void unosKolicina_TextChanged(object sender, EventArgs e)
        {
            string kolicina = "1";
            float CijenaProv = 0;
            float ukupnaCijena = 0;

            string cijena = unosCijena.Text;

            if (unosKolicina.Text == "")
            {
                kolicina = "1";
            }
            else if (unosKolicina.Text == "paušal")
            {
                kolicina = "1";
            }
            else if (unosKolicina.Text == "komplet")
            {
                kolicina = "1";
            }
            else
            {
                kolicina = unosKolicina.Text;
            }
          
                CijenaProv = float.Parse(unosCijena.Text);
                ukupnaCijena = Int32.Parse(kolicina) * CijenaProv;
       

            manulanoUkupnaCijena.Text = ukupnaCijena.ToString() + " kn";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            string desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.InitialDirectory = desktopLocation;
            openFileDialog1.Title = "Slika proizvoda (.jpg/.png)";
           // openFileDialog1.DefaultExt = "mdf";
            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";
            //textBox1.Text = openFileDialog1.FileName;
            string fileCopyObrada = "";
            string extension = Path.GetExtension(openFileDialog1.FileName);
            if (extension == ".jpg")
            {
                fileCopyObrada = Directory.GetCurrentDirectory() + "\\tempSlike\\" + sifraManualniUnos + ".jpg";
            }
            else if(extension == ".png")
            {
                fileCopyObrada = Directory.GetCurrentDirectory() + "\\tempSlike\\" + sifraManualniUnos + ".png";
            }
        
            File.Copy(openFileDialog1.FileName, fileCopyObrada);

        }
    }
    

}

