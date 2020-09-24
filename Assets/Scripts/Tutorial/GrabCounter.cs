using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCounter : MonoBehaviour
{
    public GameObject NextButton;
    public int cubecount, grenadecount;

    // Start is called before the first frame update
    void Start()
    {
    
    }

  //Update is called once per frame
  public void IncreaseCubeGrabCount()
   {
       cubecount++;
        if (grenadecount >= 2 && cubecount >= 2)
        {
            NextButton.SetActive(true);
        }
    }

    public void IncreaseGrenadeGrabCount()
    {
        grenadecount++;

        if (grenadecount >= 2 && cubecount >= 2)
        {
            NextButton.SetActive(true);
        }
    }
}
