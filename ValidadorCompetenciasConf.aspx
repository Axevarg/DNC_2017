<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ValidadorCompetenciasConf.aspx.vb" Inherits="DNC_2017.ValidadorCompetenciasConf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Configuración de Validación de Competencias | SIGIDO</title>
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


    <!-- jQuery 2.2.3 -->
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>
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
                        <h1>Configuración de Validación de Competencias
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>


                                <!--Registro DNC-->
                                <div id="divRegistro" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Tabla de Configuración</h3>

                                                <div class="box-tools pull-right">
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">

                                                <div class="row">

                                                    <div class="col-md-12" id="divAvance" runat="server">
                                                        <asp:GridView ID="grdConfiguracion" runat="server" AllowPaging="True"
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
                                                                <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="45%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtDescripcion" ClientIDMode="Static" runat="server" Text='<%# Bind("descripcion")%>' class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreDescripcion" ClientIDMode="Static" runat="server" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA INICIO " HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaInicio" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_inicial")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Bind("fecha_inicial")%>' ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaInicio" ClientIDMode="Static" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="FECHA FINAL" HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtFechaFinal" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_final")%>' class="form-control input-sm" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFechaFinal" runat="server" Text='<%# Bind("fecha_final")%>' ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAgreFechaFinal" ClientIDMode="Static" runat="server" class="form-control input-sm" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <HeaderStyle CssClass="centrar" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control">
                                                                            <asp:ListItem Value="1">Activo</asp:ListItem>
                                                                            <asp:ListItem Value="0">Inactivo</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreEstatus" ClientIDMode="Static" runat="server" class="form-control">
                                                                            <asp:ListItem Value="1">Activo</asp:ListItem>
                                                                            <asp:ListItem Value="0">Inactivo</asp:ListItem>
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
                                                                        <asp:LinkButton ID="lnkAgregarC" runat="server" Text="Agregar" Font-Size="8pt" class="label label-danger" ToolTip="Agregar" OnClick="lnkAgregarC_Click"></asp:LinkButton>
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
</body>
</html>
