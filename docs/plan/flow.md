# tabApp.Droid - Análise de Fluxo Completa
## Documentação para Migração para MAUI

---

## 📋 Índice
1. [Visão Geral da Arquitetura](#visão-geral-da-arquitetura)
2. [Estrutura de Navegação](#estrutura-de-navegação)
3. [Páginas e Fragmentos](#páginas-e-fragmentos)
4. [ViewModels e Navegação](#viewmodels-e-navegação)
5. [Adaptadores e ViewHolders](#adaptadores-e-viewholders)
6. [Serviços e Dependências](#serviços-e-dependências)
7. [Plano de Migração](#plano-de-migração)

---

## 🏗️ Visão Geral da Arquitetura

### Estrutura Atual (Android - tabApp.Droid)
- **Framework**: Xamarin.Android com MvvmCross
- **Padrão**: MVVM (Model-View-ViewModel)
- **Navegação**: MvvmCross Navigation Service
- **UI**: Android Fragments com Support Library
- **Binding**: MvvmCross Binding

### Componentes Principais
```
MainActivity (Activity Principal)
├── Navigation Drawer (Menu Lateral)
├── Toolbar (Barra Superior)
├── Fragment Container (Área de Conteúdo)
└── Progress Bar (Indicador de Carregamento)
```

---

## 🗺️ Estrutura de Navegação

### Fluxo Principal de Navegação

```
SplashFragment (Tela Inicial)
    ↓
    ├─→ LoginFragment (Se não autenticado)
    │       ↓
    └─→ HomeFragment (Página Principal)
            ├─→ ClientPageFragment (Detalhes do Cliente)
            │       ├─→ ClientOrderFragment (Encomendas)
            │       ├─→ EditClientFragment (Editar Cliente)
            │       │       ├─→ CreateNotificationsFragment
            │       │       ├─→ SelectDaysPageFragment
            │       │       └─→ ChooseProductFragment
            │       ├─→ DailysOrdersDescFragment (Detalhes Pedidos Diários)
            │       └─→ OtherOptionsFragment (Outras Opções)
            │               ├─→ CreateNoficationFragment
            │               ├─→ ChangeDailyOrderFragment
            │               ├─→ PrintAccountFragment
            │               ├─→ AddStoreRegistFragment
            │               └─→ FaturationFragment
            │
            ├─→ DeleteClientFragment (Eliminar Cliente)
            ├─→ InitDailyFragment (Iniciar Dia)
            └─→ StopDailyFragment (Terminar Dia)
```

### Menu Lateral (Navigation Drawer)

```
Menu Principal
├── Encomenda Global → GlobalOrderFragment
│                           └─→ GlobalOrderSelectDaysFragment
├── Tabela de Preços → PriceTableFragment
│                           ├─→ EditProductFragment
│                           ├─→ EditProductCostValuesFragment
│                           ├─→ PriceTableFilterFragment
│                           ├─→ PriceTableConfigurationFragment
│                           └─→ AddProductFragment
├── Sincronizar → SynchronizeFragment
│                      ├─→ BtOutcomingFragment
│                      └─→ BtIncomingFragment
├── Contas do Mês → MonthBillsHomeFragment
├── Faturação → FaturationHomeFragment
│                    ├─→ FaturationFragment
│                    └─→ TransportationDocumentsFragment
└── Outras Opções → AppOtherOptionsFragment
                         ├─→ ChangePricesFragment
                         ├─→ DatabaseManagerPageFragment
                         ├─→ HomeFinancialsFragment
                         │       ├─→ WeekFinancialsFragment
                         │       └─→ StatsFragment
                         ├─→ NotificationsDashBoardFragment
                         └─→ ReportFragment
```

### Menu Superior (Toolbar)

```
Toolbar
├── Buscar (SearchView) → Filtrar clientes na HomeFragment
├── Cliente Mais Próximo → Usar GPS para encontrar cliente
├── Abrir Porta → Integração com app externo
└── Definições → SettingsFragment
```

---

## 📄 Páginas e Fragmentos

### 1. **Autenticação e Splash**

#### SplashFragment
- **Caminho**: `tabApp/UI/Fragments/Home/SplahFragment.cs`
- **ViewModel**: `SplashViewModel`
- **Layout**: `SplahFragment.xml`
- **Função**: Tela de carregamento inicial com animação GIF
- **Navegação**: 
  - Se autenticado → HomeFragment
  - Se não autenticado → LoginFragment
- **Componentes MAUI**: ContentPage com Image/Animation

#### LoginFragment
- **Caminho**: `tabApp/UI/Fragments/Login/LoginFragment.cs`
- **ViewModel**: `LoginViewModel`
- **Layout**: `LoginFragment.xml`
- **Função**: Autenticação de utilizador
- **Componentes**:
  - EditText (Username)
  - EditText (Password)
  - Button (Login)
- **MAUI Equivalente**: ContentPage com Entry e Button

---

### 2. **Home (Página Principal)**

#### HomeFragment
- **Caminho**: `tabApp/UI/Fragments/Home/HomeFragment.cs`
- **ViewModel**: `HomeViewModel`
- **Layout**: `HomeFragment.xml`
- **Função**: Dashboard principal com lista de clientes e pedidos
- **Componentes**:
  - RecyclerView (Lista de Clientes)
  - ViewPager (Páginas: Pedidos, Mapa, Notificações)
  - TabLayout (Abas do ViewPager)
  - SwipeController (Ações de swipe)
- **Sub-Fragmentos**:
  - **HomePageOrdersFragment**: Lista de pedidos do dia
  - **HomePageMapFragment**: Mapa com localização dos clientes
  - **HomeNotificationsFragment**: Lista de notificações
- **MAUI Equivalente**: ContentPage com CollectionView e TabbedPage

#### InitDailyFragment
- **Caminho**: `tabApp/UI/Fragments/Home/InitDailyFragment.cs`
- **ViewModel**: `InitDailyViewModel`
- **Layout**: `InitDailyFragment.xml`
- **Função**: Iniciar o dia de trabalho
- **MAUI Equivalente**: ContentPage

#### StopDailyFragment
- **Caminho**: `tabApp/UI/Fragments/Home/StopDailyFragment.cs`
- **ViewModel**: `StopDailyViewModel`
- **Layout**: `StopDailyFragment.xml`
- **Função**: Finalizar o dia de trabalho
- **MAUI Equivalente**: ContentPage

#### DeleteClientFragment
- **Caminho**: `tabApp/UI/Fragments/Home/DeleteClientFragment.cs`
- **ViewModel**: `DeleteClientViewModel`
- **Layout**: `DeleteClientFragment.xml`
- **Função**: Eliminar cliente
- **MAUI Equivalente**: ContentPage

---

### 3. **Página do Cliente**

#### ClientPageFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/ClientPageFragment.cs`
- **ViewModel**: `ClientPageViewModel`
- **Layout**: `ClientPageFragment.xml`
- **Função**: Detalhes completos do cliente
- **Componentes**:
  - Informações do Cliente
  - Pedidos da Semana (Seg-Dom + Extra)
  - Botões de Ação:
    - Pagamento
    - Extras
    - Encomendas
    - Editar
    - Outras Opções
- **MAUI Equivalente**: ContentPage com ScrollView

#### ClientOrderFragment
- **Caminho**: `tabApp/UI/Fragments/ClientOrderFragment.cs`
- **ViewModel**: `ClientOrderViewModel`
- **Layout**: `ClientOrderFragment.xml`
- **Função**: Gestão de encomendas do cliente
- **Componentes**:
  - RecyclerView (Lista de Produtos)
  - DatePicker
  - Botões de adicionar/remover produtos
- **MAUI Equivalente**: ContentPage com CollectionView

#### DailysOrdersDescFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/DailysOrdersDescFragment.cs`
- **ViewModel**: `DailysOrdersDescViewModel`
- **Layout**: `DailysOrdersDescFragment.xml`
- **Função**: Descrição detalhada dos pedidos diários
- **MAUI Equivalente**: ContentPage com CollectionView

#### ChooseProductFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/ChooseProductFragment.cs`
- **ViewModel**: `ChooseProductViewModel`
- **Layout**: `ChooseProductFragment.xml`
- **Função**: Seleção de produtos
- **MAUI Equivalente**: ContentPage com CollectionView e SearchBar

---

### 4. **Edição de Cliente**

#### EditClientFragment
- **Caminho**: `tabApp/UI/Fragments/EditClientFragment.cs`
- **ViewModel**: `EditClientViewModel`
- **Layout**: `EditClientFragment.xml`
- **Função**: Editar dados do cliente
- **Componentes**:
  - ViewPager (Múltiplas abas):
    - Perfil
    - Mapa
    - Pedidos Diários
- **Sub-Fragmentos**:
  - **EditClientProfileFragment**: Dados do perfil
  - **EditClientMapFragment**: Localização no mapa
  - **EditDailyOrdersFragment**: Configuração de pedidos diários
- **MAUI Equivalente**: TabbedPage ou CarouselView

#### CreateNotificationsFragment
- **Caminho**: `tabApp/UI/Fragments/EditClient/CreateNotificationsFragment.cs`
- **ViewModel**: `CreateNotificationsViewModel`
- **Layout**: `CreateNotificationsFragment.xml`
- **Função**: Criar notificações para o cliente
- **MAUI Equivalente**: ContentPage

#### SelectDaysPageFragment
- **Caminho**: `tabApp/UI/Fragments/EditClient/SelectDaysPageFragment.cs`
- **ViewModel**: `SelectDaysPageViewModel`
- **Layout**: `SelectDaysPageFragment.xml`
- **Função**: Selecionar dias da semana
- **MAUI Equivalente**: ContentPage com CheckBoxes

---

### 5. **Outras Opções do Cliente**

#### OtherOptionsFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/OtherOptionsFragment.cs`
- **ViewModel**: `OtherOptionsViewModel`
- **Layout**: `OtherOptionsFragment.xml`
- **Função**: Menu de opções adicionais do cliente
- **Opções**:
  - Criar Notificação
  - Alterar Pedido Diário
  - Imprimir Conta
  - Adicionar Registo de Loja
  - Faturação
- **MAUI Equivalente**: ContentPage com CollectionView

#### CreateNoficationFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/OtherOptions/CreateNoficationFragment.cs`
- **ViewModel**: `CreateNoficationViewModel`
- **Layout**: `CreateNoficationFragment.xml`
- **Função**: Criar notificação
- **MAUI Equivalente**: ContentPage

#### ChangeDailyOrderFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/OtherOptions/ChangeDailyOrderFragment.cs`
- **ViewModel**: `ChangeDailyOrderViewModel`
- **Layout**: `ChangeDailyOrderFragment.xml`
- **Função**: Alterar pedido diário
- **MAUI Equivalente**: ContentPage

#### PrintAccountFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/OtherOptions/PrintAccountFragment.cs`
- **ViewModel**: `PrintAccountViewModel`
- **Layout**: `PrintAccountFragment.xml`
- **Função**: Imprimir conta do cliente
- **MAUI Equivalente**: ContentPage

#### AddStoreRegistFragment
- **Caminho**: `tabApp/UI/Fragments/ClientPage/OtherOptions/AddStoreRegistFragment.cs`
- **ViewModel**: `AddStoreRegistViewModel`
- **Layout**: `AddStoreRegistFragment.xml`
- **Função**: Adicionar registo de loja
- **MAUI Equivalente**: ContentPage

---

### 6. **Encomenda Global**

#### GlobalOrderFragment
- **Caminho**: `tabApp/UI/Fragments/Global/GlobalOrderFragment.cs`
- **ViewModel**: `GlobalOrderViewModel`
- **Layout**: `GlobalOrderFragment.xml`
- **Função**: Visualizar pedidos globais
- **MAUI Equivalente**: ContentPage com CollectionView

#### GlobalOrderSelectDaysFragment
- **Caminho**: `tabApp/UI/Fragments/Home/GlobalOrderSelectDaysFragment.cs`
- **ViewModel**: `GlobalOrderSelectDaysViewModel`
- **Layout**: `GlobalOrderSelectDaysFragment.xml`
- **Função**: Selecionar dias para pedido global
- **MAUI Equivalente**: ContentPage

---

### 7. **Tabela de Preços**

#### PriceTableFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/PriceTableFragment.cs`
- **ViewModel**: `PriceTableViewModel`
- **Layout**: `PriceTableFragment.xml`
- **Função**: Gestão de tabela de preços
- **Componentes**:
  - RecyclerView (Lista de Produtos)
  - Botões de filtro e configuração
- **MAUI Equivalente**: ContentPage com CollectionView

#### EditProductFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/EditProductFragment.cs`
- **ViewModel**: `EditProductViewModel`
- **Layout**: `EditProductFragment.xml`
- **Função**: Editar produto
- **MAUI Equivalente**: ContentPage

#### EditProductCostValuesFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/EditProductCostValuesFragment.cs`
- **ViewModel**: `EditProductCostValuesViewModel`
- **Layout**: `EditProductCostValuesFragment.xml`
- **Função**: Editar custos do produto
- **MAUI Equivalente**: ContentPage

#### PriceTableFilterFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/PriceTableFilterFragment.cs`
- **ViewModel**: `PriceTableFilterViewModel`
- **Layout**: `PriceTableFilterFragment.xml`
- **Função**: Filtrar tabela de preços
- **MAUI Equivalente**: ContentPage

#### PriceTableConfigurationFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/PriceTableConfigurationFragment.cs`
- **ViewModel**: `PriceTableConfigurationViewModel`
- **Layout**: `PriceTableConfigurationFragment.xml`
- **Função**: Configurar tabela de preços
- **MAUI Equivalente**: ContentPage

#### AddProductFragment
- **Caminho**: `tabApp/UI/Fragments/Global/PriceTable/AddProductFragment.cs`
- **ViewModel**: `AddProductViewModel`
- **Layout**: `AddProductFragment.xml`
- **Função**: Adicionar novo produto
- **MAUI Equivalente**: ContentPage

---

### 8. **Sincronização (Bluetooth)**

#### SynchronizeFragment
- **Caminho**: `tabApp/UI/Fragments/Global/SynchronizeFragment.cs`
- **ViewModel**: `SynchronizeViewModel`
- **Layout**: `SynchronizeFragment.xml`
- **Função**: Menu de sincronização
- **MAUI Equivalente**: ContentPage

#### BtOutcomingFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Bt/BtOutcomingFragment.cs`
- **ViewModel**: `BtOutcomingViewModel`
- **Layout**: `BtOutcomingFragment.xml`
- **Função**: Enviar dados via Bluetooth
- **MAUI Equivalente**: ContentPage

#### BtIncomingFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Bt/BtIncomingFragment.cs`
- **ViewModel**: `BtIncomingViewModel`
- **Layout**: `BtIncomingFragment.xml`
- **Função**: Receber dados via Bluetooth
- **MAUI Equivalente**: ContentPage

---

### 9. **Faturação**

#### FaturationHomeFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Faturation/FaturationHomeFragment.cs`
- **ViewModel**: `FaturationHomeViewModel`
- **Layout**: `FaturationHomeFragment.xml`
- **Função**: Menu principal de faturação
- **MAUI Equivalente**: ContentPage

#### FaturationFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Faturation/FaturationFragment.cs`
- **ViewModel**: `FaturationViewModel`
- **Layout**: `FaturationFragment.xml`
- **Função**: Gestão de faturação
- **MAUI Equivalente**: ContentPage

#### TransportationDocumentsFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Faturation/TransportationDocumentsFragment.cs`
- **ViewModel**: `TransportationDocumentsViewModel`
- **Layout**: `TransportationDocumentsFragment.xml`
- **Função**: Documentos de transporte
- **MAUI Equivalente**: ContentPage

---

### 10. **Contas do Mês**

#### MonthBillsHomeFragment
- **Caminho**: `tabApp/UI/Fragments/Global/MonthBills/MonthBillsHomeFragment.cs`
- **ViewModel**: `MonthBillsHomeViewModel`
- **Layout**: `MonthBillsHomeFragment.xml`
- **Função**: Visualizar contas mensais
- **MAUI Equivalente**: ContentPage with CollectionView

---

### 11. **Outras Opções Globais**

#### AppOtherOptionsFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/AppOtherOptionsFragment.cs`
- **ViewModel**: `AppOtherOptionsViewModel`
- **Layout**: `AppOtherOptionsFragment.xml`
- **Função**: Menu de opções globais
- **Opções**:
  - Alterar Preços
  - Gestor de Base de Dados
  - Financeiros
  - Notificações
  - Relatório
- **MAUI Equivalente**: ContentPage

#### ChangePricesFragment
- **Caminho**: `tabApp/UI/Fragments/Global/ChangePrices/ChangePricesFragment.cs`
- **ViewModel**: `ChangePricesViewModel`
- **Layout**: `ChangePricesFragment.xml`
- **Função**: Alterar preços globalmente
- **MAUI Equivalente**: ContentPage

#### DatabaseManagerPageFragment
- **Caminho**: `tabApp/UI/Fragments/Global/DatabaseManagerPageFragment.cs`
- **ViewModel**: `DatabaseManagerPageViewModel`
- **Layout**: `DatabaseManagerPageFragment.xml`
- **Função**: Gestão da base de dados
- **MAUI Equivalente**: ContentPage

#### HomeFinancialsFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/Finance/HomeFinancialsFragment.cs`
- **ViewModel**: `HomeFinancialsViewModel`
- **Layout**: `HomeFinancialsFragment.xml`
- **Função**: Dashboard financeiro
- **Sub-páginas**:
  - WeekFinancialsFragment
  - StatsFragment
- **MAUI Equivalente**: ContentPage

#### WeekFinancialsFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/Finance/WeekFinancialsFragment.cs`
- **ViewModel**: `WeekFinancialsViewModel`
- **Layout**: `WeekFinancialsFragment.xml`
- **Função**: Financeiros semanais
- **MAUI Equivalente**: ContentPage

#### StatsFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/Finance/StatsFragment.cs`
- **ViewModel**: `StatsViewModel`
- **Layout**: `StatsFragment.xml`
- **Função**: Estatísticas
- **MAUI Equivalente**: ContentPage

#### NotificationsDashBoardFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/NotificationsDashBoardFragment.cs`
- **ViewModel**: `NotificationsDashBoardViewModel`
- **Layout**: `NotificationsDashBoardFragment.xml`
- **Função**: Dashboard de notificações
- **MAUI Equivalente**: ContentPage

#### ReportFragment
- **Caminho**: `tabApp/UI/Fragments/Global/Other/ReportFragment.cs`
- **ViewModel**: `ReportViewModel`
- **Layout**: `ReportFragment.xml`
- **Função**: Relatórios
- **MAUI Equivalente**: ContentPage

---

### 12. **Definições**

#### SettingsFragment
- **Caminho**: `tabApp/UI/Fragments/Main/SettingsFragment.cs`
- **ViewModel**: `SettingsViewModel`
- **Layout**: `SettingsFragment.xml`
- **Função**: Configurações da aplicação
- **MAUI Equivalente**: ContentPage

---

### 13. **Snooze (Inatividade)**

#### SnoozeFragment
- **Caminho**: `tabApp/UI/Fragments/Snooze/SnoozeFragment.cs`
- **ViewModel**: `SnoozeViewModel`
- **Layout**: `SnoozeFragment.xml`
- **Função**: Tela de bloqueio por inatividade
- **MAUI Equivalente**: ContentPage

---

### 14. **Base e Genéricos**

#### BaseFragment
- **Caminho**: `tabApp/UI/Bases/BaseFragment.cs`
- **Função**: Classe base para todos os fragments
- **MAUI Equivalente**: ContentPage base class

#### BaseOptionsListFragment
- **Caminho**: `tabApp/UI/Fragments/Bases/BaseOptionsListFragment.cs`
- **ViewModel**: `BaseOptionsListViewModel`
- **Função**: Base para listas de opções
- **MAUI Equivalente**: ContentPage base com CollectionView

#### DocumentFragment
- **Caminho**: `tabApp/UI/Fragments/Bases/Generic/DocumentFragment.cs`
- **ViewModel**: `DocumentViewModel`
- **Layout**: `DocumentFragment.xml`
- **Função**: Visualizar documentos
- **MAUI Equivalente**: ContentPage with WebView

---

## 🧩 Adaptadores e ViewHolders

### Adaptadores Principais

#### 1. **Home**
- **ClientsListAdapter**: Lista de clientes
  - ViewHolder: `ClientViewHolder`
  - Layout: `ClientListItem.xml`
  
- **HomePageViewPagerAdapter**: ViewPager da home
  - Fragments: HomePageOrdersFragment, HomePageMapFragment, HomeNotificationsFragment

- **HomePageOrdersAdapter**: Lista de pedidos na home
  - ViewHolder: `HomePageOrderViewHolder`, `HomePageOrderViewHolderAndroidX`
  - Layout: `HomePagerOrderItem.xml`

#### 2. **Cliente**
- **ClientPageAdapter**: Adaptadores da página de cliente
  
- **DailyOrderDescAdapter**: Descrição de pedidos diários
  - ViewHolder: `DailyOrderProductViewHolder`
  - Layout: `DailyOrderProductItem.xml`

#### 3. **Produtos**
- **ProductsItemsListAdapter**: Lista de produtos (selecionáveis)
  - ViewHolder: `SimpleProductViewHolder`
  - Layout: `SimpleProductItem.xml`

- **ProductsAmmountListAdapter**: Lista de produtos com quantidade
  - ViewHolder: `ProductsOrderListViewHolder`
  - Layout: `ProductOrderItem.xml`

#### 4. **Edição de Cliente**
- **EditClientViewPagerAdapter**: ViewPager para edição de cliente
  
- **EditClientProfileItemsAdapter**: Itens do perfil do cliente
  - ViewHolder: `EditProfileItemViewHolder`
  - Layout: `EditProfileItem.xml`

- **EditListSelectableAdapter**: Listas selecionáveis
  - ViewHolder: `EditProfileListItemViewHolder`
  - Layout: `EditProfileListItem.xml`

#### 5. **Encomendas**
- **CakesTotalOrderAdapter**: Encomendas de bolos totais
  - ViewHolder: `IndividualCakeOrderItemViewHolder`
  - Layout: `IndividualCakeOrderItem.xml`

#### 6. **Outras Opções**
- **OtherOptionsAdapter**: Lista de outras opções
  - ViewHolder: Custom ViewHolder
  - Layout: `OtherOptionItem.xml`

#### 7. **Global**
- **GlobalOrdersAdapter**: Pedidos globais
  - ViewHolder: `OrderViewHolder`
  - Layout: `OrderItem.xml`

#### 8. **Tabela de Preços**
- **PriceTableAdapter**: Lista de produtos na tabela de preços
  - ViewHolder: Custom ViewHolder
  - Layout: `PriceTableItem.xml`

#### 9. **Faturação**
- **FaturationAdapter**: Produtos para faturação
  - ViewHolder: Custom ViewHolder
  - Layout: `ProductFatItem.xml`

- **TransportationDocumentsAdapter**: Documentos de transporte
  - ViewHolder: Custom ViewHolder
  - Layout: `LastTrasnportationsDocItem.xml`

#### 10. **Financeiros**
- **HomeFinancialsProductsAdapter**: Produtos financeiros
  - ViewHolder: `HomeFinancialsProductsViewHolder`
  - Layout: Custom

- **WeekFinancialsAdapter**: Financeiros semanais
  - ViewHolder: `WeekFinancialsItemViewHolder`
  - Layout: `WeekFinancialsItem.xml`

#### 11. **Contas do Mês**
- **MonthBillsClientsAdapter**: Clientes com contas mensais
  - ViewHolder: `MonthBillsClientsViewHolder`
  - Layout: `MonthBillsClients.xml`

#### 12. **Notificações**
- **NotificationsAdapter**: Lista de notificações
  - ViewHolder: `NotificationItemViewHolder`
  - Layout: `NotificationItem.xml`

#### 13. **Relatórios**
- **ReportItemsAdapter**: Itens de relatório
  - ViewHolder: `ItemReportViewHolder`, `AddItemReportViewHolder`
  - Layout: `ItemReport.xml`, `AddItemReport.xml`

#### 14. **Configurações**
- **SettingsAdapter**: Itens de configuração
  - ViewHolder: Custom
  - Layout: `SingleChoiceSettingItem.xml`

#### 15. **Componentes Reutilizáveis**
- **SwipeController**: Controlador de swipe actions
- **EmptyListViewHolder**: ViewHolder para listas vazias
- **DetailViewHolder**: ViewHolder para detalhes
- **PrintPreviewViewHolder**: Preview de impressão

---

## 🔄 ViewModels e Navegação

### Mapa Completo de Navegação (ViewModel → ViewModel)

```
MainViewModel (MainActivity)
    ├─→ Navigate<SplashViewModel>
    │       ├─→ Navigate<LoginViewModel>
    │       │       └─→ Navigate<HomeViewModel>
    │       └─→ Navigate<HomeViewModel> (se autenticado)
    │
    ├─→ Navigate<HomeViewModel>
    │       ├─→ Navigate<ClientPageViewModel>
    │       │       ├─→ Navigate<ClientOrderViewModel>
    │       │       │       └─→ Navigate<ChooseProductViewModel>
    │       │       ├─→ Navigate<EditClientViewModel>
    │       │       │       ├─→ Navigate<CreateNotificationsViewModel>
    │       │       │       ├─→ Navigate<SelectDaysPageViewModel>
    │       │       │       └─→ Navigate<ChooseProductViewModel>
    │       │       ├─→ Navigate<DailysOrdersDescViewModel>
    │       │       └─→ Navigate<OtherOptionsViewModel>
    │       │               ├─→ Navigate<CreateNoficationViewModel>
    │       │               ├─→ Navigate<ChangeDailyOrderViewModel>
    │       │               │       ├─→ Navigate<CreateNotificationsViewModel>
    │       │               │       ├─→ Navigate<ChooseProductViewModel>
    │       │               │       └─→ Navigate<SelectDaysPageViewModel>
    │       │               ├─→ Navigate<PrintAccountViewModel>
    │       │               ├─→ Navigate<AddStoreRegistViewModel>
    │       │               │       └─→ Navigate<ChooseProductViewModel>
    │       │               └─→ Navigate<FaturationViewModel>
    │       │
    │       ├─→ Navigate<DeleteClientViewModel>
    │       ├─→ Navigate<InitDailyViewModel> / Navigate<StopDailyViewModel>
    │       └─→ Navigate<EditClientViewModel> (novo cliente)
    │
    ├─→ Navigate<GlobalOrderViewModel>
    │       └─→ Navigate<GlobalOrderSelectDaysViewModel>
    │
    ├─→ Navigate<PriceTableViewModel>
    │       ├─→ Navigate<EditProductViewModel>
    │       ├─→ Navigate<EditProductCostValuesViewModel>
    │       ├─→ Navigate<PriceTableFilterViewModel>
    │       └─→ Navigate<PriceTableConfigurationViewModel>
    │               └─→ Navigate<AddProductViewModel>
    │
    ├─→ Navigate<SynchronizeViewModel>
    │       ├─→ Navigate<BtOutcomingViewModel>
    │       └─→ Navigate<BtIncomingViewModel>
    │
    ├─→ Navigate<MonthBillsHomeViewModel>
    │
    ├─→ Navigate<FaturationHomeViewModel>
    │       ├─→ Navigate<FaturationViewModel>
    │       └─→ Navigate<TransportationDocumentsViewModel>
    │
    ├─→ Navigate<AppOtherOptionsViewModel>
    │       ├─→ Navigate<ChangePricesViewModel>
    │       ├─→ Navigate<DatabaseManagerPageViewModel>
    │       ├─→ Navigate<HomeFinancialsViewModel>
    │       │       ├─→ Navigate<WeekFinancialsViewModel>
    │       │       └─→ Navigate<StatsViewModel>
    │       ├─→ Navigate<NotificationsDashBoardViewModel>
    │       └─→ Navigate<ReportViewModel>
    │               └─→ Navigate<ChooseProductViewModel>
    │
    └─→ Navigate<SettingsViewModel>
```

---

## 🔧 Serviços e Dependências

### Serviços Core (Compartilhados)

#### Database & Storage
- **IDataBaseManagerService** / **DataBaseManagerService**
- **ISQLiteService** / **SQLiteService**
- **IFileService** / **FileService**

#### Clientes
- **IClientsManagerService** / **ClientsManagerService**
- **IChooseClientService** / **ChooseClientService**
- **IClientsListFilterService** / **ClientsListFilterService**

#### Produtos
- **IProductsManagerService** / **ProductsManagerService**

#### Pedidos
- **IOrdersManagerService** / **OrdersManagerService**

#### Notificações
- **INotificationsManagerService** / **NotificationsManagerService**

#### Outros
- **IDialogService** / **DialogService** (Platform-specific)
- **IGetSpinnerDatesService** / **GetSpinnerDatesService**
- **IAmmountToPayService** / **AmmountToPayService**
- **IInativityTimerService** / **InativityTimerService**
- **IDeliverysManagerService** / **DeliverysManagerService**

### Serviços Específicos Android

#### Native Services
- **IBluetoothService** / **BluetoothService**
- **ForegroundService** (Serviço em foreground)
- **NotificationHelper** (Notificações nativas)
- **PrinterHelper** (Impressão térmica)

#### Helpers
- **ImageHelper**
- **LoadingPopPupHelper**
- **MyWebViewClient**
- **Downloader**
- **FragmentHelper**
- **DecimalDigitsInputFilter**
- **StringHelper**

---

## 📱 Componentes UI Específicos

### Componentes Android → MAUI

| Android Component | MAUI Equivalent |
|-------------------|-----------------|
| Fragment | ContentPage |
| RecyclerView | CollectionView |
| ViewPager | CarouselView / TabbedPage |
| TabLayout | TabbedPage |
| DrawerLayout | FlyoutPage |
| NavigationView | FlyoutPage |
| Toolbar | NavigationPage |
| EditText | Entry |
| TextView | Label |
| Button | Button |
| ImageView | Image |
| ProgressBar | ActivityIndicator / ProgressBar |
| SearchView | SearchBar |
| WebView | WebView |
| MapView | Map (Microsoft.Maui.Controls.Maps) |
| SwipeController | SwipeView |
| AlertDialog | DisplayAlert / Custom Popup |
| Spinner | Picker |
| CheckBox | CheckBox |
| RadioButton | RadioButton |

---

## 🗂️ Layouts XML → XAML

### Estrutura de Conversão

Cada layout XML Android precisa ser convertido para XAML MAUI. Exemplo:

**Android XML** (`HomeFragment.xml`):
```xml
<LinearLayout>
    <RecyclerView android:id="@+id/clientsList" />
    <ViewPager android:id="@+id/homeViewPager" />
    <TabLayout android:id="@+id/tabLayout" />
</LinearLayout>
```

**MAUI XAML** (`HomePage.xaml`):
```xaml
<ContentPage>
    <VerticalStackLayout>
        <CollectionView x:Name="clientsList" />
        <TabbedPage x:Name="homeViewPager" />
    </VerticalStackLayout>
</ContentPage>
```

---

## 📋 Plano de Migração

### Fase 1: Infraestrutura Base (Semanas 1-2)
✅ **Prioridade: ALTA**

1. **Estrutura de Projeto MAUI**
   - Configurar projeto MAUI
   - Migrar ViewModels base (já em `tabApp.Core`)
   - Configurar serviços de navegação (MvvmCross → MAUI Shell ou NavigationPage)

2. **Serviços Core**
   - Migrar serviços de `tabApp.Core` para MAUI
   - Adaptar `ISQLiteService` para MAUI
   - Migrar `IDialogService` para alerts/popups MAUI
   - Implementar serviços de arquivo e storage

3. **Modelos de Dados**
   - Verificar compatibilidade dos models
   - Ajustar se necessário

### Fase 2: Autenticação e Navegação Básica (Semanas 3-4)
✅ **Prioridade: ALTA**

4. **SplashPage**
   - Criar `SplashPage.xaml`
   - Migrar lógica de `SplashViewModel`
   - Implementar animação de loading

5. **LoginPage**
   - Criar `LoginPage.xaml`
   - Migrar `LoginViewModel`
   - Implementar validação e autenticação

6. **Navegação Shell**
   - Configurar MAUI Shell
   - Definir rotas de navegação
   - Implementar FlyoutPage (equivalente ao Drawer)

### Fase 3: Home e Dashboard (Semanas 5-7)
✅ **Prioridade: ALTA**

7. **HomePage**
   - Criar `HomePage.xaml`
   - Migrar `HomeViewModel`
   - Implementar CollectionView para lista de clientes
   - Criar TabbedPage para pedidos/mapa/notificações
   - Implementar SwipeView para ações

8. **Sub-páginas Home**
   - `HomePageOrdersPage` (lista de pedidos)
   - `HomePageMapPage` (mapa com pins)
   - `HomeNotificationsPage` (notificações)

9. **Iniciar/Parar Dia**
   - `InitDailyPage`
   - `StopDailyPage`

### Fase 4: Página do Cliente (Semanas 8-10)
✅ **Prioridade: ALTA**

10. **ClientPage**
    - Criar `ClientPage.xaml`
    - Migrar `ClientPageViewModel`
    - Implementar visualização de pedidos da semana
    - Adicionar botões de ação

11. **ClientOrderPage**
    - Criar `ClientOrderPage.xaml`
    - Migrar `ClientOrderViewModel`
    - Implementar seleção de produtos

12. **DailysOrdersDescPage**
    - Criar `DailysOrdersDescPage.xaml`
    - Migrar `DailysOrdersDescViewModel`

13. **ChooseProductPage**
    - Criar `ChooseProductPage.xaml`
    - Migrar `ChooseProductViewModel`
    - Implementar pesquisa e filtro

### Fase 5: Edição de Cliente (Semanas 11-13)
✅ **Prioridade: MÉDIA-ALTA**

14. **EditClientPage** (TabbedPage)
    - Criar estrutura de abas
    - Migrar `EditClientViewModel`

15. **Sub-páginas de Edição**
    - `EditClientProfilePage`
    - `EditClientMapPage`
    - `EditDailyOrdersPage`

16. **Páginas Auxiliares**
    - `CreateNotificationsPage`
    - `SelectDaysPage`

17. **DeleteClientPage**
    - Criar `DeleteClientPage.xaml`
    - Migrar `DeleteClientViewModel`

### Fase 6: Outras Opções do Cliente (Semanas 14-15)
✅ **Prioridade: MÉDIA**

18. **OtherOptionsPage**
    - Criar menu de opções
    - Migrar `OtherOptionsViewModel`

19. **Sub-opções**
    - `CreateNotificationPage`
    - `ChangeDailyOrderPage`
    - `PrintAccountPage`
    - `AddStoreRegistPage`

### Fase 7: Encomenda Global (Semana 16)
✅ **Prioridade: MÉDIA**

20. **GlobalOrderPage**
    - Criar `GlobalOrderPage.xaml`
    - Migrar `GlobalOrderViewModel`

21. **GlobalOrderSelectDaysPage**
    - Criar `GlobalOrderSelectDaysPage.xaml`
    - Migrar `GlobalOrderSelectDaysViewModel`

### Fase 8: Tabela de Preços (Semanas 17-18)
✅ **Prioridade: MÉDIA**

22. **PriceTablePage**
    - Criar `PriceTablePage.xaml`
    - Migrar `PriceTableViewModel`
    - Implementar lista de produtos

23. **Sub-páginas**
    - `EditProductPage`
    - `EditProductCostValuesPage`
    - `PriceTableFilterPage`
    - `PriceTableConfigurationPage`
    - `AddProductPage`

### Fase 9: Sincronização Bluetooth (Semanas 19-20)
✅ **Prioridade: MÉDIA-BAIXA**

24. **SynchronizePage**
    - Criar `SynchronizePage.xaml`
    - Migrar `SynchronizeViewModel`

25. **Bluetooth**
    - Implementar serviço Bluetooth para MAUI
    - `BtOutcomingPage`
    - `BtIncomingPage`

### Fase 10: Faturação (Semanas 21-22)
✅ **Prioridade: MÉDIA**

26. **FaturationHomePage**
    - Criar `FaturationHomePage.xaml`
    - Migrar `FaturationHomeViewModel`

27. **Sub-páginas**
    - `FaturationPage`
    - `TransportationDocumentsPage`

### Fase 11: Contas do Mês (Semana 23)
✅ **Prioridade: BAIXA**

28. **MonthBillsHomePage**
    - Criar `MonthBillsHomePage.xaml`
    - Migrar `MonthBillsHomeViewModel`

### Fase 12: Outras Opções Globais (Semanas 24-26)
✅ **Prioridade: BAIXA-MÉDIA**

29. **AppOtherOptionsPage**
    - Criar menu de opções globais
    - Migrar `AppOtherOptionsViewModel`

30. **Sub-páginas**
    - `ChangePricesPage`
    - `DatabaseManagerPage`
    - `HomeFinancialsPage`
      - `WeekFinancialsPage`
      - `StatsPage`
    - `NotificationsDashBoardPage`
    - `ReportPage`

### Fase 13: Configurações (Semana 27)
✅ **Prioridade: BAIXA**

31. **SettingsPage**
    - Criar `SettingsPage.xaml`
    - Migrar `SettingsViewModel`
    - Implementar preferências

### Fase 14: Snooze e Funcionalidades Extras (Semana 28)
✅ **Prioridade: BAIXA**

32. **SnoozePage**
    - Criar `SnoozePage.xaml`
    - Migrar `SnoozeViewModel`
    - Implementar timer de inatividade

33. **Serviço Foreground**
    - Adaptar para MAUI (Background tasks)

### Fase 15: Testes e Polimento (Semanas 29-32)
✅ **Prioridade: CRÍTICA**

34. **Testes**
    - Testar navegação completa
    - Testar todas as funcionalidades
    - Testar em diferentes dispositivos
    - Testar orientação landscape

35. **Otimizações**
    - Performance
    - Consumo de bateria
    - Tamanho da app

36. **Documentação**
    - Atualizar documentação técnica
    - Manual de utilizador

---

## 📊 Resumo de Páginas por Prioridade

### 🔴 ALTA Prioridade (Funcionalidades Core)
- SplashPage
- LoginPage
- HomePage (+ sub-páginas)
- ClientPage
- ClientOrderPage
- EditClientPage (+ sub-páginas)
- ChooseProductPage
- DeleteClientPage
- InitDailyPage / StopDailyPage

**Total: ~15 páginas**

### 🟡 MÉDIA Prioridade (Funcionalidades Importantes)
- OtherOptionsPage (+ sub-páginas)
- GlobalOrderPage
- PriceTablePage (+ sub-páginas)
- FaturationPage (+ sub-páginas)

**Total: ~15 páginas**

### 🟢 BAIXA Prioridade (Funcionalidades Secundárias)
- SynchronizePage (+ Bluetooth)
- MonthBillsPage
- AppOtherOptionsPage (+ sub-páginas)
- SettingsPage
- SnoozePage

**Total: ~15 páginas**

---

## 🎯 Próximos Passos Imediatos

### 1. Começar pela Fase 1
   - ✅ Configurar projeto MAUI base
   - ✅ Verificar compatibilidade dos ViewModels
   - ✅ Migrar serviços críticos

### 2. Implementar MVP (Minimum Viable Product)
   - Splash → Login → Home → ClientPage
   - Funcionalidade básica de visualizar e editar clientes
   - Pedidos básicos

### 3. Iteração Progressiva
   - Adicionar páginas por ordem de prioridade
   - Testar cada módulo antes de avançar
   - Coletar feedback dos utilizadores

---

## 📝 Notas Importantes

### Diferenças Críticas Android → MAUI

1. **Navegação**
   - Android: FragmentManager + MvvmCross
   - MAUI: Shell Navigation ou NavigationPage

2. **Lifecycle**
   - Android: OnCreate, OnResume, OnPause, OnDestroy
   - MAUI: OnAppearing, OnDisappearing

3. **Binding**
   - Android: MvvmCross Binding
   - MAUI: XAML Binding nativo

4. **Listas**
   - Android: RecyclerView + Adapter + ViewHolder
   - MAUI: CollectionView + DataTemplate

5. **Orientação**
   - Garantir que LANDSCAPE mode está configurado corretamente

6. **Permissões**
   - Migrar system de permissões Android para MAUI

7. **Serviços Nativos**
   - Bluetooth, GPS, Impressora → Implementar com Dependency Injection

---

## 🔗 Referências e Recursos

### Documentação
- [MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [MAUI Shell](https://docs.microsoft.com/dotnet/maui/fundamentals/shell/)
- [MAUI Navigation](https://docs.microsoft.com/dotnet/maui/fundamentals/shell/navigation)

### Migração
- [Xamarin.Android to MAUI Migration Guide](https://docs.microsoft.com/dotnet/maui/migration/)

### Componentes
- [MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)
- [MAUI Maps](https://docs.microsoft.com/dotnet/maui/user-interface/controls/map)

---

## ✅ Checklist por Página

Usar esta checklist para cada página durante a migração:

- [ ] Layout XAML criado
- [ ] ViewModel migrado e funcional
- [ ] Navegação implementada (entrada e saída)
- [ ] Binding de dados implementado
- [ ] Comandos e eventos funcionais
- [ ] Adaptado para LANDSCAPE
- [ ] Testado em Android
- [ ] Testado em iOS (se aplicável)
- [ ] Documentação atualizada

---

**Documento criado em**: 2026-03-04  
**Última atualização**: 2026-03-04  
**Versão**: 1.0  
**Autor**: Análise Automática do Projeto tabApp.Droid

