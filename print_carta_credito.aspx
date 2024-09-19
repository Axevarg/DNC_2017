<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="print_carta_credito.aspx.vb" Inherits="DNC_2017.print_carta_credito" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

  <title>Print Cartas de Crédito | SIGIDO</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
  <!-- Bootstrap 3.3.6 -->
    <!-- Bootstrap 3.3.4 
 <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>-->
   <link href="bootstrap/bootsrap%203.3.4/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
  <!-- Font Awesome -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css"/>
  <!-- Ionicons -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>

  <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
  <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
  <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

    <style>
        .tamano {
            font-size: 8pt;
        }

        @media print {
            div.saltopagina {
                display: block;
                page-break-before: always;
            }

            .hidden-print {
                display: none !important;
            }
        }
        .Naranja{
            color: #E36C0A !important; 

        }
    </style>
</head>
   
<body style="font-family: Tahoma;"  onload="window.print();">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdUsuario" runat="server" />
        <asp:HiddenField ID="hdRol" runat="server" />
             <asp:HiddenField ID="hdClaveEmpleadoAD" runat="server" />
        <div class="wrapper">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label"></asp:Label>
            <!-- Main content -->
            <section class="invoice">
                <!-- title row -->
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="page-header" style="color: #E36C0A;">
                        <%--    <strong class="Naranja" style="font-size: 24pt">PASSA </strong>--%>
                            <img src="img/passa.png" width="118" height="53"  />
                            <small class="pull-right Naranja">
                                <strong>
                                <asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label></strong></small>  <small class="pull-right"><strong>Fecha: </strong> </small>
                        </h2>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- info row -->

                <div class="row">
                    <div class="col-xs-12 table-responsive">
                        <address>
                            <strong>PARA: EISL MEXICO SC / INTERLINGUA</strong><br />
                            <br />
                            Plantel:
                            <asp:Label ID="lblPlantel" runat="server" Text="Label" class="Naranja"></asp:Label>
                        </address>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
                <br />
       
                <!-- Table row -->
                <div class="row">
                    <div class="col-xs-12 table-responsive">
                        Por medio de la presente, autorizamos a  <strong class="Naranja">
                            <asp:Label ID="lblNombreColaborador" runat="server" Text="Label"></asp:Label></strong> de la empresa 
         
                        <strong class="Naranja">PASSA ADMINISTRACIÓN Y SERVICIOS S.A DE C.V., </strong>
                        para <strong class="Naranja">
                                <asp:Label ID="lblTipoAutorizacion" runat="server" Text="Label"></asp:Label></strong> sus estudios de inglés, en el<strong class="Naranja">  
                            <asp:Label ID="lblNivel" runat="server" Text="Label"></asp:Label></strong> , 
                            Curso <strong class="Naranja">
                                <asp:Label ID="lblHorario" runat="server" Text="Label"></asp:Label></strong> en la institución a su cargo, bajo el convenio establecido 
        entre esta empresa con EISL MEXICO SC. Esta carta cubre el <strong class="Naranja">80%</strong> del curso.
         
                        <br />
                        <br />
                        Esta carta deberá ser presentada dentro de las fechas límite de inscripción que la sucursal establezca.
         
                        <br />
                        <br />
                        <strong>En caso de que el alumno no apruebe el nivel anterior al que se reinscribe mediante esta carta crédito, y la empresa no se le 
          autorice expresamente a repetir el curso y consecuente pago, esta carta debe ser devuelta a la empresa, ya que carecerá de validez.</strong>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
                <br />
                <br />
                <div class="row">
                    <!-- accepted payments column -->
                    <div class="col-xs-6">
                        <p class="lead">A t e n t a m e n t e, </p>
                        <img src="img/Firma_DO.png" width="135" height="69" />
                        <br />
                        <strong>Lic. Angel Del Valle Marín Rojas<br />
                            Coordinador de Capacitación
                        </strong>
                        
                    </div>
                    <!-- /.col -->
                    <div class="col-xs-6">
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
                <br />
                <br />
                <!-- Table row -->
                <div class="row">
                    <div class="col-xs-12 table-responsive">
                        <strong>NOTA: </strong>
                        <br />
                        1.	Si el alumno tiene que cubrir algún porcentaje de su curso, este deberá ser liquidado a la entrega de esta carta en su sucursal.<br />
                        2.	Para la emisión y renovación de esta carta crédito, el alumno deberá obtener como resultados mínimos una calificación de 8 y asistencia del 75%.<br />
                        <br />
                        <br />

                        <strong>SERIE CARTA: PB-IN-IN-<asp:Label ID="lblConsecutivo" runat="server" Text="Label"></asp:Label>
                        </strong>
                        <br />
                        <br />

                    </div>
                    <!-- /.col -->
                </div>
          
                <!--Row-->
                <div class="row" style="font-size: 6pt; border-top: 1px solid gray">
                    <div class="col-xs-12">
                        <p class="text-muted text-gray" style="margin-top: 10px; text-align: right;">
                            PASSA Administración y Servicios S.A. de C.V.<br />
                            Corredor Industrial S/N, Ciudad Fray Bernardino de Sahagún,<br />
                            Centro Cd. Sahagún, Acueducto y Vía de Ferrocarril
                            <br />
                            Tepeapulco Hidalgo 43990<br />
                        </p>
                    </div>
                </div>


                 
                 
            </section>
            <!-- /.content -->
        </div>
        <!-- ./wrapper -->
    </form>
</body>
</html>
