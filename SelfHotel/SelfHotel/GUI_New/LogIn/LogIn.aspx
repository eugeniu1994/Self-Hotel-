<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="SelfHotel.GUI_New.LogIn.LogIn" %>

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
		<div class="container-login100" style="background-image: url('../../Scripts/Login_v12/images/img-01.jpg');">
			<div class="wrap-login100 p-t-190 p-b-30">

				<form class="login100-form validate-form" id="divBodyID">
					<div class="login100-form-avatar">
						<img src="../../Icoane/user.png" alt="AVATAR" />
					</div>

					<span class="login100-form-title p-t-20 p-b-45">
						Log In
					</span>

               		<div class="wrap-input100 validate-input m-b-10" data-validate = "Email obligatoriu">
						<input class="input100" type="email" placeholder="Email" />
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-user"></i>
						</span>
					</div>

					<div class="wrap-input100 validate-input m-b-10" data-validate = "Parola obligatorie">
						<input class="input100" type="password" placeholder="Parola" />
						<span class="focus-input100"></span>
						<span class="symbol-input100">
							<i class="fa fa-lock"></i>
						</span>
					</div>

					<div class="container-login100-form-btn p-t-10">
						<button class="login100-form-btn">
							Login
						</button>
					</div>

					<div class="text-center w-full p-t-25 p-b-230">
						<a href="#" class="txt1">
							
						</a>
					</div>

					<div class="text-center w-full">
						<a class="txt1" href="#" onclick="createNewUser()">
							Creaza cont nou
							<i class="fa fa-long-arrow-right"></i>						
						</a>
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
