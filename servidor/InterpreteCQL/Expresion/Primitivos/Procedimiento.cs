using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class Procedimiento
    {
        private List<TeDeU> tiposRetornos;
        private List<NodoDeclararParametro> nodosDeclararParametros;
        private List<NodoDeclararParametro> nodosRetornos;
        


        private List<Instruccion> listaInstrucciones;
        private ZContenedor ambitoCapturado;
        private string cadena="";

        public string Cadena
        {
            get => cadena;
         
        }


        public List<NodoDeclararParametro> NodosRetornos
        {
            get => nodosRetornos;
            
        }
        
        
        public List<TeDeU> TiposRetornos
        {
            get => tiposRetornos;
          
        }

        public List<NodoDeclararParametro> NodosDeclararParametros
        {
            get => nodosDeclararParametros;
           
        }
        
        
        public Procedimiento( List<NodoDeclararParametro> nodosDeclararParametros,List<NodoDeclararParametro> nodosRetornos,List<TeDeU> tiposRetornos, List<Instruccion> listaInstrucciones, ZContenedor ambitoCapturado)
        {
            this.tiposRetornos = tiposRetornos;
            this.nodosDeclararParametros = nodosDeclararParametros;
            this.nodosRetornos = nodosRetornos;
            this.listaInstrucciones = listaInstrucciones;
            this.ambitoCapturado = ambitoCapturado;
        }

        /// <summary>
        /// vericia si no hay procedimiento con la misma firma de lo contrario se agrega; declaracion de procedimientos 
        /// </summary>
        /// <param name="procedimiento"></param>
        /// <returns></returns>
        public bool mismaFirma(Procedimiento procedimiento)
        {
            if (cantPa() != procedimiento.cantPa())
            {
                return false;
            }

            for (int x = 0; x < cantPa(); x++)
            {
                TeDeU tip1 = (TeDeU) nodosDeclararParametros[x].Tipo.ejecutar(ambitoCapturado);
                TeDeU tip2 = (TeDeU) procedimiento.nodosDeclararParametros[x].Tipo.ejecutar(ambitoCapturado);
                if (tip1 != tip2)
                {
                    return false;
                }
                
            }
            return true;
            
                
        }
        
        /// <summary>
        /// Se encarga, de que el procedimiento invocado, haga match con alguno de los ya declarados
        /// </summary>
        /// <param name="argumentos"></param>
        /// <returns></returns>
        public bool mismFirma(List<ZContenedor> argumentos)
        {
            if (cantPa() != argumentos.Count)
            {
                return false;
            }

            for (int x = 0; x < argumentos.Count; x++)
            {
                TeDeU tipo1 = (TeDeU) nodosDeclararParametros[x].Tipo.ejecutar(ambitoCapturado);
                TeDeU tipo2 = argumentos[x].Origen;
                if (tipo1 != tipo2 && tipo2 != TiposPrimitivos.tipoNulo)
                {
                    return false;
                }

            }
            //son iguales
            return true;
            
        }
        public int cantPa()
        {
            return nodosDeclararParametros.Count;
        }

        public ZTupla ejecutarProcedimiento(List<ZContenedor> argumentos)
        {
            ZContenedor ambitoProcedimiento = new ZContenedor(ambitoCapturado, null);
            ambitoProcedimiento.enProcedimiento = true;

            //los argumentos ya estan desembuetlos
            for (int i = 0; i < cantPa(); i++)
            {
                Simbolo sim = (Simbolo) nodosDeclararParametros[i].ejecutar(ambitoProcedimiento);
                sim.definirValor(argumentos[i]);
            }

            var result = Utilidades.ejecutarSentencias(listaInstrucciones, ambitoProcedimiento);

            if (result is RetornoProc r)
            {
                if (r.tupla.argumentos.Count != tiposRetornos.Count)
                {
                    throw new SemanticError("la cantidad de expresiones que se retornan en el procedimiento no es la misma que sea han declarado");
                }

                for (int x = 0; x < tiposRetornos.Count;x++) {

                    if (r.tupla.argumentos[x].Origen != tiposRetornos[x] && r.tupla.argumentos[x].Origen != TiposPrimitivos.tipoNulo)
                    {
                        throw new SemanticError("el tipo de exp que se estan retornando no son los indiciados");
                    }
                    
                }

                return r.tupla;

            }
           
            return null;
        }

        public void asignarCadena(string cadena)
        {
            this.cadena = cadena;
        }

    }
}