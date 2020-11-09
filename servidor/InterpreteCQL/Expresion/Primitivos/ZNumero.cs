using System;
using servidor.InterpreteCQL.Expresion;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class ZNumero:ZContenedor {
    private  int valor;
    public bool isCount = false;

    public ZNumero(int valor) : base(null, TiposPrimitivos.tipoNumero)
    {
        this.valor = valor;
    }
    
    public int obtenerValor() {
        return valor;
    }
    
    public override string stringBonito()
    {
        return valor + "";
    }


    public void definirValor(int valor)
    {
        this.valor = valor;
    }

    }
}