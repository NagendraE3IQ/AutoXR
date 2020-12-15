using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLengthUpdateScript : MonoBehaviour
{
    public LineScript _LineScriptReference;

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(_LineScriptReference.lineLength);
       // transform.localScale = new Vector3(0.01f, _LineScriptReference.lineLength, 0.01f);
    }
}
