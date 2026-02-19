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

$Labels = @(
    @{name = "type:migration"; color = "2E8B57"; description = "Tarefa de migracao" },
    @{name = "type:infra"; color = "1E90FF"; description = "Infraestrutura" },
    @{name = "type:ui"; color = "FF69B4"; description = "Interface do usuario" },
    @{name = "type:feature"; color = "32CD32"; description = "Implementacao de feature" },
    @{name = "type:security"; color = "FF0000"; description = "Seguranca" },
    @{name = "type:test"; color = "9370DB"; description = "Testes" },
    @{name = "platform:maui"; color = "6F42C1"; description = "MAUI" },
    @{name = "platform:android"; color = "3DDC84"; description = "Android" },
    @{name = "platform:ios"; color = "000000"; description = "iOS" },
    @{name = "platform:windows"; color = "0078D4"; description = "Windows" },
    @{name = "risk:critical"; color = "B60205"; description = "Risco critico" },
    @{name = "risk:high"; color = "D73A49"; description = "Risco alto" },
    @{name = "risk:medium"; color = "ED6A43"; description = "Risco medio" },
    @{name = "risk:low"; color = "FBCA04"; description = "Risco baixo" },
    @{name = "phase:assessment"; color = "5DADE2"; description = "Assessment" },
    @{name = "phase:setup"; color = "52BE80"; description = "Setup" },
    @{name = "phase:core"; color = "F39C12"; description = "Core" },
    @{name = "phase:infrastructure"; color = "8E44AD"; description = "Infrastructure" },
    @{name = "phase:ui"; color = "E74C3C"; description = "UI" },
    @{name = "phase:feature"; color = "3498DB"; description = "Feature" },
    @{name = "phase:testing"; color = "16A085"; description = "Testing" },
    @{name = "phase:release"; color = "C0392B"; description = "Release" },
    @{name = "priority:P0"; color = "B60205"; description = "P0" },
    @{name = "priority:P1"; color = "D73A49"; description = "P1" },
    @{name = "priority:P2"; color = "F39C12"; description = "P2" },
    @{name = "priority:P3"; color = "FBCA04"; description = "P3" },
    @{name = "component:navigation"; color = "0366D6"; description = "Navigation" },
    @{name = "component:database"; color = "6F42C1"; description = "Database" },
    @{name = "component:authentication"; color = "FF0000"; description = "Authentication" },
    @{name = "component:bluetooth"; color = "2E8B57"; description = "Bluetooth" },
    @{name = "component:maps"; color = "FF6B6B"; description = "Maps" },
    @{name = "component:notifications"; color = "4ECDC4"; description = "Notifications" }
)

Write-Host "Creating labels..." -ForegroundColor Green

$Created = 0
foreach ($Label in $Labels) {
    try {
        $Uri = "$BaseUrl/repos/$Owner/$Repo/labels"
        $Body = @{
            name = $Label.name
            color = $Label.color
            description = $Label.description
        } | ConvertTo-Json
        $Response = Invoke-RestMethod -Uri $Uri -Method POST -Headers $Headers -Body $Body
        Write-Host "OK: $($Label.name)" -ForegroundColor Green
        $Created++
    }
    catch {
        if ($_.Exception.Message -like "*422*" -or $_.Exception.Message -like "*already exists*") {
            Write-Host "SKIP: $($Label.name) (já existe)" -ForegroundColor Yellow
        } else {
            Write-Host "ERROR: $($Label.name)" -ForegroundColor Red
        }
    }
    Start-Sleep -Milliseconds 150
}

Write-Host ""
Write-Host "Created $Created labels" -ForegroundColor Green


