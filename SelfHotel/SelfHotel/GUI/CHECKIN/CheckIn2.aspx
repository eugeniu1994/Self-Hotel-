<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckIn2.aspx.cs" Inherits="SelfHotel.GUI.CHECKIN.CheckIn2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Check-In</title>
    <link href="https://fonts.googleapis.com/css?family=Raleway" rel="stylesheet" />
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.1.0.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui.js") %>"></script>
    <link href="../../Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Scripts/startbootstrap-agency-gh-pages/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/googleApisCSS/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href='../../Scripts/googleApisCSS/StyleSheet2.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet3.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet4.css' rel='stylesheet' type='text/css' />
    <link href="../../Scripts/startbootstrap-agency-gh-pages/css/agency.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../Scripts/finalFolder/bootstrapMin.css"/>
    <script src="../../Scripts/finalFolder/bootstrapMin.js"></script>
    <link rel="stylesheet" href="../../Scripts/finalFolder/ResetMin.css" />
    <script src="../../Scripts/colorlib-wizard-2/alertify.js"></script>
	<link rel="stylesheet" href="../../Scripts/colorlib-wizard-2/alertifyCss.css" />
	<link rel="stylesheet" href="../../Scripts/colorlib-wizard-2/defaultAlertify.css" id="toggleCSS" />
    <link rel="stylesheet" href="css/style.css" />
    <link href="nav/css3-responsive-menu/style/reset.css" rel="stylesheet" type="text/css" />
    <link href="nav/css3-responsive-menu/style/example.css" rel="stylesheet" type="text/css" />
    <script src="nav/css3-responsive-menu/style/script/respond.js"></script>
    <script>
          jQuery(function ($) {
                var open = false;

                function resizeMenu() {
                    if ($(this).width() < 768) {
                        if (!open) {
                            $("#menu-drink").hide();
                        }
                        $("#menu-button").show();
                        //$("#logo").attr("src", "nav/css3-responsive-menu/image/logo.png");
                    }
                    else if ($(this).width() >= 768) {
                        if (!open) {
                            $("#menu-drink").show();
                        }
                        $("#menu-button").hide();
                        //$("#logo").attr("src", "nav/css3-responsive-menu/image/logo-large.png");
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
         body::-webkit-scrollbar-track,table::-webkit-scrollbar-track, .sectiuneCamere::-webkit-scrollbar-track
            {
	            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	            background-color: #F5F5F5;
            }

            body::-webkit-scrollbar, table::-webkit-scrollbar, .sectiuneCamere::-webkit-scrollbar
            {
	            width: 10px;
	            background-color: #F5F5F5;
            }

            body::-webkit-scrollbar-thumb, table::-webkit-scrollbar-thumb, .sectiuneCamere::-webkit-scrollbar-thumb
            {
	            background-color: #0ae;
	
	            background-image: -webkit-gradient(linear, 0 0, 0 100%,
	                               color-stop(.5, rgba(255, 255, 255, .2)),
					               color-stop(.5, transparent), to(transparent));
            }
        .alertify-buttons {
            margin-top:10px;
        }
body {
  /*background-color: #f1f1f1;*/
  background-image: url(../../Icoane/b3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
    background-position: center;
}

#regForm {
  background-color: #ffffff;
  margin: 180px auto;
  font-family: Raleway;
  padding: 40px;
  width: 70%;
  min-width: 300px;
      border: 1px solid #0ae;
    border-radius: 10px;
}
@media only screen and (max-width: 780px) {
        #regForm {
            width: 100%;
        }
}
h1 {
  text-align: center;  
}

input {
  padding: 10px;
  width: 100%;
  font-size: 17px;
  font-family: Raleway;
  border: 1px solid #aaaaaa;
      padding-right: 0px;
}

/* Mark input boxes that gets an error on validation: */
input.invalid {
  border: 1px solid red !important;
}
/* Hide all steps by default: */
.tab {
  display: none;
}

button {
  background-color: #4CAF50;
  color: #ffffff;
  border: none;
  padding: 10px 20px;
  font-size: 17px;
  font-family: Raleway;
  cursor: pointer;
}

button:hover {
  opacity: 0.8;
}

#prevBtn {
  background-color: #bbbbbb;
}

/* Make circles that indicate the steps of the form: */
.step {
  height: 15px;
  width: 15px;
  margin: 0 2px;
  background-color: #bbbbbb;
  border: none;  
  border-radius: 50%;
  display: inline-block;
  opacity: 0.5;
}

.step.active {
  opacity: 1;
}

/* Mark the steps that are finished and valid: */
.step.finish {
  background-color: #4CAF50;
}
.cameraCheckIn {
              border: 1px solid #0ae;
    border-radius: 10px;
    padding: 10px 25px 31px 36px;
    position: relative;
    background: rgba(255, 255, 255, 0.1);
    margin:auto;
    margin-top: 5px;
    max-width: 300px;
    /*margin-right: 5px;*/
            }
.flex-container {
  padding: 0;
  margin: 0;
  list-style: none;
  border: 1px solid silver;
  -ms-box-orient: horizontal;
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -moz-flex;
  display: -webkit-flex;
  display: flex;
}
.wrap    { 
  -webkit-flex-wrap: wrap;
  flex-wrap: wrap;
}  
.flex-item {
  background: tomato;
  padding: 5px;
  width: 100px;
  height: 100px;
  margin: 10px;
  
  line-height: 100px;
  color: white;
  font-weight: bold;
  font-size: 2em;
  text-align: center;
}

.form-wrapper-outer{
            padding: 40px;
            border-radius: 8px;
            margin: auto;
            width: 460px;
            border: 1px solid #DADCE0;
            margin-top: 7%;
        }
 
        .form-wrapper-outer .form-logo{
            margin: 0px auto 15px;
            width: 100px;
        }
 
        .form-wrapper-outer .form-logo img{
            width: 100%;
        }
 
        .form-greeting{
            text-align: center;
            font-size: 25px;
            margin-bottom: 5px;
        }
 
        .form-button{
            text-align: right;
        }
 
        .field-wrapper{
            position: relative;
            margin-bottom: 5px;
        }
 
        .field-wrapper input{
            border: 1px solid #DADCE0;
            padding: 15px;
            border-radius: 4px;
            width: 100%;
        }
 
        .field-wrapper .field-placeholder{
            font-size: 16px;
            position: absolute;
            /* background: #fff; */
            bottom: 17px;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
            color: #80868b;
            left: 8px;
            padding: 0 8px;
            -webkit-transition: transform 150ms cubic-bezier(0.4,0,0.2,1),opacity 150ms cubic-bezier(0.4,0,0.2,1);
            transition: transform 150ms cubic-bezier(0.4,0,0.2,1),opacity 150ms cubic-bezier(0.4,0,0.2,1);
            z-index: 1;
 
            text-align: left;
            width: 100%;
        }        
        
        .field-wrapper .field-placeholder span{
            background: #ffffff;
            padding: 0px 8px;
        }
 
        .field-wrapper input:not([disabled]):focus~.field-placeholder
        {
            color:#1A73E8;
        }
 
        .field-wrapper input:not([disabled]):focus~.field-placeholder,
        .field-wrapper.hasValue input:not([disabled])~.field-placeholder
        {
            -webkit-transform: scale(.75) translateY(-28px) translateX(-37px);
            transform: scale(.75) translateY(-28px) translateX(-37px);
        }
        .titluForm {
                text-align: center;
                font-style: oblique;
                font-size: x-large;
                color: black;
                -webkit-text-fill-color: white;
                -webkit-text-stroke-width: 1px;
                -webkit-text-stroke-color: black;
                padding-bottom: 10px;
        }


@-moz-keyframes bounce {
  0%, 100% {
    -moz-transform: translateX(0);
    transform: translateX(0);
  }
  56% {
    -moz-transform: translateX(4px);
    transform: translateX(4px);
  }
}
@-webkit-keyframes bounce {
  0%, 100% {
    -webkit-transform: translateX(0);
    transform: translateX(0);
  }
  56% {
    -webkit-transform: translateX(4px);
    transform: translateX(4px);
  }
}
@keyframes bounce {
  0%, 100% {
    -moz-transform: translateX(0);
    -ms-transform: translateX(0);
    -webkit-transform: translateX(0);
    transform: translateX(0);
  }
  56% {
    -moz-transform: translateX(4px);
    -ms-transform: translateX(4px);
    -webkit-transform: translateX(4px);
    transform: translateX(4px);
  }
}

@-moz-keyframes bounce2 {
  0%, 100% {
    -moz-transform: translateX(4px);
    transform: translateX(4px);
  }
  56% {
    -moz-transform: translateX(0);
    transform: translateX(0);
  }
}
@-webkit-keyframes bounce2 {
  0%, 100% {
    -webkit-transform: translateX(4px);
    transform: translateX(4px);
  }
  56% {
    -webkit-transform: translateX(0);
    transform: translateX(0);
  }
}
@keyframes bounce2 {
  0%, 100% {
    -moz-transform: translateX(4px);
    -ms-transform: translateX(4px);
    -webkit-transform: translateX(4px);
    transform: translateX(4px);
  }
  56% {
    -moz-transform: translateX(0);
    -ms-transform: translateX(0);
    -webkit-transform: translateX(0);
    transform: translateX(0);
  }
} 

.arrow::after {
  display: inline-block;
  padding-left: 8px;
  content: "\2192";
  transition: -webkit-transform 0.3s ease-out;
  transition: transform 0.3s ease-out;
  transition: transform 0.3s ease-out, -webkit-transform 0.3s ease-out;
}
.arrow2::before {
  display: inline-block;
  content: "\2190";
  transition: -webkit-transform 0.3s ease-out;
  transition: transform 0.3s ease-out;
  transition: transform 0.3s ease-out, -webkit-transform 0.3s ease-out;
}
.arrow2:hover::before {
  -moz-animation: bounce2 0.5s infinite;
  -webkit-animation: bounce2 0.5s infinite;
  animation: bounce2 0.5s infinite;
}
.arrow:hover::after {
  -moz-animation: bounce 0.5s infinite;
  -webkit-animation: bounce 0.5s infinite;
  animation: bounce 0.5s infinite;
}

</style>
    <style>
        #ClientTopHeader {
            min-width: 604px;
            margin: 0 0 8px 0;
            width: 100%;
            background: rgba(0,0,0,0.65);
            font-family: Calibri;
            font-weight: bold;
            color: #FFFFFF;
            font-size: 14px;
            padding-top: 15px;
            padding-bottom: 15px;
            display:none;
        }
        #ClientTopHeaderLR {
                width: 600px;
                overflow: auto;
                margin: 0 auto;
            }
        #ClientTopHeaderRight {
    float: right;
}
        #ClientHotelTel, #ClientHotelHome {
    float: right;
    padding: 4px;
}
        a.HotelLink {
    font-family: Calibri;
    font-weight: bold;
    color: #FFFFFF;
    font-size: 14px;
    text-decoration: none;
}
        #ClientHotelEmail, #ClientHotelTel, #ClientHotelHome {
    float: right;
    padding: 4px;
}
        #ClientHotelEmail, #ClientHotelTel, #ClientHotelHome {
    float: right;
    padding: 4px;
}
        #ClientTopHeaderLeft {
    float: left;
}
        #ClientHotelName, #ClientHotelStars {
    float: left;
    padding: 4px;
}
        #ClientHotelName, #ClientHotelStars {
    float: left;
    padding: 4px;
}
        #ClientTopHeaderMiddle {
    clear: both;
    padding: 4px;
}

        @media only screen and (max-width: 600px) {
               #ClientTopHeader {
    min-width: 320px;
    margin: 0 0 8px 0;
    width: 100%;
    background: rgba(0,0,0,0.65);
    font-family: Calibri;
    font-weight: bold;
    color: #FFFFFF;
    font-size: 14px;
    padding-top: 15px;
    padding-bottom: 15px;
}
               #ClientTopHeaderLR {
    width: 316px;
    overflow: auto;
    margin: 0 auto;
}
               #ClientTopHeaderRight {
    float: left;
}
               #ClientHotelEmail, #ClientHotelTel, #ClientHotelHome {
    float: right;
    padding: 4px;
}
               a.HotelLink {
    font-family: Calibri;
    font-weight: bold;
    color: #FFFFFF;
    font-size: 14px;
    text-decoration: none;
}
#ClientTopHeaderLeft {
    float: left;
}
#ClientHotelName, #ClientHotelStars {
    float: left;
    padding: 4px;
}
#ClientHotelName, #ClientHotelStars {
    float: left;
    padding: 4px;
}
#ClientTopHeaderMiddle {
    clear: both;
    padding: 4px;
}


/*#msform input:optional {
  border-left-color: #999;
}
#msform  input:required {
  border-left-color: palegreen;
}
#msform  input:invalid {
  border-left-color: salmon;
}*/
              /*input:required:invalid, input:focus:invalid, field-wrapper input:required:invalid , field-wrapper input:focus:invalid, field-wrapper, field-placeholder span{
                background-image: url(../../Icoane/invalid.png);
                background-position: right top;
                background-repeat: no-repeat;
              }
              field-wrapper input:required:valid, field-wrapper {
                background-image: url(../../Icoane/valid.png);
                background-position: right top;
                background-repeat: no-repeat;
              }*/
              /*background-image: url(../../Icoane/invalid.png);
                background-position: left;
                background-repeat: no-repeat;*/
        }
        #btnPrinteazaID, #btnPlataID {
            color: #fff !important;
            text-transform: uppercase;
            background: #4caf50;
            padding: 15px;
            border-radius: 50px;
            display: inline-block;
            border: none;
         }
         #btnPrinteazaID:hover, #btnPlataID:hover {
            text-shadow: 0px 0px 6px rgba(255, 255, 255, 1);
            -webkit-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
            -moz-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
            transition: all 0.4s ease 0s;
        }
        #btnPlataID {
            width:170px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Scripts/Customizable-Loading-Modal-Plugin/css/modal-loading.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Customizable-Loading-Modal-Plugin/css/modal-loading-animate.css" />
    <script src="../../Scripts/jquery-3.1.0.js" type="text/javascript" ></script>
    <script src="../../Scripts/jquery.translate.js" type="text/javascript" ></script>
 </head>
<body id="bodyID">
    <div id="ClientTopHeader" >
        <div id="ClientTopHeaderLR">
            <div id="ClientTopHeaderRight">
                <div id="ClientHotelTel"><img src="../../Icoane/tel_24px.png" width="24" style="vertical-align:middle" /> <a class="HotelLink" href="tel:+40 241 615 211">+40 241 615 211</a></div>
                <div id="ClientHotelEmail"><img src="../../Icoane/email_24px.png" width="24" style="vertical-align:middle" /> <a class="HotelLink" href="mailto:alin.viespe@multisoft.ro">E-MAIL</a></div>
                <div id="ClientHotelHome"><img src="../../Icoane/home_24px.png" width="23" style="vertical-align:middle" /> <a class="HotelLink" href="../../../Home.aspx">HOME</a></div>
            </div>
            <div id="ClientTopHeaderLeft">
                <div id="ClientHotelName"><a class="HotelLink" href="#">HOTEL DEMO</a></div>
                <div id="ClientHotelStars"><img src="../../Icoane/star_16px.png" width="16" style="vertical-align:middle" /><img src="../../Icoane/star_16px.png" width="16" style="vertical-align:middle" /><img src="../../Icoane/star_16px.png" width="16" style="vertical-align:middle"/><img src="../../Icoane/star_16px.png" width="16" style="vertical-align:middle" /><img src="../../Icoane/star_16px.png" width="16" style="vertical-align:middle" /></div>
            </div>
            <div id="ClientTopHeaderMiddle">Str. Trandafirului Nr. 13A, Constanta, Romania.</div>
        </div>
    </div>

    <div style="position: absolute;float: right;right: 0;color: #f8f9fa;z-index: 999999999;">
            <span id="tag1" class="trn" >Limba:</span>
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
                <nav id="menu-drink">
                    <ul>
                        <li>
                            <a class="meniu beer trn"  href="../../../Home.aspx"><span id="HomeID" class="icon"></span><span id="tag2">Acasa</span></a>
                        </li>
                        <li>
                            <a class="meniu beer" href="CheckIn2.aspx"><span id="CheckInID" class="icon"></span>Check in</a>
                        </li>
                        <li>
                            <a class="meniu wine trn"  onclick='goToAlin()'><span id="RezervaID" class="icon"></span><span id="tag3">Rezervare</span></a>
                        </li>
                        <li>
                            <a class="meniu soft-drink" href="../CHECKOUT/PlataForm2.aspx"><span id="CheckOutID" class="icon"></span>Check out</a>
                        </li>
                    </ul>
                </nav>
            </header>
        </div>

    <form id="regForm" style="margin-top:23px" autocomplete="off" >
      <h1 class="titluForm">Check-In:</h1>
      <div class="tab"><a href="#" id="tag4" style=" color: black;
    font-family: 'Kaushan Script',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,'Helvetica Neue',Arial,sans-serif,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol','Noto Color Emoji';">Completati codul rezervarii</a>
          <div id="parinteInput" runat="server" class="field-wrapper" style="margin-top: 15px;">
            <input type="text" id="txtCod" runat="server" oninput="this.className = ''" required="required" />
            <div class="field-placeholder"><span class="trn" id="tag5">Codul rezervarii</span></div>
          </div>
          <hr />
            <p style="font-size: 14px;" id="pTC">By clicking next you  agree to our  <a id="aTC" href="#myModal" data-toggle="modal" data-target="#myModal" >Terms & Privacy</a>.</p>
      </div>

      <div class="tab "><a id="aNrCamere" href="#" style=" color: black;
           font-family: 'Kaushan Script',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,'Helvetica Neue',Arial,sans-serif,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol','Noto Color Emoji';">Aveti 5 camere rezervate</a>
         
           <div id="divCuCamereID" class="flex-container wrap sectiuneCamere" style="overflow-y: auto;max-height: 450px;">
                  <div class=" cameraCheckIn .flex-item" > <!--form-row-->
                            <div class="form-row">
	                    	        <label style="width: -webkit-fill-available;">
	                    		       Camera
	                    	        </label>
	                    	        <div class="form-holder">
	                    		        <input type="text" readonly="readonly" class="form-control" value="Dubla" />
	                    	        </div>
	                        </div>
                            	
                            <div class="form-row">
	                    	    <label for="" style="width: -webkit-fill-available;">
	                    		   Nr. Persoane
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <input type="text" readonly="readonly" class="form-control" value="2 adulti si 3 copii" />
	                    	    </div>
	                        </div>	

                            <div class="form-row">
	                    	    <label style="width: -webkit-fill-available;">
	                    		   Servicii
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <ul style="font-size:12px;">
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
	                    		    </ul>
	                    	    </div>
	                        </div>

                            <span style="float: right;">Pret 150 RON</span>
	              </div>	
                  <div class=" cameraCheckIn .flex-item" > <!--form-row-->
	                    	 	
                             <div class="form-row">
	                    	        <label style="width: -webkit-fill-available;">
	                    		       Camera
	                    	        </label>
	                    	        <div class="form-holder">
	                    		        <input type="text" readonly="readonly" class="form-control" value="Dubla" />
	                    	        </div>
	                            </div>
                            	
                            <div class="form-row">
	                    	    <label for="">
	                    		   Nr. Persoane
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <input type="text" readonly="readonly" class="form-control" value="2 adulti si 3 copii" />
	                    	    </div>
	                        </div>	

                            <div class="form-row">
	                    	    <label style="width: -webkit-fill-available;">
	                    		   Servicii
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <ul style="font-size:12px;">
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
	                    		    </ul>
	                    	    </div>
	                        </div>

                            <span style="float: right;">Pret 150 RON</span>
	                    </div>
                  <div class=" cameraCheckIn .flex-item" > <!--form-row-->
	                    	 	
                             <div class="form-row">
	                    	        <label style="width: -webkit-fill-available;">
	                    		       Camera
	                    	        </label>
	                    	        <div class="form-holder">
	                    		        <input type="text" readonly="readonly" class="form-control" value="Dubla" />
	                    	        </div>
	                            </div>
                            	
                            <div class="form-row">
	                    	    <label for="">
	                    		   Nr. Persoane
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <input type="text" readonly="readonly" class="form-control" value="2 adulti si 3 copii" />
	                    	    </div>
	                        </div>	

                            <div class="form-row">
	                    	    <label style="width: -webkit-fill-available;">
	                    		   Servicii
	                    	    </label>
	                    	    <div class="form-holder">
	                    		    <ul style="font-size:12px;">
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
                                        <li>serv</li>
	                    		    </ul>
	                    	    </div>
	                        </div>

                            <span style="float: right;">Pret 150 RON</span>
	                    </div>
            </div>
      
      </div>

      <div id="tabCuCamere" class="tab"><a href="#" id="tag0" style=" color: black;
         font-family: 'Kaushan Script',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,'Helvetica Neue',Arial,sans-serif,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol','Noto Color Emoji';">Completati fisele de anuntare a sosirii</a>
            <div id="msform"> <%--style="background: darkcyan;"--%>
              <ul id="progressbar" >
                <li class="active" style="color: black !important;">Persoana x</li>
                <li style="color: black !important;">Social Profiles</li>
                <li style="color: black !important;">Personal Details</li>
                <li style="color: black !important;">Social Profiles</li>
                <li style="color: black !important;">Personal Details</li>
                <li style="color: black !important;">Social Profiles</li>
              </ul>

              <fieldset id="field1" data-idcam="1" data-tipcam="dubla" data-idturist="1" data-idrezervare="1" data-idrezervarecamera="2" >
                <h2 class="fs-title">Camera x double</h2>
                <h3 class="fs-subtitle">Persoana x</h3>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" class="txtNumePrenume" maxlength="20" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nume si Prenume</span></div>
                  </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="txtDataN" oninput="this.className = ''"  />
                        <div class="field-placeholder"><span>Data nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtLocN" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Locul nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtCetatenie" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Cetatenie</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtLocalitate" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Localitatea</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtStrada" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtNrStrada" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nr. strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtTara" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Tara</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="txtDataS" />
                        <div class="field-placeholder"><span>Data sosirii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="txtDataP" />
                        <div class="field-placeholder"><span>Data plecarii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtScop" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Scop calatoriei</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtActIdentitate" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Act identitate</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtSerie" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Seria </span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="txtNrAct" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Numar  </span></div>
                   </div>
                  <input type="button" name="next" class="next action-button"  value="Urmatorul" /> <%--onclick="nextPrev2(1,this)"--%>
              </fieldset>

              <fieldset id="Fieldset2">
                <h2 class="fs-title">Camera x double</h2>
                <h3 class="fs-subtitle">Persoana x</h3>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text1" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nume si Prenume</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date1" />
                        <div class="field-placeholder"><span>Data nasterii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text2" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Locul nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text3" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Cetatenie</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text4" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Localitatea</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text5" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text6" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nr. strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text7" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Tara</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date2" />
                        <div class="field-placeholder"><span>Data sosirii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date3" />
                        <div class="field-placeholder"><span>Data plecarii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text8" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Scop calatoriei</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text9" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Act identitate</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text10" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Seria </span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text11" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Numar  </span></div>
                   </div>
                  <input type="button" name="previous" class="previous action-button" value="Precedent" />
                  <input type="button" name="next" class="next action-button"  value="Urmatorul" /> <%--onclick="nextPrev2(1,this)"--%>
              </fieldset>
              <fieldset id="Fieldset3">
                <h2 class="fs-title">Camera x double</h2>
                <h3 class="fs-subtitle">Persoana x</h3>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text12" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nume si Prenume</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date4" />
                        <div class="field-placeholder"><span>Data nasterii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text13" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Locul nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text14" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Cetatenie</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text15" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Localitatea</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text16" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text17" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nr. strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text18" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Tara</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date5" />
                        <div class="field-placeholder"><span>Data sosirii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date6" />
                        <div class="field-placeholder"><span>Data plecarii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text19" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Scop calatoriei</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text20" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Act identitate</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text21" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Seria </span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text22" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Numar  </span></div>
                   </div>
                  <input type="button" name="previous" class="previous action-button" value="Precedent" />
                <input type="button" name="next" class="next action-button"  value="Urmatorul" /> <%--onclick="nextPrev2(1,this)"--%>
              </fieldset>
              <fieldset id="Fieldset4">
                <h2 class="fs-title">Camera x double</h2>
                <h3 class="fs-subtitle">Persoana x</h3>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text23" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nume si Prenume</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date7" />
                        <div class="field-placeholder"><span>Data nasterii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text24" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Locul nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text25" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Cetatenie</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text26" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Localitatea</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text27" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text28" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nr. strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text29" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Tara</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date8" />
                        <div class="field-placeholder"><span>Data sosirii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date9" />
                        <div class="field-placeholder"><span>Data plecarii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text30" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Scop calatoriei</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text31" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Act identitate</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text32" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Seria </span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text33" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Numar  </span></div>
                   </div>
                  <input type="button" name="previous" class="previous action-button" value="Precedent" />
                <input type="button" name="next" class="next action-button"  value="Urmatorul" /> <%--onclick="nextPrev2(1,this)"--%>
              </fieldset>
              <fieldset id="Fieldset5">
                <h2 class="fs-title">Camera x double</h2>
                <h3 class="fs-subtitle">Persoana x</h3>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text34" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nume si Prenume</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date10" />
                        <div class="field-placeholder"><span>Data nasterii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text35" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Locul nasterii</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text36" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Cetatenie</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text37" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Localitatea</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text38" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text39" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Nr. strada</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text40" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Tara</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date11" />
                        <div class="field-placeholder"><span>Data sosirii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="date" id="Date12" />
                        <div class="field-placeholder"><span>Data plecarii</span></div>
                   </div>
                   <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text41" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Scop calatoriei</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text42" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Act identitate</span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text43" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Seria </span></div>
                   </div>
                  <div class="field-wrapper col-xs-12 col-md-6 col-lg-6 col-sm-6">
                        <input type="text" id="Text44" oninput="this.className = ''" />
                        <div class="field-placeholder"><span>Numar  </span></div>
                   </div>
                  <input type="button" name="previous" class="previous action-button" value="Precedent" />
                <input type="button" name="next" class="next action-button"  value="Urmatorul" /> <%--onclick="nextPrev2(1,this)"--%>
              </fieldset>

              <fieldset id="Fieldset1">
                <h2 class="fs-title">Success</h2>
                <h3 class="fs-subtitle">Toate fisele au fost completate</h3>
    
                <input type="button" name="next" onclick="nextPrev(1)" class=" action-button" value="Gata" />
              </fieldset>
            </div>
      </div>

      <div id="tabFinal" class="tab" style="text-align: center;">
            <a href="#" id="tag27" style=" color: black;display: inherit;font-family: 'Kaushan Script',-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,'Helvetica Neue',Arial,sans-serif,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol','Noto Color Emoji';">Check-in realizat cu success </a>
            <button id="btnPrinteazaID" type="button"  onclick="printeazaFise()">Printeaza fise</button>
            <button type="button" id="btnPlataID" onclick="deschideFormularPlata()" >Plata </button>
      </div>

      <div style="overflow:auto;  margin-top: 10px;">
        <div >
          <button type="button" class="arrow2" id="prevBtn"  onclick="nextPrev(-1)">Inapoi</button>
          <button type="button" class="arrow" id="nextBtn" style="float:right;" onclick="nextPrev(1)">Cauta</button>
        </div>
      </div>

      <div style="text-align:center;margin-top:40px;">
        <span class="step"></span>
        <span class="step"></span>
        <span class="step"></span>
      </div>
    </form>
       <!-- termeni si conditii -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog" style="margin: 174px auto;">
    
      <div class="modal-content">
        <div class="modal-header">
            <h4 class="modal-title" id="tcTag" >Termeni si Conditii</h4>
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          
        </div>
        <div class="modal-body">
          <p id="TCContinut" >termeni si conditii din baza.</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>


    <script type="text/javascript" src="checkin2.js"></script>
    <script src="../../Scripts/finalFolder/Easing.js"></script>
    <script src="../../Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="../../Scripts/colorlib-wizard-2/notify.js" type="text/javascript"></script>  
    
    <script type="text/javascript">
        $(function () {
            $('.meniu').each(function () {
                if ($(this).prop('href') == window.location.href) {
                    $(this).addClass('activa');
                }
            });
        });
     </script>
    <script  src="js/index.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/Customizable-Loading-Modal-Plugin/js/modal-loading.js") %>"></script>


</body>
</html>


