// Tipos definidos por el usuario (create type) e instancias
create type persona {
    nombre string,
    edad int
}

create type curso {
    nombre string,
    catedratico persona
}

persona p = new persona;
p.nombre = "Ana";
p.edad = 25;
log(p.nombre + " tiene " + p.edad + " anios");

curso c = new curso;
c.nombre = "Compiladores 2";
c.catedratico = p;
log(c.nombre + " lo da " + c.catedratico.nombre);

// instancia con valores
persona otra = {"Luis", 30} as persona;
log(otra.nombre);
log(otra.edad);
