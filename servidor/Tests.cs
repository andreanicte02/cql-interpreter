using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Irony.Parsing;
using NUnit.Framework;
using servidor.Analizadores;
using servidor.InterpreteCQL;
using servidor.InterpreteCQL.dbms;
using servidor.InterpreteCQL.Interfaces;

namespace servidor
{

    public class Tests
    {
        [Test, Category("Prueba CQL 2")]
        public void pruebaCqlFuncionesList()
        {

            var entrada = @"

list<int> ids = [];
log(ids);

ids.insert(1993);
ids.insert(93);
ids.insert(17);
log(ids);

log(ids.get(1));

ids.set(1, 993);
log(ids);

ids.remove(1);
log(ids);

log(ids.size());

log(ids.contains(17));
log(ids.contains(18));

ids.clear();

log(ids);

";
            
            
            var an = new AnalizadorCQL();
            var parser = new Parser(an);
            var arbol = parser.Parse(entrada);

            var mensajes = arbol
                    .ParserMessages
                    .Select(message => $"{message.Location} - {message.Message}")
                ;
            var errores = string.Join("\n", mensajes);
            
            Assert.IsFalse(arbol.HasErrors(), errores);
            
            List<Instruccion> listIns= (  List<Instruccion>) arbol.Root.AstNode;
            ZContenedor global = new ZContenedor(null, null);
            
            Dbms.cargarTedeUs();
            
            Utilidades.cargarFuncionesNativas(global);
            
            //funciones
//            loadToday(global);
//            loadNow(global);

    
            
            Utilidades.ejecutarSentencias(listIns, global);
            

        }
        
        [Category("Prueba CQL 2")]
        public void pruebaCqlFuncionesAgregacion()
        {

            var entrada = @"


create database prueba;
use prueba;
create table alumnos (peso int, edad int);
insert into alumnos values (50, 26);
insert into alumnos values (502, 36);
insert into alumnos values (93, 17);

int algo = count(<<select * from alumnos>>);
log(algo);

int sumaTotal = sum(<< select edad from alumnos >>);
log(sumaTotal);

int promedio = avg(<< select edad from alumnos >>);
log(promedio);

int maximo = max(<< select edad from alumnos >>);
int minimo = min(<< select edad from alumnos >>);
log(maximo);
log(minimo);


";
            
            
            var an = new AnalizadorCQL();
            var parser = new Parser(an);
            var arbol = parser.Parse(entrada);

            var mensajes = arbol
                    .ParserMessages
                    .Select(message => $"{message.Location} - {message.Message}")
                ;
            var errores = string.Join("\n", mensajes);
            
            Assert.IsFalse(arbol.HasErrors(), errores);
            
            List<Instruccion> listIns= (  List<Instruccion>) arbol.Root.AstNode;
            ZContenedor global = new ZContenedor(null, null);
            
            Dbms.cargarTedeUs();
            
            Utilidades.cargarFuncionesNativas(global);
            
            //funciones
//            loadToday(global);
//            loadNow(global);

    
            
            Utilidades.ejecutarSentencias(listIns, global);
            

        }

       
        [Category("Prueba CQL")]
        public void pruebaCql()
        {

            var entrada = @"


list<int> hola = [2, 3, 4];
int otro = hola[1];
log(otro);

log([2,3,4][1]);

list<int> ids = [9 , 7, 6] + [ 3, 1, 0];
log(ids);

list<list<int>> matrix = [ [6, 3, 2], [9, 8], [7] ];
log(matrix);

list<string> vacio = [];
log(vacio);

";
            
            var an = new AnalizadorCQL();
            var parser = new Parser(an);
            var arbol = parser.Parse(entrada);

            var mensajes = arbol
                .ParserMessages
                .Select(message => $"{message.Location} - {message.Message}")
                ;
            var errores = string.Join("\n", mensajes);
            
            Assert.IsFalse(arbol.HasErrors(), errores);
            
            List<Instruccion> listIns= (  List<Instruccion>) arbol.Root.AstNode;
            ZContenedor global = new ZContenedor(null, null);
            
            Dbms.cargarTedeUs();
            
            //funciones
//            loadToday(global);
//            loadNow(global);
            
            
            Utilidades.ejecutarSentencias(listIns, global);
            

        }
        
        
    }
}