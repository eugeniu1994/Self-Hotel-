var days = ['D', 'L', 'M', 'M', 'J', 'V', 'S'];

function getTarifeValori() {
    $.ajax({
        url: 'Administrare.aspx/getTarifeValoriDesign',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                var tabel = document.getElementById("test-table");
                tabel.innerHTML = "";
                var i = 0, dictionar = ok.d;
                var trHeader = document.createElement("tr");
                var colspan2 = document.createElement("th");
                colspan2.setAttribute("colspan", 2);
                trHeader.appendChild(colspan2);
                
                for (var key in dictionar) {
                    var lista = dictionar[key];

                    var th = document.createElement("th");
                    th.innerHTML = "Tip cam: " + key;
                    var NrPersoane = [];
                    for (var k = 0; k < lista.length; k++) {
                        NrPersoane.push(lista[k].NrPersoane);
                    }
                    var max = Math.max.apply(null, NrPersoane);
                    th.setAttribute("colSpan", max);
                    trHeader.appendChild(th);
                }
                tabel.appendChild(trHeader);
                var secondTr = document.createElement("tr");
                tabel.appendChild(secondTr);
                var td0 = document.createElement("th");
                td0.innerHTML = "Zi";
                secondTr.appendChild(td0);

                var td1 = document.createElement("th");
                td1.innerHTML = "Data";
                secondTr.appendChild(td1);
                for (var key in dictionar) {
                    var lista = dictionar[key];

                    var NrPersoane = [];
                    for (var k = 0; k < lista.length; k++) {
                        NrPersoane.push(lista[k].NrPersoane);
                    }
                    var max = Math.max.apply(null, NrPersoane);
                    for (var j = 0; j < max; j++) {
                        var th = document.createElement("th");
                        th.innerHTML = j+1 + " Persoane";
                        secondTr.appendChild(th);
                    }
                }
               
                //var m = ok.d.dictionar;

                //for (var key in m) {
                //    var rand = ok.d.dictionar[key];

                //    var newTr = document.createElement("tr");

                //    var thZi = document.createElement("th");
                //    thZi.innerHTML = rand[0];
                //    newTr.appendChild(thZi);
                    
                //    var dataTr = document.createElement("th");
                //    dataTr.innerHTML = key;
                //    newTr.appendChild(dataTr);

                //    for (i = 0; i < rand.length; i++) {
                //        var td = document.createElement("td");
                //        td.innerHTML = rand[i].NrPersoane;
                //        newTr.appendChild(td);
                //    }

                //    tabel.appendChild(newTr);
                //}
            } else {
                alert('ok.d = false');
            }
        },
        error: function () {
            alert('eroare la conexiune');
            return null;
        }
    });

}


function creazaTarifeValori() {
    var arrDate = document.getElementById("Dela").value;
    var depDate = document.getElementById("Panala").value;
    $.ajax({
        url: 'Administrare.aspx/creazaTarifValori',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                var tabel = document.getElementById("test-table");
                tabel.innerHTML = "";
                var i = 0;
                var trHeader = document.createElement("tr");
                var colspan2 = document.createElement("th");
                colspan2.setAttribute("colspan", 2);
                trHeader.appendChild(colspan2);
                var n = ok.d.length;
                for (i = 0; i < n;i++) {

                    var th = document.createElement("th");
                    th.innerHTML = ok.d[i].DenTipCamera;
                    th.setAttribute("colSpan", ok.d[i].MaxAdulti);
                    trHeader.appendChild(th);
                }
                tabel.appendChild(trHeader);

                var secondTr = document.createElement("tr");
                tabel.appendChild(secondTr);
                var td0 = document.createElement("th");
                td0.innerHTML = "Zi";
                secondTr.appendChild(td0);

                var td1 = document.createElement("th");
                td1.innerHTML = "Data";
                secondTr.appendChild(td1);

                for (var j = 0 ; j < n;j++) {
                    for (var j1 = 0; j1 < ok.d[j].MaxAdulti; j1++) {
                        var th = document.createElement("th");
                        th.innerHTML = j1 + 1 + " pers";
                        secondTr.appendChild(th);
                    }
                }

                var start = new Date("02/05/2013");
                var end = new Date("02/10/2013");

                var arrDateArr = arrDate.split("-");
                var depDateArr = depDate.split("-");
                var start = new Date(arrDateArr[0], arrDateArr[1], arrDateArr[2]);
                start.setMonth(start.getMonth() - 1);
                var end = new Date(depDateArr[0], depDateArr[1], depDateArr[2]);
                end.setMonth(end.getMonth() - 1);
                var loop = new Date(start);

                while (loop <= end) {
                    var trRand = document.createElement("tr");
                    tabel.appendChild(trRand);

                    var tdZi = document.createElement("th");
                    tdZi.innerHTML = days[loop.getDay()];
                    trRand.appendChild(tdZi);

                    var tdData = document.createElement("th");
                    tdData.innerHTML = loop.toLocaleDateString();;
                    trRand.appendChild(tdData);

                    var dd = loop.getDate();
                    var mm = loop.getMonth() + 1; //Ianuarie este 0!

                    var yyyy = loop.getFullYear();
                    if (dd < 10) {
                        dd = '0' + dd;
                    }
                    if (mm < 10) {
                        mm = '0' + mm;
                    }
                    var today = yyyy + '-' + mm + '-' + dd;

                    for (var j = 0 ; j < n; j++) {
                        for (var j1 = 0; j1 < ok.d[j].MaxAdulti; j1++) {
                            var _td = document.createElement("td");
                            _td.setAttribute("class", "celulaValoare");
                            _td.setAttribute("data-IdTipCamera", ok.d[j].ID);
                            _td.setAttribute("data-NrPersoane", j1 + 1);
                            _td.setAttribute("data-Data", today);
                            _td.setAttribute("data-IdTipCamera_H", ok.d[j].IdTipCam_H);
                            trRand.appendChild(_td);
                        }
                    }

                    var newDate = loop.setDate(loop.getDate() + 1);
                    loop = new Date(newDate);
                }
            } else {
                alert('ok.d = false');
            }
        },
        error: function () {
            alert('eroare la conexiune');
            return null;
        }
    });

}

function saveData() {
    var x = document.getElementsByClassName("celulaValoare");
    alert('sunt ' + x.length + ' celule');
    var i = 0, n = x.length;
    var listaCelule = [];
    for (i = 0; i < n; i++) {
        var celula = {
            IdTipCamera: x[i].getAttribute("data-IdTipCamera"),
            Data:x[i].getAttribute("data-Data"),
            NrPersoane: x[i].getAttribute("data-NrPersoane"),
            Valoare: x[i].innerHTML,
            IdTipCamera_H: x[i].getAttribute("data-IdTipCamera_H")
        }
        listaCelule.push(celula);
    }
    var str = JSON.stringify(listaCelule);
    $.ajax({
        url: 'Administrare.aspx/salveazaValoriTarif',
        data: "{str:" + str + "}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                alert('ok');
            } else {
                alert('ok.d = false');
            }
        },
        error: function () {
            alert('eroare la conexiune');
            return null;
        }
    });
}








//-----------------Dictionar ---------------------

function Dictionary() {
    this.datastore = new Array();
    this.add = add;
    this.datastore = new Array();
    this.find = find;
    this.modifica = modifica;
    this.remove = remove;
    this.showAll = showAll;
    this.showSorted = showSorted;
    this.count = count;
    this.clear = clear;
}

function add(key, value) {
    this.datastore[key] = value;
}

function find(key) {
    return this.datastore[key];
}

function modifica(key, value) {
    this.datastore[key] = value;
}

function remove(key) {
    delete this.datastore[key];
}

function showAll() {
    for (var key in Object.keys(this.datastore).sort()) {
        console.log(key + " -> " + this.datastore[key]);
    }
}

function showSorted() {
    for (var key in Object.keys(this.datastore).sort()) {
        console.log(key + " -> " + this.datastore[key]);
    }
}

function count() {
    var n = 0;
    for (var key in Object.keys(this.datastore)) {
        ++n;
    }
    return n;
}

function clear() {
    for (var key in Object.keys(this.datastore)) {
        delete this.datastore[key];
    }
}

//-----------------Dictionar ---------------------

