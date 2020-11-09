using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    public class NodoBuscarTabla:Instruccion
    {
        private string id;

        public NodoBuscarTabla(string id)
        {
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
          
            return Dbms.getBd().getTabla(id);
        }
    }
}