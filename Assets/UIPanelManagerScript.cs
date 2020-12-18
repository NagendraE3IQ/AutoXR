using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPanelManagerScript : MonoBehaviour
{
    public Color DefaultButtontextColor;
    public Color HighlightButtontextColor;

    public void OnHoverEnterButtonColorChange(TextMeshProUGUI CurrentText)
    {
        CurrentText.color = HighlightButtontextColor;
    } 
    
    public void OnHoverExitButtonColorChange(TextMeshProUGUI CurrentText)
    {
        CurrentText.color = DefaultButtontextColor;
    }

  
}
