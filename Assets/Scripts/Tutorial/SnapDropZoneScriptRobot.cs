using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (BoxCollider))]

public class SnapDropZoneScriptRobot : MonoBehaviour
{
    public GameObject snapmodel;
     MeshRenderer[] rend;
    MeshRenderer[] ModelRend;
    GameObject goroot;
    public static int robotPartCount;
    public GameObject SFX;

    private void Start()
    {
        rend = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].material = (Material)Resources.Load("ItemPickupOutline");
            this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            this.GetComponent<BoxCollider>().isTrigger = true;
        }

      
            ModelRend= snapmodel.GetComponentsInChildren<MeshRenderer>(includeInactive:true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Snappable" && other.gameObject.name==snapmodel.name)
        {
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].material = ModelRend[i].material;
            }
            other.gameObject.transform.parent = null;
            SFX.GetComponent<AudioSource>().Play();
            robotPartCount += 1;

            if (robotPartCount >= 3)
            {
                this.transform.parent.GetComponent<Animator>().Play("RobotBody");
                this.GetComponent<AudioSource>().Play();
                robotPartCount = 0;
            }
            //enabled = false;
            Destroy(this);
            Destroy(other.gameObject);
        }     
    }
}
