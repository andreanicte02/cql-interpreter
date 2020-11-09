using System.Collections.Generic;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones.Sentencias
{
    public class NodoDeclararProce:Instruccion
    {
        private string id;
        private List<NodoDeclararParametro> parametros;
        private List<NodoDeclararParametro> tipoRetorno;
        private List<Instruccion> _instrucciones;

        public NodoDeclararProce(string id, List<NodoDeclararParametro> parametros, List<NodoDeclararParametro> tipoRetorno, List<Instruccion> instrucciones)
        {
            this.id = id;
            this.parametros = parametros;
            this.tipoRetorno = tipoRetorno;
            _instrucciones = instrucciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            BaseDeDatos bd = Dbms.getBd();

            //preguntar si los tipos se mandan desenbueltos de una
            List<TeDeU> listaTedeus = new List<TeDeU>();
            foreach (NodoDeclararParametro nodo in tipoRetorno)
            {
                listaTedeus.Add((TeDeU)nodo.Tipo.ejecutar(e));
                
            } 
            Procedimiento proc = new Procedimiento(parametros,tipoRetorno,listaTedeus,_instrucciones,e);
            bd.decProcedimiento(id,proc);
            return null;
        }
        
    }
}