Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.Drawing

Public Class configuracionPuestos
    Inherits System.Web.UI.Page
    Public LLENAR_BUSQUEDA As String = ""


#Region "Configuracion menu y carga (seguridad)"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        ID = CType(Request.QueryString("id"), String)
        btnEdit.Visible = False
        btnNew.Visible = True
        lblError.Text = ""
        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            LlenarTablaBusqueda()
            LlenarFormularioddl()

            If Not String.IsNullOrEmpty(ID) Then
                LlenarFormulario(ID)
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Comportamiento", "<script>ComportamientosJS()</script>", False)
    End Sub


    Protected Sub btn_editar_rol_Click(sender As Object, e As EventArgs)
        ' Mostrar el TextBox y ocultar el DropDownList y el botón de editar
        ddl_rol.Style("display") = "none"
        txt_rol.Style("display") = "block"
        txt_rol.Text = ddl_rol.SelectedItem.Text
        btn_editar_rol.Style("display") = "none"
        btn_guardar_rol.Style("display") = "block"
    End Sub


    Protected Sub btn_guardar_rol_Click(sender As Object, e As EventArgs)
        Try
            ' Actualizar el texto del DropDownList con el valor del TextBox
            Dim selectedItem As ListItem = ddl_rol.Items.FindByValue(ddl_rol.SelectedValue)
            If selectedItem IsNot Nothing Then
                selectedItem.Text = txt_rol.Text
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('No se encontró el elemento seleccionado.');", True)
                Return
            End If

            ' Asegúrate de que hay un valor seleccionado
            If String.IsNullOrEmpty(ddl_rol.SelectedValue) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Por favor selecciona un rol.');", True)
                Return
            End If

            ' Configurar el SqlDataSource para la actualización
            sqlMain.UpdateCommand = "UPDATE DO_ROL_CT SET descripcion = @descripcion WHERE id = @id"
            sqlMain.UpdateParameters.Clear()
            sqlMain.UpdateParameters.Add("descripcion", txt_rol.Text)
            sqlMain.UpdateParameters.Add("id", ddl_rol.SelectedValue)

            ' Ejecutar el comando de actualización
            Dim rowsAffected As Integer = sqlMain.Update()

            ' Verificación de filas afectadas
            If rowsAffected > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('El rol se ha actualizado correctamente.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('No se actualizó ningún registro. Verifica el ID.');", True)
            End If

        Catch ex As Exception
            ' Manejo de excepciones y mostrar mensaje de error
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Ocurrió un error al actualizar el rol: " & ex.Message & "');", True)
        End Try

        ' Mostrar el DropDownList y ocultar el TextBox y el botón de guardar
        ddl_rol.Style("display") = "block"
        txt_rol.Style("display") = "none"
        btn_editar_rol.Style("display") = "block"
        btn_guardar_rol.Style("display") = "none"
    End Sub





    Sub LlenarFormulario(id_puesto As String)


        sqlMain.SelectCommand = "SELECT a.id, codigo, Descripcion,tipo_puesto, (SELECT DESCRIPCION FROM DO_NIVEL_CT AA WHERE AA.ID=A.NIVEL) AS Nivel, unidad_negocio, posiciones, fk_id_area, estatus, empresa, 
                                  (SELECT [descripcion] FROM DO_PUESTOS_TB WHERE ID = A.jefe) AS Nombre_Jefe, 
                                    a.jefe AS id_jefe,          
                                   (SELECT CASE WHEN LEN([descripcion])>55 THEN 
					                    SUBSTRING([descripcion] , 1, 55)
			                       ELSE
					                    [descripcion] 
				                    END  FROM DO_PUESTO_VT AA WHERE AA.CLAVE=A.clave_giro) AS Puesto_GIRO FROM DO_PUESTOS_TB A
                                    WHERE A.id='" & id_puesto & "' "

        Dim dvdescripcion As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvdescripcion

            txtCodigo.Text = drv("codigo").ToString

            ScriptManager.RegisterStartupScript(Me, [GetType](), "SetTextBoxValue", "document.getElementById('txtCodigo2').value = '" & drv("codigo").ToString() & "';", True)


            txtposicion.Text = drv("posiciones").ToString
            'txtDescripcion.Text = drv("descripcion").ToString

        Next

        ' Obtener el valor de 'estatus' y asignarlo a los radio buttons
        Dim estatus As String = ObtenerEstatus(id_puesto) ' Función para obtener el valor de 'estatus'
        If estatus IsNot Nothing Then
            If estatus = "1" Then
                radio1.Checked = True
            ElseIf estatus = "0" Then
                radio2.Checked = True
            End If
        End If


        ' Seleccionar el valor correspondiente al ID recibido en el URL en el DropDownList select1
        ' Realizar la lógica necesaria para seleccionar el valor correspondiente al ID recibido en el DropDownList select2
        ' Por ejemplo, suponiendo que hay un campo en la base de datos que relaciona el puesto con el nivel
        ' Debes obtener ese valor y seleccionar el ListItem correspondiente en select2
        Dim nivelPuesto As String = ObtenerNivelPuesto(id_puesto) ' Esta función debería obtener el nivel del puesto
        If nivelPuesto IsNot Nothing Then
            select2.SelectedValue = nivelPuesto
        End If

        Dim puesto As String = ObtenerPuesto(id_puesto) ' Esta función debería obtener el puesto
        If puesto IsNot Nothing Then
            ddl_puesto.ClearSelection() ' Limpiamos cualquier selección previa
            Dim item As ListItem = ddl_puesto.Items.FindByText(puesto) ' Buscamos el ListItem por el texto
            If item IsNot Nothing Then
                item.Selected = True ' Seleccionamos el ListItem encontrado
                Call ObtenerRoles_Puestos(ddl_puesto.SelectedValue)
            End If
        End If


        Dim rol As String = ObtenerRol(id_puesto) ' Esta función debería obtener el rol
        If rol IsNot Nothing Then
            ddl_rol.ClearSelection() ' Limpiamos cualquier selección previa
            Dim item As ListItem = ddl_rol.Items.FindByText(rol) ' Buscamos el ListItem por el texto
            If item IsNot Nothing Then
                item.Selected = True ' Seleccionamos el ListItem encontrado
            End If
        End If

        Dim giro As String = obtenerGiro(id_puesto) ' Esta función debería obtener el rol
        If giro IsNot Nothing Then
            ddl_Giro.ClearSelection() ' Limpiamos cualquier selección previa
            Dim item As ListItem = ddl_Giro.Items.FindByText(giro) ' Buscamos el ListItem por el texto
            If item IsNot Nothing Then
                item.Selected = True ' Seleccionamos el ListItem encontrado
            End If
        End If


        Dim jefe As String = obtenerJefe(id_puesto) ' Esta función debería obtener el rol
        If jefe IsNot Nothing Then
            ddl_jefe.ClearSelection() ' Limpiamos cualquier selección previa
            Dim item As ListItem = ddl_jefe.Items.FindByText(jefe) ' Buscamos el ListItem por el texto
            If item IsNot Nothing Then
                item.Selected = True ' Seleccionamos el ListItem encontrado
            End If
        End If

        Dim unidadNegocio As String = obtenerUnidad(id_puesto) ' Esta función debería obtener la unidad de negocio
        If unidadNegocio IsNot Nothing Then
            select3.SelectedValue = unidadNegocio
        End If

        Dim empresa As String = obtenerEmpresa(id_puesto) ' Esta función debería obtener la empresa
        If empresa IsNot Nothing Then
            select4.SelectedValue = empresa
        End If

        Dim tipoPuesto As String = obtenerTipo(id_puesto) ' Esta función debería obtener el tipo puesto
        If tipoPuesto IsNot Nothing Then
            select5.SelectedValue = tipoPuesto
        End If

        Dim infogiro As String = obtenerinfogiro(id_puesto) ' Esta función debería obtener el rol
        If infogiro IsNot Nothing Then
            ddl_infogiro.ClearSelection() ' Limpiamos cualquier selección previa
            Dim item As ListItem = ddl_infogiro.Items.FindByText(infogiro) ' Buscamos el ListItem por el texto
            If item IsNot Nothing Then
                item.Selected = True ' Seleccionamos el ListItem encontrado
            End If
        End If

        btnEdit.Visible = True
        btnNew.Visible = False


    End Sub


    Function obtenerinfogiro(id_puesto As String) As String
        Dim infogiro As String = Nothing
        sqlMain.SelectCommand = "SELECT DO_AREA_ADSCRIBE_TB.area_adscribe " &
                          "FROM DO_AREA_ADSCRIBE_TB " &
                          "JOIN DO_PUESTOS_TB ON DO_AREA_ADSCRIBE_TB.ID = DO_PUESTOS_TB.fk_id_area " &
                          "WHERE DO_PUESTOS_TB.ID = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            infogiro = dv(0)("area_adscribe").ToString()
        End If
        Return infogiro
    End Function


    Function obtenerTipo(id_puesto As String) As String
        Dim tipoPuesto As String = Nothing
        sqlMain.SelectCommand = "SELECT tipo_puesto FROM DO_PUESTOS_TB WHERE ID = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            tipoPuesto = dv(0)("tipo_puesto").ToString()
        End If
        Return tipoPuesto
    End Function


    Function obtenerEmpresa(id_puesto As String) As String
        Dim empresa As String = Nothing
        sqlMain.SelectCommand = "SELECT empresa FROM DO_PUESTOS_TB WHERE ID = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            empresa = dv(0)("empresa").ToString()
        End If
        Return empresa
    End Function

    Function obtenerUnidad(id_puesto As String) As String
        Dim unidad As String = Nothing
        sqlMain.SelectCommand = "SELECT unidad_negocio FROM DO_PUESTOS_TB WHERE ID = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            unidad = dv(0)("unidad_negocio").ToString()
        End If
        Return unidad
    End Function

    Function obtenerGiro(id_puesto As String) As String
        Dim giro As String = Nothing
        sqlMain.SelectCommand = "SELECT a.id,(SELECT CASE WHEN LEN([descripcion])>55 THEN 
					SUBSTRING([descripcion] , 1, 55)
			   ELSE
					[descripcion] 
				END  FROM DO_PUESTO_VT AA WHERE AA.CLAVE=A.clave_giro) AS puesto_giro FROM DO_PUESTOS_TB A
                WHERE A.id='" & id_puesto & "'"

        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            giro = dv(0)("puesto_giro").ToString()
        End If
        Return giro
    End Function

    Function obtenerJefe(id_puesto As String) As String
        Dim jefe As String = Nothing
        sqlMain.SelectCommand = "SELECT a.id, a.jefe AS id_jefe, 
                               (SELECT d.descripcion FROM DO_PUESTOS_TB AS d WHERE d.ID = a.jefe) AS nombre_jefe 
                                FROM DO_PUESTOS_TB a 
                                WHERE a.id ='" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            jefe = dv(0)("nombre_jefe").ToString()
        End If
        Return jefe
    End Function

    Function ObtenerEstatus(id_puesto As String) As String


        Dim estatus As String = Nothing
        sqlMain.SelectCommand = "SELECT estatus FROM DO_PUESTOS_TB WHERE id = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            estatus = dv(0)("Estatus").ToString()
        End If
        Return estatus
    End Function



    Function ObtenerNivelPuesto(id_puesto As String) As String
        Dim nivel As String = Nothing
        sqlMain.SelectCommand = "SELECT NIVEL FROM DO_PUESTOS_TB WHERE ID = '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            nivel = dv(0)("NIVEL").ToString()
        End If
        Return nivel
    End Function

    Function ObtenerPuesto(id_puesto As String) As String
        Dim puesto As String = Nothing
        sqlMain.SelectCommand = "SELECT c.descripcion AS descripcion_puesto FROM DO_PUESTOS_TB a INNER JOIN dbo.DO_PUESTO_CT AS c ON a.fk_id_puesto=c.id WHERE a.id= '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            puesto = dv(0)("descripcion_puesto").ToString()
        End If
        Return puesto
    End Function

    Function ObtenerRol(id_puesto As String) As String
        Dim rol As String = Nothing
        sqlMain.SelectCommand = "SELECT c.descripcion AS descripcion_rol FROM DO_PUESTOS_TB a INNER JOIN dbo.DO_ROL_CT AS c ON a.fk_id_rol=c.id WHERE a.id= '" & id_puesto & "'"
        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        If dv.Count > 0 Then
            rol = dv(0)("descripcion_rol").ToString()
        End If
        Return rol
    End Function



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
            '   strNombreUsuario = "practicasdo"
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
        strPaginaActual = HttpContext.Current.Request.Url.AbsolutePath.Replace("/", "")
        'If validaPaginaHabilitada(strPaginaActual, odbConexion) = False Then Response.Redirect("sinacceso.html")

        'Genera Menu de Pagina
        Call GeneraMenu(dsDatos, odbConexion)


    End Sub


#Region "Menu"

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
        strPaginaActual = HttpContext.Current.Request.Url.AbsolutePath.Replace("/", "")
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


#End Region



#Region "Llenar tablas BD"

    Sub LlenarTablaBusqueda()

        LLENAR_BUSQUEDA = ""

        sqlMain.SelectCommand = "SELECT 
                                a.id, 
                                a.codigo, 
                                a.descripcion,
                                b.descripcion AS descripcion_puesto,
                                c.descripcion AS descripcion_rol,
                                a.tipo_puesto, 
                                (SELECT aa.descripcion FROM DO_NIVEL_CT aa WHERE aa.id=a.nivel) AS nivel, 
                                a.unidad_negocio,
                                a.posiciones, 
                                a.area_adscribe, 
                                a.estatus,
                                a.empresa,      
                                (SELECT area_adscribe FROM DO_AREA_ADSCRIBE_TB AA WHERE AA.id=A.fk_id_area) AS Desarea_adscribe,
                                (SELECT d.descripcion FROM DO_PUESTOS_TB AS d WHERE d.ID = A.jefe) AS nombre_Jefe, 
                                a.jefe AS id_jefe,
                                (SELECT [descripcion]
                                 FROM DO_PUESTO_VT
                                 WHERE CLAVE = a.clave_giro) AS puesto_giro
                            FROM DO_PUESTOS_TB a
                            INNER JOIN dbo.DO_PUESTO_CT AS b ON b.id = a.fk_id_puesto
                            INNER JOIN dbo.DO_ROL_CT AS c ON c.id = a.fk_id_rol"

        Dim dv As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)

        For Each drv As DataRowView In dv

            Dim codigoEmpresa As String = drv("empresa").ToString
            Dim estatus As String = drv("estatus").ToString
            Dim nomestatus As String = If(estatus = "1", "habilitado", "deshabilitado")
            Dim nombreEmpresa As String = ObtenerNombreEmpresa(codigoEmpresa)
            LLENAR_BUSQUEDA &= "<tr ondblclick='$(location).attr(""href"",""configuracionPuestos.aspx?id=" & drv("id").ToString & """);'>" &
                                     "<td>" & drv("codigo").ToString & "</td>" &
                                     "<td>" & drv("descripcion_puesto").ToString & "</td>" &
                                     "<td> " & drv("descripcion_rol").ToString & " </td>" &
                                     "<td>" & drv("tipo_puesto").ToString & "</td>" &
                                     "<td>" & drv("nivel").ToString & "</td>" &
                                     "<td>" & drv("unidad_negocio").ToString & "</td>" &
                                     "<td>" & nombreEmpresa & "</td>" &
                                     "<td>" & nomestatus & "</td>" &
                                     "<td>" & drv("Desarea_adscribe").ToString & "</td>" &
                                     "<td>" & drv("posiciones").ToString & "</td>" &
                                     "<td>" & drv("nombre_jefe").ToString & "</td>" &
                                     "<td>" & drv("puesto_giro").ToString & "</td>" &
                                     "</tr>"

        Next

    End Sub



    Function ObtenerNombreEmpresa(codigoEmpresa As String) As String
        Dim nombreEmpresa As String = String.Empty
        Select Case codigoEmpresa
            Case "001"
                nombreEmpresa = "Passa Administración y Servicios S.A. de C.V"
            Case "002"
                nombreEmpresa = "DINA CAMIONES"
            Case "003"
                nombreEmpresa = "MERCADER FINANCIAL"
            Case "004"
                nombreEmpresa = "DINA COMERCIALIZACION AUTOMOTRIZ (DICOMER)"
            Case "005"
                nombreEmpresa = "TRANSPORTES Y LOGISTICA DE JALISCO"
            Case "006"
                nombreEmpresa = "DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV (DICOSER)"
            Case Else
                nombreEmpresa = "Empresa no encontrada"
        End Select
        Return nombreEmpresa
    End Function




#End Region

#Region "DDL"
    Sub LlenarFormularioddl()
        ' Llenar el DropDownList select1 con los puestos

        ' Llenar el DropDownList select2 con los niveles
        sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_NIVEL_CT ORDER BY descripcion"
        Dim dvNiveles As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvNiveles
            Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
            select2.Items.Add(item)
        Next

        sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_PUESTO_CT ORDER BY descripcion"
        Dim dvPuestos As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvPuestos
            Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
            ddl_Puesto.Items.Add(item)
        Next

        sqlMain.SelectCommand = "SELECT id AS id_jefe, descripcion AS nombre_jefe FROM DO_PUESTOS_TB ORDER BY [descripcion]"
        Dim dvJefe As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvJefe
            Dim item As New ListItem(drv("nombre_jefe").ToString(), drv("id_jefe").ToString())
            ddl_jefe.Items.Add(item)
        Next



        sqlMain.SelectCommand = " SELECT clave , descripcion  FROM DO_PUESTO_VT  ORDER BY descripcion"

        Dim dvGiro As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvGiro
            Dim item As New ListItem(drv("descripcion").ToString(), drv("clave").ToString())
            ddl_Giro.Items.Add(item)
        Next




        '' Llenar el DropDownList select3 con las unidades de negocio
        'sqlMain.SelectCommand = "SELECT id, unidad_negocio FROM DO_PUESTOS_TB"
        'Dim dvUnidad As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        'For Each drv As DataRowView In dvUnidad
        '    Dim item As New ListItem(drv("unidad_negocio").ToString(), drv("id").ToString())
        '    select3.Items.Add(item)
        'Next

        'sqlMain.SelectCommand = "SELECT id, empresa FROM DO_PUESTOS_TB"
        'Dim dvCompañia As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        'For Each drv As DataRowView In dvCompañia
        '    Dim item As New ListItem(drv("empresa").ToString(), drv("id").ToString())
        '    select4.Items.Add(item)
        'Next



        sqlMain.SelectCommand = "SELECT id, area_adscribe FROM DO_AREA_ADSCRIBE_TB ORDER BY area_adscribe"
        Dim dvAreaAds As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvAreaAds
            Dim item As New ListItem(drv("area_adscribe").ToString(), drv("id").ToString())
            ddl_infogiro.Items.Add(item)
        Next
    End Sub

    Protected Sub ddlpuesto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_puesto.SelectedIndexChanged
        If ddl_puesto.SelectedValue <> "" Then
            Call ObtenerRoles_Puestos(ddl_puesto.SelectedValue)
        Else

        End If

    End Sub

#End Region

    Sub ObtenerRoles_Puestos(Rol_puesto As String)

        ddl_rol.Items.Clear()
        sqlMain.SelectCommand = "SELECT 
	                            b.[id] AS id_rol, b.[descripcion] AS descripcion_rol
                              FROM [dbo].[DO_PUESTO_ROL] AS a 
                              INNER JOIN [dbo].[DO_ROL_CT] AS b ON b.[id] = a.[fk_id_rol]
                              WHERE a.[fk_id_puesto]=  '" & Rol_puesto & "'"



        Dim dvRol_puesto As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvRol_puesto
            Dim item As New ListItem(drv("descripcion_rol").ToString(), drv("id_rol").ToString())
            ddl_rol.Items.Add(item)
        Next


    End Sub

#Region "Crud catalogo"

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs)
        Dim id As String = Request.QueryString("id") ' Obtener el ID desde la URL
        If String.IsNullOrEmpty(id) Then
            Dim script As String = "alert('Seleccione registro para editar  .');"
            ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)
            LlenarTablaBusqueda()
            Return
        End If

        Dim idExiste As Boolean = IdExisteEnBaseDeDatos(id)
        If idExiste Then
            ActualizarEnBaseDeDatos()
            LlenarTablaBusqueda()
        Else
            Dim script As String = "alert('No existe el registro.');"
            ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)
        End If
    End Sub








    'Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
    '    Dim id As String = Request.QueryString("id") ' Obtener el ID desde la URL
    '    If String.IsNullOrEmpty(id) Then
    '        ' Mostrar un mensaje de alerta indicando que no se ha seleccionado ningún elemento
    '        Dim script As String = "alert('Por favor, seleccione elemento a eliminar'); window.location.href = 'configuracionPuestos.aspx';"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)
    '        Return  
    '    End If
    '    ' Verificar si el ID existe en la base de datos
    '    Dim idExiste As Boolean = IdExisteEnBaseDeDatos(id)
    '    If idExiste Then
    '        ' El ID existe en la base de datos, llamar a la función para eliminar

    '    Else
    '        ' Mostrar un mensaje de alerta indicando que el elemento no existe
    '        Dim script As String = "alert('El elemento que intenta eliminar no existe');"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)
    '    End If
    'End Sub


    Protected Sub btnNew_Click(sender As Object, e As EventArgs)
        Dim codigo As String = Request.Form("txtCodigo") ' Obtener el código del formulario
        If String.IsNullOrEmpty(codigo) Then
            ' Mostrar un mensaje indicando que el código es obligatorio
            Dim script As String = "alert('Por favor, ingrese un código.');"
            ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)

            Return
        End If

        ' Verificar si el código ya existe en la base de datos
        Dim codigoExiste As Boolean = CodigoExisteEnBaseDeDatos(codigo)
        If codigoExiste Then
            ' Mostrar un mensaje indicando que el código ya existe
            Dim script As String = "alert('Ya existe el registro en la base de datos');"
            ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)

            LlenarTablaBusqueda()

        Else
            ' El código no existe en la base de datos, llamar a la función de insertar
            InsertarEnBaseDeDatos()
        End If
    End Sub


    Private Sub ActualizarEnBaseDeDatos()
        ' Obtener los valores de los controles en el formulario
        Dim codigo As String = txtCodigo.Text
        Dim rol As String = ddl_rol.SelectedValue
        Dim puesto As String = ddl_puesto.SelectedValue

        Dim jefe As String = ddl_jefe.SelectedValue
        Dim giro As String = ddl_Giro.SelectedValue
        Dim nivel As String = select2.SelectedValue

        Dim tipoPuesto As String = select5.SelectedValue
        Dim empresa As String = select4.SelectedValue


        Dim posicion As String = txtposicion.Text
        Dim comentarios As String = TextBox2.Text
        Dim estatus As String = If(radio1.Checked, "1", "0")

        Dim puestoId As String = ddl_puesto.SelectedValue
        Dim rolId As String = ddl_rol.SelectedValue
        Dim infogiro As String = ddl_infogiro.SelectedValue

        Dim puestoDescripcion As String = ddl_puesto.SelectedItem.Text
        Dim rolDescripcion As String = ddl_rol.SelectedItem.Text

        ' Concatenar descripción de puesto y rol
        Dim descripcion As String = $"{puestoDescripcion} de {rolDescripcion}"

        Dim unidadNegocio As String = select3.SelectedValue
        Dim id As String = Request.QueryString("id") ' Obtener el ID desde la URL

        ' Actualizar en la tabla DO_PUESTOS_TB utilizando el SQLDataSource
        sqlMain.UpdateCommand = "UPDATE DO_PUESTOS_TB
                                    SET codigo= @codigo,
                                    descripcion = @descripcion,
                                    nivel = @nivel,
                                    tipo_puesto = @tipo_puesto,
                                    empresa = @empresa,
                                    comentarios = @comentarios,
                                    estatus = @estatus,
                                    unidad_negocio = @unidad_negocio,
                                    fecha_modificacion = GETDATE(),
                                    usuario_modificacion = @usuario_modificacion,
                                    posiciones = @posiciones,
                                    jefe = @jefe,
                                    clave_giro = @clave_giro,
                                    fk_id_puesto = @fk_id_puesto,
                                    fk_id_rol = @fk_id_rol,
                                    fk_id_area = @fk_id_area
                                WHERE id = @id;"

        sqlMain.UpdateParameters.Clear()
        sqlMain.UpdateParameters.Add("codigo", codigo)
        sqlMain.UpdateParameters.Add("descripcion", descripcion)
        sqlMain.UpdateParameters.Add("nivel", nivel)
        sqlMain.UpdateParameters.Add("tipo_puesto", tipoPuesto)
        sqlMain.UpdateParameters.Add("empresa", empresa)
        sqlMain.UpdateParameters.Add("comentarios", comentarios)
        sqlMain.UpdateParameters.Add("estatus", estatus)
        sqlMain.UpdateParameters.Add("unidad_negocio", unidadNegocio)
        sqlMain.UpdateParameters.Add("usuario_modificacion", hdUsuario.Value)
        sqlMain.UpdateParameters.Add("posiciones", posicion)
        sqlMain.UpdateParameters.Add("fk_id_puesto", puestoId)
        sqlMain.UpdateParameters.Add("fk_id_rol", rolId)
        sqlMain.UpdateParameters.Add("jefe", jefe)
        sqlMain.UpdateParameters.Add("clave_giro", giro)
        sqlMain.UpdateParameters.Add("fk_id_area", infogiro)
        sqlMain.UpdateParameters.Add("id", id)


        sqlMain.Update()

        ' Limpiar los controles después de la actualización si es necesario
        LimpiarControles()

        ' Mostrar un mensaje utilizando JavaScript
        Dim script As String = "alert('Datos actualizados correctamente');  window.location.href = 'configuracionPuestos.aspx';"
        ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)

        LlenarTablaBusqueda()
    End Sub


    Private Sub EliminarDeBaseDeDatos(id As String)

        sqlMain.DeleteCommand = "DELETE FROM DO_PUESTOS_TB WHERE id = @id"

        sqlMain.DeleteParameters.Clear()
        sqlMain.DeleteParameters.Add("id", id)

        sqlMain.Delete()
        LimpiarControles()

        ' Mostrar un mensaje utilizando JavaScript
        Dim script As String = "alert('Registro Eliminado Correctamente');"
        ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)

        LlenarTablaBusqueda()
    End Sub



    Private Function IdExisteEnBaseDeDatos(id As String) As Boolean
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("sqlConnectioncustom").ConnectionString
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT id FROM DO_PUESTOS_TB WHERE Id = @Id"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Id", id)
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Function CodigoExisteEnBaseDeDatos(codigo As String) As Boolean
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("sqlConnectioncustom").ConnectionString
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT codigo FROM DO_PUESTOS_TB WHERE Codigo = @Codigo"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Codigo", codigo)
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function


    Private Sub InsertarEnBaseDeDatos()
        ' Obtener los valores de los controles en el formulario
        Dim codigo As String = txtCodigo.Text
        Dim puestoId As String = ddl_puesto.SelectedValue
        Dim rolId As String = ddl_rol.SelectedValue
        Dim nivel As String = select2.SelectedValue
        Dim tipoPuesto As String = select5.SelectedValue
        Dim jefe As String = ddl_jefe.SelectedValue
        Dim giro As String = ddl_Giro.SelectedValue
        Dim empresa As String = select4.SelectedValue

        Dim comentarios As String = TextBox2.Text
        Dim estatus As String = If(radio1.Checked, "1", "0")
        Dim posiciones As String = txtposicion.Text
        Dim unidadNegocio As String = select3.SelectedValue
        Dim infogiro As String = ddl_infogiro.SelectedValue

        ' Obtener la descripción de puesto y rol
        Dim puestoDescripcion As String = ddl_puesto.SelectedItem.Text
        Dim rolDescripcion As String = ddl_rol.SelectedItem.Text

        ' Concatenar descripción de puesto y rol
        Dim descripcion As String = $"{puestoDescripcion} de {rolDescripcion}"

        ' Insertar en la tabla DO_PUESTOS_TB utilizando el SQLDataSource
        sqlMain.InsertCommand = "INSERT INTO DO_PUESTOS_TB (codigo, descripcion, nivel, tipo_puesto, empresa,
                        comentarios, estatus, unidad_negocio, fecha_creacion, usuario_creacion, posiciones, jefe, clave_giro, fk_id_puesto, fk_id_rol, fk_id_area) VALUES (@codigo, @descripcion, 
                        @nivel, @tipo_puesto, @empresa, @comentarios, @estatus, @unidad_negocio, GETDATE(), @usuario_creacion, @posiciones, @jefe, @clave_giro, @fk_id_puesto, @fk_id_rol, @fk_id_area)"
        sqlMain.InsertParameters.Clear()
        sqlMain.InsertParameters.Add("codigo", codigo)
        sqlMain.InsertParameters.Add("descripcion", descripcion)
        sqlMain.InsertParameters.Add("nivel", nivel)
        sqlMain.InsertParameters.Add("tipo_puesto", tipoPuesto)
        sqlMain.InsertParameters.Add("empresa", empresa)
        sqlMain.InsertParameters.Add("comentarios", comentarios)
        sqlMain.InsertParameters.Add("estatus", estatus)
        sqlMain.InsertParameters.Add("unidad_negocio", unidadNegocio)
        sqlMain.InsertParameters.Add("posiciones", posiciones)
        sqlMain.InsertParameters.Add("usuario_creacion", hdUsuario.Value)
        sqlMain.InsertParameters.Add("fk_id_puesto", puestoId)
        sqlMain.InsertParameters.Add("fk_id_rol", rolId)
        sqlMain.InsertParameters.Add("jefe", jefe)
        sqlMain.InsertParameters.Add("clave_giro", giro)
        sqlMain.InsertParameters.Add("fk_id_area", infogiro)

        ' Ejecutar la inserción
        sqlMain.Insert()

        ' Limpiar los controles después de la inserción si es necesario
        LimpiarControles()

        ' Mostrar un mensaje utilizando JavaScript
        Dim script As String = "alert('Datos insertados correctamente');"
        ClientScript.RegisterStartupScript(Me.GetType(), "alertScript", script, True)

        LlenarTablaBusqueda()
    End Sub




    Private Sub LimpiarControles()
        txtCodigo.Text = ""
        select2.SelectedIndex = -1
        select5.SelectedIndex = -1
        select4.SelectedIndex = -1

        TextBox2.Text = ""
        ddl_puesto.SelectedIndex = -1
        ddl_rol.SelectedIndex = -1
        radio1.Checked = False
        radio2.Checked = False
        select3.SelectedIndex = -1
    End Sub

#End Region



End Class