# 📊 Диаграмма Структуры Проекта DnD-Dynamics

## 🏗️ Общая Архитектура

```
/workspace (Unity Project)
│
├── 📁 Assets/                          # Основные ресурсы проекта
│   ├── 📁 DnD-Dynamics/                # Основной контент игры
│   │   ├── 📁 Fonts/                   # Шрифты (.ttf, .SDF)
│   │   ├── 📁 Prefabs/                 # Префабы
│   │   │   ├── Managers/               # Префабы менеджеров
│   │   │   └── Windows/                # Префабы окон UI
│   │   ├── 📁 Resources/               # Ресурсы Unity
│   │   │   └── Data/                   # Данные конфигурации
│   │   ├── 📁 Scenes/                  # Сцены Unity
│   │   ├── 📁 Scripts/                 # Исходный код C#
│   │   │   ├── MVP/                    # Архитектура Model-View-Presenter
│   │   │   │   ├── Models/             # Модели данных
│   │   │   │   ├── Presenters/         # Презентеры (логика)
│   │   │   │   └── Views/              # Представления (UI компоненты)
│   │   │   ├── Managers/               # Менеджеры системы
│   │   │   │   └── Installers/         # Zenject установщики
│   │   │   ├── Models/                 # Игровые модели
│   │   │   │   ├── Combat/             # Боевая система
│   │   │   │   ├── DTO/                # Data Transfer Objects
│   │   │   │   └── Entities/           # Сущности игры
│   │   │   ├── Services/               # Сервисы
│   │   │   │   ├── Implements/         # Реализации сервисов
│   │   │   │   └── Interfaces/         # Интерфейсы сервисов
│   │   │   ├── UI/                     # Пользовательский интерфейс
│   │   │   │   └── Windows/            # Окна UI
│   │   │   └── Utils/                  # Утилиты и помощники
│   │   └── 📁 Sprites/                 # Графические ресурсы
│   │       ├── Dark Blue/              # Тема "Тёмно-синий"
│   │       │   ├── Controls/           # Элементы управления
│   │       │   │   ├── Buttons/        # Кнопки
│   │       │   │   ├── Input Field/    # Поля ввода
│   │       │   │   ├── Select Field/   # Выпадающие списки
│   │       │   │   ├── Sliders/        # Ползунки
│   │       │   │   └── Toggles/        # Переключатели
│   │       │   ├── Cursors/            # Курсоры
│   │       │   ├── HUD/                # Heads-Up Display
│   │       │   │   ├── Action Bar/     # Панель действий
│   │       │   │   ├── Cast Bars/      # Полосы заклинаний
│   │       │   │   ├── Chat/           # Чат
│   │       │   │   ├── Minimap/        # Миникарта
│   │       │   │   ├── Nameplate/      # Имена персонажей
│   │       │   │   ├── Notifications/  # Уведомления
│   │       │   │   ├── Quest Tracker/  # Трекер заданий
│   │       │   │   ├── Tooltip/        # Подсказки
│   │       │   │   └── Unit Frames/    # Рамки юнитов
│   │       │   ├── Loading Bar/        # Полоса загрузки
│   │       │   ├── Lobby/              # Лобби
│   │       │   │   ├── Character Create/   # Создание персонажа
│   │       │   │   ├── Character Select/   # Выбор персонажа
│   │       │   │   └── Hero Select/        # Выбор героя
│   │       │   ├── Miscellaneous/      # Разное
│   │       │   ├── Mobile/             # Мобильные элементы
│   │       │   ├── Modal Box/          # Модальные окна
│   │       │   ├── Spell & Item Icons/ # Иконки заклинаний и предметов
│   │       │   ├── Text Boxes/         # Текстовые поля
│   │       │   └── Windows/            # Окна
│   │       │       ├── Character/      # Окно персонажа
│   │       │       ├── Inventory/      # Инвентарь
│   │       │       ├── Spellbook/      # Книга заклинаний
│   │       │       ├── Talents/        # Таланты
│   │       │       └── Vendor/         # Торговец
│   │       └── Dark Yellow/            # Тема "Тёмно-жёлтый"
│   │           ├── Action Bar/         # Панель действий
│   │           ├── Buttons/            # Кнопки
│   │           ├── Cast Bars/          # Полосы заклинаний
│   │           ├── Character Create/   # Создание персонажа
│   │           ├── Character Select/   # Выбор персонажа
│   │           ├── Character Window/   # Окно персонажа
│   │           ├── Chat/               # Чат
│   │           ├── Dialog/             # Диалоги
│   │           ├── Inventory/          # Инвентарь
│   │           ├── Minimap/            # Миникарта
│   │           ├── Spell Book/         # Книга заклинаний
│   │           ├── Unit Frames/        # Рамки юнитов
│   │           └── Window/             # Окна
│   │
│   ├── 📁 Plugins/                     # Сторонние плагины
│   │   └── 📁 Zenject/                 # Dependency Injection Framework
│   │       ├── OptionalExtras/         # Дополнительные модули
│   │       │   ├── IntegrationTests/   # Интеграционные тесты
│   │       │   ├── MemoryPoolMonitor/  # Монитор пулов памяти
│   │       │   ├── ReflectionBaking/   # Оптимизация рефлексии
│   │       │   ├── SampleGame1/        # Пример игры (Beginner)
│   │       │   ├── SampleGame2/        # Пример игры (Advanced)
│   │       │   ├── Signals/            # Система событий
│   │       │   ├── TestFramework/      # Фреймворк тестирования
│   │       │   └── UnitTests/          # Юнит-тесты
│   │       └── Source/                 # Исходный код Zenject
│   │           ├── Binding/            # Привязки DI
│   │           ├── Editor/             # Редакторские скрипты
│   │           ├── Factories/          # Фабрики объектов
│   │           ├── Injection/          # Внедрение зависимостей
│   │           ├── Install/            # Установка контекстов
│   │           ├── Providers/          # Провайдеры
│   │           ├── Runtime/            # Рантайм компоненты
│   │           ├── Usage/              # Использование
│   │           ├── Util/               # Утилиты
│   │           └── Validation/         # Валидация
│   │
│   ├── 📁 Settings/                    # Настройки проекта
│   │   ├── Default Input/              # Настройки ввода
│   │   ├── Render/                     # Настройки рендеринга
│   │   └── Scenes/                     # Настройки сцен
│   │
│   └── 📁 TextMesh Pro/                # Библиотека текста
│       ├── Fonts/                      # Шрифты TMP
│       ├── Resources/                  # Ресурсы TMP
│       ├── Shaders/                    # Шейдеры TMP
│       └── Sprites/                    # Спрайты TMP
│
├── 📁 ProjectSettings/                 # Настройки проекта Unity
│   └── Packages/                       # Настройки пакетов
│       └── com.unity.dedicated-server/ # Настройки сервера
│
├── 📁 Packages/                        # Unity пакеты (manifest.json)
│
├── 📁 UIElementsSchema/                # Схемы UI Elements
│
└── 📄 README.md                        # Документация проекта
```

## 🎯 Ключевые Компоненты

### 1. Архитектура MVP
```
┌─────────────────────────────────────────┐
│              MVP Pattern                │
├─────────────────────────────────────────┤
│  Model ↔ Presenter ↔ View               │
│                                         │
│  • Models: Данные и бизнес-логика       │
│  • Presenters: Обработка событий        │
│  • Views: Отображение UI                │
└─────────────────────────────────────────┘
```

### 2. Dependency Injection (Zenject)
```
┌─────────────────────────────────────────┐
│           Zenject DI Container          │
├─────────────────────────────────────────┤
│  • Installers: Конфигурация зависимостей│
│  • Factories: Создание объектов         │
│  • Signals: Система событий             │
│  • Memory Pools: Оптимизация памяти     │
└─────────────────────────────────────────┘
```

### 3. UI Система
```
┌─────────────────────────────────────────┐
│            UI Components                │
├─────────────────────────────────────────┤
│  • MainMenuWindow                       │
│  • CharacterListWindow                  │
│  • CharacterDetailWindow                │
│  • CombatTrackerView                    │
│  • DiceRollerView                       │
│  • DMToolsWindow                        │
└─────────────────────────────────────────┘
```

## 📦 Основные Модули

| Модуль | Описание | Расположение |
|--------|----------|--------------|
| **MVP** | Архитектурный паттерн | `Assets/DnD-Dynamics/Scripts/MVP/` |
| **Managers** | Системные менеджеры | `Assets/DnD-Dynamics/Scripts/Managers/` |
| **Models** | Игровые модели | `Assets/DnD-Dynamics/Scripts/Models/` |
| **Services** | Сервисы (Interfaces/Implements) | `Assets/DnD-Dynamics/Scripts/Services/` |
| **UI** | Пользовательский интерфейс | `Assets/DnD-Dynamics/Scripts/UI/` |
| **Utils** | Утилиты | `Assets/DnD-Dynamics/Scripts/Utils/` |

## 🎨 Графические Ресурсы

- **2 темы оформления**: Dark Blue, Dark Yellow
- **~3500 файлов** в проекте
- **Основные категории спрайтов**:
  - Controls (кнопки, поля ввода, переключатели)
  - HUD (панели, миникарта, чат, уведомления)
  - Windows (окна персонажа, инвентаря, заклинаний)
  - Lobby (создание/выбор персонажа)
  - Mobile (мобильные элементы управления)

## 🔧 Технические Детали

- **Движок**: Unity
- **DI Framework**: Zenject
- **Архитектура**: MVP (Model-View-Presenter)
- **UI System**: Unity UI / UI Toolkit
- **Text Rendering**: TextMesh Pro
- **Пакеты**: Unity Dedicated Server support

---
*Диаграмма сгенерирована на основе анализа файловой структуры проекта*
