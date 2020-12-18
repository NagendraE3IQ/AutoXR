using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StickyNoteAttachedinformationManager : MonoBehaviour
{
    public TextMeshProUGUI PartNameText;
    Material CurrentPartMaterial;
    public Material HighlightMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hand"))
        {
        CurrentPartMaterial = other.gameObject.GetComponent<MeshRenderer>().material;
        other.gameObject.GetComponent<MeshRenderer>().material = HighlightMaterial;
        PartNameText.text = other.gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Hand"))
        {
        //CurrentPartMaterial = other.gameObject.GetComponent<MeshRenderer>().material;
        other.gameObject.GetComponent<MeshRenderer>().material = CurrentPartMaterial;
        PartNameText.text = "";
        }
    }
}

