using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicDifficultySetter
{
    private static float damageFormula = -1;
    private static float shootFormula = -1;
    private static bool apply = true;
    private static bool serialized = false;
    public static void SetDifficulty()
    {
        damageFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() * 0.3f - GameManager.GetManager().GetCurrentRoomIndex() * 0.3f;
        shootFormula = GameManager.GetManager().GetLevelData().LoadDeathsPlayer() * 0.25f - GameManager.GetManager().GetCurrentRoomIndex() * 0.25f;
    }
    public static float GetDamage(float baseDamage)
    {
        if (!apply)
            return baseDamage;
        
        if (!serialized)
        {
            serialized = true;
            SetDifficulty();
        }

        float shootsToKill = 100 / baseDamage  + damageFormula;

        if (shootsToKill < 4) shootsToKill = 4;
        else if (shootsToKill > 6.1f) shootsToKill = 6.5f;

        Debug.Log("DEB_SHOOTS TO KILL = " + shootsToKill + " // Real Damage = " + 100 / shootsToKill);
        return 100/shootsToKill;
    }
    public static float GetShootSpeed(float baseShootSpeed)
    {
        if (!apply)
            return baseShootSpeed;

        if (!serialized)
        {
            serialized = true;
            SetDifficulty();
        }
        Debug.Log("DEB_ShootSpeed = " + shootFormula + " // current: " + (baseShootSpeed + shootFormula));
        if (shootFormula > 1) shootFormula = 1;
        if (shootFormula < 0) shootFormula = 0;
        return baseShootSpeed + shootFormula;
    }
    public static void SetDifficultyBool(bool dynDiff)
    {
        apply = dynDiff;
    }
}
