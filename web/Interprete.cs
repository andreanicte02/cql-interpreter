using System.Reflection;
using servidor.InterpreteChison.Utils;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Expresion.Primitivos;
using servidor.InterpreteCQL.InstruccionesDB;

namespace CqlWeb;

public enum Lenguaje { Cql, Chison }

public record Ejemplo(string Grupo, string Nombre, string Recurso, Lenguaje Lenguaje, string Descripcion)
{
    public string Codigo()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Recurso)
                           ?? throw new InvalidOperationException($"no existe el recurso {Recurso}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

public record TablaVista(string Nombre, List<string> Columnas, List<List<string>> Filas);

public record BaseVista(string Nombre, List<string> Tipos, List<string> Procedimientos, List<TablaVista> Tablas);

public record Resultado(string Salida, List<BaseVista> Bases, string? Chison, long Milisegundos);

public static class Interprete
{
    public static readonly List<Ejemplo> Ejemplos = new()
    {
        new("Ejemplos", "Básicos", "ejemplos/01-basicos.cql", Lenguaje.Cql, "Variables, ciclos, if, funciones con sobrecarga y funciones nativas."),
        new("Ejemplos", "Tipos de usuario", "ejemplos/02-tipos.cql", Lenguaje.Cql, "create type, instancias con new y con valores, acceso a atributos."),
        new("Ejemplos", "Base de datos", "ejemplos/03-base-de-datos.cql", Lenguaje.Cql, "create database / table, insert, select con where, update y alter."),
        new("Ejemplos", "Procedimientos", "ejemplos/04-procedimientos.cql", Lenguaje.Cql, "Procedimientos con parámetros de entrada y de retorno."),
        new("Ejemplos", "Commit (Chison)", "ejemplos/05-commit.cql", Lenguaje.Cql, "commit guarda todo el DBMS en formato Chison."),
        new("Archivos de prueba originales", "PruebaEntrada.txt", "pruebas/PruebaEntrada.txt", Lenguaje.Cql, "Prueba original del proyecto. Se detiene en la línea 72: el ++ dentro de una expresión no quedó implementado."),
        new("Archivos de prueba originales", "pruebaEntrada2.txt", "pruebas/pruebaEntrada2.txt", Lenguaje.Cql, "Prueba original del proyecto. Las fechas con comilla simple no quedaron implementadas."),
        new("Archivos de prueba originales", "pruebaEntrada3.txt", "pruebas/pruebaEntrada3.txt", Lenguaje.Cql, "Prueba original del proyecto: tablas, counter, alter, update y procedimientos. Termina con log(variable) sin declarar, para probar el error."),
        new("Archivos de prueba originales", "pruebaChison.txt", "pruebas/pruebaChison.txt", Lenguaje.Chison, "Archivo Chison original: carga bases de datos, tipos, tablas y usuarios."),
        new("Archivos de prueba originales", "chison.txt", "pruebas/chison.txt", Lenguaje.Chison, "Chison generado por un commit del proyecto original. No se puede volver a cargar: el commit escribe \"PK\"=\"TRUE\" con comillas y el parser de Chison espera true/false."),
    };

    // NodoCommit escribe en esta ruta relativa; en WebAssembly el sistema de archivos es virtual (en memoria)
    private const string RutaCommit = "./../../chison.txt";

    /// <summary>
    /// Construye la gramatica (lo mas lento en WebAssembly) antes de que el usuario ejecute algo.
    /// </summary>
    public static void Preparar(Lenguaje lenguaje) => Ejecutar("", lenguaje);

    public static Resultado Ejecutar(string codigo, Lenguaje lenguaje)
    {
        Dbms.reiniciar();
        if (File.Exists(RutaCommit))
        {
            File.Delete(RutaCommit);
        }

        var cronometro = System.Diagnostics.Stopwatch.StartNew();
        var salida = new StringWriter();
        var anterior = Console.Out;
        Console.SetOut(salida);
        try
        {
            if (lenguaje == Lenguaje.Cql)
            {
                new EjecutarAnalizadorCQL().ejecutarAnalizador(codigo);
            }
            else
            {
                new EjecutarAChison().ejecutarAnalizador(codigo);
            }
        }
        catch (Exception)
        {
            // el interprete ya escribio el error en la consola (con linea y columna)
            salida.WriteLine(">> ejecución detenida por un error");
        }
        finally
        {
            Console.SetOut(anterior);
        }

        var chison = File.Exists(RutaCommit) ? File.ReadAllText(RutaCommit) : null;
        return new Resultado(salida.ToString(), CapturarBases(), chison, cronometro.ElapsedMilliseconds);
    }

    private static List<BaseVista> CapturarBases()
    {
        var bases = new List<BaseVista>();
        foreach (var bd in Dbms.Bases.Values)
        {
            var tipos = bd.Tdeus.Where(t => !servidor.InterpreteCQL.Utilidades.esPrimitivo(t.Value)).Select(t => t.Value.Nombre).ToList();
            var procedimientos = bd.AgentesProcedimiento.Keys.ToList();
            var tablas = bd.Tablas.Select(t => VistaTabla(t.Key, t.Value)).ToList();
            bases.Add(new BaseVista(bd.Nombre, tipos, procedimientos, tablas));
        }
        return bases;
    }

    private static TablaVista VistaTabla(string nombre, Tabla tabla)
    {
        var columnas = tabla.Encabezados.Select(e => e.Nombre).ToList();
        var filas = tabla.Filas
            .Select(fila => fila.TablaSimbolo.Values.Select(s => Texto(s.obtenerValor())).ToList())
            .ToList();
        return new TablaVista(nombre, columnas, filas);
    }

    private static string Texto(ZContenedor valor)
    {
        try
        {
            return valor is ZInstancia ? NodoSelect.escribirContenedor(valor) : valor.stringBonito();
        }
        catch (Exception)
        {
            return "?";
        }
    }
}
