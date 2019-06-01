using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsAnimations : MonoBehaviour
{
    public GameObject stars;
    public GameObject starsA;
    public GameObject starsB;

    public void StartAnimationsBigLitl()
    {
        stars.GetComponent<Animation>().Play("StarsBigLitl");
    }
    public void StartAnimationsBigLitlA()
    {
        starsA.GetComponent<Animation>().Play("StarsBigLitlA");
    }
    public void StartAnimationsBigLitlB()
    {
        starsB.GetComponent<Animation>().Play("StarsBigLitlB");
    }
}
