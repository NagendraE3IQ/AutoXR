using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookManager : MonoBehaviour
{  
   public  GameObject[] Cubes;
   public GameObject HeadPointer, Eye;
    public int LookCount = 0;

    public static HeadLookManager Instance;

    void Start()
    {
        Instance = this;

        Invoke("EnableCubes", 14);
    }

    void EnableCubes()
    {
        for(int i=0;i<Cubes.Length;i++)
        {
            Cubes[i].SetActive(true);
        }      
    }
}
