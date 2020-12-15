using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
 

public class UITransitionManager : MonoBehaviour
{
    
    public Transform MoveToUsePosition;
    public Transform MoveAwayFromUsePosition;

    public void MoveToUse()
    {
        iTween.MoveTo(this.gameObject,iTween.Hash("position",MoveToUsePosition.position,"time",0.25f,"easetype",iTween.EaseType.easeInOutSine));
    }    

    public void MoveAwayFromUse(GameObject MainPanel)
       
    {
        iTween.MoveTo(this.gameObject,iTween.Hash("position", MoveAwayFromUsePosition.position,"time",0.25f,"easetype",iTween.EaseType.easeInOutSine));
        MainPanel.SetActive(true);
    }


}
