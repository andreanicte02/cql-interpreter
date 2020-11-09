using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesLogicas
{
    public class NodoAnd:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoAnd(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);
            var algo2 = i2.ejecutar(e);

            ZContenedor valor1 = Utilidades.desenvolver(algo1);
            ZContenedor valor2 = Utilidades.desenvolver(algo2);

            if (valor1 is ZBool n1 && valor2 is ZBool n2)
            {
                return new ZBool(n1.obtenerValor() && n2.obtenerValor());
            }

            throw new SemanticError("Error; operacion && no compatible con tipos");


        }
    }
}