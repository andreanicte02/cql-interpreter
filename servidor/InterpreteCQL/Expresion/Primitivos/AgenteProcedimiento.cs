using System.Collections.Generic;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class AgenteProcedimiento
    {
        List<Procedimiento> listaProcedimientos = new List<Procedimiento>();

        public List<Procedimiento> ListaProcedimientos
        {
            get => listaProcedimientos;
            
        }


        public void agregarProcedimiento(Procedimiento procedimiento)
        {
            verificarFirma(procedimiento);
            listaProcedimientos.Add(procedimiento);
            
        }

        public void verificarFirma(Procedimiento procedimiento)
        {
            foreach (Procedimiento proc in listaProcedimientos)
            {
                if (proc.mismaFirma(procedimiento))
                {
                    throw new SemanticError("ya existe un procedimiento con la misma firma");
                }
            }
            
        }

        public ZTupla ejecutar(List<ZContenedor> argumentos)
        {
            Procedimiento pro = buscarProc(argumentos);
            return pro.ejecutarProcedimiento(argumentos);
        }

        public Procedimiento buscarProc(List<ZContenedor> argumentos)
        {
            foreach (Procedimiento procedimiento in listaProcedimientos)
            {
                if (procedimiento.mismFirma(argumentos))
                {
                    return procedimiento;
                }
            }
            throw new SemanticError("no existe la funcion esa firma" + strFirma(argumentos));
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