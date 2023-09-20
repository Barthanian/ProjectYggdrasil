using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerBase {
    public static InputManager instance;
    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            CharacterPlayer.instance.Move(MoveAxis.MOVE_UP);
        }
        if (Input.GetKey(KeyCode.A)) {
            CharacterPlayer.instance.Move(MoveAxis.MOVE_LEFT);
        }
        if (Input.GetKey(KeyCode.S)) {
            CharacterPlayer.instance.Move(MoveAxis.MOVE_DOWN);
        }
        if (Input.GetKey(KeyCode.D)) {
            CharacterPlayer.instance.Move(MoveAxis.MOVE_RIGHT);
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            CharacterPlayer.instance.Jump();
        }

        //CharacterPlayer.instance.Move(MoveAxis.MOVE_RIGHT);
    }
}
