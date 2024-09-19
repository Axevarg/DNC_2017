Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
Imports Ionic.Zip
Public Class IdentificacionPuestoDescarga
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblMensaje.Text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call CargarCatalogos()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
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
#Region "Catalago"
    Public Sub CargarCatalogos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()

            Call ObtPuestoJefe(odbConexion)

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    Private Sub ObtPuestoJefe(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID, DESCRIPCION AS DESCRIPCION FROM DO_PUESTOS_TB " & _
            " WHERE ID IN(SELECT ISNULL(jefe,0) FROM DO_PUESTOS_TB GROUP BY jefe)  ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlPuesto.DataSource = odbLector
        ddlPuesto.DataValueField = "id"
        ddlPuesto.DataTextField = "DESCRIPCION"
        ddlPuesto.DataBind()
        ddlPuesto.Items.Insert(0, New ListItem("Seleccionar", 0))
    End Sub
#End Region

#Region "Imprimir"
    Protected Sub lnkDescargar_Click(sender As Object, e As EventArgs)
        Call ImprimirDescriptivoP()
        Call obtenerUsuarioAD()

    End Sub
    Protected Sub DownloadFiles()
        Using zip As New ZipFile()
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            zip.AddDirectoryByName("Files")
            For Each row As GridViewRow In grdDescriptivos.Rows
                If TryCast(row.FindControl("chkSelect"), CheckBox).Checked Then
                    Dim filePath As String = TryCast(row.FindControl("lblFilePath"), Label).Text
                    zip.AddFile(filePath, "Files")
                End If
            Next

        End Using
    End Sub
    Public Sub ImprimirDescriptivoP()

        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim path As String = Server.MapPath("~/UploadedFiles/")
        Dim strCarpetaPuesto As String = CStr(Now.Year & "\" & Now.Month & "\" & Now.Day & "\" & Now.Hour & Now.Minute & Now.Millisecond) & "\"
        Dim strCarpeta As String = path & "\Reclutamiento\DescriptivosPuesto\" & strCarpetaPuesto
        Dim strNombreArchivo As String = ""
        Dim strDescriptivo As String = ""
        Dim strArchivo As String = path & "\DescriptivodePuesto.xlsx" 'Base del Descriptivos
        Dim dsDatos As New DataSet
        Try
            Dim zip As New ZipFile()
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            zip.AddDirectoryByName("Puestos_" & ddlPuesto.SelectedItem.Text)
            odbConexion.Open()

            For Each row As GridViewRow In grdDescriptivos.Rows
                If TryCast(row.FindControl("chkSelect"), CheckBox).Checked Then
                    Dim idPuesto As String = TryCast(row.FindControl("lblId"), Label).Text
                    strNombreArchivo = row.Cells(3).Text & ".xlsx"
                    strDescriptivo = strCarpeta & strNombreArchivo
                    'Valida que el archivo Base Exista
                    If My.Computer.FileSystem.FileExists(strArchivo) Then

                        'crea directorio
                        If Not (Directory.Exists(strCarpeta)) Then
                            Directory.CreateDirectory(strCarpeta)
                        End If
                        'Copia
                        File.Copy(strArchivo, strDescriptivo)
                    End If


                    Dim odbComando As New OleDbCommand
                    odbComando.CommandText = "do_info_puesto_sel_excel_print_sp"
                    odbComando.Connection = odbConexion
                    odbComando.CommandType = CommandType.StoredProcedure

                    'parametros
                    odbComando.Parameters.AddWithValue("@PIdPuesto", idPuesto)
                    Dim odbAdaptador As New OleDbDataAdapter
                    odbAdaptador.SelectCommand = odbComando

                    odbAdaptador.Fill(dsDatos)
                    Call WriteExcel(strDescriptivo, dsDatos)
                    'Escribe Excel

                    dsDatos.Dispose()
                    zip.AddFile(strDescriptivo, "Puestos_" & ddlPuesto.SelectedItem.Text)
                End If
            Next ' fin for each
            odbConexion.Close()

            'Descarga el Archivo Creado
            Response.Clear()
            Response.BufferOutput = False
            Dim zipName As String = ddlPuesto.SelectedItem.Text & "_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".zip"
            Response.ContentType = "application/zip"
            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
            zip.Save(Response.OutputStream)
            Directory.Delete(strCarpeta, True)

            Response.End()

        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try


    End Sub

    Protected Sub WriteExcel(strRuta As String, dsDatos As DataSet)
        'Open the Excel file using ClosedXML.
        Dim workBook As New XLWorkbook(strRuta)
        'Read the first Sheet from Excel file.
        Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
        workSheet.Cell("L5").Value = dsDatos.Tables(0).Rows(0)(0).ToString 'Fecha Creacion
        workSheet.Cell("AA5").Value = dsDatos.Tables(0).Rows(0)(1).ToString 'Fecha modificacion
        workSheet.Cell("F9").Value = dsDatos.Tables(0).Rows(0)(2).ToString 'Compañía
        workSheet.Cell("F11").Value = dsDatos.Tables(0).Rows(0)(3).ToString 'Puesto
        workSheet.Cell("V11").Value = dsDatos.Tables(0).Rows(0)(4).ToString 'Nivel
        workSheet.Cell("D14").Value = dsDatos.Tables(0).Rows(0)(5).ToString 'Objetivo
        workSheet.Cell("D26").Value = dsDatos.Tables(0).Rows(0)(6).ToString 'Area
        workSheet.Cell("V16").Value = dsDatos.Tables(0).Rows(0)(7).ToString 'Puesto Reporto
        workSheet.Cell("AB20").Value = dsDatos.Tables(0).Rows(0)(8).ToString 'NumPuestosMeReportan
        workSheet.Cell("V22").Value = dsDatos.Tables(0).Rows(0)(9).ToString 'PuestosMeReportan
        workSheet.Cell("D40").Value = dsDatos.Tables(0).Rows(0)(10).ToString 'responsabilidad_1
        workSheet.Cell("D42").Value = dsDatos.Tables(0).Rows(0)(11).ToString 'responsabilidad_2
        workSheet.Cell("D44").Value = dsDatos.Tables(0).Rows(0)(12).ToString 'responsabilidad_3
        workSheet.Cell("D46").Value = dsDatos.Tables(0).Rows(0)(13).ToString 'responsabilidad_4
        workSheet.Cell("D48").Value = dsDatos.Tables(0).Rows(0)(14).ToString 'responsabilidad_5
        workSheet.Cell("D50").Value = dsDatos.Tables(0).Rows(0)(15).ToString 'responsabilidad_6
        workSheet.Cell("D52").Value = dsDatos.Tables(0).Rows(0)(16).ToString 'responsabilidad_7
        workSheet.Cell("D54").Value = dsDatos.Tables(0).Rows(0)(17).ToString 'responsabilidad_8
        workSheet.Cell("D56").Value = dsDatos.Tables(0).Rows(0)(18).ToString 'responsabilidad_9
        workSheet.Cell("D58").Value = dsDatos.Tables(0).Rows(0)(19).ToString 'responsabilidad_10
        workSheet.Cell("D62").Value = dsDatos.Tables(0).Rows(0)(20).ToString 'responsabilidad_11
        workSheet.Cell("D64").Value = dsDatos.Tables(0).Rows(0)(21).ToString 'responsabilidad_12
        workSheet.Cell("D66").Value = dsDatos.Tables(0).Rows(0)(71).ToString 'responsabilidad_13
        workSheet.Cell("D70").Value = dsDatos.Tables(0).Rows(0)(22).ToString 'facultades_autorizacion
        workSheet.Cell("D78").Value = (dsDatos.Tables(0).Rows(0)(23).ToString) 'relacion_interna_quien
        workSheet.Cell("S78").Value = (dsDatos.Tables(0).Rows(0)(24).ToString) 'relacion_interna_para
        workSheet.Cell("D82").Value = (dsDatos.Tables(0).Rows(0)(25).ToString)  'relacion_externa_quien
        workSheet.Cell("S82").Value = (dsDatos.Tables(0).Rows(0)(26).ToString)  'relacion_externa_para
        workSheet.Cell("L90").Value = (dsDatos.Tables(0).Rows(0)(27).ToString) 'grado_escolaridad
        workSheet.Cell("L92").Value = (dsDatos.Tables(0).Rows(0)(28).ToString) 'carrera_especializacion
        workSheet.Cell("D98").Value = dsDatos.Tables(0).Rows(0)(29).ToString 'formacion_1
        workSheet.Cell("AE98").Value = dsDatos.Tables(0).Rows(0)(30).ToString 'domino_1
        workSheet.Cell("D100").Value = dsDatos.Tables(0).Rows(0)(31).ToString 'formacion_2
        workSheet.Cell("AE100").Value = dsDatos.Tables(0).Rows(0)(32).ToString 'domino_2
        workSheet.Cell("D102").Value = dsDatos.Tables(0).Rows(0)(33).ToString 'formacion_3
        workSheet.Cell("AE102").Value = dsDatos.Tables(0).Rows(0)(34).ToString 'domino_3
        workSheet.Cell("D104").Value = dsDatos.Tables(0).Rows(0)(35).ToString 'formacion_4
        workSheet.Cell("AE104").Value = dsDatos.Tables(0).Rows(0)(36).ToString 'domino_4
        workSheet.Cell("D106").Value = dsDatos.Tables(0).Rows(0)(37).ToString 'formacion_5
        workSheet.Cell("AE106").Value = dsDatos.Tables(0).Rows(0)(38).ToString 'domino_5
        workSheet.Cell("D108").Value = dsDatos.Tables(0).Rows(0)(39).ToString 'formacion_6
        workSheet.Cell("AE108").Value = dsDatos.Tables(0).Rows(0)(40).ToString 'domino_6
        workSheet.Cell("D112").Value = dsDatos.Tables(0).Rows(0)(41).ToString 'idioma_1
        workSheet.Cell("O112").Value = dsDatos.Tables(0).Rows(0)(42).ToString 'dominio_1
        workSheet.Cell("T112").Value = dsDatos.Tables(0).Rows(0)(43).ToString 'idioma_2
        workSheet.Cell("AE112").Value = dsDatos.Tables(0).Rows(0)(44).ToString 'dominio_2
        workSheet.Cell("D117").Value = dsDatos.Tables(0).Rows(0)(45).ToString 'habilidad_1
        workSheet.Cell("O117").Value = dsDatos.Tables(0).Rows(0)(46).ToString 'domino_1
        workSheet.Cell("T117").Value = dsDatos.Tables(0).Rows(0)(47).ToString 'habilidad_2
        workSheet.Cell("AE117").Value = dsDatos.Tables(0).Rows(0)(48).ToString 'domino_2
        workSheet.Cell("D119").Value = dsDatos.Tables(0).Rows(0)(49).ToString 'habilidad_3
        workSheet.Cell("O119").Value = dsDatos.Tables(0).Rows(0)(50).ToString 'domino_3
        workSheet.Cell("T119").Value = dsDatos.Tables(0).Rows(0)(51).ToString 'habilidad_4
        workSheet.Cell("AE119").Value = dsDatos.Tables(0).Rows(0)(52).ToString 'domino_4
        workSheet.Cell("D121").Value = dsDatos.Tables(0).Rows(0)(53).ToString 'habilidad_5
        workSheet.Cell("O121").Value = dsDatos.Tables(0).Rows(0)(54).ToString 'domino_5
        workSheet.Cell("T121").Value = dsDatos.Tables(0).Rows(0)(55).ToString 'habilidad_6
        workSheet.Cell("AE121").Value = dsDatos.Tables(0).Rows(0)(56).ToString 'domino_6
        workSheet.Cell("D123").Value = dsDatos.Tables(0).Rows(0)(57).ToString 'habilidad_7
        workSheet.Cell("O123").Value = dsDatos.Tables(0).Rows(0)(58).ToString 'domino_7
        workSheet.Cell("T123").Value = dsDatos.Tables(0).Rows(0)(59).ToString 'habilidad_8
        workSheet.Cell("AE123").Value = dsDatos.Tables(0).Rows(0)(60).ToString 'domino_8
        workSheet.Cell("F126").Value = dsDatos.Tables(0).Rows(0)(61).ToString 'word
        workSheet.Cell("J126").Value = dsDatos.Tables(0).Rows(0)(62).ToString 'excel
        workSheet.Cell("O126").Value = dsDatos.Tables(0).Rows(0)(63).ToString 'power_point
        workSheet.Cell("S126").Value = dsDatos.Tables(0).Rows(0)(64).ToString 'outlook
        workSheet.Cell("W126").Value = dsDatos.Tables(0).Rows(0)(65).ToString 'access
        workSheet.Cell("AA126").Value = dsDatos.Tables(0).Rows(0)(66).ToString 'project
        workSheet.Cell("I130").Value = dsDatos.Tables(0).Rows(0)(67).ToString 'areas_experiencia
        workSheet.Cell("AD130").Value = dsDatos.Tables(0).Rows(0)(68).ToString 'TIEMPO
        workSheet.Cell("D134").Value = dsDatos.Tables(0).Rows(0)(69).ToString 'sistema_gestion_integrado
        workSheet.Cell("D137").Value = dsDatos.Tables(0).Rows(0)(70).ToString 'requisitos_condiciones

        workBook.Save()

    End Sub
#End Region
#Region "Puesto"
    Private Sub ddlPuesto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPuesto.SelectedIndexChanged
        Call ObtInformacionPuestos()
        Call obtReporte()
        Call Comportamientos()
    End Sub
    ''Carga Datos del Puesto
    Public Sub ObtInformacionPuestos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_info_puesto_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlPuesto.SelectedValue)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                lblEmpresa.Text = "<strong>Compañía: </strong>" & odbLector(0).ToString
                lblNivel.Text = "<br/><strong>Nivel: </strong>" & odbLector(1).ToString
                lblFechas.Text = "<br/><strong>Fecha de elaboración: </strong>" & odbLector(28).ToString & _
             "<br/><strong>Fecha de actualización: </strong>" & odbLector(29).ToString
                odbLector.Close()
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'obtiene el Cotizacion 
    Public Sub obtReporte(Optional ByVal strBuscar As String = "")
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strFiltro As String = ""
        Dim srtQuery As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_puestos_jefe_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlPuesto.SelectedValue)


            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsCatalogo)
            grdDescriptivos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdDescriptivos.DataBind()

            If grdDescriptivos.Rows.Count = 0 Then
                grdDescriptivos.Visible = False
                lblMensaje.Text = "No hay Información para mostrar"
            Else
                grdDescriptivos.Visible = True
                lblMensaje.Text = ""

            End If

            odbConexion.Close()

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdDescriptivos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdDescriptivos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")

                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Comportamientos"
    Public Sub Comportamientos()
        If ddlPuesto.SelectedValue = "0" Or ddlPuesto.SelectedValue = "" Then
            lblEmpresa.Text = ""
            lblNivel.Text = ""
            lblFechas.Text = ""
            divDescriptivos.Visible = False
        Else
            divDescriptivos.Visible = True
        End If
    End Sub


#End Region
End Class