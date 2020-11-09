using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL
{
    public class NodoInstanciaConValores:Instruccion
    {
        private Instruccion tupla;
        private Instruccion tipo;

        public NodoInstanciaConValores(Instruccion tupla, Instruccion tipo)
        {
            this.tupla = tupla;
            this.tipo = tipo;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU @struct =  (TeDeU)tipo.ejecutar(e); 
            ZTupla algo = (ZTupla) tupla.ejecutar(e);

           
            ZInstancia zinstancia = @struct.crearInstancia(e);
            @struct.asignarValores(algo.argumentos,zinstancia);
            return zinstancia;
            
            
        }
    }
}