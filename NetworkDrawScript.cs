using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDrawScript : MonoBehaviour
{
   // public static NetworkDrawScript instance;
    int NumClicks = 0;
    LineRenderer CurrentLine;
    bool IsDraw = false;
    public Material LineMaterial;
    PhotonView PV;
    private void OnEnable()
    {
      //  instance = this;
        PV = GetComponent<PhotonView>();
    }
        
    public void StartNetworkDrawRPC()
    {
        if(PV.IsMine)
        {
         PV.RPC("RPC_StartDrawing", RpcTarget.AllBufferedViaServer);
        }

    }
    public void StoptNetworkDrawRPC()
    {
        if(PV.IsMine)
        {
        PV.RPC("RPC_StopDrawing", RpcTarget.AllBufferedViaServer);
        }

    }

    [PunRPC]
    public void RPC_StartDrawing()
    {
        IsDraw = true;
        GameObject SprayLine = new GameObject();
        CurrentLine = SprayLine.AddComponent<LineRenderer>();
        CurrentLine.material = LineMaterial;
        CurrentLine.SetWidth(0.01f, 0.01f);
    }
        

     
      
    [PunRPC]
    public void RPC_StopDrawing()
    {
        NumClicks = 0;
        IsDraw = false;
        Debug.Log(GetComponent<PhotonView>().ViewID);
    }




    private void Update()
    {
        if (IsDraw)
        {
            CurrentLine.positionCount = NumClicks + 1;
            CurrentLine.SetPosition(NumClicks, transform.position);
            NumClicks++;
        }
    }


}
