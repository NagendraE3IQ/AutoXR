using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caster : MonoBehaviour
{
    LineScriptTutorial LS;
    private void Start()
    {
        LS = GetComponent<LineScriptTutorial>();
    }
    void Update()
    {
        if (LS.gameobj != null)
        {
            if (LS.gameobj.CompareTag("Destroyable"))
            {
                LS.gameobj.GetComponent<Animator>().Play("rotate");
            }
        }

    }
}
