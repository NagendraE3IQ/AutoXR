using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;
//using System.Globalization;
using System;
using UnityEngine.XR;
using Valve.VR;
public class RoomIDManager : MonoBehaviourPunCallbacks
{
    public static RoomIDManager instance;
    bool IsRoomIDGenerated = false;
    public string RoomID;
    public string PlayerName;
    List<RoomInfo> CurrentRoomIDs;
    public byte _maxPlayers;
    public TMP_InputField _MaxPlayers;
    public TextMeshProUGUI _CreateRoomID;
    public TMP_InputField _CreatePlayerName;
    public TMP_InputField _JoinRoomID;
    public TMP_InputField _JoinPlayerName;
    public bool isCreate = false;
    public GameObject SceneObjectsParent;
    public GameObject UiCanvas;


    private void Awake()
    {
        instance = this;
       // GameObject.Find("PhotonMono").GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = false ;
       // GameObject.Find("PhotonMono").GetComponent<PhotonHandler>().KeepAliveInBackground = 0 ;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("3DviewerMenuScene"));
       
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!");
        base.OnJoinedLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("From RoomList Update");
        CurrentRoomIDs = roomList;
    }

    public void GenerateRoomID()
    {
        Debug.Log(PhotonNetwork.IsConnectedAndReady);
       // Debug.Log();
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("No Rooms Found... Creating Room");
            RoomID = UnityEngine.Random.Range(100000, 999999).ToString();
            Debug.Log(RoomID);
            _CreateRoomID.text = RoomID;
            return;
        }

        else
        {

            RoomID = UnityEngine.Random.Range(100000, 999999).ToString();
            foreach (RoomInfo r in CurrentRoomIDs)
            {
                if (r.Name != RoomID)
                {
                    IsRoomIDGenerated = true;
                    break;
                }
                else
                {
                    GenerateRoomID();
                    
                }
            }
            _CreateRoomID.text = RoomID;

        }


    }

    public void OnCreateRoomClicked()
    {
        PlayerName = _CreatePlayerName.text;
        string a = _MaxPlayers.text;
        int z = int.Parse(a);
        _maxPlayers = (byte)z;
        isCreate = true;
        StartCoroutine(LoadVisor("OpenVR", true));
       }

    public void OnJoinRoomClicked()
    {
        isCreate = false;
        PlayerName = _JoinPlayerName.text;
        RoomID = _JoinRoomID.text;
        StartCoroutine(LoadVisor("OpenVR", true));
    }

    private IEnumerator LoadVisor(string StringVisor, bool BoolEnable)
    {
        Debug.Log("Open vr called!");
        XRSettings.LoadDeviceByName(StringVisor);
        yield return null;

        if (BoolEnable == true)
        {
            XRSettings.enabled = true;

            InputTracking.disablePositionalTracking = false;
            InputTracking.Recenter();
            //  SceneManager.LoadScene("MultiplayertestRoomID",LoadSceneMode.Additive);
            Camera temp = Camera.main;
            SceneObjectsParent.transform.localScale = new Vector3(0, 0, 0);
         
            temp.GetComponent<SteamVR_LoadLevel>().enabled = true;
           
        }
        else
        {
            XRSettings.enabled = false;
        }
    }

    public void OnClickMainMenu()
    {
        SceneManager.UnloadSceneAsync("3DviewerMenuScene");
        GameObject a = (GameObject)Resources.Load("MainCamera");
        GameObject b = Instantiate(a, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        MainMenuManagerScript.Instance.SceneObjects.transform.localScale = new Vector3(1, 1, 1);
        b.transform.parent = MainMenuManagerScript.Instance.SceneObjects.transform;
        MainMenuManagerScript.Instance.MainMenuUICanvas.GetComponent<Canvas>().worldCamera = b.GetComponent<Camera>();
        PhotonNetwork.Disconnect();
        
        
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Leaving Lobby....switching to main menu");
    }
        

}


