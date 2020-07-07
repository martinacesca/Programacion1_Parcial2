using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    public class FileHelper
    {
        #region Variables
        string nombreArchivo;
        #endregion

        #region FileHelper
        public FileHelper(string nombreArchivo)
        {
            this.nombreArchivo = nombreArchivo;
        }
        #endregion

        #region Metodos ABM
        public void AltaRegistroOrdenado(string registro)
        {
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.Open);
            StreamReader lector = new StreamReader(archivo);
            FileStream nuevoarchivo = new FileStream("auxiliar.txt", FileMode.Append);
            StreamWriter escritor = new StreamWriter(nuevoarchivo);

            int idRegistroNuevo = int.Parse(registro.Split('|')[0]);
            string linea = lector.ReadLine();
            bool insertoRegistro = false;
            while (linea != null)
            {
                int idRegistroLeido = int.Parse(linea.Split('|')[0]);
                if (!insertoRegistro && idRegistroLeido > idRegistroNuevo)
                {
                    escritor.WriteLine(registro);
                    insertoRegistro = true;
                }
                escritor.WriteLine(linea);
                linea = lector.ReadLine();
            }
            if (!insertoRegistro)
            {
                escritor.WriteLine(registro);
            }
            lector.Close();
            escritor.Close();
            archivo.Close();
            nuevoarchivo.Close();
            System.IO.File.Delete(this.nombreArchivo);
            System.IO.File.Move("auxiliar.txt", this.nombreArchivo);
        }

        public void AltaRegistro(string registro)
        {
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.Append);
            StreamWriter escritor = new StreamWriter(archivo);
            escritor.WriteLine(registro);
            escritor.Close();
            archivo.Close();
        }
        public void ModificacionRegistro(int numeroRegistro, string registroModificado, bool ordenado)
        {
            this.BajaRegistro(numeroRegistro);
            if (ordenado)
            {
                this.AltaRegistroOrdenado(registroModificado);
            }
            else
            {
                this.AltaRegistro(registroModificado);
            }
        }
        public void BajaRegistro(int numeroRegistro)
        {
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.Open);
            StreamReader lector = new StreamReader(archivo);
            FileStream nuevoarchivo = new FileStream("auxiliar.txt", FileMode.Append);
            StreamWriter escritor = new StreamWriter(nuevoarchivo);
            string linea = lector.ReadLine();

            int contador = 0;

            while (linea != null)
            {
                if (numeroRegistro != contador)
                {
                    escritor.WriteLine(linea);
                }
                contador++;
                linea = lector.ReadLine();
            }

            lector.Close();
            escritor.Close();
            archivo.Close();
            nuevoarchivo.Close();

            System.IO.File.Delete(this.nombreArchivo);
            System.IO.File.Move("auxiliar.txt", this.nombreArchivo);
        }
        #endregion






        //private string Consultar(string archivo, string valor)
        //{
        //    FileStream archivoALeer = new FileStream(archivo, FileMode.OpenOrCreate);
        //    StreamReader lector = new StreamReader(archivoALeer);

        //    string linea = lector.ReadLine();
        //    string[] datos;
        //    if (linea != null) datos = linea.Split('|');
        //    while (linea != null)
        //    {
        //        datos = linea.Split('|');
        //        if (datos[0] == valor)
        //        {
        //            lector.Close();
        //            archivoALeer.Close();
        //            return datos[1];
        //        }
        //        linea = lector.ReadLine();
        //    }
        //    lector.Close();
        //    archivoALeer.Close();
        //    return null;
        //}
        //private void btnReporte_Click(object sender, EventArgs e)
        //{
        //    FileStream archivoALeer = new FileStream("alumnos.txt", FileMode.OpenOrCreate);
        //    StreamReader lector = new StreamReader(archivoALeer);
        //    FileStream reporte = new FileStream("reporte.txt", FileMode.Create);
        //    StreamWriter escritor = new StreamWriter(reporte);

        //    string linea = lector.ReadLine();
        //    string[] datos = linea.Split('|');

        //    string universidadActual, facultadActual, carreraActual;

        //    int cantUni, cantFac, cantCarr;

        //    while (linea != null)
        //    {
        //        cantUni = 0;
        //        universidadActual = datos[0];
        //        escritor.WriteLine(this.Consultar("universidades.txt", datos[0]));
        //        while (linea != null && datos[0] == universidadActual)
        //        {
        //            cantFac = 0;
        //            facultadActual = datos[1];
        //            escritor.WriteLine("   Facultad: " + this.Consultar("facultades.txt", datos[1]));
        //            while (linea != null && datos[0] == universidadActual && datos[1] == facultadActual)
        //            {
        //                cantCarr = 0;
        //                carreraActual = datos[2];
        //                escritor.WriteLine("        Carrera: " + this.Consultar("carreras.txt", datos[2]));
        //                while (linea != null && datos[0] == universidadActual && datos[1] == facultadActual && datos[2] == carreraActual)
        //                {
        //                    cantUni++;
        //                    cantFac++;
        //                    cantCarr++;

        //                    linea = lector.ReadLine();
        //                    if (linea != null) datos = linea.Split('|');
        //                }

        //                escritor.WriteLine("        Carrera Cant: " + cantCarr);
        //            }
        //            escritor.WriteLine("    Facultad Cant: " + cantFac);
        //        }
        //        escritor.WriteLine("Universidad Cant: " + cantUni);
        //    }





        //    lector.Close();
        //    escritor.Close();
        //    archivoALeer.Close();
        //    reporte.Close();









        //    /*
        //         FileStream miArchivo = new FileStream("a1.txt", FileMode.OpenOrCreate); StreamReader lector = new StreamReader(miArchivo); FileStream miArchivo2 = new FileStream("Reporte.txt", FileMode.Create); StreamWriter escritor = new StreamWriter(miArchivo2); string linea, alumnos, facu, carre; string[] registro; int calumnos, cfacu, ccarre;
        //linea = lector.ReadLine(); registro = linea.Split('|');
        //while (linea != null)
        //{
        //    uni = registro[0]; cuni = 0;
        //    escritor.WriteLine(consultar("Universidades.txt", uni)); while (linea != null && uni == registro[0])
        //    {
        //        facu = registro[1]; cfacu = 0; escritor.WriteLine(" " + consultar("Facultades.txt", facu)); while (linea != null && uni == registro[0] && facu == registro[1])
        //        {
        //            carre = registro[2]; ccarre = 0; escritor.Write(" " + consultar("Carreras.txt", carre)); while (linea != null && uni == registro[0] && facu == registro[1] && carre == registro[2])
        //            {
        //                cuni++; cfacu++; ccarre++;
        //                linea = lector.ReadLine(); if (linea != null) { registro = linea.Split('|'); }
        //            }
        //            escritor.WriteLine(" " + ccarre);
        //        }
        //        escritor.WriteLine("Total " + cfacu);
        //    }
        //    escritor.WriteLine("Total de " + uni + " " + cuni); escritor.WriteLine("-------------------------");
        //}
        //lector.Close(); miArchivo.Close(); escritor.Close(); miArchivo2.Close(); msgBoxArchivo("Reporte.txt");



        //     */
        //}



        ////private void msgBoxArchivo(string ruta) { 

        ////        FileStream miArchivo = new FileStream(ruta, FileMode.OpenOrCreate); 
        ////        StreamReader lector = new StreamReader(miArchivo); 
        ////        MessageBox.Show(lector.ReadToEnd()); 
        ////        lector.Close(); miArchivo.Close(); 
        ////    }
        ////private string consultar(string ruta, string valor)
        ////{
        ////    FileStream miArchivo = new FileStream(ruta, FileMode.OpenOrCreate); StreamReader lector = new StreamReader(miArchivo);
        ////    string linea, respuesta;
        ////        string[] registro;
        ////        linea = lector.ReadLine();
        ////        respuesta = null; 
        ////        while (linea != null) 
        ////        { registro = linea.Split('|');
        ////            if (registro[0] == valor) 
        ////            { 
        ////                respuesta = registro[1];
        ////            } 
        ////            linea = lector.ReadLine(); 
        ////        }
        ////    lector.Close();
        ////        miArchivo.Close(); return respuesta;
        ////}



    }
}
