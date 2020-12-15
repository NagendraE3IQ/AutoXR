using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class CopyScript : MonoBehaviour
{
    public int index;
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PhotonView>().IsMine)
        {

            switch(index)
            {
                case 1:
                    transform.position = CameraRigManager.instance.Head.transform.position;
                    transform.rotation = CameraRigManager.instance.Head.transform.rotation;
                    transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
                    break;
                case 2:
                    transform.position = CameraRigManager.instance.LeftHand.transform.position;
                    transform.rotation = CameraRigManager.instance.LeftHand.transform.rotation;
                    break;
                case 3:
                    transform.position = CameraRigManager.instance.RightHand.transform.position;
                    transform.rotation = CameraRigManager.instance.RightHand.transform.rotation;
                    break;
            }
           
        }
    }

}
