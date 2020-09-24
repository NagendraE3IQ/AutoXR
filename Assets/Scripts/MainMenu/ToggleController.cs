using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleController : MonoBehaviour
{
    public bool isOn;
    public RectTransform toggle;
    public GameObject handle;
    public float handleOffset;
    public float speed;

    [SerializeField]
    TextMeshProUGUI PCtext, VRtext;

    private RectTransform handleTransform;
    private float handleSize;
    private float onPosX;
    private float offPosX;
    private bool switching = false;

    static float t = 0.0f;

    void Awake()
    {
        handleTransform = handle.GetComponent<RectTransform>();
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleSize = handleRect.sizeDelta.x;
        float toggleSizeX = toggle.sizeDelta.x;
        onPosX = (toggleSizeX / 2) - (handleSize / 2) - handleOffset;
        offPosX = onPosX * -1;
    }


    void Start()
    {
        if (isOn)
        {
            handleTransform.localPosition = new Vector3(onPosX, 0f, 0f);
            PCtext.color = Color.gray;
            VRtext.color = Color.white;
        }
        else
        {
            handleTransform.localPosition = new Vector3(offPosX, 0f, 0f);
            PCtext.color = Color.white;
            VRtext.color = Color.gray;
        }
    }

    void Update()
    {
        if (switching)
        {
            Toggle(isOn);
        }
    }

    public void DoYourStaff()
    {

    }

    public void Switching()
    {
        switching = true;
    }


    public void Toggle(bool toggleStatus)
    {
        if (toggleStatus)
        {
            handleTransform.localPosition = SmoothMove(handle, onPosX, offPosX);
        }
        else
        {
            handleTransform.localPosition = SmoothMove(handle, offPosX, onPosX);
        }
    }

    Vector3 SmoothMove(GameObject toggleHandle, float startPosX, float endPosX)
    {
        Vector3 position = new Vector3(Mathf.Lerp(startPosX, endPosX, t += speed * Time.deltaTime), 0f, 0f);
        StopSwitching();
        return position;
    }

    void StopSwitching()
    {
        if (t > 1.0f)
        {
            switching = false;

            t = 0.0f;
            switch (isOn)
            {
                case true:
                    isOn = false;
                    PCtext.color = Color.white;
                    VRtext.color = Color.gray;
                    DoYourStaff();
                    break;

                case false:
                    isOn = true;
                    PCtext.color = Color.gray;
                    VRtext.color = Color.white;
                    DoYourStaff();
                    break;
            }
        }
    }
}
