# Intérprete CQL

Intérprete de **CQL** (un lenguaje de consultas inspirado en Cassandra) y de **Chison**, el formato en el que se
guarda la base de datos. Está hecho en C# con [Irony](https://github.com/IronyProject/Irony) y corre en el
navegador con Blazor WebAssembly.

**Demo:** https://andreanicte02.github.io/cql-interpreter/

El lenguaje incluye:

- Variables, `if`, `while`, `do-while`, `for`, funciones con sobrecarga y funciones nativas (`today()`, `now()`, métodos de `string`)
- Tipos de usuario con `create type` e instancias (`new tipo`, `{...} as tipo`)
- DBMS en memoria: `create database`, `use`, `create table`, `insert`, `select ... where`, `update`, `alter table`, columnas `counter`
- Procedimientos con parámetros de entrada y valores de retorno
- `commit`, que guarda todo el DBMS en formato Chison, y carga de archivos Chison

## Ejemplos

En la página se puede elegir cualquiera de estos archivos, ejecutarlo y ver la consola, las tablas y el Chison generado.

| Ejemplo | Archivo |
|---|---|
| Básicos | `web/Ejemplos/01-basicos.cql` |
| Tipos de usuario | `web/Ejemplos/02-tipos.cql` |
| Base de datos | `web/Ejemplos/03-base-de-datos.cql` |
| Procedimientos | `web/Ejemplos/04-procedimientos.cql` |
| Commit (Chison) | `web/Ejemplos/05-commit.cql` |
| Archivos de prueba originales | `servidor/PruebaEntrada.txt`, `pruebaEntrada2.txt`, `pruebaEntrada3.txt`, `pruebaChison.txt`, `chison.txt` |

Algunas partes del lenguaje quedaron sin terminar y los archivos de prueba originales lo muestran tal cual:
la multiplicación, la división, el módulo y la potencia están comentados en la gramática, el `-` binario no se
reconoce, `x++` dentro de una expresión no está implementado y las fechas con comilla simple tampoco.

## Estructura

```
servidor/   el intérprete original (.NET Framework, app de consola)
CqlCore/    compila el código de servidor/ como librería .NET 8
web/        la página (Blazor WebAssembly) y los ejemplos
```

## Desarrollo

Se necesita el [SDK de .NET 8](https://dotnet.microsoft.com/download).

```bash
dotnet run --project web           # abre la página en http://localhost:5000 (o el puerto que indique)
dotnet publish web -c Release -o publicado
```

Cada push a `main` publica la página con `.github/workflows/pages.yml`.
