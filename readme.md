# dev-bootstrap

A Windows Forms application for managing developer environment setup for FreeFlow Hub.
Select which repos to install or uninstall through a visual interface.

## What it does

- Provides a GUI with checkboxes to select which repos to install or uninstall
- Checks Git is installed (installs via winget if missing)
- Configures Git identity (name and email)
- Checks GitHub CLI is installed (installs via winget if missing)
- Checks GitHub CLI is authenticated (runs `gh auth login` if not, opening a browser for login)
- Checks Visual Studio 2022 Community is installed (installs via winget if missing)
- Checks Claude Code is installed (installs via winget if missing)
- Adds Claude Code to PATH if missing
- Creates C:\Projects base directory
- Clones selected repos from GitHub (or pulls latest if already cloned)
- Removes unselected repos from disk when uninstall is confirmed

## Features

- **Repo selector** -- checklist of all available FreeFlow Hub repos with Select All / Deselect All
- **Install** -- clones checked repos (or pulls latest if already present)
- **Uninstall** -- removes unchecked repos from disk after confirmation
- **Status panel** -- real-time progress log showing each operation
- **Auto-detect** -- repos already cloned under C:\Projects are pre-checked on launch

## Requirements

- Windows 10/11
- .NET Framework 4.7.2 or later (included with Windows 10 1803+)
- winget (comes with Windows 11, available via Microsoft Store on Windows 10)
- Internet connection
- GitHub access to the FreeFlow Hub organisation
- Anthropic account (Pro, Max, Team or Enterprise) for Claude Code

## Usage

### Option 1 -- Run from source

```powershell
# Clone this repo
git clone https://github.com/YOUR-ORG/dev-bootstrap.git C:\Projects\dev-bootstrap

# Open the solution in Visual Studio and run, or build from CLI
cd C:\Projects\dev-bootstrap
dotnet build
dotnet run
```

### Option 2 -- Run published executable

```powershell
# Download the latest release and run
.\DevBootstrap.exe
```

### Using the app

1. Launch **DevBootstrap.exe**
2. The app checks for required tools (Git, GitHub CLI) and installs any that are missing
3. If GitHub CLI is not authenticated, the app runs `gh auth login` and opens a browser -- log in with your GitHub account that has access to the FreeFlow Hub org
4. The repo list loads with already-cloned repos pre-checked
5. Check the repos you want installed, uncheck the ones you want removed
6. Click **Install** to clone/pull the checked repos
7. Click **Uninstall** to remove unchecked repos (a confirmation dialog will appear)
8. Monitor progress in the status panel at the bottom

## Project structure

```
dev-bootstrap/
  DevBootstrap.sln            -- Visual Studio solution
  DevBootstrap/
    Program.cs                 -- Entry point
    MainForm.cs                -- Main window with repo checklist
    MainForm.Designer.cs       -- Form layout (auto-generated)
    GitManager.cs              -- Git clone, pull, and remove logic
    ToolInstaller.cs           -- winget install/update for Git, GitHub CLI, VS 2022, Claude Code, MarkText
    Config.cs                  -- Centralised configuration (org, repos, paths)
    repos.json                 -- Repo catalogue with per-project dependency lists
```

## Repo catalogue (repos.json)

Each repo entry in `repos.json` includes a dependency list so the app knows
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
      "dependencies": ["python3", "vs2022"]
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
2. App reads `repos.json` and unions all dependencies from the selected repos
3. Already-installed tools are skipped (detected via `winget list` or `where`)
4. Missing tools are installed via winget
5. Repos are then cloned or pulled

When uninstalling, only repos are removed -- shared tools are left in place
since other projects may still need them.

## After setup

Each repo will be available under C:\Projects. To start a Claude Code session
in any project:

```powershell
cd C:\Projects\YOUR-REPO
claude
```

## Adding new repos

Add an entry to `repos.json` with the repo name, description, and its
dependencies. No rebuild needed -- the app reads the file on launch.

## Config

All configuration is centralised in `Config.cs`:

| Property | Description |
|---|---|
| `GitEmail` | Git global email address |
| `GitName` | Git global display name |
| `BaseDir` | Local root folder for all projects (default C:\Projects) |
| `GitHubOrg` | GitHub organisation or username |
| `Repos` | Loaded from `repos.json` -- repo names, descriptions, and dependencies |

## First Time Login

### GitHub CLI

After setup, the GitHub CLI requires a one-time authentication:

```powershell
gh auth login
```

Follow the prompts to authenticate via browser. This grants `git clone` access
to private FreeFlow Hub repos without needing a separate PAT.

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

To refresh repos via the GUI, just re-launch **DevBootstrap.exe** and click **Install**.

## Troubleshooting

| Problem | Fix |
|---|---|
| `claude` not recognised | Run `$env:PATH += ";$env:USERPROFILE\.local\bin"` then restart PowerShell |
| Git identity error | Run `git config --global user.email "you@example.com"` |
| Repo clone fails | Run `gh auth status` to check GitHub CLI login, or check your PAT has `repo` scope |
| `gh` not recognised | Restart PowerShell so the updated PATH picks up the GitHub CLI |
| winget not found | Install App Installer from the Microsoft Store |
| VS 2022 install hangs | Run `winget install Microsoft.VisualStudio.2022.Community` manually from an elevated PowerShell |
| App won't launch | Ensure .NET Framework 4.7.2+ is installed |
| Uninstall does nothing | Only unchecked repos are removed -- uncheck the ones you want to delete |

## Visual Studio 2022 Community

dev-bootstrap installs Visual Studio 2022 Community Edition, which is required
by many of the FreeFlow Hub projects. The installer includes the following
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
It is installed automatically as part of the setup.

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
