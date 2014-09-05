Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Net
Imports System.Text

<DataContract> _
Public Class RMD
    <DataMember> Public key_id As Integer
    <DataMember> Public site_title As String
    <DataMember> Public business_type As String
    <DataMember> Public company_name As String
    <DataMember> Public address As String
    <DataMember> Public city As String
    <DataMember> Public province As String
    <DataMember> Public postalcode As String
    <DataMember> Public num_bedrooms As Integer
    <DataMember> Public rent_amount As Integer
    <DataMember> Public incentive As String
    <DataMember> Public num_bathrooms As Double
    <DataMember> Public square_footage As Integer
    <DataMember> Public year_built As Integer
    <DataMember> Public availability As String
    <DataMember> Public num_units As Integer
    <DataMember> Public electricity As Integer
    <DataMember> Public electricity_extra As String
    <DataMember> Public heating As Integer
    <DataMember> Public heating_type As String
    <DataMember> Public heating_extra As String
    <DataMember> Public hot_water As Integer
    <DataMember> Public hot_water_extra As String
    <DataMember> Public laundry_type As String
    <DataMember> Public cable As Integer
    <DataMember> Public cable_extra As String
    <DataMember> Public internet As Integer
    <DataMember> Public internet_extra As String
    <DataMember> Public storage As Integer
    <DataMember> Public storage_extra As String
    <DataMember> Public pets As Integer
    <DataMember> Public pets_extra As String
    <DataMember> Public pets_comments As String
    <DataMember> Public parking_type As String
    <DataMember> Public parking_extra As String
    <DataMember> Public has_elevator As Integer
    <DataMember> Public has_month_to_month_lease As Integer
    <DataMember> Public security_deposit As Integer
    <DataMember> Public senior_discount As Integer
    <DataMember> Public website As String
    <DataMember> Public has_aircon As Integer
    <DataMember> Public has_dishwasher As Integer
    <DataMember> Public has_washer As Integer
    <DataMember> Public has_dryer As Integer
    <DataMember> Public has_stove As Integer
    <DataMember> Public has_fridge As Integer
    <DataMember> Public has_microwave As Integer
    <DataMember> Public has_furniture As Integer
    <DataMember> Public has_common_room As Integer
    <DataMember> Public has_exercise_room As Integer
    <DataMember> Public has_pool As Integer
    <DataMember> Public has_adults_only As Integer
    <DataMember> Public has_den As Integer
    <DataMember> Public has_balcony As Integer
    <DataMember> Public has_smoking As Integer
    <DataMember> Public near_parks As Integer
    <DataMember> Public near_hospital As Integer
    <DataMember> Public near_school As Integer
    <DataMember> Public near_church As Integer
    <DataMember> Public near_fire_department As Integer
    <DataMember> Public near_transit As Integer
    <DataMember> Public near_mall As Integer
    <DataMember> Public near_police As Integer
    <DataMember> Public near_ambulance As Integer
    <DataMember> Public created_by As String
    <DataMember> Public created_timestamp As String
    <DataMember> Public powered_parking As Integer
    <DataMember> Public parking_stalls_included As Integer
    <DataMember> Public secure_parking As Integer
    <DataMember> Public contact_info As String
    '<DataMember> Public sent_to_AGOL As String
End Class