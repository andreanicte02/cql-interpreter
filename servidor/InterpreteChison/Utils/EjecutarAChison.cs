using System;
using System.Collections.Generic;
using Irony.Parsing;
using servidor.Analizadores;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison.Utils
{
    public class EjecutarAChison
    {
        // construir la gramatica es lo mas lento, se hace una sola vez
        private static readonly Parser parserChison = new Parser(new AnalizadorChison());

        public void ejecutarAnalizador(string entrada)
        {
            
            Parser parser = parserChison;
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                
                if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
                {
                    //---------------------> Hay Errores      

                    foreach (var item in arbol.ParserMessages)
                    {
                        Console.WriteLine("Error->"+item.Message+" Line:"+(item.Location.Line + 1));

                    }
                    return;
                }

            }
            
            List<Instruccion> listIns= (   List<Instruccion> ) arbol.Root.AstNode;
            ZContenedor global = new ZContenedor(null, null);
            
            Dbms.cargarTedeUs();
             
            EjecutarAnalizadorCQL.loadToday(global);
            EjecutarAnalizadorCQL.loadNow(global);

            Utilidades.ejecutarSenteciass(listIns, global);

            BaseDeDatos bd = Dbms.BdSeleccionada;
            Console.WriteLine("--- analizado chison ---");


            
        }
    }
}