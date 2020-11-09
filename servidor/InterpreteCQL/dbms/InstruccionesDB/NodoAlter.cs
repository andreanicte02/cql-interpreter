using System.Collections.Generic;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    
    public class NodoTruncate : Instruccion
    {
        private string id;

        public NodoTruncate(string id)
        {
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
         
            Tabla tab = Dbms.getBd().getTabla(id);
            tab.truncate();

            return null;
        }
    }


    public class NodoAlterDrop:Instruccion
    {
        private string nombreTabla;
        private List<NodoBuscarId> listaId;

        public NodoAlterDrop(string nombreTabla, List<NodoBuscarId> listaId)
        {
            this.nombreTabla = nombreTabla;
            this.listaId = listaId;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
           
           

            Tabla tab = Dbms.getBd().getTabla(nombreTabla);
            tab.alterDrop(listaId);

            return null;

        }
    }

    public class NodoAlterAdd:Instruccion
    {
        private string nombreTabla;
        private List<NodoDeclararEncabezados> encabezados;

        public NodoAlterAdd(string nombreTabla, List<NodoDeclararEncabezados> encabezados)
        {
            this.nombreTabla = nombreTabla;
            this.encabezados = encabezados;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
           
            Tabla tab = Dbms.getBd().getTabla(nombreTabla);
            tab.alterAdd(encabezados);
            return null;
        }
    }
}