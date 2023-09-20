using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData {
    public float Duration;
    public List<Buff> Buffs = new List<Buff>();
    public List<Buff> PermanentBuffs = new List<Buff>();
    public List<EElementType> ActivatedElements = new List<EElementType>();
    public int Lives;

    public PlayerData(float duration, List<Buff> buffs, List<Buff> permanentBuffs, List<EElementType> activatedElements) {
        Duration = duration;
        Buffs = buffs;
        PermanentBuffs = permanentBuffs;
        ActivatedElements = activatedElements;
        Lives = PlayerManager.STARTING_LIVES;
    }
}

public class PlayerManager : ManagerBase {
    public static PlayerManager instance;
    public static int STARTING_LIVES = 3;

    private void Awake() {
        instance = this;
    }

    PlayerData CurrentPlayerData = new PlayerData(0, new List<Buff>(), new List<Buff>(), new List<EElementType>());

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CurrentPlayerData.Duration += Time.deltaTime;
        TickBuffs();
    }

    int TryCounter = 0;

    void TickBuffs() {
        for (int i = CurrentPlayerData.Buffs.Count - 1; i >= 0; --i) {
            if (!CurrentPlayerData.Buffs[i].Tick(Time.deltaTime)) {
                //Debug.Log("Removed buff " + CurrentPlayerData.Buffs[i].BuffType);

                CurrentPlayerData.Buffs.RemoveAt(i);

            }
        }
    }

    public PlayerData GetPlayerData() {
        if(CurrentPlayerData == null) {
            CurrentPlayerData = new PlayerData(0, new List<Buff>(), new List<Buff>(), new List<EElementType>());
        }
        return CurrentPlayerData;
    }

    public void SetPlayerData(PlayerData data) {
        CurrentPlayerData = data;
    }


    public void AddDuration(float duration) {
        CurrentPlayerData.Duration += duration;
    }

    public void AddBuff(Buff buff) {
        //Debug.Log("Added buff " + buff.BuffType);
        CurrentPlayerData.Buffs.Add(buff);
    }

    public void AddPermanentBuff(Buff buff) {
        //Debug.Log("Added permanent buff " + buff.BuffType);
        CurrentPlayerData.PermanentBuffs.Add(buff);
    }

    public void NotifyGameOver() {
       

        SaveManager.instance.SaveFile();

    }

    public void NotifyGameStart(bool sendNotification) {
        ResetPlayerData(sendNotification);
    }

    public void ResetPlayerData(bool showNotification) {
        EMotivationType motivationType = EMotivationType.MOT_GENERIC;


        CurrentPlayerData.Duration = 0;
        CurrentPlayerData.Buffs = new List<Buff>();
        CurrentPlayerData.PermanentBuffs = new List<Buff>();
        CurrentPlayerData.ActivatedElements = new List<EElementType>();
        CurrentPlayerData.Lives = STARTING_LIVES;
        CharacterPlayer.instance.SetTrailEnabled(false);

        if(showNotification) {
            HUD.instance.FadeInMotivation(motivationType);
        }
        //WorldManager.instance.SetSectionCount(5);
    }

    public void HandlePickup(Pickup pickup) {
        switch (pickup.PickupType) {
            case EPickupType.PT_SCORE:
                break;
            case EPickupType.PT_ELEMENT:

                switch (pickup.GetElement()) {
                    case EElementType.ET_STAR:
                        CurrentPlayerData.ActivatedElements.Add(EElementType.ET_STAR);
                        StarManager.instance.ActivateStars();
                        HUD.instance.FadeInMotivation(EMotivationType.MOT_GENERIC, "reach for the stars");
                        break;
                    case EElementType.ET_SUN:
                        CurrentPlayerData.ActivatedElements.Add(EElementType.ET_SUN);
                        StarManager.instance.ActivateSun();
                        WorldManager.instance.Expand(2);
                        HUD.instance.FadeInMotivation(EMotivationType.MOT_GENERIC, "light the way");
                        break;
                    case EElementType.ET_MOON:
                        CurrentPlayerData.ActivatedElements.Add(EElementType.ET_MOON);
                        StarManager.instance.ActivateMoon();
                        HUD.instance.FadeInMotivation(EMotivationType.MOT_GENERIC, "ebb and flow");
                        break;
                    case EElementType.ET_WIND:
                        CurrentPlayerData.ActivatedElements.Add(EElementType.ET_WIND);

                        Buff newBuff = new Buff();
                        newBuff.Duration = -1.0f;
                        newBuff.BuffType = EStat.STAT_SPEED;
                        newBuff.Value = 1.25f;
                        AddPermanentBuff(newBuff);

                        CharacterPlayer.instance.SetTrailEnabled(true);

                        HUD.instance.FadeInMotivation(EMotivationType.MOT_GENERIC, "think");

                        break;
                    case EElementType.ET_FIRE:
                        CurrentPlayerData.ActivatedElements.Add(EElementType.ET_FIRE);

                        HUD.instance.FadeInMotivation(EMotivationType.MOT_GENERIC, "feel.");

                        Buff betterBuff = new Buff();
                        betterBuff.Duration = -1.0f;
                        betterBuff.BuffType = EStat.STAT_SPEED;
                        betterBuff.Value = 3.25f;
                        AddPermanentBuff(betterBuff);

                        WorldManager.instance.SetSectionWidthMinMax(15, 25);

                        break;
                    case EElementType.ET_NONE:
                        break;
                }
                break;

        }

        Destroy(pickup.gameObject);
    }

    public float GetBuffValue(EStat buffType) {
        float highestValue = 1.0f;
        for (int i = 0; i < CurrentPlayerData.Buffs.Count; ++i) {
            if (CurrentPlayerData.Buffs[i].BuffType == buffType && CurrentPlayerData.Buffs[i].Value > highestValue) {
                highestValue = CurrentPlayerData.Buffs[i].Value;
            }
        }
        for (int i = 0; i < CurrentPlayerData.PermanentBuffs.Count; ++i) {
            if (CurrentPlayerData.PermanentBuffs[i].BuffType == buffType && CurrentPlayerData.PermanentBuffs[i].Value > highestValue) {
                highestValue = CurrentPlayerData.PermanentBuffs[i].Value;
            }
        }
        return highestValue;
    }

    public bool HasDoubleJump() {
        return CurrentPlayerData.ActivatedElements.Contains(EElementType.ET_MOON);
    }

}
