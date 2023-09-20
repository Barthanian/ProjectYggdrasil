using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStat {
    STAT_SPEED,
    STAT_SCORE_MULTIPLIER,
}

[System.Serializable]
public class Buff
{
    public EStat BuffType = EStat.STAT_SPEED;
    public float Value = 2.0f;
    public float Duration = 1.0f;

    // return true if alive
    public bool Tick(float deltaTime) {
        Duration -= deltaTime;

        return Duration > 0.0f;
    }
}
