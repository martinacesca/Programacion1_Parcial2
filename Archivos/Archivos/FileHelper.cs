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
            FileStream archivo = new FileStream(this.nombreArchivo, FileMode.OpenOrCreate);
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


    }
}
