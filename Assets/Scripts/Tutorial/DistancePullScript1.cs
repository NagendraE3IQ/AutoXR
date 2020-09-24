using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DistancePullScript1 : MonoBehaviour
{
    public SteamVR_Action_Boolean pickup;
    public SteamVR_Action_Boolean pull;
    public SteamVR_Input_Sources grabhand;
    public LineScript InputModule;

    public GameObject RightHand, NextButton;
    bool inhand = false;
    bool shallpull = false;

    GameObject Obj;
    int count = 0;


    // Start is called before the first frame update
    void Start()
    {     
        pickup.AddOnStateDownListener(pickupObject, grabhand);
        pickup.AddOnStateUpListener(DropObject, grabhand);     
        pull.AddOnStateDownListener(pullObject, grabhand);
        pull.AddOnStateUpListener(leaveObject, grabhand);   
    }

    private void OnDestroy()
    {
        RemoveListners();
    }

    private void OnDisable()
    {
        RemoveListners();
    }

    void RemoveListners()
    {
        pickup.RemoveOnStateDownListener(pickupObject, grabhand);
        pickup.RemoveOnStateUpListener(DropObject, grabhand);
        pull.RemoveOnStateDownListener(pullObject, grabhand);
        pull.RemoveOnStateUpListener(leaveObject, grabhand);
    }

    private void leaveObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        shallpull = false;
    }

    private void pullObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        shallpull = true;
    }

    private void pickupObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (InputModule.gameobj)
        {
            Obj = InputModule.gameobj;

            if (Obj.tag == "Pullable" && !inhand && Vector3.Distance(Obj.transform.position, RightHand.transform.position) > 0.3f)
            {
                Obj.transform.parent = RightHand.transform;
                inhand = true;
            }
        }
    }

    private void DropObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (Obj.tag == "Pullable")
        {
            Obj.transform.parent = null;
            inhand = false;
            count += 1; ;
            if (count > 2)
            {
                NextButton.SetActive(true);
            }
        }        
    }

    private void Update()
    {
        if (shallpull && inhand && Vector3.Distance(Obj.transform.position,RightHand.transform.position)>0.3f)
        {
            Obj.transform.position=Vector3.Lerp(Obj.transform.position, RightHand.transform.position,1.5f* Time.deltaTime);
        }

        if (inhand)
        {
            if (shallpull)
                Obj.GetComponent<Interactable>().enabled = false;
            else
            {
                Obj.GetComponent<Interactable>().enabled = true;
            }
        }
    }
}  
