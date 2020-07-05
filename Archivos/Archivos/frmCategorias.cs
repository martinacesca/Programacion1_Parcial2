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
    public partial class frmCategorias : Form
    {

        #region Varaibles
        string nombreArchivo = "categorias.txt";
        FileHelper fileHelper = new FileHelper("categorias.txt");
        #endregion

        #region Form
        public frmCategorias()
        {
            InitializeComponent();
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
        private bool EsNumerico(TextBox textBox, ref float valor, string datoErroneo)
        {
            //funciona si se ingreso coma o punto como separador decimal
            textBox.Text = textBox.Text.Replace('.', ',');

            if (!float.TryParse(textBox.Text, out float valorNumerico))
            {
                MessageBox.Show(datoErroneo + " debe ser un valor numerico", "Oops", MessageBoxButtons.OK);
                textBox.Clear();
                textBox.Focus();
                valor = 0;
                return false;
            }
            valor = valorNumerico;
            return true;
        }
        private bool EsPositivo(TextBox textBox, ref float valor, string datoErroneo)
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
            float valor = 0;
            //Validaciones 
            if (this.TextBoxVacio(this.txtCategoria, "la categoria ")) return false;
            if (this.TextBoxVacio(this.txtValor, "el valor ")) return false;
            if (!this.EsNumerico(this.txtValor, ref valor, "El valor ")) return false;
            if (!this.EsPositivo(this.txtValor, ref valor, "El valor ")) return false;
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
            this.txtCategoria.Text = datos[0];
            this.txtValor.Text = datos[1];
            this.Editar(true);
        }
        private void Cancelar()
        {
            txtCategoria.Text = "";
            txtValor.Text = "";
            this.Editar(false);
        }
        #endregion

        #region Metodos ABM
        private void Alta()
        {
            if (!this.CamposValidos()) return;
            string registro = txtCategoria.Text + "|" + txtValor.Text;
            fileHelper.AltaRegistro(registro);
            this.Mostrar();
            this.Cancelar();
        }
        private void Modificacion()
        {
            if (!this.CamposValidos()) return;
            string registro = txtCategoria.Text + "|" + txtValor.Text;
            fileHelper.ModificacionRegistro(listBox1.SelectedIndex, registro);
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
