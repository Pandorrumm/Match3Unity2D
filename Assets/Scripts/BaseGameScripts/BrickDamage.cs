using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDamage : MonoBehaviour {

    public int hitsToBreak;
    public Sprite hitSprite;

    void Start () {
		
	}

    public void BreakBrick()
    {
        hitsToBreak--;    
        GetComponent<SpriteRenderer>().sprite = hitSprite;
    }
}
