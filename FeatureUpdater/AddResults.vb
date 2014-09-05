Imports Microsoft.VisualBasic
Imports System.Runtime.Serialization

<DataContract> _
Public Class AddResults
    <DataMember> Public addResults() As ResultItem
End Class

<DataContract> _
Public Class ResultItem
    <DataMember> Public objectId As String
    <DataMember> Public globalId As String
    <DataMember> Public success As Boolean
    <DataMember> Public [error] As ResultError
End Class

<DataContract> _
Public Class ResultError
    <DataMember> Public code As Double
    <DataMember> Public description As String
End Class
