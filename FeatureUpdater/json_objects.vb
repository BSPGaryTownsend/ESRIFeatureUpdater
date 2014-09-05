Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Net
Imports System.Text

<DataContract> _
Public Class rmd_json
    <DataMember> _
    Public geometry As Point

    <DataMember> _
    Public attributes As RMD

    Sub New()
        geometry = New Point
        attributes = New RMD
    End Sub

End Class

<DataContract> _
Public Class sites_json
    <DataMember> _
    Public geometry As Point

    <DataMember> _
    Public attributes As Sites

    Sub New()
        geometry = New Point
        attributes = New Sites
    End Sub
End Class

Public Class sites_return
    Public features() As sites_features
    Public fields() As esri_fields
    Public geometryType As String
    Public globalIdFieldName As String
    Public objectFieldIdName As String
    Public spatialReference As esri_srs
End Class

Public Class sites_features
    Public attributes As Sites
    Public geometry As Point
End Class

Public Class esri_fields
    Public [alias] As String
    Public defaultValue As String
    Public domain As String
    Public length As Long
    Public name As String
    Public sqlType As String
    Public type As String
End Class

Public Class esri_srs
    Public latestWkid As Long
    Public wkid As Long
End Class

Public Class EsriDateTimeConverter
    Inherits Newtonsoft.Json.JsonConverter

    Public Overrides Function CanConvert(objectType As Type) As Boolean
        Return objectType = GetType(DateTime)
    End Function

    Public Overrides Function ReadJson(reader As Newtonsoft.Json.JsonReader, objectType As Type, existingValue As Object, serializer As Newtonsoft.Json.JsonSerializer) As Object
        Dim t As Long = Long.Parse(CType(reader.Value, String))
        Return New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t)
    End Function

    Public Overrides Sub WriteJson(writer As Newtonsoft.Json.JsonWriter, value As Object, serializer As Newtonsoft.Json.JsonSerializer)
        Throw New NotImplementedException
    End Sub
End Class