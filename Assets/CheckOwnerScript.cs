using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR.InteractionSystem;
public class CheckOwnerScript : MonoBehaviour
{
     Interactable _interactable;
      
    private void OnEnable()
    {
        if(GetComponent<PhotonView>().IsMine && gameObject.CompareTag("Hand"))
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

       else if(GetComponent<Interactable>())
        {

            _interactable = GetComponent<Interactable>();
             _interactable.IsLocallyControlled = GetComponent<PhotonView>().IsMine;
            if (!_interactable.IsLocallyControlled)
            {
             GetComponent<Throwable>().enabled = false;
            }
        }
    }
}
