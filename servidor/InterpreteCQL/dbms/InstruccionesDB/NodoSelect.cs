using System;
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.InstruccionesDB
{
    public class NodoSelect:Instruccion
    {
        private List<Instruccion> nodosExp;
        private Instruccion tabla;
        private Instruccion where;


        public NodoSelect(List<Instruccion> nodosExp, Instruccion tabla, Instruccion @where)
        {
            this.nodosExp = nodosExp;
            this.tabla = tabla;
            this.@where = @where;
        
        }


        public override object ejecutarSinposicion(ZContenedor e)
        {
            var tab = (Tabla) tabla.ejecutar(e);
            var result = procesarFilas(tab.Filas);
            
            imprimirTabla(result);
            
            return new ZResultadoQuery(result);
        }

        public  List<List<ZContenedor>>  procesarFilas(  List<ZFila> zfilas )
        {
            List<List<ZContenedor>> result = new List<List<ZContenedor>>();
            foreach (ZFila fila in zfilas)
            {
                bool bandera = true;
                
                List<ZContenedor> contedores;

                contedores = nodosExp == null?fila.obtenerValorFila(): Utilidades.desnvolverArgumento(nodosExp,fila);
                
                
                if (@where != null)
                {
                    var algo = @where.ejecutar(fila);
                    ZContenedor valor = Utilidades.desenvolver(algo);
                    bandera = ejecutarWhere(valor);
                }

                if (bandera)
                {
                    result.Add(contedores);
                }


//                //con where
//                if (bandera  && @where != null)
//                {
//                    result.Add(contedores);
//                    continue;
//                }
//                
//                //sin where
//                if (!bandera && @where == null)
//                {
//                    result.Add(contedores);
//                }
                
                

            }

            return result;

        }
        
        
      

        public static bool ejecutarWhere(ZContenedor valor)
        {
            if (valor is ZBool v1)
            {
                if (v1.obtenerValor())
                {
                    return true;
                }

            }else if (valor is ZNumero v2)
            {
                if (v2.obtenerValor() > 0)
                {
                    return true;
                }

            }
            else
            {
                throw new SemanticError("error al evaluar el whre");
            }

            return false;

        }


        public static void imprimirTabla(List<List<ZContenedor>> salida)
        {

            foreach (List<ZContenedor> uno in salida)
            {
                foreach (ZContenedor dos in uno)
                {
                    if (Utilidades.esPrimitivo(dos.Origen))
                    {
                        Console.Write($"{dos.stringBonito()} |");
                    }

                    if (dos is ZNull)
                    {
                        Console.Write($"{dos.stringBonito()} |");
                    }

                    if (dos is ZInstancia)
                    {
                        Console.Write(escribirContenedor(dos)+" |");
                    }

                }
                Console.WriteLine("");
                
            }
        }
        
        public static string escribirContenedor(ZContenedor contenedor)
        {
            string result = " < ";
            bool primero = false;
            
            foreach (KeyValuePair<string, Simbolo> nodo in contenedor.TablaSimbolo)
            {

                if (!primero)
                {
                    result += escrbiriAtributosContenedor(nodo.Key, nodo.Value);
                    primero = true;
                    continue;
                }

                result += ",";
                result += escrbiriAtributosContenedor(nodo.Key, nodo.Value);




            }

            result += ">";
            return result;

        }

        public static  string escrbiriAtributosContenedor(string nombre, Simbolo nodo)
        {
            string result = "";
            TeDeU type = nodo.obtenerInstanciaTipo();
            if (Utilidades.esPrimitivo(type))
            {
                return  $"{nodo.obtenerValor().stringBonito()} ";
            }

            if (nodo.obtenerValor() is ZInstancia instancia)
            {
                
                result += escribirContenedor(instancia);
               
                return result;

            }

            if (nodo.obtenerValor() is ZNull)
            {
                return  $"{nodo.obtenerValor().stringBonito()}";
            }




            return result;
        }

    }
}