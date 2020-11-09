using servidor.InterpreteCQL.Expresion;
using servidor.InterpreteCQL.Expresion.Primitivos;

namespace servidor.InterpreteCQL.Instruccionesn
{
    public class TiposPrimitivos
    {

         public static TeDeU tipoNumero = new TeDeU("int");
         
         public static TeDeU tipoDicimal = new TeDeU("double");
         
         public static TeDeU tipoBool = new TeDeU("bool");
         
         public static TeDeU tipoString = new TeDeU("string");
         
         public static TeDeU tipoDate = new TeDeU("date");
         
         public static TeDeU tipoTime = new TeDeU("time");

         public static TeDeU tipoNulo = new TeDeU("nulo");
         
        // public static TeDeU tipoFila = new TeDeU("fila");
         
        
         public static ZNull instanicaNulo = new ZNull(null, tipoNulo);
         
         public static TeDeU tipoListaVacia = new TeDeU("list<*sinTipoxD>");
         public static TeDeU tipoResultadoQuery = new TeDeU("<ResultadoQuery>");
         
         public static TeDeU tipoVoid = new TeDeU("TipoVacio");
         public static ZContenedor instanciaVoid = new ZContenedor(null, tipoVoid);
        
    }
}