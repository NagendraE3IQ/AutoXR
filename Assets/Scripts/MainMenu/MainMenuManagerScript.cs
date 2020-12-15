using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR;
using Valve.VR;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject HideMenuPanel;
    public GameObject[] MenuBarButtons;
    public GameObject[] SettingsPanelMasterButtons;
    public GameObject[] VideoSettingsButtons;
    public GameObject[] MenuSlides;

    public TextMeshProUGUI TopHeaderUserName;
    public GameObject SceneObjects;
    public GameObject MainMenuUICanvas;


    Sprite ButtonHighlightImage, CurrentSettingsTabSprite, HighlightSettingsTabSprite;

    [Header("PROFILE")]
    public static MainMenuManagerScript Instance;

    void Start()
    {
        Instance = this;
        TopHeaderUserName.text = UserData.Instance.UserDocument["name"].ToString();

        HighlightSettingsTabSprite = SettingsPanelMasterButtons[0].GetComponent<Image>().sprite;
        CurrentSettingsTabSprite = SettingsPanelMasterButtons[1].GetComponent<Image>().sprite;
        ButtonHighlightImage = MenuBarButtons[0].GetComponent<Image>().sprite;
    }

    public void RevealMenu()
    {
        iTween.MoveTo(HideMenuPanel, iTween.Hash("x", 91.6f, "islocal", true, "easeType", "none", "loopType", "none", "delay", 0));
    }
    public void HideMenu()
    {
        iTween.MoveTo(HideMenuPanel, iTween.Hash("x", 28f, "islocal", true, "easeType", "none", "loopType", "none", "delay", 0));
    }

    public void ButtonClicked(GameObject ClickedButton)
    {
        for (int i = 0; i < MenuBarButtons.Length; i++)
        {
            MenuBarButtons[i].GetComponent<Image>().sprite = null;
            MenuBarButtons[i].GetComponent<ButtonMenuAttached>().Menuslide.GetComponent<RectTransform>().localPosition = new Vector3(0, 2000, 0);
        }
        ClickedButton.GetComponent<Image>().sprite = ButtonHighlightImage;
        ClickedButton.GetComponent<ButtonMenuAttached>().Menuslide.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
    }

    public void GetModuleButtonClicked(GameObject ButtonName)
    {
        for (int i = 0; i < MenuBarButtons.Length; i++)
        {
            MenuBarButtons[i].GetComponent<ButtonMenuAttached>().Menuslide.SetActive(false);
        }
    }

    public void StartLearningClicked()
    {
        this.StartCoroutine(this.LoadVisor("OpenVR", true, "Tutorial"));
        GameObject g = Camera.main.gameObject;
        g.GetComponent<SteamVR_LoadLevel>().levelName = "Tutorial";
        g.GetComponent<SteamVR_LoadLevel>().enabled = true;
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
            SceneObjects.transform.localScale = new Vector3(0,0,0);
        }
        else
        {
            XRSettings.enabled = false;
            SceneManager.LoadScene(scenename);
        }
    }

    public void SettingsButtonSpriteChange(GameObject ButtonGO)
    {
        for (int i = 0; i < SettingsPanelMasterButtons.Length; i++)
        {
            SettingsPanelMasterButtons[i].GetComponent<Button>().interactable = true;
            SettingsPanelMasterButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        ButtonGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        ButtonGO.GetComponent<Button>().interactable = false;
    }


    public void VideoButtonSettingsSpriteChange(GameObject ButtonName)
    {
        for (int i = 0; i < VideoSettingsButtons.Length; i++)
        {
            if (VideoSettingsButtons[i].transform.parent == ButtonName.transform.parent)
            {
                //VideoSettingsButtons[i].GetComponent<Image>().sprite = CurrentSettingsTabSprite;
                VideoSettingsButtons[i].GetComponent<Button>().interactable = true;
                VideoSettingsButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            }
        }
        ButtonName.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        //ButtonName.GetComponent<Image>().sprite = HighlightSettingsTabSprite;
        ButtonName.GetComponent<Button>().interactable = false;
    }

    //Exit Page
    public void OnExitYesBtnClick()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OnExitNoBtnClick(GameObject ClickedButton)
    {
        for (int i = 0; i < MenuBarButtons.Length; i++)
        {
            MenuBarButtons[i].GetComponent<Image>().sprite = null;
            MenuBarButtons[i].GetComponent<ButtonMenuAttached>().Menuslide.GetComponent<RectTransform>().localPosition = new Vector3(0, 2000, 0);
        }
        ClickedButton.GetComponent<Image>().sprite = ButtonHighlightImage;
        ClickedButton.GetComponent<ButtonMenuAttached>().Menuslide.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    public void ButtonSpriteSlideIn(GameObject ButtonGO)
    {
        if (ButtonGO != null)
        {
            Transform LoginSlidePosition = ButtonGO.GetComponent<GetSlideInPosition>().loginSlideInPosition.transform;
            iTween.MoveTo(ButtonGO, iTween.Hash("x", LoginSlidePosition.localPosition.x, "islocal", true, "easeType", "none", "loopType", "none", "delay", 0));
            ButtonGO = null;
        }
    }

    public void ButtonSpriteSlideOut(GameObject ButtonGO)
    {
        if (ButtonGO != null)
        {
            iTween.MoveTo(ButtonGO, iTween.Hash("x", 225.7f, "islocal", true, "easeType", "none", "loopType", "none", "delay", 0));
            ButtonGO = null;
        }
    }
}


