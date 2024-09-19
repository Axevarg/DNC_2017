Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class AltaRegistroCursos
    Inherits System.Web.UI.Page
    Private strCurso As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        strCurso = Request.QueryString("iCR")
        ' strCurso = 1
        If Not Page.IsPostBack Then
            If IsNumeric(strCurso) = False Then
                Response.Redirect("sinacceso.html")
            End If

            hdIdCurso.Value = strCurso
            Call obtenerUsuarioAD()
            Call obtCursosRegistrados(strCurso)
            Call ObtAgendaCurso()
        End If
        Call Comportamiento()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogos"



    Private Sub ObtHorarios(ByVal odbConexion As OleDbConnection, ByVal strCurso As String)
        Dim strFiltro As String = ""
        Dim strQuery As String = ""

        strQuery += "SELECT  id,'De '+CONVERT(varchar,fecha_desde,105 ) +' a '+ CONVERT(varchar,fecha_hasta,105) +' en Horario '+ horario as descripcion " & _
                    " FROM GC_REGISTRO_HORARIOS_TB WHERE fk_id_registro_curso=" & strCurso & " ORDER BY ID"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlHorarios.DataSource = odbLector
        ddlHorarios.DataValueField = "ID"
        ddlHorarios.DataTextField = "descripcion"

        ddlHorarios.DataBind()
        ddlHorarios.Items.Insert(0, New ListItem("<Seleccionar>", "0"))
    End Sub
#End Region
#Region "Comportamientos"
    Public Sub Comportamiento()

        If hdEstatusCurso.Value = "REGISTRADO" Then
            btnAgregar.Visible = False

            divCursosMensaje.Visible = True
            divCursosMensaje.Attributes("class") = "callout callout-success"
            divAuto.Visible = True
            divHorariosSel.Visible = False
        Else
            divHorariosSel.Visible = True
            btnAgregar.Visible = True
            divAuto.Visible = False
        End If

        'End If


    End Sub
#End Region
#Region "Acciones"
    Public Sub obtCursosRegistrados(ByVal idCurso As String)
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet


        Try
            odbConexion.Open()
            Dim strExtencion As String = ""
            'obtiene la informacion de las cartas configuradas al siguiente fecha de calendario
            strQuery = "  select  A.descripcion_capacitacion_corta " & _
                       "         ,A.objetivo" & _
                       "	     ,A.duracion" & _
                       "	     ,B.comentarios" & _
                      "	     ,B.comentarios_correo" & _
                       "  FROM  DNC_CURSOS_TB A INNER JOIN GC_REGISTRO_CURSO_TB B ON" & _
                       "            A.ID = B.fk_id_curso" & _
                       "            WHERE B.ID = " & idCurso

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                lblNombreCurso.Text = odbLector(0).ToString
                lblObjetivo.Text = odbLector(1).ToString
                lblDuracion.Text = odbLector(2).ToString & " Hrs"
                txtDescripcion.Text = odbLector(3).ToString
                hdComentarios.Value = odbLector(4).ToString
                odbLector.Close()
            End If
            ' Obtiene los horarios
            Call ObtHorarios(odbConexion, idCurso)
            GC.Collect()
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub obtCuposHorarios(ByVal idHorario As String)
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet


        Try
            odbConexion.Open()
            Dim strExtencion As String = ""
            'obtiene la informacion de las cartas configuradas al siguiente fecha de calendario
            strQuery = "  select  A.no_personas " & _
                       "         ,A.no_personas - (SELECT COUNT(*) FROM GC_REGISTRO_CURSO_COLABORADORES_TB WHERE fk_id_registro_horario=" & idHorario & ")" & _
                       "  FROM GC_REGISTRO_HORARIOS_TB A " & _
                       "            WHERE A.ID = " & idHorario

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                lblLugares.Text = odbLector(0).ToString
                lblLugaresDisponibles.Text = odbLector(1).ToString

                odbLector.Close()
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
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

                GC.Collect()
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

        lblPerfil.Text = "Pefil: " & obtNombrePerfil(hdRol.Value, odbConexion)

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

#End Region



    Private Sub ddlHorarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlHorarios.SelectedIndexChanged
        Call obtCuposHorarios(ddlHorarios.SelectedValue)
    End Sub

    Private Sub btnAgregar_ServerClick(sender As Object, e As EventArgs) Handles btnAgregar.ServerClick
        Call AgendaCurso()
    End Sub

    Public Sub AgendaCurso()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strMensaje As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_agenda_curso_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdCurso", hdIdCurso.Value)
            odbComando.Parameters.AddWithValue("@PIdHorario", ddlHorarios.SelectedValue)
            odbComando.Parameters.AddWithValue("@PIdColaborador", hdClaveEmpleadoAD.Value)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)

            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                strMensaje = odbLector(0).ToString
                odbLector.Close()
            End If
            If strMensaje <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('" + strMensaje + "');</script>", False)
            End If
            Call obtCuposHorarios(ddlHorarios.SelectedValue)
            Call ObtAgendaCurso()
            Call Comportamiento()
            Call FormatoCorreo()
            odbConexion.Close()
        Catch ex As Exception
            lblError.ForeColor = Color.Red
            lblError.Text = ex.Message
        End Try

    End Sub
    'Obtiene Datos del Registro


    Public Sub ObtAgendaCurso()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strMensaje As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "gc_agenda_curso_datos_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdCurso", hdIdCurso.Value)
            odbComando.Parameters.AddWithValue("@PIdColaborador", hdClaveEmpleadoAD.Value)


            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                hdEstatusCurso.Value = "REGISTRADO"
                lblCurso.Text = "Curso Agendado <strong>" & lblNombreCurso.Text & "</strong> en Horario <strong>" & odbLector(0).ToString & "</strong>."
                odbLector.Close()
            Else
                hdEstatusCurso.Value = ""
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblError.ForeColor = Color.Red
            lblError.Text = ex.Message
        End Try

    End Sub

#Region "Correo"

    Public Sub FormatoCorreo()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim arrValores As New ArrayList
        Dim strCuerpo As String = ""
        Dim strUrl As String = ""
        Dim strAsunto As String = ""
        Dim strDestrinatario As String = ""
        Dim strCopia As String = ""
        Dim strNombreArchivos As String = ""
        Dim strEncabezadoCuerpo As String = ""
        Try
            odbConexion.Open()

            'Valida el tipo de Correo que tiene que enviar
            strAsunto = "SIGIDO | Capacitación Agendada"
            strDestrinatario = obtCorreoColaborador(hdClaveEmpleadoAD.Value, odbConexion)
            strEncabezadoCuerpo = "Estimado (a) " & lblNombre.Text & ".<br /> <br />Nos complace informarte, que ha quedado agendada la siguiente capacitación:"

            If strDestrinatario = "" Then Exit Sub

            'Cuerpo
            strCuerpo = "<p style=""font-family:Arial; font-size: 11pt;"">" & strEncabezadoCuerpo & " </p>" & vbCrLf

            'tabla de contenido
            strCuerpo = strCuerpo & "<table style=""font-family:Arial; font-size: 11pt;""> " & vbCrLf
            strCuerpo = strCuerpo & "<tr><td><p>Nombre: </p></td> <td>" & lblNombreCurso.Text & "  </td></tr>" & vbCrLf
            strCuerpo = strCuerpo & "<tr><td><p>Objetivo: </p></td> <td>" & lblObjetivo.Text & "  </td></tr>" & vbCrLf
            strCuerpo = strCuerpo & "<tr><td>Horario: </td> <td>" & ddlHorarios.SelectedItem.Text & "</td></tr>" & vbCrLf
            strCuerpo = strCuerpo & "<tr><td>Comentarios: </td> <td>" & hdComentarios.Value & "</td></tr>" & vbCrLf
            strCuerpo = strCuerpo & "</table> " & vbCrLf
            strCuerpo = strCuerpo & "<hr />" & vbCrLf
            strCuerpo = strCuerpo & "<p style=""font-family:Arial;"">Atentamente,</p>" & vbCrLf
            strCuerpo = strCuerpo & "<strong><p style=""font-family:Arial; font-size: 11pt;"">Desarrollo Organizacional</p></strong>" & vbCrLf & vbCrLf
            strCuerpo = strCuerpo & "<img src=""Desarrollo.jpg""/> " & vbCrLf & vbCrLf & vbCrLf






            'Envia Correo
            Call EnviaCorreo(strDestrinatario, strAsunto, strCuerpo, strCopia, odbConexion)

            arrValores = Nothing
            odbConexion.Close()
        Catch ex As Exception

            arrValores = Nothing
            GC.Collect()
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Function obtCorreoColaborador(strClave As String, odbConexion As OleDbConnection)
        Dim arrValores As New ArrayList
        Dim strResultado As String = ""

        'Cuenta de correo electronico destinatario puede ser mas de 1 separados por ";"


        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "sigido_obt_empleado_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@PIdClave", hdClaveEmpleadoAD.Value)


        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()

        End If

        arrValores = Nothing

        Return strResultado.Trim
    End Function
    'Ejecuta Crear Texto
    Public Sub EnviaCorreo(Destinatario As String, Asunto As String, Cuerpo As String, Copia As String, odbConexion As OleDbConnection)
        Dim arrValores As New ArrayList
        Dim strResultado As String = ""


        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "sigido_enviaremail_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@destinatario", Destinatario)
        odbComando.Parameters.AddWithValue("@asunto", Asunto)
        odbComando.Parameters.AddWithValue("@cuerpo", Cuerpo)
        odbComando.Parameters.AddWithValue("@Concopia", Copia)

        odbComando.ExecuteNonQuery()


        arrValores = Nothing
    End Sub
#End Region
End Class