// commit guarda todo el DBMS en formato Chison (ver la pestania "Chison")
create database tienda;
use tienda;

create type producto {
    nombre string,
    precio double
}

create table inventario (id counter, nombre string, existencias int);
insert into inventario values (0, "teclado", 10);
insert into inventario values (0, "mouse", 25);

commit;
log("commit realizado");
