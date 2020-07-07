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
    public partial class frmHoras : Form
    {
       
        #region Varaibles
        string nombreArchivo = "horas.txt";
        FileHelper fileHelper = new FileHelper("horas.txt");
        #endregion

        #region Form
        public frmHoras()
        {
            InitializeComponent();
            this.CargaCombo();
            this.Mostrar();
            this.Editar(false);
        }
        #endregion

        #region Validaciones 
        private bool TextBoxVacio(TextBox textBox, string datoFaltante)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show("Debe ingresar " + datoFaltante, "Oops", MessageBoxButtons.OK);
                textBox.Clear();
                textBox.Focus();
                return true;
            }
            return false;
        }
        private bool ComboBoxVacio(ComboBox combo, string datoFaltante)
        {
            if (combo.SelectedIndex < 0)
            {
                MessageBox.Show("Debe ingresar " + datoFaltante, "Oops", MessageBoxButtons.OK);
                return true;
            }
            return false;
        }
        private bool EsNumerico(TextBox textBox, ref int valor, string datoErroneo)
        {
            if (!int.TryParse(textBox.Text, out int valorNumerico))
            {
                MessageBox.Show(datoErroneo + " debe ser un valor numerico entero", "Oops", MessageBoxButtons.OK);
                textBox.Clear();
                textBox.Focus();
                valor = 0;
                return false;
            }
            valor = valorNumerico;
            return true;
        }
        private bool EsPositivo(TextBox textBox, ref int valor, string datoErroneo)
        {
            if (valor < 0)
            {
                MessageBox.Show(datoErroneo + " debe ser positivo", "Oops", MessageBoxButtons.OK);
                textBox.Clear();
                textBox.Focus();
                valor = 0;
                return false;
            }
            return true;
        }
        
        private bool CamposValidos()
        {
            if (this.ComboBoxVacio(this.cmbLegajo, "el legajo ")) return false;
            if (this.ComboBoxVacio(this.cmbDia, "el dia ")) return false;
            if (this.ComboBoxVacio(this.cmbHoras, "las horas ")) return false;

            return true;
        }
        #endregion

        #region Eventos
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) => this.SelectedListbox();
        private void btnAlta_Click(object sender, EventArgs e) => this.Alta();
        private void btnModificacion_Click(object sender, EventArgs e) => this.Modificacion();
        private void btnCancelar_Click(object sender, EventArgs e) => this.Cancelar();
        private void btnBaja_Click(object sender, EventArgs e) => this.Baja();

        #endregion

        #region Metodos Generales
        private void CargaCombo()
        {
            //COMBO LEGAJO
            FileStream archivo = new FileStream("empleados.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivo);
            string linea = lector.ReadLine();
            while (linea != null)
            {
                string[] datos = linea.Split('|');
                cmbLegajo.Items.Add(datos[0]);
                linea = lector.ReadLine();
            }
            lector.Close();
            archivo.Close();

            //COMBO DIA
            for (int i = 1; i <= 31; i++)
            {
                cmbDia.Items.Add(i);
            }
            //COMBO HORAS
            for (int i = 0; i <= 24; i++)
            {
                cmbHoras.Items.Add(i);
            }
        }
        private void Mostrar()
        {
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivo);
            string linea = lector.ReadLine();
            this.listBox1.Items.Clear();
            while (linea != null)
            {
                this.listBox1.Items.Add(linea);
                linea = lector.ReadLine();
            }
            lector.Close();
            archivo.Close();
        }
        private void Editar(bool estado)
        {
            btnAlta.Enabled = !estado;
            btnBaja.Enabled = estado;
            btnModificacion.Enabled = estado;
            btnCancelar.Enabled = estado;
        }
        private void SelectedListbox()
        {
            string registro = listBox1.SelectedItem.ToString();
            string[] datos = registro.Split('|');
            this.cmbLegajo.SelectedIndex = this.cmbLegajo.FindStringExact(datos[0]);
            this.cmbDia.SelectedIndex = this.cmbDia.FindStringExact(datos[1]);
            this.cmbHoras.SelectedIndex = this.cmbHoras.FindStringExact(datos[2]);
            this.Editar(true);
        }
        private void Cancelar()
        {
            cmbHoras.SelectedIndex = -1;
            cmbLegajo.SelectedIndex = -1;
            cmbDia.SelectedIndex = -1;
            this.Editar(false);
        }
        #endregion

        #region Metodos ABM
        private void Alta()
        {
            if (!this.CamposValidos()) return;
            string registro = cmbLegajo.SelectedItem + "|" + cmbDia.SelectedItem + "|" + cmbHoras.SelectedItem;
            fileHelper.AltaRegistroOrdenado(registro);
            this.Mostrar();
            this.Cancelar();
        }
        private void Modificacion()
        {
            if (!this.CamposValidos()) return;
            string registro = cmbLegajo.SelectedItem + "|" + cmbDia.SelectedItem + "|" + cmbHoras.SelectedItem;
            fileHelper.ModificacionRegistro(listBox1.SelectedIndex, registro, true);
            this.Mostrar();
            this.Cancelar();
        }
        private void Baja()
        {
            fileHelper.BajaRegistro(listBox1.SelectedIndex);
            this.Mostrar();
            this.Cancelar();
        }
        #endregion

    }
    

    
}
