# 📊 Полная таблица модулей проекта DnD-Dynamics

| Модуль | Назначение | Ключевые компоненты |
|--------|------------|---------------------|
| **Core/Character** | Реализация бизнес-логики персонажей, расчет характеристик, управление данными персонажа | `CharacterData.cs`, `CharacterStats.cs`, `SerializableCharacterData.cs`, `Inventory.cs`, `Spellbook.cs`, `Skill.cs` |
| **Core/Combat** | Управление боевыми механиками, очередностью ходов, отслеживание инициативы | `CombatSession.cs`, `Combatant.cs`, `CombatService.cs`, `ICombatService.cs`, `DiceRollerService.cs` |
| **Core/Database** | Сериализация и сохранение данных, работа с JSON, управление сохранениями | `SaveData.cs`, `GameDataService.cs`, `IGameDataService.cs`, `DataService.cs`, `IDataService.cs` |
| **Core/Entities** | Модели сущностей игры (расы, классы, предметы, заклинания, монстры) | `CharacterRace.cs`, `CharacterClass.cs`, `Item.cs`, `Spell.cs`, `Monster.cs`, `HandbookEntity.cs` |
| **Core/Homebrew** | Редактирование и создание пользовательского контента, кастомизация правил | `ItemData.cs`, `SpellData.cs`, `SkillData.cs`, DTO-классы для сериализации |
| **Core/Skills** | Управление навыками и умениями персонажей, расчет модификаторов | `SkillManager.cs`, `Skill.cs`, `SkillData.cs` |
| **UI/Character** | Визуальное представление данных персонажа, окна просмотра и редактирования | `CharacterListWindow.cs`, `CharacterDetailWindow.cs`, `CreateCharacterWindow.cs`, `CharacterListItemView.cs` |
| **UI/Combat** | Интерфейс трекера инициативы и боевых действий, лог боя | `CombatTrackerView.cs`, `CombatantItemView.cs`, `DiceRollerView.cs` |
| **UI/Handbook** | Отображение справочника игровых данных (предметы, заклинания, монстры) | `HandbookWindow.cs`, `HandbookCard.cs`, `DMToolsWindow.cs` |
| **UI/MainMenu** | Главное меню навигации по приложению | `MainMenuWindow.cs`, `UIHandler.cs` |
| **Presenters** | Координация между Model и View, обработка событий пользовательского ввода | `BaseCharacterPresenter.cs`, `CharacterListPresenter.cs`, `CharacterDetailPresenter.cs`, `CreateCharacterPresenter.cs`, `HandbookPresenter.cs` |
| **Views (Interfaces)** | Интерфейсы для UI-компонентов в рамках MVP паттерна | `ICharacterListView.cs`, `ICharacterDetailView.cs`, `ICreateCharacterView.cs`, `IHandbookView.cs`, `IBaseView.cs` |
| **Models (MVP)** | Бизнес-модели для MVP паттерна, абстракция данных для Presenter | `CharacterModel.cs`, `HandbookModel.cs` |
| **Installers** | Конфигурация контейнера зависимостей Zenject, инъекция зависимостей | `GlobalInstaller.cs` |
| **Services/Search** | Поиск и фильтрация данных в справочнике и списках | `SearchService.cs`, `ISearchService.cs`, `HandbookFilterService.cs`, `IHandbookFilterService.cs` |
| **Managers/Core** | Глобальные менеджеры управления состоянием игры | `GameManager.cs`, `SkillManager.cs` |
| **Utils** | Утилитарные классы, константы и вспомогательные функции | `Constants.cs` |
| **DTO** | Объекты передачи данных для сериализации и межслойного взаимодействия | `SaveData.cs`, `ItemData.cs`, `SpellData.cs`, `SkillData.cs`, `SerializableCharacterData.cs` |

---

## 📁 Архитектурное разделение по слоям

### 🔵 Core Layer (Бизнес-логика)
| Модуль | Файлы |
|--------|-------|
| Character | `CharacterData.cs`, `CharacterStats.cs`, `Inventory.cs`, `Spellbook.cs` |
| Combat | `CombatSession.cs`, `Combatant.cs`, `DiceRollerService.cs` |
| Database | `GameDataService.cs`, `DataService.cs`, `SaveData.cs` |
| Entities | `CharacterRace.cs`, `CharacterClass.cs`, `Item.cs`, `Spell.cs`, `Monster.cs` |

### 🟢 MVP Layer (Presentation Logic)
| Компонент | Файлы |
|-----------|-------|
| Models | `CharacterModel.cs`, `HandbookModel.cs` |
| Presenters | `BaseCharacterPresenter.cs`, `CharacterListPresenter.cs`, `CharacterDetailPresenter.cs`, `CreateCharacterPresenter.cs`, `HandbookPresenter.cs` |
| Views (Interfaces) | `ICharacterListView.cs`, `ICharacterDetailView.cs`, `ICreateCharacterView.cs`, `IHandbookView.cs`, `IBaseView.cs` |

### 🟡 UI Layer (Unity MonoBehaviour)
| Окно | Файлы |
|------|-------|
| Character List | `CharacterListWindow.cs`, `CharacterListItemView.cs` |
| Character Detail | `CharacterDetailWindow.cs` |
| Create Character | `CreateCharacterWindow.cs` |
| Combat Tracker | `CombatTrackerView.cs`, `CombatantItemView.cs` |
| Dice Roller | `DiceRollerView.cs` |
| Handbook | `HandbookWindow.cs`, `HandbookCard.cs` |
| DM Tools | `DMToolsWindow.cs` |
| Main Menu | `MainMenuWindow.cs` |

### 🔴 Services Layer
| Сервис | Интерфейс | Реализация |
|--------|-----------|------------|
| Combat Service | `ICombatService.cs` | `CombatService.cs` |
| Data Service | `IDataService.cs` | `DataService.cs` |
| Game Data Service | `IGameDataService.cs` | `GameDataService.cs` |
| Search Service | `ISearchService.cs` | `SearchService.cs` |
| Handbook Filter | `IHandbookFilterService.cs` | `HandbookFilterService.cs` |
| Dice Roller | - | `DiceRollerService.cs` |

### 🟣 Infrastructure Layer
| Модуль | Файлы |
|--------|-------|
| Installers | `GlobalInstaller.cs` |
| Managers | `GameManager.cs`, `SkillManager.cs` |
| Utils | `Constants.cs` |

---

## 🏗️ Общая архитектура проекта

```
┌─────────────────────────────────────────────────────────┐
│                    Unity Engine Layer                    │
│  (MonoBehaviour, Prefabs, Scenes, Canvas, UI Elements)  │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                      UI Layer (Views)                    │
│  CharacterListWindow, CharacterDetailWindow, etc.        │
│  Implements: ICharacterListView, ICharacterDetailView    │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                  MVP Presentation Layer                  │
│  Presenters: CharacterListPresenter, HandbookPresenter   │
│  Models: CharacterModel, HandbookModel                   │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                     Core Business Layer                  │
│  CharacterData, CombatSession, Inventory, Spellbook      │
│  Entities: CharacterRace, CharacterClass, Item, Spell    │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                      Services Layer                      │
│  CombatService, DataService, SearchService, DiceRoller   │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                   │
│  Zenject DI Container, GlobalInstaller, GameManager      │
└─────────────────────────────────────────────────────────┘
```

---

## 📈 Статистика проекта

| Категория | Количество |
|-----------|------------|
| **Всего скриптов (.cs)** | 57 |
| **MVP Presenters** | 5 |
| **MVP Views (Interfaces)** | 5 |
| **MVP Models** | 2 |
| **UI Windows** | 9 |
| **Services (Interfaces + Implements)** | 11 |
| **Core Models** | 13 |
| **DTO Classes** | 5 |
| **Entity Classes** | 6 |
| **Installers** | 1 |
| **Managers** | 2 |

---

## 🔑 Паттерны и технологии

| Технология | Назначение |
|------------|------------|
| **MVP (Model-View-Presenter)** | Разделение логики отображения и бизнес-логики |
| **Zenject** | Dependency Injection контейнер для Unity |
| **Service Locator** | Доступ к сервисам через интерфейсы |
| **DTO (Data Transfer Object)** | Сериализация и передача данных между слоями |
| **Factory Pattern** | Создание объектов (персонажи, предметы) |
| **Observer Pattern** | Обработка событий через Presenter-View взаимодействие |
