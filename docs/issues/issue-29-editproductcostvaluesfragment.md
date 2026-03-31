# Issue 29 - Migrar EditProductCostValuesFragment para MAUI

## Meta
- Origem Android: `EditProductCostValuesFragment`
- Destino MAUI: `EditProductCostValuesPage`
- ViewModel: `EditProductCostValuesViewModel`
- Layout atual: `EditProductCostValuesFragment.xml`
- Prioridade: `MÃ‰DIA`
- Estimativa: `2d`
- Status: `PENDENTE`

## Funcionalidades migradas
- [ ] Reproduzir o comportamento funcional da pagina Android.
- [ ] Migrar todos os bindings e comandos do ViewModel.
- [ ] Migrar validacoes, estados de loading e tratamento de erro.
- [ ] Garantir navegacao de entrada e saida equivalente.

## Checklist tecnica MAUI
- [ ] Criar `Views/EditProductCostValuesPage.xaml` e code-behind.
- [ ] Registar rota no Shell/Navigation.
- [ ] Adaptar componentes Android para MAUI (CollectionView, TabbedPage, Map, etc.).
- [ ] Integrar servicos/dependencias necessarias da feature.
- [ ] Validar orientacao landscape quando aplicavel.

## Criterios de aceitacao
- [ ] Paridade funcional com a versao Android.
- [ ] Testado com dados reais e fluxos principais.
- [ ] Sem regressao de navegacao e sem crashes.
- [ ] Documentacao atualizada.
