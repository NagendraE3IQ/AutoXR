using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLib;

public class FitToBoundsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ShouldFitToBounds = false;
    GameObject LoadedModel;
    public static FitToBoundsScript instance;
    void Start()
    {
        instance = this;
        if(ShouldFitToBounds)
        {
             Debug.Log("Fittobounds!");
            Debug.Log("Asset Loaded" + Camera.main.name);
        }
    }   


    public void FitToBounds(GameObject x)
    {
        LoadedModel = x;
        Debug.Log(LoadedModel.gameObject.name);
        Camera.main.FitToBounds(LoadedModel.transform, 4f);
        RoomIDManager.instance.UiCanvas.GetComponent<Canvas>().planeDistance = Camera.main.farClipPlane - LoadedModel.transform.localScale.x;

    }
}
           
