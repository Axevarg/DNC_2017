Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
'Imports Microsoft.Office.Interop.Excel

Public Class IdentificacionPuesto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""

        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call CargarCatalogos()
            Call LlenarFormularioddl()
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Comportamiento", "<script>ComportamientosJS()</script>", False)
        Call Comportamientos()
    End Sub


    Sub LlenarFormularioddl()
        Dim ddlTipoPuesto As DropDownList = CType(FindControl("ddlTipoPuesto"), DropDownList)

        ' Crear un nuevo SqlDataSource y configurarlo
        Dim sqlMain As New SqlDataSource()
        sqlMain.ConnectionString = ConfigurationManager.ConnectionStrings("sqlConnectioncustom").ConnectionString
        sqlMain.SelectCommand = "SELECT id, CONCAT(codigo, ' - ', descripcion) AS descripcion_completa  FROM DO_PUESTOS_TB"

        ' Obtener los datos y asignarlos al DropDownList
        ddlTipoPuesto.DataSource = sqlMain
        ddlTipoPuesto.DataTextField = "descripcion_completa"
        ddlTipoPuesto.DataValueField = "id"
        ddlTipoPuesto.DataBind()

        ' Agregar un elemento al principio de la lista
        ddlTipoPuesto.Items.Insert(0, New ListItem("Selecciona...", ""))
    End Sub


#Region "Catalogo"
    Public Sub CargarCatalogos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Call ObtCarrera(odbConexion)
            Call ObtEscolaridad(odbConexion)
            Call ObtFacultadesAutorizacion(odbConexion)
            Call ObtIdioma(odbConexion)
            Call ObtTiempo(odbConexion)
            Call ObtColaborador(odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub


    Private Sub ObtCarrera(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID, DESCRIPCION AS DESCRIPCION FROM DO_CARRERAS_CT  ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlCarreras.DataSource = odbLector
        ddlCarreras.DataValueField = "id"
        ddlCarreras.DataTextField = "DESCRIPCION"
        ddlCarreras.DataBind()

    End Sub
    Private Sub ObtEscolaridad(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID, DESCRIPCION AS DESCRIPCION FROM DO_ESCOLARIDAD_CT  ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlEscolaridad.DataSource = odbLector
        ddlEscolaridad.DataValueField = "id"
        ddlEscolaridad.DataTextField = "DESCRIPCION"
        ddlEscolaridad.DataBind()
    End Sub

    Private Sub ObtColaborador(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT [CLAVE] " &
                            " ,[NOMBRE]+space(1)+[APPAT]+space(1)+[APMAT] as COMPLETO " &
                            " FROM [SGIDO_INFOGIRO_GIRO_VT] " &
                            " where estatus='activo' and TIPO_NOMINA<>'sem' " &
                            " ORDER BY 2 ASC"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlColaboradoresEnviar.DataSource = odbLector
        ddlColaboradoresEnviar.DataValueField = "CLAVE"
        ddlColaboradoresEnviar.DataTextField = "COMPLETO"
        ddlColaboradoresEnviar.DataBind()

        'Copia Jefe

        odbComando = New OleDbCommand(sQry, odbConexion)
        odbLector = odbComando.ExecuteReader()

        ddlCopiaJefe.DataSource = odbLector
        ddlCopiaJefe.DataValueField = "CLAVE"
        ddlCopiaJefe.DataTextField = "COMPLETO"
        ddlCopiaJefe.DataBind()
        'Obtienen DO
        Call ObtColaboradoresDesarrolloOrganizacional(odbConexion)
    End Sub
    'Obtiene el Listado del Personal de DO
    Private Sub ObtColaboradoresDesarrolloOrganizacional(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT CLAVE,NOMBRE FROM SIGIDO_USUARIOS_TB WHERE ROL IN (SELECT ID FROM SIGIDO_PERFILES_CT WHERE administrador=1)"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlCorreoDO.DataSource = odbLector
        ddlCorreoDO.DataValueField = "CLAVE"
        ddlCorreoDO.DataTextField = "NOMBRE"
        ddlCorreoDO.DataBind()
    End Sub
    Private Sub ObtFacultadesAutorizacion(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID, DESCRIPCION AS DESCRIPCION FROM DO_FACULTADES_AUTORIZACION_CT  ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlFacultadesAutorizacion.DataSource = odbLector
        ddlFacultadesAutorizacion.DataValueField = "id"
        ddlFacultadesAutorizacion.DataTextField = "DESCRIPCION"
        ddlFacultadesAutorizacion.DataBind()
    End Sub

    Private Sub ObtTiempo(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID, DESCRIPCION AS DESCRIPCION FROM DO_TIEMPOS_CT  ORDER BY ID"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlTiempo.DataSource = odbLector
        ddlTiempo.DataValueField = "id"
        ddlTiempo.DataTextField = "DESCRIPCION"
        ddlTiempo.DataBind()
    End Sub
    Private Sub ObtIdioma(ByVal odbConexion As OleDbConnection)

        Dim sQry As String = "SELECT  ID,DESCRIPCION AS DESCRIPCION FROM DO_IDIOMAS_CT  ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(sQry, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlIdioma.DataSource = odbLector
        ddlIdioma.DataValueField = "id"
        ddlIdioma.DataTextField = "DESCRIPCION"
        ddlIdioma.DataBind()
        ddlIdioma.Items.Insert(0, New ListItem("Seleccionar", 0))
        odbLector.Close()
        'Idioma 2

        odbComando = New OleDbCommand(sQry, odbConexion)
        odbLector = odbComando.ExecuteReader()

        ddlIdioma2.DataSource = odbLector
        ddlIdioma2.DataValueField = "id"
        ddlIdioma2.DataTextField = "DESCRIPCION"
        ddlIdioma2.DataBind()
        ddlIdioma2.Items.Insert(0, New ListItem("Seleccionar", 0))
    End Sub

#End Region
#Region "IDP"
    Public Sub ObtInformacionIDP()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_idp_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()

                odbLector.Close()
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'Obtiene Funciones
    Public Sub ObtInformacionID2P()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_idp_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()

                odbLector.Close()
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
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
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                lblEmpresa.Text = "<strong>Compañía: </strong>" & odbLector(0).ToString
                lblNivel.Text = "<br/><strong>Nivel: </strong>" & odbLector(1).ToString
                txtAreaAdscribe.Text = odbLector(2).ToString
                txtObjetivoPuesto.Text = odbLector(3).ToString
                txtPuestoReporto.Text = odbLector(4).ToString
                txtNumPuestoR.Text = odbLector(5).ToString
                txtDescripcionPuesto.Text = odbLector(6).ToString
                ' Facultades
                Dim Facultades() As String = Split(odbLector(7).ToString, ",")
                'ciclo de servicios para selecionar informacion
                'Selected en False
                For i = 0 To ddlFacultadesAutorizacion.Items.Count - 1
                    ddlFacultadesAutorizacion.Items(i).Selected = False
                Next

                For i = 0 To Facultades.Count - 1
                    For iFac = 0 To ddlFacultadesAutorizacion.Items.Count - 1
                        If ddlFacultadesAutorizacion.Items(iFac).Value = Facultades(i) Then
                            ddlFacultadesAutorizacion.Items(iFac).Selected = True
                        End If
                    Next
                Next


                'txtRelaInternasQuien.Text = odbLector(8).ToString.Replace("/", vbCrLf)
                'txtRelaInternasPara.Text = odbLector(9).ToString.Replace("/", vbCrLf)
                'txtRelaExternasQuien.Text = odbLector(10).ToString.Replace("/", vbCrLf)
                'txtRelaExternasPara.Text = odbLector(11).ToString.Replace("/", vbCrLf)
                'Escolaridad 
                Dim Escolaridad() As String = Split(odbLector(12).ToString, ",")
                'ciclo de servicios para selecionar informacion
                'Selected en False
                For i = 0 To ddlEscolaridad.Items.Count - 1
                    ddlEscolaridad.Items(i).Selected = False
                Next

                For i = 0 To Escolaridad.Count - 1
                    For iEsc = 0 To ddlEscolaridad.Items.Count - 1
                        If ddlEscolaridad.Items(iEsc).Value = Escolaridad(i) Then
                            ddlEscolaridad.Items(iEsc).Selected = True
                        End If
                    Next

                Next
                'Carrera
                Dim Carrera() As String = Split(odbLector(13).ToString, ",")
                'ciclo de servicios para selecionar informacion
                'Selected en False365
                For i = 0 To ddlCarreras.Items.Count - 1
                    ddlCarreras.Items(i).Selected = False
                Next

                For i = 0 To Carrera.Count - 1
                    For iCar = 0 To ddlCarreras.Items.Count - 1
                        If ddlCarreras.Items(iCar).Value = Carrera(i) Then
                            ddlCarreras.Items(iCar).Selected = True
                        End If
                    Next
                Next
                ddlIdioma.SelectedValue = IIf(odbLector(14).ToString = "", "0", odbLector(14).ToString)
                ddlidioma_dominio_1.SelectedValue = IIf(odbLector(15).ToString = "", "0", odbLector(15).ToString)
                ddlIdioma2.SelectedValue = IIf(odbLector(16).ToString = "", "0", odbLector(16).ToString)
                ddlidioma_dominio_2.SelectedValue = IIf(odbLector(17).ToString = "", "0", odbLector(17).ToString)
                ddlWord.SelectedValue = odbLector(18).ToString
                ddlExcel.SelectedValue = odbLector(19).ToString
                ddlPoweP.SelectedValue = odbLector(20).ToString
                ddlOutlook.SelectedValue = odbLector(21).ToString
                ddlAccess.SelectedValue = odbLector(22).ToString
                ddlProject.SelectedValue = odbLector(23).ToString
                txtAreaExperiencia.Text = odbLector(24).ToString
                ddlTiempo.SelectedValue = IIf(odbLector(25).ToString = "", "0", odbLector(25).ToString)
                txtSgi.Text = odbLector(26).ToString.Replace("/", vbCrLf)
                txtRequisito.Text = odbLector(27).ToString.Replace("/", vbCrLf)
                lblFechas.Text = "<br/><strong>Fecha de elaboración: </strong>" & odbLector(28).ToString &
                    "<br/><strong>Fecha de actualización: </strong>" & odbLector(29).ToString
                odbLector.Close()
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Private Sub ddlTipoPuesto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoPuesto.SelectedIndexChanged
        Call ObtInformacionIDP()
        Call obtGridFunciones()
        Call obtGridFormacion()
        Call HabilidadesCompetencia()
        Call ObtInformacionPuestos()
        Call obtRelacionesInternas()
        Call obtRelacionesExternas()
        Call Comportamientos()
    End Sub
#End Region
#Region "Grid Funciones"
    'obtiene el catalogo 
    Public Sub obtGridFunciones()
        Dim dsDatos As New DataSet
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_responsabilidades_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsDatos, "Tabla")

            grdFunciones.DataSource = dsDatos.Tables(0).DefaultView
            grdFunciones.DataBind()
            odbConexion.Close()

            dsDatos.Dispose()
            Dim strIdCorrelativo As String = ""
            For iFil As Integer = 0 To grdFunciones.Rows.Count - 1
                Dim btnEditar As LinkButton = grdFunciones.Rows(iFil).Controls(3).Controls(0)

                strIdCorrelativo = DirectCast(grdFunciones.Rows(iFil).FindControl("lblCorrelativo"), Label).Text

                If strIdCorrelativo = "11" Or strIdCorrelativo = "12" Or strIdCorrelativo = "13" Then
                    btnEditar.Visible = False
                    DirectCast(grdFunciones.Rows(iFil).FindControl("lblCorrelativo"), Label).Text = IIf(strIdCorrelativo = "11", "1", IIf(strIdCorrelativo = "12", "2", IIf(strIdCorrelativo = "13", "3", "")))
                    DirectCast(grdFunciones.Rows(iFil).FindControl("lblDescripcion"), Label).ToolTip = "2.2 Responsabilidades relacionadas con el Sistema de Gestión"
                    If strIdCorrelativo = "11" Then
                        DirectCast(grdFunciones.Rows(iFil).FindControl("lblCorrelativo"), Label).Text = "<br />" &
                        DirectCast(grdFunciones.Rows(iFil).FindControl("lblCorrelativo"), Label).Text

                        DirectCast(grdFunciones.Rows(iFil).FindControl("lblDescripcion"), Label).Text = "<label style='font-size: 9pt; background-color: #CCFF99;'>2.2 Responsabilidades relacionadas con el Sistema de Gestión</label> <br />" &
                    DirectCast(grdFunciones.Rows(iFil).FindControl("lblDescripcion"), Label).Text

                    End If
                End If


                If iFil Mod 2 = 0 Then
                    grdFunciones.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdFunciones_PreRender(sender As Object, e As EventArgs) Handles grdFunciones.PreRender
        Try
            Dim header = DirectCast(grdFunciones.Controls(0).Controls(0), GridViewRow)

            header.Cells(1).Visible = False
            header.Cells(2).ColumnSpan = 2
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdFunciones_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdFunciones.RowCancelingEdit
        grdFunciones.EditIndex = -1
        Call obtGridFunciones()

    End Sub

    'habilita el modo edicion
    Private Sub grdFunciones_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdFunciones.RowEditing
        grdFunciones.EditIndex = e.NewEditIndex
        Call obtGridFunciones()

    End Sub
    'actualiza la descripcion
    Private Sub grdFunciones_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdFunciones.RowUpdating
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Try
            odbConexion.Open()
            strId = DirectCast(grdFunciones.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strDescripcion = (DirectCast(grdFunciones.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_responsabilidades_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdFuncion", strId)
            odbComando.Parameters.AddWithValue("@PDescripcion", strDescripcion)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()
            grdFunciones.EditIndex = -1

            Call obtGridFunciones()
            odbConexion.Close()


        Catch ex As Exception
            lblError.Text = ex.Message
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
            ' strNombreUsuario = "isemac"
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
        If hdRol.Value <> 2 Then
            lnkEnviar.Visible = False
            btnCargarExcel.Visible = False
        End If

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
#Region "Grid Formacion"
    'obtiene el catalogo 
    Public Sub obtGridFormacion()
        Dim dsDatos As New DataSet
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_formacion_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsDatos, "Tabla")

            grdFormacion.DataSource = dsDatos.Tables(0).DefaultView
            grdFormacion.DataBind()
            odbConexion.Close()

            dsDatos.Dispose()
            For iFil As Integer = 0 To grdFormacion.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdFormacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Private Sub ggrdFormacion_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdFormacion.RowCancelingEdit
        grdFormacion.EditIndex = -1
        Call obtGridFormacion()

    End Sub

    'habilita el modo edicion
    Private Sub grdFormacion_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdFormacion.RowEditing
        grdFormacion.EditIndex = e.NewEditIndex
        Call obtGridFormacion()

    End Sub
    'actualiza la descripcion
    Private Sub grdFormacion_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdFormacion.RowUpdating
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strDominio As String = ""
        Try
            odbConexion.Open()
            strId = DirectCast(grdFormacion.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strDescripcion = (DirectCast(grdFormacion.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text)
            strDominio = (DirectCast(grdFormacion.Rows(e.RowIndex).FindControl("ddlDominio"), DropDownList).Text)

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_formacion_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'valida que debe de tenr domino si exuste descripcion
            If strDescripcion <> "" And strDominio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Dominio.');</script>", False)
                Exit Sub
            End If
            If strDescripcion = "" And strDominio <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de Capturar el conocimiento técnicos.');</script>", False)
                Exit Sub
            End If

            'parametros
            odbComando.Parameters.AddWithValue("@PIdFuncion", strId)
            odbComando.Parameters.AddWithValue("@PDescripcion", strDescripcion)
            odbComando.Parameters.AddWithValue("@PDominio", strDominio)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()
            grdFormacion.EditIndex = -1

            Call obtGridFormacion()
            odbConexion.Close()
            GC.Collect()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida que el grid tiene informacion registrado
    Public Function valFormacion()
        Dim blnResultado As Boolean = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "do_formacion_val_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure
        'parametros
        odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0).ToString = "", False, True)
            odbLector.Close()
        End If
        Return blnResultado
    End Function
#End Region
#Region "Grid Habilidades"
    'obtiene el catalogo 
    Public Sub HabilidadesCompetencia()
        Dim dsDatos As New DataSet
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_habilidades_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)

            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsDatos, "Tabla")

            grdHabilidadesCompetencia.DataSource = dsDatos.Tables(0).DefaultView
            grdHabilidadesCompetencia.DataBind()
            odbConexion.Close()
            For i = 0 To grdHabilidadesCompetencia.Rows.Count - 1
                Dim iId As String

                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdHabilidadesCompetencia.Rows(i).Controls(5).Controls(0)

                iId = DirectCast(grdHabilidadesCompetencia.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    Dim ddlHabilidades1 As DropDownList = grdHabilidadesCompetencia.Rows(i).FindControl("ddlHabilidades1")
                    Dim ddlDominio1 As DropDownList = grdHabilidadesCompetencia.Rows(i).FindControl("ddlDominio1")
                    Dim ddlHabilidades2 As DropDownList = grdHabilidadesCompetencia.Rows(i).FindControl("ddlHabilidades2")
                    Dim ddlDominio2 As DropDownList = grdHabilidadesCompetencia.Rows(i).FindControl("ddlDominio2")

                    Call obtddlHabilidades(ddlHabilidades1)
                    Call obtddlHabilidades(ddlHabilidades2)

                    For iContador As Integer = 0 To dsDatos.Tables(0).Rows.Count - 1
                        If dsDatos.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlHabilidades1.SelectedValue = dsDatos.Tables(0).Rows(iContador)(1).ToString
                            ddlDominio1.SelectedValue = dsDatos.Tables(0).Rows(iContador)(3).ToString
                            ddlHabilidades2.SelectedValue = dsDatos.Tables(0).Rows(iContador)(4).ToString
                            ddlDominio2.SelectedValue = dsDatos.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                End If
            Next

            For iFil As Integer = 0 To grdHabilidadesCompetencia.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdHabilidadesCompetencia.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            dsDatos.Dispose()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Sub obtddlHabilidades(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT id,DESCRIPCION  FROM DO_HABILIDADES_COMPETENCIAS_CT  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "DESCRIPCION"
        ddl.DataValueField = "id"

        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem("Seleccionar", 0))
        odbConexion.Close()
    End Sub

    Private Sub grdHabilidadesCompetencia_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHabilidadesCompetencia.RowCancelingEdit
        grdHabilidadesCompetencia.EditIndex = -1
        Call HabilidadesCompetencia()

    End Sub

    'habilita el modo edicion
    Private Sub HabilidadesCompetencia_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdHabilidadesCompetencia.RowEditing
        grdHabilidadesCompetencia.EditIndex = e.NewEditIndex
        Call HabilidadesCompetencia()
    End Sub
    'actualiza la descripcion
    Private Sub grdHabilidadesCompetencia_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdHabilidadesCompetencia.RowUpdating
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbAdaptador As New OleDbDataAdapter
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strDominio As String = ""
        Dim strDescripcion2 As String = ""
        Dim strDominio2 As String = ""
        Dim strHabilidad1 As String = ""
        Dim strHabilidad2 As String = ""
        Try
            odbConexion.Open()
            strId = DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strDescripcion = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlHabilidades1"), DropDownList).Text)
            strDominio = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlDominio1"), DropDownList).Text)
            strDescripcion2 = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlHabilidades2"), DropDownList).Text)
            strDominio2 = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlDominio2"), DropDownList).Text)

            strHabilidad1 = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlHabilidades1"), DropDownList).SelectedItem.Text)
            strHabilidad2 = (DirectCast(grdHabilidadesCompetencia.Rows(e.RowIndex).FindControl("ddlHabilidades2"), DropDownList).SelectedItem.Text)
            'valida que debe de tenr domino si exuste descripcion
            If strDescripcion <> "0" And strDominio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Dominio.');</script>", False)
                Exit Sub
            End If
            If strDescripcion = "0" And strDominio <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de Capturar la habilidad.');</script>", False)
                Exit Sub
            End If

            If strDescripcion2 <> "0" And strDominio2 = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Dominio 2.');</script>", False)
                Exit Sub
            End If
            If strDescripcion2 = "0" And strDominio2 <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de Capturar la habilidad 2.');</script>", False)
                Exit Sub
            End If
            'No habilidades repetidas
            If strDescripcion <> "0" And strDescripcion2 <> "0" Then
                If strDescripcion = strDescripcion2 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No puede tener repetida la misma habilidad.');</script>", False)
                    Exit Sub
                End If
            End If


            Dim strHRepetida As String = ""
            'valida que no exista ninguna habilidad repetida
            For iFil As Integer = 0 To grdHabilidadesCompetencia.Rows.Count - 1
                Dim iId As String
                Dim strHa As String = grdHabilidadesCompetencia.Rows(iFil).Cells(1).ToString
                Dim btnEditar As LinkButton = grdHabilidadesCompetencia.Rows(iFil).Controls(5).Controls(0)
                iId = DirectCast(grdHabilidadesCompetencia.Rows(iFil).Cells(0).FindControl("lblId"), Label).Text
                If btnEditar.Text = "Editar" Then
                    'No cuente el que se esta editando
                    If strId <> iId Then
                        Dim lblHabilidad1 As New Label
                        lblHabilidad1 = grdHabilidadesCompetencia.Rows(iFil).FindControl("lblHabilidad1")
                        Dim lblHabilidad2 As New Label
                        lblHabilidad2 = grdHabilidadesCompetencia.Rows(iFil).FindControl("lblHabilidad2")
                        'valida que la habilidad no este repetida
                        If strHabilidad1 = lblHabilidad1.Text Or strHabilidad1 = lblHabilidad2.Text Then strHRepetida = strHabilidad1
                        If strHRepetida <> "" Then Exit For
                        If strHabilidad2 = lblHabilidad1.Text Or strHabilidad2 = lblHabilidad2.Text Then strHRepetida = strHabilidad2
                        If strHRepetida <> "" Then Exit For
                    End If
                End If
            Next
            If strHRepetida <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('[" & strHRepetida & "] ya existe.');</script>", False)
                Exit Sub
            End If

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_habilidades_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdFuncion", strId)
            odbComando.Parameters.AddWithValue("@PDescripcion", strDescripcion)
            odbComando.Parameters.AddWithValue("@PDominio", strDominio)
            odbComando.Parameters.AddWithValue("@PDescripcion2", strDescripcion2)
            odbComando.Parameters.AddWithValue("@PDominio2", strDominio2)
            odbComando.Parameters.AddWithValue("@PUsuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()
            grdHabilidadesCompetencia.EditIndex = -1

            Call HabilidadesCompetencia()
            odbConexion.Close()
            GC.Collect()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida que el grid tiene informacion registrado
    Public Function valHabilidades()
        Dim blnResultado As Boolean = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "do_habilidades_val_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure
        'parametros
        odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0).ToString = "", False, True)
            odbLector.Close()
        End If
        Return blnResultado
    End Function
#End Region
#Region "Comportamientos"
    Public Sub Comportamientos()
        If ddlTipoPuesto.SelectedValue = "0" Or ddlTipoPuesto.SelectedValue = "" Then
            divInformacion.Visible = False
            btnCargarExcel.Visible = False
            lblEmpresa.Text = ""
            lblNivel.Text = ""
            lblFechas.Text = ""
        Else
            divInformacion.Visible = True
            btnCargarExcel.Visible = True
            divInforGuardada.Visible = False
        End If

        'colorea las celdas del grid
        For iFil As Integer = 0 To grdFormacion.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdFormacion.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'colorea las celdas del grid
        For iFil As Integer = 0 To grdFunciones.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdFunciones.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'colorea las celdas del grid
        For iFil As Integer = 0 To grdHabilidadesCompetencia.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdHabilidadesCompetencia.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next


        For iFil As Integer = 0 To grdRelacionesExternas.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdRelacionesExternas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next


        For iFil As Integer = 0 To grdRelacionesInternas.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdRelacionesInternas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Validacion de rol de usuario para el rol 3 Usuario
        If hdRol.Value = "3" Then
            btnCargarExcel.Visible = False
            grdFunciones.Columns(3).Visible = False
            grdRelacionesInternas.Columns(5).Visible = False
            grdRelacionesExternas.Columns(5).Visible = False
            grdFormacion.Columns(3).Visible = False
            grdHabilidadesCompetencia.Columns(5).Visible = False
            lnkGuardar.Visible = False
            ddlFacultadesAutorizacion.Enabled = True
            txtObjetivoPuesto.ReadOnly = True
            txtAreaAdscribe.ReadOnly = True
            txtSgi.ReadOnly = True
            txtRequisito.ReadOnly = True
            txtAreaExperiencia.ReadOnly = True
            ddlFacultadesAutorizacion.Attributes.Add("disabled", "disabled")
            ddlCarreras.Attributes.Add("disabled", "disabled")
            ddlAccess.Attributes.Add("disabled", "disabled")
            ddlCarreras.Attributes.Add("disabled", "disabled")
            ddlEscolaridad.Attributes.Add("disabled", "disabled")
            ddlExcel.Attributes.Add("disabled", "disabled")
            ddlIdioma.Attributes.Add("disabled", "disabled")
            ddlidioma_dominio_1.Attributes.Add("disabled", "disabled")
            ddlidioma_dominio_2.Attributes.Add("disabled", "disabled")
            ddlIdioma2.Attributes.Add("disabled", "disabled")
            ddlOutlook.Attributes.Add("disabled", "disabled")
            ddlPoweP.Attributes.Add("disabled", "disabled")
            ddlProject.Attributes.Add("disabled", "disabled")
            ddlTiempo.Attributes.Add("disabled", "disabled")
            ddlWord.Attributes.Add("disabled", "disabled")
        End If
    End Sub
#End Region
#Region "Guardar"

    Public Sub GuardaInformacion()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_info_puesto_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            Dim strFacultades As String = ""
            Dim strCarrera As String = ""
            Dim strEscolaridad As String = ""
            For Each item As ListItem In ddlFacultadesAutorizacion.Items
                If item.Selected Then
                    strFacultades += item.Value + ","
                End If
            Next
            If Len(strFacultades) > 0 Then strFacultades = strFacultades.Substring(0, strFacultades.LastIndexOf(","))
            'Carrera
            For Each item As ListItem In ddlCarreras.Items
                If item.Selected Then
                    strCarrera += item.Value + ","
                End If
            Next
            If Len(strCarrera) > 0 Then strCarrera = strCarrera.Substring(0, strCarrera.LastIndexOf(","))
            'Escolaridad
            For Each item As ListItem In ddlEscolaridad.Items
                If item.Selected Then
                    strEscolaridad += item.Value + ","
                End If
            Next
            If Len(strEscolaridad) > 0 Then strEscolaridad = strEscolaridad.Substring(0, strEscolaridad.LastIndexOf(","))
            'Validaciones de Relaciones
            'Valida  que no queden ningun vacio
            Dim strId As String = ""
            For i As Integer = 0 To grdRelacionesInternas.Rows.Count - 1
                strId = DirectCast(grdRelacionesInternas.Rows(i).FindControl("lblId"), Label).Text
                If strId <> "" Then
                    Dim strCorrelativo As String = grdRelacionesInternas.Rows(i).Cells(1).Text
                    Dim txtConQuien As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
                    Dim txtPara As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

                    If txtConQuien.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Interna Con quien " & strCorrelativo & ".');</script>", False)
                        Exit Sub
                    End If

                    If txtPara.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Interna Para " & strCorrelativo & ".');</script>", False)
                        Exit Sub
                    End If
                End If

            Next


            'Valida  que no queden ningun vacio
            For i As Integer = 0 To grdRelacionesExternas.Rows.Count - 1
                strId = DirectCast(grdRelacionesExternas.Rows(i).FindControl("lblId"), Label).Text
                If strId <> "" Then
                    Dim strCorrelativo As String = grdRelacionesExternas.Rows(i).Cells(1).Text
                    Dim txtConQuien As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
                    Dim txtPara As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

                    If txtConQuien.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Externa Con quien " & strCorrelativo & ".');</script>", False)
                        Exit Sub
                    End If

                    If txtPara.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Externa Para " & strCorrelativo & ".');</script>", False)
                        Exit Sub
                    End If
                End If
            Next
            'VALIDA FORMACION
            If valFormacion() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe registrar la Formación .');</script>", False)
                Exit Sub
            End If
            'VALIDA HABILIDADES
            If valHabilidades() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe registrar Habilidades y Competencias.');</script>", False)
                Exit Sub
            End If
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pobjetivo_puesto", txtObjetivoPuesto.Text)
            odbComando.Parameters.AddWithValue("@Pfacultades_autorizacion", strFacultades)
            odbComando.Parameters.AddWithValue("@Prelacion_interna_quien", "")
            odbComando.Parameters.AddWithValue("@Prelacion_interna_para", "")
            odbComando.Parameters.AddWithValue("@Prelacion_externa_quien", "")
            odbComando.Parameters.AddWithValue("@Prelacion_externa_para", "")
            odbComando.Parameters.AddWithValue("@Pgrado_escolaridad", strEscolaridad)
            odbComando.Parameters.AddWithValue("@Pcarrera_especializacion", strCarrera)
            odbComando.Parameters.AddWithValue("@Pidioma_1", IIf(ddlIdioma.SelectedValue = "", "0", ddlIdioma.SelectedValue))
            odbComando.Parameters.AddWithValue("@Pdominio_1", ddlidioma_dominio_1.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pidioma_2", IIf(ddlIdioma2.SelectedValue = "", "0", ddlIdioma2.SelectedValue))
            odbComando.Parameters.AddWithValue("@Pdominio_2", ddlidioma_dominio_2.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pword", ddlWord.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pexcel", ddlExcel.SelectedValue)
            odbComando.Parameters.AddWithValue("@Ppower_point", ddlPoweP.SelectedValue)
            odbComando.Parameters.AddWithValue("@Poutlook", ddlOutlook.SelectedValue)
            odbComando.Parameters.AddWithValue("@Paccess", ddlAccess.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pproject", ddlProject.SelectedValue)
            odbComando.Parameters.AddWithValue("@Pareas_experiencia", txtAreaExperiencia.Text)
            odbComando.Parameters.AddWithValue("@Ptiempo", ddlTiempo.SelectedValue)
            odbComando.Parameters.AddWithValue("@Psistema_gestion_integrado", txtSgi.Text)
            odbComando.Parameters.AddWithValue("@Prequisitos_codiciones", txtRequisito.Text)
            odbComando.Parameters.AddWithValue("@Pestatus", "Guardado")
            odbComando.Parameters.AddWithValue("@Pusuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()

            'Actualiza Relaciones Externas
            Call UpdRelacionesExternas(odbConexion)

            'Actualiza Relaciones Internas
            Call UpdRelacionesInternas(odbConexion)

            odbConexion.Close()
            Call obtGridFunciones()
            Call obtGridFormacion()
            Call HabilidadesCompetencia()
            Call ObtInformacionPuestos()
            Call obtRelacionesExternas()
            Call obtRelacionesInternas()
            Call Comportamientos()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Información Guardada correctamente.');</script>", False)
            'divInforGuardada.Visible = True
            'lblMensaje.Text = "Guardada correctamente."
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Protected Sub lnkGuardar_Click(sender As Object, e As EventArgs)
        Call GuardaInformacion()
    End Sub
#End Region
#Region "Excel"
    'METODO QUE GUARDA EL ARCHIVO EN EL SERVIDOR
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
                fileExtension = System.IO.Path.
                    GetExtension(fuExcel.FileName).ToLower()
                Dim allowedExtensions As String() =
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
                            Dim strCarpeta As String = path & "\Reclutamiento\"
                            strNombreArchivo = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & fuExcel.FileName


                            'crea directorio
                            If Not (Directory.Exists(strCarpeta)) Then
                                Directory.CreateDirectory(strCarpeta)
                            End If

                            fuExcel.PostedFile.SaveAs(strCarpeta & strNombreArchivo)

                            Call leerExcel(strCarpeta & strNombreArchivo, fileExtension, sIdFile, strNombreArchivo)
                            Call ObtInformacionIDP()
                            Call obtGridFunciones()
                            Call obtGridFormacion()
                            Call HabilidadesCompetencia()
                            Call ObtInformacionPuestos()
                            Call obtRelacionesInternas()
                            Call obtRelacionesExternas()
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
        lblmessage.Text = ""

        Try
            ' Leer el archivo Excel usando la función ImportExcel
            Dim dt As DataTable = ImportExcel(strRuta)
            ' Verificar si el DataTable tiene al menos una fila y los encabezados esperados
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Columns.Contains("Objetivo") Then
                ' Conectar a la base de datos
                Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
                Using odbConexion As New OleDbConnection(sConnString)
                    odbConexion.Open()
                    Using odbComando As New OleDbCommand
                        odbComando.CommandText = "do_info_puesto_excel_sp"
                        odbComando.Connection = odbConexion
                        odbComando.CommandType = CommandType.StoredProcedure

                        ' Obtener los datos del DataTable según los nombres de las columnas
                        Dim strObjetivo As String = dt.Rows(0)("Objetivo").ToString().Trim().Replace(vbNullChar & " ", " ").Replace(Chr(0), " ")
                        'Dim strNivel As String = dt.Rows(0)("Nivel").ToString()
                        'Dim strCompania As String = dt.Rows(0)("Compania").ToString()
                        'Dim strPuesto_Reporto As String = dt.Rows(0)("Puesto_Reporto").ToString()
                        'Dim strArea As String = dt.Rows(0)("Area").ToString()
                        Dim strGrado_Escolaridad As String = dt.Rows(0)("Grado_Escolaridad").ToString()
                        Dim strCarrera_Especializacion As String = dt.Rows(0)("Carrera_Especializacion").ToString()
                        Dim strResponsabilidad1 As String = dt.Rows(0)("Responsabilidad1").ToString()
                        Dim strResponsabilidad2 As String = dt.Rows(0)("Responsabilidad2").ToString()
                        Dim strResponsabilidad3 As String = dt.Rows(0)("Responsabilidad3").ToString()
                        Dim strResponsabilidad4 As String = dt.Rows(0)("Responsabilidad4").ToString()
                        Dim strResponsabilidad5 As String = dt.Rows(0)("Responsabilidad5").ToString()
                        Dim strResponsabilidad6 As String = dt.Rows(0)("Responsabilidad6").ToString()
                        Dim strResponsabilidad7 As String = dt.Rows(0)("Responsabilidad7").ToString()
                        Dim strResponsabilidad8 As String = dt.Rows(0)("Responsabilidad8").ToString()
                        Dim strResponsabilidad9 As String = dt.Rows(0)("Responsabilidad9").ToString()
                        Dim strResponsabilidad10 As String = dt.Rows(0)("Responsabilidad10").ToString()
                        Dim strResponsabilidad11 As String = dt.Rows(0)("Responsabilidad11").ToString()
                        Dim strResponsabilidad12 As String = dt.Rows(0)("Responsabilidad12").ToString()

                        Dim strHabilidad1 As String = dt.Rows(0)("Habilidad1").ToString()
                        Dim strHabilidad2 As String = dt.Rows(0)("Habilidad2").ToString()
                        Dim strHabilidad3 As String = dt.Rows(0)("Habilidad3").ToString()
                        Dim strHabilidad4 As String = dt.Rows(0)("Habilidad4").ToString()
                        Dim strHabilidad5 As String = dt.Rows(0)("Habilidad5").ToString()
                        Dim strHabilidad6 As String = dt.Rows(0)("Habilidad6").ToString()
                        Dim strHabilidad7 As String = dt.Rows(0)("Habilidad7").ToString()
                        Dim strHabilidad8 As String = dt.Rows(0)("Habilidad8").ToString()

                        Dim strDomino_Habilidad1 As String = dt.Rows(0)("Domino_Habilidad1").ToString()
                        Dim strDomino_Habilidad2 As String = dt.Rows(0)("Domino_Habilidad2").ToString()
                        Dim strDomino_Habilidad3 As String = dt.Rows(0)("Domino_Habilidad3").ToString()
                        Dim strDomino_Habilidad4 As String = dt.Rows(0)("Domino_Habilidad4").ToString()
                        Dim strDomino_Habilidad5 As String = dt.Rows(0)("Domino_Habilidad5").ToString()
                        Dim strDomino_Habilidad6 As String = dt.Rows(0)("Domino_Habilidad6").ToString()
                        Dim strDomino_Habilidad7 As String = dt.Rows(0)("Domino_Habilidad7").ToString()
                        Dim strDomino_Habilidad8 As String = dt.Rows(0)("Domino_Habilidad8").ToString()


                        Dim strRelacionesIntQ As String = dt.Rows(0)("RelacionesIntQ").ToString()
                        Dim strRelacionesIntP As String = dt.Rows(0)("RelacionesIntP").ToString()
                        Dim strRelacionesExtQ As String = dt.Rows(0)("RelacionesExtQ").ToString()
                        Dim strRelacionesExtP As String = dt.Rows(0)("RelacionesExtP").ToString()
                        Dim strFormacion1 As String = dt.Rows(0)("Formacion1").ToString()
                        Dim strFormacionDom1 As String = dt.Rows(0)("FormacionDom1").ToString()
                        Dim strFormacion2 As String = dt.Rows(0)("Formacion2").ToString()
                        Dim strFormacionDom2 As String = dt.Rows(0)("FormacionDom2").ToString()
                        Dim strFormacion3 As String = dt.Rows(0)("Formacion3").ToString()
                        Dim strFormacionDom3 As String = dt.Rows(0)("FormacionDom3").ToString()
                        Dim strFormacion4 As String = dt.Rows(0)("Formacion4").ToString()
                        Dim strFormacionDom4 As String = dt.Rows(0)("FormacionDom4").ToString()
                        Dim strFormacion5 As String = dt.Rows(0)("Formacion5").ToString()
                        Dim strFormacionDom5 As String = dt.Rows(0)("FormacionDom5").ToString()
                        Dim strFormacion6 As String = dt.Rows(0)("Formacion6").ToString()
                        Dim strFormacionDom6 As String = dt.Rows(0)("FormacionDom6").ToString()
                        Dim strIdioma As String = dt.Rows(0)("Idioma").ToString()
                        Dim strIdiomaDom As String = dt.Rows(0)("IdiomaDom").ToString().ToUpper()
                        Dim strIdioma2 As String = dt.Rows(0)("Idioma2").ToString()
                        Dim strIdiomaDo2 As String = dt.Rows(0)("IdiomaDo2").ToString().ToUpper()
                        Dim strAreaExperiencia As String = dt.Rows(0)("AreaExperiencia").ToString()
                        Dim strAreaExperienciaAños As String = dt.Rows(0)("AreaExperienciaAños").ToString()
                        Dim strRequisito As String = dt.Rows(0)("Requisito").ToString()
                        Dim strWord As String = dt.Rows(0)("Word").ToString().ToUpper()
                        Dim strExcel As String = dt.Rows(0)("Excel").ToString().ToUpper()
                        Dim strPowerP As String = dt.Rows(0)("PowerP").ToString().ToUpper()
                        Dim strOutlook As String = dt.Rows(0)("Outlook").ToString().ToUpper()
                        Dim strAccess As String = dt.Rows(0)("Access").ToString().ToUpper()
                        Dim strProject As String = dt.Rows(0)("Project").ToString().ToUpper()

                        ' Mapeo de valores si es necesario (por ejemplo, para convertir niveles de habilidad)
                        strFormacionDom3 = IIf(strFormacionDom3.Contains("B"), "Básico", IIf(strFormacionDom3.Contains("M"), "Medio", IIf(strFormacionDom3.Contains("A"), "Avanzado", "")))
                        strFormacionDom4 = IIf(strFormacionDom4.Contains("B"), "Básico", IIf(strFormacionDom4.Contains("M"), "Medio", IIf(strFormacionDom4.Contains("A"), "Avanzado", "")))
                        strFormacionDom5 = IIf(strFormacionDom5.Contains("B"), "Básico", IIf(strFormacionDom5.Contains("M"), "Medio", IIf(strFormacionDom5.Contains("A"), "Avanzado", "")))
                        strFormacionDom6 = IIf(strFormacionDom6.Contains("B"), "Básico", IIf(strFormacionDom6.Contains("M"), "Medio", IIf(strFormacionDom6.Contains("A"), "Avanzado", "")))
                        strIdiomaDom = IIf(strIdiomaDom.Contains("B"), "Básico", IIf(strIdiomaDom.Contains("M"), "Medio", IIf(strIdiomaDom.Contains("A"), "Avanzado", "")))
                        strIdiomaDo2 = IIf(strIdiomaDo2.Contains("B"), "Básico", IIf(strIdiomaDo2.Contains("M"), "Medio", IIf(strIdiomaDo2.Contains("A"), "Avanzado", "")))

                        strWord = IIf(strWord.Contains("B"), "Básico", IIf(strWord.Contains("M"), "Medio", IIf(strWord.Contains("A"), "Avanzado", "")))
                        strExcel = IIf(strExcel.Contains("B"), "Básico", IIf(strExcel.Contains("M"), "Medio", IIf(strExcel.Contains("A"), "Avanzado", "")))
                        strPowerP = IIf(strPowerP.Contains("B"), "Básico", IIf(strPowerP.Contains("M"), "Medio", IIf(strPowerP.Contains("A"), "Avanzado", "")))
                        strOutlook = IIf(strOutlook.Contains("B"), "Básico", IIf(strOutlook.Contains("M"), "Medio", IIf(strOutlook.Contains("A"), "Avanzado", "")))
                        strAccess = IIf(strAccess.Contains("B"), "Básico", IIf(strAccess.Contains("M"), "Medio", IIf(strAccess.Contains("A"), "Avanzado", "")))
                        strProject = IIf(strProject.Contains("B"), "Básico", IIf(strProject.Contains("M"), "Medio", IIf(strProject.Contains("A"), "Avanzado", "")))


                        ' Agregar parámetros al comando
                        odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
                        'odbComando.Parameters.AddWithValue("@Nivel", strNivel)
                        'odbComando.Parameters.AddWithValue("@Empresa", strCompania)
                        odbComando.Parameters.AddWithValue("@Objetivo", strObjetivo)
                        'odbComando.Parameters.AddWithValue("@Area", strArea)
                        'odbComando.Parameters.AddWithValue("@Puesto_Reporto", strPuesto_Reporto)
                        odbComando.Parameters.AddWithValue("@Grado_Escolaridad", strGrado_Escolaridad)
                        odbComando.Parameters.AddWithValue("@Carrera_Especializacion", strCarrera_Especializacion)
                        odbComando.Parameters.AddWithValue("@Responsabilidad1", strResponsabilidad1)
                        odbComando.Parameters.AddWithValue("@Responsabilidad2", strResponsabilidad2)
                        odbComando.Parameters.AddWithValue("@Responsabilidad3", strResponsabilidad3)
                        odbComando.Parameters.AddWithValue("@Responsabilidad4", strResponsabilidad4)
                        odbComando.Parameters.AddWithValue("@Responsabilidad5", strResponsabilidad5)
                        odbComando.Parameters.AddWithValue("@Responsabilidad6", strResponsabilidad6)
                        odbComando.Parameters.AddWithValue("@Responsabilidad7", strResponsabilidad7)
                        odbComando.Parameters.AddWithValue("@Responsabilidad8", strResponsabilidad8)
                        odbComando.Parameters.AddWithValue("@Responsabilidad9", strResponsabilidad9)
                        odbComando.Parameters.AddWithValue("@Responsabilidad10", strResponsabilidad10)
                        odbComando.Parameters.AddWithValue("@Responsabilidad11", strResponsabilidad11)
                        odbComando.Parameters.AddWithValue("@Responsabilidad12", strResponsabilidad12)

                        odbComando.Parameters.AddWithValue("@Habilidad1", strHabilidad1)
                        odbComando.Parameters.AddWithValue("@Habilidad2", strHabilidad2)
                        odbComando.Parameters.AddWithValue("@Habilidad3", strHabilidad3)
                        odbComando.Parameters.AddWithValue("@Habilidad4", strHabilidad4)
                        odbComando.Parameters.AddWithValue("@Habilidad5", strHabilidad5)
                        odbComando.Parameters.AddWithValue("@Habilidad6", strHabilidad6)
                        odbComando.Parameters.AddWithValue("@Habilidad7", strHabilidad7)
                        odbComando.Parameters.AddWithValue("@Habilidad8", strHabilidad8)

                        odbComando.Parameters.AddWithValue("@Domino_Habilidad1", strDomino_Habilidad1)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad2", strDomino_Habilidad2)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad3", strDomino_Habilidad3)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad4", strDomino_Habilidad4)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad5", strDomino_Habilidad5)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad6", strDomino_Habilidad6)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad7", strDomino_Habilidad7)
                        odbComando.Parameters.AddWithValue("@Domino_Habilidad8", strDomino_Habilidad8)

                        odbComando.Parameters.AddWithValue("@RelacionesIntQ", strRelacionesIntQ)
                        odbComando.Parameters.AddWithValue("@RelacionesIntP", strRelacionesIntP)
                        odbComando.Parameters.AddWithValue("@RelacionesExtQ", strRelacionesExtQ)
                        odbComando.Parameters.AddWithValue("@RelacionesExtP", strRelacionesExtP)
                        odbComando.Parameters.AddWithValue("@Formacion1", strFormacion1)
                        odbComando.Parameters.AddWithValue("@FormacionDom1", strFormacionDom1)
                        odbComando.Parameters.AddWithValue("@Formacion2", strFormacion2)
                        odbComando.Parameters.AddWithValue("@FormacionDom2", strFormacionDom2)
                        odbComando.Parameters.AddWithValue("@Formacion3", strFormacion3)
                        odbComando.Parameters.AddWithValue("@FormacionDom3", strFormacionDom3)
                        odbComando.Parameters.AddWithValue("@Formacion4", strFormacion4)
                        odbComando.Parameters.AddWithValue("@FormacionDom4", strFormacionDom4)
                        odbComando.Parameters.AddWithValue("@Formacion5", strFormacion5)
                        odbComando.Parameters.AddWithValue("@FormacionDom5", strFormacionDom5)
                        odbComando.Parameters.AddWithValue("@Formacion6", strFormacion6)
                        odbComando.Parameters.AddWithValue("@FormacionDom6", strFormacionDom6)
                        odbComando.Parameters.AddWithValue("@Idioma", strIdioma)
                        odbComando.Parameters.AddWithValue("@IdiomaDom", strIdiomaDom)
                        odbComando.Parameters.AddWithValue("@Idioma2", strIdioma2)
                        odbComando.Parameters.AddWithValue("@IdiomaDo2", strIdiomaDo2)
                        odbComando.Parameters.AddWithValue("@AreaExperiencia", strAreaExperiencia)
                        odbComando.Parameters.AddWithValue("@AreaExperienciaAños", strAreaExperienciaAños)
                        odbComando.Parameters.AddWithValue("@Requisito", strRequisito)
                        odbComando.Parameters.AddWithValue("@Word", strWord)
                        odbComando.Parameters.AddWithValue("@Excel", strExcel)
                        odbComando.Parameters.AddWithValue("@PowerP", strPowerP)
                        odbComando.Parameters.AddWithValue("@Outlook", strOutlook)
                        odbComando.Parameters.AddWithValue("@Access", strAccess)
                        odbComando.Parameters.AddWithValue("@Project", strProject)



                        ' Ejecutar el comando
                        odbComando.ExecuteNonQuery()
                    End Using
                End Using

                ' Mostrar mensaje de éxito
                lblmessage.ForeColor = Drawing.Color.Green
                lblmessage.Text = "Puesto Registrado Exitosamente."
            Else
                ' Mostrar mensaje de error si no se encuentran los encabezados esperados
                lblmessage.ForeColor = Drawing.Color.Red
                lblmessage.Text = "El DataTable no contiene los encabezados esperados."
            End If
        Catch ex As Exception
            ' Manejo de excepciones
            lblmessage.ForeColor = Drawing.Color.Red
            lblmessage.Text = "Error: " & ex.Message
        End Try
    End Sub


    Private Function MapSkillLevel(skillLevel As String) As Integer
        Select Case skillLevel.ToUpper()
            Case "NULO"
                Return 1
            Case "BAJO"
                Return 2
            Case "BÁSICO"
                Return 3
            Case "INTERMEDIO"
                Return 4
            Case "AVANZADO"
                Return 5
            Case Else
                Return 0
        End Select
    End Function

    Private Sub btnCargaFormato_ServerClick(sender As Object, e As EventArgs) Handles btnCargaFormato.ServerClick
        Call cargaArchivo()
        Call obtenerUsuarioAD()
    End Sub

    'METODO ENCARGADO DE INSERTAR LA INFORMACION EN LA TABLA DE PASO

    Function obtieneObjetivo(ByVal FileName As String, sFileProc As String)
        Dim strResultado As String = ""

        'creamos una apliacacion deexcel
        Dim x = CreateObject("Excel.application")
        'nos conectamos con el archivo y lo abrimosen segundo plano

        With x
            .Workbooks.Open(FileName:=sFileProc)

            'leeemos el contenido de la celda A1
            strResultado = .worksheets(1).cells(14, 4).FormulaR1C1Local.ToString()

            'quitamos la aplicacion
            .Application.Quit()
            'cerrar la app jsts
            '.Workbooks(FileName:=sFileProc).Close()
        End With
        'liberamos de la memoria el archivo
        x = Nothing
        Return strResultado
    End Function


    Public Function LoadRange(
      ByVal strRuta As String,
      ByVal sSheetName As String,
      ByVal sRange As String) As DataTable

        Try

            ' // Declarar la Cadena de conexión   

            Dim sCs As String = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & ";Extended Properties=""Excel 12.0""")
            Using objOleConnection As New OleDbConnection(sCs)

                ' // Declarar la consulta SQL que indica el libro y el rango de la hoja   
                Dim sSql As String = String.Format("SELECT * FROM [{0}${1}]", sSheetName, sRange)
                Dim objDataAdapter As New OleDbDataAdapter(sSql, objOleConnection)
                Dim dt As New DataTable()

                objDataAdapter.Fill(dt)

                Return dt

            End Using

        Catch ex As Exception
            ' Devolvemos la excepción
            Throw

        End Try

    End Function


    Protected Function ImportExcel(strRuta As String) As DataTable
        ' Abrir el archivo Excel usando ClosedXML.
        Using workBook As New XLWorkbook(strRuta)
            ' Leer la primera hoja del archivo Excel.
            Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
            ' Crear un DataTable para almacenar los datos
            Dim dt As New DataTable()
            ' Agregar columnas al DataTable con los nombres específicos
            dt.Columns.Add("Puesto", GetType(String))
            'dt.Columns.Add("Nivel", GetType(String))
            dt.Columns.Add("Objetivo", GetType(String))
            ' dt.Columns.Add("Puesto_Reporto", GetType(String))
            dt.Columns.Add("NumPuestosMeReportan", GetType(String))
            dt.Columns.Add("PuestosMeReportan", GetType(String))
            dt.Columns.Add("Responsabilidad1", GetType(String))
            dt.Columns.Add("Responsabilidad2", GetType(String))
            dt.Columns.Add("Responsabilidad3", GetType(String))
            dt.Columns.Add("Responsabilidad4", GetType(String))
            dt.Columns.Add("Responsabilidad5", GetType(String))
            dt.Columns.Add("Responsabilidad6", GetType(String))
            dt.Columns.Add("Responsabilidad7", GetType(String))
            dt.Columns.Add("Responsabilidad8", GetType(String))
            dt.Columns.Add("Responsabilidad9", GetType(String))
            dt.Columns.Add("Responsabilidad10", GetType(String))
            dt.Columns.Add("Responsabilidad11", GetType(String))
            dt.Columns.Add("Responsabilidad12", GetType(String))
            dt.Columns.Add("Responsabilidad13", GetType(String))
            dt.Columns.Add("Facultades_Autorizacion", GetType(String))
            dt.Columns.Add("RelacionesIntQ", GetType(String))
            dt.Columns.Add("RelacionesIntP", GetType(String))
            dt.Columns.Add("RelacionesExtQ", GetType(String))
            dt.Columns.Add("RelacionesExtP", GetType(String))
            dt.Columns.Add("Grado_Escolaridad", GetType(String))
            dt.Columns.Add("Carrera_Especializacion", GetType(String))
            dt.Columns.Add("Formacion1", GetType(String))
            dt.Columns.Add("FormacionDom1", GetType(String))
            dt.Columns.Add("Formacion2", GetType(String))
            dt.Columns.Add("FormacionDom2", GetType(String))
            dt.Columns.Add("Formacion3", GetType(String))
            dt.Columns.Add("FormacionDom3", GetType(String))
            dt.Columns.Add("Formacion4", GetType(String))
            dt.Columns.Add("FormacionDom4", GetType(String))
            dt.Columns.Add("Formacion5", GetType(String))
            dt.Columns.Add("FormacionDom5", GetType(String))
            dt.Columns.Add("Formacion6", GetType(String))
            dt.Columns.Add("FormacionDom6", GetType(String))
            dt.Columns.Add("Idioma", GetType(String))
            dt.Columns.Add("IdiomaDom", GetType(String))
            dt.Columns.Add("Idioma2", GetType(String))
            dt.Columns.Add("IdiomaDo2", GetType(String))
            dt.Columns.Add("Habilidad1", GetType(String))
            dt.Columns.Add("Domino_Habilidad1", GetType(String))
            dt.Columns.Add("Habilidad2", GetType(String))
            dt.Columns.Add("Domino_Habilidad2", GetType(String))
            dt.Columns.Add("Habilidad3", GetType(String))
            dt.Columns.Add("Domino_Habilidad3", GetType(String))
            dt.Columns.Add("Habilidad4", GetType(String))
            dt.Columns.Add("Domino_Habilidad4", GetType(String))
            dt.Columns.Add("Habilidad5", GetType(String))
            dt.Columns.Add("Domino_Habilidad5", GetType(String))
            dt.Columns.Add("Habilidad6", GetType(String))
            dt.Columns.Add("Domino_Habilidad6", GetType(String))
            dt.Columns.Add("Habilidad7", GetType(String))
            dt.Columns.Add("Domino_Habilidad7", GetType(String))
            dt.Columns.Add("Habilidad8", GetType(String))
            dt.Columns.Add("Domino_Habilidad8", GetType(String))
            dt.Columns.Add("Word", GetType(String))
            dt.Columns.Add("Excel", GetType(String))
            dt.Columns.Add("PowerP", GetType(String))
            dt.Columns.Add("Outlook", GetType(String))
            dt.Columns.Add("Access", GetType(String))
            dt.Columns.Add("Project", GetType(String))
            dt.Columns.Add("AreaExperiencia", GetType(String))
            dt.Columns.Add("AreaExperienciaAños", GetType(String))
            dt.Columns.Add("Sistema_Gestion_Integrado", GetType(String))
            dt.Columns.Add("Requisito", GetType(String))

            ' Leer los datos de las celdas específicas y agregarlos al DataTable
            dt.Rows.Add(
            workSheet.Cell("F11").Value.ToString(),     ' Puesto
            workSheet.Cell("D14").Value.ToString(),     ' Objetivo
            workSheet.Cell("AB20").Value.ToString(),    ' NumPuestosMeReportan
            workSheet.Cell("V22").Value.ToString(),     ' PuestosMeReportan
            workSheet.Cell("D40").Value.ToString(),     ' responsabilidad_1
            workSheet.Cell("D42").Value.ToString(),     ' responsabilidad_2
            workSheet.Cell("D44").Value.ToString(),     ' responsabilidad_3
            workSheet.Cell("D46").Value.ToString(),     ' responsabilidad_4
            workSheet.Cell("D48").Value.ToString(),     ' responsabilidad_5
            workSheet.Cell("D50").Value.ToString(),     ' responsabilidad_6
            workSheet.Cell("D52").Value.ToString(),     ' responsabilidad_7
            workSheet.Cell("D54").Value.ToString(),     ' responsabilidad_8
            workSheet.Cell("D56").Value.ToString(),     ' responsabilidad_9
            workSheet.Cell("D58").Value.ToString(),     ' responsabilidad_10
            workSheet.Cell("D62").Value.ToString(),     ' responsabilidad_11
            workSheet.Cell("D64").Value.ToString(),     ' responsabilidad_12
            workSheet.Cell("D66").Value.ToString(),     ' responsabilidad_13
            workSheet.Cell("D70").Value.ToString(),     ' facultades_autorizacion
            workSheet.Cell("D78").Value.ToString(),     ' relacion_interna_quien
            workSheet.Cell("S78").Value.ToString(),     ' relacion_interna_para
            workSheet.Cell("D82").Value.ToString(),     ' relacion_externa_quien
            workSheet.Cell("S82").Value.ToString(),     ' relacion_externa_para
            workSheet.Cell("L90").Value.ToString(),     ' grado_escolaridad
            workSheet.Cell("L92").Value.ToString(),     ' carrera_especializacion
            workSheet.Cell("D98").Value.ToString(),     ' formacion_1
            workSheet.Cell("AE98").Value.ToString(),    ' domino_1
            workSheet.Cell("D100").Value.ToString(),    ' formacion_2
            workSheet.Cell("AE100").Value.ToString(),   ' domino_2
            workSheet.Cell("D102").Value.ToString(),    ' formacion_3
            workSheet.Cell("AE102").Value.ToString(),   ' domino_3
            workSheet.Cell("D104").Value.ToString(),    ' formacion_4
            workSheet.Cell("AE104").Value.ToString(),   ' domino_4
            workSheet.Cell("D106").Value.ToString(),    ' formacion_5
            workSheet.Cell("AE106").Value.ToString(),   ' domino_5
            workSheet.Cell("D108").Value.ToString(),    ' formacion_6
            workSheet.Cell("AE108").Value.ToString(),   ' domino_6
            workSheet.Cell("D112").Value.ToString(),    ' idioma_1
            workSheet.Cell("O112").Value.ToString(),    ' dominio_1
            workSheet.Cell("T112").Value.ToString(),    ' idioma_2
            workSheet.Cell("AE112").Value.ToString(),   ' dominio_2
            workSheet.Cell("D117").Value.ToString(),    ' habilidad_1
            workSheet.Cell("O117").Value.ToString(),    ' domino_1
            workSheet.Cell("T117").Value.ToString(),    ' habilidad_2
            workSheet.Cell("AE117").Value.ToString(),   ' domino_2
            workSheet.Cell("D119").Value.ToString(),    ' habilidad_3
            workSheet.Cell("O119").Value.ToString(),    ' domino_3
            workSheet.Cell("T119").Value.ToString(),    ' habilidad_4
            workSheet.Cell("AE119").Value.ToString(),   ' domino_4
            workSheet.Cell("D121").Value.ToString(),    ' habilidad_5
            workSheet.Cell("O121").Value.ToString(),    ' domino_5
            workSheet.Cell("T121").Value.ToString(),    ' habilidad_6
            workSheet.Cell("AE121").Value.ToString(),   ' domino_6
            workSheet.Cell("D123").Value.ToString(),    ' habilidad_7
            workSheet.Cell("O123").Value.ToString(),    ' domino_7
            workSheet.Cell("T123").Value.ToString(),    ' habilidad_8
            workSheet.Cell("AE123").Value.ToString(),   ' domino_8
            workSheet.Cell("F126").Value.ToString(),    ' word
            workSheet.Cell("J126").Value.ToString(),    ' excel
            workSheet.Cell("O126").Value.ToString(),    ' power_point
            workSheet.Cell("S126").Value.ToString(),    ' outlook
            workSheet.Cell("W126").Value.ToString(),    ' access
            workSheet.Cell("AA126").Value.ToString(),   ' project
            workSheet.Cell("I130").Value.ToString(),    ' areas_experiencia
            workSheet.Cell("AD130").Value.ToString(),   ' TIEMPO
            workSheet.Cell("D134").Value.ToString(),    ' sistema_gestion_integrado
            workSheet.Cell("D137").Value.ToString()     ' requisitos_condiciones
        )

            ' Retornar el DataTable con los datos
            Return dt
        End Using
    End Function


#End Region
#Region "Imprimir"
    Protected Sub lnkImprimir_Click(sender As Object, e As EventArgs)
        Call ImprimirDescriptivoP()
        Call obtenerUsuarioAD()
    End Sub

    Public Sub ImprimirDescriptivoP()

        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim path As String = Server.MapPath("~/UploadedFiles/")
        Dim strCarpetaPuesto As String = CStr(Now.Year & "\" & Now.Month & "\" & Now.Day & "\" & Now.Hour & Now.Minute & Now.Millisecond) & "\"
        Dim strCarpeta As String = path & "\Reclutamiento\DescriptivosPuesto\" & strCarpetaPuesto
        Dim strNombreArchivo As String = ddlTipoPuesto.SelectedItem.Text & ".xlsx"
        Dim strDescriptivo As String = strCarpeta & strNombreArchivo
        Dim strArchivo As String = path & "\DescriptivodePuesto.xlsx"
        Dim dsDatos As New DataSet
        Try
            'Valida que el archivo Base Exista
            If My.Computer.FileSystem.FileExists(strArchivo) Then

                'crea directorio
                If Not (Directory.Exists(strCarpeta)) Then
                    Directory.CreateDirectory(strCarpeta)
                End If
                'Valida que el archivo exista si existe lo elimina
                'If My.Computer.FileSystem.FileExists(strDescriptivo) Then My.Computer.FileSystem.DeleteFile(strDescriptivo)
                'If File.Exists(strDescriptivo) Then
                '    File.Delete(strDescriptivo)
                '    'Copia el Archivo en la Ruta
                'End If
                File.Copy(strArchivo, strDescriptivo)
            End If

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_info_puesto_sel_excel_print_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsDatos)
            Call WriteExcel(strDescriptivo, dsDatos)
            'Escribe Excel
            '    Call EscribirExcel(strDescriptivo, dsDatos)
            dsDatos.Dispose()
            odbConexion.Close()

            'Descarga el Archivo Creado
            Response.Clear()

            Response.AddHeader("Content-Disposition", "attachment; filename=" & strNombreArchivo)
            Response.Flush()
            Response.TransmitFile(strDescriptivo)
            Response.End()


        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try


    End Sub
    'Inserta Informacion en formato de excel
    Private Sub EscribirExcel(strArchivo As String, dsDatos As DataSet)
        Dim sConnString As String = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & strArchivo & "; Extended Properties=""Excel 12.0 Xml; HDR = NO;"""
        Dim odbConexion As New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Try
            strQuery += "INSERT INTO [FO-RH-4(2)$D13:D13] VALUES( 'Administrar, controlar y monitorear  el stock de refacciones existente. Con la finalidad de suministrar y eficientar los recursos disponibles que  logren la satisfacción del cliente. Por medio del manejo, análisis de inventarios e indicadores que registre')" ' Objetivo
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbConexion.Open()
            odbComando.ExecuteNonQuery()
        Finally
            odbConexion.Close()
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
#Region "Relaciones Internas"
    Public Sub obtRelacionesInternas()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_relaciones_interna_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            If ddlTipoPuesto.SelectedValue = "0" Then Exit Sub

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsDatos)
            grdRelacionesInternas.DataSource = dsDatos.Tables(0).DefaultView
            grdRelacionesInternas.DataBind()

            If grdRelacionesInternas.Rows.Count = 0 Then
                Call insFilaRelacionesInt()
                grdRelacionesInternas.Rows(0).Visible = False

            Else
                grdRelacionesInternas.Rows(0).Visible = True
            End If
            odbConexion.Close()

            For iFil As Integer = 0 To grdRelacionesInternas.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdRelacionesInternas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaRelacionesInt()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("correlativo"))
        dt.Columns.Add(New DataColumn("con_quien"))
        dt.Columns.Add(New DataColumn("para"))
        dr = dt.NewRow
        dr("id") = ""
        dr("correlativo") = "1"
        dr("con_quien") = ""
        dr("para") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdRelacionesInternas.DataSource = dt.DefaultView
        grdRelacionesInternas.DataBind()
    End Sub
    Private Sub AddNewRowToGrid()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Dim odbComando As New OleDbCommand
        Try
            odbConexion.Open()
            ''Valida  que no queden ningun vacio
            'For i As Integer = 0 To grdRelacionesInternas.Rows.Count - 1
            '    Dim strCorrelativo As String = grdRelacionesInternas.Rows(i).Cells(1).Text
            '    Dim txtConQuien As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
            '    Dim txtPara As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

            '    If txtConQuien.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Interna Con quien " & strCorrelativo & ".');</script>", False)
            '        Exit Sub
            '    End If

            '    If txtPara.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Interna Para " & strCorrelativo & ".');</script>", False)
            '        Exit Sub
            '    End If
            'Next

            'Actualiza Relaciones Internas
            Call UpdRelacionesInternas(odbConexion)

            odbComando = New OleDbCommand
            odbComando.CommandText = "do_relaciones_interna_ins_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            If ddlTipoPuesto.SelectedValue = "0" Then Exit Sub

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)


            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            'Inserta Registras
            Call obtRelacionesInternas()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Sub UpdRelacionesInternas(odbConexion As OleDbConnection)
        Dim odbComando = New OleDbCommand

        'Inserta la información Actual
        Dim strId As String = ""
        For i As Integer = 0 To grdRelacionesInternas.Rows.Count - 1
            strId = DirectCast(grdRelacionesInternas.Rows(i).FindControl("lblId"), Label).Text
            Dim txtConQuien As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
            Dim txtPara As TextBox = CType(grdRelacionesInternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

            odbComando = New OleDbCommand
            odbComando.CommandText = "do_relaciones_interna_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PId", strId)
            odbComando.Parameters.AddWithValue("@ConQuien", txtConQuien.Text)
            odbComando.Parameters.AddWithValue("@Para", txtPara.Text)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()
        Next
    End Sub

    Protected Sub lnkAgregarR_Click(sender As Object, e As EventArgs)
        Call AddNewRowToGrid()
    End Sub

    Protected Sub grdRelacionesInternas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Try
            strId = DirectCast(grdRelacionesInternas.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub


            'If grdRelacionesInternas.Rows.Count = 1 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar el ultimo registro.');</script>", False)
            '    Exit Sub
            'End If

            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_relaciones_interna_del_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PId", strId)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            Call obtRelacionesInternas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub
#End Region
#Region "Relaciones Externas"

    Public Sub obtRelacionesExternas()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_relaciones_externa_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            If ddlTipoPuesto.SelectedValue = "0" Then Exit Sub

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsDatos)
            grdRelacionesExternas.DataSource = dsDatos.Tables(0).DefaultView
            grdRelacionesExternas.DataBind()

            If grdRelacionesExternas.Rows.Count = 0 Then
                Call insFilaRelacionesExt()
                grdRelacionesExternas.Rows(0).Visible = False
            Else
                grdRelacionesExternas.Rows(0).Visible = True

            End If

            odbConexion.Close()


            For iFil As Integer = 0 To grdRelacionesExternas.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdRelacionesExternas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaRelacionesExt()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("correlativo"))
        dt.Columns.Add(New DataColumn("con_quien"))
        dt.Columns.Add(New DataColumn("para"))
        dr = dt.NewRow
        dr("id") = ""
        dr("correlativo") = "1"
        dr("con_quien") = ""
        dr("para") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdRelacionesExternas.DataSource = dt.DefaultView
        grdRelacionesExternas.DataBind()
    End Sub
    Private Sub AddNewRowToGridExt()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsDatos As New DataSet
        Dim odbComando As New OleDbCommand
        Try
            odbConexion.Open()
            'Valida  que no queden ningun vacio
            'For i As Integer = 0 To grdRelacionesExternas.Rows.Count - 1
            '    Dim strCorrelativo As String = grdRelacionesExternas.Rows(i).Cells(1).Text
            '    Dim txtConQuien As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
            '    Dim txtPara As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

            '    If txtConQuien.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Externa Con quien " & strCorrelativo & ".');</script>", False)
            '        Exit Sub
            '    End If

            '    If txtPara.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Relación Externa Para " & strCorrelativo & ".');</script>", False)
            '        Exit Sub
            '    End If
            'Next

            'Actualiza Relaciones Internas
            Call UpdRelacionesExternas(odbConexion)

            odbComando = New OleDbCommand
            odbComando.CommandText = "do_relaciones_externas_ins_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            If ddlTipoPuesto.SelectedValue = "0" Then Exit Sub

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            'Inserta Registras
            Call obtRelacionesExternas()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Public Sub UpdRelacionesExternas(odbConexion As OleDbConnection)
        Dim odbComando = New OleDbCommand

        'Inserta la información Actual
        Dim strId As String = ""
        For i As Integer = 0 To grdRelacionesExternas.Rows.Count - 1
            strId = DirectCast(grdRelacionesExternas.Rows(i).FindControl("lblId"), Label).Text
            Dim txtConQuien As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(1).FindControl("txtConQuien"), TextBox)
            Dim txtPara As TextBox = CType(grdRelacionesExternas.Rows(i).Cells(2).FindControl("txtPara"), TextBox)

            odbComando = New OleDbCommand
            odbComando.CommandText = "do_relaciones_externas_upd_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PId", strId)
            odbComando.Parameters.AddWithValue("@ConQuien", txtConQuien.Text)
            odbComando.Parameters.AddWithValue("@Para", txtPara.Text)
            odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)
            odbComando.ExecuteNonQuery()
        Next
    End Sub


    Protected Sub lnkAgregarRExter_Click(sender As Object, e As EventArgs)
        Call AddNewRowToGridExt()
    End Sub
    Protected Sub grdRelacionesExternas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Try
            strId = DirectCast(grdRelacionesExternas.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub


            'If grdRelacionesExternas.Rows.Count = 1 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar el ultimo registro.');</script>", False)
            '    Exit Sub
            'End If

            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_relaciones_externas_del_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            'parametros
            odbComando.Parameters.AddWithValue("@PId", strId)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            Call obtRelacionesExternas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Envia Documento"

    Protected Sub lnkEnviarDocumentos_Click(sender As Object, e As EventArgs)
        Call EnviarFormato()
    End Sub

    Public Sub EnviarFormato()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim arrValores As New ArrayList
        Dim strCuerpo As String = ""
        Dim strUrl As String = ""
        Dim strAsunto As String = ""
        Dim strDestrinatario As String = ""
        Dim strDesarrolloOrg As String = ""
        Dim strCopia As String = ""
        Dim strNombreArchivos As String = ""
        Dim strEncabezadoCuerpo As String = ""
        Dim strColaboradores As String = ""
        Dim strDescriptivo As String = ""
        Dim strEmpresa As String = ""
        Try
            strEmpresa = lblEmpresa.Text
            strEmpresa = strEmpresa.Substring(strEmpresa.LastIndexOf(">") + 1)
            odbConexion.Open()

            'Copia Jefe
            For Each item As ListItem In ddlCopiaJefe.Items
                If item.Selected Then
                    strColaboradores += item.Value + ";"
                    strCopia += obtCorreoColaborador(item.Value, odbConexion) + ";"
                End If
            Next
            'Desarrollo Organizacional
            For Each item As ListItem In ddlCorreoDO.Items
                If item.Selected Then
                    strColaboradores += item.Value + ";"
                    strDesarrolloOrg += obtCorreoColaborador(item.Value, odbConexion) + ";"
                End If
            Next

            'Crea Excel
            strDescriptivo = CreaDescriptivo()

            'Valida el tipo de Correo que tiene que enviar
            strAsunto = "SIGIDO |" & IIf(rdTextoColaborador.Checked, " Notificación", "") & " Descriptivo de Puesto: " & ddlTipoPuesto.SelectedItem.Text
            If rdTextoColaborador.Checked Then
                strEncabezadoCuerpo = "Estimado Colaborador (a).<br /> <br />" &
                         "Por medio del presente, cumplimos con enviarle y hacer de su conocimiento la actualización del Descriptivo de Puesto - Identificación de la Competencia, anexo (Formato FO-RH-4/4), correspondiente al puesto que desempeñas en  " & strEmpresa & ", en el entendido que " &
                         "esta comunicación sirve como notificación formal. <br /> <br />" &
                         "Te agradecemos leer el contenido del documento anexo y cualquier duda podrás consultarla con tu jefe inmediato o en la Coordinación de Desarrollo " &
                         "Organizacional.<br /><br /> Se copia al Jefe Inmediato, a los fines de cualquier validación adicional u observación sobre el contenido del Descriptivo de Puesto." &
                         "<br /> <br /> <strong>Es importante que por este mismo medio nos notifiques de recibido y enterado de la información enviada</strong> <br /> <br />"
            ElseIf rdTextoJefe.Checked Then
                strEncabezadoCuerpo = "Estimado Colaborador (a).<br /> <br />" &
                    "Anexo enviamos el Descriptivo de Puesto - Identificación de la Competencia (Formato FO-RH-4/4), correspondiente al puesto indicado" &
                    " en el asunto, para su revisión, actualización y/o validación. <br /> <br />" &
                    "Si tiene alguna observación o considera necesaria la modificación del contenido del Descriptivo de Puesto enviado, le agradecemos" &
                    " plasmarlas de manera clara, en el formato enviado y devolverlo a Desarrollo Organizacional, para ser incorporadas al Módulo para la" &
                    " Administración de la Estructura de Puestos.<br /> <br />" &
                    "Las modificaciones realizadas no deben alterar el alcance o esencia del puesto y en función de esto, estarán sujetas a la validación" &
                    " de la Coordinación de Desarrollo organizacional, tomando en consideración el nivel actual del puesto. "
            ElseIf rdTextoJefeSinColaborador.Checked Then
                strEncabezadoCuerpo = "Estimado Colaborador (a).<br /> <br />" &
                    "Anexo enviamos el Descriptivo de Puesto - Identificación de la Competencia (Formato FO-RH-4/4), correspondiente al puesto que se indica en el asunto, " &
                    " para que a través de usted pueda ser entregado al ocupante, quien le reporta directamente y originado de la falta de una cuenta de correo institucional.  <br /> <br />" &
                    "Previo a la entrega del documento, si tiene alguna observación o considera necesaria la modificación del contenido del Descriptivo de Puesto enviado," &
                    " le agradecemos plasmarlas de manera clara, en el formato enviado y devolverlo a Desarrollo Organizacional, para ser incorporadas al Módulo para la Administración de la Estructura de Puestos.  <br /> <br />" &
                    "Las modificaciones realizadas no deben alterar el alcance o esencia del puesto y estarán sujetas a la validación de la Coordinación de Desarrollo organizacional, " &
                    "tomando en consideración el nivel actual del puesto. <br /> <br />" &
                    "Por último le solicitamos que sobre este mismo correo nos confirme que se realizó  la revisión y entrega al ocupante del puesto."

            End If

            'Cuerpo
            strCuerpo = "<p style=""font-family:Arial; font-size: 11pt;"">" & strEncabezadoCuerpo & " </p>" & vbCrLf

            strCuerpo = strCuerpo & "<hr />" & vbCrLf
            strCuerpo = strCuerpo & "<p style=""font-family:Arial;"">Atentamente,</p>" & vbCrLf
            strCuerpo = strCuerpo & "<strong><p style=""font-family:Arial; font-size: 11pt;"">Desarrollo Organizacional</p></strong>" & vbCrLf & vbCrLf
            strCuerpo = strCuerpo & "<img src=""Desarrollo.jpg""/> " & vbCrLf & vbCrLf & vbCrLf

            'Colaborador
            For Each item As ListItem In ddlColaboradoresEnviar.Items
                If item.Selected Then
                    ' strColaboradores += item.Value + ";"
                    strDestrinatario = obtCorreoColaborador(item.Value, odbConexion) + ";"
                    If strDestrinatario = "" Then Continue For
                    'Envia Correo
                    Call EnviaCorreoAdjunto(strDestrinatario, strAsunto, strCuerpo, strCopia, strDescriptivo, strDesarrolloOrg, odbConexion)
                End If
            Next



            arrValores = Nothing
            odbConexion.Close()

            'Desabilita la informacion de los colaboradores
            For Each item As ListItem In ddlColaboradoresEnviar.Items
                item.Selected = False
            Next

            For Each item As ListItem In ddlCopiaJefe.Items
                item.Selected = False
            Next
            For Each item As ListItem In ddlCorreoDO.Items
                item.Selected = False
            Next
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
        odbComando.Parameters.AddWithValue("@PIdClave", strClave)


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
    Public Sub EnviaCorreoAdjunto(Destinatario As String, Asunto As String, Cuerpo As String, Copia As String, Adjunto As String, strCopiaOculta As String, odbConexion As OleDbConnection)
        Dim arrValores As New ArrayList
        Dim strResultado As String = ""


        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "sigido_enviaremail_adjuntos_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@destinatario", Destinatario)
        odbComando.Parameters.AddWithValue("@asunto", Asunto)
        odbComando.Parameters.AddWithValue("@cuerpo", Cuerpo)
        odbComando.Parameters.AddWithValue("@Concopia", Copia)
        odbComando.Parameters.AddWithValue("@Adjunto", ";" + Adjunto)
        odbComando.Parameters.AddWithValue("@CopiaOculta", strCopiaOculta)

        odbComando.ExecuteNonQuery()


        arrValores = Nothing
    End Sub

    Public Function CreaDescriptivo()

        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim path As String = Server.MapPath("~/UploadedFiles/")
        Dim strCarpetaPuesto As String = CStr(Now.Year & "\" & Now.Month & "\" & Now.Day & "\" & Now.Hour & Now.Minute & Now.Millisecond) & "\"
        Dim strCarpeta As String = path & "\Reclutamiento\DescriptivosPuesto\" & strCarpetaPuesto
        Dim strNombreArchivo As String = ddlTipoPuesto.SelectedItem.Text & ".xlsx"
        Dim strDescriptivo As String = strCarpeta & strNombreArchivo
        Dim strArchivo As String = path & "\DescriptivodePuesto.xlsx"
        Dim dsDatos As New DataSet
        Try
            'Valida que el archivo Base Exista
            If My.Computer.FileSystem.FileExists(strArchivo) Then

                'crea directorio
                If Not (Directory.Exists(strCarpeta)) Then
                    Directory.CreateDirectory(strCarpeta)
                End If
                'Valida que el archivo exista si existe lo elimina
                'If My.Computer.FileSystem.FileExists(strDescriptivo) Then My.Computer.FileSystem.DeleteFile(strDescriptivo)
                'If File.Exists(strDescriptivo) Then
                '    File.Delete(strDescriptivo)
                '    'Copia el Archivo en la Ruta
                'End If
                File.Copy(strArchivo, strDescriptivo)
            End If

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_info_puesto_sel_excel_print_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PIdPuesto", ddlTipoPuesto.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsDatos)
            Call WriteExcel(strDescriptivo, dsDatos)
            'Escribe Excel
            '    Call EscribirExcel(strDescriptivo, dsDatos)
            dsDatos.Dispose()
            odbConexion.Close()



        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
        Return strDescriptivo

    End Function
#End Region

End Class