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

    public static readonly UnityEngine.Color PrimaryColor = new UnityEngine.Color(0.32f, 0.27f, 0.83f); // #512BD4
    public static readonly UnityEngine.Color SecondaryColor = new UnityEngine.Color(0.17f, 0.07f, 0.25f); // #2B0B3F
    public static readonly UnityEngine.Color SuccessColor = new UnityEngine.Color(0.06f, 0.72f, 0.51f); // #10B981
    public static readonly UnityEngine.Color DangerColor = new UnityEngine.Color(0.94f, 0.27f, 0.27f); // #EF4444
    public static readonly UnityEngine.Color WarningColor = new UnityEngine.Color(0.96f, 0.53f, 0.11f); // #F59E0B

    public const float ANIMATION_DURATION = 0.3f;

    public const string APP_VERSION = "1.0.0";
}