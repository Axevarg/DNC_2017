Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class gestion_grupos_preparatoria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorAsistencia.Text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call CargaCatalogos()
            Call obtColaboradoresPorGrupo()
            hdEdicionAsistencia.Value = 0
        End If
        Call comportamientos()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
    End Sub

#Region "Catalogos"
    Public Sub CargaCatalogos()
        Call obtGrupos()
        Call obtCatalogoColaborador()
        Call obtGruposAsistencia()
    End Sub
    'Obtiene el catalogo de Años
    Public Sub obtGrupos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año


            strQuery = " SELECT * FROM BECAS_GRUPOS_PREPARATORIA_CT WHERE ESTATUS=1 order by descripcion"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlGrupos.DataSource = odbLector
            ddlGrupos.DataTextField = "descripcion"
            ddlGrupos.DataValueField = "id"

            ddlGrupos.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub


    'Obtiene el catalogo de los Colaboradores Configurados como beca de prepa y Modalidad Grupo
    Public Sub obtCatalogoColaborador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()


            strQuery = "SELECT 0 as CLAVE,' Seleccionar'  AS NOMBRE UNION ALL" & _
                       " SELECT CLAVE,(NOMBRE + SPACE(1) + APPAT + SPACE(1) +  APMAT)  AS NOMBRE  FROM SGIDO_INFOGIRO_GIRO_VT " & _
                       " WHERE CLAVE IN (SELECT fk_id_colaborador FROM BECAS_GESTION_BECAS_TB WHERE fk_id_tipo_beca=3 AND fk_id_modalidad_estudio=1) " & _
                       " AND CLAVE NOT IN (SELECT fk_id_colaborador FROM [BECAS_GRUPOS_PREPARATORIA_TB])  order by 2"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlColaboradores.DataSource = odbLector
            ddlColaboradores.DataTextField = "NOMBRE"
            ddlColaboradores.DataValueField = "CLAVE"

            ddlColaboradores.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    'Obtiene el catalogo de Años
    Public Sub obtGruposAsistencia()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año


            strQuery = " SELECT * FROM BECAS_GRUPOS_PREPARATORIA_CT WHERE ESTATUS=1 order by descripcion"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlGrupoAsistencia.DataSource = odbLector
            ddlGrupoAsistencia.DataTextField = "descripcion"
            ddlGrupoAsistencia.DataValueField = "id"

            ddlGrupoAsistencia.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
#End Region

#Region "Configuracion Grupos"
    ''Obtiene la informacion de las Calificaciones de Ingles
    Public Sub obtColaboradoresPorGrupo()
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Try

            odbConexion.Open()

            'Obtiene el catalogo de los Colaboradores Configurados como beca de prepa y Modalidad Grupo
            strQuery = "SELECT [CLAVE] " & _
                       " ,(NOMBRE + SPACE(1) + APPAT + SPACE(1) + APMAT)  AS NOMBRE" & _
                       " ,REPLACE([DIRAREA], 'DIRECCION DE ', '') AS DIRAREA" & _
                       " ,REPLACE([DIR], 'DIRECCION DE ', '') AS DIR" & _
                       " ,[PUESTO]" & _
                       "  FROM [SGIDO_INFOGIRO_GIRO_VT] A INNER JOIN BECAS_GRUPOS_PREPARATORIA_TB B ON A.CLAVE=B.fk_id_colaborador " & _
                       "  WHERE fk_id_grupo_preparatoria=" & ddlGrupos.SelectedValue

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdColaboradoresGrupo.DataSource = dsCatalogo.Tables(0).DefaultView
            grdColaboradoresGrupo.DataBind()
            If grdColaboradoresGrupo.Rows.Count = 0 Then
                divColaboradoresGrupo.Visible = False
                lblRegistrosCandidatos.Text = "No hay Colaboradores Registrados para ese Grupo."
            Else
                divColaboradoresGrupo.Visible = True
                lblRegistrosCandidatos.Text = ""
            End If


            For iFil As Integer = 0 To grdColaboradoresGrupo.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdColaboradoresGrupo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub


    Private Sub ddlGrupos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGrupos.SelectedIndexChanged
        Call obtColaboradoresPorGrupo()
    End Sub



    Private Sub btnAgregar_ServerClick(sender As Object, e As EventArgs) Handles btnAgregar.ServerClick
        Call insColaboradorGrupo()
    End Sub
    'inserta Colaborador en Grupo
    Public Sub insColaboradorGrupo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim iContador As Integer = 0

        Try
            odbConexion.Open()
            strQuery = "INSERT INTO [BECAS_GRUPOS_PREPARATORIA_TB] " & _
                       " ([fk_id_colaborador],[fk_id_grupo_preparatoria],[fecha_creacion] ,[usuario_creacion]) " & _
                       "VALUES (" & ddlColaboradores.SelectedValue & "," & ddlGrupos.SelectedValue & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()
            Call obtColaboradoresPorGrupo()
            Call obtCatalogoColaborador()
        Catch ex As Exception
            lblError.Text = ex.Message.ToString
        End Try
    End Sub

    Public Sub comportamientos()
        'Colaboradores Grupo
        For iFil As Integer = 0 To grdColaboradoresGrupo.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdColaboradoresGrupo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Asistencia 
        For iFil As Integer = 0 To grdAsistenciaColaboradores.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdAsistenciaColaboradores.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'valida la edicion de algun dato del control de asistencia
        If hdEdicionAsistencia.Value = 0 Then
            btnGuardar.Attributes.CssStyle.Add("display ", "none")
        Else
            btnGuardar.Attributes.CssStyle.Add("display ", "inline")
        End If

    End Sub

    Private Sub grdColaboradoresGrupo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdColaboradoresGrupo.RowDataBound
        For i As Integer = 0 To grdColaboradoresGrupo.Rows.Count - 1


            Dim btnEliminar As LinkButton = grdColaboradoresGrupo.Rows(i).Controls(5).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el colaborador " + grdColaboradoresGrupo.Rows(i).Cells(1).Text + _
                " del grupo?')){ return false; };"

        Next
    End Sub
    Private Sub grdColaboradoresGrupo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdColaboradoresGrupo.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strClave As String = ""
        Try
            strClave = DirectCast(grdColaboradoresGrupo.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strClave = "" Then Exit Sub
            odbConexion.Open()

            strQuery = " DELETE BECAS_GRUPOS_PREPARATORIA_TB " & _
                       " WHERE fk_id_colaborador=" & strClave & " AND fk_id_grupo_preparatoria=" & ddlGrupos.SelectedValue

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            'actualiza informacion cargada en los grids
            Call obtColaboradoresPorGrupo()
            Call obtCatalogoColaborador()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

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
#Region "Asistencia"
    'Crear lista de Asistencia
    Public Sub CrearAsistenciaGrupo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""

        Try
            odbConexion.Open()

            If validaRegistroAsistencia() Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe Registro de Asistencia para la Fecha " & txtFecha.Text & ".');</script>", False)
                Exit Sub
            End If
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_grupo_preparatoria_asistencia_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@idGrupo", ddlGrupoAsistencia.SelectedValue)
            odbComando.Parameters.AddWithValue("@fechaAsistencia", txtFecha.Text)
            odbComando.Parameters.AddWithValue("@usuarioCreacion", hdUsuario.Value)

            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtAsistenciaColaboradores()
        Catch ex As Exception
            lblErrorAsistencia.Text = ex.Message.ToString
        End Try
    End Sub


    'calcula el numero correlativo
    Private Sub btnAsistencia_ServerClick(sender As Object, e As EventArgs) Handles btnAsistencia.ServerClick
        Call CrearAsistenciaGrupo()
    End Sub
    Private Sub btnBuscar_ServerClick(sender As Object, e As EventArgs) Handles btnBuscar.ServerClick
        hdEdicionAsistencia.Value = 0
        Call obtAsistenciaColaboradores()
        Call comportamientos()
    End Sub

    ''Obtiene la asistencia de los colaboradores
    Public Sub obtAsistenciaColaboradores()
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Try

            odbConexion.Open()

            'obtiene la informacion del registro de Asistencia
            strQuery = "SELECT * FROM BECAS_GRUPO_PREPARATORIA_ASISTENCIA_VT WHERE FK_ID_GRUPO_PREPARATORIA=" & IIf(ddlGrupoAsistencia.SelectedValue = "", 0, ddlGrupoAsistencia.SelectedValue) & _
                " AND FECHA_ASISTENCIA='" & txtFecha.Text & "' ORDER BY CLAVE"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdAsistenciaColaboradores.DataSource = dsCatalogo.Tables(0).DefaultView
            grdAsistenciaColaboradores.DataBind()
            If grdAsistenciaColaboradores.Rows.Count = 0 Then
                divAsistenciaColaboradores.Visible = False
                lblRegistrosCarta.Text = "No hay Registros de Asistencia."
            Else
                divAsistenciaColaboradores.Visible = True
                lblRegistrosCarta.Text = ""
            End If


            For iFil As Integer = 0 To grdAsistenciaColaboradores.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdAsistenciaColaboradores.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            odbConexion.Close()
        Catch ex As Exception
            lblErrorAsistencia.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'VALIDA QUE YA EXISTA ASISTENCIA PARA ESA FECHA
    Public Function validaRegistroAsistencia() As Boolean
        Dim blResultado As Boolean = False
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try

            odbConexion.Open()

            'obtiene la informacion de las cartas configuradas al siguiente fecha de calendario
            strQuery = "SELECT COUNT(*) FROM [BECAS_GRUPOS_PREPARATORIA_ASISTENCIA_TB] WHERE FK_ID_GRUPO_PREPARATORIA=" & IIf(ddlGrupoAsistencia.SelectedValue = "", 0, ddlGrupoAsistencia.SelectedValue) & _
                " AND FECHA_ASISTENCIA='" & txtFecha.Text & "' "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                blResultado = IIf(odbLector(0) = 0, False, True)
                odbLector.Close()
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblErrorAsistencia.Text = Err.Number & " " & ex.Message
        End Try
        Return blResultado
    End Function
#End Region


    Private Sub btnGuardar_ServerClick(sender As Object, e As EventArgs) Handles btnGuardar.ServerClick
        Call actualizaAsistenciaGrupo()
    End Sub
    'Guarda Cambios de Asistencia
    Public Sub actualizaAsistenciaGrupo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim iContador As Integer = 0

        Try
            odbConexion.Open()
            For Each row As GridViewRow In grdAsistenciaColaboradores.Rows
                Dim chkcheck As CheckBox = DirectCast(row.FindControl("chkAsistencia"), CheckBox)
                Dim strId As String = DirectCast(row.FindControl("lblId"), Label).Text


                strQuery = "UPDATE [dbo].[BECAS_GRUPOS_PREPARATORIA_ASISTENCIA_TB]" & _
                               "  SET [asistencia] = " & IIf(chkcheck.Checked, 1, 0) & "" & _
                               "     ,[fecha_modificacion] = GETDATE()" & _
                               "     ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                               "WHERE ID=" & strId

                Dim odbComando As New OleDbCommand(strQuery, odbConexion)
                odbComando.ExecuteNonQuery()


            Next

            odbConexion.Close()
            'actualiza informacion cargada en los grids
            Call obtAsistenciaColaboradores()
            hdEdicionAsistencia.Value = 0
            Call comportamientos()

        Catch ex As Exception
            lblErrorAsistencia.Text = ex.Message.ToString
        End Try
    End Sub

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