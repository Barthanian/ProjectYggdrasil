using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : ManagerBase
{

    public static StarManager instance;

    private void Awake() {
        instance = this;
    }

    public List<Star> StarTemplates = new List<Star>();
    public GameObject SunTemplate = null;
    public GameObject MoonTemplate = null;

    public float MinStarSpawnTime = 1.0f;

    public float MaxStarSpawnTime = 5.0f;

    float RemainingStarSpawnTime = 0.0f;

    bool bStarsActivated = false;

    List<Star> StarList = new List<Star>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bStarsActivated) {
            RemainingStarSpawnTime -= Time.deltaTime;
            if (RemainingStarSpawnTime < 0) {
                SpawnStar();
            }
        }
    }

    public override void InitManager() {
        base.InitManager();

    }

    public void SpawnStar() {
        RemainingStarSpawnTime = Random.Range(MinStarSpawnTime, MaxStarSpawnTime);
        Star newStar = Instantiate(StarTemplates[Random.Range(0, StarTemplates.Count)]);
        newStar.InitStar(Random.Range(2.0f, 5.0f));
        StarList.Add(newStar);
    }

    public void RemoveStar(Star star) {
        StarList.Remove(star);
        Destroy(star.gameObject);
    }

    public void ActivateStars() {
        bStarsActivated = true;
        SpawnStar();
    }

    public void DeactivateStars() {
        bStarsActivated = false;
        for(int i = StarList.Count - 1; i >= 0; --i) {
            StarList[i].SetWaning();
        }
    }

    public void ActivateSun() {
        HUD.instance.FadeInSun();
    }

    public void DeactivateSun() {
        HUD.instance.FadeOutSun();
    }

    public void ActivateMoon() {
        HUD.instance.FadeInMoon();
    }

    public void DeactivateMoon() {
        HUD.instance.FadeOutMoon();
    }

}
