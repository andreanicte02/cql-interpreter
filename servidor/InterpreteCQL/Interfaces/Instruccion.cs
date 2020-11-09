using System;
using Irony.Parsing;

namespace servidor.InterpreteCQL.Interfaces
{
    public abstract class Instruccion
    {
    
        private string nombreArchivo { get; }
        
        SourceLocation location;

        public SourceLocation Location
        {
            
            set => location = value;
        }


        public Instruccion()
        {
     
           nombreArchivo = "";
        }

        public abstract Object ejecutarSinposicion(ZContenedor e) ;

        public object ejecutar(ZContenedor e)
        {
            try
            {
                return ejecutarSinposicion(e);
            }
            catch (SemanticError exception)
            {
                Console.WriteLine(exception.Message + " | linea: " + location.Line + " columna: " + location.Column);
                throw exception;
            }
        }
    }
}