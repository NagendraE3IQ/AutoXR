using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractionPanelManager : MonoBehaviour
{
    public static InteractionPanelManager instance;

    public GameObject Model;
    public LinearMapping XLMReference;
    public LinearMapping YLMReference;
    public LinearMapping ZLMReference;
    public LinearMapping ScaleLMReference;
    public float RotationSpeed=5f;
    float XPreviousValue, YPreviousValue, ZPreviousValue,ScalePreviousValue;
    Transform StartTransform;

    private void Start()
    {
        instance = this;
    }
    private void OnEnable()
    {
      //  Model = AssetLoaderManager.Instance.LoadedModel;

       // Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        XPreviousValue = XLMReference.value;
        YPreviousValue = YLMReference.value;
        ZPreviousValue = ZLMReference.value;
        ScalePreviousValue = ScaleLMReference.value;
       // StartTransform.position = Model.transform.position;
       // StartTransform.rotation = Model.transform.rotation;
       // StartTransform.localScale = Model.transform.localScale;
        
    
    }
    

    public void ResetAssetTransform()
    {
        Model.transform.position = new Vector3(0, 0, 0);
        Model.transform.rotation =  Quaternion.Euler(0, 0, 0);
        Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }


    //public void OnLoadDiffModel()
    //{
    //    Model = AssetLoaderManager.Instance.LoadedModel;
    //    Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    //}

    private void Update()
    {
        if(XLMReference.value-XPreviousValue>= 0.0125)
        {
           // Debug.Log(XLMReference.value);
            float delta = XLMReference.value - XPreviousValue;
            Model.transform.Rotate(Vector3.right * delta * 360);
            XPreviousValue = XLMReference.value;
            //increase x
        }
        else if (XLMReference.value - XPreviousValue <= -0.0125)
        {
            //Debug.Log(XLMReference.value);
            float delta = XLMReference.value - XPreviousValue;
            Model.transform.Rotate(Vector3.right * delta * 360);
            XPreviousValue = XLMReference.value;
            //decrease x
        }
        else if (YLMReference.value - YPreviousValue >= 0.0125)
        {
            Debug.Log(YLMReference.value);
            float delta = YLMReference.value - YPreviousValue;
            Model.transform.Rotate(Vector3.up * delta * 360);
            YPreviousValue = YLMReference.value;
            //increase Y
        }
        else if (YLMReference.value - YPreviousValue <= -0.0125)
        {
            //Debug.Log(YLMReference.value);
            float delta = YLMReference.value - YPreviousValue;
            Model.transform.Rotate(Vector3.up * delta * 360);
            YPreviousValue = YLMReference.value;

            //decrease Y
        }
        else if (ZLMReference.value - ZPreviousValue >= 0.0125)
        {
            //Debug.Log(ZLMReference.value);
            float delta = ZLMReference.value - ZPreviousValue;
            Model.transform.Rotate(Vector3.forward * delta * 360);
            ZPreviousValue = ZLMReference.value;

            //increase z
        }
        else if (ZLMReference.value - ZPreviousValue <= -0.0125)
        {
            //Debug.Log(ZLMReference.value);
            float delta = ZLMReference.value - ZPreviousValue;
            Model.transform.Rotate(Vector3.forward * delta * 360);
            ZPreviousValue = ZLMReference.value;
            //decrease z
        }
        else if (ScaleLMReference.value - ScalePreviousValue >= 0.0125)
        {
           // Debug.Log(ScaleLMReference.value);
            float delta = ScaleLMReference.value - ScalePreviousValue;
            Model.transform.localScale += new Vector3(delta, delta, delta);
            ScalePreviousValue = ScaleLMReference.value;

            //increase z
        }
        else if (ScaleLMReference.value - ScalePreviousValue <= -0.0125)
        {
           // Debug.Log(ScaleLMReference.value);
            float delta = ScaleLMReference.value - ScalePreviousValue;
            Model.transform.localScale += new Vector3(delta, delta, delta);
            ScalePreviousValue = ScaleLMReference.value;
            //decrease z
        }
    }


}
