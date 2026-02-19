# 🚀 GitHub Scripts - MAUI Migration Setup

**Localização:** `docs/scripts/`  
**Status:** ✅ Pronto para usar

---

## 📋 Scripts Disponíveis

### 1. `create-milestones.ps1`
Cria as 7 milestones do projeto no GitHub

### 2. `create-labels.ps1`
Cria todos os labels para categorização de issues

---

## 🔐 Passo 1: Obter Token de Acesso do GitHub

### Opção A: Interface Web (Recomendado)

1. Acesse: https://github.com/settings/tokens
2. Clique em **"Generate new token"** → **"Generate new token (classic)"**
3. Preencha:
   - **Token name:** `MAUI Migration Setup`
   - **Expiration:** 90 days (recomendado)
   - **Scopes:** Marque ✓ `repo` (Controle total de repositórios)
4. Clique em **"Generate token"**
5. **COPIE** o token imediatamente (não será mostrado novamente!)

### Opção B: GitHub CLI

```powershell
gh auth login
# Siga os prompts de autenticação
```

---

## 🖥️ Como Executar os Scripts

### Pré-requisitos
- PowerShell 5.0+
- Acesso ao repositório no GitHub
- Token de acesso com scope `repo`

### Executar Script de Milestones

```powershell
# Navegue até a pasta do projeto
cd "C:\Users\flipper09112\Documents\GestorApp"

# Execute o script
.\docs\scripts\create-milestones.ps1 -Owner "seu-usuario" -Repo "GestorApp" -Token "ghp_seu_token"
```

**Substitua:**
- `seu-usuario` → Seu usuário GitHub ou nome da organização
- `ghp_seu_token` → Seu token do passo anterior

### Executar Script de Labels

```powershell
.\docs\scripts\create-labels.ps1 -Owner "seu-usuario" -Repo "GestorApp" -Token "ghp_seu_token"
```

### Ambos os Scripts (Sequência Recomendada)

```powershell
$Owner = "seu-usuario"
$Repo = "GestorApp"
$Token = "ghp_seu_token"

Write-Host "Criando milestones..." -ForegroundColor Green
.\docs\scripts\create-milestones.ps1 -Owner $Owner -Repo $Repo -Token $Token

Write-Host "Criando labels..." -ForegroundColor Green
.\docs\scripts\create-labels.ps1 -Owner $Owner -Repo $Repo -Token $Token

Write-Host "Concluído!" -ForegroundColor Green
```

---

## 📊 O Que Será Criado

### Milestones (7)
1. **Milestone 1:** Assessment & Planning (1 semana)
2. **Milestone 2:** MAUI Foundation (2 semanas)
3. **Milestone 3:** Core & Infrastructure (3 semanas)
4. **Milestone 4:** UI Layer Migration (6 semanas)
5. **Milestone 5:** Feature Integration (3 semanas)
6. **Milestone 6:** Testing & Hardening (2 semanas)
7. **Milestone 7:** Release & Go-Live (1 semana)

### Labels (31 no total)
- **Type:** 6 labels (migration, infra, ui, feature, security, test)
- **Platform:** 4 labels (maui, android, ios, windows)
- **Risk:** 4 labels (critical, high, medium, low)
- **Phase:** 8 labels (assessment, setup, core, infrastructure, ui, feature, testing, release)
- **Priority:** 4 labels (P0, P1, P2, P3)
- **Component:** 6 labels (navigation, database, authentication, bluetooth, maps, notifications)

---

## ✅ Exemplo de Execução

```powershell
PS C:\Users\flipper09112\Documents\GestorApp> .\docs\scripts\create-milestones.ps1 -Owner "flipper09112" -Repo "GestorApp" -Token "ghp_xxxxx"

╔════════════════════════════════════════════════════╗
║       GitHub Milestone Creator - MAUI Migration   ║
╚════════════════════════════════════════════════════╝

Validando informações...
  Owner: flipper09112
  Repo: GestorApp
  Token: ghp_xxxxx...

[1] Criando milestone: Milestone 1: Assessment & Planning...
  ✓ Sucesso!
    URL: https://github.com/flipper09112/GestorApp/milestone/1

[2] Criando milestone: Milestone 2: MAUI Foundation...
  ✓ Sucesso!
    URL: https://github.com/flipper09112/GestorApp/milestone/2

... (5 milestones mais)

╔════════════════════════════════════════════════════╗
║                     Sumário                        ║
╚════════════════════════════════════════════════════╝

Total de milestones criadas: 7 / 7
✓ Todas as milestones foram criadas com êxito!
```

---

## 🔍 Verificar no GitHub

Após executar os scripts:

1. Acesse seu repositório: `https://github.com/seu-usuario/GestorApp`
2. Clique em **"Issues"** no menu esquerdo
3. Clique em **"Milestones"** (canto superior direito)
4. Você verá as 7 milestones com prazos

Para ver os labels:
1. Clique em **"Labels"** ao lado de Milestones
2. Você verá todos os 31 labels organizados por categoria

---

## ❌ Troubleshooting

### "Command not found"
```
O termo '.\docs\scripts\create-milestones.ps1' não é reconhecido
```

**Solução:** Navegue até a pasta correta:
```powershell
cd "C:\Users\flipper09112\Documents\GestorApp"
```

### "Execution policy error"
```
O PowerShell não pode ser carregado porque scripts estão desabilitados
```

**Solução:** Altere a política de execução temporariamente:
```powershell
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
```

### "Authentication failed" (401)
```
✗ Erro ao criar milestone
  Erro: 401 Unauthorized
```

**Soluções:**
1. Verifique se o token está correto e não expirou
2. Token precisa do scope `repo`
3. Para repositórios de organização, pode precisar de permissões adicionais

### "Repository not found" (404)
```
✗ Erro ao criar milestone
  Erro: 404 Not Found
```

**Soluções:**
1. Verifique o nome do repositório (case-sensitive)
2. Verifique se você tem acesso ao repositório
3. Para repositórios privados, o token precisa do scope `repo`

---

## 🔒 Segurança

### Proteção do Token
- ⚠️ **NUNCA** commit do token no git
- ⚠️ **NUNCA** compartilhe o token publicamente
- ✅ Use variáveis de ambiente ou arquivo `.env` (git ignored)
- ✅ Rotacionem tokens periodicamente
- ✅ Deletem tokens quando não precisarem

### Uso Seguro
```powershell
# Criar arquivo .github-token (adicione ao .gitignore)
# Conteúdo: seu_token_aqui

$token = Get-Content .github-token
$owner = "seu-usuario"
$repo = "GestorApp"

.\docs\scripts\create-milestones.ps1 -Owner $owner -Repo $repo -Token $token
```

---

## 📝 Próximos Passos

Após criar milestones e labels:

### 1. Criar Issues
Use o arquivo `docs/GITHUB_ISSUES_READY.md` para criar as 142 issues manualmente ou com script.

### 2. Configurar GitHub Projects Board
1. Vá para **Projects** tab
2. Crie novo projeto (Board view)
3. Adicione as issues como cards
4. Organize por milestone

### 3. Atribuir Issues às Milestones
1. Abra cada issue
2. Selecione a milestone na dropdown
3. Salve

---

## 📚 Estrutura de Pastas

```
docs/
├── scripts/
│   ├── create-milestones.ps1  ← Script de milestones
│   ├── create-labels.ps1      ← Script de labels
│   └── README.md              ← Este arquivo
├── issues/
│   ├── PHASE_0_ASSESSMENT.md
│   ├── PHASE_1_MAUI_SETUP.md
│   └── ...
└── ...
```

---

## 💬 Dúvidas?

Referências:
- [GitHub API Docs - Milestones](https://docs.github.com/en/rest/issues/milestones)
- [GitHub API Docs - Labels](https://docs.github.com/en/rest/issues/labels)
- [PowerShell Docs](https://learn.microsoft.com/en-us/powershell/)

---

**Versão:** 1.0  
**Última atualização:** 2026-02-19  
**Status:** ✅ Pronto para usar


