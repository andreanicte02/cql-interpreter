using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison.Utils
{
    public class Utilidades
    {
        public static object ejecutarSenteciass(List<Instruccion> lista, ZContenedor f)
        {
            foreach (Instruccion ins in lista)
            {
                ins.ejecutar(f);
            }

            return null;

        }
        
    }
    
}