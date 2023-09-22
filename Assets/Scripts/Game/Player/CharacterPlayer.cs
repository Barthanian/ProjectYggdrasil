using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveAxis {
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,
}

public class CharacterPlayer : CharacterBase {
    public static CharacterPlayer instance;
    public Rigidbody2D CharacterRigidBody;
    public TrailRenderer CharacterTrail;
    public float JumpForce;
    public Camera PlayerCamera;

    bool bCanDoubleJump = false;
    float CurrentSpeed = 1.6f;

    Vector3 LastPosition;
    List<Collider2D> TouchedTiles = new List<Collider2D>();
    Animator animator;


    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource Landsfx;
    bool Soprikos = false;
    // Start is called before the first frame update

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.position.y < -10.0f) {
            GameManager.instance.NotifyGameOver();
        }

        float movedDistance = (gameObject.transform.position.x - LastPosition.x);

        if(CurrentSpeed < GetMaxSpeed()) {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed + Time.deltaTime / 10.0f, 1.6f, GetMaxSpeed()); 
        }
        if(CurrentSpeed > GetMaxSpeed()) {
            CurrentSpeed = GetMaxSpeed();
        }
    }

    public float GetMaxSpeed() {
        return GetBaseSpeed() * GetSpeedMultiplier();
    }


    public void Jump() {
        if(!CanJump() && !CanDoubleJump()) {
            return;
            
        }
        jumpSoundEffect.Play();
        bCanDoubleJump = !bCanDoubleJump;
        //CharacterRigidBody.AddForce(new Vector3(0, 100, 0), ForceMode2D.Impulse);
        CharacterRigidBody.velocity = new Vector2(CharacterRigidBody.velocity.x, JumpForce);
        Soprikos = false;

    }

    public void SetLocation(Vector3 location) {
        Vector3 diff = location - gameObject.transform.position;

        if (IsTrailEnabled())
        {
            SetTrail(diff);
        }

        gameObject.transform.position = location;

        

        LastPosition = location;
    }

    public void Move(MoveAxis moveAxis) {
        float speed = GetSpeed();
        switch (moveAxis) {
            case MoveAxis.MOVE_UP:
                CharacterRigidBody.velocity = new Vector2(CharacterRigidBody.velocity.x, speed);
                break;
            case MoveAxis.MOVE_DOWN:
                CharacterRigidBody.velocity = new Vector2(CharacterRigidBody.velocity.x, -speed);
                break;
            case MoveAxis.MOVE_LEFT:
                //CharacterRigidBody.MovePosition(gameObject.transform.position - new Vector3(1f, 0, 0) * Time.deltaTime);
                CharacterRigidBody.velocity = new Vector2(-speed, CharacterRigidBody.velocity.y);
                animator.SetBool("isRunning", true);
                break;
            case MoveAxis.MOVE_RIGHT:
                CharacterRigidBody.velocity = new Vector2(speed, CharacterRigidBody.velocity.y);
                animator.SetBool("isRunning", true);

                //CharacterRigidBody.MovePosition(gameObject.transform.position + new Vector3(1f, 0, 0) * Time.deltaTime);
                break;
            default:
                animator.SetBool("isRunning", false);
                break;

        }
    }

    public void Shift(bool rightwards, int shiftAmount, float tileSize) {
        //CharacterTrail.enabled = false;
        Vector3 shiftVector = new Vector3((rightwards ? -shiftAmount : shiftAmount) * tileSize, 0, 0);
        SetLocation(gameObject.transform.position + shiftVector);
        
        //StartCoroutine(SetTrail());



    }

    public void SetTrailEnabled(bool isEnabled) {
        CharacterTrail.enabled = isEnabled;
    }

    public bool IsTrailEnabled() {
        return CharacterTrail.enabled;
    }

    void SetTrail(Vector3 shiftVector) {
            
        //CharacterTrail.time = 1;

        Vector3[] positions = new Vector3[CharacterTrail.positionCount];
        CharacterTrail.GetPositions(positions);
        CharacterTrail.emitting = false;
        CharacterTrail.Clear();
        //CharacterTrail.time = 0;

        Debug.Log("SHIFT");
        CharacterTrail.emitting = true;
        for (int i = 0; i < positions.Length; ++i) {
            CharacterTrail.AddPosition(positions[i] + shiftVector);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider == collision.gameObject.GetComponent<Tile>().GroundCollider)
        {
            TouchedTiles.Add(collision.collider);
            bCanDoubleJump = false;
            if (Soprikos == false)
            {
                Soprikos = true;
                Landsfx.Play();
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision) {

        if (collision.collider == collision.gameObject.GetComponent<Tile>().GroundCollider) {
            TouchedTiles.Remove(collision.collider);
        }
    }

    public bool CanJump() {
        return TouchedTiles.Count > 0;
    }

    public bool CanDoubleJump() {
        return bCanDoubleJump && PlayerManager.instance.GetPlayerData().ActivatedElements.Contains(EElementType.ET_MOON);
    }

    public float GetSpeed() {
        return CurrentSpeed;
    }

    public float GetBaseSpeed() {
        return 1.6f;
    }

    public float GetSpeedMultiplier() {
        return PlayerManager.instance.GetBuffValue(EStat.STAT_SPEED);
    }
}