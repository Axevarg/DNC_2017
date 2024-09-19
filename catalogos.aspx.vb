Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class catalogos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorBecas.Text = ""
        lblErrorIngles.Text = ""
        lblErrorGestionCap.text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call inicialiciaTabs()
            Call CargaCatalogosDnc()
            Call CargaCatalogosBecas()
            Call CargaCatalogosBecasIngles()
            Call CargaCatalogosGestionCapacitacion()
        End If
        Call comportamientos()
    End Sub
    '************************* - Catalogos Generales - *******************************************
#Region "Grid Servicios Proveedor"
    'obtiene el catalogo 
    Public Sub obtCatalogoServiciosProveedor()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM SIGIDO_SERVICIOS_PROVEEDOR_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdServicioProveedor.DataSource = dsCatalogo.Tables(0).DefaultView
            grdServicioProveedor.DataBind()

            If grdServicioProveedor.Rows.Count = 0 Then
                Call insFilaVaciaServicios()
                grdServicioProveedor.Rows(0).Visible = False

            Else
                grdServicioProveedor.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdServicioProveedor.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdServicioProveedor.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdServicioProveedor.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdServicioProveedor.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdServicioProveedor.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdServicioProveedor.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdServicioProveedor.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaServicios()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdServicioProveedor.DataSource = dt.DefaultView
        grdServicioProveedor.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionServicios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdServicioProveedor.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdServicioProveedor.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionServiciosProve(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO SIGIDO_SERVICIOS_PROVEEDOR_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoServiciosProveedor()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdServicioProveedor_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdServicioProveedor.RowCancelingEdit
        grdServicioProveedor.ShowFooter = True
        grdServicioProveedor.EditIndex = -1
        Call obtCatalogoServiciosProveedor()
    End Sub

    'TOOLTIPS
    Private Sub grdServicioProveedor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdServicioProveedor.RowDataBound

        For i As Integer = 0 To grdServicioProveedor.Rows.Count - 1

            Dim btnEditar As LinkButton = grdServicioProveedor.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdServicioProveedor.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdServicioProveedor.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdServicioProveedor.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdServicioProveedor.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdServicioProveedor_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdServicioProveedor.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdServicioProveedor.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaServiciosProveedor(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM SIGIDO_SERVICIOS_PROVEEDOR_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdServicioProveedor.EditIndex = -1
            grdServicioProveedor.ShowFooter = True
            Call obtCatalogoServiciosProveedor()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaServiciosProveedor(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdServicioProveedor_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdServicioProveedor.RowEditing
        grdServicioProveedor.ShowFooter = False
        grdServicioProveedor.EditIndex = e.NewEditIndex
        Call obtCatalogoServiciosProveedor()
    End Sub
    'actualiza la descripcion
    Private Sub grdServicioProveedor_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdServicioProveedor.RowUpdating
        grdServicioProveedor.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdServicioProveedor.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdServicioProveedor.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdServicioProveedor.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionServiciosProve(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE SIGIDO_SERVICIOS_PROVEEDOR_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdServicioProveedor.EditIndex = -1
            Call obtCatalogoServiciosProveedor()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarServicioPro_Click(sender As Object, e As EventArgs)
        Call insDescripcionServicios()
    End Sub

    Protected Sub grdServicioProveedor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdServicioProveedor.PageIndexChanging
        grdServicioProveedor.ShowFooter = True
        grdServicioProveedor.PageIndex = e.NewPageIndex
        grdServicioProveedor.DataBind()
        Call obtCatalogoServiciosProveedor()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionServiciosProve(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM SIGIDO_SERVICIOS_PROVEEDOR_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
    '************************* - Catalogos de DNC - *******************************************
    Public Sub CargaCatalogosDnc()
        Call obtCatalogoCompetencia()
        Call obtCatalogoHabilidades()
        Call obtCatalogoMedirEfectividad()
        Call obtCatalogoMotivo()
        Call obtCatalogoObjetivosCorporativos()
        Call obtCatalogoServiciosProveedor()
        Call obtCatalogoTipoIndicador()
        Call obtCatalogoIndicador()
        Call obtCatalogoObjetivosDet()
        Call obtCatalogoEstatus()
        Call obtCatalogoModalidadCursos()
    End Sub
#Region "Grid Competencias Vinculadas"
    ''obtiene el catalogo 
    Public Sub obtCatalogoCompetencia()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_COMPETENCIAS_VINCULADAS_CT  ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdCompetenciasV.DataSource = dsCatalogo.Tables(0).DefaultView
            grdCompetenciasV.DataBind()

            If grdCompetenciasV.Rows.Count = 0 Then
                Call insFilaVacia()
                grdCompetenciasV.Rows(0).Visible = False


            Else
                grdCompetenciasV.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdCompetenciasV.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdCompetenciasV.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdCompetenciasV.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdCompetenciasV.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdCompetenciasV.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'competencia vinculada
            For iFil As Integer = 0 To grdCompetenciasV.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdCompetenciasV.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionCompetencias(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_COMPETENCIAS_VINCULADAS_CT where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVacia()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("definicion"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("definicion") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdCompetenciasV.DataSource = dt.DefaultView
        grdCompetenciasV.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionCompetencia()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strDefinicion As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdCompetenciasV.FooterRow.FindControl("txtAgregaDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdCompetenciasV.FooterRow.FindControl("txtAgregarDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdCompetenciasV.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Definición.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionCompetencias(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO DNC_COMPETENCIAS_VINCULADAS_CT (descripcion,definicion,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strDefinicion & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoCompetencia()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdCompetenciasV_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdCompetenciasV.RowCancelingEdit
        grdCompetenciasV.ShowFooter = True
        grdCompetenciasV.EditIndex = -1
        Call obtCatalogoCompetencia()
    End Sub

    Private Sub grdCompetenciasV_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCompetenciasV.RowDataBound

        For i As Integer = 0 To grdCompetenciasV.Rows.Count - 1
            Dim btnEditar As LinkButton = grdCompetenciasV.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdCompetenciasV.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdCompetenciasV.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdCompetenciasV.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdCompetenciasV.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiCompetencia(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [DNC_GESTION_CURSOS_TB] WHERE [fk_id_competencia_vinculada]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdCompetenciasV_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCompetenciasV.RowEditing
        grdCompetenciasV.ShowFooter = False
        grdCompetenciasV.EditIndex = e.NewEditIndex
        Call obtCatalogoCompetencia()
    End Sub
    'actualiza la descripcion
    Private Sub grdCompetenciasV_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdCompetenciasV.RowUpdating
        grdCompetenciasV.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strDefinicion, strEstatus As String
        Try
            strId = DirectCast(grdCompetenciasV.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdCompetenciasV.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdCompetenciasV.Rows(e.RowIndex).FindControl("txtDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdCompetenciasV.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la definición.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionCompetencias(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [DNC_COMPETENCIAS_VINCULADAS_CT] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[definicion] ='" & strDefinicion & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdCompetenciasV.EditIndex = -1
            Call obtCatalogoCompetencia()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdCompetenciasV_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCompetenciasV.PageIndexChanging
        Try
            grdCompetenciasV.ShowFooter = True
            grdCompetenciasV.EditIndex = -1
            grdCompetenciasV.PageIndex = e.NewPageIndex
            grdCompetenciasV.DataBind()
            Call obtCatalogoCompetencia()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarCondicion_Click(sender As Object, e As EventArgs)
        Call insDescripcionCompetencia()
    End Sub

    Private Sub grdCompetenciasV_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCompetenciasV.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdCompetenciasV.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_COMPETENCIAS_VINCULADAS_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiCompetencia(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdCompetenciasV.EditIndex = -1
            grdCompetenciasV.ShowFooter = True
            Call obtCatalogoCompetencia()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region

#Region "Grid Habilidades"
    'obtiene el catalogo 
    Public Sub obtCatalogoHabilidades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_HABILIDADES_DINA_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdHabilidadesDina.DataSource = dsCatalogo.Tables(0).DefaultView
            grdHabilidadesDina.DataBind()

            If grdHabilidadesDina.Rows.Count = 0 Then
                Call insFilaVaciaHabilidades()
                grdHabilidadesDina.Rows(0).Visible = False

            Else
                grdHabilidadesDina.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdHabilidadesDina.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdHabilidadesDina.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdHabilidadesDina.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdHabilidadesDina.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdHabilidadesDina.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'habilidades dina
            For iFil As Integer = 0 To grdHabilidadesDina.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdHabilidadesDina.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaHabilidades()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdHabilidadesDina.DataSource = dt.DefaultView
        grdHabilidadesDina.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionHabilidades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdHabilidadesDina.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdHabilidadesDina.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHabilidades(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DNC_HABILIDADES_DINA_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdHabilidadesDina_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHabilidadesDina.RowCancelingEdit
        grdHabilidadesDina.ShowFooter = True
        grdHabilidadesDina.EditIndex = -1
        Call obtCatalogoHabilidades()
    End Sub

    'TOOLTIPS
    Private Sub grdHabilidadesDina_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdHabilidadesDina.RowDataBound

        For i As Integer = 0 To grdHabilidadesDina.Rows.Count - 1

            Dim btnEditar As LinkButton = grdHabilidadesDina.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdHabilidadesDina.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdHabilidadesDina.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdHabilidadesDina.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdHabilidadesDina.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdHabilidadesDina_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdHabilidadesDina.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdHabilidadesDina.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaHabilidades(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DNC_HABILIDADES_DINA_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHabilidadesDina.EditIndex = -1
            grdHabilidadesDina.ShowFooter = True
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaHabilidades(odbConexion As OleDbConnection, id_Habilidad As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from DNC_CURSOS_TB where fk_id_habilidad_dina=" & id_Habilidad.ToString
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

    'habilita el modo edicion
    Private Sub grdHabilidadesDina_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdHabilidadesDina.RowEditing
        grdHabilidadesDina.ShowFooter = False
        grdHabilidadesDina.EditIndex = e.NewEditIndex
        Call obtCatalogoHabilidades()
    End Sub
    'actualiza la descripcion
    Private Sub grdHabilidadesDina_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdHabilidadesDina.RowUpdating
        grdHabilidadesDina.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdHabilidadesDina.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdHabilidadesDina.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdHabilidadesDina.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHabilidades(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_HABILIDADES_DINA_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHabilidadesDina.EditIndex = -1
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarHabilidades_Click(sender As Object, e As EventArgs)
        Call insDescripcionHabilidades()
    End Sub

    Protected Sub grdHabilidadesDina_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHabilidadesDina.PageIndexChanging
        grdHabilidadesDina.ShowFooter = True
        grdHabilidadesDina.PageIndex = e.NewPageIndex
        grdHabilidadesDina.DataBind()
        Call obtCatalogoHabilidades()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionHabilidades(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_HABILIDADES_DINA_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Medir Efectividad"
    'obtiene el catalogo 
    Public Sub obtCatalogoMedirEfectividad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_MEDIR_EFECTIVIDAD_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdMedirEfectividad.DataSource = dsCatalogo.Tables(0).DefaultView
            grdMedirEfectividad.DataBind()

            If grdMedirEfectividad.Rows.Count = 0 Then
                Call insFilaVaciaMedirEfectividad()
                grdMedirEfectividad.Rows(0).Visible = False

            Else
                grdMedirEfectividad.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdMedirEfectividad.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdMedirEfectividad.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdMedirEfectividad.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdMedirEfectividad.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdMedirEfectividad.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdMedirEfectividad.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdMedirEfectividad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaMedirEfectividad()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdMedirEfectividad.DataSource = dt.DefaultView
        grdMedirEfectividad.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionMedirEfectividad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdMedirEfectividad.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdMedirEfectividad.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionMedirEfectividad(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DNC_MEDIR_EFECTIVIDAD_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoMedirEfectividad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdMedirEfectividad_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdMedirEfectividad.RowCancelingEdit
        grdMedirEfectividad.ShowFooter = True
        grdMedirEfectividad.EditIndex = -1
        Call obtCatalogoMedirEfectividad()
    End Sub

    'TOOLTIPS
    Private Sub grdMedirEfectividad_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMedirEfectividad.RowDataBound

        For i As Integer = 0 To grdMedirEfectividad.Rows.Count - 1

            Dim btnEditar As LinkButton = grdMedirEfectividad.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdMedirEfectividad.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdMedirEfectividad.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdMedirEfectividad.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdMedirEfectividad.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdMedirEfectividad_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdMedirEfectividad.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdMedirEfectividad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaMedicion(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DNC_MEDIR_EFECTIVIDAD_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdMedirEfectividad.EditIndex = -1
            grdMedirEfectividad.ShowFooter = True
            Call obtCatalogoMedirEfectividad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaMedicion(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from DNC_GESTION_CURSOS_TB where fk_id_medicion_efectividad=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdMedirEfectividad_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdMedirEfectividad.RowEditing
        grdMedirEfectividad.ShowFooter = False
        grdMedirEfectividad.EditIndex = e.NewEditIndex
        Call obtCatalogoMedirEfectividad()
    End Sub
    'actualiza la descripcion
    Private Sub grdMedirEfectividad_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdMedirEfectividad.RowUpdating
        grdMedirEfectividad.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdMedirEfectividad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdMedirEfectividad.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdMedirEfectividad.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionMedirEfectividad(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_MEDIR_EFECTIVIDAD_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdMedirEfectividad.EditIndex = -1
            Call obtCatalogoMedirEfectividad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarMedir_Click(sender As Object, e As EventArgs)
        Call insDescripcionMedirEfectividad()
    End Sub

    Protected Sub grdMedirEfectividad_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdMedirEfectividad.PageIndexChanging
        grdMedirEfectividad.ShowFooter = True
        grdMedirEfectividad.PageIndex = e.NewPageIndex
        grdMedirEfectividad.DataBind()
        Call obtCatalogoMedirEfectividad()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionMedirEfectividad(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_MEDIR_EFECTIVIDAD_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Motivos"
    ''obtiene el catalogo 
    Public Sub obtCatalogoMotivo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_MOTIVOS_CT  ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdMotivo.DataSource = dsCatalogo.Tables(0).DefaultView
            grdMotivo.DataBind()

            If grdMotivo.Rows.Count = 0 Then
                Call insFilaVaciaMotivo()
                grdMotivo.Rows(0).Visible = False


            Else
                grdMotivo.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdMotivo.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdMotivo.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdMotivo.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdMotivo.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdMotivo.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'competencia vinculada
            For iFil As Integer = 0 To grdMotivo.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdMotivo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionMotivo(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_MOTIVOS_CT where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaMotivo()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("definicion"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("definicion") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdMotivo.DataSource = dt.DefaultView
        grdMotivo.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionMotivo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strDefinicion As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdMotivo.FooterRow.FindControl("txtAgregaDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdMotivo.FooterRow.FindControl("txtAgregarDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdMotivo.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Definición.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionMotivo(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO DNC_MOTIVOS_CT (descripcion,definicion,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strDefinicion & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoMotivo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdMotivo_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdMotivo.RowCancelingEdit
        grdMotivo.ShowFooter = True
        grdMotivo.EditIndex = -1
        Call obtCatalogoMotivo()
    End Sub

    Private Sub grdMotivo_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMotivo.RowDataBound

        For i As Integer = 0 To grdMotivo.Rows.Count - 1
            Dim btnEditar As LinkButton = grdMotivo.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdMotivo.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdMotivo.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdMotivo.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdMotivo.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el Motivo 
    Public Function validaExiMotivo(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [DNC_GESTION_CURSOS_TB] WHERE [fk_id_motivo]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdMotivo_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdMotivo.RowEditing
        grdMotivo.ShowFooter = False
        grdMotivo.EditIndex = e.NewEditIndex
        Call obtCatalogoMotivo()
    End Sub
    'actualiza la descripcion
    Private Sub grdMotivo_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdMotivo.RowUpdating
        grdMotivo.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strDefinicion, strEstatus As String
        Try
            strId = DirectCast(grdMotivo.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdMotivo.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdMotivo.Rows(e.RowIndex).FindControl("txtDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdMotivo.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la definición.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionMotivo(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [DNC_MOTIVOS_CT] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[definicion] ='" & strDefinicion & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdMotivo.EditIndex = -1
            Call obtCatalogoMotivo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdMotivo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdMotivo.PageIndexChanging
        Try
            grdMotivo.ShowFooter = True
            grdMotivo.EditIndex = -1
            grdMotivo.PageIndex = e.NewPageIndex
            grdMotivo.DataBind()
            Call obtCatalogoMotivo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarMotivo_Click(sender As Object, e As EventArgs)
        Call insDescripcionMotivo()
    End Sub

    Private Sub grdMotivo_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdMotivo.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdMotivo.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_MOTIVOS_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiMotivo(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdMotivo.EditIndex = -1
            grdMotivo.ShowFooter = True
            Call obtCatalogoMotivo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Objetivos Corporativos"
    'obtiene el catalogo 
    Public Sub obtCatalogoObjetivosCorporativos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_OBJETIVO_CORPORATIVOS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdObjetivosCorporativos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdObjetivosCorporativos.DataBind()

            If grdObjetivosCorporativos.Rows.Count = 0 Then
                Call insFilaVaciaObjetivoCorporativo()
                grdObjetivosCorporativos.Rows(0).Visible = False

            Else
                grdObjetivosCorporativos.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdObjetivosCorporativos.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdObjetivosCorporativos.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdObjetivosCorporativos.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdObjetivosCorporativos.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdObjetivosCorporativos.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdObjetivosCorporativos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdObjetivosCorporativos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            Call obtCatalogoObjetivosDet()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaObjetivoCorporativo()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdObjetivosCorporativos.DataSource = dt.DefaultView
        grdObjetivosCorporativos.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionObjetivo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdObjetivosCorporativos.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdObjetivosCorporativos.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionObjetivoCorporativo(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DNC_OBJETIVO_CORPORATIVOS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoObjetivosCorporativos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdObjetivosCorporativos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdObjetivosCorporativos.RowCancelingEdit
        grdObjetivosCorporativos.ShowFooter = True
        grdObjetivosCorporativos.EditIndex = -1
        Call obtCatalogoObjetivosCorporativos()
    End Sub

    'TOOLTIPS
    Private Sub grdObjetivosCorporativos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdObjetivosCorporativos.RowDataBound

        For i As Integer = 0 To grdObjetivosCorporativos.Rows.Count - 1

            Dim btnEditar As LinkButton = grdObjetivosCorporativos.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdObjetivosCorporativos.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdObjetivosCorporativos.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdObjetivosCorporativos.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdObjetivosCorporativos.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdObjetivosCorporativos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdObjetivosCorporativos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdObjetivosCorporativos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaObjetivoCorporativos(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DNC_OBJETIVO_CORPORATIVOS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdObjetivosCorporativos.EditIndex = -1
            grdObjetivosCorporativos.ShowFooter = True
            Call obtCatalogoObjetivosCorporativos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaObjetivoCorporativos(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from DNC_GESTION_CURSOS_TB where fk_id_objetivo_corporativo=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdObjetivosCorporativos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdObjetivosCorporativos.RowEditing
        grdObjetivosCorporativos.ShowFooter = False
        grdObjetivosCorporativos.EditIndex = e.NewEditIndex
        Call obtCatalogoObjetivosCorporativos()
    End Sub
    'actualiza la descripcion
    Private Sub grdObjetivosCorporativos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdObjetivosCorporativos.RowUpdating
        grdObjetivosCorporativos.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdObjetivosCorporativos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdObjetivosCorporativos.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdObjetivosCorporativos.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionObjetivoCorporativo(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_OBJETIVO_CORPORATIVOS_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdObjetivosCorporativos.EditIndex = -1
            Call obtCatalogoObjetivosCorporativos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarObjetivosCor_Click(sender As Object, e As EventArgs)
        Call insDescripcionObjetivo()
    End Sub

    Protected Sub grdObjetivosCorporativos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdObjetivosCorporativos.PageIndexChanging
        grdObjetivosCorporativos.ShowFooter = True
        grdObjetivosCorporativos.PageIndex = e.NewPageIndex
        grdObjetivosCorporativos.DataBind()
        Call obtCatalogoObjetivosCorporativos()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionObjetivoCorporativo(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_OBJETIVO_CORPORATIVOS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Objetivo Corporativo Detllar"
    ''obtiene el catalogo 
    Public Sub obtCatalogoObjetivosDet()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT  ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdObjetivosCorpDet.DataSource = dsCatalogo.Tables(0).DefaultView
            grdObjetivosCorpDet.DataBind()

            If grdObjetivosCorpDet.Rows.Count = 0 Then
                Call insFilaVaObjetivosDet()
                grdObjetivosCorpDet.Rows(0).Visible = False


            Else
                grdObjetivosCorpDet.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdObjetivosCorpDet.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim ddlObjetivo As DropDownList
                Dim btnEditar As LinkButton = grdObjetivosCorpDet.Rows(i).Controls(5).Controls(0)
                iId = DirectCast(grdObjetivosCorpDet.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlObjetivo = grdObjetivosCorpDet.Rows(i).Cells(1).FindControl("ddlObjetivoDet")

                    'obtiene provedor
                    Call obtddlObjetivosCorp(ddlObjetivo)

                    ddlEstatus = grdObjetivosCorpDet.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlObjetivo.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(1).ToString
                        End If
                    Next
                Else
                    'Estatus
                    Dim lblEstatus As New Label
                    lblEstatus = grdObjetivosCorpDet.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")

                    'Objetivo
                    Dim lblObjetivo As New Label
                    lblObjetivo = grdObjetivosCorpDet.Rows(i).FindControl("lblObjetivoDet")
                    lblObjetivo.Text = obtTextoObjetivosCorp(lblObjetivo.Text)
                End If


            Next

            Dim ddlAgreObjetivoDet As DropDownList
            ddlAgreObjetivoDet = grdObjetivosCorpDet.FooterRow.FindControl("ddlAgreObjetivoDet")
            Call obtddlObjetivosCorp(ddlAgreObjetivoDet)
            odbConexion.Close()

            'competencia vinculada
            For iFil As Integer = 0 To grdObjetivosCorpDet.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdObjetivosCorpDet.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'Obtiene el catalogo de proveedores
    Public Sub obtddlObjetivosCorp(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT ID,descripcion FROM DNC_OBJETIVO_CORPORATIVOS_CT where estatus=1 ORDER BY descripcion"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoObjetivosCorp(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_OBJETIVO_CORPORATIVOS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function validaDesObjetivosCorp(strNombre As String, strDireccion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT where (descripcion='" & strNombre & "' and direccion='" & strDireccion & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaObjetivosDet()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("fk_id_objetivo_corporativo"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("direccion"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("fk_id_objetivo_corporativo") = ""
        dr("descripcion") = ""
        dr("direccion") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdObjetivosCorpDet.DataSource = dt.DefaultView
        grdObjetivosCorpDet.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDesObjetivosCorp()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strDireccion As String = ""
        Dim strEstatus As String = ""
        Dim strObjetivo As String = ""
        Try
            strObjetivo = (DirectCast(grdObjetivosCorpDet.FooterRow.FindControl("ddlAgreObjetivoDet"), DropDownList).Text)
            strDescripcion = (DirectCast(grdObjetivosCorpDet.FooterRow.FindControl("txtAgregaDescripcion"), TextBox).Text)
            strDireccion = (DirectCast(grdObjetivosCorpDet.FooterRow.FindControl("txtAgregarDireccion"), TextBox).Text)
            strEstatus = (DirectCast(grdObjetivosCorpDet.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strObjetivo = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe haber Objetivos Corporativos para registrar el Detalle.');</script>", False)
                Exit Sub
            End If
            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strDireccion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Dirección.');</script>", False)
                Exit Sub
            End If


            If validaDesObjetivosCorp(strDescripcion, strDireccion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción para esa Dirección.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT (fk_id_objetivo_corporativo,descripcion,direccion,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES (" & strObjetivo & ", '" & strDescripcion & "','" & strDireccion & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoObjetivosDet()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdObjetivosCorpDet_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdObjetivosCorpDet.RowCancelingEdit
        grdObjetivosCorpDet.ShowFooter = True
        grdObjetivosCorpDet.EditIndex = -1
        Call obtCatalogoObjetivosDet()
    End Sub

    Private Sub grdObjetivosCorpDet_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdObjetivosCorpDet.RowDataBound

        For i As Integer = 0 To grdObjetivosCorpDet.Rows.Count - 1
            Dim btnEditar As LinkButton = grdObjetivosCorpDet.Rows(i).Controls(5).Controls(0)
            Dim btnEliminar As LinkButton = grdObjetivosCorpDet.Rows(i).Controls(6).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdObjetivosCorpDet.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdObjetivosCorpDet.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdObjetivosCorpDet.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiObjetivoCorp(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [DNC_GESTION_CURSOS_TB] WHERE [fk_id_objetivo_corporativo]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdObjetivosCorpDet_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdObjetivosCorpDet.RowEditing
        grdObjetivosCorpDet.ShowFooter = False
        grdObjetivosCorpDet.EditIndex = e.NewEditIndex
        Call obtCatalogoObjetivosDet()
    End Sub
    'actualiza la descripcion
    Private Sub grdObjetivosCorpDet_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdObjetivosCorpDet.RowUpdating
        grdObjetivosCorpDet.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strDireccion, strEstatus As String
        Dim strObjetivo As String = ""
        Try
            strId = DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros
            strObjetivo = (DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("ddlObjetivoDet"), DropDownList).Text)
            strDescripcion = (DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDireccion = (DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("txtDireccion"), TextBox).Text)
            strEstatus = (DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If

            If strDireccion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Dirección.');</script>", False)
                Exit Sub
            End If

            If validaDesObjetivosCorp(strDescripcion, strDireccion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción para esa Dirección.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[direccion] ='" & strDireccion & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ", fk_id_objetivo_corporativo=" & strObjetivo & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdObjetivosCorpDet.EditIndex = -1
            Call obtCatalogoObjetivosDet()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdObjetivosCorpDet_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdObjetivosCorpDet.PageIndexChanging
        Try
            grdObjetivosCorpDet.ShowFooter = True
            grdObjetivosCorpDet.EditIndex = -1
            grdObjetivosCorpDet.PageIndex = e.NewPageIndex
            grdObjetivosCorpDet.DataBind()
            Call obtCatalogoObjetivosDet()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarObjetivosDet_Click(sender As Object, e As EventArgs)
        Call insDesObjetivosCorp()
    End Sub

    Private Sub grdObjetivosCorpDet_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdObjetivosCorpDet.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdObjetivosCorpDet.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiObjetivoCorp(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdObjetivosCorpDet.EditIndex = -1
            grdObjetivosCorpDet.ShowFooter = True
            Call obtCatalogoObjetivosDet()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region

#Region "Grid Tipo Indicador"
    ''obtiene el catalogo 
    Public Sub obtCatalogoTipoIndicador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_TIPO_INDICADOR_CT  ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTipoIndicador.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTipoIndicador.DataBind()

            If grdTipoIndicador.Rows.Count = 0 Then
                Call insFilaVaciaTipoIndicador()
                grdTipoIndicador.Rows(0).Visible = False


            Else
                grdTipoIndicador.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdTipoIndicador.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdTipoIndicador.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdTipoIndicador.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTipoIndicador.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTipoIndicador.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'competencia vinculada
            For iFil As Integer = 0 To grdTipoIndicador.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTipoIndicador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTipoIndicador(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_TIPO_INDICADOR_CT where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoIndicador()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("definicion"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("definicion") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTipoIndicador.DataSource = dt.DefaultView
        grdTipoIndicador.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionTipoIndicador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strDefinicion As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdTipoIndicador.FooterRow.FindControl("txtAgregaDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdTipoIndicador.FooterRow.FindControl("txtAgregarDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoIndicador.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Definición.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionTipoIndicador(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO DNC_TIPO_INDICADOR_CT (descripcion,definicion,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strDefinicion & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoTipoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTipoIndicador_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTipoIndicador.RowCancelingEdit
        grdTipoIndicador.ShowFooter = True
        grdTipoIndicador.EditIndex = -1
        Call obtCatalogoTipoIndicador()
    End Sub

    Private Sub grdTipoIndicador_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTipoIndicador.RowDataBound

        For i As Integer = 0 To grdTipoIndicador.Rows.Count - 1
            Dim btnEditar As LinkButton = grdTipoIndicador.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdTipoIndicador.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdTipoIndicador.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdTipoIndicador.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdTipoIndicador.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiTipoIndicador(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [DNC_INDICADORES_CT] WHERE [fk_id_tipo_indicador]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdTipoIndicador_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTipoIndicador.RowEditing
        grdTipoIndicador.ShowFooter = False
        grdTipoIndicador.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoIndicador()
    End Sub
    'actualiza la descripcion
    Private Sub grdTipoIndicador_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTipoIndicador.RowUpdating
        grdTipoIndicador.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strDefinicion, strEstatus As String
        Try
            strId = DirectCast(grdTipoIndicador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdTipoIndicador.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdTipoIndicador.Rows(e.RowIndex).FindControl("txtDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoIndicador.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la definición.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoIndicador(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [DNC_TIPO_INDICADOR_CT] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[definicion] ='" & strDefinicion & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoIndicador.EditIndex = -1
            Call obtCatalogoTipoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdTipoIndicador_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTipoIndicador.PageIndexChanging
        Try
            grdTipoIndicador.ShowFooter = True
            grdTipoIndicador.EditIndex = -1
            grdTipoIndicador.PageIndex = e.NewPageIndex
            grdTipoIndicador.DataBind()
            Call obtCatalogoTipoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarTipoIndicador_Click(sender As Object, e As EventArgs)
        Call insDescripcionTipoIndicador()
    End Sub

    Private Sub grdTipoIndicador_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTipoIndicador.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTipoIndicador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_TIPO_INDICADOR_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiTipoIndicador(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Indicador.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdTipoIndicador.EditIndex = -1
            grdTipoIndicador.ShowFooter = True
            Call obtCatalogoTipoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Indicador"
    ''obtiene el catalogo 
    Public Sub obtCatalogoIndicador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_INDICADORES_CT  ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdIndicador.DataSource = dsCatalogo.Tables(0).DefaultView
            grdIndicador.DataBind()

            If grdIndicador.Rows.Count = 0 Then
                Call insFilaVaciaIndicador()
                grdIndicador.Rows(0).Visible = False


            Else
                grdIndicador.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdIndicador.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim ddlTipoIndicador As DropDownList
                Dim btnEditar As LinkButton = grdIndicador.Rows(i).Controls(5).Controls(0)
                iId = DirectCast(grdIndicador.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlTipoIndicador = grdIndicador.Rows(i).Cells(1).FindControl("ddlTipoIndicador")

                    'obtiene provedor
                    Call obtddlTipoIndicadores(ddlTipoIndicador)

                    ddlEstatus = grdIndicador.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlTipoIndicador.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(1).ToString
                        End If
                    Next
                Else
                    'Estatus
                    Dim lblEstatus As New Label
                    lblEstatus = grdIndicador.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")

                    'Tipo de Empleado
                    Dim lblTipoIndicador As New Label
                    lblTipoIndicador = grdIndicador.Rows(i).FindControl("lblTipoIndicador")
                    lblTipoIndicador.Text = obtTextoTipoIndicadores(lblTipoIndicador.Text)
                End If


            Next

            Dim ddlAgreTipoIndicador As DropDownList
            ddlAgreTipoIndicador = grdIndicador.FooterRow.FindControl("ddlAgreTipoIndicador")
            Call obtddlTipoIndicadores(ddlAgreTipoIndicador)
            odbConexion.Close()

            'competencia vinculada
            For iFil As Integer = 0 To grdIndicador.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdIndicador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'Obtiene el catalogo de proveedores
    Public Sub obtddlTipoIndicadores(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT ID,descripcion FROM DNC_TIPO_INDICADOR_CT  ORDER BY descripcion"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoTipoIndicadores(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_TIPO_INDICADOR_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function validaDescripcionIndicador(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_INDICADORES_CT where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaIndicador()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("definicion"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("definicion") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdIndicador.DataSource = dt.DefaultView
        grdIndicador.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub validaDescripcionIndicador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strDefinicion As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdIndicador.FooterRow.FindControl("txtAgregaDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdIndicador.FooterRow.FindControl("txtAgregarDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdIndicador.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Definición.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionIndicador(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO DNC_INDICADORES_CT (descripcion,definicion,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strDefinicion & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdIndicador_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdIndicador.RowCancelingEdit
        grdIndicador.ShowFooter = True
        grdIndicador.EditIndex = -1
        Call obtCatalogoIndicador()
    End Sub

    Private Sub grdIndicador_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdIndicador.RowDataBound

        For i As Integer = 0 To grdIndicador.Rows.Count - 1
            Dim btnEditar As LinkButton = grdIndicador.Rows(i).Controls(5).Controls(0)
            Dim btnEliminar As LinkButton = grdIndicador.Rows(i).Controls(6).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdIndicador.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdIndicador.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdIndicador.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiIndicador(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [DNC_INDICADORES_CT] WHERE [fk_id_tipo_indicador]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdIndicador_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdIndicador.RowEditing
        grdIndicador.ShowFooter = False
        grdIndicador.EditIndex = e.NewEditIndex
        Call obtCatalogoIndicador()
    End Sub
    'actualiza la descripcion
    Private Sub grdIndicador_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdIndicador.RowUpdating
        grdIndicador.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strDefinicion, strEstatus As String
        Try
            strId = DirectCast(grdIndicador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdIndicador.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDefinicion = (DirectCast(grdIndicador.Rows(e.RowIndex).FindControl("txtDefinicion"), TextBox).Text)
            strEstatus = (DirectCast(grdIndicador.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If

            If strDefinicion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la definición.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionIndicador(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [DNC_INDICADORES_CT] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[definicion] ='" & strDefinicion & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdIndicador.EditIndex = -1
            Call obtCatalogoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdIndicador_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdIndicador.PageIndexChanging
        Try
            grdIndicador.ShowFooter = True
            grdIndicador.EditIndex = -1
            grdIndicador.PageIndex = e.NewPageIndex
            grdIndicador.DataBind()
            Call obtCatalogoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarIndicador_Click(sender As Object, e As EventArgs)
        Call validaDescripcionIndicador()
    End Sub

    Private Sub grdIndicador_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdIndicador.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdIndicador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_INDICADORES_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiIndicador(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Indicador.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdIndicador.EditIndex = -1
            grdIndicador.ShowFooter = True
            Call obtCatalogoIndicador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Estatus DNC"
    'obtiene el catalogo 
    Public Sub obtCatalogoEstatus()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_ESTATUS_TB ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdEstatusDNC.DataSource = dsCatalogo.Tables(0).DefaultView
            grdEstatusDNC.DataBind()

            If grdEstatusDNC.Rows.Count = 0 Then
                Call insFilaVaciaEstatus()
                grdEstatusDNC.Rows(0).Visible = False

            Else
                grdEstatusDNC.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdEstatusDNC.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdEstatusDNC.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdEstatusDNC.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdEstatusDNC.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdEstatusDNC.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'habilidades dina
            For iFil As Integer = 0 To grdEstatusDNC.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdEstatusDNC.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaEstatus()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdEstatusDNC.DataSource = dt.DefaultView
        grdEstatusDNC.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionEstatus()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdEstatusDNC.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdEstatusDNC.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatus(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DNC_ESTATUS_TB (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoEstatus()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdEstatusDNC_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdEstatusDNC.RowCancelingEdit
        grdEstatusDNC.ShowFooter = True
        grdEstatusDNC.EditIndex = -1
        Call obtCatalogoEstatus()
    End Sub

    'TOOLTIPS
    Private Sub grdEstatusDNC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEstatusDNC.RowDataBound

        For i As Integer = 0 To grdEstatusDNC.Rows.Count - 1

            Dim btnEditar As LinkButton = grdEstatusDNC.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdEstatusDNC.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar  " + DirectCast(grdEstatusDNC.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdEstatusDNC.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar  " + DirectCast(grdEstatusDNC.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdEstatusDNC_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdEstatusDNC.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdEstatusDNC.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaEstatus(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DNC_ESTATUS_TB WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusDNC.EditIndex = -1
            grdEstatusDNC.ShowFooter = True
            Call obtCatalogoEstatus()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaEstatus(odbConexion As OleDbConnection, id_Habilidad As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from DNC_GESTION_CURSOS_TB where fk_id_estatus=" & id_Habilidad.ToString
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

    'habilita el modo edicion
    Private Sub grdEstatusDNC_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdEstatusDNC.RowEditing
        grdEstatusDNC.ShowFooter = False
        grdEstatusDNC.EditIndex = e.NewEditIndex
        Call obtCatalogoEstatus()
    End Sub
    'actualiza la descripcion
    Private Sub grdEstatusDNC_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdEstatusDNC.RowUpdating
        grdEstatusDNC.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdEstatusDNC.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdEstatusDNC.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdEstatusDNC.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatus(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_ESTATUS_TB " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusDNC.EditIndex = -1
            Call obtCatalogoEstatus()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarEstatus_Click(sender As Object, e As EventArgs)
        Call insDescripcionEstatus()
    End Sub

    Protected Sub grdEstatusDNC_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEstatusDNC.PageIndexChanging
        grdEstatusDNC.ShowFooter = True
        grdEstatusDNC.PageIndex = e.NewPageIndex
        grdEstatusDNC.DataBind()
        Call obtCatalogoEstatus()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionEstatus(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_ESTATUS_TB where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Modalidad"
    'obtiene el catalogo 
    Public Sub obtCatalogoModalidadCursos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_MODALIDAD_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdModalidadCurso.DataSource = dsCatalogo.Tables(0).DefaultView
            grdModalidadCurso.DataBind()

            If grdModalidadCurso.Rows.Count = 0 Then
                Call insFilaVaciaModalidadCursos()
                grdModalidadCurso.Rows(0).Visible = False

            Else
                grdModalidadCurso.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdModalidadCurso.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdModalidadCurso.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdModalidadCurso.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdModalidadCurso.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdModalidadCurso.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'habilidades dina
            For iFil As Integer = 0 To grdModalidadCurso.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdModalidadCurso.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaModalidadCursos()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdModalidadCurso.DataSource = dt.DefaultView
        grdModalidadCurso.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionModalidadCursos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdModalidadCurso.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdModalidadCurso.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadCurso(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DNC_MODALIDAD_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoModalidadCursos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdModalidadCurso_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdModalidadCurso.RowCancelingEdit
        grdModalidadCurso.ShowFooter = True
        grdModalidadCurso.EditIndex = -1
        Call obtCatalogoModalidadCursos()
    End Sub

    'TOOLTIPS
    Private Sub grdModalidadCurso_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdModalidadCurso.RowDataBound

        For i As Integer = 0 To grdModalidadCurso.Rows.Count - 1

            Dim btnEditar As LinkButton = grdModalidadCurso.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdModalidadCurso.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadCurso.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdModalidadCurso.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadCurso.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdModalidadCurso_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdModalidadCurso.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdModalidadCurso.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaModalidadCurso(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DNC_MODALIDAD_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadCurso.EditIndex = -1
            grdModalidadCurso.ShowFooter = True
            Call obtCatalogoModalidadCursos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaModalidadCurso(odbConexion As OleDbConnection, id_Habilidad As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from DNC_CURSOS_TB where modalidad=" & id_Habilidad.ToString
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

    'habilita el modo edicion
    Private Sub grdModalidadCurso_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdModalidadCurso.RowEditing
        grdModalidadCurso.ShowFooter = False
        grdModalidadCurso.EditIndex = e.NewEditIndex
        Call obtCatalogoModalidadCursos()
    End Sub
    'actualiza la descripcion
    Private Sub grdModalidadCurso_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdModalidadCurso.RowUpdating
        grdModalidadCurso.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdModalidadCurso.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdModalidadCurso.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdModalidadCurso.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadCurso(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_MODALIDAD_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadCurso.EditIndex = -1
            Call obtCatalogoModalidadCursos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarModalidadCurso_Click(sender As Object, e As EventArgs)
        Call insDescripcionModalidadCursos()
    End Sub
    Protected Sub grdModalidadCurso_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdModalidadCurso.PageIndexChanging
        grdModalidadCurso.ShowFooter = True
        grdModalidadCurso.PageIndex = e.NewPageIndex
        grdModalidadCurso.DataBind()
        Call obtCatalogoModalidadCursos()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionModalidadCurso(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_MODALIDAD_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
    '************************* - Catalogos de Becas - *******************************************
    Public Sub CargaCatalogosBecas()
        Call obtCatalogoTipoBeca()
        Call obtCatalogoPeriodoAsignacion()
        Call obtCatalogoTipoAsignacion()
        Call obtCatalogoModalidadEstudio()
        Call obtCatalogoTipoProyecto()
        Call obtCatalogoTipoEstatus()
        Call obtCatalogoModalidadPago()
        Call obtCatalogoEstatusPago()
        Call obtCatalogoConceptoPago()
    End Sub
    'Grid de Tipo Becas
#Region "Grid Tipo Becas"
    'obtiene el catalogo 
    Public Sub obtCatalogoTipoBeca()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_TIPO_BECA_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTipoBeca.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTipoBeca.DataBind()

            If grdTipoBeca.Rows.Count = 0 Then
                Call insFilaVaciaTipoBeca()
                grdTipoBeca.Rows(0).Visible = False

            Else
                grdTipoBeca.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdTipoBeca.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdTipoBeca.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdTipoBeca.Rows(i).Controls(4).Controls(1)
                iId = DirectCast(grdTipoBeca.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTipoBeca.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTipoBeca.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If

                If iId <= 6 Then
                    btnEditar.Visible = False
                    btnEliminar.Visible = False
                Else
                    btnEditar.Visible = True
                    btnEliminar.Visible = True
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdTipoBeca.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTipoBeca.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoBeca()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTipoBeca.DataSource = dt.DefaultView
        grdTipoBeca.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionTipoBeca()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdTipoBeca.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoBeca.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoBecas(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_TIPO_BECA_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoTipoBeca()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTipoBeca_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTipoBeca.RowCancelingEdit
        grdTipoBeca.ShowFooter = True
        grdTipoBeca.EditIndex = -1
        Call obtCatalogoTipoBeca()
    End Sub

    'TOOLTIPS
    Private Sub grdTipoBeca_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTipoBeca.RowDataBound

        For i As Integer = 0 To grdTipoBeca.Rows.Count - 1

            Dim btnEditar As LinkButton = grdTipoBeca.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdTipoBeca.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoBeca.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdTipoBeca.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoBeca.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdTipoBeca_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTipoBeca.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTipoBeca.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaTipoBeca(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_TIPO_BECA_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoBeca.EditIndex = -1
            grdTipoBeca.ShowFooter = True
            Call obtCatalogoTipoBeca()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaTipoBeca(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_tipo_beca=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdTipoBeca_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTipoBeca.RowEditing
        grdTipoBeca.ShowFooter = False
        grdTipoBeca.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoBeca()
    End Sub
    'actualiza la descripcion
    Private Sub grdTipoBeca_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTipoBeca.RowUpdating
        grdTipoBeca.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdTipoBeca.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdTipoBeca.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdTipoBeca.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoBecas(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_TIPO_BECA_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoBeca.EditIndex = -1
            Call obtCatalogoTipoBeca()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTipoBeca_Click(sender As Object, e As EventArgs)
        Call insDescripcionTipoBeca()
    End Sub

    Protected Sub grdTipoBeca_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTipoBeca.PageIndexChanging
        grdTipoBeca.ShowFooter = True
        grdTipoBeca.PageIndex = e.NewPageIndex
        grdTipoBeca.DataBind()
        Call obtCatalogoTipoBeca()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTipoBecas(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_TIPO_BECA_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
    'Grid Periodo Asignado
#Region "Grid Periodo Asignacion"
    'obtiene el catalogo 
    Public Sub obtCatalogoPeriodoAsignacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_PERIODO_ASIGNACION_BECA_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPeriodoAsignacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPeriodoAsignacion.DataBind()

            If grdPeriodoAsignacion.Rows.Count = 0 Then
                Call insFilaVaciaPeriodoAsignacion()
                grdPeriodoAsignacion.Rows(0).Visible = False

            Else
                grdPeriodoAsignacion.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdPeriodoAsignacion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus, ddlVerano As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdPeriodoAsignacion.Rows(i).Controls(4).Controls(0)

                iId = DirectCast(grdPeriodoAsignacion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlVerano = grdPeriodoAsignacion.Rows(i).Cells(2).FindControl("ddlVeranos")
                    ddlEstatus = grdPeriodoAsignacion.Rows(i).Cells(3).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                            ddlVerano.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdPeriodoAsignacion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")

                    Dim lblVerano As New Label
                    lblVerano = grdPeriodoAsignacion.Rows(i).FindControl("lblVeranos")
                    lblVerano.Text = IIf(lblVerano.Text = "Si", "Incluye", "No Incluye")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdPeriodoAsignacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPeriodoAsignacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaPeriodoAsignacion()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("incluye_veranos"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("incluye_veranos") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPeriodoAsignacion.DataSource = dt.DefaultView
        grdPeriodoAsignacion.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionPeriodoAsignacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Dim strIncluye As String = ""
        Try

            strDescripcion = (DirectCast(grdPeriodoAsignacion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdPeriodoAsignacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            strIncluye = (DirectCast(grdPeriodoAsignacion.FooterRow.FindControl("ddlAgreVeranos"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPeriodoAsignacion(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_PERIODO_ASIGNACION_BECA_CT (descripcion,estatus,fecha_creacion,usuario_creacion,incluye_veranos) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "','" & strIncluye & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPeriodoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdPeriodoAsignacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPeriodoAsignacion.RowCancelingEdit
        grdPeriodoAsignacion.ShowFooter = True
        grdPeriodoAsignacion.EditIndex = -1
        Call obtCatalogoPeriodoAsignacion()
    End Sub

    '
    Private Sub grdPeriodoAsignacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPeriodoAsignacion.RowDataBound

        For i As Integer = 0 To grdPeriodoAsignacion.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPeriodoAsignacion.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdPeriodoAsignacion.Rows(i).Controls(5).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPeriodoAsignacion.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdPeriodoAsignacion.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPeriodoAsignacion.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdPeriodoAsignacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPeriodoAsignacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPeriodoAsignacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaPeriodoAsignacion(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_PERIODO_ASIGNACION_BECA_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPeriodoAsignacion.EditIndex = -1
            grdPeriodoAsignacion.ShowFooter = True
            Call obtCatalogoPeriodoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaPeriodoAsignacion(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_periodo_asignacion=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdPeriodoAsignacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPeriodoAsignacion.RowEditing
        grdPeriodoAsignacion.ShowFooter = False
        grdPeriodoAsignacion.EditIndex = e.NewEditIndex
        Call obtCatalogoPeriodoAsignacion()
    End Sub
    'actualiza la descripcion
    Private Sub grdPeriodoAsignacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPeriodoAsignacion.RowUpdating
        grdPeriodoAsignacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Dim strIncluye As String = ""
        Try
            strId = DirectCast(grdPeriodoAsignacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdPeriodoAsignacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdPeriodoAsignacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text
            strIncluye = DirectCast(grdPeriodoAsignacion.Rows(e.RowIndex).FindControl("ddlVeranos"), DropDownList).Text
            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPeriodoAsignacion(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_PERIODO_ASIGNACION_BECA_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                         ",incluye_veranos='" & strIncluye & "'" & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPeriodoAsignacion.EditIndex = -1
            Call obtCatalogoPeriodoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarPeriodoAsignacion_Click(sender As Object, e As EventArgs)
        Call insDescripcionPeriodoAsignacion()
    End Sub

    Protected Sub grdPeriodoAsignacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPeriodoAsignacion.PageIndexChanging
        grdPeriodoAsignacion.ShowFooter = True
        grdPeriodoAsignacion.PageIndex = e.NewPageIndex
        grdPeriodoAsignacion.DataBind()
        Call obtCatalogoPeriodoAsignacion()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionPeriodoAsignacion(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_PERIODO_ASIGNACION_BECA_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
    ' tipo de asignacion
#Region "Grid Tipo Asignacion"
    'obtiene el catalogo 
    Public Sub obtCatalogoTipoAsignacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_TIPO_ASIGNACION_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTipoAsignacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTipoAsignacion.DataBind()

            If grdTipoAsignacion.Rows.Count = 0 Then
                Call insFilaVaciaTipoAsignacion()
                grdTipoAsignacion.Rows(0).Visible = False

            Else
                grdTipoAsignacion.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdTipoAsignacion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdTipoAsignacion.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdTipoAsignacion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTipoAsignacion.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTipoAsignacion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'formato
            For iFil As Integer = 0 To grdTipoAsignacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTipoAsignacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoAsignacion()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTipoAsignacion.DataSource = dt.DefaultView
        grdTipoAsignacion.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionTipoAsignacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdTipoAsignacion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoAsignacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoAsignacion(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_TIPO_ASIGNACION_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoTipoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTipoAsignacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTipoAsignacion.RowCancelingEdit
        grdTipoAsignacion.ShowFooter = True
        grdTipoAsignacion.EditIndex = -1
        Call obtCatalogoTipoAsignacion()
    End Sub

    'TOOLTIPS
    Private Sub grdTipoAsignacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTipoAsignacion.RowDataBound

        For i As Integer = 0 To grdTipoAsignacion.Rows.Count - 1

            Dim btnEditar As LinkButton = grdTipoAsignacion.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdTipoAsignacion.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoAsignacion.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdTipoAsignacion.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoAsignacion.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdTipoAsignacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTipoAsignacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTipoAsignacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaTipoAsignacion(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_TIPO_ASIGNACION_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoAsignacion.EditIndex = -1
            grdTipoAsignacion.ShowFooter = True
            Call obtCatalogoTipoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaTipoAsignacion(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where [fk_id_tipo_asignacion]=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdTipoAsignacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTipoAsignacion.RowEditing
        grdTipoAsignacion.ShowFooter = False
        grdTipoAsignacion.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoAsignacion()
    End Sub
    'actualiza la descripcion
    Private Sub grdTipoAsignacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTipoAsignacion.RowUpdating
        grdTipoAsignacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdTipoAsignacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdTipoAsignacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdTipoAsignacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoAsignacion(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_TIPO_ASIGNACION_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoAsignacion.EditIndex = -1
            Call obtCatalogoTipoAsignacion()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTipoAsignacion_Click(sender As Object, e As EventArgs)
        Call insDescripcionTipoAsignacion()
    End Sub

    Protected Sub grdTipoAsignacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTipoAsignacion.PageIndexChanging
        grdTipoAsignacion.ShowFooter = True
        grdTipoAsignacion.PageIndex = e.NewPageIndex
        grdTipoAsignacion.DataBind()
        Call obtCatalogoTipoAsignacion()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTipoAsignacion(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_TIPO_ASIGNACION_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Modalidad Estudio"
    'obtiene el catalogo 
    Public Sub obtCatalogoModalidadEstudio()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_MODALIDAD_ESTUDIOS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdModalidadEstudio.DataSource = dsCatalogo.Tables(0).DefaultView
            grdModalidadEstudio.DataBind()

            If grdModalidadEstudio.Rows.Count = 0 Then
                Call insFilaVaciaModalidad()
                grdModalidadEstudio.Rows(0).Visible = False

            Else
                grdModalidadEstudio.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdModalidadEstudio.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdModalidadEstudio.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdModalidadEstudio.Rows(i).Controls(4).Controls(1)

                iId = DirectCast(grdModalidadEstudio.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdModalidadEstudio.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdModalidadEstudio.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If

                If iId <= 2 Then
                    btnEditar.Visible = False
                    btnEliminar.Visible = False
                Else
                    btnEditar.Visible = True
                    btnEliminar.Visible = True
                End If

            Next

            'Formato
            For iFil As Integer = 0 To grdModalidadEstudio.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdModalidadEstudio.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaModalidad()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdModalidadEstudio.DataSource = dt.DefaultView
        grdModalidadEstudio.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insModalidadEstudio()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdModalidadEstudio.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdModalidadEstudio.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadEstudio(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_MODALIDAD_ESTUDIOS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoModalidadEstudio()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdModalidadEstudio_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdModalidadEstudio.RowCancelingEdit
        grdModalidadEstudio.ShowFooter = True
        grdModalidadEstudio.EditIndex = -1
        Call obtCatalogoModalidadEstudio()
    End Sub

    'TOOLTIPS
    Private Sub grdModalidadEstudio_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdModalidadEstudio.RowDataBound

        For i As Integer = 0 To grdModalidadEstudio.Rows.Count - 1

            Dim btnEditar As LinkButton = grdModalidadEstudio.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdModalidadEstudio.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadEstudio.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdModalidadEstudio.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadEstudio.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdModalidadEstudio_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdModalidadEstudio.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdModalidadEstudio.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaModalidadEstudio(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_MODALIDAD_ESTUDIOS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadEstudio.EditIndex = -1
            grdModalidadEstudio.ShowFooter = True
            Call obtCatalogoModalidadEstudio()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaModalidadEstudio(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_modalidad_estudio=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdModalidadEstudio_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdModalidadEstudio.RowEditing
        grdModalidadEstudio.ShowFooter = False
        grdModalidadEstudio.EditIndex = e.NewEditIndex
        Call obtCatalogoModalidadEstudio()
    End Sub
    'actualiza la descripcion
    Private Sub grdModalidadEstudio_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdModalidadEstudio.RowUpdating
        grdModalidadEstudio.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdModalidadEstudio.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdModalidadEstudio.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdModalidadEstudio.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadEstudio(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_MODALIDAD_ESTUDIOS_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadEstudio.EditIndex = -1
            Call obtCatalogoModalidadEstudio()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarModalidadEstudio_Click(sender As Object, e As EventArgs)
        Call insModalidadEstudio()
    End Sub

    Protected Sub grdModalidadEstudio_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdModalidadEstudio.PageIndexChanging
        grdModalidadEstudio.ShowFooter = True
        grdModalidadEstudio.PageIndex = e.NewPageIndex
        grdModalidadEstudio.DataBind()
        Call obtCatalogoModalidadEstudio()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionModalidadEstudio(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_MODALIDAD_ESTUDIOS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid tipo de proyecto"
    'obtiene el catalogo 
    Public Sub obtCatalogoTipoProyecto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_TIPO_PROYECTOS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTProyecto.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTProyecto.DataBind()

            If grdTProyecto.Rows.Count = 0 Then
                Call insFilaVaciaTipoProyecto()
                grdTProyecto.Rows(0).Visible = False

            Else
                grdTProyecto.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdTProyecto.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdTProyecto.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdTProyecto.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTProyecto.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTProyecto.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdTProyecto.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTProyecto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoProyecto()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTProyecto.DataSource = dt.DefaultView
        grdTProyecto.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insTipoProyecto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdTProyecto.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdTProyecto.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoProyecto(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_TIPO_PROYECTOS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoTipoProyecto()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTProyecto_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTProyecto.RowCancelingEdit
        grdTProyecto.ShowFooter = True
        grdTProyecto.EditIndex = -1
        Call obtCatalogoTipoProyecto()
    End Sub

    'TOOLTIPS
    Private Sub grdTProyecto_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTProyecto.RowDataBound

        For i As Integer = 0 To grdTProyecto.Rows.Count - 1

            Dim btnEditar As LinkButton = grdTProyecto.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdTProyecto.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTProyecto.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdTProyecto.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTProyecto.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdTProyecto_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTProyecto.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTProyecto.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaTipoProyecto(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_TIPO_PROYECTOS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTProyecto.EditIndex = -1
            grdTProyecto.ShowFooter = True
            Call obtCatalogoTipoProyecto()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaTipoProyecto(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_tipo_proyecto=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdTProyecto_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTProyecto.RowEditing
        grdTProyecto.ShowFooter = False
        grdTProyecto.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoProyecto()
    End Sub
    'actualiza la descripcion
    Private Sub grdTProyecto_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTProyecto.RowUpdating
        grdTProyecto.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdTProyecto.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdTProyecto.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdTProyecto.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTipoProyecto(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_TIPO_PROYECTOS_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTProyecto.EditIndex = -1
            Call obtCatalogoTipoProyecto()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTProyecto_Click(sender As Object, e As EventArgs)
        Call insTipoProyecto()
    End Sub
    Protected Sub grdTProyecto_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTProyecto.PageIndexChanging
        grdTProyecto.ShowFooter = True
        grdTProyecto.PageIndex = e.NewPageIndex
        grdTProyecto.DataBind()
        Call obtCatalogoModalidadEstudio()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTipoProyecto(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_TIPO_PROYECTOS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Tipo Estatus"
    'obtiene el catalogo 
    Public Sub obtCatalogoTipoEstatus()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_TIPO_ESTATUS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTipoEstatus.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTipoEstatus.DataBind()

            If grdTipoEstatus.Rows.Count = 0 Then
                Call insFilaVaciaTipoEstatus()
                grdTipoEstatus.Rows(0).Visible = False

            Else
                grdTipoEstatus.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdTipoEstatus.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdTipoEstatus.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdTipoEstatus.Rows(i).Controls(4).Controls(1)
                iId = DirectCast(grdTipoEstatus.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTipoEstatus.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTipoEstatus.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If

                If iId <= 6 Then
                    btnEditar.Visible = False
                    btnEliminar.Visible = False
                Else
                    btnEditar.Visible = True
                    btnEliminar.Visible = True
                End If
            Next

            'Formato
            For iFil As Integer = 0 To grdTipoEstatus.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTipoEstatus.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoEstatus()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTipoEstatus.DataSource = dt.DefaultView
        grdTipoEstatus.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insTipoEstatus()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdTipoEstatus.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoEstatus.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTEstatus(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_TIPO_ESTATUS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoTipoEstatus()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTipoEstatus_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTipoEstatus.RowCancelingEdit
        grdTipoEstatus.ShowFooter = True
        grdTipoEstatus.EditIndex = -1
        Call obtCatalogoTipoEstatus()
    End Sub

    'TOOLTIPS
    Private Sub grdTipoEstatus_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTipoEstatus.RowDataBound

        For i As Integer = 0 To grdTipoEstatus.Rows.Count - 1

            Dim btnEditar As LinkButton = grdTipoEstatus.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdTipoEstatus.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoEstatus.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdTipoEstatus.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdTipoEstatus.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdTipoEstatus_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTipoEstatus.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTipoEstatus.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaTipoEstatus(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_TIPO_ESTATUS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoEstatus.EditIndex = -1
            grdTipoEstatus.ShowFooter = True
            Call obtCatalogoTipoEstatus()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaTipoEstatus(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_tipo_estatus=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdTipoEstatus_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTipoEstatus.RowEditing
        grdTipoEstatus.ShowFooter = False
        grdTipoEstatus.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoEstatus()
    End Sub
    'actualiza la descripcion
    Private Sub grdTipoEstatus_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTipoEstatus.RowUpdating
        grdTipoEstatus.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdTipoEstatus.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdTipoEstatus.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdTipoEstatus.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionTEstatus(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_TIPO_ESTATUS_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoEstatus.EditIndex = -1
            Call obtCatalogoTipoEstatus()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTEstatus_Click(sender As Object, e As EventArgs)
        Call insTipoEstatus()
    End Sub

    Protected Sub grdTipoEstatus_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTipoEstatus.PageIndexChanging
        grdTipoEstatus.ShowFooter = True
        grdTipoEstatus.PageIndex = e.NewPageIndex
        grdTipoEstatus.DataBind()
        Call obtCatalogoTipoEstatus()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTEstatus(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_TIPO_ESTATUS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Modalidad Pago"
    'obtiene el catalogo 
    Public Sub obtCatalogoModalidadPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_MODALIDAD_PAGO_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdModalidadPago.DataSource = dsCatalogo.Tables(0).DefaultView
            grdModalidadPago.DataBind()

            If grdModalidadPago.Rows.Count = 0 Then
                Call insFilaVaciaModalidadP()
                grdModalidadPago.Rows(0).Visible = False

            Else
                grdModalidadPago.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdModalidadPago.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdModalidadPago.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdModalidadPago.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdModalidadPago.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdModalidadPago.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Formato
            For iFil As Integer = 0 To grdModalidadPago.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdModalidadPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaModalidadP()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdModalidadPago.DataSource = dt.DefaultView
        grdModalidadPago.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insModalidadPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdModalidadPago.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdModalidadPago.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadP(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_MODALIDAD_PAGO_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoModalidadPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdModalidadPago_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdModalidadPago.RowCancelingEdit
        grdModalidadPago.ShowFooter = True
        grdModalidadPago.EditIndex = -1
        Call obtCatalogoModalidadPago()
    End Sub

    'TOOLTIPS
    Private Sub grdModalidadPago_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdModalidadPago.RowDataBound

        For i As Integer = 0 To grdModalidadPago.Rows.Count - 1

            Dim btnEditar As LinkButton = grdModalidadPago.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdModalidadPago.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadPago.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdModalidadPago.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdModalidadPago.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdModalidadPago_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdModalidadPago.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdModalidadPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaModalidadPago(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM BECAS_MODALIDAD_PAGO_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadPago.EditIndex = -1
            grdModalidadPago.ShowFooter = True
            Call obtCatalogoModalidadPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaModalidadPago(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_modalidad_pago=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdModalidadPago_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdModalidadPago.RowEditing
        grdModalidadPago.ShowFooter = False
        grdModalidadPago.EditIndex = e.NewEditIndex
        Call obtCatalogoModalidadPago()
    End Sub
    'actualiza la descripcion
    Private Sub grdModalidadPago_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdModalidadPago.RowUpdating
        grdModalidadPago.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdModalidadPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdModalidadPago.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdModalidadPago.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionModalidadP(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_MODALIDAD_PAGO_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdModalidadPago.EditIndex = -1
            Call obtCatalogoModalidadPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarModalidadP_Click(sender As Object, e As EventArgs)
        Call insModalidadPago()
    End Sub

    Protected Sub grdModalidadPago_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdModalidadPago.PageIndexChanging
        grdModalidadPago.ShowFooter = True
        grdModalidadPago.PageIndex = e.NewPageIndex
        grdModalidadPago.DataBind()
        Call obtCatalogoModalidadPago()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionModalidadP(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_MODALIDAD_PAGO_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Estatus Pago"
    'obtiene el catalogo 
    Public Sub obtCatalogoEstatusPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_ESTATUS_PAGO_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdEstatusPago.DataSource = dsCatalogo.Tables(0).DefaultView
            grdEstatusPago.DataBind()

            If grdEstatusPago.Rows.Count = 0 Then
                Call insFilaVaciaEstatusPago()
                grdEstatusPago.Rows(0).Visible = False

            Else
                grdEstatusPago.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdEstatusPago.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdEstatusPago.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdEstatusPago.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdEstatusPago.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdEstatusPago.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Formato
            For iFil As Integer = 0 To grdEstatusPago.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdEstatusPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaEstatusPago()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdEstatusPago.DataSource = dt.DefaultView
        grdEstatusPago.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insEstatusPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdEstatusPago.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdEstatusPago.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatusPago(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_ESTATUS_PAGO_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoEstatusPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdEstatusPago_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdEstatusPago.RowCancelingEdit
        grdEstatusPago.ShowFooter = True
        grdEstatusPago.EditIndex = -1
        Call obtCatalogoEstatusPago()
    End Sub

    'TOOLTIPS
    Private Sub grdEstatusPago_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEstatusPago.RowDataBound

        For i As Integer = 0 To grdEstatusPago.Rows.Count - 1

            Dim btnEditar As LinkButton = grdEstatusPago.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdEstatusPago.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEstatusPago.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdEstatusPago.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEstatusPago.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdEstatusPago_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdEstatusPago.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdEstatusPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaEstatusPago(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Beca.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM BECAS_ESTATUS_PAGO_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusPago.EditIndex = -1
            grdEstatusPago.ShowFooter = True
            Call obtCatalogoEstatusPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaEstatusPago(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_estatus_pago=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdEstatusPago_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdEstatusPago.RowEditing
        grdEstatusPago.ShowFooter = False
        grdEstatusPago.EditIndex = e.NewEditIndex
        Call obtCatalogoEstatusPago()
    End Sub
    'actualiza la descripcion
    Private Sub grdEstatusPago_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdEstatusPago.RowUpdating
        grdEstatusPago.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdEstatusPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdEstatusPago.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdEstatusPago.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatusPago(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_ESTATUS_PAGO_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusPago.EditIndex = -1
            Call obtCatalogoEstatusPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarEstatusPago_Click(sender As Object, e As EventArgs)
        Call insEstatusPago()
    End Sub

    Protected Sub grdEstatusPago_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEstatusPago.PageIndexChanging
        grdEstatusPago.ShowFooter = True
        grdEstatusPago.PageIndex = e.NewPageIndex
        grdEstatusPago.DataBind()
        Call obtCatalogoEstatusPago()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionEstatusPago(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_ESTATUS_PAGO_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Grid Concepto Pago"
    'obtiene el catalogo 
    Public Sub obtCatalogoConceptoPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_CONCEPTO_PAGO_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdConceptoPago.DataSource = dsCatalogo.Tables(0).DefaultView
            grdConceptoPago.DataBind()

            If grdConceptoPago.Rows.Count = 0 Then
                Call insFilaVaciaConceptoPago()
                grdConceptoPago.Rows(0).Visible = False

            Else
                grdConceptoPago.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdConceptoPago.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdConceptoPago.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdConceptoPago.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdConceptoPago.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdConceptoPago.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Formato
            For iFil As Integer = 0 To grdConceptoPago.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdConceptoPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaConceptoPago()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdConceptoPago.DataSource = dt.DefaultView
        grdConceptoPago.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insConceptoPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdConceptoPago.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdConceptoPago.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionConceptoPago(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_CONCEPTO_PAGO_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoConceptoPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    Private Sub grdConceptoPago_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdConceptoPago.RowCancelingEdit
        grdConceptoPago.ShowFooter = True
        grdConceptoPago.EditIndex = -1
        Call obtCatalogoConceptoPago()
    End Sub

    'TOOLTIPS
    Private Sub grdConceptoPago_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdConceptoPago.RowDataBound

        For i As Integer = 0 To grdConceptoPago.Rows.Count - 1

            Dim btnEditar As LinkButton = grdConceptoPago.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdConceptoPago.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdConceptoPago.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdConceptoPago.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdConceptoPago.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingBecas').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdConceptoPago_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdConceptoPago.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdConceptoPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaConceptoPago(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a una Becas.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM BECAS_CONCEPTO_PAGO_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdConceptoPago.EditIndex = -1
            grdConceptoPago.ShowFooter = True
            Call obtCatalogoConceptoPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaConceptoPago(odbConexion As OleDbConnection, id_Medicion As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from BECAS_GESTION_BECAS_TB where fk_id_concepto_pago]=" & id_Medicion.ToString
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

    'habilita el modo edicion
    Private Sub grdConceptoPago_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdConceptoPago.RowEditing
        grdConceptoPago.ShowFooter = False
        grdConceptoPago.EditIndex = e.NewEditIndex
        Call obtCatalogoConceptoPago()
    End Sub
    'actualiza la descripcion
    Private Sub grdConceptoPago_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdConceptoPago.RowUpdating
        grdConceptoPago.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdConceptoPago.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdConceptoPago.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdConceptoPago.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionConceptoPago(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_CONCEPTO_PAGO_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdConceptoPago.EditIndex = -1
            Call obtCatalogoConceptoPago()
        Catch ex As Exception
            lblErrorBecas.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarConceptoPago_Click(sender As Object, e As EventArgs)
        Call insConceptoPago()
    End Sub

    Protected Sub grdConceptoPago_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdConceptoPago.PageIndexChanging
        grdConceptoPago.ShowFooter = True
        grdConceptoPago.PageIndex = e.NewPageIndex
        grdConceptoPago.DataBind()
        Call obtCatalogoConceptoPago()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionConceptoPago(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_CONCEPTO_PAGO_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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

#Region "Comportamientos"
    Public Sub inicialiciaTabs()
        'Tabs DNC
        hdIdTabDnc.Value = 1
        hdIdTabBecas.Value = 1
        hdIdTabBecasIngles.Value = 1
        hdIdGestionCapacitacion.Value = 1
    End Sub
    Public Sub comportamientos()
        'Comportamientos de DNC
        Call remueveEstilosTabDnc()
        Call AsignaEstilosTabDnc()
        Call AsignaEstiloTabDncActive()
        'Comportamientos de Becas
        Call remueveEstilosTabBecas()
        Call AsignaEstilosTabBecas()
        Call AsignaEstiloTabBecasActive()
        'comportamientos de becas de Ingles
        Call remueveEstilosTabBecasIngles()
        Call AsignaEstilosTabBecasIngles()
        Call AsignaEstiloTabBecasInglesActive()
        'Gestion de la capacitacion
        Call remueveEstilosTabGestionCap()
        Call AsignaEstilosTabGestionCaps()
        Call AsignaEstiloTabGestionCapActive()
        Call formatoGrids()
    End Sub
    'asigna la propiedad de Active al Tab
    Public Sub AsignaEstiloTabDncActive()
        'asigna estatus active
        If hdIdTabDnc.Value = 1 Then
            tabDnc_1.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc1.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 2 Then
            tabDnc_2.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc2.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 3 Then
            tabDnc_3.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc3.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 4 Then
            tabDnc_4.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc4.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 5 Then
            tabDnc_5.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc5.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 6 Then
            tabDnc_6.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc6.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 7 Then
            tabDnc_7.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc7.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 8 Then
            tabDnc_8.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc8.Attributes.Add("class", "active")
        ElseIf hdIdTabDnc.Value = 9 Then
            tabDnc_9.Attributes.Add("class", "tab-pane active") '
            lnkTabDnc9.Attributes.Add("class", "active")
        End If
    End Sub
    'remueva los estyilos de las tabs
    Public Sub remueveEstilosTabDnc()
        tabDnc_1.Attributes.Remove("class") '
        tabDnc_2.Attributes.Remove("class") '
        tabDnc_3.Attributes.Remove("class") '
        tabDnc_4.Attributes.Remove("class")
        tabDnc_5.Attributes.Remove("class")
        tabDnc_6.Attributes.Remove("class")
        tabDnc_7.Attributes.Remove("class")
        tabDnc_8.Attributes.Remove("class")
        tabDnc_9.Attributes.Remove("class")
        lnkTabDnc1.Attributes.Remove("class")
        lnkTabDnc2.Attributes.Remove("class")
        lnkTabDnc3.Attributes.Remove("class")
        lnkTabDnc4.Attributes.Remove("class")
        lnkTabDnc5.Attributes.Remove("class")
        lnkTabDnc6.Attributes.Remove("class")
        lnkTabDnc7.Attributes.Remove("class")
        lnkTabDnc8.Attributes.Remove("class")
        lnkTabDnc9.Attributes.Remove("class")
    End Sub
    'asigna comportamientos de Tab
    Public Sub AsignaEstilosTabDnc()
        tabDnc_1.Attributes.Add("class", "tab-pane") '
        tabDnc_2.Attributes.Add("class", "tab-pane") ''
        tabDnc_3.Attributes.Add("class", "tab-pane") '
        tabDnc_4.Attributes.Add("class", "tab-pane") '
        tabDnc_5.Attributes.Add("class", "tab-pane") '
        tabDnc_6.Attributes.Add("class", "tab-pane") '
        tabDnc_7.Attributes.Add("class", "tab-pane") '
        tabDnc_8.Attributes.Add("class", "tab-pane") '
        tabDnc_9.Attributes.Add("class", "tab-pane") '
    End Sub
    '********************* Becas ****************************************
    'remueva los estyilos de las tabs Becas
    Public Sub remueveEstilosTabBecas()
        tabBecas_1.Attributes.Remove("class") '
        tabBecas_2.Attributes.Remove("class") '
        tabBecas_3.Attributes.Remove("class") '
        tabBecas_4.Attributes.Remove("class")
        tabBecas_5.Attributes.Remove("class")
        tabBecas_6.Attributes.Remove("class")
        tabBecas_7.Attributes.Remove("class")
        tabBecas_8.Attributes.Remove("class")
        tabBecas_9.Attributes.Remove("class")
        lnkTabBecas1.Attributes.Remove("class")
        lnkTabBecas2.Attributes.Remove("class")
        lnkTabBecas3.Attributes.Remove("class")
        lnkTabBecas4.Attributes.Remove("class")
        lnkTabBecas5.Attributes.Remove("class")
        lnkTabBecas6.Attributes.Remove("class")
        lnkTabBecas7.Attributes.Remove("class")
        lnkTabBecas8.Attributes.Remove("class")
        lnkTabBecas9.Attributes.Remove("class")
    End Sub
    'asigna tab de Becas
    Public Sub AsignaEstilosTabBecas()
        tabBecas_1.Attributes.Add("class", "tab-pane") '
        tabBecas_2.Attributes.Add("class", "tab-pane") ''
        tabBecas_3.Attributes.Add("class", "tab-pane") '
        tabBecas_4.Attributes.Add("class", "tab-pane") '
        tabBecas_5.Attributes.Add("class", "tab-pane") '
        tabBecas_6.Attributes.Add("class", "tab-pane") '
        tabBecas_7.Attributes.Add("class", "tab-pane") '
        tabBecas_8.Attributes.Add("class", "tab-pane") '
        tabBecas_9.Attributes.Add("class", "tab-pane") '
    End Sub
    'asigna la propiedad de Active al Tab de Becas
    Public Sub AsignaEstiloTabBecasActive()
        'asigna estatus active
        If hdIdTabBecas.Value = 1 Then
            tabBecas_1.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas1.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 2 Then
            tabBecas_2.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas2.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 3 Then
            tabBecas_3.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas3.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 4 Then
            tabBecas_4.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas4.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 5 Then
            tabBecas_5.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas5.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 6 Then
            tabBecas_6.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas6.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 7 Then
            tabBecas_7.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas7.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 8 Then
            tabBecas_8.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas8.Attributes.Add("class", "active")
        ElseIf hdIdTabBecas.Value = 9 Then
            tabBecas_9.Attributes.Add("class", "tab-pane active") '
            lnkTabBecas9.Attributes.Add("class", "active")
        End If
    End Sub

    '********************* Becas  Ingles****************************************
    'remueva los estyilos de las tabs Becas Ingles
    Public Sub remueveEstilosTabBecasIngles()
        tabBecasIngles_1.Attributes.Remove("class") '
        tabBecasIngles_2.Attributes.Remove("class") '
        tabBecasIngles_3.Attributes.Remove("class") '
        tabBecasIngles_4.Attributes.Remove("class") '
        lnkTabBecasIngles1.Attributes.Remove("class")
        lnkTabBecasIngles2.Attributes.Remove("class")
        lnkTabBecasIngles3.Attributes.Remove("class")
        lnkTabBecasIngles4.Attributes.Remove("class")
    End Sub
    'asigna tab de Becas
    Public Sub AsignaEstilosTabBecasIngles()
        tabBecasIngles_1.Attributes.Add("class", "tab-pane") '
        tabBecasIngles_2.Attributes.Add("class", "tab-pane") ''
        tabBecasIngles_3.Attributes.Add("class", "tab-pane") '
        tabBecasIngles_4.Attributes.Add("class", "tab-pane") '
    End Sub
    'asigna la propiedad de Active al Tab de Becas
    Public Sub AsignaEstiloTabBecasInglesActive()
        'asigna estatus active
        If hdIdTabBecasIngles.Value = 1 Then
            tabBecasIngles_1.Attributes.Add("class", "tab-pane active") '
            lnkTabBecasIngles1.Attributes.Add("class", "active")
        ElseIf hdIdTabBecasIngles.Value = 2 Then
            tabBecasIngles_2.Attributes.Add("class", "tab-pane active") '
            lnkTabBecasIngles2.Attributes.Add("class", "active")
        ElseIf hdIdTabBecasIngles.Value = 3 Then
            tabBecasIngles_3.Attributes.Add("class", "tab-pane active") '
            lnkTabBecasIngles3.Attributes.Add("class", "active")
        ElseIf hdIdTabBecasIngles.Value = 4 Then
            tabBecasIngles_4.Attributes.Add("class", "tab-pane active") '
            lnkTabBecasIngles4.Attributes.Add("class", "active")
        End If
    End Sub

    '********************* Gestion de Capacitacion****************************************
    'remueva los estyilos de las tabs Gestion
    Public Sub remueveEstilosTabGestionCap()
        tabGestionCap_1.Attributes.Remove("class") '
        tabGestionCap_2.Attributes.Remove("class") '
        tabGestionCap_3.Attributes.Remove("class") '
        tabGestionCap_4.Attributes.Remove("class") '
        tabGestionCap_5.Attributes.Remove("class") '
        tabGestionCap_6.Attributes.Remove("class") '
        tabGestionCap_7.Attributes.Remove("class") '
        tabGestionCap_8.Attributes.Remove("class") '

        lnkTabGestionCap1.Attributes.Remove("class")
        lnkTabGestionCap2.Attributes.Remove("class")
        lnkTabGestionCap3.Attributes.Remove("class")
        lnkTabGestionCap4.Attributes.Remove("class")
        lnkTabGestionCap5.Attributes.Remove("class")
        lnkTabGestionCap6.Attributes.Remove("class")
        lnkTabGestionCap7.Attributes.Remove("class")
        lnkTabGestionCap8.Attributes.Remove("class")
    End Sub
    'asigna tab de Gestion
    Public Sub AsignaEstilosTabGestionCaps()
        tabGestionCap_1.Attributes.Add("class", "tab-pane") '
        tabGestionCap_2.Attributes.Add("class", "tab-pane") '
        tabGestionCap_3.Attributes.Add("class", "tab-pane") '
        tabGestionCap_4.Attributes.Add("class", "tab-pane") '
        tabGestionCap_5.Attributes.Add("class", "tab-pane") '
        tabGestionCap_6.Attributes.Add("class", "tab-pane") '
        tabGestionCap_7.Attributes.Add("class", "tab-pane") '
        tabGestionCap_8.Attributes.Add("class", "tab-pane") '
        '
    End Sub
    'asigna la propiedad de Active al Tab de Gestion
    Public Sub AsignaEstiloTabGestionCapActive()
        'asigna estatus active
        If hdIdGestionCapacitacion.Value = 1 Then
            tabGestionCap_1.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap1.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 2 Then
            tabGestionCap_2.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap2.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 3 Then
            tabGestionCap_3.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap3.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 4 Then
            tabGestionCap_4.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap4.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 5 Then
            tabGestionCap_5.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap5.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 6 Then
            tabGestionCap_6.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap6.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 7 Then
            tabGestionCap_7.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap7.Attributes.Add("class", "active")
        ElseIf hdIdGestionCapacitacion.Value = 8 Then
            tabGestionCap_8.Attributes.Add("class", "tab-pane active") '
            lnkTabGestionCap8.Attributes.Add("class", "active")
        End If
    End Sub
    Public Sub formatoGrids()
        'competencia vinculada
        For iFil As Integer = 0 To grdCompetenciasV.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCompetenciasV.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'habilidades dina
        For iFil As Integer = 0 To grdHabilidadesDina.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdHabilidadesDina.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Medir Efectiva
        For iFil As Integer = 0 To grdMedirEfectividad.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdMedirEfectividad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'MOTIVO
        For iFil As Integer = 0 To grdMotivo.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdMotivo.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Objetivo Corporativo
        For iFil As Integer = 0 To grdObjetivosCorporativos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdObjetivosCorporativos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Servicios provvedor
        For iFil As Integer = 0 To grdServicioProveedor.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdServicioProveedor.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'tipo de indicador
        For iFil As Integer = 0 To grdTipoIndicador.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTipoIndicador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Indicador
        For iFil As Integer = 0 To grdIndicador.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdIndicador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Estatus
        For iFil As Integer = 0 To grdEstatusDNC.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdEstatusDNC.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        '***********************- Becas - *******************************++
        'Tipo Beca
        For iFil As Integer = 0 To grdTipoBeca.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTipoBeca.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Periodo Asignacion
        For iFil As Integer = 0 To grdPeriodoAsignacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPeriodoAsignacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Tipo Asignacion
        For iFil As Integer = 0 To grdTipoAsignacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTipoAsignacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Modalidad Estudio
        For iFil As Integer = 0 To grdModalidadEstudio.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdModalidadEstudio.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Tipo Estatus
        For iFil As Integer = 0 To grdTipoEstatus.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTipoEstatus.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Tipo de Proyecto
        For iFil As Integer = 0 To grdTProyecto.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTProyecto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'modalidad de pago
        For iFil As Integer = 0 To grdModalidadPago.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdModalidadPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'estatus Pago
        For iFil As Integer = 0 To grdEstatusPago.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdEstatusPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Concepto Pago
        For iFil As Integer = 0 To grdConceptoPago.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdConceptoPago.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        '**************** Becas de Ingles ***********************************+++
        'Tipo Autorizacion
        For iFil As Integer = 0 To grdInglesTipoAutorizacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdInglesTipoAutorizacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Horario
        For iFil As Integer = 0 To grdInglesHorario.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdInglesHorario.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Plantel
        For iFil As Integer = 0 To grdPlantelIngles.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPlantelIngles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Niveles
        For iFil As Integer = 0 To grdNiveles.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdNiveles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Tipo de Agente
        For iFil As Integer = 0 To grdTipoAgente.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdTipoAgente.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Estatus Capacitacion
        For iFil As Integer = 0 To grdEstatusCapacitacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdEstatusCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Establecimientos
        For iFil As Integer = 0 To GrdEstablecimientos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdEstablecimientos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Modalidad
        For iFil As Integer = 0 To GrdModalidad.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdModalidad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Area Tematica
        For iFil As Integer = 0 To GrdAreaTematica.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdAreaTematica.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Clave
        For iFil As Integer = 0 To grdClaveCapacitacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdClaveCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        For iFil As Integer = 0 To grdOcuapacionDC3.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdOcuapacionDC3.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next


        For iFil As Integer = 0 To grdClaveCurso.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdClaveCurso.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To GrdModalidad.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdModalidad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To GrdEstablecimientos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdEstablecimientos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
    End Sub
    'Clave

#End Region
    '**************** Catalogo de Becas de Ingles **********************************++

    Public Sub CargaCatalogosBecasIngles()
        Call obtCatalogoTipoAutorizacionIngles()
        Call obtCatalogoHorarioIngles()
        Call obtCatalogoPlantelIngles()
        Call obtCatalogoNivelesIngles()
    End Sub

#Region "Grid Tipo Autorizacion"
    'obtiene el catalogo 
    Public Sub obtCatalogoTipoAutorizacionIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_INGLES_TIPO_AUTORIZACION_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdInglesTipoAutorizacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdInglesTipoAutorizacion.DataBind()

            If grdInglesTipoAutorizacion.Rows.Count = 0 Then
                Call insFilaVaciaTipoAutorizacionIngles()
                grdInglesTipoAutorizacion.Rows(0).Visible = False

            Else
                grdInglesTipoAutorizacion.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdInglesTipoAutorizacion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdInglesTipoAutorizacion.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdInglesTipoAutorizacion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdInglesTipoAutorizacion.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdInglesTipoAutorizacion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Tipo Autorizacion
            For iFil As Integer = 0 To grdInglesTipoAutorizacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdInglesTipoAutorizacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoAutorizacionIngles()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdInglesTipoAutorizacion.DataSource = dt.DefaultView
        grdInglesTipoAutorizacion.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionTipoAutorizacionIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdInglesTipoAutorizacion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdInglesTipoAutorizacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDesTipoAutorizacionIngles(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_INGLES_TIPO_AUTORIZACION_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoTipoAutorizacionIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    Private Sub grdInglesTipoAutorizacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdInglesTipoAutorizacion.RowCancelingEdit
        grdInglesTipoAutorizacion.ShowFooter = True
        grdInglesTipoAutorizacion.EditIndex = -1
        Call obtCatalogoTipoAutorizacionIngles()
    End Sub

    'Validacion de Controles de edicion y eliminar
    Private Sub grdInglesTipoAutorizacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdInglesTipoAutorizacion.RowDataBound

        For i As Integer = 0 To grdInglesTipoAutorizacion.Rows.Count - 1

            Dim btnEditar As LinkButton = grdInglesTipoAutorizacion.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdInglesTipoAutorizacion.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdInglesTipoAutorizacion.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdInglesTipoAutorizacion.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdInglesTipoAutorizacion.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('imgBecaIngles').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdInglesTipoAutorizacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdInglesTipoAutorizacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdInglesTipoAutorizacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            ' If validaTipoAutorizacionIngles(odbConexion, strId) Then
            '     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '     Exit Sub
            ' End If

            strQuery = "DELETE FROM BECAS_INGLES_TIPO_AUTORIZACION_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdInglesTipoAutorizacion.EditIndex = -1
            grdInglesTipoAutorizacion.ShowFooter = True
            Call obtCatalogoTipoAutorizacionIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaTipoAutorizacionIngles(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdInglesTipoAutorizacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdInglesTipoAutorizacion.RowEditing
        grdInglesTipoAutorizacion.ShowFooter = False
        grdInglesTipoAutorizacion.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoAutorizacionIngles()
    End Sub
    'actualiza la descripcion
    Private Sub grdInglesTipoAutorizacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdInglesTipoAutorizacion.RowUpdating
        grdInglesTipoAutorizacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdInglesTipoAutorizacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdInglesTipoAutorizacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdInglesTipoAutorizacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDesTipoAutorizacionIngles(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_INGLES_TIPO_AUTORIZACION_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdInglesTipoAutorizacion.EditIndex = -1
            Call obtCatalogoTipoAutorizacionIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTipoAutorizacionIngles_Click(sender As Object, e As EventArgs)
        Call insDescripcionTipoAutorizacionIngles()
    End Sub

    Protected Sub grdInglesTipoAutorizacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdInglesTipoAutorizacion.PageIndexChanging
        grdInglesTipoAutorizacion.ShowFooter = True
        grdInglesTipoAutorizacion.PageIndex = e.NewPageIndex
        grdInglesTipoAutorizacion.DataBind()
        Call obtCatalogoTipoAutorizacionIngles()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDesTipoAutorizacionIngles(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_INGLES_TIPO_AUTORIZACION_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Horario"
    'obtiene el catalogo 
    Public Sub obtCatalogoHorarioIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_INGLES_HORARIO_ESTUDIO_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdInglesHorario.DataSource = dsCatalogo.Tables(0).DefaultView
            grdInglesHorario.DataBind()

            If grdInglesHorario.Rows.Count = 0 Then
                Call insFilaVaciaHorarioIngles()
                grdInglesHorario.Rows(0).Visible = False

            Else
                grdInglesHorario.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdInglesHorario.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdInglesHorario.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdInglesHorario.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdInglesHorario.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdInglesHorario.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Tipo Autorizacion
            For iFil As Integer = 0 To grdInglesHorario.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdInglesHorario.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaHorarioIngles()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdInglesHorario.DataSource = dt.DefaultView
        grdInglesHorario.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionHorarioIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdInglesHorario.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdInglesHorario.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHorarioIngles(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_INGLES_HORARIO_ESTUDIO_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoHorarioIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    Private Sub grdInglesHorario_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdInglesHorario.RowCancelingEdit
        grdInglesHorario.ShowFooter = True
        grdInglesHorario.EditIndex = -1
        Call obtCatalogoHorarioIngles()
    End Sub

    'Validacion de Controles de edicion y eliminar
    Private Sub grdInglesHorario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdInglesHorario.RowDataBound

        For i As Integer = 0 To grdInglesHorario.Rows.Count - 1

            Dim btnEditar As LinkButton = grdInglesHorario.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdInglesHorario.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdInglesHorario.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdInglesHorario.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdInglesHorario.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('imgBecaIngles').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdInglesHorario_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdInglesHorario.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdInglesHorario.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            ' If validaHorarioIngles(odbConexion, strId) Then
            '     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '     Exit Sub
            ' End If

            strQuery = "DELETE FROM BECAS_INGLES_HORARIO_ESTUDIO_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdInglesHorario.EditIndex = -1
            grdInglesHorario.ShowFooter = True
            Call obtCatalogoHorarioIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaHorarioIngles(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdInglesHorario_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdInglesHorario.RowEditing
        grdInglesHorario.ShowFooter = False
        grdInglesHorario.EditIndex = e.NewEditIndex
        Call obtCatalogoHorarioIngles()
    End Sub
    'actualiza la descripcion
    Private Sub grdInglesHorario_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdInglesHorario.RowUpdating
        grdInglesHorario.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdInglesHorario.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdInglesHorario.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdInglesHorario.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHorarioIngles(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_INGLES_HORARIO_ESTUDIO_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdInglesHorario.EditIndex = -1
            Call obtCatalogoHorarioIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarHorarioIngles_Click(sender As Object, e As EventArgs)
        Call insDescripcionHorarioIngles()
    End Sub

    Protected Sub grdInglesHorario_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdInglesHorario.PageIndexChanging
        grdInglesHorario.ShowFooter = True
        grdInglesHorario.PageIndex = e.NewPageIndex
        grdInglesHorario.DataBind()
        Call obtCatalogoHorarioIngles()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionHorarioIngles(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_INGLES_HORARIO_ESTUDIO_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Plantel"
    'obtiene el catalogo 
    Public Sub obtCatalogoPlantelIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_INGLES_PLANTEL_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPlantelIngles.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPlantelIngles.DataBind()

            If grdPlantelIngles.Rows.Count = 0 Then
                Call insFilaVaciaPlantelIngles()
                grdPlantelIngles.Rows(0).Visible = False

            Else
                grdPlantelIngles.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdPlantelIngles.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdPlantelIngles.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdPlantelIngles.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdPlantelIngles.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdPlantelIngles.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Tipo Autorizacion
            For iFil As Integer = 0 To grdPlantelIngles.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPlantelIngles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaPlantelIngles()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPlantelIngles.DataSource = dt.DefaultView
        grdPlantelIngles.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionPlantelIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdPlantelIngles.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdPlantelIngles.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPlantelIngles(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_INGLES_PLANTEL_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPlantelIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    Private Sub grdPlantelIngles_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPlantelIngles.RowCancelingEdit
        grdPlantelIngles.ShowFooter = True
        grdPlantelIngles.EditIndex = -1
        Call obtCatalogoPlantelIngles()
    End Sub

    'Validacion de Controles de edicion y eliminar
    Private Sub grdPlantelIngles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPlantelIngles.RowDataBound

        For i As Integer = 0 To grdPlantelIngles.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPlantelIngles.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdPlantelIngles.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPlantelIngles.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdPlantelIngles.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPlantelIngles.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('imgBecaIngles').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdPlantelIngles_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPlantelIngles.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPlantelIngles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            ' If validaPlantelIngles(odbConexion, strId) Then
            '     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '     Exit Sub
            ' End If

            strQuery = "DELETE FROM BECAS_INGLES_PLANTEL_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPlantelIngles.EditIndex = -1
            grdPlantelIngles.ShowFooter = True
            Call obtCatalogoPlantelIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaPlantelIngles(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdPlantelIngles_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPlantelIngles.RowEditing
        grdPlantelIngles.ShowFooter = False
        grdPlantelIngles.EditIndex = e.NewEditIndex
        Call obtCatalogoPlantelIngles()
    End Sub
    'actualiza la descripcion
    Private Sub grdPlantelIngles_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPlantelIngles.RowUpdating
        grdPlantelIngles.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdPlantelIngles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdPlantelIngles.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdPlantelIngles.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPlantelIngles(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_INGLES_PLANTEL_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPlantelIngles.EditIndex = -1
            Call obtCatalogoPlantelIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarPlantelI_Click(sender As Object, e As EventArgs)
        Call insDescripcionPlantelIngles()
    End Sub

    Protected Sub grdPlantelIngles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPlantelIngles.PageIndexChanging
        grdPlantelIngles.ShowFooter = True
        grdPlantelIngles.PageIndex = e.NewPageIndex
        grdPlantelIngles.DataBind()
        Call obtCatalogoPlantelIngles()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionPlantelIngles(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_INGLES_PLANTEL_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Niveles"
    'obtiene el catalogo 
    Public Sub obtCatalogoNivelesIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM BECAS_INGLES_NIVELES_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdNiveles.DataSource = dsCatalogo.Tables(0).DefaultView
            grdNiveles.DataBind()

            If grdNiveles.Rows.Count = 0 Then
                Call insFilaVaciaNivelesIngles()
                grdNiveles.Rows(0).Visible = False

            Else
                grdNiveles.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdNiveles.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdNiveles.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdNiveles.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdNiveles.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdNiveles.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Niveles
            For iFil As Integer = 0 To grdNiveles.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdNiveles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaNivelesIngles()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdNiveles.DataSource = dt.DefaultView
        grdNiveles.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionNivelIngles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdNiveles.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdNiveles.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionNivelesIngles(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO BECAS_INGLES_NIVELES_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoNivelesIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    Private Sub grdNiveles_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdNiveles.RowCancelingEdit
        grdNiveles.ShowFooter = True
        grdNiveles.EditIndex = -1
        Call obtCatalogoNivelesIngles()
    End Sub

    'Validacion de Controles de edicion y eliminar
    Private Sub grdNiveles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdNiveles.RowDataBound

        For i As Integer = 0 To grdNiveles.Rows.Count - 1

            Dim btnEditar As LinkButton = grdNiveles.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdNiveles.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdNiveles.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdNiveles.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdNiveles.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('imgBecaIngles').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdNiveles_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdNiveles.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdNiveles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            ' If validaNivelesIngles(odbConexion, strId) Then
            '     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '     Exit Sub
            ' End If

            strQuery = "DELETE FROM BECAS_INGLES_NIVELES_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdNiveles.EditIndex = -1
            grdNiveles.ShowFooter = True
            Call obtCatalogoNivelesIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaNivelesIngles(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor=" & id.ToString
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

    'habilita el modo edicion
    Private Sub grdNiveles_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdNiveles.RowEditing
        grdNiveles.ShowFooter = False
        grdNiveles.EditIndex = e.NewEditIndex
        Call obtCatalogoNivelesIngles()
    End Sub
    'actualiza la descripcion
    Private Sub grdNiveles_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdNiveles.RowUpdating
        grdNiveles.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdNiveles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdNiveles.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdNiveles.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionNivelesIngles(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE BECAS_INGLES_NIVELES_CT " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdNiveles.EditIndex = -1
            Call obtCatalogoNivelesIngles()
        Catch ex As Exception
            lblErrorIngles.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarNiveles_Click(sender As Object, e As EventArgs)
        Call insDescripcionNivelIngles()
    End Sub

    Protected Sub grdNiveles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdNiveles.PageIndexChanging
        grdNiveles.ShowFooter = True
        grdNiveles.PageIndex = e.NewPageIndex
        grdNiveles.DataBind()
        Call obtCatalogoNivelesIngles()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionNivelesIngles(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM BECAS_INGLES_NIVELES_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
    '******************* Gestion de Capacitacion ******************************
    Public Sub CargaCatalogosGestionCapacitacion()
        Call obtCatalogoEstatusCursos()
        Call obtCatalogoTipoAgente()
        Call obtCatalogoAreaTematica()
        Call obtCatalogoClaveCap()
        Call obtCatalogoEstablecimeinto()
        Call obtCatalogoModalidad()
        Call obtCatalogoClaveCurso()
        Call obtCatalogoOcupacionDC3()
    End Sub
#Region "Grid Estatus Cursos"
    'obtiene el catalogo 
    Public Sub obtCatalogoEstatusCursos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_ESTATUS_CAPACITACION ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdEstatusCapacitacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdEstatusCapacitacion.DataBind()

            If grdEstatusCapacitacion.Rows.Count = 0 Then
                Call insFilaVaciaEstatusCursos()
                grdEstatusCapacitacion.Rows(0).Visible = False

            Else
                grdEstatusCapacitacion.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdEstatusCapacitacion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdEstatusCapacitacion.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdEstatusCapacitacion.Rows(i).Controls(4).Controls(1)

                iId = DirectCast(grdEstatusCapacitacion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdEstatusCapacitacion.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdEstatusCapacitacion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If

                If iId <= 7 Then
                    btnEditar.Visible = False
                    btnEliminar.Visible = False
                Else
                    btnEditar.Visible = True
                    btnEliminar.Visible = True
                End If

            Next

            'Estatus Capacitacion
            For iFil As Integer = 0 To grdEstatusCapacitacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdEstatusCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaEstatusCursos()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdEstatusCapacitacion.DataSource = dt.DefaultView
        grdEstatusCapacitacion.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insEstatusCapacitacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdEstatusCapacitacion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdEstatusCapacitacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatusCapacitacion(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO GC_CT_ESTATUS_CAPACITACION (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoEstatusCursos()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub grdEstatusCapacitacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdEstatusCapacitacion.RowCancelingEdit
        grdEstatusCapacitacion.ShowFooter = True
        grdEstatusCapacitacion.EditIndex = -1
        Call obtCatalogoEstatusCursos()
    End Sub

    'TOOLTIPS
    Private Sub grdEstatusCapacitacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEstatusCapacitacion.RowDataBound

        For i As Integer = 0 To grdEstatusCapacitacion.Rows.Count - 1

            Dim btnEditar As LinkButton = grdEstatusCapacitacion.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdEstatusCapacitacion.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEstatusCapacitacion.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdEstatusCapacitacion.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEstatusCapacitacion.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdEstatusCapacitacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdEstatusCapacitacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdEstatusCapacitacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaEstatusCapacitacion(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM GC_CT_ESTATUS_CAPACITACION WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusCapacitacion.EditIndex = -1
            grdEstatusCapacitacion.ShowFooter = True
            Call obtCatalogoEstatusCursos()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaEstatusCapacitacion(odbConexion As OleDbConnection, id_Habilidad As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from GC_GESTION_CAPACITACION_TB where fk_id_estatus=" & id_Habilidad.ToString
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

    'habilita el modo edicion
    Private Sub grdEstatusCapacitacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdEstatusCapacitacion.RowEditing
        grdEstatusCapacitacion.ShowFooter = False
        grdEstatusCapacitacion.EditIndex = e.NewEditIndex
        Call obtCatalogoEstatusCursos()
    End Sub
    'actualiza la descripcion
    Private Sub grdEstatusCapacitacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdEstatusCapacitacion.RowUpdating
        grdEstatusCapacitacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdEstatusCapacitacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdEstatusCapacitacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdEstatusCapacitacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEstatusCapacitacion(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE GC_CT_ESTATUS_CAPACITACION " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEstatusCapacitacion.EditIndex = -1
            Call obtCatalogoEstatusCursos()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub


    Protected Sub lnkAgregarEstatusCursos_Click(sender As Object, e As EventArgs)
        Call insEstatusCapacitacion()
    End Sub
    Protected Sub grdEstatusCapacitacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEstatusCapacitacion.PageIndexChanging
        grdEstatusCapacitacion.ShowFooter = True
        grdEstatusCapacitacion.PageIndex = e.NewPageIndex
        grdEstatusCapacitacion.DataBind()
        Call obtCatalogoEstatusCursos()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionEstatusCapacitacion(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_ESTATUS_CAPACITACION where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
#Region "Grid Tipo Agente"
    ''obtiene el catalogo 
    Public Sub obtCatalogoTipoAgente()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_TIPO_AGENTE  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdTipoAgente.DataSource = dsCatalogo.Tables(0).DefaultView
            grdTipoAgente.DataBind()

            If grdTipoAgente.Rows.Count = 0 Then
                Call insFilaVaciaTipoAgente()
                grdTipoAgente.Rows(0).Visible = False


            Else
                grdTipoAgente.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdTipoAgente.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdTipoAgente.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdTipoAgente.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdTipoAgente.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdTipoAgente.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'Tipo de Agente
            For iFil As Integer = 0 To grdTipoAgente.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdTipoAgente.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionTipoAgente(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_TIPO_AGENTE where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaTipoAgente()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdTipoAgente.DataSource = dt.DefaultView
        grdTipoAgente.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionTipoAgente()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdTipoAgente.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdTipoAgente.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoAgente.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionTipoAgente(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_TIPO_AGENTE (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoTipoAgente()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub grdTipoAgente_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdTipoAgente.RowCancelingEdit
        grdTipoAgente.ShowFooter = True
        grdTipoAgente.EditIndex = -1
        Call obtCatalogoTipoAgente()
    End Sub

    Private Sub grdTipoAgente_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTipoAgente.RowDataBound

        For i As Integer = 0 To grdTipoAgente.Rows.Count - 1
            Dim btnEditar As LinkButton = grdTipoAgente.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdTipoAgente.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdTipoAgente.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdTipoAgente.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdTipoAgente.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiTipoAgente(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE [fk_id_tipo_agente]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdTipoAgente_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdTipoAgente.RowEditing
        grdTipoAgente.ShowFooter = False
        grdTipoAgente.EditIndex = e.NewEditIndex
        Call obtCatalogoTipoAgente()
    End Sub
    'actualiza la descripcion
    Private Sub grdTipoAgente_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdTipoAgente.RowUpdating
        grdTipoAgente.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(grdTipoAgente.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdTipoAgente.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdTipoAgente.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(grdTipoAgente.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionTipoAgente(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_TIPO_AGENTE] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdTipoAgente.EditIndex = -1
            Call obtCatalogoTipoAgente()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdTipoAgente_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdTipoAgente.PageIndexChanging
        Try
            grdTipoAgente.ShowFooter = True
            grdTipoAgente.EditIndex = -1
            grdTipoAgente.PageIndex = e.NewPageIndex
            grdTipoAgente.DataBind()
            Call obtCatalogoTipoAgente()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarTipoAgente_Click(sender As Object, e As EventArgs)
        Call insDescripcionTipoAgente()
    End Sub
    Private Sub grdTipoAgente_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdTipoAgente.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdTipoAgente.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_TIPO_AGENTE WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiTipoAgente(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdTipoAgente.EditIndex = -1
            grdTipoAgente.ShowFooter = True
            Call obtCatalogoTipoAgente()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Area Tematica"
    ''obtiene el catalogo 
    Public Sub obtCatalogoAreaTematica()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_AREA_TEMATICA  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            GrdAreaTematica.DataSource = dsCatalogo.Tables(0).DefaultView
            GrdAreaTematica.DataBind()

            If GrdAreaTematica.Rows.Count = 0 Then
                Call insFilaVaciaAreaTematica()
                GrdAreaTematica.Rows(0).Visible = False


            Else
                GrdAreaTematica.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To GrdAreaTematica.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = GrdAreaTematica.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(GrdAreaTematica.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = GrdAreaTematica.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = GrdAreaTematica.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'Tipo de Agente
            For iFil As Integer = 0 To GrdAreaTematica.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    GrdAreaTematica.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionAreaTematica(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_AREA_TEMATICA where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaAreaTematica()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        GrdAreaTematica.DataSource = dt.DefaultView
        GrdAreaTematica.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionAreaTematica()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(GrdAreaTematica.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdAreaTematica.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdAreaTematica.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionAreaTematica(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_AREA_TEMATICA (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoAreaTematica()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub GrdAreaTematica_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GrdAreaTematica.RowCancelingEdit
        GrdAreaTematica.ShowFooter = True
        GrdAreaTematica.EditIndex = -1
        Call obtCatalogoAreaTematica()
    End Sub

    Private Sub GrdAreaTematica_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdAreaTematica.RowDataBound

        For i As Integer = 0 To GrdAreaTematica.Rows.Count - 1
            Dim btnEditar As LinkButton = GrdAreaTematica.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = GrdAreaTematica.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdAreaTematica.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = GrdAreaTematica.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdAreaTematica.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiAreaTematica(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE [fk_id_area_tematica]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub GrdAreaTematica_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdAreaTematica.RowEditing
        GrdAreaTematica.ShowFooter = False
        GrdAreaTematica.EditIndex = e.NewEditIndex
        Call obtCatalogoAreaTematica()
    End Sub
    'actualiza la descripcion
    Private Sub GrdAreaTematica_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GrdAreaTematica.RowUpdating
        GrdAreaTematica.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(GrdAreaTematica.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(GrdAreaTematica.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdAreaTematica.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdAreaTematica.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionAreaTematica(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_AREA_TEMATICA] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdAreaTematica.EditIndex = -1
            Call obtCatalogoAreaTematica()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub GrdAreaTematica_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdAreaTematica.PageIndexChanging
        Try
            GrdAreaTematica.ShowFooter = True
            GrdAreaTematica.EditIndex = -1
            GrdAreaTematica.PageIndex = e.NewPageIndex
            GrdAreaTematica.DataBind()
            Call obtCatalogoAreaTematica()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarAreaTematica_Click(sender As Object, e As EventArgs)
        Call insDescripcionAreaTematica()
    End Sub
    Private Sub GrdAreaTematica_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdAreaTematica.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdAreaTematica.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_AREA_TEMATICA WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiAreaTematica(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            GrdAreaTematica.EditIndex = -1
            GrdAreaTematica.ShowFooter = True
            Call obtCatalogoAreaTematica()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Clave Capacitacion"
    ''obtiene el catalogo 
    Public Sub obtCatalogoClaveCap()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_CLAVE_CAPACITACION  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdClaveCapacitacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdClaveCapacitacion.DataBind()

            If grdClaveCapacitacion.Rows.Count = 0 Then
                Call grdinsFilaVaciaClaveCapacitacion()
                grdClaveCapacitacion.Rows(0).Visible = False


            Else
                grdClaveCapacitacion.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdClaveCapacitacion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdClaveCapacitacion.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdClaveCapacitacion.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdClaveCapacitacion.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdClaveCapacitacion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'Color
            For iFil As Integer = 0 To grdClaveCapacitacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdClaveCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionClaveCap(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_CLAVE_CAPACITACION where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub grdinsFilaVaciaClaveCapacitacion()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdClaveCapacitacion.DataSource = dt.DefaultView
        grdClaveCapacitacion.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionClaveCap()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdClaveCapacitacion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdClaveCapacitacion.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(grdClaveCapacitacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionClaveCap(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_CLAVE_CAPACITACION (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoClaveCap()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub grdClaveCapacitacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdClaveCapacitacion.RowCancelingEdit
        grdClaveCapacitacion.ShowFooter = True
        grdClaveCapacitacion.EditIndex = -1
        Call obtCatalogoClaveCap()
    End Sub

    Private Sub grdClaveCapacitacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdClaveCapacitacion.RowDataBound

        For i As Integer = 0 To grdClaveCapacitacion.Rows.Count - 1
            Dim btnEditar As LinkButton = grdClaveCapacitacion.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdClaveCapacitacion.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdClaveCapacitacion.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdClaveCapacitacion.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdClaveCapacitacion.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiClaveCap(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE [fk_id_clave_capacitacion]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdClaveCapacitacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdClaveCapacitacion.RowEditing
        grdClaveCapacitacion.ShowFooter = False
        grdClaveCapacitacion.EditIndex = e.NewEditIndex
        Call obtCatalogoClaveCap()
    End Sub
    'actualiza la descripcion
    Private Sub grdClaveCapacitacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdClaveCapacitacion.RowUpdating
        grdClaveCapacitacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(grdClaveCapacitacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdClaveCapacitacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdClaveCapacitacion.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(grdClaveCapacitacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionClaveCap(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_CLAVE_CAPACITACION] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdClaveCapacitacion.EditIndex = -1
            Call obtCatalogoClaveCap()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdClaveCapacitacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdClaveCapacitacion.PageIndexChanging
        Try
            grdClaveCapacitacion.ShowFooter = True
            grdClaveCapacitacion.EditIndex = -1
            grdClaveCapacitacion.PageIndex = e.NewPageIndex
            grdClaveCapacitacion.DataBind()
            Call obtCatalogoClaveCap()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarClaveCapacitacion_Click(sender As Object, e As EventArgs)
        Call insDescripcionClaveCap()
    End Sub

    Private Sub grdClaveCapacitacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdClaveCapacitacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdClaveCapacitacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_CLAVE_CAPACITACION WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiClaveCap(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdClaveCapacitacion.EditIndex = -1
            grdClaveCapacitacion.ShowFooter = True
            Call obtCatalogoClaveCap()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Establecimiento"
    ''obtiene el catalogo 
    Public Sub obtCatalogoEstablecimeinto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_CLAVE_ESTABLECIMIENTO  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            GrdEstablecimientos.DataSource = dsCatalogo.Tables(0).DefaultView
            GrdEstablecimientos.DataBind()

            If GrdEstablecimientos.Rows.Count = 0 Then
                Call grdinsFilaVaciaEstablecimiento()
                GrdEstablecimientos.Rows(0).Visible = False


            Else
                GrdEstablecimientos.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To GrdEstablecimientos.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = GrdEstablecimientos.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(GrdEstablecimientos.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = GrdEstablecimientos.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = GrdEstablecimientos.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()

            'COlor
            For iFil As Integer = 0 To GrdEstablecimientos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    GrdEstablecimientos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionEstablecimiento(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_CLAVE_ESTABLECIMIENTO where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub grdinsFilaVaciaEstablecimiento()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        GrdEstablecimientos.DataSource = dt.DefaultView
        GrdEstablecimientos.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionEstablecimientos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(GrdEstablecimientos.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdEstablecimientos.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdEstablecimientos.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionEstablecimiento(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_CLAVE_ESTABLECIMIENTO (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoEstablecimeinto()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub GrdEstablecimientos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GrdEstablecimientos.RowCancelingEdit
        GrdEstablecimientos.ShowFooter = True
        GrdEstablecimientos.EditIndex = -1
        Call obtCatalogoEstablecimeinto()
    End Sub

    Private Sub GrdEstablecimientos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdEstablecimientos.RowDataBound

        For i As Integer = 0 To GrdEstablecimientos.Rows.Count - 1
            Dim btnEditar As LinkButton = GrdEstablecimientos.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = GrdEstablecimientos.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdEstablecimientos.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = GrdEstablecimientos.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdEstablecimientos.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiEstablecimientos(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE [fk_id_clave_establecimiento]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub GrdEstablecimientos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdEstablecimientos.RowEditing
        GrdEstablecimientos.ShowFooter = False
        GrdEstablecimientos.EditIndex = e.NewEditIndex
        Call obtCatalogoEstablecimeinto()
    End Sub
    'actualiza la descripcion
    Private Sub GrdEstablecimientos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GrdEstablecimientos.RowUpdating
        GrdEstablecimientos.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(GrdEstablecimientos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(GrdEstablecimientos.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdEstablecimientos.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdEstablecimientos.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionEstablecimiento(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_CLAVE_ESTABLECIMIENTO] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdEstablecimientos.EditIndex = -1
            Call obtCatalogoEstablecimeinto()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub GrdEstablecimientos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdEstablecimientos.PageIndexChanging
        Try
            GrdEstablecimientos.ShowFooter = True
            GrdEstablecimientos.EditIndex = -1
            GrdEstablecimientos.PageIndex = e.NewPageIndex
            GrdEstablecimientos.DataBind()
            Call obtCatalogoEstablecimeinto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarEstablecimientos_Click(sender As Object, e As EventArgs)
        Call insDescripcionEstablecimientos()
    End Sub
    Private Sub GrdEstablecimientos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdEstablecimientos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdEstablecimientos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_CLAVE_ESTABLECIMIENTO WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiEstablecimientos(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            GrdEstablecimientos.EditIndex = -1
            GrdEstablecimientos.ShowFooter = True
            Call obtCatalogoEstablecimeinto()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Modalidad"
    ''obtiene el catalogo 
    Public Sub obtCatalogoModalidad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_CLAVE_MODALIDAD  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            GrdModalidad.DataSource = dsCatalogo.Tables(0).DefaultView
            GrdModalidad.DataBind()

            If GrdModalidad.Rows.Count = 0 Then
                Call grdinsFilaVaciaModaliad()
                GrdModalidad.Rows(0).Visible = False


            Else
                GrdModalidad.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To GrdModalidad.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = GrdModalidad.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(GrdModalidad.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = GrdModalidad.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = GrdModalidad.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            odbConexion.Close()

            'Tipo de Agente
            For iFil As Integer = 0 To GrdModalidad.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    GrdModalidad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionModalidad(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_CLAVE_MODALIDAD where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub grdinsFilaVaciaModaliad()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        GrdModalidad.DataSource = dt.DefaultView
        GrdModalidad.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionModalidad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(GrdModalidad.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdModalidad.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdModalidad.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionModalidad(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_CLAVE_MODALIDAD (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoModalidad()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub GrdModalidad_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GrdModalidad.RowCancelingEdit
        GrdModalidad.ShowFooter = True
        GrdModalidad.EditIndex = -1
        Call obtCatalogoModalidad()
    End Sub

    Private Sub GrdModalidad_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdModalidad.RowDataBound

        For i As Integer = 0 To GrdModalidad.Rows.Count - 1
            Dim btnEditar As LinkButton = GrdModalidad.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = GrdModalidad.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdModalidad.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = GrdModalidad.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(GrdModalidad.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiModalidad(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE fk_id_clave_modalidad=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub GrdModalidad_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdModalidad.RowEditing
        GrdModalidad.ShowFooter = False
        GrdModalidad.EditIndex = e.NewEditIndex
        Call obtCatalogoModalidad()
    End Sub
    'actualiza la descripcion
    Private Sub GrdModalidad_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GrdModalidad.RowUpdating
        GrdModalidad.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(GrdModalidad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(GrdModalidad.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(GrdModalidad.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(GrdModalidad.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionModalidad(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR
            strQuery = " UPDATE [GC_CT_CLAVE_MODALIDAD] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdModalidad.EditIndex = -1
            Call obtCatalogoModalidad()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub GrdModalidad_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdModalidad.PageIndexChanging
        Try
            GrdModalidad.ShowFooter = True
            GrdModalidad.EditIndex = -1
            GrdModalidad.PageIndex = e.NewPageIndex
            GrdModalidad.DataBind()
            Call obtCatalogoModalidad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarModalidad_Click(sender As Object, e As EventArgs)
        Call insDescripcionModalidad()
    End Sub
    Private Sub GrdModalidad_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdModalidad.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdModalidad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_CLAVE_MODALIDAD WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiModalidad(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            GrdModalidad.EditIndex = -1
            GrdModalidad.ShowFooter = True
            Call obtCatalogoModalidad()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Clave Curso"
    ''obtiene el catalogo 
    Public Sub obtCatalogoClaveCurso()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_CLAVE_CURSO  ORDER BY 3 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdClaveCurso.DataSource = dsCatalogo.Tables(0).DefaultView
            grdClaveCurso.DataBind()

            If grdClaveCurso.Rows.Count = 0 Then
                Call insFilaVaciaClaveCurso()
                grdClaveCurso.Rows(0).Visible = False


            Else
                grdClaveCurso.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdClaveCurso.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEditar As LinkButton = grdClaveCurso.Rows(i).Controls(4).Controls(0)
                iId = DirectCast(grdClaveCurso.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdClaveCurso.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdClaveCurso.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            odbConexion.Close()


            For iFil As Integer = 0 To grdClaveCurso.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdClaveCurso.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionClaveCurso(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_CLAVE_CURSO where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaClaveCurso()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))



        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdClaveCurso.DataSource = dt.DefaultView
        grdClaveCurso.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionClaveCurso()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Try
            strDescripcion = (DirectCast(grdClaveCurso.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdClaveCurso.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(grdClaveCurso.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionClaveCurso(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_CLAVE_CURSO (descripcion,clave,estatus,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoClaveCurso()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub grdClaveCurso_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdClaveCurso.RowCancelingEdit
        grdClaveCurso.ShowFooter = True
        grdClaveCurso.EditIndex = -1
        Call obtCatalogoClaveCurso()
    End Sub

    Private Sub grdClaveCurso_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdClaveCurso.RowDataBound

        For i As Integer = 0 To grdClaveCurso.Rows.Count - 1
            Dim btnEditar As LinkButton = grdClaveCurso.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdClaveCurso.Rows(i).Controls(5).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdClaveCurso.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdClaveCurso.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdClaveCurso.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiClaveCurso(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE fk_id_clave_curso=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdClaveCurso_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdClaveCurso.RowEditing
        grdClaveCurso.ShowFooter = False
        grdClaveCurso.EditIndex = e.NewEditIndex
        Call obtCatalogoClaveCurso()
    End Sub
    'actualiza la descripcion
    Private Sub grdClaveCurso_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdClaveCurso.RowUpdating
        grdClaveCurso.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus As String
        Try
            strId = DirectCast(grdClaveCurso.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdClaveCurso.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdClaveCurso.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(grdClaveCurso.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionClaveCurso(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_CLAVE_CURSO] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdClaveCurso.EditIndex = -1
            Call obtCatalogoClaveCurso()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdClaveCurso_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdClaveCurso.PageIndexChanging
        Try
            grdClaveCurso.ShowFooter = True
            grdClaveCurso.EditIndex = -1
            grdClaveCurso.PageIndex = e.NewPageIndex
            grdClaveCurso.DataBind()
            Call obtCatalogoClaveCurso()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarClaveCurso_Click(sender As Object, e As EventArgs)
        Call insDescripcionClaveCurso()
    End Sub
    Private Sub grdClaveCurso_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdClaveCurso.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdClaveCurso.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_CLAVE_CURSO WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiClaveCurso(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdClaveCurso.EditIndex = -1
            grdClaveCurso.ShowFooter = True
            Call obtCatalogoClaveCurso()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Dc3"
    ''obtiene el catalogo 
    Public Sub obtCatalogoOcupacionDC3()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM GC_CT_CLAVE_OCUPACION_DC3  ORDER BY id ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdOcuapacionDC3.DataSource = dsCatalogo.Tables(0).DefaultView
            grdOcuapacionDC3.DataBind()

            If grdOcuapacionDC3.Rows.Count = 0 Then
                Call insFilaVaciaOcupacionDc3()
                grdOcuapacionDC3.Rows(0).Visible = False


            Else
                grdOcuapacionDC3.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If


            For i = 0 To grdOcuapacionDC3.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim ddlSeleccionar As DropDownList
                Dim btnEditar As LinkButton = grdOcuapacionDC3.Rows(i).Controls(5).Controls(0)
                iId = DirectCast(grdOcuapacionDC3.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdOcuapacionDC3.Rows(i).Cells(2).FindControl("ddlEstatus")
                    ddlSeleccionar = grdOcuapacionDC3.Rows(i).Cells(2).FindControl("ddlSeleccionar")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlSeleccionar.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(8).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdOcuapacionDC3.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")

                    Dim lblSeleccionar As New Label
                    lblSeleccionar = grdOcuapacionDC3.Rows(i).FindControl("lblSeleccionar")
                    lblSeleccionar.Text = IIf(lblSeleccionar.Text = "0", "No", "Sí")
                End If


            Next

            odbConexion.Close()

            'Tipo de Agente
            For iFil As Integer = 0 To grdOcuapacionDC3.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdOcuapacionDC3.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionDc3(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM GC_CT_CLAVE_OCUPACION_DC3 where (descripcion='" & strNombre & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaOcupacionDc3()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("estatus"))
        dt.Columns.Add(New DataColumn("seleccionar"))


        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("clave") = ""
        dr("estatus") = ""
        dr("seleccionar") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdOcuapacionDC3.DataSource = dt.DefaultView
        grdOcuapacionDC3.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcionDc3()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Dim strSeleccionar As String = ""
        Try
            strDescripcion = (DirectCast(grdOcuapacionDC3.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdOcuapacionDC3.FooterRow.FindControl("txtAgreClave"), TextBox).Text)
            strEstatus = (DirectCast(grdOcuapacionDC3.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            strSeleccionar = (DirectCast(grdOcuapacionDC3.FooterRow.FindControl("ddlAgreSeleccionar"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If



            If validaDescripcionDc3(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa Descripción.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO GC_CT_CLAVE_OCUPACION_DC3 (descripcion,clave,estatus,fecha_creacion,usuario_creacion,seleccionar)" & _
                       " VALUES ('" & strDescripcion & "','" & strClave & "'," & strEstatus & "," & _
                      "GETDATE(),'" & hdUsuario.Value & "'," & strSeleccionar & ")  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoOcupacionDC3()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Private Sub grdOcuapacionDC3_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdOcuapacionDC3.RowCancelingEdit
        grdOcuapacionDC3.ShowFooter = True
        grdOcuapacionDC3.EditIndex = -1
        Call obtCatalogoOcupacionDC3()
    End Sub

    Private Sub grdOcuapacionDC3_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdOcuapacionDC3.RowDataBound

        For i As Integer = 0 To grdOcuapacionDC3.Rows.Count - 1
            Dim btnEditar As LinkButton = grdOcuapacionDC3.Rows(i).Controls(5).Controls(0)
            Dim btnEliminar As LinkButton = grdOcuapacionDC3.Rows(i).Controls(6).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdOcuapacionDC3.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdOcuapacionDC3.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdOcuapacionDC3.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogosDNC').style.display = 'inline'")
            Next
        End If
    End Sub
    'elimina fila
    'valida si exite el proveedor 
    Public Function validaExiDc3(odbConexion As OleDbConnection, idCompetencia As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM [GC_GESTION_CAPACITACION_TB] WHERE [fk_id_clave_ocupacion_dc3]=" & idCompetencia.ToString
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
    'habilita el modo edicion
    Private Sub grdOcuapacionDC3_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdOcuapacionDC3.RowEditing
        grdOcuapacionDC3.ShowFooter = False
        grdOcuapacionDC3.EditIndex = e.NewEditIndex
        Call obtCatalogoOcupacionDC3()
    End Sub
    'actualiza la descripcion
    Private Sub grdOcuapacionDC3_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdOcuapacionDC3.RowUpdating
        grdOcuapacionDC3.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strDescripcion, strClave, strEstatus, strSeleccionar As String
        Try
            strId = DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strDescripcion = (DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strClave = (DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("txtClave"), TextBox).Text)
            strEstatus = (DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            strSeleccionar = (DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("ddlSeleccionar"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strClave = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la clave.');</script>", False)
                Exit Sub
            End If

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la descripción.');</script>", False)
                Exit Sub
            End If


            If validaDescripcionDc3(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe la descripción.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [GC_CT_CLAVE_OCUPACION_DC3] " & _
                        " SET [descripcion]= '" & strDescripcion & "'" & _
                        ",[clave] ='" & strClave & "' " & _
                        ",[estatus] ='" & strEstatus & "' " & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        ",seleccionar='" + strSeleccionar & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdOcuapacionDC3.EditIndex = -1
            Call obtCatalogoOcupacionDC3()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdOcuapacionDC3_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdOcuapacionDC3.PageIndexChanging
        Try
            grdOcuapacionDC3.ShowFooter = True
            grdOcuapacionDC3.EditIndex = -1
            grdOcuapacionDC3.PageIndex = e.NewPageIndex
            grdOcuapacionDC3.DataBind()
            Call obtCatalogoOcupacionDC3()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarOcupacionDc3_Click(sender As Object, e As EventArgs)
        Call insDescripcionDc3()
    End Sub
    Private Sub grdOcuapacionDC3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdOcuapacionDC3.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdOcuapacionDC3.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CT_CLAVE_OCUPACION_DC3 WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiDc3(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso de DNC.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdOcuapacionDC3.EditIndex = -1
            grdOcuapacionDC3.ShowFooter = True
            Call obtCatalogoOcupacionDC3()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
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