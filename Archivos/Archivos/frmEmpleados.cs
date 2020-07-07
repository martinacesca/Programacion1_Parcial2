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
        private bool ExisteLegajo(int legajoInsertar)
        {
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivo);
            string linea = lector.ReadLine();
            while (linea != null)
            {
                int legajo = int.Parse(linea.Split('|')[0]);
                if (legajo == legajoInsertar)
                {
                    lector.Close();
                    archivo.Close();
                    MessageBox.Show($"Ya existe un empleado con legajo número {legajoInsertar}");
                    return true;
                }
                linea = lector.ReadLine();
            }
            lector.Close();
            archivo.Close();
            return false;
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
            if (this.ExisteLegajo(int.Parse(this.txtLegajo.Text))) return false;
            return true;
        }
        #endregion


        #region Eventos
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) => this.SelectedListbox();
        private void btnAlta_Click(object sender, EventArgs e) => this.Alta();
        private void btnModificacion_Click(object sender, EventArgs e) => this.Modificacion();
        private void btnCancelar_MouseClick(object sender, MouseEventArgs e) => this.Cancelar();
        private void btnBaja_Click(object sender, EventArgs e) => this.Baja();
        private void btnReporte_Click(object sender, EventArgs e) => this.GenerarReporte();
        #endregion

        #region Metodos Generales
        private void CargaCombo()
        {
            FileStream archivo = new FileStream("categorias.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivo);
            string linea = lector.ReadLine();

            while (linea != null)
            {
                string[] datos = linea.Split('|');
                cmbCategoria.Items.Add(datos[0]);
                linea = lector.ReadLine();
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
            fileHelper.AltaRegistroOrdenado(registro);
            this.Mostrar();
            this.Cancelar();
        }
        private void Modificacion()
        {
            if (!this.CamposValidos()) return;
            string registro = txtLegajo.Text + "|" + cmbCategoria.SelectedItem.ToString() + "|" + txtApellido.Text + "|" + txtNombre.Text;
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


        #region Metodos de Reporte
        private void MostrarReporte()
        {
            FileStream reporte = new FileStream("reporteEmpleados.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(reporte);
            MessageBox.Show(lector.ReadToEnd(), "Reporte de Importes a cobrar");
            lector.Close();
            reporte.Close();
        }
        private string ConsultarValorCategoria(string categoria)
        {
            FileStream archivoALeer = new FileStream("categorias.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivoALeer);

            string linea = lector.ReadLine();
            string[] datos;
            while (linea != null)
            {
                datos = linea.Split('|');
                if (datos[0] == categoria)
                {
                    lector.Close();
                    archivoALeer.Close();
                    return datos[1];
                }
                linea = lector.ReadLine();
            }
            lector.Close();
            archivoALeer.Close();
            return null;
        }
        private void GenerarReporte()
        {
            FileStream archivoEmpleado = new FileStream("empleados.txt", FileMode.OpenOrCreate);
            StreamReader lectorEmpleado = new StreamReader(archivoEmpleado);
            FileStream reporte = new FileStream("reporteEmpleados.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(reporte);
            FileStream archivoHoras = new FileStream("horas.txt", FileMode.OpenOrCreate);
            StreamReader lectorHoras = new StreamReader(archivoHoras);

            string lineaEmpleado = lectorEmpleado.ReadLine();
            string[] datosEmpleado = new string[4];
            string lineaHoras = lectorHoras.ReadLine();
            string[] datosHoras = new string[3];
            string empleadoActual, diaActual;
            int horasDia = 0, horasTotales = 0;

            while (lineaEmpleado != null)
            {
                datosEmpleado = lineaEmpleado.Split('|');
                empleadoActual = datosEmpleado[0];
                escritor.WriteLine("EMPLEADO: " + datosEmpleado[3] + " " + datosEmpleado[2] + ", legajo: " + empleadoActual);
                while (lineaEmpleado != null && datosEmpleado[0] == empleadoActual)
                {
                    int valorHora = int.Parse(this.ConsultarValorCategoria(datosEmpleado[1]));
                    horasTotales = 0;
                    if (lineaHoras != null) datosHoras = lineaHoras.Split('|');
                    //mismo legajo 
                    while (lineaEmpleado != null && lineaHoras != null && datosEmpleado[0] == empleadoActual && datosHoras[0] == datosEmpleado[0])
                    {
                        diaActual = datosHoras[1];
                        escritor.WriteLine("   DIA: " + diaActual);
                        horasDia = 0;
                        //mismo dia y legajo
                        while (lineaEmpleado != null && lineaHoras != null && datosEmpleado[0] == empleadoActual && datosHoras[0] == datosEmpleado[0] && diaActual == datosHoras[1])
                        {
                            horasDia += int.Parse(datosHoras[2]);
                            lineaHoras = lectorHoras.ReadLine();
                            if (lineaHoras != null) datosHoras = lineaHoras.Split('|');
                        }
                        horasTotales += horasDia;
                        escritor.WriteLine("        horas trabajadas en el dia: " + horasDia);
                    }
                    escritor.WriteLine("   HORAS TOTALES: " + horasTotales);
                    escritor.WriteLine("         IMPORTE A COBRAR: " + horasTotales * valorHora);
                    escritor.WriteLine();
                    lineaEmpleado = lectorEmpleado.ReadLine();
                    if (lineaEmpleado != null) datosEmpleado = lineaEmpleado.Split('|');
                }
            }
            lectorHoras.Close();
            archivoHoras.Close();
            lectorEmpleado.Close();
            archivoEmpleado.Close();
            escritor.Close();
            reporte.Close();
            this.MostrarReporte();
        }
        #endregion


    }
}
