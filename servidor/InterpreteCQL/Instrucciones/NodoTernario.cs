using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresionn
{
    public class NodoTernario:Instruccion
    {
        private Instruccion condicion;
        private Instruccion valor1;
        private Instruccion valor2;

        public NodoTernario(Instruccion condicion, Instruccion valor1, Instruccion valor2)
        {
            this.condicion = condicion;
            this.valor1 = valor1;
            this.valor2 = valor2;
        }
        public override object ejecutarSinposicion(ZContenedor e)
        {
            ZBool cond = Utilidades.evaluarCondicion(condicion, e);

            return cond.obtenerValor()
                ? Utilidades.desenvolver(valor1.ejecutar(e))
                : Utilidades.desenvolver(valor2.ejecutar(e));

        }
    }
}