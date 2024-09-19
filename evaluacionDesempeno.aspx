<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="evaluacionDesempeno.aspx.vb" Inherits="DNC_2017.evaluacionDesempeno" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Evaluación de Desempeño | SIGIDO</title>
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                              <div id="divInicio" runat="server" class="row">
                                          <p style="text-align:justify">
                                              En Dina estamos comprometidos con el Desarrollo de nuestros colaboradores, as&iacute; como con nuestra filosof&iacute;a de Mejora Continua, 
                                              por lo anterior, como parte de las iniciativas de nuestro programa "Talento Dina", queremos fomentar una cultura de Evaluaci&oacute;n 
                                              con el fin de medir las competencias, comportamientos y resultados relacionados al trabajo, a trav&eacute;s de un procedimiento de 
                                              retroalimentaci&oacute;n estructurado y sistem&aacute;tico. Tu Jefe Directo realiz&aacute;ra la evaluaci&oacute;n de tu desempeño utilizando &eacute;ste formato.
                                          </p>
                              </div>
                        <!--Datos Colaborador-->
                        <div id="divColaborador" runat="server" class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nombre de Colaborador</label>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlColaborador" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>
                                        <img id="loading" src="img/glyphLoading.gif" style="display: none" />
                                    </div>
                                    <!-- /.input group -->
                                </div>
                                <!-- /.form group -->
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">

                                    <asp:Label ID="lblNombreCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                    <asp:Label ID="lblPuestoCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                    <asp:Label ID="lblArea" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                    <asp:Label ID="lblDireccion" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Evaluación de Desempeño</h3>
                                        <div class="box-tools pull-right">

                                            <strong>
                                                <label>Solicitud </label>
                                            </strong>
                                            <asp:Label ID="lblSolicitudes" runat="server" Text="S-1"></asp:Label>
                                        </div>
                                    </div>
                                    <!-- /.box-header -->
                                    <div class="box-body">

                                        <div class="row">
                                            <div class="col-md-6">
                                                <label for="ddlAreaResponsable">Área responsable</label>
                                                <asp:DropDownList ID="ddlAreaResponsable" class="form-control input-sm" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-2">
                                                <label for="txtFolioReferencia" title="Folio referencia">Folio referencia</label>
                                                <asp:TextBox ID="txtFolioReferencia" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Número de Unidades</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-bus"></i></span>
                                                        <asp:TextBox ID="txtNoUnidades" runat="server" ReadOnly="true" class="form-control" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="lnkUnidades" runat="server" ToolTip="Vizualiza Unidades" data-toggle="modal" data-target="#modalNumeroUnidades"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <label for="txtPuesto">Fecha Emisión</label>
                                                <asp:TextBox ID="txtFechaEmision" runat="server" ReadOnly="true" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                    onchange="replaceCaracteres(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label for="txtDescripcionFallaMejora" title="">Descripción de falla o mejora:</label>
                                                <textarea id="txtDescripcionFallaMejora" runat="server" class="form-control" rows="6" onkeypress="return caracteres(event);"
                                                    onchange="replaceCaracteres(this)" maxlength="8000"></textarea>
                                            </div>
                                        </div>
                                        <!--CPV-->
                                        <div class="row">

                                            <div class="col-md-3">
                                                <label for="txtAnioCPV" title="Año CPV">Año CPV</label>
                                                <asp:DropDownList ID="ddlAnio" class="form-control input-sm" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <label for="txtModelo" title="Módelo">Módelo</label>
                                                <asp:DropDownList ID="ddlModelo" class="form-control input-sm" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <label for="txtCliente" title="Cliente">Cliente</label>
                                                <asp:DropDownList ID="ddlCliente" class="form-control input-sm" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <label for="txtCampana" title="Campaña">Campaña</label>
                                                <asp:DropDownList ID="ddlCampanas" class="form-control input-sm" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>

                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">
                                                <label for="txtAccionesRealizar" title="">Acciones a realizar</label>
                                                <textarea id="txtAccionesRealizar" runat="server" class="form-control" rows="6" onkeypress="return caracteres(event);"
                                                    onchange="replaceCaracteres(this)" maxlength="8000"></textarea>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <p class="lead">Gastos Por Unidad</p>
                                            </div>

                                        </div>
                                        <div class="row">
                                        </div>
                                        <!--Gasto-->
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Costo Tiempo de operación</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtCostoOperacion" runat="server" class="form-control" ReadOnly="true" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Vizualiza Operaciones" data-toggle="modal" data-target="#modalTiempoOperacion"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Costo de Materiales</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtCostoMateriales" runat="server" class="form-control" ReadOnly="true" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Vizualiza Materiales" data-toggle="modal" data-target="#modalMateriales"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Mano de obra + Materiales </label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtMOMateriales" runat="server" class="form-control" ReadOnly="true" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <!--Otros-->
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Gastos Adicionales</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtGastosAdicionales" runat="server" ReadOnly="true" class="form-control" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="LinkButton9" runat="server" ToolTip="Vizualiza Gastos" data-toggle="modal" data-target="#modalAdicionales"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Costo Envio</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtGastoEnvio" runat="server" ReadOnly="true" class="form-control" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="LinkButton7" runat="server" ToolTip="Vizualiza Envios" data-toggle="modal" data-target="#modalEnvio"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Viaticos</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtViaticos" runat="server" ReadOnly="true" class="form-control" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <asp:LinkButton ID="LinkButton8" runat="server" ToolTip="Vizualiza Viaticos" data-toggle="modal" data-target="#modalViaticos"><i class="fa fa-external-link"></i></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">


                                            <div class="col-md-4">
                                                <div class="radio" style="text-align: left;">
                                                    <button id="btnImagen" runat="server" type="button" class="btn btn-default btn-xs" data-toggle="modal" data-target="#modalMenu"><i class="fa fa-paperclip"></i>Anexar Evidencia</button>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Costo total del cambio s/IVA</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-dollar"></i></span>
                                                        <asp:TextBox ID="txtTotalCambio" runat="server" ReadOnly="true" class="form-control" onkeypress="return validNumericos(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <!--Estatus del Reporte-->
                                        <div id="divEstatus" runat="server" class="alert alert-success alert-dismissible">
                                            <h4>
                                                <button id="btnAutorizaciones" runat="server" type="button" class="btn btn-default btn-xs pull-left" data-toggle="modal" data-target="#modalFlujoAutorizacion" title="Flujo de Autorización"><i class="fa fa fa-check"></i></button>
                                                <asp:Label ID="lblEstatus" runat="server" Text="Label"></asp:Label>
                                            </h4>

                                            <asp:Label ID="lblEstatusMensaje" runat="server" Text="Label"></asp:Label>
                                        </div>

                                    </div>

                                </div>
                                <!-- ./box-body -->
                                <div class="box-footer">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <button class="btn btn-danger btn-flat pull-left" runat="server" id="btnAutorizar" name="btnAutorizar" onclick="return autorizaSolicitud();">
                                                <i class="fa fa-check"></i>
                                                Autorizar
                                            </button>

                                            <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnNuevoSolicitud" name="btnNuevoSolicitud" onclick="return NuevaSolicitud();">
                                                <i class="fa  fa-plus"></i>
                                                Nueva Solicitud
                                            </button>
                                            <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnCancelar" name="btnCancelar" onclick="return cancelaAFC();">
                                                <i class="fa fa-archive"></i>
                                                Cancelar
                                            </button>

                                            <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnImprimir" name="btnImprimir" onclick="return imprimeReporte();">
                                                <i class="fa fa-print"></i>
                                                Imprimir
                                            </button>
                                            <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnCrear" name="btnCrear" onclick="return validaciones();">
                                                <i class="fa fa-edit"></i>Crear AFC
                                            </button>
                                        </div>

                                        <img id="loadingA" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                    </div>

                                    <br />
                                    <!-- row -->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <!-- The time line -->
                                            <!-- The time line -->
                                            <ul class="timeline" id="lstLineaTiempo" runat="server" style="background-color: #f7f7f7">
                                                <li>
                                                    <div class="timeline-item">

                                                        <h3 class="timeline-header"><a href="#">Comentarios Análisis de Factibilidad de Cambios </a></h3>
                                                        <div class="timeline-body">
                                                            <textarea class="form-control" rows="6" id="txtDescripcion" runat="server" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" maxlength="6000"></textarea>
                                                        </div>
                                                        <div class='timeline-footer'>
                                                            <a class="btn btn-primary btn-xs" id="lnkGuardar" name="lnkGuardar" runat="server" onclick="return guardarCom();">Guardar Comentario</a>
                                                            <img id="loadingG" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        </div>
                                                    </div>


                                                </li>

                                            </ul>
                                        </div>
                                        <!-- /.col -->
                                    </div>



                                </div>
                                <!-- /.box-footer -->
                            </div>
                            <!-- /.box -->
                        </div>

                        <!-- /.col -->
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

</body>
</html>
