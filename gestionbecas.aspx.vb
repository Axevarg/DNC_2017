Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class gestionbecas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorCalificacionIngles.Text = ""
        lblErrorPago.Text = ""
        lblErrorGestion.Text = ""
        lblErrorCalificacion.Text = ""
        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            Call obtenerUsuarioAD()
            Call CargaColaboradores()
            Call CargaCatalogos()
            Call ObtDatosColaboradores()
        End If
        Call comportamiento()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "list", "<script>comportamientosJS()</script>", False)
    End Sub
#Region "Catalogos Cartas de Crédito"

    Private Sub ObtHorarioEstudio(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_INGLES_HORARIO_ESTUDIO_CT WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlHorarioEstudio.DataSource = odbLector
        ddlHorarioEstudio.DataValueField = "ID"
        ddlHorarioEstudio.DataTextField = "descripcion"

        ddlHorarioEstudio.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ObtPlantel(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion  FROM BECAS_INGLES_PLANTEL_CT WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlPlantel.DataSource = odbLector
        ddlPlantel.DataValueField = "ID"
        ddlPlantel.DataTextField = "descripcion"

        ddlPlantel.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ObtNivelInicial(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = " SELECT 0 AS ID,' Seleccionar' AS descripcion,0 AS ORDEN UNION ALL " & _
                                 " SELECT ID,descripcion,ORDEN  FROM BECAS_INGLES_NIVELES_CT WHERE ESTATUS=1 ORDER BY ORDEN"

        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlNivelInicial.DataSource = odbLector
        ddlNivelInicial.DataValueField = "ID"
        ddlNivelInicial.DataTextField = "descripcion"

        ddlNivelInicial.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub ObtTipoAutorizacion(ByVal odbConexion As OleDbConnection)

        Dim strQuery As String = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion  FROM BECAS_INGLES_TIPO_AUTORIZACION_CT WHERE ESTATUS=1 ORDER BY 2"


        Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlTipoAutorizacion.DataSource = odbLector
        ddlTipoAutorizacion.DataValueField = "ID"
        ddlTipoAutorizacion.DataTextField = "descripcion"

        ddlTipoAutorizacion.DataBind()
        'valida si los item estan vacios

    End Sub

    Public Sub CargaCatalogos()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Call ObtHorarioEstudio(odbConexion)
            Call ObtPlantel(odbConexion)
            Call ObtNivelInicial(odbConexion)
            Call ObtTipoAutorizacion(odbConexion)

            odbConexion.Close()
        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try

    End Sub


    'Private Sub btnGuardaCalificacionesIngles_ServerClick(sender As Object, e As EventArgs) Handles btnGuardaCalificacionesIngles.ServerClick
    '    'valida que evento ejecutar
    '    If hdIdComportamientosBecaIngles.Value = 0 Then
    '        Call ActulizaBecaIngles()
    '    Else
    '        Call guardaCalificacionesIngles()
    '    End If

    'End Sub


    'evento de eliminación de cartas de credito
    Protected Sub lnkElminaCartas_Click(sender As Object, e As EventArgs)


        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try

            odbConexion.Open()

            strQuery = "DELETE FROM [dbo].[BECAS_INGLES_CARTAS_CREDITO_TB] " & _
                       "WHERE ID=" & hdidCartasCredito.Value


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            Call obtBecas(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub
#End Region
#Region "Administración de Becas"
    Public Sub obtBecas(ByVal strColaborador As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strFiltro As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_muestra_lista_becas_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCatalogo)
            grdBecas.DataSource = dsCatalogo.Tables(0).DefaultView
            grdBecas.DataBind()
            If grdBecas.Rows.Count = 0 Then
                Call insFilaVaciaBecas()
                grdBecas.Rows(0).Visible = False
                lblGrid.Text = ""

            Else
                grdBecas.Rows(0).Visible = True
                lblGrid.Text = ""
            End If
            For i = 0 To grdBecas.Rows.Count - 1
                Dim btnEditar As LinkButton = grdBecas.Rows(i).Controls(16).Controls(0)
                Dim btnEliminar As LinkButton = grdBecas.Rows(i).Controls(17).Controls(1)
                Dim ddlTipoBeca As DropDownList

                Dim ddlPeriodoAsignacion As DropDownList
                Dim ddlTipoAsignacion As DropDownList
                Dim ddlModalidadEstudio As DropDownList
                Dim ddlTipoProyecto As DropDownList
                Dim ddlTipoEstus As DropDownList
                Dim ddlEstatusPago As DropDownList
                Dim ddlProveedor As DropDownList
                Dim strNombreBecas As String = ""
                Dim iIdBecas As String
                iIdBecas = DirectCast(grdBecas.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlTipoBeca = grdBecas.Rows(i).Cells(2).FindControl("ddlTipoBeca")

                    ddlPeriodoAsignacion = grdBecas.Rows(i).Cells(4).FindControl("ddlPeriodoAsignacion")
                    ddlTipoAsignacion = grdBecas.Rows(i).Cells(5).FindControl("ddlTipoAsignacion")
                    ddlModalidadEstudio = grdBecas.Rows(i).Cells(6).FindControl("ddlModalidadEstudio")
                    ddlTipoProyecto = grdBecas.Rows(i).Cells(7).FindControl("ddlTipoProyecto")
                    ddlTipoEstus = grdBecas.Rows(i).Cells(8).FindControl("ddlTipoEstus")
                    ddlEstatusPago = grdBecas.Rows(i).Cells(9).FindControl("ddlEstatusPago")
                    ddlProveedor = grdBecas.Rows(i).Cells(10).FindControl("ddlProveedor")
                    'Tipo Beca
                    Call obtddlTipoBecas(ddlTipoBeca)

                    'Periodo Asignacion
                    Call obtddlPeriodoAsignacion(ddlPeriodoAsignacion)
                    'Tipo Asignacion
                    Call obtddlTipoAsignacion(ddlTipoAsignacion)
                    'Modalidad Estudios
                    Call obtddlModalidadEstudio(ddlModalidadEstudio)
                    'Tipo Proyecto
                    Call obtddlTipoProyecto(ddlTipoProyecto)
                    'Tipo Estatus
                    Call obtddlTipoEstatus(ddlTipoEstus)

                    'Estatus Pago
                    Call obtddlEstatusPago(ddlEstatusPago)
                    'proveedor
                    Call obtddlProveedor(ddlProveedor)


                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdBecas Then

                            ddlTipoBeca.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(3).ToString
                            ddlPeriodoAsignacion.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(5).ToString
                            ddlTipoAsignacion.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                            ddlModalidadEstudio.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlTipoProyecto.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(8).ToString
                            ddlTipoEstus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(9).ToString
                            ddlEstatusPago.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(10).ToString
                            ddlProveedor.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(11).ToString
                        End If
                    Next
                    strNombreBecas = ddlTipoBeca.SelectedItem.Text
                    ddlTipoBeca.Attributes.Add("disabled", "disabled")
                Else

                    'Tipo Beca
                    Dim lblTipoBeca As New Label
                    lblTipoBeca = grdBecas.Rows(i).FindControl("lblTipoBeca")
                    lblTipoBeca.Text = obtTextoTipoBeca(lblTipoBeca.Text)

                    Dim btnEdicion As LinkButton

                    btnEdicion = grdBecas.Rows(i).Controls(1).Controls(0)
                    'modal


                    If lblTipoBeca.Text = "Ingles" Then
                        btnEdicion.Attributes.Add("onclick", " idBecasIngles(" & iIdBecas & ")")
                        btnEdicion.ToolTip = "Configuración Cartas de Créditos"
                    Else
                        btnEdicion.ToolTip = "Configuración de Calificaciones"
                        btnEdicion.Attributes.Add("onclick", " idBeca(" & iIdBecas & ")")
                    End If


                    'Periodo Asignacion
                    Dim lblPeriodoAsignacion As New Label
                    lblPeriodoAsignacion = grdBecas.Rows(i).FindControl("lblPeriodoAsignacion")
                    lblPeriodoAsignacion.Text = obtTextoPeriodoAsignacion(lblPeriodoAsignacion.Text)
                    'Tipo Asignacion
                    Dim lblTipoAsignacion As New Label
                    lblTipoAsignacion = grdBecas.Rows(i).FindControl("lblTipoAsignacion")
                    lblTipoAsignacion.Text = obtTextoTipoAsignacion(lblTipoAsignacion.Text)
                    'Modalidad Estudios
                    Dim lblModalidadEstudio As New Label
                    lblModalidadEstudio = grdBecas.Rows(i).FindControl("lblModalidadEstudio")
                    lblModalidadEstudio.Text = obtTextoModalidaEstudio(lblModalidadEstudio.Text)
                    'Tipo Proyecto
                    Dim lblTipoProyecto As New Label
                    lblTipoProyecto = grdBecas.Rows(i).FindControl("lblTipoProyecto")
                    lblTipoProyecto.Text = obtTextoTipoProyecto(lblTipoProyecto.Text)
                    'Tipo Estatus
                    Dim lblTipoEstus As New Label
                    lblTipoEstus = grdBecas.Rows(i).FindControl("lblTipoEstus")
                    lblTipoEstus.Text = obtTextoTipoEstatus(lblTipoEstus.Text)
                    'Estatus Pago
                    Dim lblEstusPago As New Label
                    lblEstusPago = grdBecas.Rows(i).FindControl("lblEstusPago")
                    lblEstusPago.Text = obtTextoEstatusPago(lblEstusPago.Text)

                    'Proveedor
                    Dim lblProveedor As New Label
                    lblProveedor = grdBecas.Rows(i).FindControl("lblProveedor")
                    lblProveedor.Text = obtTextoProveedor(lblProveedor.Text)


                    strNombreBecas = lblTipoBeca.Text


                End If
                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + strNombreBecas + "?')){ return false; } else { document.getElementById('loadingPagina').style.display = 'inline'};"
            Next

            'Tipo Beca
            Dim ddlAgreTipoBeca As DropDownList
            ddlAgreTipoBeca = grdBecas.FooterRow.FindControl("ddlAgreTipoBeca")
            Call obtddlTipoBecas(ddlAgreTipoBeca)
            ''Año Asignacion
            'Dim ddlAgreAnioAsignacion As DropDownList
            'ddlAgreAnioAsignacion = grdBecas.FooterRow.FindControl("ddlAgreAnioAsignacion")
            'Call obtddlAnio(ddlAgreAnioAsignacion)

            'Periodo Asignacion
            Dim ddlAgrePeriodoAsignacion As DropDownList
            ddlAgrePeriodoAsignacion = grdBecas.FooterRow.FindControl("ddlAgrePeriodoAsignacion")
            Call obtddlPeriodoAsignacion(ddlAgrePeriodoAsignacion)
            'Tipo Asignacion
            Dim ddlAgreTipoAsignacion As DropDownList
            ddlAgreTipoAsignacion = grdBecas.FooterRow.FindControl("ddlAgreTipoAsignacion")
            Call obtddlTipoAsignacion(ddlAgreTipoAsignacion)
            'Modalidad Estudios
            Dim ddlAgreModalidadEstudio As DropDownList
            ddlAgreModalidadEstudio = grdBecas.FooterRow.FindControl("ddlAgreModalidadEstudio")
            Call obtddlModalidadEstudio(ddlAgreModalidadEstudio)
            'Tipo Proyecto
            Dim ddlAgreTipoProyecto As DropDownList
            ddlAgreTipoProyecto = grdBecas.FooterRow.FindControl("ddlAgreTipoProyecto")
            Call obtddlTipoProyecto(ddlAgreTipoProyecto)

            'Tipo Estatus
            Dim ddlAgreTipoEstus As DropDownList
            ddlAgreTipoEstus = grdBecas.FooterRow.FindControl("ddlAgreTipoEstus")
            Call obtddlTipoEstatus(ddlAgreTipoEstus)

            'Estatus Pago
            Dim ddlAgreEstusPago As DropDownList
            ddlAgreEstusPago = grdBecas.FooterRow.FindControl("ddlAgreEstusPago")
            Call obtddlEstatusPago(ddlAgreEstusPago)

            'Proveedor
            Dim ddlAgreProveedor As DropDownList
            ddlAgreProveedor = grdBecas.FooterRow.FindControl("ddlAgreProveedor")
            Call obtddlProveedor(ddlAgreProveedor)

            For iFil As Integer = 0 To grdBecas.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdBecas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
            'obtien los pagos.


        Catch ex As Exception
            lblError.Text = ex.Message
        End Try


    End Sub
    '********************- Tipo Becas -******************************
    'Obtiene el catalogo de los Tipos de Becas
    Public Sub obtddlTipoBecas(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_TIPO_BECA_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoTipoBeca(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_TIPO_BECA_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Año -******************************
    'Obtiene el catalogo de los Años
    Public Sub obtddlAnio(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT anio,CAST(anio AS varchar) FROM BECAS_ANIOS_CT  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    '********************- Periodo de Asignación -******************************
    'Obtiene el catalogo del Periodo de Asignación
    Public Sub obtddlPeriodoAsignacion(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_PERIODO_ASIGNACION_BECA_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoPeriodoAsignacion(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_PERIODO_ASIGNACION_BECA_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    '********************- Tipo Asignacion -******************************
    'Obtiene el catalogo de los Tipos de Asignacion
    Public Sub obtddlTipoAsignacion(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_TIPO_ASIGNACION_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoTipoAsignacion(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_TIPO_ASIGNACION_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Modalidad de Estudios -******************************
    'Obtiene el catalogo de los Modalidad de Estudios
    Public Sub obtddlModalidadEstudio(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_MODALIDAD_ESTUDIOS_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoModalidaEstudio(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_MODALIDAD_ESTUDIOS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    '********************- Tipo Proyecto -******************************
    'Obtiene el catalogo de los Tipos de Proyecto
    Public Sub obtddlTipoProyecto(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_TIPO_PROYECTOS_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoTipoProyecto(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_TIPO_PROYECTOS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Tipo Estatus -******************************
    'Obtiene el catalogo de los Tipos de Estatus
    Public Sub obtddlTipoEstatus(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_TIPO_ESTATUS_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoTipoEstatus(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_TIPO_ESTATUS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Estatus Pago -******************************
    'Obtiene el catalogo de los Estatus Pagos
    Public Sub obtddlEstatusPago(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM BECAS_ESTATUS_PAGO_CT   WHERE ESTATUS=1  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoEstatusPago(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM BECAS_ESTATUS_PAGO_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Proveedor -******************************
    'Obtiene el catalogo de los Tipos de Asignacion
    Public Sub obtddlProveedor(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,nombre FROM SIGIDO_PROVEEDORES_CT where fk_id_servicios_proveedor like '%2%'  ORDER BY 2"
        odbConexion.Open()

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoProveedor(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT nombre FROM SIGIDO_PROVEEDORES_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    'funcion de caractes a javascript
    Public Function remplazaAcentosJS(strTexto As String) As String
        Dim strTextoRes As String = strTexto
        strTextoRes = strTextoRes.Replace("Ñ", "N")
        strTextoRes = strTextoRes.Replace("ñ", "n;")
        strTextoRes = strTextoRes.Replace("Á", "A")
        strTextoRes = strTextoRes.Replace("á", "a")
        strTextoRes = strTextoRes.Replace("É", "E")
        strTextoRes = strTextoRes.Replace("é", "e")
        strTextoRes = strTextoRes.Replace("Í", "I")
        strTextoRes = strTextoRes.Replace("í", "i")
        strTextoRes = strTextoRes.Replace("Ó", "O")
        strTextoRes = strTextoRes.Replace("ó", "o")
        strTextoRes = strTextoRes.Replace("Ú", "U")
        strTextoRes = strTextoRes.Replace("ú", "u")


        Return strTextoRes
    End Function
    'valida que no se puedan tener mas de dos becas Activas
    Public Function validaBecasActivas(odbConexion As OleDbConnection, ByVal modoEdicion As Integer, ByVal idBecas As Integer)
        Dim blResultado As Boolean = False
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""
        'CONSULTA QUE CUENTA CUANTAS BECAS TIENE ACTIVAS
        If modoEdicion = 0 Then
            strQuery = "SELECT COUNT(*) FROM BECAS_GESTION_BECAS_TB WHERE estatus<>'ELIMINADO' AND fk_id_colaborador=" & ddlColaborador.SelectedValue & " AND fk_id_tipo_estatus=1"
        Else
            strQuery = "SELECT COUNT(*) FROM BECAS_GESTION_BECAS_TB WHERE estatus<>'ELIMINADO' AND fk_id_colaborador=" & ddlColaborador.SelectedValue & _
                       " AND fk_id_tipo_estatus=1 AND ID<>" & idBecas.ToString
        End If


        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            blResultado = IIf(odbLector(0) >= 1, True, False)
            odbLector.Close()
        End If

        Return blResultado
    End Function
    'obtiene los meses de configurados en la tabla de parametrizacion
    Public Function obtMesesConfiguradosBeca(odbConexion As OleDbConnection, iTipoBeca As Integer)
        Dim strResultado As String = "0"
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""
        strQuery = "SELECT MESES FROM BECAS_MONTOS_DURACION_CT WHERE fk_id_tipo_beca=" & iTipoBeca.ToString

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
    'inserta registro de las Becas
    Public Sub insBecas()

        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strBeca As String = ""
        Dim strFechaAsignacion As String = ""
        Dim strPeriodoAsignacion As String = ""
        Dim strTipoAsignacion As String = ""
        Dim strModalidadEstudios As String = ""
        Dim strTipoProyecto As String = ""
        Dim strTipoEstatus As String = ""
        Dim strEstatusPago As String = ""
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strProveedores As String = ""
        Dim strObservaciones As String = ""
        Dim strFechaBajaTemp As String = ""
        Try
            odbConexion.Open()
            strBeca = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreTipoBeca"), DropDownList).Text)
            strFechaAsignacion = (DirectCast(grdBecas.FooterRow.FindControl("txtAgreFechaAsignacion"), TextBox).Text)
            strPeriodoAsignacion = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgrePeriodoAsignacion"), DropDownList).Text)
            strTipoAsignacion = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreTipoAsignacion"), DropDownList).Text)
            strModalidadEstudios = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreModalidadEstudio"), DropDownList).Text)
            strTipoProyecto = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreTipoProyecto"), DropDownList).Text)
            strTipoEstatus = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreTipoEstus"), DropDownList).Text)
            strEstatusPago = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreEstusPago"), DropDownList).Text)
            strFechaInicio = (DirectCast(grdBecas.FooterRow.FindControl("txtAgreFechaInicio"), TextBox).Text)
            strFechaFinal = "(SELECT DATEADD(month, " & obtMesesConfiguradosBeca(odbConexion, strBeca) & ", '" & strFechaInicio & "'))" ' AUMENTAN LOS MESES INDICADOS EN LA TABLA DE PARAMETRIZACION
            strProveedores = (DirectCast(grdBecas.FooterRow.FindControl("ddlAgreProveedor"), DropDownList).Text)
            strObservaciones = (DirectCast(grdBecas.FooterRow.FindControl("txtAgreObservaciones"), TextBox).Text)
            strFechaBajaTemp = (DirectCast(grdBecas.FooterRow.FindControl("txtAgreFechaBajaTem"), TextBox).Text)

            'validaciones para insertar registros

            If strBeca = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If strFechaAsignacion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Asignación.');</script>", False)
                Exit Sub
            End If
            If strPeriodoAsignacion = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el Periodo de Asignación.');</script>", False)
                Exit Sub
            End If
            If strTipoAsignacion = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Asignación.');</script>", False)
                Exit Sub
            End If
            If strModalidadEstudios = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar la modalidad de estudio.');</script>", False)
                Exit Sub
            End If
            If strTipoProyecto = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Proyecto.');</script>", False)
                Exit Sub
            End If
            If strTipoEstatus = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Estatus.');</script>", False)
                Exit Sub
            End If

            If strEstatusPago = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el estatus de Pago.');</script>", False)
                Exit Sub
            End If

            If strProveedores = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el Proveedor de la Beca.');</script>", False)
                Exit Sub
            End If
            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio.');</script>", False)
                Exit Sub
            End If
            'Estatus Beca Activo
            If strTipoEstatus = "1" Then
                'valida si tiene >= 1 Beca activa
                If validaBecasActivas(odbConexion, 0, 0) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede tener mas de una Beca Activa.');</script>", False)
                    Exit Sub
                End If
            End If





            strQuery = " INSERT INTO [dbo].[BECAS_GESTION_BECAS_TB] " & _
                     "  ([fk_id_colaborador]" & _
                    "   ,[no_correlativo] " & _
                    "   ,[fk_id_tipo_beca]" & _
                    "   ,[fecha_asignacion]" & _
                    "   ,[fk_id_periodo_asignacion]" & _
                    "   ,[fk_id_tipo_asignacion]" & _
                    "   ,[fk_id_modalidad_estudio]" & _
                    "   ,[fk_id_tipo_proyecto]" & _
                    "   ,[fk_id_tipo_estatus]" & _
                    "   ,[fk_id_estatus_pago]" & _
                    "   ,[fecha_inicio] " & _
                    "   ,[fecha_termino]" & _
                    "   ,[fk_id_proveedor]" & _
                    "   ,[observaciones]" & _
                    "   ,[fecha_baja_temporal]" & _
                    "   ,[estatus]" & _
                    "   ,[fecha_creacion]" & _
                    "   ,[usuario_creacion])" & _
                    "    VALUES(" & _
                    "    " & ddlColaborador.SelectedValue & " " & _
                    "   ,NULL" & _
                    "   ,'" & strBeca & "' " & _
                    "   ,'" & strFechaAsignacion & "'" & _
                    "   ," & strPeriodoAsignacion & _
                    "   ," & strTipoAsignacion & _
                    "   ," & strModalidadEstudios & _
                    "   ," & strTipoProyecto & _
                    "   ," & strTipoEstatus & _
                    "   ," & strEstatusPago & _
                    "   ,'" & strFechaInicio & "'" & _
                    "   ," & strFechaFinal & "" & _
                    "   ,'" & strProveedores & "'" & _
                    "   ,'" & strObservaciones & "'" & _
                    "   ," & IIf(strFechaBajaTemp = "", "NULL", "'" & strFechaBajaTemp & "'") & " " & _
                    "   ,'CREADO'" & _
                    "   ,GETDATE()" & _
                    "   ,'" & hdUsuario.Value & "') Select @@Identity "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim iId As Integer = 0
            iId = CInt(odbComando.ExecuteScalar())
            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)

            odbConexion.Close()
            Call obtBecas(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregar_Click(sender As Object, e As EventArgs)
        divBecasIngles.Visible = False
        divCalificacionesBecas.Visible = False
        Call insBecas()
    End Sub

    Private Sub grdBecas_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdBecas.RowCancelingEdit
        grdBecas.ShowFooter = True
        grdBecas.EditIndex = -1
        Call obtBecas(ddlColaborador.SelectedValue)
    End Sub
    'INSERTA FILA VACIA CUANDO NO EXISTA NINGUN REGISTRO
    Public Sub insFilaVaciaBecas()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("no_correlativo"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_beca"))
        dt.Columns.Add(New DataColumn("fecha_asignacion"))
        dt.Columns.Add(New DataColumn("fk_id_periodo_asignacion"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_asignacion"))
        dt.Columns.Add(New DataColumn("fk_id_modalidad_estudio"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_proyecto"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_estatus"))
        dt.Columns.Add(New DataColumn("fk_id_estatus_pago"))
        dt.Columns.Add(New DataColumn("fecha_inicio"))
        dt.Columns.Add(New DataColumn("fecha_termino"))
        dt.Columns.Add(New DataColumn("fk_id_proveedor"))
        dt.Columns.Add(New DataColumn("observaciones"))
        dt.Columns.Add(New DataColumn("fecha_baja_temporal"))

        dr = dt.NewRow
        dr("id") = ""
        dr("no_correlativo") = ""
        dr("fk_id_tipo_beca") = ""
        dr("fecha_asignacion") = ""
        dr("fk_id_periodo_asignacion") = ""
        dr("fk_id_tipo_asignacion") = ""
        dr("fk_id_modalidad_estudio") = ""
        dr("fk_id_tipo_proyecto") = ""
        dr("fk_id_tipo_estatus") = ""
        dr("fk_id_estatus_pago") = ""
        dr("fecha_inicio") = ""
        dr("fecha_termino") = ""
        dr("fk_id_proveedor") = ""
        dr("observaciones") = ""
        dr("fecha_baja_temporal") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdBecas.DataSource = dt.DefaultView
        grdBecas.DataBind()


    End Sub
    Private Sub grdBecas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBecas.RowDataBound
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
    Private Sub grdBecas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdBecas.RowDeleting
        divBecasIngles.Visible = False
        divCalificacionesBecas.Visible = False
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdBecas.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "UPDATE [dbo].[BECAS_GESTION_BECAS_TB] " & _
                               "  SET [estatus] ='ELIMINADO'  " & _
                               "     ,[fecha_modificacion] = GETDATE()           " & _
                               "     ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                               "WHERE ID=" & strId


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)
            odbConexion.Close()

            grdBecas.EditIndex = -1
            grdBecas.ShowFooter = True

            Call obtBecas(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub

    Private Sub grdBecas_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdBecas.RowEditing
        grdBecas.ShowFooter = False
        divBecasIngles.Visible = False
        divCalificacionesBecas.Visible = False
        hdIndexCurso.Value = e.NewEditIndex
        grdBecas.EditIndex = e.NewEditIndex
        Call obtBecas(ddlColaborador.SelectedValue)
    End Sub

    Private Sub grdBecas_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdBecas.RowUpdating
        grdBecas.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strBeca As String = ""
        Dim strFechaAsignacion As String = ""
        Dim strPeriodoAsignacion As String = ""
        Dim strTipoAsignacion As String = ""
        Dim strModalidadEstudios As String = ""
        Dim strTipoProyecto As String = ""
        Dim strTipoEstatus As String = ""
        Dim strEstatusPago As String = ""
        Dim strFechaInicio As String = ""
        Dim strFechaFinal As String = ""
        Dim strProveedor As String = ""
        Dim strObservaciones As String = ""
        Dim strId As String = ""
        Dim strFechaBajaTemp As String = ""
        Try
            strId = DirectCast(grdBecas.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strBeca = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlTipoBeca"), DropDownList).Text)
            strFechaAsignacion = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("txtFechaAsignacion"), TextBox).Text)
            strPeriodoAsignacion = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlPeriodoAsignacion"), DropDownList).Text)
            strTipoAsignacion = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlTipoAsignacion"), DropDownList).Text)
            strModalidadEstudios = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlModalidadEstudio"), DropDownList).Text)
            strTipoProyecto = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlTipoProyecto"), DropDownList).Text)
            strTipoEstatus = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlTipoEstus"), DropDownList).Text)
            strEstatusPago = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlEstatusPago"), DropDownList).Text)
            strFechaInicio = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("txtFechaInicio"), TextBox).Text)
            strFechaFinal = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("txtFechaFinal"), TextBox).Text)
            strProveedor = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("ddlProveedor"), DropDownList).Text)
            strObservaciones = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("txtObservaciones"), TextBox).Text)
            strFechaBajaTemp = (DirectCast(grdBecas.Rows(e.RowIndex).FindControl("txtFechaBajaTem"), TextBox).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strBeca = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Beca.');</script>", False)
                Exit Sub
            End If
            If strFechaAsignacion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de Capturar la Fecha de Asignación.');</script>", False)
                Exit Sub
            End If
            If strPeriodoAsignacion = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el Periodo de Asignación.');</script>", False)
                Exit Sub
            End If
            If strTipoAsignacion = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Asignación.');</script>", False)
                Exit Sub
            End If
            If strModalidadEstudios = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar la modalidad de estudio.');</script>", False)
                Exit Sub
            End If
            If strTipoProyecto = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Proyecto.');</script>", False)
                Exit Sub
            End If
            If strTipoEstatus = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el tipo de Estatus.');</script>", False)
                Exit Sub
            End If

            If strEstatusPago = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el estatus de Pago.');</script>", False)
                Exit Sub
            End If

            If strProveedor = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar el Proveedor.');</script>", False)
                Exit Sub
            End If
            If strFechaInicio = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Inicio.');</script>", False)
                Exit Sub
            End If
            If strFechaFinal = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Fecha de Final.');</script>", False)
                Exit Sub
            End If
            If CDate(strFechaInicio) >= CDate(strFechaFinal) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('La Fecha de Inicio no puede ser menor o igual a la Fecha Final.');</script>", False)
                Exit Sub
            End If

            'Estatus Beca Activo
            If strTipoEstatus = "1" Then
                'valida si tiene >= 1 Beca activa en modo edicion
                If validaBecasActivas(odbConexion, 1, strId) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede tener mas de una Beca Activa.');</script>", False)
                    Exit Sub
                End If
            End If



            strQuery = "UPDATE [dbo].[BECAS_GESTION_BECAS_TB]  " & _
                       " SET [fk_id_colaborador] = " & ddlColaborador.SelectedValue & " " & _
                       " ,[fk_id_tipo_beca] = " & strBeca & "" & _
                       " ,[fecha_asignacion] = '" & strFechaAsignacion & "' " & _
                       " ,[fk_id_periodo_asignacion] =" & strPeriodoAsignacion & " " & _
                       " ,[fk_id_tipo_asignacion] = " & strTipoAsignacion & " " & _
                       " ,[fk_id_modalidad_estudio] = " & strModalidadEstudios & " " & _
                       " ,[fk_id_tipo_proyecto] = " & strTipoProyecto & " " & _
                       " ,[fk_id_tipo_estatus] = " & strTipoEstatus & " " & _
                       " ,[fk_id_estatus_pago] = " & strEstatusPago & " " & _
                       " ,[fecha_inicio] = '" & strFechaInicio & "'" & _
                       " ,[fecha_termino] = '" & strFechaFinal & "'" & _
                       " ,[fk_id_proveedor] = '" & strProveedor & "'" & _
                       " ,[observaciones] = '" & strObservaciones & "'" & _
                       "  ,[estatus] ='MODIFICADO'                         " & _
                       "  ,[fecha_modificacion] = GETDATE()           " & _
                       "  ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                       "  ,[fecha_baja_temporal] = " & IIf(strFechaBajaTemp = "", "NULL", "'" & strFechaBajaTemp & "'") & "  " & _
                       "WHERE ID=" & strId

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)
            odbConexion.Close()
            grdBecas.EditIndex = -1
            Call obtBecas(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try

    End Sub
    'calcula el numero correlativo
    Public Sub CargaCorrelativo(odbConexion As OleDbConnection)
        Try
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_calcula_correlativo_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)

            odbComando.ExecuteNonQuery()

        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub
    'obtiene la informacion de las Becas
    Private Sub grdBecas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdBecas.SelectedIndexChanged
        If hdTipoBeca.Value = "INGLES" Then
            divBecasIngles.Visible = True
            divCalificacionesBecas.Visible = False
            txtCalificacion.Text = ""
            txtAsistencia.Text = ""
            'obtiene la configuracion beca de ingles
            Call obtConfiguracionBecaIngles(hdIdBecaIngles.Value)
        Else
            'obtiene la configuracion de las becas
            divCalificacionesBecas.Visible = True
            divBecasIngles.Visible = False
            txtConcepto.Text = ""
            txtCalificacionBecas.Text = ""
            Call obtCalficacionesBecas(hdIdBeca.Value)
        End If


    End Sub
#End Region
#Region "Colaboradores"


    'obtiene en el numero de Nomina del Jefe
    Public Function ObtNoNomina() As String
        Dim strResultado As String = ""
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        odbConexion.Open()
        strQuery = "SELECT * FROM SIGIDO_USUARIOS_TB where usuario='" & hdUsuario.Value & "' ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(1).ToString

        End If
        odbConexion.Close()
        Return strResultado
    End Function

    Public Sub CargaColaboradores()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            hdNoNominaUsuario.Value = ObtNoNomina()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "becas_colaboradores_reportan_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametro
            odbComando.Parameters.AddWithValue("@parametrizacion", 0)
            odbComando.Parameters.AddWithValue("@jefe", hdNoNominaUsuario.Value)
            odbComando.Parameters.AddWithValue("@perfil", hdRol.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlColaborador.DataSource = odbLector
            ddlColaborador.DataValueField = "CLAVE"
            ddlColaborador.DataTextField = "NOMBRE"

            ddlColaborador.DataBind()


            For Each li As ListItem In ddlColaborador.Items

                li.Attributes.Add("style", "background-color: Red")

            Next

            odbConexion.Close()
            'valida si tiene asignado empleado
            If ddlColaborador.Items.Count = 0 Then
                divRegistro.Visible = False
                divDatos.Visible = False
                lblErrorGestion.Text = "No tiene colaboradores para registrar en la Becas."
            End If

        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

    Private Sub ObtDatosColaboradores()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Dim strQuery As String = ""
            strQuery = " SELECT [CLAVE]                               " & _
            "    ,[NOMBRE] + SPACE(1)+[APPAT]+ SPACE(1) +[APMAT] AS NOMBRE_COMPLETO   " & _
            "    ,[CURP]                                                              " & _
            "    ,[DIRAREA]                                                           " & _
            "    ,[DIR]                                                               " & _
            "    ,[GERENCIA]                                                          " & _
            "    ,[DEPARTAMENTO]                                                      " & _
            "    ,[DEPARTAMENTO2]                                                     " & _
            "    ,[PUESTO]                                                            " & _
            "    ,REPLACE([UBICACION],'DINA ','') AS UBICACION  " & _
            "    ,[UNIDAD_DE_NEGOCIO]" & _
            "    ,[ESTATUS] " & _
            "FROM [SGIDO_INFOGIRO_GIRO_VT] " & _
            " WHERE [CLAVE]=" & IIf(ddlColaborador.SelectedValue = "", 0, ddlColaborador.SelectedValue)
            ' If hdRol.Value = "U" Then strQuery += " and (ESTATUS='ACTIVO' AND [CLAVEJEFE]=" & hdNoNominaUsuario.Value & ")"


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            lblPuestoCol.Text = ""
            lblDireccion.Text = ""
            lblNombreCol.Text = ""
            lblArea.Text = ""
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                lblNombreCol.Text = StrConv(odbLector(1).ToString, VbStrConv.ProperCase) & " - " & odbLector(0).ToString & " " & _
                                    (IIf(odbLector(11).ToString = "ACTIVO", "<span class='text-green'>Activo</span>", "<span class='text-yellow'>Baja</span>"))
                lblPuestoCol.Text = "<br /> <strong>Puesto: </strong>" & StrConv(odbLector(8).ToString, VbStrConv.ProperCase)
                lblArea.Text = IIf(odbLector(5).ToString.Trim = "" Or odbLector(5).ToString.Trim = "N/A", "", "<br /> <strong>Área: </strong>" & StrConv(odbLector(5).ToString.Trim, VbStrConv.ProperCase))
                lblDireccion.Text = "<br /> <strong>Dirección: </strong>" & IIf(odbLector(4).ToString = "", StrConv(odbLector(3).ToString, VbStrConv.ProperCase), StrConv(odbLector(4).ToString, VbStrConv.ProperCase))

                'obtiene cursos
                Call obtBecas(ddlColaborador.SelectedValue)

                odbLector.Close()
                divRegistro.Visible = True
            Else 'si no tiene colaboradores asignados
                divRegistro.Visible = False

            End If
            divBecasIngles.Visible = False
            divCalificacionesBecas.Visible = False
            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub


    Private Sub ddlColaborador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlColaborador.SelectedIndexChanged
        Call ObtDatosColaboradores()
    End Sub
#End Region
#Region "Comportamientos"
    Public Sub comportamiento()
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdBecas.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdBecas.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To grdCalificaciones.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCalificaciones.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To grdCalificacionesIngles.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCalificacionesIngles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
    End Sub


#End Region
#Region "Excel"
    Public Sub obtExportarExcel(dsDatos As DataSet)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try

            Dim filename As String = "rpt_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second) & ".xls"
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

    Private Sub btnExportarCursos_ServerClick(sender As Object, e As EventArgs) Handles btnExportarCursos.ServerClick
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim sSQL As String = ""
        Dim dsCursos As New DataSet
        Try
            odbConexion.Open()
            sSQL = "SELECT * FROM BECAS_GESTION_BECAS_VT WHERE  [CLAVE]=" & ddlColaborador.SelectedValue
            Dim odbComando As New OleDbCommand(sSQL, odbConexion)

            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCursos)

            Call obtExportarExcel(dsCursos)
            odbConexion.Close()

            Call comportamiento()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
#End Region
#Region "Becas Ingles"
    ''Obtiene la infromacion de los Archivos de Reporte
    Public Sub obtCalificacionesIngles(iIdBecas As Integer)
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Try

            odbConexion.Open()
            Dim strExtencion As String = ""

            strQuery = "SELECT A.id, (SELECT DESCRIPCION FROM BECAS_INGLES_NIVELES_CT AA WHERE AA.ID=A.fk_id_ingles_niveles ) AS [Ingles_niveles] " & _
                              ",[calificacion]" & _
                              ",[asistencia] " & _
                       "FROM BECAS_INGLES_CALIFICACIONES_INGLES_TB A WHERE fk_id_beca=" & iIdBecas & " ORDER BY 1 DESC"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdCalificacionesIngles.DataSource = dsCatalogo.Tables(0).DefaultView
            grdCalificacionesIngles.DataBind()
            If grdCalificacionesIngles.Rows.Count = 0 Then

                grdCalificacionesIngles.Visible = False
            Else

                grdCalificacionesIngles.Visible = True
            End If
            'óbtiene el nivel de la beca
            Call ObtNivel(odbConexion, iIdBecas)

            For iFil As Integer = 0 To grdCalificacionesIngles.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdCalificacionesIngles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            odbConexion.Close()
        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub obtConfiguracionBecaIngles(iIdBecas As Integer)
        Dim dsArchivos As New DataSet
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsCatalogo As New DataSet
        Dim strJavaScript As String = ""
        Dim strNombreCarta As String = ""
        Dim strResultado As String = ""
        Dim strTablaVisibilidad As String = ""
        Dim strComportamientoControles As String = ""
        Try

            odbConexion.Open()
            Dim strExtencion As String = ""

            strQuery = "SELECT ISNULL([fk_id_horario_estudio_ingles],0) " & _
                       "      ,ISNULL([fk_id_plantel_ingles],0)" & _
                       "      ,ISNULL([fk_id_nivel_inicial_ingles],0)" & _
                       "      ,ISNULL([fk_id_tipo_autorizacion_ingles],0)" & _
                       " FROM BECAS_GESTION_BECAS_TB WHERE id=" & iIdBecas & " ORDER BY 1 DESC"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                ddlHorarioEstudio.SelectedValue = odbLector(0).ToString
                ddlPlantel.SelectedValue = odbLector(1).ToString
                ddlNivelInicial.SelectedValue = odbLector(2).ToString
                ddlTipoAutorizacion.SelectedValue = odbLector(3).ToString


                odbLector.Close()
            End If
            'validacion de comportamientos
            If validaConfiguracionIngles(odbConexion, iIdBecas) Then

                ddlTipoAutorizacion.Attributes.Add("disabled", "disabled")
                ddlHorarioEstudio.Attributes.Add("disabled", "disabled")
                ddlPlantel.Attributes.Add("disabled", "disabled")
                ddlNivelInicial.Attributes.Add("disabled", "disabled")
                divGuardaConfBIngles.Visible = False
                'obtiene el nivel
                divCalificaciones.Visible = True

            Else
                ddlTipoAutorizacion.Attributes.Remove("disabled")
                ddlHorarioEstudio.Attributes.Remove("disabled")
                ddlPlantel.Attributes.Remove("disabled")
                ddlNivelInicial.Attributes.Remove("disabled")
                divGuardaConfBIngles.Visible = True
                divCalificaciones.Visible = False
            End If

            Call obtCalificacionesIngles(iIdBecas)
            odbConexion.Close()
        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'valida si existe configuracion en la beca
    Public Function validaConfiguracionIngles(odbConexion As OleDbConnection, strid As String) As Boolean
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blnResultado As Boolean = False
        Dim odbLector As OleDbDataReader


        strQuery = " SELECT ISNULL(fk_id_horario_estudio_ingles,0) FROM BECAS_GESTION_BECAS_TB  WHERE ID=" & IIf(strid = "", 0, strid)

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            blnResultado = IIf(odbLector(0) = 0, False, True)
            odbLector.Close()
        End If


        Return blnResultado
    End Function
    'inserta las calificaciones de becas de ingles
    Public Sub insCalificacionesIngles()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()

            strQuery = "INSERT INTO BECAS_INGLES_CALIFICACIONES_INGLES_TB (fk_id_beca,fk_id_ingles_niveles,calificacion,asistencia,fecha_creacion,usuario_creacion )" & _
                " VALUES (" & hdIdBecaIngles.Value & "," & ddlNivel.SelectedValue & "," & txtCalificacion.Text & "," & txtAsistencia.Text & ",getdate(),'" & hdUsuario.Value & "')"
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()
            Call obtCalificacionesIngles(hdIdBecaIngles.Value)
            'limpia controles
            ddlNivel.SelectedValue = 0
            txtCalificacion.Text = ""
            txtAsistencia.Text = ""

        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'actualiza los datos de configuracion 
    Public Sub ActulizaBecaIngles()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()

            strQuery = " UPDATE [dbo].[BECAS_GESTION_BECAS_TB] " & _
                      " SET [fk_id_horario_estudio_ingles] = " & ddlHorarioEstudio.SelectedValue & "" & _
                      " ,[fk_id_plantel_ingles] = " & ddlPlantel.SelectedValue & "" & _
                      " ,[fk_id_nivel_inicial_ingles] =  " & ddlNivelInicial.SelectedValue & "" & _
                      " ,[fk_id_tipo_autorizacion_ingles] = " & ddlTipoAutorizacion.SelectedValue & "" & _
                      "  ,[fecha_modificacion] = GETDATE()           " & _
                      "  ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                      " Where id=" & IIf(hdIdBecaIngles.Value = "", 0, hdIdBecaIngles.Value)

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()
            Call obtConfiguracionBecaIngles(hdIdBecaIngles.Value)
        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    'obtiene el nivel del combo
    Private Sub ObtNivel(ByVal odbConexion As OleDbConnection, strId As String)

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "becas_ingles_niveles_calificaciones_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@id_beca", strId)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()

        ddlNivel.DataSource = odbLector
        ddlNivel.DataValueField = "ID"
        ddlNivel.DataTextField = "descripcion"

        ddlNivel.DataBind()
        'valida si los item estan vacios

    End Sub
    Private Sub btnGuardaConfiguracionBIngles_ServerClick(sender As Object, e As EventArgs) Handles btnGuardaConfiguracionBIngles.ServerClick
        Call ActulizaBecaIngles()
    End Sub

    Private Sub btnAgregaCalfIngles_ServerClick(sender As Object, e As EventArgs) Handles btnAgregaCalfIngles.ServerClick
        Call insCalificacionesIngles()
    End Sub

    Private Sub grdCalificacionesIngles_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCalificacionesIngles.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdCalificacionesIngles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = " DELETE [dbo].[BECAS_INGLES_CALIFICACIONES_INGLES_TB] " & _
                       " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()

            grdCalificacionesIngles.EditIndex = -1
            grdCalificacionesIngles.ShowFooter = True

            Call obtCalificacionesIngles(hdIdBecaIngles.Value)
        Catch ex As Exception
            lblErrorCalificacionIngles.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Private Sub grdCalificacionesIngles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCalificacionesIngles.RowDataBound
        For i As Integer = 0 To grdCalificacionesIngles.Rows.Count - 1
            Dim btnEliminar As LinkButton = grdCalificacionesIngles.Rows(i).Controls(4).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el " + grdCalificacionesIngles.Rows(i).Cells(1).Text + "?')){ return false; };"
        Next
    End Sub

#End Region
#Region "Calificaciones Becas"
    Public Sub obtCalficacionesBecas(iIdBecas As Integer)
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Dim dsDatos As New DataSet

        Try

            odbConexion.Open()


            strQuery = "SELECT * FROM [BECAS_CALIFICACIONES_TB] WHERE fk_id_beca=" & iIdBecas & " ORDER BY 1 DESC"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbAdaptador As New OleDbDataAdapter

            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsDatos)
            grdCalificaciones.DataSource = dsDatos.Tables(0).DefaultView
            grdCalificaciones.DataBind()

            odbConexion.Close()

            For iFil As Integer = 0 To grdCalificaciones.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdCalificaciones.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

        Catch ex As Exception
            lblErrorCalificacion.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    'inserta las calificaciones de becas
    Public Sub insCalificacionesBecas()
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As New OleDbConnection(strConexion)
        Dim strQuery As String = ""
        Try
            odbConexion.Open()

            strQuery = "INSERT INTO [BECAS_CALIFICACIONES_TB] (fk_id_beca,concepto,calificacion,fecha_creacion,usuario_creacion )" & _
                " VALUES (" & hdIdBeca.Value & ",'" & txtConcepto.Text & "'," & txtCalificacionBecas.Text & ",getdate(),'" & hdUsuario.Value & "')"
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()
            odbConexion.Close()
            Call obtCalficacionesBecas(hdIdBeca.Value)
            'limpia controles

            txtConcepto.Text = ""
            txtCalificacionBecas.Text = ""

        Catch ex As Exception
            lblErrorCalificacion.Text = Err.Number & " " & ex.Message
        End Try
    End Sub
    Private Sub btnAgregarCalificaciones_ServerClick(sender As Object, e As EventArgs) Handles btnAgregarCalificaciones.ServerClick
        Call insCalificacionesBecas()
    End Sub

    Private Sub grdCalificaciones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCalificaciones.RowDataBound
        For i As Integer = 0 To grdCalificaciones.Rows.Count - 1
            Dim btnEliminar As LinkButton = grdCalificaciones.Rows(i).Controls(3).Controls(1)
            'JAVA SCRIPT
            btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar el " + grdCalificaciones.Rows(i).Cells(1).Text + "?')){ return false; };"
        Next
    End Sub

    Private Sub grdCalificaciones_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCalificaciones.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdCalificaciones.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = " DELETE [dbo].[BECAS_CALIFICACIONES_TB] " & _
                       " WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()


            Call obtCalficacionesBecas(hdIdBeca.Value)
        Catch ex As Exception
            lblErrorCalificacion.Text = Err.Number & " " & ex.Message
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