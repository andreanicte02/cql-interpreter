using System;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoAccesoPunto:Instruccion
    {
        private Instruccion exp;
        private string id;

        public NodoAccesoPunto(Instruccion exp, string id)
        {
            this.exp = exp;
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            Object algo = exp.ejecutar(e);
            ZContenedor zAlgo = Utilidades.desenvolver(algo);
            Simbolo sim = acPunto(zAlgo, id);
            return sim;
        }

        public Simbolo acPunto(ZContenedor Zalgo, string id)
        {
            if (Zalgo.exsiteVariableActual(id))
            {
                return  Zalgo.getVariableActual(id);
            }

            throw new SemanticError("no posee el valor "+ id);
        }
    }
}