using System;
using UnityEngine;

public static class ExperienceTable
{
    public static int GetExperienceForLevel(int level)
    {
        return level switch
        {
            1 => 0,
            2 => 300,
            3 => 900,
            4 => 2700,
            5 => 6500,
            6 => 14000,
            7 => 23000,
            8 => 34000,
            9 => 48000,
            10 => 64000,
            11 => 85000,
            12 => 100000,
            13 => 120000,
            14 => 140000,
            15 => 165000,
            16 => 195000,
            17 => 225000,
            18 => 265000,
            19 => 305000,
            20 => 355000,
            _ => int.MaxValue
        };
    }

    public static bool CanLevelUp(int currentLevel, int currentXP)
    {
        if (currentLevel >= 20) return false;

        return currentXP >= GetExperienceForLevel(currentLevel + 1);
    }

    public static float CalculateProgress(int currentLevel, int currentXP)
    {
        if (currentLevel >= 20) return 1f;

        int xpForCurrent = GetExperienceForLevel(currentLevel);
        int xpForNext = GetExperienceForLevel(currentLevel + 1);
        int xpNeeded = xpForNext - xpForCurrent;

        if (xpNeeded <= 0) return 0f;

        int xpProgress = currentXP - xpForCurrent;
        return Mathf.Clamp01((float)xpProgress / xpNeeded);
    }

    public static int GetRemainingXP(int currentLevel, int currentXP)
    {
        if (currentLevel >= 20)
            return 0;

        int xpForNext = GetExperienceForLevel(currentLevel + 1);

        return Mathf.Max(0, xpForNext - currentXP);
    }
}