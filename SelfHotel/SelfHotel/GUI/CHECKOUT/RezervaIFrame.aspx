<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RezervaIFrame.aspx.cs" Inherits="SelfHotel.GUI.CHECKOUT.RezervaIFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rezerva</title>
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.1.0.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui.js") %>"></script>
    <link href="../CHECKIN/nav/css3-responsive-menu/style/reset.css" rel="stylesheet" type="text/css" />
    <link href="../CHECKIN/nav/css3-responsive-menu/style/example.css" rel="stylesheet" type="text/css" />
    <script src="../CHECKIN/nav/css3-responsive-menu/style/script/respond.js"></script>
    <script>
        jQuery(function ($) {
            var open = false;

            function resizeMenu() {
                if ($(this).width() < 768) {
                    if (!open) {
                        $("#menu-drink").hide();
                    }
                    $("#menu-button").show();
                }
                else if ($(this).width() >= 768) {
                    if (!open) {
                        $("#menu-drink").show();
                    }
                    $("#menu-button").hide();
                }
            }

            function setupMenuButton() {
                $("#menu-button").click(function (e) {
                    e.preventDefault();

                    if (open) {
                        $("#menu-drink").fadeOut();
                        $("#menu-button").toggleClass("selected");
                    }
                    else {
                        $("#menu-drink").fadeIn();
                        $("#menu-button").toggleClass("selected");
                    }
                    open = !open;
                });
            }

            $(window).resize(resizeMenu);

            resizeMenu();
            setupMenuButton();
        });
    </script>
    <style>
        body {
            /*background-image: url(../../Icoane/bM2.jpg);
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;*/
            /*background-color: #00000070;*/
              background: #4b6cb7; 
              background: -webkit-linear-gradient(to right, #4b6cb7, #182848);
              background: linear-gradient(to right, #4b6cb7, #182848);
        }
         .resp-container {
                position: relative;
                overflow: hidden;
                padding-top: 56.25%;
                padding-top: 100%;
                height: -webkit-fill-available;
                border-radius: 10px;
                margin: 15px;
            }
        .resp-iframe {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            border: 0;
        }
         body::-webkit-scrollbar-track, .resp-iframe::-webkit-scrollbar-track, .resp-container::-webkit-scrollbar-track
            {
	            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	            background-color: #F5F5F5;
            }

            body::-webkit-scrollbar, .resp-iframe::-webkit-scrollbar, .resp-container::-webkit-scrollbar
            {
	            width: 10px;
	            background-color: #F5F5F5;
            }

            body::-webkit-scrollbar-thumb, .resp-iframe::-webkit-scrollbar-thumb, .resp-container::-webkit-scrollbar-thumb
            {
	            background-color: #0ae;
	
	            background-image: -webkit-gradient(linear, 0 0, 0 100%,
	                               color-stop(.5, rgba(255, 255, 255, .2)),
					               color-stop(.5, transparent), to(transparent));
            }
            #ClientTopHeader {
                display: none !important;
            }

            #ClientTopHeader iframe
            {
                display: none !important;
            }
    </style>
</head>
<body>
    <div style="position: absolute;float: right;right: 0;color: #f8f9fa;z-index: 999999999;">
            <span id="tag1" >Limba:</span>
            <span onclick="toRomanian()" ><img src="../../Icoane/romania.png" style="cursor: pointer;border-radius: 50%;width:35px;height:25px;" /></span>
            <span onclick="toEnglish()" ><img src="../../Icoane/english.png" style="cursor: pointer;border-radius: 50%;width:35px;height:25px;" /></span>
        </div>
    <div id="banner-wrapper">
            <header id="banner" role="banner">
                <div id="banner-inner-wrapper" >
                    <div id="banner-inner">
                        <nav id="menu-nav">
                            <div id="menu-button">
                                <div id="menu-button-inner"></div>
                            </div>
                        </nav>
                    </div>
                </div>

                <nav id="menu-drink" style="z-index: 999999;">
                    <ul>
                        <li>
                            <a class="meniu beer" href="../../../Home.aspx"><span id="HomeID" class="icon"></span><span id="tag2">Acasa</span></a>
                        </li>
                        <li>
                            <a class="meniu beer" href="../CHECKIN/CheckIn2.aspx"><span id="CheckInID" class="icon"></span>Check in</a>
                        </li>
                        <li>
                            <a class="meniu wine" href="RezervaIFrame.aspx" ><span id="RezervaID" class="icon"></span><span id="tag3">Rezervare</span></a>
                        </li>
                        <li>
                            <a class="meniu soft-drink" href="PlataForm2.aspx"><span id="CheckOutID" class="icon"></span>Check out</a>
                        </li>
                    </ul>
                </nav>
            </header>
    </div>
     
    <form id="form1" runat="server"> 
        <div class="resp-container">
            <iframe id="IFrameId" class="resp-iframe" src="http://192.168.127.81/Solon.OB/Booking/?Hotel=1SE7T4&idFirma=1" gesture="media"  allow="encrypted-media" allowfullscreen></iframe>
        </div>
    </form>

    <script type="text/javascript">
        var defaultLang = "RO";
        $(function () {
            $('.meniu').each(function () {
                if ($(this).prop('href') == window.location.href) {
                    $(this).addClass('activa');
                }
            });
        });

        function goToAlin() {
            //$.ajax({
            //    url: 'Home.aspx/goToAlin',
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

        
        function toRomanian() {
            defaultLang = "RO";
            setCookie("lang", defaultLang, 1);
            changeLanguage();
            location.reload();
        }
        function toEnglish() {
            defaultLang = "EN";
            setCookie("lang", defaultLang, 1);
            changeLanguage();
            location.reload();s
        }
        var dicLab = {
            'EN': {
                'tag1': { 'isValue': '0', 'Text': 'Language' },
                'tag2': { 'isValue': '0', 'Text': 'Home' },
                'tag3': { 'isValue': '0', 'Text': 'Reservation' },
            },
            'RO': {
                'tag1': { 'isValue': '0', 'Text': 'Limba' },
                'tag2': { 'isValue': '0', 'Text': 'Acasa' },
                'tag3': { 'isValue': '0', 'Text': 'Rezervare' },
            }
        };
        $(document).ready(function () {
            try {
                checkCookie();
                changeLanguage();
            } catch (err) { }
        });
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

     </script>

</body>
</html>
