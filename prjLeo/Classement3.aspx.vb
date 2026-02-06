Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace StoryHub

    Partial Public Class Classement3
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
                hfTopPeriod.Value = "WEEK"
                SetTopButtonsActive("WEEK")
                LoadTop3("WEEK")
                EnsureSeedData()





                BindAll()


                ' Le reste de ta page (winners, etc.)
                ' LoadWinners()
            Else
                ' garde l'état visuel au postback
                SetTopButtonsActive(hfTopPeriod.Value)
            End If

        End Sub
        Protected Sub btnTopWeek_Click(sender As Object, e As EventArgs)
            hfTopPeriod.Value = "WEEK"
            SetTopButtonsActive("WEEK")
            LoadTop3("WEEK")
        End Sub

        Protected Sub btnTopMonth_Click(sender As Object, e As EventArgs)
            hfTopPeriod.Value = "MONTH"
            SetTopButtonsActive("MONTH")
            LoadTop3("MONTH")
        End Sub

        Protected Sub btnTopYear_Click(sender As Object, e As EventArgs)
            hfTopPeriod.Value = "YEAR"
            SetTopButtonsActive("YEAR")
            LoadTop3("YEAR")
        End Sub


        'Private Function GetTopPeriod() As String
        '    Dim v As String = ""
        '    If ddlTopPeriod IsNot Nothing AndAlso ddlTopPeriod.SelectedValue IsNot Nothing Then
        '        v = ddlTopPeriod.SelectedValue.Trim().ToUpperInvariant()
        '    End If
        '    If v <> "WEEK" AndAlso v <> "MONTH" AndAlso v <> "YEAR" Then v = "WEEK"
        '    Return v
        'End Function

        Private Sub LoadTop3(period As String)
            Dim startDate As DateTime
            Dim endDate As DateTime
            GetPeriodRange(period, startDate, endDate)

            ' IMPORTANT:
            ' - Ici, on calcule le Top 3 par nombre de votes dans la période.
            ' - Adapte les noms: Stories, StoryVotes, VoteDate, etc.
            Dim dt As DataTable = GetTop3FromDb(startDate, endDate)

            RenderTop3Panels(dt, period, startDate, endDate)
        End Sub

        Private Sub RenderTop3Panels(dt As DataTable, period As String, startDate As DateTime, endDate As DateTime)
            ' Sécurité si les panels n'existent pas (selon ton ASPX)
            If pnlTop1 Is Nothing OrElse pnlTop2 Is Nothing OrElse pnlTop3 Is Nothing Then Return

            pnlTop1.Controls.Clear()
            pnlTop2.Controls.Clear()
            pnlTop3.Controls.Clear()

            ' Si pas de data
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                pnlTop1.Controls.Add(New LiteralControl(BuildTopCardHtml(1, "", "", "", 0, True)))
                pnlTop2.Controls.Add(New LiteralControl(BuildTopCardHtml(2, "", "", "", 0, True)))
                pnlTop3.Controls.Add(New LiteralControl(BuildTopCardHtml(3, "", "", "", 0, True)))
                Return
            End If

            ' Remplit jusqu'à 3 cartes
            For i As Integer = 0 To 2
                Dim title As String = ""
                Dim author As String = ""
                Dim category As String = ""
                Dim votes As Integer = 0

                If i < dt.Rows.Count Then
                    title = Convert.ToString(dt.Rows(i)("Title"))
                    author = Convert.ToString(dt.Rows(i)("Author"))
                    category = Convert.ToString(dt.Rows(i)("Category"))
                    votes = Convert.ToInt32(dt.Rows(i)("Votes"))
                End If

                Dim html As String = BuildTopCardHtml(i + 1, title, author, category, votes, False)

                Select Case i
                    Case 0 : pnlTop1.Controls.Add(New LiteralControl(html))
                    Case 1 : pnlTop2.Controls.Add(New LiteralControl(html))
                    Case 2 : pnlTop3.Controls.Add(New LiteralControl(html))
                End Select
            Next
        End Sub

        Private Function BuildTopCardHtml(rank As Integer, title As String, author As String, category As String, votes As Integer, empty As Boolean) As String
            Dim badge As String
            Dim votesClass As String

            Select Case rank
                Case 1
                    badge = "<span class='top-badge'>#1</span>"
                    votesClass = "votes-amber"
                Case 2
                    badge = "<span class='badge-2'>#2</span>"
                    votesClass = "votes-gray"
                Case Else
                    badge = "<span class='badge-3'>#3</span>"
                    votesClass = "votes-orange"
            End Select

            If empty OrElse String.IsNullOrWhiteSpace(title) Then
                title = "Aucune histoire"
                author = "—"
                category = "—"
                votes = 0
            End If

            Dim sb As New StringBuilder()
            sb.Append("<div class='mini-row'>")
            sb.Append(badge)
            sb.Append("<span class='" & votesClass & "'>" & votes.ToString() & " votes</span>")
            sb.Append("</div>")

            sb.Append("<div style='font-weight:900; font-size:16px; margin-bottom:6px;'>")
            sb.Append(Server.HtmlEncode(title))
            sb.Append("</div>")

            sb.Append("<div style='color: var(--gray-600); font-size:13px; margin-bottom:6px;'>")
            sb.Append("par <b>" & Server.HtmlEncode(author) & "</b>")
            sb.Append("</div>")

            sb.Append("<div style='display:flex; gap:8px; flex-wrap:wrap;'>")
            sb.Append("<span class='pill purple'>" & Server.HtmlEncode(category) & "</span>")
            sb.Append("</div>")

            Return sb.ToString()
        End Function


        Private Function GetTop3FromDb(startDate As DateTime, endDate As DateTime) As DataTable
            '            Dim sql As String =
            '"SELECT TOP 3
            '    s.Id,
            '    s.Title,
            '    s.AuthorName AS Author,
            '    s.Category,
            '    COUNT(v.Id) AS Votes
            'FROM Stories s
            'LEFT JOIN StoryVotes v
            '    ON v.StoryId = s.Id
            '    AND v.VoteDate >= @startDate
            '    AND v.VoteDate <  @endDate
            'GROUP BY s.Id, s.Title, s.AuthorName, s.Category
            'ORDER BY COUNT(v.Id) DESC, s.Title ASC;"

            '            Dim dt As New DataTable()

            '            Using cn As New SqlConnection(ConnStr)
            '                Using cmd As New SqlCommand(sql, cn)
            '                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate
            '                    cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate

            '                    Using da As New SqlDataAdapter(cmd)
            '                        da.Fill(dt)
            '                    End Using
            '                End Using
            '            End Using

            'Return dt
            Return Nothing
        End Function
        Private Sub GetPeriodRange(period As String, ByRef startDate As DateTime, ByRef endDate As DateTime)
            Dim now As DateTime = DateTime.Now

            ' endDate = borne exclusive (plus simple pour SQL)
            Select Case period
                Case "WEEK"
                    ' Semaine ISO/locale (lundi -> dimanche)
                    Dim dayOfWeek As Integer = CInt(now.DayOfWeek) ' Dimanche=0
                    Dim mondayOffset As Integer = If(dayOfWeek = 0, -6, 1 - dayOfWeek)
                    startDate = now.Date.AddDays(mondayOffset)
                    endDate = startDate.AddDays(7)

                Case "MONTH"
                    startDate = New DateTime(now.Year, now.Month, 1)
                    endDate = startDate.AddMonths(1)

                Case "YEAR"
                    startDate = New DateTime(now.Year, 1, 1)
                    endDate = startDate.AddYears(1)

                Case Else
                    startDate = now.Date.AddDays(-7)
                    endDate = now.Date.AddDays(1)
            End Select
        End Sub



        Private Sub SetTopButtonsActive(period As String)
            ' reset
            btnTopWeek.CssClass = "top-pill"
            btnTopMonth.CssClass = "top-pill"
            btnTopYear.CssClass = "top-pill"

            Select Case period
                Case "MONTH"
                    btnTopMonth.CssClass = "top-pill active"
                Case "YEAR"
                    btnTopYear.CssClass = "top-pill active"
                Case Else
                    btnTopWeek.CssClass = "top-pill active"
            End Select
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

            'lblWeekVotes.Text = If(week Is Nothing, "0 votes", week.Votes.ToString() & " votes")
            'lblWeekWinner.Text = If(week Is Nothing, "—", week.Author)

            'lblMonthVotes.Text = If(month Is Nothing, "0 votes", month.Votes.ToString() & " votes")
            'lblMonthWinner.Text = If(month Is Nothing, "—", month.Author)

            'lblAllVotes.Text = If(allp Is Nothing, "0 votes", allp.Votes.ToString() & " votes")
            'lblAllWinner.Text = If(allp Is Nothing, "—", allp.Author)

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
