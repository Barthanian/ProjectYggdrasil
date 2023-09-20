using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    int SectionWidth;
    Tile TileTemplate;
    List<int> Heights = new List<int>();

    List<Tile> Tiles = new List<Tile>();

    Tile RightEndTile = null;
    Tile LeftEndTile = null;

    int SectionIndex;
    int StartX;
    Vector3 StartPosition;
    Vector3 EndPosition;

    bool bIsTriggerSection = false;

    Pickup RolledPickup = null;


    // Update is called once per frame
    void Update() {
        if (IsTriggerSection()) {
            if (CheckIfPastRightSide()) {
                WorldManager.instance.GenerateSection(true);
            }
            if (CheckIfPastLeftSide()) {
                WorldManager.instance.GenerateSection(false);
            }
        }
    }

    public void Init(int index, int sectionWidth, List<int> heights, Tile tileTemplate, int startX) {
        SectionIndex = index;
        Heights = heights;
        TileTemplate = tileTemplate;
        SectionWidth = sectionWidth;
        StartX = startX;

        StartPosition = new Vector3(startX * TileTemplate.GetComponent<SpriteRenderer>().size.x + 0.5f * TileTemplate.GetComponent<SpriteRenderer>().size.x, 0, 0);
        EndPosition = new Vector3((startX + SectionWidth ) * TileTemplate.GetComponent<SpriteRenderer>().size.x + 0.5f * TileTemplate.GetComponent<SpriteRenderer>().size.x, 0, 0);

        bool pickupRoll = Random.Range(0.0f, 1.0f) > 0.90f;
        int pickupTileIndex = Random.Range(1, SectionWidth - 1);
        for (int j = 0; j < Heights.Count; ++j) {
            for (int i = 1; i < SectionWidth - 1; ++i) {
                
                Tile newTile = GameObject.Instantiate(TileTemplate);
                newTile.gameObject.transform.parent = gameObject.transform;
                newTile.gameObject.transform.position = GetWorldPositionByCoordinates(StartX + i, Heights[j], TileTemplate.GetComponent<SpriteRenderer>().size.x);
                newTile.Init(Heights[j], this);
                if(pickupRoll && i == pickupTileIndex) {
                    RolledPickup = WorldManager.instance.SpawnPickup(GetWorldPositionByCoordinates(StartX + i, Heights[j], TileTemplate.GetComponent<SpriteRenderer>().size.x) + new Vector3(0, 0.48f, 0), newTile);
                }

                if(i == 1) {
                    LeftEndTile = newTile;
                } else if(i == SectionWidth - 2) {
                    RightEndTile = newTile;
                }

                Tiles.Add(newTile);
            }
        }
    }

    public void SetTriggerSection(bool isTrigger) {
        bIsTriggerSection = isTrigger;
    }

    public bool IsTriggerSection() {
        return bIsTriggerSection;
    }

    public static Vector3 GetWorldPositionByCoordinates(int x, int y, float tileSize) {
        return new Vector3(x * tileSize + 0.5f * tileSize, y * tileSize + 0.5f * tileSize, 0);
    }

    public int GetStartX() {
        return StartX;
    }

    public int GetWidth() {
        return SectionWidth;
    }

    public void Shift(bool rightwards, int shiftAmount) {
        if(rightwards) {
            SectionIndex--;
            StartX -= shiftAmount;
        }
        else {
            SectionIndex++;
            StartX += shiftAmount;
        }

        for (int i = 1; i < SectionWidth - 1; ++i) {
            Tiles[i - 1].gameObject.transform.position = GetWorldPositionByCoordinates(StartX + i, Tiles[i - 1].GetHeight(), TileTemplate.GetComponent<SpriteRenderer>().size.x);
        }

        StartPosition = new Vector3(StartX * TileTemplate.GetComponent<SpriteRenderer>().size.x + 0.5f * TileTemplate.GetComponent<SpriteRenderer>().size.x, 0, 0);
        EndPosition = new Vector3((StartX + SectionWidth) * TileTemplate.GetComponent<SpriteRenderer>().size.x + 0.5f * TileTemplate.GetComponent<SpriteRenderer>().size.x, 0, 0);

    }

    public Vector3 GetCenterTilePosition() {
        return Tiles[Mathf.RoundToInt(Tiles.Count / 2)].gameObject.transform.position;
    }

    public bool CheckIfPastRightSide() {
        if (CharacterPlayer.instance.gameObject.transform.position.x > EndPosition.x) {
            return true;
        }
        return false;
    }

    public bool CheckIfPastLeftSide() {
        if (CharacterPlayer.instance.gameObject.transform.position.x < StartPosition.x) {
            return true;
        }
        return false;
    }
}
