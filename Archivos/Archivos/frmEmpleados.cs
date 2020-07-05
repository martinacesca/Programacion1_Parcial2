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
    public partial class frmEmpleados : Form
    {
        #region Varaibles
        string nombreArchivo = "empleados.txt";
        FileHelper fileHelper = new FileHelper("empleados.txt");
        #endregion

        #region Form
        public frmEmpleados()
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
            int legajo = 0;
            //Validaciones 
            if (this.TextBoxVacio(this.txtLegajo, "el legajo del empleado ")) return false;
            if (this.TextBoxVacio(this.txtApellido, "el apellido del empleado ")) return false;
            if (this.TextBoxVacio(this.txtNombre, "el nombre del empleado ")) return false;
            if (this.ComboBoxVacio(this.cmbCategoria, "la categoria ")) return false;
            if (!this.EsNumerico(this.txtLegajo, ref legajo, "El legajo ")) return false;
            if (!this.EsPositivo(this.txtLegajo, ref legajo, "El legajo ")) return false;
            return true;
        }
        #endregion


        #region Eventos
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) => this.SelectedListbox();
        private void btnAlta_Click(object sender, EventArgs e) => this.Alta();
        private void btnModificacion_Click(object sender, EventArgs e) => this.Modificacion();
        private void btnCancelar_MouseClick(object sender, MouseEventArgs e) => this.Cancelar();
        private void btnBaja_Click(object sender, EventArgs e) => this.Baja();

        #endregion

        #region Metodos Generales
        private void CargaCombo()
        {
            FileStream archivo = new FileStream("categorias.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivo);
            string linea = lector.ReadLine();
            string[] datos = linea.Split('|');
            while (linea != null)
            {
                cmbCategoria.Items.Add(datos[0]);
                linea = lector.ReadLine();
                if (linea != null) datos = linea.Split('|');
            }
            lector.Close();
            archivo.Close();
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
            this.txtLegajo.Text = datos[0];
            this.cmbCategoria.SelectedIndex = this.cmbCategoria.FindStringExact(datos[1]);
            this.txtApellido.Text = datos[2];
            this.txtNombre.Text = datos[3];
            this.Editar(true);
        }
        private void Cancelar()
        {
            txtLegajo.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtApellido.Text = "";
            txtNombre.Text = "";
            this.Editar(false);
        }
        #endregion

        #region Metodos ABM
        private void Alta()
        {
            if (!this.CamposValidos()) return;
            string registro = txtLegajo.Text + "|" + cmbCategoria.SelectedItem.ToString() + "|" + txtApellido.Text + "|" + txtNombre.Text;
            fileHelper.AltaRegistro(registro);
            this.Mostrar();
            this.Cancelar();
        }
        private void Modificacion()
        {
            if (!this.CamposValidos()) return;
            string registro = txtLegajo.Text + "|" + cmbCategoria.SelectedItem.ToString() + "|" + txtApellido.Text + "|" + txtNombre.Text;
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
