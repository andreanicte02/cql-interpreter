

using System.Collections.Generic;
using System.Linq;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion.OperacionesRelacionales;
using servidor.InterpreteCQL.Instruccionesn;

namespace servidor.InterpreteCQL.Expresion.Primitivos
{

    public class ZResultadoQuery : ZContenedor
    {
        public List<List<ZContenedor>> Resultado { get; }

        public ZResultadoQuery(List<List<ZContenedor>> resultado) : base(null, TiposPrimitivos.tipoResultadoQuery)
        {
            Resultado = resultado;
        }
    }
    
    public class ZLista: ZContenedor
    {

        // factory method
        public static ZLista crearListaVaciaSinTipo()
        {
            return new ZLista(new List<ZContenedor>(), TiposPrimitivos.tipoListaVacia);
        }
        public static ZLista crearLista(List<ZContenedor> elementos)
        {
            if (elementos.Count == 0)
            {
                throw new SemanticError("Desarrollor: 'crearLista' no debe ser llamado con 0 elementos");
            }

            var tipoElemento = elementos[0].Origen;


            if (elementos.TrueForAll(c => c.Origen == tipoElemento))
            {
                return new ZLista(elementos, Dbms.obtenerTeDeUListaDeCache(tipoElemento));                
            }
            
            var teDeUDiferente = elementos.Find(c => c.Origen != tipoElemento).Origen;
            
            throw new SemanticError($"Todos los elementos de la lista deben de ser del mismo tipo." +
                                        $" Tipos encontrados: '{tipoElemento.Nombre}', '{teDeUDiferente.Nombre}' ");
        }
        
        //--------------------------

        public List<ZContenedor> Elementos { get; }
        
        public ZLista(List<ZContenedor> elementos, TeDeU origen) : base(null, origen)
        {
            this.Elementos = elementos;
            
            cargarFuncionInsert();
            cargarFuncionGet();
            cargarFuncionSet();
            cargarFuncionRemove();
            cargarFuncionSize();
            cargarFuncionClear();
            cargarFuncionContains();
        }

        public ZContenedor obtenerElemento(int indice)
        {
            if (indice < 0 || indice >= this.Elementos.Count)
            {
                throw new SemanticError($"Accesso [{indice}] fuera de rango  [0..{this.Elementos.Count}]");
            }

            return this.Elementos[indice];
        }

        public void setValor(int indice, ZContenedor valor)
        {
            if (indice < 0 || indice >= this.Elementos.Count)
            {
                throw new SemanticError($"Accesso [{indice}] fuera de rango  [0..{this.Elementos.Count}]");
            }

            this.Elementos[indice] = valor;
        }

        public void removeValor(int indice)
        {
            if (indice < 0 || indice >= this.Elementos.Count)
            {
                throw new SemanticError($"Accesso [{indice}] fuera de rango  [0..{this.Elementos.Count}]");
            }
            
            this.Elementos.RemoveAt(indice);
        }
        
        public int getSize()
        {
            return Elementos.Count;
        }

        public void clear()
        {
            Elementos.Clear();
        }

        public bool contains(ZContenedor zContenedor)
        {
            var find = Elementos.Find(c => NodoIgualQue.IgualIgual(c, zContenedor).obtenerValor() );
            return find != null;
        }
        
        
        
        public override string stringBonito()
        {
            var elementos = string.Join(", ", Elementos.Select(c => c.stringBonito()));
            return $"[{elementos}]";
        }
        
        
        
        
        
        private void cargarFuncionInsert()
        {
            if (!(Origen is TeDeULista teDeULista))
            {
                return;
            }

            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(teDeULista.TipoElemento), 
                this,
                args =>
                {
                    var zContenedor = args[0];
                    Elementos.Add(zContenedor);
                    return TiposPrimitivos.instanciaVoid;
                }
            );
            
            declararFuncion("insert",f);
        }
        
        private void cargarFuncionGet()
        {
            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(TiposPrimitivos.tipoNumero), 
                this,
                args =>
                {
                    var zNumero = (ZNumero) args[0];
                    return obtenerElemento(zNumero.obtenerValor());
                }
            );
            
            declararFuncion("get",f);
        }
        
        private void cargarFuncionSet()
        {
            if (!(Origen is TeDeULista teDeULista))
            {
                return;
            }
            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(TiposPrimitivos.tipoNumero, teDeULista.TipoElemento), 
                this,
                args =>
                {
                    var zNumero = (ZNumero) args[0];
                    var zContenedor = args[1];
                    
                    setValor(zNumero.obtenerValor(), zContenedor);
                    return TiposPrimitivos.instanciaVoid;
                }
            );
            
            declararFuncion("set",f);
        }



        private void cargarFuncionRemove()
        {
            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(TiposPrimitivos.tipoNumero), 
                this,
                args =>
                {
                    var zNumero = (ZNumero) args[0];
                    removeValor(zNumero.obtenerValor());
                    return TiposPrimitivos.instanciaVoid;
                }
            );
            
            declararFuncion("remove",f);
        }

        private void cargarFuncionSize()
        {
            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(), 
                this,
                args => new ZNumero(getSize()));
            
            declararFuncion("size",f);
        }
        private void cargarFuncionClear()
        {
            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(), 
                this,
                args =>
                {
                    clear();
                    return TiposPrimitivos.instanciaVoid;
                });
            
            declararFuncion("clear",f);
        }
        
        
        private void cargarFuncionContains()
        {
            if (!(Origen is TeDeULista teDeULista))
            {
                return;
            }

            var f = new FuncionNativa(
                TiposPrimitivos.tipoVoid, 
                Utilidades.crearListaParametros(teDeULista.TipoElemento), 
                this,
                args =>
                {
                    var zContenedor = args[0];

                    return new ZBool(contains(zContenedor));
                }
            );
            
            declararFuncion("contains",f);
        }
        
        
        
        
    }
}