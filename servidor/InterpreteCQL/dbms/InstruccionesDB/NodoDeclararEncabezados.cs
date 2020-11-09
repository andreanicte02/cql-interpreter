using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    public class NodoDeclararEncabezados:Instruccion
    {
        private string nombre;

        public string Nombre
        {
            get => nombre;
            set => nombre = value;
        }

        private Instruccion tipo ;

        public Instruccion Tipo
        {
            get => tipo;
            set => tipo = value;
        }


        public NodoDeclararEncabezados(string nombre, Instruccion tipo)
        {
            this.nombre = nombre;
            this.tipo = tipo;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

            TeDeU type = (TeDeU) this.tipo.ejecutar(e);
            

            return type;

        }
    }
}