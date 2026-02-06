<%@ Page Language="VB" AutoEventWireup="true" MaintainScrollPositionOnPostBack="true"
    CodeBehind="Accueil.aspx.vb" Inherits="StoryHub.Accueil" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Home - Story Sharing Platform</title>

    <style>
        :root {
            --font: "Inter", system-ui, -apple-system, Segoe UI, Roboto, Arial, sans-serif;
            --blue-50: #eff6ff;
            --cyan-50: #ecfeff;
            --blue-600: #2563eb;
            --blue-700: #1d4ed8;
            --gray-50: #f9fafb;
            --gray-100: #f3f4f6;
            --gray-200: #e5e7eb;
            --gray-300: #d1d5db;
            --gray-400: #9ca3af;
            --gray-500: #6b7280;
            --gray-600: #4b5563;
            --gray-700: #374151;
            --gray-900: #111827;
            --amber-50: #fffbeb;
            --orange-50: #fff7ed;
            --amber-200: #fde68a;
            --amber-300: #fcd34d;
            --amber-500: #f59e0b;
            --amber-600: #d97706;
            --pink-100: #fce7f3;
            --pink-700: #be185d;
            --purple-100: #ede9fe;
            --purple-700: #6d28d9;
            --orange-200: #fed7aa;
            --orange-400: #fb923c;
            --orange-500: #f97316;
            --orange-600: #ea580c;
            --shadow-sm: 0 1px 2px rgba(0,0,0,.06);
            --shadow-md: 0 10px 15px -3px rgba(0,0,0,.10), 0 4px 6px -4px rgba(0,0,0,.10);
            --shadow-xl: 0 25px 50px -12px rgba(0,0,0,.25);
        }

        html, body { height: 100%; }

        body {
            margin: 0;
            font-family: var(--font);
            background: linear-gradient(135deg, var(--blue-50) 0%, var(--cyan-50) 100%);
            color: var(--gray-900);
        }

        /* Layout */
        .container { max-width: 1280px; margin: 0 auto; padding: 0 16px; }
        .main { padding: 32px 0; }

        /* Header */
        .header {
            background: #fff;
            border-bottom: 1px solid var(--gray-200);
            position: sticky;
            top: 0;
            z-index: 50;
            box-shadow: var(--shadow-sm);
        }

        .header-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 16px 0;
            gap: 16px;
        }

        .brand { display: flex; align-items: center; gap: 12px; }
        .brand h1 { margin: 0; font-size: 24px; font-weight: 800; color: var(--gray-900); }

        .nav { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
        .nav .navbtn {
            border: 0;
            background: transparent;
            padding: 10px 14px;
            border-radius: 10px;
            font-weight: 600;
            color: var(--gray-700);
            cursor: pointer;
        }
        .nav .navbtn:hover { background: var(--gray-100); }
        .nav .navbtn.active { background: #dbeafe; color: var(--gray-700); }

        /* Hero */
        .gradient-bg { background: linear-gradient(135deg, #7c3aed 0%, #9333ea 100%); }

        .hero {
            border-radius: 18px;
            padding: 32px;
            color: #fff;
            box-shadow: var(--shadow-xl);
            margin-bottom: 32px;
        }
        .hero h2 { margin: 0 0 10px 0; font-size: 40px; font-weight: 800; }
        .hero p { margin: 0 0 16px 0; font-size: 18px; color: rgba(255,255,255,.92); }

        .hero-row {
            display: flex;
            align-items: center;
            gap: 12px;
            font-size: 20px;
            font-weight: 700;
        }
        .hero-icon { width: 40px; height: 40px; color: #4ade80; }

        /* Cards */
        .card { background: #fff; border: 1px solid var(--gray-200); border-radius: 14px; box-shadow: var(--shadow-md); }
        .card-pad { padding: 22px; }

        /* Form */
        .form-title { margin: 0 0 14px 0; font-size: 20px; font-weight: 800; }

        .field { margin-bottom: 14px; }

        .label {
            display: block;
            font-size: 13px;
            font-weight: 700;
            color: var(--gray-700);
            margin-bottom: 8px;
        }

        .input, .select, .textarea {
            width: 100%;
            border: 1px solid var(--gray-300);
            border-radius: 10px;
            padding: 10px 12px;
            font-size: 14px;
            outline: none;
            box-sizing: border-box;
            background: #fff;
        }
        .textarea { resize: none; }
        .input:focus, .select:focus, .textarea:focus {
            border-color: transparent;
            box-shadow: 0 0 0 3px rgba(37,99,235,.25);
        }

        .grid-2 { display: grid; grid-template-columns: 1fr; gap: 14px; }
        @media (min-width: 768px) { .grid-2 { grid-template-columns: 1fr 1fr; } }

        .grid-3 { display: grid; grid-template-columns: 1fr; gap: 14px; }
        @media (min-width: 900px) { .grid-3 { grid-template-columns: 1fr 1fr 1fr; } }

        .btn {
            border: 0;
            border-radius: 12px;
            padding: 12px 14px;
            font-weight: 800;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
        }
        .btn.primary { width: 100%; background: #a78bfa; color: #fff; }
        .btn.primary:hover { background: #8b5cf6; }

        .msg { margin-top: 10px; font-size: 13px; color: var(--gray-600); }
        .msg.ok { color: #047857; }
        .msg.err { color: #b91c1c; }

        /* Filter / sort */
        .filterbar {
            display: flex;
            flex-direction: column;
            gap: 12px;
            align-items: flex-start;
            justify-content: space-between;
            margin: 18px 0;
        }
        @media (min-width: 640px) { .filterbar { flex-direction: row; align-items: center; } }

        .filter-left { display: flex; align-items: center; gap: 10px; }
        .filter-icon { width: 20px; height: 20px; color: var(--gray-600); }

        .seg { display: flex; gap: 10px; }
        .seg .segbtn {
            border: 0;
            border-radius: 10px;
            padding: 10px 14px;
            font-weight: 800;
            cursor: pointer;
            background: var(--gray-100);
            color: var(--gray-700);
        }
        .seg .segbtn:hover { background: var(--gray-200); }
        .seg .segbtn.active { background: #a78bfa; color: #fff; }

        /* Stories */
        .stories { display: grid; gap: 18px; }
        .story-card { transition: transform .2s, box-shadow .2s; }
        .story-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1);
        }

        .story-row { display: flex; gap: 16px; align-items: flex-start; }

        .vote-col { display: flex; flex-direction: column; align-items: center; gap: 8px; }

        .btn-vote {
            border: 0;
            border-radius: 12px;
            padding: 12px;
            background: #eff6ff;
            color: var(--blue-600);
            cursor: pointer;
            transition: all .2s;
        }
        .btn-vote:hover { background: #dbeafe; transform: scale(1.05); }

        .vote-num { font-size: 20px; font-weight: 900; color: var(--gray-900); }

        .meta { display: flex; gap: 10px; flex-wrap: wrap; align-items: center; margin-bottom: 8px; }

        .pill {
            font-size: 12px;
            font-weight: 900;
            padding: 6px 10px;
            border-radius: 999px;
            display: inline-block;
        }
        .pill.blue { background: #dbeafe; color: #1d4ed8; }
        .pill.pink { background: var(--pink-100); color: var(--pink-700); }
        .pill.purple { background: var(--purple-100); color: var(--purple-700); }

        .by { color: var(--gray-500); font-size: 13px; }
        .by b { color: var(--gray-700); }
        .time { color: var(--gray-400); font-size: 13px; }

        .story-title { margin: 0 0 10px 0; font-size: 20px; font-weight: 900; }
        .story-text { margin: 0 0 14px 0; color: var(--gray-700); line-height: 1.6; }

        .actions { display: flex; gap: 16px; color: var(--gray-600); font-size: 13px; }

        .action {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            border: 0;
            background: transparent;
            cursor: pointer;
            color: inherit;
            padding: 6px 8px;
            border-radius: 10px;
        }
        .action:hover { color: var(--blue-600); background: #eff6ff; }

        /* Footer */
        .footer {
            margin-top: 64px;
            background: #fff;
            border-top: 1px solid var(--gray-200);
            padding: 28px 0;
            color: var(--gray-600);
            text-align: center;
            font-size: 13px;
        }

        .mt-16 { margin-top: 16px; }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    <script type="text/javascript">
        // IMPORTANT: FriendlyUrls changes the URL; force the real endpoint
        if (typeof PageMethods !== "undefined" && PageMethods.set_path) {
            PageMethods.set_path("<%= ResolveUrl("~/Accueil.aspx") %>");
        }
    </script>

    <!-- Header -->
    <header class="header">
        <div class="container">
            <div class="header-row">
                <div class="brand">
                    <div class="brand flex items-center gap-3">
                        <img src="logo.png" alt="StoryHub logo" style="height: 36px; width: auto;" />
                    </div>
                </div>

                <nav class="nav">
                    <asp:Button ID="btnNavHome" runat="server" CssClass="navbtn active" Text="Home" UseSubmitBehavior="false" />
                    <asp:Button ID="btnNavRank" runat="server" CssClass="navbtn" Text="Leaderboard" OnClick="btnNavRank_Click" UseSubmitBehavior="false" />
                </nav>
            </div>
        </div>
    </header>

    <!-- Main -->
    <main class="container main">

        <!-- Hero -->
        <section class="hero gradient-bg">
            <h2>Share your stories</h2>
            <p>Join a community of passionate writers and share your best stories.</p>
            <div class="hero-row">
                <svg class="hero-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2.5" aria-hidden="true">
                    <path stroke-linecap="round" stroke-linejoin="round"
                        d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z">
                    </path>
                </svg>
                <span>Become an author and win rewards</span>
            </div>
        </section>

        <!-- Story Submission -->
        <section class="card card-pad">
            <h3 class="form-title">Submit your story</h3>

            <asp:Label ID="lblMsg" runat="server" CssClass="msg" EnableViewState="false" />

            <div class="mt-16">
                <div class="grid-3">

                    <div class="field">
                        <label class="label" for="tbUserName">Username</label>
                        <asp:TextBox ID="tbUserName" runat="server"
                            CssClass="input"
                            placeholder="Your username" />
                    </div>

                    <div class="field">
                        <label class="label" for="ddlCategory">Category</label>
                        <asp:DropDownList ID="ddlCategory"
                            runat="server"
                            CssClass="select" />
                    </div>

                    <div class="field">
                        <label class="label" for="tbTitle">Title</label>
                        <asp:TextBox ID="tbTitle"
                            runat="server"
                            CssClass="input"
                            placeholder="Enter a catchy title..." />
                    </div>

                </div>

                <div class="field">
                    <label class="label" for="tbStory">Your story</label>
                    <asp:TextBox ID="tbStory" runat="server" CssClass="textarea" TextMode="MultiLine" Rows="6"
                        placeholder="Write your story here..." />
                </div>

                <asp:Button ID="btnPublish" runat="server" CssClass="btn primary"
                    Text="Publish story" OnClick="btnPublish_Click" />
            </div>
        </section>

        <!-- Filter / Sort -->
        <section class="filterbar">
            <div class="filter-left">
                <svg class="filter-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24" aria-hidden="true">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                        d="M3 4a1 1 0 011-1h16a1 1 0 01.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z">
                    </path>
                </svg>

                <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="select" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged" />
            </div>

            <div class="seg">
                <asp:Button ID="btnSortRecent" runat="server" CssClass="segbtn active" Text="Recent"
                    OnClick="btnSortRecent_Click" UseSubmitBehavior="false" />
                <asp:Button ID="btnSortPopular" runat="server" CssClass="segbtn" Text="Popular"
                    OnClick="btnSortPopular_Click" UseSubmitBehavior="false" />
            </div>
        </section>

        <!-- Stories List -->
        <section class="stories">
            <asp:Repeater ID="rptStories" runat="server">
                <ItemTemplate>
                    <div class="card card-pad story-card">
                        <div class="story-row">

                            <div class="vote-col">
                                <asp:Button ID="btnVoteUp" runat="server" CssClass="btn-vote"
                                    Text="▲" CommandName="vote" CommandArgument='<%# Eval("Id") %>'
                                    OnCommand="Story_Command" UseSubmitBehavior="false" />
                                <div class="vote-num"><%# Eval("Votes") %></div>
                            </div>

                            <div style="flex: 1;">
                                <div class="meta">
                                    <span class='pill <%# Eval("PillClass") %>'><%# Eval("Category") %></span>
                                    <span class="by">by <b><%# Eval("Author") %></b></span>
                                    <span class="time">• <%# Eval("RelativeTime") %></span>
                                </div>

                                <h3 class="story-title"><%# Eval("Title") %></h3>
                                <p class="story-text"><%# Eval("Text") %></p>

                                <div class="actions">
                                    <button type="button" class="action"
                                        data-storyid="<%# Eval("StoryGUID") %>"
                                        onclick="likeStory(this)"
                                        title="Like"
                                        aria-label="Like">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">
                                            <path d="M7 10v12"></path>
                                            <path d="M15 5.88 14 10h5.83a2 2 0 0 1 1.92 2.56l-2.33 8A2 2 0 0 1 17.5 22H4a2 2 0 0 1-2-2v-8a2 2 0 0 1 2-2h2.76a2 2 0 0 0 1.79-1.11L12 2h0a3.13 3.13 0 0 1 3 3.88Z"></path>
                                        </svg>
                                        <span class="likeCount" style="font-weight: 800;"><%# Eval("Likes") %></span>
                                    </button>

                                    <button type="button" class="action"
                                        data-storyid="<%# Eval("StoryGUID") %>"
                                        onclick="dislikeStory(this)"
                                        title="Dislike"
                                        aria-label="Dislike">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">
                                            <path d="M17 14V2"></path>
                                            <path d="M9 18.12 10 14H4.17a2 2 0 0 1-1.92-2.56l2.33-8A2 2 0 0 1 6.5 2H20a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2h-2.76a2 2 0 0 0-1.79 1.11L12 22h0a3.13 3.13 0 0 1-3-3.88Z"></path>
                                        </svg>
                                        <span class="dislikeCount" style="font-weight: 800;"><%# Eval("Dislikes") %></span>
                                    </button>
                                </div>

                                <!-- COMMENTS INLINE -->
                                <div style="margin-top: 16px; border-top: 1px solid var(--gray-200); padding-top: 14px;">
                                    <div style="font-weight: 900; margin-bottom: 10px;">Comments</div>

                                    <!-- Comments list -->
                                    <asp:Repeater ID="rptComments" runat="server">
                                        <HeaderTemplate>
                                            <div style="margin-top: 12px; display: grid; gap: 10px;">
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <div style="background: var(--gray-50); border: 1px solid var(--gray-200); border-radius: 12px; padding: 10px;">
                                                <div style="font-weight: 900;">
                                                    <%# Eval("Name") %>
                                                    <span style="font-weight: 600; color: var(--gray-500); font-size: 12px;">• <%# Eval("RelativeTime") %></span>
                                                </div>
                                                <div style="color: var(--gray-700); margin-top: 6px; line-height: 1.5;">
                                                    <%# Eval("Text") %>
                                                </div>
                                            </div>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            </div>
                                        </FooterTemplate>
                                    </asp:Repeater>

                                    <!-- Add comment form -->
                                    <div style="background: var(--gray-50); border: 1px solid var(--gray-200); border-radius: 12px; padding: 12px; margin-top: 12px;">
                                        <div class="grid-2">
                                            <div class="field" style="margin-bottom: 0;">
                                                <label class="label" for="tbCmtEmail">Name</label>
                                                <asp:TextBox ID="tbCmtEmail" runat="server" CssClass="input" placeholder="Your name (optional)" />
                                            </div>
                                            <div style="display:flex; justify-content:flex-end;">
                                                <asp:Button ID="btnAddComment" runat="server"
                                                    CssClass="btn primary" Style="width:auto;"
                                                    Text="Post"
                                                    CommandName="addComment"
                                                    CommandArgument='<%# Eval("StoryGUID") %>'
                                                    OnCommand="Comment_Command"
                                                    UseSubmitBehavior="false" />
                                            </div>
                                        </div>

                                        <div class="field" style="margin-top: 10px;">
                                            <label class="label" for="tbCmtText">Your comment</label>
                                            <asp:TextBox ID="tbCmtText" runat="server" CssClass="textarea" TextMode="MultiLine" Rows="3"
                                                placeholder="Write your comment..." />
                                        </div>

                                        <asp:Label ID="lblCmtMsg" runat="server" CssClass="msg" EnableViewState="false" />
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </section>
    </main>

    <script>
        function setButtonsDisabled(container, disabled) {
            const btns = container.querySelectorAll("button.action");
            btns.forEach(b => b.disabled = disabled);
        }

        function updateVotesFromResult(container, data) {
            const likeSpan = container.querySelector(".likeCount");
            const dislikeSpan = container.querySelector(".dislikeCount");

            const card = container.closest(".story-card");
            const voteDiv = card ? card.querySelector(".vote-num") : null;

            if (voteDiv && typeof data.votes !== "undefined") voteDiv.textContent = data.votes;
            if (likeSpan && typeof data.likes !== "undefined") likeSpan.textContent = data.likes;
            if (dislikeSpan && typeof data.dislikes !== "undefined") dislikeSpan.textContent = data.dislikes;
        }

        async function callVoteAsmx(methodName, storyId) {
            const res = await fetch("leo.asmx/" + methodName, {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify({ storyId: storyId })
            });

            const json = await res.json();
            return json.d; // ASMX => response is in .d
        }

        async function likeStory(btn) {
            const storyId = btn.getAttribute("data-storyid");
            const container = btn.closest(".actions") || btn.parentElement;

            try {
                setButtonsDisabled(container, true);

                const data = await callVoteAsmx("likeStory", storyId);

                if (!data || data.ok !== true) {
                    alert((data && data.error) ? data.error : "Unknown error");
                    return;
                }

                updateVotesFromResult(container, data);

            } catch (e) {
                console.error(e);
                alert("Service call failed (like).");
            } finally {
                setButtonsDisabled(container, false);
            }
        }

        async function dislikeStory(btn) {
            const storyId = btn.getAttribute("data-storyid");
            const container = btn.closest(".actions") || btn.parentElement;

            try {
                setButtonsDisabled(container, true);

                const data = await callVoteAsmx("DislikeStory", storyId);

                if (!data || data.ok !== true) {
                    alert((data && data.error) ? data.error : "Unknown error");
                    return;
                }

                updateVotesFromResult(container, data);

            } catch (e) {
                console.error(e);
                alert("Service call failed (dislike).");
            } finally {
                setButtonsDisabled(container, false);
            }
        }
    </script>

    <!-- Footer -->
    <footer class="footer">
        <div class="container">
            StoryHub - Creative story sharing platform
        </div>
    </footer>

</form>
</body>
</html>
