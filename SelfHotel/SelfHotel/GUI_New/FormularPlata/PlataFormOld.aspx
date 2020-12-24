<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlataFormOld.aspx.cs" Inherits="SelfHotel.GUI_New.FormularPlata.PlataForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plata</title>
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.1.0.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui.js") %>"></script>
    <link href="../../Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Scripts/startbootstrap-agency-gh-pages/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/googleApisCSS/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href='../../Scripts/googleApisCSS/StyleSheet2.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet3.css' rel='stylesheet' type='text/css' />
    <link href='../../Scripts/googleApisCSS/StyleSheet4.css' rel='stylesheet' type='text/css' />
    <link href="../../Scripts/startbootstrap-agency-gh-pages/css/agency.min.css" rel="stylesheet" />
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
        body {
          padding: 60px 0;
          background-color: rgba(178,209,229,0.7);
          margin: 0 auto;
          /*width: 600px;*/
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
                top: 9px;
                right: 10px;
                width: 33px;
                height: 25px;
                pointer-events: none;
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
                  height: 60px;
                  width: 60px;
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

                table {
                  width: 100%;
                }
                table td, table th {
                  color: #2b686e;
                  padding: 10px;
                }
                table td {
                  text-align: center;
                  vertical-align: middle;
                }
                table td:last-child {
                  font-size: 0.95em;
                  line-height: 1.4;
                  text-align: left;
                }
                table th {
                  background-color: #daeff1;
                  font-weight: 300;
                }
                table tr:nth-child(2n) {
                  background-color: white;
                }
                table tr:nth-child(2n+1) {
                  background-color: #edf7f8;
                }

                @media screen and (max-width: 700px) {
                  table, tr, td {
                    display: block;
                  }

                  td:first-child {
                    position: absolute;
                    top: 50%;
                    -webkit-transform: translateY(-50%);
                            transform: translateY(-50%);
                    width: 100px;
                  }
                  td:not(:first-child) {
                    clear: both;
                    margin-left: 100px;
                    padding: 4px 20px 4px 90px;
                    position: relative;
                    text-align: left;
                  }
                  td:not(:first-child):before {
                    color: #91ced4;
                    content: '';
                    display: block;
                    left: 0;
                    position: absolute;
                  }
                  td:nth-child(2):before {
                    content: 'Camera:';
                  }
                  td:nth-child(3):before {
                    content: 'Adulti:';
                  }
                  td:nth-child(4):before {
                    content: 'Copii:';
                  }
                  td:nth-child(5):before {
                    content: 'Total Plata:';
                  }

                  tr {
                    padding: 10px 0;
                    position: relative;
                  }
                  tr:first-child {
                    display: none;
                  }
                }
                @media screen and (max-width: 500px) {
                  .header {
                    background-color: transparent;
                    color: white;
                    font-size: 2em;
                    font-weight: 700;
                    padding: 0;
                    text-shadow: 2px 2px 0 rgba(0, 0, 0, 0.1);
                  }

                  img {
                    border: 3px solid;
                    border-color: #daeff1;
                    height: 100px;
                    margin: 0.5rem 0;
                    width: 100px;
                  }

                  td:first-child {
                    background-color: #c8e7ea;
                    border-bottom: 1px solid #91ced4;
                    border-radius: 10px 10px 0 0;
                    position: relative;
                    top: 0;
                    -webkit-transform: translateY(0);
                            transform: translateY(0);
                    width: 100%;
                  }
                  td:not(:first-child) {
                    margin: 0;
                    padding: 5px 1em;
                    width: 100%;
                  }
                  td:not(:first-child):before {
                    font-size: .8em;
                    padding-top: 0.3em;
                    position: relative;
                  }
                  td:last-child {
                    padding-bottom: 1rem !important;
                  }

                  tr {
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
                      font-size: 3vmin;
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
                      padding: 1em 5em;
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
</head>
<body >
           <nav class="navbar navbar-expand-lg navbar-dark fixed-top" id="mainNav" style="background-color: #212529;">
              <div class="container">
                <a class="navbar-brand js-scroll-trigger" href="../../Home.aspx"><i class="fa fa-home" aria-hidden="true"></i>Smart Hotel</a>
                <button class="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
                  Meniu
                  <i class="fas fa-bars"></i>
                </button>
                <div class="collapse navbar-collapse" id="navbarResponsive">
                  <ul class="navbar-nav text-uppercase ml-auto">
                    <li class="nav-item">
                      <a class="nav-link js-scroll-trigger" href="../CHECKIN/CheckIn.aspx">Check in</a>
                    </li>
                    <li class="nav-item">
                      <a class="nav-link js-scroll-trigger" href="../REZERVA/Rezervare.aspx">Rezervare</a>
                    </li>
                    <li class="nav-item">
                      <a class="nav-link js-scroll-trigger" href="../CHECKOUT/CheckOut.aspx">Check out</a>
                    </li>
                  </ul>
                </div>
              </div>
            </nav>
             
           <div id="divCuCamereID"     style="background-image: url(../../Style/oras0.jpeg); background-repeat: no-repeat;background-size: cover;">
   
                                    <div id="price" style="margin-top: 40px"> 
                                      
                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox1" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>

                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox2" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>

                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox3" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>

                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox4" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>

                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox5" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>

                                      <div class="plan">
                                        <div class="plan-inner">
                                          <div class="entry-title">
                                            <h3>Camera small</h3>
                                            <div class="price">
                                                <img src="../../Icoane/oras.jpg" style="width: inherit; height: -webkit-fill-available;" alt="Image cam" />
                                            </div>
                                          </div>
                                          <div class="entry-content">
                                            <ul>
                                              <li><strong>1x</strong> Adulti</li>
                                              <li><strong>2x</strong> Copii</li>
                                              <li style="font-size: small;"><strong>3x</strong> Servicii</li>
                                              <li style="font-size: small;"><strong>Free</strong> Servos</li>
                                              <li style="font-size: small;"><strong>Unlimited</strong> Servos</li>
                                            </ul>
                                          </div>
                                          <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					                            <div class="checkbox-container">
						                            <input id="checkbox6" type="checkbox"  />
						                            <div class="checkbox-checkmark"></div>
					                            </div>
					                            <label for="checkbox1">Achita</label>
				                            </div>
                                        </div>
                                      </div>
                         
                                    </div>

                        <div class="box">
                          <input class="Switcher__checkbox sr-only" id="io" type="checkbox" onclick="changeTipPlata(this)" checked="checked"/>
                          <label class="Switcher" for="io" style="background-color: #3c8dc5;">
                            <div class="Switcher__trigger" style="color: white;" data-value="Cash" ></div>
                            <div class="Switcher__trigger" style="color: white;" data-value="Card" ></div>
                          </label>
                        </div>
                        <div class="box"> 
                            <img id="imgTipPlataID" src="../../Icoane/creditCard.gif" title="Plata cu cardul" style="width: 150px;height: auto;margin:auto;" />
                        </div>
                 </div>

	   <form class="login100-form validate-form" id="divBodyID" style="margin-top: 5px !important;">
                   
                    <div class="table-users" style="margin-top: 0;">
                       <div class="header">Camere selectate pentru achitare</div>
   
                       <table cellspacing="0" style="height: 560px;display: inline-block;overflow: auto;">
                          <tr>
                             <th></th>
                             <th>Camera</th>
                             <th>Adulti</th>
                             <th>Copii</th>
                             <th width="230">Total Plata</th>
                          </tr>

                          <tr>
                             <td><img  class="imgTabel" src="http://lorempixel.com/100/100/people/1" alt="" /></td>
                             <td>Jane Doe</td>
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
                </div>

                    <div class="form-container" style="width: 500px;margin: auto;margin-top: 0;">

                        <div class="personal-information container-login100-form-btn p-t-10 input100" style="padding-top: 12px;margin-bottom: 10px;">
                             <h1 >Total de plata: 1500 ron</h1>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Nume obligatoriu">
						    <input class="input100" type="text" placeholder="Nume" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                         <div class="wrap-input100 validate-input m-b-10" data-validate = "Prenume obligatoriu">
						    <input class="input100" type="text" placeholder="Prenume" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>


                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Email obligatoriu">
						    <input class="input100" type="email" placeholder="Email" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-user"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Adresa obligatorie">
						    <input class="input100" type="text" placeholder="Adresa" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-building"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Localitatea obligatorie">
						    <input class="input100" type="text" placeholder="Localitatea" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <div class="wrap-input100 validate-input m-b-10" data-validate = "Tara obligatorie">
						    <input class="input100" type="text" placeholder="Tara" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-institution"></i>
						    </span>
					    </div>

                        <div class="checkbox medium" style="text-align: center;margin-bottom: 10px;">
					        <div class="checkbox-container">
						        <input id="checkbox-medium" type="checkbox" onclick="facturaPeFirmaClick(this)" />
						        <div class="checkbox-checkmark"></div>
					        </div>
					        <label for="checkbox-medium">Factura pe firma</label>
				        </div>


                         <div class="wrap-input100 validate-input m-b-10" data-validate = "CIF obligatoriu">
						    <span class="open-button" onclick="descarcaDateFirmaClick()">
                                <button type="button" onclick="descarcaDateFirmaClick()" ><img onclick="descarcaDateFirmaClick()" src="../../Icoane/descarca.png" /></button>
                            </span>	
                             <input class="input100" type="text" placeholder="CIF" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-pencil-square"></i>
						    </span>
                         </div>

                         <div class="wrap-input100 validate-input m-b-10" data-validate = "RegCom obligatoriu">
						    <input class="input100" type="text" placeholder="Registru comert" />
						    <span class="focus-input100"></span>
						    <span class="symbol-input100">
							    <i class="fa fa-pencil-square"></i>
						    </span>
					    </div>


					    <div class="container-login100-form-btn p-t-10">
						    <button class="login100-form-btn">
							    Plateste
						    </button>
					    </div>

                    </div>

				</form>
                    
    <script src="../../Scripts/startbootstrap-agency-gh-pages/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/jquery/jquery-3.2.1.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/popper.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/select2/select2.min.js"></script>
    <script src="../../Scripts/Login_v12/js/main.js"></script>
    <script type="text/javascript" src="formPlata.js"></script>
</body>
</html>
