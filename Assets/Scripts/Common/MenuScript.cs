using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;

public class MenuScript : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean MenuAction;// a reference to the hand
    public SteamVR_Input_Sources handType;//reference to the sphere
    public bool IsPaused = false;

    public GameObject pauseMenuUI, PausePanelOnTab;

    public Material renderSkybox;
    void Start()
    {
        MenuAction.AddOnStateDownListener(TriggerDown, handType);
    }

    private void OnDisable()
    {
        MenuAction.RemoveOnStateDownListener(TriggerDown, handType);
    }

    private void OnDestroy()
    {
        MenuAction.RemoveOnStateDownListener(TriggerDown, handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        PausePanelOnTab.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        AudioListener.pause = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        PausePanelOnTab.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        AudioListener.pause = true;
    }

    public void Reload(string scenename)
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.UnloadSceneAsync(scenename);
        SceneManager.LoadScene(scenename, LoadSceneMode.Additive);
    }

    public void OnMainMenuBtnClicked(string scenename)
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        RenderSettings.skybox = renderSkybox;
        this.StartCoroutine(this.LoadVisor("", false, scenename));
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
            GameObject camera = Instantiate(g, transform.position, transform.rotation);
            camera.transform.SetParent(MainMenuManagerScript.Instance.SceneObjects.transform);
            Destroy(GameObject.Find("[SteamVR]"));
            SceneManager.UnloadSceneAsync(scenename);
        }
    }
}