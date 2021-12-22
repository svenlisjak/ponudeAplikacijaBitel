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
    public partial class editOpis : Form
    {
        public editOpis()
        {
            InitializeComponent();
            opisEdit.Text = Form1.opisZaEditForm;
            
            opisEdit.Multiline= true;
        }

        public static string tekst;
        public static string returnEdit() {
            if (tekst != null)
            {
                return tekst;
            }
            else
            {
                return Form1.opisZaEditForm;
            }
           
        }
        public bool edited = false;
        private void Edit_Click(object sender, EventArgs e)
        {
                tekst = opisEdit.Text;
                edited = true;
            // returnEdit();
                
            DialogResult = DialogResult.OK;
         
            Close();


        }
    }
}
