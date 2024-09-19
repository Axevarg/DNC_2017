<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="permisoPerfil.aspx.vb" Inherits="DNC_2017.permisoPerfil" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Permisos de Perfil | SIGIDO</title>
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
    <!--Estilos TreeView-->
    <link href="css/estiloTreeView.css" rel="stylesheet" />
    <!-- Sub Menu -->
    <link href="bootstrap/css/dropdown-submenu.css" rel="stylesheet" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

    <script>
        function TreeView() {
            $("#TreModulos").tree;
        }

        function ModalOcultar() {
            $('#modalPerfil').modal('hide');
        }
        function Modal() {
            //Llama el Modal de Paros
            $('#modalPerfil').modal('show');
        }


    </script>

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
                        <h1>Permisos de Perfil          <small>SIGIDO</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">
        
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- Modal de Perfiles -->
                                <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalPerfil" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalCentroTrabajo">Administración Perfiles
                                                </h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12" style="overflow: auto">
                                                        <asp:GridView ID="grdPerfiles" runat="server" AllowPaging="True"
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
                                                                <asp:TemplateField HeaderText="DESCRIPCIÓN" HeaderStyle-Width="65%">
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

                                                                <asp:TemplateField HeaderText="ESTATUS" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control" Width="100%">
                                                                            <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                            <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control" Width="100%">
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
                                                                        <asp:LinkButton ID="lnkAgregarPerfil" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClientClick="ModalOcultar()" OnClick="lnkAgregarPerfil_Click"></asp:LinkButton>
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
                                        </div>
                                    </div>
                                </div>
                                <!--Fin de Perfiles -->
                                <!--Datos-->
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Perfiles</label>
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddlPerfil" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>
                                                <img id="loading" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        <!-- /.form group -->
                                    </div>
                                    <div class="col-md-4"></div>
                                    <div class="col-md-2">
                                        <label>&nbsp;</label><br />
                                        <button id="btnPerfiles" name="btnPerfiles" runat="server" type="button" class="btn btn-danger btn-flat pull-left" data-toggle="modal" data-target="#modalPerfil" data-placement="rigth" title="Agrega Perfiles">
                                            <i class="fa fa-user"></i>Administración de Perfiles
                                        </button>
                                    </div>
                                     <div class="col-md-12">
                                     
                                     </div>
                                </div>

                                <div class="row" id="divModulos" runat="server">
                                    <!-- Administraciones -->
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Acceso a Módulos</h3>
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
                                                    <div class="col-md-12">
                                                        <asp:TreeView ID="TreModulos" runat="server" ShowCheckBoxes="All" CssClass="tree"
                                                            onclick="ClientSideChangeSelection()"
                                                            NodeStyle-CssClass="treeli"
                                                            RootNodeStyle-CssClass="treeli"
                                                            LeafNodeStyle-CssClass="treeli" HoverNodeStyle-CssClass="">
                                                        </asp:TreeView>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- ./box-body -->


                                            <div class="box-footer">
                                                <button class="btn btn-danger btn-flat pull-left" runat="server" id="btnGuardarAccesos" name="btnGuardarAccesos" onclick="return guardaConfiguracion();">
                                                    <i class="fa fa-save"></i>
                                                    Guardar
                                                </button>

                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>

                                            <!-- /.box-footer -->
                                        </div>
                                        <!-- /.box -->
                                    </div>
                                    <!-- /.col -->

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <asp:HiddenField ID="hfGridView1SV" runat="server" />
                        <asp:HiddenField ID="hfGridView1SH" runat="server" />
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
        function cargarInfo() {

            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }

        function guardaConfiguracion() {

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }

        //Funcion para selecionar los hijos de los nodos seleccionados
        function ClientSideChangeSelection() {
            var chkBox = window.event.srcElement;
            var isChecked;
            var isCheckBox = false;

            if (chkBox.tagName == "INPUT" && chkBox.type.toUpperCase() == "CHECKBOX") {
                var treeNode = chkBox;
                isChecked = treeNode.checked;
                do {
                    chkBox = chkBox.parentElement;
                } while (chkBox.tagName != "TABLE")
                var firstLevel = chkBox.rows[0].cells.length;

                var tableElements = chkBox.parentElement.getElementsByTagName("TABLE");
                var tableElementsCount = tableElements.length;
                if (tableElementsCount > 0) {
                    for (i = 0; i < tableElementsCount; i++) {
                        if (tableElements[i] == chkBox) {
                            i++;
                            isCheckBox = true;
                            if (i == tableElementsCount) {
                                return;
                            }
                        }
                        if (isCheckBox == true) {
                            var secondLevel = tableElements[i].rows[0].cells.length;
                            if (secondLevel > firstLevel) {
                                var cell = tableElements[i].rows[0].cells[secondLevel - 1];
                                var inputElement = cell.getElementsByTagName("INPUT");
                                inputElement[0].checked = isChecked;
                            }
                            else {
                                return;
                            }
                        }
                    }
                }
            }
        }
    </script>
</body>
</html>
