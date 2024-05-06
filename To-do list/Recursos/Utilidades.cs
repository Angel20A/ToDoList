using System.Security.Cryptography; 
using System.Text;

namespace To_do_list.Recursos
{
    public class Utilidades
    {
        //Método para encriptar la clave
        public static string EncriptarClave(string clave)
        {
            StringBuilder sb = new StringBuilder(); 

            using (SHA256 hash  = SHA256Managed.Create()) //hash nos permite encriptar
            {
                Encoding enc = Encoding.UTF8; //codificación

                byte[] result = hash.ComputeHash(enc.GetBytes(clave)); 

                foreach(byte b in result) //iterar los elementos que se encuentran en el result
                {
                    sb.Append(b.ToString("x2")); //concatenar el resultado. x2 nos indica que el formato se debe formatear en hexadecimal
                }
            }
            return sb.ToString(); //devolver variable sb en string
        }
    }
}
