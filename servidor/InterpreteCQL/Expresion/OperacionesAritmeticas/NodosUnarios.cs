using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesAritmeticas
{
    public class NodoUnarioResta:Instruccion
    {
        private readonly Instruccion i1;

        public NodoUnarioResta(Instruccion i1)
        {
            this.i1 = i1;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);

            ZContenedor valor1 = Utilidades.desenvolver(algo1);

            if (valor1 is ZNumero n1)
            {
                return new ZNumero(n1.obtenerValor() * -1);
                
            }
            
            if (valor1 is ZDecimal n2)
            {
                return    new ZDecimal( n2.obtenerValor() *-1);
            }
            
            throw new SemanticError("Error operacion -ex; tipo no compatibles");
        }
    }

    public class NodoUnarioSuma:Instruccion
    {
        private readonly Instruccion i1;

        public NodoUnarioSuma(Instruccion i1)
        {
            this.i1 = i1;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var algo1 = i1.ejecutar(e);

            ZContenedor valor1 = Utilidades.desenvolver(algo1);

            if (valor1 is ZNumero n1)
            {
                return new ZNumero(n1.obtenerValor() * +1);
                
            }
            
            if (valor1 is ZDecimal n2)
            {
                return    new ZDecimal( n2.obtenerValor() *+1);
            }
            
            throw new SemanticError("Error operacion +ex; tipo no compatibles");
        }
        
    }

    //------------------------incoremento decremento
    public class NodoDecremento:Instruccion
    {
        private readonly Instruccion i1;

        public NodoDecremento(Instruccion i1)
        {
            this.i1 = i1;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var val = Utilidades.desenvolver(s1);

                if (val is ZNumero n1)
                {
                   ZNumero novo = new ZNumero(n1.obtenerValor()-1);
                   s1.definirValor(novo);
                   return new ZNumero(n1.obtenerValor());
                }

                if (val is ZDecimal n2)
                {
                    ZDecimal novo = new ZDecimal(n2.obtenerValor() -1);
                    s1.definirValor(novo);
                    return new ZDecimal(n2.obtenerValor());;
                }
                throw new SemanticError("error en exp--; tipos no compatbiles");
            }
            
            throw new SemanticError("el lado izquierdo no es un simbolo exp--");
            
           
        }
    }

    public class NodoIncremento:Instruccion
    {
        private readonly Instruccion i1;
        
        public NodoIncremento(Instruccion i1)
        {
            this.i1 = i1;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var val = Utilidades.desenvolver(s1);

                if (val is ZNumero n1)
                {
                    ZNumero novo = new ZNumero(n1.obtenerValor()+1);
                    s1.definirValor(novo);
                    return new ZNumero(n1.obtenerValor());
                }

                if (val is ZDecimal n2)
                {
                    ZDecimal novo = new ZDecimal(n2.obtenerValor() +1);
                    s1.definirValor(novo);
                    return new ZDecimal(n2.obtenerValor());;
                }
                
                throw new SemanticError("error en exp++; tipos no compatbiles");
            }
            
            throw  new SemanticError("el lado izquierdo no es un simbolo exp++");
            
           
        }
    }
}