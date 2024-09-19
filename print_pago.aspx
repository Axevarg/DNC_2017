<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="print_pago.aspx.vb" Inherits="DNC_2017.print_pago" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Print Solicitud Pago | SIGIDO</title>
    <link rel="shortcut icon" href="img/favicon.ico" />
    <style>
        form input[type="text"], input[type="password"], textarea {
            font-size: 5pt;
        }

        .centrar {
            text-align: center;
        }

        .celda_derecha {
            border-right: 1px solid black;
        }

        .celda_izquierda {
            border-left: 1px solid black;
            border-left-style: solid;
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

        .auto-style6 {
            height: 2px;
        }

        .auto-style13 {
            height: 19px;
        }

        .auto-style15 {
            height: 20px;
        }

        .auto-style16 {
            height: 18px;
        }


        .tablaBor {
            border-spacing: 0;
            border-collapse: collapse;
        }

        .celdasBor {
        }

        /*td, th {
    padding: 0;
}*/
        .auto-style18 {
            width: 77px;
        }

        .auto-style19 {
            height: 13pt;
        }

        .verticalText {
            /* Rotate div */
            -ms-transform: rotate(-90deg) !important; /* IE 9 */
            -webkit-transform: rotate(-90deg) !important; /* Chrome, Safari, Opera */
            transform: rotate(-90deg) !important;
            width: 15px !important;
            text-align: right !important;
              position: relative !important;
              border-top: 1px solid black !important;
        }
  
   
  
   
    </style>
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css" />


</head>
<body  onload="window.print();">
    <form id="form1" runat="server">


        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 6pt; width: 100%; ">
            <tr style="height: 6.75pt; text-align: right;">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td class="auto-style20"></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>

            </tr>
            <tr style="text-align: right; border-bottom: 1px solid black;">
                <td style="font-family: Arial; font-size: 8pt;" colspan="36" class="auto-style19"><strong>SOLICITUD DE CHEQUE</strong> </td>
            </tr>
            <tr style="text-align: left; border-top: 1px solid black;">
                <td colspan="36" class="auto-style6">(D-0004)</td>
            </tr>
       
           
      
             </table>
        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 7pt; width: 100%; border-bottom: 1px solid black;">
             <tr style="border-left: none">
                <td colspan="26"> <asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>
                <td style="background-color: #C0C0C0 !important; border: 1px solid black" colspan="4">A FAVOR DE</td>
                <td></td>
                <td style="background-color: #C0C0C0 !important; border: 1px solid black" colspan="4">FECHA</td>
                <td></td>
            </tr>
            <tr style="border-left: none">
                <td style=""></td>
                <td style="">&nbsp;</td>
                <td style="">&nbsp;</td>
                <td style="" colspan="10">&nbsp;</td>
                <td style="" colspan="5">&nbsp;</td>
                <td colspan="8">&nbsp;</td>
                <td style="border: 1px solid black" colspan="4">
                    <asp:Label ID="lblAFavorPago" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td style="border: 1px solid black" colspan="4">
                    <asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label></td>
                <td></td>
            </tr>
              <tr style="height: 6.75pt; text-align: right;">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
          
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>

            </tr>
            <tr style="border-left: 1px solid black  !important; border-right: 1px solid black;">
                <td style="border-top: 1px solid black; border-left: 1px solid black !important; background-color: #C0C0C0 !important;"></td>
                <td style="border-top: 1px solid black; background-color: #C0C0C0 !important; border-right: 1px solid black; font-size: 12pt;" rowspan="10"></td>

                <td style="border-top: 1px solid black; border-right: 1px solid black;" colspan="24" class="auto-style13">&nbsp;<strong>EMPRESA </strong></td>

                <td style="border-top: 1px solid black;" colspan="9" class="auto-style13"><strong>&nbsp; NOMBRE DE PROYECTO</strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;" class="auto-style13"></td>
            </tr>

            <tr style="border-left: 1px solid black !important;">
                <td style="border-left: 1px solid black !important; background-color: #C0C0C0 !important; text-align: right; vertical-align: bottom;" class="celda_izquierda" rowspan="10">&nbsp;
                    <p class="verticalText" style="font-size: 10pt; text-align: right;"><strong>G&nbsp;E&nbsp;N&nbsp;E&nbsp;R&nbsp;A&nbsp;L&nbsp;E&nbsp;S</strong></p>
                </td>

                <td style="border-top: 1px solid black; border-right: 1px solid black;" colspan="24" class="auto-style20">&nbsp;
                    <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label>
                </td>

                <td style="border-top: 1px solid black;" colspan="9" class="auto-style20">&nbsp; <strong>
                    <asp:Label ID="lblTipoBeca" runat="server" Text=""></asp:Label></strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;" class="auto-style20"></td>

            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="33"><strong>&nbsp; A FAVOR DE  </strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>

            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="33">&nbsp;
                    <asp:Label ID="lblAFavor" runat="server" Text="Label"></asp:Label>
                </td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>

            <tr>

                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>SOLICITA</strong></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>PUESTO</strong></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>FECHA LIMITE DE PAGO</strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="11">&nbsp;
                    <asp:Label ID="lblSolicita" runat="server" Text=""></asp:Label></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;
                    <asp:Label ID="lblPuesto" runat="server" Text=""></asp:Label></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;
                    <asp:Label ID="lblFechaLimte" runat="server" Text=""></asp:Label></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>GERENCIA</strong></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>DIRECCIÓN</strong></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp;<strong>MONEDA</strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="11">&nbsp;
                    <asp:Label ID="lblGerencia" runat="server" Text=""></asp:Label></td>
                <td style="border-top: 1px solid black;" colspan="11">&nbsp; 
                    <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label></td>
                <td style="border-top: 1px solid black;" colspan="11">PESOS
                    <asp:CheckBox ID="chkPesos" runat="server" />
                    DOLARES
                    <asp:CheckBox ID="chkDolares" runat="server" />
                </td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr>

                <td style="border-top: 1px solid black;" colspan="22" class="auto-style13">&nbsp;IMPORTE       <strong>
                    <asp:Label ID="lblImporte" runat="server" Text="Label"></asp:Label></strong>              </td>

                <td style="border-top: 1px solid black;" colspan="11" class="auto-style13"><strong>TIPO DE CAMBIO </strong></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;" class="auto-style13"></td>
            </tr>
            <tr>

                <td style="border-top: 1px solid black; border-bottom: 1px solid black;" colspan="22"><strong>&nbsp;
                    <asp:Label ID="lblImporteLetra" runat="server" Text="Label"></asp:Label></strong>

                </td>

                <td style="border-top: 1px solid black; border-bottom: 1px solid black;" colspan="11">
                    <asp:Label ID="lblTipoCambio" runat="server" Text=""></asp:Label></td>
                <td style="border-right: 1px solid black; border-bottom: 1px solid black; border-top: 1px solid black;"></td>
            </tr>

        </table>
       <br />
        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 6pt; width: 100%; border-left: 1px solid black; ">
         
            <tr style="font-size: 8pt; border-left: 1px solid black;">
                <td style="border: 1px solid black; background-color: #C0C0C0 !important; text-align: center;" colspan="25" class="auto-style15"><strong>EN CASO DE SOLICITAR ANTICIPO PARA VIAJES</strong></td>
                <td style="border-right: 1px solid black; border-left: 1px solid black; background-color: white;" colspan="3"><strong></strong></td>
                <td style="border: 1px solid black; background-color: #C0C0C0 !important; text-align: center;" colspan="8" class="auto-style15"><strong>No. ANEXOS</strong> </td>

            </tr>
            <tr style="font-size: 8pt;">
                <td style="border-right: 1px solid black; border-left: 1px solid black; text-align: center;" colspan="25" class="auto-style15"></td>
                <td style="border-right: 1px solid black; border-left: 1px solid black; text-align: center;" colspan="3" class="auto-style15"></td>
                <td style="border-right: 1px solid black; border-left: 1px solid black; text-align: center;" colspan="8" class="auto-style15">
                    <asp:Label ID="lblAnexo" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr style="font-size: 6pt;">
                <td style="border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black; text-align: center;" colspan="25" class="auto-style15">FECHA DE SALIDA____________   FECHA DE REGRESO_____________  LUGAR AL QUE VIAJA______________
                </td>
                <td style="border-right: 1px solid black; border-left: 1px solid black; text-align: center;" colspan="3"></td>
                <td style="border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black; text-align: center;" colspan="8" class="auto-style15"></td>
            </tr>

            </table>
         <br />
         <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 6pt; width: 100%; border-left: 1px solid black; ">
      

            <tr style="background-color: #C0C0C0 !important; font-size: 8pt; border: 1px solid black">
                <td style="border: 1px solid black; text-align: center;" colspan="36" class="auto-style15"><strong>MOTIVO</strong></td>

            </tr>
            <tr style="font-size: 8pt; border-right: 1px solid black;  border-bottom: 1px solid black;">
                <td style="border-right: 1px solid black; border-left: 1px solid black; text-align: center;" colspan="36" rowspan="5" class="auto-style15">
                    <textarea id="txtMotivo" runat="server" readonly="readonly" rows="5" style="width: 100%; border: 0; font-family: Arial; overflow: hidden; font-size: 7pt;"></textarea>
                </td>

            </tr>

        </table>
        <br />
        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 7pt; width: 100%; border-left: 1px solid black;">
        
            <tr style="height: 1px; text-align: right;">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td class="auto-style18"></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>

            </tr>
            <tr>
                <td colspan="11" rowspan="3" style="width: 30%; text-align: justify; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px solid black; font-size: 6pt;">El solicitante reconoce estar obligado a comprobar la
                  
                    entrega del título de crédito al beneficiario, o devolver
                    éste, dentro de los 7 días siguientes a aquél en que lo recibío
                </td>
                <td rowspan="3" style="width: 10pt"></td>
                <td colspan="11" rowspan="3" style="width: 30%; background-color: #FCFBC1 !important; text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px solid black;"><strong>
                    <br />
                    VISTO BUENO DEL PROYECTO									
                </strong></td>
                <td rowspan="3" style="width: 10pt"></td>
                <td colspan="12" style="width: 30%; text-align: center; border-top: 1px solid black; border-right: 1px solid black; border-left: 1px solid black;"><strong>AUTORIZA<br />
                </strong></td>
            </tr>

            <tr>


                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;"></td>
            </tr>
            <tr>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;"></td>
            </tr>
            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;">&nbsp;</td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black"></td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px dotted black;">ROSA ISELA MACEDO GARCÏA</td>
            </tr>
            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;">SOLICITA A FAVOR DE TERCEROS</td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black"></td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black;"><strong>NOMBRE Y FIRMA</strong></td>
            </tr>
            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;" class="auto-style16"></td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black" class="auto-style16"></td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;" class="auto-style16"><strong>AUTORIZA</strong></td>
            </tr>

            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;" class="auto-style16"></td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black" class="auto-style16"></td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;" class="auto-style16"></td>
            </tr>

            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px dotted black;">
                    <asp:Label ID="lblSolicitaPago" runat="server" Text="Label"></asp:Label>
                </td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px dotted black;">ALEJANDRA YULETH BENITEZ FALCON</td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-top: 1px dotted black;"></td>
            </tr>

            <tr>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black"><strong>NOMBRE Y FIRMA</strong>  									</td>
                <td></td>
                <td colspan="11" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black;"><strong>NOMBRE Y FIRMA</strong>
                </td>
                <td></td>
                <td colspan="12" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black; border-left: 1px solid black;"><strong>NOMBRE Y FIRMA</strong>
                </td>
            </tr>
        
        </table>
         <br />
        <asp:Table ID="tbPartidas" runat="server" CssClass="tablaBor" Style="font-family: Arial; font-size: 6pt; width: 100%">
        </asp:Table>
        <div style="border-style: solid; border-width: 2px; width: 100%; color: #989898; background-color: #989898; height: 1px;" class="hidden-print "></div>
        <div class="saltopagina"></div>

        <asp:HiddenField ID="hdRol" runat="server" />
        <asp:HiddenField ID="hdUsuario" runat="server" />
    </form>
</body>
</html>
