using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameSetup GS;

    public Transform[] spawnPoints;
    private void OnEnable()
    {
        if(GameSetup.GS==null)
        {
        GameSetup.GS = this;
        }

    }
}
