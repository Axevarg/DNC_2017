//desabilitar el boton derecho
//  document.oncontextmenu = function () { return false }

//replace caracteres
function replaceCaracteres(element) {
    var strF = element.value;
    var res = strF.replace(new RegExp("'", 'g'), "");
    res = res.replace(/[~'"<>{};]/g, '');
    res = res.replace("--", "")
    res = res.replace("/*", "")
    res = res.replace("*/", "")
    element.value = res;
}
//funcion para no registrar caracteres de inyeccion
function caracteres(evt) {

    //asignamos el valor de la tecla a keynum
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }


    //valida el '
    if (keynum == 39) {
        return false;
    }
    else if (keynum > 47 && keynum < 58) {
        return true;
    }
        //mayusculas
    else if (keynum > 64 && keynum < 91) {
        return true;
    }
        //minusculas
    else if (keynum > 96 && keynum < 123) {
        return true;
    }
        //Ñ o ñ
    else if (keynum == 164 || keynum == 165 || keynum == 209 || keynum == 241) {
        return true;
    }
        //Espacio
    else if (keynum == 32) {
        return true;
    }

}
//Remplaza Numeros no validos
function replaceNumeros(element) {
    if (isNaN(element.value) == true) {
        element.value = 0;
        element.focus();
        alert("Número Invalido.")
    }

    if (element.value == '') {
        element.value = 0;
    }

}
//Valida Decimales
function validaNumerosDecimales(evt) {

    //asignamos el valor de la tecla a keynum
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }
    //validamos el .
    if (keynum == 46) {
        return true;
    }
    //comprobamos si se encuentra en el rango
    if (keynum > 47 && keynum < 58) {
        return true;
    }
    else {
        return false;
    }
}

//VALIDACION DE NUMEROS ENTEROS
function validaNumeroEntero(evt) {

    //asignamos el valor de la tecla a keynum
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }
    //valida el retoceso
    if (keynum == 8) {
        return true;
    }
    //comprobamos si se encuentra en el rango
    if (keynum > 47 && keynum < 58) {
        return true;
    }
    else {
        return false;
    }
}


//Funcion del Formato de Fecha
function existeFecha(txt) {

    var fecha = document.getElementById(txt).value;
    var fechaf = fecha.split("-");
    var day = fechaf[2];
    var month = fechaf[1];
    var year = fechaf[0];

    //validacion de caracteres
    if (fecha.length < 10) {

        alert("El Formato de la Fecha es Incorrecto.");
        return false;
    }

    if (year < 1900) {
        alert("El A\u00f1o no debe de ser menor a 1900.");
        return false;
    }

    if (year > 2050) {
        alert("El A\u00f1o no debe ser mayor a 2050.");
        return false;
    }
    //Mes
    if (month < 0) {
        alert("El Mes debe de ser mayor a 0.");
        return false;
    }

    if (month > 12) {
        alert("El Mes debe se menor que 12.");
        return false;
    }



    var uDia;
    //dias
    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
        uDia = 31;
    }
    else {
        if (month == 2) {
            if (year % 4 == 0) {
                uDia = 29;
            }
            else { uDia = 28; }
        }
        else {
            uDia = 30;
        }
    }
    //valida rango de dias

    if (day > uDia) {
        alert("El D\u00ed a es Incorrecto.");
        return false;
    }
    if (day == 0) {
        alert("El D\u00ed a es Incorrecto.");
        return false;
    }

    return true;
}

function replaceFechas(element) {
    var fecha = element.value;
    var fechaf = fecha.split("-");
    var day = fechaf[2];
    var month = fechaf[1];
    var year = fechaf[0];

    //validacion de caracteres
    if (fecha.length < 10) {

        alert("El Formato de la Fecha es Incorrecto.");
        element.value = '';
        element.focus();
        return false;
    }

    if (year < 1900) {
        alert("El A\u00f1o no debe de ser menor a 1900.");
        element.value = '';
        element.focus();
        return false;
    }

    if (year > 2050) {
        alert("El A\u00f1o no debe ser mayor a 2050.");
        element.value = '';
        element.focus();
        return false;
    }
    //Mes
    if (month < 0) {
        alert("El Mes debe de ser mayor a 0.");
        element.value = '';
        element.focus();
        return false;
    }

    if (month > 12) {
        alert("El Mes debe se menor que 12.");
        element.value = '';
        element.focus();
        return false;
    }



    var uDia;
    //dias
    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
        uDia = 31;
    }
    else {
        if (month == 2) {
            if (year % 4 == 0) {
                uDia = 29;
            }
            else { uDia = 28; }
        }
        else {
            uDia = 30;
        }
    }
    //valida rango de dias

    if (day > uDia) {
        element.value = '';
        element.focus();

        alert("El D\u00ed a es Incorrecto.");
        return false;

    }
    if (day == 0) {
        alert("El D\u00ed a es Incorrecto.");
        element.value = '';
        element.focus();
        return false;
    }
}


function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}


function onlyTelefono(evt) {

    //asignamos el valor de la tecla a keynum
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }

    //comprobamos si se encuentra en el rango
    if (keynum > 47 && keynum < 58) {
        return true;
    }
    else {
        return false;
    }
}
