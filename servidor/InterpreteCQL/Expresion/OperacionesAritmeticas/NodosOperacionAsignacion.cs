using System;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Expresion.OperacionesAritmeticas
{
    public class NodoAsignarOperarMas:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoAsignarOperarMas(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var valor1 = Utilidades.desenvolver(s1);
                var algo2 = i2.ejecutar(e);

                var valor2 = Utilidades.desenvolver(algo2);

                if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
                {
                    
                    ZNumero novo =  new ZNumero(n1.obtenerValor() + n2.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZDecimal n11 && valor2 is ZDecimal n12)
                {
                    ZDecimal novo =  new ZDecimal(n11.obtenerValor() + n12.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZNumero n111 && valor2 is ZDecimal n122)
                {
                    throw new SemanticError("tipos no compatibles");
                
                }
                if (valor1 is ZDecimal n11a && valor2 is ZNumero n12b)
                {
                    ZDecimal novo = new ZDecimal(n11a.obtenerValor() + n12b.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                throw  new SemanticError("tipos no compatiples exp+=");
            }
            
            throw  new SemanticError("el lado izquierdo no es un simbolo exp+=");
            
        }
    }

    public class NodoAsignarOperarMenos: Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoAsignarOperarMenos(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var valor1 = Utilidades.desenvolver(s1);
                var algo2 = i2.ejecutar(e);

                var valor2 = Utilidades.desenvolver(algo2);

                if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
                {
                    
                    ZNumero novo =  new ZNumero(n1.obtenerValor() - n2.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZDecimal n11 && valor2 is ZDecimal n12)
                {
                    ZDecimal novo =  new ZDecimal(n11.obtenerValor() - n12.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZNumero n111 && valor2 is ZDecimal n122)
                {
                    throw new SemanticError("tipos no compatibles");
                
                }
                if (valor1 is ZDecimal n11a && valor2 is ZNumero n12b)
                {
                    ZDecimal novo = new ZDecimal(n11a.obtenerValor() - n12b.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                
                throw  new SemanticError("tipos no compatiples exp-=");
            }
            
            throw  new SemanticError("el lado izquierdo no es un simbolo exp-=exp");
            
        }
    }

    public class NodoAsignarOperarPor:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoAsignarOperarPor(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var valor1 = Utilidades.desenvolver(s1);
                var algo2 = i2.ejecutar(e);

                var valor2 = Utilidades.desenvolver(algo2);

                if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
                {
                    
                    ZNumero novo =  new ZNumero(n1.obtenerValor() * n2.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZDecimal n11 && valor2 is ZDecimal n12)
                {
                    ZDecimal novo =  new ZDecimal(n11.obtenerValor() * n12.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZNumero n111 && valor2 is ZDecimal n122)
                {
                    throw new SemanticError("tipos no compatibles");
                
                }
                if (valor1 is ZDecimal n11a && valor2 is ZNumero n12b)
                {
                    ZDecimal novo = new ZDecimal(n11a.obtenerValor() * n12b.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                
                throw  new SemanticError("tipos no compatiples exp-=");
            }
            
            throw  new SemanticError("el lado izquierdo no es un simbolo exp-=exp");
            
        }
    }

    public class NodoAsignarOperarDiv:Instruccion
    {
        private readonly Instruccion i1;
        private readonly Instruccion i2;

        public NodoAsignarOperarDiv(Instruccion i1, Instruccion i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }
        
        public override object ejecutarSinposicion(ZContenedor e)
        {
            var sim = i1.ejecutar(e);
            if (sim is Simbolo s1)
            {
                var valor1 = Utilidades.desenvolver(s1);
                var algo2 = i2.ejecutar(e);

                var valor2 = Utilidades.desenvolver(algo2);

                if (valor1 is ZNumero n1 && valor2 is ZNumero n2)
                {
                    NodoDivision.isZero(n2.obtenerValor());
                    
                    ZNumero novo =  new ZNumero(n1.obtenerValor() / n2.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZDecimal n11 && valor2 is ZDecimal n12)
                {
                    NodoDivision.isZero(n12.obtenerValor());
                    
                    ZDecimal novo =  new ZDecimal(n11.obtenerValor() / n12.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                if (valor1 is ZNumero n111 && valor2 is ZDecimal n122)
                {
                    throw new SemanticError("tipos no compatibles");
                
                }
                if (valor1 is ZDecimal n11a && valor2 is ZNumero n12b)
                {
                    NodoDivision.isZero(n12b.obtenerValor());
                    
                    ZDecimal novo = new ZDecimal(n11a.obtenerValor() / n12b.obtenerValor());
                    s1.definirValor(novo);
                    return novo;

                }
                
                throw  new SemanticError("tipos no compatiples exp-=");
            }
            
            throw  new SemanticError("el lado izquierdo no es un simbolo exp-=exp");
            
        }
        
        
    }


}