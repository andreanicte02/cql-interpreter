using System.Collections.Generic;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    public class NodoUpdate:Instruccion
    {
        private Instruccion tabla;
        private List<Instruccion> asignaciones;
        private Instruccion where;

        public NodoUpdate(Instruccion tabla, List<Instruccion> asignaciones, Instruccion @where)
        {
            this.tabla = tabla;
            this.asignaciones = asignaciones;
            this.@where = @where;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
  
            Tabla tab = (Tabla) tabla.ejecutar(e);
            List<ZFila> zfilas = tab.Filas;

            foreach (ZFila fila in zfilas)
            {
                bool bandera = false;
                
                if (@where != null)
                {
                    var algo = @where.ejecutar(fila);
                    ZContenedor valor = Utilidades.desenvolver(algo);
                    bandera = NodoSelect.ejecutarWhere(valor);
                }
                
                               
                
                //con where
                if (bandera  && @where != null)
                {
                    Utilidades.ejecutarSentencias(asignaciones, fila);
                    continue;
                }
                
                //sin where
                if (!bandera && @where == null)
                {
                    Utilidades.ejecutarSentencias(asignaciones, fila);
                }


            }
            


            return null;
        }
    }
}