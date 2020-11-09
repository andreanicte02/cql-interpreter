using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    //
    public class TeDeU
    {
        private readonly string nombre;

        public string Nombre => nombre;

        private List<NodoDeclararParametro> listaDelcaraciones= new List<NodoDeclararParametro>();

        public List<NodoDeclararParametro> ListaDelcaraciones
        {
            get => listaDelcaraciones;
            set => listaDelcaraciones = value;
        }

        public bool isCount = false;

        public TeDeU(string nombre)
        {
            this.nombre = nombre;
        }

        public void definirListaDeclaraciones(List<NodoDeclararParametro> listaDelcaraciones)
        {
            this.listaDelcaraciones = listaDelcaraciones;
        }

        //crea contenedores

        public ZInstancia crearInstancia(ZContenedor anterior)
        {
           ZInstancia zInstanica  = new ZInstancia(anterior, this);

           foreach (NodoDeclararParametro d in listaDelcaraciones)
           {
               d.ejecutar(zInstanica);
           }

           return zInstanica;

        }
        

        public void asignarValores(List<ZContenedor> args, ZContenedor e)
        {
            //revisar si hace match
            

            if (args.Count == listaDelcaraciones.Count)
            {
                for (int x = 0; x < args.Count; x++)
                {
                    TeDeU tipo = (TeDeU)listaDelcaraciones[x].Tipo.ejecutar(e);
                    TeDeU tipo2 = args[x].Origen;
                    if (tipo != tipo2)
                    {
                        
                        throw new SemanticError("los argumentos enviados no corresponden al TypeUser que se quiere instanciar"); 
                    }
                    
                    
                    Utilidades.asginar(e.getVariableActual(listaDelcaraciones[x].Id),args[x]);
                    
                }
                return;
                
            }

            throw new SemanticError("los argumentos enviados no corresponden al TypeUser que se quiere instanciar");

        }
    }
}