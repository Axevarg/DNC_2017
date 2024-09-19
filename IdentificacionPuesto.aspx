<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IdentificacionPuesto.aspx.vb" Inherits="DNC_2017.IdentificacionPuesto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Identificación de Competencias del Puesto</title>
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

        function getUniqueValues() {
            var SinFac = 0;
            var OtrasFac = 0;
            for (var i = 0; i < this.options.length; i++) {
                if (this.options[i].selected) {
                    if (this.options[i].value == "1") {
                        SinFac = 1;
                    } else {

                        OtrasFac = OtrasFac + 1;
                    }
                }
            }
            //Validacion de  Sin facultades de autorización
            if (SinFac == 1 && OtrasFac >= 1) {
                alert('No se puede seleccionar otra facultad si tiene seleccionada Sin facultades de autorización');
            }

        }


        function ComportamientosJS() {

            $("#ddlColaboradoresEnviar").select2({
                placeholder: "Seleccionar"
            });

            $("#ddlCopiaJefe").select2({
                placeholder: "Seleccionar"
            });
            $("#ddlCorreoDO").select2({
                placeholder: "Seleccionar"
            });

            $("#ddlEscolaridad").select2({
                placeholder: "Seleccionar Escolaridad"
            });
            $("#ddlCarreras").select2({
                placeholder: "Seleccionar Carreras"
            });
             $("#ddlTipoPuesto").select2({
             placeholder: "Seleccionar Puesto"
             });

            $("#ddlFacultadesAutorizacion").select2({
                placeholder: "Selecciona Facultades de Autorización."
            });
        }

        //Ocultar
        function OcultarModalEnviar() {
            $("#modalEnviaFormato").modal('hide');
        }

    </script>
    <style>
        .BorderIzq {
            border-left: 2px solid silver !important;
        }
    </style>
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
                        <h1>Descriptivo de Puesto - Identificación de Competencias del Puesto (FO-RH-4/4) </h1>
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
                                                <h4 class="modal-title" id="myModalLabel2">Cargar Formato</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">

                                                        <div class="form-group">
                                                            <label for="exampleInputFile"><i class="fa fa-paperclip"></i>Cargar Formato de Identificación de Competencias del Puesto FO-RH-4/2</label>
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
                                                <button id="btnCargaFormato" runat="server" class="btn margin pull-right bg-red"
                                                    name="btnCargaFormato" onclick="return CargarExcel();">
                                                    <i class="fa fa-upload"></i>Cargar</button>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--Fin  Modal -->
                                <!-- Modal Envíar-->
                                <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalEnviaFormato" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalEnviar">Envía Descriptivo de puesto</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">Colaborador</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlColaboradoresEnviar" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">Copia a Jefe</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlCopiaJefe" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">Desarrollo Organizacional</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlCorreoDO" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>

                                                        </div>
                                                    </div>
                                                    <div id="div2" runat="server" class="col-md-12">
                                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Green"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:RadioButton ID="rdTextoColaborador" runat="server" GroupName="Texto"  Text=" Texto Colaborador" Checked="true"/>
                                                    </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="rdTextoJefe" runat="server"  GroupName="Texto" Text=" Texto Jefe"/>
                                                    </div>
                                                      <div class="col-md-4">
                                                            <asp:RadioButton ID="rdTextoJefeSinColaborador" runat="server"  GroupName="Texto" Text=" Texto Jefe (Colaborador sin Correo)"/>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-default pull-left bg-red" data-dismiss="modal">Cerrar</button>
                                                <asp:LinkButton ID="lnkEnviarDocumentos" runat="server"
                                                    OnClientClick="return EnviarDocumento();" OnClick="lnkEnviarDocumentos_Click"
                                                    class="btn margin pull-right bg-red"><i class="fa fa-send-o"></i>Envíar Excel</asp:LinkButton>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--Fin  Modal -->
                               

                                <%-- FIN IMG 03-11-2022 -- NUEVAS COMPAÑIAS --%>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Puesto</label>
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddlTipoPuesto" class="form-control select2" runat="server" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                                <img id="type" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        <!-- /.form group -->
                                    </div>

                                  
                                    <div class="col-md-6">
                                        <div class="form-group">

                                            <asp:Label ID="lblEmpresa" runat="server" Font-Size="10pt" Text="" for="lblEmpresa"></asp:Label>
                                            <button id="btnCargarExcel" runat="server" type="button" class="btn btn-default btn-xs pull-right" data-toggle="modal" data-target="#modalCargarPuesto"><i class="fa fa-file-excel-o"></i>Cargar Excel</button>
                                            <asp:Label ID="lblNivel" runat="server" Font-Size="10pt" Text="" for="lblNivel"></asp:Label>
                                            <asp:Label ID="lblFechas" runat="server" Font-Size="10pt" Text="" for="lblEmpresa"></asp:Label>

                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="divInformacion" runat="server">
                                    <div class="col-md-12">
                                        <!-- Horizontal Form -->
                                        <div class="box box-default">

                                            <!-- /.box-header -->
                                            <div class="box-body">

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>1. Datos Generales</dt>
                                                        </dl>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">1.1 Objetivo del Puesto</label>
                                                        <asp:TextBox ID="txtObjetivoPuesto" runat="server" MaxLength="4000" TextMode="MultiLine" Rows="3" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                    <div class="col-md-12">
                                                        <br />
                                                        <dl style="font-size: 9pt;">
                                                            <dt>1.2 Ubicación en la estructura orgánica (Autoridades)</dt>
                                                        </dl>
                                                    </div>


                                                    <div class="col-md-8">
                                                        <label style="font-size: 9pt;">Puesto al que reporta</label>
                                                        <asp:TextBox ID="txtPuestoReporto" runat="server" ReadOnly="true" Class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label style="font-size: 9pt;">Número de puestos que le reportan directamente</label>
                                                        <asp:TextBox ID="txtNumPuestoR" runat="server" MaxLength="3" ReadOnly="true" Class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">Puestos que le reportan directamente</label>
                                                        <asp:TextBox ID="txtDescripcionPuesto" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="4" MaxLength="8000" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">Áreas en las que se adscribe el puesto</label>
                                                        <asp:TextBox ID="txtAreaAdscribe" runat="server" ReadOnly="true" Class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99; color: white;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>2.1 Responsabilidades de Carácter General (Máximo 10):</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdFunciones" runat="server" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" Style="font-size: 9pt; width: 100%"
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-bordered table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="3%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCorrelativo" runat="server" Text='<%# Bind("correlativo")%>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="2.1 Responsabilidades de Carácter General" HeaderStyle-Width="90%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' TextMode="MultiLine" Rows="3" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="2000"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />

                                                                </asp:CommandField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">2.3 Autoridad del Puesto</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlFacultadesAutorizacion" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"
                                                                onchange="getUniqueValues.call(this)"></asp:ListBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>3. Relaciones</dt>
                                                        </dl>
                                                    </div>
                                                    <%-- <div class="col-md-6">
                                                        <label style="font-size: 9pt;">3.1 Internas</label>
                                                        <asp:TextBox ID="txtRelaInternasQuien" TextMode="MultiLine" Rows="3" placeholder="Con Quien" runat="server" MaxLength="200" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-size: 9pt;">Internas </label>
                                                        <asp:TextBox ID="txtRelaInternasPara" placeholder="Para" TextMode="MultiLine" Rows="3" runat="server" MaxLength="200" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-size: 9pt;">3.2 Externas</label>
                                                        <asp:TextBox ID="txtRelaExternasQuien" placeholder="Con Quien" TextMode="MultiLine" Rows="3" runat="server" MaxLength="200" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-size: 9pt;">Externas</label>
                                                        <asp:TextBox ID="txtRelaExternasPara" placeholder="Para" TextMode="MultiLine" Rows="3" runat="server" MaxLength="200" Class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>--%>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">3.1 Internas</label>
                                                        <asp:GridView ID="grdRelacionesInternas" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                                            Width="100%" Style="font-size: 8pt;" RowStyle-Height="10px" class="table table-bordered table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True"
                                                            OnRowDeleting="grdRelacionesInternas_RowDeleting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="correlativo" HeaderText="No." HeaderStyle-Width="2%" />

                                                                <asp:TemplateField HeaderText="Con Quien" HeaderStyle-Width="43%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtConQuien" runat="server" Text='<%# Bind("con_quien")%>' placeholder="Con Quien" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="1000"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="correlativo" HeaderText="No. " HeaderStyle-Width="2%" />
                                                                <asp:TemplateField HeaderText="Para" HeaderStyle-Width="43%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPara" runat="server" Text='<%# Bind("para")%>' placeholder="Para" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="1000"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Relación">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregarR" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarR_Click"></asp:LinkButton>
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
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">3.2 Externas</label>
                                                        <asp:GridView ID="grdRelacionesExternas" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                                            Width="100%" Style="font-size: 8pt;" RowStyle-Height="10px" class="table table-bordered table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True"
                                                            OnRowDeleting="grdRelacionesExternas_RowDeleting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="correlativo" HeaderText="No." HeaderStyle-Width="2%" />

                                                                <asp:TemplateField HeaderText="Con Quien" HeaderStyle-Width="43%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtConQuien" runat="server" Text='<%# Bind("con_quien")%>' placeholder="Con Quien" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="1000"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="correlativo" HeaderText="No. " HeaderStyle-Width="2%" />
                                                                <asp:TemplateField HeaderText="Para" HeaderStyle-Width="43%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPara" runat="server" Text='<%# Bind("para")%>' placeholder="Para" class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="1000"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Relación">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="8pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregarRExter" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarRExter_Click"></asp:LinkButton>
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
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>4. Educación</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-size: 9pt;">4.1 Grado de Escolaridad</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlEscolaridad" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>

                                                        </div>

                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-size: 9pt;">4.2 Carrera o Especialización</label>
                                                        <div class="input-group date">
                                                            <div class="input-group-addon">
                                                                <i class="fa fa-search"></i>
                                                            </div>
                                                            <asp:ListBox ID="ddlCarreras" runat="server" class="form-control select2" Width="100%" SelectionMode="Multiple"></asp:ListBox>

                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>5. Formación</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdFormacion" runat="server" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" Style="font-size: 9pt; width: 100%"
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-bordered table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Conocimientos Técnicos" HeaderStyle-Width="80%">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("descripcion")%>' TextMode="MultiLine" Rows="2" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dominio" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlDominio" runat="server" class="form-control">
                                                                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                                                                            <asp:ListItem Value="Basico">Basico</asp:ListItem>
                                                                            <asp:ListItem Value="Medio">Medio</asp:ListItem>
                                                                            <asp:ListItem Value="Avanzado">Avanzado</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDominio" runat="server" Text='<%# Bind("dominio")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>6. Idiomas</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label style="font-size: 9pt;">Idioma 1</label>
                                                        <asp:DropDownList ID="ddlIdioma" class="form-control select2" runat="server" Width="100%" onchange="validaIdioma();">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Dominio</label>
                                                        <asp:DropDownList Class="form-control  input-sm" ID="ddlidioma_dominio_1" runat="server">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label style="font-size: 9pt;">Idioma 2</label>
                                                        <asp:DropDownList ID="ddlIdioma2" class="form-control select2" runat="server" Width="100%" onchange="validaIdioma();">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Dominio</label>
                                                        <asp:DropDownList Class="form-control  input-sm" ID="ddlidioma_dominio_2" runat="server">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>7. Habilidades del puesto y Competencias del Negocio</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdHabilidadesCompetencia" runat="server" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" Style="font-size: 9pt; width: 100%"
                                                            RowStyle-Height="10px" ShowFooter="False" class="table table-bordered table-responsive"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                            <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Habilidad / Competencia" HeaderStyle-Width="35%"
                                                                    HeaderStyle-CssClass="BorderIzq"
                                                                    ItemStyle-CssClass="BorderIzq">
                                                                    <EditItemTemplate>

                                                                        <asp:DropDownList ID="ddlHabilidades1" runat="server" class="form-control"></asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHabilidad1" runat="server" Text='<%# Bind("descripcion1")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dominio" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlDominio1" runat="server" class="form-control">
                                                                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                                                                            <asp:ListItem Value="Basico">Basico</asp:ListItem>
                                                                            <asp:ListItem Value="Medio">Medio</asp:ListItem>
                                                                            <asp:ListItem Value="Avanzado">Avanzado</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDominio1" runat="server" Text='<%# Bind("dominio1")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Habilidad / Competencia" HeaderStyle-Width="35%" HeaderStyle-CssClass="BorderIzq"
                                                                    ItemStyle-CssClass="BorderIzq">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlHabilidades2" runat="server" class="form-control"></asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHabilidad2" runat="server" Text='<%# Bind("descripcion2")%>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dominio" HeaderStyle-Width="15%">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlDominio2" runat="server" class="form-control">
                                                                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                                                                            <asp:ListItem Value="Basico">Basico</asp:ListItem>
                                                                            <asp:ListItem Value="Medio">Medio</asp:ListItem>
                                                                            <asp:ListItem Value="Avanzado">Avanzado</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDominio2" runat="server" Text='<%# Bind("dominio2")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar"
                                                                    HeaderStyle-CssClass="BorderIzq"
                                                                    ItemStyle-CssClass="BorderIzq">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />

                                                                </asp:CommandField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12"><strong>Herramientas Informáticas</strong> </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Word</label>
                                                        <asp:DropDownList ID="ddlWord" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Excel</label>
                                                        <asp:DropDownList ID="ddlExcel" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Power Point</label>
                                                        <asp:DropDownList ID="ddlPoweP" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Outlook</label>
                                                        <asp:DropDownList ID="ddlOutlook" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Access</label>
                                                        <asp:DropDownList ID="ddlAccess" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label style="font-size: 9pt;">Project</label>
                                                        <asp:DropDownList ID="ddlProject" class="form-control select2" runat="server" Width="100%">
                                                            <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                                            <asp:ListItem>Básico</asp:ListItem>
                                                            <asp:ListItem>Medio</asp:ListItem>
                                                            <asp:ListItem>Avanzado</asp:ListItem>
                                                            <asp:ListItem Value="">No Requerido</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>8. Experiencia</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <label style="font-size: 9pt;">Áreas de Experiencia</label>
                                                        <asp:TextBox ID="txtAreaExperiencia" runat="server" MaxLength="300" Class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label style="font-size: 9pt;">Tiempo</label>
                                                        <asp:DropDownList ID="ddlTiempo" class="form-control select2" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>9. Sistemas de Gestión</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">9.1 Sistema de Gestión</label>
                                                        <asp:TextBox ID="txtSgi" runat="server" MaxLength="200" Class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="background-color: #CCFF99;"></label>
                                                        <dl style="background-color: #CCFF99;">
                                                            <dt>10. Requisitos y condiciones</dt>
                                                        </dl>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label style="font-size: 9pt;">10.1 Requisitos y condiciones adicionales del puesto/ocupante (de lo contrario, indicar no aplica)</label>
                                                        <asp:TextBox ID="txtRequisito" runat="server" MaxLength="500" Class="form-control  input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)"></asp:TextBox>
                                                    </div>


                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div id="divInforGuardada" runat="server" visible="false" class="alert alert-success ">
                                                            <strong>Información!</strong><br />
                                                            <asp:Label ID="lblMensaje" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click" class="btn btn-danger btn-flat pull-left" OnClientClick="return Imprimir();"><i class="fa fa-print"></i> Descargar Excel para Impresión </asp:LinkButton>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lnkEnviar" runat="server" class="btn btn-danger btn-flat pull-left" data-toggle="modal" data-target="#modalEnviaFormato"><i class="fa fa-send"></i> Envíar Descriptivo de Puesto</asp:LinkButton>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lnkGuardar" runat="server" OnClientClick="return ActualizaPuesto();" OnClick="lnkGuardar_Click" class="btn btn-danger btn-flat pull-right"><i class="fa fa-edit"></i> Guardar</asp:LinkButton>
                                                    </div>
                                                </div>


                                                <img id="loadingAccion" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCargaFormato" />
                                <asp:PostBackTrigger ControlID="lnkImprimir" />
                            </Triggers>
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
        function Imprimir() {
            if (!confirm('El proceso de impresión puede tardar varios segundos.')) { return false; };
        }
        function ActualizaPuesto() {

            //Objetivo 
            if (document.getElementById("txtObjetivoPuesto").value == '') {
                alert("Debe capturar el Objetivo .");
                document.getElementById("txtObjetivoPuesto").focus();
                return false;
            }
            //Facultades
            var SinFac = 0;
            var OtrasFac = 0;
            var vFacultades = "";
            var ddlFacultadesAutorizacion = document.getElementById("ddlFacultadesAutorizacion");

            for (var i = 0; i < ddlFacultadesAutorizacion.options.length; i++) {
                if (ddlFacultadesAutorizacion.options[i].selected) {
                    vFacultades += ddlFacultadesAutorizacion.options[i].value;
                    if (ddlFacultadesAutorizacion.options[i].value == "1") {
                        SinFac = 1;
                    } else {
                        OtrasFac = OtrasFac + 1;
                    }
                }
            }

            if (vFacultades == '') {
                alert("Debe de seleccionar Facultades Autorizacion .");
                ddlFacultadesAutorizacion.focus();
                return false;
            }

            //Validacion de  Sin facultades de autorización
            if (SinFac == 1 && OtrasFac >= 1) {
                alert('No se puede seleccionar otra facultad si tiene seleccionada Sin facultades de autorización');
                ddlFacultadesAutorizacion.focus();
                return false;
            }
            ////Internas con quien 
            //if (document.getElementById("txtRelaInternasQuien").value == '') {
            //    alert("Debe capturar Relaciones Internas con quien .");
            //    document.getElementById("txtRelaInternasQuien").focus();
            //    return false;
            //}
            ////Internas con Para 
            //if (document.getElementById("txtRelaInternasPara").value == '') {
            //    alert("Debe capturar Relaciones Internas para.");
            //    document.getElementById("txtRelaInternasPara").focus();
            //    return false;
            //}
            ////Externas con quien 
            //if (document.getElementById("txtRelaExternasQuien").value == '') {
            //    alert("Debe capturar Relaciones Externas con quien .");
            //    document.getElementById("txtRelaExternasQuien").focus();
            //    return false;
            //}
            ////Externas con para
            //if (document.getElementById("txtRelaExternasPara").value == '') {
            //    alert("Debe capturar Relaciones Externas para.");
            //    document.getElementById("txtRelaExternasPara").focus();
            //    return false;
            //}
            ////Grado de Escolaridad
            var vGradoEscolaridad = "";
            var ddlEscolaridad = document.getElementById("ddlEscolaridad");

            for (var i = 0; i < ddlEscolaridad.options.length; i++) {
                if (ddlEscolaridad.options[i].selected) {
                    vGradoEscolaridad += ddlEscolaridad.options[i].value;
                }
            }

            if (vGradoEscolaridad == '') {
                alert("Debe de seleccionar Grado de Escolaridad .");
                ddlEscolaridad.focus();
                return false;
            }

            //Carrera o Especialización
            var vCarrera = "";
            var ddlCarreras = document.getElementById("ddlCarreras");

            for (var i = 0; i < ddlCarreras.options.length; i++) {
                if (ddlCarreras.options[i].selected) {
                    vCarrera += ddlCarreras.options[i].value;
                }
            }

            if (vCarrera == '') {
                alert("Carrera o Especialización.");
                ddlCarreras.focus();
                return false;
            }
            // Idioma
            //var ddlIdioma = document.getElementById("ddlIdioma");
            //var ddlidioma_dominio_1 = document.getElementById("ddlidioma_dominio_1");

            //if (ddlIdioma.options[ddlIdioma.selectedIndex].value != '0' || ddlidioma_dominio_1.options[ddlidioma_dominio_1.selectedIndex].value != '0') {
            //    alert("Debe seleccionar idioma y dominio.");
            //    ddlIdioma.focus();
            //    return false;
            //}

            //var ddlIdioma2 = document.getElementById("ddlIdioma2");
            //if (ddlIdioma2.options[ddlIdioma2.selectedIndex].value == 0) {
            //    alert("Debe seleccionar idioma 2.");
            //    ddlIdioma.focus();
            //    return false;
            //}

            //var ddlIdioma = document.getElementById("ddlIdioma");
            //if (ddlIdioma.options[ddlIdioma.selectedIndex].value == 0) {
            //    alert("Debe seleccionar idioma.");
            //    ddlIdioma.focus();
            //    return false;
            //     }
            // word
            var ddlWord = document.getElementById("ddlWord");
            if (ddlWord.options[ddlWord.selectedIndex].value == '0') {
                alert("Debe seleccionar Word.");
                ddlWord.focus();
                return false;
            }
            // Excel
            var ddlExcel = document.getElementById("ddlExcel");
            if (ddlExcel.options[ddlExcel.selectedIndex].value == '0') {
                alert("Debe seleccionar Excel.");
                ddlExcel.focus();
                return false;
            }
            // Power Point
            var ddlPoweP = document.getElementById("ddlPoweP");
            if (ddlPoweP.options[ddlPoweP.selectedIndex].value == '0') {
                alert("Debe seleccionar Power Point.");
                ddlPoweP.focus();
                return false;
            }
            // Outlook
            var ddlOutlook = document.getElementById("ddlOutlook");
            if (ddlOutlook.options[ddlOutlook.selectedIndex].value == '0') {
                alert("Debe seleccionar Outlook.");
                ddlOutlook.focus();
                return false;
            }
            // Access
            var ddlAccess = document.getElementById("ddlAccess");
            if (ddlAccess.options[ddlAccess.selectedIndex].value == '0') {
                alert("Debe seleccionar Access.");
                ddlAccess.focus();
                return false;
            }
            // Project
            var ddlProject = document.getElementById("ddlProject");
            if (ddlProject.options[ddlProject.selectedIndex].value == '0') {
                alert("Debe seleccionar Project");
                ddlProject.focus();
                return false;
            }
            //txtAreaExperiencia
            if (document.getElementById("txtAreaExperiencia").value == '') {
                alert("Debe capturar Áreas de Experiencia.");
                document.getElementById("txtAreaExperiencia").focus();
                return false;
            }
            // ddlTiempo
            var ddlTiempo = document.getElementById("ddlTiempo");
            if (ddlTiempo.options[ddlTiempo.selectedIndex].value == '0') {
                alert("Debe seleccionar el tiempo");
                ddlTiempo.focus();
                return false;
            }
            //SGI
            if (document.getElementById("txtSgi").value == '') {
                alert("Debe capturar Sistema de Gestión Integrado.");
                document.getElementById("txtSgi").focus();
                return false;
            }

            //Requisistos y condicones adicionales del puesto/ocupante
            if (document.getElementById("txtRequisito").value == '') {
                alert("Debe capturar Requisistos y condicones adicionales del puesto/ocupante.");
                document.getElementById("txtRequisito").focus();
                return false;
            }


            //  if (!confirm('Al Aceptar se guardaran los cambios')) { return false; };
            var theImg = document.getElementById("loadingAccion");
            theImg.style.display = "inline";
            return true;
        }
        function CargarExcel() {
            if (!confirm('Al Aceptar se remplazaran los datos por el Excel cargado.')) { return false; };
            return true;
        }

        // Funcion de Enviar Documento FIrmado
        function EnviarDocumento() {


            var x = document.getElementById("ddlColaboradoresEnviar");
            var vContadorSeleccion = 0;
            for (var i = 0; i < x.options.length; i++) {
                if (x.options[i].selected) {
                    vContadorSeleccion = vContadorSeleccion + 1;
                }
            }
            // Contador de Selccion

            if (vContadorSeleccion == 0) {
                alert('Debe de Seleccionar al menos una persona para enviarle el documento.');
                return false;
            }
            // Valida que este creado el Documento para cargar
            if (!confirm('Al Aceptar el documento se enviará a las personas indicadas.')) { return false; };

            OcultarModalEnviar();
            return true;
        }

        //Valida que no Requitan 2 idiomas
        function validaIdioma() {
            var ddlIdioma = document.getElementById("ddlIdioma");
            var ddlIdioma2 = document.getElementById("ddlIdioma2");
            // Valida que sea difrente a seleccionar
            if (ddlIdioma.options[ddlIdioma.selectedIndex].value == '0' && ddlIdioma2.options[ddlIdioma2.selectedIndex].value == '0') {
            } else {

                if (ddlIdioma.options[ddlIdioma.selectedIndex].value == ddlIdioma2.options[ddlIdioma2.selectedIndex].value) {
                    alert('No puedes escoger el mismo idioma.');
                    ddlIdioma.selectedIndex = '0';
                    ddlIdioma2.selectedIndex = '0';
                }
            }
        }
    </script>
</body>
</html>
