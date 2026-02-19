#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Create all MAUI Migration milestones in GitHub
    
.DESCRIPTION
    This script creates 7 milestones for the MAUI migration project in GitHub
    
.PARAMETER Owner
    GitHub repository owner (your username or organization)
    
.PARAMETER Repo
    GitHub repository name
    
.PARAMETER Token
    GitHub Personal Access Token (must have 'repo' scope)
    
.EXAMPLE
    .\create-milestones.ps1 -Owner "your-username" -Repo "GestorApp" -Token "ghp_xxxxx"
    
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

# Calculate dates (starting from next Monday)
$StartDate = Get-Date -Date "2026-02-24"  # Next Monday
$Week1End   = $StartDate.AddDays(7)       # 1 week
$Week3End   = $Week1End.AddDays(14)       # +2 weeks = 3 total
$Week6End   = $Week3End.AddDays(21)       # +3 weeks = 6 total
$Week12End  = $Week6End.AddDays(42)       # +6 weeks = 12 total
$Week15End  = $Week12End.AddDays(21)      # +3 weeks = 15 total
$Week17End  = $Week15End.AddDays(14)      # +2 weeks = 17 total
$Week20End  = $Week17End.AddDays(21)      # +3 weeks = 20 total

# Define milestones with description and due dates
$Milestones = @(
    @{
        title       = "Milestone 1: Assessment & Planning"
        description = "Fase 0: Análise completa do projeto e criação do roadmap de migração. Auditoria de dependências, decisões arquiteturais e estratégias de risco. 8 issues."
        due_on      = $Week1End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 8
        duration    = "1 semana"
    },
    @{
        title       = "Milestone 2: MAUI Foundation"
        description = "Fase 1: Configuração da infraestrutura MAUI. Setup do DI, Shell navigation, banco de dados, logging, permissões. 12 issues."
        due_on      = $Week3End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 12
        duration    = "2 semanas"
    },
    @{
        title       = "Milestone 3: Core & Infrastructure"
        description = "Fases 2-3: Migração da camada core e serviços de plataforma. Migração de 47 ViewModels, 96 serviços, SQLite, Bluetooth, background tasks. 37 issues."
        due_on      = $Week6End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 37
        duration    = "3 semanas"
    },
    @{
        title       = "Milestone 4: UI Layer Migration"
        description = "Fase 4: Migração de todas as 51 Fragments para Pages/Views MAUI. Implementação de navegação, TabPages, maps integration. 51 issues."
        due_on      = $Week12End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 51
        duration    = "6 semanas"
    },
    @{
        title       = "Milestone 5: Feature Integration"
        description = "Fase 5: Migração de 42 adapters para CollectionView. Integração completa de features e testes end-to-end. 24 issues."
        due_on      = $Week15End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 24
        duration    = "3 semanas"
    },
    @{
        title       = "Milestone 6: Testing & Hardening"
        description = "Fase 6: Testes abrangentes, validação de segurança, performance testing, UAT. Cobertura >80% nos testes unitários. 8 issues."
        due_on      = $Week17End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 8
        duration    = "2 semanas"
    },
    @{
        title       = "Milestone 7: Release & Go-Live"
        description = "Fase 7: Preparação para produção. Build configuration, signing, store submission, go-live. 2 issues."
        due_on      = $Week20End.ToString("yyyy-MM-dd") + "T23:59:59Z"
        issues      = 2
        duration    = "1 semana"
    }
)

# Function to create a milestone
function New-GitHubMilestone {
    param(
        [hashtable]$Milestone,
        [int]$Index
    )
    
    $Uri = "$BaseUrl/repos/$Owner/$Repo/milestones"
    $Body = @{
        title       = $Milestone.title
        description = $Milestone.description
        due_on      = $Milestone.due_on
        state       = "open"
    } | ConvertTo-Json
    
    try {
        Write-Host "[$Index] Criando milestone: $($Milestone.title)..." -ForegroundColor $Colors.Info
        $Response = Invoke-RestMethod -Uri $Uri -Method POST -Headers $Headers -Body $Body
        Write-Host "  ✓ Sucesso!" -ForegroundColor $Colors.Success
        Write-Host "    URL: $($Response.html_url)" -ForegroundColor $Colors.Success
        return $Response
    }
    catch {
        Write-Host "  ✗ Erro ao criar milestone" -ForegroundColor $Colors.Error
        $ErrorMsg = $_.Exception.Response.Content | ConvertFrom-Json | Select-Object -ExpandProperty message
        Write-Host "    Erro: $ErrorMsg" -ForegroundColor $Colors.Error
        throw $_
    }
}

# Main execution
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║       GitHub Milestone Creator - MAUI Migration   ║" -ForegroundColor $Colors.Header
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

# Validate inputs
Write-Host "Validando informações..." -ForegroundColor $Colors.Info
Write-Host "  Owner: $Owner" -ForegroundColor $Colors.Info
Write-Host "  Repo: $Repo" -ForegroundColor $Colors.Info
Write-Host "  Token: $($Token.Substring(0, 10))..." -ForegroundColor $Colors.Info
Write-Host ""

# Create all milestones
$CreatedMilestones = @()
$Index = 1

foreach ($Milestone in $Milestones) {
    try {
        $Created = New-GitHubMilestone -Milestone $Milestone -Index $Index
        $CreatedMilestones += $Created
        Start-Sleep -Milliseconds 500  # Rate limiting
        $Index++
    }
    catch {
        Write-Host ""
        Write-Host "Abortando criação de milestones..." -ForegroundColor $Colors.Error
        exit 1
    }
}

# Summary
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║                     Sumário                        ║" -ForegroundColor $Colors.Header
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""
Write-Host "Total de milestones criadas: $($CreatedMilestones.Count) / $($Milestones.Count)" -ForegroundColor $Colors.Success
Write-Host ""

# Display details
foreach ($Index in 0..($CreatedMilestones.Count - 1)) {
    $Milestone = $CreatedMilestones[$Index]
    $Details = $Milestones[$Index]
    
    Write-Host "[$($Index + 1)] $($Milestone.title)" -ForegroundColor $Colors.Success
    Write-Host "    Duração: $($Details.duration) | Issues: $($Details.issues)" -ForegroundColor $Colors.Info
    Write-Host "    Prazo: $($Milestone.due_on)" -ForegroundColor $Colors.Info
    Write-Host "    Link: $($Milestone.html_url)" -ForegroundColor $Colors.Success
    Write-Host ""
}

Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor $Colors.Header
Write-Host "║    ✓ Todas as milestones foram criadas com êxito! ║" -ForegroundColor $Colors.Success
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor $Colors.Header
Write-Host ""

# Next steps
Write-Host "Próximos passos:" -ForegroundColor $Colors.Info
Write-Host "  1. Execute: .\create-labels.ps1 -Owner '$Owner' -Repo '$Repo' -Token '***'" -ForegroundColor $Colors.Info
Write-Host "  2. Crie as issues usando: docs/GITHUB_ISSUES_READY.md" -ForegroundColor $Colors.Info
Write-Host "  3. Configure o GitHub Projects board" -ForegroundColor $Colors.Info
Write-Host ""

