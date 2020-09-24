using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class ThreeDimVideo : MonoBehaviour
{
    //public RenderTexture rt;

    public MenuScript PauseScript;
    public VideoClip[] videoclips;
    //public AudioClip[] audioclips;

    VideoPlayer vp;
    //AudioSource Audsource;
    //int v;
    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        VideoClip v = (VideoClip)Resources.Load("ConstructionVideo");
        vp.clip = v;
    }

    // Start is called before the first frame update
    void Start()
    {
        //vp = GetComponent<VideoPlayer>();
        //Audsource = GetComponent<AudioSource>();

        //v = Random.Range(0, videoclips.Length);
        //vp.clip = videoclips[0];

        //if (audioclips[0])
        //Audsource.clip = audioclips[0];
        //else
        //Audsource.clip = null;

        //rt = new RenderTexture(1024, 1024, 2);
        //vp.targetTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {
       if(PauseScript.IsPaused)
        {
            vp.Pause();
            //Audsource.Pause();
        }
       else
        {
            vp.Play();
            //Audsource.Play();
        }
    }
}
