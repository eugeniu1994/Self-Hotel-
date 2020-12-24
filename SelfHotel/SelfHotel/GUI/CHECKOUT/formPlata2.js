var codRezervare = "";
var defaultLang = "RO";
var rezervariOK = false;
var currentTab = 0; 
showTab(currentTab); 

function incarcaDetaliiHeader() {
    $.ajax({
        url: 'PlataForm2.aspx/getSetariHeader',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                try {
                    //din baza
                    var ClientHotelName = document.getElementById("ClientHotelName");
                    ClientHotelName.innerHTML = "";
                    var aDenumire = document.createElement("a");
                    ClientHotelName.appendChild(aDenumire);
                    aDenumire.setAttribute("class", "HotelLink");
                    aDenumire.setAttribute("href", "#");
                    aDenumire.innerHTML = ok.d[1];
                    var ClientHotelStars = document.getElementById("ClientHotelStars");
                    ClientHotelStars.innerHTML = "";
                    var nrStele = parseInt(ok.d[0], 10);
                    for (var i = 0; i < nrStele; i++) {
                        var img = document.createElement("img");
                        ClientHotelStars.appendChild(img);
                        img.setAttribute("src", "../../Icoane/star_16px.png");
                        img.setAttribute("width", "16");
                        img.setAttribute("style", "vertical-align:middle");
                    }
                    var ClientTopHeaderMiddle = document.getElementById("ClientTopHeaderMiddle");
                    ClientTopHeaderMiddle.innerHTML = "";
                    ClientTopHeaderMiddle.innerHTML = ok.d[2];
                } catch (err) {
                    //default
                    var ClientHotelName = document.getElementById("ClientHotelName");
                    ClientHotelName.innerHTML = "";
                    var aDenumire = document.createElement("a");
                    ClientHotelName.appendChild(aDenumire);
                    aDenumire.setAttribute("class", "HotelLink");
                    aDenumire.setAttribute("href", "#");
                    aDenumire.innerHTML = "HOTEL DEMO";
                    var ClientHotelStars = document.getElementById("ClientHotelStars");
                    ClientHotelStars.innerHTML = "";
                    var nrStele = 5;
                    for (var i = 0; i < nrStele; i++) {
                        var img = document.createElement("img");
                        ClientHotelStars.appendChild(img);
                        img.setAttribute("src", "../../Icoane/star_16px.png");
                        img.setAttribute("width", "16");
                        img.setAttribute("style", "vertical-align:middle");
                    }
                    var ClientTopHeaderMiddle = document.getElementById("ClientTopHeaderMiddle");
                    ClientTopHeaderMiddle.innerHTML = "Str. Trandafirului Nr. 13A, Constanta, Romania.";
                }
            }
        },
        error: function (msg) {
        }
    });
}

function showTab(n) {
    var x = document.getElementsByClassName("tab");
    x[n].style.display = "block";
    if (n == 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n == (x.length - 1)) {
        document.getElementById("nextBtn").innerHTML = "Finish";
        document.getElementById("nextBtn").style.display = "none";
    } else {
        if (defaultLang == "RO") {
            document.getElementById("nextBtn").innerHTML = "Inainte";
        } else {
            document.getElementById("nextBtn").innerHTML = "Next";
        }
        document.getElementById("nextBtn").style.display = "block";
    }
    fixStepIndicator(n)
}

function nextPrev(n) {
    var x = document.getElementsByClassName("tab");
    if (n == 1 && !validateForm()) return false;

    x[currentTab].style.display = "none";
    if (currentTab == 0 || currentTab == 2) {
        if (sejurAchitat) {
            currentTab = currentTab + 2*n;
        } else {
            currentTab = currentTab + n;
        }
    } else {
        currentTab = currentTab + n;
    }
    if (currentTab >= x.length) {
        document.getElementById("regForm").submit();
        return false;
    }
    showTab(currentTab);
    changeLanguage();
}
var sejurAchitat = false;
function validateForm() {
    var x, y, i, valid = true;
    x = document.getElementsByClassName("tab");
    y = x[currentTab].getElementsByTagName("input");

    if (currentTab == 1) {
            var input = $('.validate-input .input100');

            $('.validate-form .input100').each(function () {
                $(this).focus(function () {
                    hideValidate(this);
                });
            });
            $('.input100').each(function () {
                $(this).focus(function () {
                    hideValidate(this);
                });
            });

            function validate(input) {
                if ($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
                    if ($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                        return false;
                    }
                }
                else {
                    if ($(input).val().trim() == '') {
                        return false;
                    }
                }
            }

            function showValidate(input) {
                var thisAlert = $(input).parent();

                $(thisAlert).addClass('alert-validate');
            }

            function hideValidate(input) {
                var thisAlert = $(input).parent();

                $(thisAlert).removeClass('alert-validate');
            }

            var check = true;

            for (var i = 0; i < input.length; i++) {
                if (validate(input[i]) == false) {
                    showValidate(input[i]);
                    check = false;
                } else {
                    hideValidate(input[i]);
                }
            }
            valid = check;
    }
    if (valid) {
        document.getElementsByClassName("step")[currentTab].className += " finish";
    }
    return valid; 
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function fixStepIndicator(n) {
    var i, x = document.getElementsByClassName("step");
    for (i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(" active", "");
    }
    x[n].className += " active";
}

function promalert() {
    var codRdinSesiune = "";
    sejurAchitat = false;
    var variabila = "Introduceti codul rezervarii:";
    if (defaultLang == "RO") {
        variabila = "Introduceti codul rezervarii:";
    } else {
        variabila = "Enter the reservation code:";
    }
    try{
        $.ajax({
            url: 'PlataForm2.aspx/autocompleteRezervare',
            data: "{}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d) {
                    codRdinSesiune = ok.d;
                    var loading;
                    if (defaultLang == "RO") {
                        loading = new Loading({
                            title: ' Va rugam asteptati',
                            direction: 'hor',
                            discription: 'Se incarca datele...',
                            defaultApply: true,
                        });
                    } else {
                        loading = new Loading({
                            title: ' Please wait',
                            direction: 'hor',
                            discription: 'Data is loading...',
                            defaultApply: true,
                        });
                    }
                    setTimeout(function () {
                        getDateRezervare(codRdinSesiune);
                        try {
                            loading.out();
                        } catch (err) { }
                    }, 300);
                } else {
                    alertify.prompt(variabila, function (e, str) {
                                if (e) {
                                    getDateRezervare(str);
                                    if (!rezervariOK) { setTimeout(reload, 2500); }
                                } else {
                                    if (!rezervariOK) { setTimeout(reload, 4000); }
                                }
                            }, codRdinSesiune);
                }
            },
            error: function (error) {
                try {
                    loading.out();
                } catch (err) { }
            }
        });
    } catch (err) { }
    
    //alertify.prompt(variabila, function (e, str) {
    //        if (e) {
    //            getDateRezervare(str);
    //            if (!rezervariOK) { setTimeout(reload, 2500); }
    //        } else {
    //            if (!rezervariOK) { setTimeout(reload, 4000); }
    //        }
    //    }, codRdinSesiune);
    
}

function cautaRezervare() {
    alertify.prompt('Introduceti codul rezervarii:', function (e, str) {
        if (e) {
            getDateRezervare(str);
            if (!rezervariOK) { setTimeout(reload, 2500); }
        } else {
            if (!rezervariOK) { setTimeout(reload, 4000); }
        }
    }, "");
}

function reload() {
    location.reload();
}

function deschideFormularCheckIn() {
    alertify.success('mergem la checkIn');
    window.location = "../CHECKIN/CheckIn2.aspx";
}

$(document).ready(function () {
    try {
        checkCookie();
    } catch (err) { }
    try {
        changeLanguage();
    } catch (err) { }
    promalert();
    
});

function getDateRezervare(CodRezervare) {
        $.ajax({
            url: 'PlataForm2.aspx/getRezervare',
            data: "{CodRezervareTmp:'" + CodRezervare + "'}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d) {
                    codRezervare = CodRezervare;
                    var btnTabel = document.getElementById("btnTabel");btnTabel.innerHTML = "";
                    var tabClient = document.getElementById("tabClient"); tabClient.innerHTML = "";
                    var tabFinal = document.getElementById("tabFinal"); tabFinal.innerHTML = "";
                    if (ok.d.status == 0) { //nu exista rezervarea 'Rezervarea cu codul ' + CodRezervare + ' nu a fost gasita.'
                        if (defaultLang == "RO") {
                            alertify.alert('Rezervarea cu codul ' + CodRezervare + ' nu a fost gasita.');
                        } else {
                            alertify.alert("Reservation with code ' + CodRezervare + ' wasn't found");
                        }
                        rezervareOK = false;
                        sejurAchitat = false;
                    } else if (ok.d.status == 1) { //nu este cazata, mergi la cazare
                        if (defaultLang == "RO") {
                            alertify.confirm('Rezervare nu este cazata, Mergeti la Check In???', function (e) {
                                if (e) {
                                    deschideFormularCheckIn();
                                }
                            });
                        } else {
                            alertify.confirm('Reservation is not Checked In, would you like to go to Check In ???', function (e) {
                                if (e) {
                                    deschideFormularCheckIn();
                                }
                            });
                        }
                        rezervareOK = false;
                        sejurAchitat = false;
                    } else if (ok.d.status == 4) { // a intervenit o eroare necunoscuta
                        if (defaultLang == "RO") {
                            alertify.alert("A intervenit o eroare necunoscuta, dati un refresh.");
                        } else {
                            alertify.alert("An unknown error occurred, please refresh");
                        }
                        rezervareOK = false;
                        sejurAchitat = false;
                    } else if (ok.d.status == 5) { // rezervarea este iesita
                        if (defaultLang == "RO") {
                            alertify.alert("CheckOut pentru aceasta rezervare a fost realizat, camerele sunt iesite.");
                        } else {
                            alertify.alert("Check out for this reservation was made, the rooms are closed.");
                        }
                        rezervareOK = false;
                        sejurAchitat = false;
                    } else { // ok
                        if (ok.d.status == 2) {
                            sejurAchitat = true;
                        }
                        var i = 0, lista = ok.d.lista, n = ok.d.lista.length;
                        var tr0 = document.createElement("tr");
                        btnTabel.appendChild(tr0);
                        var totalGeneral = 0;
                        if (defaultLang == "RO") {
                            document.getElementById("CamereCountID").innerHTML = n + " Camere";
                        } else {
                            document.getElementById("CamereCountID").innerHTML = n + " Rooms";
                        }
                        try {
                            for (i = 0; i < n; i++) {
                                var tr = document.createElement("tr");

                                var td0 = document.createElement("td");
                                tr.appendChild(td0);
                                var img0 = document.createElement("img");
                                img0.setAttribute("class", "imgTabel");
                                img0.src = "../../Nomenclatoare/Handler1.ashx?id=" + lista[i].IdCamera;
                                td0.appendChild(img0);
                                var spanDenCamera = document.createElement("span");
                                spanDenCamera.innerHTML = "Camera " + lista[i].Denumire;
                                td0.appendChild(spanDenCamera);

                                var td1 = document.createElement("td");
                                tr.appendChild(td1);
                                if (defaultLang == "RO") {
                                    td1.innerHTML = "Camera " + lista[i].IdCamera + " " + lista[i].Denumire + " [" + lista[i].NrAdulti + " x";
                                } else {
                                    td1.innerHTML = "Room  " + lista[i].IdCamera + " " + lista[i].Denumire + " [" + lista[i].NrAdulti + " x";
                                }
                                var imgAdult = document.createElement("img");
                                imgAdult.setAttribute("src", "../../Icoane/adult.png");
                                imgAdult.setAttribute("title", "Numar adulti");
                                imgAdult.setAttribute("alt", "Adult");
                                td1.appendChild(imgAdult);
                                td1.innerHTML = td1.innerHTML + "] [" + lista[i].NrCopii + " x";
                                var imgChild = document.createElement("img");
                                imgChild.setAttribute("src", "../../Icoane/child.png");
                                imgChild.setAttribute("title", "Numar copii");
                                imgChild.setAttribute("alt", "Copii");
                                td1.appendChild(imgChild);
                                td1.innerHTML = td1.innerHTML + "] ";
                                var spanTotalCamera = document.createElement("span");

                                spanTotalCamera.setAttribute("style", "float: right;font-weight: bolder;margin-right: 15px;");
                                td1.appendChild(spanTotalCamera);

                                var totalDePlata = 0;

                                for (var j = 0; j < lista[i].entitateServiciiLista.length; j++) {
                                    var td_ = document.createElement("td");
                                    tr.appendChild(td_);

                                    totalDePlata = totalDePlata + lista[i].entitateServiciiLista[j].SoldRON;
                                    totalGeneral = totalGeneral + lista[i].entitateServiciiLista[j].SoldRON;
                                    if (lista[i].entitateServiciiLista[j].SoldRON > 0) {
                                        td_.innerHTML = lista[i].entitateServiciiLista[j].DenumireServiciu + "  ---  " + lista[i].entitateServiciiLista[j].SoldRON + " RON";//ValoareRon RamasDePlataRON SoldRON
                                    } else {
                                        td_.innerHTML = lista[i].entitateServiciiLista[j].DenumireServiciu;
                                    }
                                }

                                var imgOK = document.createElement("img");

                                imgOK.setAttribute("style", "position: absolute;top: 0px;right: 12px;");
                                if (totalDePlata > 0) {
                                    spanTotalCamera.innerHTML = "Total " + totalDePlata + " RON";
                                    imgOK.setAttribute("src", "../../Icoane/error.png");
                                    imgOK.setAttribute("title", "Camena neachitata");
                                } else {
                                    spanTotalCamera.innerHTML = "Achitata ";
                                    imgOK.setAttribute("src", "../../Icoane/correct.png");
                                    imgOK.setAttribute("title", "Camena achitata");
                                }
                                td1.appendChild(imgOK);
                                btnTabel.appendChild(tr);
                            }
                        } catch (err) {
                            //try { loading.out(); } catch (err) { }
                        }
                        try {

                            //adauga aici text
                            var divWrap = document.createElement("div");
                            tabFinal.appendChild(divWrap);
                            divWrap.setAttribute("class", "tg-wrap");
                            divWrap.setAttribute("style", "background: white;");
                            var tableLP = document.createElement("table");
                            divWrap.appendChild(tableLP);
                            tableLP.setAttribute("id", "tg-rwLtp");
                            tableLP.setAttribute("class", "tg");
                            var trTitlu = document.createElement("tr");
                            tableLP.appendChild(trTitlu);
                            var th_0T = document.createElement("th");
                            th_0T.setAttribute("class", "tg-0pky");
                            th_0T.setAttribute("colspan", "2");
                            th_0T.innerHTML = "Furnizor: $[Furnizor]<br>Capital social: $[Capitalsocial]<br>Reg com: $[Regcom]<br>Codul Fiscal: $[Cif]<br>Sediul: $[Sediul]<br>$[Cont]";
                            trTitlu.appendChild(th_0T);
                            var th_1T = document.createElement("th");
                            th_1T.setAttribute("class", "tg-baqh");
                            th_1T.setAttribute("colspan", "3");
                            trTitlu.appendChild(th_1T);

                            th_1T.innerHTML = '<span style="font-weight:700">Seria: ' + ok.d.serieFactura + ' Nr. ' + ok.d.numarFactura + '</span><br><span style="font-weight:700">Factura</span><br><span style="font-weight:700">Fiscala</span><br><span style="font-weight:700">Nr. facturii: ' + ok.d.numarFactura + '</span><br><span style="font-weight:700">Data: ' + ok.d.datalucru + ' </span>';
                            var th_2T = document.createElement("th");
                            trTitlu.appendChild(th_2T);
                            th_2T.setAttribute("class", "tg-0lax");
                            th_2T.setAttribute("colspan", "3");
                            th_2T.innerHTML = 'Cumparator: $[Cumparator]<br>Nr. Reg. Com.: $[RegComcumparator]<br>CUI/CNP: $[Cifcumparator]<br>Sediul: $[Sediulcumparator]<br>$[ContCumparator]<br>$[BancaCumparator]';

                            var CampTabele = document.createElement("tr");
                            tableLP.appendChild(CampTabele);
                            CampTabele.innerHTML = ' <td class="tg-fymr">Nr.<br>Crt.</td> <td class="tg-kiyi">Denumirea produselor sau a serviciilor</td><td class="tg-amwm">Cota <br>TVA<br><br></td><td class="tg-amwm">U.M.</td><td class="tg-88nc">Cantitate</td><td class="tg-amwm">Pret unitar <br>(fara TVA)<br>lei</td> <td class="tg-amwm">Valoare<br>(fara TVA)<br>lei</td><td class="tg-88nc">Valoare<br>TVA<br>LEI</td>';

                            //i = 0, lista = ok.d.lista, n = ok.d.lista.length;
                            var totalFaraTVA = 0;
                            var totalTVAProcent = 0
                            for (i = 0; i < n; i++) {
                                var cameraRand = document.createElement("tr");
                                cameraRand.innerHTML = "<td class='tg-0pky'>" + i + "</td>" +
                                                "<td class='tg-xldj '>Camera " + lista[i].Denumire + "</td>" +
                                                "<td class='tg-0lax 'colspan='6'></td>";
                                //"<td class='tg-0lax '></td>" +
                                //"<td class='tg-xldj '></td>" +
                                //"<td class='tg-0lax '></td>" +
                                //"<td class='tg-0lax '></td>" +
                                //"<td class='tg-xldj '></td>";
                                tableLP.appendChild(cameraRand);
                                var j = 0, serviciu = lista[i].entitateServiciiLista, m = lista[i].entitateServiciiLista.length;
                                for (j = 0; j < m; j++) {
                                    var totalServiciuFaraTVA = serviciu[j].TotalRON - serviciu[j].ProcentTVA * serviciu[j].TotalRON / 100;
                                    var PretUnitar = totalServiciuFaraTVA / serviciu[j].Cantitate;
                                    var tvaCameraValoare = serviciu[j].TotalRON * serviciu[j].ProcentTVA / 100;

                                    var serviciuRand = document.createElement("tr");
                                    tableLP.appendChild(serviciuRand);
                                    serviciuRand.innerHTML = "<td class='tg-0pky celulaAscunsa'></td>" +
                                                "<td class='tg-xldj celulaAscunsa'>" + serviciu[j].DenumireServiciu + "</td>" +
                                                "<td class='tg-0lax celulaAscunsa'>" + serviciu[j].ProcentTVA + "</td>" +
                                                "<td class='tg-0lax celulaAscunsa'>" + serviciu[j].UM + "</td>" +
                                                "<td class='tg-xldj celulaAscunsa'>" + serviciu[j].Cantitate + "</td>" +
                                                "<td class='tg-0lax celulaAscunsa'>" + parseFloat(PretUnitar).toFixed(2) + "</td>" +
                                                "<td class='tg-0lax celulaAscunsa'>" + parseFloat(serviciu[j].Cantitate * PretUnitar).toFixed(2) + "</td>" +
                                                "<td class='tg-xldj celulaAscunsa'>" + parseFloat(tvaCameraValoare).toFixed(2) + "</td>";
                                    totalTVAProcent = totalTVAProcent + (serviciu[j].TotalRON * serviciu[j].ProcentTVA / 100);
                                    totalFaraTVA += serviciu[j].TotalRON - (serviciu[j].TotalRON * serviciu[j].ProcentTVA / 100);
                                }
                            }

                            var trSubsol = document.createElement("tr");
                            tableLP.appendChild(trSubsol);
                            trSubsol.innerHTML = '<td class="tg-0lax" colspan="8">' + ok.d.obsFactura + '</td>';

                            var trTermen = document.createElement("tr");
                            tableLP.appendChild(trTermen);
                            trTermen.innerHTML = '<td class="tg-0lax" colspan="8">Termen de plata: ' + ok.d.TermenPlata + '</td>';

                            var trSubsol2 = document.createElement("tr");
                            tableLP.appendChild(trSubsol2);
                            trSubsol2.innerHTML = '<td class="tg-0lax" rowspan="2">Semnatura si stampila<br>furnizorului<br><br><br><br></td><td class="tg-0lax" rowspan="2">Data privind expeditia<br>Numele delegatului: $[Delegat]<br>Buletin/cartea de identitate: $[CIDelegat]<br>Mijloc de transport: ' + ok.d.Transport + '</td><td class="tg-0lax" colspan="2">Total LEI</td><td class="tg-0lax" colspan="2">' + parseFloat(totalFaraTVA).toFixed(2) + '</td> <td class="tg-0lax" colspan="2">' + parseFloat(totalTVAProcent).toFixed(2) + ' </td>';

                            var trSubsol3 = document.createElement("tr");
                            tableLP.appendChild(trSubsol3);
                            trSubsol3.innerHTML = '<td class="tg-0lax" colspan="2">Semnatura de primire<br><br></td><td class="tg-0lax" colspan="2">Total de plata LEI</td> <td class="tg-0lax" colspan="2">' + parseFloat(totalFaraTVA + totalTVAProcent).toFixed(2) + '</td>';

                            var lastTr = document.createElement("tr");
                            tableLP.appendChild(lastTr);
                            var lastTd = document.createElement("td");
                            lastTd.setAttribute("colspan", "8");
                            lastTr.appendChild(lastTd);
                            lastTd.innerHTML = ok.d.obsFactura;
                            //var IdNomPartener = document.getElementById("selectPersoanaFacturareID").getAttribute("data-IdNomPartener");
                            getDateClient(lista[0].IdTurist, "tg-rwLtp");
                        } catch (err) {

                        }

                        document.getElementById("totalGeneralID").innerHTML = "Total de plata " + totalGeneral + " RON";
                        rezervariOK = true;
                        if (ok.d.status == 2) {//nu mai are nimic de plata ii afisez doar Check out
                            var h3 = document.createElement("h3");
                            h3.setAttribute("style", "margin:auto;text-align:center;");
                            h3.setAttribute("id", "tag6");
                            h3.innerHTML = "Finalizare sejur";
                            h3.setAttribute("class", "titluForm");
                            tabFinal.appendChild(h3);

                            var divButtons = document.createElement("div");
                            divButtons.setAttribute("id", "divButtons");
                            divButtons.setAttribute("class", "container-login100-form-btn p-t-10");
                            tabFinal.appendChild(divButtons);
                            var btnPrinteaza = document.createElement("button");
                            btnPrinteaza.setAttribute("type", "button");
                            btnPrinteaza.setAttribute("class", "login100-form-btn");
                            btnPrinteaza.setAttribute("id", "btnPrinteazaFacturaID");
                            btnPrinteaza.setAttribute("style", "width:49%");
                            btnPrinteaza.setAttribute("onclick", "printezaFactura()");
                            var imaginePrinter = '<img src="../../Icoane/printer.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                            if (defaultLang == "RO") {
                                btnPrinteaza.innerHTML = imaginePrinter+"Printeaza factura";
                            } else {
                                btnPrinteaza.innerHTML = imaginePrinter + "Print invoice";
                            }
                            divButtons.appendChild(btnPrinteaza);
                            var btnEmail = document.createElement("button");
                            btnEmail.setAttribute("type", "button");
                            btnEmail.setAttribute("class", "login100-form-btn");
                            btnEmail.setAttribute("id", "btnEmailID");
                            btnEmail.setAttribute("style", "width:49%");
                            btnEmail.setAttribute("onclick", "trimiteEmailFactura()");
                            var imagineEmail = '<img src="../../Icoane/email.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                            if (defaultLang == "RO") {
                                btnEmail.innerHTML = imagineEmail+"Trimite factura email";
                            } else {
                                btnEmail.innerHTML = imagineEmail + "Email invoice";
                            }
                            
                            divButtons.appendChild(btnEmail);
                            var checkOutBTN = document.createElement("div");
                            checkOutBTN.setAttribute("class", "login100-form-btn");
                            checkOutBTN.setAttribute("style", "margin-top: 15px;");
                            var checkOutImagine = '<img src="../../Icoane/ok.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                            checkOutBTN.innerHTML = checkOutImagine+"CheckOut";
                            checkOutBTN.setAttribute("onclick", "checkOutBtn(" + false + ")");
                            tabFinal.appendChild(checkOutBTN);
                        }
                        else {
                            var h3 = document.createElement("h3");
                            h3.setAttribute("style", "margin:auto;text-align:center;");
                            h3.setAttribute("class", "titluForm");
                            h3.setAttribute("id", "tag7");
                            h3.innerHTML = "Finalizare plata";
                            tabFinal.appendChild(h3);

                            var divButtons = document.createElement("div");
                            divButtons.setAttribute("id", "divButtons");
                            divButtons.setAttribute("class", "container-login100-form-btn p-t-10");
                            tabFinal.appendChild(divButtons);
                            var btnCash = document.createElement("button");
                            btnCash.setAttribute("type", "button");
                            btnCash.setAttribute("class", "login100-form-btn");
                            btnCash.setAttribute("id", "btnPlataCash");
                            btnCash.setAttribute("style", "width:49%");
                            btnCash.setAttribute("onclick", "plataCash()");
                            var imagineBani = '<img src="../../Icoane/money2.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                            if (defaultLang == "RO") {
                                btnCash.innerHTML = imagineBani + " Plata cash";
                            } else {
                                btnCash.innerHTML = imagineBani + " Cash payment";
                            }
                            //btnCash.innerHTML = "Plata cash";
                            divButtons.appendChild(btnCash);
                            var btnCard = document.createElement("button");
                            btnCard.setAttribute("type", "button");
                            btnCard.setAttribute("class", "login100-form-btn");
                            btnCard.setAttribute("id", "btnPlataCard");
                            btnCard.setAttribute("style", "width:49%");
                            btnCard.setAttribute("onclick", "plataCard()");
                            var imagineCard = '<img src="../../Icoane/card.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                            if (defaultLang == "RO") {
                                btnCard.innerHTML = imagineCard + " Plata card";
                            } else {
                                btnCard.innerHTML = imagineCard + " Online payment";
                            }
                            //btnCard.innerHTML = "Plata card";
                            divButtons.appendChild(btnCard);
                        }

                        var container = document.createElement("div");
                        tabClient.appendChild(container);
                        container.setAttribute("id", "DetaliiFacturareID");
                        container.setAttribute("class", "form-container");

                        var t0 = document.createElement("h3");
                        t0.setAttribute("style", "text-align: center; margin: auto;");
                        t0.innerHTML = "Persoana: ";
                        t0.setAttribute("id", "tag8");
                        container.appendChild(t0);

                        var t1 = document.createElement("div");
                        container.appendChild(t1);
                        t1.setAttribute("class", "box");
                        var inp1 = document.createElement("input");
                        t1.appendChild(inp1);
                        inp1.setAttribute("class", "Switcher__checkbox sr-only");
                        inp1.setAttribute("id", "io");
                        inp1.setAttribute("onchange", "changePersoana(this)");
                        inp1.setAttribute("type", "checkbox");
                        var label1 = document.createElement("label");
                        t1.appendChild(label1);
                        label1.setAttribute("class", "Switcher");
                        label1.setAttribute("for", "io");
                        label1.setAttribute("style", "background-color: #3c8dc5;");
                        var d1 = document.createElement("div");
                        label1.appendChild(d1);
                        d1.setAttribute("class", "Switcher__trigger");
                        d1.setAttribute("style", "color: white;");
                        d1.setAttribute("data-value", "Fizica");
                        var d2 = document.createElement("div");
                        label1.appendChild(d2);
                        d2.setAttribute("class", "Switcher__trigger");
                        d2.setAttribute("style", "color: white;");
                        d2.setAttribute("data-value", "Juridica");

                        var t2 = document.createElement("div");
                        container.appendChild(t2);
                        t2.setAttribute("class", "personal-information container-login100-form-btn p-t-10 input100");
                        t2.setAttribute("style", "padding-top: 12px;margin-bottom: 10px;background:#068c95;");
                        var select = document.createElement("select");
                        select.setAttribute("id", "selectPersoanaFacturareID");
                        select.setAttribute("data-IdNomPartener", "0");
                        select.setAttribute("style", "display: inline-block;margin: inherit; border-radius: 22px; margin-left: 12px;");
                        select.setAttribute("onchange", "changePersoanaFacturare()");
                        t2.appendChild(select);

                        var t3 = document.createElement("div");
                        container.appendChild(t3);
                        t3.setAttribute("class", "wrap-input100 validate-input m-b-10");
                        if (defaultLang == "RO") {
                            t3.setAttribute("data-validate", "Nume obligatoriu");
                        } else {
                            t3.setAttribute("data-validate", "Name is required");
                        }
                        var inp3 = document.createElement("input");
                        t3.appendChild(inp3);
                        inp3.setAttribute("id", "txtNume");
                        inp3.setAttribute("maxlength", "64");
                        inp3.setAttribute("class", "input100");
                        inp3.setAttribute("type", "text");
                        if (defaultLang == "RO") {
                            inp3.setAttribute("placeholder", "Nume *");
                        } else {
                            inp3.setAttribute("placeholder", "Name *");
                        }
                        var spn3 = document.createElement("span");
                        t3.appendChild(spn3);
                        spn3.setAttribute("class", "focus-input100");
                        var spn3_1 = document.createElement("span");
                        spn3_1.setAttribute("class", "symbol-input100");
                        t3.appendChild(spn3_1);
                        var i3 = document.createElement("i");
                        spn3_1.appendChild(i3);
                        i3.setAttribute("class", "fa fa-user");

                        var t4 = document.createElement("div");
                        container.appendChild(t4);
                        t4.setAttribute("class", "wrap-input100 validate-input m-b-10");
                        if (defaultLang == "RO") {
                            t4.setAttribute("data-validate", "Prenume obligatoriu");
                        } else {
                            t4.setAttribute("data-validate", "Surname is required");
                        }
                        var inp4 = document.createElement("input");
                        t4.appendChild(inp4);
                        inp4.setAttribute("id", "txtPrenume");
                        inp4.setAttribute("class", "input100");
                        inp4.setAttribute("maxlength", "64");
                        inp4.setAttribute("type", "text");
                        if (defaultLang == "RO") {
                            inp4.setAttribute("placeholder", "Prenume *");
                        } else {
                            inp4.setAttribute("placeholder", "Surname *");
                        }
                        var spn4 = document.createElement("span");
                        t4.appendChild(spn4);
                        spn4.setAttribute("class", "focus-input100");
                        var spn4_1 = document.createElement("span");
                        spn4_1.setAttribute("class", "symbol-input100");
                        t4.appendChild(spn4_1);
                        var i4 = document.createElement("i");
                        spn4_1.appendChild(i4);
                        i4.setAttribute("class", "fa fa-user");

                        var t5 = document.createElement("div");
                        container.appendChild(t5);
                        t5.setAttribute("class", "wrap-input100 validate-input m-b-10");
                        if (defaultLang == "RO") {
                            t5.setAttribute("data-validate", "Email obligatoriu");
                        } else {
                            t5.setAttribute("data-validate", "Email required");
                        }
                        var inp5 = document.createElement("input");
                        t5.appendChild(inp5);
                        inp5.setAttribute("id", "txtEmail");
                        inp5.setAttribute("class", "input100");
                        inp5.setAttribute("maxlength", "30");
                        inp5.setAttribute("type", "email");
                        inp5.setAttribute("placeholder", "Email *");
                        var spn5 = document.createElement("span");
                        t5.appendChild(spn5);
                        spn5.setAttribute("class", "focus-input100");
                        var spn5_1 = document.createElement("span");
                        spn5_1.setAttribute("class", "symbol-input100");
                        t5.appendChild(spn5_1);
                        var i5 = document.createElement("i");
                        spn5_1.appendChild(i5);
                        i5.setAttribute("class", "fa fa-user");

                        var t6 = document.createElement("div");
                        container.appendChild(t6);
                        t6.setAttribute("class", "wrap-input100 m-b-10");
                        if (defaultLang == "RO") {
                            t6.setAttribute("data-validate", "Adresa obligatoriu");
                        } else {
                            t6.setAttribute("data-validate", "Adress required");
                        }
                        var inp6 = document.createElement("input");
                        t6.appendChild(inp6);
                        inp6.setAttribute("id", "txtAdresa");
                        inp6.setAttribute("class", "input100");
                        inp6.setAttribute("type", "text");
                        inp6.setAttribute("maxlength", "50");
                        if (defaultLang == "RO") {
                            inp6.setAttribute("placeholder", "Adresa ");
                        } else {
                            inp6.setAttribute("placeholder", "Adress ");
                        }
                        var spn6 = document.createElement("span");
                        t6.appendChild(spn6);
                        spn6.setAttribute("class", "focus-input100");
                        var spn6_1 = document.createElement("span");
                        spn6_1.setAttribute("class", "symbol-input100");
                        t6.appendChild(spn6_1);
                        var i6 = document.createElement("i");
                        spn6_1.appendChild(i6);
                        i6.setAttribute("class", "fa fa-building");

                        var t7 = document.createElement("div");
                        container.appendChild(t7);
                        t7.setAttribute("class", "wrap-input100 validate-input m-b-10");
                        if (defaultLang == "RO") {
                            t7.setAttribute("data-validate", "Localitatea obligatoriu");
                        } else {
                            t7.setAttribute("data-validate", "City required");
                        }
                        var inp7 = document.createElement("input");
                        t7.appendChild(inp7);
                        inp7.setAttribute("id", "txtLocalitatea");
                        inp7.setAttribute("class", "input100");
                        inp7.setAttribute("maxlength", "30");
                        inp7.setAttribute("type", "text");
                        if (defaultLang == "RO") {
                            inp7.setAttribute("placeholder", "Localitatea *");
                        } else {
                            inp7.setAttribute("placeholder", "City *");
                        }
                        var spn7 = document.createElement("span");
                        t7.appendChild(spn7);
                        spn7.setAttribute("class", "focus-input100");
                        var spn7_1 = document.createElement("span");
                        spn7_1.setAttribute("class", "symbol-input100");
                        t7.appendChild(spn7_1);
                        var i7 = document.createElement("i");
                        spn7_1.appendChild(i7);
                        i7.setAttribute("class", "fa fa-institution");

                        var t8 = document.createElement("div");
                        container.appendChild(t8);
                        t8.setAttribute("class", "wrap-input100 validate-input m-b-10");
                        if (defaultLang == "RO") {
                            t8.setAttribute("data-validate", "Tara obligatoriu");
                        } else {
                            t8.setAttribute("data-validate", "Country required");
                        }
                        var inp8 = document.createElement("input");
                        t8.appendChild(inp8);
                        inp8.setAttribute("id", "txtTara");
                        inp8.setAttribute("class", "input100");
                        inp8.setAttribute("type", "text");
                        inp8.setAttribute("maxlength", "32");
                        if (defaultLang == "RO") {
                            inp8.setAttribute("placeholder", "Tara *");
                        } else {
                            inp8.setAttribute("placeholder", "Country *");
                        }
                        var spn8 = document.createElement("span");
                        t8.appendChild(spn8);
                        spn8.setAttribute("class", "focus-input100");
                        var spn8_1 = document.createElement("span");
                        spn8_1.setAttribute("class", "symbol-input100");
                        t8.appendChild(spn8_1);
                        var i8 = document.createElement("i");
                        spn8_1.appendChild(i8);
                        i8.setAttribute("class", "fa fa-institution");

                        var t9 = document.createElement("div");
                        container.appendChild(t9);
                        t9.setAttribute("class", "wrap-input100 m-b-10");
                        if (defaultLang == "RO") {
                            t9.setAttribute("data-validate", "Telefon obligatoriu");
                        } else {
                            t9.setAttribute("data-validate", "Phone rquired");
                        }
                        var inp9 = document.createElement("input");
                        t9.appendChild(inp9);
                        inp9.setAttribute("id", "txtTelefon");
                        inp9.setAttribute("class", "input100");
                        inp9.setAttribute("type", "text");
                        inp9.setAttribute("maxlength", "20");
                        if (defaultLang == "RO") {
                            inp9.setAttribute("placeholder", "Telefon ");
                        } else {
                            inp9.setAttribute("placeholder", "Phone number ");
                        }
                        var spn9 = document.createElement("span");
                        t9.appendChild(spn9);
                        spn9.setAttribute("class", "focus-input100");
                        var spn9_1 = document.createElement("span");
                        spn9_1.setAttribute("class", "symbol-input100");
                        t9.appendChild(spn9_1);
                        var i9 = document.createElement("i");
                        spn9_1.appendChild(i9);
                        i9.setAttribute("class", "fa fa-institution");
                    }
                    try{
                        getClientiRezervare();
                    } catch (err) {
                        //try { loading.out(); } catch (err) { }
                    }
                }
                else {
                    //try{
                    //    loading.out();
                    //} catch (err) { }
                    alertify.alert("Nu exista date de afisat");
                    rezervareOK = false;
                    setTimeout(reload, 2500);
                }
            },
            error: function () {
                rezervariOK = false;
                codRezervare = "";
                alertify.alert("Sa pierdut conexiunea, dati un refresh");
                sejurAchitat = false;
                return false;
            }
        });
        try{
            loading.out();
        } catch (err) { }
   // }, 500);
}

function getDateClient(id, objID) {
    $.ajax({
        url: 'PlataForm2.aspx/getDetaliiClient',
        data: "{id:'" + id + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (partener) {
            if (partener.d) {
                partener = partener.d;
                var doc = document.getElementById(objID);
                doc.innerHTML = doc.innerHTML.replace("$[Cumparator]", partener.NumePartener ? partener.NumePartener:"---");
                doc.innerHTML = doc.innerHTML.replace("$[RegComcumparator]", partener.RegCom && partener.RegCom!="null" ? partener.RegCom : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Cifcumparator]", partener.CodFiscalNumar && partener.CodFiscalNumar != "null" ? partener.CodFiscalNumar : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Sediulcumparator]", partener.Strada && partener.Strada != "null" ? partener.Strada : "---");
                doc.innerHTML = doc.innerHTML.replace("$[ContCumparator]", partener.ContBanca && partener.ContBanca != "null" ? partener.ContBanca : "---");
                doc.innerHTML = doc.innerHTML.replace("$[BancaCumparator]", partener.Banca && partener.Banca != "null" ? partener.Banca : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Delegat]", partener.Nume + " " + partener.Prenume);
                doc.innerHTML = doc.innerHTML.replace("$[CIDelegat]", partener.CodFiscalNumar && partener.CodFiscalNumar != "null" ? partener.CodFiscalNumar : "---");
            }
        },
        error: function () {
            //alert('nu am incarcat turistul');
        }
    });

    $.ajax({
        url: 'PlataForm2.aspx/getDetaliiFurnizor',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (partenerFurnizor) {
            if (partenerFurnizor.d) {
                partenerFurnizor = partenerFurnizor.d;
                var doc = document.getElementById(objID);
                doc.innerHTML = doc.innerHTML.replace("$[Furnizor]", partenerFurnizor.NumePartener ? partenerFurnizor.NumePartener : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Capitalsocial]", partenerFurnizor.CapitalSocial ? partenerFurnizor.CapitalSocial : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Regcom]", partenerFurnizor.RegCom ? partenerFurnizor.RegCom : "---");
                doc.innerHTML = doc.innerHTML.replace("$[Cif]", partenerFurnizor.CodFiscalAtribut + " " + partenerFurnizor.CodFiscalNumar);
                doc.innerHTML = doc.innerHTML.replace("$[Sediul]", partenerFurnizor.Judet + " " + partenerFurnizor.Oras + " " + partenerFurnizor.Strada + " " + partenerFurnizor.Nr + " " + partenerFurnizor.Bloc);
                doc.innerHTML = doc.innerHTML.replace("$[Cont]", partenerFurnizor.ContBanca ? partenerFurnizor.ContBanca : "---");
            }
        },
        error: function () {
            //alert('nu am incarcat turistul');
        }
    });
}

var iesireAnticipata = false;
function checkOutBtn(thing) {
    $.ajax({
        url: 'PlataForm2.aspx/checkOutClick',
        data: "{iesireAnticipata:" + thing + "}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d == 0) {
                if (defaultLang == "RO") {
                    alertify.alert("CheckOut realizat cu succese, Va mai asteptam !!! ");
                } else {
                    alertify.alert("Checkout completed successfully, See you soon!!! ");
                }
                iesireAnticipata = false;
                setTimeout(reload, 3000);
            } else if (ok.d == 1) {
                if (defaultLang == "RO") {
                    alertify.alert("CheckOut nu s-a realizat, exista plati neachitate.");
                } else {
                    alertify.alert("Checkout has not been completed, there are unpaid payments.");
                }
            } else if (ok.d == 2) {
                if (defaultLang == "RO") {
                    alertify.confirm("Sigur doriti sa faceti CheckOut anticipat ?",
                              function (e) {
                                  if (e) {
                                      alertify.success('Iesire anticipata');
                                      checkOutBtn(true);
                                  } else {
                                      alertify.error('Renunta');
                                  }
                              });
                } else {
                    alertify.confirm("Sure you want to make CheckOut in advance ?",
                                  function (e) {
                                      if (e) {
                                          alertify.success('CheckOut in advance');
                                          checkOutBtn(true);
                                      } else {
                                          alertify.success('Cancel');
                                      }
                                  });
                }
            } else {
                if (defaultLang == "RO") {
                    alertify.alert("CheckOut nu s-a realizat, a intervenit o eroare necunoascuta.");
                } else {
                    alertify.alert("CheckOut was not completed, an unknown error occurred.");
                }
            }
        },
        error: function (error) {
            if (defaultLang == "RO") {
                alertify.alert("A aparut o eroare la conexiune, dati un refresh");
            } else {
                alertify.alert("Connection lost, please refresh");
            }
            iesireAnticipata = false;
        }
    });
}

function changePersoana(thing) {
    var container = document.getElementById("DetaliiFacturareID");
    container.innerHTML = "";
    if (thing.checked) {
        var t0_ = document.createElement("h3");
        t0_.setAttribute("style", "text-align: center; margin: auto;");
        t0_.setAttribute("id", "tag8");
        if (defaultLang == "RO") {
            t0_.innerHTML = "Persoana: ";
        } else {
            t0_.innerHTML = "Person: ";
        }
        
        container.appendChild(t0_);

        var t = document.createElement("div");
        container.appendChild(t);
        t.setAttribute("class", "box");
        var inp = document.createElement("input");
        t.appendChild(inp);
        inp.setAttribute("class", "Switcher__checkbox sr-only");
        inp.setAttribute("id", "io");
        inp.setAttribute("onchange", "changePersoana(this)");
        inp.setAttribute("type", "checkbox");
        inp.setAttribute("checked", "checked");
        var label = document.createElement("label");
        t.appendChild(label);
        label.setAttribute("class", "Switcher");
        label.setAttribute("for", "io");
        label.setAttribute("style", "background-color: #3c8dc5;");
        var d1 = document.createElement("div");
        label.appendChild(d1);
        d1.setAttribute("class", "Switcher__trigger");
        d1.setAttribute("style", "color: white;");
        if (defaultLang == "RO") {
            d1.setAttribute("data-value", "Fizica");
        } else {
            d1.setAttribute("data-value", "Individual");
        }
        var d2 = document.createElement("div");
        label.appendChild(d2);
        d2.setAttribute("class", "Switcher__trigger");
        d2.setAttribute("style", "color: white;");
        if (defaultLang == "RO") {
            d2.setAttribute("data-value", "Juridica");
        } else {
            d2.setAttribute("data-value", "Legal");
        }
       
        var t0 = document.createElement("div");
        container.appendChild(t0);
        t0.setAttribute("id", "divCIFid");
        t0.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t0.setAttribute("data-validate", "CIF obligatoriu");
        } else {
            t0.setAttribute("data-validate", "UIC obligatoriu");
        }
        var inp0 = document.createElement("input");
        t0.appendChild(inp0);
        inp0.setAttribute("id", "txtCUI");
        inp0.setAttribute("class", "input100");
        inp0.setAttribute("type", "text");
        inp0.setAttribute("maxlength", "15");
        if (defaultLang == "RO") {
            inp0.setAttribute("placeholder", "CIF *");
        } else {
            inp0.setAttribute("placeholder", "Unique Identification Code *");
        }
        var btn0 = document.createElement("button");
        t0.appendChild(btn0);
        btn0.setAttribute("class", "open-button");
        btn0.setAttribute("type", "button");
        btn0.setAttribute("onclick", "descarcaDateFirmaClick()");
        var img0 = document.createElement("img");
        btn0.appendChild(img0);
        img0.setAttribute("class", "open-button");
        img0.setAttribute("src", "../../Icoane/descarca.png");
        var spn0 = document.createElement("span");
        t0.appendChild(spn0);
        spn0.setAttribute("class", "focus-input100");
        var spn0_1 = document.createElement("span");
        t0.appendChild(spn0_1);
        spn0_1.setAttribute("class", "symbol-input100");
        spn0_1.setAttribute("onclick", "descarcaDateFirmaClick()");
        var i0 = document.createElement("i");
        spn0_1.appendChild(i0);
        i0.setAttribute("class", "fa fa-pencil-square");
        i0.setAttribute("onclick", "descarcaDateFirmaClick()");

        var t1 = document.createElement("div");
        container.appendChild(t1);
        t1.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t1.setAttribute("data-validate", "Denumire obligatoriu");
        } else {
            t1.setAttribute("data-validate", "Name required");
        }
        var inp1 = document.createElement("input");
        t1.appendChild(inp1);
        inp1.setAttribute("id", "txtDenumire");
        inp1.setAttribute("class", "input100");
        inp1.setAttribute("type", "text");
        inp1.setAttribute("maxlength", "100");
        if (defaultLang == "RO") {
            inp1.setAttribute("placeholder", "Denumire firma *");
        } else {
            inp1.setAttribute("placeholder", "Company name *");
        }
        var spn1 = document.createElement("span");
        t1.appendChild(spn1);
        spn1.setAttribute("class", "focus-input100");
        var spn1_1 = document.createElement("span");
        spn1_1.setAttribute("class", "symbol-input100");
        t1.appendChild(spn1_1);
        var i1 = document.createElement("i");
        spn1_1.appendChild(i1);
        i1.setAttribute("class", "fa fa-user");

        var t2 = document.createElement("div");
        container.appendChild(t2);
        t2.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t2.setAttribute("data-validate", "Email obligatoriu");
        } else {
            t2.setAttribute("data-validate", "Email required");
        }
        var inp2 = document.createElement("input");
        t2.appendChild(inp2);
        inp2.setAttribute("id", "txtEmail");
        inp2.setAttribute("class", "input100");
        inp2.setAttribute("type", "email");
        inp2.setAttribute("maxlength", "30");
        inp2.setAttribute("placeholder", "Email *");
        var spn2 = document.createElement("span");
        t2.appendChild(spn2);
        spn2.setAttribute("class", "focus-input100");
        var spn2_1 = document.createElement("span");
        spn2_1.setAttribute("class", "symbol-input100");
        t2.appendChild(spn2_1);
        var i2 = document.createElement("i");
        spn2_1.appendChild(i2);
        i2.setAttribute("class", "fa fa-user");

        var t3 = document.createElement("div");
        container.appendChild(t3);
        t3.setAttribute("class", "wrap-input100 m-b-10");
        if (defaultLang == "RO") {
            t3.setAttribute("data-validate", "Telefon obligatoriu");
        } else {
            t3.setAttribute("data-validate", "Phone required");
        }
        var inp3 = document.createElement("input");
        t3.appendChild(inp3);
        inp3.setAttribute("id", "txtTelefon");
        inp3.setAttribute("class", "input100");
        inp3.setAttribute("maxlength", "20");
        inp3.setAttribute("type", "text");
        if (defaultLang == "RO") {
            inp3.setAttribute("placeholder", "Telefon");
        } else {
            inp3.setAttribute("placeholder", "Phone");
        }
        var spn3 = document.createElement("span");
        t3.appendChild(spn3);
        spn3.setAttribute("class", "focus-input100");
        var spn3_1 = document.createElement("span");
        spn3_1.setAttribute("class", "symbol-input100");
        t3.appendChild(spn3_1);
        var i3 = document.createElement("i");
        spn3_1.appendChild(i3);
        i3.setAttribute("class", "fa fa-user");

        var t4 = document.createElement("div");
        container.appendChild(t4);
        t4.setAttribute("class", "wrap-input100  m-b-10");
        if (defaultLang == "RO") {
            t4.setAttribute("data-validate", "Adresa obligatoriu");
        } else {
            t4.setAttribute("data-validate", "Adress required");
        }
        var inp4 = document.createElement("input");
        t4.appendChild(inp4);
        inp4.setAttribute("id", "txtAdresa");
        inp4.setAttribute("class", "input100");
        inp4.setAttribute("type", "text");
        inp4.setAttribute("maxlength", "50");
        if (defaultLang == "RO") {
            inp4.setAttribute("placeholder", "Punct de lucru ");
        } else {
            inp4.setAttribute("placeholder", "Work place ");
        }
        var spn4 = document.createElement("span");
        t4.appendChild(spn4);
        spn4.setAttribute("class", "focus-input100");
        var spn4_1 = document.createElement("span");
        spn4_1.setAttribute("class", "symbol-input100");
        t4.appendChild(spn4_1);
        var i4 = document.createElement("i");
        spn4_1.appendChild(i4);
        i4.setAttribute("class", "fa fa-building");

        var t5 = document.createElement("div");
        container.appendChild(t5);
        t5.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t5.setAttribute("data-validate", "Localitatea obligatoriu");
        } else {
            t5.setAttribute("data-validate", "City required");
        }
        var inp5 = document.createElement("input");
        t5.appendChild(inp5);
        inp5.setAttribute("id", "txtLocalitatea");
        inp5.setAttribute("class", "input100");
        inp5.setAttribute("type", "text");
        inp5.setAttribute("maxlength", "30");
        if (defaultLang == "RO") {
            inp5.setAttribute("placeholder", "Localitatea *");
        } else {
            inp5.setAttribute("placeholder", "City *");
        }
        var spn5 = document.createElement("span");
        t5.appendChild(spn5);
        spn5.setAttribute("class", "focus-input100");
        var spn5_1 = document.createElement("span");
        spn5_1.setAttribute("class", "symbol-input100");
        t5.appendChild(spn5_1);
        var i5 = document.createElement("i");
        spn5_1.appendChild(i5);
        i5.setAttribute("class", "fa fa-institution");

        var t6 = document.createElement("div");
        container.appendChild(t6);
        t6.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t6.setAttribute("data-validate", "Tara obligatoriu");
        } else {
            t6.setAttribute("data-validate", "Country required");
        }
        var inp6 = document.createElement("input");
        t6.appendChild(inp6);
        inp6.setAttribute("id", "txtTara");
        inp6.setAttribute("class", "input100");
        inp6.setAttribute("type", "text");
        inp6.setAttribute("maxlength", "32");
        if (defaultLang == "RO") {
            inp6.setAttribute("placeholder", "Tara *");
        } else {
            inp6.setAttribute("placeholder", "Country *");
        }
        var spn6 = document.createElement("span");
        t6.appendChild(spn6);
        spn6.setAttribute("class", "focus-input100");
        var spn6_1 = document.createElement("span");
        spn6_1.setAttribute("class", "symbol-input100");
        t6.appendChild(spn6_1);
        var i6 = document.createElement("i");
        spn6_1.appendChild(i6);
        i6.setAttribute("class", "fa fa-institution");

        var t7 = document.createElement("div");
        container.appendChild(t7);
        t7.setAttribute("class", "wrap-input100  m-b-10");
        if (defaultLang == "RO") {
            t7.setAttribute("data-validate", "RegCom obligatoriu");
        } else {
            t7.setAttribute("data-validate", "T.Reg. required");
        }
        var inp7 = document.createElement("input");
        t7.appendChild(inp7);
        inp7.setAttribute("id", "txtRegCom");
        inp7.setAttribute("class", "input100");
        inp7.setAttribute("type", "text");
        inp7.setAttribute("maxlength", "30");
        if (defaultLang == "RO") {
            inp7.setAttribute("placeholder", "Registru comert ");
        } else {
            inp7.setAttribute("placeholder", "Trade register");
        }
        var spn7 = document.createElement("span");
        t7.appendChild(spn7);
        spn7.setAttribute("class", "focus-input100");
        var spn7_1 = document.createElement("span");
        spn7_1.setAttribute("class", "symbol-input100");
        t7.appendChild(spn7_1);
        var i7 = document.createElement("i");
        spn7_1.appendChild(i7);
        i7.setAttribute("class", "fa fa-pencil-square");

        var t8 = document.createElement("div");
        container.appendChild(t8);
        t8.setAttribute("class", "personal-information container-login100-form-btn p-t-10 input100");
        t8.setAttribute("style", "padding-top: 12px;margin-bottom: 10px;background:#dcdbdb;");
        var select = document.createElement("select");
        t8.appendChild(select);
        select.setAttribute("id", "selectPersoanaFacturareID");
        select.setAttribute("data-iddelegat", "0");
        select.setAttribute("style", "display: inline-block;margin: inherit; border-radius: 22px; margin-left: 12px;");
        select.setAttribute("onchange", "changePersoanaFacturare()");
        var option0 = document.createElement("option");
        option0.setAttribute("value", "0");
        option0.setAttribute("id", "tag9");
        if (defaultLang == "RO") {
            option0.innerHTML = "-Selecteaza delegat-";
        } else {
            option0.innerHTML = "-Select Delegate-";
        }
        select.appendChild(option0);
        
        var t9 = document.createElement("div");
        container.appendChild(t9);
        t9.setAttribute("class", "container-login100-form-btn p-t-10");
        t9.setAttribute("style", "padding: 0;");

        var t9_1 = document.createElement("div");
        t9.appendChild(t9_1);
        t9_1.setAttribute("class", "wrap-input100 m-b-10");
        t9_1.setAttribute("style", "width:49%");
        if (defaultLang == "RO") {
            t9_1.setAttribute("data-validate", "Nume delegat");
        } else {
            t9_1.setAttribute("data-validate", "Delegate name");
        }
        var inp9_1 = document.createElement("input");
        t9_1.appendChild(inp9_1);
        inp9_1.setAttribute("id", "txtNumePrenumeDelagat");
        inp9_1.setAttribute("class", "input100");
        inp9_1.setAttribute("maxlength", "50");
        inp9_1.setAttribute("type", "text");
        if (defaultLang == "RO") {
            inp9_1.setAttribute("placeholder", "Denumire delegat ");
        } else {
            inp9_1.setAttribute("placeholder", "Delegate ");
        }
        var spn9_1 = document.createElement("span");
        t9_1.appendChild(spn9_1);
        spn9_1.setAttribute("class", "focus-input100");
        var spn9_1_ = document.createElement("span");
        t9_1.appendChild(spn9_1_);
        spn9_1_.setAttribute("class", "symbol-input100");
        var i9_1 = document.createElement("i");
        spn9_1_.appendChild(i9_1);
        i9_1.setAttribute("class", "fa fa-pencil-square");

        var t9_2 = document.createElement("div");
        t9.appendChild(t9_2);
        t9_2.setAttribute("class", "wrap-input100  m-b-10");
        t9_2.setAttribute("style", "width:49%");
        if (defaultLang == "RO") {
            t9_2.setAttribute("data-validate", "CI Delegat ");
        } else {
            t9_2.setAttribute("data-validate", "Delegate identitty ");
        }
        var inp9_2 = document.createElement("input");
        t9_2.appendChild(inp9_2);
        inp9_2.setAttribute("id", "txtCIDelegat");
        inp9_2.setAttribute("class", "input100");
        inp9_2.setAttribute("type", "text");
        inp9_2.setAttribute("maxlength", "15");
        if (defaultLang == "RO") {
            inp9_2.setAttribute("placeholder", "CI Delegat ");
        } else {
            inp9_2.setAttribute("placeholder", "Delegate identitty ");
        }
        var spn9_2 = document.createElement("span");
        t9_2.appendChild(spn9_2);
        spn9_2.setAttribute("class", "focus-input100");
        var spn9_2_ = document.createElement("span");
        t9_2.appendChild(spn9_2_);
        spn9_2_.setAttribute("class", "symbol-input100");
        var i9_2 = document.createElement("i");
        spn9_2_.appendChild(i9_2);
        i9_2.setAttribute("class", "fa fa-pencil-square");


    } else {
        var t0 = document.createElement("h3");
        t0.setAttribute("style", "text-align: center; margin: auto;");
        t0.setAttribute("id", "tag8");
        if (defaultLang == "RO") {
            t0.innerHTML = "Persoana: ";
        } else {
            t0.innerHTML = "Person: ";
        }
        container.appendChild(t0);

        var t1 = document.createElement("div");
        container.appendChild(t1);
        t1.setAttribute("class", "box");
        var inp1 = document.createElement("input");
        t1.appendChild(inp1);
        inp1.setAttribute("class", "Switcher__checkbox sr-only");
        inp1.setAttribute("id", "io");
        inp1.setAttribute("onchange", "changePersoana(this)");
        inp1.setAttribute("type", "checkbox");
        var label1 = document.createElement("label");
        t1.appendChild(label1);
        label1.setAttribute("class", "Switcher");
        label1.setAttribute("for", "io");
        label1.setAttribute("style", "background-color: #3c8dc5;");
        var d1 = document.createElement("div");
        label1.appendChild(d1);
        d1.setAttribute("class", "Switcher__trigger");
        d1.setAttribute("style", "color: white;");
        if (defaultLang == "RO") {
            d1.setAttribute("data-value", "Fizica");
        } else {
            d1.setAttribute("data-value", "Individual");
        }
        var d2 = document.createElement("div");
        label1.appendChild(d2);
        d2.setAttribute("class", "Switcher__trigger");
        d2.setAttribute("style", "color: white;");
        if (defaultLang == "RO") {
            d2.setAttribute("data-value", "Juridica");
        } else {
            d2.setAttribute("data-value", "Legal");
        }

        var t2 = document.createElement("div");
        container.appendChild(t2);
        t2.setAttribute("class", "personal-information container-login100-form-btn p-t-10 input100");
        t2.setAttribute("style", "padding-top: 12px;margin-bottom: 10px;background:#dcdbdb;");
        var select = document.createElement("select");
        select.setAttribute("id", "selectPersoanaFacturareID");
        select.setAttribute("data-IdNomPartener", "0");
        select.setAttribute("style", "display: inline-block;margin: inherit; border-radius: 22px; margin-left: 12px;");
        select.setAttribute("onchange", "changePersoanaFacturare()");
        var option0 = document.createElement("option");
        option0.setAttribute("value", "0");
        option0.setAttribute("id", "tag10");
        if (defaultLang == "RO") {
            option0.innerHTML = "-Selecteaza Client-";
        } else {
            option0.innerHTML = "-Choose customer-";
        }
        select.appendChild(option0);
        t2.appendChild(select);

        var t3 = document.createElement("div");
        container.appendChild(t3);
        t3.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t3.setAttribute("data-validate", "Nume obligatoriu");
        } else {
            t3.setAttribute("data-validate", "Name is required");
        }
        var inp3 = document.createElement("input");
        t3.appendChild(inp3);
        inp3.setAttribute("id", "txtNume");
        inp3.setAttribute("class", "input100");
        inp3.setAttribute("type", "text");
        inp3.setAttribute("maxlength", "64");
        if (defaultLang == "RO") {
            inp3.setAttribute("placeholder", "Nume *");
        } else {
            inp3.setAttribute("placeholder", "Name *");
        }
        var spn3 = document.createElement("span");
        t3.appendChild(spn3);
        spn3.setAttribute("class", "focus-input100");
        var spn3_1 = document.createElement("span");
        spn3_1.setAttribute("class", "symbol-input100");
        t3.appendChild(spn3_1);
        var i3 = document.createElement("i");
        spn3_1.appendChild(i3);
        i3.setAttribute("class", "fa fa-user");

        var t4 = document.createElement("div");
        container.appendChild(t4);
        t4.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t4.setAttribute("data-validate", "Prenume obligatoriu");
        } else {
            t4.setAttribute("data-validate", "Surname required");
        }
        var inp4 = document.createElement("input");
        t4.appendChild(inp4);
        inp4.setAttribute("id", "txtPrenume");
        inp4.setAttribute("class", "input100");
        inp4.setAttribute("type", "text");
        inp4.setAttribute("maxlength", "64");
        if (defaultLang == "RO") {
            inp4.setAttribute("placeholder", "Prenume *");
        } else {
            inp4.setAttribute("placeholder", "Surname *");
        }
        var spn4 = document.createElement("span");
        t4.appendChild(spn4);
        spn4.setAttribute("class", "focus-input100");
        var spn4_1 = document.createElement("span");
        spn4_1.setAttribute("class", "symbol-input100");
        t4.appendChild(spn4_1);
        var i4 = document.createElement("i");
        spn4_1.appendChild(i4);
        i4.setAttribute("class", "fa fa-user");

        var t5 = document.createElement("div");
        container.appendChild(t5);
        t5.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t5.setAttribute("data-validate", "Email obligatoriu");
        } else {
            t5.setAttribute("data-validate", "Email required");
        }
        var inp5 = document.createElement("input");
        t5.appendChild(inp5);
        inp5.setAttribute("id", "txtEmail");
        inp5.setAttribute("class", "input100");
        inp5.setAttribute("type", "email");
        inp5.setAttribute("placeholder", "Email *");
        inp5.setAttribute("maxlength", "30");
        var spn5 = document.createElement("span");
        t5.appendChild(spn5);
        spn5.setAttribute("class", "focus-input100");
        var spn5_1 = document.createElement("span");
        spn5_1.setAttribute("class", "symbol-input100");
        t5.appendChild(spn5_1);
        var i5 = document.createElement("i");
        spn5_1.appendChild(i5);
        i5.setAttribute("class", "fa fa-user");

        var t6 = document.createElement("div");
        container.appendChild(t6);
        t6.setAttribute("class", "wrap-input100 m-b-10");
        if (defaultLang == "RO") {
            t6.setAttribute("data-validate", "Adresa obligatoriu");
        } else {
            t6.setAttribute("data-validate", "Adress required");
        }
        var inp6 = document.createElement("input");
        t6.appendChild(inp6);
        inp6.setAttribute("id", "txtAdresa");
        inp6.setAttribute("class", "input100");
        inp6.setAttribute("type", "text");
        inp6.setAttribute("maxlength", "50");
        if (defaultLang == "RO") {
            inp6.setAttribute("placeholder", "Adresa ");
        } else {
            inp6.setAttribute("placeholder", "Adress ");
        }
        var spn6 = document.createElement("span");
        t6.appendChild(spn6);
        spn6.setAttribute("class", "focus-input100");
        var spn6_1 = document.createElement("span");
        spn6_1.setAttribute("class", "symbol-input100");
        t6.appendChild(spn6_1);
        var i6 = document.createElement("i");
        spn6_1.appendChild(i6);
        i6.setAttribute("class", "fa fa-building");

        var t7 = document.createElement("div");
        container.appendChild(t7);
        t7.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t7.setAttribute("data-validate", "Localitatea obligatoriu");
        } else {
            t7.setAttribute("data-validate", "City obligatoriu");
        }
        var inp7 = document.createElement("input");
        t7.appendChild(inp7);
        inp7.setAttribute("id", "txtLocalitatea");
        inp7.setAttribute("class", "input100");
        inp7.setAttribute("type", "text");
        inp7.setAttribute("maxlength", "30");
        if (defaultLang == "RO") {
            inp7.setAttribute("placeholder", "Localitatea *");
        } else {
            inp7.setAttribute("placeholder", "City *");
        }
        var spn7 = document.createElement("span");
        t7.appendChild(spn7);
        spn7.setAttribute("class", "focus-input100");
        var spn7_1 = document.createElement("span");
        spn7_1.setAttribute("class", "symbol-input100");
        t7.appendChild(spn7_1);
        var i7 = document.createElement("i");
        spn7_1.appendChild(i7);
        i7.setAttribute("class", "fa fa-institution");

        var t8 = document.createElement("div");
        container.appendChild(t8);
        t8.setAttribute("class", "wrap-input100 validate-input m-b-10");
        if (defaultLang == "RO") {
            t8.setAttribute("data-validate", "Tara obligatoriu");
        } else {
            t8.setAttribute("data-validate", "Country required");
        }
        var inp8 = document.createElement("input");
        t8.appendChild(inp8);
        inp8.setAttribute("id", "txtTara");
        inp8.setAttribute("class", "input100");
        inp8.setAttribute("type", "text");
        inp8.setAttribute("maxlength", "32");
        if (defaultLang == "RO") {
            inp8.setAttribute("placeholder", "Tara *");
        } else {
            inp8.setAttribute("placeholder", "Contry *");
        }
        var spn8 = document.createElement("span");
        t8.appendChild(spn8);
        spn8.setAttribute("class", "focus-input100");
        var spn8_1 = document.createElement("span");
        spn8_1.setAttribute("class", "symbol-input100");
        t8.appendChild(spn8_1);
        var i8 = document.createElement("i");
        spn8_1.appendChild(i8);
        i8.setAttribute("class", "fa fa-institution");

        var t9 = document.createElement("div");
        container.appendChild(t9);
        t9.setAttribute("class", "wrap-input100  m-b-10");
        if (defaultLang == "RO") {
            t9.setAttribute("data-validate", "Telefon obligatoriu");
        } else {
            t9.setAttribute("data-validate", "Phone required");
        }
        var inp9 = document.createElement("input");
        t9.appendChild(inp9);
        inp9.setAttribute("id", "txtTelefon");
        inp9.setAttribute("class", "input100");
        inp9.setAttribute("type", "text");
        inp9.setAttribute("maxlength", "20");
        if (defaultLang == "RO") {
            inp9.setAttribute("placeholder", "Telefon ");
        } else {
            inp9.setAttribute("placeholder", "Phone number ");
        }
        var spn9 = document.createElement("span");
        t9.appendChild(spn9);
        spn9.setAttribute("class", "focus-input100");
        var spn9_1 = document.createElement("span");
        spn9_1.setAttribute("class", "symbol-input100");
        t9.appendChild(spn9_1);
        var i9 = document.createElement("i");
        spn9_1.appendChild(i9);
        i9.setAttribute("class", "fa fa-institution");
    }
    getClientiRezervare();
}

function getClientiRezervare() {
    $.ajax({
        url: 'PlataForm2.aspx/getClientRezervare',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                try{
                    var select = document.getElementById("selectPersoanaFacturareID");
                    select.innerHTML = "";
                    var i = 0, n = ok.d.length;
                    var defaultOption = document.createElement("option");
                    var io = document.getElementById("io").checked;
                    if (io) {
                        if (defaultLang == "RO") {
                            defaultOption.setAttribute("value", "-Selecteaza delegat-");
                            defaultOption.innerHTML = "-Selecteaza delagat-";
                        } else {
                            defaultOption.setAttribute("value", "-Chose delegate-");
                            defaultOption.innerHTML = "-Choose delegate-";
                        }
                    } else {
                        if (defaultLang == "RO") {
                            defaultOption.setAttribute("value", "-Selecteaza client-");
                            defaultOption.innerHTML = "-Selecteaza client-";
                        } else {
                            defaultOption.setAttribute("value", "-Chose client-");
                            defaultOption.innerHTML = "-Chose client-";
                        }
                    }
                    select.appendChild(defaultOption);
                    for (i = 0; i < n; i++) {
                        var option = document.createElement("option");
                        select.appendChild(option);
                        option.setAttribute("data-NumePartener", ok.d[i].NumePartener);
                        option.setAttribute("data-FirstName", ok.d[i].Nume);
                        option.setAttribute("data-LastName", ok.d[i].Prenume);
                        option.setAttribute("data-Localitate", ok.d[i].Oras);
                        option.setAttribute("data-Strada", ok.d[i].Strada);
                        option.setAttribute("data-Tara", ok.d[i].Tara);
                        option.innerHTML = ok.d[i].NumePartener;
                        option.setAttribute("data-CodFiscalNumar", ok.d[i].CodFiscalNumar);
                        option.setAttribute("data-RegCom", ok.d[i].RegCom);
                        if (ok.d[i].ContDeEmail) {
                            option.setAttribute("data-Email", ok.d[i].ContDeEmail);
                        } else {
                            option.setAttribute("data-Email", ok.d[i].MailAddress);
                        }
                        option.setAttribute("data-Telefon", ok.d[i].Telefon);
                        option.setAttribute("data-IdNomPartener", ok.d[i].IdPartener);
                    }
                } catch (err) { }
            }
        },
        error: function (msg) {}
    });
}

function changePersoanaFacturare() {
    var d = document.getElementById("selectPersoanaFacturareID");
    var value = d[d.selectedIndex];
    var io = document.getElementById("io").checked;
    if (io) {
        document.getElementById("txtNumePrenumeDelagat").value = value.getAttribute("data-FirstName") + " " + value.getAttribute("data-LastName");
        if (value.getAttribute("data-Strada")) {
            document.getElementById("txtAdresa").value = value.getAttribute("data-Strada");
        }
        if (value.getAttribute("data-Localitate")) {
            document.getElementById("txtLocalitatea").value = value.getAttribute("data-Localitate");
        }
        if (value.getAttribute("data-Tara")) {
            document.getElementById("txtTara").value = value.getAttribute("data-Tara");
        }
        if (value.getAttribute("data-Telefon")) {
            document.getElementById("txtTelefon").value = value.getAttribute("data-Telefon");
        }
        if (value.getAttribute("data-Email")) {
            document.getElementById("txtEmail").value = value.getAttribute("data-Email");
        }
        if (document.getElementById("txtCUI")) {
            document.getElementById("txtCUI").value = value.getAttribute("data-CodFiscalNumar");
        }
        if (value.getAttribute("data-NumePartener")) {
            document.getElementById("txtDenumire").value = value.getAttribute("data-NumePartener");
        }
        d.setAttribute("data-IdNomPartener", value.getAttribute("data-IdNomPartener"));
    } else {
        if (value.getAttribute("data-FirstName")) {
            document.getElementById("txtNume").value = value.getAttribute("data-FirstName");
        }
        if (value.getAttribute("data-LastName")) {
            document.getElementById("txtPrenume").value = value.getAttribute("data-LastName");
        }
        if (value.getAttribute("data-Strada")) {
            document.getElementById("txtAdresa").value = value.getAttribute("data-Strada");
        }
        if (value.getAttribute("data-Localitate")) {
            document.getElementById("txtLocalitatea").value = value.getAttribute("data-Localitate");
        }
        if (value.getAttribute("data-Tara")) {
            document.getElementById("txtTara").value = value.getAttribute("data-Tara");
        }
        if (document.getElementById("txtCUI")) {
            document.getElementById("txtCUI").value = value.getAttribute("data-CodFiscalNumar");
        }
        if (value.getAttribute("data-Telefon")) {
            document.getElementById("txtTelefon").value = value.getAttribute("data-Telefon");
        }
        if (value.getAttribute("data-Email")) {
            document.getElementById("txtEmail").value = value.getAttribute("data-Email");
        }
        d.setAttribute("data-IdNomPartener", value.getAttribute("data-IdNomPartener"));
    }
}

function descarcaDateFirmaClick() {
    var CIF = document.getElementById("txtCUI").value;
    if (CIF != "") {
        checkCui(CIF);
    } else {
        if (defaultLang == "RO") {
            $("#txtCUI").notify("Introduceti un CUI valid", "error");
            alertify.error('Introduceti un CUI valid');
        } else {
            $("#txtCUI").notify("Enter a valid identifier number", "error");
            alertify.error('Enter a valid identifier number');
        }
    }
}

function checkCui(input) {
    $.ajax({
        url: 'PlataForm2.aspx/IsCIF',
        data: "{cif: '" + input + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                getDateFirma(input);
            } else {
                if (defaultLang == "RO") {
                    $("#txtCUI").notify("Introduceti un CUI valid", "error");
                    alertify.error('Introduceti un CUI valid');
                } else {
                    $("#txtCUI").notify("Enter a valid identifier number", "error");
                    alertify.error('Enter a valid identifier number');
                }
            }
        },
        error: function () {
            if (defaultLang == "RO") {
                $("#txtCUI").notify("A aparut o eroare la conexiunea, dati un refresh", "error");
                alertify.error('A aparut o eroare la conexiunea, dati un refresh');
            } else {
                $("#txtCUI").notify("Connection lost, please refresh", "error");
                alertify.error('Connection lost, please refresh');
            }
            return null;
        }
    });
}

function getDateFirma(input) {
    $.ajax({
        url: 'PlataForm2.aspx/getDateFirma',
        data: "{cifIntrodus: '" + input + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                var divDenumire = document.getElementById("divCuButoaneID");
                if (divDenumire != null) {
                    while (divDenumire.firstChild) {
                        divDenumire.removeChild(divDenumire.firstChild);
                    }
                }
                var n = ok.d.length;
                if (n > 1) {
                    var DivCuButoane = document.getElementById("divCuButoaneID")
                    var i = 0;
                    for (i = 0; i < n; i++) {
                        var buton = document.createElement("button");
                        buton.type = "button"
                        buton.style.width = "100%";
                        buton.setAttribute("class", "btnFirma");
                        buton.innerHTML = ok.d[i].NumePartener;
                        var denumire = ok.d[i].NumePartener;
                        var regCom = ok.d[i].RegCom;
                        var oras = ok.d[i].Oras;
                        buton.setAttribute("data-denumire", denumire);
                        buton.setAttribute("data-regCom", regCom);
                        buton.setAttribute("data-oras", oras);
                        buton.setAttribute("onclick", "AlegetButon(this)");
                        DivCuButoane.appendChild(buton);
                    }
                    document.getElementById("declanseazaModal").click();
                } else if (n == 1) {
                    document.getElementById("txtDenumire").value = ok.d[0].NumePartener;
                    document.getElementById("txtLocalitatea").value = ok.d[0].Oras;
                    if (document.getElementById("txtRegCom")) {
                        document.getElementById("txtRegCom").value = ok.d[0].RegCom;
                    }
                }
            }
        },
        error: function () {
            if (defaultLang == "RO") {
                $("#txtCUI").notify("A aparut o eroare la conexiune, dati un refresh", "error");
                alertify.error('A aparut o eroare la conexiunea, dati un refresh');
            } else {
                $("#txtCUI").notify("Connection lost, please refresh", "error");
                alertify.error('Connection lost, please refresh');
            }
            return null;
        }
    });
}

function AlegetButon(thing) {
    var denumire = thing.getAttribute("data-denumire");
    var regCom = thing.getAttribute("data-regCom");
    var oras = thing.getAttribute("data-oras");
    document.getElementById("txtDenumire").value = denumire;
    document.getElementById("txtLocalitatea").value = oras;
    if (document.getElementById("txtRegCom")) {
        document.getElementById("txtRegCom").value = regCom;
    }
    document.getElementById("closeModal").click();
}

function printezaFactura() {
    var loading;
    if (defaultLang == "RO") {
        loading = new Loading({
            title: ' Va rugam asteptati',
            direction: 'hor',
            discription: 'Se printeaza factura...',
            defaultApply: true,
        });
    } else {
        loading = new Loading({
            title: ' Please wait',
            direction: 'hor',
            discription: 'The invoice is printing...',
            defaultApply: true,
        });
    }
    setTimeout(function () {
    var IdNomPartener = document.getElementById("selectPersoanaFacturareID").getAttribute("data-IdNomPartener");
    $.ajax({
        url: 'PlataForm2.aspx/printeazaDocument',
        data: "{IdNomPartener:'" + IdNomPartener + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            try {
                loading.out();
            } catch (err) { }
            var fisier = response.d;
            if (fisier == null) {
                if (defaultLang == "RO") {
                    $("#btnPrinteazaFacturaID").notify("A aparut o eroare la descarcare", "error");
                    alertify.error('A aparut o eroare la descarcare');
                } else {
                    $("#btnPrinteazaFacturaID").notify("An error occurred at saving invoice", "error");
                    alertify.error('An error occurred at saving invoice');
                }
            } else {
                saveData(new Uint8Array(fisier.Continut), fisier.Denumire, fisier.Tip);
                if (defaultLang == "RO") {
                    $("#btnPrinteazaFacturaID").notify("Fisier salvat", "success");
                    alertify.success('Fisier salvat');
                } else {
                    $("#btnPrinteazaFacturaID").notify("File saved", "success");
                    alertify.success('File saved');
                }
            }
        },
        error: function () {
            if (defaultLang == "RO") {
                $("#btnPrinteazaFacturaID").notify("Sa pierdut conexiunea, dati un refresh", "error");
                alertify.error('A aparut o eroare la conexiunea, dati un refresh');
            } else {
                $("#btnPrinteazaFacturaID").notify("Connection lost, please refresh", "error");
                alertify.error('Connection lost, please refresh');
            }
            try {
                loading.out();
            } catch (err) { }
            return null;
        }
    });
        try {
            loading.out();
        } catch (err) { }
    }, 500);
}

function trimiteEmailFactura() {
    var loading;
    if (defaultLang == "RO") {
         loading = new Loading({
            title: ' Va rugam asteptati',
            direction: 'hor',
            discription: 'Se trimite factura...',
            defaultApply: true,
        });
    } else {
         loading = new Loading({
            title: ' Please wait',
            direction: 'hor',
            discription: 'The invoice is sending on email...',
            defaultApply: true,
        });
    }
    setTimeout(function () {
    var IdNomPartener = document.getElementById("selectPersoanaFacturareID").getAttribute("data-IdNomPartener");
    $.ajax({
        url: 'PlataForm2.aspx/trimiteFacturaPeMail',
        data: "{IdNomPartener:'" + IdNomPartener + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            try {
                loading.out();
            } catch (err) { }
            if (ok.d) {
                if (defaultLang == "RO") {
                    $("#btnEmailID").notify("Datele au fost trimise", "success");
                    alertify.success('Datele au fost trimise');
                } else {
                    $("#btnEmailID").notify("Data were send", "success");
                    alertify.success('Datele au fost trimise');
                }
            } else {
                if (defaultLang == "RO") {
                    $("#btnEmailID").notify("A aparut o eroare la trimitere pe email", "error"); //error
                    alertify.error('A aparut o eroare la trimitere pe email');
                } else {
                    $("#btnEmailID").notify("An error occurred at sending email", "error"); //error
                    alertify.error('An error occurred at sending email');
                }
            }
        },
        error: function () {
            if (defaultLang == "RO") {
                $("#btnEmailID").notify("Sa pierdut conexiunea, dati un refresh", "error");
                alertify.error('Sa pierdut conexiunea, dati un refresh');
            } else {
                $("#btnEmailID").notify("Connection lost, please refresh", "error");
                alertify.error('Connection lost, please refresh');
            }
            try {
                loading.out();
            } catch (err) { }
            return null;
        }
    });
        try{
            loading.out();
        } catch (err) { }
         }, 500);
}

function saveData(data, fileName, fileType) {
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";

    var json = JSON.stringify(data),
        blob = new Blob([data], { type: fileType }),
        url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
}

function plataCash() {
    var FirstName = document.getElementById("txtNume");
    if (FirstName) {
        FirstName = FirstName.value;
    }
    var LastName = document.getElementById("txtPrenume");
    if (LastName) {
        LastName = LastName.value;
    }
    var Tara = document.getElementById("txtTara");
    if (Tara) {
        Tara = Tara.value;
    }
    var Localitate = document.getElementById("txtLocalitatea");
    if (Localitate) {
        Localitate = Localitate.value;
    }
    var Strada = document.getElementById("txtAdresa");
    if (Strada) {
        Strada = Strada.value;
    }
    var Email = document.getElementById("txtEmail");
    if (Email) {
        Email = Email.value;
    }
    var CUI = document.getElementById("txtCUI");
    if (CUI) {
        CUI = CUI.value;
    }
    var RegCom = document.getElementById("txtRegCom");
    if (RegCom) {
        RegCom = RegCom.value;
    }
    var Telefon = document.getElementById("txtTelefon");
    if (Telefon) {
        Telefon = Telefon.value;
    }
    var Denumire = document.getElementById("txtDenumire");
    if (Denumire) {
        Denumire = Denumire.value;
    }
    var CNP = document.getElementById("txtCIDelegat");
    if (CNP) {
        CNP = CNP.value;
    }
    var Delegat = document.getElementById("txtNumePrenumeDelagat");
    if (Delegat) {
        Delegat = Delegat.value;
    }

    var io = document.getElementById("io");
    if (io.checked) {//juridic
        io = false;
    } else {//fizic
        io = true;
    }
    var IdNomPartener = document.getElementById("selectPersoanaFacturareID").getAttribute("data-IdNomPartener");
        $.ajax({
            url: 'PlataForm2.aspx/plataCashFunction',
            data: "{IdNomPartener:'" + IdNomPartener + "',io:" + io + ",FirstName:'" + FirstName + "',LastName:'" + LastName + "',Tara:'" + Tara + "',Localitate:'" + Localitate + "',Strada:'" + Strada + "',Email:'" + Email + "',CUI:'" + CUI + "',RegCom:'" + RegCom + "',Telefon:'" + Telefon + "',Denumire:'" + Denumire + "',CNP:'" + CNP + "',Delegat:'" + Delegat + "'}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d.Suma > 0) {
                    try {
                        var listaProduse = [];
                        var i = 0, n = ok.d.lista.length, j=0;
                        for (i = 0; i < n; i++) {
                            var camera=ok.d.lista[i];
                            for(j=0;j<camera.entitateServiciiLista.length;j++){
                                var produs = {
                                    Denumire: camera.entitateServiciiLista[j].DenumireServiciu,
                                    Pret: camera.entitateServiciiLista[j].SoldRON,
                                    Cantitate: camera.entitateServiciiLista[j].Cantitate,
                                    UM:camera.entitateServiciiLista[j].UM,
                                    CodCotaTVA: camera.entitateServiciiLista[j].CodCotaTVA
                                }
                                listaProduse.push(produs);
                            }
                        }
                        var str = JSON.stringify(listaProduse);
                        //callbackObj.showMessage(ok.d.Suma + "", str);
                        updateAjaxPlataCash();
                    } catch (err) {
                        if (defaultLang == "RO") {
                            alertify.alert("Plata cash disponibila doar la kiosk");
                        } else {
                            alertify.alert("Cash only available at the kiosk");
                        }
                    }
                } else {
                    if (defaultLang == "RO") {
                        alertify.alert("Cash payment was not made");
                    } else {
                        alertify.alert("");
                    }
                }
            },
            error: function (error) {
                alertify.alert("A aparut o eroare la conexiune, dati un refresh");
            }
        });
}

function updateAjaxPlataCash() {
    //alertify.alert("Plata sa realizat ok");
    $.ajax({
        url: 'PlataForm2.aspx/achitaCash',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                if (defaultLang == "RO") {
                    alertify.alert("Plata cash sa realizat cu success");
                } else {
                    alertify.alert("Cash payment successfully done");
                }
               
                var tabelDate = document.getElementById("tg-rwLtp").innerHTML;
                var tabFinal = document.getElementById("tabFinal"); tabFinal.innerHTML = "";
                var divWrap = document.createElement("div");
                tabFinal.appendChild(divWrap);
                divWrap.setAttribute("class", "tg-wrap");
                divWrap.setAttribute("style", "background: white;");
                var tableLP = document.createElement("table");
                divWrap.appendChild(tableLP);
                tableLP.setAttribute("id", "tg-rwLtp");
                tableLP.setAttribute("class", "tg");
                tableLP.innerHTML = tabelDate;

                var h3 = document.createElement("h3");
                h3.setAttribute("style", "margin:auto;text-align:center;");
                if (defaultLang == "RO") {
                    h3.innerHTML = "Finalizare sejur";
                } else {
                    h3.innerHTML = "Finish";
                }
                h3.setAttribute("id", "tag6");

                h3.setAttribute("class", "titluForm");
                tabFinal.appendChild(h3);

                var divButtons = document.createElement("div");
                divButtons.setAttribute("id", "divButtons");
                divButtons.setAttribute("class", "container-login100-form-btn p-t-10");
                tabFinal.appendChild(divButtons);
                var btnPrinteaza = document.createElement("button");
                btnPrinteaza.setAttribute("type", "button");
                btnPrinteaza.setAttribute("class", "login100-form-btn");
                btnPrinteaza.setAttribute("id", "btnPrinteazaFacturaID");
                btnPrinteaza.setAttribute("style", "width:49%");
                btnPrinteaza.setAttribute("onclick", "printezaFactura()");
                var imaginePrinter = '<img src="../../Icoane/printer.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                if (defaultLang == "RO") {
                    btnPrinteaza.innerHTML = imaginePrinter + "Printeaza factura";
                } else {
                    btnPrinteaza.innerHTML = imaginePrinter + "Print invoice";
                }
                divButtons.appendChild(btnPrinteaza);
                var btnEmail = document.createElement("button");
                btnEmail.setAttribute("type", "button");
                btnEmail.setAttribute("class", "login100-form-btn");
                btnEmail.setAttribute("id", "btnEmailID");
                btnEmail.setAttribute("style", "width:49%");
                btnEmail.setAttribute("onclick", "trimiteEmailFactura()");
                var imagineEmail = '<img src="../../Icoane/email.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                if (defaultLang == "RO") {
                    btnEmail.innerHTML = imagineEmail + "Trimite factura email";
                } else {
                    btnEmail.innerHTML = imagineEmail + "Email invoice";
                }
                divButtons.appendChild(btnEmail);
                var checkOutBTN = document.createElement("div");
                checkOutBTN.setAttribute("class", "login100-form-btn");
                checkOutBTN.setAttribute("style", "margin-top: 15px;");
                var checkOutImagine = '<img src="../../Icoane/ok.png" style="width: 32px;height: 32px; margin-right: 12px;" />';
                checkOutBTN.innerHTML = checkOutImagine+"CheckOut";
                checkOutBTN.setAttribute("onclick", "checkOutBtn("+false+")");
                tabFinal.appendChild(checkOutBTN);


               
                //adauga aici text
                //var divWrap = document.createElement("div");
                //tabFinal.appendChild(divWrap);
                //divWrap.setAttribute("class", "tg-wrap");
                //divWrap.setAttribute("style", "background: white;");
                //var tableLP = document.createElement("table");
                //divWrap.appendChild(tableLP);
                //tableLP.setAttribute("id", "tg-rwLtp");
                //tableLP.setAttribute("class", "tg");
                //var trTitlu = document.createElement("tr");
                //tableLP.appendChild(trTitlu);
                //var th_0T = document.createElement("th");
                //th_0T.setAttribute("class", "tg-0pky");
                //th_0T.setAttribute("colspan", "2");
                //th_0T.innerHTML = "Furnizor: $[Furnizor]<br>Capital social: $[Capitalsocial]<br>Reg com: $[Regcom]<br>Codul Fiscal: $[Cif]<br>Sediul: $[Sediul]<br>$[Cont]";
                //trTitlu.appendChild(th_0T);
                //var th_1T = document.createElement("th");
                //th_1T.setAttribute("class", "tg-baqh");
                //th_1T.setAttribute("colspan", "3");
                //trTitlu.appendChild(th_1T);
                //var span_1T = document.createElement("span");;
                // th_1T.appendChild(span_1T);
                // span_1T.setAttribute("style", "font-weight:700");
                // span_1T.innerHTML = "Seria: $[Seria] Nr. $[Numar]";
                //th_1T.innerHTML = '<span style="font-weight:700">Seria: $[Seria] Nr. $[Numar]</span><br><span style="font-weight:700">Factura</span><br><span style="font-weight:700">Fiscala</span><br><span style="font-weight:700">Nr. facturii: $[Numar]</span><br><span style="font-weight:700">Data: $[Data]</span><br><span style="font-weight:700">$[PL]</span><br><span style="font-weight:700">$[Storno]</span>';
                //var th_2T = document.createElement("th");
                //trTitlu.appendChild(th_2T);
                //th_2T.setAttribute("class", "tg-0lax");
                //th_2T.setAttribute("colspan", "3");
                //th_2T.innerHTML = 'Cumparator: $[Cumparator]<br>Nr. Reg. Com.: $[RegComcumparator]<br>CUI/CNP: $[Cifcumparator]<br>Sediul: $[Sediulcumparator]<br>$[ContCumparator]<br>$[BancaCumparator]';

                //var CampTabele = document.createElement("tr");
                //tableLP.appendChild(CampTabele);
                //CampTabele.innerHTML = ' <td class="tg-fymr">Nr.<br>Crt.</td> <td class="tg-kiyi">Denumirea produselor sau a serviciilor</td><td class="tg-amwm">Cota <br>TVA<br><br></td><td class="tg-amwm">U.M.</td><td class="tg-88nc">Cantitate</td><td class="tg-amwm">Pret unitar <br>(fara TVA)<br>lei</td> <td class="tg-amwm">Valoare<br>(fara TVA)<br>lei</td><td class="tg-88nc">Valoare<br>TVA<br>LEI</td>';

                //var trSubsol = document.createElement("tr");
                //tableLP.appendChild(trSubsol);
                //trSubsol.innerHTML = '<td class="tg-0lax" colspan="8">$[ObsSubsol]<br>Termen de plata $[TermenDePlata] zile<br>Intocmit: $[Intocmit]</td>';

                //var trSubsol2 = document.createElement("tr");
                //tableLP.appendChild(trSubsol2);
                //trSubsol2.innerHTML = '<td class="tg-0lax" rowspan="2">Semnatura si stampila<br>furnizorului<br><br><br><br></td><td class="tg-0lax" rowspan="2">Data privind expeditia<br>Numele delegatului: $[Delegat]<br>Buletin/cartea de identitate: $[CIDelegat]<br>Mijloc de transport: $[Transport]</td><td class="tg-0lax" colspan="2">Total LEI</td><td class="tg-0lax" colspan="2">$[TotalFaraTVA]</td> <td class="tg-0lax" colspan="2">$[TVA]</td>';

                //var trSubsol3 = document.createElement("tr");
                //tableLP.appendChild(trSubsol3);
                //trSubsol3.innerHTML = '<td class="tg-0lax" colspan="2">Semnatura de primire<br><br></td><td class="tg-0lax" colspan="2">Total de plata LEI</td> <td class="tg-0lax" colspan="2">$[Total]</td>';

            } else {
                if (defaultLang == "RO") {
                    alertify.alert("Plata cash nu sa realizat, a intervenit o eroare");
                } else {
                    alertify.alert("The cash payment was not made, an error occurred");
                }
            }
        },
        error: function (error) {
            if (defaultLang == "RO") {
                alertify.alert("A aparut o eroare la conexiune, dati un refresh");
            } else {
                alertify.alert("Connection lost, please refresh");
            }
        }
    });
}

function plataCard() {
    var FirstName = document.getElementById("txtNume");
    if (FirstName) {
        FirstName = FirstName.value;
    }
    var LastName = document.getElementById("txtPrenume");
    if (LastName) {
        LastName = LastName.value;
    }
    var Tara = document.getElementById("txtTara");
    if (Tara) {
        Tara = Tara.value;
    }
    var Localitate = document.getElementById("txtLocalitatea");
    if (Localitate) {
        Localitate = Localitate.value;
    }
    var Strada = document.getElementById("txtAdresa");
    if (Strada) {
        Strada = Strada.value;
    }
    var Email = document.getElementById("txtEmail");
    if (Email) {
        Email = Email.value;
    }
    var CUI = document.getElementById("txtCUI");
    if (CUI) {
        CUI = CUI.value;
    }
    var RegCom = document.getElementById("txtRegCom");
    if (RegCom) {
        RegCom = RegCom.value;
    }
    var Telefon = document.getElementById("txtTelefon");
    if (Telefon) {
        Telefon = Telefon.value;
    }
    var Denumire = document.getElementById("txtDenumire");
    if (Denumire) {
        Denumire = Denumire.value;
    }
    var CNP = document.getElementById("txtCIDelegat");
    if (CNP) {
        CNP = CNP.value;
    }
    var Delegat = document.getElementById("txtNumePrenumeDelagat");
    if (Delegat) {
        Delegat = Delegat.value;
    }

    var io = document.getElementById("io");
    if (io.checked) {//juridic
        io = false;
    } else {//fizic
        io = true;
    }
    var IdNomPartener = document.getElementById("selectPersoanaFacturareID").getAttribute("data-IdNomPartener");
    $.ajax({
        url: 'PlataForm2.aspx/plataCardFunction',
        data: "{IdNomPartener:'" + IdNomPartener + "',io:" + io + ",FirstName:'" + FirstName + "',LastName:'" + LastName + "',Tara:'" + Tara + "',Localitate:'" + Localitate + "',Strada:'" + Strada + "',Email:'" + Email + "',CUI:'" + CUI + "',RegCom:'" + RegCom + "',Telefon:'" + Telefon + "',Denumire:'" + Denumire + "',CNP:'" + CNP + "',Delegat:'" + Delegat + "'}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                ReplaceContent(ok.d);
            } else {
                if (defaultLang == "RO") {
                    alertify.alert("Plata card nu sa realizat");
                } else {
                    alertify.alert("Online payment wasn't finished ");
                }
            }
        },
        error: function (error) {
            if (defaultLang == "RO") {
                alertify.alert("A aparut o eroare la conexiune, dati un refresh");
            } else {
                alertify.alert("Connection lost, please refresh");
            }
        }
    });
}

function ReplaceContent(NC) {
    document.open();
    document.write(NC);
    document.close();
}

function goToAlin() {
    //$.ajax({
    //    url: 'PlataForm2.aspx/goToAlin',
    //    data: "{}",
    //    type: 'POST',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    cache: false,
    //    async: false,
    //    success: function (ok) {
    //        if (ok.d) {
    //            window.open("http://rezervari.multisoft.ro/Booking/?Hotel=13TYD3&CheckIn=20-12-2018&CheckOut=21-12-2018", "_top");
    //        } else {
    //            alertify.error('Nu se poate naviga la rezervare');
    //        }
    //    },
    //    error: function (msg) {
    //        alertify.alert("Sa pierdut conexiunea, dati un refresh");
    //    }
    //});
    window.location = "RezervaIFrame.aspx";
}

function checkCookie() {
    var limba = getCookie("lang");
    if (limba != "") {
        defaultLang = limba;
    } else {
        limba = defaultLang;
        if (limba != "" && limba != null) {
            setCookie("lang", limba, 1);
        }
    }
}

function toRomanian() {
    defaultLang = "RO";
    setCookie("lang", defaultLang, 1);
    changeLanguage();
}
function toEnglish() {
    defaultLang = "EN";
    setCookie("lang", defaultLang, 1);
    changeLanguage();
}

var dicLab = {
    'EN': {
        'tag1': { 'isValue': '0', 'Text': 'Language' },
        'tag2': { 'isValue': '0', 'Text': 'Home' },
        'tag3': { 'isValue': '0', 'Text': 'Reservation' },
        'CamereCountID': { 'isValue': '0', 'Text': 'Rooms' },
        'tipPersoanaID': { 'isValue': '0', 'Text': 'Person: ' },
        'prevBtn': { 'isValue': '0', 'Text': 'Back' },
        'nextBtn': { 'isValue': '0', 'Text': 'Next' },
        //'btnPlataCash': { 'isValue': '0', 'Text': 'Cash payment' },
        //'btnPlataCard': { 'isValue': '0', 'Text': 'Online payment' },
        'closeModal': { 'isValue': '0', 'Text': 'X close' },
        'tag6': { 'isValue': '0', 'Text': 'Finish' },
        'tag7': { 'isValue': '0', 'Text': 'Finish' },
        'tag8': { 'isValue': '0', 'Text': 'Person:' },
       // 'btnPrinteazaFacturaID': { 'isValue': '0', 'Text': 'Print invoice' },
        //'btnEmailID': { 'isValue': '0', 'Text': 'Email invoice' },
    },
    'RO': {
        'tag1': { 'isValue': '0', 'Text': 'Limba' },
        'tag2': { 'isValue': '0', 'Text': 'Acasa' },
        'tag3': { 'isValue': '0', 'Text': 'Rezervare' },
        'CamereCountID': { 'isValue': '0', 'Text': 'Camere' },
        'tipPersoanaID': { 'isValue': '0', 'Text': 'Sunt persoana' },
        'prevBtn': { 'isValue': '0', 'Text': 'Inapoi' },
        'nextBtn': { 'isValue': '0', 'Text': 'Inainte' },
        //'btnPlataCash': { 'isValue': '0', 'Text': 'Plata cash' },
        //'btnPlataCard': { 'isValue': '0', 'Text': 'Plata card' },
        'closeModal': { 'isValue': '0', 'Text': 'X Inchide' },
        'tag6': { 'isValue': '0', 'Text': 'Finalizare sejur' },
        'tag7': { 'isValue': '0', 'Text': 'Finalizare plata' },
        'tag8': { 'isValue': '0', 'Text': 'Persoana:' },
        //'btnPrinteazaFacturaID': { 'isValue': '0', 'Text': 'Printeaza factura' },
        //'btnEmailID': { 'isValue': '0', 'Text': 'Trimite factura email' },

    }
};

function changeLanguage() {
    for (i in dicLab[defaultLang]) {
        try {
            if (dicLab[defaultLang][i].isValue == '1') {
                var lis = document.getElementsByName(i);
                for (var elem in lis) {
                    lis[elem].innerHTML = dicLab[defaultLang][i].Text;
                }
            } else {
                document.getElementById(i).innerHTML = dicLab[defaultLang][i].Text;
            }
        } catch (err) { }
    }
}
