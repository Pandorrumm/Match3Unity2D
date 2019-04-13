using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBackground : MonoBehaviour
{
    public GameObject[] circle;

	void Start ()
    {
        Initialize();

    }
	
	
	void Update ()
    {
		
	}
    void Initialize()
    {
        //int dotToUse = Random.Range(0, circle.Length);
        //GameObject circl = Instantiate(circle[dotToUse], transform.position, Quaternion.identity);
        //circl.transform.parent = this.transform;
        //circl.name = this.gameObject.name;
    }
}
