using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteChison
{
    public class NodoData:Instruccion
    {
        private List<NodoFila> filas; // cada nodo fila tiene una fila de datos

        public List<NodoFila> Filas
        {
            get => filas;
           
        }


        public NodoData(List<NodoFila> filas)
        {
            this.filas = filas;
        }


       

        public override object ejecutarSinposicion(ZContenedor e)
        {
            
            foreach (NodoFila nodo in filas)
            {
                
                nodo.ejecutar(e);
            }

            return null;
        }
    }
}