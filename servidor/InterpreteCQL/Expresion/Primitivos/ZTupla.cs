using System.Collections.Generic;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZTupla: ZContenedor
    {
        public List<ZContenedor> argumentos;
        

        public ZTupla(List<ZContenedor> argumentos) : base(null,null)
        {
            this.argumentos = argumentos;
        }

       

    }
}