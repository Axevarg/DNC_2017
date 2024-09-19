Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class reportednc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblMnsJEr.Text = ""
        If Not Page.IsPostBack Then
            lblMensajeBuscar.Text = ""
            Call obtenerUsuarioAD()
            Call CargaCatalogos()
            txtFechaEmision.Text = PrimerDiaDelMes(Now.Date)
            txtFechaEmisionHasta.Text = UltimoDiaDelMes(Now.Date)

        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Comportamientos", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogos"
    Public Sub CargaCatalogos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Call obtEstatus(odbConexion)
            Call obtUnidadNegocio(odbConexion)
            Call obtDireccion(odbConexion)
            Call obtHabilidad(odbConexion)
            Call obtPrioridades(odbConexion)
            Call ObtDNC(odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'Obtiene el catalogo de los estatus
    Public Sub obtEstatus(odbConexion As OleDbConnection)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        strQuery = "SELECT ID,descripcion FROM DNC_ESTATUS_TB WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddlEstatus.DataSource = odbLector
        ddlEstatus.DataTextField = "descripcion"
        ddlEstatus.DataValueField = "id"

        ddlEstatus.DataBind()

    End Sub

    Public Sub obtUnidadNegocio(odbConexion As OleDbConnection)
        Dim dsCatalogo As New DataSet
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_unidades_negocio_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddlUnidadNegocio.DataSource = odbLector
        ddlUnidadNegocio.DataTextField = "UNIDAD_NEGOCIO"
        ddlUnidadNegocio.DataValueField = "UNIDAD_NEGOCIO"

        ddlUnidadNegocio.DataBind()

    End Sub

    Public Sub obtDireccion(odbConexion As OleDbConnection)
        Dim dsCatalogo As New DataSet
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_direcciones_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        ddlDireccion.DataSource = odbLector
        ddlDireccion.DataTextField = "DIRECCION"
        ddlDireccion.DataValueField = "DIRECCION"

        ddlDireccion.DataBind()
    End Sub
    'Obtiene Prioridades
    Public Sub obtPrioridades(odbConexion As OleDbConnection)
        Dim dsCatalogo As New DataSet

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_prioridades_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure
        'parametros
        odbComando.Parameters.AddWithValue("@parametrizacion", obtIdDNC())

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddlPrioridad.DataSource = odbLector
        ddlPrioridad.DataTextField = "PRIORIDADES"
        ddlPrioridad.DataValueField = "PRIORIDADES"

        ddlPrioridad.DataBind()
    End Sub
    Public Sub obtHabilidad(odbConexion As OleDbConnection)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""

        strQuery = "SELECT ID,descripcion FROM DNC_HABILIDADES_DINA_CT WHERE ESTATUS=1  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader

        ddlHabilidad.DataSource = odbLector
        ddlHabilidad.DataTextField = "descripcion"
        ddlHabilidad.DataValueField = "ID"

        ddlHabilidad.DataBind()

    End Sub

    Private Sub ObtDNC(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT * FROM DNC_PARAMETRIZACION_CT ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlDNC.DataSource = odbLector
        ddlDNC.DataValueField = "ID"
        ddlDNC.DataTextField = "nombre"

        ddlDNC.DataBind()

        ddlDNC.Items.Insert(0, New ListItem("Seleccionar", 0))
    End Sub
    Public Function obtIdDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        odbConexion.Open()
        strQuery = "SELECT * FROM DNC_PARAMETRIZACION_CT where ESTATUS='VIGENTE' ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If
        odbConexion.Close()

        Return strResultado
    End Function
#End Region
#Region "Reporte Cursos"

    'Genera la columna dependiendo del reporte a ejecutar
    Public Sub generaCol(ByVal ds As DataSet, grd As GridView)
        Dim inContador As Integer = 0
        Dim iCol As Integer = grd.Columns.Count
        Dim iItem As Integer = 0

        If ds.Tables(0).Rows.Count > 0 Then

            'ELIMANA LAS FILAS ACTUALES
            If iCol > 1 Then
                For inContador = 0 To iCol - 1
                    iItem = (iCol - 1) - inContador
                    grd.Columns.RemoveAt(iItem)

                Next
            End If
        End If
        'SE CREAN COLUMNAS EN TIEMPO DE EJECUCION
        For inContador = 0 To ds.Tables(0).Columns.Count - 1
            Dim bfield As New BoundField()
            If ds.Tables(0).Columns(inContador).ColumnName <> "ID" Then
                bfield.HeaderText = ds.Tables(0).Columns(inContador).ColumnName.ToString().ToUpper
                bfield.DataField = ds.Tables(0).Columns(inContador).ColumnName
                grd.Columns.Add(bfield)
            End If

        Next



    End Sub

    'obtiene el Cotizacion 
    Public Sub obtReporte(Optional ByVal strBuscar As String = "")
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strFiltro As String = ""
        Dim strCorreo As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_reportes_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@idDnc", ddlDNC.SelectedValue)

            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsCatalogo)
            Call generaCol(dsCatalogo, grdReporte)
            grdReporte.DataSource = dsCatalogo.Tables(0).DefaultView
            grdReporte.DataBind()

            If grdReporte.Rows.Count = 0 Then
                grdReporte.Visible = False
                lblMensajeBuscar.Text = "No hay Información para mostrar"
            Else
                grdReporte.Visible = True
                lblMensajeBuscar.Text = ""

            End If

            odbConexion.Close()

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdReporte.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdReporte.Rows(iFil).BackColor = Color.FromName("#F2F2F2")

                End If
            Next

            For i As Integer = 0 To grdReporte.Columns.Count - 1
                ' grdReporte.Columns(i).ItemStyle.Width = 450
                grdReporte.Columns(i).ItemStyle.Wrap = False
                grdReporte.Columns(i).ItemStyle.HorizontalAlign = HorizontalAlign.Left
                grdReporte.Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Private Sub grdReporte_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdReporte.RowDataBound

        'PAGINACION
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim tb As New Table
            tb = e.Row.Cells(0).Controls(0)
            For Each pageCell As TableCell In tb.Rows(0).Cells
                'valida que se acontrol ImageButton
                Dim lnk As ImageButton
                lnk = pageCell.Controls(0)
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingG').style.display = 'inline'")


            Next
        End If

    End Sub

    Protected Sub grdReporte_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdReporte.PageIndexChanging
        Try
            grdReporte.ShowFooter = True
            grdReporte.EditIndex = -1
            grdReporte.PageIndex = e.NewPageIndex
            grdReporte.DataBind()
            Call obtReporte()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub btnBuscar_ServerClick(sender As Object, e As EventArgs) Handles btnBuscar.ServerClick
        Call obtReporte()

    End Sub
#End Region

#Region "Excel"
    Public Sub obtExportarExcel(dsDatos As DataSet)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try

            Dim filename As String = "rpt_Dnc_" & ddlTipoReporte.SelectedItem.Text.Replace("ó", "o") & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".xls"
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New GridView()

            dgGrid.DataSource = dsDatos.Tables(0).DefaultView
            dgGrid.DataBind()

            dgGrid.AllowPaging = False
            dgGrid.HeaderStyle.ForeColor = Drawing.Color.White
            'Formato Encabezados
            For Each cell As TableCell In dgGrid.HeaderRow.Cells
                cell.BackColor = Color.FromName("#e63c2f")
                cell.Style.Add("font-size", "8pt")
                ' cell.Style.Add("width", "120px")
            Next

            Dim iColum As Integer = 0
            For Each row As GridViewRow In dgGrid.Rows
                row.BackColor = Color.White
                iColum = 0
                For Each cell As TableCell In row.Cells
                    cell.Style.Add("font-size", "8pt")
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = Color.FromName("#F2F2F2")
                    End If
                Next
            Next


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

    Private Sub btnExportarReporte_ServerClick(sender As Object, e As EventArgs) Handles btnExportarReporte.ServerClick
        Call exportExcel()
    End Sub
    Public Sub exportExcel(Optional ByVal strBuscar As String = "")
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim dsDatos As New DataSet
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_reportes_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            'parametros
            odbComando.Parameters.AddWithValue("@idDnc", ddlDNC.SelectedValue)

            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsDatos)

            Call obtExportarExcel(dsDatos)
            odbConexion.Close()


        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
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
#Region "Reporte DNC"
    Private Sub btnReporteDNC_ServerClick(sender As Object, e As EventArgs) Handles btnReporteDNC.ServerClick
        Call oReporteDNC()
    End Sub

    Protected Sub oReporteDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strFiltro As String = ""
        Dim strCorreo As String = ""
        Try
            Dim dsDatos As New DataSet
            dsDatos = GeneraReporteDNC()
            'VALIDACION DE INFORMACION
            If dsDatos.Tables(0).Rows.Count = 0 Then
                grdReporteDNC.Visible = False
                lblMnsJEr.Text = "No hay Información para mostrar"
            Else
                Call generaCol(dsDatos, grdReporteDNC)
                grdReporteDNC.DataSource = dsDatos.Tables(0).DefaultView
                grdReporteDNC.DataBind()
                grdReporteDNC.Visible = True
                lblMnsJEr.Text = ""
                'colorea las celdas del grid
                For iFil As Integer = 0 To grdReporteDNC.Rows.Count - 1
                    If iFil Mod 2 = 0 Then
                        grdReporteDNC.Rows(iFil).BackColor = Color.FromName("#F2F2F2")

                    End If
                Next
            End If



        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Function PrimerDiaDelMes(ByVal dtmFecha As Date) As String
        Dim strFecha As String = ""
        strFecha = dtmFecha.Year.ToString & "-" & Right("00" & dtmFecha.Month.ToString, 2) & "-" & "01"
        Return strFecha
    End Function

    'para saber el ultimo dia del mes
    Function UltimoDiaDelMes(ByVal dtmFecha As Date) As String
        Dim strFecha As String = ""
        strFecha = dtmFecha.Year.ToString & "-" & Right("00" & dtmFecha.Month.ToString, 2) & "-" & Right("00" & DateSerial(Year(dtmFecha), Month(dtmFecha) + 1, 0).Day.ToString, 2)
        Return strFecha
    End Function

    Public Function GeneraReporteDNC() As DataSet
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim dsDatos As New DataSet
        Dim strPrioridad As String = ""
        Dim strDireccion As String = ""
        Dim strHabilidad As String = ""
        Dim strUnidadNegocio As String = ""
        Dim strEstatus As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_reporte_dnc_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@FECHA_DESDE", txtFechaEmision.Text)
            odbComando.Parameters.AddWithValue("@FECHA_HASTA", txtFechaEmisionHasta.Text)

            'Prioridad
            For Each item As ListItem In ddlPrioridad.Items
                If item.Selected Then
                    strPrioridad += item.Value + ","
                End If
            Next
            If Len(strPrioridad) > 0 Then strPrioridad = strPrioridad.Substring(0, strPrioridad.LastIndexOf(","))
            'Direccion
            For Each item As ListItem In ddlDireccion.Items
                If item.Selected Then
                    strDireccion += "'" + item.Text + "',"
                End If
            Next
            If Len(strDireccion) > 0 Then strDireccion = strDireccion.Substring(0, strDireccion.LastIndexOf(","))
            'Habilidad
            For Each item As ListItem In ddlHabilidad.Items
                If item.Selected Then
                    strHabilidad += "'" + item.Text + "',"
                End If
            Next
            If Len(strHabilidad) > 0 Then strHabilidad = strHabilidad.Substring(0, strHabilidad.LastIndexOf(","))
            'Unidad de Negocio
            For Each item As ListItem In ddlUnidadNegocio.Items
                If item.Selected Then
                    strUnidadNegocio += "'" + item.Text + "',"
                End If
            Next
            If Len(strUnidadNegocio) > 0 Then strUnidadNegocio = strUnidadNegocio.Substring(0, strUnidadNegocio.LastIndexOf(","))
            'Estatus
            For Each item As ListItem In ddlEstatus.Items
                If item.Selected Then
                    strEstatus += "'" + item.Text + "',"
                End If
            Next
            If Len(strEstatus) > 0 Then strEstatus = strEstatus.Substring(0, strEstatus.LastIndexOf(","))
            strPrioridad = IIf(strPrioridad = "", "TODAS", strPrioridad)
            strDireccion = IIf(strDireccion = "", "TODAS", strDireccion)
            strHabilidad = IIf(strHabilidad = "", "TODAS", strHabilidad)
            strUnidadNegocio = IIf(strUnidadNegocio = "", "TODAS", strUnidadNegocio)
            strEstatus = IIf(strEstatus = "", "TODOS", strEstatus)

            odbComando.Parameters.AddWithValue("@PRIORIDAD", strPrioridad)
            odbComando.Parameters.AddWithValue("@DIRECCION", strDireccion)
            odbComando.Parameters.AddWithValue("@TIPO_HABILIDAD", strHabilidad)
            odbComando.Parameters.AddWithValue("@UNIDAD_NEGOCIO", strUnidadNegocio)
            odbComando.Parameters.AddWithValue("@ESTATUS", strEstatus)
            odbComando.Parameters.AddWithValue("@TIPO_CURSO", ddlTipoCurso.SelectedItem.Text)
            odbComando.Parameters.AddWithValue("@TIPO_REPORTE", ddlTipoReporte.SelectedValue)


            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsDatos)
            odbConexion.Close()


        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
        Return dsDatos
    End Function

    Private Sub btnExportRepDNC_ServerClick(sender As Object, e As EventArgs) Handles btnExportRepDNC.ServerClick
        Dim dsDatos As New DataSet
        dsDatos = GeneraReporteDNC()

        Call obtExportarExcel(dsDatos)
    End Sub

#End Region


End Class