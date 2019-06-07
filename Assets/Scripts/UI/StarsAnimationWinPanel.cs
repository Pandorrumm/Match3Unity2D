using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsAnimationWinPanel : MonoBehaviour {

    public GameObject stars;
    public GameObject starsA;
    public GameObject starsB;

    public void StartAnimationsWinPanelBigLitl()
    {
        stars.GetComponent<Animation>().Play("StarsBigLitlFadePanel");
    }
    public void StartAnimationsWinPanelBigLitllA()
    {
        starsA.GetComponent<Animation>().Play("StarsBigLitlFadePanelA");
    }
    public void StartAnimationsWinPanelBigLitlB()
    {
        starsB.GetComponent<Animation>().Play("StarsBigLitlFadePanelB");
    }
}
