using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public delegate ZContenedor funcionNativaDelegado(List<ZContenedor> arg); //definir tipo de posibles funciones
    //suma de tipo + entorno

    public class FuncionNativa:Funcion
    {
        private readonly funcionNativaDelegado _f;

        //
        public FuncionNativa(TeDeU tipoRetorno, List<NodoDeclararParametro> nodosDeclararParametro, ZContenedor ambitoCapturado,funcionNativaDelegado f) : base(tipoRetorno, nodosDeclararParametro, new List<Instruccion>(), ambitoCapturado)
        {
            _f = f;
        }

        public override ZContenedor ejecutarFuncion(List<ZContenedor> argumentos)
        {

            return _f(argumentos);

        }
    }
}