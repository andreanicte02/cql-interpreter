using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;

namespace servidor.InterpreteCQL
{
    public class Simbolo
    {
        //contenedor
        private TeDeU instanciaTipo;
        private ZContenedor valor;
        
        public Simbolo(TeDeU instanciaTipo, ZContenedor valorInicial){
            this.instanciaTipo = instanciaTipo;
            valor = valorInicial;
        }
        
        public TeDeU obtenerInstanciaTipo(){
            return instanciaTipo;
        }
        
        public ZContenedor obtenerValor(){
            return valor;
        }
        
        public void definirValor(ZContenedor valor){
            this.valor = valor;
        }
        
        
    }
}