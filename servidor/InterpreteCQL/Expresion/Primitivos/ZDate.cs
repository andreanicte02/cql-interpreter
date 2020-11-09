using System;
using System.Collections.Generic;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZDate:ZContenedor
    {
        private DateTime valor;
        
        
        public ZDate(string valor) : base(null, TiposPrimitivos.tipoDate)
        {
            try
            {
                
                this.valor = Convert.ToDateTime(valor);

            }
            catch (Exception e)
            {
                throw new SemanticError(" enrror al crear un tipo date");
            }
             
            loadgetYear();
            loadgetMonth();
            loadGetDay();
        }

        public ZDate(DateTime valor) : base(null, TiposPrimitivos.tipoDate)
        {
            this.valor = valor;
            loadgetYear();
            loadgetMonth();
            loadGetDay();
        }

        public DateTime obtenerValor()
        {
            return valor;
        }

        public override string stringBonito()
        {
            return valor.ToString("yyyy-MM-dd");
        }

        public void loadgetYear()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Year);       

                }

            );
            
            declararFuncion("getyear",f);

        }
        
        public void loadgetMonth()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Month);       

                }

            );
            
            declararFuncion("getmonth",f);

        }
        
        public void loadGetDay()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Day);       

                }

            );
            
            declararFuncion("getday",f);

        }

        
        
    }
}