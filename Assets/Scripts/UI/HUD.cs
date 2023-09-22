using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ELerpType {
    LERP_UP,
    LERP_DOWN,
}

public class Lerper {
    public float LerpAmount = 0.0f;
    public ELerpType LerpType = ELerpType.LERP_DOWN;

    public void Lerp(float delta) {
        switch (LerpType) {
            case ELerpType.LERP_UP:
                LerpAmount = Mathf.Min(LerpAmount + delta, 1.0f);
                break;
            case ELerpType.LERP_DOWN:
                LerpAmount = Mathf.Max(LerpAmount - delta, 1.0f);
                break;
        }
    }
}

public enum EMotivationType {
    MOT_GENERIC,
    MOT_HIGH_SCORE,
}

public class HUD : MonoBehaviour {
    public static HUD instance;

    private void Awake() {
        instance = this;
    }

    public Text Duration;
    public Text TextMotivation;
    public Text highScoreText;
    [SerializeField]
    Text Lives;
    

    public RawImage ImageSun;
    public RawImage ImageMoon;

    float SunLerp = 0.0f;
    float MoonLerp = 0.0f;
    float MotivationLerp = 0.0f;

    bool bIsSunFadingIn = false;
    bool bIsMoonFadingIn = false;


    bool bIsSunFadingOut = false;
    bool bIsMoonFadingOut = false;


    bool bIsMotivationFadingIn = false;
    bool bIsMotivationFadingOut = false;

    public List<string> GenericMotivations = new List<string>();
    public List<string> HighScoreReachedMotivations = new List<string>();

    // Start is called before the first frame update
    void Start() {
        SetLives(GameManager.instance.StartingLives);
    }

    // Update is called once per frame
    void Update() {
        Duration.text = PlayerManager.instance.GetPlayerData().Duration.ToString("F2");
        

        if (bIsSunFadingIn) {
            SunLerp = Mathf.Clamp(SunLerp + Time.deltaTime, 0.0f, 1.0f);
            ImageSun.color = new Color(ImageSun.color.r, ImageSun.color.g, ImageSun.color.b, SunLerp);
            if (SunLerp == 1.0f) {
                bIsSunFadingIn = false;
            }
        }
        if (bIsSunFadingOut) {
            SunLerp = Mathf.Clamp(SunLerp - Time.deltaTime, 0.0f, 1.0f);
            ImageSun.color = new Color(ImageSun.color.r, ImageSun.color.g, ImageSun.color.b, SunLerp);
            if (SunLerp == 0.0f) {
                bIsSunFadingOut = false;
            }
        }

        if (bIsMoonFadingIn) {
            MoonLerp = Mathf.Clamp(MoonLerp + Time.deltaTime / 3.0f, 0.0f, 1.0f);
            ImageMoon.color = new Color(ImageMoon.color.r, ImageMoon.color.g, ImageMoon.color.b, MoonLerp * 0.5f);
            if (MoonLerp == 1.0f) {
                bIsMoonFadingIn = false;
            }
        }
        if (bIsMoonFadingOut) {
            MoonLerp = Mathf.Clamp(MoonLerp - Time.deltaTime / 3.0f, 0.0f, 1.0f);
            ImageMoon.color = new Color(ImageMoon.color.r, ImageMoon.color.g, ImageMoon.color.b, MoonLerp);
            if (MoonLerp == 0.0f) {
                bIsMoonFadingOut = false;
            }
        }

        if (bIsMotivationFadingIn) {
            MotivationLerp = Mathf.Clamp(MotivationLerp + Time.deltaTime, 0.0f, 1.0f);
            TextMotivation.color = new Color(ImageMoon.color.r, ImageMoon.color.g, ImageMoon.color.b, MotivationLerp);
            if (MotivationLerp == 1.0f) {
                bIsMotivationFadingIn = false;
                FadeOutMotivation();
            }
        }
        if (bIsMotivationFadingOut) {
            MotivationLerp = Mathf.Clamp(MotivationLerp - Time.deltaTime, 0.0f, 1.0f);
            TextMotivation.color = new Color(ImageMoon.color.r, ImageMoon.color.g, ImageMoon.color.b, MotivationLerp);
            if (MotivationLerp == 0.0f) {
                bIsMotivationFadingOut = false;
            }
        }
    }

    public void SetLives()
    {
        Lives.text = PlayerManager.instance.GetPlayerData().Lives.ToString();
    }

    public void SetLives(int NewLives)
    {
        Lives.text = NewLives.ToString();
    }

    public void FadeInSun() {
        bIsSunFadingIn = true;
        bIsSunFadingOut = false;
    }

    public void FadeInMoon() {
        bIsMoonFadingIn = true;
        bIsMoonFadingOut = false;
    }

    public void FadeOutSun() {
        bIsSunFadingIn = false;
        bIsSunFadingOut = true;
    }

    public void FadeOutMoon() {
        bIsMoonFadingIn = false;
        bIsMoonFadingOut = true;
    }

    public void FadeInMotivation(EMotivationType motivationType, string message = "") {

        if (!bIsMotivationFadingIn) {
            if (message == "") {
                switch (motivationType) {
                    case EMotivationType.MOT_GENERIC:
                        TextMotivation.text = GenericMotivations[Random.Range(0, GenericMotivations.Count - 1)];
                        break;
                    case EMotivationType.MOT_HIGH_SCORE:
                        TextMotivation.text = HighScoreReachedMotivations[Random.Range(0, GenericMotivations.Count - 1)];
                        break;
                }
            }
            else {
                TextMotivation.text = message;
            }

            bIsMotivationFadingIn = true;
            bIsMotivationFadingOut = false;
        }
    }

    public void FadeOutMotivation() {
        bIsMotivationFadingIn = false;
        bIsMotivationFadingOut = true;
    }
}