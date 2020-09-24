using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerHighlightOculus : MonoBehaviour
{
    public GameObject[] InstructionCanvasLinesOculus, InstructionCanvasLinesVive;
    public GameObject InstructionCanvas;
    public GameObject RightHand, LeftHand;
    public GameObject RightControllerModel;
    public GameObject LeftControllerModel;

    public Material HightLightMaterial;
    public Material CurrentMaterial;
    public AudioSource[] ButtonAudios;

    public GameObject[] RightButtons;
    public GameObject[] LeftButtons;

    public SteamVR_Action_Boolean RightTrigger;
    public SteamVR_Action_Boolean LeftTrigger;
    public SteamVR_Action_Boolean LeftGrip;
    public SteamVR_Action_Boolean RightGrip;
    public SteamVR_Action_Boolean LeftTrackpad;
    public SteamVR_Action_Boolean RightTrackpad;

    public SteamVR_Input_Sources RightHandInput;
    public SteamVR_Input_Sources LeftHandInput;

    public GameObject TrackpadTooltipLines;

    public AudioSource SFXAudiosource;

    int StepCount = 0;
    bool IsOculusDevice = true;

    public AudioSource[] OculusAndVive;
    void Awake()
    {
        string modelname = XRDevice.model;

        if (modelname.Contains("Oculus"))
        {
            IsOculusDevice = true;
        }
        else
        {
            IsOculusDevice = false;
        }

        if(IsOculusDevice)
        {
            ButtonAudios[0] = OculusAndVive[0];
            OculusAndVive[1].gameObject.SetActive(false);
        }
        else
        {
            ButtonAudios[0] = OculusAndVive[1];
            OculusAndVive[0].gameObject.SetActive(false);

        }
    }

    private void Start()
    {
        //Enable Controllers
        Invoke("ShowController", 3);
        Invoke("AnimateHandWithController", 3);

        Invoke("StartToolTip", 3.01f);
        InstructionCanvas.SetActive(true);

        //Adding Steam vr input actions
        RightTrigger.AddOnStateDownListener(RightTriggerEnable, RightHandInput);
        LeftTrigger.AddOnStateDownListener(LeftTriggerEnable, LeftHandInput);
        LeftGrip.AddOnStateDownListener(LeftGripEnable, LeftHandInput);
        RightGrip.AddOnStateDownListener(RightGripEnable, RightHandInput);
        LeftTrackpad.AddOnStateDownListener(LeftTrackPadClick, LeftHandInput);
        RightTrackpad.AddOnStateDownListener(RightTrackpadClick, RightHandInput);

        RightButtons = new GameObject[3];
        LeftButtons = new GameObject[3];
    }

    private void OnDisable()
    {
        RemoveListners();
    }

    private void OnDestroy()
    {
        RemoveListners();
    }

    void RemoveListners()
    {
        RightTrigger.RemoveOnStateDownListener(RightTriggerEnable, RightHandInput);
        LeftTrigger.RemoveOnStateDownListener(LeftTriggerEnable, LeftHandInput);
        LeftGrip.RemoveOnStateDownListener(LeftGripEnable, LeftHandInput);
        RightGrip.RemoveOnStateDownListener(RightGripEnable, RightHandInput);
        LeftTrackpad.RemoveOnStateDownListener(LeftTrackPadClick, LeftHandInput);
        RightTrackpad.RemoveOnStateDownListener(RightTrackpadClick, RightHandInput);
    }

    void StartToolTip()
    {
        if(IsOculusDevice)
        {
            InstructionCanvasLinesOculus[0].SetActive(true);
            InstructionCanvasLinesOculus[1].SetActive(true);
        }
        else
        {
            InstructionCanvasLinesVive[0].SetActive(true);
            InstructionCanvasLinesVive[1].SetActive(true);
        }
        
        ButtonAudios[StepCount].gameObject.SetActive(true);
        LeftButtons[StepCount].GetComponent<MeshRenderer>().material = HightLightMaterial;
        RightButtons[StepCount].GetComponent<MeshRenderer>().material = HightLightMaterial;
        SFXAudiosource.Play();
    }

    private void RightTrackpadClick(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 0)
        {
            OnTrackpadClick();
        }
    }

    private void LeftTrackPadClick(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 0)
        {
            OnTrackpadClick();
        }
    }

    void OnTrackpadClick()
    {
        StepCount++;
        if(IsOculusDevice)
        {
            InstructionCanvasLinesOculus[0].SetActive(false);
            InstructionCanvasLinesOculus[1].SetActive(false);
            InstructionCanvasLinesOculus[2].SetActive(true);
            InstructionCanvasLinesOculus[3].SetActive(true);
        }
        else
        {
            InstructionCanvasLinesVive[0].SetActive(false);
            InstructionCanvasLinesVive[1].SetActive(false);
            InstructionCanvasLinesVive[2].SetActive(true);
            InstructionCanvasLinesVive[3].SetActive(true);
        }
        
        SFXAudiosource.Play();
        ButtonAudios[StepCount].gameObject.SetActive(true);
        ButtonAudios[StepCount - 1].gameObject.SetActive(false);
        UpdateMaterials(CurrentMaterial, HightLightMaterial);
    }

    private void RightGripEnable(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 1)
        {
            OnGripBtnClick();
        }
    }

    private void LeftGripEnable(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 1)
        {
            OnGripBtnClick();
        }
    }

    void OnGripBtnClick()
    {
        StepCount++;
        if(IsOculusDevice)
        {
            InstructionCanvasLinesOculus[2].SetActive(false);
            InstructionCanvasLinesOculus[3].SetActive(false);
            InstructionCanvasLinesOculus[4].SetActive(true);
            InstructionCanvasLinesOculus[5].SetActive(true);
        }
        else
        {
            InstructionCanvasLinesVive[2].SetActive(false);
            InstructionCanvasLinesVive[3].SetActive(false);
            InstructionCanvasLinesVive[4].SetActive(true);
            InstructionCanvasLinesVive[5].SetActive(true);
        }
        
        SFXAudiosource.Play();

        ButtonAudios[StepCount].gameObject.SetActive(true);
        ButtonAudios[StepCount - 1].gameObject.SetActive(false);
        UpdateMaterials(CurrentMaterial, HightLightMaterial);
    }

    private void LeftTriggerEnable(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 2)
        {
            OnTiggerBtnClick();
        }
    }

    private void RightTriggerEnable(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (StepCount == 2)
        {
            OnTiggerBtnClick();
        }
    }

    void OnTiggerBtnClick()
    {
        StepCount++;
        if(IsOculusDevice)
        {
            InstructionCanvasLinesOculus[4].SetActive(false);
            InstructionCanvasLinesOculus[5].SetActive(false);
        }
        else
        {
            InstructionCanvasLinesVive[4].SetActive(false);
            InstructionCanvasLinesVive[5].SetActive(false);
        }
        
        SFXAudiosource.Play();

        ButtonAudios[StepCount - 1].gameObject.SetActive(false);
        RightButtons[StepCount - 1].GetComponent<MeshRenderer>().material = CurrentMaterial;
        LeftButtons[StepCount - 1].GetComponent<MeshRenderer>().material = CurrentMaterial;
        InstructionCanvas.SetActive(false);
        HideController();
        AnimateHandWithoutController();
        StepControllerScript.Instance.CallNextStep();
    }

    void UpdateMaterials(Material c, Material h)
    {
        RightButtons[StepCount - 1].GetComponent<MeshRenderer>().material = c;
        LeftButtons[StepCount - 1].GetComponent<MeshRenderer>().material = c;
        RightButtons[StepCount].GetComponent<MeshRenderer>().material = h;
        LeftButtons[StepCount].GetComponent<MeshRenderer>().material = h;
    }

     void HideController()
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

    void AnimateHandWithController()
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

    void AnimateHandWithoutController()
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


    void ShowController()
    {
        //TrackpadTooltipLines.SetActive(true);

        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Valve.VR.InteractionSystem.Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.ShowController(true);
            }
        }

        RightControllerModel = RightHand.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject;
        LeftControllerModel = LeftHand.transform.GetChild(6).gameObject.transform.GetChild(1).gameObject;
        if (IsOculusDevice)
        {
            //Trackpad
            RightButtons[0] = RightControllerModel.transform.GetChild(7).gameObject;
            LeftButtons[0] = LeftControllerModel.transform.GetChild(7).gameObject;
            //Grip Button
            RightButtons[1] = RightControllerModel.transform.GetChild(5).gameObject;
            LeftButtons[1] = LeftControllerModel.transform.GetChild(3).gameObject;
            //Trigger Button
            RightButtons[2] = RightControllerModel.transform.GetChild(9).gameObject;
            LeftButtons[2] = LeftControllerModel.transform.GetChild(9).gameObject;
        }
        else 
        {
            RightButtons[0] = RightControllerModel.transform.GetChild(13).gameObject;
            LeftButtons[0] = LeftControllerModel.transform.GetChild(13).gameObject;
            //Grip Button
            RightButtons[1] = RightControllerModel.transform.GetChild(7).gameObject;
            LeftButtons[1] = LeftControllerModel.transform.GetChild(8).gameObject;
            //Trigger Button
            RightButtons[2] = RightControllerModel.transform.GetChild(16).gameObject;
            LeftButtons[2] = LeftControllerModel.transform.GetChild(16).gameObject;
        }
    }
}
