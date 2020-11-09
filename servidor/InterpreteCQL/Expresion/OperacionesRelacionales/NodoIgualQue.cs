using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesRelacionales
{
    public class NodoIgualQue:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoIgualQue(Instruccion i1, Instruccion i2)
        {
            
            this.i1 = i1;
            this.i2 = i2;
        }

        public static ZBool IgualIgual(ZContenedor valor1, ZContenedor valor2)
        {
            if(valor1 is ZBool b1 && valor2 is ZBool b2){
                
                return new ZBool(b1.obtenerValor() == b2.obtenerValor());
            }
            
            if (valor1 is ZCadena c1 && valor2 is ZCadena c2){
                
                return  new ZBool(c1.obtenerValor()==c2.obtenerValor());
            }
            
            if(valor1 is ZDecimal d1 && valor2 is ZDecimal d2){
                
                return  new ZBool(d1.obtenerValor()==d2.obtenerValor());
            }

            if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
            {
                return  new ZBool(n1.obtenerValor()==n2.obtenerValor());

            }
            
            if (valor1 is ZNumero n3 && valor2 is ZDecimal d3){
                
                return  new ZBool(n3.obtenerValor()==d3.obtenerValor());
            }
            
            if (valor1 is ZDecimal d4 && valor2 is ZNumero n4 ){
                    
                return  new ZBool(d4.obtenerValor()==n4.obtenerValor());

            }

            if (valor1 is ZDate d5 && valor2 is ZDate n5)
            {
                return new ZBool(d5.obtenerValor().Equals(n5.obtenerValor()));
            }
            
            if (valor1 is ZTiempo d6 && valor2 is ZTiempo n6)
            {
                return new ZBool(d6.obtenerValor().Equals(n6.obtenerValor()));
            }

            if ((valor1 is ZInstancia|| valor1.Origen == TiposPrimitivos.tipoNulo) && (valor2 is ZInstancia || valor2.Origen == TiposPrimitivos.tipoNulo))
            {
                return new ZBool(valor1 == valor2);
            }

            if (valor1 is ZCadena d9 && valor2 is ZNull)
            {
                return new ZBool(d9.obtenerValor() == null);
            }

            if (valor1 is ZNull && valor2 is ZCadena d10 )
            {
                return new ZBool(d10.obtenerValor() == null);
            }

            return null;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            
            var algo1 = i1.ejecutar(e);
            var algo2 = i2.ejecutar(e);

            ZContenedor valor1 = Utilidades.desenvolver(algo1);
            ZContenedor valor2 = Utilidades.desenvolver(algo2);

            var resultado = IgualIgual(valor1, valor2);

            return resultado ?? throw new SemanticError("Error; operacion == | != no compatible con tipos");
        }
        
        
    }
    
    public class NodoDiferenteQue:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoDiferenteQue(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            NodoIgualQue valor = new NodoIgualQue(i1,i2);
            var algo = valor.ejecutarSinposicion(e);
            if (algo is ZBool a)
            {
                return  new ZBool(!a.obtenerValor());
            }
            
            throw new SemanticError("Error != operacion != no compatible con tipos");

        }
    }
}