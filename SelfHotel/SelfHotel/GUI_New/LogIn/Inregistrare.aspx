﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inregistrare.aspx.cs" Inherits="SelfHotel.GUI_New.Inregistrare.Inregistrare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="../../Scripts/Login_v12/images/icons/favicon.ico"/>
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/fonts/Linearicons-Free-v1.0.0/icon-font.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/animate/animate.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/css-hamburgers/hamburgers.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/vendor/select2/select2.min.js" />
    <link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/css/util.css" />
	<link rel="stylesheet" type="text/css" href="../../Scripts/Login_v12/css/main.css" />
</head>
<body>
    <div class="limiter">
		<div class="container-login100" style="background-image: url('images/img-01.jpg');">
			<div class="wrap-login100 p-t-190 p-b-30">
				<form class="login100-form validate-form" id="divBodyID">

					<span class="login100-form-title p-t-20 p-b-45">
						Email
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Email is required">
						<input class="input100" type="text" name="email" placeholder="email">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Parola
					</span>

					<div class="wrap-input100 validate-input m-b-10" data-validate = "Password is required">
						<input class="input100" type="password" name="parola" placeholder="Parola">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-lock"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Confirmare parola
					</span>
                    <div class="wrap-input100 validate-input m-b-10" data-validate = "Password is required">
						<input class="input100" type="password" name="confirmaParola" placeholder="Confirma parola">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-lock"></i>
						</span>
					</div>

					<span class="login100-form-title p-t-20 p-b-45">
						Nume
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Nume is required">
						<input class="input100" type="text" name="nume" placeholder="Nume">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Prenume
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Prenume is required">
						<input class="input100" type="text" name="prenume" placeholder="Prenume">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>


                    <span class="login100-form-title p-t-20 p-b-45">
						Telefon
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Telefon is required">
						<input class="input100" type="text" name="telefon" placeholder="Telefon">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>
			

                    <span class="login100-form-title p-t-20 p-b-45">
						Tara
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Tara is required">
						<input class="input100" type="text" name="tara" placeholder="Tara">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Judet
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Judet is required">
						<input class="input100" type="text" name="judet" placeholder="Judet">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>
                    
                    <span class="login100-form-title p-t-20 p-b-45">
						Localitate
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Localitate is required">
						<input class="input100" type="text" name="localitate" placeholder="Localitate">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Strada
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Strada is required">
						<input class="input100" type="text" name="strada" placeholder="LocalitateStrada">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Nr.
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Nr is required">
						<input class="input100" type="text" name="nrStrada" placeholder="Nr.">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

				</form>
			</div>
		</div>
	</div>

    <script src="../../Scripts/Login_v12/vendor/jquery/jquery-3.2.1.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/popper.js"></script>
    <script src="../../Scripts/Login_v12/vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Login_v12/vendor/select2/select2.min.js"></script>
    <script src="../../Scripts/Login_v12/js/main.js"></script>
</body>
</html>