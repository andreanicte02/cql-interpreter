// Procedimientos: parametros de entrada y valores de retorno
create database if not exists pruebas;
use pruebas;

procedure calcular (int x, int y), (int suma, int doble) {
    log("calculando con " + x + " y " + y);
    if (y == 0) {
        log("y es cero, regresa null");
        return null, null;
    } else {
        log("suma = " + (x + y) + ", doble = " + (x + x));
        return x + y, x + x;
    }
}

call calcular(4, 5);
call calcular(7, 0);
