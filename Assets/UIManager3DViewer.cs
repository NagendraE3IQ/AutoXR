using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLib;
//using UI.ThreeDimensional;
using UnityEngine.SceneManagement;

public class UIManager3DViewer : MonoBehaviour
{
   
    public static UIManager3DViewer Instance;

    public GameObject CreateRoomButton;
    public GameObject LoadedGameObjectParent;
    public float speed;
    public GameObject ModelPreview;

    
    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("3DviewerMenuScene"));
        Debug.Log(SceneManager.GetActiveScene().name);
       Invoke("LoadAssetIntoCanvas",1f);
    }

    public void LoadAssetIntoCanvas()
    {
        Debug.Log(PlayerPrefs.GetString("3DViewerModelPath"));
        AssetLoader _assetLoader = new AssetLoader();
        AssetLoaderOptions _assetLoaderOptions = new AssetLoaderOptions();
       // _assetLoaderOptions.
        GameObject a = _assetLoader.LoadFromFile(PlayerPrefs.GetString("3DViewerModelPath"));
        a.transform.parent = RoomIDManager.instance.SceneObjectsParent.transform;
        FitToBoundsScript.instance.FitToBounds(a);
        Debug.Log(Camera.main.transform.position.x);
        Debug.Log("Asset Loaded"+Camera.main.name);
     
    }
   

}

      
      

      