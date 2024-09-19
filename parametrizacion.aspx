<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="parametrizacion.aspx.vb" Inherits="DNC_2017.parametrizacion" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Parametrización | SIGIDO</title>
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
                <br />
                <br />
                <div class="container">
                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Parametrización
          
          <small>DNC</small>
                        </h1>

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
                                    <!-- Catalogo de Condiciones -->
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Configuración de DNC</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorProveedor" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-12" id="divAvance" runat="server">
                                                        <asp:GridView ID="grdParametrizacion" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="15">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="NOMBRE" HeaderStyle-Width="30%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtNombre" runat="server" Text='<%# Bind("nombre")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("nombre")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregaNombre" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA INICIO REGISTRO" HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaInicio" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_registro_inicial")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Bind("fecha_registro_inicial")%>' ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaInicio" ClientIDMode="Static" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA FINAL REGISTRO" HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaFinal" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_registro_final")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaFinal" runat="server" Text='<%# Bind("fecha_registro_final")%>' ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaFinal" ClientIDMode="Static" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="OPCIONES DE CONSULTA">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlConsultas" runat="server" class="form-control input-sm">
                                                                            <asp:ListItem Value="SI">CONSULTAR CERRADO EL PERIODO</asp:ListItem>
                                                                            <asp:ListItem Value="NO">NO CONSULTAR CERRADO EL PERIODO</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblConsultas" runat="server" Text='<%# Bind("consulta_registros")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreConsultas" runat="server" class="form-control input-sm">
                                                                            <asp:ListItem Value="SI">CONSULTAR CERRADO EL PERIODO</asp:ListItem>
                                                                            <asp:ListItem Value="NO">NO CONSULTAR CERRADO EL PERIODO</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="ESTATUS">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control input-sm">
                                                                            <asp:ListItem>VIGENTE</asp:ListItem>
                                                                            <asp:ListItem>TERMINADO</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control input-sm">
                                                                            <asp:ListItem>VIGENTE</asp:ListItem>

                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">

                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="ELIMINAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregarCondicion" runat="server" Text="Agregar" Font-Size="8pt" class="label label-danger" ToolTip="Agregar" OnClick="lnkAgregarCondicion_Click"></asp:LinkButton>
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
                                                        <img id="loadingPagina" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                    </div>

                                                </div>

                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                </div>

                                <!-- Catalogo de Años Propuestos -->
                                <div class="col-md-12">
                                    <div class="box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Catálogos de Años Propuestos</h3>
                                            <div class="box-tools pull-right">
                                            </div>
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblErrorAnios" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                </div>
                                            </div>

                                            <div class="row">

                                                <!-- /.Grid de Catalogos-->
                                                <div class="col-md-12" id="div1" runat="server">
                                                    <asp:GridView ID="grdAnioPropuesto" runat="server" AllowPaging="True"
                                                        AutoGenerateColumns="False" Width="100%"
                                                        RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                        Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                        <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DNC" HeaderStyle-Width="40%">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlDnc" runat="server" class="form-control input-sm">
                                                                    </asp:DropDownList>

                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDNC" runat="server" Text='<%# Bind("fk_id_parametrizacion")%>'></asp:Label>

                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAgreDnc" runat="server" class="form-control input-sm">
                                                                    </asp:DropDownList>

                                                                </FooterTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="AÑO" HeaderStyle-Width="40%">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtAnio" runat="server" Text='<%# Bind("anio")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAnio" runat="server" Text='<%# Bind("anio")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAgregaAnio" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="50%" />
                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ESTATUS">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control">
                                                                        <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                        <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control">
                                                                        <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="ELIMINAR">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton ID="lnkAgregarFacilitador" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarFacilitador_Click"></asp:LinkButton>
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
                                                    <img id="Img1" runat="server" src="img/glyphLoading.gif" style="display: none" />


                                                </div>
                                                <!-- /.Grid de Catalogos-->
                                            </div>
                                        </div>
                                        <!-- ./box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>


                                <!--Limite de Dnc-->
                                <div class="row">
                                    <!-- Catalogo de Condiciones -->
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Configuración de DNC</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6" id="div3" runat="server">
                                                        <div class="form-group">
                                                            <label>DNC</label>
                                                            <asp:DropDownList ID="ddlDnc" runat="server" class="form-control" Style="width: 100%;" AutoPostBack="true" onchange="CargarDnc();"></asp:DropDownList>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6" id="div5" runat="server">
                                                        <img id="imgDNC" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>

                                                    <div class="col-md-12" id="div2" runat="server" visible="false">
                                                        <asp:GridView ID="grdLimiteDnc" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
                                                            RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PUESTO" HeaderStyle-Width="30%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtPuesto" runat="server" Text='<%# Bind("puesto")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="150"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPuesto" runat="server" Text='<%# Bind("puesto")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgrePuesto" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="150"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="# PERSONA" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtNumPersona" runat="server" Text='<%# Bind("numero_personas")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNumPersona" runat="server" Text='<%# Bind("numero_personas")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregaNumPersona" runat="server" class="form-control input-sm" Font-Size="8pt" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="CUOTA PRELIMINAR" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtCuotaPre" runat="server" Text='<%# Bind("cuota_preliminar")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCuotaPre" runat="server" Text='<%# Bind("cuota_preliminar")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregarCuotaPre" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CUOTA POR PERSONA" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtCuotaPersona" runat="server" Text='<%# Bind("cuota_por_persona")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblImporte" runat="server" Text='<%# Bind("cuota_por_persona")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregarCuotaPersona" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="HORAS ANUALES" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtHorasAnuales" runat="server" Text='<%# Bind("horas_anuales")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHorasAnuales" runat="server" Text='<%# Bind("horas_anuales")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregaHorasAnuales" runat="server" class="form-control input-sm" Font-Size="8pt" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TOTAL" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtTotal" runat="server" Text='<%# Bind("total")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("total")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgregarTotal" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="OPERATIVO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlOperativo" runat="server" class="form-control  input-sm" Font-Size="8pt">
                                                                            <asp:ListItem Value="1">Sí</asp:ListItem>
                                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOpertativo" runat="server" Text='<%# Bind("operativo")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreOperativo" runat="server" class="form-control  input-sm" Font-Size="8pt">
                                                                            <asp:ListItem Value="1">Sí</asp:ListItem>
                                                                            <asp:ListItem Value="0">No</asp:ListItem>
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
                                                                        <asp:LinkButton ID="lnkAgregarConcepto" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarConcepto_Click"></asp:LinkButton>
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
                                                        <img id="loadingLimite" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                    </div>

                                                </div>

                                                <div class="row">

                                                    <!-- /.Grid de Parametro-->
                                                    <div class="col-md-12" id="div4" runat="server">
                                                        <asp:GridView ID="grdParametro" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Número Cursos Registro DNC" HeaderStyle-Width="45%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtNoCursos" runat="server" Text='<%# Bind("numero_cursos")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNoCursos" runat="server" Text='<%# Bind("numero_cursos")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ajuste Inflación" HeaderStyle-Width="45%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtAjusteI" runat="server" Text='<%# Bind("ajuste_inflacion")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAjusteI" runat="server" Text='<%# Bind("ajuste_inflacion")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>



                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>

                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <PagerStyle BackColor="White" ForeColor="White" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                        </asp:GridView>
                                                        <img id="Img2" runat="server" src="img/glyphLoading.gif" style="display: none" />


                                                    </div>
                                                    <!-- /.Grid de Catalogos-->
                                                </div>

                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>

                                    <!-- Catalogo de Parametros -->
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Configuración de Presupuesto de Gestión de la Capacitación</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorPresupuesto" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>
                                                <!-- Encabezado de Presupuesto -->
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GrdPresupuesto" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Presupuesto" HeaderStyle-Width="30%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtPresupuesto" runat="server" Text='<%# Bind("presupuesto")%>' class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="200"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPresupuesto" runat="server" Text='<%# Bind("presupuesto")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgrePresupuesto" runat="server" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="200"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Monto " HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtMontoPresupuestos" runat="server" Text='<%# Bind("monto")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMontoPresupuestos" runat="server" Text='<%# Bind("monto_pesos")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreMontoPresupuestos" runat="server" Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="20"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Comentario" HeaderStyle-Width="40%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtComentario" runat="server" Text='<%# Bind("comentarios")%>' class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblComentario" runat="server" Text='<%# Bind("comentarios")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreComentario" runat="server" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>



                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregarPresupuestoEncabezado" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPresupuestoEncabezado_Click"></asp:LinkButton>
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
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GrdPresupuestoDetalle" runat="server" AllowPaging="True"
                                                            AutoGenerateColumns="False" Width="100%"
                                                            RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Presupuesto" HeaderStyle-Width="15%">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPresupuesto" runat="server" Text='<%# Bind("Presupuesto")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgrePresupuesto" runat="server" class="form-control  input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlAgrePor_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Por " HeaderStyle-Width="11%">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPor" runat="server" Text='<%# Bind("Por")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgrePor" runat="server" class="form-control  input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlAgrePor_SelectedIndexChanged">
                                                                            <asp:ListItem Value="DirA">Dirección A</asp:ListItem>
                                                                            <asp:ListItem Value="DirB">Dirección B</asp:ListItem>
                                                                            <asp:ListItem Value="Ger">Gerencia</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Área " HeaderStyle-Width="25%">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblArea" runat="server" Text='<%# Bind("Area")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreArea" runat="server" class="form-control  input-sm">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Aplica a " HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlAplica" runat="server" class="form-control  input-sm">
                                                                            <asp:ListItem>Todos</asp:ListItem>
                                                                            <asp:ListItem>Administrativos</asp:ListItem>
                                                                            <asp:ListItem>Opertativos</asp:ListItem>

                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAplica" runat="server" Text='<%# Bind("APLICA")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreAplica" runat="server" class="form-control  input-sm">
                                                                            <asp:ListItem>Todos</asp:ListItem>
                                                                            <asp:ListItem>Administrativos</asp:ListItem>
                                                                            <asp:ListItem>Opertativos</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Porcentaje " HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtPorcentaje" runat="server" Text='<%# Bind("monto_porcentaje")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPorcentaje" runat="server" Text='<%# Bind("porcentaje")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgrePorcentaje" runat="server" Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="3"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Monto " HeaderStyle-Width="10%">
                                                                  
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMontoPresupuestos" runat="server" Text='<%# Bind("monto")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                
                                                                </asp:TemplateField>

                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregarPresupuesto" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPresupuesto_Click"></asp:LinkButton>
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
                                                        <img id="Img3" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTotal" runat="server" Text="Label"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- ./box-body -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                                <asp:HiddenField ID="hdIndexPresupuesto" runat="server" />
                            </ContentTemplate>

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
        function CargarDnc() {

            var theImg = document.getElementById("imgDNC");
            theImg.style.display = "inline";

        }
    </script>
</body>
</html>
