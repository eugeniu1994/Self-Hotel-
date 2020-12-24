<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlataForm2.aspx.cs" Inherits="SelfHotel.GUI.CHECKOUT.PlataForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CheckOut-Plata</title>
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.1.0.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui.js") %>"></script>
    <link href="../../Scripts/googleApisCSS/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href='../../Scripts/googleApisCSS/StyleSheet2.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet3.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet4.css' rel='stylesheet' type='text/css' />
    <link rel="icon" type="image/png" href="../../Scripts/Login_v12/images/icons/favicon.ico"/>
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/fonts/Linearicons-Free-v1.0.0/icon-font.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/animate/animate.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/css-hamburgers/hamburgers.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/select2/select2.min.js" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/css/util.css" />
	<link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/css/main.css" />
    <link rel="stylesheet" type="text/css" href="../../Style/css/custom-checkbox-radio.css" />
    
    <style>
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

    h1 {
      text-align: center;  
    }

    /*input {
      padding: 10px;
      width: 100%;
      font-size: 17px;
      font-family: Raleway;
      border: 1px solid #aaaaaa;
    }

    input.invalid {
      background-color: #ffdddd;
    }*/

    .tab {
      display: none;
    }

    .btnFooter {
      background-color: #4CAF50;
      color: #ffffff;
      border: none;
      padding: 10px 20px;
      font-size: 17px;
      font-family: Raleway;
      cursor: pointer;
    }

    .btnFooter:hover {
      opacity: 0.8;
    }

    #prevBtn {
      background-color: #bbbbbb;
    }

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

    .step.finish {
      background-color: #4CAF50;
    }
</style>
    <style>
        body {
          padding: 60px 0;
          background-color: rgba(178,209,229,0.7);
          margin: 0 auto;
          /*width: 600px;*/
        }
        .notifyjs-bootstrap-base notifyjs-bootstrap-error {
            z-index:9999;
        }
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

        .body-text {
          padding: 0 20px 30px 20px;
          font-family: "Roboto";
          font-size: 1em;
          color: #333;
          text-align: center;
          line-height: 1.2em;
        }
        .form-container {
          flex-direction: column;
          justify-content: center;
          align-items: center;
          width: 100%;
        }
        .card-wrapper {
          background-color: #6FB7E9;
          width: 100%;
          display: flex;

        }
        .personal-information {
          background-color: #3C8DC5;
          color: #fff;
          padding: 1px 0;
          text-align: center;
        }

        h1 {
          font-size: 1.3em;
          font-family: "Roboto"
        }
        input {
          margin: 1px 0;
          padding-left: 3%;
          font-size: 14px;
        }
        input[type="text"]{
          display: block;
          height: 50px;
          /*width: 97%;*/
          border: none;
        }
        input[type="email"]{
          display: block;
          height: 50px;
          /*width: 97%;*/
          border: none;
        }
        input[type="submit"]{
          display: block;
          height: 60px;
          width: 100%;
          border: none;
          background-color: #3C8DC5;
          color: #fff;
          margin-top: 2px;
          curson: pointer;
          font-size: 0.9em;
          text-transform: uppercase;
          font-weight: bold;
          cursor: pointer;
        }
        input[type="submit"]:hover{
          background-color: #6FB7E9;
          transition: 0.3s ease;
        }
        #Coloana-stanga {
          width: 46.8%;
          float: left;
          margin-bottom: 2px;
        }
        #Coloana-dreapta {
          width: 46.8%;
          float: right;
        }
       
         @media only screen and (max-width: 480px){
          body {
            width: 100%;
            margin: 0 auto;
          }
          .form-container {
            margin: 0 2%;
          }
          input {
            font-size: 1em;
          }
          #input-button {
            width: 100%;
          }
          #Camp-Input {
            width: 96.5%;
          }
          h1 {
            font-size: 1.2em;
          }
          input {
            margin: 2px 0;
          }
          input[type="submit"]{
            height: 50px;
          }
          #Coloana-stanga {
            width: 96.5%;
            display: block;
            float: none;
          }
          #Coloana-dreapta {
            width: 96.5%;
            display: block;
            float: none;
          }
        }

         .open-button {
                position: absolute;
                top: 4px;
                right: 9px;
                width: 34px;
                height: 32px;
                /*pointer-events: none;*/
        }

        .open-button button {
          border: none;
          background: transparent;
        }

        
            #price {
              text-align: center;
            }

            .plan {
              display: inline-block;
              margin: 10px 1%;
              font-family: "Lato", Arial, sans-serif;
            }

            .plan-inner {
              background: #fff;
              margin: 0 auto;
              min-width: 280px;
              max-width: 100%;
              position: relative;
            }

            .entry-title {
              background: #53cfe9;
              height: 100px;
              position: relative;
              text-align: center;
              color: #fff;
              margin-bottom: 12px;
            }

            .entry-title > h3 {
              background: #20bada;
              font-size: 20px;
              padding: 5px 0;
              text-transform: uppercase;
              font-weight: 700;
              margin: 0;
            }

            .entry-title .price {
              position: absolute;
              bottom: -25px;
              background: #20bada;
              height: 95px;
              width: 95px;
              margin: 0 auto;
              left: 0;
              right: 0;
              overflow: hidden;
              border-radius: 50px;
              border: 5px solid #fff;
              line-height: 80px;
              font-size: 28px;
              font-weight: 700;
            }

            .price span {
              position: absolute;
              font-size: 9px;
              bottom: -10px;
              left: 30px;
              font-weight: 400;
            }

            .entry-content {
              color: #323232;
            }

            .entry-content ul {
              margin: 0;
              padding: 0;
              list-style: none;
              text-align: center;
            }

            .entry-content li {
              border-bottom: 1px solid #e5e5e5;
              padding: 5px 0;
            }

            .entry-content li:last-child {
              border: none;
            }

            .btn {
              padding: 1em 0;
              text-align: center;
            }

            .btn a {
              background: #323232;
              padding: 10px 30px;
              color: #fff;
              text-transform: uppercase;
              font-weight: 700;
              text-decoration: none;
            }
            .hot {
              position: absolute;
              top: -7px;
              background: #f80;
              color: #fff;
              text-transform: uppercase;
              z-index: 2;
              padding: 2px 5px;
              font-size: 9px;
              border-radius: 2px;
              right: 10px;
              font-weight: 700;
            }
            .basic .entry-title {
              background: #75ddd9;
            }

            .basic .entry-title > h3 {
              background: #44cbc6;
            }

            .basic .price {
              background: #44cbc6;
            }

            .standard .entry-title {
              background: #4484c1;
            }

            .standard .entry-title > h3 {
              background: #3772aa;
            }

            .standard .price {
              background: #3772aa;
            }

            .ultimite .entry-title > h3 {
              background: #dd4b5e;
            }

            .ultimite .entry-title {
              background: #f75c70;
            }

            .ultimite .price {
              background: #dd4b5e;
            }
                .header {
                  background-color: #327a81;
                  color: white;
                  font-size: 1.5em;
                  padding: 1rem;
                  text-align: center;
                  text-transform: uppercase;
                }

                .imgTabel {
                  border-radius: 50%;
                  height: 80px;
                  width: 80px;
                }

                .table-users {
                  border: 1px solid #327a81;
                  border-radius: 10px;
                  box-shadow: 3px 3px 0 rgba(0, 0, 0, 0.1);
                  max-width: calc(100% - 2em);
                  margin: 1em auto;
                  overflow: hidden;
                  width: 800px;
                }

               #tabCuCamereID table {
                  width: 100%;
                }
               #tabCuCamereID  table td, table th {
                  color: black;
                  padding: 10px;
                }
               #tabCuCamereID  table td {
                  text-align: center;
                  vertical-align: middle;
                }
               #tabCuCamereID  table td:last-child {
                  font-size: 0.95em;
                  line-height: 1.4;
                  text-align: left;
                }
               #tabCuCamereID  table th {
                  background-color: #daeff1;
                  font-weight: 300;
                }
               #tabCuCamereID  table tr:nth-child(2n) {
                  background-color: white;
                }
               #tabCuCamereID  table tr:nth-child(2n+1) {
                  background-color: #edf7f8;
                }

                /*@media screen and (max-width: 700px) {*/
                 #tabCuCamereID  table, #tabCuCamereID tr,#tabCuCamereID td {
                    display: block;
                  }

                 #tabCuCamereID  td:first-child {
                    position: absolute;
                    top: 50%;
                    -webkit-transform: translateY(-50%);
                            transform: translateY(-50%);
                    width: 100px;
                  }
                #tabCuCamereID   td:not(:first-child) {
                    clear: both;
                    margin-left: 100px;
                    padding: 4px 20px 4px 90px;
                    position: relative;
                    text-align: left;
                  }
                #tabCuCamereID   td:not(:first-child):before {
                    color: #3C8DC5;
                    content: '';
                    display: block;
                    left: 0;
                    position: absolute;
                  }
                #tabCuCamereID   td:nth-child(2):before {
                    content: 'Camera:';
                  }
                #tabCuCamereID   td:nth-child(3):before {
                    content: 'Servicii:';
                  }
                  /*td:nth-child(4):before {
                    content: 'Copii:';
                  }
                  td:nth-child(5):before {
                    content: 'Total Plata:';
                  }*/

                 #tabCuCamereID  tr {
                    padding: 10px 0;
                    position: relative;
                  }
                #tabCuCamereID   tr:first-child {
                    display: none;
                  }
                /*}*/
                @media screen and (max-width: 750px) {
                  .header {
                    background-color: transparent;
                    color: white;
                    font-size: 2em;
                    font-weight: 700;
                    padding: 0;
                    text-shadow: 2px 2px 0 rgba(0, 0, 0, 0.1);
                  }

                  imgTabel {
                    border: 3px solid;
                    border-color: #daeff1;
                    height: 100px;
                    margin: 0.5rem 0;
                    width: 100px;
                  }

                #tabCuCamereID   td:first-child {
                    background-color: #c8e7ea;
                    border-bottom: 1px solid #91ced4;
                    border-radius: 10px 10px 0 0;
                    position: relative;
                    top: 0;
                    -webkit-transform: translateY(0);
                            transform: translateY(0);
                    width: 100%;
                  }
                #tabCuCamereID   td:not(:first-child) {
                    margin: 0;
                    padding: 5px 1em;
                    width: 100%;
                  }
                #tabCuCamereID   td:not(:first-child):before {
                    font-size: .8em;
                    padding-top: 0.3em;
                    position: relative;
                  }
                #tabCuCamereID   td:last-child {
                    padding-bottom: 1rem !important;
                  }

                 #tabCuCamereID  tr {
                    background-color: white !important;
                    border: 1px solid #6cbec6;
                    border-radius: 10px;
                    box-shadow: 2px 2px 0 rgba(0, 0, 0, 0.1);
                    margin: 0.5rem 0;
                    padding: 0;
                  }

                  .table-users {
                    border: none;
                    box-shadow: none;
                    overflow: visible;
                  }
                }
                
            button:disabled,
            button[disabled]{
              border: 1px solid #999999 !important;
              background-color: #cccccc !important;
              color: #666666 !important;
              cursor: not-allowed;
              pointer-events:none;
            }
            .divCuButoanePrintare ul {
		    margin: auto;
		    text-align: center;
	    }

	    .divCuButoanePrintare li {
		    list-style: none;
		    position: relative;
		    display: inline-block;
		    width: 100px;
		    height: 100px;
	    }

	    @-moz-keyframes rotate {
		    0% {transform: rotate(0deg);}
		    100% {transform: rotate(-360deg);}
	    }

	    @-webkit-keyframes rotate {
		    0% {transform: rotate(0deg);}
		    100% {transform: rotate(-360deg);}
	    }

	    @-o-keyframes rotate {
		    0% {transform: rotate(0deg);}
		    100% {transform: rotate(-360deg);}
	    }

	    @keyframes rotate {
		    0% {transform: rotate(0deg);}
		    100% {transform: rotate(-360deg);}
	    }

	     .divCuButoanePrintare .round {
		    display: block;
		    position: absolute;
		    left: 0;
		    top: 0;
		    width: 100%;
		    height: 100%;
		    padding-top: 30px;		
		    text-decoration: none;		
		    text-align: center;
		    font-size: 20px;		
		    text-shadow: 0 1px 0 rgba(255,255,255,.7);
		    letter-spacing: -.065em;
		    font-family: "Hammersmith One", sans-serif;		
		    -webkit-transition: all .25s ease-in-out;
		    -o-transition: all .25s ease-in-out;
		    -moz-transition: all .25s ease-in-out;
		    transition: all .25s ease-in-out;
		    box-shadow: 2px 2px 7px rgba(0,0,0,.2);
		    border-radius: 300px;
		    z-index: 1;
		    border-width: 4px;
		    border-style: solid;
	    }

	    .divCuButoanePrintare .round:hover {
		    width: 130%;
		    height: 130%;
		    left: -15%;
		    top: -15%;
		    font-size: 33px;
		    padding-top: 38px;
		    -webkit-box-shadow: 5px 5px 10px rgba(0,0,0,.3);
		    -o-box-shadow: 5px 5px 10px rgba(0,0,0,.3);
		    -moz-box-shadow: 5px 5px 10px rgba(0,0,0,.3);
		    box-shadow: 5px 5px 10px rgba(0,0,0,.3);
		    z-index: 2;
		    border-size: 10px;
		    -webkit-transform: rotate(-360deg);
		    -moz-transform: rotate(-360deg);
		    -o-transform: rotate(-360deg);
		    transform: rotate(-360deg);
	    }
	    .divCuButoanePrintare a.green {
		    background-color: rgba(1,151,171,1);
		    color: rgba(0,63,71,1);
		    border-color: rgba(0,63,71,.2);
	    }

	    .divCuButoanePrintare a.green:hover {
		    color: rgba(1,151,171,1);
	    }

	    .divCuButoanePrintare a.yellow {
		    background-color: rgba(252,227,1,1);
		    color: rgba(153,38,0,1);
		    border-color: rgba(153,38,0,.2);
	    }

	    .divCuButoanePrintare a.yellow:hover {
		    color: rgba(252,227,1,1);
	    }

	    .divCuButoanePrintare .round span.round {
		    display: block;
		    opacity: 0;
		    -webkit-transition: all .5s ease-in-out;
		    -moz-transition: all .5s ease-in-out;
		    -o-transition: all .5s ease-in-out;
		    transition: all .5s ease-in-out;
		    font-size: 1px;
		    border: none;
		    padding: 40% 20% 0 20%;
		    color: #fff;
	    }

	    .divCuButoanePrintare .round span:hover {
		    opacity: .85;
		    font-size: 16px;
		    -webkit-text-shadow: 0 1px 1px rgba(0,0,0,.5);
		    -moz-text-shadow: 0 1px 1px rgba(0,0,0,.5);
		    -o-text-shadow: 0 1px 1px rgba(0,0,0,.5);
		    text-shadow: 0 1px 1px rgba(0,0,0,.5);	
	    }

	    .divCuButoanePrintare .green span {
		    background: rgba(0,63,71,.7);		
	    }

	    .divCuButoanePrintare .yellow span {
		    background: rgba(161,145,0,.7);	

	    }
            .modal-window {
                position: fixed;
                background-color: rgba(255, 255, 255, 0.25);
                top: 0;
                right: 0;
                bottom: 0;
                left: 0;
                z-index: 999;
                opacity: 0;
                pointer-events: none;
                transition: all 0.3s;
            }

                .modal-window:target {
                    opacity: 1;
                    pointer-events: auto;
                }

                .modal-window > div {
                    width: 400px;
                    position: absolute;
                    top: 50%;
                    left: 50%;
                    -webkit-transform: translate(-50%, -50%);
                    transform: translate(-50%, -50%);
                    padding: 2em;
                    background: #ffffff;
                    color: #333333;
                }

                .modal-window header {
                    font-weight: bold;
                }

                .modal-window h1 {
                    font-size: 150%;
                    margin: 0 0 15px;
                    color: #333333;
                }

            .modal-close {
                color: #aaa;
                line-height: 50px;
                font-size: 80%;
                position: absolute;
                right: 0;
                text-align: center;
                top: 0;
                width: 70px;
                text-decoration: none;
            }

                .modal-close:hover {
                    color: #000;
                }
            .btnFirma {
                cursor: pointer;
                padding: 7px 25px 7px 26px;
                text-transform: uppercase;
                color: #fff;
                background-color: #3c8dc5;
                border-radius: 30px;
                margin-bottom: 5px;
             }


            .custom-search {
	            position: absolute;
	            right: 20px;
	            top: 1px;
            }
            #express-form-typeahead {
	            background-color: transparent;
                background-image: url(../../Icoane/searchIcon.png);
                background-position: 5px center;
                background-repeat: no-repeat;
                background-size: 35px 35px;
                border: none;
                cursor: pointer;
                height: 40px;
                margin: 3px 0;
                padding: 0px 0 0 50px;
                position: relative;
                -webkit-transition: width 400ms ease, background 400ms ease;
                transition: width 400ms ease, background 400ms ease;
                width: 0;  
            }

            .Switcher {
                      position: relative;
                      display: flex;
                      border-radius: 5em;
                      box-shadow: inset 0 0 0 1px;
                      overflow: hidden;
                      cursor: pointer;
                      -webkit-animation: r-n .5s;
                              animation: r-n .5s;
                      -webkit-user-select: none;
                         -moz-user-select: none;
                          -ms-user-select: none;
                              user-select: none;
                      /*font-size: 3vmin;*/
                      font-size: 14px;
                      will-change: transform;
                    }
                    .Switcher__checkbox:checked + .Switcher {
                      -webkit-animation-name: r-p;
                              animation-name: r-p;
                    }
                    @-webkit-keyframes r-p {
                      50% {
                        -webkit-transform: rotateY(45deg);
                                transform: rotateY(45deg);
                      }
                    }
                    @keyframes r-p {
                      50% {
                        -webkit-transform: rotateY(45deg);
                                transform: rotateY(45deg);
                      }
                    }
                    @-webkit-keyframes r-n {
                      50% {
                        -webkit-transform: rotateY(-45deg);
                                transform: rotateY(-45deg);
                      }
                    }
                    @keyframes r-n {
                      50% {
                        -webkit-transform: rotateY(-45deg);
                                transform: rotateY(-45deg);
                      }
                    }
                    .Switcher::before {
                      content: '';
                      position: absolute;
                      top: 0;
                      left: 0;
                      bottom: 0;
                      width: 200%;
                      border-radius: inherit;
                      background-color: #fff;
                      -webkit-transform: translateX(-75%);
                              transform: translateX(-75%);
                      transition: -webkit-transform .5s ease-in-out;
                      transition: transform .5s ease-in-out;
                      transition: transform .5s ease-in-out, -webkit-transform .5s ease-in-out;
                    }
                    .Switcher__checkbox:checked + .Switcher::before {
                      -webkit-transform: translateX(25%);
                              transform: translateX(25%);
                    }

                    .Switcher__trigger {
                      position: relative;
                      z-index: 1;
                      padding: 1em 3em;
                    }
                    .Switcher__trigger::after {
                      content: attr(data-value);
                    }
                    .Switcher__trigger::before {
                      --i: var(--x);
                      content: attr(data-value);
                      position: absolute;
                      color: #00a8ff;
                      transition: opacity .3s;
                      opacity: calc((var(--i) + 1) / 2);
                      transition-delay: calc(.3s * (var(--i) - 1) / -2);
                    }
                    .Switcher__checkbox:checked + .Switcher .Switcher__trigger::before {
                      --i: calc(var(--x) * -1);
                    }
                    .Switcher__trigger:nth-of-type(1)::before {
                      --x: 1;
                    }
                    .Switcher__trigger:nth-of-type(2)::before {
                      --x: -1;
                    }

                    .sr-only {
                      position: absolute;
                      width: 1px;
                      height: 1px;
                      padding: 0;
                      margin: -1px;
                      overflow: hidden;
                      clip: rect(0, 0, 0, 0);
                      border: 0;
                    }

                    .box {
                      display: flex;
                      flex: 1;
                      justify-content: center;
                      align-items: center;
                      flex-wrap: wrap;
                      overflow: hidden;
                      -webkit-perspective: 750px;
                              perspective: 750px;
                    }
    </style>
    <script src="../../Scripts/colorlib-wizard-2/alertify.js"></script>
	<link rel="stylesheet" href="../../Scripts/colorlib-wizard-2/alertifyCss.css" />
	<link rel="stylesheet" href="../../Scripts/colorlib-wizard-2/defaultAlertify.css" id="toggleCSS" />
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
                        //$("#logo").attr("src", "image/logo.png");
                    }
                    else if ($(this).width() >= 768) {
                        if (!open) {
                            $("#menu-drink").show();
                        }
                        $("#menu-button").hide();
                        //$("#logo").attr("src", "image/logo-large.png");
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
                      background-image: url(../../Icoane/bM2.jpg);
                        background-repeat: no-repeat;
                        background-size: cover;
                        background-position: center;
                    }
        #DetaliiFacturareID {
            margin-top:-20px;
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

            }
        </style>
    <link rel="stylesheet" type="text/css" href="../../Scripts/Customizable-Loading-Modal-Plugin/css/modal-loading.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Customizable-Loading-Modal-Plugin/css/modal-loading-animate.css" />

    <style type="text/css">
        .tg  {border-collapse:collapse;border-spacing:0;margin:0px auto;}
        .tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
        .tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
        .tg .tg-88nc{font-weight:bold;border-color:inherit;text-align:center}
        .tg .tg-kiyi{font-weight:bold;border-color:inherit;text-align:left}
        .tg .tg-baqh{text-align:center;vertical-align:top}
        .tg .tg-0pky{border-color:inherit;text-align:left;vertical-align:top}
        .tg .tg-0lax{text-align:left;vertical-align:top}
        .tg .tg-fymr{font-weight:bold;border-color:inherit;text-align:left;vertical-align:top}
        .tg .tg-amwm{font-weight:bold;text-align:center;vertical-align:top}
        .tg .tg-xldj{border-color:inherit;text-align:left}
        .tg-sort-header::-moz-selection{background:0 0}.tg-sort-header::selection{background:0 0}.tg-sort-header{cursor:pointer}.tg-sort-header:after{content:'';float:right;margin-top:7px;border-width:0 5px 5px;border-style:solid;border-color:#404040 transparent;visibility:hidden}.tg-sort-header:hover:after{visibility:visible}.tg-sort-asc:after,.tg-sort-asc:hover:after,.tg-sort-desc:after{visibility:visible;opacity:.4}.tg-sort-desc:after{border-bottom:none;border-width:5px 5px 0}@media screen and (max-width: 767px) {.tg {width: auto !important;}.tg col {width: auto !important;}.tg-wrap {background: white;overflow-x: auto;-webkit-overflow-scrolling: touch;margin: auto 0px;}}

        .celulaAscunsa {
            border: none !important;
            border-left: 1px solid !important;
            border-right: 1px solid !important;    
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
</style>
</head>
<body>
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
            <span id="tag1">Limba:</span>
            <span onclick="toRomanian()" ><img src="../../Icoane/romania.png" style="cursor: pointer;border-radius: 50%;width:35px;height:25px;" /></span>
            <span onclick="toEnglish()" ><img src="../../Icoane/english.png" style="cursor: pointer;border-radius: 50%;width:35px;height:25px;" /></span>
        </div>
     <div id="banner-wrapper">
            <header id="banner" role="banner">
                <div id="banner-inner-wrapper">
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
                            <a class="meniu beer" href="../../../Home.aspx"><span id="HomeID" class="icon"></span><span id="tag2">Acasa</span></a>
                        </li>
                        <li>
                            <a class="meniu beer" href="../CHECKIN/CheckIn2.aspx"><span id="CheckInID" class="icon"></span>Check in</a>
                        </li>
                        <li>
                            <a class="meniu wine" onclick='goToAlin()'><span id="RezervaID" class="icon"></span><span id="tag3">Rezervare</span></a>
                        </li>
                        <li>
                            <a class="meniu soft-drink" href="PlataForm2.aspx"><span id="CheckOutID" class="icon"></span>Check out</a>
                        </li>
                    </ul>
                </nav>
            </header>
        </div>

    <form id="regForm" style="margin-top: 15px !important;background: #dcdbdb;margin-bottom: 10px !important;padding-bottom: 15px;padding-top: 20px;padding-right: 35px;padding-left: 35px;" autocomplete="off"  >
      <div class="span3 widget-span widget-type-raw_html custom-search" style="margin-top: 110px;" data-widget-type="raw_html" data-x="4" data-w="3">
              <input  class="form-control tt-input" id="express-form-typeahead" onclick="cautaRezervare()" type="button" />
         </div>

      <div class="tab" id="tabCuCamereID">
                           <div class="header" id="CamereCountID">Camere</div>
   
                           <table id="btnTabel" cellspacing="0" style="height: 450px;display: inline-block;overflow: auto;">

                              <tr>
                                  <td>
                                     <img  class="imgTabel" src="http://lorempixel.com/100/100/people/1" alt="" />
                                 </td>
                                 <td>Camera 12 small 2 adulti 1 copil</td>
                                 <td>jane.doe@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Lorem ipsum dolor sit amet, consectetur adipisicing elit. </td>
                              </tr>

                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/sports/2" alt="" /></td>
                                 <td>John Doe</td>
                                 <td>john.doe@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Blanditiis, aliquid numquam iure voluptatibus ut maiores explicabo ducimus neque, nesciunt rerum perferendis, inventore.</td>
                              </tr>

                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/people/9" alt="" /></td>
                                 <td>Jane Smith</td>
                                 <td>jane.smith@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td> Culpa praesentium unde pariatur fugit eos recusandae voluptas.</td>
                              </tr>
      
                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/people/3" alt="" /></td>
                                 <td>John Smith</td>
                                 <td>john.smith@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Aut voluptatum accusantium, eveniet, sapiente quaerat adipisci consequatur maxime temporibus quas, dolorem impedit.</td>
                              </tr>

                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/people/3" alt="" /></td>
                                 <td>John Smith</td>
                                 <td>john.smith@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Aut voluptatum accusantium, eveniet, sapiente quaerat adipisci consequatur maxime temporibus quas, dolorem impedit.</td>
                              </tr>

                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/people/3" alt="" /></td>
                                 <td>John Smith</td>
                                 <td>john.smith@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Aut voluptatum accusantium, eveniet, sapiente quaerat adipisci consequatur maxime temporibus quas, dolorem impedit.</td>
                              </tr>

                              <tr>
                                 <td><img class="imgTabel" src="http://lorempixel.com/100/100/people/3" alt="" /></td>
                                 <td>John Smith</td>
                                 <td>john.smith@foo.com</td>
                                 <td>01 800 2000</td>
                                 <td>Aut voluptatum accusantium, eveniet, sapiente quaerat adipisci consequatur maxime temporibus quas, dolorem impedit.</td>
                              </tr>

                           </table>

                           <div id="lastChildID" class="login100-form-btn">
                                 <h1 id="totalGeneralID">Total de plata: 0 ron</h1>
					        </div>
      </div>
     
      <div class="tab" id="tabClient">
            <div id="DetaliiFacturareID" class="form-container" ><%--style="width: 500px;margin: auto;margin-top: 0;"--%>
                        <h3 style="text-align: center; margin: auto;" id="tipPersoanaID">Sunt persoana:</h3>
                        
                        <div class="box">
                          <input class="Switcher__checkbox sr-only" id="io" type="checkbox" /> <%-- onclick="changeTipPlata(this)"   --%>
                          <label class="Switcher" for="io" style="background-color: #3c8dc5;">
                            <div class="Switcher__trigger" style="color: white;" data-value="Fizica" ></div>
                            <div class="Switcher__trigger" style="color: white;" data-value="Juridica" ></div>
                          </label>
                        </div>

                        <div class="personal-information container-login100-form-btn p-t-10 input100" style="padding-top: 12px;margin-bottom: 10px;background:#dcdbdb !important;">
                             <select id="selectPersoanaFacturareID" data-IdNomPartener="0" style="display: inline-block;margin: inherit; border-radius: 22px; margin-left: 12px;" onchange="changePersoanaFacturare()" >
                                <option value="0">-Selecteaza-</option>
                                <option value="1">Adrian</option>
                                <option value="2">Alin</option>
                                <option value="3" disabled="disabled">Felicia</option>
                                <option value="4">Cristi</option>
                              </select>
					    </div>

                        <div class="wrap-input100 m-b-10" data-validate = "Nume obligatoriu">
						    <input id="txtNume" class="input100" maxlength="64" oninput="this.className = ''"  type="text" placeholder="Nume" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Prenume obligatoriu">
						    <input id="txtPrenume" class="input100" maxlength="64" type="text" placeholder="Prenume" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Email obligatoriu">
						    <input id="txtEmail" class="input100" type="email" maxlength="30"  placeholder="Email" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Adresa obligatorie">
						    <input id="txtAdresa" class="input100" maxlength="50" type="text" placeholder="Adresa" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-building"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Localitatea obligatorie">
						    <input id="txtLocalitatea" class="input100" maxlength="30"  type="text" placeholder="Localitatea" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Tara obligatorie">
						    <input id="txtTara" class="input100" maxlength="32"  type="text" placeholder="Tara" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Telefon obligatoriu">
						    <input id="txtTelefon" class="input100" maxlength="20"  type="text" placeholder="Telefon" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <%--<div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					        <div class="checkbox-container">
						        <input id="checkbox-medium" type="checkbox" onclick="facturaPeFirmaClick(this)" />
						        <div class="checkbox-checkmark"></div>
					        </div>
					        <label for="checkbox-medium">Factura pe firma</label>
				        </div>

                        <div id="divCIFid" class="wrap-input100 validate-input m-b-10" data-validate = "CIF obligatoriu">
                            <input id="txtCUI" class="input100" type="text" placeholder="CIF" />
                            <button class="open-button" type="button" onclick="descarcaDateFirmaClick()" ><img class="open-button"  src="../../Icoane/descarca.png" /></button>
						    <span class="focus-input100"></span>
						    <span class="symbol-input100" onclick="descarcaDateFirmaClick()">
							    <i class="fa fa-pencil-square" onclick="descarcaDateFirmaClick()"></i>
						    </span>
                         </div>

                        <div id="divRCid" class="wrap-input100 validate-input m-b-10" data-validate = "RegCom obligatoriu">
						    <input id="txtRegCom" class="input100" type="text" placeholder="Registru comert" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-pencil-square"></i>
						    </span>
					    </div>

					    <div id="divButtons" class="container-login100-form-btn p-t-10">
						    <button  type="button" class="login100-form-btn" id="btnPlataCash" style="width:49%" onclick="plataCash();" > 
							    Plata cash
						    </button>

                            <button type="button" class="login100-form-btn" style="width:49%" onclick="plataCard()">
							    Plata card
						    </button>
					    </div>--%>

                    </div>
      </div>
    
      <%--<div class="tab">
        <div id="Div1" class="form-container" >
                <h3 style="text-align: center; margin: auto;">Sunt persoana:</h3>
                        <div class="box">
                          <input class="Switcher__checkbox sr-only" id="Checkbox1" type="checkbox" />
                          <label class="Switcher" for="io" style="background-color: #3c8dc5;">
                            <div class="Switcher__trigger" style="color: white;" data-value="Fizica" ></div>
                            <div class="Switcher__trigger" style="color: white;" data-value="Juridica" ></div>
                          </label>
                        </div>

                        <div id="divCIFid" class="wrap-input100 validate-input m-b-10" data-validate = "CIF obligatoriu">
                            <input id="txtCUI" class="input100" type="text" placeholder="CIF" />
                            <button class="open-button" type="button" onclick="descarcaDateFirmaClick()" ><img class="open-button"  src="../../Icoane/descarca.png" /></button>
						    <span class="focus-input100"></span>
						    <span class="symbol-input100" onclick="descarcaDateFirmaClick()">
							    <i class="fa fa-pencil-square" onclick="descarcaDateFirmaClick()"></i>
						    </span>
                         </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Denumire obligatorie">
						    <input id="txtDenumire" class="input100"  type="text" placeholder="Denumire firma" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					     </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Email obligatoriu">
						    <input id="txtEmail2" class="input100" type="email"  placeholder="Email" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Telefon obligatoriu">
						    <input id="txtTelefon2" class="input100" type="text"  placeholder="Telefon" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Adresa">
						    <input id="Text3" class="input100"  type="text" placeholder="Punct de lucru" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-building"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Localitatea obligatorie">
						    <input id="Text4" class="input100"  type="text" placeholder="Localitatea" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Tara obligatorie">
						    <input id="Text5" class="input100"  type="text" placeholder="Tara" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>
                        
                        <div id="divRCid" class="wrap-input100 validate-input m-b-10" data-validate = "RegCom obligatoriu">
						    <input id="txtRegCom" class="input100" type="text" placeholder="Registru comert" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-pencil-square"></i>
						    </span>
					    </div>

                        <div class="personal-information container-login100-form-btn p-t-10 input100" style="padding-top: 12px;margin-bottom: 10px;">
                            <select id="select1" data-IdNomPartener="0" style="display: inline-block;margin: inherit; border-radius: 22px; margin-left: 12px;" onchange="changePersoanaFacturare()" >
                                <option value="0">-Selecteaza-</option>
                                <option value="1">Adrian</option>
                                <option value="2">Alin</option>
                                <option value="3" disabled="disabled">Felicia</option>
                                <option value="4">Cristi</option>
                              </select>
					    </div>

   
                            <div id="div4" class="container-login100-form-btn p-t-10" style="padding: 0;">
			                       <div id="div2" class="wrap-input100 validate-input m-b-10"  style="width:49%" data-validate = "Nume Prenume">
						                <input id="txtNumePrenumeDelagat" class="input100" type="text" placeholder="Nume Prenume" />
						                <span class="focus-input100"></span>
						                <span class="symbol-input100">
							                <i class="fa fa-pencil-square"></i>
						                </span>
					                </div>

                                   <div id="div3" class="wrap-input100 validate-input m-b-10"  style="width:49%" data-validate = "CI Delegat">
						                <input id="txtCIDelegat" class="input100" type="text" placeholder="CI Delegat" />
						                <span class="focus-input100"></span>
						                <span class="symbol-input100">
							                <i class="fa fa-pencil-square"></i>
						                </span>
					                </div>
		                   </div>
                    </div>
      </div>--%>
     
      <div class="tab" id="tabFinal">
          <h3 style="margin:auto;text-align:center;" id="tag6" >Finalizare plata</h3>
         <div id="divButtons" class="container-login100-form-btn p-t-10">
			  <button  type="button" class="login100-form-btn" id="btnPlataCash" style="width:49%" onclick="plataCash();" > 
					 
                  Plata cash
                  
			  </button>

              <button type="button" class="login100-form-btn" id="btnPlataCard" style="width:49%" onclick="plataCard()">
				  <img src="../../Icoane/ok.png" style="width: 32px;height: 32px; margin-right: 12px;" />
                  Plata card
			  </button> 
		 </div>
          <div class="tg-wrap" style="background: white;">

                <table id="tg-rwLtp" class="tg">
                        <tr>
                            <th class="tg-0pky" colspan="2">Furnizor: $[Furnizor]<br>Capital social: $[Capitalsocial]<br>Reg com: $[Regcom]<br>Codul Fiscal: $[Cif]<br>Sediul: $[Sediul]<br>$[Cont]</th>
                            <th class="tg-baqh" colspan="3"><span style="font-weight:700">Seria: $[Seria] Nr. $[Numar]</span><br><span style="font-weight:700">Factura</span><br><span style="font-weight:700">Fiscala</span><br><span style="font-weight:700">Nr. facturii: $[Numar]</span><br><span style="font-weight:700">Data: $[Data]</span><br><span style="font-weight:700">$[PL]</span><br><span style="font-weight:700">$[Storno]</span></th>
                            <th class="tg-0lax" colspan="3">Cumparator: $[Cumparator]<br>Nr. Reg. Com.: $[RegComcumparator]<br>CUI/CNP: $[Cifcumparator]<br>Sediul: $[Sediulcumparator]<br>$[ContCumparator]<br>$[BancaCumparator]</th>
                          </tr>

                         <tr>
                            <td class="tg-fymr">Nr.<br>Crt.</td>
                            <td class="tg-kiyi">Denumirea produselor sau a serviciilor</td>
                            <td class="tg-amwm">Cota <br>TVA<br><br></td>
                            <td class="tg-amwm">U.M.</td>
                            <td class="tg-88nc">Cantitate</td>
                            <td class="tg-amwm">Pret unitar <br>(fara TVA)<br>lei</td>
                            <td class="tg-amwm">Valoare<br>(fara TVA)<br>lei</td>
                            <td class="tg-88nc">Valoare<br>TVA<br>LEI</td>
                          </tr>

                             <!--$[Camere]-->
                <%--  <tr>
                    <td class="tg-0pky">$[t0]</td>
                    <td class="tg-xldj">$[t1]</td>
                    <td class="tg-0lax">$[t2]</td>
                    <td class="tg-0lax">$[t3]</td>
                    <td class="tg-xldj">$[t4]</td>
                    <td class="tg-0lax">$[t5]</td>
                    <td class="tg-0lax">$[t6]</td>
                    <td class="tg-xldj">$[t7]</td>
                  </tr>--%>

                  <tr>
                    <td class="tg-0lax" colspan="8">$[ObsSubsol]<br>Termen de plata $[TermenDePlata] zile<br>Intocmit: $[Intocmit]</td>
                  </tr>
                  <tr>
                    <td class="tg-0lax" rowspan="2">Semnatura si stampila<br>furnizorului<br><br><br><br></td>
                    <td class="tg-0lax" rowspan="2">Data privind expeditia<br>Numele delegatului: $[Delegat]<br>Buletin/cartea de identitate: $[CIDelegat]<br>Mijloc de transport: $[Transport]</td>
                    <td class="tg-0lax" colspan="2">Total LEI</td>
                    <td class="tg-0lax" colspan="2">$[TotalFaraTVA]</td>
                    <td class="tg-0lax" colspan="2">$[TVA]</td>
                  </tr>
                  <tr>
                    <td class="tg-0lax" colspan="2">Semnatura de primire<br><br></td>
                    <td class="tg-0lax" colspan="2">Total de plata LEI</td>
                    <td class="tg-0lax" colspan="2">$[Total]</td>
                  </tr>
                </table>

            </div>
      </div>

  <div style="overflow:auto;margin-top:10px;">
    <div >
      <button type="button" class="btnFooter arrow2" id="prevBtn" onclick="nextPrev(-1)">Inapoi</button>
      <button type="button" class="btnFooter arrow" style="float:right;" id="nextBtn" onclick="nextPrev(1)">Inainte</button>
    </div>
  </div>

  <div style="text-align:center;margin-top:4px;">
    <span class="step"></span>
    <span class="step"></span>
    <span class="step"></span>
  </div>

        <a id="declanseazaModal" href="#open-modal" ></a> <%--style="visibility: hidden"--%> 
        <div id="open-modal" class="modal-window">
                                <div style="border: 1px solid black;">
                                    <a id="closeModal" href="#" title="Close" class="modal-close" >X Inchide</a>
                                    <h1 id="tag7">Firme</h1>
                                    <div id="divCuButoaneID">
                                        <button type="button" class="btnFirma" style="width: 100%;">firma 1</button>
                                    </div>
                                </div>
                </div>
</form>

    <script src="../../Scripts/Login_v12/vendor/jquery/jquery-3.2.1.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/popper.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/select2/select2.min.js"></script>
    <script src="../../Scripts/Login_v12/js/main.js"></script>
    <script type="text/javascript" src="formPlata2.js"></script>
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
    <script src="<%= ResolveUrl("~/Scripts/Customizable-Loading-Modal-Plugin/js/modal-loading.js") %>"></script>
</body>
</html>
