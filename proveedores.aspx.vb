
Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class proveedores
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        Call limpiaLabel()
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call ObtServiciosPrestados()
            Call obtCatalogoProveedores()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
    End Sub

#Region "Catalogo"
    Private Sub ObtServiciosPrestados()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()

            Dim strQuery As String = "SELECT * FROM SIGIDO_SERVICIOS_PROVEEDOR_CT ORDER BY 2"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlServiciosPrestados.DataSource = odbLector
            ddlServiciosPrestados.DataValueField = "ID"
            ddlServiciosPrestados.DataTextField = "descripcion"

            ddlServiciosPrestados.DataBind()
            'valida si los item estan vacios
            odbConexion.Close()
        Catch ex As Exception
            lblErrorProveedor.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
#End Region
#Region "Catálogos de Proveedores"
    ''obtiene el catalogo 
    Public Sub obtCatalogoProveedores()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM SIGIDO_PROVEEDORES_CT   ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdProveedores.DataSource = dsCatalogo.Tables(0).DefaultView
            grdProveedores.DataBind()

            If grdProveedores.Rows.Count = 0 Then
                Call insFilaVacia()
                grdProveedores.Rows(0).Visible = False


            Else
                grdProveedores.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If




            For i = 0 To grdProveedores.Rows.Count - 1
                Dim iIdProveedor As String
                Dim ddlTipoProveedor As DropDownList
                Dim strTipo As String = ""
                Dim btnEditar As LinkButton = grdProveedores.Rows(i).Controls(5).Controls(0)
                Dim btnEdicion As LinkButton
                iIdProveedor = DirectCast(grdProveedores.Rows(i).Cells(2).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlTipoProveedor = grdProveedores.Rows(i).Cells(2).FindControl("ddlTipo")


                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdProveedor Then
                            strTipo = dsCatalogo.Tables(0).Rows(iContador)(3).ToString
                        End If
                    Next
                    ddlTipoProveedor.SelectedValue = strTipo
                Else
                    btnEdicion = grdProveedores.Rows(i).Controls(1).Controls(0)
                    'modal
                    btnEdicion.ToolTip = "Datos Proveedor"
                    btnEdicion.Attributes.Add("data-toggle", "modal")
                    btnEdicion.Attributes.Add("data-target", "#modalProveedor")
                    Call obtDatosAdmonProveedor(iIdProveedor, odbConexion, btnEdicion)
                End If


            Next

            'carga Grid de Facilitadores
            Call obtCatalogoFacilitadores()
            'colorea las celdas del grid
            For iFil As Integer = 0 To grdProveedores.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdProveedores.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            odbConexion.Close()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try


    End Sub
    'obtien la informacion de los datos del proveedor
    Public Sub obtDatosAdmonProveedor(strId As String, odbConexion As OleDbConnection, btnEdicion As LinkButton)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strServicios As String = ""
        strQuery = "SELECT * FROM [SIGIDO_PROVEEDORES_CT] where (id=" & IIf(strId = "", 0, strId) & ") ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update

            txtDomicilio.Text = odbLector(9).ToString
            txtTelefono.Text = odbLector(10).ToString
            txtPersonaContacto.Text = odbLector(11).ToString
            txtTelefonoContacto.Text = odbLector(12).ToString
            txtPuestoContacto.Text = odbLector(13).ToString
            txtDiasCreditos.Text = odbLector(14).ToString
            ddlAvisoPrivacidad.SelectedValue = odbLector(15).ToString
            ddlConvenio.SelectedValue = odbLector(16).ToString
            txtNombreComercialInstitucion.Text = odbLector(18).ToString
            'ddlServiciosPrestados.SelectedValue = odbLector(17).ToString
            'convierte el resultado en un arreglo
            Dim result() As String = Split(odbLector(17).ToString.Replace("-", ","), ",")
            'ciclo de servicios para selecionar informacion
            strServicios = " document.getElementById('hdIdProveedor').value=" & IIf(strId = "", 0, strId) & "; " & _
                           " document.getElementById('lblProveedores').innerHTML='" & odbLector(1).ToString & "'; " & _
                           " var listBox = document.getElementById('ddlServiciosPrestados');"
            'Selected en False

            For iServicio = 0 To ddlServiciosPrestados.Items.Count - 1
                strServicios += " listBox.options[" & iServicio & "].selected = false;"
            Next


            For i = 0 To result.Count - 1
                For iServicio = 0 To ddlServiciosPrestados.Items.Count - 1
                    If ddlServiciosPrestados.Items(iServicio).Value = result(i) Then
                        strServicios += " listBox.options[" & iServicio & "].selected = true;"
                    End If
                Next
            Next
            strServicios += " combo(); "
            btnEdicion.Attributes.Add("onclick", " obtDatosProveedor('" & txtDomicilio.Text & "','" & txtTelefono.Text & _
                                        "','" & txtPersonaContacto.Text & "','" & txtTelefonoContacto.Text & "','" & _
                                            txtPuestoContacto.Text & "','" & txtDiasCreditos.Text & "','" & _
                                            ddlAvisoPrivacidad.SelectedIndex & "','" & ddlConvenio.SelectedIndex & "','" & txtNombreComercialInstitucion.Text & "'); " & strServicios & " ")

            odbLector.Close()
        End If

    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaNombre(strNombre As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM SIGIDO_PROVEEDORES_CT where (nombre='" & strNombre & "') ORDER BY 1"
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
        strQuery = "SELECT COUNT(*) FROM SIGIDO_PROVEEDORES_CT where (rfc='" & strRFC & "') ORDER BY 1"
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

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVacia()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("nombre"))
        dt.Columns.Add(New DataColumn("rfc"))
        dt.Columns.Add(New DataColumn("tipo_proveedor"))



        dr = dt.NewRow
        dr("id") = ""
        dr("nombre") = ""
        dr("rfc") = ""
        dr("tipo_proveedor") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdProveedores.DataSource = dt.DefaultView
        grdProveedores.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionProveedor()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strNombre As String = ""
        Dim strRfc As String = ""
        Dim strTipoProveedor As String = ""
        Try
            strNombre = (DirectCast(grdProveedores.FooterRow.FindControl("txtAgregaNombre"), TextBox).Text)
            strRfc = (DirectCast(grdProveedores.FooterRow.FindControl("txtAgregarRfc"), TextBox).Text)
            strTipoProveedor = (DirectCast(grdProveedores.FooterRow.FindControl("ddlAgreTipo"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If

            If strRfc = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el RFC.');</script>", False)
                Exit Sub
            End If


            If validaNombre(strNombre, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe el nombre en otro Proveedor.');</script>", False)
                Exit Sub
            End If


            strQuery = "INSERT INTO SIGIDO_PROVEEDORES_CT (nombre,rfc,tipo_proveedor,fecha_creacion,usuario_creacion,fk_id_parametrizacion)" & _
                       " VALUES ('" & strNombre & "','" & strRfc & "','" & strTipoProveedor & "'," & _
                      "GETDATE(),'" & hdUsuario.Value & "'," & IIf(hdIdDNC.Value = "", 0, hdIdDNC.Value) & ")  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoProveedores()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
    Private Sub grdProveedores_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdProveedores.RowCancelingEdit
        grdProveedores.ShowFooter = True
        grdProveedores.EditIndex = -1
        Call obtCatalogoProveedores()
    End Sub

    Private Sub grdProveedores_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdProveedores.RowDataBound

        For i As Integer = 0 To grdProveedores.Rows.Count - 1
            Dim btnEditar As LinkButton = grdProveedores.Rows(i).Controls(5).Controls(0)
            Dim btnEliminar As LinkButton = grdProveedores.Rows(i).Controls(6).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar"
                'Elimina registros
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdProveedores.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar"
                Dim cancelar As LinkButton = grdProveedores.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdProveedores.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
    Public Function validaExiProveedor(odbConexion As OleDbConnection, idProveedor As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM DNC_CURSOS_TB WHERE [fk_id_proveedor]=" & idProveedor.ToString
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
    Public Function validaExiProveedorFacilitador(odbConexion As OleDbConnection, idProveedor As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) as tipo FROM DNC_FACILITADORES_CT WHERE [fk_id_proveedor]=" & idProveedor.ToString
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
    Private Sub grdProveedores_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdProveedores.RowEditing
        grdProveedores.ShowFooter = False
        grdProveedores.EditIndex = e.NewEditIndex
        Call obtCatalogoProveedores()
    End Sub
    'actualiza la descripcion
    Private Sub grdProveedores_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdProveedores.RowUpdating
        grdProveedores.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String
        Dim strNombre, strRfc, strTipoProveedor As String
        Try
            strId = DirectCast(grdProveedores.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strNombre = (DirectCast(grdProveedores.Rows(e.RowIndex).FindControl("txtNombre"), TextBox).Text)
            strRfc = (DirectCast(grdProveedores.Rows(e.RowIndex).FindControl("txtRfc"), TextBox).Text)
            strTipoProveedor = (DirectCast(grdProveedores.Rows(e.RowIndex).FindControl("ddlTipo"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If

            If strRfc = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el RFC.');</script>", False)
                Exit Sub
            End If

            If validaNombre(strNombre, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe el nombre en otro Proveedor.');</script>", False)
                Exit Sub
            End If

            'If validaRFC(strRfc, 1) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe el RFC en otro Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [SIGIDO_PROVEEDORES_CT] " & _
                        " SET [nombre] = '" & strNombre & "'" & _
                        ",[rfc] ='" & strRfc & "' " & _
                        ",[tipo_proveedor] = '" & strTipoProveedor & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdProveedores.EditIndex = -1
            Call obtCatalogoProveedores()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdProveedores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdProveedores.PageIndexChanging
        Try
            grdProveedores.ShowFooter = True
            grdProveedores.EditIndex = -1
            grdProveedores.PageIndex = e.NewPageIndex
            grdProveedores.DataBind()
            Call obtCatalogoProveedores()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarCondicion_Click(sender As Object, e As EventArgs)
        Call insDescripcionProveedor()
    End Sub

    Private Sub grdProveedores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdProveedores.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdProveedores.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM SIGIDO_PROVEEDORES_CT WHERE ID=" & strId
            'valida en Facilitadores
            If validaExiProveedorFacilitador(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Facilitador.');</script>", False)
                Exit Sub
            End If
            'Cursos
            If validaExiProveedor(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdProveedores.EditIndex = -1
            grdProveedores.ShowFooter = True
            Call obtCatalogoProveedores()
        Catch ex As Exception
            lblErrorProveedor.Text = ex.Message
        End Try
    End Sub

    Public Sub udpProveedores()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim strServicio As String = ""
        Try
            odbConexion.Open()
            For Each item As ListItem In ddlServiciosPrestados.Items
                If item.Selected Then
                    strServicio += item.Value + "-"
                End If
            Next
            If Len(strServicio) > 0 Then strServicio = strServicio.Substring(0, strServicio.LastIndexOf("-"))

            strQuery = " UPDATE [dbo].[SIGIDO_PROVEEDORES_CT]   " & _
            "            SET [domicilio] = '" & txtDomicilio.Text & "'" & _
            "           ,[telefono] = " & IIf(txtTelefono.Text = "", 0, txtTelefono.Text) & "" & _
            "           ,[persona_contacto] = '" & txtPersonaContacto.Text & "'" & _
            "           ,[telefono_contacto] = '" & IIf(txtTelefonoContacto.Text = "", 0, txtTelefonoContacto.Text) & "'" & _
            "           ,[puesto_contacto] = '" & txtPuestoContacto.Text & "'" & _
            "           ,[dias_credito] = '" & IIf(txtDiasCreditos.Text = "", 0, txtDiasCreditos.Text) & "'" & _
            "           ,[aviso_privacidad] = '" & ddlAvisoPrivacidad.SelectedValue & "'" & _
            "           ,[convenio_confidencialidad] = '" & ddlConvenio.SelectedValue & "'" & _
            "           ,[fk_id_servicios_proveedor] = '" & strServicio & "'" & _
            "           ,[nombre_comercial] = '" & txtNombreComercialInstitucion.Text & "'" & _
            "           ,[fecha_modificacion] = GETDATE()      " & _
            "           ,[usuario_modificacion] ='" & hdUsuario.Value & "'" & _
            " WHERE ID=" & hdIdProveedor.Value.ToString


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            Call obtCatalogoProveedores()
        Catch ex As Exception
            lblErrorProveedorModal.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Private Sub btnGuardarInfoProveedor_ServerClick(sender As Object, e As EventArgs) Handles btnGuardarInfoProveedor.ServerClick
        Call udpProveedores()
    End Sub
#End Region

#Region "Catálogos de Facilitadores"
    'obtiene el catalogo 
    Public Sub obtCatalogoFacilitadores()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM DNC_FACILITADORES_CT   ORDER BY 2 ASC  "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdFacilitador.DataSource = dsCatalogo.Tables(0).DefaultView
            grdFacilitador.DataBind()

            If grdFacilitador.Rows.Count = 0 Then
                Call insFilaFacilitador()
                grdFacilitador.Rows(0).Visible = False


            Else
                grdFacilitador.Rows(0).Visible = True
                '   lblEstatus.Text = ""
            End If

            odbConexion.Close()

            For i = 0 To grdFacilitador.Rows.Count - 1
                Dim iIdProveedor As String
                Dim ddlProveedor As DropDownList
                Dim strTipo As String = ""
                Dim btnEditar As LinkButton = grdFacilitador.Rows(i).Controls(3).Controls(0)

                iIdProveedor = DirectCast(grdFacilitador.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlProveedor = grdFacilitador.Rows(i).Cells(1).FindControl("ddlProveedor")

                    'obtiene provedor
                    Call obtddlProveedor(ddlProveedor)

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdProveedor Then
                            strTipo = dsCatalogo.Tables(0).Rows(iContador)(1).ToString
                        End If
                    Next
                    ddlProveedor.SelectedValue = strTipo
                Else
                    'empleado
                    Dim lblProveedor As New Label
                    lblProveedor = grdFacilitador.Rows(i).FindControl("lblProveedor")
                    lblProveedor.Text = obtTextoProveedor(lblProveedor.Text)

                End If
            Next

            Dim ddlAgregarProveedor As DropDownList
            ddlAgregarProveedor = grdFacilitador.FooterRow.FindControl("ddlAgreProveedor")
            Call obtddlProveedor(ddlAgregarProveedor)
            'colorea las celdas del grid
            For iFil As Integer = 0 To grdFacilitador.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdFacilitador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblErrorFacilitador.Text = ex.Message
        End Try


    End Sub

    'Obtiene el catalogo de proveedores
    Public Sub obtddlProveedor(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT ID,NOMBRE FROM SIGIDO_PROVEEDORES_CT  ORDER BY NOMBRE"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "NOMBRE"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoProveedor(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT NOMBRE FROM SIGIDO_PROVEEDORES_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function validaNombreFacilitador(strDescipcion As String, provider_ As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_FACILITADORES_CT where nombre='" & strDescipcion & "' AND [fk_id_proveedor]='" & provider_ & "' ORDER BY 1"
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


    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaFacilitador()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("fk_id_proveedor"))
        dt.Columns.Add(New DataColumn("nombre"))

        dr = dt.NewRow
        dr("id") = ""
        dr("fk_id_proveedor") = ""
        dr("nombre") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdFacilitador.DataSource = dt.DefaultView
        grdFacilitador.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insFacilitadores()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strProveedor As String = ""
        Dim strNombre As String = ""

        Try

            strProveedor = (DirectCast(grdFacilitador.FooterRow.FindControl("ddlAgreProveedor"), DropDownList).Text)
            strNombre = (DirectCast(grdFacilitador.FooterRow.FindControl("txtAgregaNombre"), TextBox).Text)

            odbConexion.Open()
            'validaciones para insertar registros
            If strProveedor = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de estar Registrado un Proveedor en el Catálogo de Proveedores.');</script>", False)
                Exit Sub
            End If

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If

            If validaNombreFacilitador(strNombre, strProveedor) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El nombre ya existe.');</script>", False)
                Exit Sub
            End If




            strQuery = "INSERT INTO DNC_FACILITADORES_CT (fk_id_proveedor,nombre,fecha_creacion,usuario_creacion)" & _
                       " VALUES ('" & strProveedor & "','" & strNombre & "'," & _
                      "GETDATE(),'" & hdUsuario.Value & "')  "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoFacilitadores()
        Catch ex As Exception
            lblErrorFacilitador.Text = ex.Message
        End Try
    End Sub
    Private Sub grdFacilitador_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdFacilitador.RowCancelingEdit
        grdFacilitador.ShowFooter = True
        grdFacilitador.EditIndex = -1
        Call obtCatalogoFacilitadores()
    End Sub

    'TOOLTIPS
    Private Sub grdFacilitador_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdFacilitador.RowDataBound

        For i As Integer = 0 To grdFacilitador.Rows.Count - 1

            Dim editar As LinkButton = grdFacilitador.Rows(i).Controls(3).Controls(0)
            Dim btnEl As LinkButton = grdFacilitador.Rows(i).Controls(4).Controls(1)

            If editar.Text = "Editar" Then
                editar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdFacilitador.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                editar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdFacilitador.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + DirectCast(grdFacilitador.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
    Private Sub grdFacilitador_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdFacilitador.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdFacilitador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM DNC_FACILITADORES_CT WHERE ID=" & strId

            If validaExiCursos(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso.');</script>", False)
                Exit Sub
            End If
            'Valida Gestion del Curso
            If validaExiGestionCursos(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Curso Gestionado.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdFacilitador.EditIndex = -1
            grdFacilitador.ShowFooter = True
            Call obtCatalogoFacilitadores()
        Catch ex As Exception
            lblErrorFacilitador.Text = ex.Message
        End Try

    End Sub

    'valida si exite en condiciones de unidad de Autos 
    Public Function validaExiCursos(odbConexion As OleDbConnection, strFacilitador As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(fk_id_facilitador) as tipo FROM DNC_CURSOS_TB WHERE [fk_id_facilitador]=" & strFacilitador.ToString

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

    'valida si exite en condiciones de unidad de Autos 
    Public Function validaExiGestionCursos(odbConexion As OleDbConnection, strFacilitador As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(fk_id_facilitador) as tipo FROM GC_GESTION_CAPACITACION_TB WHERE [fk_id_facilitador]=" & strFacilitador.ToString

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
    Private Sub grdFacilitador_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdFacilitador.RowEditing
        grdFacilitador.ShowFooter = False
        grdFacilitador.EditIndex = e.NewEditIndex
        Call obtCatalogoFacilitadores()
    End Sub
    'actualiza la descripcion
    Private Sub grdFacilitador_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdFacilitador.RowUpdating
        grdFacilitador.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strProveedor, strNombre As String
        Dim strId As String

        Try
            strId = DirectCast(grdFacilitador.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strProveedor = (DirectCast(grdFacilitador.Rows(e.RowIndex).FindControl("ddlProveedor"), DropDownList).Text)
            strNombre = (DirectCast(grdFacilitador.Rows(e.RowIndex).FindControl("txtNombre"), TextBox).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If

            'If validaNombreFacilitador(strNombre, 1) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El nombre ya existe.');</script>", False)
            '    Exit Sub
            'End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE DNC_FACILITADORES_CT " & _
                        "SET [fk_id_proveedor] = '" & strProveedor & "'" & _
                         ",[nombre] = '" & strNombre & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdFacilitador.EditIndex = -1
            Call obtCatalogoFacilitadores()
        Catch ex As Exception
            lblErrorFacilitador.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdFacilitador_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdFacilitador.PageIndexChanging
        Try
            grdFacilitador.ShowFooter = True
            grdFacilitador.EditIndex = -1
            grdFacilitador.PageIndex = e.NewPageIndex
            grdFacilitador.DataBind()
            Call obtCatalogoFacilitadores()
        Catch ex As Exception
            lblErrorFacilitador.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarFacilitador_Click(sender As Object, e As EventArgs)
        Call insFacilitadores()
    End Sub
    'limia los label de erroresl
    Public Sub limpiaLabel()
        lblErrorProveedor.Text = ""
        lblErrorFacilitador.Text = ""
        lblErrorProveedorModal.Text = ""
        lblError.Text = ""
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdFacilitador.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdFacilitador.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdProveedores.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdProveedores.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
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

    Protected Sub grdFacilitador_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

#End Region
End Class