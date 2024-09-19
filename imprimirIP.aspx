
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="imprimirIP.aspx.vb" Inherits="DNC_2017.imprimirIP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Identificación de Competencias del Puesto</title>
      <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
      <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport"/>
    <script>
        //URL

        function ChangeUrl(title, url) {
            if (typeof (history.pushState) != "undefined") {
                var obj = { Title: title, Url: url };
                history.pushState(obj, obj.Title, obj.Url);
            }
        }
    </script>
    <style>

              @media print{
   div.saltopagina{ 
      display:block; 
      page-break-before:always;
   }
   .hidden-print {
    display: none !important;
  }
}

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
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">

        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 10pt;width: 100%" >
            <tr style="height: 12.75pt; text-align: right; border-bottom: 1px solid black;">
                <td style="font-family: Arial; font-size: 8pt;" colspan="35"><strong>Identificación de Competencias del Puesto</strong> </td>
                <td></td>
            </tr>
            <tr style="height: 6.75pt; text-align: right; border-top: 1px solid black;">
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
                <td></td>
             
            </tr>
               <tr style="background-color: #C5D9F1;">
            <td style="border-top: 1px solid black; border-left: 1px solid black;" ></td>
            <td style="border-top: 1px solid black" >&nbsp;</td>
            <td style="border-top: 1px solid black" >&nbsp;</td>
            <td style="border-top: 1px solid black"  colspan="6">Fecha de Elaboración:</td>
            <td style="border-top: 1px solid black"  colspan="9">
            <asp:TextBox ID="txtFecEla" runat="server" ReadOnly="true"></asp:TextBox></td>
            <td style="border-top: 1px solid black" ></td>
            <td style="border-top: 1px solid black" ></td>
            <td style="border-top: 1px solid black" >&nbsp;</td>

            <td style="border-top: 1px solid black"  colspan="5">Fecha de actualización:</td>
            <td style="border-top: 1px solid black"  colspan="8"> <asp:TextBox ID="txtFecIng" runat="server" ReadOnly="true"></asp:TextBox></td>
            <td style="border-top: 1px solid black" >&nbsp;</td>
              <td style="border-right: 1px solid black; border-top: 1px solid black">&nbsp;</td>
        </tr>
                <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; border-bottom: 1px solid black; height: 1px;"></td>
            </tr>
            <tr style="border-left: 1px solid black; border-right: 1px solid black;">
                <td style="border-top: 1px solid black; background-color: #C5D9F1;"></td>
                <td style="border-top: 1px solid black; background-color: #C5D9F1; border-right: 1px solid black;" rowspan="17">D<br />
                    A<br />
                    T<br />
                    O<br />
                    S<br />
                    <br />
                    G<br />
                    E<br />
                    N<br />
                    E<br />
                    R<br />
                    A<br />
                    L<br />
                    E<br />
                    S<br />
                    

                </td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr style="border-left: 1px solid black; height: 15pt;"">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="2" style="text-align: left">Puesto</td>
                <td colspan="14">
                    <asp:TextBox ID="txtPuesto" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
  
                <td colspan="16">
        En caso de que el puesto no este incluido en el listado<br />
                    favor de indicarlo junto con el nivel correspondiente
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>

                  <tr style="border-left: 1px solid black; height: 15pt;"">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="3" style="text-align: left">Nivel</td>
                <td colspan="13">
                    <asp:TextBox ID="txtNivel" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                
                <td colspan="16">
                    <asp:TextBox ID="txtPuestNivel" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>

                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="5" style="text-align: left">Dirección de Área</td>
                <td colspan="11">
                    <asp:TextBox ID="txtDirArea" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td colspan="5" style="text-align: right">No. de Puestos que reportan directamente</td>
                <td colspan="11">
                    <asp:TextBox ID="txtNumeroPuesto" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="3" style="text-align: left">Dirección</td>
                <td colspan="13">
                    <asp:TextBox ID="txtDir" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                
                <td colspan="16">

                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="3" style="text-align: left">Gerencia</td>
                <td colspan="13">
                    <asp:TextBox ID="txtGerencia" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
          
                <td colspan="16">
                               <asp:TextBox ID="txtorganigrama_1" runat="server" Width="100%" ReadOnly="true" Text="Dirección General"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="3" style="text-align: left">C.C:</td>
                <td colspan="13">
                    <asp:TextBox ID="txtCentroCostos" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>

                    <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                
                <td colspan="16">
          Objetivo del Puesto
                </td>
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_3" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
              <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                
                <td colspan="16" rowspan="6">
          Objetivo del Puesto
                 <textarea id="txtObjetivoPuesto" runat="server" readonly="readonly" rows="6" style="width: 100%; font-family: Arial; overflow:hidden"></textarea>
                </td>
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_4" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
            <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td>&nbsp;</td>
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_5" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                  <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td>&nbsp;</td>
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_6" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                  <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td>&nbsp;</td>
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_7" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                  <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td>&nbsp;</td>
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_8" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                  <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td>&nbsp;</td>
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_9" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
               
                        <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
    
                <td colspan="16">
                  Puesto que le reportan directamente:
                </td>
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_10" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                                    <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                
                <td colspan="16" rowspan="2">
                           <textarea id="txtPuestoQReporta" runat="server" readonly="readonly" rows="2" style="width: 100%; font-family: Arial; overflow:hidden"></textarea>
                </td>
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_11" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                        <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
       
                <td>&nbsp;</td>
 
                <td colspan="16">
                    <asp:TextBox ID="txtorganigrama_12" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
           <tr style="background: white; ">
                <td colspan="36" style="border-top: 1px solid black;"></td>
            </tr>
 </table>
        <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 10pt;width: 100%" >
               <tr style=" text-align: right; border-top: 1px solid black;">
                <td></td>
                <td></td>
                <td style="width: 15px"></td>
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
            <tr style="border-left: 1px solid black; border-right: 1px solid black; height: 1px;">
                <td style="border-top: 1px solid black; background-color: #C5D9F1;" class="celda_izquierda"></td>
                <td style="border-top: 1px solid black; background-color: #C5D9F1; border-right: 1px solid black; font-size:6pt" rowspan="17">
                    F<br />u<br />n<br />c<br />i<br />o<br />n<br />e<br />s<br /> <br />y<br /> <br />
                    F<br />a<br />c<br />u<br />l<br />t<br />a<br />d<br />e<br />s<br /> <br />d<br />e<br /> <br />
                    A<br />u<br />t<br />o<br />r<br />i<br />z<br />a<br />c<br />i<br />ó<br />n

                </td>

                <td style="border-top: 1px solid black;" colspan="33"></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>

                <td colspan="33" style="text-align: left; background-color: #CCFF99;">Funciones :</td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>
            <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
          <td>1</td>
                <td colspan="32">
                     <textarea id="txtfunciones_1" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 15pt;"></textarea>
             
                </td>
          
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
      <td>2</td>
                <td colspan="32">
              <textarea id="txtfunciones_2" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 15pt;"></textarea>
                </td>
          
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >3 </td>
                <td colspan="32">
              <textarea id="txtfunciones_3" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 15pt;"></textarea>
                </td>
            
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >4 </td>
                <td colspan="32">
        <textarea id="txtfunciones_4" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 15pt;"></textarea>
                </td>
       
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >5 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_5" runat="server" Width="100%" ReadOnly="true"   Height="15pt"></asp:TextBox>
                </td>
      
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >6 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_6" runat="server" Width="100%" ReadOnly="true"   Height="15pt"></asp:TextBox>
                </td>
          
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >7 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_7" runat="server" Width="100%" ReadOnly="true"   Height="15pt"></asp:TextBox>
                </td>
       
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >8 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_8" runat="server" Width="100%" ReadOnly="true" Height="15pt"></asp:TextBox>
                </td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >9 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_9" runat="server" Width="100%" ReadOnly="true" Height="15pt"></asp:TextBox>
                </td>
      
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >10 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_10" runat="server" Width="100%" ReadOnly="true" Height="15pt"></asp:TextBox>
                </td>

                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >11 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_11" runat="server" Width="100%" ReadOnly="true" Height="15pt"></asp:TextBox>
                </td>
      
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black; height: 15pt;">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
                 <td >12 </td>
                <td colspan="32">
                    <asp:TextBox ID="txtfunciones_12" runat="server" Width="100%" ReadOnly="true" Height="15pt"></asp:TextBox>
                </td>
         
                <td style="border-right: 1px solid black"></td>
            </tr>
                     <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
          <td colspan="16">
                    Facultades de Autorización
                </td>
                <td colspan="16">
                    Alcance de Responsabilidad
                </td>
                <td>&nbsp;</td>
                <td style="border-right: 1px solid black"></td>
            </tr>
         
                         <tr style="border-left: 1px solid black; height: 20pt">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
          <td colspan="16" rowspan="2">
           <textarea id="txtFacultadesAutorizacion" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 20pt;"></textarea>
                </td>
                <td colspan="16"  rowspan="2">
                    <textarea id="txtAlcanceResponsabilidad" runat="server" readonly="readonly" rows="2"  style="width: 100%; font-family: Arial; overflow:hidden; height: 20pt;"></textarea>
                </td>
                <td>&nbsp;</td>
                <td style="border-right: 1px solid black"></td>
            </tr>

                                 <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>
   
                <td>&nbsp;</td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                 <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; ; height: 1px;">
                             </td>
            </tr>
        
        </table>
             <br/>
          <div style="border-style: solid; border-width: 2px; width: 100%; color: #989898; background-color: #989898; height: 1px;" class="hidden-print " ></div>
    <div class="saltopagina"></div>
         <br/>
         <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 10pt; width:100% " >
        
            <tr style="height: 6.75pt; text-align: right; ">
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
                <td></td>
             
            </tr>
             
    
         
               <tr style="border-left: 1px solid black; border-right: 1px solid black; height: 1px;">
                <td style="border-top: 1px solid black; background-color: #C5D9F1;" class="celda_izquierda"></td>
                <td style="border-top: 1px solid black; background-color: #C5D9F1; border-right: 1px solid black;" rowspan="6">R<br />e<br />l<br />a<br />c<br />i<br />o<br />n<br />e<br />s
               </td>

                <td style="border-top: 1px solid black;" colspan="33"></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>

                <td style="" colspan="33"><strong>Relaciones Interna o Externas: </strong> </td>
       
                <td class="celda_derecha"></td>
            </tr>
            <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>

                <td style="" colspan="33" rowspan="4">
                                        <textarea id="txtRelaciones" runat="server" readonly="readonly" rows="4"  style="width: 100%; font-family: Arial; overflow:hidden; height: 20pt;"></textarea>

                </td>
       
                <td class="celda_derecha"></td>
            </tr>
                 <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
                   <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
                   <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
            <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; border-bottom: 1px solid black; height: 1px;"></td>
            </tr>
               <tr style="border-left: 1px solid black; border-right: 1px solid black;">
                <td style="border-top: 1px solid black; background-color: #C5D9F1;"></td>
                <td style="border-top: 1px solid black; background-color: #C5D9F1; border-right: 1px solid black;" rowspan="24">
                    E<br />s<br />t<br />r<br />u<br />c<br />t<br />u<br />r<br />a<br /><br /> d<br />e<br /> <br />C<br />o<br />m<br />p<br />o<br />t<br />e<br />n<br />c<br />i<br />a<br />s
                </td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-top: 1px solid black"></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
             <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>

                <td colspan="33" style="text-align: left; background-color: #CCFF99;">Educación :</td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>

            <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="5" style="text-align: left">Seleccione el grado de Escolaridad</td>
                                
                <td colspan="28">
                    <asp:TextBox ID="txtgrado_escolaridad" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                        <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="5" style="text-align: left">Carrera (s) o especialización</td>
                                
                <td colspan="28">
                    <asp:TextBox ID="txtCarrera_especializacion" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
       
            <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>

                <td colspan="33" style="text-align: left; background-color: #CCFF99;">Formación :</td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="27" style="text-align: left">Conocimientos Técnicos</td>
                    <td></td>
                <td colspan="5">
               Dominio
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
             <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>
                          <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>
                          <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_3" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_3" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>
                          <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_4" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_4" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>
                          <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_5" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_5" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>
             <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="27">
                     <asp:TextBox ID="txtconocimiento_tecnico_6" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td></td>
                 <td colspan="5">
                     <asp:TextBox ID="txtconocimiento_dominio_6" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
                 <td style="border-right: 1px solid black"></td>
             </tr>

                             <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">Idioma</td>
                <td></td>
                <td colspan="5">
               Dominio
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">Idioma</td>
                    <td></td>
                <td colspan="5">
               Dominio
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                                          <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txtidioma_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txtidioma_dominio_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txtidioma_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                    <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txtidioma_dominio_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>

                    <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="32" style="text-align: left">Sistema de Gestión Integrado</td>
                    <td></td>
                <td style="border-right: 1px solid black"></td>
            </tr>
             <tr style="border-left: 1px solid black;">
                 <td style="background-color: #C5D9F1;"></td>
                 <td colspan="33">
                     <asp:TextBox ID="txtsistema_gestio_integrado" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                 </td>
         
                 <td style="border-right: 1px solid black"></td>
             </tr>
                      <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>

                <td colspan="33" style="text-align: left; background-color: #CCFF99;">Habilidad </td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>
               <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">Habilidades y Competencias</td>
                <td></td>
                <td colspan="5">
               Dominio
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">Habilidades y Competencias</td>
                    <td></td>
                <td colspan="5">
               Dominio
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                                          <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_1" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                    <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_2" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_3" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_3" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_4" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                    <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_4" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_5" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_5" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_6" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                    <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_6" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_7" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                <td></td>
                <td colspan="5">
                          <asp:TextBox ID="txthabilidades_competencias_dominio_7" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td colspan="10" style="text-align: left">           <asp:TextBox ID="txthabilidades_competencias_8" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></td>
                    <td></td>
                <td colspan="5">
                          <asp:TextBox ID="TextBox68" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
                          <tr style="border-left: 1px solid black">
                <td style="background-color: #C5D9F1;">&nbsp;</td>

                <td colspan="33" style="text-align: left; background-color: #CCFF99;">Experiencia </td>
        
                <td style="border-right: 1px solid black"></td>
            </tr>
                                   <tr style="border-left: 1px solid black;">
                <td style="background-color: #C5D9F1;"></td>
                <td colspan="5" style="text-align: left">'Area de Experiencia</td>
                                
                <td colspan="13">
                    <asp:TextBox ID="txtareas_expereiencia" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                                             <td colspan="5" style="text-align: left">&#39;Tiempo:</td>
                                
                <td colspan="10">
                    <asp:TextBox ID="txttiempo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="border-right: 1px solid black"></td>
            </tr>
               
        
             <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; border-bottom: 1px solid black; height: 1px;"></td>
            </tr>

                    <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; border-bottom: 1px solid black; height: 1px;"></td>
            </tr>
               <tr style="border-left: 1px solid black; border-right: 1px solid black; height: 1px;">
                <td style="border-top: 1px solid black; background-color: #C5D9F1;" class="celda_izquierda"></td>
                <td style="border-top: 1px solid black; background-color: #C5D9F1; border-right: 1px solid black;" rowspan="6">
               </td>

                <td style="border-top: 1px solid black;" colspan="33"></td>
                <td style="border-right: 1px solid black; border-top: 1px solid black;"></td>
            </tr>
            <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>

                <td style="" colspan="33"><strong>Observaciones </strong> </td>
       
                <td class="celda_derecha"></td>
            </tr>
            <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>

                <td style="" colspan="33" rowspan="4">
                    <textarea id="txtObservaciones" runat="server" readonly="readonly" rows="4"  style="width: 100%; font-family: Arial; overflow:hidden; height: 20pt;"></textarea>

                </td>
       
                <td class="celda_derecha"></td>
            </tr>
                 <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
                   <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
                   <tr style="height: 17px">
                <td style="background-color: #C5D9F1;" class="celda_izquierda"></td>
       
                <td class="celda_derecha"></td>
            </tr>
                     <tr style="background: white; height: 10px">
                <td colspan="36" style="border-top: 1px solid black; height: 1px;"></td>
            </tr>
             </table>
        <br/>
         <table style="border-collapse: collapse; border: 0; font-family: Arial; font-size: 10pt; width:100% " >
        
            <tr style="height: 6.75pt; text-align: right; ">
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
                <td></td>
             
            </tr>
             
          

                     <tr style="background-color: #C5D9F1;">
                                   <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black;"><strong>
                    <br />
                Preparo (RH)								
                </strong></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black;"><strong>
                    <br />
           Elaboró									
                </strong></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black;"><strong>
                    <br />
             Revisó							
                </strong></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black;"><strong><br />
             Aprobó								
                </strong></td>
            </tr>

            <tr>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;">&nbsp;</td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
            </tr>

         
            <tr>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;">&nbsp;</td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
            </tr>

            <tr>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black;">&nbsp;</td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black;"></td>
            </tr>
            <tr>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black">Nombre y Firma 							</td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black">Nombre y Firma									
                </td>
                <td colspan="9" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black">Nombre y Firma 									
                </td>
                      <td colspan="9" style="text-align: center; border-right: 1px solid black; border-bottom: 1px solid black">Nombre y Firma 									
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

