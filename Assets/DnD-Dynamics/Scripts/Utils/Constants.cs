using UnityEngine;

public static class Constants
{
    public const string SAVE_KEY_CHARACTERS = "characters";
    public const string SAVE_KEY_SETTINGS = "settings";
    public const string SAVE_KEY_GAME_STATE = "game_state";

    public const int DEFAULT_STRENGTH = 10;
    public const int DEFAULT_DEXTERITY = 10;
    public const int DEFAULT_CONSTITUTION = 10;
    public const int DEFAULT_INTELLIGENCE = 10;
    public const int DEFAULT_WISDOM = 10;
    public const int DEFAULT_CHARISMA = 10;

    public const int MIN_ABILITY_SCORE = 3;
    public const int MAX_ABILITY_SCORE = 20;

    public const int MAX_LEVEL = 20;

    public static readonly Color PrimaryColor = new Color(0.32f, 0.27f, 0.83f);
    public static readonly Color SecondaryColor = new Color(0.17f, 0.07f, 0.25f);
    public static readonly Color SuccessColor = new Color(0.06f, 0.72f, 0.51f);
    public static readonly Color DangerColor = new Color(0.94f, 0.27f, 0.27f);
    public static readonly Color WarningColor = new Color(0.96f, 0.53f, 0.11f);

    public const float ANIMATION_DURATION = 0.3f;

    public const string APP_VERSION = "1.0.0";
}