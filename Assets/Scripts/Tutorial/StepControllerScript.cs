using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class StepControllerScript : MonoBehaviour
    {
    public GameObject teleportingGO;
    bool flag = false;
    public int StepCount = 0;
    [SerializeField]
    public GameObject[] StepList;
    [SerializeField]
    AudioClip[] StepAudios;
      
    public LineScript Lscript;
    public LineScriptTutorial LTScript;

    AudioSource AudSource;
    public static StepControllerScript Instance;

    private void Awake()
    { 
        Component[] VRInModule;

        VRInModule = GetComponents(typeof(VRInputModule));

        Lscript.InputModule = VRInModule[0] as VRInputModule;
        LTScript.InputModule = VRInModule[1] as VRInputModule;
    }

    void Start()
    {           
      Instance = this;
        AudSource = GetComponent<AudioSource>();

    }

    //bool flag = false;
    public void CallNextStep()
    {
        if (StepCount < StepList.Length)
        {
            StepList[StepCount + 1].SetActive(true);
            AudSource.clip = StepAudios[StepCount + 1];
            AudSource.Play();
            if (StepCount <= 1)
            {
                StepList[StepCount].SetActive(false);
            }
        }
       
        StepCount++;
    }

    private void Update()
    {
        if(StepCount==3 && !flag)
        {
            teleportingGO.SetActive(true);
            flag = true;
        }
    }

    public void OnSkipTutorialBtnClicked()
    {
        if(Time.timeScale != 0)
        {
            this.StartCoroutine(this.LoadVisor("", false, "Tutorial"));
        }
    }

    private IEnumerator LoadVisor(string StringVisor, bool BoolEnable, string scenename)
    {
        XRSettings.LoadDeviceByName(StringVisor);

        yield return null;

        if (BoolEnable == true)
        {
            XRSettings.enabled = true;

            InputTracking.disablePositionalTracking = false;
            InputTracking.Recenter();
            SceneManager.UnloadSceneAsync(scenename);
        }
        else
        {
            XRSettings.enabled = false;
            MainMenuManagerScript.Instance.SceneObjects.transform.localScale = new Vector3(1, 1, 1);
            GameObject g = Resources.Load("MainCamera") as GameObject;
            GameObject camera  = Instantiate(g, transform.position, transform.rotation);
            camera.transform.SetParent(MainMenuManagerScript.Instance.SceneObjects.transform);
            Destroy(GameObject.Find("[SteamVR]"));
            SceneManager.UnloadSceneAsync(scenename);
        }
    }
}
