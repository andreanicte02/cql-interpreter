using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class NodoBuscarId:Instruccion
    {
        private readonly string id;

        public string Id => id;

        public NodoBuscarId(string id)
        {
            this.id = id;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            Simbolo sim = Utilidades.buscarId(e, id);
            if (sim == null)
            {
                throw new SemanticError("Id: '"+ id + "' no se encuentra declarado o no es accesible desde aqui");
            }

            return sim;
        }
        
        
    }
}