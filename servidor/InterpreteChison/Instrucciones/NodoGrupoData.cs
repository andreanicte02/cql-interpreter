using System;
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instrucciones.Sentencias;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoGrupoData:Instruccion
    
    
    {
        private List<Instruccion> data;//---------> < lista de atrr >
        private BaseDeDatos bd;

        public NodoGrupoData(List<Instruccion> data)
        {
            this.data = data;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            bd = Dbms.BdSeleccionada;
            if (!data.Exists(v => v is NodoObtenerTipo))
            {
                throw new SemanticError(" no se definio el atrbuto tipo");
            }
            

            var nTipo = data.First(v => v is NodoObtenerTipo);
            string tipo =(string) nTipo.ejecutar(e);

            if (tipo.Equals("object"))
            {
                return crearObjeto(e);
            }

            if (tipo.Equals("table"))
            {
                return crearTabla(e);
            }

            if (tipo.Equals("procedure"))
            {
                return crearProc(e);
            }

            return null;
        }

  
        public object crearObjeto(ZContenedor e)
        {
            if (!data.Exists(v => v is NodoCrearNombre) ||  !data.Exists(v => v is NodoAtrr))
            {
                throw new SemanticError(" no se definio el atrbuto tipo o los parametros del objeto");
            }
            
            string nombre = (string) data.First(v => v is NodoCrearNombre).ejecutar(e);
            NodoAtrr nAtrr = (NodoAtrr) data.First(v => v is NodoAtrr);
            nAtrr.definirNombre(nombre);
           
            
            return nAtrr.ejecutar(e);
        }

        public Object crearTabla(ZContenedor e)
        {

            if (!data.Exists(v => v is NodoCrearNombre) || !data.Exists(v => v is NodoColumns))
            {
                throw new SemanticError(" no se definio el atrbuto tipo o los parametros del objeto");
            }
            string nombre = (string) data.First(v => v is NodoCrearNombre).ejecutar(e);
            NodoColumns nC = (NodoColumns) data.First(v => v is NodoColumns);
            nC.definirTabla(nombre);
            nC.ejecutar(e);
            Tabla tab = Dbms.getBd().getTabla(nombre);
            

            if (!data.Exists(v => v is NodoData))
            {
                throw new SemanticError("no existe este el atributo data");
                
            }
            NodoData nD = (NodoData) data.First(v => v is NodoData);
            List<NodoFila> nodoFilas = nD.Filas;

            foreach (NodoFila nodo in nodoFilas)
            {
                ZFila entorno = tab.crearFila();
                nodo.ejecutar(entorno);
                tab.addFila(entorno);
                

            }
            
            
            //-------------------------------------------------------------

            return null;
        }

        public object crearProc(ZContenedor e)
        {
            if (!data.Exists(v => v is NodoCrearNombre) ||  !data.Exists(v => v is NodoParametro)
                || !data.Exists(v=>v is NodoCrearInstruProc))
            {
                throw new SemanticError(" no se definio el atrbuto tipo o los parametros del procedimiento");
            }


            string nombre = (string) data.First(v => v is NodoCrearNombre).ejecutar(e);
            List<NodoCrearParametro> list =(List<NodoCrearParametro>) data.First(v => v is NodoParametro).ejecutar(e);

            List<NodoDeclararParametro> lEntrada = new List<NodoDeclararParametro>();
            List<NodoDeclararParametro> lSalida = new List<NodoDeclararParametro>();

            foreach (NodoCrearParametro nodo in list)
            {
                if (nodo.isIn())
                {
                    lEntrada.Add((NodoDeclararParametro)nodo.ejecutar(e));
                }
                else
                {
                    lSalida.Add((NodoDeclararParametro)nodo.ejecutar(e));
                }
                
            }

            List<Instruccion> lis = (List<Instruccion>)data.First(v => v is NodoCrearInstruProc).ejecutar(e);


            NodoDeclararProce nProc = new NodoDeclararProce(nombre,lEntrada, lSalida, lis);
           
            
            return  nProc.ejecutar(e);
        }
        
    }
}