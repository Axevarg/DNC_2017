Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Public Class ValidadorCompetenciasConf
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        If Not Page.IsPostBack Then
            Call SeguridadAplicativo()
            Call ObtConfiguracionValidador()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Seguridad"
    Public Sub SeguridadAplicativo()
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

    Public Sub validaAccesoAplicacion(ByVal NombreUsuario As String, ByVal usuario As String,
                                            ByVal email As String, ByVal strClaveEmpleado As String, ByVal odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim odbComando As OleDbCommand
        Dim strNomina As String = ""


        'si existe actualiza la fecha de acceso
        If existeUsuario(usuario, odbConexion) Then
            strNomina = IIf(strClaveEmpleado = "", obtNumeroNomina(hdUsuario.Value, odbConexion), strClaveEmpleado)
            'actualiza la fecha de acceso
            strQuery = "UPDATE SIGIDO_USUARIOS_TB " &
                  " SET  ultimo_acceso=(GETDATE())" &
                  " WHERE usuario='" & usuario & "'"

            odbComando = New OleDbCommand(strQuery, odbConexion)
        Else 'como no existe inserta sus datos al sistema

            strNomina = IIf(strClaveEmpleado = "", obtNumeroNomina(hdUsuario.Value, odbConexion), strClaveEmpleado)
            strQuery = "INSERT INTO SIGIDO_USUARIOS_TB(  [clave]" &
           ",[nombre] ,[email] ,[usuario] ,[primer_Aceeso] ,[rol]) VALUES(" &
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


        strQuery = "  SELECT [CLAVE] " &
        "FROM [SGIDO_INFOGIRO_GIRO_VT]" &
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

        strQuery = "SELECT ISNULL(estatus,0) FROM SIGIDO_PERMISOS_PERFIL_TB " &
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
            strQuery = "SELECT  [NOMBRE] +' ' +[APPAT] + ' '+ [APMAT] AS NOMBRE_COMPLETO" &
                        "       ,[DEPARTAMENTO]" &
                        "       ,[PUESTO]" &
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

#Region "Grid Configuracion"
    'obtiene el catalogo 
    Public Sub ObtConfiguracionValidador()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim odbAdaptador As New OleDbDataAdapter
        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_tb_configuracion_validador_competencias_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsCatalogo)
            grdConfiguracion.DataSource = dsCatalogo.Tables(0).DefaultView
            grdConfiguracion.DataBind()

            If grdConfiguracion.Rows.Count = 0 Then
                Call nombre_estatus()
                grdConfiguracion.Rows(0).Visible = False
            Else
                grdConfiguracion.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdConfiguracion.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdConfiguracion.Rows(i).Controls(5).Controls(0)
                Dim btnEliminar As LinkButton = grdConfiguracion.Rows(i).Controls(6).Controls(1)
                iId = DirectCast(grdConfiguracion.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdConfiguracion.Rows(i).Cells(4).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(4).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdConfiguracion.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If


            Next

            grdConfiguracion.ShowFooter = True
            ' Establecer el índice de la fila en edición a -1 para salir del modo de edición
            grdConfiguracion.EditIndex = -1
            ' Actualizar el GridView
            'If btnEditar.Text = "Editar" Then
            '    btnEditar.ToolTip = "Editar Descripción"
            '    'JAVA SCRIPT
            '    btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdConfiguracion.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            'Else
            '    btnEditar.ToolTip = "Actualizar Descripción"
            '    Dim cancelar As LinkButton = grdConfiguracion.Rows(i).Controls(3).Controls(2)
            '    cancelar.ToolTip = "Cancelar Edición"

            '    btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdConfiguracion.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
            'End If


            'Motivo de Permisos
            For iFil As Integer = 0 To grdConfiguracion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdConfiguracion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub nombre_estatus()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("fecha_inicial"))
        dt.Columns.Add(New DataColumn("fecha_final"))
        dt.Columns.Add(New DataColumn("estatus"))
        dt.Columns.Add(New DataColumn("nombre_estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("fecha_inicial") = ""
        dr("fecha_final") = ""
        dr("estatus") = ""
        dr("nombre_estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdConfiguracion.DataSource = dt.DefaultView
        grdConfiguracion.DataBind()


    End Sub

    Protected Sub lnkAgregarC_Click(sender As Object, e As EventArgs)
        Call InsConfiguracion()
    End Sub
    'inserta registro al catalogo
    Public Sub InsConfiguracion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strDescripcion As String = ""
        Dim strEstatus As String = ""
        Dim strFechaIni As String = ""
        Dim strFechaFin As String = ""

        Try

            strDescripcion = (DirectCast(grdConfiguracion.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdConfiguracion.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            strFechaIni = (DirectCast(grdConfiguracion.FooterRow.FindControl("txtAgreFechaInicio"), TextBox).Text)
            strFechaFin = (DirectCast(grdConfiguracion.FooterRow.FindControl("txtAgreFechaFinal"), TextBox).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If strEstatus = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un estatus.');</script>", False)
                Exit Sub
            End If


            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "cop_mo_ct_motivo_permisos_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            odbComando.Parameters.AddWithValue("@IdMotivos", "0")
            odbComando.Parameters.AddWithValue("@Descripcion", strDescripcion)
            odbComando.Parameters.AddWithValue("@Estatus", strEstatus)
            odbComando.Parameters.AddWithValue("@FechaInicial", strFechaIni)
            odbComando.Parameters.AddWithValue("@FechaFinal", strFechaFin)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                If odbLector(0).ToString <> "" Then ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('" & odbLector(0).ToString & "');</script>", False)
                odbLector.Close()
            End If

            odbConexion.Close()

            Call ObtConfiguracionValidador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdConfiguracion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdConfiguracion.RowCancelingEdit
        grdConfiguracion.ShowFooter = True
        grdConfiguracion.EditIndex = -1
        ObtConfiguracionValidador()
    End Sub

    'TOOLTIPS
    Private Sub grdConfiguracion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdConfiguracion.RowDataBound

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
    Private Sub grdConfiguracion_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdConfiguracion.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Try
            strId = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()


            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "cop_mo_ct_motivo_permisos_del_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@IdMotivos", strId)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                If odbLector(0).ToString <> "" Then ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('" & odbLector(0).ToString & "');</script>", False)
                odbLector.Close()
            End If


            odbConexion.Close()

            grdConfiguracion.EditIndex = -1

            Call ObtConfiguracionValidador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'habilita el modo edicion
    Private Sub grdConfiguracion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdConfiguracion.RowEditing
        grdConfiguracion.ShowFooter = False
        grdConfiguracion.EditIndex = e.NewEditIndex
        Call ObtConfiguracionValidador()
    End Sub


    'actualiza la descripcion
    Protected Sub grdConfiguracion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdConfiguracion.RowUpdating
        Dim Descripcion As String = ""
        Dim Estatus As String = ""
        Dim FechaIni As String = ""
        Dim FechaFin As String = ""
        Dim strId As String = ""
        strId = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("lblId"), Label).Text
        Descripcion = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
        Estatus = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text
        FechaIni = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("txtFechaInicio"), TextBox).Text
        FechaFin = DirectCast(grdConfiguracion.Rows(e.RowIndex).FindControl("txtFechaFinal"), TextBox).Text

        Dim script As String = "alert('ID: " & strId & "\nDescripción: " & Descripcion & "\nEstatus: " & Estatus & "\nFecha Inicial: " & FechaIni & "\nFecha Final: " & FechaFin & "');"

        ' Registrar el script en la página para que se muestre la alerta
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", script, True)

        ActualizarConfiguracion(strId, Descripcion, Estatus, FechaIni, FechaFin)

        grdConfiguracion.EditIndex = -1
        ObtConfiguracionValidador()
        grdConfiguracion.ShowFooter = True
    End Sub


    'Actualizar

    Public Sub ActualizarConfiguracion(ByVal idMotivo As Integer, ByVal descripcion As String, ByVal estatus As String, ByVal fechaInicial As String, ByVal fechaFinal As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)


        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "cop_mo_ct_motivo_permisos_ins_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            odbComando.Parameters.AddWithValue("@IdMotivos", idMotivo)
            odbComando.Parameters.AddWithValue("@Descripcion", descripcion)
            odbComando.Parameters.AddWithValue("@Estatus", estatus)
            odbComando.Parameters.AddWithValue("@FechaInicial", fechaInicial)
            odbComando.Parameters.AddWithValue("@FechaFinal", fechaFinal)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                If odbLector(0).ToString <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('" & odbLector(0).ToString & "');</script>", False)
                End If
                odbLector.Close()
            End If

            odbConexion.Close()
            Call ObtConfiguracionValidador()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Protected Sub grdConfiguracion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdConfiguracion.PageIndexChanging
        grdConfiguracion.ShowFooter = True
        grdConfiguracion.PageIndex = e.NewPageIndex
        grdConfiguracion.DataBind()
        Call ObtConfiguracionValidador()
    End Sub
#End Region

End Class