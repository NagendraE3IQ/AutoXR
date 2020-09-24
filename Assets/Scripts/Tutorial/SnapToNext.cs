using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToNext : MonoBehaviour
{
    public void CallNext()
    {
        StepControllerScript.Instance.CallNextStep();
    }
}
