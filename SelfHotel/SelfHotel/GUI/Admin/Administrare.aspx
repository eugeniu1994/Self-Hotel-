<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrare.aspx.cs" Inherits="SelfHotel.GUI.Admin.Administrare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrare</title>
    <%--<link href="http://localhost:31363/maxcdn.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
--%>


    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    
    <style>
        .row.heading h2 {
    color: #fff;
    font-size: 52.52px;
    line-height: 95px;
    font-weight: 400;
    text-align: center;
    margin: 0 0 40px;
    padding-bottom: 20px;
    text-transform: uppercase;
}
ul{
  margin:0;
  padding:0;
  list-style:none;
}
.heading.heading-icon {
    display: block;
}
.padding-lg {
	display: block;
	padding-top: 60px;
	padding-bottom: 60px;
}
.practice-area.padding-lg {
    padding-bottom: 55px;
    padding-top: 55px;
}
.practice-area .inner{ 
     border:1px solid #999999; 
	 text-align:center; 
	 margin-bottom:28px; 
	 padding:40px 25px;
}
.our-webcoderskull .cnt-block:hover {
    box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
    border: 0;
}
.practice-area .inner h3{ 
    color:#3c3c3c; 
	font-size:24px; 
	font-weight:500;
	font-family: 'Poppins', sans-serif;
	padding: 10px 0;
}
.practice-area .inner p{ 
    font-size:14px; 
	line-height:22px; 
	font-weight:400;
}
.practice-area .inner img{
	display:inline-block;
}


.our-webcoderskull{
  /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
  
}
        img {
            margin: auto;
        }
.our-webcoderskull .cnt-block{ 
   float:left; 
   width:100%; 
   background:#fff; 
   padding:30px 20px; 
   text-align:center; 
   border:2px solid #d5d5d5;
   margin: 0 0 28px;
}
.our-webcoderskull .cnt-block figure{
   width:148px; 
   height:148px; 
   border-radius:100%; 
   display:inline-block;
   margin-bottom: 15px;
}
.our-webcoderskull .cnt-block img{ 
   width:148px; 
   height:148px; 
   border-radius:100%; 
}
.our-webcoderskull .cnt-block h3{ 
   color:#2a2a2a; 
   font-size:20px; 
   font-weight:500; 
   padding:6px 0;
   text-transform:uppercase;
}
.our-webcoderskull .cnt-block h3 a{
  text-decoration:none;
	color:#2a2a2a;
}
.our-webcoderskull .cnt-block h3 a:hover{
	color:#337ab7;
}
.our-webcoderskull .cnt-block p{ 
   color:#2a2a2a; 
   font-size:13px; 
   line-height:20px; 
   font-weight:400;
}
.our-webcoderskull .cnt-block .follow-us{
	margin:-1px 0 0;
}
.our-webcoderskull .cnt-block .follow-us li{ 
    display:inline-block; 
	width:auto; 
	margin:0 5px;
}
.our-webcoderskull .cnt-block .follow-us li .fa{ 
   font-size:24px; 
   color:#767676;
}
.our-webcoderskull .cnt-block .follow-us li .fa:hover{ 
   color:#025a8e;
}

    </style>
</head>
<body>
   
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Selectati firma"></asp:Label>
            <asp:DropDownList ID="selectFirmaID" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Selectati Hotelul"></asp:Label>
            <asp:DropDownList ID="selectIdHotel" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnSalveazaFirmasiHotel" runat="server" OnClick="btnSalveazaFirmasiHotel_Click" Text="Salveaza Firma si Hotel" />

            <br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="AccountKey"></asp:Label>
            <asp:TextBox ID="txtAccountKey" runat="server" Width="326px"></asp:TextBox>
            <asp:Label ID="Label5" runat="server" Text="Admin mail"></asp:Label>
            <asp:TextBox ID="txtMailAdmin" runat="server" Width="398px"></asp:TextBox>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <asp:Label ID="Label4" runat="server" Text="MerchantID"></asp:Label>
            <asp:TextBox ID="txtMerchantID" runat="server" Width="329px"></asp:TextBox>

            <asp:Label ID="Label6" runat="server" Text="Observtii factura"></asp:Label>
            <asp:TextBox ID="txtObsFactura" runat="server" Width="359px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnEuPlatesc" runat="server" OnClick="btnEuPlatesc_Click" Text="Salveaza date" Width="522px" />
            <br />

        <br />
        <hr />
  
        </div>
        
   <%--<div class="our-webcoderskull padding-lg">
  <div class="container">
    <ul class="row">
      <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <img src="../../Icoane/Screenshot_1.png" class="img-responsive" alt="" />
            <h3><a href="#">Tip Camera</a></h3>
            <p>Incarcati poza pentru acest tip de camera</p>
            <ul class="follow-us clearfix">
              <li><asp:FileUpload ID="FileUpload1" runat="server" /></li>
            </ul>
            <ul class="follow-us clearfix">
              <li><button>Trimite</button></li>
            </ul>
          </div>
      </li>


      <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <img src="../../Icoane/128_Handshake_circle_blue.png" class="img-responsive" alt="" />
            <h3><a href="#">Tip Camera </a></h3>
            <p>Id Camera</p>
            <ul class="follow-us clearfix">
              <li><button>Incarca</button></li>
              <li><button>Trimite</button></li>
              <li><button>Vizualizeaza</button></li>
            </ul>
          </div>
      </li>

      <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <figure><img src="http://www.webcoderskull.com/img/team4.png" class="img-responsive" alt=""></figure>
            <h3><a href="http://www.webcoderskull.com/">Manish </a></h3>
            <p>Freelance Web Developer</p>
            <ul class="follow-us clearfix">
              <li><a href="#"><i class="fa fa-facebook" aria-hidden="true"></i></a></li>
              <li><a href="#"><i class="fa fa-twitter" aria-hidden="true"></i></a></li>
              <li><a href="#"><i class="fa fa-linkedin" aria-hidden="true"></i></a></li>
            </ul>
          </div>
       </li>
      <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <figure><img src="http://www.webcoderskull.com/img/team2.png" class="img-responsive" alt=""></figure>
            <h3><a href="http://www.webcoderskull.com/">Atul </a></h3>
            <p>Freelance Web Developer</p>
            <ul class="follow-us clearfix">
              <li><a href="#"><i class="fa fa-facebook" aria-hidden="true"></i></a></li>
              <li><a href="#"><i class="fa fa-twitter" aria-hidden="true"></i></a></li>
              <li><a href="#"><i class="fa fa-linkedin" aria-hidden="true"></i></a></li>
            </ul>
          </div>
      </li>

          <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <figure><img src="../../Icoane/Screenshot_1.png" class="img-responsive" alt="" /></figure>
            <h3><a href="#">Tip Camera</a></h3>
            <p>Id Camera</p>
            <ul class="follow-us clearfix">
              <li><button>Incarca</button></li>
              <li><button>Trimite</button></li>
              <li><button>Vizualizeaza</button></li>
            </ul>
          </div>
      </li>


      <li class="col-12 col-md-6 col-lg-3">
          <div class="cnt-block equal-hight" style="height: 349px;">
            <figure><img src="../../Icoane/128_Handshake_circle_blue.png" class="img-responsive" alt="" /></figure>
            <h3><a href="#">Tip Camera </a></h3>
            <p>Id Camera</p>
            <ul class="follow-us clearfix">
              <li><button>Incarca</button></li>
              <li><button>Trimite</button></li>
              <li><button>Vizualizeaza</button></li>
            </ul>
          </div>
      </li>
    </ul>
  </div>
</div>--%>
    </form>

 
        

    

    <script type="text/javascript" src="administrare.js"></script>
</body>
</html>
