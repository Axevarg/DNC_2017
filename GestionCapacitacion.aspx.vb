Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
Imports Ionic.Zip

Public Class GestionCapacitacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""

        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call inicialiciaTabs()
            Call CargaCatalogosDatos()
            Call CargaCatalogosST()

        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Comportamientos", "<script>ComportamientosJS()</script>", False)
        'ScriptManager.RegisterStartupScript(Me, GetType(Page), "GridView", "<script> gridviewScroll();</script>", False)

        Call Comportamientos()
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
#Region "Catalogos de Datos"

    Public Sub CargaCatalogosDatos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Call ObtProveedor(odbConexion)
            Call ObtEstatusGestion(odbConexion, ddlEstatus, 1)
            Call ObtHabilidadDina(odbConexion)
            Call ObtModalidad(odbConexion)
            Call ObtFacilitador(ddlProveedor.SelectedValue)
            Call ObtConsultaCursosGestionados(ddlDnc.SelectedValue)
            Call ObtConsultaCursosAutorizados(ddlDnc.SelectedValue)
            Call ObtPagosCreados(ddlProveedor.SelectedValue)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub ObtHabilidadDina(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT * FROM DNC_HABILIDADES_DINA_CT WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlHabilidadDina.DataSource = odbLector
        ddlHabilidadDina.DataValueField = "ID"
        ddlHabilidadDina.DataTextField = "descripcion"

        ddlHabilidadDina.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ObtModalidad(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT * FROM DNC_MODALIDAD_CT WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlModalidad.DataSource = odbLector
        ddlModalidad.DataValueField = "ID"
        ddlModalidad.DataTextField = "descripcion"

        ddlModalidad.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ObtEstatusGestion(ByVal odbConexion As OleDbConnection, ddl As DropDownList, iCatalago As Integer)

        Dim strQuery As String = "SELECT * FROM GC_CT_ESTATUS_CAPACITACION WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddl.DataSource = odbLector
        ddl.DataValueField = "ID"
        ddl.DataTextField = "descripcion"

        ddl.DataBind()
        'valida si es para cargar el catalogo
        If iCatalago = 1 Then
            'valida si los item estan vacios
            ddl.Items.Insert(0, New ListItem("Seleccionar", 0))
        End If

    End Sub

    'COlaboradores de la Gestion
    Private Sub ObtGestionCOlaboradores(ByVal odbConexion As OleDbConnection, ddl As DropDownList)

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_cursos_sel_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@IdDNC", ddlDnc.SelectedValue)
        odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddl.DataSource = odbLector
        ddl.DataValueField = "CLAVE"
        ddl.DataTextField = "COLABORADOR"

        ddl.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ddlDnc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDnc.SelectedIndexChanged
        Call ObtConsultaCursosGestionados(ddlDnc.SelectedValue)
        Call ObtConsultaCursosAutorizados(ddlDnc.SelectedValue)
        Call ComportamientoLimpiaDncCursos()
        Call Comportamientos()
    End Sub
    Private Sub ObtProveedor(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT * FROM SIGIDO_PROVEEDORES_CT ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlProveedor.DataSource = odbLector
        ddlProveedor.DataValueField = "ID"
        ddlProveedor.DataTextField = "nombre"

        ddlProveedor.DataBind()
        'valida si los item estan vacios
        If ddlProveedor.Items.Count = 0 Then
            Response.Redirect("catalogos.aspx")
        End If
    End Sub
    Private Sub ddlProveedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProveedor.SelectedIndexChanged
        Call ObtFacilitador(ddlProveedor.SelectedValue)
        Call ObtPagosCreados(ddlProveedor.SelectedValue)
    End Sub
    'Obtine Facilador
    Private Sub ObtFacilitador(ByVal strProveedor As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim strQuery As String = "SELECT * FROM DNC_FACILITADORES_CT where fk_id_proveedor=" & IIf(strProveedor = "", 0, strProveedor) & " ORDER BY 2"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlFacilitador.DataSource = odbLector
            ddlFacilitador.DataValueField = "ID"
            ddlFacilitador.DataTextField = "nombre"

            ddlFacilitador.DataBind()
            'valida si los item estan vacios
            If ddlFacilitador.Items.Count = 0 Then
                strQuery = "SELECT '0' as ID, 'NO TIENE FACILITADORES' as NOMBRE"

                odbComando = New OleDbCommand(strQuery, odbConexion)

                Dim odbLectorVacio As OleDbDataReader
                odbLectorVacio = odbComando.ExecuteReader()

                ddlFacilitador.DataSource = odbLectorVacio
                ddlFacilitador.DataValueField = "ID"
                ddlFacilitador.DataTextField = "nombre"

                ddlFacilitador.DataBind()
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'Obtiene Listado de Cursos Gestinados
    Private Sub ObtConsultaCursosGestionados(ByVal strDnc As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim strQuery As String = " SELECT id,CONVERT(VARCHAR,correlativo) +' - '+descripcion_capacitacion_corta as descripcion " & _
                                     " FROM GC_GESTION_CAPACITACION_TB " & _
                                     " WHERE fk_id_parametrizacion=" & IIf(strDnc = "", 0, strDnc) & " ORDER BY correlativo"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlConsultaCursosGestionados.DataSource = odbLector
            ddlConsultaCursosGestionados.DataValueField = "ID"
            ddlConsultaCursosGestionados.DataTextField = "descripcion"
            ddlConsultaCursosGestionados.DataBind()
            If ddlConsultaCursosGestionados.Items.Count = 0 Then
                ddlConsultaCursosGestionados.Items.Insert(0, New ListItem("Seleccionar DNC", 0))
            Else
                ddlConsultaCursosGestionados.Items.Insert(0, New ListItem("Seleccionar", 0))
            End If
            odbLector.Close()

            'Curso Gestionado relacionado DNC
            odbComando = New OleDbCommand(strQuery, odbConexion)

            Dim odbLectorRelacionado As OleDbDataReader
            odbLectorRelacionado = odbComando.ExecuteReader()
            ddlCursoGestionadoRelacionado.Items.Clear()
            ddlCursoGestionadoRelacionado.DataSource = odbLectorRelacionado
            ddlCursoGestionadoRelacionado.DataValueField = "ID"
            ddlCursoGestionadoRelacionado.DataTextField = "descripcion"
            ddlCursoGestionadoRelacionado.DataBind()
            ddlCursoGestionadoRelacionado.Items.Insert(0, New ListItem("Seleccionar", 0))

            Call ObtClaveOcupacionDc3(odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    'Obtiene Listado de Cursos Autorizados
    Private Sub ObtPagosCreados(ByVal strProveedor As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_curso_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@IdCursoGestionado", ddlConsultaCursosGestionados.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdProveedor", strProveedor)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlPagoRelacionado.DataSource = odbLector
            ddlPagoRelacionado.DataValueField = "ID"
            ddlPagoRelacionado.DataTextField = "descripcion"
            ddlPagoRelacionado.DataBind()

            ddlPagoRelacionado.Items.Insert(0, New ListItem("Seleccionar", 0))


            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    '   
    Private Sub ObtConsultaCursosAutorizados(ByVal strDnc As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_cursos_autorizados_dnc_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@IdDNC", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@EsDnc", ddlEsDNC.SelectedValue)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlCursoAutorizadoDNC.DataSource = odbLector
            ddlCursoAutorizadoDNC.DataValueField = "ID"
            ddlCursoAutorizadoDNC.DataTextField = "descripcion"
            ddlCursoAutorizadoDNC.DataBind()

            ddlCursoAutorizadoDNC.Items.Insert(0, New ListItem("Seleccionar", 0))


            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

#End Region
#Region "Catalogos Secretaria"
    Public Sub CargaCatalogosST()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Call ObtDNC(odbConexion)
            Call ObtClaveCursoDc3(odbConexion)
            Call ObtClaveAreaTematicaDc3(odbConexion)
            Call ObtClaveTipoAgenteDc3(odbConexion)
            Call ObtClaveModalidadDc3(odbConexion)
            Call ObtClaveCapacitacionDc3(odbConexion)
            Call ObtClaveEstablecimientoDc3(odbConexion)
            Call ObtClaveOcupacionDc3(odbConexion)
            odbConexion.Close()
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

    End Sub
    Private Sub ObtClaveCursoDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_CLAVE_CURSO WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlClaveCursoDc3.DataSource = odbLector
        ddlClaveCursoDc3.DataValueField = "id"
        ddlClaveCursoDc3.DataTextField = "DESCRIPCIONC"

        ddlClaveCursoDc3.DataBind()
        ddlClaveCursoDc3.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveAreaTematicaDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_AREA_TEMATICA WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlClaveTematicaDc3.DataSource = odbLector
        ddlClaveTematicaDc3.DataValueField = "id"
        ddlClaveTematicaDc3.DataTextField = "DESCRIPCIONC"

        ddlClaveTematicaDc3.DataBind()
        ddlClaveTematicaDc3.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveTipoAgenteDc3(odbConexion As OleDbConnection)
        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_TIPO_AGENTE WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlTipoAgenteDc3.DataSource = odbLector
        ddlTipoAgenteDc3.DataValueField = "id"
        ddlTipoAgenteDc3.DataTextField = "DESCRIPCIONC"

        ddlTipoAgenteDc3.DataBind()
        ddlTipoAgenteDc3.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveModalidadDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_CLAVE_MODALIDAD WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlClaveModalidadDc3.DataSource = odbLector
        ddlClaveModalidadDc3.DataValueField = "id"
        ddlClaveModalidadDc3.DataTextField = "DESCRIPCIONC"

        ddlClaveModalidadDc3.DataBind()
        ddlClaveModalidadDc3.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveCapacitacionDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_CLAVE_CAPACITACION WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlClaveCapacitacionDc3.DataSource = odbLector
        ddlClaveCapacitacionDc3.DataValueField = "id"
        ddlClaveCapacitacionDc3.DataTextField = "DESCRIPCIONC"

        ddlClaveCapacitacionDc3.DataBind()
        ddlClaveCapacitacionDc3.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveEstablecimientoDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,clave + ' ' + descripcion AS DESCRIPCIONC FROM GC_CT_CLAVE_ESTABLECIMIENTO WHERE ESTATUS=1 ORDER BY descripcion"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlClaveEstablecimiento.DataSource = odbLector
        ddlClaveEstablecimiento.DataValueField = "id"
        ddlClaveEstablecimiento.DataTextField = "DESCRIPCIONC"

        ddlClaveEstablecimiento.DataBind()
        ddlClaveEstablecimiento.Items.Insert(0, New ListItem("Seleccionar", 0))

    End Sub
    Private Sub ObtClaveOcupacionDc3(odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT id,CAST(CAST(clave AS float) AS VARCHAR) + ' ' + descripcion AS DESCRIPCIONC,seleccionar " & _
                                 "  FROM GC_CT_CLAVE_OCUPACION_DC3 " & _
                                 " WHERE ESTATUS=1 ORDER BY CAST(clave AS float)"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlOcupacionDC3.DataSource = odbLector
        ddlOcupacionDC3.DataValueField = "id"
        ddlOcupacionDC3.DataTextField = "DESCRIPCIONC"

        ddlOcupacionDC3.DataBind()

        odbComando = New OleDbCommand(strQuery, odbConexion)

        Dim odbLectorOcultar As OleDbDataReader
        odbLectorOcultar = odbComando.ExecuteReader()

        While odbLectorOcultar.Read()

            For Each li As ListItem In ddlOcupacionDC3.Items
                If li.Value = odbLectorOcultar(0).ToString Then
                    If odbLectorOcultar(2).ToString = "0" Then
                        li.Attributes.Add("disabled", "disabled")
                    End If
                End If
            Next
        End While

        ddlOcupacionDC3.Items.Insert(0, New ListItem("Seleccionar", 0))
    End Sub
    'Bloquerar ITEM OCupacion
    Public Sub BloqueaItemOcupacionDC3()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)

        Try
            odbConexion.Open()
            Dim strQuery As String = "SELECT id,CAST(CAST(clave AS float) AS VARCHAR) + ' ' + descripcion AS DESCRIPCIONC,seleccionar " & _
                              "  FROM GC_CT_CLAVE_OCUPACION_DC3 " & _
                              " WHERE ESTATUS=1 ORDER BY CAST(clave AS float)"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            Dim odbLectorOcultar As OleDbDataReader
            odbLectorOcultar = odbComando.ExecuteReader()

            While odbLectorOcultar.Read()

                For Each li As ListItem In ddlOcupacionDC3.Items
                    If li.Value = odbLectorOcultar(0).ToString Then
                        If odbLectorOcultar(2).ToString = "0" Then
                            li.Attributes.Add("disabled", "disabled")
                        End If
                    End If
                Next
            End While
            odbConexion.Close()
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Comportamientos"
    Public Sub Comportamientos()
        'Comportamientos
        If ddlDnc.SelectedValue = "0" Then
            divCursosGestion.Visible = False
        Else
            divCursosGestion.Visible = True
            'Datos de Generales
            If ddlEsDNC.SelectedValue = "0" Then
                ' ddlCursoAutorizadoDNC.Attributes.Add("disabled", "disabled")
                lblCurso.InnerText = "Curso"
            Else
                ' ddlCursoAutorizadoDNC.Attributes.Remove("disabled")
                lblCurso.InnerText = "Curso Autorizado del Plan de Capacitación"
            End If
            'Valida si el idDe gestionExiste
            If hdIdGestionCapacitacion.Value = "" Or hdIdGestionCapacitacion.Value = "0" Then
                divGestionColaborador.Visible = False
                lblColaboradoresGestion.Text = "Necesita Guardar los datos del Curso antes de agregar a los colaboradores."
                lnkGestionarNuevoCurso.Visible = False
                lblEstatusCurso.Text = ""
                divEstatusColaboradores.Visible = False
            Else
                divGestionColaborador.Visible = True
                lblColaboradoresGestion.Text = ""
                lnkGestionarNuevoCurso.Visible = True
                divEstatusColaboradores.Visible = True
                lnkGenerarDc3.Visible = IIf(ddlDC3.SelectedValue = "No", False, True)
            End If
        End If
        'Asigna Comportamientos de Tabs
        Call remueveEstilosTabGestion()
        Call AsignaEstilosTabDnc()
        Call AsignaEstiloTabActive()

        For iFil As Integer = 0 To grdColaboradoresGestion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdColaboradoresGestion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'BLoquea Item
        Call BloqueaItemOcupacionDC3()

    End Sub
    'remueva los estyilos de las tabs
    Public Sub remueveEstilosTabGestion()
        tabGestion_1.Attributes.Remove("class") '
        tabGestion_2.Attributes.Remove("class") '
        tabGestion_3.Attributes.Remove("class") '

        lnkTabGestion1.Attributes.Remove("class")
        lnkTabGestion2.Attributes.Remove("class")
        lnkTabGestion3.Attributes.Remove("class")
    End Sub
    Public Sub AsignaEstilosTabDnc()
        tabGestion_1.Attributes.Add("class", "tab-pane") '
        tabGestion_2.Attributes.Add("class", "tab-pane") ''
        tabGestion_3.Attributes.Add("class", "tab-pane") '
    End Sub
    Public Sub AsignaEstiloTabActive()
        'asigna estatus active
        If hdIdGestion.Value = 1 Then
            tabGestion_1.Attributes.Add("class", "tab-pane active") '
            lnkTabGestion1.Attributes.Add("class", "active")
        ElseIf hdIdGestion.Value = 2 Then
            tabGestion_2.Attributes.Add("class", "tab-pane active") '
            lnkTabGestion2.Attributes.Add("class", "active")
        ElseIf hdIdGestion.Value = 3 Then
            tabGestion_3.Attributes.Add("class", "tab-pane active") '
            lnkTabGestion3.Attributes.Add("class", "active")
        End If
    End Sub
    Public Sub inicialiciaTabs()
        'Tabs DNC
        hdIdGestion.Value = 1
    End Sub
    Public Sub ComportamientoLimpiaDncCursos()
        'Comportamientos

        txtDescripcionCorta.Text = ""
        txtDescripcion.Text = ""
        txtObjetivo.Text = ""
        txtHabilidadesDesarrollar.Text = ""
        ddlTipoUbicacion.SelectedValue = "Interna"
        ddlMoneda.SelectedValue = "Pesos"
        ddlEvaluacion.SelectedValue = "Si"
        ddlConstancia.SelectedValue = "Si"
        ddlDC3.SelectedValue = "Si"
        txtNoMaximoParticipante.Text = "0"
        txtDuracionHoras.Text = "0"
        txtCostoIndividual.Text = "0"
        txtCostoGrupo.Text = "0"
        txtIVA.Text = "0"
        txtCostoTotal.Text = 0
        txtFechaApertura.Text = Now.Year.ToString & "-" & Right("00" & Now.Month.ToString, 2) & "-" & "01"
        txtFechaCierre.Text = Now.Year.ToString & "-" & Right("00" & Now.Month.ToString, 2) & "-" & "01"
        txtHorarioMDesde.Text = "07:00"
        txtHorarioMHasta.Text = "14:00"
        txtHorarioTDesde.Text = "00:00"
        txtHorarioTHasta.Text = "00:00"
        ddlCursoGestionadoRelacionado.SelectedValue = "0"
        ddlEstatus.SelectedValue = "0"
        txtUbicacion.Text = ""
        txtComentario.Text = ""
        ddlClaveCursoDc3.SelectedValue = "0"
        ddlClaveTematicaDc3.SelectedValue = "0"
        ddlTipoAgenteDc3.SelectedValue = "0"
        ddlClaveModalidadDc3.SelectedValue = "0"
        ddlClaveCapacitacionDc3.SelectedValue = "0"
        ddlClaveEstablecimiento.SelectedValue = "0"
        ddlOcupacionDC3.SelectedValue = "0"
    End Sub
#End Region
#Region "Datos Generales"
    Private Sub ddlEsDNC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEsDNC.SelectedIndexChanged
        Call ObtConsultaCursosAutorizados(ddlDnc.SelectedValue)
        Call ComportamientoLimpiaDncCursos()
        Call Comportamientos()
    End Sub
    Private Sub ddlCursoAutorizadoDNC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCursoAutorizadoDNC.SelectedIndexChanged
        Call ComportamientoLimpiaDncCursos()
        Call ObtieneDatosCursosDNC()
    End Sub
    'Obtiene los Cursos de la DNC Autorizados
    Public Sub ObtieneDatosCursosDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()


            strQuery = "SELECT * FROM [DNC_CURSOS_TB] WHERE id=" & ddlCursoAutorizadoDNC.SelectedValue

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader

            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                Call ObtFacilitador(odbLector(1).ToString)
                Call ObtPagosCreados(odbLector(1).ToString)
                ddlProveedor.SelectedValue = odbLector(1).ToString
                ddlFacilitador.SelectedValue = odbLector(2).ToString
                txtDescripcionCorta.Text = odbLector(3).ToString
                txtDescripcion.Text = odbLector(4).ToString
                txtObjetivo.Text = odbLector(5).ToString
                ddlModalidad.SelectedValue = odbLector(6).ToString
                txtNoMaximoParticipante.Text = odbLector(7).ToString
                txtHabilidadesDesarrollar.Text = odbLector(8).ToString
                ddlHabilidadDina.SelectedValue = odbLector(9).ToString
                txtDuracionHoras.Text = odbLector(10).ToString
                txtCostoIndividual.Text = odbLector(11).ToString
                txtCostoGrupo.Text = odbLector(12).ToString
                txtIVA.Text = odbLector(13).ToString
                ddlMoneda.SelectedValue = odbLector(14).ToString
                ddlEvaluacion.SelectedValue = odbLector(15).ToString.Trim
                ddlConstancia.SelectedValue = odbLector(16).ToString.Trim
                ddlDC3.SelectedValue = odbLector(17).ToString.Trim
                odbLector.Close()
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'Consulta los cursos Gestionados
    Private Sub ddlConsultaCursosGestionados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlConsultaCursosGestionados.SelectedIndexChanged
        hdIdGestionCapacitacion.Value = ddlConsultaCursosGestionados.SelectedValue

        Call ComportamientoLimpiaDncCursos()
        Call ObtieneDatosCursosDNC()
        Call ObtCursoGestionado()
        Call Comportamientos()

    End Sub
    Protected Sub lnkGuardarCursoGestionado_Click(sender As Object, e As EventArgs)
        Call GuardarCursoGestionado()
    End Sub
    'Guarda informacion de curso
    Public Sub GuardarCursoGestionado()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim strNumeroParticipantes As String = ""
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            strNumeroParticipantes = IIf(ddlConsultaCursosGestionados.SelectedValue = "0", IIf(txtNoMaximoParticipante.Text = "", "0", txtNoMaximoParticipante.Text),
                                                                             IIf(lblRegistroColaboradores.Text = "Debe de Agregar los Colaboradores de esta capacitación.", "0", grdColaboradoresGestion.Rows.Count))
            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pes_curso_dnc", ddlEsDNC.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_curso", ddlCursoAutorizadoDNC.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_proveedor", ddlProveedor.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_facilitador", ddlFacilitador.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pdescripcion_capacitacion_corta", txtDescripcionCorta.Text)
            odbComando.Parameters.AddWithValue("@Pdescripcion_capacitacion", txtDescripcion.Text)
            odbComando.Parameters.AddWithValue("@Pobjetivo", txtObjetivo.Text)
            odbComando.Parameters.AddWithValue("@Pnumero_participantes", strNumeroParticipantes)
            odbComando.Parameters.AddWithValue("@Phabilidades_desarrollar", txtHabilidadesDesarrollar.Text)
            odbComando.Parameters.AddWithValue("@Pfk_id_habilidad_dina", ddlHabilidadDina.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pmodalidad", ddlModalidad.SelectedValue)
            odbComando.Parameters.AddWithValue("@Ptipo_ubicacion", ddlTipoUbicacion.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pmoneda", ddlMoneda.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@Pevaluacion", ddlEvaluacion.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@Pconstacia", ddlConstancia.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@Pdc3", ddlDC3.SelectedValue.ToString.Trim)
            odbComando.Parameters.AddWithValue("@Pduracion", IIf(txtDuracionHoras.Text = "", 0, txtDuracionHoras.Text))
            odbComando.Parameters.AddWithValue("@Pcosto_individual", IIf(txtCostoIndividual.Text = "", 0, txtCostoIndividual.Text))
            odbComando.Parameters.AddWithValue("@Pcosto_grupo", IIf(txtCostoGrupo.Text = "", 0, txtCostoGrupo.Text))
            odbComando.Parameters.AddWithValue("@Piva", IIf(txtIVA.Text = "", 0, txtIVA.Text))
            odbComando.Parameters.AddWithValue("@Pcosto_final", IIf(txtCostoTotal.Text = "", 0, txtCostoTotal.Text))
            odbComando.Parameters.AddWithValue("@Pfecha_apertura", txtFechaApertura.Text)
            odbComando.Parameters.AddWithValue("@Pfecha_cierre", txtFechaCierre.Text)
            odbComando.Parameters.AddWithValue("@Phorario_manana_desde", IIf(txtHorarioMDesde.Text = "", "00:00", txtHorarioMDesde.Text))
            odbComando.Parameters.AddWithValue("@Phorario_manana_hasta", IIf(txtHorarioMHasta.Text = "", "00:00", txtHorarioMHasta.Text))
            odbComando.Parameters.AddWithValue("@Phorario_tarde_desde", IIf(txtHorarioTDesde.Text = "", "00:00", txtHorarioTDesde.Text))
            odbComando.Parameters.AddWithValue("@Phorario_tarde_hasta", IIf(txtHorarioTHasta.Text = "", "00:00", txtHorarioTHasta.Text))
            odbComando.Parameters.AddWithValue("@Pfk_id_gestion_capacitacion", ddlCursoGestionadoRelacionado.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_estatus", ddlEstatus.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_parametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_clave_curso", ddlClaveCursoDc3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_area_tematica", ddlClaveTematicaDc3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_tipo_agente", ddlTipoAgenteDc3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_clave_modalidad", ddlClaveModalidadDc3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_clave_capacitacion", ddlClaveCapacitacionDc3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_clave_establecimiento", ddlClaveEstablecimiento.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_clave_ocupacion_dc3", ddlOcupacionDC3.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pubicacion", txtUbicacion.Text)
            odbComando.Parameters.AddWithValue("@Pcomentario", txtComentario.Text)
            odbComando.Parameters.AddWithValue("@Pfk_id_pago", ddlPagoRelacionado.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value)

            Dim iId As Integer = 0
            iId = odbComando.ExecuteScalar
            hdIdGestionCapacitacion.Value = iId
            'Carga los Cursos Gestionados
            Call ObtConsultaCursosGestionados(ddlDnc.SelectedValue)
            ddlConsultaCursosGestionados.SelectedValue = iId.ToString
            'Actualiza el estatus de los colaboradores
            If ChkEstatus.Checked Then Call ActualizaEstatusCursoGestionadoColaboradores(odbConexion)
            Call ObtCursoGestionado() 'Obtiene los Curso Gestionados
            Call Comportamientos()
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = "GuardarCursoGestionado " & Err.Number & ex.Message
        End Try
    End Sub
    'Actualiza el estatus de todos los colaboradores
    Public Sub ActualizaEstatusCursoGestionadoColaboradores(odbConexion As OleDbConnection)

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_estatus_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
        odbComando.Parameters.AddWithValue("@Pfk_id_estatus", ddlEstatus.SelectedValue)
        odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value)
        odbComando.ExecuteNonQuery()
    End Sub
    'Obtiene la informacion del curso Gestionado
    Public Sub ObtCursoGestionado()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)


            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()

                ddlEsDNC.SelectedValue = odbLector(2).ToString
                'Asigna Valor de Gestion de la Capacitacion
                Call ObtConsultaCursosAutorizados(ddlDnc.SelectedValue)

                ddlCursoAutorizadoDNC.SelectedValue = odbLector(3).ToString
                ddlProveedor.SelectedValue = odbLector(4).ToString
                Call ObtFacilitador(ddlProveedor.SelectedValue)
                Call ObtPagosCreados(ddlProveedor.SelectedValue)
                ddlFacilitador.SelectedValue = odbLector(5).ToString
                txtDescripcionCorta.Text = odbLector(6).ToString
                txtDescripcion.Text = odbLector(7).ToString
                txtObjetivo.Text = odbLector(8).ToString
                ddlModalidad.SelectedValue = odbLector(9).ToString
                txtHabilidadesDesarrollar.Text = odbLector(11).ToString
                ddlHabilidadDina.SelectedValue = odbLector(12).ToString
                ddlTipoUbicacion.SelectedValue = odbLector(13).ToString
                ddlMoneda.SelectedValue = odbLector(14).ToString
                ddlEvaluacion.SelectedValue = odbLector(15).ToString
                ddlConstancia.SelectedValue = odbLector(16).ToString
                ddlDC3.SelectedValue = odbLector(17).ToString
                txtDuracionHoras.Text = odbLector(18).ToString
                txtCostoIndividual.Text = odbLector(19).ToString
                txtCostoGrupo.Text = odbLector(20).ToString
                txtIVA.Text = odbLector(21).ToString
                txtCostoTotal.Text = odbLector(22).ToString
                txtFechaApertura.Text = odbLector(23).ToString
                txtFechaCierre.Text = odbLector(24).ToString
                txtHorarioMDesde.Text = odbLector(25).ToString
                txtHorarioMHasta.Text = odbLector(26).ToString
                txtHorarioTDesde.Text = odbLector(27).ToString
                txtHorarioTHasta.Text = odbLector(28).ToString
                ddlCursoGestionadoRelacionado.SelectedValue = odbLector(29).ToString
                ddlEstatus.SelectedValue = odbLector(30).ToString
                ddlClaveCursoDc3.SelectedValue = odbLector(32).ToString
                ddlClaveTematicaDc3.SelectedValue = odbLector(33).ToString
                ddlTipoAgenteDc3.SelectedValue = odbLector(34).ToString
                ddlClaveModalidadDc3.SelectedValue = odbLector(35).ToString
                ddlClaveCapacitacionDc3.SelectedValue = odbLector(36).ToString
                ddlClaveEstablecimiento.SelectedValue = odbLector(37).ToString
                ddlOcupacionDC3.SelectedValue = odbLector(38).ToString
                txtUbicacion.Text = odbLector(43).ToString
                txtComentario.Text = odbLector(44).ToString
                ddlPagoRelacionado.SelectedValue = odbLector(45).ToString
                lblEstatusCurso.Text = "El Curso ya esta registrado."
                odbLector.Close()
            Else
                ddlEsDNC.SelectedValue = "0"
                ddlCursoAutorizadoDNC.SelectedValue = "0"
            End If
            Call obtGestionCapacitacionColaboradores()
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = "ObtCursoGestionado " & Err.Number & ex.Message
        End Try
    End Sub

#End Region
#Region "Colaboradores"
    'Carga el listado de Colaboradores
    Public Sub obtGestionCapacitacionColaboradores()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)

            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsDatos)
            grdColaboradoresGestion.DataSource = dsDatos.Tables(0).DefaultView
            grdColaboradoresGestion.DataBind()

            If grdColaboradoresGestion.Rows.Count = 0 Then
                lblRegistroColaboradores.Text = "Debe de Agregar los Colaboradores de esta capacitación."
                Call insFilaVaciaGestionColaborador()
                grdColaboradoresGestion.Rows(0).Visible = False
                hdNumerosColaboradores.Value = 0

            Else
                grdColaboradoresGestion.Rows(0).Visible = True
                lblRegistroColaboradores.Text = ""
                lblRegistroColaboradores.Text = "Total: " & grdColaboradoresGestion.Rows.Count.ToString
                hdNumerosColaboradores.Value = grdColaboradoresGestion.Rows.Count
            End If

            Dim i As Int16 = 0

            For i = 0 To grdColaboradoresGestion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim btnEliminar As LinkButton = grdColaboradoresGestion.Rows(i).Controls(6).Controls(1)
                Dim btnEditar As LinkButton = grdColaboradoresGestion.Rows(i).Controls(5).Controls(0)
                iId = DirectCast(grdColaboradoresGestion.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdColaboradoresGestion.Rows(i).Cells(2).FindControl("ddlEstatus")
                    Call ObtEstatusGestion(odbConexion, ddlEstatus, 0)
                    For iContador As Integer = 0 To dsDatos.Tables(0).Rows.Count - 1
                        If dsDatos.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsDatos.Tables(0).Rows(iContador)(4).ToString
                        End If
                    Next

                End If

                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar a " + DirectCast(grdColaboradoresGestion.Rows(i).Controls(3).Controls(1), Label).Text + "?')){ return false; };"
                'Formato de Celdas
                If i Mod 2 = 0 Then
                    grdColaboradoresGestion.Rows(i).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            'Asigna Valores de Colaboradores 
            Dim ddlAgreColaborador As DropDownList
            ddlAgreColaborador = grdColaboradoresGestion.FooterRow.FindControl("ddlAgreColaborador")
            Call ObtGestionCOlaboradores(odbConexion, ddlAgreColaborador)

            Dim ddlAgreEstatus As DropDownList
            ddlAgreEstatus = grdColaboradoresGestion.FooterRow.FindControl("ddlAgreEstatus")
            Call ObtEstatusGestion(odbConexion, ddlAgreEstatus, 0)
            ' ddlAgreEstatus.SelectedValue = ddlEstatus.SelectedValue
            'Formato
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Sub insFilaVaciaGestionColaborador()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("clave"))
        dt.Columns.Add(New DataColumn("colaborador"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("clave") = ""
        dr("colaborador") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdColaboradoresGestion.DataSource = dt.DefaultView
        grdColaboradoresGestion.DataBind()


    End Sub
    Protected Sub lnkAgregarColaborador_Click(sender As Object, e As EventArgs)
        Call insDescripcionColaboradores()
    End Sub
    'inserta registro de los colaboradores en los cursos
    Public Sub insDescripcionColaboradores()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strColaborador As String = ""
        Dim strClave As String = ""
        Dim strEstatus As String = ""
        Dim strSeleccionar As String = ""
        Try
            strColaborador = (DirectCast(grdColaboradoresGestion.FooterRow.FindControl("ddlAgreColaborador"), DropDownList).Text)
            strEstatus = (DirectCast(grdColaboradoresGestion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros


            If strColaborador = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un colaborador.');</script>", False)
                Exit Sub
            End If
            If strEstatus = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un estatus.');</script>", False)
                Exit Sub
            End If

            'Validacion para no insertar mas que el numero de participantes
            Dim iNumeroParticpantesCurso As Integer = CInt(ParticipantesMaximo(odbConexion))
            Dim iColaboradores As Integer = CInt(hdNumerosColaboradores.Value) + 1
            If iColaboradores > iNumeroParticpantesCurso Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No puedes insertar mas colaboradores que los permitidos " &
                                                        iNumeroParticpantesCurso.ToString & ".');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacionColaborador", "0")
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_colaborador", strColaborador)
            odbComando.Parameters.AddWithValue("@Pfk_id_estatus", strEstatus)
            odbComando.Parameters.AddWithValue("@Pfk_id_parametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            Call ObtCursoGestionado()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
    Public Function ParticipantesMaximo(odbConexion As OleDbConnection)
        Dim strResultado As String = "0"
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "gc_participantes_maximos_curso_sel_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        odbComando.Parameters.AddWithValue("@PIdCurso", ddlCursoAutorizadoDNC.SelectedValue)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If
        Return strResultado
    End Function
    Private Sub grdColaboradoresGestion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdColaboradoresGestion.RowCancelingEdit
        grdColaboradoresGestion.ShowFooter = True
        grdColaboradoresGestion.EditIndex = -1
        Call obtGestionCapacitacionColaboradores()
    End Sub



    'habilita el modo edicion
    Private Sub grdColaboradoresGestion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdColaboradoresGestion.RowEditing
        grdColaboradoresGestion.ShowFooter = False
        grdColaboradoresGestion.EditIndex = e.NewEditIndex
        Call obtGestionCapacitacionColaboradores()
    End Sub
    'actualiza la descripcion
    Private Sub grdColaboradoresGestion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdColaboradoresGestion.RowUpdating
        grdColaboradoresGestion.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String
        Dim strColaborador, strEstatus As String
        Try
            strId = DirectCast(grdColaboradoresGestion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            'validaciones para insertar registros

            strColaborador = (DirectCast(grdColaboradoresGestion.Rows(e.RowIndex).FindControl("lblClave"), Label).Text)
            strEstatus = (DirectCast(grdColaboradoresGestion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()

            If strColaborador = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un colaborador.');</script>", False)
                Exit Sub
            End If
            If strEstatus = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un estatus.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacionColaborador", strId)
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pfk_id_colaborador", strColaborador)
            odbComando.Parameters.AddWithValue("@Pfk_id_estatus", strEstatus)
            odbComando.Parameters.AddWithValue("@Pfk_id_parametrizacion", ddlDnc.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value)


            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdColaboradoresGestion.EditIndex = -1
            Call obtGestionCapacitacionColaboradores()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdColaboradoresGestion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdColaboradoresGestion.PageIndexChanging
        Try
            grdColaboradoresGestion.ShowFooter = True
            grdColaboradoresGestion.EditIndex = -1
            grdColaboradoresGestion.PageIndex = e.NewPageIndex
            grdColaboradoresGestion.DataBind()
            Call obtGestionCapacitacionColaboradores()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Private Sub grdColaboradoresGestion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdColaboradoresGestion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Try
            strId = DirectCast(grdColaboradoresGestion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_del_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdGestionCapacitacionColaborador", strId)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdColaboradoresGestion.EditIndex = -1
            grdColaboradoresGestion.ShowFooter = True
            Call ObtCursoGestionado()
        Catch ex As Exception
            lblErrorGestionCap.Text = ex.Message
        End Try
    End Sub
#End Region

    Protected Sub lnkGestionarNuevoCurso_Click(sender As Object, e As EventArgs)
        hdIdGestionCapacitacion.Value = 0
        ddlEsDNC.SelectedValue = "0"
        ddlCursoAutorizadoDNC.SelectedValue = "0"
        ddlConsultaCursosGestionados.SelectedValue = 0
        Call ComportamientoLimpiaDncCursos()
        Call Comportamientos()

    End Sub

#Region "Generar Lista de Asistencia"
    Protected Sub lnkGenerarListaAsistencia_Click(sender As Object, e As EventArgs)
        Call GenerarListaAsistencia()
    End Sub
    'Metodo Encardado de generar las listas de asistencia de los colaboradores
    Private Sub GenerarListaAsistencia()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim chkColaborador As Boolean = False
        Dim strIdPlan As String = ""
        Dim strClave As String = ""
        Dim iNumeroListas As Integer = 0
        Dim arrColaborador As New ArrayList
        Dim strNombreCurso As String = ""
        strNombreCurso = NormalizeDirName(ddlConsultaCursosGestionados.SelectedItem.Text)
        strNombreCurso = NormalizaNombreTexto(strNombreCurso)
        'Valida si los caracteres son mayores a 200
        If CStr(Len(strNombreCurso)) > 80 Then strNombreCurso = strNombreCurso.Substring(0, 80)
        Try

            'Recorre los colaboradores que se les agregara en la lista de Asitencia
            For i As Integer = 0 To grdColaboradoresGestion.Rows.Count - 1
                chkColaborador = CType(grdColaboradoresGestion.Rows(i).FindControl("chkClave"), CheckBox).Checked
                If chkColaborador Then
                    strClave = (DirectCast(grdColaboradoresGestion.Rows(i).FindControl("lblClave"), Label).Text)
                    arrColaborador.Add(strClave)
                End If
            Next

            'If arrColaborador.Count = 0 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Por favor, seleccione al menos un Colaborador.');</script>", False)
            '    Exit Sub
            'End If

            'Calcula el numero de Listas a Crear 22 porque ese ess el estandar de filas
            iNumeroListas = Math.Ceiling((arrColaborador.Count / 22))
            'Cilco para crear las listas de asistencias FO-RH-13 Lista de Asistencia a Capacitación
            'Crea el Archivo ZiP
            Dim zip As New ZipFile()
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            zip.AddDirectoryByName("Lista de Asistencia a Capacitación")
            'Asignacopon de Variables asociadas a al archivo Origen

            Dim path As String = Server.MapPath("~/UploadedFiles/")
            Dim strArchivoBaseListas As String = path & "\FormatosBase\Lista de Asistencia de Capacitación.xlsx" 'Base de las Listas Asistencias
            Dim strCarpetaLista As String = CStr(Now.Year & "\" & Now.Month & "\" & Now.Day & "\" & Now.Hour & Now.Minute & Now.Millisecond) & "\"
            Dim strCarpeta As String = path & "GestionCapacitacion\ListasAsistencia\" & strCarpetaLista

            Dim strNombreLista As String = ""
            Dim strListaNueva As String = ""
            Dim iColaborador As Integer = 0
            Dim iRegistrosPermitidos As Integer = 0
            Dim strMatricula As String = ""
            odbConexion.Open()
            For i As Integer = 1 To iNumeroListas
                strNombreLista = "Lista de Asistencia a Capacitacion " & strNombreCurso & " " & i.ToString & ".xlsx"
                strListaNueva = strCarpeta & strNombreLista
                'Realiza la Copia del Archivo
                'Valida que el archivo Base Exista
                If My.Computer.FileSystem.FileExists(strArchivoBaseListas) Then
                    'crea directorio
                    If Not (Directory.Exists(strCarpeta)) Then
                        Directory.CreateDirectory(strCarpeta)
                    End If
                    'Copia

                    File.Copy(strArchivoBaseListas, strListaNueva)
                End If
                'Guarda la informacion de los encabezados

                Dim odbComando As New OleDbCommand
                odbComando.CommandText = "gc_gestion_capacitacion_lista_asistencia_encabezado_sp"
                odbComando.Connection = odbConexion
                odbComando.CommandType = CommandType.StoredProcedure

                'parametros
                odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
                Dim odbLector As OleDbDataReader
                odbLector = odbComando.ExecuteReader
                If odbLector.HasRows Then
                    odbLector.Read()
                    'Registra Informacion del encabezado
                    Call WriteExcelListaAsistenciaEncabezadp(strListaNueva, odbLector)
                    odbLector.Close()
                End If
                odbComando.Dispose()
                odbLector = Nothing
                'Inserta  los datos de los 22 Colaboradores y si detecta mas ya no considera los que se ingreasron
                iColaborador = 0
                iRegistrosPermitidos = IIf(arrColaborador.Count >= 22, 21, (arrColaborador.Count - 1))
                While iColaborador <= iRegistrosPermitidos
                    odbComando = New OleDbCommand
                    odbComando.CommandText = "gc_gestion_capacitacion_lista_asistencia_detalle_sp"
                    odbComando.Connection = odbConexion
                    odbComando.CommandType = CommandType.StoredProcedure
                    strMatricula = arrColaborador(0)
                    'parametros
                    odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
                    odbComando.Parameters.AddWithValue("@PColaborador", strMatricula)

                    odbLector = odbComando.ExecuteReader
                    'Obtiene los Datos del Colaborador
                    If odbLector.HasRows Then
                        odbLector.Read()
                        'Registra Informacion del encabezado
                        Call WriteExcelListaAsistencia(strListaNueva, odbLector, iColaborador)
                        odbLector.Close()
                    End If
                    odbComando.Dispose()
                    odbLector = Nothing

                    'Elmina el colaborador
                    arrColaborador.RemoveAt(0)
                    iColaborador += 1
                    'si detecta mas de 21 posiciones da el brinco ya que serian los 22 colaboradores
                    If iColaborador = 22 Then Exit While

                End While
                'Ellimina los primeros 22 Registros del arreglo ya que se consideran
                'If arrColaborador.Count >= 22 Then
                iColaborador = 0

                'While iColaborador <= iRegistrosPermitidos
                '    'Se indica el Item 0 ya que se recorren el numero de indices
                '    arrColaborador.RemoveAt(0)
                '    iColaborador += 1
                '    ''si detecta mas de 21 posiciones da el brinco ya que serian los 22 colaboradores
                '    'If iColaborador = 22 Then Exit While

                'End While
                'End If
                'Guardar el Archivo en el ZIP
                zip.AddFile(strListaNueva, "Lista de Asistencia a Capacitación")

            Next
            'Descarga el Archivo Creado
            Response.Clear()
            Response.BufferOutput = False
            Dim zipName As String = strNombreCurso & "_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".zip"
            Response.ContentType = "application/zip"
            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
            zip.Save(Response.OutputStream)
            'Elimina toda la Carpeta
            '      Directory.Delete(strCarpeta, True)
            Response.End()


            odbConexion.Close()
            GC.Collect()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Shared Function NormalizeDirName(dirName As String) As String
        Dim invalidChars As String = System.Text.RegularExpressions.Regex.Escape(New String(System.IO.Path.GetInvalidPathChars()))
        Dim invalidRegStr As String = String.Format("([{0}]*\.+$)|([{0}]+)", invalidChars)
        Return System.Text.RegularExpressions.Regex.Replace(dirName, invalidRegStr, "_")
    End Function

    'Escribe los Comentarios de la Listas Asistencia
    Protected Sub WriteExcelListaAsistenciaEncabezadp(strRuta As String, odbLector As OleDbDataReader)
        'Open the Excel file using ClosedXML.
        Dim workBook As New XLWorkbook(strRuta)
        Dim strFecha As String = IIf(odbLector(1).ToString = odbLector(2).ToString, odbLector(1).ToString, odbLector(1).ToString & " al " & odbLector(2).ToString)
        Dim strHorario As String = odbLector(3).ToString & " a " & odbLector(4).ToString & _
            IIf(odbLector(5).ToString = "00:00", "", " y " & odbLector(5).ToString & " a " & odbLector(6).ToString)
        'Read the first Sheet from Excel file.
        Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
        workSheet.Cell("D4").Value = odbLector(0).ToString 'Curso
        workSheet.Cell("D5").Value = strFecha 'Fecha
        workSheet.Cell("D6").Value = strHorario 'Horario
        workSheet.Cell("D7").Value = odbLector(7).ToString & " Horas" 'Duracion

        workSheet.Cell("H6").Value = odbLector(8).ToString 'Lugar
        workSheet.Cell("H5").Value = odbLector(9).ToString 'Instructor
        workSheet.Cell("H4").Value = odbLector(10).ToString 'Proveedor

        workBook.Save()

    End Sub
    'Escribe la lineas de la Lista de Asistencia
    Protected Sub WriteExcelListaAsistencia(strRuta As String, odbLector As OleDbDataReader, iColaborador As Integer)
        'Open the Excel file using ClosedXML.
        Dim workBook As New XLWorkbook(strRuta)
        'Read the first Sheet from Excel file.
        Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
        Dim iFilaInicial As Integer = 11
        iFilaInicial = iFilaInicial + iColaborador
        workSheet.Cell("B" & iFilaInicial.ToString).Value = odbLector(0).ToString  'Nomina
        workSheet.Cell("C" & iFilaInicial.ToString).Value = odbLector(1).ToString  'Nombre
        workSheet.Cell("E" & iFilaInicial.ToString).Value = odbLector(2).ToString  'Area
        workSheet.Cell("G" & iFilaInicial.ToString).Value = odbLector(3).ToString  'Puesto


        workBook.Save()

    End Sub

    Public Function NormalizaNombreTexto(strTexto As String) As String

        strTexto = strTexto.Replace("ó", "o")
        strTexto = strTexto.Replace(",", "")
        strTexto = strTexto.Replace(":", "")

        Return strTexto
    End Function
#End Region
#Region "GeneraDC3"
    Protected Sub lnkGenerarDc3_Click(sender As Object, e As EventArgs)
        Call GenerarDc3()
    End Sub
    'Metodo Encardado de generar las listas de asistencia de los colaboradores
    Private Sub GenerarDc3()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim chkColaborador As Boolean = False
        Dim strIdPlan As String = ""
        Dim strClave As String = ""
        Dim arrColaborador As New ArrayList
        Dim strNombreCurso As String = ""
        strNombreCurso = NormalizeDirName(ddlConsultaCursosGestionados.SelectedItem.Text)
        strNombreCurso = NormalizaNombreTexto(strNombreCurso)
        Try
            Dim path As String = Server.MapPath("~/UploadedFiles/")
            Dim strformato As String = String.Empty

            'Dim strArchivoBaseListas As String = path & "\FormatosBase\DC-3.xlsx" 'Base de las Listas Asistencias
            Dim strArchivoBaseListas As String '= path & "\FormatosBase\" 'Base de las Listas Asistencias
            Dim strCarpetaLista As String = CStr(Now.Year & "\" & Now.Month & "\" & Now.Day & "\" & Now.Hour & Now.Minute & Now.Millisecond) & "\"
            Dim strCarpeta As String = path & "GestionCapacitacion\DC3\" & strCarpetaLista

            'Crea el Archivo ZiP
            Dim zip As New ZipFile()
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            zip.AddDirectoryByName("Dc3")
            Dim strNombreDc3 As String = ""
            odbConexion.Open()
            'Recorre los colaboradores que se les genere su DC3
            For i As Integer = 0 To grdColaboradoresGestion.Rows.Count - 1
                strArchivoBaseListas = path & "FormatosBase\"
                chkColaborador = CType(grdColaboradoresGestion.Rows(i).FindControl("chkClave"), CheckBox).Checked
                If chkColaborador Then
                    strClave = (DirectCast(grdColaboradoresGestion.Rows(i).FindControl("lblClave"), Label).Text)

                    Dim odbComando As New OleDbCommand
                    odbComando.CommandText = "gc_gestion_capacitacion_dc3_sp"
                    odbComando.Connection = odbConexion
                    odbComando.CommandType = CommandType.StoredProcedure

                    'parametros
                    odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
                    odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", strClave)
                    Dim odbLector As OleDbDataReader
                    odbLector = odbComando.ExecuteReader
                    If odbLector.HasRows Then
                        odbLector.Read()

                        Select Case odbLector(18).ToString
                            Case "PASSA ADMINISTRACION Y SERVICIOS S.A. DE C.V."
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Pasa.xlsx"
                            Case "DINA CAMIONES SA DE CV"
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Dina.xlsx"
                            Case "MERCADER FINANCIAL SA SOFOM ER"
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Mercader.xlsx"
                            Case "DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV"
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Dicoser.xlsx"
                            Case "DINA COMERCIALIZACION AUTOMOTRIZ SA DE CV"
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Dicomer.xlsx"
                            Case "TRANSPORTES Y LOGISTICA DE JALISCO SA DE CV"
                                strArchivoBaseListas = strArchivoBaseListas + "DC-3-Tlj.xlsx"
                        End Select

                        strNombreDc3 = "DC3 " & odbLector(1).ToString & " " & odbLector(2).ToString & " " & odbLector(3).ToString & ".xlsx"
                        strNombreDc3 = strCarpeta & strNombreDc3
                        'Realiza la Copia del Archivo
                        'Valida que el archivo Base Exista
                        If My.Computer.FileSystem.FileExists(strArchivoBaseListas) Then
                            'crea directorio
                            If Not (Directory.Exists(strCarpeta)) Then
                                Directory.CreateDirectory(strCarpeta)
                            End If
                            'Copia
                            File.Copy(strArchivoBaseListas, strNombreDc3)
                        End If

                        'Registra Informacion de la DC3
                        Call WriteExcelListaDc3(strNombreDc3, odbLector)
                        odbLector.Close()
                    End If
                    odbComando.Dispose()
                    zip.AddFile(strNombreDc3, "DC3")
                End If
            Next
            'Descarga el Archivo Creado
            Response.Clear()
            Response.BufferOutput = False
            Dim zipName As String = "DC3" & strNombreCurso & "_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".zip"
            Response.ContentType = "application/zip"
            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
            zip.Save(Response.OutputStream)
            'Elimina toda la Carpeta
            Directory.Delete(strCarpeta, True)
            Response.End()


            odbConexion.Close()
            GC.Collect()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'Escribre los datos de la DC3
    Protected Sub WriteExcelListaDc3(strRuta As String, odbLector As OleDbDataReader)
        'Open the Excel file using ClosedXML.
        Dim workBook As New XLWorkbook(strRuta)
        'Read the first Sheet from Excel file.
        Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
        workSheet.Cell("A6").Value = odbLector(1).ToString & " " & odbLector(2).ToString & " " & odbLector(3).ToString 'Nombre
        workSheet.Cell("S8").Value = odbLector(5).ToString 'OCUPACION
        workSheet.Cell("A10").Value = odbLector(6).ToString 'Puesto
        workSheet.Cell("A20").Value = odbLector(7).ToString 'Nombre Curso
        workSheet.Cell("A22").Value = odbLector(8).ToString 'Duracion
        workSheet.Cell("A24").Value = odbLector(15).ToString 'Area Tematica
        workSheet.Cell("A26").Value = odbLector(16).ToString 'Agente
        workSheet.Cell("A35").Value = odbLector(17).ToString 'Instructor
        'Curp
        Dim strCurp As String = odbLector(4).ToString
        Dim iContador As Integer = 1
        iContador = 1
        For Each item As Char In strCurp
            ' en item tiene cada caracter del texto
            workSheet.Cell(AsignaCelda(iContador) & "8").Value = item
            iContador += 1
        Next
        'Fechas
        'Fecha Apertura
        'Año
        iContador = 1
        Dim strAnio As String = odbLector(9).ToString
        Dim strMes As String = odbLector(10).ToString
        Dim strDia As String = odbLector(11).ToString
        Dim strLetra As String = ""
        For Each item As Char In strAnio
            Select Case iContador
                Case 1
                    strLetra = "L"
                Case 2
                    strLetra = "M"
                Case 3
                    strLetra = "N"
                Case 4
                    strLetra = "O"
            End Select
            ' en item tiene cada caracter del texto
            workSheet.Cell(strLetra & "22").Value = item
            iContador += 1
        Next
        'Mes
        strLetra = ""
        iContador = 1
        If strMes.Length = 2 Then
            For Each item As Char In strMes
                Select Case iContador
                    Case 1
                        strLetra = "P"
                    Case 2
                        strLetra = "Q"
                End Select
                ' en item tiene cada caracter del texto
                workSheet.Cell(strLetra & "22").Value = item
                iContador += 1
            Next
        Else
            workSheet.Cell("P22").Value = "0"
            workSheet.Cell("Q22").Value = strMes
        End If
        'dIA
        strLetra = ""
        iContador = 1
        If strDia.Length = 2 Then
            For Each item As Char In strDia
                Select Case iContador
                    Case 1
                        strLetra = "R"
                    Case 2
                        strLetra = "S"
                End Select
                ' en item tiene cada caracter del texto
                workSheet.Cell(strLetra & "22").Value = item
                iContador += 1
            Next
        Else
            workSheet.Cell("R22").Value = "0"
            workSheet.Cell("S22").Value = strDia
        End If
        'Fecha de Cierre
        strAnio = odbLector(12).ToString
        strMes = odbLector(13).ToString
        strDia = odbLector(14).ToString
        strLetra = ""
        iContador = 1
        For Each item As Char In strAnio
            Select Case iContador
                Case 1
                    strLetra = "U"
                Case 2
                    strLetra = "V"
                Case 3
                    strLetra = "W"
                Case 4
                    strLetra = "X"
            End Select
            ' en item tiene cada caracter del texto
            workSheet.Cell(strLetra & "22").Value = item
            iContador += 1
        Next
        'Mes
        strLetra = ""
        iContador = 1
        If strMes.Length = 2 Then
            For Each item As Char In strMes
                Select Case iContador
                    Case 1
                        strLetra = "Y"
                    Case 2
                        strLetra = "Z"
                End Select
                ' en item tiene cada caracter del texto
                workSheet.Cell(strLetra & "22").Value = item
                iContador += 1
            Next
        Else
            workSheet.Cell("Y22").Value = "0"
            workSheet.Cell("Z22").Value = strMes
        End If
        'dIA
        iContador = 1
        strLetra = ""
        If strDia.Length = 2 Then
            For Each item As Char In strDia
                Select Case iContador
                    Case 1
                        strLetra = "AA"
                    Case 2
                        strLetra = "AB"
                End Select
                ' en item tiene cada caracter del texto
                workSheet.Cell(strLetra & "22").Value = item
                iContador += 1
            Next
        Else
            workSheet.Cell("AA22").Value = "0"
            workSheet.Cell("AB22").Value = strDia
        End If

        workBook.Save()

    End Sub
    'Manda Celda de Curp
    Public Function AsignaCelda(Contador As Integer) As String
        Dim strResultado As String = ""
        Select Case Contador
            Case 1
                strResultado = "A"
            Case 2
                strResultado = "B"
            Case 3
                strResultado = "C"
            Case 4
                strResultado = "D"
            Case 5
                strResultado = "E"
            Case 6
                strResultado = "F"
            Case 7
                strResultado = "G"
            Case 8
                strResultado = "H"
            Case 9
                strResultado = "I"
            Case 10
                strResultado = "J"
            Case 11
                strResultado = "K"
            Case 12
                strResultado = "L"
            Case 13
                strResultado = "M"
            Case 14
                strResultado = "N"
            Case 15
                strResultado = "O"
            Case 16
                strResultado = "P"
            Case 17
                strResultado = "Q"
            Case 18
                strResultado = "R"
        End Select

        Return strResultado
    End Function
#End Region
#Region "Cargar LayOut Colaboradores"
    Public Sub cargaArchivo()
        If IsPostBack Then
            Dim path As String = Server.MapPath("~/UploadedFiles/")
            Dim fileOK As Boolean = False
            Dim sIdFile As String = ""
            Dim strNombreArchivo As String = ""

            lblmessage.Text = ""

            If fuExcel.HasFile Then

                If fuExcel.FileBytes.Length > 6000000 Then
                    divMensaje.Attributes("class") = "warning-box round"
                    lblmessage.Text = "* El tamaño de archivo no debe sobrepasar los 500 Kb."
                    Exit Sub
                End If

                Dim fileExtension As String
                fileExtension = System.IO.Path. _
                    GetExtension(fuExcel.FileName).ToLower()
                Dim allowedExtensions As String() = _
                    {".xls", ".xlsx"}
                For i As Integer = 0 To allowedExtensions.Length - 1
                    If fileExtension = allowedExtensions(i).ToString Then
                        fileOK = True
                    End If
                Next
                If fileOK Then
                    Try
                        If Mid(System.IO.Path.GetExtension(fuExcel.FileName).ToLower().ToString, 1, 4) <> ".xls" Then
                            divMensaje.Attributes("class") = "warning-box round"
                            lblmessage.Text = "* Tipo de Archivo Incorrecto. Sólo se admiten archivos Excel"
                        Else
                            Dim strCarpeta As String = path & "\GestionCapacitacion\Colaboradores\"
                            strNombreArchivo = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & fuExcel.FileName


                            'crea directorio
                            If Not (Directory.Exists(strCarpeta)) Then
                                Directory.CreateDirectory(strCarpeta)
                            End If

                            fuExcel.PostedFile.SaveAs(strCarpeta & strNombreArchivo)

                            Call leerExcel(strCarpeta & strNombreArchivo, fileExtension, sIdFile, strNombreArchivo)

                            Call Comportamientos()

                            '   lblmessage.ForeColor = Drawing.Color.Black
                            If lblmessage.Text = "" Then
                                lblmessage.Font.Bold = True
                                lblmessage.Text = "El archivo se ha cargado correctamente!"
                                divMensaje.Attributes("class") = "confirmation-box round"
                            End If
                            'elimina el archivo subido
                            If My.Computer.FileSystem.FileExists(strCarpeta & strNombreArchivo) Then
                                My.Computer.FileSystem.DeleteFile(strCarpeta & strNombreArchivo)
                            End If

                        End If
                    Catch ex As Exception
                        divMensaje.Attributes("class") = "warning-box round"
                        lblmessage.Text = "* No se pudo cargar el archivo: " & ex.Message.ToString


                    End Try
                Else
                    divMensaje.Attributes("class") = "warning-box round"
                    lblmessage.Text = "* Tipo de Archivo Incorrecto. Sólo se admiten archivos Excel"
                End If
            End If
        End If
    End Sub
    Sub leerExcel(ByVal strRuta As String, ByVal strExtencion As String, ByVal IdFile As String, ByVal FileName As String)
        Dim dsDatos As New DataSet
        Dim odbAdaptador As System.Data.OleDb.OleDbDataAdapter
        Dim sConnString As String
        Dim dt As New DataTable()
        lblmessage.Text = ""

        Try


            If strExtencion = ".xls" Then
                sConnString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & ";Extended Properties=""Excel 8.0;""")

            Else  '"xlsx"
                '   sConnString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & ";Extended Properties=""Excel 12.0 Xml;"" HDR=Yes;")
                sConnString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & ";Extended Properties=""Excel 12.0""")
            End If

            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Dim intNumFilas, intNumColumnas As Integer


            odbAdaptador = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Colaboradores$]", odbConexion)
            odbAdaptador.TableMappings.Add("Table", "TestTable")
            dsDatos = New System.Data.DataSet
            'se ejecuta la consulta
            odbAdaptador.Fill(dsDatos)
            odbAdaptador.Dispose()
            odbConexion.Close()
            intNumFilas = dsDatos.Tables(0).Rows.Count - 1
            intNumColumnas = dsDatos.Tables(0).Columns.Count - 1

            If intNumColumnas = 1 And intNumFilas > 0 Then
                sConnString = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
                odbConexion = New OleDbConnection(sConnString)
                Dim odbComando As New OleDbCommand
                Dim strColaborador As String = ""
                odbConexion.Open()
                'Cliclo para obtener todas las matriculas
                For i As Integer = 0 To dsDatos.Tables(0).Rows.Count - 1
                    odbComando = New OleDbCommand
                    odbComando.CommandText = "gc_gestion_capacitacion_colaboradores_ins_upd_sp"
                    odbComando.Connection = odbConexion
                    odbComando.CommandType = CommandType.StoredProcedure
                    strColaborador = dsDatos.Tables(0).Rows(i)(0).ToString
                    'parametros
                    odbComando.Parameters.AddWithValue("@PIdGestionCapacitacionColaborador", "0")
                    odbComando.Parameters.AddWithValue("@PIdGestionCapacitacion", ddlConsultaCursosGestionados.SelectedValue)
                    odbComando.Parameters.AddWithValue("@Pfk_id_colaborador", strColaborador)
                    odbComando.Parameters.AddWithValue("@Pfk_id_estatus", ddlEstatus.SelectedValue)
                    odbComando.Parameters.AddWithValue("@Pfk_id_parametrizacion", ddlDnc.SelectedValue)
                    odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value & "Xlsx")
                    If IsNumeric(strColaborador) Then odbComando.ExecuteNonQuery() 'valida si es verdadero la accion
                Next

            End If
            Call obtGestionCapacitacionColaboradores()
            'carga menu de Pagina
            Call CargaMenuPagina(hdRol.Value, odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
#End Region

    Private Sub btnCargaColaboradores_ServerClick(sender As Object, e As EventArgs) Handles btnCargaColaboradores.ServerClick
        Call cargaArchivo()
    End Sub
End Class