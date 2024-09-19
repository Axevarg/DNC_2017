<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestionCapacitacion.aspx.vb" Inherits="DNC_2017.GestionCapacitacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Gestión de Capacitación</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
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
    <!-- Bootstrap time Picker -->
    <link href="plugins/timepicker/bootstrap-timepicker.min.css" rel="stylesheet" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="js/gridviewScroll.min.js"></script>

    <script>


        $(document).ready(function () {

            // gridviewScroll();

            $(window).resize(function () {
                //     gridviewScroll();

            });
        });
        //Grid Cursos
        function gridviewScroll() {
            gridView1 = $('#grdColaboradoresGestion').gridviewScroll({
                width: 'auto',
                height: 300,
                headerrowcount: 1,

                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }


            });

            }
            function ComportamientosJS() {
                $("#ddlConsultaCursosGestionados").select2();
                $("#ddlCursoAutorizadoDNC").select2();
                $("#ddlProveedor").select2();
                $("#ddlFacilitador").select2();
                $("#ddlCursoGestionadoRelacionado").select2();
                $("#ddlClaveCursoDc3").select2();
                $("#ddlClaveTematicaDc3").select2();
                $("#ddlTipoAgenteDc3").select2();
                $("#ddlClaveModalidadDc").select2();
                $("#ddlClaveCapacitacionDc3").select2();
                $("#ddlClaveEstablecimiento").select2();
              //  $("#ddlOcupacionDC3").select2();
                $("#ddlPagoRelacionado").select2();
                

                $("#<%=grdColaboradoresGestion.ClientID%> select[id*='ddlColaborador']").select2();

                $("#<%=grdColaboradoresGestion.ClientID%> select[id*='ddlAgreColaborador']").select2();

                //Fechas
                $('#txtFechaApertura').datepicker({
                    autoclose: true
                });

                $('#txtFechaCierre').datepicker({
                    autoclose: true
                });

                $(".timepicker").timepicker({
                    showInputs: false,
                    minuteStep: 5,
                    showMeridian: false,
                    defaultTime: false,
                    showMeridian: false,
                    format: 'H:mm'
                });

                /*Formato de TImerPicker*/
                $("#txtHorarioMDesde").timepicker({
                    showInputs: false,
                    minuteStep: 5,
                    showMeridian: false,
                    defaultTime: false,
                    showMeridian: false,
                    format: 'H:mm'
                });

                $("#txtHorarioMHasta").timepicker({
                    showInputs: false,
                    minuteStep: 5,
                    showMeridian: false,
                    defaultTime: false,
                    showMeridian: false,
                    format: 'H:mm'
                });
                $("#txtHorarioTDesde").timepicker({
                    showInputs: false,
                    minuteStep: 5,
                    showMeridian: false,
                    defaultTime: false,
                    showMeridian: false,
                    format: 'H:mm'
                });
                $("#txtHorarioTHasta").timepicker({
                    showInputs: false,
                    minuteStep: 5,
                    showMeridian: false,
                    defaultTime: false,
                    showMeridian: false,
                    format: 'H:mm'
                });
            }

        function cargarColaborador() {
            var theImg = document.getElementById("imgColaborador");
            theImg.style.display = "inline";
        }
            function cargarCurso() {

                var theImg = document.getElementById("imgCurso");
                theImg.style.display = "inline";

            }
            function costoxHora() {

            }

            // Valida Plan
            function ValidaGridColaboradoresAsistencia() {
                var valid = false;

                if (document.getElementById('<%= hdColaboradores.ClientID%>').value == "1") {
                    valid = true;
                }

                //Valida si hay colaboradores seleccioonados
                if (valid == false) {
                    alert("Por favor, seleccione al menos un Colaborador.");
                    return false;
                }

                if (!confirm('Al Aceptar se generarán la lista de asistencia.')) { return false; };
                return true;
            }

            function ValidaGridColaboradoresDc3() {
                var valid = false;

                if (document.getElementById('<%= hdColaboradores.ClientID%>').value == "1") {
                valid = true;
            }

            //Valida si hay colaboradores seleccioonados
            if (valid == false) {
                alert("Por favor, seleccione al menos un Colaborador.");
                return false;
            }

            if (!confirm('Al Aceptar se generarán la DC3 de las personas marcadas.')) { return false; };
            return true;
        }

        function Check_Click(objRef) {
            document.getElementById('<%= hdColaboradores.ClientID%>').value = "0";
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "#00c0ef";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "white";
                }
                else {
                    row.style.backgroundColor = "#F2F2F2";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //El primer elemento es la casilla de verificación de encabezado
                var headerCheckBox = inputList[0];

                // Ciclo para indicar si hay casillas activas de ser asi marca uno
                if (inputList[i].type == "checkbox") {
                    if (!inputList[i].checked) {
                        document.getElementById('<%= hdColaboradores.ClientID%>').value = "1";
                    }
                }
                //Basado en todas o ninguna casilla de verificación
                // están marcados marcar / desmarcar encabezado casilla de verificación
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        document.getElementById('<%= hdColaboradores.ClientID%>').value = "1";
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
    </script>


    <!-- jQuery 2.2.3 
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>-->



    <!-- Bootstrap 3.3.6 -->
    <script src="js/bootstrap.min.js"></script>
    <!-- Select2 -->
    <script src="plugins/select2/select2.full.min.js"></script>
    <!-- bootstrap datepicker -->
    <script src="plugins/datepicker/bootstrap-datepicker.js"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <!-- bootstrap time picker -->
    <script src="plugins/timepicker/bootstrap-timepicker.min.js" type="text/javascript"></script>
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
                <br />
                <br />
                <div class="container">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Gestión de Capacitación    
                            <small>SIGIDO</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                             <!-- Modal -->
                                <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalCargarPuesto" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalLabel2">Cargar LayOut de Colaboradores</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">

                                                        <div class="form-group">
                                                            <label for="exampleInputFile"><i class="fa fa-paperclip"></i>Cargar LayOut de Colaboradores</label>
                                                            <asp:FileUpload ID="fuExcel" runat="server" Width="100%" />
                                                            <p class="help-block">Tipo de archivo permitido:  .xls, .xlsx</p>
                                                        </div>
                                                    </div>

                                                    <div id="divMensaje" runat="server" class="col-md-12">
                                                        <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="Green"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-default pull-left bg-red" data-dismiss="modal">Cerrar</button>
                                                <button id="btnCargaColaboradores" runat="server" class="btn margin pull-right bg-red"
                                                    name="btnCargaColaboradores" onclick="return CargarExcel();">
                                                    <i class="fa fa-upload"></i>Cargar</button>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--Fin  Modal -->

                                <div class="col-md-12">
                                    <img id="imgCurso" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                </div>

                                <div class="row">
                                    <div class="col-md-4" id="div3" runat="server">
                                        <div class="form-group">
                                            <label>DNC</label>
                                            <asp:DropDownList ID="ddlDnc" runat="server" class="form-control input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarCurso();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-8" id="divCursos" runat="server">
                                        <div class="form-group">
                                            <label>Consultar de Cursos Gestionados</label>
                                            <asp:DropDownList ID="ddlConsultaCursosGestionados" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarCurso();">
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                </div>

                                <!--Catalogos Gestion de Capacitación-->
                                <div class="row" id="divCursosGestion" runat="server">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Gestionar Cursos de Capacitación</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorGestionCap" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-12" id="div22" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="lnkTabGestion1" runat="server" class="active"><a href="#tabGestion_1" data-toggle="tab" onclick="controlTabsGestion(1);">Datos Generales</a></li>
                                                                <li id="lnkTabGestion2" runat="server"><a href="#tabGestion_2" data-toggle="tab" onclick="controlTabsGestion(2);">Datos de la Secretaria del Trabajo </a></li>
                                                                <li id="lnkTabGestion3" runat="server"><a href="#tabGestion_3" data-toggle="tab" onclick="controlTabsGestion(3);">Colaboradores participantes </a></li>

                                                            </ul>

                                                            <div class="tab-content">
                                                                <!-- Gestión de Capacitación -->
                                                                <div class="tab-pane active" id="tabGestion_1" runat="server">
                                                                    <div class="row" id="divDatosCursos" runat="server">
                                                                        <!-- text input -->
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">
                                                                                <label>¿Es Curso de la DNC?</label>
                                                                                <asp:DropDownList ID="ddlEsDNC" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarCurso();">
                                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Sí</asp:ListItem>
                                                                                </asp:DropDownList>

                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-9">
                                                                            <div class="form-group">
                                                                                <label id="lblCurso" runat="server"> </label>
                                                                                <asp:DropDownList ID="ddlCursoAutorizadoDNC" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarCurso();"></asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Proveedor</label>
                                                                                <asp:DropDownList ID="ddlProveedor" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarCurso();"></asp:DropDownList>
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
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Habilidad DINA </label>
                                                                                <asp:DropDownList ID="ddlHabilidadDina" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Modalidad </label>
                                                                                <asp:DropDownList ID="ddlModalidad" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>



                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Moneda </label>
                                                                                <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control" Style="width: 100%;">
                                                                                    <asp:ListItem>Pesos</asp:ListItem>
                                                                                    <asp:ListItem>Dolares</asp:ListItem>
                                                                                    <asp:ListItem>Euros</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Evaluación</label>
                                                                                <asp:DropDownList ID="ddlEvaluacion" runat="server" class="form-control" Style="width: 100%;">
                                                                                    <asp:ListItem>Si</asp:ListItem>
                                                                                    <asp:ListItem>No</asp:ListItem>
                                                                                    <asp:ListItem>N/A</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Constancia</label>
                                                                                <asp:DropDownList ID="ddlConstancia" runat="server" class="form-control" Style="width: 100%;">
                                                                                    <asp:ListItem>Si</asp:ListItem>
                                                                                    <asp:ListItem>No</asp:ListItem>
                                                                                    <asp:ListItem>N/A</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>DC3 </label>
                                                                                <asp:DropDownList ID="ddlDC3" runat="server" class="form-control" Style="width: 100%;">
                                                                                    <asp:ListItem Value="Si">Si</asp:ListItem>
                                                                                    <asp:ListItem Value="No">No</asp:ListItem>
                                                                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>No. Máx. Participantes</label>
                                                                                <asp:TextBox ID="txtNoMaximoParticipante" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4" oninput="costoxHora();" ReadOnly="true"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Duración Horas </label>
                                                                                <asp:TextBox ID="txtDuracionHoras" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="3" oninput="costoxHora();"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Costo Individual (SN IVA)</label>
                                                                                <asp:TextBox ID="txtCostoIndividual" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" oninput="costoxHora();" MaxLength="14"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Costo Total (SN IVA)</label>
                                                                                <asp:TextBox ID="txtCostoGrupo" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>IVA</label>
                                                                                <asp:TextBox ID="txtIVA" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Costo Final</label>
                                                                                <asp:TextBox ID="txtCostoTotal" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="14"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-2">
                                                                            <label for="txtFechaApertura" title="Fecha Emisión Desde">Fecha Apertura</label>
                                                                            <asp:TextBox ID="txtFechaApertura" runat="server" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                                                onchange="replaceCaracteres(this)"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <label for="txtFechaCierre" title="Fecha Emisión Hasta">Fecha Cierre</label>
                                                                            <asp:TextBox ID="txtFechaCierre" runat="server" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                                                onchange="replaceCaracteres(this)"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="bootstrap-timepicker">
                                                                                <div class="form-group">
                                                                                    <label for="txtHoraRetiro">Horario Mañana Desde</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtHorarioMDesde" class="form-control timepicker" runat="server" MaxLength="5" />
                                                                                        <div class="input-group-addon">
                                                                                            <i class="fa fa-clock-o"></i>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- /.form group -->
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="bootstrap-timepicker">
                                                                                <div class="form-group">
                                                                                    <label for="txtHoraRetiro">Horario Mañana Hasta</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtHorarioMHasta" class="form-control timepicker" runat="server" MaxLength="5" />
                                                                                        <div class="input-group-addon">
                                                                                            <i class="fa fa-clock-o"></i>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- /.form group -->
                                                                            </div>

                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="bootstrap-timepicker">
                                                                                <div class="form-group">
                                                                                    <label for="txtHoraRetiro">Horario Tarde Desde</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtHorarioTDesde" class="form-control timepicker" runat="server" MaxLength="5" />
                                                                                        <div class="input-group-addon">
                                                                                            <i class="fa fa-clock-o"></i>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- /.form group -->
                                                                            </div>

                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="bootstrap-timepicker">
                                                                                <div class="form-group">
                                                                                    <label for="txtHoraRetiro">Horario Tarde Hasta</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtHorarioTHasta" class="form-control timepicker" runat="server" MaxLength="5" />
                                                                                        <div class="input-group-addon">
                                                                                            <i class="fa fa-clock-o"></i>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- /.form group -->
                                                                            </div>

                                                                        </div>

                                                                        <div class="col-md-8">
                                                                            <div class="form-group">
                                                                                <label>Curso Gestionado relacionado DNC</label>
                                                                                <asp:DropDownList ID="ddlCursoGestionadoRelacionado" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Tipo Ubicación</label>
                                                                                <asp:DropDownList ID="ddlTipoUbicacion" runat="server" class="form-control" Style="width: 100%;">
                                                                                    <asp:ListItem>Interna</asp:ListItem>
                                                                                    <asp:ListItem>Externa</asp:ListItem>
                                                                                </asp:DropDownList>

                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <label>Estatus</label>
                                                                                <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>

                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div class="form-group">
                                                                                <label>Ubicación</label>
                                                                                <asp:TextBox ID="txtUbicacion" runat="server" Rows="2" TextMode="MultiLine" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="2000"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div class="form-group">
                                                                                <label>Comentario </label>
                                                                                <asp:TextBox ID="txtComentario" runat="server" Rows="2" TextMode="MultiLine" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="4000"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" id="divEstatusColaboradores" runat="server">
                                                                            <div class="col-md-4">
                                                                                <div class="form-group">
                                                                                    <label>
                                                                                        <asp:CheckBox ID="ChkEstatus" runat="server" />
                                                                                        Aplicar el estatus de Datos Generales a todos los colaboradores</label>

                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <div class="form-group">
                                                                                    <label>Pago Relacionado</label>
                                                                                    <asp:DropDownList ID="ddlPagoRelacionado" runat="server" class="form-control" Style="width: 100%;">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                </div>
                                                                <!-- Datos de la Secretaria -->
                                                                <div class="tab-pane" id="tabGestion_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Curso </label>
                                                                                <asp:DropDownList ID="ddlClaveCursoDc3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Área Temática</label>
                                                                                <asp:DropDownList ID="ddlClaveTematicaDc3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Tipo Agente</label>
                                                                                <asp:DropDownList ID="ddlTipoAgenteDc3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Modalidad</label>
                                                                                <asp:DropDownList ID="ddlClaveModalidadDc3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Capacitación</label>
                                                                                <asp:DropDownList ID="ddlClaveCapacitacionDc3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Establecimiento</label>
                                                                                <asp:DropDownList ID="ddlClaveEstablecimiento" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label>Clave Ocupación DC3</label>
                                                                                <asp:DropDownList ID="ddlOcupacionDC3" runat="server" class="form-control" Style="width: 100%;">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Colaboradores -->
                                                                <div class="tab-pane" id="tabGestion_3" runat="server">
                                                                    <div class="row" id="divGestionColaborador" runat="server">
                                                                          <div class="col-md-12">
                                                                               <button id="btnCargarExcel" runat="server" type="button" class="btn btn-default btn-xs pull-right" data-toggle="modal" data-target="#modalCargarPuesto"><i class="fa fa-file-excel-o"></i>Cargar Colaboradores</button>
                                                                          </div>
                                                                        <div class="col-md-12">
                                                                            <asp:GridView ID="grdColaboradoresGestion" runat="server" AllowPaging="false"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
                                                                                RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                                                Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                                                <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="ChkTodos" runat="server" onclick="checkAll(this);" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkClave" runat="server" onclick="Check_Click(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Matricula" HeaderStyle-Width="5%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>

                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Colaborador" HeaderStyle-Width="60%">

                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblColaborador" runat="server" Text='<%# Bind("colaborador")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreColaborador" runat="server" class="form-control input-sm" Width="100%">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>

                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="ESTATUS">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control input-sm" Width="100%">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control input-sm">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarColaborador" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClientClick="cargarColaborador();" OnClick="lnkAgregarColaborador_Click"></asp:LinkButton>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <PagerStyle BackColor="White" ForeColor="White" />
                                                                                <RowStyle Height="10px" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                                            </asp:GridView>
                                                                            <img id="imgColaborador" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                                            <asp:Label ID="lblRegistroColaboradores" runat="server" Text=""></asp:Label>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <asp:LinkButton ID="lnkGenerarListaAsistencia" runat="server" class="btn btn-info btn-flat pull-left" OnClientClick="return ValidaGridColaboradoresAsistencia();" OnClick="lnkGenerarListaAsistencia_Click"><i class="fa  fa-list-alt"></i> Generar Lista de Asistencia </asp:LinkButton>
                                                                          
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:LinkButton ID="lnkGenerarDc3" runat="server" class="btn btn-info btn-flat pull-right" OnClientClick="return ValidaGridColaboradoresDc3();" OnClick="lnkGenerarDc3_Click"><i class="fa fa-file-zip-o"></i> Generar Formato DC3 </asp:LinkButton>
                                                                            </div>

                                                                        </div>

                                                                    </div>
                                                                    <div class="col-md-12" id="div1" runat="server" style="text-align: center">
                                                                        <asp:Label ID="lblColaboradoresGestion" runat="server" Text="Label"></asp:Label>
                                                                    </div>
                                                                </div>




                                                                <!-- /.tab-pane -->
                                                            </div>
                                                            <!-- /.tab-content -->
                                                        </div>
                                                        <!-- nav-tabs-custom -->
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- ./box-body -->
                                            <div class="box-footer">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lnkGuardarCursoGestionado" runat="server" class="btn btn-danger btn-flat pull-left" OnClientClick="return ValidaCreacionGestion();" OnClick="lnkGuardarCursoGestionado_Click"><i class="fa fa-save"></i> Guardar Datos del Curso </asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <img id="imgProcesar" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-4" style="text-align: right">
                                                        <asp:LinkButton ID="lnkGestionarNuevoCurso" runat="server" class="btn btn-danger btn-flat pull-right" OnClientClick="return NuevoCursoGestionado();" OnClick="lnkGestionarNuevoCurso_Click"><i class="fa fa-file"></i> Gestionar Nuevo Curso </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label CssClass="text-green" ID="lblEstatusCurso" runat="server" Text=""></asp:Label>
                                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>
                                <asp:HiddenField ID="hdIdGestionCapacitacion" runat="server" />
                                <asp:HiddenField ID="hdIdGestion" runat="server" />
                                <asp:HiddenField ID="hdNumerosColaboradores" runat="server" />
                                <asp:HiddenField ID="hdColaboradores" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkGenerarListaAsistencia" />
                                <asp:PostBackTrigger ControlID="lnkGenerarDc3" />
                                <asp:PostBackTrigger ControlID="btnCargaColaboradores" />
                            </Triggers>
                        </asp:UpdatePanel>


                        <!-- /.col -->
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdNoNominaUsuario" runat="server" />
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <!-- /.box -->
                        <asp:HiddenField ID="hfGridView1SV" runat="server" />
                        <asp:HiddenField ID="hfGridView1SH" runat="server" />
                    </section>
                    <!-- /.content -->
                </div>
                <!-- /.container -->
            </div>
            <!-- /.content-wrapper -->
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

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            var theImg = document.getElementById("imgProcesar");
            theImg.style.display = "inline";

            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        row.style.backgroundColor = "#00c0ef";
                        inputList[i].checked = true;
                        document.getElementById('<%= hdColaboradores.ClientID%>').value = "1";
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "white";
                        }
                        else {
                            row.style.backgroundColor = "#F2F2F2";
                        }
                        inputList[i].checked = false;
                        document.getElementById('<%= hdColaboradores.ClientID%>').value = "0";
                    }
                }
            }
            theImg.style.display = "none";
        }


        function cargarInfo() {
            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";
        }
        //Funcion Validar los datos de la Gestion de Capacitacion
        function ValidaCreacionGestion() {
            //
            var ddlEsDNC = document.getElementById("ddlEsDNC");
            if (ddlEsDNC.options[ddlEsDNC.selectedIndex].value == "1") {
            
            }
            var ddlCursoAutorizadoDNC = document.getElementById("ddlCursoAutorizadoDNC");
            //Curso DNC
            if (ddlCursoAutorizadoDNC.options[ddlCursoAutorizadoDNC.selectedIndex].value == 0) {
                alert("Debe de seleccionar el Curso.");
                ddlCursoAutorizadoDNC.focus();
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
            //No. Máx. Participantes
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

            //Costo Total (SN IVA)
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

            //Costo Final
            if (document.getElementById("txtCostoTotal").value == '') {
                alert("Debe capturar el Costo Final.");
                document.getElementById("txtCostoTotal").focus();
                return false;
            }

            var ddlEstatus = document.getElementById("ddlEstatus");
            //Estatus
            if (ddlEstatus.options[ddlEstatus.selectedIndex].value == 0) {
                alert("Debe de seleccionar el estatus.");
                ddlEstatus.focus();
                return false;
            }
            // Valida de los campos de la dc3 si capturan que aplica

            var ddlDC3 = document.getElementById("ddlDC3");
            //Estatus
            if (ddlDC3.options[ddlDC3.selectedIndex].value == 'Si') {
                //ddlClaveCursoDc3
              //  var ddlClaveCursoDc3 = document.getElementById("ddlClaveCursoDc3");
              //  if (ddlClaveCursoDc3.options[ddlClaveCursoDc3.selectedIndex].value == 0) {
              //      alert("Debe de seleccionar la Clave Curso Dc3 en Datos de la Secretaria del Trabajo.");
              //      ddlClaveCursoDc3.focus();
              //      return false;
              //  }
                //ddlClaveTematicaDc3
                var ddlClaveTematicaDc3 = document.getElementById("ddlClaveTematicaDc3");
                if (ddlClaveTematicaDc3.options[ddlClaveTematicaDc3.selectedIndex].value == 0) {
                    alert("Debe de seleccionar la Clave Área Temática Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlClaveTematicaDc3.focus();
                    return false;
                }
                //Tipo de Agennte
                var ddlTipoAgenteDc3 = document.getElementById("ddlTipoAgenteDc3");
                if (ddlTipoAgenteDc3.options[ddlTipoAgenteDc3.selectedIndex].value == 0) {
                    alert("Debe de seleccionar el Tipo de Agente Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlTipoAgenteDc3.focus();
                    return false;
                }
                //Modalidad
                var ddlClaveModalidadDc3 = document.getElementById("ddlClaveModalidadDc3");
                if (ddlClaveModalidadDc3.options[ddlClaveModalidadDc3.selectedIndex].value == 0) {
                    alert("Debe de seleccionar la Modalidad Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlClaveModalidadDc3.focus();
                    return false;
                }
                //ddlClave Capacitación
                var ddlClaveCapacitacionDc3 = document.getElementById("ddlClaveCapacitacionDc3");
                if (ddlClaveCapacitacionDc3.options[ddlClaveCapacitacionDc3.selectedIndex].value == 0) {
                    alert("Debe de seleccionar la Clave Capacitación Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlClaveCapacitacionDc3.focus();
                    return false;
                }
                //ddlClave Establecimiento
                var ddlClaveEstablecimiento = document.getElementById("ddlClaveEstablecimiento");
                if (ddlClaveEstablecimiento.options[ddlClaveEstablecimiento.selectedIndex].value == 0) {
                    alert("Debe de seleccionar la Clave Establecimiento Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlClaveEstablecimiento.focus();
                    return false;
                }
                //ddlClave Ocupación
                var ddlOcupacionDC3 = document.getElementById("ddlOcupacionDC3");
                if (ddlOcupacionDC3.options[ddlOcupacionDC3.selectedIndex].value == 0) {
                    alert("Debe de seleccionar la Clave Ocupación Dc3 en Datos de la Secretaria del Trabajo.");
                    ddlOcupacionDC3.focus();
                    return false;
                }

            }

            //Comentario referente a actualizar la informacion

            //ddlClave Ocupación
            var ddlConsultaCursosGestionados = document.getElementById("ddlConsultaCursosGestionados");
            if (ddlConsultaCursosGestionados.options[ddlConsultaCursosGestionados.selectedIndex].value == 0) {
                if (!confirm('¿Deseas gestionar este curso con la información registrada?, Una vez guardada la información podrás registrar los colaboradores.')) { return false; };
            } else {
                if (document.getElementById("ChkEstatus").checked == true) {
                    if (!confirm('Al Aceptar se actualizará el estatus indicado en la sección de Datos Generales a todos los colaboradores registrados. ¿Deseas continuar?')) { return false; };
                } else {
                    if (!confirm('¿Desea actualizar la información del curso con la información registrada?')) { return false; };
                }

            }

            var theImg = document.getElementById("imgProcesar");
            theImg.style.display = "inline";

            return true;
        }

        function controlTabsGestion(id) {
            //Asigna el valor de la Tab de Becas Ingles
            document.getElementById('hdIdGestion').value = id;
        }

        function NuevoCursoGestionado() {
            if (!confirm('¿Desea Gestionar Nuevo Curso?')) { return false; };
            return true;
        }
        // Validacion para Lista de asistencia,DC3 y Diplomas
        function CargarExcel() {
            if (!confirm('Al Aceptar se cargarán los datos de los colaboradores.')) { return false; };
            return true;
        }
    </script>
</body>
</html>
