using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoDeclararStruct:Instruccion
    {
        private readonly string id;
        private List<NodoDeclararParametro> listaAtributos;

        public NodoDeclararStruct(string id, List<NodoDeclararParametro> listaAtributos)
        {
            this.id = id;
            this.listaAtributos = listaAtributos;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU nuevoTeDeU = new TeDeU(id);
            nuevoTeDeU.definirListaDeclaraciones(listaAtributos);
            e.setTeDeU(id,nuevoTeDeU);
            
            
            //se declara el TeDeU
            return null;
        }

        


    }
    
}