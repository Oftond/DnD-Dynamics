#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class UIAssetGenerator : EditorWindow
{
    [MenuItem("Tools/DnD Dynamics/Generate UI Assets")]
    public static void GenerateAssets()
    {
        string baseDir = "Assets/DnD-Dynamics/Resources/UI";
        
        if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);
        if (!Directory.Exists(baseDir + "/Styles")) Directory.CreateDirectory(baseDir + "/Styles");
        if (!Directory.Exists(baseDir + "/Layouts")) Directory.CreateDirectory(baseDir + "/Layouts");

        // 1. Global Styles
        CreateFile(baseDir + "/Styles/Common.uss", GetCommonStyles());
        
        // 2. Main Menu
        CreateFile(baseDir + "/Layouts/MainMenu.uxml", GetMainMenuLayout());
        
        // 3. Character Creator
        CreateFile(baseDir + "/Layouts/CharacterCreator.uxml", GetCharacterCreatorLayout());
        
        // 4. Handbook
        CreateFile(baseDir + "/Layouts/Handbook.uxml", GetHandbookLayout());
        
        // 5. DM Tools
        CreateFile(baseDir + "/Layouts/DMTools.uxml", GetDMToolsLayout());
        
        // 6. Combat Tracker
        CreateFile(baseDir + "/Layouts/CombatTracker.uxml", GetCombatTrackerLayout());

        AssetDatabase.Refresh();
        Debug.Log("✅ UI Assets generated successfully in " + baseDir);
    }

    private static void CreateFile(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    #region Styles
    private static string GetCommonStyles()
    {
        return @"
:root {
    --bg-color: #1e1e1e;
    --panel-color: #2d2d2d;
    --accent-color: #d4a017;
    --text-color: #ffffff;
    --text-muted: #aaaaaa;
    --danger-color: #cf4545;
    --success-color: #45cf68;
    --font-size-base: 14px;
    --font-size-header: 20px;
    --radius: 8px;
}

* {
    background-color: var(--bg-color);
    color: var(--text-color);
    font-size: var(--font-size-base);
    font-family: 'Roboto';
}

.window-root {
    flex-direction: column;
    height: 100%;
    width: 100%;
    padding: 10px;
    background-color: var(--bg-color);
}

.panel {
    background-color: var(--panel-color);
    border-radius: var(--radius);
    padding: 15px;
    margin-bottom: 10px;
}

.header {
    font-size: var(--font-size-header);
    color: var(--accent-color);
    margin-bottom: 15px;
    border-bottom: 2px solid var(--accent-color);
    padding-bottom: 5px;
}

.button {
    background-color: var(--accent-color);
    color: #000;
    border-radius: 4px;
    padding: 8px 16px;
    font-weight: bold;
    margin-top: 5px;
}

.button:hover {
    opacity: 0.9;
}

.button:active {
    transform: scale(0.98);
}

.input-field {
    background-color: #3d3d3d;
    border: 1px solid #555;
    border-radius: 4px;
    margin-bottom: 10px;
}

.list-view {
    flex-grow: 1;
    background-color: #252525;
    border: 1px solid #444;
    border-radius: 4px;
}

.list-item {
    padding: 8px;
    border-bottom: 1px solid #333;
}

.list-item:hover {
    background-color: #3d3d3d;
}

.tab-view {
    flex-direction: row;
}

.tab-button {
    flex: 1;
    text-align: center;
    background-color: #333;
    margin-right: 2px;
}

.tab-button:checked {
    background-color: var(--accent-color);
    color: #000;
}

.combatant-row {
    flex-direction: row;
    align-items: center;
    padding: 5px;
    background-color: #333;
    margin-bottom: 4px;
    border-radius: 4px;
}

.stat-box {
    min-width: 40px;
    text-align: center;
    background-color: #222;
    border-radius: 4px;
    padding: 4px;
}
";
    }
    #endregion

    #region Layouts
    private static string GetMainMenuLayout()
    {
        return @"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"" xsi=""http://www.w3.org/2001/XMLSchema-instance"" engine=""UnityEngine.UIElements"" editor=""UnityEditor.UIElements"" noNamespaceSchemaLocation=""../../../UIElementsSchema/UIElements.xsd"" editor-extension-mode=""False"">
    <Style src=""project://database/Assets/DnD-Dynamics/Resources/UI/Styles/Common.uss"" />
    <VisualElement name=""MainMenuRoot"" class=""window-root"">
        <Label text=""DnD Dynamics"" class=""header"" style=""font-size: 32px; text-align: center;"" />
        
        <VisualElement class=""panel"" style=""flex-direction: column; align-items: center; justify-content: center; flex-grow: 1;"">
            <Button name=""BtnNewGame"" text=""Новая игра / Персонажи"" style=""width: 200px; margin: 5px;"" />
            <Button name=""BtnHandbook"" text=""Справочник"" style=""width: 200px; margin: 5px;"" />
            <Button name=""BtnCombat"" text=""Трекер Боя"" style=""width: 200px; margin: 5px;"" />
            <Button name=""BtnDMTools"" text=""Инструменты Мастера"" style=""width: 200px; margin: 5px;"" />
            <Button name=""BtnSettings"" text=""Настройки"" style=""width: 200px; margin: 5px;"" />
            <Button name=""BtnExit"" text=""Выход"" style=""width: 200px; margin: 5px; background-color: var(--danger-color);"" />
        </VisualElement>
    </VisualElement>
</ui:UXML>";
    }

    private static string GetCharacterCreatorLayout()
    {
        return @"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"" xsi=""http://www.w3.org/2001/XMLSchema-instance"" engine=""UnityEngine.UIElements"" editor=""UnityEditor.UIElements"" noNamespaceSchemaLocation=""../../../UIElementsSchema/UIElements.xsd"" editor-extension-mode=""False"">
    <Style src=""project://database/Assets/DnD-Dynamics/Resources/UI/Styles/Common.uss"" />
    <VisualElement name=""CharacterCreatorRoot"" class=""window-root"">
        <Label text=""Создание Персонажа"" class=""header"" />
        
        <VisualElement style=""flex-direction: row; flex-grow: 1;"">
            <!-- Left: Stats & Info -->
            <VisualElement class=""panel"" style=""width: 40%; flex-direction: column;"">
                <TextField name=""InputName"" label=""Имя"" />
                <DropdownField name=""DropRace"" label=""Раса"" choices=""Человек,Эльф,Дварф,Орк"" />
                <DropdownField name=""DropClass"" label=""Класс"" choices=""Воин,Волшебник,Плут,Жрец"" />
                <IntegerField name=""InputLevel"" label=""Уровень"" value=""1"" />
                
                <Label text=""Характеристики"" style=""margin-top: 10px;"" />
                <VisualElement style=""flex-direction: row; flex-wrap: wrap;"">
                    <IntegerField name=""StatSTR"" label=""STR"" value=""10"" class=""stat-box"" />
                    <IntegerField name=""StatDEX"" label=""DEX"" value=""10"" class=""stat-box"" />
                    <IntegerField name=""StatCON"" label=""CON"" value=""10"" class=""stat-box"" />
                    <IntegerField name=""StatINT"" label=""INT"" value=""10"" class=""stat-box"" />
                    <IntegerField name=""StatWIS"" label=""WIS"" value=""10"" class=""stat-box"" />
                    <IntegerField name=""StatCHA"" label=""CHA"" value=""10"" class=""stat-box"" />
                </VisualElement>
                
                <Button name=""BtnRollStats"" text=""🎲 Бросить характеристики"" style=""margin-top: 10px;"" />
                <Button name=""BtnBuyStats"" text=""🛒 Покупка очков"" style=""margin-top: 5px;"" />
            </VisualElement>

            <!-- Right: Equipment & Spells -->
            <VisualElement class=""panel"" style=""width: 60%; flex-direction: column;"">
                <TabView name=""TabsCreator"">
                    <Button text=""Снаряжение"" data-tab=""equipment"" class=""tab-button"" />
                    <Button text=""Заклинания"" data-tab=""spells"" class=""tab-button"" />
                    <Button text=""Биография"" data-tab=""bio"" class=""tab-button"" />
                </TabView>
                
                <ScrollView name=""ContentEquipment"" style=""flex-grow: 1;"">
                    <Label text=""Список стартового снаряжения..."" />
                </ScrollView>
                <ScrollView name=""ContentSpells"" style=""flex-grow: 1; display: none;"">
                    <Label text=""Выберите заклинания..."" />
                </ScrollView>
            </VisualElement>
        </VisualElement>

        <VisualElement style=""flex-direction: row; justify-content: flex-end; margin-top: 10px;"">
            <Button name=""BtnCancel"" text=""Отмена"" style=""width: 100px; background-color: var(--danger-color);"" />
            <Button name=""BtnSaveCharacter"" text=""Сохранить персонажа"" style=""width: 150px; background-color: var(--success-color);"" />
        </VisualElement>
    </VisualElement>
</ui:UXML>";
    }

    private static string GetHandbookLayout()
    {
        return @"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"" xsi=""http://www.w3.org/2001/XMLSchema-instance"" engine=""UnityEngine.UIElements"" editor=""UnityEditor.UIElements"" noNamespaceSchemaLocation=""../../../UIElementsSchema/UIElements.xsd"" editor-extension-mode=""False"">
    <Style src=""project://database/Assets/DnD-Dynamics/Resources/UI/Styles/Common.uss"" />
    <VisualElement name=""HandbookRoot"" class=""window-root"">
        <Label text=""Справочник (Handbook)"" class=""header"" />
        
        <!-- Filters -->
        <VisualElement class=""panel"" style=""flex-direction: row; align-items: center;"">
            <TextField name=""InputSearch"" placeholder=""Поиск..."" style=""flex-grow: 1; margin-right: 10px;"" />
            <DropdownField name=""DropCategory"" label=""Категория"" choices=""Все,Расы,Классы,Заклинания,Предметы,Монстры"" />
            <Toggle name=""ToggleHomebrewOnly"" label=""Только Homebrew"" />
            <Button name=""BtnAddNew"" text=""➕ Добавить"" style=""width: 100px;"" />
        </VisualElement>

        <VisualElement style=""flex-direction: row; flex-grow: 1;"">
            <!-- List -->
            <ListView name=""ListHandbook"" class=""list-view"" style=""width: 30%;"" 
                      item-height=""40"" 
                      show-border=""true"" 
                      show-alternating-row-backgrounds=""true"" />
            
            <!-- Details -->
            <VisualElement class=""panel"" style=""width: 70%; flex-direction: column;"">
                <Label name=""LblDetailTitle"" text=""Название элемента"" class=""header"" style=""font-size: 24px;"" />
                <Label name=""LblDetailSubtitle"" text=""Тип: Подтип"" style=""color: var(--text-muted); margin-bottom: 15px;"" />
                
                <ScrollView style=""flex-grow: 1;"">
                    <Label name=""LblDetailDescription"" text=""Полное описание элемента..."" style=""white-space: normal;"" />
                    <VisualElement name=""ContainerStats"" style=""margin-top: 15px; flex-direction: column;"">
                        <!-- Dynamic stats will be injected here -->
                    </VisualElement>
                </ScrollView>

                <VisualElement style=""flex-direction: row; margin-top: 10px;"">
                    <Button name=""BtnEditItem"" text=""✏️ Редактировать"" style=""flex: 1; margin-right: 5px;"" />
                    <Button name=""BtnDeleteItem"" text=""🗑️ Удалить"" style=""flex: 1; background-color: var(--danger-color);"" />
                </VisualElement>
            </VisualElement>
        </VisualElement>
    </VisualElement>
</ui:UXML>";
    }

    private static string GetDMToolsLayout()
    {
        return @"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"" xsi=""http://www.w3.org/2001/XMLSchema-instance"" engine=""UnityEngine.UIElements"" editor=""UnityEditor.UIElements"" noNamespaceSchemaLocation=""../../../UIElementsSchema/UIElements.xsd"" editor-extension-mode=""False"">
    <Style src=""project://database/Assets/DnD-Dynamics/Resources/UI/Styles/Common.uss"" />
    <VisualElement name=""DMToolsRoot"" class=""window-root"">
        <Label text=""Инструменты Мастера (DM Tools)"" class=""header"" />
        
        <VisualElement style=""flex-direction: row; flex-grow: 1;"">
            <!-- Notes List -->
            <VisualElement class=""panel"" style=""width: 25%; flex-direction: column;"">
                <Label text=""Заметки"" class=""header"" style=""font-size: 16px;"" />
                <Button name=""BtnNewNote"" text=""➕ Новая заметка"" style=""margin-bottom: 10px;"" />
                <ListView name=""ListNotes"" class=""list-view"" style=""flex-grow: 1;"" item-height=""30"" />
            </VisualElement>

            <!-- Active Note Editor -->
            <VisualElement class=""panel"" style=""width: 40%; flex-direction: column;"">
                <TextField name=""InputNoteTitle"" label=""Заголовок"" style=""font-size: 18px; font-weight: bold;"" />
                <TextArea name=""AreaNoteContent"" placeholder=""Текст заметки..."" style=""flex-grow: 1; white-space: normal;"" />
                <VisualElement style=""flex-direction: row; margin-top: 5px;"">
                    <Button name=""BtnSaveNote"" text=""💾 Сохранить"" style=""flex: 1;"" />
                    <Button name=""BtnDeleteNote"" text=""🗑️"" style=""width: 40px; background-color: var(--danger-color);"" />
                </VisualElement>
            </VisualElement>

            <!-- Quick Dice & Random -->
            <VisualElement class=""panel"" style=""width: 35%; flex-direction: column;"">
                <Label text=""Быстрый бросок"" class=""header"" style=""font-size: 16px;"" />
                <VisualElement style=""flex-direction: row; flex-wrap: wrap; justify-content: center;"">
                    <Button name=""DiceD4"" text=""d4"" style=""width: 50px; height: 50px; margin: 5px;"" />
                    <Button name=""DiceD6"" text=""d6"" style=""width: 50px; height: 50px; margin: 5px;"" />
                    <Button name=""DiceD8"" text=""d8"" style=""width: 50px; height: 50px; margin: 5px;"" />
                    <Button name=""DiceD10"" text=""d10"" style=""width: 50px; height: 50px; margin: 5px;"" />
                    <Button name=""DiceD12"" text=""d12"" style=""width: 50px; height: 50px; margin: 5px;"" />
                    <Button name=""DiceD20"" text=""d20"" style=""width: 60px; height: 60px; margin: 5px; font-size: 20px; background-color: var(--accent-color);"" />
                    <Button name=""DiceD100"" text=""d100"" style=""width: 50px; height: 50px; margin: 5px;"" />
                </VisualElement>
                <Label name=""LblLastRoll"" text=""Последний бросок: -"" style=""text-align: center; margin-top: 10px; font-size: 18px; color: var(--accent-color);"" />
                
                <Label text=""Случайное имя NPC"" class=""header"" style=""font-size: 16px; margin-top: 20px;"" />
                <Button name=""BtnGenNPC"" text=""🎲 Сгенерировать"" style=""width: 100%;"" />
                <Label name=""LblNPCName"" text="""" style=""text-align: center; font-style: italic;"" />
            </VisualElement>
        </VisualElement>
    </VisualElement>
</ui:UXML>";
    }

    private static string GetCombatTrackerLayout()
    {
        return @"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"" xsi=""http://www.w3.org/2001/XMLSchema-instance"" engine=""UnityEngine.UIElements"" editor=""UnityEditor.UIElements"" noNamespaceSchemaLocation=""../../../UIElementsSchema/UIElements.xsd"" editor-extension-mode=""False"">
    <Style src=""project://database/Assets/DnD-Dynamics/Resources/UI/Styles/Common.uss"" />
    <VisualElement name=""CombatRoot"" class=""window-root"">
        <Label text=""Трекер Боя"" class=""header"" />
        
        <!-- Top Bar: Controls -->
        <VisualElement class=""panel"" style=""flex-direction: row; align-items: center;"">
            <Button name=""BtnStartCombat"" text=""▶️ Начать бой"" style=""width: 120px; background-color: var(--success-color);"" />
            <Button name=""BtnNextTurn"" text=""➡️ Следующий ход"" style=""width: 120px; margin-left: 10px;"" />
            <Label name=""LblCurrentTurn"" text=""Ход: -"" style=""margin-left: 20px; font-size: 18px; font-weight: bold; color: var(--accent-color);"" />
            
            <VisualElement style=""flex-grow: 1;"" />
            
            <Button name=""BtnAddMonster"" text=""➕ Монстр из бестиария"" style=""width: 160px;"" />
            <Button name=""BtnAddPC"" text=""➕ Персонаж"" style=""width: 120px;"" />
            <Button name=""BtnEndCombat"" text=""⏹️ Завершить"" style=""width: 100px; background-color: var(--danger-color); margin-left: 10px;"" />
        </VisualElement>

        <VisualElement style=""flex-direction: row; flex-grow: 1;"">
            <!-- Initiative List -->
            <VisualElement class=""panel"" style=""width: 60%; flex-direction: column;"">
                <Label text=""Инициатива"" class=""header"" style=""font-size: 16px;"" />
                <ScrollView name=""ScrollInitiative"" style=""flex-grow: 1;"">
                    <VisualElement name=""ContainerInitiativeList"" style=""flex-direction: column;"">
                        <!-- Template for Combatant Row (injected via C# or defined here) -->
                    </VisualElement>
                </ScrollView>
            </VisualElement>

            <!-- Selected Combatant Details -->
            <VisualElement class=""panel"" style=""width: 40%; flex-direction: column;"">
                <Label name=""LblSelName"" text=""Выберите участника"" class=""header"" style=""font-size: 20px;"" />
                
                <VisualElement style=""flex-direction: row; margin-bottom: 10px;"">
                    <VisualElement class=""stat-box"" style=""flex: 1;"">
                        <Label text=""HP"" style=""font-size: 10px;"" />
                        <Label name=""LblSelHP"" text=""20/20"" style=""font-size: 18px; color: var(--success-color);"" />
                    </VisualElement>
                    <VisualElement class=""stat-box"" style=""flex: 1;"">
                        <Label text=""AC"" style=""font-size: 10px;"" />
                        <Label name=""LblSelAC"" text=""15"" style=""font-size: 18px;"" />
                    </VisualElement>
                    <VisualElement class=""stat-box"" style=""flex: 1;"">
                        <Label text=""Init"" style=""font-size: 10px;"" />
                        <Label name=""LblSelInit"" text=""12"" style=""font-size: 18px;"" />
                    </VisualElement>
                </VisualElement>

                <Label text=""Эффекты и Состояния"" style=""margin-top: 10px;"" />
                <VisualElement name=""ContainerConditions"" style=""flex-direction: row; flex-wrap: wrap; margin-bottom: 10px;"">
                    <!-- Condition badges go here -->
                </VisualElement>
                <Button name=""BtnAddCondition"" text=""➕ Добавить состояние"" style=""width: 100%; margin-bottom: 10px;"" />

                <Label text=""Действия"" style=""margin-top: 10px;"" />
                <TextArea name=""AreaCombatLog"" placeholder=""Лог боя..."" style=""height: 100px; font-size: 12px;"" />
                <Button name=""BtnApplyDamage"" text=""💥 Нанести урон"" style=""background-color: var(--danger-color);"" />
                <Button name=""BtnHeal"" text=""❤️ Лечение"" style=""background-color: var(--success-color);"" />
            </VisualElement>
        </VisualElement>
    </VisualElement>
</ui:UXML>";
    }
    #endregion
}
#endif
