using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementSpritePair {
    public EElementType ElementType = EElementType.ET_NONE;
    public Sprite ElementSprite = null;
}

public class AssetManager : ManagerBase
{
    public static AssetManager instance;

    private void Awake() {
        instance = this;
    }

    public List<ElementSpritePair> ElementSpritePairs = new List<ElementSpritePair>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetElementSprite(EElementType type) {
        for(int i = 0; i < ElementSpritePairs.Count; ++i) {
            if(ElementSpritePairs[i].ElementType == type) {
                return ElementSpritePairs[i].ElementSprite;
            }
        }
        return null;
    }
}
