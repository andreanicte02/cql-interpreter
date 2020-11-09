using Irony.Parsing;
using servidor.IronyExt;
using System;
using System.Collections.Generic;
using Irony.Parsing;
using servidor.InterpreteChison;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instrucciones.Ciclos;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;
using servidor.IronyExt;
using static servidor.IronyExt.IronyUtils;

namespace servidor.Analizadores
{
    public class AnalizadorChison:Grammar
    {
        public string archivo;

        public AnalizadorChison(string archivo = "") : base(caseSensitive: false)
        {
            
            MarkReservedWords("\"DATA\"");
            MarkReservedWords("\"NAME\"");
            MarkReservedWords("true");
            MarkReservedWords("false");
            MarkReservedWords("null");
            

            this.archivo = archivo;
            
            var dataBase = ToTerm("\"DATABASE\"");
            var data = ToTerm("\"DATA\"");
            var name= ToTerm("\"NAME\"");
            var cqlType= ToTerm("\"CQL-TYPE\"");
            var attr = ToTerm("\"ATTRS\"");
            var type = ToTerm("\"TYPE\"");
            var colummns = ToTerm("\"COLUMNS\"");
            var pk = ToTerm("\"PK\"");
            var parametres = ToTerm("\"PARAMETERS\"");
            
            var @as = ToTerm("\"AS\"");
            
            var instr = ToTerm("\"INSTR\"");
            
            var @true = ToTerm("true");
            var @false = ToTerm("false");
            var @null = ToTerm("null");
            var @in = ToTerm("in");
            var @out = ToTerm("out");



            Terminal dolar = ToTerm("$");
            Terminal abrir = ToTerm("<");
            
            Terminal igual = ToTerm("=");
            Terminal acor = ToTerm("[");
            Terminal ccor = ToTerm("]");
            
            Terminal ini = ToTerm("$<");
            
            Terminal fin = ToTerm(">$");
            Terminal cerrar = ToTerm(">");
           
            
            var inicio = new AutoNonTerminal("inicio");
            
            var grupo = new AutoNonTerminal("grupo");
            var pGrupo = new AutoNonTerminal("pGrupo");
            var pBase = new AutoNonTerminal("pBase");
            var lpBase = new AutoNonTerminal("lpBase");
            var lpGrupo = new AutoNonTerminal("lpGrupo");
            var lGrupo = new AutoNonTerminal("lpGrupo");
            
            var pAttr = new AutoNonTerminal("pattr");
            var lpAttr = new AutoNonTerminal("lpattr");
            
            
            var lpCol = new AutoNonTerminal("lpattr");
            var pCol = new AutoNonTerminal("lpattr");
            var pPk = new AutoNonTerminal("pPk");
            var pBools = new AutoNonTerminal("pBools");
            
            
            var pFilas = new AutoNonTerminal("pFilas");
            var lpFilas = new AutoNonTerminal("lpFilas");
            var filas = new AutoNonTerminal("filas");
            var lFilas = new AutoNonTerminal("lFilas");
            
            
         
            var lParametros = new AutoNonTerminal("lParametros");

            var parametros = new AutoNonTerminal("parametros");

            
            var exp = new AutoNonTerminal("exp");
            
            var inOn = new AutoNonTerminal("inON");
            


            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsLineBreak| StringOptions.AllowsAllEscapes);
            cadena._accion((v, n) => n.Token.Value.ToString().ToLower());

            StringLiteral cuerpInstru = new StringLiteral("cuerpInstru", "$", StringOptions.AllowsLineBreak| StringOptions.AllowsAllEscapes);
            cuerpInstru._accion((v, n) => n.Token.Value.ToString().ToLower());
            
            RegexBasedTerminal entero = new RegexBasedTerminal("entero", "[0-9]+");
            entero._accion((values, node) => node.Token.Text);
                
            RegexBasedTerminal @decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            @decimal._accion((values, node) => node.Token.Text);
            
             
            RegexBasedTerminal @date = new RegexBasedTerminal("date", "['][0-9]+[-][0-9]+[-][0-9]+[']");
            @date._accion((values, node) => node.Token.Text.Replace("'",""));
                
                
            RegexBasedTerminal @time = new RegexBasedTerminal("time", "['][0-9]+[:][0-9]+[:][0-9]+[']");
            @time._accion((values, node) => node.Token.Text.Replace("'",""));


            inicio.Rule = produccion(ini + dataBase + igual + acor +  lpBase+ccor  +fin)
                ._accionChison(0, v=>v[4]);

            this.ConfigList<Instruccion>(false, lpBase, pBase, ",");
            
            
            pBase.Rule = produccion(abrir+name + igual + cadena + ToTerm(",") + data + igual + acor + lpGrupo+ ccor+ cerrar)
                    ._accionChison(2, v=> new NodoBase(new NodoCrearBD(v[3]),v[8]) )
                         | produccion(abrir + data + igual + acor+ lpGrupo + ccor + ToTerm(",")+ name + igual + cadena + cerrar)
                             ._accionChison(2, v =>new NodoBase(new NodoCrearBD(v[9]),v[4]));



            this.ConfigList<Instruccion>(false, lpGrupo, pGrupo,",");
            
            pGrupo.Rule = produccion(abrir + lGrupo+ cerrar)
                ._accionChison(0, v=> new NodoGrupoData(v[1]));
            
            
            this.ConfigList<Instruccion>(true, lGrupo, grupo,",");


            grupo.Rule = produccion(name + igual + cadena)
                             ._accionChison(1, v => new NodoCrearNombre(v[2]))
                         | produccion(attr + igual + acor + lpAttr + ccor)
                             ._accionChison(1, v => new NodoAtrr(v[3]))
                         | produccion(cqlType + igual + cadena)
                             ._accionChison(1, v => new NodoObtenerTipo(v[2]))
                         | produccion(colummns + igual + acor + lpCol + ccor)
                             ._accionChison(1, v => new NodoColumns(v[3]))
                         | produccion(data + igual + acor + lpFilas + ccor)
                             ._accionChison(1, v => new NodoData(v[3]))
                         | produccion(parametres + igual + acor + lParametros + ccor)
                             ._accionChison(1, v => new NodoParametro(v[3]))
                         | produccion(instr + igual + cuerpInstru)
                             ._accionChison(1, v=> new NodoCrearInstruProc(v[2]));




            this.ConfigList<NodoDeclararParametro>(false, lpAttr, pAttr,",");
            
            pAttr.Rule =produccion( abrir+name + igual + cadena + ToTerm(",") + type + igual + cadena  + cerrar)
                    ._accionChison(4, v=>
                    {
                        if (v[7].Equals("counter"))
                        {
                            return new NodoDeclararParametro(new NodoBuscarCount(), v[3]);   
                        }

                        return new NodoDeclararParametro(new NodoObtenerTeDeU(v[7]), v[3]);
                    })
                        |produccion(abrir +type + igual + cadena  + ToTerm(",")+ name + igual + cadena + cerrar)
                            ._accionChison(4, v=>
                            {
                                if (v[3].Equals("counter"))
                                {
                                    return new NodoDeclararParametro(new NodoBuscarCount(), v[7]);
                                }

                                return new NodoDeclararParametro(new NodoObtenerTeDeU(v[3]), v[7]);
                            });

            
            
            this.ConfigList<NodoDeclararEncabezados>(true, lpCol, pCol, ",");
            
            pCol.Rule = produccion( abrir+name + igual + cadena + ToTerm(",") + type + igual + cadena +pPk + cerrar)
                    ._accionChison(4, v=>
                    {
                        if (v[7].Equals("counter"))
                        {
                            return new NodoDeclararEncabezados( v[3],new NodoBuscarCount() );   
                        }

                        return new NodoDeclararEncabezados(v[3],new NodoObtenerTeDeU(v[7]));
                    })
                        |produccion(abrir +type + igual + cadena  + ToTerm(",")+ name + igual + cadena+ pPk + cerrar)
                            ._accionChison(4, v=>
                            {
                                if (v[3].Equals("counter"))
                                {
                                    return new NodoDeclararEncabezados(v[7], new NodoBuscarCount());
                                }

                                return new NodoDeclararEncabezados( v[7], new NodoObtenerTeDeU(v[3]));
                            });
            
            

            pPk.Rule = produccion( ToTerm(",") + pk + igual + pBools)._accionChison(1,v=> new NodoSimple(null) )
                       | produccion(Empty)._accionChison(1, v=> new NodoSimple(null));


            pBools.Rule = produccion(@true)._accionChison(0, v=>new  NodoSimple(null))
                          |produccion( @false)._accionChison(0, v=> new NodoSimple(null));



            this.ConfigList<NodoFila>(false, lpFilas, pFilas,",");
            
            pFilas.Rule = produccion(abrir + lFilas + cerrar)._accionChison(0, v=> new NodoFila(v[1]));
            
            this.ConfigList<NodoAsignar>(true, lFilas,filas, ",");

            filas.Rule = produccion(cadena + igual + exp)
                ._accionChison(1,v=>new NodoAsignar(new NodoBuscarId(v[0]),v[2]));

            exp.Rule = produccion(@false)._accionChison(0, values => new NodoCrearBool(false))
                       | produccion(@true)._accionChison(0, values => new NodoCrearBool(true))
                       | produccion(cadena)._accionChison(0, values => new NodoCrearCadena(values[0]))
                       | produccion(entero)._accionChison(0, values => new NodoCrearNumero(Int32.Parse(values[0])))
                       | produccion(@decimal)._accionChison(0, values => new NodoCrearDecimal(Double.Parse(values[0])))
                       | produccion(time)._accionChison(0, v => new NodoCrearTime(v[0]))
                       | produccion(date)._accionChison(0, v => new NodoCrearDate(v[0]))
                       | produccion(@null)._accionChison(0, values => new NodoCrearNulo())
                       | produccion(pFilas)._accionChison(0, v => v[0]);
                       //faltarian lo de las listas


            this.ConfigList<NodoCrearParametro>(false, lParametros, parametros, "," );

            parametros.Rule = 
                produccion(abrir+name + igual + cadena + ToTerm(",") + type + igual + cadena + ToTerm(",") + @as + igual + inOn + cerrar)._accionChison
                    (1, v=>new NodoCrearParametro(new NodoObtenerTeDeU(v[7]),v[3] ,v[11]))
                |  produccion(abrir+ type + igual + cadena + ToTerm(",") +  name + igual + cadena+ ToTerm(",")+ @as + igual + inOn + cerrar)._accionChison
                    (1, v=> new NodoCrearParametro(new NodoObtenerTeDeU(v[3]),v[7] ,v[11]));

            inOn.Rule = produccion(@in)._accionChison(0, v=>"in")
                        | produccion(@out)._accionChison(0, v=> "out");
            
            LanguageFlags |= LanguageFlags.CreateAst;
            Root = inicio;





        }


    }
}