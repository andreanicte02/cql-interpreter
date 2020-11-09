using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoCrearBD:Instruccion
    {
        private string nombre;


        public NodoCrearBD(string nombre)
        {
            this.nombre = nombre;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            BaseDeDatos bd = Dbms.crearBDChison(nombre);
            Dbms.seleccionarBaseDeDatos(nombre);
            return bd;
        }
    }
}