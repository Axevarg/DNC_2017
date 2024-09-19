|<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="catalogosDO.aspx.vb" Inherits="DNC_2017.catalogosDO" EnableEventValidation="false" %>

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

    <!-- jQuery 2.2.3 
    <script src="plugins/jQuery/jquery-2.2.3.min.js"></script>-->
    <script src="js/jquery_1-9-0.js"></script>
    <script src="js/jquery-ui_1_9_1.min.js"></script>
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

            gridView1 = $('#grdPuestos').gridviewScroll({
                width: 'auto',
                height: 500,
                headerrowcount: 1,
                freezesize: 2,
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

    </script>
    <!-- Bootstrap 3.3.6 -->
    <script src="js/bootstrap.min.js"></script>
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
    <style type="text/css">
        #grdPuestos th {
            text-align: center;
        }

        .GridHeader {
            text-align: center !important;
        }

        .alignCenter {
            text-align: center !important;
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
                <br /><br />
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
                                                <h3 class="box-title">Catálogos Generales Estructura de Puestos</h3>
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

                                                    <div class="col-md-12" id="div1" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">
                                                                <li id="Li1" runat="server" class="active"><a href="#tabGeneral_1" data-toggle="tab">Puestos</a></li>
                                                                <img id="loadingCatalogos" runat="server" src="img/glyphLoading.gif" style="display: none" />

                                                            </ul>
                                                            <!-- Competencia vinculada -->
                                                            <div class="tab-content">
                                                                <div class="tab-pane active" id="tabGeneral_1" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div9" runat="server">
                                                                            <asp:GridView ID="grdPuestos" runat="server" AllowPaging="False"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 6pt;" HeaderStyle-CssClass="GridHeader"
                                                                                RowStyle-Height="10px" ShowFooter="True" class="table table-bordered"
                                                                                Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="10">
                                                                                <PagerSettings PageButtonCount="20" Mode="NextPreviousFirstLast" FirstPageImageUrl="img/resultset_first.png" LastPageImageUrl="img/resultset_last.png" NextPageImageUrl="img/resultset_next.png" PreviousPageImageUrl="img/resultset_previous.png" />
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <HeaderStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />

                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Código" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt">

                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtCodigo" runat="server" Text='<%# Bind("codigo")%>' Width="40px" Font-Size="6pt" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="5"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("codigo")%>' Width="40px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreCodigo" runat="server" Font-Size="6pt" Width="40px" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="5"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Puesto / Rol" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="alignCenter" HeaderStyle-Font-Size="8pt" >
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtDecripcion" runat="server" Text='<%# Bind("descripcion")%>' Width="300px" Font-Size="6pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>' Width="300px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreDescripcion" runat="server" class="form-control input-sm" Width="300px" Font-Size="6pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Nivel" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt" HeaderStyle-CssClass="alignCenter" >
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlNivel" runat="server" class="form-control" Font-Size="6pt" Width="115px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblNivel" runat="server" Text='<%# Bind("DescNivel")%>' Width="150px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreNivel" runat="server" class="form-control" Font-Size="6pt" Width="80px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Tipo de Puestos" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt" HeaderStyle-CssClass="alignCenter">

                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlTipoPuesto" runat="server" class="form-control" Font-Size="6pt" Width="80px">
                                                                                                <asp:ListItem>Administrativo</asp:ListItem>
                                                                                                <asp:ListItem>Operativo</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTipoPuesto" runat="server" Text='<%# Bind("tipo_puesto")%>' Width="115px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreTipoPuesto" runat="server" class="form-control" Font-Size="6pt" Width="80px">
                                                                                                <asp:ListItem>Administrativo</asp:ListItem>
                                                                                                <asp:ListItem>Operativo</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Compañía" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEmpresa" runat="server" class="form-control" Font-Size="6pt" Width="200px">
                                                                                                <asp:ListItem Value="002">Passa Administración y Servicios S.A. de C.V</asp:ListItem>
                                                                                                <asp:ListItem Value="001">DINA CAMIONES</asp:ListItem>
                                                                                                <asp:ListItem Value="003">MERCADER FINANCIAL</asp:ListItem>
                                                                                                <asp:ListItem Value="004">DINA COMERCIALIZACION AUTOMOTRIZ (DICOMER)</asp:ListItem>
                                                                                                <asp:ListItem Value="005">TRANSPORTES Y LOGISTICA DE JALISCO</asp:ListItem>
                                                                                                <asp:ListItem Value="006">DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV (DICOSER)</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEmpresa" runat="server" Text='<%# Bind("empresa")%>' Width="200px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEmpresa" runat="server" class="form-control" Font-Size="6pt" Width="200px">
                                                                                                <asp:ListItem Value="002">PASSA ADMINISTRACION Y SERVICIOS S.A. DE C.V.</asp:ListItem>
                                                                                                <asp:ListItem Value="001">DINA CAMIONES</asp:ListItem>
                                                                                                <asp:ListItem Value="003">MERCADER FINANCIAL</asp:ListItem>
                                                                                                <asp:ListItem Value="004">DINA COMERCIALIZACION AUTOMOTRIZ (DICOMER)</asp:ListItem>
                                                                                                <asp:ListItem Value="005">TRANSPORTES Y LOGISTICA DE JALISCO</asp:ListItem>
                                                                                                <asp:ListItem Value="006">DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV (DICOSER)</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Unidad Negocio" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlUNegocio" runat="server" class="form-control" Font-Size="6pt" Width="80px">
                                                                                                <asp:ListItem >Dina</asp:ListItem> 
                                                                                                <asp:ListItem >Mercader</asp:ListItem>
                                                                                                <asp:ListItem >Dicoser</asp:ListItem>
                                                                                                <asp:ListItem >Passa</asp:ListItem>
                                                                                                <asp:ListItem >Dicomer</asp:ListItem>
                                                                                                <asp:ListItem >TLJ</asp:ListItem>

                                                                                          </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblUNegicio" runat="server" Text='<%# Bind("unidad_negocio")%>' Width="80px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreUNegicio" runat="server" class="form-control" Font-Size="6pt" Width="80px">
                                                                                                 <asp:ListItem >Dina</asp:ListItem> 
                                                                                                <asp:ListItem >Mercader</asp:ListItem>
                                                                                                <asp:ListItem >Dicoser</asp:ListItem>
                                                                                                <asp:ListItem >Passa</asp:ListItem>
                                                                                                <asp:ListItem >Dicomer</asp:ListItem>
                                                                                                <asp:ListItem >TLJ</asp:ListItem>

                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                           <asp:TemplateField HeaderText="Posición" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                               <asp:TextBox ID="txtPosicion" runat="server" Text='<%# Bind("posiciones")%>' Width="40px" Font-Size="6pt" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="5"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPosicion" runat="server" Text='<%# Bind("posiciones")%>' Width="40px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                               <asp:TextBox ID="txtAgregarPosicion" runat="server" Width="40px" Font-Size="6pt" class="form-control input-sm" onkeypress="return validaNumeroEntero(event);" onchange="replaceCaracteres(this)" MaxLength="5"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                          <asp:TemplateField HeaderText="Jefe" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlClaveJefe" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblClaveJefe" runat="server" Text='<%# Bind("Nombre_Jefe")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreClaveJefe" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"  HeaderStyle-CssClass="alignCenter" HeaderStyle-Font-Size="8pt">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN13" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN13" runat="server" Text='<%# Bind("n13Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre13" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN12" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN12" runat="server" Text='<%# Bind("n12Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre12" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN11" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN11" runat="server" Text='<%# Bind("n11Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre11" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN10" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN10" runat="server" Text='<%# Bind("n10Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre10" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN9" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN9" runat="server" Text='<%# Bind("n9Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre9" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN8" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN8" runat="server" Text='<%# Bind("n8Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre8" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN7" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN7" runat="server" Text='<%# Bind("n7Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre7" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN6" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN6" runat="server" Text='<%# Bind("n6Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre6" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN5" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN5" runat="server" Text='<%# Bind("n5Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre5" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN4" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN4" runat="server" Text='<%# Bind("n4Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre4" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN3" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN3" runat="server" Text='<%# Bind("n3Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre3" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN2" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN2" runat="server" Text='<%# Bind("n2Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre2" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlN1" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblN1" runat="server" Text='<%# Bind("n1Puesto")%>' Width="250px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgre1" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                  
                                                                                
                                                                                          <asp:TemplateField HeaderText="Puesto GIRO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlPuestoGiro" runat="server" class="form-control input-sm" Font-Size="6pt" Width="300px">
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPuestoGiro" runat="server" Text='<%# Bind("Puesto_GIRO")%>' Width="350px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgrePuestoGiro" runat="server" class="form-control input-sm" Font-Size="6pt" Width="300px">
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Área adscribe " HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                                <asp:DropDownList ID="ddlAreaAdscribe" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px"></asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblArea" runat="server" Text='<%# Bind("Desarea_adscribe")%>' Width="260px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreAreaAdscribe" runat="server" class="form-control input-sm" Font-Size="6pt" Width="200px"></asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Comentarios" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtComentarios" runat="server" Text='<%# Bind("comentarios")%>' Width="200px" Font-Size="6pt" class="form-control input-sm" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblComentarios" runat="server" Text='<%# Bind("comentarios")%>' Width="200px" Font-Size="6pt"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAgreComentarios" runat="server" class="form-control input-sm" Width="200px" Font-Size="6pt" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="500"></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Estatus" HeaderStyle-Font-Size="8pt"  HeaderStyle-CssClass="alignCenter">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlEstatus" runat="server" Font-Size="6pt" CssClass="form-control input-sm" Width="80px">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                                <asp:ListItem Value="0">Deshabilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus")%>' Width="80px"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreEstatus" runat="server" Font-Size="6pt" class="form-control input-sm" Width="80px">
                                                                                                <asp:ListItem Value="1">Habilitado</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                     
                                                                                    <asp:CommandField ShowEditButton="True" ShowHeader="true" UpdateText="Actualizar" CancelText="Cancelar" EditText="Editar" HeaderText="Editar" HeaderStyle-Font-Size="8pt">
                                                                                        <ControlStyle CssClass="label label-danger" Font-Size="6pt" />

                                                                                    </asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="Eliminar" HeaderStyle-Font-Size="8pt">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Delete" Font-Size="6pt" class="label label-danger" ToolTip="Eliminar"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAgregarPuesto" runat="server" Font-Size="6pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPuesto_Click" OnClientClick="CargaInformacion();"></asp:LinkButton>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>

                                                                                <PagerStyle BackColor="#3f4551" ForeColor="White" CssClass="pagination" />
                                                                                <RowStyle BackColor="#E1E0DC" ForeColor="#333333" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                                            </asp:GridView>


                                                                        </div>
                                                                        <img id="Img1" runat="server" src="img/glyphLoading.gif" style="display: none" />
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
                                <!--Catalogos Generales-->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Catálogos de Estructura de puestos</h3>
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

                                                    <div class="col-md-12" id="div7" runat="server">
                                                        <!-- Custom Tabs -->
                                                        <div class="nav-tabs-custom">
                                                            <ul class="nav nav-tabs">

                                                                <li id="lnkTabRec2" runat="server" class="active"><a href="#tabReclutamiento_2" data-toggle="tab" onclick="controlTabsReclumiento(2);">Carrera o Especialización</a></li>
                                                                <li id="lnkTabRec3" runat="server"><a href="#tabReclutamiento_3" data-toggle="tab" onclick="controlTabsReclumiento(3);">Escolaridad</a></li>
                                                                <li id="lnkTabRec4" runat="server"><a href="#tabReclutamiento_4" data-toggle="tab" onclick="controlTabsReclumiento(4);">Facultades de Autorización</a></li>
                                                                <li id="lnkTabRec5" runat="server"><a href="#tabReclutamiento_5" data-toggle="tab" onclick="controlTabsReclumiento(5);">Habilidades y competencias</a></li>
                                                                <li id="lnkTabRec6" runat="server"><a href="#tabReclutamiento_6" data-toggle="tab" onclick="controlTabsReclumiento(6);">Idiomas</a></li>
                                                                <li id="lnkTabRec7" runat="server"><a href="#tabReclutamiento_7" data-toggle="tab" onclick="controlTabsReclumiento(7);">Nivel</a></li>
                                                                <li id="lnkTabRec8" runat="server"><a href="#tabReclutamiento_8" data-toggle="tab" onclick="controlTabsReclumiento(8);">Puestos</a></li>
                                                                <li id="lnkTabRec9" runat="server"><a href="#tabReclutamiento_9" data-toggle="tab" onclick="controlTabsReclumiento(9);">Roles</a></li>
                                                                <li id="lnkTabRec10" runat="server"><a href="#tabReclutamiento_10" data-toggle="tab" onclick="controlTabsReclumiento(10);">Puestos-Roles</a></li>


                                                            </ul>


                                                            <!-- Competencia vinculada -->
                                                            <div class="tab-content">

                                                                <!-- Carrera -->
                                                                <div class="tab-pane active" id="tabReclutamiento_2" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div2" runat="server">
                                                                            <asp:GridView ID="grdCarreras" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarCarreras" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarCarreras_Click"></asp:LinkButton>
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
                                                                <!-- Carrera -->

                                                                <!-- Escolaridad -->
                                                                <div class="tab-pane" id="tabReclutamiento_3" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div3" runat="server">
                                                                            <asp:GridView ID="grdEscolaridad" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarEscolaridad" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarEscolaridad_Click"></asp:LinkButton>
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
                                                                <!-- Escolaridad -->

                                                                <!-- Facultades de autorizacion -->
                                                                <div class="tab-pane" id="tabReclutamiento_4" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div5" runat="server">
                                                                            <asp:GridView ID="grdFacultades" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarFacultades" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarFacultades_Click"></asp:LinkButton>
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
                                                                <!-- Facultades de autorizacion -->

                                                                <!-- Habilidades y Competencia -->
                                                                <div class="tab-pane" id="tabReclutamiento_5" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div8" runat="server">
                                                                            <asp:GridView ID="grdHabilidades" runat="server" AllowPaging="True"
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
                                                                <!-- Habilidades y Competencia -->

                                                                <!-- Idioma -->
                                                                <div class="tab-pane" id="tabReclutamiento_6" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div10" runat="server">
                                                                            <asp:GridView ID="grdIdioma" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarIdioma" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarIdioma_Click"></asp:LinkButton>
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
                                                                <!-- Idioma -->

                                                                <!-- Nivel -->
                                                                <div class="tab-pane" id="tabReclutamiento_7" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div6" runat="server">
                                                                            <asp:GridView ID="grdNivel" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarNivel" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarNivel_Click"></asp:LinkButton>
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
                                                                <!-- Nivel -->

                                                                 <div class="tab-pane" id="tabReclutamiento_8" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div11" runat="server">
                                                                            <asp:GridView ID="grdPuestosAgre" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarPuestosAgre" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPuestosAgre_Click"></asp:LinkButton>
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

                                                                 <div class="tab-pane" id="tabReclutamiento_9" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div12" runat="server">
                                                                            <asp:GridView ID="grdRoles" runat="server" AllowPaging="True"
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
                                                                                            <asp:LinkButton ID="lnkAgregarRol" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarRol_Click"></asp:LinkButton>
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

                                                                
                                                                 <div class="tab-pane" id="tabReclutamiento_10" runat="server">
                                                                    <div class="row">
                                                                        <div class="col-md-12" id="div13" runat="server">
                                                                            <asp:GridView ID="grdPuestoRol" runat="server" AllowPaging="True"
                                                                                AutoGenerateColumns="False" Width="100%" Style="font-size: 8pt;"
                                                                                RowStyle-Height="10px" ShowFooter="True" class="table table-condensed"
                                                                                Font-Size="Small" HorizontalAlign="Justify" CellPadding="1" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" PageSize="20">
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
                                                                                            <asp:DropDownList ID="ddlDescripcion" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreDescripcion" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="ROLES" HeaderStyle-Width="70%">
                                                                                        <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRol" runat="server" Text='<%# Bind("rol")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlAgreRol" runat="server" class="form-control" onkeypress="return caracteres(event);" onchange="replaceCaracteres(this)" MaxLength="100"></asp:DropDownList>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>



                                                                                    <asp:TemplateField HeaderText="ESTATUS" >
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
                                                                                            <asp:LinkButton ID="lnkAgregarPuestos" runat="server" Font-Size="8pt" class="label label-danger" Text="Agregar" ToolTip="Agregar" OnClick="lnkAgregarPuestoRol_Click" ></asp:LinkButton>
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


                                <asp:HiddenField ID="hdIdTabReclutamiento" runat="server" />
                                <asp:HiddenField ID="hdIdTabBecas" runat="server" />
                                <asp:HiddenField ID="hdIdTabBecasIngles" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdIdDNC" runat="server" />
                        <asp:HiddenField ID="hdRol" runat="server" />
                        <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
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

        //Control de Tabs DNC
        function controlTabsReclumiento(id) {
            //Asigna el valor de la Tab de la DNC
            document.getElementById('hdIdTabReclutamiento').value = id;

        }


        function CargaInformacion() {
            var theImg = document.getElementById("loadingCatalogos");
            theImg.style.display = "inline";
        }


    </script>
</body>
</html>
