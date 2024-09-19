<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="gestion_cartas_credito.aspx.vb" Inherits="DNC_2017.gestion_cartas_credito" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <title>Gestion Cartas Crédito | SIGIDO</title>
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
          
           }

           $(document).ready(function () {

               gridviewScroll();

               $(window).resize(function () {
                   gridviewScroll();

               });
           });
           //Grid Cursos
           function gridviewScroll() {

               gridView1 = $('#grdCandidatos').gridviewScroll({
                   width: 'auto',
                   height: 300,
                   headerrowcount: 1,
                   //   freezesize: 4,
                   startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                    startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                    onScrollVertical: function (delta) {
                        $("#<%=hfGridView1SV.ClientID%>").val(delta);
                    },
                    onScrollHorizontal: function (delta) {
                        $("#<%=hfGridView1SH.ClientID%>").val(delta);
                    }


                });
               gridviewScrollCartasCredito();
                }
                function gridviewScrollCartasCredito() {

                    gridView1 = $('#grdCartasCredito').gridviewScroll({
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
                        <h1>Gestión de Cartas Crédito
                                       <small>Ingles</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <!--Fin  Modal Archivos -->
                
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <!--Datos de Cartas de Crédito-->
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Año</label>
                                            <asp:DropDownList ID="ddlAnio" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Horario de Estudio</label>
                                            <asp:DropDownList ID="ddlHorarioEstudio" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Calendario</label>
                                            <asp:DropDownList ID="ddlCalendario" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <br />
                                        <button class="btn btn-danger btn-flat" runat="server" id="btnCrearCarta" name="btnCrearCarta">
                                            <i class="fa fa-gears"></i>
                                            Crear Cartas Crédito
                                        </button>
                                        <img id="loadingControl" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                        <div class="form-group" style="text-align: center">
                                            <br />

                                        </div>
                                    </div>


                                    <img id="loading" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                </div>
                                <!--Generación de Cartas Crédito-->
                                <div id="divRegistro" runat="server" class="row">
                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Candidatos a tener de Carta Crédito</h3>

                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Consulta de Colaboraadores candidatos  Cartas de Crédito-->
                                                <div class="row" id="divCandidatos" runat="server">
                                                    <div class="col-md-12" style="overflow: auto">
                                                        <asp:GridView ID="grdCandidatos" runat="server" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" 
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="12">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="image/resultset_first.png" LastPageImageUrl="image/resultset_last.png" NextPageImageUrl="image/resultset_next.png" PreviousPageImageUrl="image/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkCartaCredito" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="clave" HeaderText="CLAVE" HeaderStyle-Width="10%" />
                                                                <asp:BoundField DataField="nombre" HeaderText="NOMBRE" HeaderStyle-Width="25%" />
                                                                <asp:BoundField DataField="puesto" HeaderText="PUESTO" HeaderStyle-Width="20%" />
                                                                <asp:BoundField DataField="nivel" HeaderText="NIVEL" HeaderStyle-Width="20%" />
                                                                <asp:BoundField DataField="calificacion" HeaderText="CALIFICACIÓN" HeaderStyle-Width="10%" />
                                                                <asp:BoundField DataField="asistencia" HeaderText="ASISTENCIA" HeaderStyle-Width="10%" />
                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <PagerStyle BackColor="White" ForeColor="White" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                        </asp:GridView>
                                                        <img id="loadingColaboradores" runat="server" src="image/glyphLoading.gif" style="display: none" />

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="lblRegistrosCandidatos" runat="server" Text="Label"></asp:Label>
                                                    </div>
                                                </div>

                                                <!-- /.Grid  -->
                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>

                                <!--Generación de Cartas Crédito-->
                                <div id="div1" runat="server" class="row">
                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Cartas de Crédito Configuradas en Calendario</h3>

                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="lblErrorCartasCredito" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>

                                                <!--Datos de Cartas de Crédito-->
                                                <div class="row">
                                                    <div class="col-md-9">
                                                        <div class="form-group">
                                                       </div>
                                                    </div>
                                                    
                                                    <div class="col-md-3">
                                                        <br />
                                                        <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnImprimir" name="btnImprimir">
                                                            <i class="fa fa-print"></i>
                                                            Impresión en Lotes
                                                        </button>
                                                  
                                                    </div>

                                               </div>
                                                </br>
                                                <!-- Consulta Cartas de Crédito a Colaboradores-->
                                                <div class="row" id="divCartasCredito" runat="server">
                                                    <div class="col-md-12" style="overflow: auto">
                                                        <asp:GridView ID="grdCartasCredito" runat="server" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="12">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="image/resultset_first.png" LastPageImageUrl="image/resultset_last.png" NextPageImageUrl="image/resultset_next.png" PreviousPageImageUrl="image/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField HeaderText="" SelectText="Imprimir" ShowSelectButton="True">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                       <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkImprimir" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="clave" HeaderText="CLAVE" HeaderStyle-Width="10%" />
                                                                <asp:BoundField DataField="nombre" HeaderText="NOMBRE" HeaderStyle-Width="25%" />
                                                                <asp:BoundField DataField="puesto" HeaderText="PUESTO" HeaderStyle-Width="20%" />
                                                                <asp:BoundField DataField="ingles_niveles" HeaderText="NIVEL" HeaderStyle-Width="20%" />
                                                                <asp:BoundField DataField="calificacion" HeaderText="CALIFICACIÓN" HeaderStyle-Width="10%" />
                                                                <asp:BoundField DataField="asistencia" HeaderText="ASISTENCIA" HeaderStyle-Width="10%" />
                                                                <asp:TemplateField HeaderText="ELIMINAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminarAdjuntos" runat="server" Font-Size="8pt" class="label label-danger" Text="Eliminar" CommandName="Delete" ToolTip="Eliminar"> </asp:LinkButton>
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
                                                        <img id="Img1" runat="server" src="image/glyphLoading.gif" style="display: none" />

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="lblRegistrosCarta" runat="server" Text="Label"></asp:Label>
                                                    </div>
                                                </div>

                                                <!-- /.Grid  -->
                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- /.row -->

                                <!-- END CUSTOM TABS -->

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdNoNominaUsuario" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                           <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                                <!-- /.box -->

                                          <!-- /.box -->
                        <asp:HiddenField ID="hfGridView1SV" runat="server" />
                        <asp:HiddenField ID="hfGridView1SH" runat="server" />

                        <asp:HiddenField ID="hfGridView2SV" runat="server" />
                        <asp:HiddenField ID="hfGridView2SH" runat="server" />
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

        function cargarInfo() {
            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }

        function imprimeCartaCredito(id) {

            window.open('print_carta_credito.aspx?idCartaCredito=' + id, '_blank')
        }

    </script>


</body>
</html>
