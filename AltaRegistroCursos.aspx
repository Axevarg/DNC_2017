<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AltaRegistroCursos.aspx.vb" Inherits="DNC_2017.AltaRegistroCursos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Alta Registro Cursos | SIGIDO</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="plugins/select2/select2.min.css" />
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="plugins/datepicker/datepicker3.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css" />
    <!-- Sub Menu -->
    <link href="bootstrap/css/dropdown-submenu.css" rel="stylesheet" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

    <!-- jQuery 2.2.3 -->
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
    <!-- Bootstrap 3.3.6 -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- Select2 -->
    <script src="plugins/select2/select2.full.min.js"></script>
    <!-- bootstrap datepicker -->
    <script src="plugins/datepicker/bootstrap-datepicker.js"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="plugins/fastclick/fastclick.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/app.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <!--Validaciones Form -->
    <script src="js/validacionesForm.js"></script>

    <script>

        function comportamientosJS() {

        }

    </script>

</head>
<body class="hold-transition skin-black layout-top-nav">
    <form id="form1" runat="server">
        <div class="wrapper">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <header class="main-header">
                <asp:UpdatePanel runat="server" ID="uPMenu" UpdateMode="Conditional">
                    <ContentTemplate>
                        <nav class="navbar navbar-static-top">
                            <div class="container">
                                <div class="navbar-header">
                                    <a href="http://dina.com.mx/" class="navbar-brand">
                                        <img src="img/logo.png" alt="Dina Camiones S.A. de C.V." title="Dina Camiones S.A. de C.V." height="25" />
                                    </a>
                                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar-collapse">
                                        <i class="fa fa-bars"></i>
                                    </button>

                                </div>

                                <!-- Collect the nav links, forms, and other content for toggling -->
                                <!-- Collect the nav links, forms, and other content for toggling -->
                                <!-- Menu Dinamico Bootstrap -->
                                <div class="collapse navbar-collapse pull-left" id="navbar-collapse">
                                    <ul class="nav navbar-nav" id="ulMenu" runat="server">
                                    </ul>
                                </div>
                                <!-- /.navbar-collapse -->
                                <!-- Navbar Right Menu -->
                                <div class="navbar-custom-menu">
                                    <ul class="nav navbar-nav">
                                        <!-- User Account Menu -->
                                        <li class="dropdown user user-menu">
                                            <!-- Menu Toggle Button -->
                                            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                                <!-- The user image in the navbar-->

                                                <!-- hidden-xs hides the username on small devices so only the image appears. -->
                                                <span class="hidden-xs">
                                                    <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></span>
                                            </a>
                                            <ul class="dropdown-menu">
                                                <!-- The user image in the menu -->
                                                <li class="user-header">
                                                    <p>
                                                        <asp:Label ID="lblNombre2" runat="server" Text="Label"></asp:Label>
                                                        <small>
                                                            <asp:Label ID="lblPuesto" runat="server" Text=""></asp:Label></small>
                                                        <small>
                                                            <asp:Label ID="lblDepartamento" runat="server" Text="Label"></asp:Label></small>
                                                        <small>
                                                            <asp:Label ID="lblPerfil" runat="server" Text=""></asp:Label></small>
                                                    </p>
                                                </li>


                                            </ul>
                                        </li>
                                        <!-- Foto Mercader-->
                                        <li>
                                            <a href="http://www.mercader.mx/" class="navbar-brand">
                                                <img src="img/logo_mercader.png" width="65" height="35" /></a>
                                        </li>

                                    </ul>
                                </div>
                                <!-- /.navbar-custom-menu -->
                            </div>
                            <!-- /.container-fluid -->
                        </nav>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </header>
            <!-- Full Width Column -->
            <div class="content-wrapper">
                <div class="container">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <!-- Horizontal Form -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Registro del Cursos <strong>
                                                    <asp:Label ID="lblNombreCurso" runat="server" Text="" for="lblNombreCurso"></asp:Label></strong> </h3>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div role="form">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <strong><i class="fa fa-star-o margin-r-5"></i>Objetivo</strong>
                                                                <p>
                                                                    <asp:Label ID="lblObjetivo" runat="server" Text="" for="lblObjetivo"></asp:Label>
                                                                </p>
                                                                <strong><i class="fa fa-clock-o margin-r-5"></i>Duración</strong>
                                                                <p>
                                                                    <asp:Label ID="lblDuracion" runat="server" Text="" for="lblArea"></asp:Label>
                                                                </p>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row" id="divHorariosSel" runat="server">
                                                        <!-- text input -->
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Horarios</label>
                                                                <asp:DropDownList ID="ddlHorarios" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true"></asp:DropDownList>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <strong><i class="fa fa-users margin-r-5"></i>Cupo</strong>
                                                           <p> <asp:Label ID="lblLugares" runat="server" Text="" for="lblLugares"></asp:Label></p>

                                                          
                                                        </div>
                                                           <div class="col-md-3">
                                                              <strong><i class="fa  fa-male margin-r-5"></i>Disponibles</strong>
                                                              <p> <asp:Label ID="lblLugaresDisponibles" runat="server" Text="" for="lblLugaresDisponibles"></asp:Label></p>

                                                          
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label><i class="fa fa-file-text-o margin-r-5"></i>Comentarios del Curso</label>
                                                                <asp:TextBox ID="txtDescripcion" runat="server" ReadOnly="true" class="form-control" TextMode="MultiLine" Rows="8" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="5000"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="col-md-12" id="divAuto" runat="server">
                                                            <div id="divCursosMensaje" runat="server" class="callout callout-success">

                                                                <h4><i class="icon fa fa-info"></i>Curso</h4>
                                                                <asp:Label ID="lblCurso" runat="server" Text="Label"></asp:Label>

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">

                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnAgregar" name="btnAgregar" onclick="return insCurso();">
                                                    <i class="fa fa-calendar"></i>
                                                    Agendar Curso
                                                </button>
                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                                <asp:HiddenField ID="hdEstatusCurso" runat="server" />
                            <asp:HiddenField ID="hdComentarios" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />

                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <!-- /.box -->
                    </section>
                    <!-- /.content -->
                </div>
                <!-- /.container -->
            </div>
            <!-- /.content-wrapper -->
            <footer class="main-footer">
                <div class="container">
                    <div class="pull-left hidden-xs">
                        <strong>Sistema de Gestión Integral de Desarrollo Organizacional</strong> | <a href="#">SIGIDO</a> | <small>Desarrollado por TI DINA</small>
                    </div>
                    <div class="pull-right hidden-xs">
                        <b>Version</b> 1.0
                    </div>
                </div>
                <!-- /.container -->
            </footer>
        </div>
        <!-- ./wrapper -->
    </form>

    <script>


        function insCurso() {

            var ddlHorarios = document.getElementById("ddlHorarios");
            if (ddlHorarios.options[ddlHorarios.selectedIndex].value == 0) {
                alert("Debe de Seleccionar el Horario.");
                ddlHorarios.focus();
                return false;
            }


            if (!confirm('Al Aceptar no se podrá realizar cambio de Horario del Curso, ¿Desea Continuar?')) { return false; };



            var theControl = document.getElementById("btnAgregar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;

        }
    </script>
</body>
</html>
