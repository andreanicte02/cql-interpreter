using System;
using System.Collections.Generic;
using Irony.Parsing;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.dbms.InstruccionesChison;
using servidor.InterpreteCQL.Expresion.OperacionesAritmeticas;
using servidor.InterpreteCQL.Expresion.OperacionesLogicas;
using servidor.InterpreteCQL.Expresion.OperacionesRelacionales;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Expresionn;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instrucciones.Ciclos;
using servidor.InterpreteCQL.Instrucciones.Sentencias;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;
using servidor.IronyExt;
using static servidor.IronyExt.IronyUtils;
namespace servidor.Analizadores
{
    
    public class AnalizadorCQL: Grammar
    {
        public string archivo { get; }

        public AnalizadorCQL(string archivo = "") : base(caseSensitive: false)
            {
                this.archivo = archivo;
                LanguageFlags |= LanguageFlags.CreateAst;
                CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
                CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/*", "*/");
                NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
                NonGrammarTerminals.Add(COMENTARIO_MULT);
                
                #region lexico

                MarkReservedWords("new");
                MarkReservedWords("true");
                MarkReservedWords("false");
                MarkReservedWords("print");
                MarkReservedWords("null");
       
                MarkReservedWords("return");
                MarkReservedWords("log");
                MarkReservedWords("while");
                MarkReservedWords("if");
                MarkReservedWords("else");
                MarkReservedWords("do");
                MarkReservedWords("switch");
                MarkReservedWords("create");
                MarkReservedWords("type");
                MarkReservedWords("for");
                MarkReservedWords("break");
                MarkReservedWords("continue");
                MarkReservedWords("as");
                MarkReservedWords("procedure");
                MarkReservedWords("call");
                MarkReservedWords("database");
                MarkReservedWords("drop");
                MarkReservedWords("use");
                MarkReservedWords("table");
                MarkReservedWords("primary");
                MarkReservedWords("key");
                MarkReservedWords("insert");
                MarkReservedWords("into");
                MarkReservedWords("values");
                MarkReservedWords("counter");
                MarkReservedWords("truncate");
                MarkReservedWords("alter");
                MarkReservedWords("select");
                MarkReservedWords("from");
                MarkReservedWords("where");
                MarkReservedWords("update");
                MarkReservedWords("set");
                MarkReservedWords("user");
                MarkReservedWords("commit");
                MarkReservedWords("list");
                
                
                var @new = ToTerm("new");
                var @true = ToTerm("true");
                var @false = ToTerm("false");
                var log = ToTerm("log");
                var @null = ToTerm("null");
                var @return = ToTerm("return");
                var @while = ToTerm("while");
                var @if = ToTerm("if");
                var @else = ToTerm("else");
                var @do = ToTerm("do");
                var @switch = ToTerm("switch");
                var @case =  ToTerm("case");
                var @default = ToTerm("default");
                var create = ToTerm("create");
                var type = ToTerm("type");
                var @for = ToTerm("for");
                var @break = ToTerm("break");
                var @continue = ToTerm("continue");
                var @as = ToTerm("as");
                var procedure = ToTerm("procedure");
                var call = ToTerm("call");
                var table = ToTerm("table");
                var primary = ToTerm("primary");
                var key = ToTerm("key");
                var insert = ToTerm("insert");
                var into = ToTerm("into");
                var valu = ToTerm("values");
                var counter = ToTerm("counter");
                var alter = ToTerm("alter");
                var select = ToTerm("select");
                var @from = ToTerm("from");
                var @where = ToTerm("where");
                var set = ToTerm("set");
                
                Terminal mas = ToTerm("+");
                Terminal menos = ToTerm("-");
                Terminal por = ToTerm("*");
                Terminal div = ToTerm("/");
                Terminal apar = ToTerm("(");
                Terminal cpar = ToTerm(")");
                Terminal and = ToTerm("&&");
                Terminal or = ToTerm("||");
                Terminal xor = ToTerm("^");
                Terminal igualQue = ToTerm("==");
                Terminal diferenteQue = ToTerm("!=");
                Terminal mayor = ToTerm(">");
                Terminal menor = ToTerm("<");
                Terminal mayorIgual = ToTerm(">=");
                Terminal menorIgual = ToTerm("<=");
                Terminal alla = ToTerm("{");
                Terminal clla = ToTerm("}");
                Terminal not = ToTerm("!");
                
                
                Terminal igual = ToTerm("=");
                Terminal potencia = ToTerm("**");
                Terminal modulo = ToTerm("%");
                Terminal incremento = ToTerm("++");
                Terminal decremento = ToTerm("--");
                Terminal pcoma = ToTerm(";");
                Terminal dosp = ToTerm(":");
                Terminal punto = ToTerm(".");
                Terminal masIgual = ToTerm("+=");
                Terminal menosIgual = ToTerm("-=");
                Terminal porIgual = ToTerm("*=");
                Terminal divIgual = ToTerm("/=");







                RegexBasedTerminal entero = new RegexBasedTerminal("entero", "[0-9]+");
                entero._accion((values, node) => node.Token.Text);
                
                RegexBasedTerminal @decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
                @decimal._accion((values, node) => node.Token.Text);

                
                RegexBasedTerminal id = new RegexBasedTerminal("id", "[@|A-Za-z|_]([A-Za-z|_|0-9]+)?");
                id._accion((values, node) => node.Token.Text.ToLower());
                
                StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsLineBreak| StringOptions.AllowsAllEscapes);
                cadena._accion((v, n) => n.Token.Value);
                
                
                RegexBasedTerminal @date = new RegexBasedTerminal("date", "['][0-9]+[-][0-9]+[-][0-9]+[']");
                @date._accion((values, node) => node.Token.Text.Replace("'",""));
                
                
                RegexBasedTerminal @time = new RegexBasedTerminal("time", "['][0-9]+[:][0-9]+[:][0-9]+[']");
                @time._accion((values, node) => node.Token.Text.Replace("'",""));
                
                
                
                
                #endregion

                //declararcion
                //asignacion
                //definir tipos

                #region gramatica
                
                var inicio = new AutoNonTerminal("inicio");
                var exp = new  AutoNonTerminal("exp");
                var sentencia = new AutoNonTerminal("sentencia");
                var sentencias = new AutoNonTerminal("sentencias");
                var tipo = new AutoNonTerminal("tipo");
               
                
                var declararVar = new AutoNonTerminal("declararVariable");
                var declararVars = new AutoNonTerminal("decararVars");
                var declaracionVariables = new AutoNonTerminal("declarar");
                
                var parametro = new AutoNonTerminal("parametro");
                var parametros = new AutoNonTerminal("parametros"); 
                var argumentos = new AutoNonTerminal("argumentos");
                var argumentosObligatorios = new AutoNonTerminal("argumentosObligatorios");
                var asignar = new AutoNonTerminal("asignar");

                var sentenciaWhile = new AutoNonTerminal("while");
                var sentenciaIf = new AutoNonTerminal("if");
                var ifCuerpo = new AutoNonTerminal("cuerpo if");
                var sentenciaSwitch = new AutoNonTerminal("sentencia switch");
                var lCases = new AutoNonTerminal("lCases");
                var sentenciaCase = new AutoNonTerminal("senetnciaCase");

                var sentenciaCreateType = new  AutoNonTerminal("setenciaCreateType");
                var crearAtributo = new  AutoNonTerminal("crearAtributos");
                var listaAtributos = new  AutoNonTerminal("listaAtributos");
                var sentenciaDeclararFuncion = new AutoNonTerminal("sentenciaCrearFuncion");
               
                
                var sentenciaDoWhile = new  AutoNonTerminal("sentenciaDoWhile");
                
                var sentenciaFor = new  AutoNonTerminal("sentenciaFor");

                var expAumento = new  AutoNonTerminal("expAumento");
                var expOperarAsignar = new AutoNonTerminal("expOperarAsignar");
                var forInit = new  AutoNonTerminal("forInit");
                var sentenciaReturn = new  AutoNonTerminal("sentenciaReturn");
                var invocarFuncion = new  AutoNonTerminal("sentenciaInvocarSoloFuncion");
                var expZTupla = new  AutoNonTerminal("expZTupla");
                var expInstanciaConValores = new  AutoNonTerminal("expInstanciaConValores");
              
                var senDeclararProce = new AutoNonTerminal("sentDeclararProce");
                
                var parametroTypeUser = new AutoNonTerminal("parametroTypeUser");
                var parametrosTypeUser = new AutoNonTerminal("parametrosTypeUser");
                
                var createTable = new AutoNonTerminal("createTable");
               
                var dropTable = new AutoNonTerminal("");
                
                var lId = new AutoNonTerminal("");
                var sentenciaId = new AutoNonTerminal("sentenciaId");
                var crearColumna = new AutoNonTerminal("crearColumna");
                var columnas = new AutoNonTerminal("columnas");
                var insertInto = new AutoNonTerminal("");
                var alterTable = new AutoNonTerminal("");
                var selectTable = new AutoNonTerminal("");
                var all = new AutoNonTerminal("");
                var whereProdu = new AutoNonTerminal("");
                var updateTable = new AutoNonTerminal("");
                var listaAsignacion = new AutoNonTerminal("");
                var userProdu = new AutoNonTerminal("");
                
                
                inicio.Rule = 
                    produccion(sentencias)._acciondCQL(0,values=>values[0]);

                tipo.Rule =
                    produccion(id)._acciondCQL(0, values => new NodoObtenerTeDeU(values[0]))
                    | produccion(counter)._acciondCQL(0, v => new NodoBuscarCount())
                    | produccion(ToTerm("list") + "<" + tipo + ">")
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.obtenerTeDeULista(v[2], a)))
                    ;

    
                declaracionVariables.Rule =
                    produccion(tipo + declararVars)._acciondCQL(0, values => new NodoDeclararVars(values[0], values[1]));

                this.ConfigList<NodoDeclararVar>(true, declararVars, declararVar,",");
                
                
                declararVar.Rule = produccion(id)._acciondCQL(0, values => new NodoDeclararVar(values[0], null) )
                                   | produccion(id + igual + exp)._acciondCQL(1, values=> new NodoDeclararVar(values[0], values[2]) );
                
                asignar.Rule = 
                    produccion(exp + igual + exp)._acciondCQL(0, values => new NodoAsignar(values[0], values[2]))
                    | produccion(expOperarAsignar)._acciondCQL(0, v => v[0])
                    | produccion(expAumento)._acciondCQL(0, v=>v[0] );


                this.ConfigList<Instruccion>(false,sentencias, sentencia);


                sentencia.Rule =
                    produccion(log + apar + exp + cpar + pcoma)._acciondCQL(0, values => new NodoLogPrint(values[2]))
                    | produccion(asignar + pcoma)._acciondCQL(0, values => values[0])
                    | produccion(declaracionVariables + pcoma)._acciondCQL(0, values => values[0])
                    | produccion(sentenciaWhile)._acciondCQL(0, values => values[0])
                    | produccion(sentenciaDoWhile)._acciondCQL(0, values => values[0])
                    | produccion(sentenciaIf)._acciondCQL(0, v => v[0])
                    | produccion(sentenciaCreateType)._acciondCQL(0, v => v[0])
                    | produccion(sentenciaDeclararFuncion)._acciondCQL(0, v => v[0])
                    | produccion(sentenciaFor)._acciondCQL(0, v => v[0])
                    | produccion(@break + pcoma)._acciondCQL(0, v => new NodoBreak())
                    | produccion(@continue + pcoma)._acciondCQL(0, v => new NodoContinue())
                    | produccion(sentenciaReturn + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(invocarFuncion + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(senDeclararProce)._acciondCQL(0, v => v[0])
                    | produccion(ToTerm("delete") + "type" + id + pcoma)
                        ._acciondCQL(0, v => new NodoEliminarStruct(v[2]))

                    | produccion(create + "database" + id + pcoma)
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.crearBaseDeDatos(v[2])))
                    | produccion(create + "database" + @if + "not" + "exists" + id + pcoma)
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.crearBaseDeDatosSiNoExiste(v[5])))
                    | produccion(ToTerm("use") + id + pcoma)
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.seleccionarBaseDeDatos(v[1])))
                    | produccion(ToTerm("drop") + "database" + id + pcoma)
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.eliminarBaseDeDatos(v[2])))
                    | produccion(createTable + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(dropTable + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(insertInto + pcoma)._acciondCQL(1, v => v[0])

                    | produccion(ToTerm("truncate") + table + id + pcoma)
                        ._acciondCQL(1, v => new NodoTruncate(v[2]))


                    | produccion(alterTable + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(selectTable + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(updateTable + pcoma)._acciondCQL(1, v => v[0])
                    | produccion(userProdu + pcoma)._acciondCQL(1, v=>v[0])
                    | produccion(ToTerm("commit")+ pcoma)._acciondCQL(1, v=> new NodoCommit())
                    
                ;
                
                
                        
                


                //------------tablas

                createTable.Rule = produccion(create + table + id + apar + columnas+cpar)
                                       ._acciondCQL(0, v => new NodoSimple(a => Dbms.crearTabla(v[2], v[4],a )))
                                   | produccion(create + table + @if + "not" + "exists" + id + apar + columnas+cpar)
                                       ._acciondCQL(0, v=> new NodoSimple(a=>Dbms.createTableSiNoExiste(v[5],v[7],a )));

                dropTable.Rule = produccion(ToTerm("drop") + table + id)
                        ._acciondCQL(0, v => new NodoSimple(a => Dbms.dropTable(v[2])))
                    | produccion(ToTerm("drop") + table + @if  + "exists" + id)
                        ._acciondCQL(0, v=> new NodoSimple(a=>Dbms.dropTableSiExiste(v[5])))
                    ;


                this.ConfigList<NodoDeclararEncabezados>(false,columnas,crearColumna,",");



                crearColumna.Rule = produccion(id + tipo)
                                        ._acciondCQL(0, v => new NodoDeclararEncabezados(v[0], v[1]))
                                    | produccion(id + tipo + primary + key)
                                        ._acciondCQL(0, v => new NodoDeclararEncabezados(v[0], v[1]))
                                    | produccion(primary + key + apar + lId +cpar)._acciondCQL(0, v=> new NodoSimple(a=>null));
                     
                //---------------lista id
                
                this.ConfigList<NodoBuscarId>(false, lId, sentenciaId,",");
                
                sentenciaId.Rule =produccion(id)._acciondCQL(0,v=>new NodoBuscarId(v[0]));

                
                //--------------insert into
                insertInto.Rule = produccion(insert + into + id + valu + apar + argumentos +cpar)
                    ._acciondCQL(0,v=>new NodoInserToSimple(v[2],v[5]))
                    
                    | produccion(insert +into + id + apar + lId +cpar +valu + apar + argumentos +cpar)
                        ._acciondCQL(0, v=> new NodoInsertEspecial(v[2],v[4],v[8]));

                
                //---------------alter table

                alterTable.Rule = 
                    produccion(alter + table + id + ToTerm("drop") + lId)
                        ._acciondCQL(0, v => new NodoAlterDrop(v[2], v[4]))
                    | produccion(alter + table + id + ToTerm("add")+ columnas)
                        ._acciondCQL(0, v=> new NodoAlterAdd(v[2], v[4]))
                    ;
                
                
                //---------------select

                selectTable.Rule =
                    produccion(select + all + @from + id)
                        ._acciondCQL(0, v => new NodoSelect(v[1], new NodoBuscarTabla(v[3]), null))
                    | produccion(select + all + @from + id + where + exp)
                        ._acciondCQL(0, v => new NodoSelect(v[1], new NodoBuscarTabla(v[3]), v[5]));

                all.Rule = produccion(por)._acciondCQL(0, v => null)
                           | produccion(argumentos)._acciondCQL(0, v => v[0]);
                
                //---------------update

                updateTable.Rule = produccion(ToTerm("update") + id + set+listaAsignacion + whereProdu)
                                       ._acciondCQL(0, v=> new NodoUpdate( new NodoBuscarTabla(v[1]),v[3],v[4])); //
                    

                this.ConfigList<Instruccion>(false,listaAsignacion,asignar,",");
                                   
                whereProdu.Rule = produccion(where +exp)._acciondCQL(0, v=> v[1])
                                  | produccion(Empty)._acciondCQL(0, v=> null);
                
                //------------usuarios

                userProdu.Rule = produccion(create + ToTerm("user") + id + ToTerm("whit") + ToTerm("password") + exp)
                    ._acciondCQL(0, v => new NodoSimple(a => Dbms.crearUsuario(v[2], v[5])));
                
                
                //------------invocacion de funciones

                invocarFuncion.Rule = produccion(id + apar + argumentos + cpar)
                    ._acciondCQL(1,v=>new NodoInvocarSoloFuncion(v[0], v[2]))
                    
                    | produccion(exp + punto + id + apar +  argumentos +cpar)
                        ._acciondCQL(1, v=> new NodoInvocarFuncionPunto(v[0],v[2],v[4]))
                    
                    | produccion(call + id + apar+ argumentos + cpar)
                        ._acciondCQL(0, v => new NodoInvocarProcedure(v[1],v[3]))

                    ;

                
                //----------argumentos
                
                this.ConfigList<Instruccion>(false,argumentos,exp,",");
                this.ConfigList<Instruccion>(true, argumentosObligatorios, exp, ",");
                
                
                //-----------return
                
                sentenciaReturn.Rule = produccion(@return + argumentos)
                    ._acciondCQL(0, v=> new NodoRetornar(v[1]));
                
                //----------ciclos
                
                sentenciaWhile.Rule =
                    produccion(@while + apar + exp + cpar + alla + sentencias + clla)
                        ._acciondCQL(0, values => new NodoWhile(values[2], values[5]));


                sentenciaDoWhile.Rule =
                    produccion(@do + alla + sentencias + clla + @while + apar + exp + cpar + pcoma)
                        ._acciondCQL(0,values => new NodoDoWhile(values[6], values[2]));


                //-----------if
                sentenciaIf.Rule =
                    produccion(@if + apar + exp + cpar + ifCuerpo)
                        ._acciondCQL(0, values => new NodoIf(values[2], values[4], new List<Instruccion>()))
                    | produccion(@if + apar + exp + cpar + ifCuerpo + @else + ifCuerpo)
                        ._acciondCQL(0, values => new NodoIf(values[2], values[4], values[6]))
                   | produccion(@if + apar + exp + cpar + ifCuerpo + @else + sentenciaIf)
                    ._acciondCQL(0, v =>
                    {

                        List<Instruccion> ins = new List<Instruccion>();
                        ins.Add(v[6]);

                        return new NodoIf(v[2], v[4], ins);
                    });

                ifCuerpo.Rule =
                    produccion(alla + sentencias + clla)._acciondCQL(0, values=> values[1]);


                //----------------switch
                
                sentenciaSwitch.Rule =
                    produccion(@switch + apar + exp + cpar + alla + lCases + @default + dosp + sentencias + clla)
                        ._acciondCQL(0, values => new NodoSwitch(values[2], values[5], new NodoDefault(values[8])))
                    | produccion(@switch + apar + exp + cpar + alla + lCases + clla)
                        ._acciondCQL(0,
                            values => new NodoSwitch(values[2], values[5], new NodoDefault(new List<Instruccion>())))
                    | produccion(@switch + apar + exp + cpar + alla + clla)
                        ._acciondCQL(0,
                            values => new NodoSwitch(values[2], new List<NodoCase>(), new NodoDefault(new List<Instruccion>())))
                    ;

                this.ConfigList<NodoCase>(false,lCases,sentenciaCase);
                
                sentenciaCase.Rule = produccion( @case + exp + dosp + sentencias)
                    ._acciondCQL(0,values => new NodoCase(values[0], values[3]));

                
                //---------creacion de tdeus
                
                sentenciaCreateType.Rule = produccion(create + type + id + alla + parametrosTypeUser + clla)
                    ._acciondCQL(0, values => new NodoDeclararStruct(values[2],values[4] ));


                
                //----------declaracion de parametros
                
                this.ConfigList<NodoDeclararParametro>(false, parametrosTypeUser, parametroTypeUser,",");
                
                parametroTypeUser.Rule = produccion(id + tipo)._acciondCQL(0, v=> new NodoDeclararParametro(v[1], v[0]));
                
                this.ConfigList<NodoDeclararVars>(true,listaAtributos, crearAtributo,",");

                //---------------creacion de atributos 
                
                crearAtributo.Rule = produccion(tipo + id).
                    _acciondCQL(0, values =>
                    {
                        List<NodoDeclararVar> lis= new List<NodoDeclararVar>();
                        lis.Add(new NodoDeclararVar(values[1], null));
                        return  new NodoDeclararVars(values[0], lis);
                    });


                //--------------craecion de funciones
                
                sentenciaDeclararFuncion.Rule = produccion(tipo + id + apar+ parametros + cpar + alla + sentencias +clla)
                    ._acciondCQL(2, v=> new NodoDeclararFuncion(
                        v[0],v[1],v[3],v[6]));

                
                //-----------for
                
                sentenciaFor.Rule =
                    produccion(@for + apar + forInit + pcoma + exp + pcoma + asignar + cpar + alla + sentencias + clla)
                        ._acciondCQL(0, v => new NodoFor(v[2], v[4], v[6], v[9]))
                    ;

                forInit.Rule = produccion(asignar)._acciondCQL(0, v=> v[0])
                               | produccion(declaracionVariables)._acciondCQL(0, v=> v[0]);

                //------------expresiones
                exp.Rule =
                    produccion(exp + mas + exp)._acciondCQL(1, values => new NodoSuma(values[0], values[2]))
                    //| produccion(exp + menos + exp)._acciondCQL(1, values => new NodoResta(values[0], values[2]))
                    //| produccion(exp + por + exp)._acciondCQL(1, values => new NodoMultiplicacion(values[0], values[2]))
                    //| produccion(exp + div + exp)._acciondCQL(1, values => new NodoDivision(values[0], values[2]))
                    //| produccion(exp + modulo + exp)._acciondCQL(1, values => new NodoModular(values[0], values[2]))
                    //| produccion(exp + potencia + exp)._acciondCQL(1, values => new NodoPotencia(values[0], values[2]))

                    | produccion(exp + and + exp)._acciondCQL(1, values => new NodoAnd(values[0], values[2]))
                    | produccion(exp + or + exp)._acciondCQL(1, values => new NodoOr(values[0], values[2]))
                    //| produccion(exp + xor + exp)._acciondCQL(1, values => new NodoXor(values[0], values[2]))
                    //| produccion(not + exp)._acciondCQL(0, values => new NodoNot(values[0]))

                    | produccion(exp + igualQue + exp)._acciondCQL(1, values => new NodoIgualQue(values[0], values[2]))
                    | produccion(exp + diferenteQue + exp)._acciondCQL(1, values => new NodoDiferenteQue(values[0], values[2]))
                    | produccion(exp + mayor + exp)._acciondCQL(1, values => new NodoMayorQue(values[0], values[2]))
                    | produccion(exp + menor + exp)._acciondCQL(1, values => new NodoMenorQue(values[0], values[2]))
                    //| produccion(exp + mayorIgual + exp)
                    //    ._acciondCQL(1, values => new NodoMayorIgualQue(values[0], values[2]))
                    //| produccion(exp + menorIgual + exp)
                    //    ._acciondCQL(1, values => new NodoMenorIgualQue(values[0], values[2]))

                    //| produccion(menos + exp + ReduceHere())._acciondCQL(0, v => new NodoUnarioResta(v[1]))
                    //| produccion(mas + exp + ReduceHere())._acciondCQL(0, v => new NodoUnarioSuma(v[1]))

                   // | produccion(expAumento)._acciondCQL(0, v => v[0])

                    | produccion(@new + id)._acciondCQL(0, values => new NodoCrearInstancia(values[1]))
                    | produccion(exp + punto + id)._acciondCQL(1, values => new NodoAccesoPunto(values[0], values[2]))

                    | produccion(invocarFuncion)._acciondCQL(0, v => v[0])

                    | produccion(entero)._acciondCQL(0, values => new NodoCrearNumero(Int32.Parse(values[0])))
                    | produccion(@decimal)._acciondCQL(0, values => new NodoCrearDecimal(Double.Parse(values[0])))
                    | produccion(@false)._acciondCQL(0, values => new NodoCrearBool(false))
                    | produccion(@true)._acciondCQL(0, values => new NodoCrearBool(true))
                    | produccion(cadena)._acciondCQL(0, values => new NodoCrearCadena(values[0])) 
                    | produccion(@null)._acciondCQL(0, values => new NodoCrearNulo())
                    | produccion(id)._acciondCQL(0, values => new NodoBuscarId(values[0]))

                    | produccion(apar + exp + cpar)._acciondCQL(0, values => values[1])

                    //| produccion(@date)._acciondCQL(0, v=>new NodoCrearDate(v[0]))
                    //| produccion(@time)._acciondCQL(0, v => new NodoCrearTime(v[0]))
                    //| produccion(apar + id + cpar + exp)._acciondCQL(0, v=> new NodoCasteoExplicito(new NodoObtenerTeDeU(v[1]), v[3]) )
                    | produccion(expInstanciaConValores)._acciondCQL(0, v => v[0])
                    
                    | produccion(exp + ToTerm("?") + exp + dosp + exp)._acciondCQL(1, v=> new NodoTernario(v[0],v[2], v[4]))
                    
                    | produccion("<<" + selectTable + ">>")._acciondCQL(0, v => v[1])
                    | produccion(ToTerm("[") + "]")._acciondCQL(0, v => new NodoSimple(a =>  ZLista.crearListaVaciaSinTipo()))
                    | produccion("[" + argumentosObligatorios + "]")
                        ._acciondCQL(1, v => new NodoSimple(a =>
                        {
                            var valores = Utilidades.desnvolverArgumento(v[1], a);
                            return ZLista.crearLista(valores);
                        }))
                    | produccion( exp + "[" + exp + "]" )
                        ._acciondCQL(1, 
                            v => new NodoSimple(
                                a => Utilidades.accederElementoLista(a, v[0], v[2])))
                   
                    ;
                
                
                
                
                expAumento.Rule = 
                     produccion(exp + incremento)._acciondCQL(1, v => new NodoIncremento(v[0]))
                    |produccion(exp + decremento)._acciondCQL(1, v => new NodoDecremento(v[0]))
                    ;

                expInstanciaConValores.Rule =
                    produccion(expZTupla + @as + id)
                        ._acciondCQL(1, v => new NodoInstanciaConValores(v[0], new NodoObtenerTeDeU(v[2]) ));

                expZTupla.Rule =
                    produccion(alla + argumentos +clla)._acciondCQL(0,v=> new NodoCrearTupla(v[1]));


                this.ConfigList<NodoDeclararParametro>(false,parametros,parametro,",");
                
                parametro.Rule = produccion(tipo + id)._acciondCQL(0,v=>new NodoDeclararParametro(v[0], v[1]) );
                
                expOperarAsignar.Rule = 
                    produccion(exp + masIgual + exp)._acciondCQL(1, v=> new NodoAsignarOperarMas(v[0], v[2]))
                    | produccion(exp + menosIgual + exp)._acciondCQL(1,v=> new NodoAsignarOperarMenos(v[0], v[2]))
                    | produccion(exp + porIgual + exp)._acciondCQL(1, v=> new NodoAsignarOperarPor(v[0],v[2]))
                    | produccion(exp + divIgual + exp)._acciondCQL(1, v=> new NodoAsignarOperarDiv(v[0],v[2]));

                senDeclararProce.Rule =
                    produccion(procedure + id + apar +parametros+ cpar + ToTerm(",") + apar + parametros +cpar + alla +sentencias+ clla)
                        ._acciondCQL(0, v=> 
                            new NodoDeclararProce(v[1], v[3], v[7],v[10]));

                
                LanguageFlags |= LanguageFlags.CreateAst;
                Root = inicio;
                
                
                        
                RegisterOperators(12, Associativity.Left, apar, cpar, punto,@as);
                RegisterOperators(11, Associativity.Left, decremento, incremento);
                RegisterOperators(10, Associativity.Right, not);
                RegisterOperators(9, Associativity.Right, potencia, @new); 
                RegisterOperators(8, Associativity.Left, por, div,modulo);
                RegisterOperators(7, Associativity.Left, mas, menos);
                
                
                RegisterOperators(6,  Associativity.Neutral, mayor, mayorIgual, menor,menorIgual );

                RegisterOperators(5,  Associativity.Left, igualQue, diferenteQue );
               
                RegisterOperators(4, Associativity.Left, xor);

                RegisterOperators(3, Associativity.Left, and);
                RegisterOperators(2, Associativity.Left, or);
                RegisterOperators(1, Associativity.Right, igual,masIgual,menosIgual,divIgual,porIgual);


                // precedencia

                #endregion



            }

    }
}