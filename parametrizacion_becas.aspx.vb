Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class parametrizacion_becas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorMontosDuracion.Text = ""
        lblErrorCalendarios.Text = ""
        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            Call obtenerUsuarioAD()
            Call obtCatalogoMontos()
            Call obtAniosHorarios()
            Call obtCatalogoHorarios()
            Call obtHorarios()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
        Call comportamiento()
    End Sub

#Region "Catalogos"
    'Obtiene el catalogo de Años
    Public Sub obtAniosHorarios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año
            Call registraAnioVigente(odbConexion)

            strQuery = " SELECT * FROM BECAS_ANIOS_CT  order by 1 DESC"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlAnio.DataSource = odbLector
            ddlAnio.DataTextField = "anio"
            ddlAnio.DataValueField = "anio"

            ddlAnio.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub
    'regitra el año vigente en la tabla de aaños de becast
    Public Sub registraAnioVigente(odbConexion As OleDbConnection)
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "becas_registra_anio_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure
        odbComando.ExecuteNonQuery()
    End Sub

    'Obtiene el catalogo de Años
    Public Sub obtCatalogoHorarios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()


            strQuery = " SELECT * FROM BECAS_INGLES_HORARIO_ESTUDIO_CT  WHERE ESTATUS=1 order by 2"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlHorarioEstudio.DataSource = odbLector
            ddlHorarioEstudio.DataTextField = "descripcion"
            ddlHorarioEstudio.DataValueField = "id"

            ddlHorarioEstudio.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Catálogos de Montos y Duracion"
    'obtiene el catalogo 
    Public Sub obtCatalogoMontos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_MONTOS_DURACION_CT   ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdMontosDurCursos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdMontosDurCursos.DataBind()

            If grdMontosDurCursos.Rows.Count = 0 Then
                Call insFilaMontosCursos()
                grdMontosDurCursos.Rows(0).Visible = False


            Else
                grdMontosDurCursos.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If

            odbConexion.Close()

            For i = 0 To grdMontosDurCursos.Rows.Count - 1
                Dim iId As String
                Dim ddlTipoBeca As DropDownList
                Dim strTipo As String = ""
                Dim btnEditar As LinkButton = grdMontosDurCursos.Rows(i).Controls(4).Controls(0)

                iId = DirectCast(grdMontosDurCursos.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlTipoBeca = grdMontosDurCursos.Rows(i).Cells(1).FindControl("ddlTipoBeca")

                    'obtiene provedor
                    Call obtddlTipoBeca(ddlTipoBeca, 1, iId)

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlTipoBeca.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(1).ToString
                        End If
                    Next

                Else
                    'empleado
                    Dim lblTipoBeca As New Label
                    lblTipoBeca = grdMontosDurCursos.Rows(i).FindControl("lblTipoBeca")
                    lblTipoBeca.Text = obtTextoTipoBeca(lblTipoBeca.Text)

                End If
            Next

            Dim ddlAgregaTBeca As DropDownList
            ddlAgregaTBeca = grdMontosDurCursos.FooterRow.FindControl("ddlAgreTipoBeca")
            Call obtddlTipoBeca(ddlAgregaTBeca, 0)
            '  colorea las celdas del grid
            For iFil As Integer = 0 To grdMontosDurCursos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdMontosDurCursos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next


        Catch ex As Exception
            lblErrorMontosDuracion.Text = ex.Message
        End Try


    End Sub

    'Obtiene el catalogo de proveedores
    Public Sub obtddlTipoBeca(ByVal ddl As DropDownList, modoEdicion As Integer, Optional idEdicion As Integer = 0)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()
        'MODO 0 ES INSERTAR
        If modoEdicion = 0 Then
            strQuery = " SELECT ID,descripcion FROM BECAS_TIPO_BECA_CT WHERE ESTATUS=1 AND ID NOT IN (SELECT fk_id_tipo_beca FROM BECAS_MONTOS_DURACION_CT ) ORDER BY 2"
        Else
            strQuery = " SELECT ID,descripcion FROM BECAS_TIPO_BECA_CT " & _
                       " WHERE ESTATUS=1 AND ID NOT IN (SELECT fk_id_tipo_beca FROM BECAS_MONTOS_DURACION_CT WHERE ID<>" & idEdicion & ") " & _
                       " ORDER BY 2"
        End If

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoTipoBeca(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_TIPO_BECA_CT  WHERE ID=" & IIf(strid = "", 0, strid)

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        odbConexion.Close()
        Return strResultado
    End Function
    'valida si la descripcion ya existe en el catalogo
    Public Function validaTipoCompra(strTipoCompra As String, Optional iExistencia As Integer = 0, Optional iId As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        Dim strFiltro As String = ""
        odbConexion.Open()
        strFiltro = IIf(iId = 0, "", " and id <>" & iId.ToString)
        strQuery = "SELECT COUNT(*) FROM BECAS_MONTOS_DURACION_CT where fk_id_tipo_beca=" & strTipoCompra & strFiltro & " ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) >= 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function



    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaMontosCursos()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_beca"))
        dt.Columns.Add(New DataColumn("meses"))
        dt.Columns.Add(New DataColumn("monto"))
        dr = dt.NewRow
        dr("id") = ""
        dr("fk_id_tipo_beca") = ""
        dr("meses") = ""
        dr("monto") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdMontosDurCursos.DataSource = dt.DefaultView
        grdMontosDurCursos.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insMontosDuracionBecas()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strTipoCompra As String = ""
        Dim strMeses As String = ""
        Dim strMontos As String = ""
        Try

            strTipoCompra = (DirectCast(grdMontosDurCursos.FooterRow.FindControl("ddlAgreTipoBeca"), DropDownList).Text)
            strMeses = (DirectCast(grdMontosDurCursos.FooterRow.FindControl("txtAgregaMeses"), TextBox).Text)
            strMontos = (DirectCast(grdMontosDurCursos.FooterRow.FindControl("txtAgregarMontos"), TextBox).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strTipoCompra = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de estar Registrado un tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If validaTipoCompra(strTipoCompra) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe ese Tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If strMeses = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar los meses.');</script>", False)
                Exit Sub
            End If

            If strMontos = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Monto.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO BECAS_MONTOS_DURACION_CT (fk_id_tipo_beca,meses,monto,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strTipoCompra.ToUpper & "','" & strMeses.ToUpper & "','" & strMontos.ToUpper & "'," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoMontos()
        Catch ex As Exception
            lblErrorMontosDuracion.Text = ex.Message
        End Try
    End Sub
    Private Sub grdMontosDurCursos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdMontosDurCursos.RowCancelingEdit
        grdMontosDurCursos.ShowFooter = True
        grdMontosDurCursos.EditIndex = -1
        Call obtCatalogoMontos()
    End Sub

    'TOOLTIPS
    Private Sub grdMontosDurCursos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMontosDurCursos.RowDataBound

        For i As Integer = 0 To grdMontosDurCursos.Rows.Count - 1

            Dim editar As LinkButton = grdMontosDurCursos.Rows(i).Controls(4).Controls(0)
            Dim btnEl As LinkButton = grdMontosDurCursos.Rows(i).Controls(5).Controls(1)

            If editar.Text = "Editar" Then
                editar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdMontosDurCursos.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                editar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdMontosDurCursos.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdMontosDurCursos.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingPagina').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    Private Sub grdMontosDurCursos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdMontosDurCursos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdMontosDurCursos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM BECAS_MONTOS_DURACION_CT WHERE ID=" & strId

            If validaBecas(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdMontosDurCursos.EditIndex = -1
            grdMontosDurCursos.ShowFooter = True
            Call obtCatalogoMontos()
        Catch ex As Exception
            lblErrorMontosDuracion.Text = ex.Message
        End Try

    End Sub

    'valida si exite en condiciones de unidad de Autos 
    Public Function validaBecas(odbConexion As OleDbConnection, strAnio As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        'strQuery = "SELECT COUNT(fk_id_anio) as tipo FROM [DNC_CAPACITACIONES_REQUERIDAS_TB] WHERE [fk_id_anio]=" & strAnio.ToString

        'Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        'Dim odbLector As OleDbDataReader

        'odbLector = odbComando.ExecuteReader
        'If odbLector.HasRows Then
        '    odbLector.Read()
        '    bResultados = IIf(odbLector(0) > 0, True, False)
        '    odbLector.Close()
        'End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdMontosDurCursos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdMontosDurCursos.RowEditing
        grdMontosDurCursos.ShowFooter = False
        grdMontosDurCursos.EditIndex = e.NewEditIndex
        Call obtCatalogoMontos()
    End Sub
    'actualiza la descripcion
    Private Sub grdMontosDurCursos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdMontosDurCursos.RowUpdating
        grdMontosDurCursos.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strTipoCompra, strMeses, strMontos As String
        Dim strId As String

        Try
            strId = DirectCast(grdMontosDurCursos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strTipoCompra = (DirectCast(grdMontosDurCursos.Rows(e.RowIndex).FindControl("ddlTipoBeca"), DropDownList).Text)
            strMeses = (DirectCast(grdMontosDurCursos.Rows(e.RowIndex).FindControl("txtMeses"), TextBox).Text)
            strMontos = (DirectCast(grdMontosDurCursos.Rows(e.RowIndex).FindControl("txtMontos"), TextBox).Text)
            odbConexion.Open()

            If strTipoCompra = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de estar Registrado un tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If validaTipoCompra(strTipoCompra, 1, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe ese Tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If strMeses = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar los meses.');</script>", False)
                Exit Sub
            End If

            If strMontos = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Monto.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE BECAS_MONTOS_DURACION_CT " & _
                        "SET [fk_id_tipo_beca] = '" & strTipoCompra & "'" & _
                         ",[meses] = '" & strMeses.ToUpper & "'" & _
                         ",[monto] = '" & strMontos.ToUpper & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdMontosDurCursos.EditIndex = -1
            Call obtCatalogoMontos()
        Catch ex As Exception
            lblErrorMontosDuracion.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdMontosDurCursos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdMontosDurCursos.PageIndexChanging
        Try
            grdMontosDurCursos.ShowFooter = True
            grdMontosDurCursos.EditIndex = -1
            grdMontosDurCursos.PageIndex = e.NewPageIndex
            grdMontosDurCursos.DataBind()
            Call obtCatalogoMontos()
        Catch ex As Exception
            lblErrorMontosDuracion.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarFacilitador_Click(sender As Object, e As EventArgs)
        Call insMontosDuracionBecas()
    End Sub
    'limia los label de erroresl
    'Public Sub limpiaLabel()
    '    lblErrorProveedor.Text = ""
    '    lblErrorFacilitador.Text = ""

    '    'colorea las celdas del grid
    '    For iFil As Integer = 0 To grdMontosDurCursos.Rows.Count - 1
    '        If iFil Mod 2 = 0 Then
    '            grdMontosDurCursos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
    '        End If
    '    Next
    '    'colorea las celdas del grid
    '    For iFil As Integer = 0 To grdProveedores.Rows.Count - 1
    '        If iFil Mod 2 = 0 Then
    '            grdProveedores.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
    '        End If
    '    Next
    'End Sub
#End Region
#Region "Horarios"
    ''obtiene el catalogo 
    Public Sub obtHorarios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_INGLES_CALENDARIO_CT WHERE (fk_id_horario_estudio=" & _
                        IIf(ddlHorarioEstudio.SelectedValue = "", 0, ddlHorarioEstudio.SelectedValue) & _
                        " and anio=" & IIf(ddlAnio.SelectedValue = "", 0, ddlAnio.SelectedValue) & ")  ORDER BY fecha_inicio ASC"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdHorarios.DataSource = dsCatalogo.Tables(0).DefaultView
            grdHorarios.DataBind()

            If grdHorarios.Rows.Count = 0 Then
                Call insFilaVaciaHorario()
                grdHorarios.Rows(0).Visible = False


            Else
                grdHorarios.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If

            odbConexion.Close()

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdHorarios.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdHorarios.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            'carga años

        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try


    End Sub

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaHorario()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("fecha_inicio"))
        dt.Columns.Add(New DataColumn("fecha_termino"))
        dt.Columns.Add(New DataColumn("fecha_entrega_calificaciones"))


        dr = dt.NewRow
        dr("id") = ""
        dr("fecha_inicio") = ""
        dr("fecha_termino") = ""
        dr("fecha_entrega_calificaciones") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdHorarios.DataSource = dt.DefaultView
        grdHorarios.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insHorarios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strFechaEntrega As String = ""

        Try

            strFechaInicio = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreFechaFinal"), TextBox).Text)
            strFechaEntrega = (DirectCast(grdHorarios.FooterRow.FindControl("txtAgreFechaEntregaC"), TextBox).Text)


            odbConexion.Open()
            'validaciones para insertar registros



            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio de Registro.');</script>", False)
                Exit Sub
            End If
            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaInicio).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha de Inicio no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If strFechaFinal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha Final de Registro.');</script>", False)
                Exit Sub
            End If

            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaFinal).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha Final no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If CDate(strFechaInicio) >= CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Final.');</script>", False)
                Exit Sub
            End If
            If strFechaEntrega = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Entrega.');</script>", False)
                Exit Sub
            End If
            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaEntrega).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha de Entrega de Calificaciones no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If CDate(strFechaInicio) >= CDate(strFechaEntrega) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Entrega.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_INGLES_CALENDARIO_CT ([fk_id_horario_estudio],[anio],[fecha_inicio],[fecha_termino],[fecha_entrega_calificaciones],[fecha_creacion],[usuario_creacion])" & _
                       " VALUES (" & ddlHorarioEstudio.SelectedValue & "," & ddlAnio.SelectedValue & ",'" & strFechaInicio & "','" & strFechaFinal & "','" & strFechaEntrega & "' " & _
                      ",GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtHorarios()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub
    Private Sub grdHorarios_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHorarios.RowCancelingEdit
        grdHorarios.ShowFooter = True
        grdHorarios.EditIndex = -1
        Call obtHorarios()
    End Sub

    'Validaciones al Eliminar
    Private Sub grdHorarios_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdHorarios.RowDataBound

        For i As Integer = 0 To grdHorarios.Rows.Count - 1

            Dim btnEditar As LinkButton = grdHorarios.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdHorarios.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el horario " + DirectCast(grdHorarios.Rows(i).Controls(1).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdHorarios.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el horario" + DirectCast(grdHorarios.Rows(i).Controls(1).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingPagina').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila

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
        Dim strFechaEntrega As String = ""
        Try
            strId = DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strFechaInicio = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtFechaFinal"), TextBox).Text)
            strFechaEntrega = (DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("txtFechaEntregaC"), TextBox).Text)
            odbConexion.Open()
            'validaciones para insertar registros


            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio de Registro.');</script>", False)
                Exit Sub
            End If
            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaInicio).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha de Inicio no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If strFechaFinal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha Final de Registro.');</script>", False)
                Exit Sub
            End If

            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaFinal).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha Final no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If CDate(strFechaInicio) >= CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Final.');</script>", False)
                Exit Sub
            End If
            If strFechaEntrega = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Entrega.');</script>", False)
                Exit Sub
            End If
            'valida el año de registro sea el mismo que indica el combo
            If CDate(strFechaEntrega).Year <> ddlAnio.SelectedValue Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año de la Fecha de Entrega de Calificaciones no es igual al año a configurar.');</script>", False)
                Exit Sub
            End If

            If CDate(strFechaInicio) >= CDate(strFechaEntrega) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Entrega.');</script>", False)
                Exit Sub
            End If

            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE [dbo].[BECAS_INGLES_CALENDARIO_CT]" & _
                        " SET [fecha_inicio] = '" & strFechaInicio & "'" & _
                        " ,[fecha_termino] = '" & strFechaFinal & "'" & _
                        " ,[fecha_entrega_calificaciones] = '" & strFechaEntrega & "'" & _
                        " ,fecha_modificacion=GETDATE()" & _
                        " ,usuario_modificacion='" & hdUsuario.Value & "'" & _
                        " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHorarios.EditIndex = -1
            Call obtHorarios()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdHorarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHorarios.PageIndexChanging
        Try
            grdHorarios.ShowFooter = True
            grdHorarios.EditIndex = -1
            grdHorarios.PageIndex = e.NewPageIndex
            grdHorarios.DataBind()
            Call obtHorarios()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarHorario_Click(sender As Object, e As EventArgs)
        Call insHorarios()
    End Sub

    Private Sub grdHorarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdHorarios.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdHorarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaCartaCredito(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Carta Crédito.');</script>", False)
                Exit Sub
            End If
            strQuery = "DELETE FROM DNC_PARAMETRIZACION_CT WHERE ID=" & strId
            'valida en Facilitadores

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdHorarios.EditIndex = -1
            grdHorarios.ShowFooter = True
            Call obtHorarios()
        Catch ex As Exception
            lblErrorCalendarios.Text = ex.Message
        End Try
    End Sub

    'valida si exisen cartas crédito en
    Public Function validaCartaCredito(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_INGLES_CARTA_CREDITO_TB where fk_id_calendario_ingles=" & id_Medicion.ToString
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function
#End Region

    Public Sub comportamiento()
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdHorarios.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdHorarios.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        For iFil As Integer = 0 To grdMontosDurCursos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdMontosDurCursos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

    End Sub


    Private Sub ddlAnio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAnio.SelectedIndexChanged
        Call obtHorarios()
    End Sub

    Private Sub ddlHorarioEstudio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlHorarioEstudio.SelectedIndexChanged
        Call obtHorarios()
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