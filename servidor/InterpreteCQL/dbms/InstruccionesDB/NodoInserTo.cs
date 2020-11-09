using System.Collections.Generic;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    public class NodoInserToSimple:Instruccion
    {
        private List<Instruccion> argumentos;
        private string nombreTabla;

        public NodoInserToSimple( string nombreTabla, List<Instruccion> argumentos)
        {
            this.argumentos = argumentos;
            this.nombreTabla = nombreTabla;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

            
            //se obtiene la tabla
            //se desenvuelven los argumentos
            //se llama al metodo 'insertar normal'
            Tabla tab = Dbms.getBd().getTabla(nombreTabla);
            List<ZContenedor> args = Utilidades.desnvolverArgumento(argumentos, e);
            tab.insertNormal(args);
            
            return null;
        }
    }

    public class NodoInsertEspecial:Instruccion
    {
        private string nombre;
        private List<NodoBuscarId> nombreCampos;
        private List<Instruccion> argumentos;

        public NodoInsertEspecial(string nombre, List<NodoBuscarId> nombreCampos, List<Instruccion> argumentos)
        {
            this.nombre = nombre;
            this.nombreCampos = nombreCampos;
            this.argumentos = argumentos;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {

          
            if (argumentos.Count != nombreCampos.Count)
            {
                throw new SemanticError("la instruccion insert to, la cantidad de argumentos no correspondne a los encabezados de la tabla");

            }

            List<ZContenedor> args = Utilidades.desnvolverArgumento(argumentos, e);
            Tabla tab = Dbms.getBd().getTabla(nombre);
            tab.insertEspecial(args, nombreCampos);
            

            return null;

        }
    }
}