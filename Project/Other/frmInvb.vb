Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms

Public Class frmInvb

    Private Sub frmInvb_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Connection string to Access database
        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\db_banmai1.accdb"

        ' SQL query to fetch data
        Dim query As String = "SELECT Member.m_name AS ชื่อสมาชิก, " &
            "SUM(IIF(Income.ind_accname = 'เงินฝากสัจจะ' AND MONTH(Income.inc_date) = 1, Income.ind_amount, 0)) AS มกราคม, " &
            "SUM(IIF(Income.ind_accname = 'เงินฝากสัจจะ' AND MONTH(Income.inc_date) = 2, Income.ind_amount, 0)) AS กุมภาพันธ์, " &
            "SUM(IIF(Income.ind_accname = 'เงินฝากสัจจะ' AND MONTH(Income.inc_date) = 3, Income.ind_amount, 0)) AS มีนาคม " &
            "FROM Member LEFT JOIN Income ON Member.m_id = Income.m_id " &
            "WHERE Income.ind_accname = 'เงินฝากสัจจะ' " &
            "GROUP BY Member.m_name;"

        ' Create connection and data adapter
        Using connection As New OleDbConnection(connectionString)
            Dim adapter As New OleDbDataAdapter(query, connection)
            Dim dataSet As New DataSet()

            Try
                ' Fill the dataset
                adapter.Fill(dataSet, "MemberDeposits")

                ' Set the data source for Report Viewer
                ReportViewer1.LocalReport.DataSources.Clear()
                Dim rds As New ReportDataSource("DataSet1", dataSet.Tables("MemberDeposits"))
                ReportViewer1.LocalReport.DataSources.Add(rds)

                ' Refresh the report
                ReportViewer1.RefreshReport()
            Catch ex As OleDbException
                MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
            End Try
        End Using
    End Sub

End Class
