using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Irony.Parsing;
using servidor.Analizadores;
using System.Windows.Forms;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison.Utils
{
    public class EjecutarAChison
    {
        public void ejecutarAnalizador(string entrada)
        {
            
            AnalizadorChison analizador = new AnalizadorChison();
            Parser parser = new Parser(analizador);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                
                if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
                {
                    //---------------------> Hay Errores      

                    foreach (var item in arbol.ParserMessages)
                    {
                        MessageBox.Show("Error->"+item.Message+" Line:"+item.Location.Line);

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