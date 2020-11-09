using System;
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.dbms
{
    public class BaseDeDatos
    {
        private  string nombre;

      

        private Dictionary<string, Tabla> tablas = new Dictionary<string, Tabla>();
        private Dictionary<string, AgenteProcedimiento> agentesProcedimiento = new Dictionary<string, AgenteProcedimiento>();
        private Dictionary<string, TeDeU> tdeus = new Dictionary<string, TeDeU>();
        

        public Dictionary<string, AgenteProcedimiento> AgentesProcedimiento
        {
            get => agentesProcedimiento;
           
        }

        
        public string Nombre
        {
            get => nombre;
        }
        public Dictionary<string, Tabla> Tablas
        {
            get => tablas;
           
        }
        
        public Dictionary<string, TeDeU> Tdeus
        {
            get => tdeus;
        }

          
        public BaseDeDatos(string nombre)
        {
            this.nombre = nombre;
        }

        public void crearTabla(string id, List<NodoDeclararEncabezados> columnas, ZContenedor e)
        {
            if (tablas.ContainsKey(id))
            {
                throw new SemanticError($"ya existe una tabla con ese nombre '{id}'");
            }
            
            Tabla tab = new Tabla(id,columnas, e);
            tablas.Add(id,tab);
            
        }
        
        public void crearTablaSiNoExiste(string id, List<NodoDeclararEncabezados> columnas, ZContenedor e)
        {
            if (tablas.ContainsKey(id))
            {
                return;
            }
            Tabla tab = new Tabla(id, columnas, e);
           tablas.Add(id, tab);
            
        }
        
        

        public void eliminarTabla(string id)
        {
            if (!tablas.ContainsKey(id))
            {
                throw new SemanticError($"no existe una tabla con ese nombre '{id}''");
            }

            tablas.Remove(id);
        }

     

        public void eliminarTablaSiExiste(string id)
        {
            if (!tablas.ContainsKey(id))
            {
                return;
            }

            tablas.Remove(id);
        }

        public Tabla getTabla(string id)
        {

            if (!tablas.ContainsKey(id))
            {
                throw new SemanticError($"no existe la tabla '{id}' en la bd {nombre}");
            }

            return tablas[id];
        }


        public void decProcedimiento(string id, Procedimiento procedimiento )
        {
            AgenteProcedimiento agenteProc;

            if (agentesProcedimiento.ContainsKey(id))
            {
                agenteProc = agentesProcedimiento[id];
            }
            else
            {
                agenteProc = new AgenteProcedimiento();
                //se agrega al dicionario
                agentesProcedimiento.Add(id,agenteProc);
            }
            agenteProc.agregarProcedimiento(procedimiento);

        }

        public AgenteProcedimiento getProc(string id)
        {
           
                if (agentesProcedimiento.ContainsKey(id))
                {
                    return agentesProcedimiento[id];
                }
                
                throw new SemanticError("el proc no existe " +id);
            
        }

        public AgenteProcedimiento getProcActual(string id)
        {
            if (agentesProcedimiento.ContainsKey(id))
            {
                return agentesProcedimiento[id];
            }

            throw new SemanticError("el procedimiento no existe "+ id);
        }

        public TeDeU getTedeu(string id)
        {
            if (tdeus.ContainsKey(id))
            {
                return tdeus[id];
            }


            return Dbms.getPrimitivos(id);
        }

        public void setTedeu(string id, TeDeU tedeu)
        {
            if (tdeus.ContainsKey(id))
            {
                throw new SemanticError($"ya existe el tedu {id}");
            }
            
            tdeus.Add(id, tedeu);
        }

        public void eliminarTedeu(string id)
        {
            if (tdeus.ContainsKey(id))
            {
                tdeus.Remove(id);
                return;
            }
            
            throw new SemanticError($"el tedeu no existe'{id}'");
        }

     


    }
}