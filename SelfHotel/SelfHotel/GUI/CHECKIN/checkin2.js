var defaultLang = "RO";
$(document).ready(function () {
    $(function () {
        $(".field-wrapper .field-placeholder").on("click", function () {
            $(this).closest(".field-wrapper").find("input").focus();
        });
        $(".field-wrapper input").bind("change keyup", function () {  //on keyup
            var value = $.trim($(this).val());
            if (value) {
                $(this).closest(".field-wrapper").addClass("hasValue");
            } else {
                $(this).closest(".field-wrapper").removeClass("hasValue");
            }
        });
    });
    try{
        checkCookie();
        changeLanguage();
    } catch (err) { }
    if (defaultLang == "RO") {
        document.getElementById("pTC").innerHTML = 'Sunteti deacord cu <a id="aTC" href="#myModal" data-toggle="modal" data-target="#myModal">Termeni & Conditii</a>';
    } else {
        document.getElementById("pTC").innerHTML = 'By clicking next you agree to our <a id="aTC" href="#myModal" data-toggle="modal" data-target="#myModal">Terms & Privacy</a>';
    }
    try{
        preiaTermeniConditiiBaza();
    } catch (err) { }
});

function preiaTermeniConditiiBaza() {
    $.ajax({
        url: 'CheckIn2.aspx/getTermeniConditiiBaza',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                document.getElementById("TCContinut").innerHTML = ok.d;
            } else {
                document.getElementById("TCContinut").innerHTML = "Termeni si conditii";
            }
        },
        error: function (msg) {
            document.getElementById("TCContinut").innerHTML = "Termeni si conditii";
        }
    });
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

function incarcaDetaliiHeader() {
    $.ajax({
        url: 'CheckIn2.aspx/getSetariHeader',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                try{
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

function goHome() {
    window.location = "../../../Home.aspx";
}

var current_fs, next_fs, previous_fs; 
var left, opacity, scale; 
var animating; 
var currentTab = 0; 
showTab(currentTab); 
var codRezervare = "";
var listaFise = [];

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

function goToAlin() {
    window.location = "../CHECKOUT/RezervaIFrame.aspx";
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
        if (defaultLang == "RO") {
            document.getElementById("nextBtn").innerHTML = "Gata";
        } else {
            document.getElementById("nextBtn").innerHTML = "Finish";
        }
        document.getElementById("prevBtn").style.display = "none";
        document.getElementById("nextBtn").style.display = "none";
    } else {
        if (defaultLang == "RO") {
            document.getElementById("nextBtn").innerHTML = "Inainte";
        } else {
            document.getElementById("nextBtn").innerHTML = "Next";
        }
    }
    fixStepIndicator(n);
    if (n == 0) {
        if (defaultLang == "RO") {
            document.getElementById("nextBtn").innerHTML = "Cauta";
        } else {
            document.getElementById("nextBtn").innerHTML = "Search";
        }
    } 
    if (n == 2) {
        document.getElementById("nextBtn").style.visibility = "hidden";
    } else {
        document.getElementById("nextBtn").style.visibility = "visible";
    }
}

var rezervareOK = false;
function nextPrev(n) {
    var ok = true;
    var x = document.getElementsByClassName("tab");
    if (n == 1 && !validateForm()) return false;
    x[currentTab].style.display = "none";
    currentTab = currentTab + n;
       
    showTab(currentTab);
}

function validateForm() {
    var x, y, i, valid = true;
    x = document.getElementsByClassName("tab");
    y = x[currentTab].querySelectorAll("[required]");

    for (i = 0; i < y.length; i++) {
        if (y[i].value == "") {
            y[i].className += " invalid";
            //y[i].classList.remove("invalid");
            valid = false;
        }
    }

    if (currentTab == 0) {
        if (valid) {
            var codRezervare = document.getElementById("txtCod");
            //var loading;
            //if (defaultLang == "RO") {
            //    loading = new Loading({
            //        title: ' Va rugam asteptati',
            //        direction: 'hor',
            //        discription: 'Se cauta rezervarea...',
            //        defaultApply: true,
            //    });
            //} else {
            //    loading = new Loading({
            //        title: ' Please wait',
            //        direction: 'hor',
            //        discription: 'The reservation is searching...',
            //        defaultApply: true,
            //    });
            //}
            //setTimeout(function () {
                cautaRezervare(codRezervare.value);
                //try {
                //    loading.out();
                //} catch (err) { }
            //}, 500);
                codRezervare = codRezervare.value;
                valid = rezervareOK;

        } else {
            if (defaultLang == "RO") {
                $("#txtCod").notify("Completati codul rezervarii", "error");
            } else {
                $("#txtCod").notify("Complete reservation code", "error");
            }
            valid = false;
        }
    }

    if (valid) {
        document.getElementsByClassName("step")[currentTab].className += " finish";
    }
    return valid; 
}

function fixStepIndicator(n) {
    try{
        var i, x = document.getElementsByClassName("step");
        for (i = 0; i < x.length; i++) {
            x[i].className = x[i].className.replace(" active", "");
        }
        x[n].className += " active";
    } catch (err) { }
}

function deschideFormularPlata() {
    if (defaultLang == "RO") {
        alertify.success('mergem la plata');
    } else {
        alertify.success('go to payment');
    }
    $.ajax({
        url: 'CheckIn2.aspx/mergiLaPlata',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            //if (ok.d) {
            //    window.open("http://rezervari.multisoft.ro/Booking/?Hotel=13TYD3&CheckIn=20-12-2018&CheckOut=21-12-2018", "_top");
            //} else {
            //    alertify.error('Nu se poate naviga la rezervare');
            //}
        },
        error: function (msg) {
            //alertify.alert("Sa pierdut conexiunea, dati un refresh");
        }
    });
    window.location = "../CHECKOUT/PlataForm2.aspx";
}

function printeazaFise() {
    var str = JSON.stringify(listaFise);
    $.ajax({
        url: 'CheckIn2.aspx/printeazaDocument',
        data: "{str:" + str + "}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            var fisier = response.d;
            if (fisier == null) {
                if (defaultLang == "RO") {
                    $("#btnPrinteazaID").notify("A aparut o eroare la descarcare", "error");
                } else {
                    $("#btnPrinteazaID").notify("Can't download files", "error");
                }
            } else {
                saveData(new Uint8Array(fisier.Continut), fisier.Denumire, fisier.Tip);
                if (defaultLang == "RO") {
                    $("#btnPrinteazaID").notify("Fisier salvat", "success");
                } else {
                    $("#btnPrinteazaID").notify("File saved", "success");
                }
            }
        },
        error: function () {
            if (defaultLang == "RO") {
                $("#btnPrinteazaID").notify("Sa pierdut conexiunea, dati un refresh", "error");
            } else {
                $("#btnPrinteazaID").notify("Connection lost, please refresh", "error");
            }
            return null;
        }
    });
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

function cautaRezervare(codRezervare) {
        $.ajax({
            url: 'CheckIn2.aspx/getRezervare',
            data: "{CodRezervareTmp:'" + codRezervare + "'}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d) {
                    var divCuCamereID = document.getElementById("divCuCamereID");
                    var msform = document.getElementById("msform");
                    divCuCamereID.innerHTML = "";
                    msform.innerHTML = "";
                    if (ok.d.status == 0) {
                        if (defaultLang == "RO") {
                            alertify.alert("Rezervarea cu codul " + codRezervare + " nu a fost gasita.");
                        } else {
                            alertify.alert("Reservation with code " + codRezervare + " doesn't exist");
                        }
                        rezervareOK = false;
                    } else if (ok.d.status == 1) {
                        if (defaultLang == "RO") {
                            alertify.confirm('Rezervare este cazata deja, Doriti sa faceti plata???', function (e) {
                                if (e) {
                                    deschideFormularPlata();
                                }
                            });
                        } else {
                            alertify.confirm('Reservation is already booked, Do you want to pay ???', function (e) {
                                if (e) {
                                    deschideFormularPlata();
                                }
                            });
                        }
                        rezervareOK = false;
                    } else if (ok.d.status == 2) {
                        if (defaultLang == "RO") {
                            alertify.alert("Ziua curenta nu este ziua sosirii pentru rezervare.");
                        } else {
                            alertify.alert("The current day is not the day of arrival for the reservation.");
                        }
                        rezervareOK = false;
                    } else if (ok.d.status == 3) {
                        if (defaultLang == "RO") {
                            alertify.alert("A intervenit o eroare necunoscuta, dati un refresh.");
                        } else {
                            alertify.alert("An unknown error occured, please refresh");
                        }
                        rezervareOK = false;
                    } else if (ok.d.status == 4) {
                        var lista = ok.d.lista;
                        var i = 0, n = lista.length;
                        try {
                            if (defaultLang == "RO") {
                                document.getElementById("aNrCamere").innerHTML = "Aveti " + n + " camere rezervate pe perioada: " + ok.d.DataIn + " - " + ok.d.DataOut;
                            } else {
                                document.getElementById("aNrCamere").innerHTML = n+" rooms reserved for the period:  " + ok.d.DataIn + " - " + ok.d.DataOut;
                            }
                            for (i = 0; i < n; i++) {
                                var cameraCheckIn = document.createElement('div');
                                divCuCamereID.appendChild(cameraCheckIn);
                                cameraCheckIn.setAttribute("class", "cameraCheckIn .flex-item");

                                var row1 = document.createElement("div");
                                cameraCheckIn.appendChild(row1);
                                row1.setAttribute("class", "form-row");
                                var label1_1 = document.createElement("label");
                                row1.appendChild(label1_1);
                                label1_1.setAttribute("style", "width: -webkit-fill-available;overflow-wrap: break-word;");
                                label1_1.innerHTML = "Camera rezervata de " + lista[i].turist.NumePartener;
                                //label1_1.setAttribute("id", "tag6");
                                var context1_1 = document.createElement("div");
                                row1.appendChild(context1_1);
                                context1_1.setAttribute("class", "form-holder");
                                var input1_1 = document.createElement("input");
                                context1_1.appendChild(input1_1);
                                input1_1.setAttribute("type", "text");
                                input1_1.setAttribute("readonly", "readonly");
                                input1_1.setAttribute("class", "form-control");
                                input1_1.setAttribute("value", lista[i].Denumire);

                                var row2 = document.createElement("div");
                                cameraCheckIn.appendChild(row2);
                                row2.setAttribute("class", "form-row");
                                var label2_1 = document.createElement("label");
                                row2.appendChild(label2_1);
                                label2_1.setAttribute("style", "width: -webkit-fill-available;");
                                label2_1.innerHTML = "Nr. turisti";
                                label2_1.setAttribute("id", "tag7");
                                var context2 = document.createElement("div");
                                row2.appendChild(context2);
                                context2.setAttribute("class", "form-holder");
                                var input2 = document.createElement("input");
                                context2.appendChild(input2);
                                input2.setAttribute("type", "text");
                                input2.setAttribute("readonly", "readonly");
                                input2.setAttribute("class", "form-control");
                                input2.setAttribute("value", lista[i].NrAdulti + " adulti " + lista[i].NrCopii + " copii");

                                var row3 = document.createElement("div");
                                cameraCheckIn.appendChild(row3);
                                row3.setAttribute("class", "form-row");
                                var label3 = document.createElement("label");
                                row3.appendChild(label3);
                                label3.innerHTML = "Servicii";
                                label3.setAttribute("style", "width: -webkit-fill-available;");
                                label3.setAttribute("id","tag8");
                                var divServicii = document.createElement("div");
                                divServicii.setAttribute("class", "form-holder");
                                row3.appendChild(divServicii);
                                var ul3 = document.createElement("ul");
                                divServicii.appendChild(ul3);
                                ul3.setAttribute("style", "font-size:12px;");
                                var j = 0; m = lista[i].listaServicii.length;
                                for (j = 0; j < m; j++) {
                                    var li = document.createElement("li");
                                    li.innerHTML = lista[i].listaServicii[j].DenumireServiciu;
                                    ul3.appendChild(li);
                                }

                                var spanPret = document.createElement("span");
                                spanPret.setAttribute("style", "float: right;");
                                var txtDePlata = parseFloat(lista[i].SoldRec + lista[i].SoldVir);
                                spanPret.innerHTML = "Pret " + txtDePlata + " RON";
                                cameraCheckIn.appendChild(spanPret);
                            }
                            rezervareOK = true;
                        } catch (err) { }
                    }
                    if (rezervareOK) {
                        //creaza celelalte tab-ui
                        try{
                            var progressbar = document.createElement("ul");
                            msform.appendChild(progressbar);
                            progressbar.setAttribute("id", "progressbar");

                            for (var i = 0; i < ok.d.lista.length; i++) {
                                for (var j = 0; j < ok.d.lista[i].NrAdulti; j++) {
                                    var li0 = document.createElement("li");
                                    li0.setAttribute("style", "color: black !important;");
                                    var CameraNr = i + 1;
                                    var PersNr = j + 1;
                                    li0.innerHTML = "Cam " + CameraNr + " ,Pers " + PersNr;
                                    progressbar.appendChild(li0);
                                }
                            }
                            progressbar.firstChild.setAttribute("class", "active");

                        for (var i = 0; i < ok.d.lista.length; i++) {
                            for (var j = 0; j < ok.d.lista[i].NrAdulti; j++) {
                                var fieldset = document.createElement("fieldset");
                                fieldset.setAttribute("id", "Field_" + i + "_" + j);
                                fieldset.setAttribute("data-idcam", "");
                                fieldset.setAttribute("data-tipcam", ok.d.lista[i].Denumire);
                                fieldset.setAttribute("data-idturist", ok.d.lista[i].IdTurist);
                                fieldset.setAttribute("data-idrezervare", ok.d.lista[i].IdRezervare);
                                fieldset.setAttribute("data-idrezervarecamera", ok.d.lista[i].ID);
                                msform.appendChild(fieldset);

                                var h2 = document.createElement("h2");
                                h2.setAttribute("class", "fs-title");
                                var CameraNr = i + 1;
                                var PersNr = j + 1;
                                h2.innerHTML = "Camera " + CameraNr+" " + ok.d.lista[i].Denumire;
                                fieldset.appendChild(h2);

                                var h3 = document.createElement("h3");
                                h3.setAttribute("class", "fs-subtitle");
                                h3.innerHTML = "Persoana " + PersNr;
                                fieldset.appendChild(h3);

                                var NumePrenume = document.createElement("div");
                                NumePrenume.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputNumePrenume = document.createElement("input");
                                NumePrenume.appendChild(inputNumePrenume);
                                inputNumePrenume.setAttribute("type", "text");
                                inputNumePrenume.setAttribute("class", "txtNumePrenume");
                                inputNumePrenume.setAttribute("maxlength", "250");
                                inputNumePrenume.setAttribute("required", "required");
                                inputNumePrenume.setAttribute("oninput", "this.className = ''");
                                var divNumePrenume = document.createElement("div");
                                NumePrenume.appendChild(divNumePrenume);
                                divNumePrenume.setAttribute("class", "field-placeholder");
                                var spanNumePrenume = document.createElement("span");
                                divNumePrenume.appendChild(spanNumePrenume);
                                spanNumePrenume.innerHTML = "Nume Prenume ";
                                spanNumePrenume.setAttribute("name", "tag9");
                                var spnAsterNume = document.createElement("span");
                                spnAsterNume.setAttribute("style", "font-size: 25px;color: red;");
                                spnAsterNume.innerHTML = "*";
                                spanNumePrenume.appendChild(spnAsterNume);
                                fieldset.appendChild(NumePrenume);

                                var DataNasterii = document.createElement("div");
                                DataNasterii.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputDataNasterii = document.createElement("input");
                                DataNasterii.appendChild(inputDataNasterii);
                                inputDataNasterii.setAttribute("type", "date");
                                inputDataNasterii.setAttribute("class", "txtDataNasterii");
                                inputDataNasterii.setAttribute("oninput", "this.className = ''");
                                var divDataNasterii = document.createElement("div");
                                DataNasterii.appendChild(divDataNasterii);
                                divDataNasterii.setAttribute("class", "field-placeholder");
                                var spanDataNasterii = document.createElement("span");
                                divDataNasterii.appendChild(spanDataNasterii);
                                spanDataNasterii.innerHTML = "Data nasterii";
                                spanDataNasterii.setAttribute("name", "tag10");
                                fieldset.appendChild(DataNasterii);

                                var LocNastere = document.createElement("div");
                                LocNastere.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputLocNastere = document.createElement("input");
                                LocNastere.appendChild(inputLocNastere);
                                inputLocNastere.setAttribute("type", "text");
                                inputLocNastere.setAttribute("maxlength", "64");
                                inputLocNastere.setAttribute("class", "txtLocNastere");
                                inputLocNastere.setAttribute("oninput", "this.className = ''");
                                var divLocNastere = document.createElement("div");
                                LocNastere.appendChild(divLocNastere);
                                divLocNastere.setAttribute("class", "field-placeholder");
                                var spanLocNastere = document.createElement("span");
                                divLocNastere.appendChild(spanLocNastere);
                                spanLocNastere.innerHTML = "Locul nasterii";
                                spanLocNastere.setAttribute("name", "tag11");
                                fieldset.appendChild(LocNastere);

                                var Cetatenie = document.createElement("div");
                                Cetatenie.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputCetatenie = document.createElement("input");
                                Cetatenie.appendChild(inputCetatenie);
                                inputCetatenie.setAttribute("type", "text");
                                inputCetatenie.setAttribute("class", "txtCetatenie");
                                inputCetatenie.setAttribute("maxlength", "64");
                                inputCetatenie.setAttribute("oninput", "this.className = ''");
                                var divCetatenie = document.createElement("div");
                                Cetatenie.appendChild(divCetatenie);
                                divCetatenie.setAttribute("class", "field-placeholder");
                                var spanCetatenie = document.createElement("span");
                                divCetatenie.appendChild(spanCetatenie);
                                spanCetatenie.innerHTML = "Cetatenie";
                                spanLocNastere.setAttribute("name", "tag12");
                                fieldset.appendChild(Cetatenie);

                                var Localitatea = document.createElement("div");
                                Localitatea.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputLocalitatea = document.createElement("input");
                                Localitatea.appendChild(inputLocalitatea);
                                inputLocalitatea.setAttribute("type", "text");
                                inputLocalitatea.setAttribute("class", "txtLocalitatea");
                                inputLocalitatea.setAttribute("maxlength", "128");
                                inputLocalitatea.setAttribute("required", "required");
                                inputLocalitatea.setAttribute("oninput", "this.className = ''");
                                var divLocalitatea = document.createElement("div");
                                Localitatea.appendChild(divLocalitatea);
                                divLocalitatea.setAttribute("class", "field-placeholder");
                                var spanLocalitatea = document.createElement("span");
                                divLocalitatea.appendChild(spanLocalitatea);
                                spanLocalitatea.innerHTML = "Localitatea ";
                                spanLocalitatea.setAttribute("name", "tag13");
                                var spnAsterLocalitate = document.createElement("span");
                                spnAsterLocalitate.setAttribute("style", "font-size: 25px;color: red;");
                                spnAsterLocalitate.innerHTML = "*";
                                spanLocalitatea.appendChild(spnAsterLocalitate);
                                fieldset.appendChild(Localitatea);

                                var Strada = document.createElement("div");
                                Strada.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputStrada = document.createElement("input");
                                Strada.appendChild(inputStrada);
                                inputStrada.setAttribute("type", "text");
                                inputStrada.setAttribute("class", "txtStrada");
                                inputStrada.setAttribute("maxlength", "64");
                                inputStrada.setAttribute("oninput", "this.className = ''");
                                var divStrada = document.createElement("div");
                                Strada.appendChild(divStrada);
                                divStrada.setAttribute("class", "field-placeholder");
                                var spanStrada = document.createElement("span");
                                divStrada.appendChild(spanStrada);
                                spanStrada.innerHTML = "Strada";
                                spanStrada.setAttribute("name", "tag14");
                                fieldset.appendChild(Strada);

                                var NrStrada = document.createElement("div");
                                NrStrada.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputNrStrada = document.createElement("input");
                                NrStrada.appendChild(inputNrStrada);
                                inputNrStrada.setAttribute("type", "text");
                                inputNrStrada.setAttribute("class", "txtNrStrada");
                                inputNrStrada.setAttribute("maxlength", "16");
                                inputNrStrada.setAttribute("oninput", "this.className = ''");
                                var divNrStrada = document.createElement("div");
                                NrStrada.appendChild(divNrStrada);
                                divNrStrada.setAttribute("class", "field-placeholder");
                                var spanNrStrada = document.createElement("span");
                                divNrStrada.appendChild(spanNrStrada);
                                spanNrStrada.innerHTML = "Nr Strada";
                                spanNrStrada.setAttribute("name", "tag15");
                                fieldset.appendChild(NrStrada);

                                var Tara = document.createElement("div");
                                Tara.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputTara = document.createElement("input");
                                Tara.appendChild(inputTara);
                                inputTara.setAttribute("type", "text");
                                inputTara.setAttribute("class", "txtTara");
                                inputTara.setAttribute("required", "required");
                                inputTara.setAttribute("maxlength", "64");
                                inputTara.setAttribute("oninput", "this.className = ''");
                                var divTara = document.createElement("div");
                                Tara.appendChild(divTara);
                                divTara.setAttribute("class", "field-placeholder");
                                var spanTara = document.createElement("span");
                                divTara.appendChild(spanTara);
                                spanTara.innerHTML = "Tara ";
                                spanTara.setAttribute("name", "tag16");
                                var spnAsterTara = document.createElement("span");
                                spnAsterTara.setAttribute("style","font-size: 25px;color: red;");
                                spnAsterTara.innerHTML = "*";
                                spanTara.appendChild(spnAsterTara);
                                fieldset.appendChild(Tara);

                                var DataSosirii = document.createElement("div");
                                DataSosirii.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputDataSosirii = document.createElement("input");
                                DataSosirii.appendChild(inputDataSosirii);
                                inputDataSosirii.setAttribute("type", "date");
                                inputDataSosirii.setAttribute("class", "txtDataSosirii");
                                inputDataSosirii.setAttribute("oninput", "this.className = ''");
                                var divDataSosirii = document.createElement("div");
                                DataSosirii.appendChild(divDataSosirii);
                                divDataSosirii.setAttribute("class", "field-placeholder");
                                var spanDataSosirii = document.createElement("span");
                                divDataSosirii.appendChild(spanDataSosirii);
                                spanDataSosirii.innerHTML = "Data Sosirii";
                                spanDataSosirii.setAttribute("name", "tag17");
                                fieldset.appendChild(DataSosirii);

                                var DataPlecarii = document.createElement("div");
                                DataPlecarii.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputDataPlecarii = document.createElement("input");
                                DataPlecarii.appendChild(inputDataPlecarii);
                                inputDataPlecarii.setAttribute("type", "date");
                                inputDataPlecarii.setAttribute("class", "txtDataPlecarii");
                                inputDataPlecarii.setAttribute("oninput", "this.className = ''");
                                var divDataPlecarii = document.createElement("div");
                                DataPlecarii.appendChild(divDataPlecarii);
                                divDataPlecarii.setAttribute("class", "field-placeholder");
                                var spanDataPlecarii = document.createElement("span");
                                divDataPlecarii.appendChild(spanDataPlecarii);
                                spanDataPlecarii.innerHTML = "Data Plecarii";
                                spanDataPlecarii.setAttribute("name", "tag18");
                                fieldset.appendChild(DataPlecarii);

                                var Scop = document.createElement("div");
                                Scop.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputScop = document.createElement("input");
                                Scop.appendChild(inputScop);
                                inputScop.setAttribute("type", "text");
                                inputScop.setAttribute("class", "txtScop");
                                inputScop.setAttribute("maxlength", "50");
                                inputScop.setAttribute("oninput", "this.className = ''");
                                var divScop = document.createElement("div");
                                Scop.appendChild(divScop);
                                divScop.setAttribute("class", "field-placeholder");
                                var spanScop = document.createElement("span");
                                divScop.appendChild(spanScop);
                                spanScop.innerHTML = "Scop";
                                spanScop.setAttribute("name", "tag19");
                                fieldset.appendChild(Scop);

                                var ActIdentitate = document.createElement("div");
                                ActIdentitate.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputActIdentitate = document.createElement("input");
                                ActIdentitate.appendChild(inputActIdentitate);
                                inputActIdentitate.setAttribute("type", "text");
                                inputActIdentitate.setAttribute("class", "txtActIdentitate");
                                inputActIdentitate.setAttribute("maxlength", "16");
                                inputActIdentitate.setAttribute("oninput", "this.className = ''");
                                var divActIdentitate = document.createElement("div");
                                ActIdentitate.appendChild(divActIdentitate);
                                divActIdentitate.setAttribute("class", "field-placeholder");
                                var spanActIdentitate = document.createElement("span");
                                divActIdentitate.appendChild(spanActIdentitate);
                                spanActIdentitate.innerHTML = "Act Identitate";
                                spanActIdentitate.setAttribute("name", "tag20");
                                fieldset.appendChild(ActIdentitate);

                                var Seria = document.createElement("div");
                                Seria.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputSeria = document.createElement("input");
                                Seria.appendChild(inputSeria);
                                inputSeria.setAttribute("type", "text");
                                inputSeria.setAttribute("class", "txtSeria");
                                inputSeria.setAttribute("maxlength", "16");
                                inputSeria.setAttribute("oninput", "this.className = ''");
                                var divSeria = document.createElement("div");
                                Seria.appendChild(divSeria);
                                divSeria.setAttribute("class", "field-placeholder");
                                var spanSeria = document.createElement("span");
                                divSeria.appendChild(spanSeria);
                                spanSeria.innerHTML = "Seria";
                                spanSeria.setAttribute("name", "tag21");
                                fieldset.appendChild(Seria);

                                var Numar = document.createElement("div");
                                Numar.setAttribute("class", "field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6");
                                var inputNumar = document.createElement("input");
                                Numar.appendChild(inputNumar);
                                inputNumar.setAttribute("type", "text");
                                inputNumar.setAttribute("class", "txtNumar");
                                inputNumar.setAttribute("maxlength", "24");
                                inputNumar.setAttribute("oninput", "this.className = ''");
                                var divNumar = document.createElement("div");
                                Numar.appendChild(divNumar);
                                divNumar.setAttribute("class", "field-placeholder");
                                var spanNumar = document.createElement("span");
                                divNumar.appendChild(spanNumar);
                                spanNumar.innerHTML = "Numar";
                                spanNumar.setAttribute("name", "tag22");
                                fieldset.appendChild(Numar);

                                if (j > 0 || i>0) {
                                    var btnPrev = document.createElement("button");
                                    btnPrev.setAttribute("type", "button");
                                    btnPrev.setAttribute("class", "previous action-button");
                                    btnPrev.innerHTML = "Precedent";
                                    btnPrev.setAttribute("name", "tag23");
                                    fieldset.appendChild(btnPrev);
                                }
                                var btnNext = document.createElement("button");
                                btnNext.setAttribute("type","button");
                                btnNext.setAttribute("class","next action-button");
                                btnNext.value = "Urmatorul";
                                btnNext.setAttribute("name", "tag24");
                                fieldset.appendChild(btnNext);
                            }
                        }

                            var fieldsetFinish = document.createElement("fieldset");
                            fieldsetFinish.setAttribute("id", "Field_Final");
                            msform.appendChild(fieldsetFinish);
                            var h2Finish = document.createElement("h2");
                            h2Finish.setAttribute("class", "fs-title");
                            h2Finish.innerHTML = "Success";
                            fieldsetFinish.appendChild(h2Finish);
                            var h3Finish = document.createElement("h3");
                            h3Finish.setAttribute("class", "fs-subtitle");
                            h3Finish.innerHTML = "Toate fisele au fost completate";
                            h3Finish.setAttribute("id", "tag25");
                            fieldsetFinish.appendChild(h3Finish);
                            var inputBtnNext = document.createElement("button");
                            fieldsetFinish.appendChild(inputBtnNext);
                            inputBtnNext.setAttribute("type", "button");
                            inputBtnNext.setAttribute("onclick", "btnGataClick()");
                            inputBtnNext.setAttribute("class", "action-button");
                            inputBtnNext.setAttribute("value", "Gata");
                            inputBtnNext.innerHTML = "Gata";
                            h3Finish.setAttribute("name", "tag26");
                        } catch (err) { }
                    }
                    $(".next").click(function () {
                        current_fs = $(this).parent();
                        next_fs = $(this).parent().next();
                        var IdParinte = current_fs.attr('id');
                        if (validateForm2(IdParinte)) {
                            if (animating) return false;
                            animating = true;
                            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
                            next_fs.show();
                            current_fs.animate({ opacity: 0 }, {
                                step: function (now, mx) {
                                    scale = 1 - (1 - now) * 0.2;
                                    left = (now * 50) + "%";
                                    opacity = 1 - now;
                                    current_fs.css({
                                        'transform': 'scale(' + scale + ')',
                                        'position': 'absolute'
                                    });
                                    next_fs.css({ 'left': left, 'opacity': opacity });
                                },
                                duration: 800,
                                complete: function () {
                                    current_fs.hide();
                                    animating = false;
                                },
                                easing: 'easeInOutBack'
                            });
                        }
                    });

                    $(".previous").click(function () {
                        if (animating) return false;
                        animating = true;
                        current_fs = $(this).parent();
                        previous_fs = $(this).parent().prev();

                        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
                        previous_fs.show();
                        current_fs.animate({ opacity: 0 }, {
                            step: function (now, mx) {
                                scale = 0.8 + (1 - now) * 0.2;
                                left = ((1 - now) * 50) + "%";
                                opacity = 1 - now;
                                current_fs.css({ 'left': left });
                                previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                            },
                            duration: 800,
                            complete: function () {
                                //current_fs.hide();
                                animating = false;
                            },
                            easing: 'easeInOutBack'
                        });
                    });

                    $(function () {

                        $(".field-wrapper .field-placeholder").on("click", function () {
                            $(this).closest(".field-wrapper").find("input").focus();
                        });
                        $(".field-wrapper input").bind("keyup change", function () {  //on keyup
                            var value = $.trim($(this).val());
                            if (value) {
                                $(this).closest(".field-wrapper").addClass("hasValue");
                            } else {
                                $(this).closest(".field-wrapper").removeClass("hasValue");
                            }
                        });

                    });

                } else {
                    if (defaultLang == "RO") {
                        alertify.alert("Nu exista date de afisat");
                    } else {
                        alertify.alert("There is no data to display");
                    }
                    rezervareOK = false;
                }
                try{
                    changeLanguage();
                } catch (err) { }
            },
            error: function () {
                if (defaultLang == "RO") {
                    alertify.alert("Sa pierdut conexiunea, dati un refresh");
                } else {
                    $("#btnPrinteazaID").notify("Connection lost, please refresh", "error");
                }
                try {
                    loading.out();
                } catch (err) { }
                rezervareOK = false;
            }
        });
}

function btnGataClick() {
    listaFise = [];
    var tabCuCamere = document.getElementById("tabCuCamere");
    var camere = tabCuCamere.getElementsByTagName("fieldset");
    for (var cam = 0; cam < camere.length - 1; cam++) {
        var y = camere[cam].getElementsByTagName("input");
            var EntitateFisa = {
                NumePrenume:y[0].value,  // camere[cam].getElementsByClassName("txtNumePrenume")[0].value,
                DataNastere: y[1].value,  // camere[cam].getElementsByClassName("txtDataNasterii")[0].value,
                LocNastere: y[2].value,  // camere[cam].getElementsByClassName("txtLocNastere")[0].value,
                Cetatenie: y[3].value,  // camere[cam].getElementsByClassName("txtCetatenie")[0].value,
                Localitatea: y[4].value,  // camere[cam].getElementsByClassName("txtLocalitatea")[0].value,
                Strada: y[5].value,  // camere[cam].getElementsByClassName("txtStrada")[0].value,
                NrStrada: y[6].value,  // camere[cam].getElementsByClassName("txtNrStrada")[0].value,
                Tara: y[7].value,  // camere[cam].getElementsByClassName("txtTara")[0].value,
                DataSosire: y[8].value,  // camere[cam].getElementsByClassName("txtDataSosirii")[0].value,
                DataPlecare: y[9].value,  // camere[cam].getElementsByClassName("txtDataPlecarii")[0].value,
                Scop: y[10].value,  // camere[cam].getElementsByClassName("txtScop")[0].value,
                ActIdentitate: y[11].value,  // camere[cam].getElementsByClassName("txtActIdentitate")[0].value,
                Seria: y[12].value,  // camere[cam].getElementsByClassName("txtSeria")[0].value,
                NrAct: y[13].value,  // camere[cam].getElementsByClassName("txtNumar")[0].value,
                CodRezervare: codRezervare,
                IdCam: camere[cam].getAttribute("data-idcam"),
                TipCam: camere[cam].getAttribute("data-tipcam"),
                IdTurist: camere[cam].getAttribute("data-idturist"),
                IdRezervare: camere[cam].getAttribute("data-idrezervare"),
                IdRezervareCamera: camere[cam].getAttribute("data-idrezervarecamera")
            }
            listaFise.push(EntitateFisa);
    }
    var str = JSON.stringify(listaFise);
    $.ajax({
        url: 'CheckIn2.aspx/incarcaFiseInBaza',
        data: "{str:" + str + "}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d) {
                if (defaultLang == "RO") {
                    alertify.success("Check in realizat cu success");
                } else {
                    alertify.success("Check in done");
                }
               
                var x = document.getElementsByClassName("tab");
                x[currentTab].style.display = "none";
                currentTab = currentTab + 1;
                showTab(currentTab);
            } else {
                if (defaultLang == "RO") {
                    alertify.error('A intervenit o eroare la checkIn');
                } else {
                    alertify.error('An error occurred while checking in');
                }
            }
        },
        error: function (msg) {
            if (defaultLang == "RO") {
                alertify.alert("Sa pierdut conexiunea, dati un refresh");
            } else {
                alertify.alert("Connection lost, please refresh");
            }
        }
    });    
}

var dicLab = {
    'EN': {
        'tag0': { 'isValue': '0', 'Text': 'Fill in the arrival notification sheets' },
        'tag1': { 'isValue': '0', 'Text': 'Language' },
        'tag2': { 'isValue': '0', 'Text': 'Home' },
        'tag3': { 'isValue': '0', 'Text': 'Reservation' },
        'tag4': { 'isValue': '0', 'Text': 'Complete reservation code' },
        'tag5': { 'isValue': '0', 'Text': 'Reservation code' },
        'prevBtn': { 'isValue': '0', 'Text': 'Back' },
        'nextBtn': { 'isValue': '0', 'Text': 'Next' },
        'tag6': { 'isValue': '0', 'Text': 'Room Type' },
        'tag7': { 'isValue': '0', 'Text': 'Nr. tourists' },
        'tag8': { 'isValue': '0', 'Text': 'Services' },
        'tag9': { 'isValue': '1', 'Text': 'Name and Surname' },
        'tag10': { 'isValue': '1', 'Text': 'Date of birth' },
        'tag11': { 'isValue': '1', 'Text': 'Place of birth' },
        'tag12': { 'isValue': '1', 'Text': 'Citizenship' },
        'tag13': { 'isValue': '1', 'Text': 'Town' },
        'tag14': { 'isValue': '1', 'Text': 'Street' },
        'tag15': { 'isValue': '1', 'Text': 'Nr. Street' },
        'tag16': { 'isValue': '1', 'Text': 'Country' },
        'tag17': { 'isValue': '1', 'Text': 'Arrival date' },
        'tag18': { 'isValue': '1', 'Text': 'Departure date' },
        'tag19': { 'isValue': '1', 'Text': 'Scope' },
        'tag20': { 'isValue': '1', 'Text': 'Identity document' },
        'tag21': { 'isValue': '1', 'Text': 'Seria' },
        'tag22': { 'isValue': '1', 'Text': 'Series' },
        'tag23': { 'isValue': '1', 'Text': 'Previous' },
        'tag24': { 'isValue': '1', 'Text': 'Next' },
        'tag25': { 'isValue': '0', 'Text': 'All records were completed' },
        'tag26': { 'isValue': '1', 'Text': 'Finish' },
        'tag27': { 'isValue': '0', 'Text': 'Check-in done successfully' },
        'btnPrinteazaID': { 'isValue': '0', 'Text': 'Print files' },
        'btnPlataID': { 'isValue': '0', 'Text': 'Pay ' },
        'tcTag': { 'isValue': '0', 'Text': 'Terms & Conditions ' },
        
    },
    'RO': {
        'tag0': { 'isValue': '0', 'Text': '  Completati fisele de anuntare a sosirii' },
        'tag1': { 'isValue': '0', 'Text': 'Limba' },
        'tag2': { 'isValue': '0', 'Text': 'Acasa' },
        'tag3': { 'isValue': '0', 'Text': 'Rezervare' },
        'tag4': { 'isValue': '0', 'Text': 'Completati codul rezervarii' },
        'tag5': { 'isValue': '0', 'Text': 'Codul rezervarii' },
        'prevBtn': { 'isValue': '0', 'Text': 'Inapoi' },
        'nextBtn': { 'isValue': '0', 'Text': 'Inainte' },
        'tag6': { 'isValue': '0', 'Text': 'Camera' },
        'tag7': { 'isValue': '0', 'Text': 'Nr. turisti' },
        'tag8': { 'isValue': '0', 'Text': 'Servicii' },
        'tag9': { 'isValue': '1', 'Text': 'Nume Prenume' },
        'tag10': { 'isValue': '1', 'Text': 'Data nasterii' },
        'tag11': { 'isValue': '1', 'Text': 'Locul nasterii' },
        'tag12': { 'isValue': '1', 'Text': 'Cetatenie' },
        'tag13': { 'isValue': '1', 'Text': 'Localitatea' },
        'tag14': { 'isValue': '1', 'Text': 'Strada' },
        'tag15': { 'isValue': '1', 'Text': 'Nr. Strada' },
        'tag16': { 'isValue': '1', 'Text': 'Tara' },
        'tag17': { 'isValue': '1', 'Text': 'Data Sosirii' },
        'tag18': { 'isValue': '1', 'Text': 'Data Plecarii' },
        'tag19': { 'isValue': '1', 'Text': 'Scop' },
        'tag20': { 'isValue': '1', 'Text': 'Act Identitate' },
        'tag21': { 'isValue': '1', 'Text': 'Seria' },
        'tag22': { 'isValue': '1', 'Text': 'Numar' },
        'tag23': { 'isValue': '1', 'Text': 'Precedent' },
        'tag24': { 'isValue': '1', 'Text': 'Urmatorul' },
        'tag25': { 'isValue': '0', 'Text': 'Toate fisele au fost completate' },
        'tag26': { 'isValue': '1', 'Text': 'Gata' },
        'tag27': { 'isValue': '0', 'Text': 'Check-in realizat cu success ' },
        'btnPrinteazaID': { 'isValue': '0', 'Text': 'Printeaza fise' },
        'btnPlataID': { 'isValue': '0', 'Text': 'Plata' },
        'tcTag': { 'isValue': '0', 'Text': 'Termeni si Conditii' },
    }
};
