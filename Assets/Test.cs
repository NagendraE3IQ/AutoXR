using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Test : MonoBehaviour
{
    void Start()
    {
        //Debug.Log("Testing connection for devices");
        //SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);

       
            //lets figure what type of device got connected
            ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)1);
            if (deviceClass == ETrackedDeviceClass.HMD)
            {
                Debug.Log("Controller got connected at index:" + 1);
            }
            else if (deviceClass == ETrackedDeviceClass.GenericTracker)
            {
                Debug.Log("Tracker got connected at index:" + 1);
            }
}

    // A SteamVR device got connected/disconnected
    private void OnDeviceConnected(int index, bool connected)
    {

        if (connected)
        {
            if (OpenVR.System != null)
            {
                //lets figure what type of device got connected
                ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)index);
                if (deviceClass == ETrackedDeviceClass.HMD)
                {
                    Debug.Log("Controller got connected at index:" + index);
                }
                else if (deviceClass == ETrackedDeviceClass.GenericTracker)
                {
                    Debug.Log("Tracker got connected at index:" + index);
                }
            }
        }
    }
}
