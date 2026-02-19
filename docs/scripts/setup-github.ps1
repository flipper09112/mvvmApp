#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Complete GitHub setup for MAUI Migration project
    
.DESCRIPTION
    This script automates the complete GitHub setup:
    1. Creates 7 milestones
    2. Creates 31 labels
    
.PARAMETER Owner
    GitHub repository owner (your username or organization)
    
.PARAMETER Repo
    GitHub repository name (default: GestorApp)
    
.PARAMETER Token
    GitHub Personal Access Token (must have 'repo' scope)
    
.EXAMPLE
    .\setup-github.ps1 -Owner "your-username" -Repo "GestorApp" -Token "ghp_xxxxx"
    
.NOTES
    Requires: PowerShell 5.0 or higher
    Script location: docs/scripts/
#>

param(
    [Parameter(Mandatory = $true, HelpMessage = "GitHub username or organization name")]
    [string]$Owner,
    
    [Parameter(Mandatory = $false, HelpMessage = "GitHub repository name")]
    [string]$Repo = "GestorApp",
    
    [Parameter(Mandatory = $true, HelpMessage = "GitHub Personal Access Token")]
    [string]$Token
)

# Get script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommandPath
$Colors = @{
    Success = "Green"
    Warning = "Yellow"
    Error   = "Red"
    Info    = "Cyan"
    Header  = "Yellow"
}

# Function to run a script
function Invoke-MigrationScript {
    param(
        [string]$ScriptName,
        [string]$Description
    )
    
    Write-Host ""
    Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
    Write-Host "║ $Description" -ForegroundColor $Colors.Header
    Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
    Write-Host ""
    
    $ScriptPath = Join-Path $ScriptDir $ScriptName
    
    if (-not (Test-Path $ScriptPath)) {
        Write-Host "✗ Script não encontrado: $ScriptPath" -ForegroundColor $Colors.Error
        return $false
    }
    
    try {
        & $ScriptPath -Owner $Owner -Repo $Repo -Token $Token
        return $true
    }
    catch {
        Write-Host "✗ Erro ao executar $ScriptName" -ForegroundColor $Colors.Error
        Write-Host "Mensagem: $($_.Exception.Message)" -ForegroundColor $Colors.Error
        return $false
    }
}

# Main execution
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║     Complete GitHub Setup - MAUI Migration Project       ║" -ForegroundColor $Colors.Header
Write-Host "╚═══════════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

Write-Host "Informações:" -ForegroundColor $Colors.Info
Write-Host "  Owner: $Owner" -ForegroundColor $Colors.Info
Write-Host "  Repository: $Repo" -ForegroundColor $Colors.Info
Write-Host "  Token: $($Token.Substring(0, 10))..." -ForegroundColor $Colors.Info
Write-Host "  Scripts Dir: $ScriptDir" -ForegroundColor $Colors.Info
Write-Host ""

# Execute scripts in order
$Results = @()

# 1. Create milestones
$MilestoneSuccess = Invoke-MigrationScript "create-milestones.ps1" "Creating Milestones"
$Results += @{
    Script = "create-milestones.ps1"
    Success = $MilestoneSuccess
    Description = "7 milestones"
}

# 2. Create labels
$LabelSuccess = Invoke-MigrationScript "create-labels.ps1" "Creating Labels"
$Results += @{
    Script = "create-labels.ps1"
    Success = $LabelSuccess
    Description = "31 labels"
}

# Final Summary
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║                   FINAL SUMMARY                           ║" -ForegroundColor $Colors.Header
Write-Host "╚═══════════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

$AllSuccess = $true
foreach ($Result in $Results) {
    $Status = if ($Result.Success) { "✓ SUCCESS" } else { "✗ FAILED" }
    $Color = if ($Result.Success) { $Colors.Success } else { $Colors.Error }
    Write-Host "$Status - $($Result.Script) ($($Result.Description))" -ForegroundColor $Color
    
    if (-not $Result.Success) {
        $AllSuccess = $false
    }
}

Write-Host ""

if ($AllSuccess) {
    Write-Host "╔═══════════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Success
    Write-Host "║     ✓ GitHub Setup Completed Successfully!               ║" -ForegroundColor $Colors.Success
    Write-Host "╚═══════════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Success
    Write-Host ""
    
    Write-Host "Repository URL:" -ForegroundColor $Colors.Info
    Write-Host "  https://github.com/$Owner/$Repo" -ForegroundColor $Colors.Success
    Write-Host ""
    
    Write-Host "Next Steps:" -ForegroundColor $Colors.Info
    Write-Host "  1. Verify milestones: $Owner/$Repo → Issues → Milestones" -ForegroundColor $Colors.Info
    Write-Host "  2. Verify labels: $Owner/$Repo → Issues → Labels" -ForegroundColor $Colors.Info
    Write-Host "  3. Create issues using: docs/GITHUB_ISSUES_READY.md" -ForegroundColor $Colors.Info
    Write-Host "  4. Setup GitHub Projects board" -ForegroundColor $Colors.Info
    Write-Host ""
    
    exit 0
}
else {
    Write-Host "╔═══════════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Error
    Write-Host "║     ✗ GitHub Setup Failed - Review Errors Above          ║" -ForegroundColor $Colors.Error
    Write-Host "╚═══════════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Error
    Write-Host ""
    
    Write-Host "Troubleshooting:" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify GitHub token is valid and has 'repo' scope" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify repository name is correct (case-sensitive)" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify you have push access to the repository" -ForegroundColor $Colors.Warning
    Write-Host ""
    
    exit 1
}

