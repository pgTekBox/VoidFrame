Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Namespace prjLeo
    ' Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante.
    <System.Web.Script.Services.ScriptService()>
    <System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
    <ToolboxItem(False)>
    Public Class Leo
        Inherits System.Web.Services.WebService

        Private m_ConnectionString As String = ""
        Public Property ConnectionString() As String
            Get
                Try

                    If m_ConnectionString.Length = 0 Then
                        Dim sConnect As String = System.Configuration.ConfigurationManager.AppSettings("ConnectionString")
                        m_ConnectionString = sConnect
                    End If
                    Return m_ConnectionString
                Catch ex As Exception
                    Return ""
                End Try

            End Get
            Set(ByVal Value As String)
                m_ConnectionString = Value
            End Set
        End Property

        Public Function ExecuteSQLds(ByVal SQLStatement As String, AllParameters As Collection) As DataSet
            Try


                Dim DRconn As SqlClient.SqlConnection
                DRconn = New SqlClient.SqlConnection(ConnectionString)
                Dim MyDA As New SqlClient.SqlDataAdapter

                Dim oCom As New SqlClient.SqlCommand
                oCom.CommandText = SQLStatement
                oCom.Connection = DRconn
                oCom.CommandType = CommandType.StoredProcedure
                MyDA.SelectCommand = oCom

                For Each oParam As Data.SqlClient.SqlParameter In AllParameters
                    oCom.Parameters.Add(oParam)
                Next

                Dim oDs As New DataSet
                MyDA.Fill(oDs)
                Return oDs
            Catch ex As Exception
                Return Nothing
            End Try
        End Function


        Public Class VoteResult
            Public Property ok As Boolean
            Public Property likes As Integer
            Public Property dislikes As Integer
            Public Property votes As Integer
            Public Property [error] As String
        End Class

        <WebMethod()>
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
        Public Function DislikeStory(storyId As String) As VoteResult
            Try

                Dim id As Guid = Guid.Parse(storyId)


                Dim MyParam As New Collection
                MyParam.Add(New Data.SqlClient.SqlParameter("@StoryGUID", id))
                MyParam.Add(New Data.SqlClient.SqlParameter("@Direction", -1))
                MyParam.Add(New Data.SqlClient.SqlParameter("@VoterId", "le voter"))

                Dim Myds As DataSet = ExecuteSQLds("s0003Vote", MyParam)


                Return New VoteResult With {
                    .ok = True,
                    .likes = Myds.Tables(0)(0)("likes"),
                    .dislikes = Myds.Tables(0)(0)("dislikes"),
                    .votes = Myds.Tables(0)(0)("votes")
                }

            Catch ex As Exception
                Return New VoteResult With {
                    .ok = False,
                    .error = ex.Message
                }
            End Try
        End Function

        <WebMethod()>
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
        Public Function likeStory(storyId As String) As VoteResult
            Try

                Dim id As Guid = Guid.Parse(storyId)

                Dim MyParam As New Collection
                MyParam.Add(New Data.SqlClient.SqlParameter("@StoryGUID", id))
                MyParam.Add(New Data.SqlClient.SqlParameter("@Direction", 1))
                MyParam.Add(New Data.SqlClient.SqlParameter("@VoterId", "le voter"))

                Dim Myds As DataSet = ExecuteSQLds("s0003Vote", MyParam)


                Return New VoteResult With {
                    .ok = True,
                    .likes = Myds.Tables(0)(0)("likes"),
                    .dislikes = Myds.Tables(0)(0)("dislikes"),
                    .votes = Myds.Tables(0)(0)("votes")
                }

            Catch ex As Exception
                Return New VoteResult With {
                    .ok = False,
                    .error = ex.Message
                }
            End Try
        End Function
    End Class
End Namespace