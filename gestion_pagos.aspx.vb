Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing
Imports System.IO

Public Class gestion_pagos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorPagos.Text = ""
        lblErrorArchivo.Text = ""
        If Not Page.IsPostBack Then
            lblRegistrosCarta.Text = ""
            Call obtenerUsuarioAD()
            Call CargaCatalogos()
            Call obtPagosRegistrado()
            hdCrearPago.Value = 0
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
        Call comportamientos()
    End Sub



#Region "Catalogos"
    Public Sub CargaCatalogos()
        Call obtEmpresa()
        Call obtTipoProyecto()
        Call obtSolicitaPago()
        Call obtAFavor()

    End Sub
    'Obtiene el catalogo de Empresa
    Public Sub obtEmpresa()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año

            strQuery = " SELECT * FROM SIGIDO_EMPRESA_CT WHERE ESTATUS=1 order by descripcion"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlEmpresa.DataSource = odbLector
            ddlEmpresa.DataTextField = "descripcion"
            ddlEmpresa.DataValueField = "id"

            ddlEmpresa.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'Tipo Proyecto
    Public Sub obtTipoProyecto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año

            strQuery = " SELECT * FROM [BECAS_TIPO_BECA_CT] WHERE ESTATUS=1 order by descripcion"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlTipoProyecto.DataSource = odbLector
            ddlTipoProyecto.DataTextField = "descripcion"
            ddlTipoProyecto.DataValueField = "id"

            ddlTipoProyecto.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    'Tipo Proyecto
    Public Sub obtSolicitaPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        Try
            odbConexion.Open()
            'registra año

            strQuery = " SELECT * FROM [SIGIDO_USUARIOS_TB] WHERE rol='2' order by nombre"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlSolicitaPago.DataSource = odbLector
            ddlSolicitaPago.DataTextField = "nombre"
            ddlSolicitaPago.DataValueField = "clave"

            ddlSolicitaPago.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'obtiene los datos a Favor de Colaborador o Proveedor
    Public Sub obtAFavor()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'registra año
            If ddlAFavorDe.SelectedItem.Text = "Empleado" Then
                strQuery = " SELECT CLAVE as id ,(NOMBRE + SPACE(1) + APPAT + SPACE(1) + APMAT)  AS NOMBRE " & _
                           " FROM SGIDO_INFOGIRO_GIRO_VT " & _
                           " WHERE ESTATUS='ACTIVO' AND CLAVE NOT IN (SELECT fk_id_colaborador FROM BECAS_GESTION_BECAS_TB WHERE fk_id_estatus_pago=2) order by 2"
                'Limpia Combo
                ddlCursoRelacionado.Items.Clear()
                ddlCursoRelacionado.Items.Insert(0, New ListItem("Seleccionar", 0))
            Else
                strQuery = " SELECT id,nombre FROM [SIGIDO_PROVEEDORES_CT] order by 2"
                Call ObtCursoProveedor()
            End If


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlProvColaborador.DataSource = odbLector
            ddlProvColaborador.DataTextField = "nombre"
            ddlProvColaborador.DataValueField = "id"

            ddlProvColaborador.DataBind()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Private Sub ddlProvColaborador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvColaborador.SelectedIndexChanged
        If ddlAFavorDe.SelectedItem.Text = "Empleado" Then
            ddlCursoRelacionado.Items.Clear()
            ddlCursoRelacionado.Items.Insert(0, New ListItem("Seleccionar", 0))
        Else
            Call ObtCursoProveedor()
        End If

    End Sub
    'Obtiene lsu Cursos en caso de ser asocioado
    Public Sub ObtCursoProveedor()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'registra año
            strQuery = "SELECT ID,CAST(correlativo AS VARCHAR)+SPACE(1)+descripcion_capacitacion_corta as descripcion " & _
                       " FROM GC_GESTION_CAPACITACION_TB WHERE fk_id_proveedor=" & ddlProvColaborador.SelectedValue & _
                       " AND fk_id_pago=" & ddlPagosBuscar.SelectedValue

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlCursoRelacionado.DataSource = odbLector
            ddlCursoRelacionado.DataTextField = "descripcion"
            ddlCursoRelacionado.DataValueField = "id"

            ddlCursoRelacionado.DataBind()
            ddlCursoRelacionado.Items.Insert(0, New ListItem("Seleccionar", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'obtiene los Pagos Registradps
    Public Sub obtPagosBuscar()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'registra año
            strQuery = " Select [ID] ,'Pago ' +CAST(NO_CORRELATIVO AS VARCHAR) + ' - ' +[NOMBRE]+ ' - ' +[TIPO_PROYECTO]  AS PAGO " & _
                       "  FROM [BECAS_PAGOS_VT] ORDER BY NO_CORRELATIVO desc "


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader


            ddlPagosBuscar.DataSource = odbLector
            ddlPagosBuscar.DataTextField = "PAGO"
            ddlPagosBuscar.DataValueField = "id"

            ddlPagosBuscar.DataBind()
            ddlPagosBuscar.Items.Insert(0, New ListItem("Seleccionar", 0))
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

#End Region


#Region "Comportamientos"
    Public Sub comportamientos()


        For iFil As Integer = 0 To grdPagosRegistrados.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPagosRegistrados.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To grdPartidas.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPartidas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        'comportamientos 
        If hdCrearPago.Value = 0 Then

            divPagoCrear.Visible = False
            btnGenerarPago.Visible = False
            btnCancelar.Visible = False
        Else
            divPagoCrear.Visible = True
            btnGenerarPago.Visible = True
            btnCancelar.Visible = True
        End If

        'comportamiento de Tipo de Cambio
        If ddlMoneda.SelectedItem.Text = "Pesos" Then
            txtTipoCambio.Visible = False
        Else
            txtTipoCambio.Visible = True
        End If

        grdPartidas.Columns(7).Visible = True
        grdPartidas.Columns(8).Visible = True
    End Sub
#End Region
#Region "Pagos"
    Public Sub crearBorradorPago(Optional strSolicitud As String = "")
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim iId As Integer = 0
        Try

            odbConexion.Open()
            strQuery = "INSERT INTO [BECAS_PAGOS_TB] (" & _
                        "[fecha]," &
                        "[estatus]," & _
                        "[fecha_creacion]," & _
                        "[usuario_creacion])" & _
                        " VALUES (CAST( GETDATE() AS DATE) ,'BORRADOR' " & _
                       ",GETDATE(),'" & hdUsuario.Value & "')  Select @@Identity"
            If strSolicitud = "" Then
                Dim odbComando As New OleDbCommand(strQuery, odbConexion)
                iId = CInt(odbComando.ExecuteScalar())
                hdIdPago.Value = iId
            Else
                hdIdPago.Value = strSolicitud
            End If


            odbConexion.Close()
            Call LimpiaControles()
            Call obtCatalogoPartidas()
            Call obtArchivoMenu()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    '
    Private Sub ddlAFavorDe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAFavorDe.SelectedIndexChanged
        Call obtAFavor()
    End Sub

    Private Sub btnCrearPago_ServerClick(sender As Object, e As EventArgs) Handles btnCrearPago.ServerClick

        Call crearBorradorPago()
    End Sub
    ' Crear Pago para Usuario
    Public Sub crearPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim iId As Integer = 0
        Dim arrResultado As New ArrayList
        Try
            arrResultado = obtInformacionSolicitante()
            odbConexion.Open()
            strQuery = " UPDATE [dbo].[BECAS_PAGOS_TB] " & _
               " SET [a_favor] = '" & ddlAFavorDe.SelectedValue & "' " & _
               "    ,[fk_id_empresa] = '" & ddlEmpresa.SelectedValue & "' " & _
               "    ,[fk_id_tipo_proyecto] =  '" & ddlTipoProyecto.SelectedValue & "' " & _
               "    ,[fk_id_afavor] = '" & ddlProvColaborador.SelectedValue & "' " & _
               "    ,[fk_id_empleado_solicita] = '" & arrResultado.Item(0).ToString & "' " & _
               "    ,[puesto_empleado] = '" & arrResultado.Item(1).ToString & "'" & _
               "    ,[gerencia_empleado] = '" & arrResultado.Item(2).ToString & "'" & _
               "    ,[direccion_empleado] = '" & arrResultado.Item(3).ToString & "'" & _
               "    ,[fecha_limite] = '" & txtFechaLimite.Text & "'" & _
               "    ,[moneda] = '" & ddlMoneda.SelectedValue & "' " & _
               "    ,[tipo_cambio] = '" & IIf(txtTipoCambio.Text = "", 0, txtTipoCambio.Text) & "'" & _
               "    ,[motivo] = '" & txtMotivo.Text & "'" & _
               "    ,[no_factura] = '" & IIf(txtNumeroFacturas.Text = "", 0, txtNumeroFacturas.Text) & "'" & _
               "    ,[estatus] = 'CREADO'" & _
               "    ,[fecha_modificacion] = GETDATE()" & _
               "    ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                " WHERE ID=" & IIf(hdIdPago.Value = "", 0, hdIdPago.Value)


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            'Actualiza Pago
            Call ActualizaPagoCurso(odbConexion)
            'CALCULA CORRELATIVO
            Call CargaCorrelativo(odbConexion)
            odbConexion.Close()
            Call obtCatalogoPartidas()
            Call obtPagosRegistrado()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'Actualiza el idPago de los cursos
    Public Sub ActualizaPagoCurso(odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        strQuery = " UPDATE GC_GESTION_CAPACITACION_TB " & _
                   " SET fk_id_pago=" & IIf(hdIdPago.Value = "", 0, hdIdPago.Value) & _
                   "    ,[fecha_modificacion] = GETDATE()" & _
                   "    ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                   " WHERE ID=" & ddlCursoRelacionado.SelectedValue

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbComando.ExecuteNonQuery()
    End Sub
    Public Sub LimpiaControles()
        ddlPagosBuscar.SelectedValue = 0
        ddlAFavorDe.SelectedIndex = 0
        ddlEmpresa.SelectedIndex = 0
        ddlTipoProyecto.SelectedIndex = 0
        txtFechaLimite.Text = ""
        ddlMoneda.SelectedValue = "Pesos"
        txtTipoCambio.Text = 0
        txtTipoCambio.Visible = False
        txtMotivo.Text = ""
        txtNumeroFacturas.Text = ""

    End Sub
    'obtiene los datos de Direccion del Solicitante
    Public Function obtInformacionSolicitante() As ArrayList
        Dim arrResultado As New ArrayList
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try

            odbConexion.Open()

            'obtiene la informacion de los datos Colaborador
            strQuery = "SELECT CLAVE,[PUESTO],[GERENCIA],[DIRAREA] FROM [SGIDO_INFOGIRO_GIRO_VT] " & _
                        " WHERE CLAVE=(SELECT [clave] FROM [SIGIDO_USUARIOS_TB]" & _
                        " WHERE usuario='" & hdUsuario.Value & "')"


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                arrResultado.Add(odbLector(0).ToString.Trim)
                arrResultado.Add(odbLector(1).ToString.Trim)
                arrResultado.Add(odbLector(2).ToString.Trim)
                arrResultado.Add(odbLector(3).ToString.Trim)
                odbLector.Close()
            Else
                arrResultado.Add("")
                arrResultado.Add("")
                arrResultado.Add("")
                arrResultado.Add("")
            End If

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
        Return arrResultado
    End Function

    Private Sub btnGenerarPago_ServerClick(sender As Object, e As EventArgs) Handles btnGenerarPago.ServerClick

        Call crearPago()
    End Sub

    'calcula el numero correlativo
    Public Sub CargaCorrelativo(odbConexion As OleDbConnection)
        Try
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_calcula_correlativo_pagos_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            '   odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)

            odbComando.ExecuteNonQuery()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMoneda.SelectedIndexChanged
        Call comportamientos()
    End Sub
    Public Sub obtInfoPagos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""

        Try
            odbConexion.Open()
            'registra año
            strQuery = "SELECT * FROM [BECAS_PAGOS_TB] WHERE ID=" & ddlPagosBuscar.SelectedValue & " ORDER BY 1 "


            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                ddlAFavorDe.SelectedValue = odbLector(1).ToString
                Call obtAFavor()
                Call ObtCursoProveedor()
                ddlEmpresa.SelectedValue = odbLector(4).ToString
                ddlTipoProyecto.SelectedValue = odbLector(5).ToString
                ddlProvColaborador.SelectedValue = odbLector(6).ToString
                ddlSolicitaPago.SelectedValue = odbLector(7).ToString
                txtFechaLimite.Text = odbLector(12).ToString
                ddlMoneda.SelectedValue = odbLector(13).ToString
                txtTipoCambio.Text = odbLector(14).ToString
                txtMotivo.Text = odbLector(15).ToString
                txtNumeroFacturas.Text = odbLector(16).ToString
                ddlCursoRelacionado.SelectedValue = ObtIdCursoRelacionado(odbConexion)
                odbLector.Close()
            End If
            odbConexion.Close()

            Call comportamientos()
            'OCULTA BOTONNES DE aGREGAR
            btnGenerarPago.Visible = False
            btnCancelar.Visible = False
            grdPartidas.Columns(7).Visible = False
            grdPartidas.Columns(8).Visible = False
            Call obtCatalogoPartidas()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Public Function ObtIdCursoRelacionado(odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim strResultado As String = "0"
        strQuery = "SELECT top 1 id FROM [GC_GESTION_CAPACITACION_TB] WHERE fk_id_pago=" & ddlPagosBuscar.SelectedValue & "  "
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString
            odbLector.Close()
        End If
        Return strResultado
    End Function
    Private Sub ddlPagosBuscar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPagosBuscar.SelectedIndexChanged
        hdCrearPago.Value = ddlPagosBuscar.SelectedValue
        hdIdPago.Value = ddlPagosBuscar.SelectedValue
        Call obtInfoPagos()
        Call obtArchivoMenu()
    End Sub

#End Region
#Region "Partidas"
    ''obtiene el catalogo 
    Public Sub obtCatalogoPartidas()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strFiltro As String = ""

        Try

            odbConexion.Open()
            strQuery = "SELECT * FROM [BECAS_PAGOS_DETALLE_TB] WHERE fk_id_pago=" & IIf(hdIdPago.Value = "", 0, hdIdPago.Value) & " ORDER BY partida "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPartidas.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPartidas.DataBind()

            If grdPartidas.Rows.Count = 0 Then
                Call insFilaVaciaPartida()
                grdPartidas.Rows(0).Visible = False
                hdPartida.Value = 0
            Else
                grdPartidas.Rows(0).Visible = True
                hdPartida.Value = grdPartidas.Rows.Count
            End If

            'obtiene si las correcciones se cumplieron
            odbConexion.Close()

            Dim i As Int16 = 0

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdPartidas.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPartidas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            Call obtArchivoMenu()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    ''inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaPartida()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("partida"))
        dt.Columns.Add(New DataColumn("cuenta"))
        dt.Columns.Add(New DataColumn("concepto"))
        dt.Columns.Add(New DataColumn("centro_costo"))
        dt.Columns.Add(New DataColumn("iva"))
        dt.Columns.Add(New DataColumn("importe"))



        dr = dt.NewRow
        dr("id") = ""
        dr("partida") = ""
        dr("cuenta") = ""
        dr("centro_costo") = ""
        dr("concepto") = ""
        dr("iva") = ""
        dr("importe") = ""



        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPartidas.DataSource = dt.DefaultView
        grdPartidas.DataBind()


    End Sub
    'inserta registro al catalogo de materiales
    Public Sub insPatidasPago()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strPatida As String = ""
        Dim strCuenta As String = ""
        Dim strCentroCosto As String = ""
        Dim strImporte As String = ""
        Dim strConcepto As String = ""
        Dim strIva As String = ""
        Dim strQuery As String = ""
        Try
            strPatida = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgrePartida"), TextBox).Text)
            strCuenta = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgregaCuenta"), TextBox).Text)
            strCentroCosto = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgreCentroCosto"), TextBox).Text)
            strImporte = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgregarImporte"), TextBox).Text)
            strConcepto = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgreConcepto"), TextBox).Text)
            strIva = (DirectCast(grdPartidas.FooterRow.FindControl("txtAgregarIva"), TextBox).Text)

            odbConexion.Open()
            'validaciones para insertar registros
            If strPatida = "" Or IsNumeric(strPatida) = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar Partida en valor Númerico.');</script>", False)
                Exit Sub
            End If
            If strCuenta = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Cuenta.');</script>", False)
                Exit Sub
            End If

            If strConcepto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Concepto.');</script>", False)
                Exit Sub
            End If
            If strCentroCosto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Centro de Costo.');</script>", False)
                Exit Sub
            End If

            If strIva = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el IVA.');</script>", False)
                Exit Sub
            End If

            If strImporte = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Importe.');</script>", False)
                Exit Sub
            End If



            strQuery = "INSERT INTO [dbo].[BECAS_PAGOS_DETALLE_TB]" & _
                       " ([fk_id_pago]" & _
                       " ,[partida]" & _
                       " ,[cuenta]" & _
                       " ,[concepto]" & _
                       " ,[centro_costo]" & _
                       " ,[iva]" & _
                       " ,[importe]" & _
                       " ,[fecha_creacion]    " & _
                       " ,[usuario_creacion]) " & _
                       "  VALUES              " & _
                       " ('" & hdIdPago.Value & "' " & _
                       " ,'" & strPatida & "' " & _
                       " ,'" & strCuenta & "' " & _
                       " ,'" & strConcepto & "' " & _
                       " ,'" & strCentroCosto & "' " & _
                       " ,'" & IIf(IsNumeric(strIva), strIva, 0) & "' " & _
                       " ,'" & IIf(IsNumeric(strImporte), strImporte, 0) & "' " & _
                       " ,GETDATE()" & _
                       " ,'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()
            Call obtCatalogoPartidas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdPartidas_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPartidas.RowCancelingEdit
        grdPartidas.ShowFooter = True
        grdPartidas.EditIndex = -1
        Call obtCatalogoPartidas()
    End Sub
    'TOOLTIPS
    Private Sub grdPartidas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartidas.RowDataBound

        For i As Integer = 0 To grdPartidas.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPartidas.Rows(i).Controls(7).Controls(0)
            Dim btnEliminar As LinkButton = grdPartidas.Rows(i).Controls(8).Controls(1)

            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Acción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar  " + DirectCast(grdPartidas.Rows(i).Controls(1).Controls(1), Label).Text + "?')){ return false; };"

            Else
                btnEditar.ToolTip = "Actualizar Acción"
                Dim cancelar As LinkButton = grdPartidas.Rows(i).Controls(7).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar  " + DirectCast(grdPartidas.Rows(i).Controls(1).Controls(1), TextBox).Text + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingOtnp').style.display = 'inline'")
            Next
        End If
    End Sub
    'habilita el modo edicion
    Private Sub grdPartidas_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPartidas.RowEditing
        grdPartidas.ShowFooter = False
        grdPartidas.EditIndex = e.NewEditIndex
        Call obtCatalogoPartidas()
    End Sub
    'actualiza la descripcion
    Private Sub grdPartidas_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPartidas.RowUpdating
        grdPartidas.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strId As String = ""
        Dim strPatida As String = ""
        Dim strCuenta As String = ""
        Dim strCentroCosto As String = ""
        Dim strImporte As String = ""
        Dim strConcepto As String = ""
        Dim strIva As String = ""
        Dim strQuery As String = ""
        Try
            odbConexion.Open()
            strId = DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strPatida = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtPartida"), TextBox).Text)
            strCuenta = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtCuenta"), TextBox).Text)
            strCentroCosto = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtCentroCosto"), TextBox).Text)
            strImporte = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtImporte"), TextBox).Text)
            strConcepto = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtConcepto"), TextBox).Text)
            strIva = (DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("txtIva"), TextBox).Text)

            If strPatida = "" Or IsNumeric(strPatida) = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar Partida en valor Númerico.');</script>", False)
                Exit Sub
            End If
            If strCuenta = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Cuenta.');</script>", False)
                Exit Sub
            End If

            If strConcepto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Concepto.');</script>", False)
                Exit Sub
            End If
            If strCentroCosto = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Centro de Costo.');</script>", False)
                Exit Sub
            End If
            If strIva = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el IVA.');</script>", False)
                Exit Sub
            End If

            If strImporte = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el Importe.');</script>", False)
                Exit Sub
            End If


            'VALIDAR SI EL ROL ES ESTANDAR

            strQuery = " UPDATE [dbo].[BECAS_PAGOS_DETALLE_TB]" & _
                       " SET [partida] =  '" & strPatida & "' " & _
                       "   ,[cuenta] = '" & strCuenta & "'" & _
                       "   ,[concepto] = '" & strConcepto & "'" & _
                       "   ,[centro_costo] = '" & strCentroCosto & "' " & _
                       "   ,[iva] = " & IIf(IsNumeric(strIva), strIva, 0) & " " & _
                       "   ,[importe] = " & IIf(IsNumeric(strImporte), strImporte, 0) & " " & _
                       " ,fecha_modificacion=GETDATE()" & _
                       " ,usuario_modificacion='" & hdUsuario.Value & "'" & _
                       " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            odbConexion.Close()

            grdPartidas.EditIndex = -1
            Call obtCatalogoPartidas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub grdPartidas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPartidas.PageIndexChanging
        Try
            grdPartidas.ShowFooter = True
            grdPartidas.EditIndex = -1
            grdPartidas.PageIndex = e.NewPageIndex
            grdPartidas.DataBind()
            Call obtCatalogoPartidas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregaPartidas_Click(sender As Object, e As EventArgs)
        Call insPatidasPago()
    End Sub
    Private Sub grdPartidas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPartidas.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPartidas.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE FROM [BECAS_PAGOS_DETALLE_TB] WHERE ID=" & strId
            'valida en Facilitadores


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            'calcula correlativo


            odbConexion.Close()

            grdPartidas.EditIndex = -1
            grdPartidas.ShowFooter = True
            Call obtCatalogoPartidas()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


#End Region

#Region "Impresion Pagos"
    Public Sub obtPagosRegistrado()
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Try

            odbConexion.Open()
            Dim strExtencion As String = ""
            'obtiene la informacion de las cartas configuradas al siguiente fecha de calendario
            strQuery = "SELECT * FROM [BECAS_PAGOS_VT]  ORDER BY NO_CORRELATIVO DESC"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPagosRegistrados.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPagosRegistrados.DataBind()
            If grdPagosRegistrados.Rows.Count = 0 Then
                divCartasCredito.Visible = False
                lblRegistrosCarta.Text = "No hay Pagos Registrados"
                btnImprimir.Visible = False
            Else
                divCartasCredito.Visible = True
                lblRegistrosCarta.Text = ""
                btnImprimir.Visible = True
            End If


            For iFil As Integer = 0 To grdPagosRegistrados.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPagosRegistrados.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            Call obtPagosBuscar()
            odbConexion.Close()
        Catch ex As Exception
            lblErrorPagos.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Private Sub grdPagosRegistrados_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPagosRegistrados.RowDataBound

        For i As Integer = 0 To grdPagosRegistrados.Rows.Count - 1
            Dim strId As String
            strId = DirectCast(grdPagosRegistrados.Rows(i).FindControl("lblId"), Label).Text

            Dim btnImprimir As LinkButton = grdPagosRegistrados.Rows(i).Controls(1).Controls(0)

            btnImprimir.Attributes.Add("href", "print_pago.aspx?idPago=" & strId & "")
            btnImprimir.Attributes.Add("target", "_blank")
            btnImprimir.ToolTip = "Imprimir Pago"

            Dim btnEliminar As LinkButton = grdPagosRegistrados.Rows(i).Controls(10).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el Pago" + grdPagosRegistrados.Rows(i).Cells(3).Text + "?')){ return false; };"

        Next
    End Sub


    Private Sub grdPagosRegistrados_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPagosRegistrados.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPagosRegistrados.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            Call ActualizaPagoCursoEliminado(odbConexion, strId)

            strQuery = " DELETE BECAS_PAGOS_TB " & _
                       " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            'actualiza informacion cargada en los grids
            Call obtPagosRegistrado()
        Catch ex As Exception
            lblErrorPagos.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    'Actualiza el idPago de los cursos
    Public Sub ActualizaPagoCursoEliminado(odbConexion As OleDbConnection, strIdPago As String)
        Dim strQuery As String = ""
        strQuery = " UPDATE GC_GESTION_CAPACITACION_TB " & _
                   " SET fk_id_pago=0" & _
                   "    ,[fecha_modificacion] = GETDATE()" & _
                   "    ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                   " WHERE fk_id_pago=" & strIdPago

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbComando.ExecuteNonQuery()
    End Sub
    Private Sub btnImprimir_ServerClick(sender As Object, e As EventArgs) Handles btnImprimir.ServerClick
        Dim iContador As Integer = 0
        Dim strJavaScript As String = ""
        For Each row As GridViewRow In grdPagosRegistrados.Rows
            Dim chkcheck As CheckBox = DirectCast(row.FindControl("chkImprimir"), CheckBox)
            Dim strId As String = DirectCast(row.FindControl("lblId"), Label).Text
            If chkcheck.Checked Then

                strJavaScript += "imprimePagos(" & strId & "); "
                iContador += 1
            End If
        Next

        If iContador > 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Impresion", "<script>" & strJavaScript & "</script>", False)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar al menos un Pago para la Impresión por Lotes.');</script>", False)
            Exit Sub
        End If

    End Sub

#End Region
#Region "PDF Facturas"

    Private Sub btnCargaArchivos_ServerClick(sender As Object, e As EventArgs) Handles btnCargaArchivos.ServerClick
        Call CargaArchivosMenu()
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
                            strDirectorio = "\FacturasPagos\Pago_" & hdIdPago.Value.ToString & "\"

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
            '     Call actMenuObsoleto()
            strQuery = "INSERT INTO [dbo].[BECAS_FACTURAS_PAGOS_PDF_TB]" & _
                       "  VALUES " & _
                       "  (" & hdIdPago.Value & "" & _
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

            'Carga Archivo de Menu
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
            strQuery = "UPDATE [dbo].[BECAS_FACTURAS_PAGOS_PDF_TB]" & _
                       " SET [estatus] = 'OBSOLETO'" & _
                       "      ,[fecha_modificacion] = GETDATE()" & _
                       "      ,[usuario_modificacion] = '" & hdUsuario.Value & "'" & _
                       "WHERE fk_id_pago=" & hdIdPago.Value

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

            strQuery = "SELECT * " & _
               " FROM BECAS_FACTURAS_PAGOS_PDF_TB WHERE estatus='VIGENTE' and fk_id_pago=" & hdIdPago.Value

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsArchivos)

            grdArchivos.DataSource = dsArchivos.Tables(0).DefaultView
            grdArchivos.DataBind()
            If grdArchivos.Rows.Count = 0 Then

                grdArchivos.Visible = False
            Else

                grdArchivos.Visible = True
            End If

            'colorea las celdas del grid
            For iFil As Integer = 0 To grdArchivos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdArchivos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            odbConexion.Close()
        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Private Sub grdArchivos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdArchivos.RowDataBound
        For i As Integer = 0 To grdArchivos.Rows.Count - 1
            Dim btnVer As LinkButton = grdArchivos.Rows(i).Controls(1).Controls(0)
            Dim strRuta As String = ""
            strRuta = DirectCast(grdArchivos.Rows(i).FindControl("lblRuta"), Label).Text
            btnVer.Attributes.Add("href", strRuta)
            btnVer.Attributes.Add("target", "_blank")
            btnVer.ToolTip = "Ver Factura"
            Dim btnEliminar As LinkButton = grdArchivos.Rows(i).Controls(3).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el archivo " + grdArchivos.Rows(i).Cells(2).Text + "?')){ return false; };"
        Next
    End Sub
    Private Sub grdArchivos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdArchivos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strRuta As String = ""
        Try
            strId = DirectCast(grdArchivos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "DELETE BECAS_FACTURAS_PAGOS_PDF_TB WHERE ID=" & strId
            'valida en Facilitadores



            Dim path As String = Server.MapPath("~/UploadedFiles/")
            'calcula correlativo
            strRuta = Server.MapPath("~/UploadedFiles/") & obtRutaArchivo(strId, odbConexion)
            'crea directorio
            If (File.Exists(strRuta)) Then
                File.Delete(strRuta)
            End If
            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            Call obtArchivoMenu()
        Catch ex As Exception
            lblErrorArchivo.Text = ex.Message
        End Try
    End Sub
    Public Function obtRutaArchivo(strId As String, odbConexion As OleDbConnection)
        Dim strQuery As String = ""
        Dim strResultado As String = ""
        Try

            strQuery = "SELECT [ruta] " & _
               " FROM [BECAS_FACTURAS_PAGOS_PDF_TB] WHERE id=" & strId
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader
            If odbLector.HasRows Then
                odbLector.Read()
                strResultado = odbLector(0).ToString.Replace("UploadedFiles\", "")
                odbLector.Close()
            End If

        Catch ex As Exception
            lblErrorArchivo.Text = Err.Number & " " & ex.Message
        End Try
        Return strResultado
    End Function

#End Region
#Region "Excel"
    Public Sub obtExportarExcel(dsDatos As DataSet)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try

            Dim filename As String = "rpt_pagos_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".xls"
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()

            dgGrid.DataSource = dsDatos.Tables(0).DefaultView
            dgGrid.DataBind()


            'Get the HTML for the control.
            dgGrid.RenderControl(hw)
            'Write the HTML back to the browser.
            'Response.ContentType = application/vnd.ms-excel;
            Response.ContentType = "application/vnd.ms-excel"
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename & "")
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble())
            Me.EnableViewState = False
            Response.Write(tw.ToString())
            Response.[End]()

            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub btnExportarReporte_ServerClick(sender As Object, e As EventArgs) Handles btnExportarReporte.ServerClick
        Call exportExcel()
    End Sub
    Public Sub exportExcel(Optional ByVal strBuscar As String = "")
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim dsDatos As New DataSet
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "SELECT * FROM BECAS_PAGOS_VT ORDER BY NO_CORRELATIVO DESC"
            odbComando.Connection = odbConexion

            'parametros


            Dim odbAdaptdor As New OleDbDataAdapter
            odbAdaptdor.SelectCommand = odbComando

            odbAdaptdor.Fill(dsDatos)

            Call obtExportarExcel(dsDatos)
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