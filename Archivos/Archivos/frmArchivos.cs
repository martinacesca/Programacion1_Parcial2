using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archivos
{
    public partial class frmArchivos : Form
    {
        public frmArchivos()
        {
            InitializeComponent();
            Directory.CreateDirectory(@"D:/UAI/Programacion I/Archivos");
            Directory.SetCurrentDirectory(@"D:/UAI/Programacion I/Archivos");
        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmpleados frm = new frmEmpleados();
            frm.MdiParent = this;
            frm.Show();
        }

        private void horasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHoras frm = new frmHoras();
            frm.MdiParent = this;
            frm.Show();
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategorias frm = new frmCategorias();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
