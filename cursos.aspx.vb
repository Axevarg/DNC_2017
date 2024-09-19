Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO

Public Class cursos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorArchivo.Text = ""
        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            Call obtenerUsuarioAD()
            'Call obtIdDNC()
            'Call CargaCatalogos()
            Call ObtCatalogoDNC()
        End If
        Call Comportamiento()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
    End Sub
#Region "Catalogos"
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
    Private Sub ddlProveedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProveedor.SelectedIndexChanged
        Call ObtFacilitador(ddlProveedor.SelectedValue)
    End Sub
    Public Sub CargaCatalogos()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Call ObtProveedor(odbConexion)
            Call ObtFacilitador(ddlProveedor.SelectedValue)
            Call ObtCursoBuscar(odbConexion)
            Call ObtHabilidadDina(odbConexion)
            Call ObtModalidad(odbConexion)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    Private Sub ObtCursoBuscar(ByVal odbConexion As OleDbConnection)
        Dim strFiltro As String = ""
        Dim strQuery As String = ""

        strQuery = "SELECT '0'as ID,'  SELECCIONAR UN CURSO' as descripcion_capacitacion_corta union all "
        strQuery += "SELECT  id,descripcion_capacitacion_corta  FROM DNC_CURSOS_TB WHERE ESTATUS<>'ELIMINADO' and fk_id_parametrizacion=" & ddlDnc.SelectedValue & " ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlCursos.DataSource = odbLector
        ddlCursos.DataValueField = "ID"
        ddlCursos.DataTextField = "descripcion_capacitacion_corta"

        ddlCursos.DataBind()

    End Sub
    'DNC
    Private Sub ObtCatalogoDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        odbConexion.Open()
        Dim strFiltro As String = ""
        Dim strQuery As String = ""
        Try
            strQuery = "SELECT  id,nombre  FROM DNC_PARAMETRIZACION_CT  ORDER BY 2"

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlDnc.DataSource = odbLector
            ddlDnc.DataValueField = "ID"
            ddlDnc.DataTextField = "nombre"

            ddlDnc.DataBind()
            ddlDnc.Items.Insert(0, New ListItem("Seleccionar", "0"))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Private Sub ddlDnc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDnc.SelectedIndexChanged
        hdIdDNC.Value = ddlDnc.SelectedValue
        hdIdCurso.Value = 0
        Call limpiarControles()
        Call CargaCatalogos()

        Call Comportamiento()
    End Sub
#End Region
#Region "Comportamientos"
    Public Sub Comportamiento()
        If hdIdCurso.Value = 0 Then
            btnActualizar.Visible = False
            btnEliminar.Visible = False
            btnNuevoCurso.Visible = False
            btnAgregar.Visible = True
            divAuto.Visible = False
            btnImagen.Visible = False
        Else
            btnActualizar.Visible = True
            btnEliminar.Visible = True
            btnNuevoCurso.Visible = True
            btnAgregar.Visible = False
            divAuto.Visible = True
            btnImagen.Visible = True
        End If
        If ddlDnc.SelectedValue = "0" Then
            divCursos.Visible = False
            btnAgregar.Visible = False
            divDatosCursos.Visible = False
        Else
            divCursos.Visible = True
            divDatosCursos.Visible = True
        End If

    End Sub
#End Region
#Region "Acciones"
    'iNSERTA cURSO
    Public Sub insCursos()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim iId As Integer = 0
        Try
            odbConexion.Open()

            strQuery = "INSERT INTO [dbo].[DNC_CURSOS_TB]     " & _
                     "([fk_id_proveedor]                                " & _
                     ",[fk_id_facilitador]                              " & _
                     ",[descripcion_capacitacion_corta]                 " & _
                     ",[descripcion_capacitacion]                       " & _
                     ",[objetivo]                                       " & _
                     ",[modalidad]                                      " & _
                     ",[numero_participantes]                           " & _
                     ",[habilidades_desarrollar]                        " & _
                     ",[fk_id_habilidad_dina]                           " & _
                     ",[duracion]                                       " & _
                     ",[costo_individual]                               " & _
                     ",[costo_grupo]                                    " & _
                     ",[iva]                                    " & _
                     ",[moneda]                                         " & _
                     ",[evaluacion]                                     " & _
                     ",[constacia]                                      " & _
                     ",[dc3]                                            " & _
                     ",[fuente]                                         " & _
                     ",[estatus]                                         " & _
                     ",[fecha_creacion]                                 " & _
                     ",[usuario_creacion]  ,[fk_id_parametrizacion] )   " & _
                     " VALUES                                           " & _
                     "('" & ddlProveedor.SelectedValue & "'" & _
                     "," & ddlFacilitador.SelectedValue & " " & _
                     ",'" & txtDescripcionCorta.Text.Trim & "'" & _
                     ",'" & txtDescripcion.Text.Trim & "'" & _
                     ",'" & txtObjetivo.Text.Trim & "'" & _
                     ",'" & ddlModalidad.SelectedValue & "'" & _
                     ",'" & txtNoMaximoParticipante.Text.Trim & "'" & _
                     ",'" & txtHabilidadesDesarrollar.Text.Trim & "'" & _
                     ",'" & ddlHabilidadDina.SelectedValue & "'" & _
                     ",'" & IIf(txtDuracionHoras.Text = "", 0, txtDuracionHoras.Text.Trim) & "'" & _
                     ",'" & IIf(txtCostoIndividual.Text = "", 0, txtCostoIndividual.Text.Trim) & "'" & _
                     ",'" & IIf(txtCostoGrupo.Text = "", 0, txtCostoGrupo.Text.Trim) & "'" & _
                     ",'" & IIf(txtIVA.Text = "", 0, txtIVA.Text.Trim) & "'" & _
                     ",'" & ddlMoneda.SelectedValue.ToString.Trim & "'" & _
                     ",'" & ddlEvaluacion.SelectedValue.ToString.Trim & "'" & _
                     ",'" & ddlConstancia.SelectedValue.ToString.Trim & "'" & _
                     ",'" & ddlDC3.SelectedValue & "'" & _
                     ",'" & txtFuente.Text.Trim & "'" & _
                     ", 'CREADO'" & _
                     ", GETDATE()" & _
                     ", '" & hdUsuario.Value & "'" & _
                     ", '" & ddlDnc.SelectedValue.ToString & "'" & _
                     ") Select @@Identity"
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)

            iId = CInt(odbComando.ExecuteScalar())
            hdIdCurso.Value = iId

            Call obtCurso(iId)
            Call Comportamiento()
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub
    'obtiene los registros de los autos
    Public Sub obtCurso(Optional iId As Integer = 0)
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'actualiza carros y empleados
            Call ObtCursoBuscar(odbConexion)

            strQuery = "SELECT * FROM [DNC_CURSOS_TB] WHERE id=" & iId.ToString

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader

            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                Call ObtFacilitador(odbLector(1).ToString)

                ddlCursos.SelectedValue = odbLector(0).ToString
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
                txtFuente.Text = odbLector(18).ToString
                odbLector.Close()

                'obtien el menu vigente
                Call obtArchivoMenu()
            End If

            odbConexion.Close()
            Call Comportamiento()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub udCurso()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()


            strQuery = " UPDATE [dbo].[DNC_CURSOS_TB]   " & _
                       " SET [fk_id_proveedor] = '" & ddlProveedor.SelectedValue & "'" & _
                       "  ,[fk_id_facilitador] = '" & ddlFacilitador.SelectedValue & "'" & _
                       "  ,[descripcion_capacitacion_corta] = '" & txtDescripcionCorta.Text.Trim & "'" & _
                       "  ,[descripcion_capacitacion] = '" & txtDescripcion.Text.Trim & "'" & _
                       "  ,[objetivo] = '" & txtObjetivo.Text.Trim & "'" & _
                       "  ,[modalidad] = '" & ddlModalidad.SelectedValue & "'" & _
                       "  ,[numero_participantes]  = '" & IIf(txtNoMaximoParticipante.Text = "", 0, txtNoMaximoParticipante.Text.Trim) & "'" & _
                       "  ,[habilidades_desarrollar] = '" & txtHabilidadesDesarrollar.Text & "'" & _
                       "  ,[fk_id_habilidad_dina]  = '" & ddlHabilidadDina.SelectedValue & "'" & _
                       "  ,[duracion]  = '" & IIf(txtDuracionHoras.Text = "", 0, txtDuracionHoras.Text.Trim) & "'" & _
                       "  ,[costo_individual] = '" & IIf(txtCostoIndividual.Text = "", 0, txtCostoIndividual.Text.Trim) & "'" & _
                       "  ,[costo_grupo]  = '" & IIf(txtCostoGrupo.Text = "", 0, txtCostoGrupo.Text.Trim) & "'" & _
                       "  ,[iva]  = '" & IIf(txtIVA.Text = "", 0, txtIVA.Text.Trim) & "'" & _
                       "  ,[moneda]  = '" & ddlMoneda.SelectedValue & "'" & _
                       "  ,[evaluacion]  = '" & ddlEvaluacion.SelectedValue.ToString.Trim & "'" & _
                       "  ,[constacia] = '" & ddlConstancia.SelectedValue.ToString.Trim & "'" & _
                       "  ,[dc3]  = '" & ddlDC3.SelectedValue.ToString.Trim & "'" & _
                       "  ,[fuente]  = '" & txtFuente.Text.Trim & "'" & _
            "      ,[estatus] ='MODIFICADO'" & _
            "      ,[fecha_modificacion] = GETDATE()      " & _
            "      ,[usuario_modificacion] ='" & hdUsuario.Value & "'" & _
            " WHERE ID=" & hdIdCurso.Value.ToString
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            'Vallida si no manda mensaje
            Dim strMensaje As String = ValidaNumeroParticipantes(odbConexion)
            If strMensaje <> "" Then
                lblmessage.Text = strMensaje
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "Modal Mensaje", "AbrirModalMensaje();", True)
            End If
            Call obtCurso(hdIdCurso.Value.ToString)
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'Validacion de Cursos que tengan autorizacion de numero de personas fijas para registro de curso
    Public Function ValidaNumeroParticipantes(odbConexion As OleDbConnection)
        Dim strResultado As String = ""
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_cursos_valida_actualiza_participantes_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@PIdCurso", hdIdCurso.Value.ToString)
        odbComando.Parameters.AddWithValue("@NuParticipantes", IIf(txtNoMaximoParticipante.Text = "", 0, txtNoMaximoParticipante.Text.Trim))
        odbComando.Parameters.AddWithValue("@Usuario", hdUsuario.Value)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If

        Return strResultado
    End Function

    Private Sub btnAgregar_ServerClick(sender As Object, e As EventArgs) Handles btnAgregar.ServerClick
        Call insCursos()
    End Sub
    Private Sub btnActualizar_ServerClick(sender As Object, e As EventArgs) Handles btnActualizar.ServerClick
        Call udCurso()
    End Sub

    Public Sub delCurso()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()
            strQuery = " UPDATE [dbo].[DNC_CURSOS_TB]                                 " & _
            "   SET[estatus] ='ELIMINADO'" & _
            "      ,[fecha_modificacion] = GETDATE()      " & _
            "      ,[usuario_modificacion] ='" & hdUsuario.Value & "'" & _
            " WHERE ID=" & hdIdCurso.Value.ToString
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub limpiarControles()
        ddlCursos.SelectedValue = 0
        'ddlProveedor.SelectedValue = 1
        'ddlFacilitador.SelectedValue =
        txtDescripcion.Text = ""
        txtObjetivo.Text = ""
        '   ddlModalidad.SelectedValue = "1"
        txtNoMaximoParticipante.Text = ""
        txtHabilidadesDesarrollar.Text = ""
        ddlHabilidadDina.SelectedValue = "1"
        txtDuracionHoras.Text = ""
        txtCostoIndividual.Text = ""
        txtCostoGrupo.Text = ""
        ddlMoneda.SelectedValue = "Pesos"
        ddlEvaluacion.SelectedValue = "Si"
        ddlConstancia.SelectedValue = "Si"
        ddlDC3.SelectedValue = "Si"
        txtFuente.Text = ""
        txtDescripcionCorta.Text = ""
        txtIVA.Text = ""
    End Sub
    Private Sub ddlCursos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCursos.SelectedIndexChanged


        hdIdCurso.Value = ddlCursos.SelectedValue
        Call limpiarControles()
        Call obtCurso(hdIdCurso.Value)

        Call Comportamiento()
    End Sub

    Private Sub btnEliminar_ServerClick(sender As Object, e As EventArgs) Handles btnEliminar.ServerClick
        Call delCurso()
        hdIdCurso.Value = 0
        Call limpiarControles()
        Call Comportamiento()
        Call CargaCatalogos()
    End Sub

    Private Sub btnNuevoCurso_ServerClick(sender As Object, e As EventArgs) Handles btnNuevoCurso.ServerClick
        hdIdCurso.Value = 0
        Call limpiarControles()
        Call Comportamiento()
        Call CargaCatalogos()
    End Sub
#End Region
#Region "PDF Temario"

    Private Sub btnCargaArchivos_ServerClick(sender As Object, e As EventArgs) Handles btnCargaArchivos.ServerClick
        Call CargaArchivosMenu()
        '   Call obtCurso(hdIdCurso.Value.ToString)
        Call obtArchivoMenu()
    End Sub
    'carga imagenes de timeLine
    Public Sub CargaArchivosMenu()
        Try
            If IsPostBack Then

                Dim path As String = Server.MapPath("~/UploadedFiles/")
                Dim fileOK As Boolean = False
                If fucArchivo.HasFile Then

                    If fucArchivo.FileBytes.Length > 1073741824 Then
                        lblErrorArchivo.Text = "* El tamaño de archivo no debe sobrepasar 1GB."
                        Exit Sub
                    End If


                    Dim fileExtension As String
                    fileExtension = System.IO.Path. _
                        GetExtension(fucArchivo.FileName).ToLower()


                    Dim allowedExtensions As String() = _
                        {".pdf"}
                    For i As Integer = 0 To allowedExtensions.Length - 1
                        If fileExtension = allowedExtensions(i) Then
                            fileOK = True
                        End If
                    Next
                    If fileOK Then
                        Try
                            Dim strDirectorio As String = ""
                            strDirectorio = "\Temario\Temario_" & hdIdCurso.Value.ToString & "\"

                            'crea directorio
                            If Not (Directory.Exists(path & strDirectorio)) Then
                                Directory.CreateDirectory(path & strDirectorio)
                            End If
                            'nombre de Fotografia
                            Dim strNombre As String = strDirectorio & Now.Year.ToString & Now.Month.ToString & _
                                Now.Day.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & fucArchivo.FileName
                            Dim strRuta As String = path & strNombre
                            fucArchivo.PostedFile.SaveAs(strRuta)

                            Call insArchivoMenu("UploadedFiles" & strNombre, fileExtension.Trim, fucArchivo.FileName)
                            ' lblErrorPerfil.Text = "File uploaded!"
                        Catch ex As Exception
                            lblErrorArchivo.Text = "El archivo no se puede subir."
                        End Try
                    Else
                        lblErrorArchivo.Text = "No se puede aceptar archivos de este tipo."
                    End If
                End If
            End If
        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub


    Public Sub insArchivoMenu(ByVal strRuta As String, strExtencion As String, strNombre As String)
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try

            odbConexion.Open()
            'ACTUALIZA MENU AOBSOLETO
            Call actMenuObsoleto()
            strQuery = "INSERT INTO [dbo].[DNC_TEMARIOS_PDF_TB]" & _
                       "  VALUES " & _
                       "  (" & hdIdCurso.Value & "" & _
                       "  ,'" & strRuta & "' " & _
                       "  ,'" & strNombre & "' " & _
                       "  ,'" & strExtencion & "' " & _
                       "  , 'VIGENTE'" & _
                       "  ,GETDATE() " & _
                       "  ,'" & hdUsuario.Value & "' " & _
                       "  ,NULL " & _
                       "  ,NULL ) "
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            Call CargaMenuPagina(hdRol.Value, odbConexion)
            odbConexion.Close()

        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

    Public Sub actMenuObsoleto()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try

            odbConexion.Open()
            strQuery = "UPDATE [dbo].[DNC_TEMARIOS_PDF_TB]" & _
                       " SET [estatus] = 'OBSOLETO'" & _
                       "      ,[fecha_modificacion] = GETDATE()" & _
                       "      ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                       "WHERE fk_id_curso=" & hdIdCurso.Value

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Public Sub obtArchivoMenu()
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""

        Try

            odbConexion.Open()
            Dim strExtencion As String = ""

            strQuery = "SELECT ruta " & _
               " FROM DNC_TEMARIOS_PDF_TB WHERE estatus='VIGENTE' and fk_id_curso=" & hdIdCurso.Value

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            divCurosMensaje.Attributes.Remove("class")
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                iFramePdf.Attributes.Remove("src")
                iFramePdf.Attributes("src") = odbLector(0).ToString
                divCurosMensaje.Attributes("class") = "callout callout-success"
                lblCurso.Text = "El Curso esta cargado correctamente."
                odbLector.Close()
            Else
                iFramePdf.Attributes.Remove("src")
                divCurosMensaje.Attributes("class") = "callout callout-warning"
                lblErrorArchivo.Text = ""
                lblCurso.Text = "El Curso no tiene el Temario Cargado"
            End If
            odbConexion.Close()
        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

#End Region
#Region "DNC"
    'valida dnc
    Public Sub obtIdDNC()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT * FROM DNC_PARAMETRIZACION_CT where ESTATUS='VIGENTE' ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            hdIdDNC.Value = odbLector(0).ToString
            lblDNC.Text = odbLector(1).ToString
        Else
            Response.Redirect("parametrizacion.aspx")
        End If
        odbConexion.Close()

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