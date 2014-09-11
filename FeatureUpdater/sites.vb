Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Net
Imports System.Text

Public Class Sites
    Public SiteName As String
    Public Province As String
    Public City As String
    Public Address As String
    Public SiteStatus As String
    Public PostalCode As String
    Public Type As String
    Public VPGLCode As String
    Public project_uid As Guid
    Public NumberOfUnits As Double
    Public ExcavationDate As Date
    Public OcccupancyDate As Date
    Public MoveInDate As Date
    Public Lat As Double
    Public [Long] As Double
    Public LatString As String
    Public LongString As String
    Public CHKSUM As String
    'Public FID As Long
End Class
