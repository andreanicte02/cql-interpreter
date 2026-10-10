using System;
using Irony.Parsing;
using servidor.Analizadores;
using System.Collections.Generic;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL
{
    public class EjecutarAnalizadorCQL
    {
        // construir la gramatica es lo mas lento, se hace una sola vez
        private static readonly Parser parserCql = new Parser(new AnalizadorCQL());

        public void ejecutarAnalizador(string entrada)
        {
            //llamada a los cosos de irony
            
            Parser parser = parserCql;
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
                        Console.WriteLine("Error->"+item.Message+" Line:"+(item.Location.Line + 1));

                    }
                    return;
                }

            }

            List<Instruccion> listIns= (  List<Instruccion>) arbol.Root.AstNode;
            ZContenedor global = new ZContenedor(null, null);
            
            
            Dbms.cargarTedeUs();
            
            //funciones
            loadToday(global);
            loadNow(global);
            
            
            Utilidades.ejecutarSentencias(listIns, global);
        }
        
        
        public static void loadToday(ZContenedor e)
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoDate, parametros, e,
                list =>
                {

                    return new ZDate(DateTime.Today.ToString());       

                }

            );
            
            e.declararFuncion("today",f);

        }
        
        
        public static  void loadNow(ZContenedor e)
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoDate, parametros, e,
                list =>
                {
                 
                   
                    return new ZTiempo(DateTime.Now.ToString());       

                }

            );
            
            e.declararFuncion("now",f);

        }

    }
}