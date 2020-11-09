
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.Expresion.Primitivos;

namespace servidor.InterpreteCQL.dbms
{
    
    /// <summary>
    /// Esta clase se encarga de guardar las instancias de teDeUs que representan a las listas.
    /// Si el teDeU no existe se crea una, si ya existe se vuelve a reutilizar, de esta forma
    /// la comparacion de tipos sigue siendo segura por que es a nivel de referencia.
    /// </summary>
    public class TeDeUListaCache
    {
        private readonly List<Entry> entries = new List<Entry>();


        public TeDeU obtenerTedeULista(TeDeU teDeUElementoLista)
        {
            var entryResult = entries.Find(entry => entry.Key == teDeUElementoLista);

            if (entryResult != null) 
                return entryResult.Value;
            
            var teDeULista = new TeDeULista($"list<{teDeUElementoLista.Nombre}>", teDeUElementoLista);
            entries.Add(new Entry(teDeUElementoLista, teDeULista));
            return teDeULista;
        }
        
        private class Entry
        {
            public Entry(TeDeU key, TeDeU value)
            {
                Key = key;
                Value = value;
            }

            public TeDeU Key { get;  }
            public TeDeU Value { get;  }
        }
    }

    public class TeDeULista : TeDeU
    {
        public TeDeU TipoElemento { get; }

        public TeDeULista(string nombre, TeDeU tipoElemento) : base(nombre)
        {
            this.TipoElemento = tipoElemento;
        }
    }
}