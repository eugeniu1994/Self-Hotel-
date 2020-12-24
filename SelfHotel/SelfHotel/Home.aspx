<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SelfHotel.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Main</title>
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.1.0.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui.js") %>"></script>
    <link href="Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/startbootstrap-agency-gh-pages/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/googleApisCSS/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href='Scripts/googleApisCSS/StyleSheet2.css' rel='stylesheet' type='text/css' />
    <link href='Scripts/googleApisCSS/StyleSheet3.css' rel='stylesheet' type='text/css' />
    <link href='Scripts/googleApisCSS/StyleSheet4.css' rel='stylesheet' type='text/css' />
    <link href="Scripts/startbootstrap-agency-gh-pages/css/agency.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="Style/Home.css" />
    <style>
         body::-webkit-scrollbar-track,table::-webkit-scrollbar-track
        {
	        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	        background-color: #F5F5F5;
        }

        body::-webkit-scrollbar, table::-webkit-scrollbar
        {
	        width: 10px;
	        background-color: #F5F5F5;
        }

        body::-webkit-scrollbar-thumb, table::-webkit-scrollbar-thumb
        {
	        background-color: #0ae;
	
	        background-image: -webkit-gradient(linear, 0 0, 0 100%,
	                           color-stop(.5, rgba(255, 255, 255, .2)),
					           color-stop(.5, transparent), to(transparent));
        }
        .img-fluid {
            max-width: 100%;
            max-height: 100%;
            width: 300px;
            height: 250px;
            width: 100%;
        }
        .portfolio-modal .modal-content img {
            margin-bottom: 30px;
            height: 400px;
        }
        @media only screen and (max-width: 579px) {
            .img-fluid {
                max-width: 100%;
                width: 100%;
                max-height: 100%;
                height: 226px;        
            }
            .portfolio-modal .modal-content img {
                margin-bottom: 30px;
                height: 200px;
            }

            .centered {
              position: absolute;
              top: 50%;
              left: 50%;
              transform: translate(-50%, -50%);
            }
            .box1{
                position: relative;
                display: inline-block; /* Make the width of box same as image */
            }
             .text{
                position: absolute;
                z-index: 999;
                margin: 0 auto;
                left: 0;
                right: 0;        
                text-align: center;
                top: 40%; 
                background: rgba(0, 0, 0, 0.8);
                font-family: Arial,sans-serif;
                color: #fff;
                width: 60%; 
            }
        }
        .cssData {
            /*margin: auto;
            position: absolute;*/
            width: 150px;
            height: 50px;
            color: #ffffff;
            border: 2px solid #999;
            border-radius: 4px;
            text-align: center;
            font: 26px/50px 'DIGITAL', Helvetica;
            background: linear-gradient(90deg, #000, #555);
        }
    </style>
    <link rel="stylesheet" type="text/css" href="Style/Log.css" />
    <script src="Scripts/jquery-3.1.0.js" type="text/javascript" ></script>
    <script src="Scripts/jquery.translate.js" type="text/javascript" ></script>
</head>
<body>
    <div id ="meniu" style="background-color:white;display:inline-block;width:100%;height:65px;font-size:20px;display:none;" >
            <img style="height:60px;width:auto;" src="../../Icoane/Sigla%20MSF.png" />
            <nav>
                <a href="#" id="menu-icon"></a>
                <ul id="ulist" style="margin-block-start: 17px;">
                    <li>
                        <a id="A1"  href="Home.aspx"  runat="server" >Acasa</a>
                    </li>
                    <li>
                        <a id="A2"  href="./GUI/CHECKIN/CheckIn2.aspx"   runat="server" >Check in</a>
                    </li>
                    <li>
                        <a id="A3" href="#" onclick='goToAlin()'   runat="server" >Rezerva</a>
                    </li>
                    <li>
                        <a id="A4"  href="./GUI/CHECKOUT/PlataForm2.aspx"   runat="server" >Check Out</a>
                    </li>
                    <li id="IDlistaUtilizatori">
                        <a id="A5" href="#portfolio"  runat="server" >Servicii</a>
                    </li>
                </ul>
            </nav>
    </div>

    <form id="form1" runat="server"  >

        <div style="background-image:url('Style/hom.jpg');background-repeat:no-repeat;background-size:cover;background-position:center;height:100%;width:100%;display:inline-block;">
            <div class="mainControlPanel" style="display:table;margin-top:230px;border:thin solid black;background-color: rgba(255, 255, 255, 0.4);width: auto;height:auto;margin-left:auto;margin-right:auto;padding:10px;max-width:90%;">
               
                <div style="position: absolute;color: #f8f9fa;left: 50%;top: 157px;">
                        <span onclick="toRomanian()" style="margin-left: -49%;position: relative;display: inline-block;" ><img src="Icoane/romania.png" style="border-radius: 50%;width:100px;height:65px;" /><span onclick="toRomanian()" style="cursor:pointer; position: absolute; z-index: 999;margin: 0 auto;left: 0;right: 0;text-align: center;top: 29%;">Romana</span></span>
                        <span onclick="toEnglish()" style="position: relative;display: inline-block;" ><img src="Icoane/english.png" style="border-radius: 50%;width:100px;height:65px;" /><span onclick="toEnglish()" style="cursor:pointer;position: absolute; z-index: 999;margin: 0 auto;left: 0;right: 0;text-align: center;top: 29%;">English</span></span>
                </div>
                <div class="container">
                   
                <div class="row">
                  <div class=" col-sm-4" title="Incepe cazarea" onclick="window.location='GUI/CHECKIN/CheckIn2.aspx'" >
                    <div class="team-member">
                      <img class="mx-auto rounded-circle" src="Icoane/256_CheckIn_blue.png" onmouseout="this.src='Icoane/256_CheckIn_blue.png'" onmouseover="this.src='Icoane/256_CheckIn_green.png'" alt=""/> 
                        <h4 class="trn">Check in</h4>
                    </div>
                  </div>

                  <div class="col-sm-4" title="Creaza o rezervare noua" onclick='goToAlin()'  >
                    <div class="team-member">
                      <img class="mx-auto rounded-circle" src="Icoane/256_Reservation_blue.png" onmouseout="this.src='Icoane/256_Reservation_blue.png'" onmouseover="this.src='Icoane/256_Reservation_green.png'" alt="" />
                        <h4 class="trn">Rezerva acum</h4>
                    </div>
                  </div>

                  <div class="col-sm-4" title="Termina cazarea" onclick="window.location='GUI/CHECKOUT/PlataForm2.aspx'"  >
                    <div class="team-member">
                       <img class="mx-auto rounded-circle" src="Icoane/256_CheckOut_blue.png" onmouseout="this.src='Icoane/256_CheckOut_blue.png'" onmouseover="this.src='Icoane/256_CheckOut_green.png'" alt="" />
                        <h4 class="trn">Check out</h4>
                    </div>
                  </div>
                </div>
              </div>

            </div>
        </div>
        <div style="position: absolute;right: 20px;top: 1px;">
              <canvas id="canvas" width="150" height="150" style="border-radius:50%" style="background-color:#333"> </canvas>
             <div id="campOraDataID" class="cssData">2019:12:12</div>
        </div>

    <section class="bg-light" id="portfolio">
      <div class="container">
        <div class="row">
          <div class="col-lg-12 text-center">
            <h2 class="section-heading text-uppercase trn" >Camere - Servicii</h2>
            <h3 class="section-subheading text-muted trn">Camerele si Serviciile noastre !!!</h3>
          </div>
        </div>

        <div class="row" id="divSectionID">

              <div class="col-md-4 col-sm-6 portfolio-item">
                <a class="portfolio-link" data-toggle="modal" href="#portfolioModal1">
                  <div class="portfolio-hover">
                    <div class="portfolio-hover-content">
                      <i class="fas fa-plus fa-3x"></i>
                    </div>
                  </div>
                  <img class="img-fluid" src="img/portfolio/01-thumbnail.jpg" alt="" />
                </a>
                <div class="portfolio-caption">
                  <h4>Luxury</h4>
                  <p class="text-muted">room</p>
                </div>
              </div>

              <div class="col-md-4 col-sm-6 portfolio-item">
                <a class="portfolio-link" data-toggle="modal" href="#portfolioModal2">
                  <div class="portfolio-hover">
                    <div class="portfolio-hover-content">
                      <i class="fas fa-plus fa-3x"></i>
                    </div>
                  </div>
                  <img class="img-fluid" src="img/portfolio/02-thumbnail.jpg" alt="" />
                </a>
                <div class="portfolio-caption">
                  <h4>Apartament</h4>
                  <p class="text-muted">room</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6 portfolio-item">
                <a class="portfolio-link" data-toggle="modal" href="#portfolioModal3">
                  <div class="portfolio-hover">
                    <div class="portfolio-hover-content">
                      <i class="fas fa-plus fa-3x"></i>
                    </div>
                  </div>
                  <img class="img-fluid" src="img/portfolio/03-thumbnail.jpg" alt="" />
                </a>
                <div class="portfolio-caption">
                  <h4>Double</h4>
                  <p class="text-muted">room</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6 portfolio-item">
                <a class="portfolio-link" data-toggle="modal" href="#portfolioModal4">
                  <div class="portfolio-hover">
                    <div class="portfolio-hover-content">
                      <i class="fas fa-plus fa-3x"></i>
                    </div>
                  </div>
                  <img class="img-fluid" src="img/portfolio/04-thumbnail.jpg" alt="" />
                </a>
                <div class="portfolio-caption">
                  <h4>Single</h4>
                  <p class="text-muted">room</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6 portfolio-item">
                <a class="portfolio-link" data-toggle="modal" href="#portfolioModal5">
                  <div class="portfolio-hover">
                    <div class="portfolio-hover-content">
                      <i class="fas fa-plus fa-3x"></i>
                    </div>
                  </div>
                  <img class="img-fluid" src="img/portfolio/05-thumbnail.jpg" alt="" />
                </a>
                <div class="portfolio-caption">
                  <h4>Restaurant</h4>
                  <p class="text-muted">masa</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6 portfolio-item">
            <a class="portfolio-link" data-toggle="modal" href="#portfolioModal6">
              <div class="portfolio-hover">
                <div class="portfolio-hover-content">
                  <i class="fas fa-plus fa-3x"></i>
                </div>
              </div>
              <img class="img-fluid" src="img/portfolio/06-thumbnail.jpg" alt="" />
            </a>
            <div class="portfolio-caption">
              <h4>Gym</h4>
              <p class="text-muted">sala de forta</p>
            </div>
          </div>
        </div>
      </div>
    </section>


    <div id="divModalsID">   
        <!-- Modal 1 -->
        <div class="portfolio-modal modal fade" id="portfolioModal1" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Camera lux</h2>
                      <p class="item-intro text-muted"></p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/01-full.jpg" alt="" />
                      <p>Descriere: Aceasta este o camera lux etc.</p>
                      <ul class="list-inline">
                        <li>Data: Ieri</li>
                        <li>Pret xxx</li>
                        <li>Capacitate de cazare: 4 persoane</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal 2 -->
        <div class="portfolio-modal modal fade" id="portfolioModal2" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Apartamente</h2>
                      <p class="item-intro text-muted">room </p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/02-full.jpg" alt="" />
                      <p>Acesta este un apartament din hotelul nostru, </p>
                      <ul class="list-inline">
                        <li>Date: azi</li>
                        <li>pret</li>
                        <li>Capacitate de cazare: 1-5 persoane</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal 3 -->
        <div class="portfolio-modal modal fade" id="portfolioModal3" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Camera dubla</h2>
                      <p class="item-intro text-muted">room</p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/03-full.jpg" alt="" />
                      <p>Aceasta este o camera dubla </p>
                      <ul class="list-inline">
                        <li>Cate sunt disponibil</li>
                        <li>Pret</li>
                        <li>Capacitate de cazare: 1-2 persoane</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal 4 -->
        <div class="portfolio-modal modal fade" id="portfolioModal4" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Camera single</h2>
                      <p class="item-intro text-muted">Pentru oameni singuri</p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/04-full.jpg" alt="" />
                      <p>Aceasta este o camera single</p>
                      <ul class="list-inline">
                        <li>Data</li>
                        <li>Pret</li>
                        <li>Capacitate 1 persoana</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal 5 -->
        <div class="portfolio-modal modal fade" id="portfolioModal5" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Restaurant</h2>
                      <p class="item-intro text-muted">Exemplu restaurant</p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/05-full.jpg" alt="" />
                      <p>Serviti masa la noi</p>
                      <ul class="list-inline">
                        <li>Data</li>
                        <li>Capacitate 150 persoane</li>
                        <li>Disponibil pana la ora 23:00</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Modal 6 -->
        <div class="portfolio-modal modal fade" id="portfolioModal6" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="close-modal" data-dismiss="modal">
                <div class="lr">
                  <div class="rl"></div>
                </div>
              </div>
              <div class="container">
                <div class="row">
                  <div class="col-lg-8 mx-auto">
                    <div class="modal-body">
                      <h2 class="text-uppercase">Gym</h2>
                      <p class="item-intro text-muted"></p>
                      <img class="img-fluid d-block mx-auto" src="img/portfolio/06-full.jpg" alt="" />
                      <p> Exemplu de hotel care are si sala de forta</p>
                      <ul class="list-inline">
                        <li>Data</li>
                        <li>Disponibil full time</li>
                        <li>Capacitate 50 persoane</li>
                      </ul>
                      <button class="btn btn-primary" data-dismiss="modal" type="button">
                        <i class="fas fa-times"></i>
                        Inchide</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

   </div>  
    <!-- Footer -->
    <footer>
        <%--<input class="btn-danger" type="button" onclick="test()" />--%>
      <div class="container">
        <div class="row">
          <div class="col-md-3">
            <span class="copyright">Copyright &copy; Multisoft 2019</span>
            <p class="text-muted">SmartHotel versiune 1.0</p>
          </div>
          <div class="col-md-9">
                
            <ul class="list-inline social-buttons">
              <li class="list-inline-item">
                    <img onclick="goToLink(this);" id="H" class="fab imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.H.png" />
                    <img onclick="goToLink(this);" id="R" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.R.png" />
                    <img onclick="goToLink(this);" id="C" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.C.png" />
                    <img onclick="goToLink(this);" id="G" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.G.png" />
                    <img onclick="goToLink(this);" id="M" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.M.png" />
                    <img onclick="goToLink(this);" id="S" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.S.png" />
                    <img onclick="goToLink(this);" id="V" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.V.png" />
                    <img onclick="goToLink(this);" id="Alop" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.Alop_.png" />
                    <img onclick="goToLink(this);" id="BB" class="imgPublicitate" style="height:60px;width:auto;" src="Style/Solon.BB_.png" />
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>

    </form>

    <script src="Scripts/startbootstrap-agency-gh-pages/vendor/jquery/jquery.min.js"></script>
    <script src="Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="Scripts/startbootstrap-agency-gh-pages/vendor/jquery-easing/jquery.easing.min.js"></script>
    <script src="Scripts/startbootstrap-agency-gh-pages/js/jqBootstrapValidation.js"></script>
    <script src="Scripts/startbootstrap-agency-gh-pages/js/contact_me.js"></script>
    <script>
        function goToLink(input) {
            switch (input.id) {
                case 'Alop':
                    openInNewTab('http://www.multisoft.ro/programe/solon-alop/');
                    break;
                case 'BB':
                    openInNewTab('http://www.multisoft.ro/programe/solon-bb/');
                    break;
                case 'C':
                    openInNewTab('http://www.multisoft.ro/programe/solon-c/');
                    break;
                case 'G':
                    openInNewTab('http://www.multisoft.ro/programe/solon-g/');
                    break;
                case 'H':
                    openInNewTab('http://www.multisoft.ro/programe/solon-h/');
                    break;
                case 'M':
                    openInNewTab('http://www.multisoft.ro/programe/solon-m/');
                    break;
                case 'R':
                    openInNewTab('http://www.multisoft.ro/programe/solon-r/');
                    break;
                case 'S':
                    openInNewTab('http://www.multisoft.ro/programe/solon-s/');
                    break;
                case 'V':
                    openInNewTab('http://www.multisoft.ro/programe/solon-v/');
                    break;
                default:
                    break;
            }
        }
        function openInNewTab(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }
    </script>
    <script>
        function goToAlin() {
            
            window.location = "GUI/CHECKOUT/RezervaIFrame.aspx";
        }


        $(document).ready(function () {
            try{
                getDataLucru();
            } catch (err) { }
            test();
        });

        function getDataLucru() {
            $.ajax({
                url: 'Home.aspx/getDataLucru',
                data: "{}",
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (ok) {
                    if (ok.d) {
                        document.getElementById("campOraDataID").innerHTML = ok.d;
                    } else {
                        document.getElementById("campOraDataID").innerHTML = "";
                    }
                },
                error: function (msg) {
                    document.getElementById("campOraDataID").innerHTML = "";
                }
            });
        }

        function test() {
            try {
                $.ajax({
                    url: 'Home.aspx/getListaTipCamere',
                    data: "{}",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    async: false,
                    success: function (ok) {
                        var divModalsID = document.getElementById("divModalsID"); divModalsID.innerHTML = "";
                        var divSectionID = document.getElementById("divSectionID"); divSectionID.innerHTML = "";
                        var i = 0, n = ok.d.length;
                        for (i = 0; i < n; i++) {
                            var camera = document.createElement("div");
                            divSectionID.appendChild(camera);
                            camera.setAttribute("class", "col-md-4 col-sm-6 portfolio-item trn");
                            var a = document.createElement("a");
                            camera.appendChild(a);
                            a.setAttribute("class", "portfolio-link trn");
                            a.setAttribute("data-toggle", "modal");
                            a.setAttribute("href", "#portfolioModal" + i);
                            var portfolio_hover = document.createElement("div");
                            a.appendChild(portfolio_hover);
                            portfolio_hover.setAttribute("class", "portfolio-hover trn");
                            var portfolio_hover_content = document.createElement("div");
                            portfolio_hover.appendChild(portfolio_hover_content);
                            portfolio_hover_content.setAttribute("class", "portfolio-hover-content trn");
                            var iNode = document.createElement("i");
                            portfolio_hover_content.appendChild(iNode);
                            iNode.setAttribute("class", "fas fa-plus fa-3x trn");
                            var img = document.createElement("img");
                            a.appendChild(img);
                            img.setAttribute("class", "img-fluid trn");
                            img.setAttribute("alt", "Image");
                            var src="Nomenclatoare/Handler2.ashx?id=" + ok.d[i].ID;
                            img.src = src;
                            img.setAttribute("onerror", "this.onerror=null;this.src='Icoane/defaultROOM.jpg';");                            
                            var portfolio_caption = document.createElement("div");
                            camera.appendChild(portfolio_caption);
                            portfolio_caption.setAttribute("class", "portfolio-caption trn");
                            var h4 = document.createElement("h4");
                            portfolio_caption.appendChild(h4);
                            h4.innerHTML = ok.d[i].Denumire;
                            var p = document.createElement("p");
                            portfolio_caption.appendChild(p);
                            p.setAttribute("class","trn");
                            p.innerHTML = "Camera";

                            var roomModal = document.createElement("div");
                            divModalsID.appendChild(roomModal);
                            roomModal.setAttribute("class", "portfolio-modal modal fade trn");
                            roomModal.setAttribute("id", "portfolioModal" + i);
                            roomModal.setAttribute("tabindex", "-1");
                            roomModal.setAttribute("role", "dialog");
                            roomModal.setAttribute("aria-hidden", "true");
                            var modalDialog = document.createElement("div");
                            roomModal.appendChild(modalDialog);
                            modalDialog.setAttribute("class", "modal-dialog trn");
                            var modalContent = document.createElement("div");
                            modalDialog.appendChild(modalContent);
                            modalContent.setAttribute("class", "modal-content trn");
                            var close_modal = document.createElement("div");
                            close_modal.setAttribute("class", "close-modal trn");
                            close_modal.setAttribute("data-dismiss", "modal");
                            modalContent.appendChild(close_modal);
                            var lr = document.createElement("div");
                            close_modal.appendChild(lr);
                            lr.setAttribute("class", "lr trn");
                            var rl = document.createElement("div");
                            lr.appendChild(rl);
                            rl.setAttribute("class", "rl trn");
                            var container = document.createElement("div");
                            modalContent.appendChild(container);
                            container.setAttribute("class", "container trn");
                            var row = document.createElement("div");
                            container.appendChild(row);
                            row.setAttribute("class", "row trn");
                            var colAuto = document.createElement("div");
                            row.appendChild(colAuto);
                            colAuto.setAttribute("class", "col-lg-8 mx-auto trn");
                            var modalBody = document.createElement("div");
                            colAuto.appendChild(modalBody);
                            modalBody.setAttribute("class", "modal-body trn");
                            var h2 = document.createElement("h2");
                            modalBody.appendChild(h2);
                            h2.setAttribute("class", "text-uppercase");
                            h2.innerHTML = "Camera " + ok.d[i].Denumire;
                            var pModals = document.createElement("p");
                            modalBody.appendChild(pModals);
                            pModals.setAttribute("class", "item-intro text-muted trn");
                            var imgModals = document.createElement("img");
                            modalBody.appendChild(imgModals);
                            imgModals.setAttribute("class", "img-fluid d-block mx-auto trn");
                            imgModals.setAttribute("alt", "Image");
                            imgModals.src = src;
                            imgModals.setAttribute("onerror", "this.onerror=null;this.src='Icoane/defaultROOM.jpg';");
                            var pModal2 = document.createElement("p");
                            modalBody.appendChild(pModal2);
                            pModal2.innerHTML = "Aceasta este o camera " + ok.d[i].Denumire;
                            var list_inline = document.createElement("ul");
                            modalBody.appendChild(list_inline);
                            list_inline.setAttribute("class", "list-inline trn");
                            var li0 = document.createElement("li");
                            list_inline.appendChild(li0);
                            li0.innerHTML = "Nr Locuri: " + ok.d[i].NrPaturi;
                            var li1 = document.createElement("li");
                            list_inline.appendChild(li1);
                            li1.innerHTML = "Max Paturi suplimentare: " + ok.d[i].MaxPaturiSuplim;
                            var button = document.createElement("button");
                            modalBody.appendChild(button);
                            button.setAttribute("class", "btn btn-primary trn");
                            button.setAttribute("data-dismiss", "modal");
                            button.setAttribute("type", "button");
                            var iButon = document.createElement("i");
                            button.appendChild(iButon);
                            iButon.setAttribute("class", "fas fa-times trn");
                            button.innerHTML = button.innerHTML + " Inchide";
                        }

                    },
                    error: function () {
                        return null;
                    }
                });
            } catch (err) { }
        }

    </script>
    <script src="Scripts/jquery.translate.js" type="text/javascript" ></script>
    <script type="text/javascript">
        var defaultLanguage = "ro";
        function setCookie(cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toGMTString();
            document.cookie = cname + "=" + cvalue.toUpperCase() + ";" + expires + ";path=/";
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
                    return c.substring(name.length, c.length).toLocaleLowerCase();
                }
            }
            return "";
        }

        checkCookie();

        function checkCookie() {
            var limba = getCookie("lang");
            if (limba != "") {
                defaultLanguage = limba;
            } else {
                limba = defaultLanguage;
                if (limba != "" && limba != null) {
                    setCookie("lang", limba, 1);
                }
            }
        }

        var dict = {
            "Check in": {
                en: "Check in",
                ro: "Check in"
            },
            "Rezerva acum": {
                en: "Create rezervation",
                ro: "Rezerva acum"
            },
            "Check out": {
                en: "Check out",
                ro: "Check out"
            },
            "Limba": {
                en: "Language",
                ro: "Limba"
            },
            "Camere - Servicii": {
                en: "Rooms - Services",
                ro: "Camere - Servicii"
            },
            "Camerele si Serviciile noastre !!!": {
                en: "Our rooms and services !!!",
                ro: "Camerele si Serviciile noastre !!!"
            },
            "Nr Locuri": {
                en: "Nr places",
                ro: "Nr Locuri:"
            },
            "Max Paturi suplimentare": {
                en: "Entra beds",
                ro: "Max Paturi suplimentare: 1"
            },
            "Inchide": {
                en: "Close",
                ro: " Inchide"
            },
            " Camera": {
                en: "Room",
                ro: " Camera"
            }
        }
        var translator = $('body').translate({ lang: defaultLanguage, t: dict });
        
        function toRomanian() {
            defaultLanguage = "ro"
            setCookie("lang", defaultLanguage, 1);
            translator.lang(defaultLanguage);
            
        }
        function toEnglish() {
            defaultLanguage = "en";
            setCookie("lang", defaultLanguage, 1);
            translator.lang(defaultLanguage);
           
        }
        

        $(function () {
            $('#meniu a').each(function () {
                if ($(this).prop('href') == window.location.href) {
                    $(this).addClass('current');
                }
                else {
                    if ($(this).attr("id") != 'menu-icon') {
                        $(this).addClass('ButonMeniu');
                    }
                }
            });
        });
        
    </script>
    <script>
        var canvas = document.getElementById("canvas");
        var ctx = canvas.getContext("2d");
        var radius = canvas.height / 2;
        ctx.translate(radius, radius);
        radius = radius * 0.90
        setInterval(drawClock, 1000);

        function drawClock() {
            drawFace(ctx, radius);
            drawNumbers(ctx, radius);
            drawTime(ctx, radius);
        }

        function drawFace(ctx, radius) {
            var grad;
            ctx.beginPath();
            ctx.arc(0, 0, radius, 0, 2 * Math.PI);
            ctx.fillStyle = 'white';
            ctx.fill();
            grad = ctx.createRadialGradient(0, 0, radius * 0.95, 0, 0, radius * 1.05);
            grad.addColorStop(0, '#333');
            grad.addColorStop(0.5, 'white');
            grad.addColorStop(1, '#333');
            ctx.strokeStyle = grad;
            ctx.lineWidth = radius * 0.1;
            ctx.stroke();
            ctx.beginPath();
            ctx.arc(0, 0, radius * 0.1, 0, 2 * Math.PI);
            ctx.fillStyle = '#333';
            ctx.fill();
        }

        function drawNumbers(ctx, radius) {
            var ang;
            var num;
            ctx.font = radius * 0.15 + "px arial";
            ctx.textBaseline = "middle";
            ctx.textAlign = "center";
            for (num = 1; num < 13; num++) {
                ang = num * Math.PI / 6;
                ctx.rotate(ang);
                ctx.translate(0, -radius * 0.85);
                ctx.rotate(-ang);
                ctx.fillText(num.toString(), 0, 0);
                ctx.rotate(ang);
                ctx.translate(0, radius * 0.85);
                ctx.rotate(-ang);
            }
        }

        function drawTime(ctx, radius) {
            var now = new Date();
            var hour = now.getHours();
            var minute = now.getMinutes();
            var second = now.getSeconds();
            //ore
            hour = hour % 12;
            hour = (hour * Math.PI / 6) +
            (minute * Math.PI / (6 * 60)) +
            (second * Math.PI / (360 * 60));
            drawHand(ctx, hour, radius * 0.5, radius * 0.07);
            //minute
            minute = (minute * Math.PI / 30) + (second * Math.PI / (30 * 60));
            drawHand(ctx, minute, radius * 0.8, radius * 0.07);
            // secunde
            second = (second * Math.PI / 30);
            drawHand(ctx, second, radius * 0.9, radius * 0.02);
        }

        function drawHand(ctx, pos, length, width) {
            ctx.beginPath();
            ctx.lineWidth = width;
            ctx.lineCap = "round";
            ctx.moveTo(0, 0);
            ctx.rotate(pos);
            ctx.lineTo(0, -length);
            ctx.stroke();
            ctx.rotate(-pos);
        }
</script>
</body>
</html>
