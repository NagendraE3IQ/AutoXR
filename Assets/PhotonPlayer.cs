using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;
public class PhotonPlayer : MonoBehaviour
{

    private PhotonView PV;
    public GameObject HeadPrefab;
    public GameObject LeftHandPrefab;
    public GameObject RightHandPrefab;

    public static PhotonPlayer Instance;
   
   
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if(PV.IsMine)
        {
           GameObject a=  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", HeadPrefab.name) , CameraRigManager.instance.Head.transform.position, CameraRigManager.instance.transform.rotation, 0);
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);
            a.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName; 
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", LeftHandPrefab.name), CameraRigManager.instance.LeftHand.transform.position, CameraRigManager.instance.LeftHand.transform.rotation, 0);
             PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", RightHandPrefab.name), CameraRigManager.instance.RightHand.transform.position, CameraRigManager.instance.RightHand.transform.rotation, 0);
            
        }
    }

}
