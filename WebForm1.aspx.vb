Imports System.Data.OleDb

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call Escribir()
    End Sub

    Public Sub Prueba()
        'Variables locales

        Dim oExcel As Object
        Dim oBook As Object
        Dim oSheet As Object

        'Iniciar un nuevo libro en Excel

        oExcel = CreateObject("Excel.Application")

        oBook = oExcel.Workbooks.Add

        'Agregar datos a las celdas de la primera hoja en el libro nuevo

        oSheet = oBook.Worksheets(1)

        ' Agregamos Los datos que queremos agregar

        oSheet.Range("A3").Value = "sasaas"

        ' Esta celda tendra los datos del textbox

        oSheet.Range("A10").Value = "CODIGO"

        ' estas celdas por defecto solo seran para identificar cada columna

        oSheet.Range("B10").Value = "TIPO"

        oSheet.Range("C10").Value = "DESCRIPCION"

        oSheet.Range("D10").Value = "MODELO"

        oSheet.Range("E10").Value = "REGIS"

        oSheet.Range("F10").Value = "DISPO"

        'Desde aqui empezaremos a exportar la lista


        ' hacemos visible el documento

        oExcel.Visible = True

        oExcel.UserControl = True

        'Guardaremos el documento en el escritorio con el nombre prueba

        oBook.SaveAs(Environ("UserProfile") & "\desktop\Prueba.xls")
    End Sub
    Private Sub Escribir()
        Dim path As String = Server.MapPath("~/UploadedFiles/") & "\Reclutamiento\2018104183515Administrador de Proyectos de Inversión.xlsx"

        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & path & "; Extended Properties=""Excel 12.0 Xml; HDR = NO;"""
        Dim conn As New OleDbConnection(connectionString)
        Dim insert As String = "INSERT INTO [FO-RH-4(2)$D13:D13] VALUES( 'Miguel')"
        Dim insertCommand As New OleDbCommand(insert, conn)
        Dim cmd As New OleDbCommand
        Try
            'insertCommand.Parameters.Add("Nombre", OleDbType.VarChar).Value = "Josue"
            'insertCommand.Parameters.Add("Apellidos", OleDbType.VarChar).Value = "Torres"
            'insertCommand.Parameters.Add("Direccion", OleDbType.VarChar).Value = "Hidalgo"
            conn.Open()
            insertCommand.ExecuteNonQuery()
            cmd.Connection = conn
            Dim sql As String = "UPDATE [Prueba$A3:A3] SET F2 = @F2value"
            cmd.CommandText = "UPDATE [Prueba$A1:D1] SET F1 = 'A1', F2 = 'B1', F3 = 'C1', F4 = 'D1'"

            'cmd.Parameters.AddWithValue("@F2value", 10)

            'cmd.Parameters.AddWithValue("@F1value", 5)
            'cmd.CommandText = sql

            '  cmd.ExecuteNonQuery()
        Finally
            conn.Close()

        End Try

    End Sub




End Class