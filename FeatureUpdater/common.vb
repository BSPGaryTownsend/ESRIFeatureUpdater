Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Net
Imports System.Text

<DataContract> _
Public Class Point
    <DataMember> _
    Public x As Double

    <DataMember> _
    Public y As Double

    <DataMember>
    Public spatialReference As SRS
    Sub New()
        spatialReference = New SRS
    End Sub
End Class

<DataContract> _
Public Class SRS
    <DataMember> _
    Public wkid As Integer
End Class

