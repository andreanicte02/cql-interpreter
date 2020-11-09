using System;
using System.Collections.Generic;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;
using Utilidades = servidor.InterpreteChison.Utils.Utilidades;

namespace servidor.InterpreteChison
{
    public class NodoBase:Instruccion
    {
        private Instruccion bd;
        private List<Instruccion> data; //--> lista de --> < lista de atrr >

        public NodoBase(Instruccion bd, List<Instruccion> data)
        {
            Console.Write("");
            this.bd = bd;
            this.data = data;
        }

        public override object ejecutarSinposicion(ZContenedor e)
        {
            BaseDeDatos bd =(BaseDeDatos) this.bd.ejecutar(e);
            Utilidades.ejecutarSenteciass(data, e);
            
            return null;
        }
    }
}