Imports System.Data.OleDb
Imports System.DirectoryServices
Imports System.Drawing

Public Class gestiondnc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1)
        Response.CacheControl = "no-cache"
        lblError.Text = ""
        lblErrorGestion.Text = ""

        If Not Page.IsPostBack Then
            hdIdCurso.Value = 0
            hdIndexPrioridad.Value = 0
            Call obtenerUsuarioAD()
            Call obtIdDNC()
            Call CargaColaboradores()
            Call ObtDatosColaboradores()
        End If
        Call comportamiento()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Combos", "<script>combo()</script>", False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "scroll", "<script>gridviewScroll()</script>", False)
    End Sub
#Region "Administración de Cursos"
    Public Sub obtCursos(ByVal strColaborador As String)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strFiltro As String = ""
        Try
            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_muestra_lista_cursos_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)
            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCatalogo)
            grdCursos.DataSource = dsCatalogo.Tables(0).DefaultView
            grdCursos.DataBind()
            If grdCursos.Rows.Count = 0 Then
                Call insFilaVaciaCursos()
                grdCursos.Rows(0).Visible = False
                lblGrid.Text = ""
                btnGuardar.Visible = False
                hdIndexPrioridad.Value = 0
                lblGuardar.Text = ""
            Else
                grdCursos.Rows(0).Visible = True
                lblGrid.Text = ""
                btnGuardar.Visible = True
            End If

            For i = 0 To grdCursos.Rows.Count - 1
                Dim iIdCurso As String
                Dim ddlCursos As DropDownList
                Dim ddlPrioridad As DropDownList
                Dim ddlAnio As DropDownList
                Dim ddlMes As DropDownList
                Dim ddlMotivo As DropDownList
                Dim ddlObjetivo As DropDownList
                Dim ddlCompetencia As DropDownList
                Dim ddlTipoIndicador As DropDownList
                Dim ddlIndicador As DropDownList
                Dim ddlMedirEfectividad As DropDownList
                Dim ddlEstatus As DropDownList
                Dim strTipo As String = ""
                Dim btnEditar As LinkButton = grdCursos.Rows(i).Controls(14).Controls(0)
                Dim btnEliminar As LinkButton = grdCursos.Rows(i).Controls(15).Controls(1)
                Dim lnkMostrarTemario As New LinkButton
                Dim strNombreCurso As String = ""
                Dim strIdTemario As String = ""
                iIdCurso = DirectCast(grdCursos.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                lnkMostrarTemario.ID = "lnk_" & iIdCurso

                If btnEditar.Text <> "Editar" Then
                    ddlCursos = grdCursos.Rows(i).Cells(1).FindControl("ddlCurso")
                    ddlMotivo = grdCursos.Rows(i).Cells(4).FindControl("ddlMotivo")
                    ddlObjetivo = grdCursos.Rows(i).Cells(4).FindControl("ddlObjCorp")
                    ddlCompetencia = grdCursos.Rows(i).Cells(4).FindControl("ddlCompetencia")
                    ddlTipoIndicador = grdCursos.Rows(i).Cells(4).FindControl("ddlTIndidador")
                    ddlIndicador = grdCursos.Rows(i).Cells(4).FindControl("ddlIndicador")
                    ddlMedirEfectividad = grdCursos.Rows(i).Cells(4).FindControl("ddlMedicion")
                    ddlEstatus = grdCursos.Rows(i).Cells(4).FindControl("ddlEstatus")
                    'obtiene Curso
                    Call obtddlCursos(ddlCursos, 1)
                    'Año
                    Dim lblAño As New Label
                    lblAño = grdCursos.Rows(i).FindControl("lblAnio")
                    lblAño.Text = obtTextoAño(lblAño.Text)
                    'obtiene Motivo Configurado
                    Call obtddlMotivo(ddlMotivo)
                    'obtiene Objetivo Configurado
                    Call obtddlObjetivo(ddlObjetivo)
                    'obtiene Competencia Configurado
                    Call obtddlCompetencias(ddlCompetencia)
                    'obtiene Tipo Indicador Configurado
                    Call obtddlTipoIndicador(ddlTipoIndicador)

                    'obtiene Medir Efectividad
                    Call obtddlEfectividad(ddlMedirEfectividad)
                    'obtiene Medir Efectividad
                    Call obtddlEstatus(ddlEstatus)

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdCurso Then
                            ddlCursos.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(3).ToString
                            ddlMotivo.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(7).ToString
                            ddlObjetivo.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(8).ToString
                            ddlCompetencia.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(9).ToString
                            ddlTipoIndicador.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(10).ToString
                            'obtiene Indicador Configurado
                            Call obtddlIndicador(ddlIndicador, ddlTipoIndicador.SelectedValue)

                            ddlIndicador.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(11).ToString
                            ddlMedirEfectividad.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(12).ToString
                            ddlEstatus.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(13).ToString
                        End If
                    Next

                    strIdTemario = ddlCursos.SelectedValue
                    strNombreCurso = ddlCursos.SelectedItem.Text


                Else
                    'Curso

                    Dim lblCursos As New Label
                    lblCursos = grdCursos.Rows(i).FindControl("lblCurso")
                    strIdTemario = lblCursos.Text
                    lblCursos.Text = obtTextoCursos(lblCursos.Text)

                    'Motivo
                    Dim lblMotivo As New Label
                    lblMotivo = grdCursos.Rows(i).FindControl("lblMotivo")
                    lblMotivo.Text = obtTextoMotivo(lblMotivo.Text)
                    'Objetivo
                    Dim lblObjetivo As New Label
                    lblObjetivo = grdCursos.Rows(i).FindControl("lblObjCorp")
                    lblObjetivo.Text = obtTextoObjetivo(lblObjetivo.Text)
                    'Competencia
                    Dim lblCompetencia As New Label
                    lblCompetencia = grdCursos.Rows(i).FindControl("lblCompetencia")
                    lblCompetencia.Text = obtTextoCompetencias(lblCompetencia.Text)
                    'Tipo Indicador
                    Dim lblTipoIndicador As New Label
                    lblTipoIndicador = grdCursos.Rows(i).FindControl("lblTIndicador")
                    lblTipoIndicador.Text = obtTextoTipoIndicador(lblTipoIndicador.Text)
                    ' Indicador
                    Dim lblIndicador As New Label
                    lblIndicador = grdCursos.Rows(i).FindControl("lblIndicador")
                    lblIndicador.Text = obtTextoIndicador(lblIndicador.Text)

                    ' Efectividad
                    Dim lblMedirEfectividad As New Label
                    lblMedirEfectividad = grdCursos.Rows(i).FindControl("lblMedicion")
                    lblMedirEfectividad.Text = obtTextoEfectividad(lblMedirEfectividad.Text)
                    ' Estatus
                    Dim lblEstatus As New Label
                    lblEstatus = grdCursos.Rows(i).FindControl("lblEstatus")
                    lblEstatus.Text = obtTextoEstatus(lblEstatus.Text)

                    strNombreCurso = lblCursos.Text

                    'asigna comportamientos
                    ddlPrioridad = grdCursos.Rows(i).Cells(1).FindControl("ddlPrioridadItem")
                    ddlAnio = grdCursos.Rows(i).Cells(4).FindControl("ddlAnioItem")
                    ddlMes = grdCursos.Rows(i).Cells(4).FindControl("ddlMesItem")
                    'obtiene Prioridad asigna la fila del registro
                    Call obtddlPrioridad(ddlPrioridad, 0)
                    'ddlPrioridad.Attributes("onchange") = "PrioridadItem('" & i & "')"
                    'obtiene Año Configurado
                    Call obtddlAño(ddlAnio)

                    For iContador As Integer = 0 To dsCatalogo.Tables(0).Rows.Count - 1
                        If dsCatalogo.Tables(0).Rows(iContador)(0).ToString = iIdCurso Then
                            ddlPrioridad.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(4).ToString
                            ddlAnio.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(5).ToString
                            ddlMes.SelectedValue = dsCatalogo.Tables(0).Rows(iContador)(6).ToString

                        End If
                    Next


                End If
                strNombreCurso = (strNombreCurso).Trim

                lnkMostrarTemario = grdCursos.Rows(i).FindControl("lnkTemario")
                'lnkMostrarTemario.Text = "<i class='fa fa-file-o'></i>"
                lnkMostrarTemario.Text = "<span class='label label-danger letraTemario'>Temario</span>"
                'lnkMostrarTemario.Text = "<span class='btn btn-danger btn-flat btn-xs'>Temario</span>"
                lnkMostrarTemario.Attributes("data-toggle") = "modal"
                lnkMostrarTemario.Attributes("data-target") = "#modalTemario"
                lnkMostrarTemario.Attributes("OnClick") = "imgModalTemario('" & obtTemarioCursos(strIdTemario) & "'); javascript:document.getElementById('lblCursoTemario').innerHTML ='" & strNombreCurso.Trim & "';"

                btnEliminar.Attributes("onclick") = "if(!confirm('¿Desea Eliminar " + strNombreCurso + "?')){ return false; } else { document.getElementById('loadingPagina').style.display = 'inline'};"
            Next

            Call comportamiento()

            'Cursos
            Dim ddlAgregaCurso As DropDownList
            ddlAgregaCurso = grdCursos.FooterRow.FindControl("ddlAgreCurso")
            'manda 0 porq es para gregar
            Call obtddlCursos(ddlAgregaCurso, 0)
            If ddlAgregaCurso.Items.Count = 0 Then
                Exit Sub
            Else
                lblColaborador.Text = ""
            End If


            'Temarios
            Dim lnkTemario As New LinkButton
            Dim strNombreCursoAgre As String = ""
            lnkTemario = grdCursos.FooterRow.FindControl("lnkAgreTemario")
            'lnkTemario.Text = "<i class='fa fa-file-o'></i>"
            lnkTemario.Text = "<span class='label label-danger letraTemario'>Temario</span>"
            lnkTemario.Attributes("data-toggle") = "modal"
            lnkTemario.Attributes("data-target") = "#modalTemario"
            lnkTemario.ID = "lnk_Agre"
            strNombreCursoAgre = (ddlAgregaCurso.SelectedItem.Text.Trim)
            lnkTemario.Attributes("OnClick") = "imgModalTemario('" & obtTemarioCursos(ddlAgregaCurso.SelectedValue) & "'); javascript:document.getElementById('lblCursoTemario').innerHTML ='" & strNombreCursoAgre.Trim & "' ; "

            'Motivo
            Dim ddlAgreMotivo As DropDownList
            ddlAgreMotivo = grdCursos.FooterRow.FindControl("ddlAgreMotivo")
            Call obtddlMotivo(ddlAgreMotivo)
            'Objetivo
            Dim ddlAgreObjetivo As DropDownList
            ddlAgreObjetivo = grdCursos.FooterRow.FindControl("ddlAgreObjCorp")
            Call obtddlObjetivo(ddlAgreObjetivo)
            'Competencia
            Dim ddlAgreCompetencia As DropDownList
            ddlAgreCompetencia = grdCursos.FooterRow.FindControl("ddlAgreCompetencia")
            Call obtddlCompetencias(ddlAgreCompetencia)
            'Tipo de Indicador
            Dim ddlAgreTipoIndicador As DropDownList
            ddlAgreTipoIndicador = grdCursos.FooterRow.FindControl("ddlAgreTIndicador")
            Call obtddlTipoIndicador(ddlAgreTipoIndicador)

            'Indicador
            Dim ddlAgreIndicador As DropDownList
            ddlAgreIndicador = grdCursos.FooterRow.FindControl("ddlAgreIndicador")
            Call obtddlIndicador(ddlAgreIndicador, ddlAgreTipoIndicador.SelectedValue)

            'TiMedicion
            Dim ddlAgreMedicion As DropDownList
            ddlAgreMedicion = grdCursos.FooterRow.FindControl("ddlAgreMedicion")
            Call obtddlEfectividad(ddlAgreMedicion)
            'Estatus
            Dim ddlAgreEstatus As DropDownList
            ddlAgreEstatus = grdCursos.FooterRow.FindControl("ddlAgreEstatus")
            Call obtddlEstatus(ddlAgreEstatus)
            odbConexion.Close()



            'OCULTA COLUMNA DE QUE NO TIENEN DATOS
            '      grdCursos.Columns(8).Visible = False
            grdCursos.Columns(12).Visible = False


        Catch ex As Exception
            lblError.Text = ex.Message
        End Try


    End Sub
    'asgina prioridades
    Private Sub btnGuardar_ServerClick(sender As Object, e As EventArgs) Handles btnGuardar.ServerClick
        Call actPrioridad()

    End Sub
    Public Sub actPrioridad()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String = ""
        Dim strId As String = ""

        Try
            odbConexion.Open()
            'ciclo para validar datos de cursos
            For i = 0 To grdCursos.Rows.Count - 1
                Dim iIdCurso As String
                Dim strPrioridad As String = ""
                Dim strAnio As String = ""
                Dim strMes As String = ""
                Dim btnEditar As LinkButton = grdCursos.Rows(i).Controls(14).Controls(0)

                Dim strNombreCurso As String = ""
                iIdCurso = DirectCast(grdCursos.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                If btnEditar.Text = "Editar" Then
                    'Curso
                    Dim lblCursos As New Label
                    Dim lblCursosRep As New Label

                    lblCursos = grdCursos.Rows(i).FindControl("lblCurso")

                    'valida repetidosos
                    strPrioridad = (DirectCast(grdCursos.Rows(i).FindControl("ddlPrioridadItem"), DropDownList).Text)
                    strAnio = (DirectCast(grdCursos.Rows(i).FindControl("ddlAnioItem"), DropDownList).Text)
                    strMes = (DirectCast(grdCursos.Rows(i).FindControl("ddlMesItem"), DropDownList).Text)
                    'valida que escogan un 
                    If strPrioridad = " Seleccionar" Or strAnio = 0 Or strPrioridad = " Seleccionar" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar prioridad, año y mes del curso " & lblCursos.Text & ".');</script>", False)
                        Exit Sub
                    End If
                    Dim iCOntadorRep As Integer = 0
                    For j = 0 To grdCursos.Rows.Count - 1
                        Dim iCurValidar As String = DirectCast(grdCursos.Rows(j).Cells(0).FindControl("lblId"), Label).Text
                        Dim strPrioridadVal As String = DirectCast(grdCursos.Rows(j).Cells(0).FindControl("ddlPrioridadItem"), DropDownList).Text
                        lblCursosRep = grdCursos.Rows(j).FindControl("lblCurso")
                        'valida que sea diferente al mismo
                        If iIdCurso <> iCurValidar Then
                            'valida la prioridad
                            If strPrioridad = strPrioridadVal Then
                                'valida cuando haya prioridades repetidas
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('" & lblCursosRep.Text & " tiene la prioridad " & strPrioridad & " del curso " & lblCursos.Text & ".');</script>", False)
                                Exit Sub

                                Exit For
                            End If
                        End If

                    Next


                End If

            Next
            'insertar valores validados
            For i = 0 To grdCursos.Rows.Count - 1
                Dim iIdCurso As String
                Dim strPrioridad As String = ""
                Dim strAnio As String = ""
                Dim strMes As String = ""
                Dim btnEditar As LinkButton = grdCursos.Rows(i).Controls(14).Controls(0)

                Dim strNombreCurso As String = ""
                iIdCurso = DirectCast(grdCursos.Rows(i).Cells(0).FindControl("lblId"), Label).Text
                If btnEditar.Text = "Editar" Then
                    'Curso
                    Dim lblCursos As New Label
                    Dim lblCursosRep As New Label

                    lblCursos = grdCursos.Rows(i).FindControl("lblCurso")

                    'valida repetidosos
                    strPrioridad = (DirectCast(grdCursos.Rows(i).FindControl("ddlPrioridadItem"), DropDownList).Text)
                    strAnio = (DirectCast(grdCursos.Rows(i).FindControl("ddlAnioItem"), DropDownList).Text)
                    strMes = (DirectCast(grdCursos.Rows(i).FindControl("ddlMesItem"), DropDownList).Text)
                    'valida que escogan un 
                    strQuery = "UPDATE [dbo].[DNC_GESTION_CURSOS_TB]                                     " & _
                               "  SET [prioridad_ejecucion] = " & strPrioridad & " " & _
                               "     ,[fk_id_anio] = " & strAnio & "  " & _
                               "     ,[mes_programado] = '" & strMes & "'  " & _
                               "     ,[estatus] ='VALIDADO'                         " & _
                               "     ,[fecha_modificacion] = GETDATE()           " & _
                               "     ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                               "WHERE ID=" & iIdCurso

                    Dim odbComando As New OleDbCommand(strQuery, odbConexion)
                    odbComando.ExecuteNonQuery()

                End If
            Next
            odbConexion.Close()
            hdIndexPrioridad.Value = 0
            'actualiza cursos cursos
            Call obtCursos(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try

    End Sub
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
    'inserta registro de los cursos
    Public Sub insCursos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strCursos As String = ""

        Dim strMes As String = ""
        Dim strMotivo As String = ""
        Dim strObjetivoCor As String = ""
        Dim strCompetenciaV As String = ""
        Dim strTipoIndicador As String = ""
        Dim strIndicador As String = ""
        Dim strMedirEfectividad As String = ""
        Dim strEstatus As String = ""
        Try

            strCursos = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreCurso"), DropDownList).Text)
            strMotivo = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreMotivo"), DropDownList).Text)
            strObjetivoCor = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreObjCorp"), DropDownList).Text)
            strCompetenciaV = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreCompetencia"), DropDownList).Text)
            strTipoIndicador = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreTIndicador"), DropDownList).Text)
            strIndicador = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreIndicador"), DropDownList).Text)
            strMedirEfectividad = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreMedicion"), DropDownList).Text)
            strEstatus = (DirectCast(grdCursos.FooterRow.FindControl("ddlAgreEstatus"), DropDownList).Text)

            odbConexion.Open()
            'validaciones para insertar registros

            If strCursos = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No hay cursos configurados para la DNC.');</script>", False)
                Exit Sub
            End If
            If strCursos = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un Curso.');</script>", False)
                Exit Sub
            End If

            If strMotivo = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Motivo.');</script>", False)
                Exit Sub
            End If
            If strObjetivoCor = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Objetivo Corporativo.');</script>", False)
                Exit Sub
            End If
            If strCompetenciaV = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar la competencia vinculada.');</script>", False)
                Exit Sub
            End If
            If strTipoIndicador = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Tipo de Indicador.');</script>", False)
                Exit Sub
            End If
            If strIndicador = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Indicador.');</script>", False)
                Exit Sub
            End If
            'If strMedirEfectividad = "0" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar la medición de la Efectividad.');</script>", False)
            '    Exit Sub
            'End If


            If validaNumeroCursos() Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Por Política no se puede agregar más de " & hdMaxCurso.Value & " Cursos.');</script>", False)
                Exit Sub
            End If

            strQuery = "INSERT INTO [dbo].[DNC_GESTION_CURSOS_TB]" & _
                       "([fk_id_colaborador]                                 " & _
                       ",[no_correlativo]                                    " & _
                       ",[fk_id_curso]                                       " & _
                       ",[prioridad_ejecucion]                               " & _
                       ",[fk_id_anio]                                        " & _
                       ",[mes_programado]                                    " & _
                       ",[fk_id_motivo]                                      " & _
                       ",[fk_id_objetivo_corporativo]                        " & _
                       ",[fk_id_competencia_vinculada]                       " & _
                       ",[fk_id_tipo_indicador]                              " & _
                       ",[fk_id_indicador]                                   " & _
                       ",[fk_id_medicion_efectividad]                        " & _
                       ",[fk_id_estatus]                                     " & _
                       ",[fk_id_parametrizacion]                             " & _
                       ",[estatus]                                           " & _
                       ",[fecha_creacion]                                    " & _
                       ",[usuario_creacion])                                 " & _
                       " VALUES     " & _
                       "(" & ddlColaborador.SelectedValue & " " & _
                       ",(0) " & _
                       "," & IIf(strCursos = "", 0, strCursos) & " " & _
                       ",NULL " & _
                       ",NULL " & _
                       ",NULL" & _
                       "," & IIf(strMotivo = "", 0, strMotivo) & " " & _
                       "," & IIf(strObjetivoCor = "", 0, strObjetivoCor) & " " & _
                       "," & IIf(strCompetenciaV = "", 0, strCompetenciaV) & " " & _
                       "," & IIf(strTipoIndicador = "", 0, strTipoIndicador) & " " & _
                       "," & IIf(strIndicador = "", 0, strIndicador) & " " & _
                       "," & IIf(strMedirEfectividad = "", 0, strMedirEfectividad) & " " & _
                       "," & IIf(strEstatus = "" Or strEstatus = "0", 1, strEstatus) & " " & _
                       "," & IIf(hdIdDNC.Value = "", 0, hdIdDNC.Value) & " " & _
                       ",'CREADO' " & _
                       ",GETDATE()                      " & _
                       ",'" & hdUsuario.Value & "')   Select @@Identity "

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim iId As Integer = 0
            iId = CInt(odbComando.ExecuteScalar())
            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)

            odbConexion.Close()
            hdIndexPrioridad.Value = 1
            Call obtCursos(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub
    'valida el numero de cursos
    Public Function validaNumeroCursos() As Boolean
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT COUNT(*) FROM DNC_GESTION_CURSOS_TB where (fk_id_parametrizacion='" & hdIdDNC.Value & _
                    "' and fk_id_colaborador='" & ddlColaborador.SelectedValue & "' and estatus<>'ELIMINADO') ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            'valida si es insert o update
            If odbLector(0) >= hdMaxCurso.Value Then blResultado = True

            odbLector.Close()
        End If
        odbConexion.Close()
        Return blResultado

    End Function
    Protected Sub lnkAgregar_Click(sender As Object, e As EventArgs)
        Call insCursos()
    End Sub

    Private Sub grdCursos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdCursos.RowCancelingEdit
        grdCursos.ShowFooter = True
        grdCursos.EditIndex = -1
        Call obtCursos(ddlColaborador.SelectedValue)
    End Sub
    'INSERTA FILA VACIA CUANDO NO EXISTA NINGUN REGISTRO
    Public Sub insFilaVaciaCursos()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("id"))
        dt.Columns.Add(New DataColumn("no_correlativo"))
        dt.Columns.Add(New DataColumn("fk_id_curso"))
        dt.Columns.Add(New DataColumn("ruta"))
        dt.Columns.Add(New DataColumn("prioridad_ejecucion"))
        dt.Columns.Add(New DataColumn("fk_id_anio"))
        dt.Columns.Add(New DataColumn("mes_programado"))
        dt.Columns.Add(New DataColumn("fk_id_motivo"))
        dt.Columns.Add(New DataColumn("fk_id_objetivo_corporativo"))
        dt.Columns.Add(New DataColumn("fk_id_competencia_vinculada"))
        dt.Columns.Add(New DataColumn("fk_id_tipo_indicador"))
        dt.Columns.Add(New DataColumn("fk_id_indicador"))
        dt.Columns.Add(New DataColumn("fk_id_medicion_efectividad"))
        dt.Columns.Add(New DataColumn("fk_id_estatus"))

        dr = dt.NewRow
        dr("id") = ""
        dr("no_correlativo") = ""
        dr("fk_id_curso") = ""
        dr("ruta") = ""
        dr("prioridad_ejecucion") = ""
        dr("fk_id_anio") = ""
        dr("mes_programado") = ""
        dr("fk_id_motivo") = ""
        dr("fk_id_objetivo_corporativo") = ""
        dr("fk_id_competencia_vinculada") = ""
        dr("fk_id_tipo_indicador") = ""
        dr("fk_id_indicador") = ""
        dr("fk_id_medicion_efectividad") = ""
        dr("fk_id_estatus") = ""

        dt.Rows.Add(dr)

        ViewState("CurrentTable") = dt
        grdCursos.DataSource = dt.DefaultView
        grdCursos.DataBind()


    End Sub
    '********************- Cursos -******************************
    'Obtiene el catalogo de Cursos Agregar
    Public Sub obtddlCursos(ByVal ddl As DropDownList, modoEdicion As Integer)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""
        Dim strId As String = ""

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion_capacitacion_corta UNION ALL "
        odbConexion.Open()
        If modoEdicion = 1 Then 'edicion valida que obtenga los cursos no asignados como el que ya tiene asignado
            'obtiene el ID de edicion
            strId = DirectCast(grdCursos.Rows(hdIndexCurso.Value).FindControl("lblId"), Label).Text
            strQuery += " SELECT ID,descripcion_capacitacion_corta FROM DNC_CURSOS_TB" & _
           " WHERE (ESTATUS<>'ELIMINADO' AND fk_id_parametrizacion='" & hdIdDNC.Value & "') " & _
           " AND ID NOT IN (SELECT fk_id_curso FROM DNC_GESTION_CURSOS_TB WHERE (ESTATUS<>'ELIMINADO' AND " & _
           " fk_id_colaborador=" & ddlColaborador.SelectedValue & " AND fk_id_parametrizacion='" & hdIdDNC.Value & "' and id<>" & strId & " ) )  ORDER BY 2"

        Else 'query para insertar cursos
            strQuery += " SELECT ID,descripcion_capacitacion_corta FROM DNC_CURSOS_TB" & _
           " WHERE (ESTATUS<>'ELIMINADO' AND fk_id_parametrizacion='" & hdIdDNC.Value & "') " & _
           " AND ID NOT IN (SELECT fk_id_curso FROM DNC_GESTION_CURSOS_TB WHERE ESTATUS<>'ELIMINADO' " & _
           "AND fk_id_colaborador=" & ddlColaborador.SelectedValue & " AND fk_id_parametrizacion='" & hdIdDNC.Value & "' )  ORDER BY 2"

        End If

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion_capacitacion_corta"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub

    Public Function obtTextoCursos(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion_capacitacion_corta FROM DNC_CURSOS_TB  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Temario -******************************
    'Obtiene Temario
    Public Function obtTemarioCursos(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT ruta FROM DNC_TEMARIOS_PDF_TB  WHERE estatus='VIGENTE' and  fk_id_curso=" & IIf(strid = "", 0, strid)

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        odbLector = odbComando.ExecuteReader
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0).ToString.Replace("\", "\u005C")
            odbLector.Close()
        Else
            strResultado = "UploadedFiles\No_temario.pdf".Replace("\", "\u005C")
        End If

        odbConexion.Close()
        Return strResultado
    End Function
    '********************- Año -******************************
    'Obtiene el catalogo de Años configurados
    Public Sub obtddlAño(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 as ID, ' Seleccionar' as anio UNION ALL SELECT ID,CONVERT(VARCHAR,anio) AS anio FROM DNC_ANIO_PROPUESTO_CT WHERE fk_id_parametrizacion='" & hdIdDNC.Value & "' AND ESTATUS=1   ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "anio"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoAño(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT anio FROM DNC_ANIO_PROPUESTO_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Prioridad -******************************
    'Obtiene el catalogo de Priorida configurados
    Public Sub obtddlPrioridad(ByVal ddl As DropDownList, modoEdicion As Integer)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strPrioridad As String = ""
        Dim strId As String = "0"

        odbConexion.Open()
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_muestra_prioridad_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        If modoEdicion = 1 Then strId = DirectCast(grdCursos.Rows(hdIndexCurso.Value).FindControl("lblId"), Label).Text

        'parametros
        odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
        odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)
        odbComando.Parameters.AddWithValue("@edicion", modoEdicion)
        odbComando.Parameters.AddWithValue("@idCurso", strId)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "PRIORIDAD"
        ddl.DataValueField = "PRIORIDAD"

        ddl.DataBind()

        odbConexion.Close()
    End Sub


    '********************- Motivo -******************************
    'Obtiene el catalogo de Motivos configurados
    Public Sub obtddlMotivo(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL  SELECT ID,descripcion FROM DNC_MOTIVOS_CT WHERE ESTATUS=1  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        For Each li As ListItem In ddl.Items

            li.Attributes.Add("data-toggle", "tooltip")
            li.Attributes.Add("data-placement", "rigth")
            li.Attributes.Add("title", obtTextoMotivoToolTip(li.Value))
        Next

        odbConexion.Close()
    End Sub
    Public Function obtTextoMotivo(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_MOTIVOS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    Public Function obtTextoMotivoToolTip(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT definicion FROM DNC_MOTIVOS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    '********************- OBJETIVO -******************************
    'Obtiene el catalogo de OBJETIVO configurados
    Public Sub obtddlObjetivo(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String = ""


        odbConexion.Open()

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_objetivos_corporativos_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametro
        odbComando.Parameters.AddWithValue("@clave", ddlColaborador.SelectedValue)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()
        odbLector.Close()
        odbComando.Dispose()

        odbComando = New OleDbCommand
        odbComando.CommandText = "dnc_objetivos_corporativos_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametro
        odbComando.Parameters.AddWithValue("@clave", ddlColaborador.SelectedValue)
        Dim odbLectorOcultar As OleDbDataReader
        odbLectorOcultar = odbComando.ExecuteReader
        While odbLectorOcultar.Read()

            For Each li As ListItem In ddl.Items
                If li.Value = odbLectorOcultar(0).ToString Then
                    If odbLectorOcultar(2).ToString = "0" Then li.Attributes.Add("disabled", "disabled")
                End If

            Next
        End While

        odbConexion.Close()
    End Sub
    Public Function obtTextoObjetivo(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_OBJETIVO_CORPORATIVOS_DETALLE_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    '********************- Competencias -******************************
    'Obtiene el catalogo de Competencias configurados
    Public Sub obtddlCompetencias(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM DNC_COMPETENCIAS_VINCULADAS_CT   WHERE ESTATUS=1  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()


        For Each li As ListItem In ddl.Items

            li.Attributes.Add("data-toggle", "tooltip")
            li.Attributes.Add("data-placement", "rigth")
            li.Attributes.Add("title", obtTextoCompetenciasToolTip(li.Value))
        Next

        odbConexion.Close()
    End Sub
    Public Function obtTextoCompetencias(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_COMPETENCIAS_VINCULADAS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function obtTextoCompetenciasToolTip(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT [definicion] FROM DNC_COMPETENCIAS_VINCULADAS_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Tipo de Indicador -******************************
    'Obtiene el catalogo de Tipo Indicador configurados
    Public Sub obtddlTipoIndicador(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM DNC_TIPO_INDICADOR_CT  WHERE ESTATUS=1   ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()


        For Each li As ListItem In ddl.Items
            li.Attributes.Add("data-toggle", "tooltip")
            li.Attributes.Add("data-placement", "rigth")
            li.Attributes.Add("title", obtTextoTipoIndicadorToolTip(li.Value))
        Next

        odbConexion.Close()
    End Sub
    Public Function obtTextoTipoIndicador(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_TIPO_INDICADOR_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function obtTextoTipoIndicadorToolTip(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT [definicion] FROM DNC_TIPO_INDICADOR_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Indicador -******************************
    'Obtiene el catalogo de Indicador configurados
    Public Sub obtddlIndicador(ByVal ddl As DropDownList, Optional ByVal iTipoIndicador As Integer = 0)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM DNC_INDICADORES_CT WHERE fk_id_tipo_indicador='" & Str(iTipoIndicador) & "'   ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()
        For Each li As ListItem In ddl.Items
            li.Attributes.Add("data-toggle", "tooltip")
            li.Attributes.Add("data-placement", "rigth")
            li.Attributes.Add("title", obtTextoIndicadorToolTip(li.Value))
        Next

        odbConexion.Close()
    End Sub
    Public Function obtTextoIndicador(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_INDICADORES_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    Public Function obtTextoIndicadorToolTip(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT [definicion] FROM DNC_INDICADORES_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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
    '********************- Medir Efectividad -******************************
    'Obtiene el catalogo de Medir Efectividad configurados
    Public Sub obtddlEfectividad(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM DNC_MEDIR_EFECTIVIDAD_CT  ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoEfectividad(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_MEDIR_EFECTIVIDAD_CT  WHERE ID=" & IIf(strid = "", 0, strid)

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

    '********************- Estatus -******************************
    'Obtiene el catalogo de los estatusconfigurados
    Public Sub obtddlEstatus(ByVal ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String


        odbConexion.Open()

        strQuery = "SELECT 0 AS ID,' Seleccionar' AS descripcion UNION ALL SELECT ID,descripcion FROM DNC_ESTATUS_TB WHERE ESTATUS=1   ORDER BY 2"

        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddl.DataSource = odbLector
        ddl.DataTextField = "descripcion"
        ddl.DataValueField = "id"

        ddl.DataBind()

        odbConexion.Close()
    End Sub
    Public Function obtTextoEstatus(strid As String) As String
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim strResultado As String = ""
        Dim odbLector As OleDbDataReader
        odbConexion.Open()

        strQuery = " SELECT descripcion FROM DNC_ESTATUS_TB  WHERE ID=" & IIf(strid = "", 0, strid)

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
    'Actualiza Menu de Cursos
    Protected Sub ddlCurso_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlCurso As DropDownList

        ddlCurso = grdCursos.Rows(hdIndexCurso.Value).FindControl("ddlCurso")

        Dim lnkTemario As New LinkButton
        lnkTemario = grdCursos.Rows(hdIndexCurso.Value).FindControl("lnkTemario")
        lnkTemario.Text = "<span class='label label-danger letraTemario'>Temario</span>"

        lnkTemario.Attributes("data-toggle") = "modal"
        lnkTemario.Attributes("data-target") = "#modalTemario"
        lnkTemario.Attributes("OnClick") = "imgModalTemario('" & obtTemarioCursos(ddlCurso.SelectedValue) & "'); javascript:document.getElementById('lblCursoTemario').innerHTML ='" & (ddlCurso.SelectedItem.Text.Trim) & "' ; "

        'Objetivo
        Dim ddlObjCorp As DropDownList
        ddlObjCorp = grdCursos.Rows(hdIndexCurso.Value).FindControl("ddlObjCorp")
        Call DesabilitaItemDireccion(ddlObjCorp)
    End Sub
    'Desabilita el Item de Direccion
    Public Sub DesabilitaItemDireccion(ddl As DropDownList)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)

        odbConexion.Open()
        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_objetivos_corporativos_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametro
        odbComando.Parameters.AddWithValue("@clave", ddlColaborador.SelectedValue)
        Dim odbLectorOcultar As OleDbDataReader
        odbLectorOcultar = odbComando.ExecuteReader
        While odbLectorOcultar.Read()

            For Each li As ListItem In ddl.Items
                If li.Value = odbLectorOcultar(0).ToString Then
                    If odbLectorOcultar(2).ToString = "0" Then li.Attributes.Add("disabled", "disabled")
                End If
            Next
        End While
        odbConexion.Close()
    End Sub
    Protected Sub ddlAgreCurso_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlAgregaCurso As DropDownList

        ddlAgregaCurso = grdCursos.FooterRow.FindControl("ddlAgreCurso")

        Dim lnkTemario As New LinkButton
        lnkTemario = grdCursos.FooterRow.FindControl("lnkAgreTemario")
        lnkTemario.Text = "<span class='label label-danger letraTemario'>Temario</span>"

        lnkTemario.Attributes("data-toggle") = "modal"
        lnkTemario.Attributes("data-target") = "#modalTemario"
        lnkTemario.Attributes("OnClick") = "imgModalTemario('" & obtTemarioCursos(ddlAgregaCurso.SelectedValue) & "'); javascript:document.getElementById('lblCursoTemario').innerHTML ='" & (ddlAgregaCurso.SelectedItem.Text.Trim) & "' ; "

        Dim ddlAgreObjetivo As DropDownList
        ddlAgreObjetivo = grdCursos.FooterRow.FindControl("ddlAgreObjCorp")
        Call DesabilitaItemDireccion(ddlAgreObjetivo)
    End Sub
    'Carga Indicadores
    Protected Sub ddlAgreTIndicador_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Indicador
        Dim ddlAgreIndicador As DropDownList
        Dim ddlAgreTipoIndicador As DropDownList
        ddlAgreTipoIndicador = grdCursos.FooterRow.FindControl("ddlAgreTIndicador")
        ddlAgreIndicador = grdCursos.FooterRow.FindControl("ddlAgreIndicador")
        Call obtddlIndicador(ddlAgreIndicador, ddlAgreTipoIndicador.SelectedValue)

        Dim ddlAgreObjetivo As DropDownList
        ddlAgreObjetivo = grdCursos.FooterRow.FindControl("ddlAgreObjCorp")
        Call DesabilitaItemDireccion(ddlAgreObjetivo)
    End Sub

    Protected Sub ddlTIndidador_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlTipoIndicador As DropDownList
        Dim ddlIndicador As DropDownList
        ddlTipoIndicador = grdCursos.Rows(hdIndexCurso.Value).Cells(4).FindControl("ddlTIndidador")
        ddlIndicador = grdCursos.Rows(hdIndexCurso.Value).Cells(4).FindControl("ddlIndicador")
        'obtiene Indicador Configurado
        Call obtddlIndicador(ddlIndicador, ddlTipoIndicador.SelectedValue)
        'Objetivo
        Dim ddlObjCorp As DropDownList
        ddlObjCorp = grdCursos.Rows(hdIndexCurso.Value).FindControl("ddlObjCorp")
        Call DesabilitaItemDireccion(ddlObjCorp)
    End Sub

    Private Sub grdCursos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCursos.RowDataBound
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

    Private Sub grdCursos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCursos.RowDeleting
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strId As String = ""
        Try
            strId = DirectCast(grdCursos.Rows(e.RowIndex).FindControl("lblId"), Label).Text

            If strId = "" Then Exit Sub

            odbConexion.Open()

            strQuery = "UPDATE [dbo].[DNC_GESTION_CURSOS_TB] " & _
                               "  SET [estatus] ='ELIMINADO'  " & _
                               "     ,[fecha_modificacion] = GETDATE()           " & _
                               "     ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                               "WHERE ID=" & strId


            Dim odbComando As OleDbCommand = New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()


            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)
            odbConexion.Close()

            grdCursos.EditIndex = -1
            grdCursos.ShowFooter = True
            btnGuardar.Visible = True
            hdIndexPrioridad.Value = 1
            Call obtCursos(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try
    End Sub

    Private Sub grdCursos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCursos.RowEditing
        grdCursos.ShowFooter = False

        hdIndexCurso.Value = e.NewEditIndex
        grdCursos.EditIndex = e.NewEditIndex
        Call obtCursos(ddlColaborador.SelectedValue)
        btnGuardar.Visible = False
    End Sub

    Private Sub grdCursos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdCursos.RowUpdating
        grdCursos.ShowFooter = True
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim strQuery As String
        Dim strCursos As String = ""
        Dim strMotivo As String = ""
        Dim strObjetivoCor As String = ""
        Dim strCompetenciaV As String = ""
        Dim strTipoIndicador As String = ""
        Dim strIndicador As String = ""
        Dim strMedirEfectividad As String = ""
        Dim strEstatus As String = ""
        Dim strId As String = ""

        Try
            strId = DirectCast(grdCursos.Rows(e.RowIndex).FindControl("lblId"), Label).Text
            strCursos = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlCurso"), DropDownList).Text)
            strMotivo = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlMotivo"), DropDownList).Text)
            strObjetivoCor = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlObjCorp"), DropDownList).Text)
            strCompetenciaV = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlCompetencia"), DropDownList).Text)
            strTipoIndicador = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlTIndidador"), DropDownList).Text)
            strIndicador = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlIndicador"), DropDownList).Text)
            strMedirEfectividad = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlMedicion"), DropDownList).Text)
            strEstatus = (DirectCast(grdCursos.Rows(e.RowIndex).FindControl("ddlEstatus"), DropDownList).Text)
            odbConexion.Open()
            'validaciones para insertar registros

            If strCursos = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('No hay cursos configurados para la DNC.');</script>", False)
                Exit Sub
            End If
            If strCursos = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe seleccionar un Curso.');</script>", False)
                Exit Sub
            End If

            If strMotivo = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Motivo.');</script>", False)
                Exit Sub
            End If
            'If strObjetivoCor = "0" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Objetivo Corporativo.');</script>", False)
            '    Exit Sub
            'End If
            If strCompetenciaV = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar la competencia vinculada.');</script>", False)
                Exit Sub
            End If
            If strTipoIndicador = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Tipo de Indicador.');</script>", False)
                Exit Sub
            End If
            If strIndicador = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar el Indicador.');</script>", False)
                Exit Sub
            End If
            'If strMedirEfectividad = "0" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Debe de seleccionar la medición de la Efectividad.');</script>", False)
            '    Exit Sub
            'End If



            'If validaNumeroCursos() Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Alert", "<script>alert('Por Política No se puede Agregar más de 6 Cursos.');</script>", False)
            '    Exit Sub
            'End If

            strQuery = "UPDATE [dbo].[DNC_GESTION_CURSOS_TB]                                     " & _
                       "  SET [fk_id_colaborador] =" & ddlColaborador.SelectedValue & " " & _
                       "     ,[no_correlativo] =(0) " & _
                       "     ,[fk_id_curso] =" & IIf(strCursos = "", 0, strCursos) & " " & _
                       "     ,[fk_id_motivo] = " & IIf(strMotivo = "", 0, strMotivo) & " " & _
                       "     ,[fk_id_objetivo_corporativo] =" & IIf(strObjetivoCor = "", 0, strObjetivoCor) & " " & _
                       "     ,[fk_id_competencia_vinculada] = " & IIf(strCompetenciaV = "", 0, strCompetenciaV) & " " & _
                       "     ,[fk_id_tipo_indicador] = " & IIf(strTipoIndicador = "", 0, strTipoIndicador) & " " & _
                       "     ,[fk_id_indicador] =" & IIf(strIndicador = "", 0, strIndicador) & " " & _
                       "     ,[fk_id_medicion_efectividad] = " & IIf(strMedirEfectividad = "", 0, strMedirEfectividad) & " " & _
                       "     ,[fk_id_estatus] = " & IIf(strEstatus = "" Or strEstatus = "0", 1, strEstatus) & " " & _
                       "     ,[estatus] ='MODIFICADO'                         " & _
                       "     ,[fecha_modificacion] = GETDATE()           " & _
                       "     ,[usuario_modificacion] = '" & hdUsuario.Value & "'   " & _
                       "WHERE ID=" & strId

            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            odbComando.ExecuteNonQuery()

            'Calcula numero Correlativo
            Call CargaCorrelativo(odbConexion)
            odbConexion.Close()
            grdCursos.EditIndex = -1
            hdIndexPrioridad.Value = 1
            Call obtCursos(ddlColaborador.SelectedValue)
        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
        End Try

    End Sub
    'calcula el numero correlativo
    Public Sub CargaCorrelativo(odbConexion As OleDbConnection)
        Try
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_calcula_correlativo_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)

            odbComando.ExecuteNonQuery()

        Catch ex As Exception
            lblErrorGestion.Text = ex.Message
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
            'valida que el peridodo de registro este vigente
            If hdRol.Value <> "2" Then
                Dim strFechaInicio, strFechaFinal, strFechaActual As String
                strFechaInicio = odbLector(2).ToString
                strFechaFinal = odbLector(3).ToString
                strFechaActual = Now.Year.ToString & "-" & Right("00" & Now.Month.ToString, 2) & "-" & Right("00" & Now.Day.ToString, 2)

                'valida rangos de fecha
                If CDate(strFechaActual) >= CDate(strFechaInicio) And CDate(strFechaActual) <= CDate(strFechaFinal) Then ' periodo vigente
                    'obtiene el maximo de cursos

                Else
                    Response.Redirect("finplazo.html")
                End If

            End If
            Call obtMaxCursos()
            odbLector.Close()
        Else
            Response.Redirect("index.aspx")
        End If
        odbConexion.Close()

    End Sub

    'Obtiene el numero maximo de cursos
    Public Sub obtMaxCursos()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet
        Dim strQuery As String
        Dim blResultado As Boolean = False
        odbConexion.Open()
        strQuery = "SELECT numero_cursos FROM DNC_PARAMETROS_TB where fk_id_parametrizacion=" & hdIdDNC.Value & " ORDER BY 1"
        Dim odbComando As New OleDbCommand(strQuery, odbConexion)
        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader

        If odbLector.HasRows Then
            odbLector.Read()
            hdMaxCurso.Value = odbLector(0).ToString

        End If
        odbConexion.Close()

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
            odbComando.CommandText = "dnc_colaboradores_reportan_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametro
            odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
            odbComando.Parameters.AddWithValue("@jefe", hdNoNominaUsuario.Value)
            odbComando.Parameters.AddWithValue("@perfil", hdRol.Value)

            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()

            ddlColaborador.DataSource = odbLector
            ddlColaborador.DataValueField = "CLAVE"
            ddlColaborador.DataTextField = "NOMBRE"

            ddlColaborador.DataBind()
            odbConexion.Close()
            'valida si tiene asignado empleado
            If ddlColaborador.Items.Count = 0 Then
                divRegistro.Visible = False
                divCheck.Visible = False
                divHistorico.Visible = False
                divDatos.Visible = False
                lblColaborador.Text = "No tiene colaboradores para registrar en la DNC."

            End If

            'For Each li As ListItem In ddlColaborador.Items

            '    li.Attributes.Add("title", "Juan")
            'Next

        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub

    Private Sub ObtDatosColaboradores()
        Try
            Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
            Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
            odbConexion.Open()

            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_datos_colaborador_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure

            'parametros
            odbComando.Parameters.AddWithValue("@PCLAVE", IIf(ddlColaborador.SelectedValue = "", 0, ddlColaborador.SelectedValue))
            lblPuestoCol.Text = ""
            lblDireccion.Text = ""
            lblNombreCol.Text = ""
            lblArea.Text = ""
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader()
            If odbLector.HasRows Then
                odbLector.Read()
                lblNombreCol.Text = StrConv(odbLector(1).ToString, VbStrConv.ProperCase) & " - " & odbLector(0).ToString
                lblPuestoCol.Text = "<br /> <strong>Puesto: </strong>" & StrConv(odbLector(8).ToString, VbStrConv.ProperCase)
                lblArea.Text = IIf(odbLector(5).ToString.Trim = "" Or odbLector(5).ToString.Trim = "N/A", "", "<br /> <strong>Área: </strong>" & StrConv(odbLector(5).ToString.Trim, VbStrConv.ProperCase))
                lblDireccion.Text = "<br /><strong>Dirección: </strong>" &
                    IIf(odbLector(4).ToString = "", StrConv(odbLector(3).ToString, VbStrConv.ProperCase), StrConv(odbLector(4).ToString, VbStrConv.ProperCase))

                lblProfesion.Text = "<br /><strong>Profesión: </strong>" & StrConv(odbLector(15).ToString, VbStrConv.ProperCase)
                lblEstatus.Text = "<br /> <strong>Estatus: </strong>" & StrConv(odbLector(11).ToString, VbStrConv.ProperCase)
                lblExperiencia.Text = "<br /> <strong>Experiencia: </strong>" & StrConv(odbLector(13).ToString, VbStrConv.ProperCase)
                'obtiene Años del Historicos
                Call obtddlAño()
                'obtiene cursos Historicos
                Call obtCursosHistorico()
                'obtiene cursos
                Call obtCursos(ddlColaborador.SelectedValue)

                ' If hdRol.Value = "A" Then Call obtCursos(ddlColaborador.SelectedValue)
                odbLector.Close()
                divCheck.Visible = True
                divRegistro.Visible = True
            Else 'si no tiene colaboradores asignados
                divCheck.Visible = False
                divRegistro.Visible = False
            End If


            odbConexion.Close()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try

    End Sub


    Private Sub ddlColaborador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlColaborador.SelectedIndexChanged
        chkHistorico.Checked = False
        divHistorico.Visible = False
        ddlAnio.SelectedValue = 0
        Call ObtDatosColaboradores()
    End Sub
#End Region
#Region "Historico"
    Private Sub obtCursosHistorico()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim sSQL As String = ""
        Dim dsCursos As New DataSet
        Dim iAnio As Integer = 0
        Try
            grdHistorico.DataSource = Nothing

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_cursos_historico_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure


            'parametros
            odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)
            odbComando.Parameters.AddWithValue("@anio", ddlAnio.SelectedValue)

            Dim odbAdaptador As New OleDbDataAdapter
            odbAdaptador.SelectCommand = odbComando
            odbAdaptador.Fill(dsCursos)
            'GENERA CURSOS

            Call generaCol(dsCursos)

            grdHistorico.DataSource = dsCursos.Tables(0).DefaultView
            grdHistorico.DataBind()

            If grdHistorico.Rows.Count = 0 Then

                lblDatosHistoric.Text = "No tiene Cursos."
            Else
                lblDatosHistoric.Text = ""
            End If
            grdHistorico.Columns(0).Visible = False
            grdHistorico.Columns(1).Visible = False
            grdHistorico.Columns(2).Visible = False
            grdHistorico.Columns(3).Visible = False

            odbConexion.Close()

            Call comportamiento()
        Catch ex As Exception
            lblError.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Sub obtddlAño()
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim dsCatalogo As New DataSet

        odbConexion.Open()

        Dim odbComando As New OleDbCommand
        odbComando.CommandText = "dnc_anios_cursos_historico_sp"
        odbComando.Connection = odbConexion
        odbComando.CommandType = CommandType.StoredProcedure

        'parametros
        odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader


        ddlAnio.DataSource = odbLector
        ddlAnio.DataTextField = "ANIO"
        ddlAnio.DataValueField = "ID_ANIO"

        ddlAnio.DataBind()

        odbConexion.Close()
    End Sub
    Private Sub ddlAnio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAnio.SelectedIndexChanged
        Call obtCursosHistorico()
    End Sub
    'Genera la columna dependiendo del reporte a ejecutar
    Public Sub generaCol(ByVal ds As DataSet)
        Dim inContador As Integer = 0
        Dim iCol As Integer = grdHistorico.Columns.Count
        Dim iItem As Integer = 0

        If ds.Tables(0).Rows.Count > 0 Then

            'ELIMANA LAS FILAS ACTUALES
            If iCol > 1 Then
                For inContador = 0 To iCol - 1
                    iItem = (iCol - 1) - inContador
                    If grdHistorico.Columns(iItem).HeaderText <> "VER" Then
                        grdHistorico.Columns.RemoveAt(iItem)
                    End If
                Next
            End If
        End If
        'SE CREAN COLUMNAS EN TIEMPO DE EJECUCION
        For inContador = 0 To ds.Tables(0).Columns.Count - 1
            Dim bfield As New BoundField()
            bfield.HeaderText = ds.Tables(0).Columns(inContador).ColumnName
            bfield.DataField = ds.Tables(0).Columns(inContador).ColumnName
            bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Left
            grdHistorico.Columns.Add(bfield)
        Next
    End Sub

    Private Sub grdHistorico_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdHistorico.PageIndexChanging
        grdHistorico.PageIndex = e.NewPageIndex
        grdHistorico.DataBind()
        ddlAnio.SelectedValue = 0
        Call obtCursosHistorico()

    End Sub
#End Region
#Region "Comportamientos"
    Public Sub comportamiento()
        'colorea las celdas del grid
        For iFil As Integer = 0 To grdCursos.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdCursos.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next

        For iFil As Integer = 0 To grdHistorico.Rows.Count - 1
            If iFil Mod 2 = 0 Then
                grdHistorico.Rows(iFil).BackColor = Color.FromName("#F2F2F2")
            End If
        Next
        'valida check
        If chkHistorico.Checked Then
            divHistorico.Visible = True
        Else
            divHistorico.Visible = False
        End If
        'controla la edicion del pagina para que guarden valor
        If hdIndexPrioridad.Value = 1 Then
            ddlColaborador.Attributes.Add("disabled", "disabled")
            lblGuardar.Text = "Debe de Guardar los cursos para que tome las nuevas prioridades."
        Else
            ddlColaborador.Attributes.Remove("disabled")
            lblGuardar.Text = ""
        End If
    End Sub
    Private Sub chkHistorico_CheckedChanged(sender As Object, e As EventArgs) Handles chkHistorico.CheckedChanged
        'valida check
        If chkHistorico.Checked Then
            divHistorico.Visible = True
        Else
            divHistorico.Visible = False
        End If
    End Sub

#End Region
#Region "Excel"
    Public Sub obtExportarExcel(dsDatos As DataSet)
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Try

            Dim filename As String = "rpt_" & CStr(Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Millisecond) & ".xls"
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
    Private Sub btnExportarHis_ServerClick(sender As Object, e As EventArgs) Handles btnExportarHis.ServerClick
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim sSQL As String = ""
        Dim dsCursos As New DataSet
        Try
            grdHistorico.DataSource = Nothing

            odbConexion.Open()
            Dim odbComando As New OleDbCommand
            odbComando.CommandText = "dnc_cursos_historico_sp"
            odbComando.Connection = odbConexion
            odbComando.CommandType = CommandType.StoredProcedure


            'parametros
            odbComando.Parameters.AddWithValue("@parametrizacion", hdIdDNC.Value)
            odbComando.Parameters.AddWithValue("@empleado", ddlColaborador.SelectedValue)
            odbComando.Parameters.AddWithValue("@anio", ddlAnio.SelectedValue)

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
    Private Sub btnExportarCursos_ServerClick(sender As Object, e As EventArgs) Handles btnExportarCursos.ServerClick
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConexion As OleDbConnection = New OleDbConnection(sConnString)
        Dim sSQL As String = ""
        Dim dsCursos As New DataSet
        Try
            odbConexion.Open()
            sSQL = "SELECT * FROM DNC_GESTION_CURSOS_VT WHERE fk_id_parametrizacion=" & hdIdDNC.Value & " AND [CLAVE]=" & ddlColaborador.SelectedValue
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
            '   strNombreUsuario = "julmej"

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