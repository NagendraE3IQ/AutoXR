using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (BoxCollider))]

public class SnapDropZoneScript : MonoBehaviour
{           
    public GameObject snapmodel;
     MeshRenderer rend;
    bool isCubeDestroyed = false;
    public GameObject SFX;

    private void Start()
    {      
        rend = GetComponent<MeshRenderer>();
        rend.material = (Material)Resources.Load("ItemPickupOutline");
        this.GetComponent<BoxCollider>().size = new Vector3(0.75f, 0.75f, 0.75f);
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCubeDestroyed)
        {
            if(other.gameObject.name == snapmodel.name)
            {
                this.GetComponent<MeshRenderer>().material = snapmodel.GetComponent<MeshRenderer>().material;
                other.gameObject.transform.parent = null;
                Destroy(other.gameObject);
                isCubeDestroyed = true;
                SFX.GetComponent<AudioSource>().Play();
            }
        }
    }
}
