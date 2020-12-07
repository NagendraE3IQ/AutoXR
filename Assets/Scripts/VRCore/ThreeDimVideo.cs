using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class ThreeDimVideo : MonoBehaviour
{
    public MenuScript PauseScript;

    VideoPlayer vp;
    public Material renderSkybox;
    public static ThreeDimVideo Instance;

    private void Awake()
    {
        Instance = this;
        RenderSettings.skybox = renderSkybox;
        vp = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        VideoClip v = (VideoClip)Resources.Load(PlayerPrefs.GetString("ThreeDimVideo"));
        vp.clip = v;
    }

    // Update is called once per frame
    void Update()
    {
       if(PauseScript.IsPaused)
        {
            vp.Pause();
        }
       else
        {
            vp.Play();
        }
    }
}
