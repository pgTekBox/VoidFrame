Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Mail
Imports System.Web.Script.Services
Imports System.Web.Services

Namespace StoryHub

    Partial Public Class Accueil
        Inherits clsData

        Private Const SESSION_STORIES As String = "STORIES"
        Private Const SESSION_SORT As String = "SORT" ' "recent" | "popular"

        <Serializable>
        Private Class Comment
            Public Property Id As Guid
            Public Property StoryId As Guid
            Public Property Name As String
            Public Property Email As String
            Public Property Text As String
            Public Property CreatedAt As DateTime
        End Class

        <Serializable>
        Private Class Story
            Public Property Id As Guid
            Public Property Title As String
            Public Property Category As String
            Public Property Text As String
            Public Property Author As String
            Public Property VideoUrl As String
            Public Property Votes As Integer
            Public Property Likes As Integer
            Public Property Dislikes As Integer
            Public Property CreatedAt As DateTime
            Public Property Comments As List(Of Comment)
        End Class

        Private Class CommentVm
            Public Property Name As String
            Public Property Text As String
            Public Property RelativeTime As String
        End Class

        Private Class StoryVm
            Public Property Id As Guid
            Public Property Title As String
            Public Property Category As String
            Public Property Text As String
            Public Property Author As String
            Public Property VideoUrl As String
            Public Property Votes As Integer
            Public Property Likes As Integer
            Public Property Dislikes As Integer
            Public Property RelativeTime As String
            Public Property PillClass As String
            Public Property Comments As List(Of CommentVm)
        End Class

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not IsPostBack Then
                EnsureSeed()

                ddlFilterCategory.SelectedValue = ""

                Session(SESSION_SORT) = "recent"
                SetSortButtons(isRecent:=True)
                BindCategories()

                BindStories()
            End If
        End Sub


        Sub BindCategories()
            SetDDL(ddlCategory, "Name", "Id", "s0007GetCategories")
            SetDDL(ddlFilterCategory, "Name", "Id", "s0008GetFilterCategories")

        End Sub


        ' -----------------------------
        ' Seed / Storage (Session)
        ' -----------------------------
        Private Sub EnsureSeed()
            Dim stories = TryCast(Session(SESSION_STORIES), List(Of Story))
            If stories IsNot Nothing Then Return

            stories = New List(Of Story) From {
                New Story With {
                    .Id = Guid.NewGuid(),
                    .Title = "Le voyage intemporel",
                    .Category = "Science-Fiction",
                    .Text = "Une porte s'ouvre sur une époque oubliée...",
                    .Author = "@AlexWriter",
                    .VideoUrl = Nothing,
                    .Votes = 342,
                    .Likes = 4,
                    .Dislikes = 8,
                    .CreatedAt = DateTime.UtcNow.AddHours(-10),
                    .Comments = New List(Of Comment) From {
                        New Comment With {.Id = Guid.NewGuid(), .Name = "Marie", .Email = "marie@example.com", .Text = "Vraiment captivant!", .CreatedAt = DateTime.UtcNow.AddHours(-2)}
                    }
                },
                New Story With {
                    .Id = Guid.NewGuid(),
                    .Title = "Les secrets de la forêt",
                    .Category = "Aventure",
                    .Text = "La brume cache des murmures anciens...",
                    .Author = "@Marie_L",
                    .VideoUrl = Nothing,
                    .Votes = 298,
                    .Likes = 1,
                    .Dislikes = 0,
                    .CreatedAt = DateTime.UtcNow.AddDays(-1),
                    .Comments = New List(Of Comment)()
                },
                New Story With {
                    .Id = Guid.NewGuid(),
                    .Title = "Au-delà des étoiles",
                    .Category = "Fantasy",
                    .Text = "Un royaume suspendu entre deux constellations...",
                    .Author = "@Cosmos99",
                    .VideoUrl = Nothing,
                    .Votes = 276,
                    .Likes = 2,
                    .Dislikes = 1,
                    .CreatedAt = DateTime.UtcNow.AddDays(-3),
                    .Comments = New List(Of Comment)()
                }
            }

            Session(SESSION_STORIES) = stories
        End Sub

        Private Function GetStories() As List(Of Story)
            EnsureSeed()
            Return DirectCast(Session(SESSION_STORIES), List(Of Story))
        End Function

        ' -----------------------------
        ' UI Events
        ' -----------------------------
        Protected Sub btnPublish_Click(sender As Object, e As EventArgs)
            lblMsg.CssClass = "msg"
            lblMsg.Text = ""

            Dim title = tbTitle.Text
            Dim category = ddlCategory.SelectedValue
            Dim text = tbStory.Text
            Dim email = tbUserName.Text

            If title = "" OrElse text = "" Then
                lblMsg.CssClass = "msg err"
                lblMsg.Text = "Title and story are required."
                Return
            End If

            Dim MyParam As New Collection
            MyParam.Add(New Data.SqlClient.SqlParameter("@Title", title))
            MyParam.Add(New Data.SqlClient.SqlParameter("@category", category))
            MyParam.Add(New Data.SqlClient.SqlParameter("@text", text))
            MyParam.Add(New Data.SqlClient.SqlParameter("@email", email))


            Dim Myds As DataSet = ExecuteSQLds("s0006InsertStory", MyParam)




            'Dim stories = GetStories()

            'Dim s As New Story With {
            '    .Id = Guid.NewGuid(),
            '    .Title = title,
            '    .Category = category,
            '    .Text = text,
            '    .Author = "@Vous",
            '    .VideoUrl = If(video = "", Nothing, video),
            '    .Votes = 0,
            '    .Likes = 0,
            '    .Dislikes = 0,
            '    .CreatedAt = DateTime.UtcNow,
            '    .Comments = New List(Of Comment)()
            '}

            'stories.Insert(0, s)
            'Session(SESSION_STORIES) = stories

            tbTitle.Text = ""
            tbStory.Text = ""


            lblMsg.CssClass = "msg ok"
            lblMsg.Text = "Published story ✅"

            BindStories()
        End Sub

        Protected Sub ddlFilterCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
            BindStories()
        End Sub

        Protected Sub btnSortRecent_Click(sender As Object, e As EventArgs)
            Session(SESSION_SORT) = "recent"
            SetSortButtons(isRecent:=True)
            BindStories()
        End Sub

        Protected Sub btnSortPopular_Click(sender As Object, e As EventArgs)
            Session(SESSION_SORT) = "popular"
            SetSortButtons(isRecent:=False)
            BindStories()
        End Sub

        Private Sub SetSortButtons(isRecent As Boolean)
            btnSortRecent.CssClass = If(isRecent, "segbtn active", "segbtn")
            btnSortPopular.CssClass = If(isRecent, "segbtn", "segbtn active")
        End Sub

        Protected Sub btnNavRank_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Classement.aspx")
        End Sub

        ' Un seul handler pour Vote + AddComment
        Protected Sub Story_Command(sender As Object, e As CommandEventArgs)
            If e Is Nothing Then Return

            Dim storyId As Guid
            If Not Guid.TryParse(Convert.ToString(e.CommandArgument), storyId) Then Return

            Dim stories = GetStories()
            Dim s = stories.FirstOrDefault(Function(x) x.Id = storyId)
            If s Is Nothing Then Return

            Dim cmd = (e.CommandName Or "")

            Select Case cmd
                Case "vote"
                    s.Votes += 1

                Case "add_comment"
                    AddCommentFromRepeaterItem(sender, s)
            End Select

            Session(SESSION_STORIES) = stories
            BindStories()
        End Sub

        Private Sub AddCommentFromRepeaterItem(sender As Object, s As Story)
            Dim btn = TryCast(sender, Control)
            Dim item = If(btn Is Nothing, Nothing, TryCast(btn.NamingContainer, RepeaterItem))
            If item Is Nothing Then Return

            Dim tbName = TryCast(item.FindControl("tbCmtName"), TextBox)
            Dim tbEmail = TryCast(item.FindControl("tbCmtEmail"), TextBox)
            Dim tbText = TryCast(item.FindControl("tbCmtText"), TextBox)
            Dim lbl = TryCast(item.FindControl("lblCmtMsg"), Label)

            Dim name = ((tbName?.Text) Or "")
            Dim email = ((tbEmail?.Text) Or "")
            Dim text = ((tbText?.Text) Or "")

            If lbl IsNot Nothing Then
                lbl.CssClass = "msg"
                lbl.Text = ""
            End If

            If text = "" Then
                If lbl IsNot Nothing Then
                    lbl.CssClass = "msg err"
                    lbl.Text = "A comment is required."
                End If
                Return
            End If

            'If Not IsValidEmail(email) Then
            '    If lbl IsNot Nothing Then
            '        lbl.CssClass = "msg err"
            '        lbl.Text = "Courriel invalide."
            '    End If
            '    Return
            'End If

            If s.Comments Is Nothing Then s.Comments = New List(Of Comment)

            s.Comments.Add(New Comment With {
                .Id = Guid.NewGuid(),
                .StoryId = s.Id,
                .Name = name,
                .Email = email,
                .Text = text,
                .CreatedAt = DateTime.UtcNow
            })

            ' Clear commentaire (garder nom/email pour éviter retaper)
            If tbText IsNot Nothing Then tbText.Text = ""

            If lbl IsNot Nothing Then
                lbl.CssClass = "msg ok"
                lbl.Text = "Comment added ✅"
            End If
        End Sub

        ' -----------------------------
        ' Binding
        ' -----------------------------
        Private Sub BindStories()

            Dim recent As Integer
            recent = 0
            If Session(SESSION_SORT) = "recent" Then
                recent = 1
            End If

            If Session(SESSION_SORT) = "popular" Then
                recent = 2
            End If


            Dim MyParam As New Collection
            MyParam.Add(New Data.SqlClient.SqlParameter("@FilterCategory", ddlFilterCategory.SelectedValue))
            MyParam.Add(New Data.SqlClient.SqlParameter("@recent", recent))



            Dim dsStories As DataSet = Me.ExecuteSQLds("s0001GetStories", MyParam)

            'Dim stories = GetStories().AsEnumerable()

            'Dim filter As String = ddlFilterCategory.SelectedValue
            'If filter <> "" Then
            '    stories = stories.Where(Function(s) String.Equals(s.Category, filter, StringComparison.OrdinalIgnoreCase))
            'End If

            'Dim sort = Convert.ToString(Session(SESSION_SORT))
            'If String.Equals(sort, "popular", StringComparison.OrdinalIgnoreCase) Then
            '    stories = stories.OrderByDescending(Function(s) s.Votes).ThenByDescending(Function(s) s.CreatedAt)
            'Else
            '    stories = stories.OrderByDescending(Function(s) s.CreatedAt)
            'End If

            'Dim nowUtc = DateTime.UtcNow

            'Dim vms = stories.Select(Function(st) New StoryVm With {
            '    .Id = st.Id,
            '    .Title = Server.HtmlEncode(st.Title),
            '    .Category = Server.HtmlEncode(st.Category),
            '    .Text = Server.HtmlEncode(st.Text),
            '    .Author = Server.HtmlEncode(st.Author),
            '    .VideoUrl = st.VideoUrl,
            '    .Votes = st.Votes,
            '    .Likes = st.Likes,
            '    .Dislikes = st.Dislikes,
            '    .RelativeTime = ToRelativeTime(st.CreatedAt, nowUtc),
            '    .PillClass = CategoryToPillClass(st.Category),
            '    .Comments = (If(st.Comments, New List(Of Comment))).
            '        OrderByDescending(Function(c) c.CreatedAt).
            '        Select(Function(c) New CommentVm With {
            '            .Name = Server.HtmlEncode(c.Name),
            '            .Text = Server.HtmlEncode(c.Text),
            '            .RelativeTime = ToRelativeTime(c.CreatedAt, nowUtc)
            '        }).ToList()
            '}).ToList()

            '  rptStories.DataSource = vms
            rptStories.DataSource = dsStories.Tables(0)

            rptStories.DataBind()
        End Sub
        Private Sub rptStories_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptStories.ItemDataBound




            If e.Item.ItemType <> ListItemType.Item AndAlso e.Item.ItemType <> ListItemType.AlternatingItem Then Exit Sub

            ' 1) Récupère la ligne parent courante
            Dim drv As DataRowView = TryCast(e.Item.DataItem, DataRowView)
            If drv Is Nothing Then Exit Sub

            Dim StoryGUID As Guid = drv("StoryGUID")

            ' 2) Trouve le repeater enfant dans le template
            Dim MyrptComments As Repeater = TryCast(e.Item.FindControl("rptComments"), Repeater)
            If MyrptComments Is Nothing Then Exit Sub

            ' 3) Donne au child un DataTable filtré (ou un DataView)

            Dim MyParam As New Collection
            MyParam.Add(New Data.SqlClient.SqlParameter("@StoryGUID", StoryGUID))
            Dim Myds As DataSet = ExecuteSQLds("s0002GetStoryComments", MyParam)


            If Myds Is Nothing Then Exit Sub

            MyrptComments.DataSource = Myds.Tables(0)
            MyrptComments.DataBind()


        End Sub
        ' -----------------------------
        ' Helpers
        ' -----------------------------
        Private Function CategoryToPillClass(cat As String) As String
            Dim c As String = cat
            If c.Contains("aventure") OrElse c.Contains("science") Then Return "blue"
            If c.Contains("romance") OrElse c.Contains("comédie") Then Return "pink"
            Return "purple"
        End Function

        Private Function ToRelativeTime(createdUtc As DateTime, nowUtc As DateTime) As String
            Dim span = nowUtc - createdUtc
            If span.TotalSeconds < 60 Then Return "à l’instant"
            If span.TotalMinutes < 60 Then Return $"{CInt(Math.Floor(span.TotalMinutes))} min"
            If span.TotalHours < 24 Then Return $"{CInt(Math.Floor(span.TotalHours))} h"
            If span.TotalDays < 7 Then Return $"{CInt(Math.Floor(span.TotalDays))} j"
            Return createdUtc.ToLocalTime().ToString("yyyy-MM-dd")
        End Function

        Private Function IsValidEmail(email As String) As Boolean
            Try
                Dim addr As New MailAddress(email)
                Return addr.Address.Equals(email, StringComparison.OrdinalIgnoreCase)
            Catch
                Return False
            End Try
        End Function
        Protected Sub btnModalAddComment_Click(sender As Object, e As EventArgs)
            'lblModalMsg.CssClass = "msg"
            'lblModalMsg.Text = ""

            'Dim storyId As Guid
            'If Not Guid.TryParse(If(hfCommentStoryId.Value, ""), storyId) Then
            '    lblModalMsg.CssClass = "msg err"
            '    lblModalMsg.Text = "Story invalide."
            '    ReopenModalClientSide()
            '    Return
            'End If

            'Dim name = tbModalName.Text
            'Dim email = tbModalEmail.Text
            'Dim text = tbModalText.Text

            ''If name = "" OrElse email = "" OrElse text = "" Then
            'If text = "" Then
            '    lblModalMsg.CssClass = "msg err"
            '    lblModalMsg.Text = "Le commentaire est obligatoires."
            '    ReopenModalClientSide()
            '    Return
            'End If

            ''If Not IsValidEmail(email) Then
            ''    lblModalMsg.CssClass = "msg err"
            ''    lblModalMsg.Text = "Courriel invalide."
            ''    ReopenModalClientSide()
            ''    Return
            ''End If


            'Dim MyParam As New Collection
            'MyParam.Add(New Data.SqlClient.SqlParameter("@StoryGUID", storyId))

            'MyParam.Add(New Data.SqlClient.SqlParameter("@Email", email))
            'MyParam.Add(New Data.SqlClient.SqlParameter("@Text", text))


            'Dim Myds As DataSet = ExecuteSQLds("s0004InsertComment", MyParam)


            ''Dim stories = GetStories()
            ''Dim s = stories.FirstOrDefault(Function(x) x.Id = storyId)
            ''If s Is Nothing Then
            ''    lblModalMsg.CssClass = "msg err"
            ''    lblModalMsg.Text = "Story introuvable."
            ''    ReopenModalClientSide()
            ''    Return
            ''End If

            ''If s.Comments Is Nothing Then s.Comments = New List(Of Comment)

            ''        s.Comments.Add(New Comment With {
            ''    .Id = Guid.NewGuid(),
            ''    .StoryId = s.Id,
            ''    .Name = name,
            ''    .Email = email,
            ''    .Text = text,
            ''    .CreatedAt = DateTime.UtcNow
            ''})

            ''        Session(SESSION_STORIES) = stories

            '' Clear
            'tbModalText.Text = ""
            'tbModalName.Text = ""
            'tbModalEmail.Text = ""

            'BindStories()

            '' Fermer le modal côté client après succès
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeCmtModal", "closeCommentModal();", True)
        End Sub

        Private Sub ReopenModalClientSide()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openCmtModal", "document.getElementById('commentModal').classList.add('show'); document.getElementById('commentModal').setAttribute('aria-hidden','false');", True)
        End Sub



        'Public Class VoteResult
        '    Public Property ok As Boolean
        '    Public Property likes As Integer
        '    Public Property dislikes As Integer
        '    Public Property [error] As String
        'End Class


        '<WebMethod()>
        '<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
        'Public Shared Function test() As Object

        '    Return New With {.ok = False, .error = "GUID invalide"}

        'End Function

        '<WebMethod()>
        '<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
        'Public Shared Function LikeStory(storyId As String) As VoteResult
        '    Try
        '        Dim id As Guid = Guid.Parse(storyId)

        '        ' TODO: Update DB (increment like) and return real values
        '        Dim newLikes As Integer = 1
        '        Dim newDislikes As Integer = 0

        '        Return New VoteResult With {.ok = True, .likes = newLikes, .dislikes = newDislikes}
        '    Catch ex As Exception
        '        Return New VoteResult With {.ok = False, .error = ex.Message}
        '    End Try
        'End Function

        '<WebMethod()>
        '<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
        'Public Shared Function DislikeStory(storyId As String) As VoteResult
        '    Try
        '        Dim id As Guid = Guid.Parse(storyId)

        '        Dim newLikes As Integer = 0
        '        Dim newDislikes As Integer = 1

        '        Return New VoteResult With {.ok = True, .likes = newLikes, .dislikes = newDislikes}
        '    Catch ex As Exception
        '        Return New VoteResult With {.ok = False, .error = ex.Message}
        '    End Try
        'End Function

        Protected Sub Comment_Command(sender As Object, e As CommandEventArgs)
            If e.CommandName <> "addComment" Then Return

            Dim storyId As Guid
            If Not Guid.TryParse(CStr(e.CommandArgument), storyId) Then Return

            Dim btn = TryCast(sender, Control)
            If btn Is Nothing Then Return

            Dim item = TryCast(btn.NamingContainer, RepeaterItem)
            If item Is Nothing Then Return

            Dim tbEmail = TryCast(item.FindControl("tbCmtEmail"), TextBox)
            Dim tbName = TryCast(item.FindControl("tbCmtName"), TextBox)
            Dim tbText = TryCast(item.FindControl("tbCmtText"), TextBox)
            Dim lbl = TryCast(item.FindControl("lblCmtMsg"), Label)

            Dim email As String = If(tbEmail?.Text, "").Trim()
            Dim name As String = If(tbName?.Text, "").Trim()
            Dim text As String = If(tbText?.Text, "").Trim()

            If lbl IsNot Nothing Then
                lbl.CssClass = "msg err"
                lbl.Text = ""
            End If

            If text = "" Then
                If lbl IsNot Nothing Then lbl.Text = "A comment is required."
                Return
            End If

            'If name = "" Then name = "Anonyme"

            Try

                Dim MyParam As New Collection
                MyParam.Add(New Data.SqlClient.SqlParameter("@StoryGUID", storyId))

                MyParam.Add(New Data.SqlClient.SqlParameter("@Email", email))
                MyParam.Add(New Data.SqlClient.SqlParameter("@Text", text))


                Dim Myds As DataSet = ExecuteSQLds("s0004InsertComment", MyParam)

                ' TODO: Insert SQL (storyId, name, email, text)
                ' InsertComment(storyId, name, email, text)

                ' Recharger la liste (stories + comments)
                BindStories() ' ta méthode habituelle qui fait rptStories.DataBind()

                ' Optionnel: garder un petit message global
                ' lblMsg.CssClass = "msg ok"
                ' lblMsg.Text = "Commentaire ajouté."
            Catch ex As Exception
                If lbl IsNot Nothing Then
                    lbl.CssClass = "msg err"
                    lbl.Text = ex.Message
                End If
            End Try
        End Sub





    End Class

End Namespace
