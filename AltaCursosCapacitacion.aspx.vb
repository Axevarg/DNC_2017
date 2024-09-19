Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class AltaCursosCapacitacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""

        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            Call obtenerUsuarioAD()
            Call CargaCatalogos()

        End If
        Call Comportamiento()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogos"


    Public Sub CargaCatalogos()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Call ObtCursoBuscar(odbConexion)
            Call ObtCursoCapacitacion(odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    Private Sub ObtCursoBuscar(ByVal odbConexion As OleDbConnection)
        Dim strFiltro As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT '0'as ID,'  SELECCIONAR CURSO' as descripcion_capacitacion_corta union all "
        strQuery += "SELECT  id,descripcion_capacitacion_corta  FROM DNC_CURSOS_TB WHERE ESTATUS<>'ELIMINADO' ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlCursos.DataSource = odbLector
        ddlCursos.DataValueField = "ID"
        ddlCursos.DataTextField = "descripcion_capacitacion_corta"

        ddlCursos.DataBind()

    End Sub
    Private Sub ObtCursoCapacitacion(ByVal odbConexion As OleDbConnection)
        Dim strFiltro As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT '0'as ID,'  SELECCIONAR CURSO' as descripcion_capacitacion_corta union all "
        strQuery += "SELECT  A.ID,CAST(a.id AS VARCHAR) +' '+ B.descripcion_capacitacion_corta + ' '+ A.estatus  as descripcion  " & _
                    "FROM GC_REGISTRO_CURSO_TB A INNER JOIN DNC_CURSOS_TB B ON A.fk_id_curso=B.ID  WHERE A.ESTATUS NOT IN('ELIMINADO') ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlCursosRegistrados.DataSource = odbLector
        ddlCursosRegistrados.DataValueField = "ID"
        ddlCursosRegistrados.DataTextField = "descripcion_capacitacion_corta"

        ddlCursosRegistrados.DataBind()

    End Sub
#End Region
#Region "Comportamientos"
    Public Sub Comportamiento()
        If hdIdCurso.Value = 0 Then
            btnActualizar.Visible = False
            btnEliminar.Visible = False
            btnNuevoCurso.Visible = False
            btnAgregar.Visible = True
            divAuto.Visible = False
            divGrid.Visible = False
            divComentarios.Visible = False
            ddlCursos.Attributes.Remove("disabled")
            divLiga.Visible = False
            divCursosMensaje.Visible = False
        Else
            'Cuando el el curso esta cerrado
            divCursosMensaje.Visible = True
            divAuto.Visible = True
            If hdEstatusCurso.Value = "CERRADO" Then
                btnActualizar.Visible = False
                btnEliminar.Visible = True
                btnNuevoCurso.Visible = True
                btnAgregar.Visible = False
                divGrid.Visible = True
                divComentarios.Visible = True
                ddlCursos.Attributes.Add("disabled", "disabled")
                grdHorarios.Columns(5).Visible = False
                grdHorarios.Columns(6).Visible = False
                lblCurso.Text = "Curso creado para alta de registro de colaboradores."
                divCursosMensaje.Visible = True
                divCursosMensaje.Attributes("class") = "callout callout-success"
                divLiga.visible = True
            Else
                btnActualizar.Visible = True
                btnEliminar.Visible = True
                btnNuevoCurso.Visible = True
                btnAgregar.Visible = False

                divGrid.Visible = True
                divComentarios.Visible = True
                ddlCursos.Attributes.Add("disabled", "disabled")
                grdHorarios.Columns(5).Visible = True
                grdHorarios.Columns(6).Visible = True
                divLiga.Visible = False

                divCursosMensaje.Attributes("class") = "callout callout-warning"
                lblCurso.Text = "El curso no esta cerrado para Publicarse "
            End If

        End If


    End Sub
#End Region
#Region "Acciones"
    Private Sub btnAgregar_ServerClick(sender As Object, e As EventArgs) Handles btnAgregar.ServerClick
        Call insCursos()
    End Sub
    'iNSERTA cURSO
    Public Sub insCursos()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim iId As Integer = 0
        Try
            odbConexion.Open()

            strQuery = "INSERT INTO [GC_REGISTRO_CURSO_TB]     " & _
                     "([fk_id_curso]                                " & _
                     ",[estatus]                                         " & _
                     ",[fecha_creacion]                                 " & _
                     ",[usuario_creacion]   )   " & _
                     " VALUES  ( " & _
                     " '" & ddlCursos.SelectedValue & "'" & _
                     ", 'CREADO'" & _
                     ", GETDATE()" & _
                     ", '" & hdUsuario.Value & "'" & _
                     ") Select @@Identity"
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)

            iId = CInt(odbComando.ExecuteScalar())
            hdIdCurso.Value = iId

            Call obtCurso(iId)
            Call Comportamiento()
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    'obtiene los registros de los autos
    Public Sub obtCurso(Optional iId As Integer = 0)
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'actualiza carros y empleados
            Call ObtCursoCapacitacion(odbConexion)

            strQuery = "SELECT * FROM [GC_REGISTRO_CURSO_TB] WHERE id=" & iId.ToString

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader

            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                ddlCursosRegistrados.SelectedValue = odbLector(0).ToString
                ddlCursos.SelectedValue = odbLector(1).ToString
                txtDescripcion.Text = odbLector(2).ToString
                lblRuta.Text = odbLector(4).ToString
                hdEstatusCurso.Value = odbLector(5).ToString
                odbLector.Close()
            End If

            odbConexion.Close()
            Call obtHorarios()
            Call Comportamiento()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub udCurso()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim strUrl As String = ""
        Dim strComentario As String
        strComentario = txtDescripcion.Text.Replace("'", "")
        strComentario = strComentario.Replace(" ", "&nbsp;")
        strComentario = strComentario.Replace(vbLf, "<br />")
        Try
            odbConexion.Open()
            strUrl = HttpContext.Current.Request.Url.AbsoluteUri
            strUrl = strUrl.Replace("AltaCursosCapacitacion.aspx", "AltaRegistroCursos.aspx") & "?iCR=" & ddlCursosRegistrados.SelectedValue

            strQuery = " UPDATE [dbo].[GC_REGISTRO_CURSO_TB]   " & _
            " SET [comentarios] ='" & txtDescripcion.Text & "'" & _
            "      ,[comentarios_correo] ='" & strComentario & "'" &
            "      ,[liga_acceso] ='" & strUrl & "'" & _
            "      ,[estatus] ='CERRADO'" & _
            "      ,[fecha_modificacion] = GETDATE()      " & _
            "      ,[usuario_modificacion] ='" & hdUsuario.Value & "'" & _
            " WHERE ID=" & hdIdCurso.Value.ToString
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            Call obtCurso(hdIdCurso.Value.ToString)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Private Sub btnActualizar_ServerClick(sender As Object, e As EventArgs) Handles btnActualizar.ServerClick
        Call udCurso()
    End Sub

    Public Sub delCurso()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()
            strQuery = " UPDATE [dbo].[GC_REGISTRO_CURSO_TB]                                 " & _
            "   SET[estatus] ='ELIMINADO'" & _
            "      ,[fecha_modificacion] = GETDATE()      " & _
            "      ,[usuario_modificacion] ='" & hdUsuario.Value & "'" & _
            " WHERE ID=" & hdIdCurso.Value.ToString
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub limpiarControles()
        ddlCursosRegistrados.SelectedValue = 0
        ddlCursos.SelectedValue = 0
        txtDescripcion.Text = ""

    End Sub


    Private Sub btnEliminar_ServerClick(sender As Object, e As EventArgs) Handles btnEliminar.ServerClick
        Call delCurso()
        hdIdCurso.Value = 0
        Call limpiarControles()
        Call Comportamiento()
        Call CargaCatalogos()
    End Sub

    Private Sub btnNuevoCurso_ServerClick(sender As Object, e As EventArgs) Handles btnNuevoCurso.ServerClick
        hdIdCurso.Value = 0
        Call limpiarControles()
        Call Comportamiento()
        Call CargaCatalogos()
    End Sub
#End Region
#Region "Grid Horarios"
    'obtiene la información del
    Public Sub obtHorarios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM GC_REGISTRO_HORARIOS_TB WHERE fk_id_registro_curso=" & ddlCursosRegistrados.SelectedValue & " ORDER BY id"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdHorarios.DataSource = dsCatalogo.Tables(0).DefaultView
            grdHorarios.DataBind()

            If grdHorarios.Rows.Count = 0 Then
                Call insFilaVaciaLimite()
                grdHorarios.Rows(0).Visible = False

            Else
                grdHorarios.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0


            'habilidades dina
            For iFil As Integer = 0 To grdHorarios.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdHorarios.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaLimite()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("no_personas"))
        dt.Columns.Add(New DataColumn("fecha_desde"))
        dt.Columns.Add(New DataColumn("fecha_hasta"))
        dt.Columns.Add(New DataColumn("horario"))



        dr = dt.NewRow
        dr("id") = ""
        dr("no_personas") = ""
        dr("fecha_desde") = ""
        dr("fecha_hasta") = ""
        dr("horario") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdHorarios.DataSource = dt.DefaultView
        grdHorarios.DataBind()
    End Sub

    'inserta registro al catalogo
    Public Sub insConcepto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strHorario As String = ""
        Dim strNumPersona As String = ""

        Try

            strNumPersona = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgregaNumPersona"), TextBox).Text)
            strFechaInicio = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreFechaFinal"), TextBox).Text)
            strHorario = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreHorario"), TextBox).Text)

            odbConexion.Open()

            If strNumPersona = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Número de Personas.');</script>", False)
                Exit Sub
            End If
            If CInt(strNumPersona) <= 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Número de Personas debe ser mayor a 0.');</script>", False)
                Exit Sub
            End If

            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio de Registro.');</script>", False)
                Exit Sub
            End If
            If strFechaFinal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha Final de Registro.');</script>", False)
                Exit Sub
            End If
            If CDate(strFechaInicio) > CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor a la Fecha Final.');</script>", False)
                Exit Sub
            End If

            If strHorario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar los Horarios.');</script>", False)
                Exit Sub
            End If
            strQuery = "INSERT INTO GC_REGISTRO_HORARIOS_TB (fk_id_registro_curso,no_personas ,fecha_desde" & _
                        ",fecha_hasta,horario,estatus,fecha_creacion ,usuario_creacion) VALUES ('" & _
                hdIdCurso.Value & "'," & strNumPersona & ",'" & strFechaInicio & "'" & _
                ",'" & strFechaFinal & "','" & strHorario & "',1,GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtHorarios()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdHorarios_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHorarios.RowCancelingEdit
        grdHorarios.ShowFooter = True
        grdHorarios.EditIndex = -1
        Call obtHorarios()
    End Sub

    'TOOLTIPS
    Private Sub grdHorarios_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdHorarios.RowDataBound

        For i As Integer = 0 To grdHorarios.Rows.Count - 1

            Dim btnEditar As LinkButton = grdHorarios.Rows(i).Controls(5).Controls(0)
            Dim btnEliminar As LinkButton = grdHorarios.Rows(i).Controls(6).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el Horario " + DirectCast(grdHorarios.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdHorarios.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el Horario " + DirectCast(grdHorarios.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
            End If
        Next

        'PAGINACION CON IMAGEN DE AVANCE
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim tb As New Table
            tb = e.Row.Cells(0).Controls(0)
            For Each pageCell As TableCell In tb.Rows(0).Cells
                'valida que se acontrol ImageButton
                Dim lnk As ImageButton
                lnk = pageCell.Controls(0)
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingLimite').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdHorarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdHorarios.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_REGISTRO_HORARIOS_TB WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHorarios.EditIndex = -1
            grdHorarios.ShowFooter = True
            Call obtHorarios()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    'habilita el modo edicion
    Private Sub grdHorarios_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdHorarios.RowEditing
        grdHorarios.ShowFooter = False
        grdHorarios.EditIndex = e.NewEditIndex
        Call obtHorarios()
    End Sub
    'actualiza la descripcion
    Private Sub grdHorarios_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdHorarios.RowUpdating
        grdHorarios.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strHorario As String = ""
        Dim strNumPersona As String = ""

        Try
            strId = DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strNumPersona = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtNumPersona"), TextBox).Text)
            strFechaInicio = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtFechaFinal"), TextBox).Text)
            strHorario = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtHorario"), TextBox).Text)

            If strNumPersona = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Número de Personas.');</script>", False)
                Exit Sub
            End If
            If CInt(strNumPersona) <= 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Número de Personas debe ser mayor a 0.');</script>", False)
                Exit Sub
            End If

            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio de Registro.');</script>", False)
                Exit Sub
            End If
            If strFechaFinal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha Final de Registro.');</script>", False)
                Exit Sub
            End If
            If CDate(strFechaInicio) > CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor a la Fecha Final.');</script>", False)
                Exit Sub
            End If
            If strHorario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar los Horarios.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE GC_REGISTRO_HORARIOS_TB " & _
                        "SET [no_personas] = '" & strNumPersona & "'" & _
                        ",[fecha_desde] = '" & strFechaInicio & "'" & _
                        ",[fecha_hasta] = '" & strFechaFinal & "'" & _
                        ",[horario] = '" & strHorario & "'" & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHorarios.EditIndex = -1
            Call obtHorarios()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarConcepto_Click(sender As Object, e As EventArgs)
        Call insConcepto()
    End Sub

    Protected Sub grdHorarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHorarios.PageIndexChanging
        grdHorarios.ShowFooter = True
        grdHorarios.PageIndex = e.NewPageIndex
        grdHorarios.DataBind()
        Call obtHorarios()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionPuesto(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_REGISTRO_HORARIOS_TB where (puesto='" & strDescripcion & "' and fk_id_registro_curso= ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
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

    Private Sub ddlCursosRegistrados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCursosRegistrados.SelectedIndexChanged

        hdIdCurso.Value = ddlCursosRegistrados.SelectedValue
        Call limpiarControles()
        Call obtCurso(hdIdCurso.Value)

        Call Comportamiento()
    End Sub

End Class