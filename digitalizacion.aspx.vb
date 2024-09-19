Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO

Public Class digitalizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorArchivo.Text = ""
        If Not Page.IsPostBack Then

            Call obtenerUsuarioAD()
            Call CargaColaboradores()
            Call CargaCatalogosTipo()
            Call ObtDatosColaboradores()
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogo"
    Public Sub CargaCatalogosTipo()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Dim strQuery As String = "SELECT *  FROM BECAS_DIGITALIZACION_DOCUMENTOS_CT WHERE ESTATUS=1 ORDER BY 2"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlTipoDocumento.DataSource = odbLector
            ddlTipoDocumento.DataValueField = "ID"
            ddlTipoDocumento.DataTextField = "descripcion"

            ddlTipoDocumento.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

#End Region
#Region "Colaboradores"


    'obtiene en el numero de Nomina del Jefe
    Public Function ObtNoNomina() As String
        Dim strResultado As String = ""
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        odbConexion.Open()
        strQuery = "SELECT * FROM SIGIDO_USUARIOS_TB where usuario='" & hdUsuario.Value & "' ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(1).ToString

        End If
        odbConexion.Close()
        Return strResultado
    End Function

    Public Sub CargaColaboradores()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            hdNoNominaUsuario.Value = ObtNoNomina()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_colaboradores_reportan_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametro
            odbComando.Parameters.AddWithValue("@parametrizacion", 1)
            odbComando.Parameters.AddWithValue("@jefe", hdNoNominaUsuario.Value)
            odbComando.Parameters.AddWithValue("@perfil", hdRol.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlColaborador.DataSource = odbLector
            ddlColaborador.DataValueField = "CLAVE"
            ddlColaborador.DataTextField = "NOMBRE"

            ddlColaborador.DataBind()
            odbConexion.Close()
            'valida si tiene asignado empleado
            If ddlColaborador.Items.Count = 0 Then
                divRegistro.Visible = False
                divDatos.Visible = False
            End If

        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

    Private Sub ObtDatosColaboradores()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Dim strQuery As String = ""
            strQuery = " SELECT [CLAVE]                               " & _
            "    ,[NOMBRE] + SPACE(1)+[APPAT]+ SPACE(1) +[APMAT] AS NOMBRE_COMPLETO   " & _
            "    ,[CURP]                                                              " & _
            "    ,[DIRAREA]                                                           " & _
            "    ,[DIR]                                                               " & _
            "    ,[GERENCIA]                                                          " & _
            "    ,[DEPARTAMENTO]                                                      " & _
            "    ,[DEPARTAMENTO2]                                                     " & _
            "    ,[PUESTO]                                                            " & _
            "    ,REPLACE([UBICACION],'DINA ','') AS UBICACION  " & _
            "    ,[UNIDAD_DE_NEGOCIO]" & _
            "    ,[ESTATUS] " & _
            "FROM [SGIDO_INFOGIRO_GIRO_VT] " & _
            " WHERE [CLAVE]=" & IIf(ddlColaborador.SelectedValue = "", 0, ddlColaborador.SelectedValue)
            ' If hdRol.Value = "U" Then strQuery += " and (ESTATUS='ACTIVO' AND [CLAVEJEFE]=" & hdNoNominaUsuario.Value & ")"


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            lblPuestoCol.Text = ""
            lblDireccion.Text = ""
            lblNombreCol.Text = ""
            lblArea.Text = ""
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                lblNombreCol.Text = StrConv(odbLector(1).ToString, VbStrConv.ProperCase) & " - " & odbLector(0).ToString
                lblPuestoCol.Text = "<br /> <strong>Puesto: </strong>" & StrConv(odbLector(8).ToString, VbStrConv.ProperCase)
                lblArea.Text = IIf(odbLector(5).ToString.Trim = "" Or odbLector(5).ToString.Trim = "N/A", "", "<br /> <strong>Área: </strong>" & StrConv(odbLector(5).ToString.Trim, VbStrConv.ProperCase))
                lblDireccion.Text = "<br /> <strong>Dirección: </strong>" & IIf(odbLector(4).ToString = "", StrConv(odbLector(3).ToString, VbStrConv.ProperCase), StrConv(odbLector(4).ToString, VbStrConv.ProperCase))


                'obtiene cursos
                Call obtArchivoDigitalizados()

                ' If hdRol.Value = "A" Then Call obtCursos(ddlColaborador.SelectedValue)
                odbLector.Close()
                divRegistro.Visible = True
            Else 'si no tiene colaboradores asignados

                divRegistro.Visible = False
            End If


            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub


    Private Sub ddlColaborador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlColaborador.SelectedIndexChanged

        Call ObtDatosColaboradores()
    End Sub
#End Region
#Region "Documentos Cotizaciones"

    Private Sub ddlTipoDocumento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoDocumento.SelectedIndexChanged
        Call obtArchivoDigitalizados()
    End Sub

    Private Sub btnCargaArchivos_ServerClick(sender As Object, e As EventArgs) Handles btnCargaArchivos.ServerClick
        Call CargaArchivos()

    End Sub
    'carga imagenes de timeLine
    Public Sub CargaArchivos()
        Try
            If IsPostBack Then

                Dim path As String = Server.MapPath("~/UploadedFiles/")
                Dim fileOK As Boolean = False
                If fucArchivo.HasFile Then

                    If fucArchivo.FileBytes.Length > 1073741824 Then
                        lblErrorArchivo.Text = "* El tamaño de archivo no debe sobrepasar 1GB."
                        Exit Sub
                    End If


                    Dim fileExtension As String
                    fileExtension = System.IO.Path. _
                        GetExtension(fucArchivo.FileName).ToLower()


                    Dim allowedExtensions As String() = _
                         {".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx"}
                    For i As Integer = 0 To allowedExtensions.Length - 1
                        If fileExtension = allowedExtensions(i) Then
                            fileOK = True
                        End If
                    Next
                    If fileOK Then
                        Try
                            Dim strDirectorio As String = ""
                            Dim strTipoDocumento As String = ""
                            strTipoDocumento = ddlTipoDocumento.SelectedItem.Text.Replace(" ", "")
                            strDirectorio = "\Digitalizacion_Documentos\" & ddlColaborador.SelectedValue & "\" & strTipoDocumento & "\"

                            'crea directorio
                            If Not (Directory.Exists(path & strDirectorio)) Then
                                Directory.CreateDirectory(path & strDirectorio)
                            End If
                            'nombre de Fotografia
                            Dim strNombre As String = strDirectorio & Now.Year.ToString & Now.Month.ToString & _
                                Now.Day.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & fucArchivo.FileName
                            Dim strRuta As String = path & strNombre
                            fucArchivo.PostedFile.SaveAs(strRuta)

                            Call insArchivoDigitalizado("UploadedFiles" & strNombre, fileExtension.Trim, fucArchivo.FileName)
                            ' lblErrorPerfil.Text = "File uploaded!"
                        Catch ex As Exception
                            lblErrorArchivo.Text = "El archivo no se puede subir."
                        End Try
                    Else
                        lblErrorArchivo.Text = "No se puede aceptar archivos de este tipo."
                    End If
                End If
            End If
        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'inseta Archivo Menu
    Public Sub insArchivoDigitalizado(ByVal strRuta As String, strExtencion As String, strNombre As String)
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try

            odbConexion.Open()

            strQuery = "INSERT INTO [dbo].[BECAS_DIGITALIZACIONES_DOCUMENTOS_TB]" & _
                       "  VALUES " & _
                       "  (" & ddlColaborador.SelectedValue & "" & _
                       "  ,'" & ddlTipoDocumento.SelectedValue & "' " & _
                       "  ,'" & strRuta & "' " & _
                       "  ,'" & strNombre & "' " & _
                       "  ,'" & strExtencion & "' " & _
                       "  , 1" & _
                       "  ,GETDATE() " & _
                       "  ,'" & hdUsuario.Value & "' " & _
                       "  ,NULL " & _
                       "  ,NULL ) "
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtArchivoDigitalizados()

        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    'Obtiene la infromacion de los Archivos de Reporte
    Public Sub obtArchivoDigitalizados()
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Try

            odbConexion.Open()
            Dim strExtencion As String = ""

            strQuery = "SELECT * " & _
               " FROM [BECAS_DIGITALIZACIONES_DOCUMENTOS_TB] WHERE [fk_id_colaborador]=" & ddlColaborador.SelectedValue & _
                " and fk_id_digitalizacion_documentos=" & ddlTipoDocumento.SelectedValue & " AND ESTATUS=1 ORDER BY 1  DESC "
            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdArchivos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdArchivos.DataBind()
            If grdArchivos.Rows.Count = 0 Then
                lblRegistro.Text = "No tiene Documentos Cargados para este tipo de Documento"
                grdArchivos.Visible = False
            Else
                lblRegistro.Text = ""
                grdArchivos.Visible = True
            End If
            For iFil As Integer = 0 To grdArchivos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdArchivos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            odbConexion.Close()
        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Private Sub grdArchivos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdArchivos.RowDataBound
        For i As Integer = 0 To grdArchivos.Rows.Count - 1
            Dim btnVer As LinkButton = grdArchivos.Rows(i).Controls(1).Controls(0)
            Dim strRuta As String = ""
            strRuta = DirectCast(grdArchivos.Rows(i).FindControl("lblRuta"), Label).Text
            btnVer.Attributes.Add("href", strRuta)
            btnVer.Attributes.Add("target", "_blank")
            btnVer.ToolTip = "Ver Documento"
            Dim btnEliminar As LinkButton = grdArchivos.Rows(i).Controls(3).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el archivo " + grdArchivos.Rows(i).Cells(2).Text + "?')){ return false; };"
        Next
    End Sub
    Private Sub grdArchivos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdArchivos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strRuta As String = ""
        Try
            strId = DirectCast(grdArchivos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "UPDATE BECAS_DIGITALIZACIONES_DOCUMENTOS_TB SET ESTATUS=0 WHERE ID=" & strId
            'valida en Facilitadores

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            Dim path As String = Server.MapPath("~/UploadedFiles/")
            'calcula correlativo
            strRuta = Server.MapPath("~/UploadedFiles/") & obtRutaArchivo(strId, odbConexion)
            'crea directorio
            If (File.Exists(strRuta)) Then
                File.Delete(strRuta)
            End If

            odbConexion.Close()

            Call obtArchivoDigitalizados()
        Catch ex As Exception
            lblErrorArchivo.Text = ex.Message
        End Try
    End Sub

    Public Function obtRutaArchivo(strId As String, odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim strResultado As String = ""
        Try

            strQuery = "SELECT [ruta] " & _
               " FROM [BECAS_DIGITALIZACIONES_DOCUMENTOS_TB] WHERE id=" & strId
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                strResultado = odbLector(0).ToString.Replace("UploadedFiles\", "")
                odbLector.Close()
            End If

        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
        Return strResultado
    End Function

#End Region
#Region "Excel"
    Public Sub obtExportarExcel(dsDatos As DataSet)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try

            Dim filename As String = "rpt_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second) & ".xls"
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()

            dgGrid.DataSource = dsDatos.Tables(0).DefaultView
            dgGrid.DataBind()


            'Get the HTML for the control.
            dgGrid.RenderControl(hw)
            'Write the HTML back to the browser.
            'Response.ContentType = application/vnd.ms-excel;
            Response.ContentType = "application/vnd.ms-excel"
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename & "")
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble())
            Me.EnableViewState = False
            Response.Write(tw.ToString())
            Response.[End]()

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
                lblNombre.Text = strNombreCompletoAD

                hdClaveEmpleadoAD.Value = strClaveEmpleado
                hdUsuario.Value = strNombreUsuario
                lblNombre.Text = strNombreCompletoAD
                lblNombre2.Text = strNombreCompletoAD
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
        'Valida si el perfil no tiene permmisos
        If validaPerfilSinAcceso(hdRol.Value, odbConexion) Then Response.Redirect("sinacceso.html")

        lblPerfil.Text = "Pefil: " & obtNombrePerfil(hdRol.Value, odbConexion)
        'carga menu de Pagina
        Call CargaMenuPagina(hdRol.Value, odbConexion)

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
                lblDepartamento.Text = StrConv(odbLector(1).ToString, VbStrConv.ProperCase)
                lblPuesto.Text = StrConv(odbLector(2).ToString, VbStrConv.ProperCase)


                odbLector.Close()
            Else
                lblNombre.Text = ""
                lblNombre2.Text = ""
                lblPuesto.Text = ""
                lblDepartamento.Text = ""
            End If

            odbConexion.Close()
            ' hdIdUsuario.Value = strNombreUsuario
        Catch ex As Exception
            lblError.ForeColor = Color.Red
            lblError.Text = ex.Message
        End Try
    End Sub
    'Obtene la informacion del los modulos del Sistema
    'Parametro 1:  strRolPerfil As String     (Rol de Pefil de Usuario que ingresa al Sistema)

    Public Sub CargaMenuPagina(strRolPerfil As String, odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim dsDatos As New DataSet
        Dim strPaginaActual As String = ""
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "muestra_menu_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@idPerfil", strRolPerfil)

        Dim odbAdaptador As New OleDbDataAdapter
        odbAdaptador.SelectCommand = odbComando
        odbAdaptador.Fill(dsDatos)
        'Genera el Menu Dinamico
        ulMenu.Controls.Clear()

        'valida si la pagina esta habilitada para  los permisos
        strPaginaActual = HttpContext.Current.Request.Url.AbsolutePath
        If strPaginaActual.Contains("/") Then strPaginaActual = strPaginaActual.Substring(strPaginaActual.LastIndexOf("/") + 1)
        If validaPaginaHabilitada(strPaginaActual, odbConexion) = False Then Response.Redirect("index.aspx")

        '  lblDetalle.Text = strPaginaActual & " " & validaPaginaHabilitada(strPaginaActual, odbConexion).ToString & " " & HttpContext.Current.Request.Url.LocalPath.Replace("/", "")
        'Genera Menu de Pagina
        Call GeneraMenu(dsDatos, odbConexion)


    End Sub

    'Genera el mmenu de manera dinamico
    Public Sub GeneraMenu(dsDatos As DataSet, odbConexion As OleDbConnection)
        Dim strNombrePagina As String = ""
        Dim strUrlPagina As String = ""
        Dim strModulos As String = ""
        Dim strPaginaActual As String = ""
        Dim strSpan As String = ""
        Dim strDropDown As String = ""
        Dim menuBar As New Menu
        'obtiene la pagina actual
        strPaginaActual = HttpContext.Current.Request.Url.AbsolutePath
        If strPaginaActual.Contains("/") Then strPaginaActual = strPaginaActual.Substring(strPaginaActual.LastIndexOf("/") + 1)
        'obtiene la coleccion de Hijos del Menu Inicia en Modulo 0
        Dim dataViewPadre As New DataView(dsDatos.Tables(0))
        dataViewPadre.RowFilter = dsDatos.Tables(0).Columns(1).ColumnName & "= 0"

        For Each row As DataRowView In dataViewPadre
            Dim liPadre As New HtmlGenericControl("li")
            Dim ltcMenu As New LiteralControl("")
            Dim span As New HtmlGenericControl("span")
            'agrega menu de Item
            Dim menuItem As New MenuItem(row("NOMBRE").ToString(), row("modulo").ToString())
            menuItem.NavigateUrl = row("PAGINAASPX").ToString()
            menuBar.Items.Add(menuItem)

            'obtiene el nombre de las paginas
            strNombrePagina = row("NOMBRE").ToString().Trim()
            strUrlPagina = row("PAGINAASPX").ToString().Trim()
            strModulos = row("modulo").ToString()
            liPadre.ID = "liPadre_" & strModulos

            'valida si es pagina actual
            If strPaginaActual = strUrlPagina Then
                liPadre.Attributes.Add("class", "active")
                strSpan = "<span class='sr-only'>(current)</span>"
            End If

            liPadre.Visible = IIf(row("estatus").ToString() = "1", True, False)
            'Obtiene Ul para Hijos del Menu
            Dim ulHijos As New HtmlGenericControl("ul")
            'valida si tiene Hijos
            If validaHijos(strModulos, odbConexion) Then
                liPadre.Attributes.Add("class", "dropdown")
                strDropDown = " class='dropdown-toggle' data-toggle='dropdown' "
                strSpan = " <span class='caret'></span> "
                'U
                ulHijos.Attributes.Add("class", "dropdown-menu")
                ulHijos.Attributes.Add("role", "menu")
                liPadre.Controls.Add(ulHijos)
            End If

            'agrega Hijos de los menus
            AgrregarItemSubMenu(dsDatos.Tables(0), menuItem, ulHijos, strPaginaActual, odbConexion)

            ltcMenu.Text = "<a href='" & strUrlPagina & "' " & strDropDown & ">" & strNombrePagina & " " & strSpan & "</a>"
            liPadre.Controls.Add(ltcMenu)
            ulMenu.Controls.Add(liPadre)
        Next
    End Sub
    'agrega los sub menus
    Private Sub AgrregarItemSubMenu(table As DataTable, menuItem As MenuItem, ulHijos As HtmlGenericControl, strPaginaActual As String, odbConexion As OleDbConnection)
        Dim strSpan As String = ""
        Dim strDropDown As String = ""
        Dim strNombrePagina As String = ""
        Dim strUrlPagina As String = ""
        Dim strModulos As String = ""

        Dim viewItem As New DataView(table)
        viewItem.RowFilter = table.Columns(1).ColumnName + "=" + menuItem.Value
        For Each rowHijo As DataRowView In viewItem
            Dim liHijo As New HtmlGenericControl("li")
            Dim ltcMenuHijo As New LiteralControl("")
            Dim childItem As New MenuItem(rowHijo("NOMBRE").ToString(), rowHijo("modulo").ToString())

            childItem.NavigateUrl = rowHijo("PAGINAASPX").ToString()
            'Nombre de la Pagina
            strNombrePagina = rowHijo("NOMBRE").ToString().Trim()
            strUrlPagina = rowHijo("PAGINAASPX").ToString().Trim()
            strModulos = rowHijo("modulo").ToString()
            liHijo.ID = "liHijo_" & strModulos
            'valida si es pagina actual
            If strPaginaActual = strUrlPagina Then liHijo.Attributes.Add("class", "active")
            liHijo.Visible = IIf(rowHijo("estatus").ToString() = "1", True, False)
            'agrega el Item del Menu
            menuItem.ChildItems.Add(childItem)
            'Etiqueta de los hijos de los hijos
            Dim ulHijosHijos As New HtmlGenericControl("ul")

            'valida si tiene Hijos
            If validaHijos(strModulos, odbConexion) Then
                liHijo.Attributes.Add("class", "dropdown-submenu")
                strDropDown = " class='dropdown-toggle' data-toggle='dropdown' "
                'U
                ulHijosHijos.Attributes.Add("class", "dropdown-menu")
                ulHijosHijos.Attributes.Add("role", "menu")
                liHijo.Controls.Add(ulHijosHijos)
            End If

            ltcMenuHijo.Text = "<a href='" & strUrlPagina & "' " & strDropDown & ">" & strNombrePagina & "</a>"
            liHijo.Controls.Add(ltcMenuHijo)

            'agregar Item del Sub Menu 
            AgrregarItemSubMenu(table, childItem, ulHijosHijos, strPaginaActual, odbConexion)

            ulHijos.Controls.Add(liHijo)
        Next
    End Sub
    'Valida su la APlicacion Tiene Hijos
    Public Function validaHijos(strModulo As String, odbConexion As OleDbConnection)
        Dim blnResultado As Boolean = False
        Dim strQuery As String = ""

        strQuery = "SELECT COUNT(*) FROM SIGIDO_MODULOS_TB WHERE IDPADRE='" & strModulo & "'"
        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return blnResultado
    End Function

#End Region
End Class