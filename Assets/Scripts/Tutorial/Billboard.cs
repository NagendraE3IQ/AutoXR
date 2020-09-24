using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform headset;

    // Start is called before the first frame update
    void Start()
    {
        headset = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + headset.transform.rotation * Vector3.forward,
            headset.transform.rotation * Vector3.up);
    }
}
