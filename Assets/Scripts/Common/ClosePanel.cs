using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject Panel;

    public void hidePanel()
    {
        Panel.gameObject.SetActive(false);
    }
        
}
