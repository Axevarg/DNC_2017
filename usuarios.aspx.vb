Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class usuarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call obtCatalogo()
        End If
        Call comportamientos()
    End Sub
#Region "Grid"
    'obtiene el catalogo 
    Public Sub obtCatalogo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""

        'VALIDA COMPORTAMIENTO DE MENU

        Try
            odbConexion.Open()

            strQuery = "SELECT * FROM SIGIDO_USUARIOS_TB " & strFiltro & " ORDER BY nombre "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdUsuarios.DataSource = dsCatalogo.Tables(0).DefaultView
            grdUsuarios.DataBind()

            If grdUsuarios.Rows.Count = 0 Then
                Call insFilaVacia()
                grdUsuarios.Rows(0).Visible = False

            Else
                grdUsuarios.Rows(0).Visible = True
            End If

            odbConexion.Close()


            Dim i As Int16 = 0
            For i = 0 To grdUsuarios.Rows.Count - 1
                Dim iIdEmpleado As String
                Dim ddlRol As DropDownList
                Dim iEmpleado As String = ""
                Dim strRol As String = ""
                'inabilita los combos
                Dim btnEditar As LinkButton = grdUsuarios.Rows(i).Controls(5).Controls(0)

                If btnEditar.Text <> "Editar" Then
                    ddlRol = grdUsuarios.Rows(i).Cells(4).FindControl("ddlRol")

                    'llena los combos
                    Call obtddlPerfiles(ddlRol)

                    iIdEmpleado = DirectCast(grdUsuarios.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                    'ciclo para obtener el indice del combo
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdEmpleado Then
                            iEmpleado = dsCatalogo.Tables(0).Rows(iContador)(1).ToString
                            strRol = dsCatalogo.Tables(0).Rows(iContador)(5).ToString
                        End If
                    Next
                    ddlRol.SelectedValue = strRol
                Else

                    Dim lblRol As New Label
                    lblRol = grdUsuarios.Rows(i).FindControl("lblRol")
                    lblRol.Text = obtTextoPerfiles(lblRol.Text)
                End If

            Next
            'claves empleado

            Dim ddlAgregaRol As DropDownList
            ddlAgregaRol = grdUsuarios.FooterRow.FindControl("ddlAgreRol")
            Call obtddlPerfiles(ddlAgregaRol)
            'si es jefe se elimina el perfil de administarador

            'colorea 
            For iFil As Integer = 0 To grdUsuarios.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdUsuarios.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub
    Public Sub obtddlPerfiles(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT ID,[descripcion] FROM SIGIDO_PERFILES_CT WHERE ESTATUS=1  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoPerfiles(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM SIGIDO_PERFILES_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVacia()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("nombre"))
        dt.Columns.Add(New DataColumn("id_padre"))
        dt.Columns.Add(New DataColumn("usuario"))
        dt.Columns.Add(New DataColumn("proveedor"))
        dt.Columns.Add(New DataColumn("rol"))


        dr = dt.NewRow
        dr("id") = ""
        dr("clave") = ""
        dr("nombre") = ""
        dr("id_padre") = ""
        dr("usuario") = ""
        dr("proveedor") = ""
        dr("rol") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdUsuarios.DataSource = dt.DefaultView
        grdUsuarios.DataBind()


    End Sub
    'inserta registro al catalogo
    Public Sub insDescripcion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String

        Dim strClave As String = ""
        Dim strNombre As String = ""
        Dim strUsuario As String = ""
        Dim strRol As String = ""


        Try
            strClave = (DirectCast(grdUsuarios.FooterRow.FindControl("txtAgrNomina"), TextBox).Text)
            strNombre = (DirectCast(grdUsuarios.FooterRow.FindControl("txtAgregarNom"), TextBox).Text)
            strUsuario = (DirectCast(grdUsuarios.FooterRow.FindControl("txtAgregarUsuario"), TextBox).Text)


            strRol = (DirectCast(grdUsuarios.FooterRow.FindControl("ddlAgreRol"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros
            'valida clave

            If strClave = "" Or strClave = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El No. Nomina no puede estar vacio.');</script>", False)
                Exit Sub
            End If

            If validaColaborador(strClave) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe un colaborador con el No. Nomina " & strClave & " en el sistema.');</script>", False)
                Exit Sub
            End If

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If
            If strUsuario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el usuario de Windows.');</script>", False)
                Exit Sub
            End If
            If validaColaborador(strUsuario) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe un colaborador con ese usuario " & strUsuario & ".');</script>", False)
                Exit Sub
            End If

            If strRol = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el rol de Usuario');</script>", False)
                Exit Sub
            End If





            strQuery = "INSERT INTO SIGIDO_USUARIOS_TB ([clave],[nombre],[usuario],[rol],[fecha_creacion],[usuario_creacion])" & _
                       " VALUES (" & IIf(strClave = "", "NULL", strClave) & "," & _
                       "'" & strNombre & "', " & _
                       "'" & strUsuario.ToLower & "', " & _
                       "'" & strRol & "',GETDATE(),'" & hdUsuario.Value & "')  SELECT @@IDENTITY "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim idObjetivo As Integer
            idObjetivo = Convert.ToInt32(odbComando.ExecuteScalar())


            odbConexion.Close()

            Call obtCatalogo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'CANCELAR EDICION
    Private Sub grdUsuarios_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdUsuarios.RowCancelingEdit
        grdUsuarios.ShowFooter = True
        grdUsuarios.EditIndex = -1
        Call obtCatalogo()
    End Sub

    'TOOLTIPS
    Private Sub grdUsuarios_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUsuarios.RowDataBound

        For i As Integer = 0 To grdUsuarios.Rows.Count - 1

            Dim editar As LinkButton = grdUsuarios.Rows(i).Controls(5).Controls(0)
            Dim btnEl As LinkButton = grdUsuarios.Rows(i).Controls(6).Controls(1)

            If editar.Text = "Editar" Then
                editar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el Colaborador " + DirectCast(grdUsuarios.Rows(i).Controls(2).Controls(1), Label).Text + "?')){ return false; };"
            Else
                editar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdUsuarios.Rows(i).Controls(5).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEl.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el Colaborador " + DirectCast(grdUsuarios.Rows(i).Controls(2).Controls(1), TextBox).Text + "?')){ return false; };"
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
    Private Sub grdUsuarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdUsuarios.RowDeleting
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            Dim strQuery As String
            Dim strId As String = ""
            Dim strRol As String = ""

            strId = DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text


            If strId = "" Then Exit Sub

            strRol = obtRol(strId)
            odbConexion.Open()

            strQuery = "DELETE FROM SIGIDO_USUARIOS_TB WHERE ID=" & strId

            If validaAdministrador(strRol) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque el sistema requiere un administrador.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdUsuarios.EditIndex = -1
            Call obtCatalogo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'habilita el modo edicion
    Private Sub grdUsuarios_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdUsuarios.RowEditing
        grdUsuarios.ShowFooter = False
        grdUsuarios.EditIndex = e.NewEditIndex
        Call obtCatalogo()
    End Sub
    'actualiza la descripcion
    Private Sub grdUsuarios_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdUsuarios.RowUpdating
        grdUsuarios.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strClave As String = ""
        Dim strNombre As String = ""
        Dim strUsuario As String = ""
        Dim strRol As String = ""
        Dim strId As String

        Try
            strId = DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strClave = (DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("txtNomina"), TextBox).Text)
            strNombre = (DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("txtNombre"), TextBox).Text)
            strUsuario = (DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("txtUsuario"), TextBox).Text)
            strRol = (DirectCast(grdUsuarios.Rows(e.RowIndex).FindControl("ddlRol"), DropDownList).Text)

            odbConexion.Open()

            'validaciones para insertar registros

            If strClave = "" Or strClave = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('El No. Nomina no puede estar vacio.');</script>", False)
                Exit Sub
            End If

            If validaColaborador(strClave, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe un colaborador con el No. Nomina " & strClave & " en el sistema.');</script>", False)
                Exit Sub
            End If

            If strNombre = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nombre.');</script>", False)
                Exit Sub
            End If
            'vaida que el colaborador no se pueda asociar a el mismo como jefe
            If strUsuario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el usuario de Windows.');</script>", False)
                Exit Sub
            End If
            If validaColaborador(strUsuario, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe un colaborador con ese usuario " & strUsuario & ".');</script>", False)
                Exit Sub
            End If

            If strRol = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el rol de Usuario');</script>", False)
                Exit Sub
            End If



            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = "UPDATE SIGIDO_USUARIOS_TB " & _
                        "SET clave=" & IIf(strClave = "", "NULL", strClave) & _
                        ",nombre='" & strNombre & "'" & _
                        ",rol='" & strRol.ToUpper & "'" & _
                        ",usuario='" & strUsuario.ToLower & "'" & _
                        ",fecha_modificacion=GETDATE()" & _
                        ",usuario_modificacion='" & hdUsuario.Value & "'" & _
                        " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdUsuarios.EditIndex = -1

            Call obtCatalogo()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida que el colaborador no este dado de alta
    Public Function validaColaborador(ByVal buscar As String, Optional ByVal CLAVE As Integer = 0) As Boolean
        Dim blnResultado As Boolean = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""
        Dim odbLector As OleDbDataReader

        strFiltro = IIf(IsNumeric(buscar), "clave=" & buscar, "usuario='" & buscar & "'")

        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM SIGIDO_USUARIOS_TB WHERE ( " & strFiltro & " ) " & IIf(CLAVE > 0, "AND ID<>" & CLAVE, "")

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0) > 0, True, False)
        End If

        odbConexion.Close()
        Return blnResultado
    End Function

    'valida que el colaborador no este dado de alta
    Public Function validaAdministrador(ByVal strRol As String) As Boolean
        Dim blnResultado As Boolean = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strFiltro As String = ""
        Dim odbLector As OleDbDataReader
        If strRol = "A" Then

            odbConexion.Open()

            strQuery = " SELECT COUNT(*) FROM SIGIDO_USUARIOS_TB WHERE ROL='A' "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                blnResultado = IIf(odbLector(0) = 1, True, False)
            End If

            odbConexion.Close()
        End If

        Return blnResultado
    End Function

    'valida que el colaborador no este dado de alta
    Public Function validaRegistro(ByVal strRol As String) As Boolean
        Dim blnResultado As Boolean = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strFiltro As String = ""
        Dim odbLector As OleDbDataReader
        If strRol = "A" Then

            odbConexion.Open()

            strQuery = " SELECT COUNT(*) FROM SIGIDO_USUARIOS_TB WHERE ROL='A' "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                blnResultado = IIf(odbLector(0) = 1, True, False)
            End If

            odbConexion.Close()
        End If

        Return blnResultado
    End Function

    Public Function obtRol(ByVal strId As String) As String
        Dim strResultado As String = "U"
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strFiltro As String = ""
        Dim odbLector As OleDbDataReader


        odbConexion.Open()

        strQuery = " SELECT rol FROM SIGIDO_USUARIOS_TB WHERE id=" & strId

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
        End If

        odbConexion.Close()


        Return strResultado
    End Function

    'AGREGA ELEMENTO
    Protected Sub lnkAgregar_Click(sender As Object, e As EventArgs)
        Call insDescripcion()
    End Sub

    Protected Sub grdUsuarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdUsuarios.PageIndexChanging
        grdUsuarios.ShowFooter = True
        grdUsuarios.EditIndex = -1
        grdUsuarios.PageIndex = e.NewPageIndex
        grdUsuarios.DataBind()
        Call obtCatalogo()

    End Sub
    Protected Sub lnkAgregar_Click1(sender As Object, e As EventArgs)
        Call insDescripcion()
    End Sub
#End Region
#Region "Comportamientos"

    Public Sub comportamientos()
        lblError.Text = ""

        'CONDICIONES
        For iFil As Integer = 0 To grdUsuarios.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdUsuarios.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
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

#End Region
End Class