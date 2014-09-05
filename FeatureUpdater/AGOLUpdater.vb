Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Net
Imports System.Text
Imports System.Reflection
Imports Newtonsoft.Json



Module AGOLUpdater
    Private _appID As String = "4hq2MFABP3bduUix"
    Private _appSecret As String = "9e4b5ac15bdb48c3b3df70e521f8b8e1"
    Private _token As String
    Private _tokenURL As String = "https://www.arcgis.com/sharing/oauth2/token"

    Sub Main()

        _token = GetToken()
        If True Then
            Dim ds As DataSet = GetSitesData()

            Dim siteData As New List(Of sites_json)
            siteData = convertSitestoJSON(ds)
            SendSitesData(siteData)
        Else
            Dim newData As New List(Of rmd_json)
            'Dim di As New rmd_json

            newData = convertRMDtoJSON(GetRMDData())
            SendRMDFeature(newData)

        End If


        Console.ReadLine()
    End Sub
    Private Sub SendSitesData(siteData As List(Of sites_json))
        Dim deleteURL As String = "http://services.arcgis.com/pgFCLzxld5dtMQ6u/ArcGIS/rest/services/BaseSites/FeatureServer/0/deleteFeatures"
        deleteURL += "?token=" + _token

        Dim req As HttpWebRequest
        req = WebRequest.Create(deleteURL)
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim formData As Byte()
        Dim strFeatures As String
        strFeatures = "f=json&rollbackOnFailure=true&where=1%3D1"
        formData = UTF8Encoding.UTF8.GetBytes(strFeatures)
        Using post As Stream = req.GetRequestStream
            post.Write(formData, 0, formData.Length)
        End Using

        Using resp As HttpWebResponse = req.GetResponse
            Dim sr As New StreamReader(resp.GetResponseStream)
            Console.WriteLine(sr.ReadToEnd)
        End Using

        Dim addURL As String = "http://services.arcgis.com/pgFCLzxld5dtMQ6u/ArcGIS/rest/services/BaseSites/FeatureServer/0/addFeatures"
        addURL += "?token=" + _token
        Dim addReq As HttpWebRequest

        addReq = WebRequest.Create(addURL)
        addReq.Method = "POST"
        addReq.ContentType = "application/x-www-form-urlencoded"
        addReq.Timeout = "300000"
        Dim addData As Byte()
        Dim strAddFeatures As String
        strAddFeatures = "f=json&rollbackOnFailure=true&"
        strAddFeatures += "features=" + JsonConvert.SerializeObject(siteData, Formatting.Indented).Replace("&", "and")
        addData = UTF8Encoding.UTF8.GetBytes(strAddFeatures)
        Using addPost As Stream = addReq.GetRequestStream
            addPost.Write(addData, 0, addData.Length)
        End Using
        Using addResp As HttpWebResponse = addReq.GetResponse
            Dim sr As New StreamReader(addResp.GetResponseStream)
            Console.WriteLine(sr.ReadToEnd)
        End Using

    End Sub

    Private Function GetSitesData() As DataSet

        'because the data is delivered via stored procedure i used more traditional ado.net structures
        'Due to lack of familiarity with Linq and Stored Procedures.
        Dim retVal As New DataSet
        Dim conn As New SqlConnection("Data Source=SPD-REPORT\SQLEXPRESS;Initial Catalog=MSProjectImport;Integrated Security=True")
        Dim comm As New SqlCommand
        Dim da As New SqlDataAdapter
        comm.CommandText = "sp_all_projects_crossdomain_grouped"
        comm.CommandType = CommandType.StoredProcedure
        comm.Parameters.Add(New SqlParameter("@SiteName", DBNull.Value))
        comm.Connection = conn
        da.SelectCommand = comm
        da.Fill(retVal)

        Return retVal
    End Function

    Private Function convertSitestoJSON(ds As DataSet) As List(Of sites_json)
        Dim retVal As New List(Of sites_json)
        'Dim iStop As Integer = 0
        For Each dr As DataRow In ds.Tables(0).Rows
            Dim site As New sites_json
            site.attributes.SiteName = dr("SiteName").ToString()
            site.attributes.Address = dr("Address").ToString()
            'If Not IsDBNull(dr("CHKSUM")) Then site.attributes.CHKSUM = UTF8Encoding.UTF8.GetString(dr("CHKSUM"))
            site.attributes.City = dr("City").ToString()
            If Not IsDBNull(dr("ExcavationDate")) Then site.attributes.ExcavationDate = dr("ExcavationDate")
            If Not IsDBNull(dr("MoveInDate")) Then site.attributes.MoveInDate = dr("MoveInDate")
            If Not IsDBNull(dr("NumberOfUnits")) Then site.attributes.NumberOfUnits = dr("NumberOfUnits")
            If Not IsDBNull(dr("OccupancyDate")) Then site.attributes.OcccupancyDate = dr("OccupancyDate")
            site.attributes.PostalCode = dr("PostalCode").ToString
            If Not IsDBNull(dr("project_uid")) Then site.attributes.project_uid = dr("project_uid")
            site.attributes.Province = dr("Province").ToString
            site.attributes.SiteStatus = dr("SiteStatus").ToString
            site.attributes.Type = dr("Type").ToString
            If Not IsDBNull(dr("VPGLCode")) Then site.attributes.VPGLCode = dr("VPGLCode")
            If Not IsDBNull(dr("Long")) Then site.geometry.x = dr("Long")
            If Not IsDBNull(dr("Lat")) Then site.geometry.y = dr("Lat")
            site.geometry.spatialReference.wkid = 4326
            retVal.Add(site)
            'iStop += 1
            'If iStop = 50 Then Exit For
        Next
        Return retVal
    End Function

    Private Function GetRMDData() As List(Of rental_data_captured)
        Dim dbData As New GISEntities()

        Dim q = From p In dbData.rental_data_captured _
                Where p.sent_to_AGOL Is Nothing _
                Select p

        Dim newEntries As IEnumerable(Of rental_data_captured) = q.ToList()

        Dim i As Integer = 0
        Return newEntries
    End Function


    Private Function convertRMDtoJSON(rdc As List(Of rental_data_captured)) As List(Of rmd_json)
        Dim retVal As New List(Of rmd_json)
        Dim t As Type = rdc(0).GetType
        Dim attributeExclusion() As String = {"point", "latitude", "longitude", "sent_to_AGOL", "availability", "created_timestamp"}
        Dim exitFor As Boolean = False
        Dim iStop = 0
        If rdc.Count > 0 Then
            For Each rentRecord As rental_data_captured In rdc
                iStop += 1
                Dim jsonRec As New rmd_json
                jsonRec.geometry.spatialReference.wkid = 4326
                jsonRec.geometry.x = rentRecord.longitude
                jsonRec.geometry.y = rentRecord.latitude
                'jsonRec.attributes.sent_to_AGOL = DateTime.Now.ToString("s")
                jsonRec.attributes.created_timestamp = rentRecord.created_timestamp.ToString("s")
                If rentRecord.availability.HasValue Then jsonRec.attributes.availability = rentRecord.availability.Value.ToString("s")
                Dim jStop As Integer = 0
                For Each p As PropertyInfo In t.GetProperties
                    exitFor = False
                    jStop += 1
                    For i As Integer = 0 To attributeExclusion.Length - 1
                        If p.Name = attributeExclusion(i) Then
                            exitFor = True
                            Exit For
                        End If
                    Next
                    If Not exitFor Then
                        If p.PropertyType.Name = "String" Then
                            Dim s As String = CallByName(rentRecord, p.Name, CallType.Get)
                            If Not IsNothing(s) Then
                                s = Replace(Trim(s), "&", "and")
                                'If s.Length > 254 Then s = s.Substring(0, 254)
                                CallByName(jsonRec.attributes, p.Name, CallType.Set, s)
                            End If
                        Else
                            CallByName(jsonRec.attributes, p.Name, CallType.Set, CallByName(rentRecord, p.Name, CallType.Get))
                        End If
                    End If
                Next
                retVal.Add(jsonRec)
                If jStop = 10 Then Exit For
            Next
        End If

        Return retVal
    End Function

    Private Sub SendRMDFeature(rentData As List(Of rmd_json))
        Dim strURL As String = "https://services.arcgis.com/pgFCLzxld5dtMQ6u/ArcGIS/rest/services/RentalMarketData/FeatureServer/0/addfeatures"
        strURL += "?f=json"
        strURL += "&token=" + _token

        Dim req As HttpWebRequest
        req = WebRequest.Create(strURL)
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim formData As Byte()

        Dim ms As New MemoryStream
        Dim ser As New Json.DataContractJsonSerializer(GetType(rmd_json()))
        ser.WriteObject(ms, rentData.ToArray)
        ms.Position = 0
        Dim sr As New StreamReader(ms)
        '        Console.WriteLine(sr.ReadToEnd())
        Dim strFeatures = "features=" + sr.ReadToEnd().ToString()
        strFeatures = "f=json&rollbackOnFailure=true&" + strFeatures
        formData = UTF8Encoding.UTF8.GetBytes(strFeatures)
        Using post As Stream = req.GetRequestStream
            post.Write(formData, 0, formData.Length)
        End Using
        Using resp As HttpWebResponse = req.GetResponse
            Dim addSer As New Json.DataContractJsonSerializer(GetType(AddResults))
            Dim ar As New AddResults
            ar = CType(addSer.ReadObject(resp.GetResponseStream()), AddResults)
            'Console.WriteLine()
            For Each result As ResultItem In ar.addResults
                'If result.error.
            Next
        End Using

    End Sub
    Private Function GetToken() As String
        Dim params As String = "client_id=" + _appID
        params += "&client_secret=" + _appSecret
        params += "&grant_type=client_credentials"

        Dim req As HttpWebRequest
        req = WebRequest.Create(_tokenURL)
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim FormData As Byte() = UTF8Encoding.UTF8.GetBytes(params)
        Using post As Stream = req.GetRequestStream
            post.Write(FormData, 0, FormData.Length)
        End Using

        Dim tr As TokenResults
        Using resp As HttpWebResponse = req.GetResponse
            Dim ser As New Json.DataContractJsonSerializer(GetType(TokenResults))
            tr = CType(ser.ReadObject(resp.GetResponseStream()), TokenResults)
            'Console.WriteLine(tr.access_token)
        End Using

        Return tr.access_token
    End Function

    <DataContract> _
    Private Class TokenResults

        <DataMember> _
        Public access_token As String

        <DataMember> _
        Public expires_in As Long

    End Class

    'The following code was removed due to the complexity it was adding to the program
    'this could be revisited at a future date to allow for a more non destructive add update delete cycle.
    'Dim queryURL As String = "http://services.arcgis.com/pgFCLzxld5dtMQ6u/ArcGIS/rest/services/BaseSites/FeatureServer/0/query"
    'queryURL += "?f=json"
    'queryURL += "&token=" + _token
    'queryURL += "&where=1%3D1&outFields=*&returnGeometry=true"
    'Dim req As HttpWebRequest
    'req = WebRequest.Create(queryURL)
    'req.Method = "GET"
    'req.ContentType = "application/x-www-form-urlencoded"
    'Dim existingSites As sites_return
    'Using resp As HttpWebResponse = req.GetResponse
    '    Dim sr As New StreamReader(resp.GetResponseStream)
    '    Dim json As String = sr.ReadToEnd.ToString()
    '    existingSites = JsonConvert.DeserializeObject(Of sites_return)(json, New EsriDateTimeConverter)
    'End Using
    ''Dim formData As Byte()

    'Dim ms As New MemoryStream
    'For Each dr As DataRow In ds.Tables(0).Rows
    '    Dim siteName As String = dr.Item("SiteName")
    '    Dim siteFound As Boolean = False
    '    For Each f As sites_features In existingSites.features
    '        If f.attributes.SiteName = siteName Then
    '            siteFound = True
    '            If f.attributes.CHKSUM <> dr.Item("CHKSUM") Then

    '            End If
    '        End If
    '    Next

    'Next

End Module
