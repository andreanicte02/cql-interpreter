using System;
using System.Collections.Generic;
using System.Net.Mail;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;


namespace servidor.InterpreteCQL
{
    public class ZContenedor
    {
        public ZContenedor anterior; 
        private Dictionary<string, Simbolo> tablaSimbolo =new Dictionary<string, Simbolo>();
        //private Dictionary<string, TeDeU> tdeus = new Dictionary<string, TeDeU>();
        private Dictionary<string, AgenteFuncion> agentesFuncion = new Dictionary<string, AgenteFuncion>();
      
        private TeDeU origen;
        public bool enFuncion= false;
        public bool enProcedimiento = false;
        
        public Dictionary<string, Simbolo> TablaSimbolo
        {
            get => tablaSimbolo;
            
        }

        
        
        
        
        public TeDeU Origen
        {
            get => origen;
            
        }

        public ZContenedor(ZContenedor anterior, TeDeU origen) {
            this.anterior = anterior;
            this.origen = origen;
            
        }

        public void setearOrigin(TeDeU origen2)
        {
            this.origen = origen2;
        }
        
        
        
        public bool exsiteVariableActual(string id)
        {
            return tablaSimbolo.ContainsKey(id);
        }

        public Simbolo getVariableActual(string id)
        {
            return tablaSimbolo[id];
            
        }

        public Simbolo getVariable(string id)
        {
            for (ZContenedor e = this; e != null; e = e.anterior)
            {
                if (e.tablaSimbolo.ContainsKey(id))
                {
                    return e.tablaSimbolo[id];
                }
            }

            throw new SemanticError("el id no existe " + id);
        }

        public void setVariable(string id, Simbolo s)
        {
            if (tablaSimbolo.ContainsKey(id))
            {
                throw new SemanticError("el id ya existe "+ id);
            }
            tablaSimbolo.Add(id,s);
        }

        public TeDeU getTeDeU(string id)
        {
            return !Dbms.isSelect() ? Dbms.getPrimitivos(id) : Dbms.getBd().getTedeu(id);
        }

        public void setTeDeU(string id, TeDeU t)
        {
            //solo se pueden crear tipos cuando se estan usando base de datos?
            if (!Dbms.isSelect())
            {
                Dbms.tedeusGlobales.Add(id,t);
                return;
            }
            
            Dbms.getBd().setTedeu(id,t);
        }
        
        //existe actual

        public virtual string stringBonito()
        {
            return "alv";
        }


        public void declararFuncion(string id, Funcion funcion)
        {
            AgenteFuncion agenteFuncion;
            
            if (agentesFuncion.ContainsKey(id))
            {
                agenteFuncion = agentesFuncion[id];
            }
            else
            {
                agenteFuncion = new AgenteFuncion();
                agentesFuncion.Add(id, agenteFuncion);
            }
            
            agenteFuncion.agregarFuncion(funcion);
            
        }

        public AgenteFuncion getAgenteActual(string id)
        {
            //solo busca en uno
            if (agentesFuncion.ContainsKey(id))
            {
                return agentesFuncion[id];
            }

            throw new SemanticError("la funcion no existe "+ id);
        }

        public AgenteFuncion getAgente(string id)
        {
            for (ZContenedor e = this; e != null; e = e.anterior)
            {
                if (e.agentesFuncion.ContainsKey(id))
                {
                    return e.agentesFuncion[id];
                }
                
            }

            throw new SemanticError("la funcion no existe " +id);
        }

        

        public void eliminarTeDeU(string id)
        {
            if (!Dbms.isSelect())
            {
                Dbms.tedeusGlobales.Remove(id);
                return;
            }
            
            Dbms.getBd().eliminarTedeu(id);

        }

        public void eliminarEncabezado(string id)
        {
            if (!tablaSimbolo.ContainsKey(id))
            {
                throw new SemanticError($"no existe la columna '{id}'");

            }
            tablaSimbolo.Remove(id);
            
        }

        public List<ZContenedor> obtenerValorFila()
        {

            List<ZContenedor> valores = new List<ZContenedor>();

            foreach (KeyValuePair<string,Simbolo> result in tablaSimbolo)
            {
             
                valores.Add(result.Value.obtenerValor());
            }
            
            return valores;


        }
        
        
    }
}