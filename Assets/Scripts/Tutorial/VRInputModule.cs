using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;


public class VRInputModule : BaseInputModule
{
    public Camera cam;
    public SteamVR_Input_Sources TargetSource;
    public SteamVR_Action_Boolean ClickAction;
    public GameObject SelectedGameObject = null;

    public GameObject CurrentGameObject = null;
    private PointerEventData pointerdata = null;
   

    protected override void Awake()
    {
        base.Awake();

        pointerdata = new PointerEventData(eventSystem);       
    }

    public override void Process()
    {
        // Reset data, set camera

        pointerdata.Reset();
        pointerdata.position = new Vector2(cam.pixelWidth/2, cam.pixelHeight/2);

        // RayCast

        eventSystem.RaycastAll(pointerdata, m_RaycastResultCache);
        
        pointerdata.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        CurrentGameObject = pointerdata.pointerCurrentRaycast.gameObject;

        
        // Clear

        m_RaycastResultCache.Clear();

        // Hover
        
        HandlePointerExitAndEnter(pointerdata, CurrentGameObject);

        // Press

        if (ClickAction.GetStateDown(TargetSource))
        {
            ProcessPress(pointerdata);
        }

        //Release

        if (ClickAction.GetStateUp(TargetSource))
        {
            ProcessRelease(pointerdata);
        }

    }

    public PointerEventData GetData()
    {
        return pointerdata;
    }

    private void ProcessPress(PointerEventData data)
    {
        // Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast; // Storing the raycast information in the current pointerpress event data

        // Checking for object hit, get the down handler and call it
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(CurrentGameObject, data, ExecuteEvents.pointerDownHandler); // 

        // If no down handler, try and get click handler
        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentGameObject);

        // Set Data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = CurrentGameObject;
        SelectedGameObject = CurrentGameObject;

    }

    private void ProcessRelease(PointerEventData data)
    {
        // Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // Checking for a click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentGameObject);

        // Check if actual 
        if (data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        // Clear selected game object
        eventSystem.SetSelectedGameObject(null);

        // Reset Data

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
        SelectedGameObject = null;
    }
}
