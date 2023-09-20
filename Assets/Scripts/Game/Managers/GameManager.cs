using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase {
    public static GameManager instance;
    private void Awake() {
        instance = this;
    }


    public List<ManagerBase> ManagerTemplates = new List<ManagerBase>();
    public CharacterPlayer PlayerTemplate = null;
    public int StartingLives;


    // Start is called before the first frame update
    void Start() {
        InitManager();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void InitManager() {
        for (int i = 0; i < ManagerTemplates.Count; ++i) {
            ManagerBase newManager = Instantiate(ManagerTemplates[i]);
            newManager.InitManager();
        }
        InitPlayer();

        PlayerManager.instance.NotifyGameStart(false);
    }

    void InitPlayer() {
        CharacterPlayer player = Instantiate(PlayerTemplate);
        player.gameObject.transform.position = WorldManager.instance.GetStartLocation();
    }

    public void NotifyGameOver() {

        PlayerManager.instance.GetPlayerData().Lives = PlayerManager.instance.GetPlayerData().Lives - 1;

        if(PlayerManager.instance.GetPlayerData().Lives == 0)
        {
            PlayerManager.instance.NotifyGameOver();

            PlayerManager.instance.NotifyGameStart(true);

            StarManager.instance.DeactivateStars();

            StarManager.instance.DeactivateMoon();

            StarManager.instance.DeactivateSun();
        }

        CharacterPlayer.instance.SetLocation(WorldManager.instance.GetStartLocation());

        WorldManager.instance.SetSectionWidthMinMax(5, 15);
        WorldManager.instance.SpawnSections();

        HUD.instance.SetLives(PlayerManager.instance.GetPlayerData().Lives);


    }
}
