using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoCrearParametro:Instruccion
    {
        private Instruccion tipo;
        private string name;
        private string inON;

        public NodoCrearParametro(Instruccion tipo, string name, string entrada)
        {
            this.tipo = tipo;
            this.name = name;
            this.inON = entrada;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            return new NodoDeclararParametro(tipo, name);
        }

        public bool isIn()
        {
            if (inON.Equals("in"))
            {
                return true;
            }

            return false;
        }
    }
}