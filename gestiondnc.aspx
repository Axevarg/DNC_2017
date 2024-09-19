<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="gestiondnc.aspx.vb" Inherits="DNC_2017.gestiondnc" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Gestión DNC | SIGIDO</title>
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

            gridView1 = $('#grdCursos').gridviewScroll({
                width: 'auto',
                height: 300,
                //    headerrowcount: 1,
                freezesize: 4,
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }


            });
            gridviewScrollHis();
        }
        //Historico

        function gridviewScrollHis() {

            gridView1 = $('#grdHistorico').gridviewScroll({
                width: 'auto',
                height: 300,
                headerrowcount: 1,

            });
        }
        function combo() {
            //$("#ddlCurso").select2();
            //$("#ddlAgreCurso").select2();
            $("#ddlColaborador").select2();

            $("#ddlColaborador").tooltip({
                title: function () {
                    return $(this).prev().attr("title");
                },
                placement: "auto"
            });
            //closeOnSelect: false
        }

        //Metodo de compartamiento en Tabs
        function tab(id) {
            document.getElementById('hdIdTab').value = id;
            document.getElementById('lblPrueba').innerHTML = id;

        }
         //Desabilitar el primer Item de los Objetivos Corporativos

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
                        <h1>Detección de Necesidades de Capacitación 
          
          <small>DNC</small>
                        </h1>

                    </section>

                    <!-- Main content -->
                    <section class="content">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>

                                <!-- Modal -->
                                <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="modalTemario" runat="server">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title" id="myModalLabel2">
                                                    <asp:Label ID="lblCursoTemario" runat="server" Text="Label"></asp:Label></h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">

                                                        <iframe id="iFramePdf" runat="server" frameborder="0" width="860" height="430" marginheight="0" marginwidth="0"></iframe>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblErrorArchivo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--Fin  Modal -->

                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <!--Datos-->
                                <div id="divDatos" runat="server" class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Nombre de Colaborador</label>
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddlColaborador" class="form-control select2" runat="server" Width="100%" AutoPostBack="true" onchange="cargarInfo();"></asp:DropDownList>
                                                <img id="loading" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        <!-- /.form group -->
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="lblNombreCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                             <asp:Label ID="lblPuestoCol" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                              <asp:Label ID="lblExperiencia" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                             <asp:Label ID="lblProfesion" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                         <asp:Label ID="lblArea" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                        <asp:Label ID="lblDireccion" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                         <asp:Label ID="lblEstatus" runat="server" Font-Size="10pt" Text="" for="lblNombreB"></asp:Label>
                                        </div>
                                       
                                    </div>
                                </div>
                                <!--Check-->
                                <div id="divCheck" runat="server" class="row">
                                    <div class="col-md-6">
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkHistorico" runat="server" AutoPostBack="true" onchange="cargarInfo();" />

                                                Consultar Histórico de Capacitación
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <!--Historico-->
                                <div id="divHistorico" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Histórico de Capacitación</h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnExportarHis" name="btnExportarHis" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid Historico-->
                                                <div class="row">
                                                    <div class="col-md-8"></div>
                                                    <div class="col-md-4 pull-right">
                                                        <dl class="dl-horizontal">
                                                            <dt>Año</dt>
                                                            <dd>
                                                                <asp:DropDownList ID="ddlAnio" runat="server" class="form-control  input-sm" Style="width: 100%;" AutoPostBack="true" onchange="cargarAnio();">
                                                                </asp:DropDownList>
                                                                <img id="loadingAnio" src="img/glyphLoading.gif" style="display: none" />
                                                            </dd>
                                                        </dl>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="grdHistorico" runat="server" class="table table-condensed" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" ShowFooter="True" Style="font-size: 8pt; text-align: center"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None"
                                                            PageSize="10">
                                                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                            <HeaderStyle Font-Size="9pt" />
                                                            <Columns>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <strong>
                                                            <asp:Label ID="lblDatosHistoric" runat="server" Text="Label"></asp:Label>
                                                        </strong>
                                                    </div>
                                                </div>
                                                <!-- /.Grid Historico -->
                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>
                                <!-- Fin Historico-->
                                <!--Registro DNC-->
                                <div id="divRegistro" runat="server" class="row">

                                    <div class="col-md-12">
                                        <!-- DIRECT CHAT DANGER -->
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Registro de Capacitación</h3>

                                                <div class="box-tools pull-right">
                                                    <button id="btnExportarCursos" name="btnExportarCursos" runat="server" type="button" class="btn btn-danger btn-flat btn-sm" data-toggle="tooltip" data-placement="rigth" title="Exportar Excel">
                                                        <i class="fa fa-file-excel-o"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <!-- /.box-header -->
                                            <div class="box-body">
                                                <!-- Grid -->
                                                <div class="row">

                                                    <div class="col-md-12">

                                                        <asp:GridView ID="grdCursos" runat="server" class="table table-condensed" AllowPaging="False"
                                                            AutoGenerateColumns="False" Width="100%" ShowFooter="True" Style="font-size: 8pt; text-align: center"
                                                            Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None">
                                                            <HeaderStyle Font-Size="9pt" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="NO">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNo" runat="server" Text='<%# Bind("no_correlativo")%>' Width="10px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="CURSO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlCurso" runat="server" class="form-control select2 input-sm" Width="300px" ClientIDMode="Static" OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged" AutoPostBack="true" onchange="cargarInfo();">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("fk_id_curso")%>' Width="300px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreCurso" runat="server" class="form-control select2 input-sm" Width="300px" ClientIDMode="Static" OnSelectedIndexChanged="ddlAgreCurso_SelectedIndexChanged" AutoPostBack="true" onchange="cargarInfo();">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="VER">
                                                                    <HeaderStyle />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkTemario" runat="server" Text='<%# Bind("ruta")%>' Width="40px" ForeColor="#333333" data-toggle="tooltip" data-placement="top" title="Ver Temario"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgreTemario" runat="server" Text='<%# Bind("ruta")%>' Width="40px" ForeColor="#333333" data-toggle="tooltip" data-placement="left" title="Ver Temario"></asp:LinkButton>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PRIORIDAD">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlPrioridad" runat="server" class="form-control input-sm" Width="50px" Visible="false">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblPrioridad" runat="server" Text='<%# Bind("prioridad_ejecucion")%>' Width="50px" ForeColor="#333333"></asp:Label>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>

                                                                        <asp:DropDownList ID="ddlPrioridadItem" runat="server" class="form-control input-sm" Width="100px" AutoPostBack="false" onchange="validaCambio();">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <%--   <asp:DropDownList ID="ddlAgrePrioridad" runat="server" class="form-control input-sm" Width="50px">
                                                           </asp:DropDownList>--%>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="AÑO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlAnio" runat="server" class="form-control input-sm" Width="70px" Visible="false">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblAnio" runat="server" Text='<%# Bind("fk_id_anio")%>' Width="70px" ForeColor="#333333"></asp:Label>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>

                                                                        <asp:DropDownList ID="ddlAnioItem" runat="server" class="form-control input-sm" Width="100px" onchange="validaCambio();">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <%--   <asp:DropDownList ID="ddlAgreAnio" runat="server" class="form-control input-sm" Width="70px">
                                                
                                                            </asp:DropDownList>--%>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MES">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlMes" runat="server" class="form-control input-sm" Width="110px" Visible="false">
                                                                            <asp:ListItem></asp:ListItem>
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
                                                                        <asp:Label ID="lblMes" runat="server" Text='<%# Bind("mes_programado")%>' Width="110px" ForeColor="#333333"></asp:Label>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>

                                                                        <asp:DropDownList ID="ddlMesItem" runat="server" class="form-control input-sm" Width="110px" onchange="validaCambio();">
                                                                            <asp:ListItem> Seleccionar</asp:ListItem>
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

                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <%--            <asp:DropDownList ID="ddlAgreMes" runat="server" class="form-control input-sm" Width="110px">
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
                                                            </asp:DropDownList>--%>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MOTIVO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlMotivo" runat="server" class="form-control input-sm" Width="200px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMotivo" runat="server" Text='<%# Bind("fk_id_motivo")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreMotivo" runat="server" class="form-control input-sm" Width="150px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="OBJETIVO CORPORATIVO">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlObjCorp" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblObjCorp" runat="server" Text='<%# Bind("fk_id_objetivo_corporativo")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreObjCorp" runat="server" class="form-control input-sm" Width="150px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="COMPETENCIA VINCULADA">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlCompetencia" runat="server" class="form-control input-sm" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCompetencia" runat="server" Text='<%# Bind("fk_id_competencia_vinculada")%>' Width="200px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreCompetencia" runat="server" class="form-control input-sm" Width="200px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TIPO DE INDICADOR">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlTIndidador" runat="server" class="form-control input-sm" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlTIndidador_SelectedIndexChanged" onchange="cargarPaginacion();">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTIndicador" runat="server" Text='<%# Bind("fk_id_tipo_indicador")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreTIndicador" runat="server" class="form-control input-sm" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlAgreTIndicador_SelectedIndexChanged" onchange="cargarPaginacion();">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="INDICADOR">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlIndicador" runat="server" class="form-control input-sm" Width="240px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndicador" runat="server" Text='<%# Bind("fk_id_indicador")%>' Width="240px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreIndicador" runat="server" class="form-control input-sm" Width="240px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MEDIR EFECTIVIDAD">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlMedicion" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMedicion" runat="server" Text='<%# Bind("fk_id_medicion_efectividad")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreMedicion" runat="server" class="form-control input-sm" Width="150px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ESTATUS">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEstatus" runat="server" class="form-control input-sm" Width="150px">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("fk_id_estatus")%>' Width="150px" ForeColor="#333333"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAgreEstatus" runat="server" class="form-control input-sm" Width="150px">
                                                                            <asp:ListItem Value="0">SELECCIONAR</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="EDITAR">
                                                                    <ControlStyle CssClass="label label-danger" Font-Size="8pt" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField HeaderText="ELIMINAR" ControlStyle-Font-Size="8pt">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAgregar" runat="server" Text="Agregar" class="label label-danger letra" ToolTip="Agregar" OnClick="lnkAgregar_Click" OnClientClick="document.getElementById('loadingPagina').style.display = 'inline'"></asp:LinkButton>
                                                                    </FooterTemplate>
                                                                    <FooterStyle Font-Size="8pt" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                        </asp:GridView>
                                                        <img id="loadingPagina" runat="server" src="img/glyphLoading.gif" style="display: none" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblGrid" runat="server" Text="Label"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <strong>
                                                            <asp:Label ID="lblGuardar" class="guardar" for="inputError" runat="server" Text="Label"></asp:Label></strong>
                                                        <strong>
                                                            <asp:Label ID="lblErrorGestion" class="control-label" for="inputError" runat="server" Text="Label"></asp:Label></strong>

                                                    </div>
                                                </div>
                                                <!-- /.Grid  -->
                                            </div>
                                            <!-- /.box-body -->

                                            <!-- /.box-footer-->
                                            <div class="box-footer" style="text-align: center">

                                                <button class="btn btn-danger btn-flat" runat="server" id="btnGuardar" name="btnGuardar" onclick="return validaDNC();">
                                                    <i class="fa fa-save"></i>
                                                    Guardar Cursos para el Colaborador
                                                </button>
                                                <img id="loadingA" src="img/glyphLoading.gif" style="display: none" />
                                            </div>
                                        </div>
                                        <!--/.direct-chat -->
                                    </div>
                                </div>

                                <!-- FIN Registro DNC--->
                                <div class="row">
                                    <div class="col-md-12" style="text-align: center;">
                                        <strong>
                                            <asp:Label ID="lblColaborador" runat="server" Text=""></asp:Label>
                                        </strong>

                                    </div>
                                </div>
                                <!-- /.row -->

                                <!-- END CUSTOM TABS -->
                                <asp:HiddenField ID="hdIdTab" runat="server" />
                                <asp:HiddenField ID="hdIdCurso" runat="server" />
                                <asp:HiddenField ID="hdIndexCurso" runat="server" />
                                <asp:HiddenField ID="hdIndexPrioridad" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExportarHis" />
                                <asp:PostBackTrigger ControlID="btnExportarCursos" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdNoNominaUsuario" runat="server" />
                        <asp:HiddenField ID="hdIdDNC" runat="server" />
                        <asp:HiddenField ID="hdMaxCurso" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
                        <!-- /.box -->
                        <asp:HiddenField ID="hfGridView1SV" runat="server" />
                        <asp:HiddenField ID="hfGridView1SH" runat="server" />

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

        //VALIDACION DE INSERCION
        function validaciones() {

            var selects = document.getElementById("ddlFacilitador");
            var selectedValue = selects.options[selects.selectedIndex].value;
            //Facilitador
            if (selectedValue == 0) {
                alert("Debe de agregar un Facilitador al Proveedor.");
                return false;
            }

            //Descripcion
            if (document.getElementById("txtDescripcion").value == '') {
                alert("Debe capturar la descripción.");
                document.getElementById("txtDescripcion").focus();
                return false;
            }
            //Objetivo
            if (document.getElementById("txtObjetivo").value == '') {
                alert("Debe capturar el Objetivo. ");
                document.getElementById("txtObjetivo").focus();
                return false;
            }
            //Habilidades A Desarrollar
            if (document.getElementById("txtHabilidadesDesarrollar").value == '') {
                alert("Debe capturar las habilidades a Desarrollar");
                document.getElementById("txtHabilidadesDesarrollar").focus();
                return false;
            }
            //No. Máximo Participantes por Grupo
            if (document.getElementById("txtNoMaximoParticipante").value == '') {
                alert("Debe capturar el No. Máximo Participantes por Grupo.");
                document.getElementById("txtNoMaximoParticipante").focus();
                return false;
            }
            //Duración Horas
            if (document.getElementById("txtDuracionHoras").value == '') {
                alert("Debe capturar la Duración Horas.");
                document.getElementById("txtDuracionHoras").focus();
                return false;
            }
            //Costo Individual (Sin IVA)
            if (document.getElementById("txtCostoIndividual").value == '') {
                alert("Debe capturar el Costo Individual (Sin IVA).");
                document.getElementById("txtCostoIndividual").focus();
                return false;
            }

            //Costo Por Grupo (Sin IVA)
            if (document.getElementById("txtCostoGrupo").value == '') {
                alert("Debe capturar el Costo por Grupo.");
                document.getElementById("txtCostoGrupo").focus();
                return false;
            }

            //Duración Horas
            if (document.getElementById("txtDuracionHoras").value == '') {
                alert("Debe capturar la Duración Horas.");
                document.getElementById("txtDuracionHoras").focus();
                return false;
            }

            return true;

        }

        function insCursos() {


            if (validaciones() == false) {
                return false;
            }
            var theControl = document.getElementById("btnAgregar");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }

        function cargarInfo() {

            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }


        function updCurso() {


            if (validaciones() == false) {
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

        //desabilitar el boton derecho
        //      document.oncontextmenu = function () { return false }

        function nuevoCurso() {
            var theControl = document.getElementById("btnNuevoCurso");
            theControl.style.display = "none";

            var theImg = document.getElementById("loadingA");
            theImg.style.display = "inline";

            return true;
        }



        //limipia label
        function cleanCtrl() {
            document.getElementById("lblNombreB").innerHTML = '';
            document.getElementById("lblDatos").innerHTML = '';
            document.getElementById("lblDireccion").innerHTML = '';
            document.getElementById("lblConductor").innerHTML = '';


        }

        function obtInfoColaborador() {

            var varNomina = document.getElementById("txtNomina");

            if (varNomina.value == '') {
                alert("Debe capturar un Número de Nómina");
                varNomina.focus();
                return false;
            }

            var theControl = document.getElementById("bntObtInfo");
            theControl.style.display = "none";

            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

            return true;
        }

        function cargarInfo() {
            var theImg = document.getElementById("loading");
            theImg.style.display = "inline";

        }
        //valida si necesita guardar la DNC
        function validaEdicion() {
            cargarInfo();
        }

        function cargarPaginacion() {
            var theImg = document.getElementById("loadingPagina");
            theImg.style.display = "inline";

        }


        function cargarAnio() {
            var theImg = document.getElementById("loadingAnio");
            theImg.style.display = "inline";
        }

        //muestra Imagen en Galeria
        function imgModalTemario(ruta) {
            document.getElementById("iFramePdf").src = ruta;



        }

        function limpiaLabel() {
            document.getElementById("lblNombreCol").innerHTML = '';
            document.getElementById("lblDatos").innerHTML = '';
            document.getElementById("lblDireccion").innerHTML = '';
            document.getElementById("lblArea").innerHTML = '';
            document.getElementById("divNomina").className = '';

        }



        function validaDNC() {

            if (!confirm('¿Desea Guardar los Cursos para el colaborador?')) { return false; };

            var theImg = document.getElementById("loadingPagina");
            theImg.style.display = "inline";
            return true;
        }

        function validaCambio() {

            document.getElementById("hdIndexPrioridad").value = 1;
            document.getElementById("lblGuardar").innerHTML = 'Debe de Guardar los cursos para que tome las nuevas prioridades.';
            document.getElementById("ddlColaborador").disabled = true;
        }


        //valida que no te salgas hasta que se guarde el cambio
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (document.getElementById("hdIndexPrioridad").value == 1) {
                return "Va cerrar esta pagina, debe de guardar los cursos.";
            }

        }


    </script>


</body>
</html>
