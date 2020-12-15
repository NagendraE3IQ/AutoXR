using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkTransformTransmissionManager : MonoBehaviour
{
    public static NetworkTransformTransmissionManager instance;
    public Transform LoadedGameObjectTransform;
    private void Start()
    {
        instance = this;
    }

    public void UpdateTransform(Transform AssetTransform)
    {
        LoadedGameObjectTransform.position = AssetTransform.position;
        LoadedGameObjectTransform.rotation = AssetTransform.rotation;
    }
    [PunRPC]
    public void RPC_UpdateTransform()
    {

    }


}
