using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;
public class DrawManagerScript : MonoBehaviour
{
    public static DrawManagerScript instance;
    public SteamVR_Action_Boolean Spraytrigger;
    public SteamVR_Input_Sources SprayHand;
    public GameObject Hand;
   public bool IsSpray = false;
    int NumClicks = 0;
    LineRenderer CurrentLine;
    public Material LineMaterial;
  public  bool isCanOn = false;

    //Double Click Check
    float FirstclickTime, TimeBetweenClicks;
    private bool CoroutineAllowed;
    private int ClickCounter;
    public GameObject SprayCan;
    PhotonView pView;


  
    
    void Start()
    {
        instance = this;
        FirstclickTime = 0;
        TimeBetweenClicks = 0.5f;
        ClickCounter = 0;
        CoroutineAllowed = true;
        Spraytrigger.AddOnStateDownListener(StartSpray, SprayHand);
        Spraytrigger.AddOnStateUpListener(StopSpray, SprayHand);
       
    }

    
    private void StartSpray(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(isCanOn)
        {
            IsSpray = true;
            NetworkDrawScript[] scripts = FindObjectsOfType<NetworkDrawScript>();
            foreach (NetworkDrawScript nds in scripts)
            {
                nds.StartNetworkDrawRPC();
            }
        }
    }

    



    private void StopSpray(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ClickCounter +=1;
       if(isCanOn)
        {
            NetworkDrawScript[] scripts = FindObjectsOfType<NetworkDrawScript>();
                foreach(NetworkDrawScript nds in scripts)
            {
                nds.StoptNetworkDrawRPC();
            }
            IsSpray = false;

        }

    }
    // Update is called once per frame
    void Update()
    {
        if(ClickCounter==1 && CoroutineAllowed)
        { 
            FirstclickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }
        
    }


    private IEnumerator DoubleClickDetection()
    {
        CoroutineAllowed = false;
        while(Time.time<FirstclickTime+TimeBetweenClicks)
        {
            if(ClickCounter==2)
            {
                Debug.Log("Doubleclick");
                if(!isCanOn)
                {
                   
                    isCanOn = true;
                    SprayCan.SetActive(true);
                    
                }
                else
                {
                    isCanOn = false;
                   SprayCan.SetActive(false);
                }

                break;

            }
        yield return new WaitForEndOfFrame();
        }
        ClickCounter = 0;
        FirstclickTime = 0f;
        CoroutineAllowed = true;
        

    }
}
