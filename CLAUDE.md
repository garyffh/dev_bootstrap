# CLAUDE.md

## Project Overview

Dev.Bootstrap is a Windows Forms application that automates developer environment setup for the FreeFlow Hub organisation. It provides a GUI for selecting repos to clone/remove and auto-installs required tools via winget.

## Tech Stack

- **Language:** C#
- **Framework:** .NET Framework 4.7.2 (Windows Forms)
- **IDE:** Visual Studio 2022
- **Target OS:** Windows 10/11

## Solution Structure

```
dev-bootstrap/
  Dev.Bootstrap/
    Dev.Bootstrap.sln
    DevBootstrap/              -- WinForms project
      Program.cs               -- Entry point
      MainForm.cs              -- Main window with repo checklist
      MainForm.Designer.cs     -- Form layout (auto-generated)
      GitManager.cs            -- Git clone, pull, and remove logic
      ToolInstaller.cs         -- winget installs for Git, GitHub CLI, VS 2022, Claude Code, MarkText
      Config.cs                -- Centralised configuration (org, repos, paths)
      repos.json               -- Repo catalogue with per-project dependency lists
```

## Build & Run

```powershell
cd Dev.Bootstrap
dotnet build
dotnet run --project DevBootstrap
```

## Key Design Decisions

- All tool installation uses **winget** (no Chocolatey, no manual downloads)
- Repos are defined in `repos.json` so new repos can be added without rebuilding
- Each repo entry includes a `dependencies` array; the app unions all dependencies from selected repos and installs missing ones before cloning
- Uninstall only removes repos from disk -- shared tools are left in place
- GitHub authentication uses `gh auth login` (browser-based OAuth), not PATs
- Configuration is centralised in `Config.cs`

## Conventions

- Target the FreeFlow Hub GitHub organisation
- All repos clone to `C:\Projects\{repo-name}`
- Use async operations where possible to keep the UI responsive
- Log all operations to the status panel in MainForm
- Do not auto-generate or modify `MainForm.Designer.cs` by hand -- use the VS Forms Designer
