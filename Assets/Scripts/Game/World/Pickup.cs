using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPickupType {
    PT_SCORE,
    PT_ELEMENT,
}

public enum EElementType {
    ET_STAR,
    ET_SUN,
    ET_MOON,
    ET_WIND,
    ET_FIRE,
    ET_NONE,
}

public class Pickup : MonoBehaviour
{
    public EPickupType PickupType;
    public SpriteRenderer Renderer;

    EElementType ElementType = EElementType.ET_NONE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == CharacterPlayer.instance.gameObject) {
            //  Debug.Log("Handle Pickup!");
            PlayerManager.instance.HandlePickup(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }

    public void SetElementType(EElementType elementType) {
        ElementType = elementType;
        Renderer.sprite = AssetManager.instance.GetElementSprite(ElementType);
    }

    public void Shift(bool rightwards, int shiftAmount) {
        gameObject.transform.position = gameObject.transform.position + new Vector3((rightwards ? -shiftAmount : shiftAmount) * WorldManager.instance.TileTemplates[0].GetComponent<SpriteRenderer>().size.x, 0, 0);
    }

    private void OnDestroy() {
        WorldManager.instance.RemovePickupFromList(this);
    }

    public EElementType GetElement() {
        return ElementType;
    }


}
