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
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Owner,
    
    [Parameter(Mandatory = $false)]
    [string]$Repo = "GestorApp",
    
    [Parameter(Mandatory = $true)]
    [string]$Token
)

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommandPath
$Colors = @{
    Success = "Green"
    Warning = "Yellow"
    Error   = "Red"
    Info    = "Cyan"
    Header  = "Yellow"
}

function Invoke-MigrationScript {
    param(
        [string]$ScriptName,
        [string]$Description
    )
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor $Colors.Header
    Write-Host "$Description" -ForegroundColor $Colors.Header
    Write-Host "========================================" -ForegroundColor $Colors.Header
    Write-Host ""
    
    $ScriptPath = Join-Path $ScriptDir $ScriptName
    
    if (-not (Test-Path $ScriptPath)) {
        Write-Host "ERROR: Script not found: $ScriptPath" -ForegroundColor $Colors.Error
        return $false
    }
    
    try {
        & $ScriptPath -Owner $Owner -Repo $Repo -Token $Token
        return $true
    }
    catch {
        Write-Host "ERROR running $ScriptName" -ForegroundColor $Colors.Error
        Write-Host "Message: $($_.Exception.Message)" -ForegroundColor $Colors.Error
        return $false
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor $Colors.Header
Write-Host "GitHub Setup - MAUI Migration" -ForegroundColor $Colors.Header
Write-Host "========================================" -ForegroundColor $Colors.Header
Write-Host ""

Write-Host "Information:" -ForegroundColor $Colors.Info
Write-Host "  Owner: $Owner" -ForegroundColor $Colors.Info
Write-Host "  Repository: $Repo" -ForegroundColor $Colors.Info
Write-Host "  Token: $($Token.Substring(0, 10))..." -ForegroundColor $Colors.Info
Write-Host ""

$Results = @()

$MilestoneSuccess = Invoke-MigrationScript "create-milestones.ps1" "Creating Milestones"
$Results += @{
    Script = "create-milestones.ps1"
    Success = $MilestoneSuccess
    Description = "7 milestones"
}

$LabelSuccess = Invoke-MigrationScript "create-labels.ps1" "Creating Labels"
$Results += @{
    Script = "create-labels.ps1"
    Success = $LabelSuccess
    Description = "31 labels"
}

Write-Host ""
Write-Host "========================================" -ForegroundColor $Colors.Header
Write-Host "SUMMARY" -ForegroundColor $Colors.Header
Write-Host "========================================" -ForegroundColor $Colors.Header
Write-Host ""

$AllSuccess = $true
foreach ($Result in $Results) {
    $Status = if ($Result.Success) { "SUCCESS" } else { "FAILED" }
    $Color = if ($Result.Success) { $Colors.Success } else { $Colors.Error }
    Write-Host "$Status - $($Result.Script) ($($Result.Description))" -ForegroundColor $Color
    
    if (-not $Result.Success) {
        $AllSuccess = $false
    }
}

Write-Host ""

if ($AllSuccess) {
    Write-Host "========================================" -ForegroundColor $Colors.Success
    Write-Host "GitHub Setup Completed Successfully!" -ForegroundColor $Colors.Success
    Write-Host "========================================" -ForegroundColor $Colors.Success
    Write-Host ""
    
    Write-Host "Repository URL:" -ForegroundColor $Colors.Info
    Write-Host "  https://github.com/$Owner/$Repo" -ForegroundColor $Colors.Success
    Write-Host ""
    
    Write-Host "Next Steps:" -ForegroundColor $Colors.Info
    Write-Host "  1. Verify milestones: Issues - Milestones" -ForegroundColor $Colors.Info
    Write-Host "  2. Verify labels: Issues - Labels" -ForegroundColor $Colors.Info
    Write-Host "  3. Create issues using: docs/GITHUB_ISSUES_READY.md" -ForegroundColor $Colors.Info
    Write-Host ""
    
    exit 0
}
else {
    Write-Host "========================================" -ForegroundColor $Colors.Error
    Write-Host "GitHub Setup Failed" -ForegroundColor $Colors.Error
    Write-Host "========================================" -ForegroundColor $Colors.Error
    Write-Host ""
    
    Write-Host "Troubleshooting:" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify GitHub token is valid" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify token has 'repo' scope" -ForegroundColor $Colors.Warning
    Write-Host "  - Verify repository name is correct" -ForegroundColor $Colors.Warning
    Write-Host ""
    
    exit 1
}

