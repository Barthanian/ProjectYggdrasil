using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupTypePair {
    public EPickupType Type = EPickupType.PT_SCORE;
    public Pickup PickupTemplate = null;
}

public class WorldManager : ManagerBase {

    public static WorldManager instance;
    private void Awake() {
        instance = this;
    }

    int CurrentWidth = 0;

    public List<Section> SectionList = new List<Section>();
    private int PlatformIndex;

    List<int> PlatformHeights = new List<int>();

    public List<Tile> TileTemplates = new List<Tile>();
    public Section SectionTemplate;

    public List<PickupTypePair> PickupTemplates = new List<PickupTypePair>();

    List<Pickup> PickupList = new List<Pickup>();

    bool bElementPickupActive = false;

    int SectionCount = 5;

    int SectionWidthMin = 5;
    int SectionWidthMax = 15;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public override void InitManager() {
        PlatformHeights.Add(0);

        SpawnSections();
    }

    public void SpawnSections() {
        for (int i = 0; i < SectionList.Count; ++i) {
            Destroy(SectionList[i].gameObject);
        }
        SectionList.Clear();

        CurrentWidth = 0;

        SectionCount = 5;
        for (int i = 0; i < SectionCount; ++i) {
            Section newSection = GameObject.Instantiate(SectionTemplate);
            int randomWidth = Random.Range(SectionWidthMin, SectionWidthMax);
            PlatformHeights[0] = Random.Range(-1, 2);
            newSection.Init(i, randomWidth, PlatformHeights, TileTemplates[0], CurrentWidth);
            if (i == Mathf.FloorToInt(SectionCount / 2.0f)) {
                newSection.SetTriggerSection(true);
            }
            CurrentWidth += randomWidth;
            SectionList.Add(newSection);
        }
    }

    public void SetSectionWidthMinMax(int min, int max) {
        SectionWidthMin = min;
        SectionWidthMax = max;
    }

    public Vector3 GetStartLocation() {
        return SectionList[GetMiddleSectionIndex()].GetCenterTilePosition() + new Vector3(0, 1, 0);
    }

    public void GenerateSection(bool rightwards) {
        //Debug.Log("Generating new section");
        Section originSection = null;
        int index = 0;
        if (rightwards) {
            originSection = SectionList[0];
        }
        else {
            originSection = SectionList[SectionList.Count - 1];
            index = SectionList.Count - 1;
        }
        int previousWidth = originSection.GetWidth();
        int randomWidth = Random.Range(SectionWidthMin, SectionWidthMax);

        SectionList[GetMiddleSectionIndex()].SetTriggerSection(false);
        ShiftSections(rightwards, rightwards ? previousWidth : randomWidth);
        CurrentWidth -= previousWidth;
        SectionList.Remove(originSection);
        Destroy(originSection.gameObject);

        Section newSection = GameObject.Instantiate(SectionTemplate);
        PlatformHeights[0] = Random.Range(-1, 2);
        int startX = 0;

        if (rightwards) {
            startX = CurrentWidth;
        }
        else {
            startX = 0;
        }

        newSection.Init(index, randomWidth, PlatformHeights, TileTemplates[0], startX);
        CurrentWidth += randomWidth;


        SectionList.Insert(rightwards ? SectionList.Count : 0, newSection);
        SectionList[GetMiddleSectionIndex()].SetTriggerSection(true);


        CharacterPlayer.instance.Shift(rightwards, rightwards ? previousWidth : randomWidth, TileTemplates[0].GetComponent<SpriteRenderer>().size.x);
    }

    public void SpawnSection(bool rightwards) {
        //Debug.Log("Generating new section");
        Section originSection = null;
        int index = 0;
        if (rightwards) {
            originSection = SectionList[0];
        }
        else {
            originSection = SectionList[SectionList.Count - 1];
            index = SectionList.Count - 1;
        }
        int previousWidth = originSection.GetWidth();
        int randomWidth = Random.Range(SectionWidthMin, SectionWidthMax);


        Section newSection = GameObject.Instantiate(SectionTemplate);
        PlatformHeights[0] = Random.Range(-1, 2);
        int startX = 0;

        if (rightwards) {
            startX = CurrentWidth;// + SectionList[SectionList.Count - 1].GetWidth();
        }
        else {
            startX = 0;
        }

        newSection.Init(index, randomWidth, PlatformHeights, TileTemplates[0], startX);
        CurrentWidth += randomWidth;


        SectionList.Insert(rightwards ? SectionList.Count : 0, newSection);
        //SectionList[GetMiddleSectionIndex()].SetTriggerSection(true);
    }

    public int GetMiddleSectionIndex() {
        return Mathf.FloorToInt(SectionCount / 2.0f);
    }

    public void ShiftSections(bool rightwards, int shiftAmount) {
        for (int i = 0; i < SectionList.Count; ++i) {
            SectionList[i].Shift(rightwards, shiftAmount);
        }
    }

    public Pickup GetPickupTemplate(EPickupType type) {
        for (int i = 0; i < PickupTemplates.Count; ++i) {
            if (PickupTemplates[i].Type == type) {
                return PickupTemplates[i].PickupTemplate;
            }
        }
        return null;
    }

    public Pickup SpawnPickup(Vector3 location, Tile parentTile) {
        //Debug.Log("Spawned pickup!");
        Pickup rolledPickup = null;

        float pickupRoll = Random.Range(0.0f, 1.0f);
        if (pickupRoll > 0.5f && PlayerManager.instance != null && PlayerManager.instance.GetPlayerData().ActivatedElements.Count < (int)EElementType.ET_NONE && !IsElementPickupActive()) {
            rolledPickup = Instantiate(GetPickupTemplate(EPickupType.PT_ELEMENT));
            rolledPickup.PickupType = EPickupType.PT_ELEMENT;
            rolledPickup.SetElementType((EElementType)PlayerManager.instance.GetPlayerData().ActivatedElements.Count);
            SetElementPickupActive(true);
            rolledPickup.gameObject.transform.parent = parentTile.gameObject.transform;
            rolledPickup.gameObject.transform.localPosition = new Vector3(0, 0.64f, 0);
            PickupList.Add(rolledPickup);
        }
        else if(PlayerManager.instance != null && PlayerManager.instance.GetPlayerData().ActivatedElements.Contains(EElementType.ET_STAR)){
            rolledPickup = Instantiate(GetPickupTemplate(EPickupType.PT_SCORE));
            rolledPickup.PickupType = EPickupType.PT_SCORE;
            rolledPickup.gameObject.transform.parent = parentTile.gameObject.transform;
            rolledPickup.gameObject.transform.localPosition = new Vector3(0, 0.48f, 0);

            if(PlayerManager.instance.GetPlayerData().ActivatedElements.Contains(EElementType.ET_FIRE)) {
                rolledPickup.Renderer.color = new Color(1.0f, 0.22f, 0.22f, 1.0f);
            }
            PickupList.Add(rolledPickup);
        }



        return rolledPickup;
    }

    public void ShiftPickups(bool rightwards, int shiftAmount) {
        for (int i = 0; i < PickupList.Count; ++i) {
            PickupList[i].Shift(rightwards, shiftAmount);
        }
    }

    public void RemovePickupFromList(Pickup pickup) {
        PickupList.Remove(pickup);

        if (pickup.PickupType == EPickupType.PT_ELEMENT) {
            WorldManager.instance.SetElementPickupActive(false);
        }
    }

    public void Expand(int newCount) {
        for (int i = 0; i < newCount; ++i) {
            SpawnSection(true);
        }
    }

    public bool IsElementPickupActive() {
        return bElementPickupActive;
    }

    public void SetElementPickupActive(bool isActive) {
        bElementPickupActive = isActive;
    }
}
