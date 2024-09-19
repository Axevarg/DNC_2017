<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="gestionbecas.aspx.vb" Inherits="DNC_2017.gestionbecas" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <title>Gestión Becas | SIGIDO</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
  <!-- Bootstrap 3.3.6 -->
  <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>
  <!-- Font Awesome -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css"/>
  <!-- Ionicons -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css"/>
      <!-- Select2 -->
  <link rel="stylesheet" href="plugins/select2/select2.min.css"/>
         <!-- bootstrap datepicker -->
  <link rel="stylesheet" href="plugins/datepicker/datepicker3.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
  <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
  <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
        <!-- Sub Menu -->
    <link href="bootstrap/css/dropdown-submenu.css" rel="stylesheet" />
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

                gridviewScroll();

                $(window).resize(function () {
                    gridviewScroll();

                });
            });
            //Grid Cursos
            function gridviewScroll() {

                gridView1 = $('#grdBecas').gridviewScroll({
                    width: 'auto',
                    height: 300,
                    //    headerrowcount: 1,
                    freezesize: 4,
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
            //Historico

 
           function combo() {
               //$("#ddlCurso").select2();
               //$("#ddlAgreCurso").select2();
               $("#ddlColaborador").select2();
                
               //closeOnSelect: false
           }

           //Metodo de compartamiento en Tabs
           function tab(id) {
               document.getElementById('hdIdTab').value = id;
               document.getElementById('lblPrueba').innerHTML = id;
               
           }
            

           function comportamientosJS() {
               $('#txtFechaInicio').datepicker({
                   autoclose: true
               });

               $('#txtAgreFechaInicio').datepicker({
                   autoclose: true
               });

               $('#txtFechaFinal').datepicker({
                   autoclose: true
               });

               $('#txtAgreFechaFinal').datepicker({
                   autoclose: true
               });
          
               $('#txtFechaCarta').datepicker({
                   autoclose: true
               });
               $('#txtFechaAsignacion').datepicker({
                   autoclose: true
               });

               $('#txtAgreFechaAsignacion').datepicker({
                   autoclose: true
               });

               $('#txtFechaBajaTem').datepicker({
                   autoclose: true
               });

               $('#txtAgreFechaBajaTem').datepicker({
                   autoclose: true
               });

           }
</script>
    <style>
            .letra{
                font-size:8pt;
            }
             .letraTemario{
                font-size: 7pt;
            }
             .guardar{
                 color: #f39c12;
             }
    </style>
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
                <br /><br />
                <div class="container">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Gestión del Programa de Becas
                                       <small>Becas</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">
                        <!-- Modal de Credito-->
                        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalCartasCredito" runat="server">
                            <div class="modal-dialog modal-lg" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title" id="myModalLabel2">Configuración de Cartas de Crédito Ingles</h4>
                                    </div>
                                    <div class="modal-body">
                      
                         
                                        <div class="row">
                                            <!--Listado de Cartas-->
                                            <div class="col-md-12" style="height: 200px; overflow: auto">
                                                <asp:Table ID="tbCartas" runat="server" class="table table-condensed table-responsive" Width="100%">
                                                    <asp:TableHeaderRow>
                                                        <asp:TableCell Width="10%"></asp:TableCell>
                                                        <asp:TableCell Width="70%" Font-Bold="true">Archivo</asp:TableCell>
                                                        <asp:TableCell Width="20%"></asp:TableCell>
                                                    </asp:TableHeaderRow>

                                                </asp:Table>
                                                <!--Control de Evento Eliminar las Cartas-->
                                                <asp:LinkButton ID="lnkElminaCartas" runat="server" Font-Size="8pt" OnClick="lnkElminaCartas_Click" OnClientClick="return eliCartaCreditoJs();"></asp:LinkButton>

                                            </div>
                                            <div class="col-md-12">
                                                <asp:Label ID="lblErrorCalificacionIngles" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default pull-left bg-red" data-dismiss="modal">Cerrar</button>
                            
                                    </div>
                                </div>

                            </div>
                        </div>
                        <!--Fin  Modal -->

                             <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <!--Datos-->
                                <div id="divDatos" runat="server" class="row">
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
                                        <div class="form-group ">

                                            <asp:Label ID="lblNombreCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                            <asp:Label ID="lblPuestoCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                            <asp:Label ID="lblArea" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                            <asp:Label ID="lblDireccion" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!--Check-->

                                <!--Registro BEcas-->
                                <div id="divRegistro" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Registro de Becas</h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnExportarCursos" name="btnExportarCursos" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid -->
                                                <div class="row">

                                                    <div class="col-md-12">

                                                        <asp:GridView ID="grdBecas" runat="server" class="table table-condensed" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" ShowFooter="True" Style="font-size: 8pt; text-align: center"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None">
                                                            <HeaderStyle Font-Size="9pt" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField HeaderText="" SelectText="<i class='fa fa-file-text'></i>" ShowSelectButton="True">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>

                                                                <asp:TemplateField HeaderText="NO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNo" runat="server" Text='<%# Bind("no_correlativo")%>' Width="10px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="TIPO BECAS">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlTipoBeca" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTipoBeca" runat="server" Text='<%# Bind("fk_id_tipo_beca")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreTipoBeca" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="PERIODO ASIGNACIÓN">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlPeriodoAsignacion" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPeriodoAsignacion" runat="server" Text='<%# Bind("fk_id_periodo_asignacion")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgrePeriodoAsignacion" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA ASIGNACIÓN">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaAsignacion" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_asignacion")%>' Width="120px" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaAsignacion" runat="server" Text='<%# Bind("fecha_asignacion")%>' ForeColor="#333333" Width="120px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaAsignacion" ClientIDMode="Static" runat="server" class="form-control input-sm" Width="120px" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TIPO ASIGNACIÓN">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlTipoAsignacion" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTipoAsignacion" runat="server" Text='<%# Bind("fk_id_tipo_asignacion")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreTipoAsignacion" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MODALIDAD ESTUDIO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlModalidadEstudio" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblModalidadEstudio" runat="server" Text='<%# Bind("fk_id_modalidad_estudio")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreModalidadEstudio" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TIPO PROYECTO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlTipoProyecto" runat="server" class="form-control input-sm" Width="200px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTipoProyecto" runat="server" Text='<%# Bind("fk_id_tipo_proyecto")%>' Width="200px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreTipoProyecto" runat="server" class="form-control input-sm" Width="200px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TIPO ESTATUS">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlTipoEstus" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTipoEstus" runat="server" Text='<%# Bind("fk_id_tipo_estatus")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreTipoEstus" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="ESTATUS PAGO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEstatusPago" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstusPago" runat="server" Text='<%# Bind("fk_id_estatus_pago")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreEstusPago" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="PROVEEDOR">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlProveedor" runat="server" class="form-control input-sm" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProveedor" runat="server" Text='<%# Bind("fk_id_proveedor")%>' Width="250px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreProveedor" runat="server" class="form-control input-sm" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA INICIO" HeaderStyle-CssClass="centrar" HeaderStyle-Width="26%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaInicio" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_inicio")%>' Width="100px" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Bind("fecha_inicio")%>' ForeColor="#333333" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaInicio" ClientIDMode="Static" runat="server" class="form-control input-sm" Width="100px" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA FINAL" HeaderStyle-CssClass="centrar" HeaderStyle-Width="26%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaFinal" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_termino")%>' Width="100px" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaFinal" runat="server" Text='<%# Bind("fecha_termino")%>' ForeColor="#333333" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblAgregar" runat="server" ForeColor="#333333" Width="100px"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="OBSERVACIONES" HeaderStyle-Width="30%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtObservaciones" runat="server" Text='<%# Bind("observaciones")%>' Width="200px" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblObservaciones" runat="server" Text='<%# Bind("observaciones")%>' Width="200px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreObservaciones" runat="server" class="form-control input-sm" Width="200px" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                          <asp:TemplateField HeaderText="FECHA BAJA TEMPORAL" HeaderStyle-CssClass="centrar" HeaderStyle-Width="26%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaBajaTem" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_baja_temporal")%>' Width="100px" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaBajaTem" runat="server" Text='<%# Bind("fecha_baja_temporal")%>' ForeColor="#333333" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaBajaTem" ClientIDMode="Static" runat="server" class="form-control input-sm" Width="100px" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>


                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="ELIMINAR" ControlStyle-Font-Size="8pt">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregar" runat="server" Text="Agregar" class="label label-danger letra" ToolTip="Agregar" OnClick="lnkAgregar_Click" OnClientClick="document.getElementById('loadingPagina').style.display = 'inline'"></asp:LinkButton>
                                                                    </FooterTemplate>
                                                                    <FooterStyle Font-Size="8pt" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                        </asp:GridView>
                                                        <img id="loadingPagina" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblGrid" runat="server" Text="Label"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="lblErrorGestion" ForeColor="Red" for="inputError" runat="server" Text="Label"></asp:Label>


                                                    </div>
                                                </div>
                                                <!-- /.Grid  -->
                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->

                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>


                                <!--Creación de Cartas de Credito-->
                                <div id="divBecasIngles" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Configuración de Cartas de Crédito Ingles</h3>

                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid Historico-->
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Tipo Autorización</label>
                                                            <asp:DropDownList ID="ddlTipoAutorizacion" class="form-control select2" runat="server" Width="100%"></asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Horario Estudio</label>
                                                            <asp:DropDownList ID="ddlHorarioEstudio" class="form-control select2" runat="server" Width="100%"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Plantel</label>
                                                            <asp:DropDownList ID="ddlPlantel" class="form-control select2" runat="server" Width="100%"></asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Nivel donde Inicia</label>
                                                            <asp:DropDownList ID="ddlNivelInicial" class="form-control select2" runat="server" Width="100%"></asp:DropDownList>

                                                        </div>
                                                    </div>

                                                    <div id="divGuardaConfBIngles" runat="server" class="col-md-12">
                                                        <button id="btnGuardaConfiguracionBIngles" runat="server" class="btn btn-danger btn-flat pull-right bg-red"
                                                            name="btnGuardaConfiguracionBIngles" onclick="return validacionesConfiguracion();">
                                                            <i class="fa fa-save"></i>Guardar</button>
                                                        <img id="loadingGuardaConfiguracionBIngle" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>

                                                </div>
                                                <div id="divCalificaciones" runat="server" class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Nivel</label>
                                                            <asp:DropDownList ID="ddlNivel" class="form-control select2" runat="server" Width="100%"></asp:DropDownList>

                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Calificación</label>
                                                            <asp:TextBox ID="txtCalificacion" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Asistencia %</label>
                                                            <asp:TextBox ID="txtAsistencia" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div id="divAgregaCalfIngles" runat="server" class="col-md-12">
                                                        <button id="btnAgregaCalfIngles" runat="server" class="btn btn-danger btn-flat pull-right bg-red"
                                                            name="btnAgregaCalfIngles" onclick="return validaCalfIngles();">
                                                            <i class="fa fa-plus"></i>Agregar</button>
                                                        <img id="loadingAgregaCalfIngles" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-8">
                                                        <asp:GridView ID="grdCalificacionesIngles" runat="server" class="table table-condensed" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" ShowFooter="True"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None"
                                                            PageSize="10">
                                                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <HeaderStyle Font-Size="9pt" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="Ingles_niveles" HeaderText="NIVEL DE INGLES" HeaderStyle-Width="40%" />
                                                                <asp:BoundField DataField="calificacion" HeaderText="CALIFICACION" HeaderStyle-Width="20%" />
                                                                <asp:BoundField DataField="asistencia" HeaderText="ASISTENCIA" HeaderStyle-Width="20%" />
                                                                <asp:TemplateField HeaderText="ELIMINAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminarCalifIngles" runat="server" Font-Size="8pt" class="label label-danger" Text="Eliminar" CommandName="Delete" ToolTip="Eliminar"> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <PagerStyle BackColor="White" ForeColor="White" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-12" style="text-align: center;">

                                                        <asp:Label ID="lblErrorPago" runat="server" Text="Label" ForeColor="Red"></asp:Label>

                                                    </div>
                                                </div>
                                                <!-- /.Grid Historico -->
                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- Control de Calificaciones de  -->
                                <div id="divCalificacionesBecas" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Control de Calificaciones de Becas</h3>

                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid Historico-->

                                                <div id="div1" runat="server" class="row">
                                                    <div class="col-md-8">
                                                        <div class="form-group">
                                                            <label>Concepto</label>
                                                            <asp:TextBox ID="txtConcepto" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label>Calificación</label>
                                                            <asp:TextBox ID="txtCalificacionBecas" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>

                                                        </div>
                                                    </div>


                                                    <div class="col-md-2">
                                                        <button id="btnAgregarCalificaciones" runat="server" class="btn btn-danger btn-flat pull-right bg-redd"
                                                            name="btnAgregarCalificaciones" onclick="return validaCalficaciones();">
                                                            <i class="fa fa-plus"></i>Agregar</button>
                                                        <img id="loadingCalificaciones" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-8">
                                                        <asp:GridView ID="grdCalificaciones" runat="server" class="table table-condensed" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" ShowFooter="True"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None"
                                                            PageSize="10">
                                                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <HeaderStyle Font-Size="9pt" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="concepto" HeaderText="MATERIA / MÓDULO" HeaderStyle-Width="80%" />
                                                                <asp:BoundField DataField="calificacion" HeaderText="CALIFICACION" HeaderStyle-Width="20%" />

                                                                <asp:TemplateField HeaderText="ELIMINAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminarCalificaciones" runat="server" Font-Size="8pt" class="label label-danger" Text="Eliminar" CommandName="Delete" ToolTip="Eliminar"> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <PagerStyle BackColor="White" ForeColor="White" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-12" style="text-align: center;">

                                                        <asp:Label ID="lblErrorCalificacion" runat="server" Text="Label" ForeColor="Red"></asp:Label>

                                                    </div>
                                                </div>
                                                <!-- /.Grid Historico -->
                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- END CUSTOM TABS -->
                                <asp:HiddenField ID="hdIdTab" runat="server" />
                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                                <asp:HiddenField ID="hdIndexCurso" runat="server" />
                                <asp:HiddenField ID="hdIdBecaIngles" runat="server" />
                                <asp:HiddenField ID="hdidCartasCredito" runat="server" />
                                <asp:HiddenField ID="hdTipoBeca" runat="server" />
                                <asp:HiddenField ID="hdIdBeca" runat="server" />
                                <!--Bandera de Evento de Calificaciones Ingles de Carta Credito-->
                                <asp:HiddenField ID="hdIdComportamientosBecaIngles" runat="server" />
                            </ContentTemplate>
                            <Triggers>

                                <asp:PostBackTrigger ControlID="btnExportarCursos" />
                            </Triggers>
                        </asp:UpdatePanel>
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
        function validacionesConfiguracion() {

                        //Tipo Autorizacion
                var ddlTipoAutorizacion = document.getElementById("ddlTipoAutorizacion");
                if (ddlTipoAutorizacion.options[ddlTipoAutorizacion.selectedIndex].value == 0) {
                    ddlTipoAutorizacion.focus();
                    alert("Debe de seleccionar el Tipo de Autorización.");
                    return false;
                }


                //Horario Estudio
                var ddlHorarioEstudio = document.getElementById("ddlHorarioEstudio");
                if (ddlHorarioEstudio.options[ddlHorarioEstudio.selectedIndex].value == 0) {
                    ddlHorarioEstudio.focus();
                    alert("Debe de seleccionar el Horario Estudio.");
                    return false;
                }
                //ÁPlantel
                var ddlPlantel = document.getElementById("ddlPlantel");
                if (ddlPlantel.options[ddlPlantel.selectedIndex].value == 0) {
                    ddlPlantel.focus();
                    alert("Debe de seleccionar el Plantel.");
                    return false;
                }
                //Nivel Inicial
                var ddlNivelInicial = document.getElementById("ddlNivelInicial");
                if (ddlNivelInicial.options[ddlNivelInicial.selectedIndex].value == 0) {
                    ddlNivelInicial.focus();
                    alert("Debe de seleccionar el Nivel Inicial.");
                    return false;
                }
                   

                var theImg = document.getElementById("loadingGuardaConfiguracionBIngle");
                theImg.style.display = "inline";
            return true;

        }
        // funcion para agregar calificaciones ingles
        function validaCalfIngles() {

                //Nivel
                var ddlNivel = document.getElementById("ddlNivel");
                if (ddlNivel.options[ddlNivel.selectedIndex].value == 0) {
                    ddlNivel.focus();
                    alert("Debe de seleccionar el Nivel.");
                    return false;
                }

                //Calificacion
                if (document.getElementById("txtCalificacion").value == '') {
                    document.getElementById("txtCalificacion").focus();
                    alert("Debe capturar la calificación.");
                    return false;
                } else {
                    if (document.getElementById("txtCalificacion").value > 10) {
                        document.getElementById("txtCalificacion").focus();
                        alert("La Calificación No puede ser Mayor a 10.");
                        return false;
                    }

                }
                //Asistencia
                if (document.getElementById("txtAsistencia").value == '') {
                    document.getElementById("txtAsistencia").focus();
                    alert("Debe capturar la Asistencia.");
                    return false;
                } else {
                    if (document.getElementById("txtAsistencia").value > 100) {
                        document.getElementById("txtAsistencia").focus();
                        alert("La Asistencia No puede ser Mayor a 100.");
                        return false;
                    }
                }
                var theImg = document.getElementById("loadingAgregaCalfIngles");
                theImg.style.display = "inline";
                return true;
        }


        function cargarInfo() {

            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }


            //desabilitar el boton derecho
        //      document.oncontextmenu = function () { return false }

       //limipia label
        function cleanCtrl() {
            document.getElementById("lblNombreB").innerHTML = '';
            document.getElementById("lblDatos").innerHTML = '';
            document.getElementById("lblDireccion").innerHTML = '';
            document.getElementById("lblConductor").innerHTML = '';


        }

        function cargarInfo() {
            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }

        function cargarPaginacion() {
            var theImg = document.getElementById("loadingPagina");
            theImg.style.display = "inline";

        }

        function limpiaLabel() {
            document.getElementById("lblNombreCol").innerHTML = '';
            document.getElementById("lblDatos").innerHTML = '';
            document.getElementById("lblDireccion").innerHTML = '';
            document.getElementById("divNomina").className = '';

        }


        // Funcion para agregar nuevas tablas

        function FilasTabla(carta,idCarta) {
            // Find a <table> element with id="myTable":
            var table = document.getElementById("tbCartas");

            // Create an empty <tr> element and add it to the 1st position of the table:
            var row = table.insertRow(1);

            // Insert new cells (<td> elements) at the 1st and 2nd position of the "new" <tr> element:
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            var cell3 = row.insertCell(2);
            // Add some text to the new cells:
            cell1.style.backColor = "silver";
            var btnVer = document.createElement("a");
         
            var linkText = document.createTextNode("Ver");
            btnVer.appendChild(linkText);
            btnVer.title = "Imprimir Carta de Crédito";

            btnVer.setAttribute('onclick', 'abrirVentanaCartas(' + idCarta + ');');

            btnVer.className = "label label-danger";


            cell1.appendChild(btnVer);

            cell2.innerHTML = carta;
            //asigna comportamiento del control eliminar
            var btnEliminar = document.createElement("a");
            var linkTextEliminar = document.createTextNode("Eliminar");
            btnEliminar.appendChild(linkTextEliminar);
            btnEliminar.title = "Eliminar Carta";
            btnEliminar.setAttribute('onclick', 'elminarCartas('+ idCarta +');');
            btnEliminar.className = "label label-danger";

            btnEliminar.id = "btn_" + idCarta
    

            cell3.appendChild(btnEliminar);

        }
        //Inserta encabezado de la tabla

        function eliminaTabla(){
            var tbCartas = document.getElementById("tbCartas");
            tbCartas.style.display = '';
            var rowCount = tbCartas.rows.length;
            for (var i = rowCount - 1; i > 0; i--) {
                tbCartas.deleteRow(i);
            }

            //Inicializa los valores
            //Horario Estudio
            document.getElementById("ddlHorarioEstudio").selectedIndex= 0;
            //ÁPlantel
             document.getElementById("ddlPlantel").selectedIndex=0;
            //Nivel
             document.getElementById("ddlNivelInicial").selectedIndex = 0;
            
        }

        
        function abrirVentanaCartas(id) {
            window.open('print_carta_credito.aspx?idCartaCredito=' + id, '_blank');
            //alert('abre Ventana');
            return false;
        }
        //ejecuta el evento del control para eliminar
        function elminarCartas(id) {
            document.getElementById("hdidCartasCredito").value = id;
                document.getElementById('<%= lnkElminaCartas.ClientID%>').click();
        }
        //Valida si se requiere eliminar la carta
        function eliCartaCreditoJs() {
            if (!confirm('¿Desea Eliminar la Carta?')) { return false; };

            return true;
        }
        //Asigna el id de las Becas de Ingles
        function idBecasIngles(id) {
            document.getElementById('hdIdBecaIngles').value = id;
            document.getElementById('hdTipoBeca').value = 'INGLES';
            var theImg = document.getElementById("loadingPagina");
            theImg.style.display = "inline";
        }

        //Asigna el id de las Becas de Ingles
        function idBeca(id) {
            document.getElementById('hdIdBeca').value = id;
            
            document.getElementById('hdTipoBeca').value = 'BECA';

            var theImg = document.getElementById("loadingPagina");
            theImg.style.display = "inline";
        }

        //valida la informacion de calificaciones
        // funcion para agregar calificaciones ingles
        function validaCalficaciones() {

            //Concepto
            if (document.getElementById("txtConcepto").value == '') {
                document.getElementById("txtConcepto").focus();
                alert("Debe capturar el Concepto.");
                return false;
            }

            //Calificacion
            if (document.getElementById("txtCalificacionBecas").value == '') {
                document.getElementById("txtCalificacionBecas").focus();
                alert("Debe capturar la calificación.");
                return false;
            } else {
                if (document.getElementById("txtCalificacionBecas").value > 10) {
                    document.getElementById("txtCalificacionBecas").focus();
                    alert("La Calificación No puede ser Mayor a 10.");
                    return false;
                }

            }

            var theImg = document.getElementById("loadingCalificaciones");
            theImg.style.display = "inline";
            return true;
        }

    </script>


</body>
</html>
