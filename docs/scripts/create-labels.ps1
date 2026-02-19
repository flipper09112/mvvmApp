#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Create all MAUI Migration labels in GitHub
    
.DESCRIPTION
    This script creates GitHub labels for the MAUI migration project
    
.PARAMETER Owner
    GitHub repository owner (your username or organization)
    
.PARAMETER Repo
    GitHub repository name
    
.PARAMETER Token
    GitHub Personal Access Token (must have 'repo' scope)
    
.EXAMPLE
    .\create-labels.ps1 -Owner "your-username" -Repo "GestorApp" -Token "ghp_xxxxx"
    
.NOTES
    Requires: PowerShell 5.0 or higher
    Script location: docs/scripts/
#>

param(
    [Parameter(Mandatory = $true, HelpMessage = "GitHub username or organization name")]
    [string]$Owner,
    
    [Parameter(Mandatory = $true, HelpMessage = "GitHub repository name")]
    [string]$Repo,
    
    [Parameter(Mandatory = $true, HelpMessage = "GitHub Personal Access Token")]
    [string]$Token
)

# Set error action
$ErrorActionPreference = "Stop"

# Colors for output
$Colors = @{
    Success = "Green"
    Warning = "Yellow"
    Error   = "Red"
    Info    = "Cyan"
    Header  = "Yellow"
}

# Base URL for GitHub API
$BaseUrl = "https://api.github.com"

# Headers for GitHub API
$Headers = @{
    "Authorization" = "Bearer $Token"
    "Accept"        = "application/vnd.github+json"
    "X-GitHub-Api-Version" = "2022-11-28"
}

# Define labels
$Labels = @(
    # Type Labels
    @{ category = "Type"; name = "type:migration"; color = "2E8B57"; description = "Tarefa de migração" },
    @{ category = "Type"; name = "type:infra"; color = "1E90FF"; description = "Infraestrutura/específico de plataforma" },
    @{ category = "Type"; name = "type:ui"; color = "FF69B4"; description = "Interface do usuário" },
    @{ category = "Type"; name = "type:feature"; color = "32CD32"; description = "Implementação de feature" },
    @{ category = "Type"; name = "type:security"; color = "FF0000"; description = "Relacionado a segurança" },
    @{ category = "Type"; name = "type:test"; color = "9370DB"; description = "Testes e QA" },
    
    # Platform Labels
    @{ category = "Platform"; name = "platform:maui"; color = "6F42C1"; description = "Plataforma .NET MAUI" },
    @{ category = "Platform"; name = "platform:android"; color = "3DDC84"; description = "Específico Android" },
    @{ category = "Platform"; name = "platform:ios"; color = "000000"; description = "Específico iOS" },
    @{ category = "Platform"; name = "platform:windows"; color = "0078D4"; description = "Específico Windows" },
    
    # Risk Labels
    @{ category = "Risk"; name = "risk:critical"; color = "B60205"; description = "Risco crítico" },
    @{ category = "Risk"; name = "risk:high"; color = "D73A49"; description = "Risco alto" },
    @{ category = "Risk"; name = "risk:medium"; color = "ED6A43"; description = "Risco médio" },
    @{ category = "Risk"; name = "risk:low"; color = "FBCA04"; description = "Risco baixo" },
    
    # Phase Labels
    @{ category = "Phase"; name = "phase:assessment"; color = "5DADE2"; description = "Fase Assessment" },
    @{ category = "Phase"; name = "phase:setup"; color = "52BE80"; description = "Fase Setup" },
    @{ category = "Phase"; name = "phase:core"; color = "F39C12"; description = "Fase Core Migration" },
    @{ category = "Phase"; name = "phase:infrastructure"; color = "8E44AD"; description = "Fase Infrastructure" },
    @{ category = "Phase"; name = "phase:ui"; color = "E74C3C"; description = "Fase UI Migration" },
    @{ category = "Phase"; name = "phase:feature"; color = "3498DB"; description = "Fase Feature Integration" },
    @{ category = "Phase"; name = "phase:testing"; color = "16A085"; description = "Fase Testing & QA" },
    @{ category = "Phase"; name = "phase:release"; color = "C0392B"; description = "Fase Release" },
    
    # Priority Labels
    @{ category = "Priority"; name = "priority:P0"; color = "B60205"; description = "Prioridade crítica" },
    @{ category = "Priority"; name = "priority:P1"; color = "D73A49"; description = "Prioridade alta" },
    @{ category = "Priority"; name = "priority:P2"; color = "F39C12"; description = "Prioridade média" },
    @{ category = "Priority"; name = "priority:P3"; color = "FBCA04"; description = "Prioridade baixa" },
    
    # Component Labels
    @{ category = "Component"; name = "component:navigation"; color = "0366D6"; description = "Sistema de navegação" },
    @{ category = "Component"; name = "component:database"; color = "6F42C1"; description = "Camada de dados" },
    @{ category = "Component"; name = "component:authentication"; color = "FF0000"; description = "Autenticação" },
    @{ category = "Component"; name = "component:bluetooth"; color = "2E8B57"; description = "Funcionalidade Bluetooth" },
    @{ category = "Component"; name = "component:maps"; color = "FF6B6B"; description = "Integração de mapas" },
    @{ category = "Component"; name = "component:notifications"; color = "4ECDC4"; description = "Notificações" }
)

# Function to create a label
function New-GitHubLabel {
    param(
        [hashtable]$Label
    )
    
    $Uri = "$BaseUrl/repos/$Owner/$Repo/labels"
    $Body = @{
        name        = $Label.name
        color       = $Label.color
        description = $Label.description
    } | ConvertTo-Json
    
    try {
        $Response = Invoke-RestMethod -Uri $Uri -Method POST -Headers $Headers -Body $Body
        Write-Host "  ✓ $($Label.name)" -ForegroundColor $Colors.Success
        return $Response
    }
    catch {
        # Check if label already exists
        if ($_.Exception.Response.StatusCode -eq "Conflict") {
            Write-Host "  ⚠ $($Label.name) (já existe)" -ForegroundColor $Colors.Warning
            return $null
        }
        else {
            Write-Host "  ✗ $($Label.name) - ERRO" -ForegroundColor $Colors.Error
            return $null
        }
    }
}

# Main execution
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║         GitHub Label Creator - MAUI Migration     ║" -ForegroundColor $Colors.Header
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

# Validate inputs
Write-Host "Validando informações..." -ForegroundColor $Colors.Info
Write-Host "  Owner: $Owner" -ForegroundColor $Colors.Info
Write-Host "  Repo: $Repo" -ForegroundColor $Colors.Info
Write-Host "  Token: $($Token.Substring(0, 10))..." -ForegroundColor $Colors.Info
Write-Host "  Total de labels: $($Labels.Count)" -ForegroundColor $Colors.Info
Write-Host ""

# Create all labels grouped by category
$CreatedLabels = @()
$SkippedLabels = 0
$LastCategory = ""

foreach ($Label in $Labels) {
    # Print category header
    if ($Label.category -ne $LastCategory) {
        Write-Host "$($Label.category) Labels:" -ForegroundColor $Colors.Header
        $LastCategory = $Label.category
    }
    
    $Created = New-GitHubLabel -Label $Label
    if ($Created) {
        $CreatedLabels += $Created
    } else {
        $SkippedLabels++
    }
    Start-Sleep -Milliseconds 150  # Rate limiting
}

# Summary
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║                     Sumário                        ║" -ForegroundColor $Colors.Header
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""
Write-Host "Total de labels criadas: $($CreatedLabels.Count)" -ForegroundColor $Colors.Success
Write-Host "Labels existentes ignoradas: $SkippedLabels" -ForegroundColor $Colors.Warning
Write-Host ""

# Group by category
$Categories = $Labels | Group-Object -Property category | Sort-Object Name

foreach ($Category in $Categories) {
    Write-Host "$($Category.Name): $($Category.Count) labels" -ForegroundColor $Colors.Info
}

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║    ✓ Criação de labels concluída com sucesso!    ║" -ForegroundColor $Colors.Success
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

# Next steps
Write-Host "Próximos passos:" -ForegroundColor $Colors.Info
Write-Host "  1. Crie as issues usando: docs/GITHUB_ISSUES_READY.md" -ForegroundColor $Colors.Info
Write-Host "  2. Configure o GitHub Projects board" -ForegroundColor $Colors.Info
Write-Host ""

