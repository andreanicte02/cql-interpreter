using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instrucciones
{
    public class NodoDeclararParametro:Instruccion
    {
        private Instruccion tipo;

        

        private string id;

        public string Id => id;

        public NodoDeclararParametro(Instruccion tipo, string id)
        {
            this.tipo = tipo;
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            TeDeU tipo = (TeDeU)this.tipo.ejecutar(e);
            
            Simbolo sim = new Simbolo(tipo,null);
            //se agrega el valor inicial
            Utilidades.AsignarValorInicial(sim);
            
            e.setVariable(id,sim);

            return sim;
        }
        
        public Instruccion Tipo
        {
            get => tipo;
           
        }
    }
}