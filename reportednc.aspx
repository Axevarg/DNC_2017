<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="reportednc.aspx.vb" Inherits="DNC_2017.reportednc" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Reportes DNC | SIGIDO</title>

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
            gridviewScrollDNC();

            gridView1 = $('#grdReporte').gridviewScroll({
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

            //Grid Reporte DNC
            function gridviewScrollDNC() {

                gridView1 = $('#grdReporteDNC').gridviewScroll({
                    width: 'auto',
                    height: 300,
                    headerrowcount: 1,

                    startVertical: $("#<%=hfGridView2SV.ClientID%>").val(),
                    startHorizontal: $("#<%=hfGridView2SH.ClientID%>").val(),
                    onScrollVertical: function (delta) {
                        $("#<%=hfGridView2SV.ClientID%>").val(delta);
                    },
                    onScrollHorizontal: function (delta) {
                        $("#<%=hfGridView2SH.ClientID%>").val(delta);
                    }


                });

                }

    </script>

    <style>
        .letra {
            font-size: 8pt;
        }

        .letraTemario {
            font-size: 7pt;
        }

        .guardar {
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

    <script>
        function comportamientosJS() {
            $('#txtFechaEmision').datepicker({
                autoclose: true
            });

            $('#txtFechaEmisionHasta').datepicker({
                autoclose: true
            });

            $("#ddlPrioridad").select2({
                placeholder: "TODAS",
            });
            $("#ddlDireccion").select2({
                placeholder: "TODAS",
            });
            $("#ddlUnidadNegocio").select2({
                placeholder: "TODAS",
            });
            $("#ddlHabilidad").select2({
                placeholder: "TODAS",
            });
            $("#ddlEstatus").select2({
                placeholder: "TODOS",
            });
            $("#ddlTipoCurso").select2();
            $("#ddlEstatus").select2({
                placeholder: "TODOS",
            });
            $("#ddlTipoReporte").select2();

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
                        <h1>Reportes de DNC       
                            <small>SIGIDO</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                </div>
                                <!-- /.Reporte de Registro -->
                                <div id="div1" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Reporte DNC </h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnReporteDNC" name="btnReporteDNC" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="left" title="Buscar Información" onclick="cargarBuscarDNC();">
                                                        <i class="fa fa-search"></i>
                                                    </button>

                                                    <button id="btnExportRepDNC" name="btnExportRepDNC" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">

                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <label for="txtFechaEmision" title="Fecha Emisión Desde">Fecha Registro Desde</label>
                                                        <asp:TextBox ID="txtFechaEmision" runat="server" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                            onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label for="txtFechaEmisionHasta" title="Fecha Emisión Hasta">Fecha Registro Hasta</label>
                                                        <asp:TextBox ID="txtFechaEmisionHasta" runat="server" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                            onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <label for="ddlPrioridad" title="Prioridad">Prioridad </label>

                                                        <asp:ListBox ID="ddlPrioridad" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="ddlDireccion" title="Dirección">Dirección </label>
                                                        <asp:ListBox ID="ddlDireccion" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="ddlHabilidad" title="Tipo de Habilidad">Tipo Habilidad </label>
                                                        <asp:ListBox ID="ddlHabilidad" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>


                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <label for="ddlUnidadNegocio" title="Unidad de Negocio ">Unidad de Negocio </label>

                                                        <asp:ListBox ID="ddlUnidadNegocio" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="ddlEstatus" title="Estatus">Estatus </label>
                                                        <asp:ListBox ID="ddlEstatus" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="ddlTipoCurso" title="Tipo de Curso">Tipo de Curso </label>
                                                        <asp:DropDownList ID="ddlTipoCurso" class="form-control  input-sm" runat="server" Width="100%">
                                                            <asp:ListItem Value="0"> Todos</asp:ListItem>
                                                            <asp:ListItem Value="1">Interno</asp:ListItem>
                                                            <asp:ListItem Value="2">Externo</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="ddlTipoR" title="Tipo de Compra">Tipo de Reporte </label>
                                                        <asp:DropDownList ID="ddlTipoReporte" class="form-control  input-sm" runat="server" Width="100%">
                                                            <asp:ListItem Value="1">Agrupado</asp:ListItem>
                                                            <asp:ListItem Value="2">Detalle</asp:ListItem>
                                                            <asp:ListItem Value="3">Tipo Curso</asp:ListItem>
                                                            <asp:ListItem Value="4">Autorización de Capacitación</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>


                                                <!-- Grid General-->
                                                <div class="row">



                                                    <div class="col-md-12" style="text-align: center;">
                                                                   <img id="imgDNC" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        <img id="img1" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        <strong>
                                                            <asp:Label ID="lblMnsJEr" runat="server" Text="Label"></asp:Label>
                                                        </strong>
                                                    </div>
                                                    <br />
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <asp:GridView ID="grdReporteDNC" runat="server"
                                                                AllowPaging="False" AutoGenerateColumns="False" Width="100%"
                                                                RowStyle-Height="10px" class="table table-condensed" Style="font-size: 8pt;"
                                                                Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True">
                                                                <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                <Columns>


                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                        </ItemTemplate>
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
                                                            <img id="loadingR" src="img/glyphLoading.gif" style="display: none" />
                                                        </div>
                                                    </div>

                                                </div>
                                                <!-- /.Grid General -->



                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- Listado de Cursos Registrados--->
                                <div id="divReporte" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Listado de Cursos Regitrados en DNC </h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnBuscar" name="btnBuscar" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="left" title="Buscar Información" onclick="cargarBuscar();">
                                                        <i class="fa fa-search"></i>
                                                    </button>

                                                    <button id="btnExportarReporte" name="btnExportarReporte" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid General-->
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label for="ddlTipoCurso" title="Tipo de Curso">DNC </label>
                                                        <asp:DropDownList ID="ddlDNC" class="form-control  input-sm" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdReporte" runat="server"
                                                            AllowPaging="False" AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" class="table table-condensed" Style="font-size: 8pt;"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>


                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
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
                                                        <img id="loadingG" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                </div>
                                                <!-- /.Grid General -->
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <img id="imgBuscar" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        <strong>
                                                            <asp:Label ID="lblMensajeBuscar" runat="server" Text="Label"></asp:Label>
                                                        </strong>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExportarReporte" />
                                <asp:PostBackTrigger ControlID="btnExportRepDNC" />
                            </Triggers>
                        </asp:UpdatePanel>

                
                        <!-- Fin Reportes DNC -->
                        <asp:HiddenField ID="hdIdTabReportes" runat="server" />
                        <!-- /.col -->
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdNoNominaUsuario" runat="server" />
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <!-- /.box -->
                        <asp:HiddenField ID="hfGridView1SV" runat="server" />
                        <asp:HiddenField ID="hfGridView1SH" runat="server" />

                        <asp:HiddenField ID="hfGridView2SV" runat="server" />
                        <asp:HiddenField ID="hfGridView2SH" runat="server" />
                        <!-- /.box -->
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

        function cargarBuscar() {
            var theImg = document.getElementById("imgBuscar");
            theImg.style.display = "inline";
        }

        function cargarBuscarDNC() {
            var theImg = document.getElementById("imgDNC");
            theImg.style.display = "inline";
        }


        function controlTabsReportes(id) {
            //Asigna el valor de la Tab de Becas Ingles
            document.getElementById('hdIdTabReportes').value = id;
        }
    </script>
</body>
</html>
