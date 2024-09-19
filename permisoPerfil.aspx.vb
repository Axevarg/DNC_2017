Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class permisoPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"

        lblError.Text = ""
        'If Session("matricula") <> "" And Session("nombre") <> "" And Session("usuario") <> "" Then
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call CargaCatalogos()
            Call obtCatalogoPerfiles()
            divModulos.Visible = False
        End If
        'Else
        '    Response.Redirect("index.aspx")
        'End If
        'Call Comportamientos()
    End Sub

#Region "Catalogos"

    Public Sub CargaCatalogos()
        Call obtPerfiles()
    End Sub
    Public Sub obtPerfiles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try

            odbConexion.Open()

            strQuery = " SELECT 0 as ID,' Seleccionar' as descripcion  union all SELECT ID,descripcion FROM SIGIDO_PERFILES_CT WHERE estatus=1 ORDER BY 2"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlPerfil.DataSource = odbLector
            ddlPerfil.DataTextField = "descripcion"
            ddlPerfil.DataValueField = "ID"

            ddlPerfil.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Sub Comportamientos()
        'Perfiles
        For iFil As Integer = 0 To grdPerfiles.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPerfiles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

    End Sub
#End Region
#Region "TreeView"
    Private Sub ddlPerfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPerfil.SelectedIndexChanged
        Call obtModulos()
    End Sub
    'carga treeview
    Public Sub obtModulos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Dim tn As TreeNode = Nothing
        Dim arrArreglo As New ArrayList
        Try
            'valida si es seleccionar
            If ddlPerfil.SelectedValue > 0 Then
                odbConexion.Open()


                Dim odbComando As New OleDbCommand
                odbComando.CommandText = "muestra_modulos_sp"
                odbComando.Connection = odbConexion
                odbComando.CommandType = CommandType.StoredProcedure

                'parametros
                odbComando.Parameters.AddWithValue("@idPerifl", ddlPerfil.SelectedValue)
                odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)


                Dim odbAdaptador As New OleDbDataAdapter
                odbAdaptador.SelectCommand = odbComando
                odbAdaptador.Fill(dsDatos)

                TreModulos.Nodes.Clear()

                'crea treeview
                Call crearNodosArbol(0, tn, odbConexion, dsDatos)
                arrArreglo = validaAdministradorSinAcceso(odbConexion)
                odbConexion.Close()
                'valida si es administrador o Sin acceso
                If arrArreglo.Item(0).ToString = "1" Or arrArreglo.Item(1).ToString = "1" Then
                    btnGuardarAccesos.Visible = False
                Else
                    btnGuardarAccesos.Visible = True
                End If
                divModulos.Visible = True
            Else
                divModulos.Visible = False
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub
    'valida la si es administradir
    Public Function validaAdministradorSinAcceso(odbConexion As OleDbConnection) As ArrayList
        Dim dsCatalogo As New DataSet
        Dim arrArreglo As New ArrayList
        Dim strQuery As String = ""
        Dim odbLector As OleDbDataReader


        strQuery = " SELECT [administrador],[sin_acceso] FROM SIGIDO_PERFILES_CT  WHERE ID=" & ddlPerfil.SelectedValue

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            arrArreglo.Add(odbLector(0).ToString)
            arrArreglo.Add(odbLector(1).ToString)

            odbLector.Close()
        Else
            arrArreglo.Add(0)
            arrArreglo.Add(0)
        End If


        Return arrArreglo
    End Function

    'crea arbol del treeview
    Public Sub crearNodosArbol(ByVal iPadre As Integer, tn As TreeNode, ByVal odbConexion As OleDbConnection, dsDatos As DataSet)


        Dim dataviewHijo As New DataView(dsDatos.Tables(0))
        'filtro por el id de padre
        dataviewHijo.RowFilter = dsDatos.Tables(0).Columns(1).ColumnName + "=" + iPadre.ToString

        For Each dataRowCurrent As DataRowView In dataviewHijo
            Dim nuevoNodo As New TreeNode
            nuevoNodo.SelectAction = TreeNodeSelectAction.Expand

            nuevoNodo.Text = "<span><i class='fa fa-globe'></i> " & dataRowCurrent("nombre").ToString().Trim() & "</span>"
            nuevoNodo.Value = dataRowCurrent("ID_PERMISO").ToString().Trim()
            'valida si el modulo esta activo o inactivo
            nuevoNodo.Checked = IIf(dataRowCurrent("estatus").ToString().Trim() = "1", True, False)
            'si es vacio es nodo hijo si no padre
            If tn Is Nothing Then
                nuevoNodo.Expand()
                TreModulos.Nodes.Add(nuevoNodo)
            Else
                nuevoNodo.Expand()
                tn.ChildNodes.Add(nuevoNodo)
            End If

            'llamada redundante del metodo para crear el arbol
            Call crearNodosArbol(Int32.Parse(dataRowCurrent("modulo").ToString()), nuevoNodo, odbConexion, dsDatos)
        Next
    End Sub
#End Region

#Region "Guarda Permisos"
    Private Sub RecorrerNodos(ByVal n As TreeNode)
        System.Diagnostics.Debug.WriteLine(n.Text)
        '  MsgBox(n.Text)
        Dim aNode As TreeNode
        For Each aNode In n.ChildNodes
            RecorrerNodos(aNode)

            Call ActualizaEstatusModulos(aNode.Value, aNode.Checked)
        Next


    End Sub

    ' Call the procedure using the top nodes of the treeview.
    Private Sub CallRecursive(ByVal aTreeView As TreeView)
        Dim n As TreeNode
        For Each n In aTreeView.Nodes
            Call ActualizaEstatusModulos(n.Value, n.Checked)
            RecorrerNodos(n)
        Next
    End Sub

    Private Sub btnGuardarAccesos_ServerClick(sender As Object, e As EventArgs) Handles btnGuardarAccesos.ServerClick
        Call CallRecursive(TreModulos)
        Call obtModulos()

    End Sub
    'Actualiza el estatus
    Public Sub ActualizaEstatusModulos(strId As String, blnSeleccion As Boolean)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()

            strQuery = "UPDATE [SIGIDO_PERMISOS_PERFIL_TB] " &
                       " SET estatus=" & IIf(blnSeleccion, "1", "0") & _
                       " ,fecha_modificacion=GETDATE()" & _
                       " ,usuario_modificacion='" & hdUsuario.Value & "'" & _
                       " WHERE ID=" & strId

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Grid Perfiles"
    'obtiene el Perfil Configurados 
    Public Sub obtCatalogoPerfiles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM [dbo].[SIGIDO_PERFILES_CT] ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPerfiles.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPerfiles.DataBind()

            If grdPerfiles.Rows.Count = 0 Then
                Call insVaciasDescripcionPerfiles()
                grdPerfiles.Rows(0).Visible = False

            Else
                grdPerfiles.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdPerfiles.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdPerfiles.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdPerfiles.Rows(i).Controls(4).Controls(1)
                iId = DirectCast(grdPerfiles.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdPerfiles.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdPerfiles.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
                'valida si es administrador o sin acceso
                For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                    If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                        If dsCatalogo.Tables(0).Rows(iContador)(7).ToString = "1" Or dsCatalogo.Tables(0).Rows(iContador)(8).ToString = "1" Then
                            btnEliminar.Visible = False
                            btnEditar.Visible = False
                            Exit For
                        End If
                    End If
                Next

            Next

            'Perfiles
            For iFil As Integer = 0 To grdPerfiles.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPerfiles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            Call CargaCatalogos()
            divModulos.Visible = False
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insVaciasDescripcionPerfiles()
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
        grdPerfiles.DataSource = dt.DefaultView
        grdPerfiles.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionPerfiles()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdPerfiles.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdPerfiles.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
                Exit Sub
            End If

            If validaDescripcionPerfiles(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO [dbo].[SIGIDO_PERFILES_CT] (descripcion,estatus,fecha_creacion,usuario_creacion,administrador,sin_acceso) VALUES ('" & _
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "',0,0)"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPerfiles()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdPerfiles_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPerfiles.RowCancelingEdit
        grdPerfiles.ShowFooter = True
        grdPerfiles.EditIndex = -1
        Call obtCatalogoPerfiles()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
    End Sub
    'TOOLTIPS
    Private Sub grdPerfiles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPerfiles.RowDataBound

        For i As Integer = 0 To grdPerfiles.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPerfiles.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdPerfiles.Rows(i).Controls(4).Controls(1)
            btnEditar.Attributes("onclick") = " ModalOcultar();"
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPerfiles.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };  ModalOcultar();"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdPerfiles.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                cancelar.Attributes("onclick") = " ModalOcultar();"
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdPerfiles.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };  ModalOcultar();"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingPagina').style.display = 'inline'; ModalOcultar();")
            Next
        End If
    End Sub
    'elimina fila
    Private Sub grdPerfiles_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPerfiles.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPerfiles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaEliminarPerfiles(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Usuario.');</script>", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM [dbo].[SIGIDO_PERFILES_CT] WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPerfiles.EditIndex = -1
            grdPerfiles.ShowFooter = True
            Call obtCatalogoPerfiles()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaEliminarPerfiles(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [SIGIDO_USUARIOS_TB_] where rol=" & id.ToString
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
    Private Sub grdPerfiles_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPerfiles.RowEditing
        grdPerfiles.ShowFooter = False
        grdPerfiles.EditIndex = e.NewEditIndex
        Call obtCatalogoPerfiles()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
    End Sub
    'actualiza la descripcion
    Private Sub grdPerfiles_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPerfiles.RowUpdating
        grdPerfiles.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdPerfiles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdPerfiles.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdPerfiles.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
                Exit Sub
            End If

            If validaDescripcionPerfiles(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE [dbo].[SIGIDO_PERFILES_CT] " & _
                        "SET DESCRIPCION='" & strDescripcion & "' " & _
                         ",ESTATUS=" & strEstatus & _
                        ",fecha_modificacion=GETDATE() " & _
                        ",usuario_modificacion='" & hdUsuario.Value & "' " & _
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPerfiles.EditIndex = -1
            Call obtCatalogoPerfiles()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarPerfil_Click(sender As Object, e As EventArgs)
        Call insDescripcionPerfiles()
    End Sub

    Protected Sub grdPerfiles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPerfiles.PageIndexChanging
        grdPerfiles.ShowFooter = True
        grdPerfiles.PageIndex = e.NewPageIndex
        grdPerfiles.DataBind()
        Call obtCatalogoPerfiles()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Modal", "<script>Modal()</script>", False)
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionPerfiles(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM [dbo].[SIGIDO_PERFILES_CT] where (descripcion='" & strDescripcion & "') ORDER BY 1"
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
End Class