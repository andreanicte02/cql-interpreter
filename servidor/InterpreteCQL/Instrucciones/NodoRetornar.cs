using System;
using System.Collections.Generic;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoRetornar:Instruccion
    {
        private List<Instruccion> listaExp;

        public NodoRetornar(List<Instruccion> listaExp)
        {
            this.listaExp = listaExp;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            
            if (e.enFuncion)
            {
                return enFuncion(e);
            }
            
            if (e.enProcedimiento)
            {
                return enProcedimiento(e);
            }
            

            return null;

        }
        
        public Retorno enFuncion(ZContenedor e ){
            
            if (listaExp.Count == 1 && listaExp[0] != null)
            {
                var algo = listaExp[0].ejecutar(e); 
                ZContenedor valor = Utilidades.desenvolver(algo);
                Retorno r = new Retorno(valor.Origen, valor);
                return r;
                    
                    
            }
            throw new SemanticError("se encuentra en una funcion y esta retornando mas de 1 expresion o ninguna");
        }

        public RetornoProc enProcedimiento(ZContenedor e)
        {
            NodoCrearTupla aux = new NodoCrearTupla(listaExp);
            ZTupla tupla = (ZTupla)aux.ejecutar(e);
            RetornoProc ret = new RetornoProc(tupla);
            return ret;
        }

    }
    
  
}