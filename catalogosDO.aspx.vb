Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class catalogosDO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""

        If Not Page.IsPostBack Then
            Call obtenerUsuarioAD()
            Call inicialiciaTabs()
            Call obtCatalogoPuesto()
            Call obtCatalogoCarreras()
            Call obtCatalogoEscolaridad()
            Call obtCatalogoFacultades()
            Call obtCatalogoHabilidades()
            Call obtCatalogoIdioma()
            Call obtCatalogoNivel()
            Call obtCatalogoPuestos()
            Call obtCatalogoRol()
            Call obtCatalogoPuestoRol()
            Call LlenarFormularioddl()

        End If
        Call comportamientos()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
    End Sub
    '************************* - Catalogos Generales - *******************************************
#Region "Grid Puesto"
    'obtiene el catalogo 
    Public Sub obtCatalogoPuesto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim odbAdaptador As New OleDbDataAdapter
        Dim dsPuesto As New DataSet
        Dim dsNivel As New DataSet
        Try
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "do_puestos_sel_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure
            odbAdaptador.SelectCommand = odbComando

            odbAdaptador.Fill(dsCatalogo)
            grdPuestos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPuestos.DataBind()

            If grdPuestos.Rows.Count = 0 Then
                Call insFilaVaciaPuestos()
                grdPuestos.Rows(0).Visible = False
            Else
                grdPuestos.Rows(0).Visible = True
            End If

            odbConexion.Close()
            'Obtiene el Dataset con los puestos
            dsPuesto = obtDsPuesto(odbConexion)

            Dim i As Int16 = 0

            For i = 0 To grdPuestos.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdPuestos.Rows(i).Controls(26).Controls(0)
                Dim btnEliminar As LinkButton = grdPuestos.Rows(i).Controls(27).Controls(1)

                iId = DirectCast(grdPuestos.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                btnEditar.Attributes("onclick") = " CargaInformacion();"
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdPuestos.Rows(i).Cells(2).FindControl("ddlEstatus")
                    Dim ddlNivel As DropDownList = grdPuestos.Rows(i).FindControl("ddlNivel")
                    Call obtddlNivel(ddlNivel)
                    Dim ddlTipoPuesto As DropDownList = grdPuestos.Rows(i).FindControl("ddlTipoPuesto")
                    Dim ddlEmpresa As DropDownList = grdPuestos.Rows(i).FindControl("ddlEmpresa")
                    Dim ddlClaveJefe As DropDownList = grdPuestos.Rows(i).FindControl("ddlClaveJefe")
                    Call obtddlPuesto(ddlClaveJefe, dsPuesto)

                    Dim ddlN13 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN13")
                    Call obtddlPuesto(ddlN13, dsPuesto)
                    Dim ddlN12 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN12")
                    Call obtddlPuesto(ddlN12, dsPuesto)
                    Dim ddlN11 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN11")
                    Call obtddlPuesto(ddlN11, dsPuesto)
                    Dim ddlN10 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN10")
                    Call obtddlPuesto(ddlN10, dsPuesto)
                    Dim ddlN9 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN9")
                    Call obtddlPuesto(ddlN9, dsPuesto)
                    Dim ddlN8 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN8")
                    Call obtddlPuesto(ddlN8, dsPuesto)
                    Dim ddlN7 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN7")
                    Call obtddlPuesto(ddlN7, dsPuesto)
                    Dim ddlN6 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN6")
                    Call obtddlPuesto(ddlN6, dsPuesto)
                    Dim ddlN5 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN5")
                    Call obtddlPuesto(ddlN5, dsPuesto)
                    Dim ddlN4 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN4")
                    Call obtddlPuesto(ddlN4, dsPuesto)
                    Dim ddlN3 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN3")
                    Call obtddlPuesto(ddlN3, dsPuesto)
                    Dim ddlN2 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN2")
                    Call obtddlPuesto(ddlN2, dsPuesto)
                    Dim ddlN1 As DropDownList = grdPuestos.Rows(i).FindControl("ddlN1")
                    Call obtddlPuesto(ddlN1, dsPuesto)
                    Dim ddlAreaAdscribe As DropDownList = grdPuestos.Rows(i).FindControl("ddlAreaAdscribe")
                    Call obtddlAreaAdscribe(ddlAreaAdscribe)
                    Dim ddlPuestoGiro As DropDownList = grdPuestos.Rows(i).FindControl("ddlPuestoGiro")
                    Call obtddlPuestoGIRO(ddlPuestoGiro)

                    Dim ddlUNegocio As DropDownList = grdPuestos.Rows(i).FindControl("ddlUNegocio")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlNivel.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(3).ToString

                            ddlTipoPuesto.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(5).ToString
                            ddlEmpresa.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                            ddlN13.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlN12.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(9).ToString
                            ddlN11.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(11).ToString
                            ddlN10.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(13).ToString
                            ddlN9.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(15).ToString
                            ddlN8.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(17).ToString
                            ddlN7.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(19).ToString
                            ddlN6.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(21).ToString
                            ddlN5.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(23).ToString
                            ddlN4.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(25).ToString
                            ddlN3.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(27).ToString
                            ddlN2.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(29).ToString
                            ddlN1.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(31).ToString
                            ddlAreaAdscribe.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(33).ToString
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(35).ToString
                            ddlUNegocio.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(36).ToString
                            ddlClaveJefe.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(38).ToString
                            ddlPuestoGiro.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(40).ToString
                        End If
                    Next
                    Dim btnCancelar As LinkButton = grdPuestos.Rows(i).Controls(26).Controls(2)
                    btnCancelar.ToolTip = "Cancelar Edición"
                    btnCancelar.Attributes("onclick") = "CargaInformacion();"
                    btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el Puesto " + DirectCast(grdPuestos.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; }; CargaInformacion();"


                Else

                    'Empresa
                    Dim lblEmpresa As New Label
                    lblEmpresa = grdPuestos.Rows(i).FindControl("lblEmpresa")
                    ' lblEmpresa.Text = IIf(lblEmpresa.Text = "001", "Passa Administración y Servicios S.A. de C.V", "Promotora de Negocios G, S.A. de C.V.")
                    Dim strEmpresa As String
                    strEmpresa = String.Empty

                    Select Case lblEmpresa.Text
                        Case "001"
                            strEmpresa = "Passa Administración y Servicios S.A. de C.V"
                        Case "002"
                            strEmpresa = "DINA CAMIONES"
                        Case "003"
                            strEmpresa = "MERCADER FINANCIAL"
                        Case "004"
                            strEmpresa = "DINA COMERCIALIZACION AUTOMOTRIZ (DICOMER)"
                        Case "005"
                            strEmpresa = "TRANSPORTES Y LOGISTICA DE JALISCO"
                        Case "006"
                            strEmpresa = "DINA COMERCIALIZACION SERVICIOS Y REFACCIONES SA DE CV (DICOSER)"

                    End Select

                    lblEmpresa.Text = strEmpresa

                    'Estatus
                    Dim lblEstatus As New Label
                    lblEstatus = grdPuestos.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")

                    btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar el Puesto " + DirectCast(grdPuestos.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; }; CargaInformacion();"
                End If
            Next
            'Carga los combo de Insert

            Dim ddlAgreNivel As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgreNivel")
            Call obtddlNivel(ddlAgreNivel)

            Dim ddlAgreClaveJefe As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgreClaveJefe")
            Call obtddlPuesto(ddlAgreClaveJefe, dsPuesto)

            Dim ddlAgre13 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre13")
            Call obtddlPuesto(ddlAgre13, dsPuesto)

            Dim ddlAgre12 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre12")
            Call obtddlPuesto(ddlAgre12, dsPuesto)

            Dim ddlAgre11 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre11")
            Call obtddlPuesto(ddlAgre11, dsPuesto)

            Dim ddlAgre10 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre10")
            Call obtddlPuesto(ddlAgre10, dsPuesto)

            Dim ddlAgre9 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre9")
            Call obtddlPuesto(ddlAgre9, dsPuesto)

            Dim ddlAgre8 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre8")
            Call obtddlPuesto(ddlAgre8, dsPuesto)

            Dim ddlAgre7 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre7")
            Call obtddlPuesto(ddlAgre7, dsPuesto)

            Dim ddlAgre6 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre6")
            Call obtddlPuesto(ddlAgre6, dsPuesto)

            Dim ddlAgre5 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre5")
            Call obtddlPuesto(ddlAgre5, dsPuesto)

            Dim ddlAgre4 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre4")
            Call obtddlPuesto(ddlAgre4, dsPuesto)

            Dim ddlAgre3 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre3")
            Call obtddlPuesto(ddlAgre3, dsPuesto)

            Dim ddlAgre2 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre2")
            Call obtddlPuesto(ddlAgre2, dsPuesto)

            Dim ddlAgre1 As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgre1")
            Call obtddlPuesto(ddlAgre1, dsPuesto)

            Dim ddlAgreAreaAdscribe As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgreAreaAdscribe")
            Call obtddlAreaAdscribe(ddlAgreAreaAdscribe)

            Dim ddlAgrePuestoGiro As DropDownList = grdPuestos.FooterRow.FindControl("ddlAgrePuestoGiro")
            Call obtddlPuestoGIRO(ddlAgrePuestoGiro)

            'Efectiva
            For iFil As Integer = 0 To grdPuestos.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPuestos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next

            'For Each colum As DataGridColumn In grdPuestos.Columns
            '    colum.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            'Next
            GC.Collect()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaPuestos()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("codigo"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("nivel"))
        dt.Columns.Add(New DataColumn("tipo_puesto"))
        dt.Columns.Add(New DataColumn("empresa"))
        dt.Columns.Add(New DataColumn("n13"))
        dt.Columns.Add(New DataColumn("n12"))
        dt.Columns.Add(New DataColumn("n11"))
        dt.Columns.Add(New DataColumn("n10"))
        dt.Columns.Add(New DataColumn("n9"))
        dt.Columns.Add(New DataColumn("n8"))
        dt.Columns.Add(New DataColumn("n7"))
        dt.Columns.Add(New DataColumn("n6"))
        dt.Columns.Add(New DataColumn("n5"))
        dt.Columns.Add(New DataColumn("n4"))
        dt.Columns.Add(New DataColumn("n3"))
        dt.Columns.Add(New DataColumn("n2"))
        dt.Columns.Add(New DataColumn("n1"))
        dt.Columns.Add(New DataColumn("area_adscribe"))
        dt.Columns.Add(New DataColumn("comentarios"))
        dt.Columns.Add(New DataColumn("estatus"))
        dt.Columns.Add(New DataColumn("unidad_negocio"))
        dr = dt.NewRow
        dr("id") = ""
        dr("codigo") = ""
        dr("descripcion") = ""
        dr("nivel") = ""
        dr("tipo_puesto") = ""
        dr("empresa") = ""
        dr("n13") = ""
        dr("n12") = ""
        dr("n11") = ""
        dr("n10") = ""
        dr("n9") = ""
        dr("n8") = ""
        dr("n7") = ""
        dr("n6") = ""
        dr("n5") = ""
        dr("n4") = ""
        dr("n3") = ""
        dr("n2") = ""
        dr("n1") = ""
        dr("area_adscribe") = ""
        dr("comentarios") = ""
        dr("estatus") = ""
        dr("unidad_negocio") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPuestos.DataSource = dt.DefaultView
        grdPuestos.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insPuestos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strCodigo As String = (DirectCast(grdPuestos.FooterRow.FindControl("txtAgreCodigo"), TextBox).Text)
        Dim strDescripcion As String = (DirectCast(grdPuestos.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
        Dim strNivel As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreNivel"), DropDownList).Text)
        Dim strTipoPuesto As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreTipoPuesto"), DropDownList).Text)
        Dim strEmpresaD As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreEmpresa"), DropDownList).Text)
        Dim strN13D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre13"), DropDownList).Text)
        Dim strN12D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre12"), DropDownList).Text)
        Dim strN11D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre11"), DropDownList).Text)
        Dim strN10D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre10"), DropDownList).Text)
        Dim strN9D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre9"), DropDownList).Text)
        Dim strN8D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre8"), DropDownList).Text)
        Dim strN7D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre7"), DropDownList).Text)
        Dim strN6D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre6"), DropDownList).Text)
        Dim strN5D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre5"), DropDownList).Text)
        Dim strN4D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre4"), DropDownList).Text)
        Dim strN3D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre3"), DropDownList).Text)
        Dim strN2D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre2"), DropDownList).Text)
        Dim strN1D As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgre1"), DropDownList).Text)
        Dim strArea As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreAreaAdscribe"), DropDownList).Text)
        Dim strComentarios As String = (DirectCast(grdPuestos.FooterRow.FindControl("txtAgreComentarios"), TextBox).Text)
        Dim strEstatus As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
        Dim strUNegocio As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreUNegicio"), DropDownList).Text)
        Dim strPosiciones As String = (DirectCast(grdPuestos.FooterRow.FindControl("txtAgregarPosicion"), TextBox).Text)
        Dim strClaveJefe As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgreClaveJefe"), DropDownList).Text)
        Dim strPuestoGiro As String = (DirectCast(grdPuestos.FooterRow.FindControl("ddlAgrePuestoGiro"), DropDownList).Text)
        Try

            If strCodigo = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el código.');</script>", False)
                Exit Sub
            End If
            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If
            If strNivel = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nivel.');</script>", False)
                Exit Sub
            End If
            strPosiciones = IIf(strPosiciones = "", "0", strPosiciones)
            odbConexion.Open()
            strQuery = "INSERT INTO [dbo].[DO_PUESTOS_TB]" &
                       "    ([codigo] ,[descripcion],[nivel],[tipo_puesto],[empresa]" &
                       "    ,[n13],[n12],[n11],[n10],[n9],[n8]" &
                       "    ,[n7],[n6],[n5],[n4] ,[n3],[n2],[n1]" &
                       "    ,[area_adscribe] ,[comentarios]" &
                       "    ,[fecha_creacion],[usuario_creacion],[estatus],unidad_negocio,posiciones,jefe,clave_giro)VALUES (" &
                       " '" & strCodigo & "' ," &
                       " '" & strDescripcion & "' ," &
                       " '" & strNivel & "' ," &
                       " '" & strTipoPuesto & "' ," &
                       " '" & strEmpresaD & "' ," &
                       " '" & strN13D & "' ," &
                       " '" & strN12D & "' ," &
                       " '" & strN11D & "' ," &
                       " '" & strN10D & "' ," &
                       " '" & strN9D & "' ," &
                       " '" & strN8D & "' ," &
                       " '" & strN7D & "' ," &
                       " '" & strN6D & "' ," &
                       " '" & strN5D & "' ," &
                       " '" & strN4D & "' ," &
                       " '" & strN3D & "' ," &
                       " '" & strN2D & "' ," &
                       " '" & strN1D & "' ," &
                       " '" & strArea & "' ," &
                       " '" & strComentarios & "' ," &
                       " GETDATE(),'" & hdUsuario.Value & "'," & strEstatus & ",'" & strUNegocio & "','" & strPosiciones & "','" & strClaveJefe & "','" & strPuestoGiro & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPuesto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdPuestos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPuestos.RowCancelingEdit
        grdPuestos.ShowFooter = True
        grdPuestos.EditIndex = -1
        Call obtCatalogoPuesto()
    End Sub

    'TOOLTIPS
    Private Sub grdPuestos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPuestos.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            'Dim forcedCss As String = "alignCenter"
            ''TODO: change your col index:
            'e.Row.Cells(2).CssClass = forcedCss

        End If
        'PAGINACION CON IMAGEN DE AVANCE
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim tb As New Table
            tb = e.Row.Cells(0).Controls(0)
            For Each pageCell As TableCell In tb.Rows(0).Cells
                'valida que se acontrol ImageButton
                Dim lnk As ImageButton
                lnk = pageCell.Controls(0)
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdPuestos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPuestos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            If validaPuestosAsignados(odbConexion, strId) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Puesto><.');</script>", False)
                Exit Sub
            End If

            strQuery = "DELETE FROM DO_PUESTOS_TB WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPuestos.EditIndex = -1
            grdPuestos.ShowFooter = True
            Call obtCatalogoPuesto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaPuestosAsignados(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) FROM DO_PUESTOS_TB WHERE " &
            " id<>" & id.ToString & " and (     [n13]=" & id &
            " OR [n12] =" & id &
            " OR [n11] =" & id &
            " OR [n10] =" & id &
            " OR [n9]	 =" & id &
            " OR [n8]	 =" & id &
            " OR [n7]	 =" & id &
            " OR [n6]	 =" & id &
            " OR [n5]	 =" & id &
            " OR [n4]	 =" & id &
            " OR [n3]	 =" & id &
            " OR [n2]	 =" & id &
            " OR [n1]	 =" & id & ")"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdPuestos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPuestos.RowEditing
        grdPuestos.ShowFooter = False
        grdPuestos.EditIndex = e.NewEditIndex
        Call obtCatalogoPuesto()
    End Sub
    'actualiza la descripcion
    Private Sub grdPuestos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPuestos.RowUpdating
        grdPuestos.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strEstatus As String = ""
        Try
            odbConexion.Open()
            strId = DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            Dim strCodigo As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("txtCodigo"), TextBox).Text)
            Dim strDescripcion As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("txtDecripcion"), TextBox).Text)
            Dim strNivel As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlNivel"), DropDownList).Text)
            Dim strTipoPuesto As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlTipoPuesto"), DropDownList).Text)
            Dim strEmpresaD As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList).Text)
            Dim strN13D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN13"), DropDownList).Text)
            Dim strN12D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN12"), DropDownList).Text)
            Dim strN11D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN11"), DropDownList).Text)
            Dim strN10D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN10"), DropDownList).Text)
            Dim strN9D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN9"), DropDownList).Text)
            Dim strN8D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN8"), DropDownList).Text)
            Dim strN7D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN7"), DropDownList).Text)
            Dim strN6D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN6"), DropDownList).Text)
            Dim strN5D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN5"), DropDownList).Text)
            Dim strN4D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN4"), DropDownList).Text)
            Dim strN3D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN3"), DropDownList).Text)
            Dim strN2D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN2"), DropDownList).Text)
            Dim strN1D As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlN1"), DropDownList).Text)
            Dim strArea As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlAreaAdscribe"), DropDownList).Text)
            Dim strComentarios As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("txtComentarios"), TextBox).Text)
            Dim strUNegocio As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlUNegocio"), DropDownList).Text)
            Dim strPosiciones As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("txtPosicion"), TextBox).Text)
            Dim strClaveJefe As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlClaveJefe"), DropDownList).Text)
            Dim strPuestoGiro As String = (DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlPuestoGiro"), DropDownList).Text)
            strEstatus = DirectCast(grdPuestos.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strCodigo = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el código.');</script>", False)
                Exit Sub
            End If
            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If
            If strNivel = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar el nivel.');</script>", False)
                Exit Sub
            End If
            If strEstatus = "0" Then
                If validaPuestosAsignados(odbConexion, strId) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede Desabilitar porque esta asociado a un Puesto.');</script>", False)
                    Exit Sub
                End If

            End If
            strPosiciones = IIf(strPosiciones = "", "0", strPosiciones)

            strQuery = "UPDATE [dbo].[DO_PUESTOS_TB] " &
                       "  SET [codigo] = '" & strCodigo & "'" &
                       "     ,[descripcion] = '" & strDescripcion & "'" &
                       "     ,[nivel] = '" & strNivel & "'" &
                       "     ,[tipo_puesto] = '" & strTipoPuesto & "'" &
                       "     ,[empresa] = '" & strEmpresaD & "'" &
                       "     ,[n13] = '" & strN13D & "'" &
                       "     ,[n12] = '" & strN12D & "'" &
                       "     ,[n11] = '" & strN11D & "'" &
                       "     ,[n10] = '" & strN10D & "'" &
                       "     ,[n9] = '" & strN9D & "'" &
                       "     ,[n8] = '" & strN8D & "'" &
                       "     ,[n7] = '" & strN7D & "'" &
                       "     ,[n6] = '" & strN6D & "'" &
                       "     ,[n5] = '" & strN5D & "'" &
                       "     ,[n4] = '" & strN4D & "'" &
                       "     ,[n3] = '" & strN3D & "'" &
                       "     ,[n2] = '" & strN2D & "'" &
                       "     ,[n1] = '" & strN1D & "'" &
                       "     ,[area_adscribe] = '" & strArea & "'" &
                       "     ,[comentarios] = '" & strComentarios & "'" &
                       "     ,[unidad_negocio] = '" & strUNegocio & "'" &
                       "     ,[estatus] = '" & strEstatus & "'" &
                       "     ,[posiciones] = '" & strPosiciones & "'" &
                       "     ,[jefe] = '" & strClaveJefe & "'" &
                       "     ,[clave_giro] = '" & strPuestoGiro & "'" &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPuestos.EditIndex = -1
            Call obtCatalogoPuesto()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarPuesto_Click(sender As Object, e As EventArgs)
        Call insPuestos()
    End Sub

    Protected Sub grdPuestos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPuestos.PageIndexChanging
        grdPuestos.ShowFooter = True
        grdPuestos.PageIndex = e.NewPageIndex
        grdPuestos.DataBind()
        Call obtCatalogoPuesto()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionServiciosProve(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_PUESTOS_TB where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Carga de Combos"
    'Obtiene el Nivel
    Public Sub obtddlNivel(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT ID , DESCRIPCION  FROM DO_NIVEL_CT  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "DESCRIPCION"
        ddl.DataValueField = "ID"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    'Obtiene el Area Adscribe
    Public Sub obtddlAreaAdscribe(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT CLAVE , DESCRIPCION  FROM DO_AREA_ADSCRIPCION_VT  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "DESCRIPCION"
        ddl.DataValueField = "CLAVE"

        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(" ", 0))
        odbConexion.Close()
    End Sub
    'Obtiene el Puestos GURO
    Public Sub obtddlPuestoGIRO(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = " SELECT CLAVE , DESCRIPCION  FROM DO_PUESTO_VT  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "DESCRIPCION"
        ddl.DataValueField = "CLAVE"

        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem("", 0))
        odbConexion.Close()
    End Sub
    'Obtiene el Puesto
    Public Sub obtddlPuesto(ByVal ddl As DropDownList, ByVal dsPuesto As DataSet)
        Dim dsDatos As New DataSet
        dsDatos = dsPuesto.Copy

        ddl.DataSource = dsDatos.Tables(0)
        ddl.DataTextField = "DESCRIPCION"
        ddl.DataValueField = "ID"

        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(" ", 0))
        dsDatos.Dispose()

    End Sub
    'Obtiene el Dataset Puesto
    Public Function obtDsPuesto(odbConexion As OleDbConnection)
        Dim strQuery As String
        Dim dsDatos As New DataSet
        Dim odbAdaptador As New OleDbDataAdapter

        strQuery = " SELECT ID,  descripcion AS descripcion  FROM DO_PUESTOS_TB WHERE ESTATUS=1 ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbAdaptador.SelectCommand = odbComando

        odbAdaptador.Fill(dsDatos)
        odbAdaptador.Dispose()
        odbComando.Dispose()

        Return dsDatos
    End Function
    Public Function obtTextoNivel(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DO_NIVEL_CT  WHERE ID='" & strid & "'"

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

    Public Function obtTextoPuesto(strid As String, dsPuestos As DataSet) As String
        Dim strResultado As String = ""
        'Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        'Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        'Dim dsCatalogo As New DataSet
        'Dim strQuery As String


        'Dim odbLector As OleDbDataReader
        'If strid <> "" And strid <> "0" Then
        '    odbConexion.Open()

        '    strQuery = " SELECT descripcion FROM DO_PUESTOS_TB  WHERE id='" & strid & "'"

        '    Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        '    odbLector = odbComando.ExecuteReader
        '    If odbLector.HasRows Then
        '        odbLector.Read()
        '        strResultado = odbLector(0).ToString
        '        odbLector.Close()
        '    End If

        '    odbConexion.Close()
        'End If

        Dim dsDatos As New DataSet
        dsDatos = dsPuestos.Copy
        Dim dataviewHijo As New DataView(dsDatos.Tables(0))

        dataviewHijo.RowFilter = dsDatos.Tables(0).Columns(0).ColumnName + "=" + IIf(strid = "", "0", strid)

        For Each dataRowCurrent As DataRowView In dataviewHijo
            strResultado = dataRowCurrent("descripcion").ToString().Trim()
        Next
        dataviewHijo.Dispose()
        dsDatos.Dispose()

        Return strResultado
    End Function

#End Region
#Region "Comportamientos"
    Public Sub inicialiciaTabs()
        'Tabs DNC
        hdIdTabReclutamiento.Value = 2

    End Sub
    Public Sub comportamientos()
        'Comportamientos de Catalogos
        Call remueveEstilosTabRec()
        Call AsignaEstilosTabRec()
        Call AsignaEstiloTabRecActive()

        Call formatoGrids()
    End Sub
    'asigna la propiedad de Active al Tab
    Public Sub AsignaEstiloTabRecActive()
        'asigna estatus active
        If hdIdTabReclutamiento.Value = 2 Then
            tabReclutamiento_2.Attributes.Add("class", "tab-pane active") '
            lnkTabRec2.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 3 Then
            tabReclutamiento_3.Attributes.Add("class", "tab-pane active") '
            lnkTabRec3.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 4 Then
            tabReclutamiento_4.Attributes.Add("class", "tab-pane active") '
            lnkTabRec4.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 5 Then
            tabReclutamiento_5.Attributes.Add("class", "tab-pane active") '
            lnkTabRec5.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 6 Then
            tabReclutamiento_6.Attributes.Add("class", "tab-pane active") '
            lnkTabRec6.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 7 Then
            tabReclutamiento_7.Attributes.Add("class", "tab-pane active") '
            lnkTabRec7.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 8 Then
            tabReclutamiento_8.Attributes.Add("class", "tab-pane active") '
            lnkTabRec8.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 9 Then
            tabReclutamiento_9.Attributes.Add("class", "tab-pane active") '
            lnkTabRec9.Attributes.Add("class", "active")
        ElseIf hdIdTabReclutamiento.Value = 10 Then
            tabReclutamiento_10.Attributes.Add("class", "tab-pane active") '
            lnkTabRec10.Attributes.Add("class", "active")

        End If
    End Sub
    'remueva los estyilos de las tabs
    Public Sub remueveEstilosTabRec()

        tabReclutamiento_2.Attributes.Remove("class") '
        tabReclutamiento_3.Attributes.Remove("class") '
        tabReclutamiento_4.Attributes.Remove("class")
        tabReclutamiento_5.Attributes.Remove("class")
        tabReclutamiento_6.Attributes.Remove("class")
        tabReclutamiento_7.Attributes.Remove("class")
        tabReclutamiento_8.Attributes.Remove("class")
        tabReclutamiento_9.Attributes.Remove("class")
        tabReclutamiento_10.Attributes.Remove("class")


        lnkTabRec2.Attributes.Remove("class")
        lnkTabRec3.Attributes.Remove("class")
        lnkTabRec4.Attributes.Remove("class")
        lnkTabRec5.Attributes.Remove("class")
        lnkTabRec6.Attributes.Remove("class")
        lnkTabRec7.Attributes.Remove("class")
        lnkTabRec8.Attributes.Remove("class")
        lnkTabRec9.Attributes.Remove("class")
        lnkTabRec10.Attributes.Remove("class")

    End Sub
    'asigna comportamientos de Tab
    Public Sub AsignaEstilosTabRec()

        tabReclutamiento_2.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_3.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_4.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_5.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_6.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_7.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_8.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_9.Attributes.Add("class", "tab-pane") '
        tabReclutamiento_10.Attributes.Add("class", "tab-pane") '
    End Sub

    Public Sub formatoGrids()

        'Puestos
        For iFil As Integer = 0 To grdPuestos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPuestos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Carreras
        For iFil As Integer = 0 To grdCarreras.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCarreras.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Escolaridad
        For iFil As Integer = 0 To grdEscolaridad.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdEscolaridad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Facultades
        For iFil As Integer = 0 To grdFacultades.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdFacultades.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Habilidades
        For iFil As Integer = 0 To grdHabilidades.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdHabilidades.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Idiomas
        For iFil As Integer = 0 To grdIdioma.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdIdioma.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Nivel
        For iFil As Integer = 0 To grdNivel.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdNivel.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Puesto
        For iFil As Integer = 0 To grdPuestosAgre.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPuestosAgre.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Rol
        For iFil As Integer = 0 To grdRoles.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdRoles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'Puesto-Rol
        For iFil As Integer = 0 To grdPuestoRol.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdPuestoRol.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
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
    '************************* - Catalogos Generales - *******************************************
#Region "Grid Carreras"
    'obtiene el catalogo 
    Public Sub obtCatalogoCarreras()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_CARRERAS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdCarreras.DataSource = dsCatalogo.Tables(0).DefaultView
            grdCarreras.DataBind()

            If grdCarreras.Rows.Count = 0 Then
                Call insFilaVaciaCarreras()
                grdCarreras.Rows(0).Visible = False

            Else
                grdCarreras.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdCarreras.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdCarreras.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdCarreras.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdCarreras.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdCarreras.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdCarreras.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdCarreras.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaCarreras()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdCarreras.DataSource = dt.DefaultView
        grdCarreras.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionCarreras()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdCarreras.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdCarreras.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionCarreras(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_CARRERAS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoCarreras()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdCarreras_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdCarreras.RowCancelingEdit
        grdCarreras.ShowFooter = True
        grdCarreras.EditIndex = -1
        Call obtCatalogoCarreras()
    End Sub

    'TOOLTIPS
    Private Sub grdCarreras_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCarreras.RowDataBound

        For i As Integer = 0 To grdCarreras.Rows.Count - 1

            Dim btnEditar As LinkButton = grdCarreras.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdCarreras.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdCarreras.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdCarreras.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdCarreras.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdCarreras_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCarreras.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdCarreras.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaCarreras(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_CARRERAS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdCarreras.EditIndex = -1
            grdCarreras.ShowFooter = True
            Call obtCatalogoCarreras()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaCarreras(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdCarreras_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCarreras.RowEditing
        grdCarreras.ShowFooter = False
        grdCarreras.EditIndex = e.NewEditIndex
        Call obtCatalogoCarreras()
    End Sub
    'actualiza la descripcion
    Private Sub grdCarreras_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdCarreras.RowUpdating
        grdCarreras.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdCarreras.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdCarreras.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdCarreras.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionCarreras(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_CARRERAS_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdCarreras.EditIndex = -1
            Call obtCatalogoCarreras()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub



    Protected Sub lnkAgregarCarreras_Click(sender As Object, e As EventArgs)
        Call insDescripcionCarreras()
    End Sub

    Protected Sub grdCarreras_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCarreras.PageIndexChanging
        grdCarreras.ShowFooter = True
        grdCarreras.PageIndex = e.NewPageIndex
        grdCarreras.DataBind()
        Call obtCatalogoCarreras()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionCarreras(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_CARRERAS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Grid Escolaridad"
    'obtiene el catalogo 
    Public Sub obtCatalogoEscolaridad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_ESCOLARIDAD_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdEscolaridad.DataSource = dsCatalogo.Tables(0).DefaultView
            grdEscolaridad.DataBind()

            If grdEscolaridad.Rows.Count = 0 Then
                Call insFilaVaciaEscolaridad()
                grdEscolaridad.Rows(0).Visible = False

            Else
                grdEscolaridad.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdEscolaridad.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdEscolaridad.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdEscolaridad.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdEscolaridad.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdEscolaridad.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdEscolaridad.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdEscolaridad.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaEscolaridad()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdEscolaridad.DataSource = dt.DefaultView
        grdEscolaridad.DataBind()


    End Sub



    'inserta registro al catalogo
    Public Sub insDescripcionEscolaridad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdEscolaridad.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdEscolaridad.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEscolaridad(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_ESCOLARIDAD_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoEscolaridad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdEscolaridad_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdEscolaridad.RowCancelingEdit
        grdEscolaridad.ShowFooter = True
        grdEscolaridad.EditIndex = -1
        Call obtCatalogoEscolaridad()
    End Sub

    'TOOLTIPS
    Private Sub grdEscolaridad_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdEscolaridad.RowDataBound

        For i As Integer = 0 To grdEscolaridad.Rows.Count - 1

            Dim btnEditar As LinkButton = grdEscolaridad.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdEscolaridad.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEscolaridad.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdEscolaridad.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdEscolaridad.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdEscolaridad_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdEscolaridad.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdEscolaridad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaEscolaridad(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_ESCOLARIDAD_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEscolaridad.EditIndex = -1
            grdEscolaridad.ShowFooter = True
            Call obtCatalogoEscolaridad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaEscolaridad(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdEscolaridad_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdEscolaridad.RowEditing
        grdEscolaridad.ShowFooter = False
        grdEscolaridad.EditIndex = e.NewEditIndex
        Call obtCatalogoEscolaridad()
    End Sub
    'actualiza la descripcion
    Private Sub grdEscolaridad_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdEscolaridad.RowUpdating
        grdEscolaridad.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdEscolaridad.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdEscolaridad.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdEscolaridad.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionEscolaridad(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_ESCOLARIDAD_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdEscolaridad.EditIndex = -1
            Call obtCatalogoEscolaridad()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarEscolaridad_Click(sender As Object, e As EventArgs)
        Call insDescripcionEscolaridad()
    End Sub

    Protected Sub grdEscolaridad_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEscolaridad.PageIndexChanging
        grdEscolaridad.ShowFooter = True
        grdEscolaridad.PageIndex = e.NewPageIndex
        grdEscolaridad.DataBind()
        Call obtCatalogoEscolaridad()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionEscolaridad(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_ESCOLARIDAD_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Grid Facultades"
    'obtiene el catalogo 
    Public Sub obtCatalogoFacultades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_FACULTADES_AUTORIZACION_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdFacultades.DataSource = dsCatalogo.Tables(0).DefaultView
            grdFacultades.DataBind()

            If grdFacultades.Rows.Count = 0 Then
                Call insFilaVaciaFacultades()
                grdFacultades.Rows(0).Visible = False

            Else
                grdFacultades.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdFacultades.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdFacultades.Rows(i).Controls(3).Controls(0)
                Dim btnEliminar As LinkButton = grdFacultades.Rows(i).Controls(4).Controls(1)

                iId = DirectCast(grdFacultades.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdFacultades.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdFacultades.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
                'validaciones de Sin Facultades
                btnEditar.Visible = IIf(iId = "1", False, True)
                btnEliminar.Visible = IIf(iId = "1", False, True)
            Next

            'Efectiva
            For iFil As Integer = 0 To grdFacultades.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdFacultades.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaFacultades()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdFacultades.DataSource = dt.DefaultView
        grdFacultades.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionFacultades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdFacultades.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdFacultades.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionFacultades(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_FACULTADES_AUTORIZACION_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoFacultades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdFacultades_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdFacultades.RowCancelingEdit
        grdFacultades.ShowFooter = True
        grdFacultades.EditIndex = -1
        Call obtCatalogoFacultades()
    End Sub

    'TOOLTIPS
    Private Sub grdFacultades_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdFacultades.RowDataBound

        For i As Integer = 0 To grdFacultades.Rows.Count - 1

            Dim btnEditar As LinkButton = grdFacultades.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdFacultades.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdFacultades.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdFacultades.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdFacultades.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdFacultades_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdFacultades.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdFacultades.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaFacultades(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_FACULTADES_AUTORIZACION_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdFacultades.EditIndex = -1
            grdFacultades.ShowFooter = True
            Call obtCatalogoFacultades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaFacultades(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdFacultades_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdFacultades.RowEditing
        grdFacultades.ShowFooter = False
        grdFacultades.EditIndex = e.NewEditIndex
        Call obtCatalogoFacultades()
    End Sub
    'actualiza la descripcion
    Private Sub grdFacultades_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdFacultades.RowUpdating
        grdFacultades.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdFacultades.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdFacultades.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdFacultades.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionFacultades(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_FACULTADES_AUTORIZACION_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdFacultades.EditIndex = -1
            Call obtCatalogoFacultades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarFacultades_Click(sender As Object, e As EventArgs)
        Call insDescripcionFacultades()
    End Sub

    Protected Sub grdFacultades_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdFacultades.PageIndexChanging
        grdFacultades.ShowFooter = True
        grdFacultades.PageIndex = e.NewPageIndex
        grdFacultades.DataBind()
        Call obtCatalogoFacultades()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionFacultades(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_FACULTADES_AUTORIZACION_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Grid Habilidades"
    'obtiene el catalogo 
    Public Sub obtCatalogoHabilidades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_HABILIDADES_COMPETENCIAS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdHabilidades.DataSource = dsCatalogo.Tables(0).DefaultView
            grdHabilidades.DataBind()

            If grdHabilidades.Rows.Count = 0 Then
                Call insFilaVaciaHabilidades()
                grdHabilidades.Rows(0).Visible = False

            Else
                grdHabilidades.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdHabilidades.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdHabilidades.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdHabilidades.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdHabilidades.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdHabilidades.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdHabilidades.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdHabilidades.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaHabilidades()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdHabilidades.DataSource = dt.DefaultView
        grdHabilidades.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionHabilidades()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdHabilidades.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdHabilidades.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHabilidades(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_HABILIDADES_COMPETENCIAS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdHabilidades_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdHabilidades.RowCancelingEdit
        grdHabilidades.ShowFooter = True
        grdHabilidades.EditIndex = -1
        Call obtCatalogoHabilidades()
    End Sub

    'TOOLTIPS
    Private Sub grdHabilidades_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdHabilidades.RowDataBound

        For i As Integer = 0 To grdHabilidades.Rows.Count - 1

            Dim btnEditar As LinkButton = grdHabilidades.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdHabilidades.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdHabilidades.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdHabilidades.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdHabilidades.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdHabilidades_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdHabilidades.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdHabilidades.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaHabilidades(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_HABILIDADES_COMPETENCIAS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHabilidades.EditIndex = -1
            grdHabilidades.ShowFooter = True
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaHabilidades(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdHabilidades_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdHabilidades.RowEditing
        grdHabilidades.ShowFooter = False
        grdHabilidades.EditIndex = e.NewEditIndex
        Call obtCatalogoHabilidades()
    End Sub
    'actualiza la descripcion
    Private Sub grdHabilidades_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdHabilidades.RowUpdating
        grdHabilidades.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdHabilidades.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdHabilidades.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdHabilidades.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionHabilidades(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_HABILIDADES_COMPETENCIAS_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdHabilidades.EditIndex = -1
            Call obtCatalogoHabilidades()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarHabilidades_Click(sender As Object, e As EventArgs)
        Call insDescripcionHabilidades()
    End Sub

    Protected Sub grdHabilidades_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHabilidades.PageIndexChanging
        grdHabilidades.ShowFooter = True
        grdHabilidades.PageIndex = e.NewPageIndex
        grdHabilidades.DataBind()
        Call obtCatalogoHabilidades()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionHabilidades(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_HABILIDADES_COMPETENCIAS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If

        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Grid Idioma"
    'obtiene el catalogo 
    Public Sub obtCatalogoIdioma()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_IDIOMAS_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdIdioma.DataSource = dsCatalogo.Tables(0).DefaultView
            grdIdioma.DataBind()

            If grdIdioma.Rows.Count = 0 Then
                Call insFilaVaciaIdioma()
                grdIdioma.Rows(0).Visible = False

            Else
                grdIdioma.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdIdioma.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdIdioma.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdIdioma.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdIdioma.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdIdioma.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdIdioma.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdIdioma.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaIdioma()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdIdioma.DataSource = dt.DefaultView
        grdIdioma.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionIdioma()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdIdioma.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdIdioma.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionIdioma(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_IDIOMAS_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoIdioma()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdIdioma_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdIdioma.RowCancelingEdit
        grdIdioma.ShowFooter = True
        grdIdioma.EditIndex = -1
        Call obtCatalogoIdioma()
    End Sub

    'TOOLTIPS
    Private Sub grdIdioma_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdIdioma.RowDataBound

        For i As Integer = 0 To grdIdioma.Rows.Count - 1

            Dim btnEditar As LinkButton = grdIdioma.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdIdioma.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdIdioma.Rows(i).Controls(1).Controls(1), Label).Text.Trim + "?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdIdioma.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción " + DirectCast(grdIdioma.Rows(i).Controls(1).Controls(1), TextBox).Text.Trim + "?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub


    'elimina fila
    Private Sub grdIdioma_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdIdioma.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdIdioma.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaIdioma(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_IDIOMAS_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdIdioma.EditIndex = -1
            grdIdioma.ShowFooter = True
            Call obtCatalogoIdioma()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaIdioma(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdIdioma_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdIdioma.RowEditing
        grdIdioma.ShowFooter = False
        grdIdioma.EditIndex = e.NewEditIndex
        Call obtCatalogoIdioma()
    End Sub
    'actualiza la descripcion
    Private Sub grdIdioma_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdIdioma.RowUpdating
        grdIdioma.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdIdioma.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdIdioma.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdIdioma.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionIdioma(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_IDIOMAS_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdIdioma.EditIndex = -1
            Call obtCatalogoIdioma()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Protected Sub lnkAgregarIdioma_Click(sender As Object, e As EventArgs)
        Call insDescripcionIdioma()
    End Sub

    Protected Sub grdIdioma_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdIdioma.PageIndexChanging
        grdIdioma.ShowFooter = True
        grdIdioma.PageIndex = e.NewPageIndex
        grdIdioma.DataBind()
        Call obtCatalogoIdioma()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionIdioma(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_IDIOMAS_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region
#Region "Grid Nivel"
    'obtiene el catalogo 
    Public Sub obtCatalogoNivel()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_NIVEL_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdNivel.DataSource = dsCatalogo.Tables(0).DefaultView
            grdNivel.DataBind()

            If grdNivel.Rows.Count = 0 Then
                Call insFilaVaciaNivel()
                grdNivel.Rows(0).Visible = False

            Else
                grdNivel.Rows(0).Visible = True

            End If

            odbConexion.Close()


            Dim i As Int16 = 0

            For i = 0 To grdNivel.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdNivel.Rows(i).Controls(3).Controls(0)

                iId = DirectCast(grdNivel.Rows(i).Cells(0).FindControl("lblId"), Label).Text

                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdNivel.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdNivel.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdNivel.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdNivel.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'inserta fila vacia cuando no exista ningun registro
    Public Sub insFilaVaciaNivel()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdNivel.DataSource = dt.DefaultView
        grdNivel.DataBind()


    End Sub

    'inserta registro al catalogo
    Public Sub insDescripcionNivel()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdNivel.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text)
            strEstatus = (DirectCast(grdNivel.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionNivel(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_NIVEL_CT (descripcion,estatus,fecha_creacion,usuario_creacion) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoNivel()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdNivel_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdNivel.RowCancelingEdit
        grdNivel.ShowFooter = True
        grdNivel.EditIndex = -1
        Call obtCatalogoNivel()
    End Sub

    'TOOLTIPS
    Private Sub grdNivel_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdNivel.RowDataBound

        For i As Integer = 0 To grdNivel.Rows.Count - 1

            Dim btnEditar As LinkButton = grdNivel.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdNivel.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdNivel.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub

    'elimina fila
    Private Sub grdNivel_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdNivel.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdNivel.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            'If validaNivel(odbConexion, strId) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No se puede eliminar porque esta asociado a un Proveedor.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "DELETE FROM DO_NIVEL_CT WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdNivel.EditIndex = -1
            grdNivel.ShowFooter = True
            Call obtCatalogoNivel()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    'valida si exise relacion con Reporte
    Public Function validaNivel(odbConexion As OleDbConnection, id As Integer) As Boolean
        Dim bResultados As Boolean = False
        Dim strQuery As String

        strQuery = "SELECT COUNT(*) from [DO_IDENTIFICACION_COMPETENCIA_PUESTO_TB] where [carrera_especializacion] like '%" & id.ToString & "%'"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader

        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            bResultados = IIf(odbLector(0) > 0, True, False)
            odbLector.Close()
        End If

        Return bResultados
    End Function

    'habilita el modo edicion
    Private Sub grdNivel_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdNivel.RowEditing
        grdNivel.ShowFooter = False
        grdNivel.EditIndex = e.NewEditIndex
        Call obtCatalogoNivel()
    End Sub
    'actualiza la descripcion
    Private Sub grdNivel_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdNivel.RowUpdating
        grdNivel.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdNivel.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdNivel.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdNivel.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionNivel(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_NIVEL_CT " &
                        "SET DESCRIPCION='" & strDescripcion & "' " &
                         ",ESTATUS=" & strEstatus &
                        ",fecha_modificacion=GETDATE() " &
                        ",usuario_modificacion='" & hdUsuario.Value & "' " &
                        "WHERE ID=" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdNivel.EditIndex = -1
            Call obtCatalogoNivel()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarNivel_Click(sender As Object, e As EventArgs)
        Call insDescripcionNivel()
    End Sub
    Protected Sub grdNivel_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdNivel.PageIndexChanging
        grdNivel.ShowFooter = True
        grdNivel.PageIndex = e.NewPageIndex
        grdNivel.DataBind()
        Call obtCatalogoNivel()
    End Sub
    'valida si la descripcion ya existe en el catalogo
    Public Function validaDescripcionNivel(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_NIVEL_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function
#End Region

#Region "Grid Puestos"


    Protected Sub grdPuestosAgre_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPuestosAgre.PageIndexChanging
        grdPuestosAgre.ShowFooter = True
        grdPuestosAgre.PageIndex = e.NewPageIndex
        grdPuestosAgre.DataBind()
        Call obtCatalogoPuestos()
    End Sub

    Public Sub obtCatalogoPuestos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_PUESTO_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)
            Dim dsCatalogo As New DataSet
            odbComando.Fill(dsCatalogo)

            grdPuestosAgre.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPuestosAgre.DataBind()

            If grdPuestosAgre.Rows.Count = 0 Then
                Call insFilaVaciaPuesto()
                grdPuestosAgre.Rows(0).Visible = False
            Else
                grdPuestosAgre.Rows(0).Visible = True
            End If

            odbConexion.Close()

            For iFil As Integer = 0 To grdPuestosAgre.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPuestosAgre.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Public Sub insFilaVaciaPuesto()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPuestosAgre.DataSource = dt.DefaultView
        grdPuestosAgre.DataBind()


    End Sub

    Private Sub grdPuestosAgre_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPuestosAgre.RowEditing
        grdPuestosAgre.ShowFooter = False
        grdPuestosAgre.EditIndex = e.NewEditIndex
        Call obtCatalogoPuestos()
    End Sub

    'actualiza la descripcion
    Private Sub grdPuestosAgre_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPuestosAgre.RowUpdating
        grdPuestosAgre.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdPuestosAgre.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdPuestosAgre.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdPuestosAgre.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuesto(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_PUESTO_CT " &
                        "SET descripcion = '" & strDescripcion & "', " &
                            "estatus = " & strEstatus & " " &
                        "WHERE id = " & strId


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPuestosAgre.EditIndex = -1
            Call obtCatalogoPuestos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdPuestosAgre_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPuestosAgre.RowCancelingEdit
        grdPuestosAgre.ShowFooter = True
        grdPuestosAgre.EditIndex = -1
        Call obtCatalogoPuestos()
    End Sub

    'TOOLTIPS
    Private Sub grdPuestosAgre_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPuestosAgre.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Obtener el valor de estatus
            Dim status As String = DataBinder.Eval(e.Row.DataItem, "estatus").ToString()

            ' Obtener el control Label donde se mostrará el estatus como texto
            Dim lblEstatus As Label = DirectCast(e.Row.FindControl("lblEstatus"), Label)

            If lblEstatus IsNot Nothing Then
                ' Convertir el valor numérico a texto
                If status = "1" Then
                    lblEstatus.Text = "Habilitado"
                Else
                    lblEstatus.Text = "Deshabilitado"
                End If
            End If
        End If

        For i As Integer = 0 To grdPuestosAgre.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPuestosAgre.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdPuestosAgre.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "If(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdPuestosAgre.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub

    Public Function validaDescripcionPuesto(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_PUESTO_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function

    Public Sub insDescripcionPuesto()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdPuestosAgre.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text).Trim()
            strEstatus = (DirectCast(grdPuestosAgre.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuesto(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_PUESTO_CT (descripcion,estatus,fecha_alta,usuario_alta) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPuestos()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarPuestosAgre_Click(sender As Object, e As EventArgs)
        Call insDescripcionPuesto()
    End Sub



#End Region

#Region "Grid Roles"


    Protected Sub grdRoles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdRoles.PageIndexChanging
        grdRoles.ShowFooter = True
        grdRoles.PageIndex = e.NewPageIndex
        grdRoles.DataBind()
        Call obtCatalogoRol()
    End Sub

    Public Sub obtCatalogoRol()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT * FROM DO_ROL_CT ORDER BY [DESCRIPCION]"

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)
            Dim dsCatalogo As New DataSet
            odbComando.Fill(dsCatalogo)

            grdRoles.DataSource = dsCatalogo.Tables(0).DefaultView
            grdRoles.DataBind()

            If grdRoles.Rows.Count = 0 Then
                Call insFilaVaciaRol()
                grdRoles.Rows(0).Visible = False
            Else
                grdRoles.Rows(0).Visible = True
            End If

            odbConexion.Close()
            For iFil As Integer = 0 To grdRoles.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdRoles.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Public Sub insFilaVaciaRol()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdRoles.DataSource = dt.DefaultView
        grdRoles.DataBind()


    End Sub

    Private Sub grdRoles_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdRoles.RowEditing
        grdRoles.ShowFooter = False
        grdRoles.EditIndex = e.NewEditIndex
        Call obtCatalogoRol()
    End Sub

    'actualiza la descripcion
    Private Sub grdRoles_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdRoles.RowUpdating
        grdRoles.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try
            strId = DirectCast(grdRoles.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            strDescripcion = DirectCast(grdRoles.Rows(e.RowIndex).FindControl("txtDescripcion"), TextBox).Text
            strEstatus = DirectCast(grdRoles.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionRol(strDescripcion, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_ROL_CT" &
                "SET descripcion = '" & strDescripcion & "' " &
                   ",usuario_alta = '" & hdUsuario.Value & "' " &
                    "estatus = " & strEstatus &
                "WHERE id =" & strId

            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdRoles.EditIndex = -1
            Call obtCatalogoRol()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdRoles_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdRoles.RowCancelingEdit
        grdRoles.ShowFooter = True
        grdRoles.EditIndex = -1
        Call obtCatalogoRol()
    End Sub

    'TOOLTIPS
    Private Sub grdRoles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRoles.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Obtener el valor de estatus
            Dim status As String = DataBinder.Eval(e.Row.DataItem, "estatus").ToString()

            ' Obtener el control Label donde se mostrará el estatus como texto
            Dim lblEstatus As Label = DirectCast(e.Row.FindControl("lblEstatus"), Label)

            If lblEstatus IsNot Nothing Then
                ' Convertir el valor numérico a texto
                If status = "1" Then
                    lblEstatus.Text = "Habilitado"
                Else
                    lblEstatus.Text = "Deshabilitado"
                End If
            End If
        End If

        For i As Integer = 0 To grdRoles.Rows.Count - 1

            Dim btnEditar As LinkButton = grdRoles.Rows(i).Controls(3).Controls(0)
            Dim btnEliminar As LinkButton = grdRoles.Rows(i).Controls(4).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "If(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdRoles.Rows(i).Controls(3).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub

    Public Function validaDescripcionRol(strDescripcion As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_ROL_CT where (descripcion='" & strDescripcion & "') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If
        End If
        odbConexion.Close()
        Return blResultado

    End Function

    Public Sub insDescripcionRol()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Try

            strDescripcion = (DirectCast(grdRoles.FooterRow.FindControl("txtAgreDescripcion"), TextBox).Text).Trim()
            strEstatus = (DirectCast(grdRoles.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionRol(strDescripcion, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_ROL_CT (descripcion,estatus,fecha_alta,usuario_alta) VALUES ('" &
                strDescripcion & "'," & strEstatus & ",GETDATE(),'" & hdUsuario.Value & "')"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoRol()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarRol_Click(sender As Object, e As EventArgs)
        Call insDescripcionRol()
    End Sub


#End Region

#Region "Grid Puestos-Roles"


    Protected Sub grdPuestoRol_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPuestoRol.PageIndexChanging
        grdPuestoRol.ShowFooter = True
        grdPuestoRol.PageIndex = e.NewPageIndex
        grdPuestoRol.DataBind()
        Call obtCatalogoPuestoRol()
    End Sub

    Public Sub obtCatalogoPuestoRol()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Try
            odbConexion.Open()
            strQuery = "SELECT a.id, a.fk_id_puesto, a.fk_id_rol, b.descripcion AS descripcion, c.descripcion AS rol , a.estatus FROM DO_PUESTO_ROL AS a 
                                 INNER JOIN dbo.DO_PUESTO_CT AS b ON a.fk_id_puesto = b.id
								 INNER JOIN dbo.DO_ROL_CT AS c ON a.fk_id_rol=c.id ORDER BY b.descripcion "

            Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

            odbComando.Fill(dsCatalogo)
            grdPuestoRol.DataSource = dsCatalogo.Tables(0).DefaultView
            grdPuestoRol.DataBind()

            If grdPuestoRol.Rows.Count = 0 Then
                Call insFilaVaciaPuestoRol()
                grdPuestoRol.Rows(0).Visible = False
            Else
                grdPuestoRol.Rows(0).Visible = True
            End If

            odbConexion.Close()

            Dim i As Int16 = 0
            For i = 0 To grdPuestoRol.Rows.Count - 1
                Dim iId As String
                Dim ddlEstatus As DropDownList
                Dim iEmpresa As Integer = 0
                Dim btnEditar As LinkButton = grdPuestoRol.Rows(i).Controls(4).Controls(0)

                iId = DirectCast(grdPuestoRol.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                If btnEditar.Text <> "Editar" Then
                    ddlEstatus = grdPuestoRol.Rows(i).Cells(2).FindControl("ddlEstatus")
                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iId Then
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(5).ToString
                        End If
                    Next
                Else
                    'empleado
                    Dim lblEstatus As New Label
                    lblEstatus = grdPuestoRol.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = IIf(lblEstatus.Text = "0", "Deshabilitado", "Habilitado")
                End If
            Next

            'Efectiva
            For iFil As Integer = 0 To grdPuestoRol.Rows.Count - 1
                If iFil Mod 2 = 0 Then
                    grdPuestoRol.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
                End If
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Public Sub insFilaVaciaPuestoRol()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("descripcion"))
        dt.Columns.Add(New DataColumn("rol"))
        dt.Columns.Add(New DataColumn("estatus"))
        dr = dt.NewRow
        dr("id") = ""
        dr("descripcion") = ""
        dr("rol") = ""
        dr("estatus") = ""
        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdPuestoRol.DataSource = dt.DefaultView
        grdPuestoRol.DataBind()


    End Sub

    Private Sub grdPuestoRol_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPuestoRol.RowEditing
        grdPuestoRol.ShowFooter = False
        grdPuestoRol.EditIndex = e.NewEditIndex
        Call obtCatalogoPuestoRol()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String

        strQuery = "SELECT a.id, a.fk_id_puesto, a.fk_id_rol, b.descripcion AS descripcion, c.descripcion AS rol , a.estatus FROM DO_PUESTO_ROL AS a 
                             INNER JOIN dbo.DO_PUESTO_CT AS b ON a.fk_id_puesto = b.id
                             INNER JOIN dbo.DO_ROL_CT AS c ON a.fk_id_rol=c.id ORDER BY b.descripcion "

        Dim odbComando As New OleDbDataAdapter(strQuery, odbConexion)

        odbComando.Fill(dsCatalogo)
        grdPuestoRol.DataSource = dsCatalogo.Tables(0).DefaultView
        grdPuestoRol.DataBind()

        If grdPuestoRol.EditIndex >= 0 AndAlso grdPuestoRol.Rows.Count > grdPuestoRol.EditIndex Then
            ' Obtener la fila en modo de edición
            Dim row As GridViewRow = grdPuestoRol.Rows(grdPuestoRol.EditIndex)

            ' Encontrar los controles DropDownList en la fila de edición
            Dim ddlDescripcion As DropDownList = CType(row.FindControl("ddlDescripcion"), DropDownList)
            Dim ddlRol As DropDownList = CType(row.FindControl("ddlRol"), DropDownList)
            Dim ddlEstatus As DropDownList = CType(row.FindControl("ddlEstatus"), DropDownList)

            ' Verificar si se encontraron los controles antes de intentar llenarlos
            If ddlDescripcion IsNot Nothing AndAlso ddlRol IsNot Nothing AndAlso ddlEstatus IsNot Nothing Then
                ' Limpiar los items existentes en los DropDownList
                ddlDescripcion.Items.Clear()
                ddlRol.Items.Clear()
                ' Llenar el DropDownList ddlDescripcion con los puestos
                sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_PUESTO_CT"
                Dim dvAgreDescripcion As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
                For Each drv As DataRowView In dvAgreDescripcion
                    Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
                    ddlDescripcion.Items.Add(item)
                Next

                ' Llenar el DropDownList ddlRol con los roles
                sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_ROL_CT"
                Dim dvAgreRol As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
                For Each drv As DataRowView In dvAgreRol
                    Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
                    ddlRol.Items.Add(item)
                Next

                ' Seleccionar el valor correspondiente a la fila en edición
                Dim id As String = DirectCast(row.FindControl("lblId"), Label).Text
                Dim dv As DataView = dsCatalogo.Tables(0).DefaultView
                dv.RowFilter = "id = '" & id & "'"
                If dv.Count > 0 Then
                    ddlDescripcion.SelectedValue = dv(0)("fk_id_puesto").ToString()
                    ddlRol.SelectedValue = dv(0)("fk_id_rol").ToString()
                    ddlEstatus.SelectedValue = dv(0)("estatus").ToString()
                End If
                ' Restablecer el filtro del DataView
                dv.RowFilter = ""
            End If
        End If
    End Sub


    'actualiza la descripcion
    Private Sub grdPuestoRol_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPuestoRol.RowUpdating
        grdPuestoRol.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Dim strEmpresa As String = ""

        Try
            strId = DirectCast(grdPuestoRol.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            Dim strRol As String = DirectCast(grdPuestoRol.Rows(e.RowIndex).FindControl("ddlRol"), DropDownList).SelectedValue
            Dim strDescripcion As String = DirectCast(grdPuestoRol.Rows(e.RowIndex).FindControl("ddlDescripcion"), DropDownList).SelectedValue
            Dim strEstatus As String = DirectCast(grdPuestoRol.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).SelectedValue


            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuestoRol(strDescripcion, strRol, 1) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If
            odbConexion.Open()

            strQuery = "UPDATE DO_PUESTO_ROL " &
           "SET fk_id_puesto = '" & strDescripcion & "', " &
               "fk_id_rol = '" & strRol & "', " &
               "estatus = " & strEstatus & " " &
           "WHERE id = " & strId


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)

            odbComando.ExecuteNonQuery()
            odbConexion.Close()

            grdPuestoRol.EditIndex = -1
            Call obtCatalogoPuestoRol()
            Call LlenarFormularioddl()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdPuestoRol_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPuestoRol.RowCancelingEdit
        grdPuestoRol.ShowFooter = True
        grdPuestoRol.EditIndex = -1
        Call obtCatalogoPuestoRol()
        Call LlenarFormularioddl()
    End Sub

    Private Sub grdPuestoRol_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPuestoRol.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Obtener el valor de estatus
            Dim status As String = DataBinder.Eval(e.Row.DataItem, "estatus").ToString()

            ' Obtener el control Label donde se mostrará el estatus como texto
            Dim lblEstatus As Label = DirectCast(e.Row.FindControl("lblEstatus"), Label)

            If lblEstatus IsNot Nothing Then
                ' Convertir el valor numérico a texto
                If status = "1" Then
                    lblEstatus.Text = "Habilitado"
                Else
                    lblEstatus.Text = "Deshabilitado"
                End If
            End If
        End If


        For i As Integer = 0 To grdPuestoRol.Rows.Count - 1

            Dim btnEditar As LinkButton = grdPuestoRol.Rows(i).Controls(4).Controls(0)
            Dim btnEliminar As LinkButton = grdPuestoRol.Rows(i).Controls(5).Controls(1)
            If btnEditar.Text = "Editar" Then
                btnEditar.ToolTip = "Editar Descripción"
                'JAVA SCRIPT
                btnEliminar.Attributes("onclick") = "If(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
            Else
                btnEditar.ToolTip = "Actualizar Descripción"
                Dim cancelar As LinkButton = grdPuestoRol.Rows(i).Controls(4).Controls(2)
                cancelar.ToolTip = "Cancelar Edición"

                btnEliminar.Attributes("onclick") = "if(!confirm('Desea Eliminar la Descripción ?')){ return false; };"
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
                lnk.Attributes.Add("onclick", "javascript:document.getElementById('loadingCatalogos').style.display = 'inline'")
            Next
        End If
    End Sub

    Public Function validaDescripcionPuestoRol(strDescripcion As String, strRol As String, Optional iExistencia As Integer = 0) As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DO_PUESTO_ROL WHERE (fk_id_puesto='" & strDescripcion & "' AND fk_id_rol= '" & strRol & "' )"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If iExistencia = 0 Then
                If odbLector(0) > 0 Then blResultado = True
            Else
                If odbLector(0) > 1 Then blResultado = True
            End If


        End If
        odbConexion.Close()
        Return blResultado

    End Function

    Public Sub insDescripcionPuestoRol()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strDescripcion As String = ""
        Dim strEmpresa As String = ""
        Dim strEstatus As String = ""
        Dim strRol As String = ""

        Try

            strDescripcion = (DirectCast(grdPuestoRol.FooterRow.FindControl("ddlAgreDescripcion"), DropDownList).Text)
            strEstatus = (DirectCast(grdPuestoRol.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)
            strRol = (DirectCast(grdPuestoRol.FooterRow.FindControl("ddlAgreRol"), DropDownList).Text)

            odbConexion.Open()

            If strDescripcion = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe capturar la Descripción.');</script>", False)
                Exit Sub
            End If

            If validaDescripcionPuestoRol(strDescripcion, strRol, 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Ya existe esa descripción.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO DO_PUESTO_ROL (fk_id_puesto, fk_id_rol ,fecha_alta ,usuario_alta, estatus) VALUES ('" &
                strDescripcion & "'," & strRol & ",GETDATE(),'" & hdUsuario.Value & "'," & strEstatus & " )"

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            odbConexion.Close()
            Call obtCatalogoPuestoRol()
            Call LlenarFormularioddl()
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkAgregarPuestoRol_Click(sender As Object, e As EventArgs)
        Call insDescripcionPuestoRol()
    End Sub



#End Region

#Region "DDL"

    Sub LlenarFormularioddl()

        Dim ddlAgreDescripcion As DropDownList = CType(grdPuestoRol.FooterRow.FindControl("ddlAgreDescripcion"), DropDownList)
        Dim ddlAgreRol As DropDownList = CType(grdPuestoRol.FooterRow.FindControl("ddlAgreRol"), DropDownList)

        sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_PUESTO_CT"
        Dim dvPuestos As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvPuestos
            Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
            ddlAgreDescripcion.Items.Add(item)
        Next

        sqlMain.SelectCommand = "SELECT id, descripcion FROM DO_ROL_CT"
        Dim dvRol As DataView = DirectCast(sqlMain.Select(DataSourceSelectArguments.Empty), DataView)
        For Each drv As DataRowView In dvRol
            Dim item As New ListItem(drv("descripcion").ToString(), drv("id").ToString())
            ddlAgreRol.Items.Add(item)
        Next

    End Sub

#End Region


End Class