using System.Collections.Generic;
using System.Windows.Forms;
using Irony.Parsing;
using servidor.Analizadores;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoCrearInstruProc:Instruccion
    {
        private string entrada;

        public NodoCrearInstruProc(string entrada)
        {
            this.entrada = entrada;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            if (entrada == "")
            {
                return new List<Instruccion>();
            }

            AnalizadorCQL an = new AnalizadorCQL();
            Parser parser = new Parser(an);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            
            //verificar si el arbol no sta nulo
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                
                if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
                {
                    //---------------------> Hay Errores      

                    foreach (var item in arbol.ParserMessages)
                    {
                        MessageBox.Show("Error->"+item.Message+" Line:"+item.Location.Line);

                    }
                    return null;
                }

            }
            List<Instruccion> listIns= (  List<Instruccion>) arbol.Root.AstNode;
            
            
            
            return listIns ?? new List<Instruccion>();
        }
    }
}