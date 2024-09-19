<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="catalogos.aspx.vb" Inherits="DNC_2017.catalogos" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Configuración de Catálogos | SIGIDO</title>
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
        function combo() {

            $("#ddlJefe").select2();
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
                        <h1>

                            <small>Configuración de Catálogos</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!--Catalogos Generales-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos Generales SIGIDO</h3>
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

                                                    <div class="col-md-12" id="div7" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="Li1" runat="server" class="active"><a href="#tabGeneral_1" data-toggle="tab">Servicios Proveedor</a></li>


                                                            </ul>
                                                            <!-- Competencia vinculada -->
                                                            <div class="tab-content">
                                                                <div class="tab-pane active" id="tabGeneral_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div9" runat="server">
                                                                            <asp:GridView ID="grdServicioProveedor" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarServicioPro" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarServicioPro_Click"></asp:LinkButton>
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


                                                                        </div>
                                                                        <img id="loadingCatalogos" runat="server" src="img/glyphLoading.gif" style="display: none" />
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

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>
                                <!-- Catalogo de DNC -->
                                <div class="row">

                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos de DNC</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblError" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-12" id="divAvance" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="lnkTabDnc1" runat="server" class="active"><a href="#tabDnc_1" data-toggle="tab" onclick="controlTabsDNC(1);">Competencias Vinculadas</a></li>
                                                                <li id="lnkTabDnc2" runat="server"><a href="#tabDnc_2" data-toggle="tab" onclick="controlTabsDNC(2);">Habilidades DINA</a></li>
                                                                <li id="lnkTabDnc3" runat="server"><a href="#tabDnc_3" data-toggle="tab" onclick="controlTabsDNC(3);">Medir Efectividad</a></li>
                                                                <li id="lnkTabDnc4" runat="server"><a href="#tabDnc_4" data-toggle="tab" onclick="controlTabsDNC(4);">Motivos</a></li>
                                                                <li id="lnkTabDnc5" runat="server"><a href="#tabDnc_5" data-toggle="tab" onclick="controlTabsDNC(5);">Obj. Corporativos</a></li>
                                                                <li id="lnkTabDnc6" runat="server"><a href="#tabDnc_6" data-toggle="tab" onclick="controlTabsDNC(6);">Tipo Indicador</a></li>
                                                                <li id="lnkTabDnc7" runat="server"><a href="#tabDnc_7" data-toggle="tab" onclick="controlTabsDNC(7);">Indicador</a></li>
                                                                <li id="lnkTabDnc8" runat="server"><a href="#tabDnc_8" data-toggle="tab" onclick="controlTabsDNC(8);">Estatus</a></li>
                                                                <li id="lnkTabDnc9" runat="server"><a href="#tabDnc_9" data-toggle="tab" onclick="controlTabsDNC(9);">Modalidad</a></li>
                                                            </ul>
                                                            <!-- Competencia vinculada -->
                                                            <div class="tab-content">
                                                                <div class="tab-pane active" id="tabDnc_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div1" runat="server">
                                                                            <asp:GridView ID="grdCompetenciasV" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
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

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregaDescripcion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="DEFINICION" HeaderStyle-Width="50%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDefinicion" runat="server" Text='<%# Bind("definicion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDefinicion" runat="server" Text='<%# Bind("definicion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregarDefinicion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar Descripción"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarCondicion" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar Descripción" OnClick="lnkAgregarCondicion_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Habilidades DINA -->
                                                                <div class="tab-pane" id="tabDnc_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div2" runat="server">
                                                                            <asp:GridView ID="grdHabilidadesDina" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarHabilidades" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarHabilidades_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Medir Efectividad-->
                                                                <div class="tab-pane" id="tabDnc_3" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div3" runat="server">
                                                                            <asp:GridView ID="grdMedirEfectividad" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarMedir" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarMedir_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Motivos -->
                                                                <div class="tab-pane" id="tabDnc_4" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div4" runat="server">
                                                                            <asp:GridView ID="grdMotivo" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
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

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregaDescripcion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="DEFINICION" HeaderStyle-Width="50%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDefinicion" runat="server" Text='<%# Bind("definicion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDefinicion" runat="server" Text='<%# Bind("definicion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregarDefinicion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar Descripción"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarMotivo" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar Descripción" OnClick="lnkAgregarMotivo_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Objetivos Corporativos -->
                                                                <div class="tab-pane" id="tabDnc_5" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div5" runat="server">
                                                                            <asp:GridView ID="grdObjetivosCorporativos" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarObjetivosCor" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarObjetivosCor_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                        <div class="col-md-12" id="div12" runat="server">
                                                                            <asp:GridView ID="grdObjetivosCorpDet" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
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
                                                                                    <asp:TemplateField HeaderText="Objetivo" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlObjetivoDet" runat="server" class="form-control input-sm" Font-Size="8pt">
                                                                                            </asp:DropDownList>

                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblObjetivoDet" runat="server" Text='<%# Bind("fk_id_objetivo_corporativo")%>'></asp:Label>

                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreObjetivoDet" runat="server" class="form-control input-sm" Font-Size="8pt">
                                                                                            </asp:DropDownList>

                                                                                        </FooterTemplate>

                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="DIRECCION" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDireccion" runat="server" Text='<%# Bind("direccion")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDireccion" runat="server" Text='<%# Bind("direccion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregarDireccion" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="30%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="2000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="2000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar Descripción"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarObjetivosDet" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar Descripción" OnClick="lnkAgregarObjetivosDet_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Tipo Indicador-->
                                                                <div class="tab-pane" id="tabDnc_6" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div6" runat="server">
                                                                            <asp:GridView ID="grdTipoIndicador" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
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

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="DEFINICION" HeaderStyle-Width="50%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDefinicion" runat="server" Text='<%# Bind("definicion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDefinicion" runat="server" Text='<%# Bind("definicion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregarDefinicion" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar Descripción"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTipoIndicador" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar Descripción" OnClick="lnkAgregarTipoIndicador_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <!--  Indicador -->
                                                                <div class="tab-pane" id="tabDnc_7" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div8" runat="server">
                                                                            <asp:GridView ID="grdIndicador" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
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
                                                                                    <asp:TemplateField HeaderText="TIPO INDICADOR" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlTipoIndicador" runat="server" class="form-control input-sm" Font-Size="8pt">
                                                                                            </asp:DropDownList>

                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTipoIndicador" runat="server" Text='<%# Bind("fk_id_tipo_indicador")%>'></asp:Label>

                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreTipoIndicador" runat="server" class="form-control input-sm" Font-Size="8pt">
                                                                                            </asp:DropDownList>

                                                                                        </FooterTemplate>

                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregaDescripcion" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="DEFINICION" HeaderStyle-Width="30%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDefinicion" runat="server" Text='<%# Bind("definicion")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDefinicion" runat="server" Text='<%# Bind("definicion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgregarDefinicion" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="6000"></asp:TextBox>
                                                                                        </FooterTemplate>

                                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Font-Size="8pt">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="ELIMINAR">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" Font-Size="8pt" class="label label-danger" CommandName="Delete" ToolTip="Eliminar Descripción"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarIndicador" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar Descripción" OnClick="lnkAgregarIndicador_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <!-- /.tab-pane -->
                                                                <!-- Medir Efectividad-->
                                                                <div class="tab-pane" id="tabDnc_8" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div24" runat="server">
                                                                            <asp:GridView ID="grdEstatusDNC" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarEstatus" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarEstatus_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Modalidad -->
                                                                <div class="tab-pane" id="tabDnc_9" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div26" runat="server">
                                                                            <asp:GridView ID="grdModalidadCurso" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarModalidadCurso" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarModalidadCurso_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- /.tab-pane -->
                                                            </div>
                                                            <!-- /.tab-content -->
                                                        </div>
                                                        <!-- nav-tabs-custom -->
                                                        <img id="loadingCatalogosDNC" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                    </div>

                                                </div>

                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>

                                <!--Catalogos Becas-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos de Becas</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorBecas" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-12" id="div10" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="lnkTabBecas1" runat="server" class="active"><a href="#tabBecas_1" data-toggle="tab" onclick="controlTabsBecas(1);">Tipo</a></li>
                                                                <li id="lnkTabBecas2" runat="server"><a href="#tabBecas_2" data-toggle="tab" onclick="controlTabsBecas(2);">Período Asignación</a></li>
                                                                <li id="lnkTabBecas3" runat="server"><a href="#tabBecas_3" data-toggle="tab" onclick="controlTabsBecas(3);">Tipo Asignación</a></li>
                                                                <li id="lnkTabBecas4" runat="server"><a href="#tabBecas_4" data-toggle="tab" onclick="controlTabsBecas(4);">Modalidad Estudio</a></li>
                                                                <li id="lnkTabBecas5" runat="server"><a href="#tabBecas_5" data-toggle="tab" onclick="controlTabsBecas(5);">Tipo Proyecto</a></li>
                                                                <li id="lnkTabBecas6" runat="server"><a href="#tabBecas_6" data-toggle="tab" onclick="controlTabsBecas(6);">Tipo Estatus</a></li>
                                                                <li id="lnkTabBecas7" runat="server"><a href="#tabBecas_7" data-toggle="tab" onclick="controlTabsBecas(7);">Modalidad Pago</a></li>
                                                                <li id="lnkTabBecas8" runat="server"><a href="#tabBecas_8" data-toggle="tab" onclick="controlTabsBecas(8);">Estatus Pago </a></li>
                                                                <li id="lnkTabBecas9" runat="server"><a href="#tabBecas_9" data-toggle="tab" onclick="controlTabsBecas(9);">Concepto Pago</a></li>

                                                            </ul>

                                                            <div class="tab-content">
                                                                <!-- Tipo de Beca -->
                                                                <div class="tab-pane active" id="tabBecas_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:GridView ID="grdTipoBeca" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTipoBeca" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTipoBeca_Click"></asp:LinkButton>
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
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Período Asginación-->
                                                                <div class="tab-pane" id="tabBecas_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div13" runat="server">
                                                                            <asp:GridView ID="grdPeriodoAsignacion" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="50%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="VERANOS" HeaderStyle-Width="20%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlVeranos" runat="server" class="form-control">
                                                                                                <asp:ListItem Value="Si">Incluye</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No Incluye</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblVeranos" runat="server" Text='<%# Bind("incluye_veranos")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreVeranos" runat="server" class="form-control">
                                                                                                <asp:ListItem Value="Si">Incluye</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No Incluye</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarPeriodoAsignacion" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPeriodoAsignacion_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Tipo Asignación-->
                                                                <div class="tab-pane" id="tabBecas_3" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div15" runat="server">
                                                                            <asp:GridView ID="grdTipoAsignacion" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTipoAsignacion" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTipoAsignacion_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!---Modalidad Estudio -->
                                                                <div class="tab-pane" id="tabBecas_4" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div17" runat="server">
                                                                            <asp:GridView ID="grdModalidadEstudio" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarModalidadEstudio" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarModalidadEstudio_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Tipo Proyecto-->
                                                                <div class="tab-pane" id="tabBecas_5" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div19" runat="server">
                                                                            <asp:GridView ID="grdTProyecto" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTProyecto" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTProyecto_Click"></asp:LinkButton>
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


                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Tipo Estatus-->
                                                                <div class="tab-pane" id="tabBecas_6" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div21" runat="server">
                                                                            <asp:GridView ID="grdTipoEstatus" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTEstatus" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTEstatus_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Modalidad Pago-->
                                                                <div class="tab-pane" id="tabBecas_7" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div23" runat="server">
                                                                            <asp:GridView ID="grdModalidadPago" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarModalidadP" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarModalidadP_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Estatus Pago-->
                                                                <div class="tab-pane" id="tabBecas_8" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div25" runat="server">
                                                                            <asp:GridView ID="grdEstatusPago" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarEstatusPago" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarEstatusPago_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--Concepto de Pago--->
                                                                <div class="tab-pane" id="tabBecas_9" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div27" runat="server">
                                                                            <asp:GridView ID="grdConceptoPago" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarConceptoPago" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarConceptoPago_Click"></asp:LinkButton>
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

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <!-- /.tab-pane -->
                                                            </div>
                                                            <!-- /.tab-content -->
                                                            <img id="loadingBecas" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                        </div>
                                                        <!-- nav-tabs-custom -->
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>

                                <!--Catalogos General Becas Ingles-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos Becas Ingles</h3>
                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorIngles" runat="server" Font-Bold="False" ForeColor="Red" Text="" />
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-md-12" id="div11" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="lnkTabBecasIngles1" runat="server" class="active"><a href="#tabBecasIngles_1" data-toggle="tab" onclick="controlTabsBecasIngles(1);">Tipo Autorización</a></li>
                                                                <li id="lnkTabBecasIngles2" runat="server"><a href="#tabBecasIngles_2" data-toggle="tab" onclick="controlTabsBecasIngles(2);">Horario Estudios</a></li>
                                                                <li id="lnkTabBecasIngles3" runat="server"><a href="#tabBecasIngles_3" data-toggle="tab" onclick="controlTabsBecasIngles(3);">Plantel</a></li>
                                                                <li id="lnkTabBecasIngles4" runat="server"><a href="#tabBecasIngles_4" data-toggle="tab" onclick="controlTabsBecasIngles(4);">Niveles</a></li>
                                                            </ul>
                                                            <!-- Competencia vinculada -->
                                                            <div class="tab-content">
                                                                <div class="tab-pane active" id="tabBecasIngles_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div14" runat="server">
                                                                            <asp:GridView ID="grdInglesTipoAutorizacion" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTipoAutorizacionIngles" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTipoAutorizacionIngles_Click"></asp:LinkButton>
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


                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane" id="tabBecasIngles_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div16" runat="server">
                                                                            <asp:GridView ID="grdInglesHorario" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarHorarioIngles" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarHorarioIngles_Click"></asp:LinkButton>
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


                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane" id="tabBecasIngles_3" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div20" runat="server">
                                                                            <asp:GridView ID="grdPlantelIngles" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarPlantelI" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPlantelI_Click"></asp:LinkButton>
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


                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="tab-pane" id="tabBecasIngles_4" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div18" runat="server">
                                                                            <asp:GridView ID="grdNiveles" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarNiveles" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarNiveles_Click"></asp:LinkButton>
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


                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <img id="imgBecaIngles" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                                <!-- /.tab-pane -->
                                                            </div>
                                                            <!-- /.tab-content -->
                                                        </div>
                                                        <!-- nav-tabs-custom -->
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>

                                <!--Catalogos Gestion de Capacitación-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos Gestión de Capacitación</h3>
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
                                                                <li id="lnkTabGestionCap1" runat="server" class="active"><a href="#tabGestionCap_1" data-toggle="tab" onclick="controlTabsGestionCapacitacion(1);">Estatus Capacitación</a></li>
                                                                <li id="lnkTabGestionCap2" runat="server"><a href="#tabGestionCap_2" data-toggle="tab" onclick="controlTabsGestionCapacitacion(2);">Tipo Agente</a></li>
                                                                <li id="lnkTabGestionCap3" runat="server"><a href="#tabGestionCap_3" data-toggle="tab" onclick="controlTabsGestionCapacitacion(3);">Área Tematica</a></li>
                                                                <li id="lnkTabGestionCap4" runat="server"><a href="#tabGestionCap_4" data-toggle="tab" onclick="controlTabsGestionCapacitacion(4);">Clave Capacitación</a></li>
                                                                <li id="lnkTabGestionCap5" runat="server"><a href="#tabGestionCap_5" data-toggle="tab" onclick="controlTabsGestionCapacitacion(5);">Establecimientos</a></li>
                                                                <li id="lnkTabGestionCap6" runat="server"><a href="#tabGestionCap_6" data-toggle="tab" onclick="controlTabsGestionCapacitacion(6);">Modalidad</a></li>
                                                                <li id="lnkTabGestionCap7" runat="server"><a href="#tabGestionCap_7" data-toggle="tab" onclick="controlTabsGestionCapacitacion(7);">Clave Curso</a></li>
                                                                <li id="lnkTabGestionCap8" runat="server"><a href="#tabGestionCap_8" data-toggle="tab" onclick="controlTabsGestionCapacitacion(8);">Ocupación DC3</a></li>
                                                            </ul>

                                                            <div class="tab-content">
                                                                <!-- Estatus Curso -->
                                                                <div class="tab-pane active" id="tabGestionCap_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div29" runat="server">
                                                                            <asp:GridView ID="grdEstatusCapacitacion" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarEstatusCursos" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarEstatusCursos_Click"></asp:LinkButton>
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
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Tipo Agente -->
                                                                <div class="tab-pane" id="tabGestionCap_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div31" runat="server">
                                                                            <asp:GridView ID="grdTipoAgente" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarTipoAgente" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarTipoAgente_Click"></asp:LinkButton>
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
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <!-- Area Tematica -->
                                                                <div class="tab-pane" id="tabGestionCap_3" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div33" runat="server">
                                                                            <asp:GridView ID="GrdAreaTematica" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarAreaTematica" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarAreaTematica_Click"></asp:LinkButton>
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

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <!-- Clave Capacitacion -->
                                                                <div class="tab-pane" id="tabGestionCap_4" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div35" runat="server">
                                                                            <asp:GridView ID="grdClaveCapacitacion" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarClaveCapacitacion" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarClaveCapacitacion_Click"></asp:LinkButton>
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
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <!-- Establecimientos -->
                                                                <div class="tab-pane" id="tabGestionCap_5" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div30" runat="server">
                                                                            <asp:GridView ID="GrdEstablecimientos" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarEstablecimientos" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarEstablecimientos_Click"></asp:LinkButton>
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

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <!-- Modalidad -->
                                                                <div class="tab-pane" id="tabGestionCap_6" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div34" runat="server">
                                                                            <asp:GridView ID="GrdModalidad" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarModalidad" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarModalidad_Click"></asp:LinkButton>
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
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <!-- Clave Curso -->
                                                                <div class="tab-pane" id="tabGestionCap_7" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div32" runat="server">
                                                                            <asp:GridView ID="grdClaveCurso" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="60%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarClaveCurso" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarClaveCurso_Click"></asp:LinkButton>
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
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <!-- Ocupacion DC3 -->
                                                                <div class="tab-pane" id="tabGestionCap_8" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div37" runat="server">
                                                                            <asp:GridView ID="grdOcuapacionDC3" runat="server" AllowPaging="True"
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
                                                                                    <asp:TemplateField HeaderText="CLAVE" HeaderStyle-Width="10%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtClave" runat="server" Text='<%# Bind("clave")%>' class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="50"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# Bind("clave")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreClave" runat="server" class="form-control" onkeypress="return validaNumerosDecimales(event);" onchange="replaceNumeros(this)" MaxLength="50"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="50%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="350"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    
                                                                                    <asp:TemplateField HeaderText="Seleccionar">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlSeleccionar" runat="server" class="form-control">
                                                                                                <asp:ListItem Value="1">Sí</asp:ListItem>
                                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSeleccionar" runat="server" Text='<%# Bind("seleccionar")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreSeleccionar" runat="server" class="form-control">
                                                                                                       <asp:ListItem Value="1">Sí</asp:ListItem>
                                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
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
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarOcupacionDc3" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarOcupacionDc3_Click"></asp:LinkButton>
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
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <img id="img1" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                                <!-- /.tab-pane -->
                                                            </div>
                                                            <!-- /.tab-content -->
                                                        </div>
                                                        <!-- nav-tabs-custom -->
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- ./box-body -->

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>
                                <asp:HiddenField ID="hdIdTabDnc" runat="server" />
                                <asp:HiddenField ID="hdIdTabBecas" runat="server" />
                                <asp:HiddenField ID="hdIdTabBecasIngles" runat="server" />
                                <asp:HiddenField ID="hdIdGestionCapacitacion" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdIdDNC" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
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

        //Control de Tabs DNC
        function controlTabsDNC(id) {
            //Asigna el valor de la Tab de la DNC
            document.getElementById('hdIdTabDnc').value = id;
        }

        function controlTabsBecas(id) {
            //Asigna el valor de la Tab de Becas
            document.getElementById('hdIdTabBecas').value = id;
        }

        function controlTabsBecasIngles(id) {
            //Asigna el valor de la Tab de Becas Ingles
            document.getElementById('hdIdTabBecasIngles').value = id;
        }

        function controlTabsGestionCapacitacion(id) {
            //Asigna el valor de la Tab de Becas Ingles
            document.getElementById('hdIdGestionCapacitacion').value = id;
        }
    </script>
</body>
</html>
