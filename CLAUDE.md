# CLAUDE.md

## Project Overview

Dev.Bootstrap is a desktop application that automates developer environment setup. A WinForms client provides a GUI for selecting repos to clone/remove, calling the DAL directly (no HTTP server required). An ASP.NET Core server project exists for potential future API access. Repos are hosted under the `garyffh` GitHub account.

## Tech Stack

- **Language:** C#
- **Client:** .NET 8 Windows Forms
- **Server:** ASP.NET Core Minimal API (.NET 8)
- **Shared:** .NET 8 class library (models and interfaces)
- **DAL:** .NET 8 class library (repository implementations -- provider TBD)
- **Database:** TBD (behind interfaces -- EF Core, Dapper, or other provider added later)
- **IDE:** Visual Studio 2022
- **Target OS:** Windows 10/11

## Architecture

```
┌─────────────────────────────────────────┐
│  Client (WinForms)                       │
│                                          │
│  IApiClient ──► ApiClient                │
│                    │                     │
│              IRepoRepository             │
│              IToolRepository             │
│              IConfigRepository           │
│                    │                     │
│              DAL (concrete impl)         │
│                    │                     │
│              Database                    │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│  Server (ASP.NET Minimal API) [optional] │
│  Api/Endpoints ──► same DAL + Core       │
└─────────────────────────────────────────┘
```

- **Core** -- shared models and repository interfaces. No dependency on any DB or ORM.
- **DAL** -- implements repository interfaces from Core. Concrete DB provider chosen at implementation time.
- **Client** -- WinForms app. References DAL directly via DLL; no HTTP server required.
- **Server** -- Minimal API endpoints (optional). Uses the same DI and DAL for potential future API access.

## Solution Structure

```
Dev.Bootstrap/
  Dev.Bootstrap.sln

  src/
    DevBootstrap.Client/             -- WinForms app (.NET 8 Windows)
      Program.cs                     -- Entry point (requests admin elevation)
      MainForm.cs                    -- Main window with repo checklist
      MainForm.Designer.cs           -- Form layout (auto-generated)
      Services/
        IApiClient.cs                -- Interface for data access
        ApiClient.cs                 -- Implementation using DAL repositories directly

    DevBootstrap.Server/             -- ASP.NET Core Minimal API (.NET 8)
      Program.cs                     -- Host builder, DI registration, middleware
      Api/
        RepoEndpoints.cs             -- /api/repos endpoints
        ToolEndpoints.cs             -- /api/tools endpoints
        ConfigEndpoints.cs           -- /api/config endpoints

    DevBootstrap.Core/               -- Shared models and interfaces (.NET 8 class library)
      Models/
        Repo.cs                      -- Repo entity with name, description, dependencies
        Tool.cs                      -- Tool entity with name, winget ID, type
        AppConfig.cs                 -- App configuration model
      Interfaces/
        IRepoRepository.cs           -- Data access interface for repos
        IToolRepository.cs           -- Data access interface for tools
        IConfigRepository.cs         -- Data access interface for config

    DevBootstrap.Dal/                -- Data access layer (.NET 8 class library)
      DependencyInjection.cs         -- Service registration extension method
      (concrete repository implementations added when DB provider is chosen)

  tests/
    DevBootstrap.Server.Tests/
    DevBootstrap.Core.Tests/
```

## Build & Run

```powershell
cd Dev.Bootstrap

# Build everything
dotnet build

# Run the server
dotnet run --project src/DevBootstrap.Server

# Run the client (in a separate terminal, as Administrator)
dotnet run --project src/DevBootstrap.Client
```

## Key Design Decisions

- **Interface-driven** -- database and data access behind interfaces (IRepoRepository, IToolRepository, IConfigRepository) so the concrete provider can be swapped without changing server or client code
- **Minimal API now, controllers later** -- start with ASP.NET Core Minimal API endpoints; a controller layer can be added on top when needed
- **All projects in one solution** -- easier to develop and debug together
- **GitHub account:** `garyffh` (no org -- repos are under the personal account)
- **Git identity:** Read from `git config --global` (`Gary` / `gary@freeflowhub.com.au`)
- **repos.json:** Data moves into the database; GitHub fetch used as seed/sync mechanism
- **Target framework:** .NET 8 (LTS)
- **Admin elevation:** Client requests elevation on launch via manifest (required for winget installs)
- **Error handling:** Failed winget installs are logged and skipped; setup continues
- **Uninstall dialog:** Lists each repo being removed by name
- **Claude Code login:** User prompted to run `claude login` manually after setup
- **Base tools (always installed):** Git, GitHub CLI, VS 2022, Claude Code, MarkText
- All tool installation uses **winget** (no Chocolatey, no manual downloads)
- Uninstall only removes repos from disk -- shared tools are left in place
- GitHub authentication uses `gh auth login` (browser-based OAuth), not PATs

## Conventions

- All repos clone to `C:\Projects\{repo-name}`
- Use async operations where possible to keep the UI responsive
- Log all operations to the status panel in MainForm
- Do not modify `MainForm.Designer.cs` by hand -- use the VS Forms Designer
- Repository interfaces live in Core; implementations live in Dal
- API endpoints are grouped by domain (repos, tools, config)
