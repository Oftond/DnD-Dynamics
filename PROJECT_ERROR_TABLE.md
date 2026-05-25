# 📋 Таблица типичных ошибок и способов их устранения (DnD-Dynamics)

| Код/Сообщение об ошибке | Вероятная причина | Способ устранения |
|-------------------------|-------------------|-------------------|
| **NullReferenceException in CharacterDetailPresenter** | Отсутствует привязка View в Zenject Installer или не назначен префаб в инспекторе Unity. Проверить наличие компонента `CharacterDetailWindow` на сцене и его регистрацию в `GlobalInstaller`. | Убедиться, что `CharacterDetailWindow` добавлен на сцену и имеет компонент `ICharacterDetailView`. Проверить绑定 в `GlobalInstaller.PresentersInstall()`. |
| **MissingReferenceException: The object of type 'GameObject' has been destroyed** | Обращение к UI-элементу после смены сцены или закрытия окна. Чаще всего возникает в `CharacterListWindow`, `CombatTrackerView` при попытке обновить UI после уничтожения объекта. | Реализовать корректную отписку от событий Presenter'а в методе `OnDestroy()` View-компонента. Использовать проверку `if (this != null)` перед обращением к UI. |
| **JsonSerializationException: Self referencing loop detected** | Наличие циклических ссылок в объектах модели (например, `CharacterData` ссылается на `CharacterClass`, а класс может содержать ссылки на персонажей). Возникает при сериализации через `DataService`. | Использовать атрибут `[JsonIgnore]` для обратных ссылок в классах `CharacterData`, `Spellbook`, `Inventory`. Настроить `JsonConvert.SerializeObject` с `ReferenceLoopHandling.Ignore`. |
| **UnauthorizedAccessException при сохранении** | Отсутствие прав доступа к внешней памяти на Android 10+ (Scoped Storage). Попытка записи вне `Application.persistentDataPath`. | Использовать только путь `Application.persistentDataPath + "/GameData/"` для всех операций сохранения. Проверить `AndroidManifest.xml` на наличие разрешений `WRITE_EXTERNAL_STORAGE` для API < 29. |
| **FileNotFoundException: Data/skills.json** | Файл данных не найден в папке `Resources/Data/`. Возникает в `GameDataService.LoadSkills()` при загрузке справочника. | Убедиться, что файлы `skills.json`, `spells.json`, `items.json` находятся в папке `Assets/Resources/Data/`. Проверить имя файла и расширение (чувствительно к регистру на некоторых платформах). |
| **ZenjectResolutionException: Missing dependency: IDataService** | Сервис `IDataService` не зарегистрирован в контейнере Zenject. Ошибка возникает при создании `CharacterModel` или `CharacterListPresenter`. | Проверить `GlobalInstaller.ServicesInstall()`: должна быть строка `Container.Bind<IDataService>().To<DataService>().AsSingle();`. Убедиться, что `GlobalInstaller` добавлен на сцену. |
| **InvalidOperationException: Collection was modified** | Модификация коллекции `_characters` в `CharacterModel` во время итерации. Возникает при одновременном добавлении/удалении персонажей и обновлении UI. | Использовать `.ToList()` для создания копии коллекции перед итерацией. В `CharacterListPresenter` применять `var characters = _model.GetAllCharacters().ToList();`. |
| **ArgumentException: Invalid ability score** | Попытка установить характеристику персонажа вне диапазона 1-30. Возникает в `CharacterStats.SetAbility()` при редактировании персонажа. | Проверить валидацию входных данных в `CreateCharacterWindow` и `CharacterDetailWindow`. Использовать `Math.Clamp(value, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE)`. |
| **KeyNotFoundException: Combatant not found** | Попытка получить бойца по ID из `CombatSession.Combatants`, когда боец был удалён. Возникает в `CombatService.ApplyDamageToCombatantAsync()`. | Проверить существование бойца перед операцией: `if (_currentSession.GetCombatant(id) != null)`. Обрабатывать случай удаления бойца во время боя. |
| **Build Failed: Gradle build failed** | Конфликт версий Android SDK/Build Tools или нехватка памяти Java Heap. Часто при сборке под Android с большим количеством ресурсов. | Очистить кэш Gradle (`Edit -> Preferences -> External Tools -> Clear Gradle Cache`). Увеличить `JAVA_OPTS=-Xmx4096m` в переменных среды. Проверить `minSdkVersion` и `targetSdkVersion` в `ProjectSettings/Player`. |
| **MissingComponentException: DiceRollerService** | Сервис `DiceRollerService` не зарегистрирован в Zenject, но требуется в `DiceRollerView.Initialize()`. | Добавить в `GlobalInstaller.ServicesInstall()`: `Container.Bind<DiceRollerService>().AsSingle();`. Убедиться, что `DiceRollerView` получает сервис через `GameManager.UIHandler` или DI. |
| **TimeoutException: LoadCharactersAsync** | Длительная загрузка персонажей из JSON (>5 сек) на слабых устройствах. Возникает в `CharacterModel.LoadCharactersAsync()`. | Использовать асинхронную загрузку с прогресс-баром в `CharacterListWindow`. Разбить загрузку на чанки: `await Task.Yield()` между итерациями. Кэшировать результаты в `DataService`. |
| **InvalidCastException: Cannot cast SpellData to Spell** | Несоответствие DTO и доменной модели. Возникает при конвертации `SpellData` (из JSON) в `Spell` (для UI). | Проверить методы конвертации в `DataService`. Использовать явное преобразование: `var spell = new Spell { Id = data.Id, Name = data.Name, ... }`. |
| **UnassignedReferenceException: _combatantsContainer** | Поле `_combatantsContainer` в `CombatTrackerView` не назначено в инспекторе Unity. | В инспекторе Unity перетащить GameObject контейнера в поле `Combatants List -> Combatants Container`. Проверить префаб `CombatTrackerView`. |
| **StackOverflowException: Recursive property access** | Бесконечная рекурсия при доступе к свойствам `CharacterData.Class` → `CharacterClass.Spells` → `Spell.Class`. | Убрать навигационные свойства с обеих сторон связи. Использовать `[JsonIgnore]` на одной из сторон. Загружать связанные данные отдельно через `IDataService`. |

---

## 🔍 Дополнительные рекомендации по отладке

### Логи и трассировка
- Включить verbose-логирование в `DataService`: добавить `Debug.Log()` в каждый метод загрузки
- Использовать `UnityEngine.Profiling.Profiler` для анализа производительности `LoadCharactersAsync()`
- Написать unit-тесты на `CharacterStats.CalculateModifier()` и `DiceRollerService.RollDiceAsync()`

### Типичные проблемы архитектуры
| Проблема | Решение |
|----------|---------|
| Circular dependency между Presenters и Views | Использовать интерфейсы `ICharacterDetailView`, `ICharacterListView` |
| State mismatch между Model и View | Вызывать `NotifyCharactersChanged()` после каждой мутации в `CharacterModel` |
| Race condition при загрузке данных | Использовать `Lazy<Task<T>>` в `DataService` для предотращения повторной загрузки |

### Инструменты для диагностики
- **Zenject Diagnostics**: `Container.CheckDependencies()` в `GameManager.Initialize()`
- **JSON Validator**: проверить все `.json` файлы в `Assets/Resources/Data/` через онлайн-валидатор
- **Unity Profiler**: анализ памяти для `CombatSession.Combatants` при большом количестве бойцов
