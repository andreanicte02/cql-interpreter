using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{
    public class Retorno
    {
        private TeDeU tipo;
        private ZContenedor valor;

        public TeDeU Tipo
        {
            get => tipo;

        }
        
       
        public ZContenedor Valor
        {
            get => valor;
        
        }

        public Retorno(TeDeU tipo, ZContenedor valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

       
        
        


    }
}