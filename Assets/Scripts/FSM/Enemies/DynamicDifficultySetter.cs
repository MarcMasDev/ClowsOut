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
        damageFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() - GameManager.GetManager().GetCurrentRoomIndex()*2;
        shootFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() * 0.25f - GameManager.GetManager().GetCurrentRoomIndex() * 0.25f;
    }
    public static float GetDamage(float baseDamage)
    {
        if (!apply)
            return baseDamage;

        if (damageFormula < 1) { damageFormula = 1; }
        return baseDamage / damageFormula*0.75f;
    }
    public static float GetShootSpeed(float baseShootSpeed)
    {
        if (!apply)
            return baseShootSpeed;

        if (shootFormula == -1) SetDifficulty();
        return baseShootSpeed + shootFormula;
    }
    public static void SetDifficultyBool(bool dynDiff)
    {
        apply = dynDiff;
    }
}
