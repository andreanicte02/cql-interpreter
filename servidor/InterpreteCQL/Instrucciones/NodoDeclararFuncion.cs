using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoDeclararFuncion: Instruccion
    {
        private readonly Instruccion tipo;
        private  readonly  string id;
        private readonly List<NodoDeclararParametro> parametros;
        readonly List<Instruccion> lInsturcciones;

        public NodoDeclararFuncion(Instruccion tipo, string id, List<NodoDeclararParametro> parametros, List<Instruccion> lInsturcciones)
        {
            this.tipo = tipo;
            this.id = id;
            this.parametros = parametros;
            this.lInsturcciones = lInsturcciones;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU ti = (TeDeU) tipo.ejecutar(e);
            Funcion funcion = new Funcion(ti,parametros,lInsturcciones, e);
            e.declararFuncion(id, funcion);
            return null;
        }
    }
}