Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace StoryHub

    Partial Public Class Classement2
        Inherits System.Web.UI.Page

        Private Const SESSION_WINNERS As String = "SH_WINNERS"

        <Serializable>
        Private Class WinnerStory
            Public Property Id As Guid
            Public Property Period As String          ' WEEK / MONTH / ALL
            Public Property TagText As String         ' "Gagnant de la semaine"...
            Public Property TagClass As String        ' tag-yellow / tag-blue / tag-green
            Public Property BorderClass As String     ' border-yellow / border-blue / border-green
            Public Property StarColor As String       ' CSS color for star
            Public Property VoteBoxClass As String    ' vote-yellow / vote-blue / vote-green
            Public Property VoteNumClass As String    ' yellow / blue / green
            Public Property Category As String
            Public Property PillClass As String       ' pill-blue / pill-purple
            Public Property Author As String
            Public Property RelativeTime As String
            Public Property Title As String
            Public Property Text As String
            Public Property Votes As Integer
            Public Property Likes As Integer
            Public Property Comments As Integer

            Public ReadOnly Property LikesLabel As String
                Get
                    Return Likes.ToString() & " J'aime"
                End Get
            End Property

            Public ReadOnly Property CommentsLabel As String
                Get
                    Return Comments.ToString() & " Commentaires"
                End Get
            End Property
        End Class

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not IsPostBack Then
                EnsureSeedData()
                BindAll()
            End If
        End Sub

        Private Sub EnsureSeedData()
            If Session(SESSION_WINNERS) IsNot Nothing Then Return

            Dim list As New List(Of WinnerStory) From {
                New WinnerStory With {
                    .Id = Guid.NewGuid(),
                    .Period = "WEEK",
                    .TagText = "Gagnant de la semaine",
                    .TagClass = "tag-yellow",
                    .BorderClass = "border-yellow",
                    .StarColor = "#eab308",
                    .VoteBoxClass = "vote-yellow",
                    .VoteNumClass = "yellow",
                    .Category = "Science-Fiction",
                    .PillClass = "pill-blue",
                    .Author = "@AlexWriter",
                    .RelativeTime = "il y a 4 jours",
                    .Title = "Le voyage intemporel",
                    .Text = "Un scientifique découvre un moyen de voyager dans le temps, mais chaque saut crée des paradoxes...",
                    .Votes = 342,
                    .Likes = 156,
                    .Comments = 89
                },
                New WinnerStory With {
                    .Id = Guid.NewGuid(),
                    .Period = "MONTH",
                    .TagText = "Gagnant du mois",
                    .TagClass = "tag-blue",
                    .BorderClass = "border-blue",
                    .StarColor = "#3b82f6",
                    .VoteBoxClass = "vote-blue",
                    .VoteNumClass = "blue",
                    .Category = "Mystère",
                    .PillClass = "pill-purple",
                    .Author = "@Marie_L",
                    .RelativeTime = "il y a 2 semaines",
                    .Title = "Les secrets de la forêt",
                    .Text = "Au cœur d'une forêt millénaire, des événements étranges se produisent...",
                    .Votes = 512,
                    .Likes = 234,
                    .Comments = 167
                },
                New WinnerStory With {
                    .Id = Guid.NewGuid(),
                    .Period = "ALL",
                    .TagText = "Gagnant de tous les temps",
                    .TagClass = "tag-green",
                    .BorderClass = "border-green",
                    .StarColor = "#16a34a",
                    .VoteBoxClass = "vote-green",
                    .VoteNumClass = "green",
                    .Category = "Fantasy",
                    .PillClass = "pill-purple",
                    .Author = "@Cosmos99",
                    .RelativeTime = "il y a 3 mois",
                    .Title = "Au-delà des étoiles",
                    .Text = "Dans un univers où la magie et la technologie coexistent, une prophétie pourrait tout changer...",
                    .Votes = 1247,
                    .Likes = 892,
                    .Comments = 456
                }
            }

            Session(SESSION_WINNERS) = list
        End Sub

        Private Function GetStories() As List(Of WinnerStory)
            Dim list = TryCast(Session(SESSION_WINNERS), List(Of WinnerStory))
            If list Is Nothing Then
                list = New List(Of WinnerStory)()
                Session(SESSION_WINNERS) = list
            End If
            Return list
        End Function

        Private Sub BindAll()
            Dim stories = GetStories()

            ' Périodes
            Dim week = stories.Where(Function(s) s.Period = "WEEK").OrderByDescending(Function(s) s.Votes).FirstOrDefault()
            Dim month = stories.Where(Function(s) s.Period = "MONTH").OrderByDescending(Function(s) s.Votes).FirstOrDefault()
            Dim allp = stories.Where(Function(s) s.Period = "ALL").OrderByDescending(Function(s) s.Votes).FirstOrDefault()

            lblWeekVotes.Text = If(week Is Nothing, "0 votes", week.Votes.ToString() & " votes")
            lblWeekWinner.Text = If(week Is Nothing, "—", week.Author)

            lblMonthVotes.Text = If(month Is Nothing, "0 votes", month.Votes.ToString() & " votes")
            lblMonthWinner.Text = If(month Is Nothing, "—", month.Author)

            lblAllVotes.Text = If(allp Is Nothing, "0 votes", allp.Votes.ToString() & " votes")
            lblAllWinner.Text = If(allp Is Nothing, "—", allp.Author)

            ' Top 3 (global sur tous les enregistrements disponibles ici)
            Dim top3 = stories.OrderByDescending(Function(s) s.Votes).Take(3).ToList()
            RenderTopPanels(top3)

            ' Liste gagnants
            rptWinners.DataSource = stories.OrderByDescending(Function(s) s.Votes).ToList()
            rptWinners.DataBind()
        End Sub

        Private Sub RenderTopPanels(top3 As List(Of WinnerStory))
            pnlTop1.Controls.Clear()
            pnlTop2.Controls.Clear()
            pnlTop3.Controls.Clear()

            Dim cards = New List(Of Panel) From {pnlTop1, pnlTop2, pnlTop3}

            For i As Integer = 0 To 2
                Dim s As WinnerStory = If(i < top3.Count, top3(i), Nothing)

                Dim badgeHtml As String
                Dim votesCls As String
                Select Case i
                    Case 0
                        badgeHtml = "<span class='top-badge'>1er</span>"
                        votesCls = "votes-amber"
                    Case 1
                        badgeHtml = "<span class='badge-2'>2ème</span>"
                        votesCls = "votes-gray"
                    Case Else
                        badgeHtml = "<span class='badge-3'>3ème</span>"
                        votesCls = "votes-orange"
                End Select

                Dim title As String = If(s Is Nothing, "—", Server.HtmlEncode(s.Title))
                Dim author As String = If(s Is Nothing, "—", Server.HtmlEncode(s.Author))
                Dim votes As String = If(s Is Nothing, "0 votes", s.Votes.ToString() & " votes")

                Dim html As String =
                    "<div class='mini-row'>" &
                        badgeHtml &
                        "<span class='" & votesCls & "'>" & votes & "</span>" &
                    "</div>" &
                    "<div style='font-weight:900; margin-bottom:6px; color:var(--gray-900);'>" & title & "</div>" &
                    "<div style='font-size:13px; color:var(--gray-600);'>par " & author & "</div>"

                cards(i).Controls.Add(New LiteralControl(html))
            Next
        End Sub

        ' Vote +1 (Repeater LinkButton OnCommand)
        Protected Sub Winner_Command(sender As Object, e As CommandEventArgs)
            If e Is Nothing OrElse String.IsNullOrWhiteSpace(Convert.ToString(e.CommandArgument)) Then Return

            Dim id As Guid
            If Not Guid.TryParse(Convert.ToString(e.CommandArgument), id) Then Return

            Dim stories = GetStories()
            Dim st = stories.FirstOrDefault(Function(x) x.Id = id)
            If st Is Nothing Then Return

            st.Votes += 1
            BindAll()
        End Sub

        ' Navigation
        Protected Sub btnNavHome_Click(sender As Object, e As EventArgs)
            Response.Redirect("Default.aspx")
        End Sub

        Protected Sub btnNavProfile_Click(sender As Object, e As EventArgs)
            Response.Redirect("Profil.aspx")
        End Sub

        Protected Sub btnNavLogin_Click(sender As Object, e As EventArgs)
            Response.Redirect("Login.aspx")
        End Sub

    End Class

End Namespace
