using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoColumns:Instruccion
    {
        private List<NodoDeclararEncabezados> encabezados;
        private string nombreTabla;

        public NodoColumns(List<NodoDeclararEncabezados> encabezados)
        {
            this.encabezados = encabezados;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            NodoSimple nodo = new NodoSimple(a=>Dbms.crearTabla(nombreTabla,encabezados, e));
            return nodo.ejecutar(e);

        }

        public void definirTabla(string nombre)
        {
            nombreTabla = nombre;
        }
    }
}