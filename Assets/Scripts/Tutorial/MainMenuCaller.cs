using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MainMenuCaller : MonoBehaviour
{
    public GameObject RightHand, LeftHand;
    public GameObject[] RightButtons;
    public GameObject[] LeftButtons;
    public GameObject RightControllerModel;
    public GameObject LeftControllerModel;
    public SteamVR_Action_Boolean menubutton;
    public SteamVR_Input_Sources inputhand;
    public GameObject MenuPanel, PausePanel;
    public Material HightLightMaterial;
     Material CurrentMaterial;
    bool IsMenuPressed = false;

    void Start()
    {
        RightButtons = new GameObject[1];
        LeftButtons = new GameObject[1];
        Invoke("ShowController", 3);
        Invoke("AnimateHandWithController", 3);
        PausePanel.SetActive(false);
        menubutton.AddOnStateDownListener(CallMainMenu, inputhand);
    }

    private void OnDestroy()
    {
        menubutton.RemoveOnStateDownListener(CallMainMenu, inputhand);
    }

    private void OnDisable()
    {
        menubutton.RemoveOnStateDownListener(CallMainMenu, inputhand);
    }

    private void CallMainMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(IsMenuPressed)
        {
            IsMenuPressed = false;
            MenuPanel.SetActive(IsMenuPressed);
        }
        else
        {
            IsMenuPressed = true;
            MenuPanel.SetActive(IsMenuPressed);
        }
    }

    public  void mainmenu()
    {
        HideController();
        AnimateHandWithoutController();
        MenuPanel.SetActive(false);
        PausePanel.SetActive(true);
        StepControllerScript.Instance.OnSkipTutorialBtnClicked();
    }

    public void HideController()
    {
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Valve.VR.InteractionSystem.Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.HideController(true);
            }
        }
    }

    public void AnimateHandWithController()
    {
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Valve.VR.InteractionSystem.Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
            }
        }
    }

    public void AnimateHandWithoutController()
    {
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Valve.VR.InteractionSystem.Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
            }
        }
    }


    public void ShowController()
    {
        RightControllerModel = RightHand.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject;
        LeftControllerModel = LeftHand.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject;
        //Grip Button
        RightButtons[0] = RightControllerModel.transform.GetChild(2).gameObject;
        LeftButtons[0] = LeftControllerModel.transform.GetChild(2).gameObject;
        CurrentMaterial = RightButtons[0].GetComponent<MeshRenderer>().material;

        RightButtons[0].GetComponent<MeshRenderer>().material = HightLightMaterial;

        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Valve.VR.InteractionSystem.Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.ShowController(true);
            }
        }

    }
}
