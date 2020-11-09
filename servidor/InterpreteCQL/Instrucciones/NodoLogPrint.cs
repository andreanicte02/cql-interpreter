using System;
using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL
{
    public class NodoLogPrint:Instruccion
    {
        private Instruccion exp;

        public NodoLogPrint(Instruccion exp)
        {
            this.exp = exp;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo = exp.ejecutar(e);
            ZContenedor valor = Utilidades.desenvolver(algo);
            Console.WriteLine(valor.stringBonito());
            return null;
        }
    }
}