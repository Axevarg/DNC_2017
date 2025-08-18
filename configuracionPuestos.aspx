<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="configuracionPuestos.aspx.vb" Inherits="DNC_2017.configuracionPuestos" enableEventValidation="false"%>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Configuracion de Puestos | SIGIDO</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css" />
  
    <link rel="stylesheet" href="plugins/select2/select2.min.css" />
    
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
    <!-- jQuery 2.2.3 
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>-->
    <!-- DataTables -->
    
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>

  
    <link href="plugins/datatables/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="css/table-search.css" rel="stylesheet" />
   
    <script>

        function ComportamientosJS() {

            $("#ddl_jefe").select2();
            $("#ddl_infogiro").select2();
            $("#ddl_Giro").select2();

        }

    </script>

    <style>
      .example-modal .modal {
        position: relative;
        top: auto;
        bottom: auto;
        right: auto;
        left: auto;
        display: block;
        z-index: 1;
      }
      .example-modal .modal {
        background: transparent !important;
      }


      label.error {
        color: red;
    }

       .modal-max-width {
        max-width: 90%; /* Cambia el valor según tus necesidades */
    }
       #busca{
           padding:20px;
       }

       #inside{
            padding:30px;
       }

       .table-search {
        width: 100%;
        overflow-x: auto; /* Deslizador horizontal si la tabla es más ancha que la pantalla */
    }

       .modal-content {
        width: 100%; /* Ancho del modal, puedes ajustarlo según tus necesidades */
        max-width: 1200px; /* Ancho máximo del modal */
    }

    /* Estilo para la tabla dentro del modal */
    .modal-body table {
        width: 100%;
    }

       
    </style>

</head>
<body class="hold-transition skin-black layout-top-nav">
    <form id="form1" runat="server">

        <asp:SqlDataSource ID="sqlMain" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConnectioncustom %>"></asp:SqlDataSource>


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
                        
                    </section>



                    <!-- Main content -->
                    <section class="content">
                        <div class="col-md-12">
                            <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="row">
                            <!-- /.col -->
                            <div class="col-md-12">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <asp:Label ID="lblIcono" runat="server" CssClass="glyphicon glyphicon-cog"></asp:Label>
                                        <h3 class="box-title">Configuracion de Puestos</h3>
                                    </div>
                                    <div id="busca">
                                        <p>Buscador</p>

                                        <div class="row">
                                            <div class="col-md-12">
                                            <div class="input-group margin">
                                                <div class="input-group-btn">
                                                    <button type="button" class="btn btn-danger" data-toggle="modal" data-target="#modalBusqueda" onclick="mostrarParteFormulario()"><span class="fa fa-search"></span></button>
                                                </div>
                                                <input type="text" id="txtCodigo2" class="form-control" readonly />
                                            </div>
                                                </div>
                                 </div>


                                <!-- Modal de búsqueda -->
                                <div class="modal fade" id="modalBusqueda" tabindex="-1" role="dialog" aria-labelledby="modalBusquedaLabel">
                                    <div class="modal-dialog modal-lg modal-max-width" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="modalBusquedaLabel">Resultado de la búsqueda</h4>
                                            </div>

                                            <!-- Tabla -->
                                            <div class="modal-body">
                                                <div style="overflow-x: auto;">
                                                    <table id="tb_datos" class="table-search">
                                                        <thead>
                                                            <tr>
                                                              
                                                                <th>Código</th>
                                                                <th>Nombre</th>
                                                                <th>Puesto</th>
                                                                <th>Rol</th>
                                                                <th>Tipo Puesto</th>
                                                                <th>Nivel</th>
                                                                <th>Unidad Negocio</th>
                                                                <th>Empresa</th>
                                                                <th>Estatus</th>
                                                                <th>Área adscribe</th>
                                                                <th>Posicion</th>
                                                                <th>Jefe</th>
                                                                <th>Puesto GIRO</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <%=LLENAR_BUSQUEDA %>
                                                        </tbody>

                                                        <tfoot>
                                                            <tr>
                                                              
                                                                <th>Código</th>
                                                                <th>Puesto</th>
                                                                <th>Rol</th>
                                                                <th>Tipo Puesto</th>
                                                                <th>Nivel</th>
                                                                <th>Unidad Negocio</th>
                                                                <th>Empresa</th>
                                                                <th>Estatus</th>
                                                                <th>Área adscribe</th>
                                                                <th>Posicion</th>
                                                                <th>Jefe</th>
                                                                <th>Puesto GIRO</th>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                                    <!--------------------------->
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-solid">
                                    <div class="box-header with-border" id="inside">

                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Codigo (*):</label>
                                                    <asp:TextBox runat="server" ID="txtCodigo" class="form-control" placeholder="Enter ... "></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-1"></div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Estatus:</label>
                                                    <div class="form-group">
                                                        <label>
                                                            Habilitado:
                                                         <asp:RadioButton ID="radio1" runat="server" GroupName="r3" ClientIDMode="Static" Checked="true" />
                                                        </label>
                                                        <span>&nbsp; &nbsp;</span>

                                                        <label>
                                                            Deshabilitado:
                                                           <asp:RadioButton ID="radio2" runat="server" GroupName="r3" ClientIDMode="Static" />
                                                        </label>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-5"></div>
                                            <div class="col-md-1">
                                                <%-- <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-block btn-danger" OnClick="btnEliminar_Click" Text="Delete" UseSubmitBehavior="false" />
                                                </div>--%>
                                            </div>
                                        </div>

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <div class="form-group">
                                                            <label>Puesto (*):</label>
                                                            <asp:DropDownList ID="ddl_puesto" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlpuesto_SelectedIndexChanged" Style="width: 100%;">
                                                                <asp:ListItem Text="Selecciona..." Value="" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label></label>
                                                            <asp:DropDownList ID="ddl_opcion" runat="server" CssClass="form-control" Style="width: 100%;">
                                                                <asp:ListItem Text="Vacío" Value="" />
                                                                <asp:ListItem Text="en" Value="en" />
                                                                <asp:ListItem Text="de" Value="de" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-5">
                                                        <label>Rol (*):</label>
                                                        <div class="input-group input-group">

                                                            <asp:DropDownList ID="ddl_rol" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                                <asp:ListItem Text="Selecciona..." Value="" />
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txt_rol" runat="server" CssClass="form-control select2" Style="width: 100%; display: none; position: absolute; top: 0; left: 0;"></asp:TextBox>
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btn_editar_rol" runat="server" Text="Editar" OnClick="btn_editar_rol_Click" CssClass="btn btn-primary" />
                                                                <asp:Button ID="btn_guardar_rol" runat="server" Text="Guardar" OnClick="btn_guardar_rol_Click" CssClass="btn btn-success" Style="display: none;" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        
                                        </asp:UpdatePanel>




                                        <div class="row">

                                            <div class="col-md-4">
                                                <label for="select2">Nivel (*):</label>
                                                <asp:DropDownList ID="select2" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                    <asp:ListItem Text="Selecciona Nivel..." Value="" />
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="select3">Unidad Negocio (*):</label>
                                                    <asp:DropDownList ID="select3" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                        <asp:ListItem Text="Selecciona Unidad..." Value="" />
                                                        <asp:ListItem>Dina</asp:ListItem>
                                                        <asp:ListItem>Mercader</asp:ListItem>
                                                        <asp:ListItem>Dicoser</asp:ListItem>
                                                        <asp:ListItem>Passa</asp:ListItem>
                                                        <asp:ListItem>Dicomer</asp:ListItem>
                                                        <asp:ListItem>TLJ</asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="select4">Empresa (*):</label>
                                                    <asp:DropDownList ID="select4" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                        <asp:ListItem Text="Selecciona Compañia..." Value="" />
                                                        <asp:ListItem Value="001">PASSA ADMINISTRACION Y SERVICIOS S.A. DE C.V.</asp:ListItem>
                                                        <asp:ListItem Value="002">DINA CAMIONES</asp:ListItem>
                                                        <asp:ListItem Value="003">MERCADER FINANCIAL</asp:ListItem>
                                                        <asp:ListItem Value="004">DINA COMERCIALIZACION AUTOMOTRIZ (DICOMER)</asp:ListItem>
                                                        <asp:ListItem Value="005">TRANSPORTES Y LOGISTICA DE JALISCO</asp:ListItem>
                                                        <asp:ListItem Value="006">DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV (DICOSER)</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>


                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label for="select5">Tipo Puesto (*):</label>
                                                    <asp:DropDownList ID="select5" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                        <asp:ListItem Text="Selecciona Puesto..." Value="" />
                                                        <asp:ListItem>Administrativo</asp:ListItem>
                                                        <asp:ListItem>Operativo</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                               <div class="col-md-3">
                                                   <label for="select5">Jefe (*):</label>
                                                <asp:DropDownList ID="ddl_jefe" runat="server" class="form-control select2" Style="width: 100%;">
                                                    <asp:ListItem Text="Selecciona..." Value="" />
                                                </asp:DropDownList>
                                            </div>

                                           

                                              <div class="col-md-6">
                                                <label for="select1">Area adscribe (*):</label>
                                                <asp:DropDownList ID="ddl_infogiro" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                    <asp:ListItem Text="Selecciona area..." Value="" />
                                                </asp:DropDownList>

                                            </div>
                                          
                                        </div>

                                        <div class="row">               
                                            
                                             <div class="col-md-4">
                                               <label for="select1">Puesto GIRO (*):</label>
                                                <asp:DropDownList ID="ddl_Giro" runat="server" CssClass="form-control select2" Style="width: 100%;">
                                                    <asp:ListItem Text="Selecciona..." Value="" />
                                                </asp:DropDownList>
                                            </div>

                                             <div class="col-md-2">
                                                <label for="select1">Posicion</label>
                                                <asp:TextBox runat="server" ID="txtposicion" CssClass="form-control"   placeholder="Enter ..."></asp:TextBox>
                                            </div>


                                            <div class="col-md-6">

                                                <label for="select1">Comentarios:</label>
                                                <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control" TextMode="MultiLine"  placeholder="Enter ..."></asp:TextBox>
                                            </div>

                                        </div>

                                        <br />
                                            <div class="row">
                                                
                                                    <asp:Button ID="btnNew" runat="server" Text="Agregar" class="btn btn-primary" OnClick="btnNew_Click" />
                                         
                                                    <asp:Button ID="btnEdit" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnEdit_Click" />
                                             
                                                    <button type="button" id="btnLimpiar" class="btn btn-danger" onclick="limpiarFormulario()">Limpiar</button>
                                                

                                            </div>

                                    </div>
                                </div>


                                <!------------------------->

                                           </div>
                                </div>

                                <!-- /.box -->
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.col -->
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

    <script src="plugins/jQuery/jquery.min.js"></script>
    <!-- Bootstrap 3.3.6 -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- Select2-->
    <script src="plugins/select2/select2.full.min.js"></script>

     <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="plugins/fastclick/fastclick.js"></script>
    <!-- AdminLTE App 
    <script src="dist/js/app.min.js"></script>
    <!-- AdminLTE for demo purposes 
    <script src="dist/js/demo.js"></script>-->

    <!--Validaciones Form 
    <script src="js/validacionesForm.js"></script>
    <script src="js/script.js"></script>-->

    <!-- DataTables -->
    <script src="plugins/iCheck/icheck.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/jquery.validate.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/additional-methods.min.js"></script>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js"></script>
    <script>

       
    

    $(document).ready(function () {
        $('#modalBusqueda').on('shown.bs.modal', function () {
            var modal = $(this);
            var modalDialog = modal.find('.modal-dialog');
            var modalContent = modal.find('.modal-content');
            var windowWidth = $(window).width();
            var contentWidth = modalContent.width();
            var marginLeft = windowWidth / 2 - contentWidth / 2;
            modalDialog.css('margin-left', marginLeft);
        });


    });

      // Inicializar DataTable para example1

      $('#tb_datos').DataTable({

          "paging": true,

          "info": true,

          "iDisplayStart": 0,

          "aLengthMenu": [10, 20, 50, 100],

          "select": true,

          "order": [[1, "desc"]]

      });

      //Flat red color scheme for iCheck

        function limpiarFormulario() {
            document.getElementById('txtCodigo').value = '';
            document.getElementById('radio1').checked = false;
            document.getElementById('radio2').checked = false;
            document.getElementById('ddl_puesto').selectedIndex = 0;
            document.getElementById('ddl_rol').selectedIndex = 0;
            document.getElementById('select2').selectedIndex = 0;
            document.getElementById('select3').selectedIndex = 0;
            document.getElementById('select4').selectedIndex = 0;
            document.getElementById('select5').selectedIndex = 0;
            document.getElementById('ddl_infogiro').selectedIndex = 0;
            document.getElementById('TextBox2').value = '';
            window.location.href = 'configuracionPuestos.aspx';
        }


        $(document).ready(function () {
            $('#form1').validate({
                rules: {
            '<%= txtCodigo.ClientID %>': {
                        required: true
                    },
            '<%= ddl_puesto.ClientID %>': {
                        required: true
                    },
                '<%= select2.ClientID %>': {
                        required: true
                    },
            '<%= select3.ClientID %>': {
                        required: true
                    },
            '<%= select4.ClientID %>': {
                        required: true
                    },
            '<%= select5.ClientID %>': {
                        required: true
                    },
              '<%= ddl_infogiro.ClientID %>': {
                        required: true
                    },

                '<%= ddl_Giro.ClientID %>': {
                        required: true
                    },
                '<%= txtposicion.ClientID %>': {
                        required: true
                    },
                },
                messages: {
            '<%= txtCodigo.ClientID %>': {
                        required: "Por favor, ingrese un código"
                    },

            '<%= ddl_puesto.ClientID %>': {
                        required: "Por favor, seleccione un Puesto"
                    },

            '<%= select2.ClientID %>': {
                        required: "Por favor, seleccione un nivel"
                    },
            '<%= select3.ClientID %>': {
                        required: "Por favor, seleccione una unidad de negocio"
                    },
            '<%= select4.ClientID %>': {
                        required: "Por favor, seleccione una empresa"
                    },
            '<%= select5.ClientID %>': {
                        required: "Por favor, seleccione un tipo de puesto"
                    },
              '<%= ddl_infogiro.ClientID %>': {
                        required: "Por favor, seleccione un area adscribe"
                    },
            '<%= ddl_Giro.ClientID %>': {
                        required: "Por favor, seleccione un Puesto GIRO"
                    },
                    '<%= txtposicion.ClientID %>': {
                        required: "Por favor, ingrese una posicion"
                    },
                }
            });
});

        function confirmarEliminacion() {
            if (confirm("¿Estás seguro de que deseas eliminar este elemento?")) {
                // Aquí puedes agregar el código para eliminar el elemento
                alert("Elemento eliminado correctamente.");
            } else {
                // Aquí puedes agregar el código si el usuario decide no eliminar el elemento
                alert("Eliminación cancelada.");
            }
        }




    </script>


</body>
</html>

