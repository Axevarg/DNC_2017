Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class print_pago
    Inherits System.Web.UI.Page
    Private strPago As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"

        lblError.Text = ""
        strPago = Request.QueryString("idPago")
        If Not Page.IsPostBack Then
            If IsNumeric(strPago) = False Then
                Response.Redirect("index.aspx")
            End If

            Call obtenerUsuarioAD()
            Call obtPagosRegistrado(strPago)
        End If
    End Sub


#Region "Impresion Pagos"
    Public Sub obtPagosRegistrado(ByVal idPago As String)
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Dim strSuma As String = "0"
        Dim strIva As String = "0"
        Dim strTotal As String = "0"
        Dim strImporteLetra As String = ""
        Try
            odbConexion.Open()
            Dim strExtencion As String = ""
            'obtiene la informacion de las cartas configuradas al siguiente fecha de calendario
            strQuery = "SELECT * FROM [BECAS_PAGOS_VT] where id=" & idPago & "  ORDER BY NO_CORRELATIVO DESC"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                lblAFavorPago.Text = odbLector(1).ToString
                lblEmpresa.Text = odbLector(3).ToString
                lblTipoBeca.Text = odbLector(4).ToString
                lblAFavor.Text = odbLector(6).ToString
                lblSolicita.Text = odbLector(7).ToString.ToUpper
                lblSolicitaPago.Text = odbLector(7).ToString.ToUpper
                lblPuesto.Text = odbLector(8).ToString
                lblGerencia.Text = odbLector(9).ToString
                lblDireccion.Text = odbLector(10).ToString
                lblFecha.Text = odbLector(11).ToString
                lblFechaLimte.Text = odbLector(12).ToString
                lblImporteLetra.Text = odbLector(20).ToString
                strImporteLetra = odbLector(20).ToString

                If odbLector(13).ToString = "Pesos" Then
                    chkPesos.Checked = True
                    chkDolares.Checked = False
                Else
                    chkPesos.Checked = False
                    chkDolares.Checked = True
                    strImporteLetra = strImporteLetra.Substring(0, strImporteLetra.LastIndexOf("P"))
                    lblImporteLetra.Text = strImporteLetra.ToUpper.Replace("PESOS", "DOLARES") & "USD"

                End If
                lblTipoCambio.Text = odbLector(14).ToString
                txtMotivo.Value = odbLector(15).ToString
                lblAnexo.Text = odbLector(16).ToString

                strSuma = odbLector(17).ToString
                strIva = odbLector(18).ToString
                strTotal = odbLector(19).ToString
                lblImporte.Text = odbLector(19).ToString

                odbLector.Close()
            End If
            'obtiene Partidas
            Call obtPartidasCompra(strSuma, strIva, strTotal, idPago)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub obtPartidasCompra(strSuma As String, strIva As String, strTotal As String, Optional strPago As String = "")

        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Dim odbAdaptador As New OleDbDataAdapter
        Dim strQuery As String = ""
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_PAGOS_DETALLE_TB WHERE fk_id_pago=" & IIf(strPago = "", 0, strPago) & " ORDER BY partida ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsDatos)
            tbPartidas.Rows.Clear()

            Dim iRowTotales As Integer = 0
            Dim iRegistroPartidas As Integer = 0
            iRowTotales = IIf((dsDatos.Tables(0).Rows.Count - 1) <= 19, 19, dsDatos.Tables(0).Rows.Count - 1)
            iRegistroPartidas = dsDatos.Tables(0).Rows.Count - 1

            'ENCABEZADO
            Dim thrEncabezadoFil As New TableHeaderRow
            For iCol As Integer = 0 To 5
                Dim thcEncabezadosCol As New TableHeaderCell
                Dim lblEncabezado As New Label
                thcEncabezadosCol.HorizontalAlign = HorizontalAlign.Left
                thcEncabezadosCol.ID = "th_" & iCol.ToString
                thcEncabezadosCol.BackColor = Color.FromName("#C0C0C0  !important")
                thcEncabezadosCol.Attributes.CssStyle.Add("border", "1px solid black")
                Select Case iCol
                    Case 0
                        lblEncabezado.Text = "  <p class='verticalText' style='font-size: 6pt; text-align: right; border-top: none;'> <strong>PARA&nbsp;&nbsp;DESGLOSE&nbsp;&nbsp;" & _
                        "POR&nbsp;&nbsp;PARTIDA&nbsp;&nbsp;O&nbsp;&nbsp;" & _
                          "CENTRO&nbsp;&nbsp;DE&nbsp;&nbsp;" & _
                       "COSTO</strong></p>"


                        thcEncabezadosCol.Style.Add("width", "2%")
                        thcEncabezadosCol.Attributes.CssStyle.Add("font-size", "4pt")
                        thcEncabezadosCol.RowSpan = (iRowTotales + 2)
                        thcEncabezadosCol.Style.Add("text-align", "right")
                        thcEncabezadosCol.Style.Add("vertical-align", "bottom")

                    Case 1
                        lblEncabezado.Text = "PARTIDA"
                        thcEncabezadosCol.Style.Add("width", "10%")
                        thcEncabezadosCol.Style.Add("text-align", "center")
                    Case 2
                        lblEncabezado.Text = "CUENTA"
                        thcEncabezadosCol.Style.Add("width", "15%")
                        thcEncabezadosCol.Style.Add("text-align", "center")
                    Case 3
                        lblEncabezado.Text = "CONCEPTO"
                        thcEncabezadosCol.Style.Add("width", "48%")
                        thcEncabezadosCol.Style.Add("text-align", "center")
                    Case 4
                        lblEncabezado.Text = "CC"
                        thcEncabezadosCol.Style.Add("width", "15%")
                        thcEncabezadosCol.Style.Add("text-align", "center")
                    Case 5
                        lblEncabezado.Text = "IMPORTE"
                        thcEncabezadosCol.Style.Add("width", "10%")
                        thcEncabezadosCol.Style.Add("text-align", "center")

                End Select

                'inseta el encabezado
                thcEncabezadosCol.Controls.Add(lblEncabezado)
                'inserta la columna en la fila de encabezado
                thrEncabezadoFil.Cells.Add(thcEncabezadosCol)
            Next
            tbPartidas.Rows.AddAt(0, thrEncabezadoFil)



            ''CUERPO

            Dim iContador As Integer = 1
            Dim iContadorPor As Integer = 1




            For iFila As Integer = 0 To iRowTotales
                Dim trFila As New TableRow
                trFila.ID = "fila_" & iFila.ToString
                tbPartidas.Rows.Add(trFila)
                trFila.Attributes.CssStyle.Add("height", "10pt")
                For iCol As Integer = 0 To 4
                    Dim lblTexto As New Label
                    Dim tcCelda As New TableCell
                    tcCelda.HorizontalAlign = HorizontalAlign.Left
                    tcCelda.ID = "tr_" & iFila.ToString & "_" & iCol.ToString
                    tcCelda.Attributes.CssStyle.Add("border", "1px solid black")
                    'valida que la fila este en el rango de partidas registradas
                    If iFila <= iRegistroPartidas Then
                        Select Case iCol
                            Case 0 ' PARTIDA
                                lblTexto.Text = dsDatos.Tables(0).Rows(iFila)(2).ToString
                                tcCelda.Style.Add("text-align", "center")
                            Case 1 'CUENTA
                                lblTexto.Text = dsDatos.Tables(0).Rows(iFila)(3).ToString
                                tcCelda.Style.Add("text-align", "center")
                            Case 2 'CONCEPTO
                                lblTexto.Text = dsDatos.Tables(0).Rows(iFila)(4).ToString
                                tcCelda.Style.Add("text-align", "center")
                            Case 3 '"CENTRO COSTO
                                lblTexto.Text = dsDatos.Tables(0).Rows(iFila)(5).ToString
                                tcCelda.Style.Add("text-align", "center")
                            Case 4 'IMPORTE
                                lblTexto.Text = dsDatos.Tables(0).Rows(iFila)(7).ToString
                                tcCelda.Style.Add("text-align", "center")
                        End Select
                    End If


                    tcCelda.Controls.Add(lblTexto)
                    trFila.Cells.Add(tcCelda)
                Next
            Next

            ''inserta IVA y 

            For iFila As Integer = 0 To 2
                Dim trFila As New TableRow
                trFila.ID = "fila_" & iFila.ToString
                tbPartidas.Rows.Add(trFila)
                trFila.Attributes.CssStyle.Add("height", "9pt")
                For iCol As Integer = 0 To 5
                    Dim lblTexto As New Label
                    Dim tcCelda As New TableCell
                    tcCelda.HorizontalAlign = HorizontalAlign.Left
                    tcCelda.ID = "tr_" & iFila.ToString & "_" & iCol.ToString

                    'valida que la fila este en el rango de partidas registradas
                    tcCelda.Attributes.CssStyle.Add("text-align", "right")
                    Select Case iCol
                        Case 4 '"CONCEPTO TOTALES
                            lblTexto.Text = IIf(iFila = 0, "SUMAS", IIf(iFila = 1, "IVA", "TOTAL"))
                            tcCelda.Attributes.CssStyle.Add("border", "1px solid black")

                        Case 5 'IMPORTE
                            lblTexto.Text = "$ " & IIf(iFila = 0, strSuma, IIf(iFila = 1, strIva, strTotal))
                            tcCelda.Attributes.CssStyle.Add("border", "1px solid black")
                    End Select

                    tcCelda.Controls.Add(lblTexto)
                    trFila.Cells.Add(tcCelda)
                Next
            Next


            ''colorea las celdas del grid
            'For iFil As Integer = 0 To tbCorerectivo.Rows.Count - 1
            '    If iFil Mod 2 = 0 Then
            '        tbCorerectivo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            '    End If
            'Next
            '        tbCorerectivo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            odbConexion.Close()


        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Seguridad"
    Public Sub obtenerUsuarioAD()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            Dim strNombreUsuario As String
            Dim objDirectoryEntry As New DirectoryEntry("LDAP://" & System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain.ToString) ' "LDAP://" & me.text1.text) 
            Dim objDirectorySearcher As New DirectorySearcher(objDirectoryEntry)
            Dim mySearcher As New System.DirectoryServices.DirectorySearcher(objDirectoryEntry)
            Dim result As System.DirectoryServices.SearchResult
            Dim lstArreglo As New ArrayList
            Dim strNombreCompletoAD As String
            Dim strEmail As String = ""
            Dim strClaveEmpleado As String = ""

            odbConexion.Open()
            strNombreUsuario = IIf(User.Identity.Name = "", System.Environment.UserName, User.Identity.Name)

            If strNombreUsuario.Contains("\") Then
                Dim intfin As Integer
                intfin = (strNombreUsuario.Length - strNombreUsuario.LastIndexOf("\")) - 1
                strNombreUsuario = strNombreUsuario.Substring(strNombreUsuario.LastIndexOf("\") + 1, intfin)
            End If

            mySearcher.Filter = "(sAMAccountName=" + strNombreUsuario + ")"
            'asigna el userid
            lstArreglo.Add(strNombreUsuario)

            mySearcher.PropertiesToLoad.Add("mail")
            mySearcher.PropertiesToLoad.Add("givenname")
            mySearcher.PropertiesToLoad.Add("sn")
            mySearcher.PropertiesToLoad.Add("employeeID")

            result = mySearcher.FindOne()
            ''Validacion si existe el usuario
            If result Is Nothing Then
                Response.Redirect("sinacceso.html")
            Else
                Dim directoryEntry As New DirectoryEntry
                directoryEntry = result.GetDirectoryEntry()
                'Asigna el nombre de usuario
                'Asigna el nombre de usuario
                'Asigna el nombre de usuario
                strNombreCompletoAD = directoryEntry.Name.Replace("CN=", "").ToString.Trim
                '21-04-2016 se agrega nueva forma de obtener los nombre de los usuarios para poder presentar la info del AD
                strNombreCompletoAD = result.Properties("givenname").Item(0).ToString.Trim & " " & result.Properties("sn").Item(0).ToString.Trim
                '     MsgBox(result.Properties("employeeID").Item(0).ToString.Trim)
                If result.Properties("mail").Count > 0 Then strEmail = result.Properties("mail").Item(0).ToString
                If result.Properties("employeeID").Count > 0 Then strClaveEmpleado = result.Properties("employeeID").Item(0).ToString

                hdUsuario.Value = strNombreUsuario

                'Tratamiento de Remplazo de acentos.
                strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Á", "A")
                strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("É", "E")
                strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Í", "I")
                strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Ó", "O")
                strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Ú", "U")

                'valida si el numero de nomina existe en la tabla de login
                Call validaAccesoAplicacion(strNombreCompletoAD, strNombreUsuario, strEmail, strClaveEmpleado, odbConexion)

            End If
            odbConexion.Close()

        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

    Public Sub validaAccesoAplicacion(ByVal NombreUsuario As String, ByVal usuario As String, _
                                            ByVal email As String, ByVal strClaveEmpleado As String, ByVal odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim odbComando As OleDbCommand
        Dim strNomina As String = ""


        'si existe actualiza la fecha de acceso
        If existeUsuario(usuario, odbConexion) Then
            strNomina = IIf(strClaveEmpleado = "", obtNumeroNomina(hdUsuario.Value, odbConexion), strClaveEmpleado)
            'actualiza la fecha de acceso
            strQuery = "UPDATE SIGIDO_USUARIOS_TB " & _
                  " SET  ultimo_acceso=(GETDATE())" & _
                  " WHERE usuario='" & usuario & "'"

            odbComando = New OleDbCommand(strQuery, odbConexion)
        Else 'como no existe inserta sus datos al sistema

            strNomina = IIf(strClaveEmpleado = "", obtNumeroNomina(hdUsuario.Value, odbConexion), strClaveEmpleado)
            strQuery = "INSERT INTO SIGIDO_USUARIOS_TB(  [clave]" & _
           ",[nombre] ,[email] ,[usuario] ,[primer_Aceeso] ,[rol]) VALUES(" & _
           IIf(IsNumeric(strNomina), strNomina, 0) & ",'" & NombreUsuario & "','" & email & "','" & usuario & "', GETDATE(),'3')"
            odbComando = New OleDbCommand(strQuery, odbConexion)
        End If

        odbComando.ExecuteNonQuery()

        Call obtenerUsuario(strNomina)
        hdRol.Value = obtRol(usuario, odbConexion)

    End Sub
    'obtiene el Nombre del Perfil
    Public Function obtNombrePerfil(Perfil As String, odbConexion As OleDbConnection)
        Dim strResultado As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT [descripcion] FROM SIGIDO_PERFILES_CT WHERE id='" & Perfil & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return strResultado
    End Function

    'VALIDA SI EL USUARIO NO TIENE ACCESO
    Public Function validaPerfilSinAcceso(Perfil As String, odbConexion As OleDbConnection)
        Dim blnResultado As Boolean = False
        Dim strQuery As String = ""

        strQuery = "SELECT ISNULL(sin_acceso,0) FROM SIGIDO_PERFILES_CT WHERE id='" & Perfil & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0).ToString = "1", True, False)
            odbLector.Close()
        End If

        Return blnResultado
    End Function
    'valida si existe el ususario en la tabla de empleados
    Public Function existeUsuario(usuario As String, odbConexion As OleDbConnection)
        Dim blnResultado As Boolean = False
        Dim strQuery As String = ""

        strQuery = "SELECT COUNT(*) FROM SIGIDO_USUARIOS_TB WHERE usuario='" & usuario & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()
        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0) = 0, False, True)
            odbLector.Close()
        End If

        Return blnResultado
    End Function

    'valida si el numero de nomina existe en la tabla de login
    Public Function obtNumeroNominaGiro(ByVal NombreUsuario As String, ByVal odbConexion As OleDbConnection) As String
        Dim strQuery As String = ""
        Dim strResultado As String = ""


        strQuery = "  SELECT [CLAVE] " & _
        "FROM [SGIDO_INFOGIRO_GIRO_VT]" & _
        "WHERE (NOMBRE + ' ' + APPAT + ' ' + APMAT  LIKE  '%" & UCase(NombreUsuario) & "%' )"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader()
        'valida si el nombre del usuario existe en la vista de giro
        If odbLector.HasRows() Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return strResultado
    End Function

    'obtiene el rol del Empleado
    Public Function obtRol(usuario As String, odbConexion As OleDbConnection)
        Dim strResultado As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT [Rol] FROM SIGIDO_USUARIOS_TB WHERE usuario='" & usuario & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return strResultado
    End Function
    'obtiene los Empleado Tabla de Empleados
    Public Function obtNumeroNomina(usuario As String, odbConexion As OleDbConnection)
        Dim strResultado As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT [clave] FROM SIGIDO_USUARIOS_TB WHERE usuario='" & usuario & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return strResultado
    End Function

    'Valida si Pagina esta Habilitada
    Public Function validaPaginaHabilitada(pagina As String, odbConexion As OleDbConnection)
        Dim blnResultado As Boolean = False
        Dim strQuery As String = ""

        strQuery = "SELECT ISNULL(estatus,0) FROM SIGIDO_PERMISOS_PERFIL_TB " & _
                   " WHERE fk_id_perfil='" & hdRol.Value & "' and fk_id_modulo IN (select id from SIGIDO_MODULOS_TB where estatus=1 AND paginaAspx='" & pagina & "' )"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0).ToString = "1", True, False)
            odbLector.Close()
        End If

        Return blnResultado
    End Function
    Public Sub obtenerUsuario(ByVal strNomina As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""


        Try
            odbConexion.Open()
            strQuery = "SELECT  [NOMBRE] +' ' +[APPAT] + ' '+ [APMAT] AS NOMBRE_COMPLETO" & _
                        "       ,[DEPARTAMENTO]" & _
                        "       ,[PUESTO]" & _
                        " FROM [SGIDO_INFOGIRO_GIRO_VT] WHERE CLAVE=" & IIf(strNomina = "", 0, strNomina)
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()


                odbLector.Close()
            Else

            End If

            odbConexion.Close()
            ' hdIdUsuario.Value = strNombreUsuario
        Catch ex As Exception
            lblError.ForeColor = Color.Red
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region
End Class