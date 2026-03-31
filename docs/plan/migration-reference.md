# Referência Rápida de Migração - tabApp.Droid para MAUI

## 📊 Tabela Completa de Páginas

| # | Fragment/Page | ViewModel | Layout XML | Prioridade | Estimativa | Status |
|---|---------------|-----------|------------|------------|------------|--------|
| 1 | SplashFragment | SplashViewModel | SplahFragment.xml | ALTA | 1d | ⬜ |
| 2 | LoginFragment | LoginViewModel | LoginFragment.xml | ALTA | 2d | ⬜ |
| 3 | HomeFragment | HomeViewModel | HomeFragment.xml | ALTA | 5d | ⬜ |
| 4 | HomePageOrdersFragment | HomeViewModel | HomePageOrdersFragment.xml | ALTA | 2d | ⬜ |
| 5 | HomePageMapFragment | HomeViewModel | HomePageMapFragment.xml | ALTA | 3d | ⬜ |
| 6 | HomeNotificationsFragment | HomeViewModel | - | ALTA | 2d | ⬜ |
| 7 | InitDailyFragment | InitDailyViewModel | InitDailyFragment.xml | ALTA | 1d | ⬜ |
| 8 | StopDailyFragment | StopDailyViewModel | StopDailyFragment.xml | ALTA | 1d | ⬜ |
| 9 | DeleteClientFragment | DeleteClientViewModel | DeleteClientFragment.xml | ALTA | 1d | ⬜ |
| 10 | ClientPageFragment | ClientPageViewModel | ClientPageFragment.xml | ALTA | 4d | ⬜ |
| 11 | ClientOrderFragment | ClientOrderViewModel | ClientOrderFragment.xml | ALTA | 3d | ⬜ |
| 12 | DailysOrdersDescFragment | DailysOrdersDescViewModel | DailysOrdersDescFragment.xml | ALTA | 2d | ⬜ |
| 13 | ChooseProductFragment | ChooseProductViewModel | ChooseProductFragment.xml | ALTA | 3d | ⬜ |
| 14 | EditClientFragment | EditClientViewModel | EditClientFragment.xml | ALTA | 4d | ⬜ |
| 15 | EditClientProfileFragment | EditClientViewModel | - | ALTA | 2d | ⬜ |
| 16 | EditClientMapFragment | EditClientViewModel | MapViewEditClient.xml | ALTA | 3d | ⬜ |
| 17 | EditDailyOrdersFragment | EditClientViewModel | - | ALTA | 3d | ⬜ |
| 18 | CreateNotificationsFragment | CreateNotificationsViewModel | CreateNotificationsFragment.xml | MÉDIA | 2d | ⬜ |
| 19 | SelectDaysPageFragment | SelectDaysPageViewModel | SelectDaysPageFragment.xml | MÉDIA | 1d | ⬜ |
| 20 | OtherOptionsFragment | OtherOptionsViewModel | OtherOptionsFragment.xml | MÉDIA | 2d | ⬜ |
| 21 | CreateNoficationFragment | CreateNoficationViewModel | CreateNoficationFragment.xml | MÉDIA | 2d | ⬜ |
| 22 | ChangeDailyOrderFragment | ChangeDailyOrderViewModel | ChangeDailyOrderFragment.xml | MÉDIA | 3d | ⬜ |
| 23 | PrintAccountFragment | PrintAccountViewModel | PrintAccountFragment.xml | MÉDIA | 2d | ⬜ |
| 24 | AddStoreRegistFragment | AddStoreRegistViewModel | AddStoreRegistFragment.xml | MÉDIA | 2d | ⬜ |
| 25 | GlobalOrderFragment | GlobalOrderViewModel | GlobalOrderFragment.xml | MÉDIA | 2d | ⬜ |
| 26 | GlobalOrderSelectDaysFragment | GlobalOrderSelectDaysViewModel | GlobalOrderSelectDaysFragment.xml | MÉDIA | 1d | ⬜ |
| 27 | PriceTableFragment | PriceTableViewModel | PriceTableFragment.xml | MÉDIA | 3d | ⬜ |
| 28 | EditProductFragment | EditProductViewModel | EditProductFragment.xml | MÉDIA | 2d | ⬜ |
| 29 | EditProductCostValuesFragment | EditProductCostValuesViewModel | EditProductCostValuesFragment.xml | MÉDIA | 2d | ⬜ |
| 30 | PriceTableFilterFragment | PriceTableFilterViewModel | PriceTableFilterFragment.xml | MÉDIA | 1d | ⬜ |
| 31 | PriceTableConfigurationFragment | PriceTableConfigurationViewModel | - | MÉDIA | 1d | ⬜ |
| 32 | AddProductFragment | AddProductViewModel | AddProductFragment.xml | MÉDIA | 2d | ⬜ |
| 33 | FaturationHomeFragment | FaturationHomeViewModel | FaturationHomeFragment.xml | MÉDIA | 2d | ⬜ |
| 34 | FaturationFragment | FaturationViewModel | FaturationFragment.xml | MÉDIA | 3d | ⬜ |
| 35 | TransportationDocumentsFragment | TransportationDocumentsViewModel | TransportationDocumentsFragment.xml | MÉDIA | 2d | ⬜ |
| 36 | SynchronizeFragment | SynchronizeViewModel | SynchronizeFragment.xml | BAIXA | 1d | ⬜ |
| 37 | BtOutcomingFragment | BtOutcomingViewModel | BtOutcomingFragment.xml | BAIXA | 3d | ⬜ |
| 38 | BtIncomingFragment | BtIncomingViewModel | BtIncomingFragment.xml | BAIXA | 3d | ⬜ |
| 39 | MonthBillsHomeFragment | MonthBillsHomeViewModel | MonthBillsHomeFragment.xml | BAIXA | 2d | ⬜ |
| 40 | AppOtherOptionsFragment | AppOtherOptionsViewModel | AppOtherOptionsFragment.xml | BAIXA | 2d | ⬜ |
| 41 | ChangePricesFragment | ChangePricesViewModel | ChangePricesFragment.xml | BAIXA | 2d | ⬜ |
| 42 | DatabaseManagerPageFragment | DatabaseManagerPageViewModel | DatabaseManagerPageFragment.xml | BAIXA | 2d | ⬜ |
| 43 | HomeFinancialsFragment | HomeFinancialsViewModel | HomeFinancialsFragment.xml | BAIXA | 2d | ⬜ |
| 44 | WeekFinancialsFragment | WeekFinancialsViewModel | WeekFinancialsFragment.xml | BAIXA | 2d | ⬜ |
| 45 | StatsFragment | StatsViewModel | StatsFragment.xml | BAIXA | 2d | ⬜ |
| 46 | NotificationsDashBoardFragment | NotificationsDashBoardViewModel | NotificationsDashBoardFragment.xml | BAIXA | 2d | ⬜ |
| 47 | ReportFragment | ReportViewModel | ReportFragment.xml | BAIXA | 2d | ⬜ |
| 48 | SettingsFragment | SettingsViewModel | SettingsFragment.xml | BAIXA | 2d | ⬜ |
| 49 | SnoozeFragment | SnoozeViewModel | SnoozeFragment.xml | BAIXA | 1d | ⬜ |
| 50 | DocumentFragment | DocumentViewModel | DocumentFragment.xml | BAIXA | 1d | ⬜ |

**Total de Páginas**: 50  
**Estimativa Total**: ~105 dias de trabalho  
**Estimativa com equipa de 2**: ~52 dias  
**Estimativa com equipa de 3**: ~35 dias

---

## 🎯 Ordem de Migração Sugerida

### Sprint 1 - Fundação (2 semanas)
1. ✅ Infraestrutura MAUI
2. ✅ SplashPage
3. ✅ LoginPage
4. ✅ Navegação base (Shell/NavigationPage)

### Sprint 2 - Home Básico (2 semanas)
5. ✅ HomePage (lista de clientes)
6. ✅ HomePageOrdersPage
7. ✅ ClientPageFragment (básico)

### Sprint 3 - Cliente Completo (2 semanas)
8. ✅ ClientPageFragment (completo)
9. ✅ ClientOrderFragment
10. ✅ ChooseProductFragment
11. ✅ DailysOrdersDescFragment

### Sprint 4 - Edição de Cliente (2 semanas)
12. ✅ EditClientFragment (todas as tabs)
13. ✅ CreateNotificationsFragment
14. ✅ SelectDaysPageFragment

### Sprint 5 - Funcionalidades Diárias (1 semana)
15. ✅ InitDailyFragment
16. ✅ StopDailyFragment
17. ✅ DeleteClientFragment

### Sprint 6 - Outras Opções Cliente (2 semanas)
18. ✅ OtherOptionsFragment
19. ✅ CreateNoficationFragment
20. ✅ ChangeDailyOrderFragment
21. ✅ PrintAccountFragment
22. ✅ AddStoreRegistFragment

### Sprint 7 - Encomenda Global (1 semana)
23. ✅ GlobalOrderFragment
24. ✅ GlobalOrderSelectDaysFragment

### Sprint 8 - Tabela de Preços (2 semanas)
25. ✅ PriceTableFragment (+ todas sub-páginas)

### Sprint 9 - Mapa e Notificações (1 semana)
26. ✅ HomePageMapFragment
27. ✅ HomeNotificationsFragment

### Sprint 10 - Faturação (2 semanas)
28. ✅ FaturationHomeFragment
29. ✅ FaturationFragment
30. ✅ TransportationDocumentsFragment

### Sprint 11 - Financeiros (2 semanas)
31. ✅ AppOtherOptionsFragment
32. ✅ HomeFinancialsFragment
33. ✅ WeekFinancialsFragment
34. ✅ StatsFragment

### Sprint 12 - Outras Funcionalidades (2 semanas)
35. ✅ MonthBillsHomeFragment
36. ✅ ChangePricesFragment
37. ✅ DatabaseManagerPageFragment
38. ✅ NotificationsDashBoardFragment
39. ✅ ReportFragment

### Sprint 13 - Sincronização (2 semanas)
40. ✅ SynchronizeFragment
41. ✅ BtOutcomingFragment
42. ✅ BtIncomingFragment

### Sprint 14 - Extras (1 semana)
43. ✅ SettingsFragment
44. ✅ SnoozeFragment
45. ✅ DocumentFragment

### Sprint 15 - Testes e Polimento (2 semanas)
46. ✅ Testes completos
47. ✅ Correção de bugs
48. ✅ Otimizações de performance

**Total**: ~26 semanas (~6 meses)

---

## 📋 Tabela de Adaptadores

| Adapter Android | Tipo | MAUI Equivalent | Componentes |
|-----------------|------|-----------------|-------------|
| ClientsListAdapter | RecyclerView | CollectionView | DataTemplate |
| HomePageViewPagerAdapter | ViewPager | TabbedPage / CarouselView | ContentPage |
| HomePageOrdersAdapter | RecyclerView | CollectionView | DataTemplate |
| DailyOrderDescAdapter | RecyclerView | CollectionView | DataTemplate |
| ProductsItemsListAdapter | RecyclerView | CollectionView | DataTemplate |
| ProductsAmmountListAdapter | RecyclerView | CollectionView | DataTemplate |
| EditClientViewPagerAdapter | ViewPager | TabbedPage | ContentPage |
| EditClientProfileItemsAdapter | RecyclerView | CollectionView | DataTemplate |
| EditListSelectableAdapter | RecyclerView | CollectionView | DataTemplate |
| CakesTotalOrderAdapter | RecyclerView | CollectionView | DataTemplate |
| OtherOptionsAdapter | RecyclerView | CollectionView | DataTemplate |
| GlobalOrdersAdapter | RecyclerView | CollectionView | DataTemplate |
| PriceTableAdapter | RecyclerView | CollectionView | DataTemplate |
| FaturationAdapter | RecyclerView | CollectionView | DataTemplate |
| TransportationDocumentsAdapter | RecyclerView | CollectionView | DataTemplate |
| HomeFinancialsProductsAdapter | RecyclerView | CollectionView | DataTemplate |
| WeekFinancialsAdapter | RecyclerView | CollectionView | DataTemplate |
| MonthBillsClientsAdapter | RecyclerView | CollectionView | DataTemplate |
| NotificationsAdapter | RecyclerView | CollectionView | DataTemplate |
| ReportItemsAdapter | RecyclerView | CollectionView | DataTemplate |
| SettingsAdapter | RecyclerView | CollectionView | DataTemplate |
| SwipeController | ItemTouchHelper | SwipeView | SwipeItems |

---

## 🔧 Tabela de Serviços

| Serviço | Interface | Plataforma | Migração MAUI |
|---------|-----------|------------|---------------|
| DataBaseManagerService | IDataBaseManagerService | Core | ✅ Mantém |
| SQLiteService | ISQLiteService | Android | 🔄 Adaptar para MAUI |
| FileService | IFileService | Android | 🔄 Usar FileSystem MAUI |
| DialogService | IDialogService | Android | 🔄 Usar DisplayAlert/Popup |
| ClientsManagerService | IClientsManagerService | Core | ✅ Mantém |
| ChooseClientService | IChooseClientService | Core | ✅ Mantém |
| ClientsListFilterService | IClientsListFilterService | Core | ✅ Mantém |
| ProductsManagerService | IProductsManagerService | Core | ✅ Mantém |
| OrdersManagerService | IOrdersManagerService | Core | ✅ Mantém |
| NotificationsManagerService | INotificationsManagerService | Core | ✅ Mantém |
| BluetoothService | IBluetoothService | Android | 🔄 Reimplementar para MAUI |
| GetSpinnerDatesService | IGetSpinnerDatesService | Core | ✅ Mantém |
| AmmountToPayService | IAmmountToPayService | Core | ✅ Mantém |
| InativityTimerService | IInativityTimerService | Core | ✅ Mantém |
| DeliverysManagerService | IDeliverysManagerService | Core | ✅ Mantém |
| ForegroundService | - | Android | 🔄 Background tasks MAUI |
| NotificationHelper | - | Android | 🔄 Notifications MAUI |
| PrinterHelper | - | Android | 🔄 Plugin/Native |
| ImageHelper | - | Android | 🔄 MAUI Image handling |

**Legenda**:
- ✅ Mantém: Já está em Core, não precisa migração
- 🔄 Adaptar: Precisa ser adaptado para MAUI

---

## 🎨 Conversão de Componentes UI

### Layout Components

| Android XML | MAUI XAML | Notas |
|-------------|-----------|-------|
| `<LinearLayout orientation="vertical">` | `<VerticalStackLayout>` | Layout vertical |
| `<LinearLayout orientation="horizontal">` | `<HorizontalStackLayout>` | Layout horizontal |
| `<RelativeLayout>` | `<Grid>` ou `<AbsoluteLayout>` | Posicionamento relativo |
| `<ConstraintLayout>` | `<Grid>` | Layout com constraints |
| `<FrameLayout>` | `<Grid>` ou `<AbsoluteLayout>` | Container simples |
| `<ScrollView>` | `<ScrollView>` | ✅ Mesmo nome |
| `<RecyclerView>` | `<CollectionView>` | Listas |
| `<ListView>` | `<ListView>` ou `<CollectionView>` | Listas simples |
| `<GridView>` | `<CollectionView>` com `ItemsLayout` | Grid de itens |
| `<ViewPager>` | `<CarouselView>` | Páginas deslizantes |
| `<TabLayout>` | `<TabbedPage>` | Abas |
| `<DrawerLayout>` | `<FlyoutPage>` | Menu lateral |
| `<CoordinatorLayout>` | `<Grid>` com behaviors | Layout complexo |
| `<CardView>` | `<Frame>` | Cards |

### Input Controls

| Android XML | MAUI XAML | Notas |
|-------------|-----------|-------|
| `<EditText>` | `<Entry>` | Campo de texto simples |
| `<EditText multiline>` | `<Editor>` | Campo de texto multilinha |
| `<TextView>` | `<Label>` | Texto estático |
| `<Button>` | `<Button>` | ✅ Mesmo conceito |
| `<ImageButton>` | `<ImageButton>` | ✅ Mesmo conceito |
| `<CheckBox>` | `<CheckBox>` | ✅ Mesmo conceito |
| `<RadioButton>` | `<RadioButton>` | ✅ Mesmo conceito |
| `<Switch>` | `<Switch>` | ✅ Mesmo conceito |
| `<Spinner>` | `<Picker>` | Dropdown |
| `<SeekBar>` | `<Slider>` | Slider |
| `<ProgressBar>` | `<ActivityIndicator>` ou `<ProgressBar>` | Indicadores |
| `<RatingBar>` | Custom ou Plugin | Avaliação |
| `<SearchView>` | `<SearchBar>` | Busca |
| `<DatePicker>` | `<DatePicker>` | ✅ Mesmo conceito |
| `<TimePicker>` | `<TimePicker>` | ✅ Mesmo conceito |

### Media & Display

| Android XML | MAUI XAML | Notas |
|-------------|-----------|-------|
| `<ImageView>` | `<Image>` | Imagens |
| `<VideoView>` | `<MediaElement>` | Vídeos (Community Toolkit) |
| `<WebView>` | `<WebView>` | ✅ Mesmo conceito |
| `<MapView>` | `<Map>` | Mapas (Microsoft.Maui.Controls.Maps) |

### Special

| Android | MAUI | Notas |
|---------|------|-------|
| `AlertDialog` | `DisplayAlert()` ou Popup | Diálogos |
| `Toast` | `DisplayAlert()` ou Snackbar | Mensagens |
| `Snackbar` | Community Toolkit Snackbar | Mensagens temporárias |
| `FloatingActionButton` | Custom Button | Botão flutuante |
| `NavigationView` | `FlyoutPage` | Menu lateral |
| `Toolbar` | `NavigationPage.TitleView` | Barra superior |
| `SwipeRefreshLayout` | `RefreshView` | Pull to refresh |
| `ItemTouchHelper` (swipe) | `<SwipeView>` | Ações de swipe |

---

## 📐 Padrões de Código

### Navegação

**Android (MvvmCross)**:
```csharp
await _navigationService.Navigate<ClientPageViewModel>();
```

**MAUI (Shell)**:
```csharp
await Shell.Current.GoToAsync("clientpage");
// ou
await Shell.Current.GoToAsync(nameof(ClientPage));
```

**MAUI (NavigationPage)**:
```csharp
await Navigation.PushAsync(new ClientPage());
```

### Binding

**Android (MvvmCross)**:
```xml
<TextView
    mvx:MvxBind="Text ClientName" />
```

**MAUI**:
```xml
<Label Text="{Binding ClientName}" />
```

### Listas

**Android (RecyclerView)**:
```csharp
// Adapter
public class ClientsListAdapter : RecyclerView.Adapter
{
    // ViewHolder pattern
}

// Fragment
_recyclerView.SetAdapter(new ClientsListAdapter(items));
```

**MAUI (CollectionView)**:
```xml
<CollectionView ItemsSource="{Binding Clients}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Label Text="{Binding Name}" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

### Comandos

**Android (MvvmCross)**:
```csharp
_button.Click += (s, e) => ViewModel.Command.Execute();
```

**MAUI**:
```xml
<Button Text="Click" Command="{Binding Command}" />
```

---

## 🔍 Pontos de Atenção Críticos

### 1. ⚠️ Orientação Landscape
```xml
<!-- Android -->
[Activity(ScreenOrientation = ScreenOrientation.Landscape)]

<!-- MAUI -->
// MainPage.xaml.cs
#if ANDROID
    Platform.CurrentActivity.RequestedOrientation = 
        Android.Content.PM.ScreenOrientation.Landscape;
#endif
```

### 2. ⚠️ Bluetooth
- Android: BluetoothAdapter nativo
- MAUI: Usar Plugin.BLE ou implementar native binding

### 3. ⚠️ Impressão Térmica
- Android: PrinterHelper custom
- MAUI: Manter implementação nativa Android via Dependency Injection

### 4. ⚠️ GPS/Localização
- Android: LocationManager
- MAUI: `Microsoft.Maui.Devices.Sensors.Geolocation`

### 5. ⚠️ Notificações
- Android: NotificationHelper
- MAUI: Plugin.LocalNotification

### 6. ⚠️ Foreground Service
- Android: ForegroundService
- MAUI: Background tasks (limitado no iOS)

### 7. ⚠️ SQLite
- Android: SQLiteService custom
- MAUI: SQLite-net-pcl (compatível)

### 8. ⚠️ Arquivo e Storage
- Android: File system Android
- MAUI: `Microsoft.Maui.Storage.FileSystem`

### 9. ⚠️ Mapas
- Android: Google Maps
- MAUI: `Microsoft.Maui.Controls.Maps` (usa Google Maps no Android)

### 10. ⚠️ ViewPager com Swipe
- Android: ViewPager2
- MAUI: CarouselView ou TabbedPage (diferentes behaviors)

---

## 📦 Packages NuGet Necessários

```xml
<!-- MAUI Core -->
<PackageReference Include="Microsoft.Maui.Controls" />
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" />

<!-- Maps -->
<PackageReference Include="Microsoft.Maui.Controls.Maps" />

<!-- Database -->
<PackageReference Include="sqlite-net-pcl" />
<PackageReference Include="SQLitePCLRaw.bundle_green" />

<!-- Community Toolkit -->
<PackageReference Include="CommunityToolkit.Maui" />
<PackageReference Include="CommunityToolkit.Mvvm" />

<!-- Bluetooth -->
<PackageReference Include="Plugin.BLE" />

<!-- Notificações -->
<PackageReference Include="Plugin.LocalNotification" />

<!-- Permissions -->
<PackageReference Include="Microsoft.Maui.Essentials" />

<!-- Imagens -->
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" />

<!-- Optional -->
<PackageReference Include="Newtonsoft.Json" />
```

---

## ✅ Template de Checklist para Cada Página

```markdown
## [Nome da Página] - Status

### Desenvolvimento
- [ ] XAML criado
- [ ] Code-behind criado
- [ ] ViewModel verificado/migrado
- [ ] Navegação implementada (entrada)
- [ ] Navegação implementada (saída/back)
- [ ] Registrado no Shell/Routing

### UI/UX
- [ ] Layout responsive implementado
- [ ] Landscape mode configurado
- [ ] Binding de dados implementado
- [ ] Comandos conectados
- [ ] Validações implementadas
- [ ] Loading states implementados
- [ ] Error handling implementado

### Funcionalidades
- [ ] Todas as features do Android implementadas
- [ ] Serviços integrados
- [ ] Dados salvos/carregados corretamente
- [ ] Navegação entre páginas funcional

### Testes
- [ ] Testado em Android
- [ ] Testado em iOS (se aplicável)
- [ ] Testado em diferentes tamanhos de tela
- [ ] Testado em modo landscape
- [ ] Testado com dados reais
- [ ] Testado edge cases

### Performance
- [ ] Sem memory leaks
- [ ] Performance aceitável
- [ ] Sem crashes

### Documentação
- [ ] Código comentado
- [ ] Documentação atualizada
- [ ] Notas de migração adicionadas
```

---

## 📞 Contactos e Recursos

### Documentação Oficial
- MAUI: https://docs.microsoft.com/dotnet/maui/
- Community Toolkit: https://docs.microsoft.com/dotnet/communitytoolkit/

### Plugins Úteis
- https://github.com/jamesmontemagno/Xamarin.Plugins
- https://github.com/CommunityToolkit/Maui

### Exemplos
- https://github.com/dotnet/maui-samples

---

**Última Atualização**: 2026-03-04  
**Versão**: 1.0

