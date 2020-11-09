using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instrucciones.Ciclos;
using servidor.InterpreteCQL.Instruccionesn;
using Retorno = servidor.InterpreteCQL.Expresion.Primitivos.Retorno;

namespace servidor.InterpreteCQL
{
    public class Utilidades
    {

        /// <summary>
        /// Siempre se devuelve contendeor
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        public static ZContenedor desenvolver(Object valor)
        {

            if (valor is Simbolo sim) {
                
                return sim.obtenerValor();
            }

            if (valor is ZContenedor i) {
                return i;
            }
            
            throw new SystemException("El valor no es instancia ni simbolo");

        }

        public static Object ejecutarSentencias(List<Instruccion> lInstruc, ZContenedor c)
        {
            foreach (Instruccion ins in lInstruc)
            {
                var result = ins.ejecutar(c);

                if (result is NodoBreak || result is NodoContinue || result is Retorno || result is RetornoProc)
                {
                    return result;
                }
            }

            return null;

        }

        
        public static void asginar(Simbolo ladoIzq, Object ladoDer)
        {

            ZContenedor derecho = desenvolver(ladoDer);
            
            if (ladoIzq.obtenerInstanciaTipo() == derecho.Origen)
            {
                ladoIzq.definirValor(derecho);
                return;
            }

            if (ladoIzq.obtenerInstanciaTipo() is TeDeULista && derecho.Origen == TiposPrimitivos.tipoListaVacia)
            {
                ladoIzq.definirValor(new ZLista(new List<ZContenedor>(), ladoIzq.obtenerInstanciaTipo()));
                return;
            }

            
            if (derecho.Origen == TiposPrimitivos.tipoNulo)
            {
                ladoIzq.definirValor(TiposPrimitivos.instanicaNulo);
                return;
            }


            if (isNumeric(ladoIzq))
            {
                casteoImplicitoNumero(ladoIzq, derecho);
                return;
                
            }

         

//            throw new SemanticError("se intenta asignar algo que no es del mismo tipo");
            throw new SemanticError($"No se puede asignar valor de tipo '{derecho.Origen.Nombre}' en variable de tipo '{ladoIzq.obtenerInstanciaTipo().Nombre}'");

        }
        
        public static void casteoImplicitoNumero(Simbolo algo1, ZContenedor d2) {

            if (algo1.obtenerInstanciaTipo() == TiposPrimitivos.tipoNumero && d2 is ZDecimal n2)
            {
                
                algo1.definirValor(new ZNumero((int)n2.obtenerValor()));
                return;
               
                
            }

            if (algo1.obtenerInstanciaTipo() == TiposPrimitivos.tipoDicimal && d2 is ZNumero n3)
            {
               
                algo1.definirValor(new ZDecimal(n3.obtenerValor()));
                return;
                
            }
            
            throw new SemanticError($"No se puede asignar valor de tipo '{d2.Origen.Nombre}' en variable de tipo '{algo1.obtenerInstanciaTipo().Nombre}'");

        }

        public static bool isNumeric(Simbolo algo2)
        {
            return (algo2.obtenerInstanciaTipo() == TiposPrimitivos.tipoNumero ||
                    algo2.obtenerInstanciaTipo() == TiposPrimitivos.tipoDicimal);
        }

        public static ZBool evaluarCondicion(Instruccion exp,ZContenedor e)
        {
            var algo = exp.ejecutar(e);
            var condicion = desenvolver(algo);
            if (condicion is ZBool c)
            {
                return c;
            }
            throw  new SemanticError("la expresion evaluada no es una condicion");

        }
        
        
        public static Simbolo buscarId(ZContenedor e, string id)
        {
            for (ZContenedor temporal = e; temporal != null; temporal = temporal.anterior)
            {
                if (temporal.exsiteVariableActual(id))
                {
                    return temporal.getVariableActual(id);
                }
            }

            return null;
        }

        public static ZContenedor invocarSoloFuncion(ZContenedor c, string id, List<ZContenedor> argumentos)
        {
            //argi,emtps deberoam de or desebmuetlos
            AgenteFuncion agente = c.getAgente(id);
            return agente.ejecutar(argumentos);
            
        }

        public static ZTupla invocarProcedimiento(ZContenedor e, string id, List<ZContenedor> argumentos)
        {
            
            AgenteProcedimiento agente = Dbms.getBd().getProc(id);
            return agente.ejecutar(argumentos);

        }

        public static ZContenedor invocarFuncionPunto(ZContenedor c, string id, List<ZContenedor> argumentos)
        {
            AgenteFuncion agente = c.getAgenteActual(id);
            return agente.ejecutar(argumentos);
        }
        

        public static List<ZContenedor> desnvolverArgumento(List<Instruccion> instruccionesArgumentos, ZContenedor e)
        {
            List<ZContenedor> argumentos = new List<ZContenedor>();
            foreach (Instruccion ins in instruccionesArgumentos)
            {

                var algo = ins.ejecutar(e);
                ZContenedor valor = desenvolver(algo);
                argumentos.Add(valor);
            }
            
            

            return argumentos;

        }

        public static void AsignarValorInicial(Simbolo sim)
        {
            if (sim.obtenerInstanciaTipo() == TiposPrimitivos.tipoBool)
            {
                sim.definirValor(new ZBool(false));
                return;
            }

            if (sim.obtenerInstanciaTipo() == TiposPrimitivos.tipoNumero)
            {
                sim.definirValor(new ZNumero(0));
                return;
            }

            if (sim.obtenerInstanciaTipo() == TiposPrimitivos.tipoDicimal)
            {
                sim.definirValor(new ZDecimal(0.0));
                return;
                
            }
            
            sim.definirValor(TiposPrimitivos.instanicaNulo);

        }

     
        public static bool esPrimitivo(TeDeU type)
        {
            return type == TiposPrimitivos.tipoBool || type == TiposPrimitivos.tipoDate ||
                   type == TiposPrimitivos.tipoDicimal ||
                   type == TiposPrimitivos.tipoNumero || type == TiposPrimitivos.tipoString ||
                   type == TiposPrimitivos.tipoTime ;
        }



        public static ZContenedor accederElementoLista(ZContenedor ambito, Instruccion exp1, Instruccion exp2)
        {
            var z1 = desenvolver(exp1.ejecutar(ambito));
            var z2 = desenvolver(exp2.ejecutar(ambito));
            
            if (!(z1 is ZLista zLista))
                throw new SemanticError($"Operador de accesso [] no valido para acceder tipo '{z1.Origen.Nombre}' ");

            if (!(z2 is ZNumero zNumero))
                throw new SemanticError($"Operador de accesso [] tiene que tener valores enteros, no de tipo '{z2.Origen.Nombre}'");
            
            return zLista.obtenerElemento(zNumero.obtenerValor());

        }
        
        
        public static void cargarFuncionesNativas(ZContenedor e)
        {
            
            var countZFun = new FuncionNativa(
                TiposPrimitivos.tipoNumero, 
                crearListaParametros(TiposPrimitivos.tipoResultadoQuery), 
                e,
                args =>
                {
                    var resultadoQuery = (ZResultadoQuery) args[0];
                    return new ZNumero(resultadoQuery.Resultado.Count);
                }
                );
            
            
            
            var sumZFun = new FuncionNativa(
                TiposPrimitivos.tipoDicimal,
                crearListaParametros(TiposPrimitivos.tipoResultadoQuery),
                e,
                args =>
                {
                    var resultado = ((ZResultadoQuery) args[0]).Resultado;
                    if (resultado.Count == 0)
                    {
                        return new ZDecimal(0);
                    }

                    tieneUnaSolaColumna(resultado);

                    double total = 0;

                    foreach (var fila in resultado)
                    {

                        switch (fila[0])
                        {
                            case ZNumero zNumero:
                                total += zNumero.obtenerValor();
                                continue;
                            case ZDecimal zDecimal:
                                total += zDecimal.obtenerValor();
                                continue;
                            default:
                                throw new SemanticError($"Todos los valores deben de ser numericos en 'sum'. Encontrado '{fila[0].Origen.Nombre}'");
                        }
                    }

                    return new ZDecimal(total);
                }
                );
            
            
            var avgZFun = new FuncionNativa(
                TiposPrimitivos.tipoDicimal,
                crearListaParametros(TiposPrimitivos.tipoResultadoQuery),
                e,
                args =>
                {
                    var resultado = ((ZResultadoQuery) args[0]).Resultado;
                    if (resultado.Count == 0)
                    {
                        return new ZDecimal(0);
                    }

                    tieneUnaSolaColumna(resultado);
                    
                    double total = 0;

                    foreach (var fila in resultado)
                    {
                        switch (fila[0])
                        {
                            case ZNumero zNumero:
                                total += zNumero.obtenerValor();
                                continue;
                            case ZDecimal zDecimal:
                                total += zDecimal.obtenerValor();
                                continue;
                            default:
                                throw new SemanticError($"Todos los valores deben de ser numericos en 'sum'. Encontrado '{fila[0].Origen.Nombre}'");
                        }
                    }
                    return new ZDecimal(total / resultado.Count);
                }
            );
            
            
            
            
            var maxZFun = new FuncionNativa(
                TiposPrimitivos.tipoDicimal,
                crearListaParametros(TiposPrimitivos.tipoResultadoQuery),
                e,
                args =>
                {
                    var resultado = ((ZResultadoQuery) args[0]).Resultado;
                    if (resultado.Count == 0)
                    {
                        return new ZDecimal(0);
                    }

                    tieneUnaSolaColumna(resultado);

                    DateTime? maxTiempo = null;
                    DateTime? maxDate = null;
                    double? maximo = null;

                    foreach (var fila in resultado)
                    {
                        switch (fila[0])
                        {
                            case ZNumero zNumero:
                                if (maximo == null)
                                {
                                    maximo = zNumero.obtenerValor();
                                }
                                else
                                {
                                    if (zNumero.obtenerValor() > maximo)
                                    {
                                        maximo = zNumero.obtenerValor();
                                    }
                                }
                                continue;
                            case ZDecimal zDecimal:
                                if (maximo == null)
                                {
                                    maximo = zDecimal.obtenerValor();
                                }
                                else
                                {
                                    if (zDecimal.obtenerValor() > maximo)
                                    {
                                        maximo = zDecimal.obtenerValor();
                                    }
                                }
                                continue;
                            case ZDate zDate:
                                if (maxDate == null)
                                {
                                    maxDate = zDate.obtenerValor();
                                }
                                else
                                {
                                    if (zDate.obtenerValor() > maxDate)
                                    {
                                        maxDate = zDate.obtenerValor();
                                    }
                                }
                                continue;
                            case ZTiempo zTiempo:
                                
                                if (maxTiempo == null)
                                {
                                    maxTiempo = zTiempo.obtenerValor();
                                }
                                else
                                {
                                    if (zTiempo.obtenerValor() > maxTiempo)
                                    {
                                        maxTiempo = zTiempo.obtenerValor();
                                    }
                                }
                                
                                continue;

                            default:
                                throw new SemanticError($"Tipo no valido para 'max'. Encontrado '{fila[0].Origen.Nombre}'");
                        }
                    }
                    
                    if (maximo != null)
                        return new ZDecimal(maximo.Value);
                    if (maxDate != null)
                        return new ZDate(maxDate.Value);
                    if (maxTiempo != null)
                        return new ZTiempo(maxTiempo.Value);

                    return TiposPrimitivos.instanicaNulo;
                }
            );
            
            var minZFun = new FuncionNativa(
                TiposPrimitivos.tipoDicimal,
                crearListaParametros(TiposPrimitivos.tipoResultadoQuery),
                e,
                args =>
                {
                    var resultado = ((ZResultadoQuery) args[0]).Resultado;
                    if (resultado.Count == 0)
                    {
                        return new ZDecimal(0);
                    }

                    tieneUnaSolaColumna(resultado);

                    DateTime? maxTiempo = null;
                    DateTime? maxDate = null;
                    double? maximo = null;

                    foreach (var fila in resultado)
                    {
                        switch (fila[0])
                        {
                            case ZNumero zNumero:
                                if (maximo == null)
                                {
                                    maximo = zNumero.obtenerValor();
                                }
                                else
                                {
                                    if (zNumero.obtenerValor() < maximo)
                                    {
                                        maximo = zNumero.obtenerValor();
                                    }
                                }
                                continue;
                            case ZDecimal zDecimal:
                                if (maximo == null)
                                {
                                    maximo = zDecimal.obtenerValor();
                                }
                                else
                                {
                                    if (zDecimal.obtenerValor() < maximo)
                                    {
                                        maximo = zDecimal.obtenerValor();
                                    }
                                }
                                continue;
                            case ZDate zDate:
                                if (maxDate == null)
                                {
                                    maxDate = zDate.obtenerValor();
                                }
                                else
                                {
                                    if (zDate.obtenerValor() < maxDate)
                                    {
                                        maxDate = zDate.obtenerValor();
                                    }
                                }
                                continue;
                            case ZTiempo zTiempo:
                                
                                if (maxTiempo == null)
                                {
                                    maxTiempo = zTiempo.obtenerValor();
                                }
                                else
                                {
                                    if (zTiempo.obtenerValor() < maxTiempo)
                                    {
                                        maxTiempo = zTiempo.obtenerValor();
                                    }
                                }
                                
                                continue;

                            default:
                                throw new SemanticError($"Tipo no valido para 'min'. Encontrado '{fila[0].Origen.Nombre}'");
                        }
                    }
                    
                    if (maximo != null)
                        return new ZDecimal(maximo.Value);
                    if (maxDate != null)
                        return new ZDate(maxDate.Value);
                    if (maxTiempo != null)
                        return new ZTiempo(maxTiempo.Value);

                    return TiposPrimitivos.instanicaNulo;
                }
            );
            
            
            e.declararFuncion("count", countZFun);
            e.declararFuncion("sum", sumZFun);
            e.declararFuncion("avg", avgZFun);
            e.declararFuncion("max", maxZFun);
            e.declararFuncion("min", minZFun);
            



        }

        private static void tieneUnaSolaColumna(List<List<ZContenedor>> resultado)
        {
            if (resultado[0].Count != 1)
            {
                throw new SemanticError($"Funcion de agregación necesita de una sola columna");
            }
        }

        public static List<NodoDeclararParametro> crearListaParametros(params TeDeU[] teDeUs)
        {
            return teDeUs
                .Select(t =>  new NodoDeclararParametro(new NodoSimple(a => t), ""))
                .ToList();
        }
        
        
    }
}