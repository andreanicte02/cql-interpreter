using System;

namespace servidor.InterpreteCQL
{
    /// <summary>
    /// Clase encargad del manejo de errores
    /// </summary>
    public class SemanticError:Exception
    {
        
        public SemanticError(String descripcion) : base(descripcion)
        {
            
        }
    }
}