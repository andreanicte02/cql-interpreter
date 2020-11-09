using System;
using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZCadena:ZContenedor
    {
        private readonly string valor;
        
        public ZCadena(string valor) : base(null, TiposPrimitivos.tipoString)
        {
            
            this.valor = valor;
            
            
            
            loadLenght();
            loadToLower();
            lodadToUpper(); 
            loadStartsWith();
            loadEndsWithWith();
            loadSubString();
        }
        
        public string obtenerValor() {

            return valor;
        }

        public override string stringBonito()
        {
            return valor + "";
        }

        public void loadLenght()
        {
            
            List<Instruccion> ins =new List<Instruccion>();
            List<Instruccion> exp = new List<Instruccion> {new NodoCrearNumero(valor.Length)};
            ins.Add(new NodoRetornar(exp));
            Funcion f = new Funcion(TiposPrimitivos.tipoNumero, new List<NodoDeclararParametro>(),ins,this);
            declararFuncion("length",f);
            
        }
        
        public void loadToLower()
        {
            List<Instruccion> ins =new List<Instruccion>();
            List<Instruccion> exp = new List<Instruccion> {new NodoCrearCadena(valor.ToLower())};
            ins.Add(new NodoRetornar(exp));
            Funcion f = new Funcion(TiposPrimitivos.tipoString, new List<NodoDeclararParametro>(),ins,this);
            declararFuncion("tolowercase",f);
        }
        
        public void lodadToUpper()
        {
            List<Instruccion> ins =new List<Instruccion>();
            List<Instruccion> exp = new List<Instruccion> {new NodoCrearCadena(valor.ToUpper())};
            ins.Add(new NodoRetornar(exp));
            
            Funcion f = new Funcion(TiposPrimitivos.tipoString, new List<NodoDeclararParametro>(),ins,this);
            declararFuncion("touppercase",f);
        }

        public void loadStartsWith()
        {
            
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
            parametros.Add(new NodoDeclararParametro(new NodoObtenerTeDeU("string"), "f"));

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoBool, parametros, this,
                list =>
                {
                    ZCadena arg = (ZCadena)list[0];
                   
                    return new ZBool(valor.StartsWith(arg.obtenerValor()));       

                }

            );
            
            declararFuncion("startswith",f);


        }
        
        
        public void loadEndsWithWith()
        {
            
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
            parametros.Add(new NodoDeclararParametro(new NodoObtenerTeDeU("string"), "f"));

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoBool, parametros, this,
                list =>
                {
                    ZCadena arg = (ZCadena)list[0];
                
                    return new ZBool(valor.EndsWith(arg.obtenerValor()));       
                    
                }

            );
            
            declararFuncion("endswith",f);


        }
        
        public void loadSubString()
        {
            
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
            parametros.Add(new NodoDeclararParametro(new NodoObtenerTeDeU("int"), "f1"));
            parametros.Add(new NodoDeclararParametro(new NodoObtenerTeDeU("int"), "f2"));
            
            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoString, parametros, this,
                list =>
                {
                    ZNumero arg1 = (ZNumero) list[0];
                    ZNumero arg2 = (ZNumero) list[1];
                  
                        
                    return new ZCadena(valor.Substring(arg1.obtenerValor(), arg2.obtenerValor()));


                }

            );
            
            declararFuncion("substring",f);


        }
        
        
        
        
    }
}