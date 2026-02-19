#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory = $true)]
    [string]$Owner,
    [Parameter(Mandatory = $true)]
    [string]$Repo,
    [Parameter(Mandatory = $true)]
    [string]$Token
)

$ErrorActionPreference = "Stop"
$BaseUrl = "https://api.github.com"
$Headers = @{
    "Authorization" = "Bearer $Token"
    "Accept" = "application/vnd.github+json"
    "X-GitHub-Api-Version" = "2022-11-28"
}

$StartDate = Get-Date -Date "2026-02-24"
$Week1End = $StartDate.AddDays(7)
$Week3End = $Week1End.AddDays(14)
$Week6End = $Week3End.AddDays(21)
$Week12End = $Week6End.AddDays(42)
$Week15End = $Week12End.AddDays(21)
$Week17End = $Week15End.AddDays(14)
$Week20End = $Week17End.AddDays(21)

$Milestones = @(
    @{title = "Milestone 1: Assessment & Planning"; description = "Fase 0: Analise completa. 8 issues."; due_on = $Week1End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 2: MAUI Foundation"; description = "Fase 1: Configuracao MAUI. 12 issues."; due_on = $Week3End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 3: Core & Infrastructure"; description = "Fases 2-3: Migracao core e servicos. 37 issues."; due_on = $Week6End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 4: UI Layer Migration"; description = "Fase 4: Migracao UI. 51 issues."; due_on = $Week12End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 5: Feature Integration"; description = "Fase 5: Integracao features. 24 issues."; due_on = $Week15End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 6: Testing & Hardening"; description = "Fase 6: Testes. 8 issues."; due_on = $Week17End.ToString("yyyy-MM-dd") + "T23:59:59Z" },
    @{title = "Milestone 7: Release & Go-Live"; description = "Fase 7: Release. 2 issues."; due_on = $Week20End.ToString("yyyy-MM-dd") + "T23:59:59Z" }
)

Write-Host "Creating milestones..." -ForegroundColor Green

$Created = 0
foreach ($Milestone in $Milestones) {
    try {
        $Uri = "$BaseUrl/repos/$Owner/$Repo/milestones"
        $Body = $Milestone | ConvertTo-Json
        $Response = Invoke-RestMethod -Uri $Uri -Method POST -Headers $Headers -Body $Body
        Write-Host "OK: $($Milestone.title)" -ForegroundColor Green
        $Created++
    }
    catch {
        if ($_.Exception.Message -like "*422*" -or $_.Exception.Message -like "*Validation Failed*") {
            Write-Host "SKIP: $($Milestone.title) (já existe)" -ForegroundColor Yellow
        } else {
            Write-Host "ERROR: $($Milestone.title)" -ForegroundColor Red
        }
    }
    Start-Sleep -Milliseconds 300
}

Write-Host ""
Write-Host "Created $Created milestones" -ForegroundColor Green


