using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using TriLib;
using UnityEngine.XR;
using Valve.VR;

public class PhotonRoomManagerScript : MonoBehaviourPunCallbacks
{

    //public byte MaxPlayers;
    [HideInInspector]
   // public string PlayerName;
    public GameObject JoinButton;
    public GameObject leaveRoom;
    bool isTrainer = false;
    public bool isjoined = false;
    GameObject LoadedGameObject;
    public TextMeshProUGUI xIncoming;
    public TextMeshProUGUI yIncoming;
    public TextMeshProUGUI zincoming;
    public GameObject alpha;
    public Material SceneSkybox;
    public Material ExitSkybox;

    void Start()
    {
        RenderSettings.skybox = SceneSkybox;

        //  PhotonNetwork.ConnectUsingSettings();
        if (RoomIDManager.instance.isCreate)
        {
        _CreateRoom();
        }
        else
        {
            _JoinRoom();
        }
    }

    public void _CreateRoom()
    {
        Debug.Log("Trying to create room.");
        RoomOptions options = new RoomOptions();
        options.IsVisible = true;
        options.MaxPlayers = RoomIDManager.instance._maxPlayers;
        PhotonNetwork.CreateRoom(RoomIDManager.instance.RoomID, options, typedLobby: default);
      
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room!!");
        _CreateRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Creation Successful");
        isTrainer = true;
        PhotonNetwork.LocalPlayer.NickName = RoomIDManager.instance.PlayerName;
        base.OnCreatedRoom();
    }


    public void _JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomIDManager.instance.RoomID);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = RoomIDManager.instance.PlayerName;
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        //change with round robbin
        GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PhotonNetworkPlayer"), GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
        AssetLoader _assetLoader = new AssetLoader();
        LoadedGameObject = _assetLoader.LoadFromFile(PlayerPrefs.GetString("3DViewerModelPath"));
        //testing
        LoadedGameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        if(LoadedGameObject.GetComponent<Animation>())
        {
            LoadedGameObject.GetComponent<Animation>().enabled = false;
        }
        LoadedGameObject.transform.parent = alpha.transform;
        LoadedGameObject.AddComponent<PhotonView>();
        //change to round robbin
        LoadedGameObject.GetComponent<PhotonView>().ViewID = Random.Range(1, 9999);

       // LoadedGameObject.AddComponent<AssetTransformSyncManager>();
       // Debug.Log("assigned");
      //  Debug.Log(GetComponent<AssetTransformSyncManager>());

        InteractionPanelManager.instance.Model = alpha;
        if (isTrainer)
        {
            a.tag = "Trainer";
        }
        else
        {
            a.tag = "Trainee";
            isjoined = true;
            // float[] x = { 1.0f, 2.0f, 3.0f };

        }
        leaveRoom.SetActive(true);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join" + message);
    }

    public void LeaveRoom()
    {
        Debug.Log("Leaving Room.....");
        PhotonNetwork.LeaveRoom();  
    }
    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene("RoomUISceneDupe");
        StartCoroutine(LoadVisor("", false));
        GameObject a = (GameObject)Resources.Load("MainCamera");
        a.GetComponent<FitToBoundsScript>().ShouldFitToBounds = true;
       GameObject b = Instantiate(a, new Vector3(0, 0, 0),  Quaternion.Euler(0, 0, 0));
        b.GetComponent<SteamVR_LoadLevel>().levelName = "MultiplayertestRoomID";
        b.transform.parent = RoomIDManager.instance.SceneObjectsParent.transform;
        RoomIDManager.instance.SceneObjectsParent.transform.localScale = new Vector3(1, 1, 1);
        RoomIDManager.instance.UiCanvas.GetComponent<Canvas>().worldCamera = b.GetComponent<Camera>();

    }
    private IEnumerator LoadVisor(string StringVisor, bool BoolEnable)
    {
        XRSettings.LoadDeviceByName(StringVisor);
        yield return null;

        if (BoolEnable == true)
        {
            XRSettings.enabled = true;

            InputTracking.disablePositionalTracking = false;
            InputTracking.Recenter();
            SceneManager.LoadScene("MultiplayertestRoomID");
        }
        else
        {
            XRSettings.enabled = false;
            SceneManager.UnloadSceneAsync("MultiplayertestRoomID");
            RenderSettings.skybox = ExitSkybox;
            //SceneManager.LoadScene("RoomUISceneDupe");

        }
    }
}
        


