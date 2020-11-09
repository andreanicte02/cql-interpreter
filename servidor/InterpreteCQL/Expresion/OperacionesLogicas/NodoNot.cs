using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesLogicas
{
    public class NodoNot:Instruccion
    {
        private readonly Instruccion i1;

        public NodoNot(Instruccion i1)
        {
            this.i1 = i1;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);
            

            ZContenedor valor1 = Utilidades.desenvolver(algo1); 

            if (valor1 is ZBool n1)
            {
                return new ZBool(!n1.obtenerValor());
            }

            throw new SemanticError("Error; operacion ! no compatible con tipos");

        }
    }
}