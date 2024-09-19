<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AltaCursosCapacitacion.aspx.vb" Inherits="DNC_2017.AltaCursosCapacitacion" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Alta de Cursos Capacitación | SIGIDO</title>
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
            $("#ddlCursos").select2();
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
                                    <div class="col-md-12">
                                        <!-- Horizontal Form -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Alta de Cursos Capacitación</h3>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <div role="form">



                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Busqueda de Curso Registrado</label>
                                                                <asp:DropDownList ID="ddlCursosRegistrados" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarCurso();">
                                                                </asp:DropDownList>
                                                                <img id="imgCurso" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label>Curso</label>
                                                                <asp:DropDownList ID="ddlCursos" class="form-control select2" runat="server" Width="100%">
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>

                                                        <div class="col-md-12" id="divGrid" runat="server">
                                                            <asp:GridView ID="grdHorarios" runat="server" AllowPaging="True"
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

                                                                    <asp:TemplateField HeaderText="No. PERSONA" HeaderStyle-Width="10%">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtNumPersona" runat="server" Text='<%# Bind("no_personas")%>' Font-Size="8pt" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNumPersona" runat="server" Text='<%# Bind("no_personas")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtAgregaNumPersona" runat="server" class="form-control input-sm" Font-Size="8pt" Style="text-transform: uppercase" onkeypress="return validaNumeroEntero(event);" onchange="replaceNumeros(this)" MaxLength="4"></asp:TextBox>
                                                                        </FooterTemplate>

                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FECHA INICIO" HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtFechaInicio" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_desde")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Bind("fecha_desde")%>' ForeColor="#333333"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtAgreFechaInicio" ClientIDMode="Static" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle CssClass="centrar" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FECHA FINAL" HeaderStyle-CssClass="centrar" HeaderStyle-Width="10%">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtFechaFinal" ClientIDMode="Static" runat="server" Text='<%# Bind("fecha_hasta")%>' class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFechaFinal" runat="server" Text='<%# Bind("fecha_hasta")%>' ForeColor="#333333"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtAgreFechaFinal" ClientIDMode="Static" runat="server" class="form-control input-sm" Style="text-transform: uppercase" onkeypress="return validaNumerosDecimales(event);" onchange="replaceFechas(this)" MaxLength="10"></asp:TextBox>
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle CssClass="centrar" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HORARIO DEL GRUPO" HeaderStyle-Width="50%">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtHorario" runat="server" Text='<%# Bind("horario")%>' Font-Size="8pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="150"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHorario" runat="server" Text='<%# Bind("horario")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtAgreHorario" runat="server" class="form-control input-sm" Font-Size="8pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="150"></asp:TextBox>
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

                                                    
                                                            <div class="col-md-12" id="divComentarios" runat="server">
                                                                <div class="form-group">
                                                                    <label>Comentarios Capacitación</label>
                                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" TextMode="MultiLine" Rows="8" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="5000"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div id="divLiga" runat="server" class="col-md-12">
                                                                     <div class="form-group">
                                                                         <a href="#"><asp:Label ID="lblRuta" runat="server" Text="Label"></asp:Label></a>
                                                                          <asp:LinkButton ID="lnkCopiar" runat="server" Text="Copiar Liga de Acceso" Font-Size="8pt" class="label label-danger pull-right" ToolTip="Copiar Liga de Acceso" OnClientClick="return copiar();"></asp:LinkButton>
                                                                         
                                                                     </div>
                                                            </div>


                                                        <br />
                                                        <div class="col-md-12" id="divAuto" runat="server">
                                                            <div id="divCursosMensaje" runat="server" class="callout callout-success">

                                                                <h4><i class="icon fa fa-info"></i>Curso</h4>
                                                                <asp:Label ID="lblCurso" runat="server" Text="Label"></asp:Label>

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-footer">
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnEliminar" name="btnEliminar" onclick="return eliminaCurso();">
                                                    <i class="fa fa-archive"></i>
                                                    Eliminar
                                                </button>
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnActualizar" name="btnActualizar" onclick="return updCurso();">
                                                    <i class="fa fa-edit"></i>
                                                    Actualizar
                                                </button>

                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnNuevoCurso" name="btnNuevoCurso" onclick="return nuevoCurso();">
                                                    <i class="fa  fa-plus"></i>
                                                    Agregar Nuevo Curso
                                                </button>
                                                <button class="btn btn-danger btn-flat pull-right" runat="server" id="btnAgregar" name="btnAgregar" onclick="return insCursos();">
                                                    <i class="fa  fa-plus"></i>
                                                    Agregar Curso
                                                </button>
                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                                <asp:HiddenField ID="hdEstatusCurso" runat="server" />
                            </ContentTemplate>
                            <Triggers>
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

  
        function insCursos() {

            var selects = document.getElementById("ddlCursos");
            var selectedValue = selects.options[selects.selectedIndex].value;
            //Facilitador
            if (selectedValue == 0) {
                alert("Debe de seleccionar un Curso.");
                return false;
            }



            var theControl = document.getElementById("btnAgregar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }


        function cargarCurso() {

            var theImg = document.getElementById("imgCurso");
            theImg.style.display = "inline";

        }

        function updCurso() {


            //Descripcion COrta
            if (document.getElementById("txtDescripcion").value == '') {
                alert("Debe capturar los Comentarios Capacitación.");
                document.getElementById("txtDescripcion").focus();
                return false;
            } 
            var theControl = document.getElementById("btnActualizar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }

        // elimina Curso
        function eliminaCurso() {
            if (!confirm('¿Desea Eliminar Curso?')) { return false; };
            return true;
        }

        function nuevoCurso() {
            var theControl = document.getElementById("btnNuevoCurso");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
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

            if (!confirm('Al Aceptar el Temario vigente se remplazara por el actual.')) { return false; };

            return true;
        }

        function copiar() {

            // Crea un campo de texto "oculto"
            var aux = document.createElement("input");

            // Asigna el contenido del elemento especificado al valor del campo
            aux.setAttribute("value", document.getElementById("lblRuta").innerHTML);

            // Añade el campo a la página
            document.body.appendChild(aux);

            // Selecciona el contenido del campo
            aux.select();

            // Copia el texto seleccionado
            document.execCommand("copy");

            // Elimina el campo de la página
            document.body.removeChild(aux);

            alert("texto copiado");
            return false;
        }
    </script>
</body>
</html>
