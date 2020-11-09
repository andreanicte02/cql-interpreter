using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CSharp.RuntimeBinder;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.dbms.InstruccionesChison
{
    public class NodoCommit : Instruccion
    {
        private string co = "\"";
        private string tab = "\t";
        private ZContenedor ambitoCaputrado;

        public override object ejecutarSinposicion(ZContenedor e)
        {
            ambitoCaputrado = e;

            StreamWriter archivo = new StreamWriter(@"./../../chison.txt");
            archivo.Write("$<\n");

            archivo.Write(escribirBasesDatos());
            archivo.Write(writeUsers());

            archivo.Write(">$");
            archivo.Close();

            return null;
        }

        public string escribirBasesDatos()
        {
            string result = "";

            result += $"{co}DATABASE{co}=[\n";
            bool primero = true;

            foreach (KeyValuePair<string, BaseDeDatos> bd in Dbms.Bases)
            {

                if (primero)
                {
                    result += escribirBD(bd.Value, primero);
                    primero = false;
                    continue;
                }

                result += escribirBD(bd.Value, primero);


            }

            result += "]\n";
            return result;

        }

        public string escribirBD(BaseDeDatos bd, bool primero)
        {

            string result = "";

            result += primero ? $"{tab}<\n" : $"{tab},<\n";


            result += $"{tab}{tab}";
            result += $"{co}NAME{co}={co}{bd.Nombre}{co},\n";
            result += $"{tab}{tab}";
            result += $"{co}DATA{co}=[\n";

            result += writeTypesCql(bd);

            result += $"{tab}{tab}";
            result += "]\n";
            result += $"{tab}>";
            return result;
        }

        public string writeUsers()
        {
            string result = "";

            result += $"{co}USERS{co}=[\n";

            bool primero = true;
            foreach (KeyValuePair<string, string> usuario in Dbms.Usuarios)
            {
                if (primero)
                {
                    result += writeUser(usuario.Key, usuario.Value, primero);
                    primero = false;
                    continue;

                }

                result += writeUser(usuario.Key, usuario.Value, primero);
            }

            result += "]\n";

            return result;

        }

        public string writeUser(string id, string pass, bool primero)
        {
            string result = "";

            result += primero ? $"{tab}<\n" : $"{tab},<\n";

            result += $"{tab}{tab}";
            result += $"{co}NAME{co}={co}{id}{co},\n";
            result += $"{tab}{tab}";
            result += $"{co}PASSWORD{co}={co}{pass}{co},\n";
            result += $"{tab}{tab}";
            result += $"{co}PERMISSIONS{co}=[\n";

            result += writPermissions(id);

            result += $"{tab}{tab}";
            result += "]\n";
            result += $"{tab}>";

            return result;

        }

        public string writPermissions(string usuario)
        {
            string result = "";
            bool primero = true;



            foreach (BdUsuarios nodo in Dbms.permisosUsuarios(usuario))
            {


                result += primero ? $"{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab},<\n";
                primero = false;

                result += $"{tab}{tab}{tab}{tab}";
                result += $"{co}NAME{co}={co}{nodo.Bd}{co}\n";

                result += $"{tab}{tab}{tab}>\n";


            }



            return result;
        }

        public string writeTypesCql(BaseDeDatos bd)
        {
            string result = "";
            bool primero = true;

            foreach (KeyValuePair<string, TeDeU> nodo in bd.Tdeus)
            {
                if (Utilidades.esPrimitivo(nodo.Value))
                {
                    continue;
                }

                result += writeObject(nodo.Value, primero);
                primero = false;
            }

            foreach (KeyValuePair<string, Tabla> nodo in bd.Tablas)
            {
                result += writeTable(nodo.Value, primero, nodo.Key);

            }

            foreach (KeyValuePair<string, AgenteProcedimiento> nodo in bd.AgentesProcedimiento)
            {
                foreach (Procedimiento proc in nodo.Value.ListaProcedimientos)
                {
                    result += writeProcedimiento(proc, primero, nodo.Key);
                }
            }


            return result;
        }

        public string writeObject(TeDeU tedeu, bool primero)
        {
            string result = "";

            result += primero ? $"{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab},<\n";

            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}CQL-TYPE{co}={co}OBJECT{co},\n";

            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}NAME{co}={co}{tedeu.Nombre}{co},\n";


            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}ATTRS{co}=[\n";

            bool primeroAtr = true;

            foreach (NodoDeclararParametro nodo in tedeu.ListaDelcaraciones)
            {

                result += primeroAtr ? $"{tab}{tab}{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab}{tab}{tab},<\n";

                result += $"{tab}{tab}{tab}{tab}{tab}{tab}";
                result += $"{co}NAME{co}={co}{nodo.Id}{co},\n";

                result += $"{tab}{tab}{tab}{tab}{tab}{tab}";
                TeDeU type = (TeDeU) nodo.Tipo.ejecutar(ambitoCaputrado);

                result += $"{co}TYPE{co}={co}{type.Nombre}{co}\n";


                result += $"{tab}{tab}{tab}{tab}{tab}>\n";
                primeroAtr = false;
            }




            result += $"{tab}{tab}{tab}{tab}]\n";
            result += $"{tab}{tab}{tab}>\n";




            return result;

        }

        public string writeTable(Tabla tabla, bool primero, string nombre)
        {
            string result = "";
            result += primero ? $"{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab},<\n";

            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}CQL-TYPE{co}={co}TABLE{co},\n";
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}NAME{co}={co}{nombre}{co},\n";
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}COLUMNS{co}=[\n";


            bool primEncabezado = true;
            foreach (NodoDeclararEncabezados nodo in tabla.Encabezados)
            {
                result += writeEncabezados(nodo, primEncabezado);
                primEncabezado = false;
            }

            result += $"{tab}{tab}{tab}{tab}]\n";


            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}DATA{co}=[\n";


            bool primeroFila = true;
            foreach (ZFila nodo in tabla.Filas)
            {
                result += writeContainer(nodo, primeroFila);
                primeroFila = false;
            }


            result += $"{tab}{tab}{tab}{tab}]\n";


            result += $"{tab}{tab}{tab}>\n";



            return result;
        }



        public string writeEncabezados(NodoDeclararEncabezados nodo, bool primero)
        {
            string result = "";

            result += primero ? $"{tab}{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab}{tab},<\n";

            result += $"{tab}{tab}{tab}{tab}{tab}";
            result += $"{co}NAME{co}={co}{nodo.Nombre}{co},\n";

            result += $"{tab}{tab}{tab}{tab}{tab}";
            TeDeU tipo = (TeDeU) nodo.Tipo.ejecutar(ambitoCaputrado);
            result += $"{co}TYPE{co}={co}{tipo.Nombre}{co},\n";

            result += $"{tab}{tab}{tab}{tab}{tab}";
            result += $"{co}PK{co}={co}TRUE{co}\n";

            result += $"{tab}{tab}{tab}{tab}>\n";



            return result;

        }

        public string writeContainer(ZContenedor contenedor, bool primero)
        {
            string result = "";

            result += primero ? $"{tab}{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab}{tab},<\n";

            bool primeroFila = true;
            foreach (KeyValuePair<string, Simbolo> nodo in contenedor.TablaSimbolo)
            {

                
                
                result += $"{tab}{tab}{tab}{tab}{tab}";
                result += primeroFila?"":",";
                result += $"{co}{nodo.Key}{co}={writeAtributes(nodo.Value)}\n";
                primeroFila = false;

            }



            result += $"{tab}{tab}{tab}{tab}>\n";



            return result;
        }

        public string writeAtributes(Simbolo simbolo)
        {

            string result = "";

            TeDeU tipo = simbolo.obtenerInstanciaTipo();

            if (Utilidades.esPrimitivo(tipo) || simbolo.obtenerValor() is ZNull)
            {
               return  $"{simbolo.obtenerValor().stringBonito()}";
            }

         
            if (simbolo.obtenerValor() is ZInstancia)
            {
                
                return "\n"+writeContainer(simbolo.obtenerValor(), true);
            }

            throw new RuntimeBinderException("erro al escrbirir archivo chison, bd-atributos");
            //return result;

        }

        public string writeProcedimiento(Procedimiento proce, bool primero, string nombre)
        {
            string result = "";
            
            result += primero ? $"{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab},<\n";
            
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}CQL-TYPE{co}={co}PROCEDURE{co},\n";
            
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}NAME{co}={co}{nombre}{co},\n";
            
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}PARAMETERS{co}={co}[{co}\n";


            bool primeroPam = true;
            foreach (NodoDeclararParametro nodo in proce.NodosDeclararParametros)
            {
                result += writeParameters(nodo, primeroPam, true);
                primeroPam = false;

            }

            foreach (NodoDeclararParametro nodo in proce.NodosRetornos)
            {
                result += writeParameters(nodo, primeroPam, false);
                primeroPam = false;
                
            }

            
            result += $"{tab}{tab}{tab}{tab}]\n";

            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}INSTR{co}=${proce.Cadena}$\n";
            
            result += $"{tab}{tab}{tab}>\n";
            
            return result;
        }

        public string writeParameters(NodoDeclararParametro parametro, bool primero, bool @in)
        {
            string result = "";
            
            result += primero ? $"{tab}{tab}{tab}{tab}<\n" : $"{tab}{tab}{tab}{tab},<\n";


            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}NAME{co}={co}{parametro.Id}{co},\n";
            
            result += $"{tab}{tab}{tab}{tab}";
            TeDeU tipo = (TeDeU) parametro.Tipo.ejecutar(ambitoCaputrado);
            result += $"{co}TYPE{co}={co}{tipo.Nombre}{co},\n";
            
            result += $"{tab}{tab}{tab}{tab}";
            result += $"{co}AS{co}=";

            result += @in ? "IN": "OUT";
            result += "\n";

            
            result += $"{tab}{tab}{tab}{tab}>\n";
            return result;
        }
        
        //public string writeReturns()
    }





}