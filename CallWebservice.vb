Imports System.Data
Imports System.Collections.Generic
Imports System.Net.NetworkInformation
Imports POSMySQL.POSControl
Imports MySql.Data.MySqlClient
Imports System.Configuration
Imports AMS
Imports System.Windows.Forms
Imports DbUtilsModule

<Serializable()> _
Public Class CallWebservice
    Shared myWSV6 As New WSV6.Service
    Shared objDB As New CDBUtil()
    Shared objCnn As New MySqlConnection()
    Shared XMLProfile As AMS.Profile.Xml
    Const DATABASESETTINGNODENAME As String = "FrontDataSetting"
    Const CONFIGTAYWINFILE As String = "pRoMiSeFrontRes.xml"
    Const MANAGEDATASETTINGNODENAME As String = "ManageDataSetting"
     
    Shared Function SearchMember(ByVal searchBy As WSV6.SearchMemberBy, ByVal paramSearch As String, ByRef memberData() As WSV6.Member_Data, ByRef dsMember As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.SearchMember(searchBy, paramSearch, memberData, dsMember, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try
    End Function
    Shared Function GetMember(ByVal searchBy As WSV6.SearchMemberBy, ByVal memberCode As String, ByVal memberMobile As String, ByRef memberData As WSV6.Member_Data, ByRef dsMember As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            Dim ip As String = ""
            Dim dataBaseName As String = ""
            Dim regionID As Integer
            Dim computerid As Integer
            Dim exchange As String = ""
            ConnectionSetting(pathConfigXML, ip, dataBaseName, regionID, computerid, exchange)
            objCnn = objDB.EstablishConnection(ip, dataBaseName)
            If myWSV6.GetMember(searchBy, memberCode, memberMobile, memberData, dsMember, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try
    End Function
    Shared Function AddUpdateMembers(ByVal fromShopID As Integer, ByVal destinationShopID As Integer, ByVal dtData As DataTable, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            Dim dsData As New DataSet
            dtData.TableName = "Members"
            dsData.Tables.Add(dtData)
            ConfigWebService(pathConfigXML)
            If myWSV6.AddUpdateMembersAtQH(fromShopID, destinationShopID, dsData, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try
    End Function

    Shared Function SummaryPoint(ByVal memberID As Integer, ByRef memberData As WSV6.SummaryPoint_Data, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.SearchSummaryPoint(memberID, memberData, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString
            Return False
        End Try
    End Function
    Shared Function ImportSummarySaleByDateToHQ(ByRef dsSummarySale As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.ImportSummarySaleByDateToHQ(dsSummarySale, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try
    End Function
    Shared Function ImportSummaryPointToHQ(ByRef dsSummarySale As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.ImportRewardPointSummaryAtHQ(dsSummarySale, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try

    End Function
    Shared Function Payment_PaybyvoucherSendToHQ(ByRef dsData As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.Payment_Paybyvoucher(dsData, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try

    End Function
    Shared Function ExportDataSetToBranch(ByRef dsResult() As DataSet, ByVal regionID As Integer, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.ExportDataSetToBranch(dsResult, regionID, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try
    End Function
    Shared Function AutoUpdateDataSetToHQ(ByRef dsResult As DataSet, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.AutoUpdateDataSetToHQ(dsResult, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try

    End Function
    Shared Function SendPointSummaryToHeadquarter(ByVal objCnnn As MySql.Data.MySqlClient.MySqlConnection, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            Dim IPAddress As String = ""
            Dim databaseName As String = ""
            Dim regionID As Integer
            Dim computerID As Integer
            Dim exchangeDirectory As String = ""
            'ConfigWebService(pathConfigXML)
            ConnectionSetting(pathConfigXML, IPAddress, databaseName, regionID, computerID, exchangeDirectory)
            Dim ManageData As New pRoMiSe_ManageData_Class.pRoMiSeExportImportDataProcess(IPAddress, databaseName, regionID, 1, exchangeDirectory, Application.StartupPath, pRoMiSe_ManageData_Class.ProgramFor.HeadQuarter)
            Dim DsPointSummary() As DataSet
            ReDim DsPointSummary(-1)
            Dim DsUpdateDsPointSummary As DataSet
            Dim xResultText As String = ""
            If ManageData.AutoExportRedeemRewardPointDataToHQ(objCnnn, regionID, DsPointSummary, xResultText) = True Then
                For i As Integer = 0 To DsPointSummary.Length - 1
                    DsUpdateDsPointSummary = DsPointSummary(i).Copy
                    If ImportSummarySaleByDateToHQ(DsUpdateDsPointSummary, xResultText) = False Then
                        strResultText = "Import point at HQ fial. : " & xResultText
                        Return False
                    Else

                        If ManageData.AutoSetDataInDataSetExportToHQAtBranch(objCnnn, DsUpdateDsPointSummary, xResultText) = False Then
                            strResultText = "Set alreadyexporttohq at branch fial. : " & xResultText
                            Return False
                        Else
                            strResultText = ""
                            Return True
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString
            Return False
        End Try
        Return True
    End Function
    Private Shared Function PingIPAddress(ByVal IPHQ As String) As Boolean
        Try
            Dim ping As New Ping()
            Dim reply As PingReply = ping.Send(IPHQ, 3000)
            ' 3 sec 
            If reply.Status <> IPStatus.Success Then
                ' Server cannot connect 
                Return False
            Else
                Return True
            End If
        Catch e As Exception
            Return False
        End Try
    End Function
    Shared Function UpdateSoftwareVersion(ByVal ComputerID As Integer, ByVal ProductLevelID As Integer, ByVal IPAddress As String, ByVal FrontVersion As String, ByVal FrontFileDate As String, ByVal FrontUpdateDate As String, ByVal backOfficeVersion As String, ByVal backOfficeFileDate As String, ByVal backOfficeUpdateDate As String, ByVal InvVersion As String, ByVal InvFileDate As String, ByVal InvUpdateDate As String, ByRef strResultText As String) As Boolean
        Try
            'Dim pathConfigXML As String = ""
            'ConfigWebService(pathConfigXML)
            'If myWSV6.UpdateSoftwareVersion(ComputerID, ProductLevelID, IPAddress, FrontVersion, FrontFileDate, FrontUpdateDate, backOfficeVersion, backOfficeFileDate, backOfficeUpdateDate, InvVersion, InvFileDate, InvUpdateDate, strResultText) = True Then
            '    Return True
            'Else
            '    Return False
            'End If
            Return True
        Catch ex As Exception
            strResultText = ex.Message.ToString
            Return False
        End Try

    End Function
    Shared Function GetSoftwareversion(ByVal programTypeID As Integer, ByVal pathConfigXML As String, ByRef softwareData As WSV6.Softwareversion_Data, ByRef strResultText As String) As Boolean
        Try
            'ConfigWebService(pathConfigXML)
            'If myWSV6.GetSoftwareVersion(programTypeID, softwareData, strResultText) = True Then
            '    Return True
            'Else
            '    Return False
            'End If
            Return False
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try

    End Function
    Shared Function GetContentLastUpdate(ByVal shopID As Integer, ByVal sectionID As Integer, ByVal limitContent As Integer, ByRef contentData() As WSV6.News_CategoryData, ByRef strResultText As String) As Boolean
        Try
            Dim pathConfigXML As String = ""
            ConfigWebService(pathConfigXML)
            If myWSV6.ContentLastUpdate(shopID, sectionID, limitContent, contentData, strResultText) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            strResultText = ex.Message.ToString()
            Return False
        End Try

    End Function
    Shared Function CheckWebserviceOnlineOffline() As Boolean
        XMLProfile = New AMS.Profile.Xml
        XMLProfile.Name = Application.StartupPath & "/" & CONFIGTAYWINFILE
        Dim strSection As String
        Dim URLWSV6 As String
        strSection = DATABASESETTINGNODENAME
        URLWSV6 = XMLProfile.GetValue(strSection, "URLWebservice")
        Try
            Dim ping As New Ping()
            Dim reply As PingReply = ping.Send(URLWSV6, 3000)
            ' 3 sec 
            If reply.Status <> IPStatus.Success Then
                ' Server cannot connect 
                Return False
            Else
                Return True
            End If
        Catch e As Exception
            Return False
        End Try
    End Function
    Private Shared Sub ConfigWebService(ByVal pathConfigXML As String)
        XMLProfile = New AMS.Profile.Xml
        If pathConfigXML = "" Then
            XMLProfile.Name = Application.StartupPath & "/" & CONFIGTAYWINFILE
        Else
            XMLProfile.Name = pathConfigXML
        End If
        Dim strSection As String
        Dim URLWSV6 As String
        strSection = DATABASESETTINGNODENAME
        URLWSV6 = XMLProfile.GetValue(strSection, "URLWebservice")
        myWSV6.Timeout = (60000 * 2)
        myWSV6.Url = URLWSV6
    End Sub
    Private Shared Function ConnectionSetting(ByVal pathConfigXML As String, ByRef IPAddr As String, ByRef databaseName As String, ByRef regionID As Integer, ByRef computerID As Integer, ByRef exchangeDirectory As String) As Boolean
        Try
            XMLProfile = New AMS.Profile.Xml
            If pathConfigXML = "" Then
                XMLProfile.Name = Application.StartupPath & "/" & CONFIGTAYWINFILE
            Else
                XMLProfile.Name = pathConfigXML
            End If
            Dim strSection As String
            Dim strSectionMD As String
            strSection = DATABASESETTINGNODENAME
            IPAddr = XMLProfile.GetValue(strSection, "IPAddress")
            databaseName = XMLProfile.GetValue(strSection, "DBName")
            computerID = XMLProfile.GetValue(strSection, "ComputerID")
            strSectionMD = MANAGEDATASETTINGNODENAME
            regionID = XMLProfile.GetValue(strSectionMD, "RegionID")
            ExchangeDirectory = XMLProfile.GetValue(strSectionMD, "ExchangeDirectory")
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Shared Function Member_UpdatePackageHistoryAtHQ(ByVal dtData As DataTable, ByRef strResultText As String) As Boolean
    '    Try
    '        Dim pathConfigXML As String = ""
    '        ConfigWebService(pathConfigXML)
    '        'สร้างตัวแปรเภท object จากเซอร์วิสที่อ้างอิงอยู่
    '        Dim packageData As WSV6.Packagehistory
    '        Dim packageHistory() As WSV6.Packagehistory

    '        If dtData.Rows.Count > 0 Then
    '            ReDim packageHistory(-1)
    '            For i As Integer = 0 To dtData.Rows.Count - 1
    '                packageData = New WSV6.Packagehistory()
    '                packageData.PackageHistoryID = dtData.Rows(i)("PackageHistoryID")
    '                packageData.PackageID = dtData.Rows(i)("PackageID")
    '                packageData.ProductLevelID = dtData.Rows(i)("ProductLevelID")
    '                packageData.TransactionID = dtData.Rows(i)("TransactionID")
    '                packageData.ComputerID = dtData.Rows(i)("ComputerID")
    '                packageData.OrderDetailID = dtData.Rows(i)("OrderDetailID")
    '                If Not IsDBNull(dtData.Rows(i)("InsertDateTime")) Then
    '                    packageData.Updatedate = dtData.Rows(i)("InsertDateTime")
    '                Else
    '                    packageData.Updatedate = Date.MinValue
    '                End If
    '                ReDim Preserve packageHistory(packageHistory.Length)
    '                packageHistory(packageHistory.Length - 1) = packageData
    '            Next
    '            If myWSV6.Member_UpdatePackage(packageHistory, strResultText) = True Then
    '                Return True
    '            Else
    '                Return False
    '            End If

    '        End If


    '    Catch ex As Exception
    '        strResultText = ex.Message.ToString()
    '        Return False
    '    End Try
    'End Function
    'Public Shared Function UpdatePackageByMember(ByVal objDB As CDBUtil, ByVal objCnn As MySql.Data.MySqlClient.MySqlConnection) As Boolean
    '    Dim strSQL1 As String = ""
    '    Dim strSQL2 As String = ""
    '    Dim dtData As New DataTable
    '    Dim resultText As String = ""
    '    dtData = objDB.List("select * from packagemember where CompleteStatus=1 and SyncStatus=0", objCnn)
    '    If dtData.Rows.Count > 0 Then
    '        If Member_UpdatePackageHistoryAtHQ(dtData, resultText) = True Then
    '            strSQL1 = Member_GenerateScriptUpdatePackageHistoryAtBranch(dtData)
    '            Try
    '                objDB.sqlExecute(strSQL1, objCnn)
    '            Catch ex As Exception

    '            End Try
    '        Else
    '        End If
    '    End If
    'End Function

    'Shared Function Member_GenerateScriptUpdatePackageHistoryAtBranch(ByVal dtData As DataTable) As String
    '    Dim strSQL As String = ""
    '    Dim updateDate As DateTime
    '    updateDate = Now
    '    If dtData.Rows.Count > 0 Then
    '        objCnn = objDB.EstablishConnection()
    '        For i As Integer = 0 To dtData.Rows.Count - 1
    '            strSQL &= "update packagemember set SyncStatus=1,SyncDateTime=" & DbUtilsModule.DbUtils.FormatDateTime(updateDate) & " where PackageHistoryID=" & dtData.Rows(i)("PackageHistoryID") & " and packageid=" & dtData.Rows(i)("PackageID") & " and productlevelid=" & dtData.Rows(i)("ProductLevelID") & " ;"
    '        Next
    '    End If
    '    Return strSQL
    'End Function

End Class
