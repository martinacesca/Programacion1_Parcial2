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
    public partial class frmReporteIndividual : Form
    {
        #region Form
        public frmReporteIndividual()
        {
            InitializeComponent();
            this.CargarListbox();
        }
        #endregion

        #region Metodos Generales
        private void CargarListbox()
        {
            FileStream archivoEmpleados = new FileStream("empleados.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(archivoEmpleados);
            string linea;
            linea = lector.ReadLine();
            listBox1.Items.Clear();

            while (linea != null)
            {
                listBox1.Items.Add(linea.Replace("|", " "));
                linea = lector.ReadLine();
            }
            lector.Close();
            archivoEmpleados.Close();
        }
        #endregion


        #region Eventos
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) => this.GenerarReporteIndividual();
        #endregion
    
        #region Metodos de Reporte
        private void MostrarReporte()
        {
            FileStream reporte = new FileStream("reporteIndividual.txt", FileMode.OpenOrCreate);
            StreamReader lector = new StreamReader(reporte);
            txtReporte.Text = lector.ReadToEnd();
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
        private void GenerarReporteIndividual()
        {
            string registroSeleccionado = listBox1.SelectedItem.ToString();
            string[] datosSeleccionados = registroSeleccionado.Split(' ');

            FileStream reporte = new FileStream("reporteIndividual.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(reporte);
            FileStream archivoHoras = new FileStream("horas.txt", FileMode.OpenOrCreate);
            StreamReader lectorHoras = new StreamReader(archivoHoras);

            string lineaHoras = lectorHoras.ReadLine();
            string[] datosHoras = new string[3];
            string diaActual;
            int horasDia = 0, horasTotales = 0;

            int valorHora = int.Parse(this.ConsultarValorCategoria(datosSeleccionados[1]));
            escritor.WriteLine("EMPLEADO: " + datosSeleccionados[3] + " " + datosSeleccionados[2] + ", legajo: " + datosSeleccionados[0]);
            escritor.WriteLine();
            while (lineaHoras != null)
            {
                if (lineaHoras != null) datosHoras = lineaHoras.Split('|');
                //mismo legajo 
                while (lineaHoras != null && datosHoras[0] == datosSeleccionados[0])
                {
                    diaActual = datosHoras[1];
                    escritor.WriteLine("   DIA: " + diaActual);
                    horasDia = 0;
                    //mismo dia y legajo
                    while (lineaHoras != null && datosHoras[0] == datosSeleccionados[0] && diaActual == datosHoras[1])
                    {
                        horasDia += int.Parse(datosHoras[2]);
                        lineaHoras = lectorHoras.ReadLine();
                        if (lineaHoras != null) datosHoras = lineaHoras.Split('|');
                    }
                    horasTotales += horasDia;
                    escritor.WriteLine("        horas trabajadas en el dia: " + horasDia);
                }
                lineaHoras = lectorHoras.ReadLine();
            }
            escritor.WriteLine("   HORAS TOTALES: " + horasTotales);
            escritor.WriteLine("         IMPORTE A COBRAR: " + horasTotales * valorHora);

            lectorHoras.Close();
            archivoHoras.Close();
            escritor.Close();
            reporte.Close();
            this.MostrarReporte();
        }
        #endregion







    }
}
