using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnFallenObj : MonoBehaviour
{
    Vector3 pos;

    // Start is called before the first frame update
    GameObject CurrentGO;
    private void Start()
    {
        pos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {      
        if(collision.gameObject.tag=="Floor")
        {
            Invoke("DestroyFallenObject", 3);
        }
    }

    public void DestroyFallenObject()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }
}
