using System;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoSimple: Instruccion
    {
        //primer tipo obtiene contenedor
        //devuelvo un objeto
        private readonly Func<ZContenedor, object> accion;

        public NodoSimple(Func<ZContenedor, object> accion)
        {
            this.accion = accion;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            return accion(e);
        }
    }
}