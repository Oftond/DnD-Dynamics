# 📐 Дополнение к описанию архитектуры проекта DnD-Dynamics

## 🔵 Слой Services (Сервисы)

Сервисы предоставляют интерфейс для работы с данными, боевой системой, поиском и фильтрацией. Все сервисы объявлены через интерфейсы для поддержки зависимостей через Zenject.

### IDataService
**Файл:** `Assets/DnD-Dynamics/Scripts/Services/Interfaces/IDataService.cs`

**Назначение:** Центральный сервис для управления всеми данными справочника и персонажей. Предоставляет CRUD-операции для заклинаний, предметов, монстров, рас, классов и навыков.

**Ключевые методы:**
- `Task<List<Spell>> GetSpellsAsync()` – загрузка списка заклинаний
- `Spell GetSpellById(string id)` – получение заклинания по ID
- `Task AddSpellAsync(Spell spell)` / `UpdateSpellAsync()` / `DeleteSpellAsync()` – CRUD операции
- `Task<List<Item>> GetItemsAsync()` – загрузка предметов
- `Task<List<Monster>> GetMonstersAsync()` – загрузка монстров
- `Task<List<CharacterRace>> GetRacesAsync()` – загрузка рас
- `Task<List<CharacterClass>> GetClassesAsync()` – загрузка классов
- `Task<List<SkillData>> GetSkillsAsync()` – загрузка навыков
- `List<SkillData> GetSkillsByAbility(CharacterAbility ability)` – фильтрация навыков по характеристике
- `Task ToggleFavoriteAsync(string id, HandbookCategory category)` – переключение избранного
- `Task SaveCharactersAsync(List<CharacterData> characters)` – сохранение персонажей
- `Task<List<CharacterData>> GetCharactersAsync()` – загрузка персонажей
- `Task DeleteCharacter(string characterId)` – удаление персонажа
- `bool HasSavedCharacters()` – проверка наличия сохранений
- `Task PreloadAllAsync()` – предварительная загрузка всех данных

**События:**
- `event Action OnSpellsLoaded` – уведомление о загрузке заклинаний
- `event Action OnItemsLoaded` – уведомление о загрузке предметов
- `event Action OnMonstersLoaded` – уведомление о загрузке монстров
- `event Action OnRacesLoaded` – уведомление о загрузке рас
- `event Action OnClassesLoaded` – уведомление о загрузке классов

---

### IGameDataService
**Файл:** `Assets/DnD-Dynamics/Scripts/Services/Interfaces/IGameDataService.cs`

**Назначение:** Сервис для загрузки статических игровых данных из JSON-файлов. Работает с DTO-объектами для сериализации.

**Ключевые методы:**
- `List<SkillData> LoadSkills()` – загрузка навыков из JSON
- `List<SpellData> LoadSpells()` – загрузка заклинаний из JSON
- `List<ItemData> LoadItems()` – загрузка предметов из JSON
- `SkillData GetSkillById(string id)` – поиск навыка по ID
- `SpellData GetSpellById(string id)` – поиск заклинания по ID
- `ItemData GetItemById(string id)` – поиск предмета по ID
- `List<SkillData> GetSkillsByAbility(CharacterAbility ability)` – фильтрация по характеристике
- `List<SpellData> GetSpellsByLevel(int level)` – фильтрация по уровню заклинания
- `List<ItemData> GetItemsByType(string type)` – фильтрация по типу предмета

---

### ICombatService
**Файл:** `Assets/DnD-Dynamics/Scripts/Services/Interfaces/ICombatService.cs`

**Назначение:** Управление боевыми сессиями, ходом боя, инициативой и состоянием бойцов.

**Ключевые методы:**
- `Task<CombatSession> GetCurrentSessionAsync()` – получение текущей боевой сессии
- `Task SaveSessionAsync(CombatSession session)` – сохранение сессии
- `Task ClearSessionAsync()` – очистка сессии
- `Task<bool> HasActiveSession()` – проверка активной сессии
- `Task AddCombatantAsync(Combatant combatant)` – добавление бойца
- `Task UpdateCombatantAsync(Combatant combatant)` – обновление бойца
- `Task RemoveCombatantAsync(string id)` – удаление бойца
- `Task ApplyDamageToCombatantAsync(string id, int amount)` – нанесение урона
- `Task ApplyHealToCombatantAsync(string id, int amount)` – лечение
- `Task RollInitiativeForCombatantAsync(string id, int bonus = 0)` – бросок инициативы для бойца
- `Task RollInitiativeForAllAsync(int bonus = 0)` – бросок инициативы для всех
- `Task NextTurnAsync()` – переход к следующему ходу
- `Task PreviousTurnAsync()` – возврат к предыдущему ходу

---

### ISearchService
**Файл:** `Assets/DnD-Dynamics/Scripts/Services/Interfaces/ISearchService.cs`

**Назначение:** Поиск элементов справочника по различным критериям.

**Ключевые методы:**
- `List<Item> SearchByName(List<Item> items, string query)` – поиск по названию
- `List<Item> SearchByDescription(List<Item> items, string query)` – поиск по описанию
- `List<Item> SearchByKeyword(List<Item> items, string keyword)` – поиск по ключевому слову

---

### IHandbookFilterService
**Файл:** `Assets/DnD-Dynamics/Scripts/Services/Interfaces/IHandbookFilterService.cs`

**Назначение:** Фильтрация элементов справочника по параметрам (уровень, школа магии, редкость, тип, CR и т.д.).

**Ключевые методы:**
- `List<Spell> FilterSpells(List<Spell> spells, int? level, SpellSchool? school, string requiredClassId)` – фильтрация заклинаний
- `List<Item> FilterItems(List<Item> items, List<ItemRarity> rarities, List<ItemType> types)` – фильтрация предметов
- `List<Monster> FilterMonsters(List<Monster> monsters, float? minCr, float? maxCr, MonsterType? type, MonsterSize? size)` – фильтрация монстров
- `List<T> SearchByName<T>(List<T> items, string query) where T : HandbookEntity` – универсальный поиск по названию

---

## 🟢 Слой Models (MVP Модели)

Модели в архитектуре MVP выступают посредниками между бизнес-логикой (Core) и Presenters. Они инкапсулируют состояние и предоставляют методы для работы с данными.

### CharacterModel
**Файл:** `Assets/DnD-Dynamics/Scripts/MVP/Models/CharacterModel.cs`

**Назначение:** Управление коллекцией персонажей, их создание, редактирование, удаление и сохранение.

**Зависимости:**
- `IDataService _dataService` – сервис данных

**События:**
- `event Action<List<CharacterUIData>> OnCharactersChanged` – изменение списка персонажей
- `event Action<CharacterUIData> OnCharacterUpdated` – обновление конкретного персонажа

**Ключевые методы:**
- `Task LoadCharactersAsync()` – загрузка всех персонажей с инициализацией рас, классов и заклинаний
- `Task SaveAllAsync()` – сохранение всех персонажей
- `List<CharacterUIData> GetAllCharacters()` – получение всех персонажей (UI-версия)
- `CharacterUIData GetCharacter(string id)` – получение персонажа по ID
- `CharacterData GetRawCharacter(string id)` – получение сырых данных персонажа
- `Task<CharacterData> CreateCharacterAsync(...)` – создание нового персонажа
- `Task UpdateCharacterAsync(CharacterData character)` – обновление персонажа
- `Task ApplyDamageAsync(string characterId, int amount)` – применение урона
- `Task ApplyHealAsync(string characterId, int amount)` – применение лечения
- `Task LevelUpAsync(string characterId)` – повышение уровня
- `Task DeleteCharacterAsync(string characterId)` – удаление персонажа

---

### HandbookModel
**Файл:** `Assets/DnD-Dynamics/Scripts/MVP/Models/HandbookModel.cs`

**Назначение:** Кэширование и управление данными справочника (заклинания, предметы, монстры, расы, классы). Поддержка создания Homebrew-контента.

**Зависимости:**
- `IDataService _dataService` – сервис данных

**События:**
- `event Action<List<Spell>> OnSpellsChanged`
- `event Action<List<Item>> OnItemsChanged`
- `event Action<List<Monster>> OnMonstersChanged`
- `event Action<List<CharacterRace>> OnRacesChanged`
- `event Action<List<CharacterClass>> OnClassesChanged`

**Ключевые методы:**
- `Task<List<Spell>> GetSpellsAsync()` – загрузка заклинаний с кэшированием
- `Task<List<Item>> GetItemsAsync()` – загрузка предметов с кэшированием
- `Task<List<Monster>> GetMonstersAsync()` – загрузка монстров с кэшированием
- `Task<List<CharacterRace>> GetRacesAsync()` – загрузка рас с кэшированием
- `Task<List<CharacterClass>> GetClassesAsync()` – загрузка классов с кэшированием
- `Task<List<HandbookEntity>> GetItemsByCategoryAsync(HandbookCategory category)` – получение по категории
- `Task AddSpellAsync(Spell spell)` – добавление заклинания (Homebrew)
- `Task AddItemAsync(Item item)` – добавление предмета (Homebrew)
- `Task AddMonsterAsync(Monster monster)` – добавление монстра (Homebrew)
- `Task AddRaceAsync(CharacterRace race)` – добавление расы (Homebrew)
- `Task AddClassAsync(CharacterClass charClass)` – добавление класса (Homebrew)
- `Task DeleteSpellAsync(string id)` – удаление заклинания
- `Task ToggleFavoriteAsync(string id, HandbookCategory category)` – переключение избранного

---

## 🔴 Слой Core (Боевые модели и характеристики)

### CombatSession
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/Combat/CombatSession.cs`

**Назначение:** Модель боевой сессии, хранящая состояние боя, очередь ходов и список бойцов.

**Свойства:**
- `string Id` – уникальный идентификатор сессии
- `string Name` – название сессии
- `DateTime CreatedAt` – дата создания
- `int CurrentRound` – текущий раунд
- `int CurrentTurnIndex` – индекс текущего хода
- `List<Combatant> Combatants` – список бойцов
- `Combatant CurrentCombatant` – текущий боец (вычисляемое)

**Ключевые методы:**
- `void SortByInitiative()` – сортировка бойцов по инициативе
- `void NextTurn()` – переход к следующему ходу (с увеличением раунда)
- `void PreviousTurn()` – возврат к предыдущему ходу
- `void AddCombatant(Combatant combatant)` – добавление бойца с автосортировкой
- `void RemoveCombatant(string id)` – удаление бойца
- `Combatant GetCombatant(string id)` – получение бойца по ID
- `void Clear()` – очистка сессии

---

### Combatant
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/Combat/Combatant.cs`

**Назначение:** Модель бойца в бою (может быть персонажем или монстром).

**Свойства:**
- `string Id` – уникальный идентификатор
- `string Name` – имя бойца
- `int Initiative` – значение инициативы
- `int CurrentHp` – текущие HP
- `int MaxHp` – максимальные HP
- `int ArmorClass` – класс брони
- `bool IsPlayer` – флаг игрока
- `bool IsMonster` – флаг монстра
- `string MonsterId` – ID монстра (если монстр)
- `string CharacterId` – ID персонажа (если персонаж)
- `int HpPercentage` – процент здоровья (вычисляемое)
- `string HpText` – текст здоровья "текущее/макс" (вычисляемое)
- `bool IsAlive` – флаг жив (вычисляемое)

**Ключевые методы:**
- `void ApplyDamage(int amount)` – применение урона
- `void ApplyHeal(int amount)` – применение лечения

---

### CharacterStats
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/CharacterStats.cs`

**Назначение:** Модель характеристик персонажа (6 основных способностей D&D 5e).

**Свойства:**
- `int Strength` – Сила
- `int Dexterity` – Ловкость
- `int Constitution` – Телосложение
- `int Intelligence` – Интеллект
- `int Wisdom` – Мудрость
- `int Charisma` – Харизма

**Ключевые методы:**
- `int GetAbility(CharacterAbility ability)` – получение значения способности
- `void SetAbility(CharacterAbility ability, int value)` – установка значения (с валидацией)
- `static int CalculateModifier(int score)` – расчет модификатора: `(score - 10) / 2`
- `int GetModifier(CharacterAbility ability)` – получение модификатора способности
- `CharacterStats Clone()` – клонирование объекта

**Валидация:** Все значения ограничены константами `Constants.MIN_ABILITY_SCORE` и `Constants.MAX_ABILITY_SCORE`.

---

## 🟡 DTO-классы для сериализации

DTO (Data Transfer Objects) используются для сериализации/десериализации данных в JSON.

### SaveData
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/DTO/SaveData.cs`

**Назначение:** Корневой объект сохранения, содержащий всех персонажей.

**Структура:**
```csharp
[Serializable]
public class SaveData
{
    public List<SerializableCharacterData> characters;
    public int version;
    public string saveDate;
}
```

---

### SerializableCharacterData
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/DTO/SerializableCharacterData.cs`

**Назначение:** Сериализуемая версия CharacterData для сохранения в JSON.

**Структура:**
```csharp
[Serializable]
public class SerializableCharacterData
{
    public string Id;
    public string Name;
    public int Level;
    public int ExperiencePoints;
    
    public CharacterRace Race;
    public CharacterClass Class;
    
    public CharacterStats BaseStats;
    public CharacterStats BonusStats;
    
    public int CurrentHp;
    public int TemporaryHp;
    public int ArmorClass;
    
    public int Gold;
    public int Silver;
    public int Copper;
    
    public string PortraitPath;
    public string Backstory;
    public string Notes;
    
    public string CreatedAt;
    public string UpdatedAt;
}
```

**Методы конвертации:**
- `static SerializableCharacterData FromCharacter(CharacterData character)` – конвертация из доменной модели
- `CharacterData ToCharacter()` – конвертация в доменную модель

---

### SpellData
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/DTO/SpellData.cs`

**Назначение:** DTO для сериализации заклинаний из JSON.

---

### ItemData
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/DTO/ItemData.cs`

**Назначение:** DTO для сериализации предметов из JSON.

---

### SkillData
**Файл:** `Assets/DnD-Dynamics/Scripts/Models/DTO/SkillData.cs`

**Назначение:** DTO для сериализации навыков из JSON.

---

## 📊 Схема взаимодействия слоев

```
┌─────────────────────────────────────────────────────────────┐
│                      UI Layer (MonoBehaviour)               │
│  CharacterListWindow, CharacterDetailWindow, CombatTracker  │
└───────────────────────┬─────────────────────────────────────┘
                        │ использует
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                   Presenters Layer (MVP)                    │
│  CharacterListPresenter, CharacterDetailPresenter, etc.     │
└───────────────────────┬─────────────────────────────────────┘
                        │ координирует
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                    Models Layer (MVP)                       │
│  CharacterModel, HandbookModel                              │
└───────────────────────┬─────────────────────────────────────┘
                        │ обращается к
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                    Services Layer                           │
│  IDataService, ICombatService, ISearchService, etc.         │
└───────────────────────┬─────────────────────────────────────┘
                        │ работает с
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                     Core Layer                              │
│  CharacterData, CombatSession, Combatant, CharacterStats    │
│  + DTO classes for serialization                            │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔑 Ключевые особенности архитектуры

1. **Чистая бизнес-логика (Core)** – не зависит от Unity, позволяет проводить модульное тестирование без запуска движка.

2. **Интерфейсная изоляция (Services)** – все сервисы объявлены через интерфейсы (`I*Service`), что позволяет легко подменять реализации и тестировать компоненты.

3. **MVP паттерн** – четкое разделение между:
   - **Models** – состояние и данные
   - **Views** – отображение (MonoBehaviour)
   - **Presenters** – логика координации

4. **Dependency Injection (Zenject)** – все зависимости инжектятся через контейнер, конфигурируемый в `GlobalInstaller.cs`.

5. **Асинхронность** – большинство операций с данными выполняются асинхронно (`async/Task`), что предотвращает блокировку UI.

6. **Событийная модель** – компоненты общаются через события (`event Action<T>`), что снижает связанность.

7. **DTO для сериализации** – отдельные классы для JSON-сериализации изолируют доменные модели от формата хранения.

8. **Кэширование данных** – `HandbookModel` кэширует загруженные данные, предотвращая повторные запросы.

9. **Homebrew поддержка** – возможность создания пользовательского контента с флагом `IsHomebrew`.

10. **Валидация данных** – характеристики персонажа валидируются при установке (`Math.Clamp`).
