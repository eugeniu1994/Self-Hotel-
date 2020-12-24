<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetareParola.aspx.cs" Inherits="SelfHotel.GUI_New.ResetareParola.ResetareParola" %>

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
						<input class="input100" type="text" name="email" placeholder="Email">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						<input class="input100" type="button" name="email" value="Trimite cod" onclick="TrimiteCod()">
					</span>

					<span class="login100-form-title p-t-20 p-b-45">
						Cod securitate
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Username is required">
						<input class="input100" type="text" name="codSecuritate" placeholder="codSecuritate">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

                    <span class="login100-form-title p-t-20 p-b-45">
						Parola noua
					</span>
					<div class="wrap-input100 validate-input m-b-10" data-validate = "Password is required">
						<input class="input100" type="password" name="parolaNoua">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-lock"></i>
						</span>
					</div>

                     <span class="login100-form-title p-t-20 p-b-45">
						Confirmare parola
					</span>
                    <div class="wrap-input100 validate-input m-b-10" data-validate = "Password is required">
						<input class="input100" type="password" name="confirmareParola">
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-lock"></i>
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
