using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoCrearTupla:Instruccion
    {
        private List<Instruccion> elementosTupla;

        public NodoCrearTupla(List<Instruccion> elementosTupla)
        {
            this.elementosTupla = elementosTupla;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            List<ZContenedor> listaExp= new List<ZContenedor>();

            for (int x = 0; x < elementosTupla.Count; x++)
            {
                var algo = elementosTupla[x].ejecutar(e);
                listaExp.Add(Utilidades.desenvolver(algo));
                
            }

            return new ZTupla(listaExp);
        }
    }
}