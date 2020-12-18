using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR;
public class RoomInfoManagerScript : MonoBehaviour
{
    public static RoomInfoManagerScript Instance;
    [HideInInspector]
    public string _roomName;
    public TextMeshProUGUI CreateRoomName;
    public TextMeshProUGUI JoinRoomname;
    public bool isCreate = true;

    public string AssetDowloadURI="";

    private void Start()
    {
        Instance = this;
    }

    public void CreateRoomClicked()
    {
        _roomName = CreateRoomName.text;
        StartCoroutine(LoadVisor("OpenVR", true));
        //SceneManager.LoadScene("MultiplayertestRoomID");
    }
    public void JoinRoomClicked()
    {
        isCreate = false;
        _roomName = JoinRoomname.text;
        StartCoroutine(LoadVisor("OpenVR", true));
       
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
        }
    }


}
