using System;
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.Instrucciones;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.dbms
{
    public class Tabla
    {
       
        private List<NodoDeclararEncabezados> encabezados;

        public List<NodoDeclararEncabezados> Encabezados => encabezados;

        private List<ZFila> filas= new List<ZFila>();

        public List<ZFila> Filas
        {
            get => filas;
            set => filas = value;
        }

        private int counter = 0;
        private ZContenedor ambitoCaputrado;

        public Tabla(string nombre, List<NodoDeclararEncabezados> encabezados, ZContenedor ambitoCaputrado)
        {
            
            this.encabezados = encabezados;
            this.ambitoCaputrado = ambitoCaputrado;
        }

        public void insertNormal(List<ZContenedor> argumentos)
        {
            ZFila fila = verificarFirma(argumentos);
            filas.Add(fila);
            counter+=1;

        }

        //las tablas necesitan capturar los contenedores?
        //metodo que verifica si hace match, los argumentos enviados, con los encabezados
        //solo se usa en la inserccion simple
        //si es count, se ignora el valor que trae, se agrega el valor del counter 
        public ZFila verificarFirma(List<ZContenedor> argumentos)
        {
            if (argumentos.Count != encabezados.Count)
            {
                throw new SemanticError($"la instruccion insert to, no tiene todos los datos correspondientes en la tabla'");
            }
            
            
            ZFila fila = new ZFila(ambitoCaputrado);

            for (int x = 0; x < argumentos.Count; x++)
            {
               
                TeDeU tipo1 = (TeDeU)encabezados[x].ejecutar(fila);
                TeDeU tipo2 = argumentos[x].Origen;
                
                if (tipo1.isCount)
                {
                    tipo1.isCount = false;
                    Simbolo sim1 = new Simbolo(tipo1, new ZNumero(counter));
                    fila.setVariable(encabezados[x].Nombre,sim1);
                    Console.WriteLine($"auto incrementable {counter}");
                    continue;
                }

                if (tipo1 != tipo2)
                {
                    throw new SemanticError("la instruccion insert to, los argumentos no correspondne a los encabezados de la tabla");
                }
                
                Simbolo sim = new Simbolo(tipo1,argumentos[x]);
                fila.setVariable(encabezados[x].Nombre,sim);

            }
            
            return fila;


        }
        
        //se crea los encabezados en zfila
        //se agrega los valores de los encabezados en el entrono de zfial
        //se agrega zfila
        public void insertEspecial(List<ZContenedor> argumentos, List<NodoBuscarId> ids)
        {
            ZFila zfila = declararFila( encabezados);
            agregarValor(argumentos,ids,zfila);
            filas.Add(zfila);
            counter += 1;
        }
        

        //crea una zfila, con los valores inciales
        public ZFila declararFila( List<NodoDeclararEncabezados> encabezados)
        {
            ZFila fila = new ZFila(ambitoCaputrado);

            foreach (NodoDeclararEncabezados nodo in encabezados)
            {
                TeDeU tipo = (TeDeU)nodo.ejecutar(fila);

                Simbolo sim;
                if (tipo.isCount)
                {
                    tipo.isCount = false;
                    sim = new Simbolo(tipo, new ZNumero(counter));
                    fila.setVariable(nodo.Nombre, sim);
                    Console.WriteLine($"auto incrementable {counter}");
                    continue;
                }

                sim = new Simbolo(tipo,null);
                Utilidades.AsignarValorInicial(sim);
                fila.setVariable(nodo.Nombre, sim);
            }
            
            return fila;
        }

        //asigna el valor, a los campos enviados
        //si es count-> se setea el valor del contador y se salta toda la iteracion
        public void agregarValor(List<ZContenedor> args, List<NodoBuscarId> ids, ZContenedor fila)
        {
            for (int x = 0; x < args.Count; x++)
            {
                Simbolo sim = (Simbolo)ids[x].ejecutar(fila);
                

                if (sim.obtenerInstanciaTipo() != args[x].Origen)
                {
                    throw new SemanticError($"el tipo de los campos que se desan asignar no hacen matchm con los argumentos enviados, insert to");
                }
                sim.definirValor(args[x]);

            }
            
        }

        public void truncate()
        {
            filas.Clear();
            counter = 0;
        }

        //elimina columnas
        public void alterDrop(List<NodoBuscarId> nombres)
        {


            for (int x = 0; x < encabezados.Count; x++)
            {
                foreach (NodoBuscarId nodoId in nombres)
                {
                    if (encabezados[x].Nombre.Equals(nodoId.Id))
                    {
                        encabezados.Remove(encabezados[x]);
                        break;
                    }
                    
                }
                
            }
            

            foreach (ZFila fila in filas)
            {
                foreach (NodoBuscarId nodoId in nombres)
                {
                    fila.eliminarEncabezado(nodoId.Id);
                   
                }
            }

        }
        
        
        //agrega columnas
        public void alterAdd(List<NodoDeclararEncabezados> nuevos)
        {
            foreach (NodoDeclararEncabezados nodo in nuevos)
            {
                encabezados.Add(nodo);
            }

            int contaodr = counter;
            foreach (ZFila fila in filas)
            {
                agregarColumnasAfilas(fila, nuevos, contaodr);
                contaodr = +1;

            }
        }

        public void agregarColumnasAfilas(ZFila fila, List<NodoDeclararEncabezados> nuevos, int counter)
        {
            
            foreach (NodoDeclararEncabezados nodo in nuevos)
            {
                TeDeU tipo = (TeDeU)nodo.ejecutar(fila);
                Simbolo sim;
               
                
                if (tipo.isCount)
                {
                    tipo.isCount = false;
                    sim = new Simbolo(tipo, new ZNumero(counter));
                    fila.setVariable(nodo.Nombre, sim);
                    Console.WriteLine($"auto incrementable {counter}");
                    continue;
                }
                
                sim = new Simbolo(tipo,null);
                Utilidades.AsignarValorInicial(sim);
                fila.setVariable(nodo.Nombre, sim);
       
            }
            
        }

        public ZFila crearFila()
        {
            return declararFila(encabezados);
        }

        public void addFila(ZFila fila)
        {
            filas.Add(fila);
        }






    }
}