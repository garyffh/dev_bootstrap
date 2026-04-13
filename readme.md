# dev-bootstrap

A client/server application for managing developer environment setup for FreeFlow Hub.
A WinForms client provides a visual interface for selecting repos to install or uninstall,
while an ASP.NET Core server manages configuration and repo data backed by a database.

## Architecture

```
┌─────────────┐      HTTP       ┌──────────────────┐
│   Client     │ ◄────────────► │     Server        │
│  (WinForms)  │                │ (Minimal API)     │
│              │                │       │           │
│  IApiClient  │                │  Endpoints        │
│  ApiClient   │                │       │           │
└─────────────┘                │  Core Interfaces  │
                                │       │           │
                                │  DAL (concrete)   │
                                │       │           │
                                │   Database        │
                                └──────────────────┘
```

- **DevBootstrap.Core** -- shared models and repository interfaces (no DB dependency)
- **DevBootstrap.Server** -- ASP.NET Core Minimal API; resolves interfaces via DI
- **DevBootstrap.Dal** -- implements repository interfaces; concrete DB provider added later
- **DevBootstrap.Client** -- WinForms GUI; communicates with server via HTTP

All data access is behind interfaces (IRepoRepository, IToolRepository, IConfigRepository)
so the database provider can be swapped without changing server or client code.

## What it does

- Provides a GUI with checkboxes to select which repos to install or uninstall
- Checks Git is installed (installs via winget if missing)
- Configures Git identity (name and email from global git config)
- Checks GitHub CLI is installed (installs via winget if missing)
- Checks GitHub CLI is authenticated (runs `gh auth login` if not, opening a browser for login)
- Checks Visual Studio 2022 Community is installed (installs via winget if missing)
- Checks Claude Code is installed (installs via winget if missing)
- Adds Claude Code to PATH if missing
- Checks MarkText is installed (installs via winget if missing)
- Creates C:\Projects base directory
- Clones selected repos from GitHub (or pulls latest if already cloned)
- Removes unselected repos from disk when uninstall is confirmed

## Features

- **Repo selector** -- checklist of all available repos with Select All / Deselect All
- **Install** -- clones checked repos (or pulls latest if already present)
- **Uninstall** -- removes unchecked repos from disk after confirmation (lists each repo by name)
- **Status panel** -- real-time progress log showing each operation
- **Auto-detect** -- repos already cloned under C:\Projects are pre-checked on launch
- **Database-backed config** -- repo catalogue and tool definitions stored in a database, served via API

## Requirements

- Windows 10/11
- .NET 8 SDK
- winget (comes with Windows 11, available via Microsoft Store on Windows 10)
- Internet connection
- GitHub access to the garyffh account
- Anthropic account (Pro, Max, Team or Enterprise) for Claude Code

## Project structure

```
Dev.Bootstrap/
  Dev.Bootstrap.sln

  src/
    DevBootstrap.Client/             -- WinForms app (.NET 8 Windows)
      Program.cs                     -- Entry point (requests admin elevation)
      MainForm.cs                    -- Main window with repo checklist
      MainForm.Designer.cs           -- Form layout (auto-generated)
      Services/
        IApiClient.cs                -- Interface for server communication
        ApiClient.cs                 -- HttpClient-based implementation

    DevBootstrap.Server/             -- ASP.NET Core Minimal API (.NET 8)
      Program.cs                     -- Host builder, DI registration
      Api/
        RepoEndpoints.cs             -- /api/repos endpoints
        ToolEndpoints.cs             -- /api/tools endpoints
        ConfigEndpoints.cs           -- /api/config endpoints

    DevBootstrap.Core/               -- Shared models and interfaces (.NET 8 class library)
      Models/
        Repo.cs                      -- Repo with name, description, dependencies
        Tool.cs                      -- Tool with name, winget ID, type
        AppConfig.cs                 -- App configuration model
      Interfaces/
        IRepoRepository.cs           -- Data access interface for repos
        IToolRepository.cs           -- Data access interface for tools
        IConfigRepository.cs         -- Data access interface for config

    DevBootstrap.Dal/                -- Data access layer (.NET 8 class library)
      DependencyInjection.cs         -- Service registration extension method
      (concrete implementations added when DB provider is chosen)

  tests/
    DevBootstrap.Server.Tests/
    DevBootstrap.Core.Tests/
```

## Usage

### Option 1 -- Run from source

```powershell
# Clone this repo
git clone https://github.com/garyffh/dev_bootstrap.git C:\Projects\dev_bootstrap

# Build
cd C:\Projects\dev_bootstrap\Dev.Bootstrap
dotnet build

# Start the server
dotnet run --project src/DevBootstrap.Server

# Start the client (separate terminal, as Administrator)
dotnet run --project src/DevBootstrap.Client
```

### Option 2 -- Run published executable

```powershell
# Start the server
.\DevBootstrap.Server.exe

# Start the client (as Administrator)
.\DevBootstrap.Client.exe
```

### Using the app

1. Start the **server** first
2. Launch **DevBootstrap.Client.exe** as Administrator (the app requests elevation on startup)
3. The app checks for required tools (Git, GitHub CLI, VS 2022, Claude Code, MarkText) and installs any that are missing
4. If a tool install fails, the error is logged and setup continues with the remaining tools
5. If GitHub CLI is not authenticated, the app runs `gh auth login` and opens a browser -- log in with your GitHub account
6. The repo list is loaded from the server (originally seeded from GitHub)
7. Check the repos you want installed, uncheck the ones you want removed
8. Click **Install** to clone/pull the checked repos
9. Click **Uninstall** to remove unchecked repos (a confirmation dialog lists each repo being removed)
10. Monitor progress in the status panel at the bottom
11. After setup, run `claude login` in a terminal to authenticate Claude Code

## Repo catalogue

Repo data is stored in the database and served via the API. The database can be
seeded from the `repos.json` file in the `garyffh/dev_bootstrap` GitHub repo.

Each repo entry includes a dependency list so the app knows
exactly which tools to install when a project is selected. When you check a
repo in the UI, its dependencies are automatically resolved and installed
before cloning.

```json
{
  "repos": [
    {
      "name": "web-portal",
      "description": "Customer-facing Blazor web app",
      "dependencies": ["vs2022", "dotnet8", "node"]
    },
    {
      "name": "api-gateway",
      "description": "ASP.NET Core API layer",
      "dependencies": ["vs2022", "dotnet8"]
    },
    {
      "name": "mobile-app",
      "description": "React Native mobile client",
      "dependencies": ["node", "android-sdk"]
    },
    {
      "name": "data-pipeline",
      "description": "Python ETL and analytics",
      "dependencies": ["vs2022", "python3"]
    },
    {
      "name": "infra",
      "description": "Terraform and deployment scripts",
      "dependencies": ["terraform", "az-cli"]
    }
  ]
}
```

### Available dependency keys

| Key | What gets installed | winget ID |
|---|---|---|
| `vs2022` | Visual Studio 2022 Community | `Microsoft.VisualStudio.2022.Community` |
| `dotnet8` | .NET 8 SDK | `Microsoft.DotNet.SDK.8` |
| `node` | Node.js LTS | `OpenJS.NodeJS.LTS` |
| `python3` | Python 3 | `Python.Python.3.12` |
| `terraform` | Terraform CLI | `Hashicorp.Terraform` |
| `az-cli` | Azure CLI | `Microsoft.AzureCLI` |
| `android-sdk` | Android Studio (includes SDK) | `Google.AndroidStudio` |
| `docker` | Docker Desktop | `Docker.DockerDesktop` |

New dependency keys can be added to `ToolInstaller.cs` as needed.

### How it works

1. User checks repos in the UI
2. Client fetches the repo catalogue from the server API
3. App unions all dependencies from the selected repos
4. Already-installed tools are skipped (detected via `winget list` or `where`)
5. Missing tools are installed via winget; failures are logged and skipped
6. Repos are then cloned or pulled

When uninstalling, only repos are removed -- shared tools are left in place
since other projects may still need them.

## Base tools (always installed)

These tools are installed regardless of repo selection:

| Tool | winget ID |
|---|---|
| Git | `Git.Git` |
| GitHub CLI | `GitHub.cli` |
| Visual Studio 2022 Community | `Microsoft.VisualStudio.2022.Community` |
| Claude Code | `Anthropic.ClaudeCode` |
| MarkText | `MarkText.MarkText` |

## API Endpoints

| Method | Path | Description |
|---|---|---|
| GET | `/api/repos` | List all repos with dependencies |
| GET | `/api/repos/{name}` | Get a single repo |
| GET | `/api/tools` | List all tool definitions |
| GET | `/api/tools/{key}` | Get a single tool definition |
| GET | `/api/config` | Get app configuration |

## After setup

Each repo will be available under C:\Projects. To start a Claude Code session
in any project:

```powershell
cd C:\Projects\YOUR-REPO
claude
```

## Adding new repos

Add a repo entry via the server API or directly in the database. No rebuild needed.

## Config

Configuration is centralised in `Config.cs` (client) and the database (server):

| Property | Description |
|---|---|
| `GitEmail` | Read from `git config --global user.email` |
| `GitName` | Read from `git config --global user.name` |
| `BaseDir` | Local root folder for all projects (default C:\Projects) |
| `GitHubUser` | GitHub username (`garyffh`) |
| `ServerUrl` | Base URL for the DevBootstrap server API |

## First Time Login

### GitHub CLI

After setup, the GitHub CLI requires a one-time authentication:

```powershell
gh auth login
```

Follow the prompts to authenticate via browser. This grants `git clone` access
to private repos without needing a separate PAT.

### Claude Code

Claude Code requires a one-time browser authentication:

```powershell
claude login
```

A browser window will open. Log in with your Anthropic account (Pro, Max, Team or Enterprise).
You will only need to do this once per machine.

### Anthropic Account Requirements

- Free Claude.ai plan does **not** include Claude Code access
- Minimum plan required: **Claude Pro** ($20/month)
- Higher usage: **Claude Max** ($100-200/month)
- For teams: **Claude Team** or **Enterprise**

## Claude Code Basic Usage

Start a session in any project directory:

```powershell
cd C:\Projects\YOUR-REPO
claude
```

Useful commands inside a Claude Code session:

| Command | Description |
|---|---|
| `/help` | Show available commands |
| `/model` | Switch between Claude models |
| `/config` | View and change settings |
| `/memory` | View project memory |
| `exit` | End the session |

## Keeping Tools Updated

Claude Code (native install) updates automatically in the background.
To manually force an update:

```powershell
winget upgrade Anthropic.ClaudeCode
```

To update Git:

```powershell
winget upgrade Git.Git
```

To update the GitHub CLI:

```powershell
winget upgrade GitHub.cli
```

To update Visual Studio 2022:

```powershell
winget upgrade Microsoft.VisualStudio.2022.Community
```

To refresh repos via the GUI, just re-launch the client and click **Install**.

## Troubleshooting

| Problem | Fix |
|---|---|
| `claude` not recognised | Run `$env:PATH += ";$env:USERPROFILE\.local\bin"` then restart PowerShell |
| Git identity error | Run `git config --global user.email "you@example.com"` |
| Repo clone fails | Run `gh auth status` to check GitHub CLI login, or check your PAT has `repo` scope |
| `gh` not recognised | Restart PowerShell so the updated PATH picks up the GitHub CLI |
| winget not found | Install App Installer from the Microsoft Store |
| VS 2022 install hangs | Run `winget install Microsoft.VisualStudio.2022.Community` manually from an elevated PowerShell |
| App won't launch | Ensure .NET 8 SDK is installed |
| Uninstall does nothing | Only unchecked repos are removed -- uncheck the ones you want to delete |
| Tool install fails | The app logs the error and continues -- check the status panel for details |
| Server not reachable | Ensure the server is running before starting the client |

## Visual Studio 2022 Community

dev-bootstrap installs Visual Studio 2022 Community Edition, which is required
by many of the projects. The installer includes the following
workloads by default:

- **.NET desktop development** -- Windows Forms, WPF, console apps
- **ASP.NET and web development** -- web APIs, Blazor, MVC
- **Azure development** -- cloud services and deployment

To install manually:

```powershell
winget install -e --id Microsoft.VisualStudio.2022.Community
```

To update:

```powershell
winget upgrade Microsoft.VisualStudio.2022.Community
```

> **Note:** Visual Studio is a large install (~10-20 GB depending on workloads)
> and may take several minutes. The app's status panel will show progress.

## Markdown Editor (MarkText)

dev-bootstrap installs MarkText as the default markdown editor for viewing and
editing README files and project documentation.

MarkText is a free, open source WYSIWYG markdown editor with live preview.
It is installed automatically as part of the base tool setup.

To install manually:

```powershell
winget install -e --id MarkText.MarkText
```

To update:

```powershell
winget upgrade MarkText.MarkText
```

> **Note:** MarkText does not auto-update via winget. Run the upgrade command
> periodically or when a new version is needed.
