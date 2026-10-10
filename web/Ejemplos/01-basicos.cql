// Variables, ciclos, condiciones y funciones
log("Hola desde CQL!");
log(5 + 5);

int a, b = 4;
a = 3;
log("a = " + a + ", b = " + b);

int i = 0;
while (i < 3) {
    log("while: " + i);
    i = i + 1;
}

int contador = 0;
do {
    log("do-while se ejecuta al menos una vez");
    contador = contador + 1;
} while (contador < 0);

if (false) {
    log("no entra");
} else if (true) {
    log("entra al else if");
} else {
    log("no entra");
}

for (int n = 1; n < 5; n = n + 1) {
    if (n == 2) {
        continue;
    }
    log("for: " + n);
}

// sobrecarga de funciones
int suma(int x, int y) {
    return x + y;
}

int suma(String texto, int y) {
    log("suma con texto: " + texto);
    return y + 100;
}

log(suma(2, 3));
log(suma("hola", 5));

string saludo = "hola mundo";
log(saludo.length());
log(saludo.startsWith("hola"));
log(today());
