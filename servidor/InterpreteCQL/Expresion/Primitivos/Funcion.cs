using System;
using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class Funcion
    {
        private TeDeU tipoRetorno;
        private List<NodoDeclararParametro> nodosDeclararParametro;
        private List<Instruccion> listaInstrucciones;
        private ZContenedor ambitoCapturado;


        public Funcion(TeDeU tipoRetorno, List<NodoDeclararParametro > nodosDeclararParametro, List<Instruccion> listaInstrucciones, ZContenedor ambitoCapturado)
        {
            this.tipoRetorno = tipoRetorno;
            this.nodosDeclararParametro = nodosDeclararParametro;
            this.listaInstrucciones = listaInstrucciones;
            this.ambitoCapturado = ambitoCapturado;
           
            //se valida que el ambito capturado sea una funcion
        }

      

        /// <summary>
        /// esta funcion verifica si no hay una funcion con la misma firma, de lo contrario se agrega al entrono
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public bool mismFirma(Funcion funcion)
        {
            if (cantParametros() != funcion.cantParametros())
            {
                return false;
            }

            for (int x = 0; x < cantParametros(); x++) 
            {
                TeDeU tipo1 = (TeDeU) nodosDeclararParametro[x].Tipo.ejecutar(ambitoCapturado);
                TeDeU tipo2 = (TeDeU)funcion.nodosDeclararParametro[x].Tipo.ejecutar(ambitoCapturado);

                if (tipo1 != tipo2)
                {
                    return false;
                }
                
            }
            //son iguales 
            return true;
        }

        /// <summary>
        /// esta funcion verifia que la funcion invocada contenga los mismos argumentos
        /// </summary>
        /// <param name="argumentos"></param>
        /// <returns>bool</returns>
        public bool mismFirma(List<ZContenedor> argumentos)
        {
            if (cantParametros() != argumentos.Count)
            {
                return false;
            }
            
            for (int x = 0; x < argumentos.Count; x++)
            {
                TeDeU tipo1 = (TeDeU) nodosDeclararParametro[x].Tipo.ejecutar(ambitoCapturado);
                TeDeU tipo2 = argumentos[x].Origen;

                if (tipo1 != tipo2 && tipo2!= TiposPrimitivos.tipoNulo)
                {
                    return false;
                }

            }
            //son iguales
            return true;


        }

        private int cantParametros()
        {
            return nodosDeclararParametro.Count;
        }

        public virtual ZContenedor ejecutarFuncion(List<ZContenedor> argumentos)
        {
            ZContenedor ambitoFuncion = new ZContenedor(ambitoCapturado, null);
            ambitoFuncion.enFuncion = true;
            
            //los argumentos ya estan desembueltos
            for (int i = 0; i < nodosDeclararParametro.Count; i++)
            {
                Simbolo sim = (Simbolo)nodosDeclararParametro[i].ejecutarSinposicion(ambitoFuncion);
                sim.definirValor(argumentos[i]);
            }

            
            var result = Utilidades.ejecutarSentencias(listaInstrucciones, ambitoFuncion);

            //las funciones siempre tienn que retornar algo
            if (!(result is Retorno r))
            {
                throw new SemanticError("Error la funcion no devuelve nignun valor");
            }

            if (tipoRetorno == r.Tipo || r.Tipo == TiposPrimitivos.tipoNulo)
            {
                    
                    return r.Valor; //valor es un Z o sea un z valor
                    
            }
            throw new SemanticError("Error la funcion, no es del mismo tipo con el que se retorna");

        }
            
           
        

        public Object asd()
        {
            int f = 2;
            return  5;
        }

        //verificar firma
    }
    
    
    
    
    
 
}