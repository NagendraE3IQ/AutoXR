using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigManager : MonoBehaviour
{

    public GameObject Head;
    public GameObject LeftHand;
    public GameObject RightHand;

    public static CameraRigManager instance;


    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        if(instance==this)
        {
            instance = null; 
        }
    }
    
}
