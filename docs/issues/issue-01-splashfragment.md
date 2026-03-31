# [ISSUE-01] Migrar SplashFragment para MAUI (SplashPage)

Labels: frontend, size: small, phase-1-mvp, priority: high, enhancement, team: frontend

## Overview
Migrar a tela inicial `SplashFragment` (Xamarin.Android) para `SplashPage` (.NET MAUI), mantendo o comportamento de arranque, validacao de sessao e redirecionamento para `LoginPage` ou `HomePage`.

## User Story
As a distribuidor/motorista que inicia a app no comeco do turno (uso diario),
I want abrir a app com carregamento rapido e redirecionamento automatico correto,
So that consigo entrar no fluxo operacional sem atrasos e sem decidir manualmente para onde navegar.

## Context
- Why is this needed? A `Splash` e o primeiro ponto da experiencia; erros aqui bloqueiam 100% da jornada.
- Current workflow: abrir app -> mostrar splash/loading -> validar sessao -> navegar para `Home` (autenticado) ou `Login` (nao autenticado).
- Pain point: risco de regressao na migracao de navegação inicial e validacao de sessao, com impacto direto no inicio do dia.
- Success metric:
  - `Startup routing accuracy >= 99.5%` (navega para destino correto)
  - `Crash-free startup >= 99.9%`
  - `Tempo p95 Splash->Destino <= 2.5s` em dispositivo Android de referencia
- Reference:
  - `docs/plan/flow.md`
  - `docs/plan/migration-reference.md`

## Acceptance Criteria
- [ ] Ao abrir a app sem sessao valida, o utilizador e enviado para `LoginPage`.
- [ ] Ao abrir a app com sessao valida, o utilizador e enviado para `HomePage`.
- [ ] A `SplashPage` exibe estado de loading/animação equivalente ao Android.
- [ ] Se a validacao inicial falhar (erro de servico/timeout), mostrar erro amigavel e opcao de retry.
- [ ] Success = routing accuracy >= 99.5%, crash-free startup >= 99.9%, p95 <= 2.5s.
- [ ] Error case: em falha de bootstrap, nao crashar; manter utilizador informado e permitir recuperacao.

## Technical Requirements
- Technology/framework:
  - .NET MAUI (`ContentPage`) + MVVM com `SplashViewModel` existente
  - Navegacao via Shell (`GoToAsync`) ou padrao oficial definido no projeto MAUI
- Performance:
  - Splash-to-route em <= 2.5s (p95) com sessao local valida
  - Sem bloqueio da UI thread durante inicializacao
- Security:
  - Nao expor credenciais/tokens em logs
  - Leitura segura do estado de autenticacao local
- Accessibility:
  - Indicador visual de loading com contraste adequado
  - Texto alternativo/estado para leitor de ecra quando aplicavel

## Definition of Done
- [ ] `SplashPage` implementada e integrada ao fluxo inicial da app
- [ ] Unit tests escritos com cobertura >= 85% para regras de roteamento da splash
- [ ] Integration tests para cenarios: autenticado, nao autenticado, falha de bootstrap
- [ ] Documentacao atualizada (`docs/issues`, notas de migracao e rotas)
- [ ] Code review aprovado por 1+ reviewer
- [ ] Todos os acceptance criteria validados
- [ ] PR mergeado em `main`

## Dependencies
- Blocked by:
  - Infra de navegacao MAUI definida (Shell/rotas base)
  - Servico de sessao/autenticacao portado para MAUI
- Blocks:
  - `LoginPage` e `HomePage` no fluxo de arranque final
- Related to:
  - `docs/issues/issue-02-loginfragment.md`
  - `docs/issues/issue-03-homefragment.md`

## Estimated Effort
1 dia (size: small), considerando ViewModel existente e escopo focado em UI + roteamento.

## Related Documentation
- Product spec: `docs/plan/migration-reference.md`
- Fluxo de navegacao: `docs/plan/flow.md`
- Design: a validar (nao identificado no repositorio atual)
- Backend API: a validar (depende da estrategia de sessao/token
