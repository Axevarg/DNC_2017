Imports System.Data.OleDb
Imports System.DirectoryServices

Public Class imprimirIP
    Inherits System.Web.UI.Page
    Private sIdFile As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sConnString As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim odbConn As OleDbConnection = New OleDbConnection(sConnString)
        odbConn.Open()


        Dim lstArreglo As New ArrayList

        sIdFile = Request.QueryString("ID")
        lstArreglo = obtenerUsurioAD(odbConn)
        'lblNombreE.Text = lstArreglo.Item(1).ToStri
        Call ObtInformacion(sIdFile, odbConn)
        odbConn.Close()
        '        'IMPRIME HOJA
        'ScriptManager.RegisterStartupScript(Me, GetType(Page), "PRINT", "<script>window.print();</script>", False)
    End Sub




#Region "CARGA"
    Public Sub ObtInformacion(ByVal idFile As String, ByVal odbConexion As OleDbConnection)
        Dim strEmpresaColaborador As String = "1" 'por default  passa
        Dim strConexion As String = ConfigurationManager.ConnectionStrings("sqlConn").ConnectionString
        Dim strQuery As String = ""
        Dim iIdEmpleado As Integer = 0
        Try
            strQuery = "SELECT * FROM DO_IDENTIFICACION_COMPETENCIA_TB A INNER JOIN  DO_PUESTOS_TB B ON A.puesto=B.ID WHERE A.id=" & idFile
            Dim odbComando As New OleDbCommand(strQuery, odbConexion)
            Dim odbLector As OleDbDataReader
            odbLector = odbComando.ExecuteReader

            If odbLector.HasRows Then
                odbLector.Read()
                txtPuesto.Text = odbLector(83).ToString
                txtNivel.Text = odbLector(3).ToString
                txtPuestNivel.Text = odbLector(4).ToString
                txtDirArea.Text = odbLector(5).ToString
                txtDir.Text = odbLector(6).ToString
                txtGerencia.Text = odbLector(7).ToString
                txtCentroCostos.Text = odbLector(8).ToString
                txtObjetivoPuesto.Value = odbLector(9).ToString
                txtPuestoQReporta.Value = odbLector(10).ToString
                txtNumeroPuesto.Text = odbLector(11).ToString
                txtorganigrama_1.Text = odbLector(12).ToString
                txtorganigrama_2.Text = odbLector(13).ToString
                txtorganigrama_3.Text = odbLector(14).ToString
                txtorganigrama_4.Text = odbLector(15).ToString
                txtorganigrama_5.Text = odbLector(16).ToString
                txtorganigrama_6.Text = odbLector(17).ToString
                txtorganigrama_7.Text = odbLector(18).ToString
                txtorganigrama_8.Text = odbLector(19).ToString
                txtorganigrama_9.Text = odbLector(20).ToString
                txtorganigrama_10.Text = odbLector(21).ToString
                txtorganigrama_11.Text = odbLector(22).ToString
                txtorganigrama_12.Text = odbLector(23).ToString
                txtfunciones_1.Value = odbLector(24).ToString
                txtfunciones_2.Value = odbLector(25).ToString
                txtfunciones_3.Value = odbLector(26).ToString
                txtfunciones_4.Value = odbLector(27).ToString
                txtfunciones_5.Text = odbLector(28).ToString
                txtfunciones_6.Text = odbLector(29).ToString
                txtfunciones_7.Text = odbLector(30).ToString
                txtfunciones_8.Text = odbLector(31).ToString
                txtfunciones_9.Text = odbLector(32).ToString
                txtfunciones_10.Text = odbLector(33).ToString
                txtfunciones_11.Text = odbLector(34).ToString
                txtfunciones_12.Text = odbLector(35).ToString
                txtFacultadesAutorizacion.Value = odbLector(36).ToString
                txtAlcanceResponsabilidad.Value = odbLector(37).ToString
                txtRelaciones.Value = odbLector(38).ToString
                txtgrado_escolaridad.Text = odbLector(39).ToString
                txtCarrera_especializacion.Text = odbLector(40).ToString
                txtconocimiento_tecnico_1.Text = odbLector(41).ToString
                txtconocimiento_dominio_1.Text = odbLector(42).ToString
                txtconocimiento_tecnico_2.Text = odbLector(43).ToString
                txtconocimiento_dominio_2.Text = odbLector(44).ToString
                txtconocimiento_tecnico_3.Text = odbLector(45).ToString
                txtconocimiento_dominio_3.Text = odbLector(46).ToString
                txtconocimiento_tecnico_4.Text = odbLector(47).ToString
                txtconocimiento_dominio_4.Text = odbLector(48).ToString
                txtconocimiento_tecnico_5.Text = odbLector(49).ToString
                txtconocimiento_dominio_5.Text = odbLector(50).ToString
                txtconocimiento_tecnico_6.Text = odbLector(51).ToString
                txtconocimiento_dominio_6.Text = odbLector(52).ToString
                txtidioma_1.Text = odbLector(53).ToString
                txtidioma_dominio_1.text = odbLector(54).ToString
                txtidioma_2.Text = odbLector(55).ToString
                txtidioma_dominio_2.text = odbLector(56).ToString
                txtsistema_gestio_integrado.Text = odbLector(57).ToString
                txthabilidades_competencias_1.Text = odbLector(58).ToString
                txthabilidades_competencias_dominio_1.Text = odbLector(59).ToString
                txthabilidades_competencias_2.Text = odbLector(60).ToString
                txthabilidades_competencias_dominio_2.text = odbLector(61).ToString
                txthabilidades_competencias_3.Text = odbLector(62).ToString
                txthabilidades_competencias_dominio_3.Text = odbLector(63).ToString
                txthabilidades_competencias_4.Text = odbLector(64).ToString
                txthabilidades_competencias_dominio_4.Text = odbLector(65).ToString
                txthabilidades_competencias_5.Text = odbLector(66).ToString
                txthabilidades_competencias_dominio_5.text = odbLector(67).ToString
                txthabilidades_competencias_6.Text = odbLector(68).ToString
                txthabilidades_competencias_dominio_6.text = odbLector(69).ToString
                txthabilidades_competencias_7.Text = odbLector(70).ToString
                txthabilidades_competencias_dominio_7.Text = odbLector(71).ToString
                txthabilidades_competencias_8.Text = odbLector(72).ToString
                txthabilidades_competencias_8.Text = odbLector(73).ToString
                txtareas_expereiencia.Text = odbLector(74).ToString
                txttiempo.Text = odbLector(75).ToString
                txtObservaciones.Value = odbLector(76).ToString

                txtFecEla.Text = odbLector(78).ToString
                odbLector.Close()

            End If
            odbConexion.Close()
        Catch ex As Exception
            '   lblMensajes.Text = Err.Number & " " & ex.Message
        End Try
    End Sub

    Public Function tipoVacante(ByVal vacante As String) As String
        Dim strRes As String = ""
        Select Case vacante
            Case "RE"
                strRes = "REEMPLAZO"
            Case "EV"
                strRes = "EVENTUAL"
            Case "NC"
                strRes = "NUEVA CREACIÓN"
        End Select
        Return strRes
    End Function

    Public Function contracionD(ByVal contracion As String) As String
        Dim strRes As String = ""
        Select Case contracion
            Case "E"
                strRes = "EVENTUAL"
            Case "P"
                strRes = "PERMANENTE"

        End Select
        Return strRes
    End Function
    Public Function bancoE(ByVal banco As String) As String
        Dim strRes As String = ""
        Select Case banco
            Case "SN"
                strRes = ""
            Case "BN"
                strRes = "BANORTE"
            Case "BX"
                strRes = "BANAMEX"

        End Select
        Return strRes
    End Function

    Public Function EdoCivil(ByVal banco As String) As String
        Dim strRes As String = ""
        Select Case banco
            Case "CAS"
                strRes = "CASADO (A)"
            Case "DIV"
                strRes = "DIVORCIADO (A)"
            Case "VIU"
                strRes = "VIUDO (A)"
            Case "SOL"
                strRes = "SOLTERO (A)"
            Case "ULI"
                strRes = "UNION LIBRE"

        End Select


        Return strRes
    End Function
#End Region

#Region "seguridad"
    Public Function obtenerUsurioAD(ByVal odbConexion As OleDbConnection) As ArrayList
        Dim strNombreUsuario As String
        Dim objDirectoryEntry As New DirectoryEntry("LDAP://" & System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain.ToString) ' "LDAP://" & me.text1.text) 
        Dim objDirectorySearcher As New DirectorySearcher(objDirectoryEntry)
        Dim mySearcher As New System.DirectoryServices.DirectorySearcher(objDirectoryEntry)
        Dim result As System.DirectoryServices.SearchResult
        Dim lstArreglo As New ArrayList
        Dim strNombreCompletoAD As String

        strNombreUsuario = IIf(User.Identity.Name = "", System.Environment.UserName, User.Identity.Name)

        If strNombreUsuario.Contains("\") Then
            Dim intfin As Integer
            intfin = (strNombreUsuario.Length - strNombreUsuario.LastIndexOf("\")) - 1
            strNombreUsuario = strNombreUsuario.Substring(strNombreUsuario.LastIndexOf("\") + 1, intfin)
        End If

        mySearcher.Filter = "(sAMAccountName=" + strNombreUsuario + ")"
        'asigna el userid
        lstArreglo.Add(strNombreUsuario)

        result = mySearcher.FindOne()
        ''Validacion si existe el usuario
        If result Is Nothing Then
            Response.Redirect("sinacceso.html")
        Else
            Dim directoryEntry As New DirectoryEntry
            directoryEntry = result.GetDirectoryEntry()
            'Asigna el nombre de usuario
            'Asigna el nombre de usuario
            strNombreCompletoAD = directoryEntry.Name.Replace("CN=", "").ToString.Trim

            'Tratamiento de Remplazo de acentos.
            strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Á", "A")
            strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("É", "E")
            strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Í", "I")
            strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Ó", "O")
            strNombreCompletoAD = strNombreCompletoAD.ToUpper.Replace("Ú", "U")

            '   hdNombreCompleto.Value = strNombreCompletoAD

            lstArreglo.Add(directoryEntry.Name.Replace("CN=", "").ToString.Trim)

        End If

        Return lstArreglo
    End Function
#End Region

    Public Sub RemplazarURL()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "url", "<script>ChangeUrl('Alta de Empleado FO-RH-023/6', 'imprimir.aspx');</script>", False)
    End Sub



    Private Function ObtEmpreEmpleado(ByVal odbConexion As OleDbConnection, ByVal id As String) As Integer
        Dim iEmpresa As Integer = 1 'indicamos que cargue los catalogos de passa
        Dim SQRY As String = "SELECT EMPRESA FROM DIN_INFO_EMPLEADO WHERE IDFILE='" & id & "'"

        Dim odbComando As OleDbCommand = New OleDbCommand(SQRY, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()
        If odbLector.HasRows Then
            odbLector.Read()
            iEmpresa = odbLector(0)
            odbLector.Close()
        End If
        Return iEmpresa
    End Function
    'Nombre del Autorizador
    Private Function ObtNombreAutorizador(ByVal odbConexion As OleDbConnection, ByVal id As String) As String
        Dim strResultado As String = "" 'indicamos que cargue los catalogos de passa
        Dim SQRY As String = "SELECT [nombre] FROM [DIN_LOGIN] WHERE usuarioId='" & id & "'"

        Dim odbComando As OleDbCommand = New OleDbCommand(SQRY, odbConexion)

        Dim odbLector As OleDbDataReader
        odbLector = odbComando.ExecuteReader()
        If odbLector.HasRows Then
            odbLector.Read()
            strResultado = odbLector(0)
            odbLector.Close()
        End If
        Return strResultado
    End Function
End Class