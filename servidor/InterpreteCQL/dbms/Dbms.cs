using System;
using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.InstruccionesDB;
using servidor.InterpreteCQL.Instruccionesn;
using servidor.InterpreteCQL.Interfaces;

namespace servidor.InterpreteCQL.dbms
{
    public class Dbms
    {

        
        private static BaseDeDatos bdSeleccionada = null;
        public static string usSeleccionado = "";
        //bases de datos
        private static Dictionary<string, BaseDeDatos> bases = new Dictionary<string, BaseDeDatos>();
        private static Dictionary<string, string> usuarios = new Dictionary<string, string>();
        public static List<BdUsuarios>  listUsersBases= new List<BdUsuarios>();
        public static Dictionary<string,TeDeU> tedeusGlobales = new Dictionary<string, TeDeU>();

        public static string UsSeleccionado
        {
            get => usSeleccionado;
            set => usSeleccionado = value;
        }

        public static Dictionary<string, string> Usuarios
        {
            get => usuarios;
           
        }
        

        private static List<BdUsuarios> lus = new List<BdUsuarios>();

        public static BaseDeDatos BdSeleccionada
        {
            get => bdSeleccionada;
            set => bdSeleccionada = value;
        }

        public static Dictionary<string, BaseDeDatos> Bases
        {
            get => bases;
          
        }

        public static object crearBaseDeDatos(string nombre)
        {
            if (bases.ContainsKey(nombre.ToLower()))
            {
                throw new SemanticError($"base de datos '{nombre}' ya existente");
            }
            
            bases[nombre.ToLower()] = new BaseDeDatos(nombre);
            
            //-------------------

            if (usSeleccionado.Equals("admin"))
            {
                listUsersBases.Add(new BdUsuarios("admin",nombre));
                return null;
            }
            
            listUsersBases.Add(new BdUsuarios("admin",nombre));
            listUsersBases.Add(new BdUsuarios(usSeleccionado,nombre));


            return null;
        }

        public static object crearBaseDeDatosSiNoExiste(string nombre)
        {
            if (bases.ContainsKey(nombre.ToLower()))
            {
                return null;
            }
            
            bases[nombre.ToLower()] = new BaseDeDatos(nombre);
            
            if (usSeleccionado.Equals("admin"))
            {
                listUsersBases.Add(new BdUsuarios("admin",nombre));
                return null;
            }
            
            listUsersBases.Add(new BdUsuarios("admin",nombre));
            listUsersBases.Add(new BdUsuarios(usSeleccionado,nombre));
            
            

            return null;
        }

        public static object seleccionarBaseDeDatos(string nombre)
        {
            if (!bases.ContainsKey(nombre.ToLower()))
            {
                throw new SemanticError($"base de datos '{nombre}' no existe ");
            }

            bdSeleccionada = bases[nombre.ToLower()];

            return null;
        }


        public static object eliminarBaseDeDatos(string nombre)
        {
            if (!bases.ContainsKey(nombre.ToLower()))
            {
                throw new SemanticError($"base de datos '{nombre}' no existe ");
            }

            bases.Remove(nombre.ToLower());
            
            return null;
        }

         /// <summary>
         /// Obtiene el TeDeU que representa a un tipo de lista en especifico,
         /// Ejemplo List-string tendra su propio TeDeU
         /// </summary>
         /// <param name="instruccionTeDeU">Instruccion que devuelve un TeDeU</param>
         /// <param name="ambito"></param>
         /// <returns></returns>
        public static object obtenerTeDeULista(Instruccion instruccionTeDeU, ZContenedor ambito)
        {
            var teDeUElementoLista =(TeDeU) instruccionTeDeU.ejecutar(ambito);
            return Dbms.obtenerTeDeUListaDeCache(teDeUElementoLista);
        }
        
         
        private static readonly TeDeUListaCache Cache = new TeDeUListaCache();


        public static TeDeU obtenerTeDeUListaDeCache(TeDeU teDeUElementoLista)
        {
            return Cache.obtenerTedeULista(teDeUElementoLista);
        }
        

        public static object crearTabla(string nombre, List<NodoDeclararEncabezados> columnas, ZContenedor e)
        {

            if (bdSeleccionada == null)
            {
                throw new SemanticError($"no se encuentra ninguna base de datos seleccinoada, no se puede crear tabla  '{nombre}'");
            }
            
           
            bdSeleccionada.crearTabla(nombre, columnas , e);
            return null;
        }
        
        public static object createTableSiNoExiste(string id, List<NodoDeclararEncabezados> columnas, ZContenedor e)
        {

            if (bdSeleccionada == null)
            {
                throw new SemanticError($"no se encuentra ninguna base de datos seleccinoada con el nombre '{id}'");

                
            }
            
            bdSeleccionada.crearTablaSiNoExiste(id,columnas,e);
            

            return null;
        }
        
        

        public static object dropTable(string id)
        {
            if (bdSeleccionada == null)
            {
                throw new SemanticError($"no se encuentra ninguna base de datos seleccinoada con el nombre '{id}'");
            }
            
            bdSeleccionada.eliminarTabla(id);
            return null;
        }

   

        public static object dropTableSiExiste(string id)
        {
            if (bdSeleccionada == null)
            {
                throw new SemanticError($"no se encuentra ninguna base de datos seleccinoada con el nombre '{id}'");
            }
            
            bdSeleccionada.eliminarTablaSiExiste(id);

            return null;
        }

        public static object crearUsuario(string id, string pass)
        {
            if (usuarios.ContainsKey(id)){
                throw new SemanticError("ya existe un usuario con ese nombre");
            }
            
            usuarios.Add(id,pass);
            return null;
        }
        

        public static BaseDeDatos getBd()
        {

            if (bdSeleccionada == null)
            {
                throw new SemanticError("no se encuentra ninguna bd seleccionada");
            }

            return bdSeleccionada;

        }

        public static TeDeU getPrimitivos(string id)
        {
            if (tedeusGlobales.ContainsKey(id))
            {
                return tedeusGlobales[id];
            }

            throw new SemanticError($"no existe el tdeu '{id}' ");
        }

        public static void cargarTedeUs()
        {            
            tedeusGlobales.Add("int", TiposPrimitivos.tipoNumero);
            tedeusGlobales.Add("double", TiposPrimitivos.tipoDicimal);
            tedeusGlobales.Add("string", TiposPrimitivos.tipoString);
            tedeusGlobales.Add("bool", TiposPrimitivos.tipoBool);
            tedeusGlobales.Add("date", TiposPrimitivos.tipoDate);
            tedeusGlobales.Add("time", TiposPrimitivos.tipoBool);
            
        }

        public static bool isSelect()
        {
            return bdSeleccionada != null;
        }
        
        
        public static List<BdUsuarios> permisosUsuarios(string id) {
            
            List<BdUsuarios> list = new List<BdUsuarios>();


            foreach (BdUsuarios nodo in listUsersBases)
            {
                if (nodo.Id.Equals(id))
                {
                    list.Add(nodo);
                }
                
            }
            
            return list;

        }

        public static BaseDeDatos crearBDChison(string name)
        {
            if (bases.ContainsKey(name))
            {
                throw new SemanticError("la base de datos ya existe");
            }

           
            BaseDeDatos b = new BaseDeDatos(name);
            bases.Add(name,b);
           
            listUsersBases.Add(new BdUsuarios("admin",name));
            return b;


        }








        //-------->crear tabla 
        //eliminar tabla
        
 
        
    }
}