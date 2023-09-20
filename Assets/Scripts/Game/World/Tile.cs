using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{
    public BoxCollider2D TriggerCollider = null;
    public EdgeCollider2D GroundCollider = null;
    public EdgeCollider2D LeftEdgeCollider = null;
    public EdgeCollider2D RightEdgeCollider = null;

    int Height = 0;

    Section TileSection = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int height, Section section) {
        Height = height;
        TileSection = section;
    }

    public int GetHeight() {
        return Height;
    }



}
