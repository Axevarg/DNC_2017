Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Public Class PlanCapacitacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorPlan.Text = ""
        lblMensaje.Text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call CargaCatalogos()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Comportamientos", "<script>ComportamientosJS()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "GridView", "<script>gridviewScroll()</script>", False)
        Call Comportamientos()
    End Sub
#Region "Catalogos"

    Public Sub CargaCatalogos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Call ObtDNC(odbConexion)
            ddlDireccion.Items.Insert(0, New ListItem("Seleccionar DNC", 0))
            ddlDepartamento.Items.Insert(0, New ListItem("Seleccionar Dirección", 0))
            ddlCursos.Items.Insert(0, New ListItem("Todos", 0))
            'Plan de Ejecucion
            ddlDireccionPlan.Items.Insert(0, New ListItem("Seleccionar DNC", 0))
            ddlDepartamentoPlan.Items.Insert(0, New ListItem("Seleccionar Dirección", 0))
            ddlCursosPlan.Items.Insert(0, New ListItem("Todos", 0))
            odbConexion.Close()
            divProgramar.Visible = False
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub ObtDNC(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT * FROM DNC_PARAMETRIZACION_CT ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlDnc.DataSource = odbLector
        ddlDnc.DataValueField = "ID"
        ddlDnc.DataTextField = "nombre"

        ddlDnc.DataBind()
        ddlDnc.Items.Insert(0, New ListItem("Seleccionar", 0))
        'DncPlan de Capacitación
        odbComando = New OleDbCommand(strQuery, odbConexion)
        Dim odbLectorPlan As OleDbDataReader
        odbLectorPlan = odbComando.ExecuteReader()

        ddlDncPlanCapacitacion.DataSource = odbLectorPlan
        ddlDncPlanCapacitacion.DataValueField = "ID"
        ddlDncPlanCapacitacion.DataTextField = "nombre"

        ddlDncPlanCapacitacion.DataBind()
        ddlDncPlanCapacitacion.Items.Insert(0, New ListItem("Seleccionar", 0))
    End Sub
    Public Sub obtDireccion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_direcciones_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDnc.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlDireccion.DataSource = odbLector
            ddlDireccion.DataTextField = "DIRECCION"
            ddlDireccion.DataValueField = "DIRECCION"

            ddlDireccion.DataBind()
            ddlDireccion.Items.Insert(0, New ListItem("Todas", 0))
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Public Sub obtGerenciaDepto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gerencia_depto_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccion.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlDepartamento.DataSource = odbLector
            ddlDepartamento.DataTextField = "DEPARTAMENTO"
            ddlDepartamento.DataValueField = "DEPARTAMENTO"
            ddlDepartamento.DataBind()
            ddlDepartamento.Items.Insert(0, New ListItem("Todos", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub ObtCursoRegistrados()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_plan_capacitacion_cursos_solicitados_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccion.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PDeptoCia", ddlDepartamento.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PIdCurso", 0)
            odbComando.Parameters.AddWithValue("@PEstatus", 0)
            odbComando.Parameters.AddWithValue("@PTipoSalida", 0)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlCursos.DataSource = odbLector
            ddlCursos.DataTextField = "CURSO"
            ddlCursos.DataValueField = "ID"
            ddlCursos.DataBind()
            ddlCursos.Items.Insert(0, New ListItem("Todos", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub ddlDireccion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDireccion.SelectedIndexChanged
        ddlEstatus.SelectedValue = "Todos"
        Call obtGerenciaDepto()
        Call ObtCursoRegistrados()
        Call ObtCursosRegistradosColaborador()
    End Sub

    Private Sub ddlDnc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDnc.SelectedIndexChanged
        ddlEstatus.SelectedValue = "Todos"
        Call obtDireccion()
        Call obtGerenciaDepto()
        Call ObtCursoRegistrados()
        Call ObtCursosRegistradosColaborador()
    End Sub

    Private Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        ddlEstatus.SelectedValue = "Todos"
        Call ObtCursoRegistrados()
        Call ObtCursosRegistradosColaborador()
    End Sub
    Private Sub ddlCursos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCursos.SelectedIndexChanged
        ddlEstatus.SelectedValue = "Todos"
        Call ObtCursosRegistradosColaborador()
    End Sub
    Private Sub ddlEstatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEstatus.SelectedIndexChanged
        Call ObtCursosRegistradosColaborador()
    End Sub
#End Region
#Region "COmportamientos"
    Public Sub Comportamientos()
        For iFil As Integer = 0 To grdCursosRegistrados.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCursosRegistrados.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To GrdPlanCapacitacion.Rows.Count - 1
            'Color Celdas
            If iFil Mod 2 = 0 Then
                GrdPlanCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
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
#Region "Cursos Solicitados"
    Public Sub ObtCursosRegistradosColaborador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet

        Try

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_plan_capacitacion_cursos_solicitados_sel_sp_prueba"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccion.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PDeptoCia", ddlDepartamento.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PIdCurso", ddlCursos.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PEstatus", ddlEstatus.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PTipoSalida", 1)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            Dim odbLector As New OleDbDataAdapter
            odbLector.SelectCommand = odbComando
            odbLector.Fill(dsDatos)

            grdCursosRegistrados.DataSource = dsDatos.Tables(0).DefaultView
            grdCursosRegistrados.DataBind()

            lblTotal.Text = "Total: " & grdCursosRegistrados.Rows.Count.ToString
            odbConexion.Close()

            For iFil As Integer = 0 To grdCursosRegistrados.Rows.Count - 1
                Dim strEstatus As String = ""
                strEstatus = grdCursosRegistrados.Rows(iFil).Cells(3).Text
                grdCursosRegistrados.Rows(iFil).Cells(3).Text = IIf(strEstatus = "0", "<span class='label label-warning'>Solicitado</span>", "<span class='label label-success'>Autorizado</span>")
                CType(grdCursosRegistrados.Rows(iFil).FindControl("chkClave"), CheckBox).Checked = IIf(strEstatus = "0", False, True)
                'Color Celdas
                If iFil Mod 2 = 0 Then
                    grdCursosRegistrados.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            GC.Collect()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Protected Sub lnkGuardarAutorizacion_Click(sender As Object, e As EventArgs)
        Call GuardaCursosAutorizados()
    End Sub
    Private Sub GuardaCursosAutorizados()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim chkColaborador As Boolean = False
        Dim strColaborador As String = ""
        Dim strIdCursoDNC As String = ""
        Dim strCIdCurso As String = ""
        Try

            odbConexion.Open()

            'Prestados
            For i As Integer = 0 To grdCursosRegistrados.Rows.Count - 1
                chkColaborador = CType(grdCursosRegistrados.Rows(i).FindControl("chkClave"), CheckBox).Checked

                Dim lblEstatus As New Label

                strColaborador = (DirectCast(grdCursosRegistrados.Rows(i).FindControl("lblClave"), Label).Text)
                strIdCursoDNC = (DirectCast(grdCursosRegistrados.Rows(i).FindControl("lblIdDNC"), Label).Text)
                strCIdCurso = (DirectCast(grdCursosRegistrados.Rows(i).FindControl("lblIdCurso"), Label).Text)
                Dim odbComando As New OleDbCommand
                odbComando.CommandText = "gc_plan_capacitacion_cursos_solicitados_ins_upd_sp"
                odbComando.Connection = odbConexion
                odbComando.CommandType = CommandType.StoredProcedure

                odbComando.Parameters.AddWithValue("@PIdDNC", ddlDnc.SelectedValue)
                odbComando.Parameters.AddWithValue("@PIdCurso", strCIdCurso)
                odbComando.Parameters.AddWithValue("@PIdGestionDNC", strIdCursoDNC)
                odbComando.Parameters.AddWithValue("@PIdColaborador", strColaborador)
                odbComando.Parameters.AddWithValue("@PIdEstatus", IIf(chkColaborador, "1", "0"))
                odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

                odbComando.ExecuteNonQuery()

            Next
            odbConexion.Close()

            Call ObtCursosRegistradosColaborador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'Desabilidta Checks Gridview
    Public Sub DesabilitaCheckGridView()

        For i As Integer = 0 To grdCursosRegistrados.Rows.Count - 1
            CType(grdCursosRegistrados.Rows(i).FindControl("chkClave"), CheckBox).Checked = False

        Next

    End Sub



#End Region

#Region "Catalogos Plan"
    Public Sub obtDireccionPlan()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_direcciones_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDncPlanCapacitacion.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlDireccionPlan.DataSource = odbLector
            ddlDireccionPlan.DataTextField = "DIRECCION"
            ddlDireccionPlan.DataValueField = "DIRECCION"

            ddlDireccionPlan.DataBind()
            ddlDireccionPlan.Items.Insert(0, New ListItem("Todas", 0))
        Catch ex As Exception
            lblErrorPlan.Text = ex.Message
        End Try
    End Sub

    Public Sub obtGerenciaDeptoPlan()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gerencia_depto_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDncPlanCapacitacion.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccionPlan.SelectedValue)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlDepartamentoPlan.DataSource = odbLector
            ddlDepartamentoPlan.DataTextField = "DEPARTAMENTO"
            ddlDepartamentoPlan.DataValueField = "DEPARTAMENTO"
            ddlDepartamentoPlan.DataBind()
            ddlDepartamentoPlan.Items.Insert(0, New ListItem("Todos", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblErrorPlan.Text = ex.Message
        End Try
    End Sub
    Private Sub ObtCursoAutorizadosPlan()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim dsCatalogo As New DataSet
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_plan_capacitacion_cursos_autorizados_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDncPlanCapacitacion.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccionPlan.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PDeptoCia", ddlDepartamentoPlan.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PIdCurso", 0)
            odbComando.Parameters.AddWithValue("@PTipoSalida", 0)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            ddlCursosPlan.DataSource = odbLector
            ddlCursosPlan.DataTextField = "DESCRIPCION"
            ddlCursosPlan.DataValueField = "ID"
            ddlCursosPlan.DataBind()
            ddlCursosPlan.Items.Insert(0, New ListItem("Todos", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblErrorPlan.Text = ex.Message
        End Try
    End Sub
    Public Sub obtAnioDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT ID,CONVERT(VARCHAR,anio) AS anio FROM DNC_ANIO_PROPUESTO_CT WHERE fk_id_parametrizacion='" & ddlDncPlanCapacitacion.SelectedValue & "' AND ESTATUS=1   ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddlAnio.DataSource = odbLector
        ddlAnio.DataTextField = "anio"
        ddlAnio.DataValueField = "id"

        ddlAnio.DataBind()

        odbConexion.Close()
    End Sub
    Private Sub ddlDncPlanCapacitacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDncPlanCapacitacion.SelectedIndexChanged
        Call obtDireccionPlan()
        Call obtGerenciaDeptoPlan()
        Call ObtCursoAutorizadosPlan()
        Call ObtPlanCapacitacion()
        Call obtAnioDNC()
        divProgramar.visible = IIf(ddlDncPlanCapacitacion.SelectedValue = "0", False, True)
    End Sub

    Private Sub ddlDireccionPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDireccionPlan.SelectedIndexChanged
        Call obtGerenciaDeptoPlan()
        Call ObtCursoAutorizadosPlan()
        Call ObtPlanCapacitacion()
    End Sub
    Private Sub ddlDepartamentoPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamentoPlan.SelectedIndexChanged
        Call ObtCursoAutorizadosPlan()
        Call ObtPlanCapacitacion()
    End Sub
    Private Sub ddlCursosPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCursosPlan.SelectedIndexChanged
        Call ObtPlanCapacitacion()
    End Sub
#End Region

#Region "Pan de Capacitacion"
    Public Sub ObtPlanCapacitacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet

        Try

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_plan_capacitacion_cursos_autorizados_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbComando.Parameters.AddWithValue("@PIdDNC", ddlDncPlanCapacitacion.SelectedValue)
            odbComando.Parameters.AddWithValue("@PDireccion", ddlDireccionPlan.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PDeptoCia", ddlDepartamentoPlan.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PIdCurso", ddlCursosPlan.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@PTipoSalida", 1)

            Dim odbLector As New OleDbDataAdapter
            odbLector.SelectCommand = odbComando
            odbLector.Fill(dsDatos)

            GrdPlanCapacitacion.DataSource = dsDatos.Tables(0).DefaultView
            GrdPlanCapacitacion.DataBind()
            If GrdPlanCapacitacion.Rows.Count = 0 Then
                lblMensajePlan.Text = "No hay Cursos Autorizados"
                GrdPlanCapacitacion.Visible = False
                divProgramar.Visible = False
            Else
                lblMensajePlan.Text = ""
                GrdPlanCapacitacion.Visible = True
                divProgramar.Visible = True
                ddlMes.SelectedValue = "Enero"
            End If

            lblTotalPlan.Text = "Total: " & GrdPlanCapacitacion.Rows.Count.ToString
            odbConexion.Close()

            For iFil As Integer = 0 To GrdPlanCapacitacion.Rows.Count - 1
                'Color Celdas
                If iFil Mod 2 = 0 Then
                    GrdPlanCapacitacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            GC.Collect()
        Catch ex As Exception
            lblErrorPlan.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Protected Sub lnkProgramarPlan_Click(sender As Object, e As EventArgs)
        Call GuardaPlanTrabajo()
    End Sub
    Private Sub GuardaPlanTrabajo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim chkColaborador As Boolean = False
        Dim strIdPlan As String = ""

        Try

            odbConexion.Open()

            'Prestados
            For i As Integer = 0 To GrdPlanCapacitacion.Rows.Count - 1
                chkColaborador = CType(GrdPlanCapacitacion.Rows(i).FindControl("chkClave"), CheckBox).Checked

                strIdPlan = (DirectCast(GrdPlanCapacitacion.Rows(i).FindControl("lblId"), Label).Text)
                If chkColaborador Then
                    Dim odbComando As New OleDbCommand
                    odbComando.CommandText = "gc_plan_capacitacion_cursos_autorizados_upd_sp"
                    odbComando.Connection = odbConexion
                    odbComando.CommandType = CommandType.StoredProcedure
                    odbComando.Parameters.AddWithValue("@PIdPlanCapacitacion", strIdPlan)
                    odbComando.Parameters.AddWithValue("@PIdAnio", ddlAnio.SelectedItem.Text)
                    odbComando.Parameters.AddWithValue("@PMes", ddlMes.SelectedValue)
                    odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

                    odbComando.ExecuteNonQuery()
                End If


            Next
            odbConexion.Close()

            Call ObtPlanCapacitacion()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region



End Class