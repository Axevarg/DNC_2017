<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cursos.aspx.vb" Inherits="DNC_2017.cursos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Cursos | SIGIDO</title>
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
        function combo() {

            $("#ddlCursos").select2();
        }
        function AbrirModalMensaje() {
            $("#<%=modalMensaje.ClientID%>").modal("show");

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
                        <nav class="navbar navbar-fixed-top">
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
                <br /><br />
                <div class="container">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Gestión
                            <asp:Label ID="lblDNC" runat="server" Text=""></asp:Label>

                            <small>Cursos</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                                        <!-- Modal -->
                                <div class="modal fade bs-example-modal-lg modal-warning" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalMensaje" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalLabel24">Cursos que no se pueden actualizar</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                  <div id="divMensaje" runat="server" class="col-md-12">
                                                        <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="White"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-warning pull-left" data-dismiss="modal">Cerrar</button>
                                          </div>
                                        </div>

                                    </div>
                                </div>
                                <!--Fin  Modal -->


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
                                                <h3 class="box-title">Administración de Cursos</h3>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div role="form">


                                                    <!-- Modal -->
                                                    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalMenu" runat="server">
                                                        <div class="modal-dialog modal-lg" role="document">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                    <h4 class="modal-title" id="myModalLabel2">Administración de Temario</h4>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div id="div3" runat="server" class="col-md-12 center-block">
                                                                            <output id="listArchivo" runat="server"></output>
                                                                        </div>
                                                                        <div class="col-md-12">

                                                                            <div class="form-group">

                                                                                <label for="exampleInputFile"><i class="fa fa-paperclip"></i>Cargar PDF del Temario</label>
                                                                                <asp:FileUpload ID="fucArchivo" runat="server" accept="application/pdf, image/*" Width="100%" />

                                                                                <p class="help-block">Tipo de archivo permitido: '.pdf'</p>
                                                                            </div>
                                                                            <button id="btnCargaArchivos" runat="server" class="btn btn-danger btn-flat pull-right"
                                                                                name="btnCargaArchivos" onclick="return fn_CargarArchivoMenu();">
                                                                                <i class="fa fa-upload"></i>Subir Archivo</button>
                                                                        </div>
                                                                        <div class="col-md-12">

                                                                            <iframe id="iFramePdf" runat="server" frameborder="0" width="860" height="350" marginheight="0" marginwidth="0"></iframe>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblErrorArchivo" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>DNC</label>
                                                                <asp:DropDownList ID="ddlDnc" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarCurso();"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <img id="imgCurso" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        </div>
                                                    </div>
                                                    <!--Fin  Modal -->
                                                    <div class="col-md-12" id="divCursos" runat="server">
                                                        <div class="form-group">
                                                            <label>Busqueda de Cursos</label>
                                                            <asp:DropDownList ID="ddlCursos" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarCurso();">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="row" id="divDatosCursos" runat="server">
                                                        <!-- text input -->
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Proveedor</label>
                                                                <asp:DropDownList ID="ddlProveedor" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Facilitador</label>
                                                                <asp:DropDownList ID="ddlFacilitador" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Descripción Capacitación Corta</label>
                                                                <asp:TextBox ID="txtDescripcionCorta" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="150"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Descripción Capacitación Larga</label>
                                                                <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" TextMode="MultiLine" Rows="2" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Objetivo</label>

                                                                <asp:TextBox ID="txtObjetivo" runat="server" class="form-control" TextMode="MultiLine" Rows="2" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Habilidades a Desarrollar </label>
                                                                <asp:TextBox ID="txtHabilidadesDesarrollar" runat="server" class="form-control" TextMode="MultiLine" Rows="2" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="1000"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label>Habilidad DINA </label>
                                                                <asp:DropDownList ID="ddlHabilidadDina" runat="server" class="form-control" Style="width: 100%;">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label>Modalidad </label>
                                                                <asp:DropDownList ID="ddlModalidad" runat="server" class="form-control" Style="width: 100%;">
                                                           
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label>Moneda </label>
                                                                <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control" Style="width: 100%;">
                                                                    <asp:ListItem>Pesos</asp:ListItem>
                                                                    <asp:ListItem>Dolares</asp:ListItem>
                                                                    <asp:ListItem>Euros</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>Evaluación</label>
                                                                <asp:DropDownList ID="ddlEvaluacion" runat="server" class="form-control" Style="width: 100%;">
                                                                    <asp:ListItem>Si</asp:ListItem>
                                                                    <asp:ListItem>No</asp:ListItem>
                                                                          <asp:ListItem>N/A</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>Constancia</label>
                                                                <asp:DropDownList ID="ddlConstancia" runat="server" class="form-control" Style="width: 100%;">
                                                                    <asp:ListItem>Si</asp:ListItem>
                                                                    <asp:ListItem>No</asp:ListItem>
                                                                         <asp:ListItem>N/A</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>DC3 </label>
                                                                <asp:DropDownList ID="ddlDC3" runat="server" class="form-control" Style="width: 100%;">
                                                                    <asp:ListItem>Si</asp:ListItem>
                                                                    <asp:ListItem>No</asp:ListItem>
                                                                         <asp:ListItem>N/A</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>No. Máx. Participantes x Grupo</label>
                                                                <asp:TextBox ID="txtNoMaximoParticipante" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4" oninput="costoxHora();"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>Duración Horas </label>
                                                                <asp:TextBox ID="txtDuracionHoras" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="3" oninput="costoxHora();"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>Costo Individual (Sin IVA)</label>
                                                                <asp:TextBox ID="txtCostoIndividual" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" oninput="costoxHora();" MaxLength="14"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>Costo Por Grupo (Sin IVA)</label>
                                                                <asp:TextBox ID="txtCostoGrupo" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="form-group">
                                                                <label>IVA</label>
                                                                <asp:TextBox ID="txtIVA" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Fuente</label>
                                                                <asp:TextBox ID="txtFuente" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">

                                                            <button id="btnImagen" runat="server" type="button" class="btn btn-default btn-xs" data-toggle="modal" data-target="#modalMenu"><i class="fa fa-paperclip"></i>Adjuntar Temario</button>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            &nbsp;
                                                        </div>
                                                        <br />
                                                        <div class="col-md-12" id="divAuto" runat="server">
                                                            <div id="divCurosMensaje" runat="server" class="callout callout-success">

                                                                <h4><i class="icon fa fa-info"></i>Curso</h4>
                                                                <asp:Label ID="lblCurso" runat="server" Text="Label"></asp:Label>

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnEliminar" name="btnEliminar" onclick="return eliminaCurso();">
                                                    <i class="fa fa-archive"></i>
                                                    Eliminar
                                                </button>
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnActualizar" name="btnActualizar" onclick="return updCurso();">
                                                    <i class="fa fa-edit"></i>
                                                    Actualizar
                                                </button>

                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnNuevoCurso" name="btnNuevoCurso" onclick="return nuevoCurso();">
                                                    <i class="fa  fa-plus"></i>
                                                    Agregar Nuevo Curso
                                                </button>
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnAgregar" name="btnAgregar" onclick="return insCursos();">
                                                    <i class="fa  fa-plus"></i>
                                                    Agregar
                                                </button>
                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCargaArchivos" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdIdDNC" runat="server" />
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

        //VALIDACION DE INSERCION
        function validaciones() {

            var selects = document.getElementById("ddlFacilitador");
            var selectedValue = selects.options[selects.selectedIndex].value;
            //Facilitador
            if (selectedValue == 0) {
                alert("Debe de agregar un Facilitador al Proveedor.");
                return false;
            }

            //Descripcion COrta
            if (document.getElementById("txtDescripcionCorta").value == '') {
                alert("Debe capturar la descripción corta.");
                document.getElementById("txtDescripcionCorta").focus();
                return false;
            }

            //Descripcion
            if (document.getElementById("txtDescripcion").value == '') {
                alert("Debe capturar la descripción.");
                document.getElementById("txtDescripcion").focus();
                return false;
            }
            //Objetivo
            if (document.getElementById("txtObjetivo").value == '') {
                alert("Debe capturar el Objetivo. ");
                document.getElementById("txtObjetivo").focus();
                return false;
            }
            //Habilidades A Desarrollar
            if (document.getElementById("txtHabilidadesDesarrollar").value == '') {
                alert("Debe capturar las habilidades a Desarrollar");
                document.getElementById("txtHabilidadesDesarrollar").focus();
                return false;
            }
            //No. Máximo Participantes por Grupo
            if (document.getElementById("txtNoMaximoParticipante").value == '') {
                alert("Debe capturar el No. Máximo Participantes por Grupo.");
                document.getElementById("txtNoMaximoParticipante").focus();
                return false;
            }
            //Duración Horas
            if (document.getElementById("txtDuracionHoras").value == '') {
                alert("Debe capturar la Duración Horas.");
                document.getElementById("txtDuracionHoras").focus();
                return false;
            }
            //Costo Individual (Sin IVA)
            if (document.getElementById("txtCostoIndividual").value == '') {
                alert("Debe capturar el Costo Individual (Sin IVA).");
                document.getElementById("txtCostoIndividual").focus();
                return false;
            }

            //Costo Por Grupo (Sin IVA)
            if (document.getElementById("txtCostoGrupo").value == '') {
                alert("Debe capturar el Costo por Grupo.");
                document.getElementById("txtCostoGrupo").focus();
                return false;
            }

            //IVA
            if (document.getElementById("txtIVA").value == '') {
                alert("Debe capturar el IVA.");
                document.getElementById("txtIVA").focus();
                return false;
            }

            return true;

        }

        function insCursos() {


            if (validaciones() == false) {
                return false;
            }
            var theControl = document.getElementById("btnAgregar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }


        function cargarCurso() {

            var theImg = document.getElementById("imgCurso");
            theImg.style.display = "inline";

        }
        function costoxHora() {

        }
        function updCurso() {


            if (validaciones() == false) {
                return false;
            }
            var theControl = document.getElementById("btnActualizar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }

        // elimina Curso
        function eliminaCurso() {
            if (!confirm('¿Desea Eliminar Curso?')) { return false; };
            return true;
        }

        function nuevoCurso() {
            var theControl = document.getElementById("btnNuevoCurso");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }

        function fn_CargarArchivoMenu() {
            var validFilesTypes = ["pdf"];
            var varArchivo = document.getElementById("fucArchivo");

            if (varArchivo.value == '') {
                alert('No ha seleccionado ningún archivo para subir.');
                varArchivo.focus();
                return false;
            }

            var path = varArchivo.value;
            var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
            var isValidFile = false;
            //ciclo para validar extencion
            for (var i = 0; i < validFilesTypes.length; i++) {
                if (ext == validFilesTypes[i]) {
                    isValidFile = true;
                    break;
                }
            }

            if (!isValidFile) {

                alert('Tipo de archivo incorrecto. Favor de seleccionar un archivo con la Extensión ' + validFilesTypes.join(", "));
                varArchivo.focus();
                return false;
            }

            if (!confirm('Al Aceptar el Temario vigente se remplazara por el actual.')) { return false; };

            return true;
        }
    </script>
</body>
</html>
