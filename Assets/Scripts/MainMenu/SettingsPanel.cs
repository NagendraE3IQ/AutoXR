using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    public Slider MusicSlider, SFXSlider, MouseSensitivity;
    public TextMeshProUGUI forward, back, left, right;

    Event keyEvent;
    TextMeshProUGUI buttonText;
    KeyCode newKey;
    bool waitingForKey;

    // Start is called before the first frame update
    void Start()
    {
        waitingForKey = false;

        forward.text = InputKeyManager.GM.forward.ToString();
        back.text = InputKeyManager.GM.backward.ToString();
        left.text = InputKeyManager.GM.left.ToString();
        right.text = InputKeyManager.GM.right.ToString();

        if(PlayerPrefs.HasKey("MusicVolume"))
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if(PlayerPrefs.HasKey("SFXVolume"))
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }


    void OnGUI()
    {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(TextMeshProUGUI text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey(); //Executes endlessly until user presses a key

        switch (keyName)
        {
            case "forward":
                InputKeyManager.GM.forward = newKey; //Set forward to new keycode
                buttonText.text = InputKeyManager.GM.forward.ToString(); //Set button text to new key
                PlayerPrefs.SetString("forwardKey", InputKeyManager.GM.forward.ToString()); //save new key to PlayerPrefs
                break;
            case "backward":
                InputKeyManager.GM.backward = newKey; //set backward to new keycode
                buttonText.text = InputKeyManager.GM.backward.ToString(); //set button text to new key
                PlayerPrefs.SetString("backwardKey", InputKeyManager.GM.backward.ToString()); //save new key to PlayerPrefs
                break;
            case "left":
                InputKeyManager.GM.left = newKey; //set left to new keycode
                buttonText.text = InputKeyManager.GM.left.ToString(); //set button text to new key
                PlayerPrefs.SetString("leftKey", InputKeyManager.GM.left.ToString()); //save new key to playerprefs
                break;
            case "right":
                InputKeyManager.GM.right = newKey; //set right to new keycode
                buttonText.text = InputKeyManager.GM.right.ToString(); //set button text to new key
                PlayerPrefs.SetString("rightKey", InputKeyManager.GM.right.ToString()); //save new key to playerprefs
                break;
        }

        yield return null;
    }


    public void SetQualitySettings(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void SetSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

    public void SetMouseSensitivity()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity.value);
    }
}
