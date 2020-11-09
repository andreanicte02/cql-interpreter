using System;
using servidor.InterpreteCQL.dbms;

namespace servidor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            
            /*
            Dbms.Usuarios.Add("admin","123");
            Dbms.usSeleccionado = "admin";
            double a = 0;
            Console.WriteLine("hola mundo");
            Console.WriteLine(a+=1);
            Console.WriteLine(a);
            Console.WriteLine("---------------");
            */
            Prueba p = new Prueba();
            //p.probarCQL();
            
            p.probarChsion();
            
            
            
            
        }
    }
}