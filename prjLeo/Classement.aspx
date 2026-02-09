<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Classement.aspx.vb" Inherits="StoryHub.Classement" %>

<!DOCTYPE html>
<html lang="fr">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Classement - Plateforme d'Histoires</title>

    <style>
        /* Top 3 - boutons filtre */
        .top-head {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            flex-wrap: wrap;
            margin-bottom: 18px;
        }

        .top-head-left {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .top-filter {
            display: flex;
            gap: 8px;
            align-items: center;
            flex-wrap: wrap;
        }

        .top-pill {
            border: 2px solid var(--amber-200);
            background: #fff;
            color: var(--gray-800);
            padding: 8px 12px;
            border-radius: 999px;
            font-weight: 900;
            cursor: pointer;
            box-shadow: var(--shadow-sm);
        }

            .top-pill:hover {
                filter: brightness(0.98);
            }

            .top-pill.active {
                background: linear-gradient(135deg, #fbbf24 0%, #f59e0b 100%);
                color: #fff;
                border-color: var(--amber-300);
            }




        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

        :root {
            --font: 'Inter',system-ui,-apple-system,'Segoe UI',Roboto,Arial,sans-serif;
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
            --yellow-100: #fef9c3;
            --yellow-200: #fde68a;
            --yellow-400: #facc15;
            --yellow-500: #eab308;
            --yellow-600: #ca8a04;
            --green-100: #d1fae5;
            --green-200: #a7f3d0;
            --green-500: #22c55e;
            --green-600: #16a34a;
            --blue-200: #bfdbfe;
            --blue-400: #60a5fa;
            --blue-500: #3b82f6;
        }

        html, body {
            height: 100%;
        }

        body {
            margin: 0;
            font-family: var(--font);
            background: linear-gradient(135deg,var(--blue-50) 0%,var(--cyan-50) 100%);
            color: var(--gray-900);
            min-height: 100vh;
        }

        /* Layout */
        .container {
            max-width: 1280px;
            margin: 0 auto;
            padding: 0 16px;
        }

        .row {
            display: flex;
        }

        .flex {
            display: flex;
        }

        .items-center {
            align-items: center;
        }

        .justify-between {
            justify-content: space-between;
        }

        .gap-2 {
            gap: 8px;
        }

        .gap-3 {
            gap: 12px;
        }

        .gap-4 {
            gap: 16px;
        }

        .wrap {
            flex-wrap: wrap;
        }

        .py-4 {
            padding-top: 16px;
            padding-bottom: 16px;
        }

        .py-8 {
            padding-top: 32px;
            padding-bottom: 32px;
        }

        .mt-16 {
            margin-top: 64px;
        }

        .mb-2 {
            margin-bottom: 8px;
        }

        .mb-4 {
            margin-bottom: 16px;
        }

        .mb-6 {
            margin-bottom: 24px;
        }

        .mb-8 {
            margin-bottom: 32px;
        }

        /* Header */
        header {
            background: #fff;
            border-bottom: 1px solid var(--gray-200);
            box-shadow: 0 1px 2px rgba(0,0,0,.06);
            position: sticky;
            top: 0;
            z-index: 50;
        }

        .brand h1 {
            margin: 0;
            font-size: 24px;
            font-weight: 800;
            color: var(--gray-900);
        }

        .logo {
            width: 32px;
            height: 32px;
            color: var(--blue-600);
        }

        /* Nav buttons (ASP.NET Buttons) */
        .nav {
            display: flex;
            align-items: center;
            gap: 16px;
            flex-wrap: wrap;
        }

        .navbtn {
            border: 0;
            cursor: pointer;
            padding: 10px 16px;
            border-radius: 10px;
            font-weight: 600;
            background: transparent;
            color: var(--gray-700);
        }

            .navbtn:hover {
                background: var(--gray-100);
            }

            .navbtn.active {
                background: #dbeafe;
            }

            .navbtn.primary {
                background: var(--blue-600);
                color: #fff;
            }

                .navbtn.primary:hover {
                    background: var(--blue-700);
                }

        /* Cards */
        .card {
            background: #fff;
            border-radius: 14px;
            border: 2px solid var(--gray-200);
            box-shadow: 0 10px 15px -3px rgba(0,0,0,.10), 0 4px 6px -4px rgba(0,0,0,.10);
        }

        .p-4 {
            padding: 16px;
        }

        .p-6 {
            padding: 24px;
        }

        /* Page title */
        .title-row {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .page-ico {
            width: 32px;
            height: 32px;
            color: var(--blue-600);
        }

        .h2 {
            margin: 0;
            font-size: 30px;
            font-weight: 800;
        }

        .desc {
            margin: 0;
            color: var(--gray-600);
        }

        /* Top 3 block */
        .top-wrap {
            background: linear-gradient(135deg,var(--amber-50) 0%,var(--orange-50) 100%);
            border: 2px solid var(--amber-200);
            border-radius: 16px;
            box-shadow: 0 20px 25px -5px rgba(0,0,0,.10), 0 8px 10px -6px rgba(0,0,0,.10);
        }

        .top-head {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 18px;
        }

        .top-ico {
            width: 32px;
            height: 32px;
            color: var(--amber-500);
        }

        .h3 {
            margin: 0;
            font-size: 24px;
            font-weight: 800;
        }

        .grid-3 {
            display: grid;
            grid-template-columns: 1fr;
            gap: 16px;
        }

        @media(min-width:768px) {
            .grid-3 {
                grid-template-columns: repeat(3,1fr);
            }
        }

        .mini-card {
            background: #fff;
            border-radius: 12px;
            padding: 16px;
            border: 2px solid var(--gray-300);
            box-shadow: 0 10px 15px -3px rgba(0,0,0,.10), 0 4px 6px -4px rgba(0,0,0,.10);
        }

            .mini-card.first {
                border-color: var(--amber-300);
            }

        .mini-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 8px;
        }

        .top-badge {
            display: inline-block;
            background: linear-gradient(135deg,#fbbf24 0%,#f59e0b 100%);
            color: #fff;
            font-size: 12px;
            font-weight: 800;
            padding: 4px 10px;
            border-radius: 999px;
            box-shadow: 0 4px 6px -1px rgba(251,191,36,.3);
        }

        .badge-2 {
            display: inline-block;
            background: linear-gradient(135deg,#d1d5db 0%,#9ca3af 100%);
            color: #fff;
            font-size: 12px;
            font-weight: 800;
            padding: 4px 10px;
            border-radius: 999px;
            box-shadow: 0 2px 6px rgba(0,0,0,.08);
        }

        .badge-3 {
            display: inline-block;
            background: linear-gradient(135deg,#fb923c 0%,#f97316 100%);
            color: #fff;
            font-size: 12px;
            font-weight: 800;
            padding: 4px 10px;
            border-radius: 999px;
            box-shadow: 0 2px 6px rgba(0,0,0,.08);
        }

        .votes-amber {
            color: var(--amber-600);
            font-weight: 800;
        }

        .votes-gray {
            color: var(--gray-600);
            font-weight: 800;
        }

        .votes-orange {
            color: #ea580c;
            font-weight: 800;
        }

        /* Period winners grid */
        .period-grid {
            display: grid;
            grid-template-columns: 1fr;
            gap: 24px;
        }

        @media(min-width:1024px) {
            .period-grid {
                grid-template-columns: repeat(3,1fr);
            }
        }

        .period-card {
            overflow: hidden;
            border-radius: 16px;
        }

        .period-head {
            padding: 16px;
            color: #fff;
            font-weight: 800;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .head-yellow {
            background: linear-gradient(90deg,var(--yellow-400),var(--yellow-500));
        }

        .head-blue {
            background: linear-gradient(90deg,var(--blue-400),var(--blue-500));
        }

        .head-green {
            background: linear-gradient(90deg,var(--green-500),var(--green-600));
        }

        .center {
            text-align: center;
        }

        .big {
            font-size: 26px;
            font-weight: 900;
            margin: 0;
        }

        .sub {
            margin: 0;
            font-size: 13px;
            color: var(--gray-600);
        }

            .sub b {
                color: var(--gray-900);
            }

        /* Hourglass animation */
        .hourglass {
            width: 64px;
            height: 64px;
            display: inline-block;
            animation: rotate 3s linear infinite;
        }

        @keyframes rotate {
            0%,100% {
                transform: rotateZ(0deg);
            }

            50% {
                transform: rotateZ(180deg);
            }
        }

        .crown {
            width: 48px;
            height: 48px;
            filter: drop-shadow(0 2px 4px rgba(251,191,36,.5));
        }

        /* Winner story cards */
        .story-card {
            background: #fff;
            border-radius: 16px;
            box-shadow: 0 10px 15px -3px rgba(0,0,0,.10), 0 4px 6px -4px rgba(0,0,0,.10);
            padding: 24px;
            border: 2px solid var(--gray-200);
            transition: transform .2s, box-shadow .2s;
        }

            .story-card:hover {
                transform: translateY(-2px);
                box-shadow: 0 20px 25px -5px rgba(0,0,0,.10), 0 8px 10px -6px rgba(0,0,0,.10);
            }

        .border-yellow {
            border-color: var(--yellow-200);
        }

        .border-blue {
            border-color: var(--blue-200);
        }

        .border-green {
            border-color: var(--green-200);
        }

        .tag {
            font-size: 12px;
            font-weight: 800;
            padding: 6px 10px;
            border-radius: 999px;
            display: inline-block;
        }

        .tag-yellow {
            background: var(--yellow-100);
            color: #854d0e;
        }

        .tag-blue {
            background: #dbeafe;
            color: #1e40af;
        }

        .tag-green {
            background: var(--green-100);
            color: #065f46;
        }

        .pill {
            font-size: 12px;
            font-weight: 800;
            padding: 6px 10px;
            border-radius: 999px;
            display: inline-block;
        }

        .pill-blue {
            background: #dbeafe;
            color: #1d4ed8;
        }

        .pill-purple {
            background: #ede9fe;
            color: #6d28d9;
        }

        .vote-col {
            width: 64px;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 10px;
            padding-top: 6px;
        }

        .vote-box {
            width: 48px;
            height: 48px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .vote-yellow {
            background: #fefce8;
            color: var(--yellow-600);
        }

        .vote-blue {
            background: var(--blue-50);
            color: var(--blue-600);
        }

        .vote-green {
            background: var(--green-100);
            color: var(--green-600);
        }

        .vote-num {
            font-size: 22px;
            font-weight: 900;
        }

            .vote-num.yellow {
                color: var(--yellow-600);
            }

            .vote-num.blue {
                color: var(--blue-600);
            }

            .vote-num.green {
                color: var(--green-600);
            }

        .meta {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            align-items: center;
            margin-bottom: 10px;
        }

        .by {
            color: var(--gray-500);
            font-size: 13px;
        }

            .by b {
                color: var(--gray-700);
            }

        .time {
            color: var(--gray-400);
            font-size: 13px;
        }

        .story-title {
            margin: 0 0 10px 0;
            font-size: 24px;
            font-weight: 900;
        }

        .story-text {
            margin: 0 0 12px 0;
            color: var(--gray-700);
            line-height: 1.6;
        }

        .stats {
            display: flex;
            gap: 16px;
            color: var(--gray-600);
            font-size: 13px;
        }

        .stat {
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        .ico {
            width: 18px;
            height: 18px;
        }

        /* LinkButton look */
        .asp-linkbtn {
            border: 0;
            padding: 0;
            margin: 0;
            background: transparent;
            cursor: pointer;
            color: inherit;
            font: inherit;
        }

        footer {
            background: #fff;
            border-top: 1px solid var(--gray-200);
            padding: 32px 0;
            text-align: center;
            color: var(--gray-600);
            font-size: 13px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:Label ID="TestLabel" runat="server" />
        <!-- Header -->
        <header>
            <div class="container py-4">
                <div class="flex items-center justify-between gap-4 wrap">
                    <div class="brand flex items-center gap-3">
                         <img src="logo.png" alt="StoryHub Logo" style="height:36px; width:auto;" />
                    </div>

                    <nav class="nav">
                        <asp:Button ID="btnNavHome" runat="server" CssClass="navbtn" Text="Accueil" UseSubmitBehavior="false" OnClick="btnNavHome_Click" />
                        <asp:Button ID="btnNavRank" runat="server" CssClass="navbtn active" Text="Classement" UseSubmitBehavior="false" />
                       </nav>
                </div>
            </div>
        </header>

        <!-- Main -->
        <main class="container py-8">

            <!-- Page Header -->
            <div class="mb-8">
                <div class="title-row mb-4">
                    <svg class="page-ico" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6"></path>
                    </svg>
                    <h2 class="h2">Classement des Histoires</h2>
                </div>
                <p class="desc">Découvrez les histoires les plus populaires et votez pour vos préférées</p>
            </div>

            <!-- Top 3 -->
            <section class="top-wrap p-6 mb-8">
               <div class="top-head">

    <div class="top-head-left">
        <svg class="top-ico" fill="none" stroke="currentColor" viewBox="0 0 24 24" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round"
                  d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"></path>
        </svg>

        <h3>Top 3 des Meilleures Histoires</h3>

        <!-- Infos (non bindées) -->
        <div style="display:flex; align-items:center; gap:10px; flex-wrap:wrap;">
            <span style="
                display:inline-flex; align-items:center; gap:8px;
                padding:6px 10px; border-radius:999px;
                background:#ffffff; border:2px solid var(--amber-200);
                font-weight:900; font-size:20px; color:var(--gray-700);
                box-shadow: var(--shadow-sm);
            ">
                Temps restant: <span style="color:var(--amber-600);"><asp:Label ID="lblTempsRestant" EnableViewState="true" runat="server"    /></span>
            </span>

           
        </div>
    </div>

    <div class="top-filter">
        <asp:HiddenField ID="hfTopPeriod" runat="server" Value="WEEK" />

        <asp:Button ID="btnTopWeek" runat="server"
            Text="Cette semaine"
            CssClass="top-pill"
            UseSubmitBehavior="false"
            OnClick="btnTopWeek_Click" />

        <asp:Button ID="btnTopMonth" runat="server"
            Text="Ce mois"
            CssClass="top-pill"
            UseSubmitBehavior="false"
            OnClick="btnTopMonth_Click" />

        <asp:Button ID="btnTopYear" runat="server"
            Text="Cette année"
            CssClass="top-pill"
            UseSubmitBehavior="false"
            OnClick="btnTopYear_Click" />
    </div>

</div>

                <div class="grid-3">
                    <asp:Panel ID="pnlTop1" runat="server" CssClass="mini-card first" />
                    <asp:Panel ID="pnlTop2" runat="server" CssClass="mini-card" />
                    <asp:Panel ID="pnlTop3" runat="server" CssClass="mini-card" />
                </div>
            </section>



            <!-- Winner stories -->
            <h3 class="h3 mb-6" style="color: var(--gray-900);">Histoires Gagnantes</h3>

            <asp:Repeater ID="rptWinners" runat="server">
                <ItemTemplate>
                    <div class='story-card <%# Eval("BorderClass") %> mb-6'>

                        <div class="flex items-center gap-2 mb-4">
                            <svg width="22" height="22" viewBox="0 0 24 24" fill="currentColor" style='color: <%# Eval("StarColor") %>' aria-hidden="true">
                                <path d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z"></path>
                            </svg>
                            <span class='tag <%# Eval("TagClass") %>'><%# Eval("TagText") %></span>
                        </div>

                        <div class="flex gap-4" style="align-items: flex-start;">
                            <div class="vote-col">
                                 

                                <div class='vote-num <%# Eval("VoteNumClass") %>'><%# Eval("Votes") %></div>

                                <asp:LinkButton ID="lnkVote" runat="server" CssClass="asp-linkbtn"
                                    CommandName="vote" CommandArgument='<%# Eval("Id") %>'
                                    OnCommand="Winner_Command" ToolTip="Voter +1">
                                <span style="display:inline-block; font-size:12px; font-weight:900; color:var(--gray-600);">
                                    Voter
                                </span>
                                </asp:LinkButton>
                            </div>

                            <div style="flex: 1;">
                                <div class="meta">
                                     
                                    <span class="by">par <b><%# Eval("Author") %></b></span>
                                    <span class="time">• <%# Eval("RelativeTime") %></span>
                                </div>

                                <h3 class="story-title"><%# Eval("Title") %></h3>
                                <p class="story-text"><%# Eval("Text") %></p>

                                <div class="stats">
                                    <span class="stat">
                                        <svg class="ico" fill="currentColor" viewBox="0 0 24 24" style="color: #ef4444;">
                                            <path d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"></path>
                                        </svg>
                                        <b><%# Eval("LikesLabel") %></b>
                                    </span>

                                    <span class="stat">
                                        <svg class="ico" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                                d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z">
                                            </path>
                                        </svg>
                                        <b><%# Eval("CommentsLabel") %></b>
                                    </span>
                                </div>
                            </div>
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </main>

        <footer>
            <div class="container">
                StoryHub - Plateforme de partage d'histoires créatives
            </div>
        </footer>

    </form>
</body>
</html>
