using System;
using System.IO;
using servidor.InterpreteChison.Utils;
using servidor.InterpreteCQL;

namespace servidor
{
    public class Prueba
    {
        
        public void probarCQL()
        { 
            //DateTime time = DateTime.Now;
            //Console.WriteLine(time.ToShortDateString());

            //DateTime ti2 = Convert.ToDateTime("2018-08-24");
            //Console.WriteLine(ti2.ToString("yyyy-MM-dd"));

            //DateTime ti3 = Convert.ToDateTime("23:25:59");
            //Console.WriteLine(ti3.ToString("HH:mm:ss"));


            string entrada = File.ReadAllText(@"./../../pruebaEntrada3.txt");
            EjecutarAnalizadorCQL ex = new EjecutarAnalizadorCQL();

            ex.ejecutarAnalizador(entrada);
        }

        public void probarChsion()
        {
            string entrada = File.ReadAllText(@"./../../pruebaChison.txt");
            EjecutarAChison ex = new EjecutarAChison();
            
            ex.ejecutarAnalizador(entrada);
        }
    }
}