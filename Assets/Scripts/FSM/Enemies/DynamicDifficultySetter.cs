using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicDifficultySetter
{
    private static float damageFormula = -1;
    private static float shootFormula = -1;
    private static bool apply = true;
    public static void SetDifficulty()
    {
        damageFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() * 0.75f - GameManager.GetManager().GetCurrentRoomIndex() * 0.75f;
        shootFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() * 0.25f - GameManager.GetManager().GetCurrentRoomIndex() * 0.25f;
    }
    public static float GetDamage(float baseDamage)
    {
        if (!apply || baseDamage == 20)
            return baseDamage;
        
        float shootsToKill = 100 / baseDamage  + damageFormula;

        if (shootsToKill < 3) shootsToKill = 3;

        return 100/shootsToKill;
    }
    public static float GetShootSpeed(float baseShootSpeed)
    {
        if (!apply)
            return baseShootSpeed;

        return baseShootSpeed + shootFormula;
    }
    public static void SetDifficultyBool(bool dynDiff)
    {
        apply = dynDiff;
    }
}
