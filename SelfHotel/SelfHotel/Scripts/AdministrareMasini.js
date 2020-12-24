var tabelMasini = '';
$(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
            var actions = $("table td:last-child").html();
            //adauga new
            $(".add-new").click(function () {
                $(this).attr("disabled", "disabled");
                var index = $("table tbody tr:last-child").index();
                var row = '<tr>' +
                    '<td class="col-xs-3"><input type="text" maxlength="99" class="form-control" name="name" id="marcaId"></td>' +
                    '<td class="col-xs-3"><input type="text" maxlength="99" class="form-control" name="department" id="modelId"></td>' +
                    '<td class="col-xs-4"><input type="text" maxlength="99" class="form-control" name="phone" id="NumarId"></td>' +
                    '<td class="col-xs-2">' + actions + '</td>' +
                '</tr>';
                //$("table").append(row);
                $('table').prepend(row);
                //$("table tbody tr").eq(index + 1).find(".add, .edit").toggle();
                $("table tbody tr").eq(0).find(".add, .edit").toggle();
                $('[data-toggle="tooltip"]').tooltip();
            });
           //salveaza
            $(document).on("click", ".add", function () {
                var empty = false;
                var input = $(this).parents("tr").find('input[type="text"]');
                input.each(function () {
                    if (!$(this).val()) {
                        $(this).addClass("error");
                        empty = true;
                    } else {
                        $(this).removeClass("error");
                    }
                });
                $(this).parents("tr").find(".error").first().focus();
                if (!empty) {
                    var numar = document.getElementById("NumarId");
                    if (numar) {//adaug new
                        //functia adauga masina, if ok executa next
                        var adaugat = adaugaMasina();
                        if (adaugat) {
                            input.each(function () {
                                $(this).parent("td").html($(this).val());
                            });
                            $(this).parents("tr").find(".add, .edit").toggle();
                            $(".add-new").removeAttr("disabled");
                            $.notify({ title: '<strong>Success </strong>', icon: 'glyphicon glyphicon-star', message: "Masina a fost adaugata" }, {
                                type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                                placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                            });
                        } else {
                            $.notify({ title: '<strong>Error </strong>', icon: 'glyphicon glyphicon-star', message: "Nu s-a adaugat masina" }, {
                                type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                                placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                            });
                        }
                    } else {
                        var idMasina = $(this).attr("data-idmasina");
                        var modifica = modificaMasina(idMasina);
                        if (modifica) {
                            input.each(function () {
                                $(this).parent("td").html($(this).val());
                            });
                            $(this).parents("tr").find(".add, .edit").toggle();
                            $(".add-new").removeAttr("disabled");
                            $.notify({ title: '<strong>Success </strong>', icon: 'glyphicon glyphicon-star', message: "Masina a fost modificata" }, {
                                type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                                placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                            });
                        } else {
                            $.notify({ title: '<strong>Error </strong>', icon: 'glyphicon glyphicon-star', message: "Nu s-a modificat masina" }, {
                                type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                                placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                            });
                        }
                    }
                }
            });
            //modifica
            $(document).on("click", ".edit", function () {
                $(this).parents("tr").find("td:not(:last-child)").each(function () {
                    $(this).html('<input type="text" class="form-control" value="' + $(this).text() + '">');
                });
                $(this).parents("tr").find(".add, .edit").toggle();
                $(".add-new").attr("disabled", "disabled");
            });
            //sterge
            $(document).on("click", ".delete", function () {
                var idMasina = $(this).attr("data-idmasina");
                var sters = stergeMasina(idMasina);
                if (sters) {
                    $(this).parents("tr").remove();
                    $(".add-new").removeAttr("disabled");
                    $.notify({ title: '<strong>Success </strong>', icon: 'glyphicon glyphicon-star', message: "Masina a fost stearsa" }, {
                        type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                        placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                    });
                } else {
                    $(".add-new").removeAttr("disabled");
                    $.notify({ title: '<strong>Error </strong>', icon: 'glyphicon glyphicon-star', message: "Nu s-a sters masina" }, {
                        type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                        placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                    });
                }
            });

            $(".search").keyup(function () {
                var searchTerm = $(".search").val();
                var listItem = $('.results tbody').children('tr');
                var searchSplit = searchTerm.replace(/ /g, "'):containsi('")

                $.extend($.expr[':'], {
                    'containsi': function (elem, i, match, array) {
                        return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                    }
                });

                $(".results tbody tr").not(":containsi('" + searchSplit + "')").each(function (e) {
                    //$(this).attr('visible', 'false');
                    $(this).css("display", "none");
                });

                $(".results tbody tr:containsi('" + searchSplit + "')").each(function (e) {
                    //$(this).attr('visible', 'true');
                    $(this).css("display", "");

                });

                var jobCount = $('.results tbody tr:visible').length; //$('.results tbody tr[visible="true"]').length;
                $('.counter').text(jobCount + ' masini');

               
            });

            $('#addRezolutiebtn').click(function () {
                var newitem = $('#addRezolutieId').val();
                var addRezOk = adaugaRezolutie(newitem);
                if (addRezOk > 0) {
                    $('#list').append('<li id="' + addRezOk + '" class="list-group-item"><input type="button" data-id="' + addRezOk + '" class="listelement" value="X" /> ' + newitem + '<input type="hidden" name="listed[]" value="' + newitem + '"></li>');
                    $('#addRezolutieId').val('');
                    $.notify({ title: '<strong>Success </strong>', icon: 'glyphicon glyphicon-star', message: "Rezolutia a fost adaugata" }, {
                        type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                        placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                    });
                    incarcaListaRezolutie();
                } else {
                    if (addRezOk < 0) {
                        $.notify({ title: '<strong>Rezolutia nu a fost adaugata </strong>', icon: 'glyphicon glyphicon-star', message: "Exista aceasta rezolutie" }, {
                            type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                            placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                        });
                    } else {
                        $.notify({ title: '<strong>Rezolutia nu a fost adaugata </strong>', icon: 'glyphicon glyphicon-star', message: "" }, {
                            type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                            placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                        });
                    }
                }
                
                return false;
            });
            $('#list').delegate(".listelement", "click", function () {
                var elemid = $(this).attr('data-id');
                var sters = stergeRezolutie(elemid,false);
                if (sters) {
                    $("#" + elemid).remove();
                    $.notify({ title: '<strong>Success </strong>', icon: 'glyphicon glyphicon-star', message: "Rezolutia a fost stearsa" }, {
                        type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                        placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                    });
                } else {
                    $.notify({ title: '<strong>Nu se poate sterge </strong>', icon: 'glyphicon glyphicon-star', message: "Rezolutia este referita in interventii" }, {
                        type: 'info', animate: { enter: 'animated fadeInUp', exit: 'animated fadeOutRight' },
                        placement: { from: "top", align: "right" }, offset: 40, spacing: 20, z_index: 1031,
                    });
                }
            });

            
});
function adaugaRezolutie(item) {
    var rv = 0;
    if (item) {
        $.ajax({
            url: '../ModificaParola.aspx/adaugaRezolutie',
            data: "{tip:'" + item + "'}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                rv = ok.d;
            },
            error: function () {
                rv = false;
                return null;
            }
        });
    }
    return rv;
}
    function adaugaMasina() {
        var marca = document.getElementById("marcaId");
        var model = document.getElementById("modelId");
        var numar = document.getElementById("NumarId");
        var rv = false;
        if (marca && model && numar) {
            //adauga masina
            $.ajax({
                url: '../ModificaParola.aspx/adaugaMasina',
                data: "{marca:'" + marca.value + "',model:'" + model.value + "',numar:'" + numar.value + "'}",
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (ok) {
                    if (ok.d > 0) {
                        rv = true;
                    } else {
                        rv = false;
                    }
                },
                error: function () {
                    rv = false;
                    return null;
                }
            });
        }
        return rv;
    }
    function stergeMasina(idMasina) {
        var rv = false;
        if (idMasina) {
            $.ajax({
                url: '../ModificaParola.aspx/stergeMasina',
                data: "{ID:'" + idMasina + "'}",
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (ok) {
                    if (ok.d > 0) {
                        rv = true;
                    } else {
                        rv = false;
                    }
                },
                error: function () {
                    rv = false;
                    return null;
                }
            });
            return rv;
        }
    }
    function modificaMasina(idMasina) {
        var marca = document.getElementById("td0" + idMasina).firstElementChild.value;
        var model = document.getElementById("td1" + idMasina).firstElementChild.value;
        var numar = document.getElementById("td2" + idMasina).firstElementChild.value;
        var rv = false;
        if (marca && model && numar) {
            $.ajax({
                url: '../ModificaParola.aspx/modificaMasina',
                data: "{ID:'" + idMasina + "',marca:'" + marca + "',model:'" + model + "',numar:'" + numar + "'}",
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (ok) {
                    if (ok.d > 0) {
                        rv = true;
                    } else {
                        rv = false;
                    }
                },
                error: function () {
                    rv = false;
                    return null;
                }
            });
        }
        return rv;
    }
//incarca lista masini
function incarcaListaMasini() {
        $.ajax({
            url: '../ModificaParola.aspx/getListaMasini',
            data: "{}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d && ok.d.length > 0) {
                    var table = document.getElementById("tabelMasiniBody");
                    table.innerHTML = "";
                    var i;
                    var n = ok.d.length;
                    
                    for (i = 0;i<n;i++){
                        var rand = document.createElement("tr");
                        rand.setAttribute("id", ok.d[i].ID);
                        //rand.setAttribute("data-sortable", "true");

                        var marca = document.createElement("td");
                        marca.setAttribute("class", "col-xs-3");
                        marca.innerHTML = ok.d[i].Marca;
                        marca.setAttribute("id", "td0"+ok.d[i].ID);
                        rand.appendChild(marca);

                        var model = document.createElement("td");
                        model.setAttribute("class", "col-xs-3");
                        model.innerHTML = ok.d[i].Model;
                        model.setAttribute("id", "td1" + ok.d[i].ID);
                        rand.appendChild(model);

                        var nr = document.createElement("td");
                        nr.setAttribute("class", "col-xs-4");
                        nr.innerHTML = ok.d[i].NrImatriculare;
                        nr.setAttribute("id", "td2" + ok.d[i].ID);
                        rand.appendChild(nr);
                        
                        var actiuni = document.createElement("td");
                        actiuni.setAttribute("class", "col-xs-2");
                        var a1 = document.createElement("a");
                        a1.setAttribute("class", "add");
                        a1.setAttribute("title", "Salveaza");
                        a1.setAttribute("data-toggle", "tooltip");
                        a1.setAttribute("data-idmasina", ok.d[i].ID);
                        var i1 = document.createElement("i");
                        i1.setAttribute("class", "material-icons");
                        i1.innerHTML = "&#xE03B";
                        a1.appendChild(i1);
                        actiuni.appendChild(a1);
                        var a2 = document.createElement("a");
                        a2.setAttribute("class", "edit");
                        a2.setAttribute("title", "Modifica");
                        a2.setAttribute("data-toggle", "tooltip");
                        a2.setAttribute("data-idmasina", ok.d[i].ID);
                        var i2 = document.createElement("i");
                        i2.setAttribute("class", "material-icons");
                        i2.innerHTML = "&#xE254";
                        a2.appendChild(i2);
                        actiuni.appendChild(a2);
                        var a3 = document.createElement("a");
                        a3.setAttribute("class", "delete");
                        a3.setAttribute("title", "Sterge");
                        a3.setAttribute("data-toggle", "tooltip");
                        a3.setAttribute("data-idmasina", ok.d[i].ID);
                        var i3 = document.createElement("i");
                        i3.setAttribute("class", "material-icons");
                        i3.innerHTML = "&#xE872";
                        a3.appendChild(i3);
                        actiuni.appendChild(a3);
                        rand.appendChild(actiuni);

                        table.appendChild(rand);
                    }
                } else {
                    location.reload();//cand nu are drepturi vizualizare pentru masini
                }
            },
            error: function () {
                return null;
            }
        });
 }
//
function Car() {
    incarcaListaMasini();
}

function incarcaListaRezolutie() {
    $.ajax({
        url: '../ModificaParola.aspx/incarcaListaRezolutie',
        data: "{}",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (ok) {
            if (ok.d && ok.d.length > 0) {
                var l = document.getElementById("list");
                l.innerHTML = "";
                var i;
                var n = ok.d.length;
                for (i = 0; i < n;i++){
                    $('#list').append('<li id="' + ok.d[i].ID + '" class="list-group-item"><input type="button" data-id="' + ok.d[i].ID + '" class="listelement" value="X" /> ' + ok.d[i].Tip + '<input type="hidden" name="listed[]" value="' + ok.d[i].Tip + '"></li>');
                }
                document.getElementById("spnCountElemente").innerHTML = n + " rezolutii";
            }
        },
        error: function () {
            return null;
        }
    });
}
function Interventions() {
    incarcaListaRezolutie();
}
function stergeRezolutie(id,deTot) {
        $.ajax({
            url: '../ModificaParola.aspx/StergeRezolutie',
            data: "{ID:'" + id + "',deTot:" + deTot + "}",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (ok) {
                if (ok.d > 0) {
                    rv = true;
                } else {
                    rv = false;
                }
            },
            error: function () {
                rv = false;
                return null;
            }
        });
    return rv;
}

