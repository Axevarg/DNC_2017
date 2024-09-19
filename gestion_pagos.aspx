<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="gestion_pagos.aspx.vb" Inherits="DNC_2017.gestion_pagos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Gestion de Pagos | SIGIDO</title>
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

            gridView1 = $('#grdPagosRegistrados').gridviewScroll({
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

                }



                function combo() {
                    //$("#ddlCurso").select2();

                    $("#ddlProvColaborador").select2();

                    //closeOnSelect: false
                }


                function comportamientosJS() {
                    $('#txtFechaLimite').datepicker({
                        autoclose: true
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
                        <h1>Gestión de Pagos
                                       <small></small>
                        </h1>

                    </section>
                    <br />
                    <!-- Main content -->
                    <section class="content">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalFacturas" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalLabel2">Facturas de Pago</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div id="div3" runat="server" class="col-md-12 center-block">
                                                        <output id="listArchivo" runat="server"></output>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group">

                                                            <label for="exampleInputFile"><i class="fa fa-paperclip"></i>Cargar PDF de Factura</label>
                                                            <asp:FileUpload ID="fucArchivo" runat="server" accept="application/pdf, image/*" Width="100%" />

                                                            <p class="help-block">Tipo de archivo permitido: '.pdf'</p>
                                                        </div>
                                                        <button id="btnCargaArchivos" runat="server" class="btn btn-danger btn-flat pull-right"
                                                            name="btnCargaArchivos" onclick="return fn_CargarArchivoMenu();">
                                                            <i class="fa fa-upload"></i>Subir Archivo</button>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="col-md-2"></div>
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="col-md-8" style="overflow: auto">
                                                                    <asp:GridView ID="grdArchivos" runat="server" AllowPaging="False"
                                                                        AutoGenerateColumns="False" Width="100%"
                                                                        RowStyle-Height="10px" ShowFooter="False" class="table table-condensed table-responsive"
                                                                        Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                                        <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>

                                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:CommandField HeaderText="" SelectText="Ver" ShowSelectButton="True">
                                                                                <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                            </asp:CommandField>
                                                                            <asp:BoundField DataField="nombre" HeaderText="Archivo" HeaderStyle-Width="80%" />
                                                                            <asp:TemplateField HeaderText="ELIMINAR">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkEliminarAdjuntos" runat="server" Font-Size="8pt" class="label label-danger" Text="Eliminar" CommandName="Delete" ToolTip="Eliminar"> </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="RUTA" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRuta" runat="server" Text='<%# Bind("ruta")%>'></asp:Label>
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
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <div class="col-md-2"></div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorArchivo" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <!--Creación de Pago-->
                                <div id="divPago" runat="server" class="row">
                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Creación de Pagos </h3>

                                                <div class="box-tools pull-right">

                                                    <img id="loadingPago" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Consulta de Colaboradores por Grupo-->
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label>Busqueda de Pagos</label>
                                                            <asp:DropDownList ID="ddlPagosBuscar" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarCurso();">
                                                            </asp:DropDownList>
                                                            <img id="imgCurso" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" id="divPagoCrear" runat="server">


                                                    <!-- text input -->
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Empresa</label>
                                                            <asp:DropDownList ID="ddlEmpresa" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>A favor de</label>
                                                            <asp:DropDownList ID="ddlAFavorDe" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="cargarFavorDe();">
                                                                <asp:ListItem>Empleado</asp:ListItem>
                                                                <asp:ListItem>Tercero</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Tipo / Nombre de Proyecto</label>
                                                            <asp:DropDownList ID="ddlTipoProyecto" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Proveedor / Colaborador</label>
                                                            <asp:DropDownList ID="ddlProvColaborador" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Solicita</label>
                                                            <asp:DropDownList ID="ddlSolicitaPago" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <label for="txtPuesto">Fecha Limite</label>
                                                        <asp:TextBox ID="txtFechaLimite" runat="server" class="form-control  input-sm" MaxLength="10" Style="text-transform: uppercase" onkeypress="return caracteres(event);"
                                                            onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Moneda </label>
                                                            <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true">
                                                                <asp:ListItem>Pesos</asp:ListItem>
                                                                <asp:ListItem>Dolares</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Tipo de Cambio</label>
                                                            <asp:TextBox ID="txtTipoCambio" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="8"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label>Número de Facturas </label>
                                                            <asp:TextBox ID="txtNumeroFacturas" runat="server" class="form-control" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label>Motivo</label>
                                                            <asp:TextBox ID="txtMotivo" runat="server" class="form-control" TextMode="MultiLine" Rows="3" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="2000"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12" style="overflow: auto">
                                                        <asp:GridView ID="grdPartidas" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt"
                                                            RowStyle-Height="10px" ShowFooter="True" class="table table-condensed table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="15">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Partida" HeaderStyle-Width="5%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtPartida" runat="server" Text='<%# Bind("partida")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPartida" runat="server" Text='<%# Bind("partida")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgrePartida" runat="server" Font-Size="8pt" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cuenta" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtCuenta" runat="server" Text='<%# Bind("cuenta")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCuenta" runat="server" Text='<%# Bind("cuenta")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregaCuenta" runat="server" class="form-control input-sm" Font-Size="8pt" Style="text-transform: uppercase" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Concepto" HeaderStyle-Width="40%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtConcepto" runat="server" Text='<%# Bind("concepto")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblConcepto" runat="server" Text='<%# Bind("concepto")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreConcepto" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Centro Costo" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtCentroCosto" runat="server" Text='<%# Bind("centro_costo")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCentroCosto" runat="server" Text='<%# Bind("centro_costo")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreCentroCosto" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IVA" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtIva" runat="server" Text='<%# Bind("iva")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIva" runat="server" Text='<%# Bind("iva")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregarIva" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Importe" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtImporte" runat="server" Text='<%# Bind("importe")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblImporte" runat="server" Text='<%# Bind("importe")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregarImporte" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>


                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminarPartidas" runat="server" Font-Size="8pt" class="label label-danger" Text="Eliminar" CommandName="Delete" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregaPartidas" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregaPartidas_Click"></asp:LinkButton>
                                                                    </FooterTemplate>
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
                                                        <img id="loadingOtnp" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <button id="btnImagen" runat="server" type="button" class="btn btn-default btn-xs" data-toggle="modal" data-target="#modalFacturas"><i class="fa fa-paperclip"></i>Adjuntar Facturas</button>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="form-group">
                                                            <label>Curso Relacionado</label>
                                                            <asp:DropDownList ID="ddlCursoRelacionado" runat="server" class="form-control" Style="width: 100%;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </div>



                                                <!-- /.Grid  -->
                                            </div>

                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <button class="btn btn-danger btn-flat pull-left" runat="server" id="btnCrearPago" name="btnCrearPago" onclick="return CreaPago();">
                                                    <i class="fa fa-credit-card"></i>
                                                    Crear Nuevo Pago
                                                </button>

                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnCancelar" name="btnCancelar" onclick="return CancelaPago();">
                                                    <i class="fa fa-archive"></i>
                                                    Cancelar
                                                </button>


                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnGenerarPago" name="btnGenerarPago" onclick="return insPago();">
                                                    <i class="fa  fa-plus"></i>
                                                    Generar Pago
                                                </button>
                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!--Impresiónde Pagos-->
                                <!--Pagos Registrados-->
                                <div id="div1" runat="server" class="row">
                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Pagos Registrados</h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnExportarReporte" name="btnExportarReporte" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>

                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">


                                                <!--Datos de los Pagos-->
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
                                                <br />
                                                <!-- Consulta Cartas de Crédito a Colaboradores-->
                                                <div id="divCartasCredito" runat="server" class="row">
                                                    <div class="col-md-12" style="overflow: auto">
                                                        <asp:GridView ID="grdPagosRegistrados" runat="server" AllowPaging="False" AutoGenerateColumns="False" CellPadding="1" class="table table-condensed" Font-Size="Small" ForeColor="#333333" GridLines="None" HorizontalAlign="Justify" PageSize="12" RowStyle-Height="10px" ShowFooter="False" ShowHeaderWhenEmpty="True" Width="100%">
                                                            <PagerSettings FirstPageImageUrl="image/resultset_first.png" LastPageImageUrl="image/resultset_last.png" Mode="NextPreviousFirstLast" NextPageImageUrl="image/resultset_next.png" PageButtonCount="20" PreviousPageImageUrl="image/resultset_previous.png" />
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
                                                                <asp:TemplateField HeaderStyle-Width="5%" HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkImprimir" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="no_correlativo" HeaderStyle-Width="5%" HeaderText="No" />
                                                                <asp:BoundField DataField="a_favor" HeaderStyle-Width="10%" HeaderText=" A FAVOR" />
                                                                <asp:BoundField DataField="TIPO_PROYECTO" HeaderStyle-Width="20%" HeaderText="TIPO PROYECTO" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderStyle-Width="20%" HeaderText="NOMBRE" />
                                                                <asp:BoundField DataField="FECHA" HeaderStyle-Width="20%" HeaderText="FECHA" />
                                                                <asp:BoundField DataField="FECHA_LIMITE" HeaderStyle-Width="10%" HeaderText="FECHA_LIMITE" />
                                                                <asp:BoundField DataField="MONEDA" HeaderStyle-Width="10%" HeaderText="MONEDA" />
                                                                <asp:TemplateField HeaderText="ELIMINAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminarAdjuntos" runat="server" class="label label-danger" CommandName="Delete" Font-Size="8pt" Text="Eliminar" ToolTip="Eliminar"> </asp:LinkButton>
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
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="lblErrorPagos" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>


                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- /.row -->
                                <asp:HiddenField ID="hdPartida" runat="server" />
                                <asp:HiddenField ID="hdIdPago" runat="server" />
                                <!-- END CUSTOM TABS -->
                                <asp:HiddenField ID="hdCrearPago" runat="server" />
                            </ContentTemplate>
                            <Triggers>

                                <asp:PostBackTrigger ControlID="btnCargaArchivos" />

                                <asp:PostBackTrigger ControlID="btnExportarReporte" />

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

        function cargarFavorDe() {

            var theImg = document.getElementById("loadingPago");
            theImg.style.display = "inline";

        }
        function Monedas(ddl) {

            var vValor = ddl.selectedIndex;

            var ddlMoneda = document.getElementById("ddlMoneda");



            if (ddlMoneda.options[vValor].value == "Dolares") {
                document.getElementById("txtTipoCambio").style.visibility = false;

            } else {
                document.getElementById("txtTipoCambio").style.visibility = "";
            }

        }
        // elimina Curso
        function cancelaPago() {
            if (!confirm('¿Desea Cancelador Pago')) { return false; };
            return true;
        }
        // Inserta la Informacion Pago
        function insPago() {

            //Fecha Limite
            if (document.getElementById("txtFechaLimite").value == '') {
                alert("Debe capturar la Fecha Limite.");
                document.getElementById("txtFechaLimite").focus();
                return false;
            }

            var ddlMoneda = document.getElementById("ddlMoneda");
            if (ddlMoneda.options[ddlMoneda.selectedIndex].value == "Dolares") {
                //Tipo de Cambio
                if (document.getElementById("txtTipoCambio").value == '') {
                    alert("Debe capturar el tipo de Cambio.");
                    document.getElementById("txtTipoCambio").focus();
                    return false;
                }
            }

            //Motivo
            if (document.getElementById("txtMotivo").value == '') {
                alert("Debe capturar el Motivo.");
                document.getElementById("txtMotivo").focus();
                return false;
            }

            //creacion de Partidas
            if (document.getElementById("hdPartida").value == 0) {
                alert("Debe agregar Partidas del Pago.");
                return false;
            }

            if (!confirm('¿Al Aceptar no se podra Modificar el Pago.')) { return false; };

            document.getElementById("hdCrearPago").value = 0;

            var theControl = document.getElementById("btnGenerarPago");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingPago");
            theImg.style.display = "inline";

            return true;
        }

        function imprimePagos(id) {
            window.open('print_pago.aspx?idPago=' + id, '_blank')
        }

        // Cancelar Pago
        function CancelaPago() {
            if (!confirm('¿Desea Cancelar el Pago?')) { return false; };

            document.getElementById("hdCrearPago").value = 0;
            return true;
        }

        function CreaPago() {
            if (!confirm('¿Desea Crear un Nuevo Pago?')) { return false; };

            document.getElementById("hdCrearPago").value = 1;
        }


        function cargarCurso() {

            var theImg = document.getElementById("imgCurso");
            theImg.style.display = "inline";

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



            return true;
        }
    </script>


</body>
</html>
