using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
    public SpriteRenderer Renderer;
    public float MinSpeed = 0.1f;
    public float MaxSpeed = 1.0f;

    public float MinLifetime = 30.0f;
    public float MaxLifetime = 60.0f;

    float RemainingLifetime;
    float Speed = 0;

    bool bIsWaning = false;
    bool bIsRising = false;

    public float MinFadeinTime = 1.0f;
    public float MaxFadeinTime = 15.0f;

    float FadeTime = 0.0f;
    float FadeLerp = 0.0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        gameObject.transform.localPosition = gameObject.transform.localPosition - new Vector3(Time.deltaTime * GetSpeed(), 0, 0);

        if (bIsRising) {
            FadeLerp += Time.deltaTime / FadeTime;
            Renderer.color = Color.Lerp(new Color(1, 1, 1, 0.0f), new Color(1, 1, 1, 1.0f), FadeLerp);

            if (FadeLerp >= 1.0f) {
                bIsRising = false;
            }
        }

        if (bIsWaning) {
            FadeLerp += Time.deltaTime / FadeTime;
            Renderer.color = Color.Lerp(new Color(1, 1, 1, 1.0f), new Color(1, 1, 1, 0.0f), FadeLerp);

            if (FadeLerp >= 1.0f) {
                StarManager.instance.RemoveStar(this);

            }
        }

        RemainingLifetime -= Time.deltaTime;

        if (RemainingLifetime <= 0) {
            FadeLerp = 0.0f;
            bIsRising = false;
            bIsWaning = true;
        }
    }

    public void SetWaning() {
        if(!bIsWaning) {
            bIsRising = false;
            bIsWaning = true;
            FadeLerp = 0.0f;
        }
    }

    public void InitStar(float height) {
        Renderer.color = new Color(1, 1, 1, 0);

        gameObject.transform.parent = CharacterPlayer.instance.gameObject.transform;
        gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(15.0f, height, 0);

        RemainingLifetime = Random.Range(MinLifetime, MaxLifetime);
        Speed = Random.Range(MinSpeed, MaxSpeed);
        FadeTime = Random.Range(MinFadeinTime, MaxFadeinTime);


        bIsRising = true;
    }

    public float GetSpeed() {
        return Speed * (1.0f + CharacterPlayer.instance.GetSpeedMultiplier() / 5);
    }

    private void OnDestroy() {
    }
}
