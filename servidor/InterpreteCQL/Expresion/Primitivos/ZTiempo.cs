using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.Instruccionesn;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class ZTiempo: ZContenedor
    {
        private DateTime valor;

        public ZTiempo(string valor) : base(null, TiposPrimitivos.tipoTime)
        {

            try
            {
                this.valor = Convert.ToDateTime(valor);

            }
            catch (Exception e)
            {
                throw new SemanticError("error al intentar crear un tipotime");
            }
           
            loadGeMin();
            loadGetHour();
            loadGetSecond();
            
        }

        public ZTiempo(DateTime valor) : base(null, TiposPrimitivos.tipoTime)
        {
            this.valor = valor;
            loadGeMin();
            loadGetHour();
            loadGetSecond();
        }

        public DateTime obtenerValor()
        {
            return valor;
        }

        public override string stringBonito()
        {
            return valor.ToString("HH:mm:ss");
        }
        
        public void loadGetHour()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Hour);       

                }

            );
            
            declararFuncion("gethour",f);

        }
        
        public void loadGeMin()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Minute);       

                }

            );
            
            declararFuncion("getminuts",f);

        }
        
        
        public void loadGetSecond()
        {
            List<NodoDeclararParametro> parametros = new List<NodoDeclararParametro>();
          

            FuncionNativa f = new FuncionNativa(TiposPrimitivos.tipoNumero, parametros, this,
                list =>
                {
                 
                   
                    return new ZNumero(valor.Second);       

                }

            );
            
            declararFuncion("getseconds",f);

        }
        
        
        
    }
}