using System.Collections.Generic;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class AgenteFuncion
    {
        private List<Funcion> listaFunciones = new List<Funcion>();
        

        public void agregarFuncion(Funcion funcion)
        {
            verificarFirma(funcion);
            listaFunciones.Add(funcion);
            
        }

        public void verificarFirma(Funcion funcion)
        {
            foreach (Funcion fun in listaFunciones)
            {
                if (fun.mismFirma(funcion))
                {
                    throw  new SemanticError("ya existe una funcion con la misma firma");
                }
            }
        }

        public ZContenedor ejecutar(List<ZContenedor> argumentos)
        {
            Funcion funcion = buscarFuncion(argumentos);
            return funcion.ejecutarFuncion(argumentos);
            
         
        }

        public Funcion buscarFuncion(List<ZContenedor> argumentos)
        {
            foreach (Funcion funcion in listaFunciones)
            {
                if (funcion.mismFirma(argumentos))
                {
                    return funcion;
                }
            }
            throw new SemanticError("no existe funcion con esa firma " +strFirma(argumentos));
        }

        public string strFirma(List<ZContenedor> argumentos)
        {
            string str = "";
            foreach (ZContenedor arg in argumentos)
            {
                str += arg.stringBonito() + " , ";

            }

            return str;
        }


    }
}