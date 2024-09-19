Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class parametrizacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorAnios.Text = ""
        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            Call obtenerUsuarioAD()
            Call obtCatalogoParametrizacion()

        End If
        Call Comportamientos()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogos"
    Private Sub ObtDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim strQuery As String = "SELECT * FROM DNC_PARAMETRIZACION_CT ORDER BY 2"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlDnc.DataSource = odbLector
            ddlDnc.DataValueField = "ID"
            ddlDnc.DataTextField = "nombre"

            ddlDnc.DataBind()
            odbConexion.Close()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Parametrización"
    ''obtiene el catalogo 
    Public Sub obtCatalogoParametrizacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_PARAMETRIZACION_CT   ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdParametrizacion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdParametrizacion.DataBind()

            If grdParametrizacion.Rows.Count = 0 Then
                Call insFilaVacia()
                grdParametrizacion.Rows(0).Visible = False


            Else
                grdParametrizacion.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If

            odbConexion.Close()


            For i = 0 To grdParametrizacion.Rows.Count - 1
                Dim iIdProveedor As String
                Dim ddlConsulta As DropDownList
                Dim strTipo As String = ""
                Dim btnEditar As LinkButton = grdParametrizacion.Rows(i).Controls(6).Controls(0)
                Dim btnEliminar As LinkButton = grdParametrizacion.Rows(i).Controls(7).Controls(1)

                iIdProveedor = DirectCast(grdParametrizacion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlConsulta = grdParametrizacion.Rows(i).Cells(1).FindControl("ddlConsultas")

                    'obtiene provedor

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdProveedor Then
                            strTipo = dsCatalogo.Tables(0).Rows(iContador)(4).ToString
                        End If
                    Next
                    ddlConsulta.SelectedValue = strTipo
                Else
                    'empleado
                    Dim lblConsulta As New Label
                    lblConsulta = grdParametrizacion.Rows(i).FindControl("lblConsultas")
                    lblConsulta.Text = IIf(lblConsulta.Text = "SI", "CONSULTAR CERRADO EL PERIODO", "NO CONSULTAR CERRADO EL PERIODO")

                End If

                For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                    If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdProveedor Then
                        If dsCatalogo.Tables(0).Rows(iContador)(5).ToString = "TERMINADO" Then
                            btnEliminar.Visible = False
                        End If
                    End If
                Next

            Next
            'colorea las celdas del grid
            For iFil As Integer = 0 To grdParametrizacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdParametrizacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            'carga años
            Call obtCatalogoAnios()
            Call ObtDNC()
            Call obtLimiteDnc()
            Call obtCatalogoParametros()
            Call obtPresupuesto() 'Obtiene el Presupuesto
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try


    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaNombre(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_PARAMETRIZACION_CT where (nombre='" & strNombre & "') ORDER BY 1"
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

    'valida si la descripcion ya existe en el catalogo
    Public Function validaRFC(strRFC As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_PARAMETRIZACION_CT where (rfc='" & strRFC & "') ORDER BY 1"
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

    'valida dnc
    Public Function validadnc() As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_PARAMETRIZACION_CT where ESTATUS='VIGENTE' ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            If odbLector(0) > 0 Then blResultado = True
        End If
        odbConexion.Close()
        Return blResultado

    End Function
    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVacia()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("nombre"))
        dt.Columns.Add(New DataColumn("fecha_registro_inicial"))
        dt.Columns.Add(New DataColumn("fecha_registro_final"))
        dt.Columns.Add(New DataColumn("consulta_registros"))
        dt.Columns.Add(New DataColumn("estatus"))


        dr = dt.NewRow
        dr("id") = ""
        dr("nombre") = ""
        dr("fecha_registro_inicial") = ""
        dr("fecha_registro_final") = ""
        dr("consulta_registros") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdParametrizacion.DataSource = dt.DefaultView
        grdParametrizacion.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionParametrizacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strNombre As String = ""
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strOpcionesConsulta As String = ""
        Dim strEstatus As String = ""
        Try
            strNombre = (DirectCast(grdParametrizacion.FooterRow.FindControl("txtAgregaNombre"), TextBox).Text)
            strFechaInicio = (DirectCast(grdParametrizacion.FooterRow.FindControl("txtAgreFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdParametrizacion.FooterRow.FindControl("txtAgreFechaFinal"), TextBox).Text)
            strOpcionesConsulta = (DirectCast(grdParametrizacion.FooterRow.FindControl("ddlAgreConsultas"), DropDownList).Text)
            strEstatus = (DirectCast(grdParametrizacion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If
            If validaNombre(strNombre, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe el nombre en otra DNC.');</script>", False)
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
            If CDate(strFechaInicio) >= CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Final.');</script>", False)
                Exit Sub
            End If

            If validadnc() Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede agregar hasta que se cierre la DNC VIGENTE.');</script>", False)
                Exit Sub
            End If



            strQuery = "INSERT INTO DNC_PARAMETRIZACION_CT (nombre,[fecha_registro_inicial],[fecha_registro_final],[consulta_registros],[estatus],[fecha_creacion],[usuario_creacion])" & _
                       " VALUES ('" & strNombre.ToUpper & "','" & strFechaInicio & "','" & strFechaFinal & "','" & strOpcionesConsulta & "','" & strEstatus & "'" & _
                      ",GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoParametrizacion()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
    Private Sub grdParametrizacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdParametrizacion.RowCancelingEdit
        grdParametrizacion.ShowFooter = True
        grdParametrizacion.EditIndex = -1
        Call obtCatalogoParametrizacion()
    End Sub

    'TOOLTIPS
    Private Sub grdParametrizacion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdParametrizacion.RowDataBound

        For i As Integer = 0 To grdParametrizacion.Rows.Count - 1

            Dim btnEditar As LinkButton = grdParametrizacion.Rows(i).Controls(6).Controls(0)
            Dim btnEliminar As LinkButton = grdParametrizacion.Rows(i).Controls(7).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdParametrizacion.Rows(i).Controls(1).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdParametrizacion.Rows(i).Controls(6).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdParametrizacion.Rows(i).Controls(1).Controls(1), TextBox).Text + "?')){ return false; };"
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


    'valida si exite el proveedor 
    Public Function validaExiCursos(odbConexion As OleDbConnection, idProveedor As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM DNC_CURSOS_TB WHERE [fk_id_parametrizacion]=" & idProveedor.ToString
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

    'valida si exite el proveedor 
    Public Function validaExiProveedor(odbConexion As OleDbConnection, idProveedor As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM DNC_PROVEEDORES_CT WHERE [fk_id_parametrizacion]=" & idProveedor.ToString
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
    Private Sub grdParametrizacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdParametrizacion.RowEditing
        grdParametrizacion.ShowFooter = False
        grdParametrizacion.EditIndex = e.NewEditIndex
        Call obtCatalogoParametrizacion()
    End Sub
    'actualiza la descripcion
    Private Sub grdParametrizacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdParametrizacion.RowUpdating
        grdParametrizacion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strNombre As String = ""
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strOpcionesConsulta As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strNombre = (DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("txtNombre"), TextBox).Text)
            strFechaInicio = (DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("txtFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("txtFechaFinal"), TextBox).Text)
            strOpcionesConsulta = (DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("ddlConsultas"), DropDownList).Text)
            strEstatus = (DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If
            If validaNombre(strNombre, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe el nombre en otro DNC.');</script>", False)
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
            If CDate(strFechaInicio) >= CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Final.');</script>", False)
                Exit Sub
            End If

            'If strEstatus = "TERMINADO" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>TerminaDNC();</script>", False)
            '    Exit Sub


            'End If
            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE [dbo].[DNC_PARAMETRIZACION_CT]" & _
                        "SET [nombre] = '" & strNombre.ToUpper & "'" & _
                        "   ,[fecha_registro_inicial] = '" & strFechaInicio & "'" & _
                        "   ,[fecha_registro_final] = '" & strFechaFinal & "'" & _
                        "   ,[consulta_registros] = '" & strOpcionesConsulta & "'" & _
                        "   ,[estatus] = '" & strEstatus & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdParametrizacion.EditIndex = -1
            Call obtCatalogoParametrizacion()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdParametrizacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdParametrizacion.PageIndexChanging
        Try
            grdParametrizacion.ShowFooter = True
            grdParametrizacion.EditIndex = -1
            grdParametrizacion.PageIndex = e.NewPageIndex
            grdParametrizacion.DataBind()
            Call obtCatalogoParametrizacion()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarCondicion_Click(sender As Object, e As EventArgs)
        Call insDescripcionParametrizacion()
    End Sub

    Private Sub grdParametrizacion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdParametrizacion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdParametrizacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_PARAMETRIZACION_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiProveedor(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
                Exit Sub
            End If
            'Cursos
            If validaExiCursos(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdParametrizacion.EditIndex = -1
            grdParametrizacion.ShowFooter = True
            Call obtCatalogoParametrizacion()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Catálogos de Anios Propuestos"
    'obtiene el catalogo 
    Public Sub obtCatalogoAnios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_ANIO_PROPUESTO_CT   ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdAnioPropuesto.DataSource = dsCatalogo.Tables(0).DefaultView
            grdAnioPropuesto.DataBind()

            If grdAnioPropuesto.Rows.Count = 0 Then
                Call insFilaFacilitador()
                grdAnioPropuesto.Rows(0).Visible = False


            Else
                grdAnioPropuesto.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If

            odbConexion.Close()

            For i = 0 To grdAnioPropuesto.Rows.Count - 1
                Dim iIdProveedor As String
                Dim ddlDnc As DropDownList
                Dim ddlEstatus As DropDownList
                Dim strTipo As String = ""
                Dim strEstatus As String = ""
                Dim btnEditar As LinkButton = grdAnioPropuesto.Rows(i).Controls(4).Controls(0)

                iIdProveedor = DirectCast(grdAnioPropuesto.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlDnc = grdAnioPropuesto.Rows(i).Cells(1).FindControl("ddlDnc")
                    ddlEstatus = grdAnioPropuesto.Rows(i).Cells(3).FindControl("ddlEstatus")
                    'obtiene provedor
                    Call obtddlDnc(ddlDnc)

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdProveedor Then
                            strTipo = dsCatalogo.Tables(0).Rows(iContador)(0).ToString
                            strEstatus = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                    ddlDnc.SelectedValue = strTipo
                    ddlEstatus.SelectedValue = strEstatus
                Else
                    'empleado
                    Dim lblDnc As New Label
                    lblDnc = grdAnioPropuesto.Rows(i).FindControl("lblDNC")
                    lblDnc.Text = obtTextoDNC(lblDnc.Text)

                    Dim lblEstatus As New Label
                    lblEstatus = grdAnioPropuesto.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "1", "Habilitado", "Deshabilitado")

                End If
            Next

            Dim ddlAgregaDNC As DropDownList
            ddlAgregaDNC = grdAnioPropuesto.FooterRow.FindControl("ddlAgreDnc")
            Call obtddlDnc(ddlAgregaDNC)
            'colorea las celdas del grid
            For iFil As Integer = 0 To grdAnioPropuesto.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdAnioPropuesto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdParametrizacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdParametrizacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorAnios.Text = ex.Message
        End Try


    End Sub

    'Obtiene el catalogo de proveedores
    Public Sub obtddlDnc(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT ID,NOMBRE FROM DNC_PARAMETRIZACION_CT WHERE ESTATUS='VIGENTE'  ORDER BY NOMBRE"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "NOMBRE"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoDNC(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT NOMBRE FROM DNC_PARAMETRIZACION_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function validaNombreAnio(strDescipcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_ANIO_PROPUESTO_CT where anio='" & strDescipcion & "' ORDER BY 1"
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
    Public Sub insFilaFacilitador()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("fk_id_parametrizacion"))
        dt.Columns.Add(New DataColumn("anio"))
        dt.Columns.Add(New DataColumn("estatus"))

        dr = dt.NewRow
        dr("id") = ""
        dr("fk_id_parametrizacion") = ""
        dr("anio") = ""
        dr("estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdAnioPropuesto.DataSource = dt.DefaultView
        grdAnioPropuesto.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insAnios()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDnc As String = ""
        Dim strNombre As String = ""
        Dim strEstatus As String = ""
        Try

            strDnc = (DirectCast(grdAnioPropuesto.FooterRow.FindControl("ddlAgreDnc"), DropDownList).Text)
            strNombre = (DirectCast(grdAnioPropuesto.FooterRow.FindControl("txtAgregaAnio"), TextBox).Text)
            strEstatus = (DirectCast(grdAnioPropuesto.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros
            If strDnc = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de estar Registrado una DNC.');</script>", False)
                Exit Sub
            End If

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Año.');</script>", False)
                Exit Sub
            End If

            If validaNombreAnio(strNombre) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El Año ya existe.');</script>", False)
                Exit Sub
            End If




            strQuery = "INSERT INTO DNC_ANIO_PROPUESTO_CT (fk_id_parametrizacion,anio,fecha_creacion,usuario_creacion,estatus)" & _
                       " VALUES ('" & strDnc.ToUpper & "','" & strNombre.ToUpper & "'," & _
                      "GETDATE(),'" & hdUsuario.Value & "'," & strEstatus & ")  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoAnios()
        Catch ex As Exception
            lblErrorAnios.Text = ex.Message
        End Try
    End Sub
    Private Sub grdAnioPropuesto_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdAnioPropuesto.RowCancelingEdit
        grdAnioPropuesto.ShowFooter = True
        grdAnioPropuesto.EditIndex = -1
        Call obtCatalogoAnios()
    End Sub

    'TOOLTIPS
    Private Sub grdAnioPropuesto_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdAnioPropuesto.RowDataBound

        For i As Integer = 0 To grdAnioPropuesto.Rows.Count - 1

            Dim editar As LinkButton = grdAnioPropuesto.Rows(i).Controls(4).Controls(0)
            Dim btnEl As LinkButton = grdAnioPropuesto.Rows(i).Controls(5).Controls(1)

            If editar.Text = "Editar" Then
                editar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdAnioPropuesto.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                editar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdAnioPropuesto.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdAnioPropuesto.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
    Private Sub grdAnioPropuesto_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdAnioPropuesto.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdAnioPropuesto.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_ANIO_PROPUESTO_CT WHERE ID=" & strId

            If validaCapacitaciones(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdAnioPropuesto.EditIndex = -1
            grdAnioPropuesto.ShowFooter = True
            Call obtCatalogoAnios()
        Catch ex As Exception
            lblErrorAnios.Text = ex.Message
        End Try

    End Sub

    'valida si exite en condiciones de unidad de Autos 
    Public Function validaCapacitaciones(odbConexion As OleDbConnection, strAnio As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(fk_id_anio) as tipo FROM [DNC_GESTION_CURSOS_TB] WHERE [fk_id_anio]=" & strAnio.ToString

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
    Private Sub grdAnioPropuesto_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdAnioPropuesto.RowEditing
        grdAnioPropuesto.ShowFooter = False
        grdAnioPropuesto.EditIndex = e.NewEditIndex
        Call obtCatalogoAnios()
    End Sub
    'actualiza la descripcion
    Private Sub grdAnioPropuesto_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdAnioPropuesto.RowUpdating
        grdAnioPropuesto.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDNC, strNombre As String
        Dim strId As String
        Dim strEstatus As String
        Try
            strId = DirectCast(grdAnioPropuesto.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDNC = (DirectCast(grdAnioPropuesto.Rows(e.RowIndex).FindControl("ddlDnc"), DropDownList).Text)
            strEstatus = (DirectCast(grdAnioPropuesto.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            strNombre = (DirectCast(grdAnioPropuesto.Rows(e.RowIndex).FindControl("txtAnio"), TextBox).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Año.');</script>", False)
                Exit Sub
            End If

            'If validaNombreFacilitador(strNombre, 1) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El nombre ya existe.');</script>", False)
            '    Exit Sub
            'End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE DNC_ANIO_PROPUESTO_CT " & _
                        "SET [fk_id_parametrizacion] = '" & strDNC & "'" & _
                         ",[anio] = '" & strNombre.ToUpper & "'" & _
                         ",[estatus] = '" & strEstatus & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdAnioPropuesto.EditIndex = -1
            Call obtCatalogoAnios()
        Catch ex As Exception
            lblErrorAnios.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdAnioPropuesto_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdAnioPropuesto.PageIndexChanging
        Try
            grdAnioPropuesto.ShowFooter = True
            grdAnioPropuesto.EditIndex = -1
            grdAnioPropuesto.PageIndex = e.NewPageIndex
            grdAnioPropuesto.DataBind()
            Call obtCatalogoAnios()
        Catch ex As Exception
            lblErrorAnios.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarFacilitador_Click(sender As Object, e As EventArgs)
        Call insAnios()
    End Sub
    'limia los label de erroresl
    'Public Sub limpiaLabel()
    '    lblErrorProveedor.Text = ""
    '    lblErrorFacilitador.Text = ""

    '    'colorea las celdas del grid
    '    For iFil As Integer = 0 To grdAnioPropuesto.Rows.Count - 1
    '        If iFil Mod 2 = 0 Then
    '            grdAnioPropuesto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
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

#Region "Grid Limite"
    'obtiene la información del
    Public Sub obtLimiteDnc()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_LIMITE_TB WHERE fk_id_parametrizacion=" & ddlDnc.SelectedValue & " ORDER BY [puesto]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdLimiteDnc.DataSource = dsCatalogo.Tables(0).DefaultView
            grdLimiteDnc.DataBind()

            If grdLimiteDnc.Rows.Count = 0 Then
                Call insFilaVaciaLimite()
                grdLimiteDnc.Rows(0).Visible = False

            Else
                grdLimiteDnc.Rows(0).Visible = True

            End If

            odbConexion.Close()


            'Dim i As Int16 = 0

            For i = 0 To grdLimiteDnc.Rows.Count - 1
                Dim iId As String
                Dim ddlOperativo As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdLimiteDnc.Rows(i).Controls(8).Controls(0)

                iId = DirectCast(grdLimiteDnc.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlOperativo = grdLimiteDnc.Rows(i).Cells(2).FindControl("ddlOperativo")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlOperativo.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdLimiteDnc.Rows(i).FindControl("lblOpertativo")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "No", "Sí")
                End If
            Next

            'Limite
            For iFil As Integer = 0 To grdLimiteDnc.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdLimiteDnc.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
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
        dt.Columns.Add(New DataColumn("puesto"))
        dt.Columns.Add(New DataColumn("numero_personas"))
        dt.Columns.Add(New DataColumn("cuota_preliminar"))
        dt.Columns.Add(New DataColumn("cuota_por_persona"))
        dt.Columns.Add(New DataColumn("horas_anuales"))
        dt.Columns.Add(New DataColumn("total"))
        dt.Columns.Add(New DataColumn("operativo"))


        dr = dt.NewRow
        dr("id") = ""
        dr("puesto") = ""
        dr("numero_personas") = ""
        dr("cuota_preliminar") = ""
        dr("cuota_por_persona") = ""
        dr("horas_anuales") = ""
        dr("total") = ""
        dr("operativo") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdLimiteDnc.DataSource = dt.DefaultView
        grdLimiteDnc.DataBind()
    End Sub

    'inserta registro al catalogo
    Public Sub insConcepto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strPuesto As String = ""
        Dim strNumPersona As String = ""
        Dim strCuotaPreliminar As String = ""
        Dim strCuotaPorSemana As String = ""
        Dim strHorasAnuales As String = ""
        Dim strTotal As String = ""
        Dim strOperativo As String = ""

        Try

            strPuesto = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgrePuesto"), TextBox).Text)
            strNumPersona = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgregaNumPersona"), TextBox).Text)
            strCuotaPreliminar = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgregarCuotaPre"), TextBox).Text)
            strCuotaPorSemana = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgregarCuotaPersona"), TextBox).Text)
            strHorasAnuales = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgregaHorasAnuales"), TextBox).Text)
            strTotal = (DirectCast(grdLimiteDnc.FooterRow.FindControl("txtAgregarTotal"), TextBox).Text)

            strOperativo = (DirectCast(grdLimiteDnc.FooterRow.FindControl("ddlAgreOperativo"), DropDownList).Text)

            odbConexion.Open()

            If strPuesto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el puesto.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuesto(strPuesto, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe ese puesto para esta DNC.');</script>", False)
                Exit Sub
            End If
            If strNumPersona = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el número de personas.');</script>", False)
                Exit Sub
            End If
            If strCuotaPreliminar = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la cuota preliminar.');</script>", False)
                Exit Sub
            End If
            If strCuotaPorSemana = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la cuota por semana.');</script>", False)
                Exit Sub
            End If
            If strHorasAnuales = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar las horas Anuales.');</script>", False)
                Exit Sub
            End If
            If strTotal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Total.');</script>", False)
                Exit Sub
            End If
            strQuery = "INSERT INTO DNC_LIMITE_TB (puesto,numero_personas ,cuota_preliminar" & _
                        ",cuota_por_persona,horas_anuales,total ,operativo,fecha_creacion ,usuario_creacion,fk_id_parametrizacion) VALUES ('" & _
                strPuesto & "'," & strNumPersona & "," & strCuotaPreliminar & _
                "," & strCuotaPorSemana & "," & strHorasAnuales & "," & strTotal & "," & strOperativo & ",GETDATE(),'" & hdUsuario.Value & "'," & ddlDnc.SelectedValue & ")"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtLimiteDnc()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdLimiteDnc_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdLimiteDnc.RowCancelingEdit
        grdLimiteDnc.ShowFooter = True
        grdLimiteDnc.EditIndex = -1
        Call obtLimiteDnc()
    End Sub

    'TOOLTIPS
    Private Sub grdLimiteDnc_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLimiteDnc.RowDataBound

        For i As Integer = 0 To grdLimiteDnc.Rows.Count - 1

            Dim btnEditar As LinkButton = grdLimiteDnc.Rows(i).Controls(8).Controls(0)
            Dim btnEliminar As LinkButton = grdLimiteDnc.Rows(i).Controls(9).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el puesto " + DirectCast(grdLimiteDnc.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdLimiteDnc.Rows(i).Controls(8).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el puesto " + DirectCast(grdLimiteDnc.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
    Private Sub grdLimiteDnc_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdLimiteDnc.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_LIMITE_TB WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdLimiteDnc.EditIndex = -1
            grdLimiteDnc.ShowFooter = True
            Call obtLimiteDnc()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    'habilita el modo edicion
    Private Sub grdLimiteDnc_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdLimiteDnc.RowEditing
        grdLimiteDnc.ShowFooter = False
        grdLimiteDnc.EditIndex = e.NewEditIndex
        Call obtLimiteDnc()
    End Sub
    'actualiza la descripcion
    Private Sub grdLimiteDnc_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdLimiteDnc.RowUpdating
        grdLimiteDnc.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Dim strPuesto As String = ""
        Dim strNumPersona As String = ""
        Dim strCuotaPreliminar As String = ""
        Dim strCuotaPorSemana As String = ""
        Dim strHorasAnuales As String = ""
        Dim strTotal As String = ""
        Dim strOperativo As String = ""

        Try
            strId = DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strPuesto = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtPuesto"), TextBox).Text)
            strNumPersona = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtNumPersona"), TextBox).Text)
            strCuotaPreliminar = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtCuotaPre"), TextBox).Text)
            strCuotaPorSemana = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtCuotaPersona"), TextBox).Text)
            strHorasAnuales = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtHorasAnuales"), TextBox).Text)
            strTotal = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("txtTotal"), TextBox).Text)
            strOperativo = (DirectCast(grdLimiteDnc.Rows(e.RowIndex).FindControl("ddlOperativo"), DropDownList).Text)
            If strPuesto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el puesto.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuesto(strPuesto, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe ese puesto para esta DNC.');</script>", False)
                Exit Sub
            End If
            If strNumPersona = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el número de personas.');</script>", False)
                Exit Sub
            End If
            If strCuotaPreliminar = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la cuota preliminar.');</script>", False)
                Exit Sub
            End If
            If strCuotaPorSemana = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la cuota por semana.');</script>", False)
                Exit Sub
            End If
            If strHorasAnuales = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar las horas Anuales.');</script>", False)
                Exit Sub
            End If
            If strTotal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Total.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DNC_LIMITE_TB " & _
                        "SET [puesto] = '" & strPuesto & "'" & _
                        ",[numero_personas] = " & strNumPersona & "" & _
                        ",[cuota_preliminar] = " & strCuotaPorSemana & "" & _
                        ",[cuota_por_persona] = " & strCuotaPorSemana & "" & _
                        ",[horas_anuales] = " & strHorasAnuales & "" & _
                        ",[total] = " & strTotal & "" & _
                        ",[operativo] = " & strOperativo & "" & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdLimiteDnc.EditIndex = -1
            Call obtLimiteDnc()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarConcepto_Click(sender As Object, e As EventArgs)
        Call insConcepto()
    End Sub

    Protected Sub grdLimiteDnc_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLimiteDnc.PageIndexChanging
        grdLimiteDnc.ShowFooter = True
        grdLimiteDnc.PageIndex = e.NewPageIndex
        grdLimiteDnc.DataBind()
        Call obtLimiteDnc()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionPuesto(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_LIMITE_TB where (puesto='" & strDescripcion & "' and fk_id_parametrizacion=" & ddlDnc.SelectedValue & ") ORDER BY 1"
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

    Private Sub ddlDnc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDnc.SelectedIndexChanged
        Call obtLimiteDnc()
        Call obtCatalogoParametros()
        Call obtPresupuesto()
    End Sub

#Region "Grid Parametros"
    'obtiene el catalogo 
    Public Sub obtCatalogoParametros()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_parametros_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@parametrizacion", ddlDnc.SelectedValue)

            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCatalogo)
            grdParametro.DataSource = dsCatalogo.Tables(0).DefaultView
            grdParametro.DataBind()

            odbConexion.Close()

            'Paramentros
            For iFil As Integer = 0 To grdParametro.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdParametro.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub



    Private Sub grdParametro_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdParametro.RowCancelingEdit
        grdParametro.ShowFooter = True
        grdParametro.EditIndex = -1
        Call obtCatalogoParametros()
    End Sub

    'habilita el modo edicion
    Private Sub grdParametro_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdParametro.RowEditing
        grdParametro.ShowFooter = False
        grdParametro.EditIndex = e.NewEditIndex
        Call obtCatalogoParametros()
    End Sub
    'actualiza la descripcion
    Private Sub grdParametro_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdParametro.RowUpdating
        grdParametro.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strNumeroCurso As String = ""
        Dim strAjusteInflacion As String = ""
        Try
            strId = DirectCast(grdParametro.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strNumeroCurso = DirectCast(grdParametro.Rows(e.RowIndex).FindControl("txtNoCursos"), TextBox).Text
            strAjusteInflacion = DirectCast(grdParametro.Rows(e.RowIndex).FindControl("txtAjusteI"), TextBox).Text

            If strNumeroCurso = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Número de Curso.');</script>", False)
                Exit Sub
            End If
            If strAjusteInflacion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Ajuste Inflación.');</script>", False)
                Exit Sub
            End If

            odbConexion.Open()

            strQuery = "UPDATE DNC_PARAMETROS_TB " & _
                        "SET numero_cursos='" & strNumeroCurso & "' " & _
                         ",ajuste_inflacion=" & strAjusteInflacion & _
                         ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdParametro.EditIndex = -1
            Call obtCatalogoParametros()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Comportamientos"
    Public Sub Comportamientos()
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdParametrizacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdParametrizacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next


        For iFil As Integer = 0 To grdLimiteDnc.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdLimiteDnc.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Paramentros
        For iFil As Integer = 0 To grdParametro.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdParametro.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To grdAnioPropuesto.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdAnioPropuesto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Presupuesto
        For iFil As Integer = 0 To GrdPresupuesto.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdPresupuesto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'Detalle
        For iFil As Integer = 0 To GrdPresupuestoDetalle.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                GrdPresupuestoDetalle.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
    End Sub
#End Region

#Region "Grid Presupuesto"
    'obtiene la información del
    Public Sub obtPresupuesto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)

            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCatalogo)
            GrdPresupuesto.DataSource = dsCatalogo.Tables(0).DefaultView
            GrdPresupuesto.DataBind()

            If GrdPresupuesto.Rows.Count = 0 Then
                Call insFilaVaciaPresupuestoEncabezado()
                GrdPresupuesto.Rows(0).Visible = False

            Else
                GrdPresupuesto.Rows(0).Visible = True

            End If

            odbConexion.Close()

            'Presupuesto
            For iFil As Integer = 0 To GrdPresupuesto.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    GrdPresupuesto.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            Call obtPresupuestoDetalle()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Public Sub insFilaVaciaPresupuestoEncabezado()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("presupuesto"))
        dt.Columns.Add(New DataColumn("monto"))
        dt.Columns.Add(New DataColumn("monto_pesos"))
        dt.Columns.Add(New DataColumn("comentarios"))



        dr = dt.NewRow
        dr("id") = ""
        dr("presupuesto") = ""
        dr("monto") = ""
        dr("monto_pesos") = ""
        dr("comentarios") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        GrdPresupuesto.DataSource = dt.DefaultView
        GrdPresupuesto.DataBind()
    End Sub
    Protected Sub lnkAgregarPresupuestoEncabezado_Click(sender As Object, e As EventArgs)
        Call insPresupuesto()
    End Sub
    'inserta registro al catalogo
    Public Sub insPresupuesto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strPresupesto As String = ""
        Dim strMontoPresupuesto As String = ""
        Dim strComentario As String = ""
        Try

            strPresupesto = (DirectCast(GrdPresupuesto.FooterRow.FindControl("txtAgrePresupuesto"), TextBox).Text)
            strMontoPresupuesto = (DirectCast(GrdPresupuesto.FooterRow.FindControl("txtAgreMontoPresupuestos"), TextBox).Text)
            strComentario = (DirectCast(GrdPresupuesto.FooterRow.FindControl("txtAgreComentario"), TextBox).Text)

            odbConexion.Open()


            If strMontoPresupuesto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el monto.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdPresupesto", 0)
            odbComando.Parameters.AddWithValue("@Presupuesto", strPresupesto)
            odbComando.Parameters.AddWithValue("@Pmonto", IIf(strMontoPresupuesto = "", "0", strMontoPresupuesto))
            odbComando.Parameters.AddWithValue("@Pcomentario", strComentario)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtPresupuesto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub GrdPresupuesto_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GrdPresupuesto.RowCancelingEdit
        GrdPresupuesto.ShowFooter = True
        GrdPresupuesto.EditIndex = -1
        Call obtPresupuesto()
    End Sub

    'TOOLTIPS
    Private Sub GrdPresupuesto_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdPresupuesto.RowDataBound

        For i As Integer = 0 To GrdPresupuesto.Rows.Count - 1

            Dim btnEditar As LinkButton = GrdPresupuesto.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = GrdPresupuesto.Rows(i).Controls(5).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el presupuesto ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = GrdPresupuesto.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el presupuesto ?')){ return false; };"
            End If
        Next
    End Sub

    'elimina fila
    Private Sub GrdPresupuesto_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdPresupuesto.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdPresupuesto.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_del_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdPresupesto", strId)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdPresupuesto.EditIndex = -1
            GrdPresupuesto.ShowFooter = True
            Call obtPresupuesto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'habilita el modo edicion
    Private Sub GrdPresupuesto_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdPresupuesto.RowEditing
        GrdPresupuesto.ShowFooter = False
        GrdPresupuesto.EditIndex = e.NewEditIndex
        hdIndexPresupuesto.Value = e.NewEditIndex
        Call obtPresupuesto()
    End Sub
    'actualiza la descripcion
    Private Sub GrdPresupuesto_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GrdPresupuesto.RowUpdating
        GrdPresupuesto.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strPresupesto As String = ""
        Dim strMontoPresupuesto As String = ""
        Dim strComentario As String = ""
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdPresupuesto.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strPresupesto = (DirectCast(GrdPresupuesto.Rows(e.RowIndex).FindControl("txtPresupuesto"), TextBox).Text)
            strMontoPresupuesto = (DirectCast(GrdPresupuesto.Rows(e.RowIndex).FindControl("txtMontoPresupuestos"), TextBox).Text)
            strComentario = (DirectCast(GrdPresupuesto.Rows(e.RowIndex).FindControl("txtComentario"), TextBox).Text)

            odbConexion.Open()


            If strMontoPresupuesto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar las horas Anuales.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdPresupesto", strId)
            odbComando.Parameters.AddWithValue("@Presupuesto", strPresupesto)
            odbComando.Parameters.AddWithValue("@Pmonto", IIf(strMontoPresupuesto = "", "0", strMontoPresupuesto))
            odbComando.Parameters.AddWithValue("@Pcomentario", strComentario)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdPresupuesto.EditIndex = -1
            Call obtPresupuesto()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub GrdPresupuesto_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdPresupuesto.PageIndexChanging
        GrdPresupuesto.ShowFooter = True
        GrdPresupuesto.PageIndex = e.NewPageIndex
        GrdPresupuesto.DataBind()
        Call obtPresupuesto()
    End Sub
#End Region
#Region "Grid Presupuesto Detalle"
    Protected Sub ddlPor_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Asigna Valores de Colaboradores 
        Dim ddlAgrePor As DropDownList
        ddlAgrePor = GrdPresupuestoDetalle.Rows(hdIndexPresupuesto.Value).FindControl("ddlPor")
        'Agrega Area
        Dim ddlAgreArea As DropDownList
        ddlAgreArea = GrdPresupuestoDetalle.Rows(hdIndexPresupuesto.Value).FindControl("ddlArea")
        Call ObtArea(ddlAgrePor, ddlAgreArea)
    End Sub

    Protected Sub ddlAgrePor_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Asigna Valores de Colaboradores 
        Dim ddlAgrePor As DropDownList
        ddlAgrePor = GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgrePor")
        'Agrega Area
        Dim ddlAgreArea As DropDownList
        ddlAgreArea = GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgreArea")
        Call ObtArea(ddlAgrePor, ddlAgreArea)

    End Sub
    'obtiene la información del
    Public Sub obtPresupuestoDetalle()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_detalle_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)

            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCatalogo)
            GrdPresupuestoDetalle.DataSource = dsCatalogo.Tables(0).DefaultView
            GrdPresupuestoDetalle.DataBind()

            If GrdPresupuestoDetalle.Rows.Count = 0 Then
                Call insFilaVaciaPresupuesto()
                GrdPresupuestoDetalle.Rows(0).Visible = False
            Else
                GrdPresupuestoDetalle.Rows(0).Visible = True
            End If

            lblTotal.Text = "Monto Total: " & ObtienePresupestoTotal()
            odbConexion.Close()


            'Dim i As Int16 = 0

            For i = 0 To GrdPresupuestoDetalle.Rows.Count - 1
                Dim iId As String
                Dim ddlAplica As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = GrdPresupuestoDetalle.Rows(i).Controls(7).Controls(0)

                iId = DirectCast(GrdPresupuestoDetalle.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then

                    ddlAplica = GrdPresupuestoDetalle.Rows(i).FindControl("ddlAplica")

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlAplica.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(3).ToString
                        End If
                    Next
                End If
            Next

            'Asigna Valores de Colaboradores 
            Dim ddlAgrePor As DropDownList
            ddlAgrePor = GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgrePor")
            'Agrega Area
            Dim ddlAgreArea As DropDownList
            ddlAgreArea = GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgreArea")
            Call ObtArea(ddlAgrePor, ddlAgreArea)

            Dim ddlAgrePresupuesto As DropDownList
            ddlAgrePresupuesto = GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgrePresupuesto")
            Call obtPresupuesto(ddlAgrePresupuesto)
            'Detalle
            For iFil As Integer = 0 To GrdPresupuestoDetalle.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    GrdPresupuestoDetalle.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'Obtiene el area o Gerencia
    Public Sub ObtArea(ddlPor As DropDownList, ddlArea As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)

        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_area_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PPor", ddlPor.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlArea.DataSource = odbLector
            ddlArea.DataValueField = "CLAVE"
            ddlArea.DataTextField = "DESCRIPCION"

            ddlArea.DataBind()
            '   ddlArea.Items.Insert(0, New ListItem("Seleccionar", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'Obtiene el Presupuesto
    Public Sub ObtPresupuesto(ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)

        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros

            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddl.DataSource = odbLector
            ddl.DataValueField = "ID"
            ddl.DataTextField = "presupuesto"

            ddl.DataBind()
            If ddl.Items.Count = 0 Then
                ddl.Items.Insert(0, New ListItem("No hay", 0))
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaPresupuesto()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("por"))
        dt.Columns.Add(New DataColumn("area"))
        dt.Columns.Add(New DataColumn("APLICA"))
        dt.Columns.Add(New DataColumn("monto"))
        dt.Columns.Add(New DataColumn("monto_porcentaje"))
        dt.Columns.Add(New DataColumn("Presupuesto"))
        dt.Columns.Add(New DataColumn("porcentaje"))

        dr = dt.NewRow
        dr("id") = ""
        dr("por") = ""
        dr("area") = ""
        dr("APLICA") = ""
        dr("monto") = ""
        dr("monto_porcentaje") = ""
        dr("Presupuesto") = ""
        dr("porcentaje") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        GrdPresupuestoDetalle.DataSource = dt.DefaultView
        GrdPresupuestoDetalle.DataBind()
    End Sub

    'inserta registro al catalogo
    Public Sub insPresupuestoDetalle()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strPresupuesto As String = ""
        Dim strPor As String = ""
        Dim strArea As String = ""
        Dim strAplica As String = ""
        Dim strMontoPorcentaje As String = ""

        Try
            strPresupuesto = (DirectCast(GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgrePresupuesto"), DropDownList).Text)
            strPor = (DirectCast(GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgrePor"), DropDownList).Text)
            strArea = (DirectCast(GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgreArea"), DropDownList).Text)
            strAplica = (DirectCast(GrdPresupuestoDetalle.FooterRow.FindControl("ddlAgreAplica"), DropDownList).Text)
            strMontoPorcentaje = (DirectCast(GrdPresupuestoDetalle.FooterRow.FindControl("txtAgrePorcentaje"), TextBox).Text)


            odbConexion.Open()

            If strPresupuesto = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar Presupuesto.');</script>", False)
                Exit Sub
            End If

            If strMontoPorcentaje = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el monto.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_detalle_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdPresupesto", 0)
            odbComando.Parameters.AddWithValue("@PPresupuesto", strPresupuesto)
            odbComando.Parameters.AddWithValue("@Ppor_direccion_a", IIf(strPor = "DirA", 1, 0))
            odbComando.Parameters.AddWithValue("@Ppor_direccion_b", IIf(strPor = "DirB", 1, 0))
            odbComando.Parameters.AddWithValue("@Ppor_gerencia", IIf(strPor = "Ger", 1, 0))
            odbComando.Parameters.AddWithValue("@Pfk_id_direccion_a", IIf(strPor = "DirA", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Pfk_id_direccion_b", IIf(strPor = "DirB", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Pfk_id_gerencia", IIf(strPor = "Ger", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Padministrativos", IIf(strAplica = "Todos" Or strAplica = "Administrativos", "1", "0"))
            odbComando.Parameters.AddWithValue("@Poperativos", IIf(strAplica = "Todos" Or strAplica = "Opertativos", "1", "0"))
            odbComando.Parameters.AddWithValue("@PmontoPorcentaje", strMontoPorcentaje)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtPresupuestoDetalle()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub GrdPresupuestoDetalle_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GrdPresupuestoDetalle.RowCancelingEdit
        GrdPresupuestoDetalle.ShowFooter = True
        GrdPresupuestoDetalle.EditIndex = -1
        Call obtPresupuestoDetalle()
    End Sub

    'TOOLTIPS
    Private Sub GrdPresupuestoDetalle_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdPresupuestoDetalle.RowDataBound

        For i As Integer = 0 To GrdPresupuestoDetalle.Rows.Count - 1

            Dim btnEditar As LinkButton = GrdPresupuestoDetalle.Rows(i).Controls(7).Controls(0)
            Dim btnEliminar As LinkButton = GrdPresupuestoDetalle.Rows(i).Controls(8).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el presupuesto ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = GrdPresupuestoDetalle.Rows(i).Controls(7).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el presupuesto ?')){ return false; };"
            End If
        Next


    End Sub

    'Obtiene el monto total del presupuesto
    Public Function ObtienePresupestoTotal() As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        odbConexion.Open()
        strQuery = "SELECT CONVERT(VARCHAR, CAST(ISNULL(sum(monto),0) AS MONEY), 1) FROM GC_CG_PRESUPUESTO WHERE fk_id_parametrizacion=" & IIf(ddlDnc.SelectedValue = "", "0", ddlDnc.SelectedValue)
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If
        odbConexion.Close()
        Return strResultado

    End Function
    'elimina fila
    Private Sub GrdPresupuestoDetalle_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdPresupuestoDetalle.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM GC_CG_PRESUPUESTO WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdPresupuestoDetalle.EditIndex = -1
            GrdPresupuestoDetalle.ShowFooter = True
            Call obtPresupuestoDetalle()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'habilita el modo edicion
    Private Sub GrdPresupuestoDetalle_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdPresupuestoDetalle.RowEditing
        GrdPresupuestoDetalle.ShowFooter = False
        GrdPresupuestoDetalle.EditIndex = e.NewEditIndex
        hdIndexPresupuesto.Value = e.NewEditIndex
        Call obtPresupuestoDetalle()
    End Sub
    'actualiza la descripcion
    Private Sub GrdPresupuestoDetalle_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GrdPresupuestoDetalle.RowUpdating
        GrdPresupuestoDetalle.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strPor As String = ""
        Dim strArea As String = ""
        Dim strAplica As String = ""
        Dim strMontoPresupuesto As String = ""
        Dim strComentario As String = ""
        Dim strId As String = ""
        Try
            strId = DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'strPor = (DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("ddlPor"), DropDownList).Text)
            'strArea = (DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("ddlArea"), DropDownList).Text)
            strAplica = (DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("ddlAplica"), DropDownList).Text)
            strMontoPresupuesto = (DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("txtMontoPresupuestos"), TextBox).Text)
            strComentario = (DirectCast(GrdPresupuestoDetalle.Rows(e.RowIndex).FindControl("txtComentario"), TextBox).Text)

            odbConexion.Open()


            If strMontoPresupuesto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar monto.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cg_presupuesto_detalle_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PIdParametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdPresupesto", strId)
            odbComando.Parameters.AddWithValue("@PPresupuesto", 0)
            odbComando.Parameters.AddWithValue("@Ppor_direccion_a", IIf(strPor = "DirA", 1, 0))
            odbComando.Parameters.AddWithValue("@Ppor_direccion_b", IIf(strPor = "DirB", 1, 0))
            odbComando.Parameters.AddWithValue("@Ppor_gerencia", IIf(strPor = "Ger", 1, 0))
            odbComando.Parameters.AddWithValue("@Pfk_id_direccion_a", IIf(strPor = "DirA", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Pfk_id_direccion_b", IIf(strPor = "DirB", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Pfk_id_gerencia", IIf(strPor = "Ger", strArea, "0"))
            odbComando.Parameters.AddWithValue("@Padministrativos", IIf(strAplica = "Todos" Or strAplica = "Administrativos", "1", "0"))
            odbComando.Parameters.AddWithValue("@Poperativos", IIf(strAplica = "Todos" Or strAplica = "Opertativos", "1", "0"))
            odbComando.Parameters.AddWithValue("@Pmonto", IIf(strMontoPresupuesto = "", "0", strMontoPresupuesto))
            odbComando.Parameters.AddWithValue("@Pcomentario", strComentario)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            GrdPresupuestoDetalle.EditIndex = -1
            Call obtPresupuestoDetalle()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Protected Sub lnkAgregarPresupuesto_Click(sender As Object, e As EventArgs)
        Call insPresupuestoDetalle()
    End Sub

    Protected Sub GrdPresupuestoDetalle_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdPresupuestoDetalle.PageIndexChanging
        GrdPresupuestoDetalle.ShowFooter = True
        GrdPresupuestoDetalle.PageIndex = e.NewPageIndex
        GrdPresupuestoDetalle.DataBind()
        Call obtPresupuestoDetalle()
    End Sub

#End Region



End Class