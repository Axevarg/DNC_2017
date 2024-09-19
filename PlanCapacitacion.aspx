<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PlanCapacitacion.aspx.vb" Inherits="DNC_2017.PlanCapacitacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Plan de Capacitación | Capacitación</title>
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
            gridviewScrollPlan();
            gridView1 = $('#grdCursosRegistrados').gridviewScroll({
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

            function gridviewScrollPlan() {
                gridView1 = $("#<%=GrdPlanCapacitacion.ClientID%>").gridviewScroll({
                    width: 'auto',
                    height: 300,
                    headerrowcount: 1,

                    startVertical: $("#<%=hfGridView2SV.ClientID%>").val(),
                    startHorizontal: $("#<%=hfGridView2SH.ClientID%>").val(),
                    onScrollVertical: function (delta) {
                        $("#<%=hfGridView1SV.ClientID%>").val(delta);
                    },
                    onScrollHorizontal: function (delta) {
                        $("#<%=hfGridView1SH.ClientID%>").val(delta);
                    }


                });

                }

                function cargarBuscar() {
                    var theImg = document.getElementById("imgCargar");
                    theImg.style.display = "inline";
                }

                function ComportamientosJS() {

                    $("#ddlCursos").select2();
                    $("#ddlCursosPlan").select2();
                    
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
                        <h1>Plan de Capacitación     
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- Cursos Solicitados -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h4>Autorización </h4>
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4" id="div3" runat="server">
                                        <div class="form-group">
                                            <label>DNC</label>
                                            <asp:DropDownList ID="ddlDnc" runat="server" class="form-control input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarBuscar();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div1" runat="server">
                                        <div class="form-group">
                                            <label>Dirección</label>
                                            <asp:DropDownList ID="ddlDireccion" runat="server" class="form-control  input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarBuscar();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div2" runat="server">
                                        <div class="form-group">
                                            <label>
                                                Gerencia/Departamentos
                                            </label>
                                            <asp:DropDownList ID="ddlDepartamento" runat="server" class="form-control input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarBuscar();"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="divCursosGNC" runat="server" class="row">

                                    <div class="col-md-12">

                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">

                                                <!-- Grid General-->
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <img id="imgCargar" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        <strong>
                                                            <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
                                                        </strong>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <!--Fin  Modal -->
                                                    <div class="col-md-8" id="divCursos" runat="server">
                                                        <div class="form-group">
                                                            <label>Cursos registrados en la DNC</label>
                                                            <asp:DropDownList ID="ddlCursos" class="form-control  input-sm select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarBuscar();">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Estatus</label>
                                                            <asp:DropDownList ID="ddlEstatus" class="form-control  input-sm select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarBuscar();">
                                                                <asp:ListItem Value="Todos">Todos</asp:ListItem>
                                                                <asp:ListItem Value="1">Autorizado</asp:ListItem>
                                                                <asp:ListItem Value="0">Solicitado</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <br />

                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdCursosRegistrados" runat="server"
                                                            AllowPaging="False" AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" class="table table-condensed" Style="font-size: 7pt;"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="ChkTodos" runat="server" onclick="SelectAllCheckboxes(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkClave" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIdDNC" runat="server" Text='<%# Bind("ID_DNC")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CURSO" HeaderText="Curso" />
                                                                <asp:BoundField DataField="estatus" HeaderText="Estatus" ReadOnly="True" ItemStyle-Wrap="false" />
                                                                <asp:BoundField DataField="COLABORADOR" HeaderText="Colaborador" />
                                                                <asp:BoundField DataField="puesto" HeaderText="Puesto" />
                                                                <asp:BoundField DataField="GERENCIAS" HeaderText="Gerencia/Depatamento" />
                                                                <asp:BoundField DataField="REGISTRO" HeaderText="Registro Curso" />
                                                                <asp:BoundField DataField="unidad_negocio" HeaderText="Unidad Negocio" />
                                                                <asp:BoundField DataField="ESTATUS_COLABORADOR" HeaderText="Estatus Colaborador" />
                                                                <asp:TemplateField HeaderText="CLAVE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblClave" runat="server" Text='<%# Bind("CLAVE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FK_ID_CURSO" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIdCurso" runat="server" Text='<%# Bind("FK_ID_CURSO")%>'></asp:Label>
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
                                                <!-- /.Grid General -->



                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lnkGuardarAutorizacion" runat="server" class="btn btn-danger btn-flat pull-left" OnClientClick="return GuardarPlan();" OnClick="lnkGuardarAutorizacion_Click"><i class="fa fa-save"></i> Guardar Autorización de Cursos </asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <img id="loadingAccion" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-4" style="text-align: right">
                                                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>


                                            </div>
                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- Plan de Capacitación-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h4>Plan de Capacitación </h4>
                                        <asp:Label ID="lblErrorPlan" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4" id="div6" runat="server">
                                        <div class="form-group">
                                            <label>DNC</label>
                                            <asp:DropDownList ID="ddlDncPlanCapacitacion" runat="server" class="form-control input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarPlan();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div7" runat="server">
                                        <div class="form-group">
                                            <label>Dirección</label>
                                            <asp:DropDownList ID="ddlDireccionPlan" runat="server" class="form-control  input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarPlan();"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div8" runat="server">
                                        <div class="form-group">
                                            <label>
                                                Gerencia/Departamentos
                                            </label>
                                            <asp:DropDownList ID="ddlDepartamentoPlan" runat="server" class="form-control input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarPlan();"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="div4" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">

                                            <!-- /.box-header -->
                                            <div class="box-body">

                                                <!-- Grid General-->
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <img id="imgPlan" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <!--Fin  Modal -->
                                                    <div class="col-md-12" id="div5" runat="server">
                                                        <div class="form-group">
                                                            <label>Cursos Autorizados DNC</label>
                                                            <asp:DropDownList ID="ddlCursosPlan" class="form-control  input-sm select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarPlan();">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <br />
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GrdPlanCapacitacion" runat="server"
                                                            AllowPaging="False" AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" class="table table-condensed" Style="font-size: 7pt;"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="ChkTodos" runat="server" onclick="SelectAllCheckboxesPlan(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkClave" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID_PLAN")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CURSO" HeaderText="Curso" />
                                                                <asp:BoundField DataField="COLABORADOR" HeaderText="Colaborador" />
                                                                <asp:BoundField DataField="puesto" HeaderText="Puesto" />
                                                                <asp:BoundField DataField="GERENCIAS" HeaderText="Gerencia/Depatamento" />
                                                                <asp:BoundField DataField="ANIO" HeaderText="Año" />
                                                                <asp:BoundField DataField="MES" HeaderText="Mes" />
                                                                <asp:BoundField DataField="unidad_negocio" HeaderText="Unidad Negocio" />
                                                                <asp:BoundField DataField="ESTATUS_COLABORADOR" HeaderText="Estatus Colaborador" />
                                                                <asp:TemplateField HeaderText="CLAVE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblClave" runat="server" Text='<%# Bind("CLAVE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FK_ID_CURSO" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIdCurso" runat="server" Text='<%# Bind("FK_ID_CURSO")%>'></asp:Label>
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

                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Label ID="lblMensajePlan" runat="server" Text=""></asp:Label>
                                                    </div>

                                                </div>
                                                <!-- /.Grid General -->

                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">

                                                <div class="row">
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <img id="loadingPlan" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-4" style="text-align: right">
                                                        <asp:Label ID="lblTotalPlan" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row" id="divProgramar" runat="server">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Mes de Programación</label>
                                                            <asp:DropDownList ID="ddlMes" runat="server" class="form-control input-sm" Width="100%">
                                                                <asp:ListItem>Enero</asp:ListItem>
                                                                <asp:ListItem>Febrero</asp:ListItem>
                                                                <asp:ListItem>Marzo</asp:ListItem>
                                                                <asp:ListItem>Abril</asp:ListItem>
                                                                <asp:ListItem>Mayo</asp:ListItem>
                                                                <asp:ListItem>Junio</asp:ListItem>
                                                                <asp:ListItem>Julio</asp:ListItem>
                                                                <asp:ListItem>Agosto</asp:ListItem>
                                                                <asp:ListItem>Septiembre</asp:ListItem>
                                                                <asp:ListItem>Octubre</asp:ListItem>
                                                                <asp:ListItem>Noviembre</asp:ListItem>
                                                                <asp:ListItem>Diciembre</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>


                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Año de Programación</label>
                                                            <asp:DropDownList ID="ddlAnio" runat="server" class="form-control input-sm" Width="100%" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4" style="text-align: right">
                                                        <br />
                                                        <asp:LinkButton ID="lnkProgramarPlan" runat="server" class="btn btn-danger btn-flat pull-right" OnClientClick="return ValidaPlanCapacitacion();" OnClick="lnkProgramarPlan_Click"><i class="fa fa-tachometer"></i> Programar Plan de Capacitación </asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
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
                        <asp:HiddenField ID="hfGridView2SV" runat="server" />
                        <asp:HiddenField ID="hfGridView2SH" runat="server" />
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
        function cargarInfo() {
            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";
        }
        function cargarPlan() {
            var theImg = document.getElementById("imgPlan");
            theImg.style.display = "inline";
        }

        function GuardarPlan() {
            var valid = false;
            var strColaboradores = '';
            var lblMensaje = document.getElementById('<%= lblMensaje.ClientID%>').innerHTML;

            var theImg = document.getElementById("loadingAccion");
            theImg.style.display = "inline";
            return true;
        }
        function SelectAllCheckboxes(objHeaderChk) {

            var theImg = document.getElementById("loadingAccion");
            theImg.style.display = "inline";

            var IsChecked = objHeaderChk.checked;
            var tbl = document.getElementById('<%= grdCursosRegistrados.ClientID%>');
            var items = tbl.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
            theImg.style.display = "none";
        }
        // Seleccion de Checks
        function SelectAllCheckboxesPlan(objHeaderChk) {

            var theImg = document.getElementById("loadingPlan");
            theImg.style.display = "inline";

            var IsChecked = objHeaderChk.checked;
            var tbl = document.getElementById('<%= GrdPlanCapacitacion.ClientID%>');
            var items = tbl.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
            theImg.style.display = "none";
        }
        // Valida Plan
        function ValidaPlanCapacitacion() {
            var valid = false;
            var strColaboradores = '';
            var lblMensajeColaborador = document.getElementById('<%= lblMensajePlan.ClientID%>').innerHTML;

            //Plan de Capacitacion
            if (lblMensajeColaborador == '') {
                var gdvw = document.getElementById('<%= GrdPlanCapacitacion.ClientID%>');
                for (var i = 1; i < gdvw.rows.length; i++) {
                    var value = gdvw.rows[i].getElementsByTagName('input');
                    if (value != null) {
                        if (value[0].type == "checkbox") {
                            if (value[0].checked) {
                                valid = true;
                            }
                        }
                    }
                }
            }

            //Valida si hay colaboradores seleccioonados
            if (valid == false) {
                alert("Por favor, seleccione al menos un Curso Autorizado.");
                return false;
            }

            if (!confirm('Al Aceptar se guardar el mes y año indicado en las personas marcadas.')) { return false; };
            return true;
        }
    </script>
</body>
</html>
