// Base de datos: crear, insertar, consultar y modificar tablas
create database if not exists universidad;
use universidad;

create table estudiante (carnet counter, nombre string, nota int);

insert into estudiante values (0, "Ana", 90);
insert into estudiante values (0, "Luis", 75);
insert into estudiante (nombre, nota) values ("Sofia", 88);

log("--- select * ---");
select * from estudiante;

log("--- select con where ---");
select nombre, nota from estudiante where nota > 80;

update estudiante set nota = 100 where nombre == "Luis";
log("--- despues del update ---");
select * from estudiante;

alter table estudiante add curso string;
insert into estudiante values (0, "Mario", 60, "Compiladores 2");
log("--- despues del alter ---");
select * from estudiante;
