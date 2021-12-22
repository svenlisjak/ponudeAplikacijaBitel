using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponudeAplikacijaBitel
{
    public partial class postavke : Form
    {
        public postavke()
        {
            InitializeComponent();
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        public static string zaKonekcijuPriprema;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            string desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
            openFileDialog1.InitialDirectory = desktopLocation;
            openFileDialog1.Title = "Baza podataka (.mdf)";
            openFileDialog1.DefaultExt = "mdf";
            openFileDialog1.Filter = "mdf files (*.mdf)|*.mdf";
            textBox1.Text = openFileDialog1.FileName;

            zaKonekcijuPriprema = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =" + openFileDialog1.FileName +"; Integrated Security = True";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                dbManager.KonekcijaBaza = zaKonekcijuPriprema;
            }
            MessageBox.Show("Baza odabrana");
        }

    }
}
