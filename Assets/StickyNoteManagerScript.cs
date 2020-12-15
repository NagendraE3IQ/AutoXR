using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;
using System.IO;

public class StickyNoteManagerScript : MonoBehaviour
{
    float FirstclickTime, TimeBetweenClicks;
    private bool CoroutineAllowed;
    private int ClickCounter;
    public GameObject StickyNotes;
    public SteamVR_Action_Boolean StickyNoteTrigger;
    public SteamVR_Action_Boolean SpawnStickyNote;
    public SteamVR_Input_Sources StickyNoteHand;
    public SteamVR_Action_Vector2 StickNoteSelect;
    bool isNoteEnable = false;
    int SelectedNoteID;
    public GameObject[] StickyNotePrefabs;


    public Transform StickyNoteSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        FirstclickTime = 0;
        TimeBetweenClicks = 0.5f;
        CoroutineAllowed = true;
        ClickCounter = 0;
        StickyNoteTrigger.AddOnStateDownListener(EnableNotes, StickyNoteHand);
        StickyNoteTrigger.AddOnStateUpListener(Disablenotes, StickyNoteHand);
        StickNoteSelect.AddOnAxisListener(GetSelectedNote, StickyNoteHand);
        SpawnStickyNote.AddOnStateDownListener(SpawnStickyNotefunction, StickyNoteHand);

    }

    private void SpawnStickyNotefunction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (SelectedNoteID)
        {
            case 0:
                if (isNoteEnable)
                StickyNote();
                Debug.Log("Accepted");
                break;
            case 1:
                if(isNoteEnable)
                StickyNote();
                Debug.Log("waiting");
                break;

            case 2:
                if(isNoteEnable)
                StickyNote();
                Debug.Log("rejected");
                break;


        }
    }

    private void GetSelectedNote(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {

        if (axis.y > 0f)
        {
            SelectedNoteID = 0;
        }
        else if (axis.x < 0f)
        {
            SelectedNoteID =2 ;
        }
        else if (axis.x > 0f)
        {
            SelectedNoteID = 1;
        }

    }




    private void Disablenotes(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ClickCounter += 1;
        // throw new NotImplementedException();
    }

    private void EnableNotes(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        // throw new NotImplementedException();
    }

    private IEnumerator DoubleClickDetection()
    {
        CoroutineAllowed = false;
        while (Time.time < FirstclickTime + TimeBetweenClicks)
        {
            if (ClickCounter == 2)
            {
                Debug.Log("Doubleclick");
                if (!isNoteEnable)
                {

                    isNoteEnable = true;
                    StickyNotes.SetActive(true);

                }
                else
                {
                    isNoteEnable = false;
                    StickyNotes.SetActive(false);
                }

                break;

            }
            yield return new WaitForEndOfFrame();
        }
        ClickCounter = 0;
        FirstclickTime = 0f;
        CoroutineAllowed = true;


    }

   
    public void StickyNote()
    {
        Debug.Log(SelectedNoteID);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", StickyNotePrefabs[SelectedNoteID].name),StickyNoteSpawnPoint.position,StickyNoteSpawnPoint.rotation);
    }


    // Update is called once per frame
    void Update()
    {
        if (ClickCounter == 1 && CoroutineAllowed)
        {
            FirstclickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }
    }
}
