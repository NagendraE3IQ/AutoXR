using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    public RectTransform[] ImageScreens;
    public Button DontHaveAccount;
    Color currentcolour;
    public Color HighlightColor;
    int ScreenCount = 0;
    public GameObject SlideInDisplay, LoginSlide, SignUpSlide;
    int i, slidecount = 0;
    public bool isNotLogin;
    public GameObject MainLoginCanvas,landingpagecanvas;

    void Start()
    {
        if(!isNotLogin)
        {
        ImageScreens[0].transform.localPosition = SlideInDisplay.transform.position;
        }
    }

    public void LoginHighlight(bool IsHighlighted)
    {
        if (IsHighlighted)
        {
            currentcolour = DontHaveAccount.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color;
            DontHaveAccount.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color=HighlightColor;
        }
        else
        {
            DontHaveAccount.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color=currentcolour;
        }
    }
    public void StartLearning()
    {
        landingpagecanvas.SetActive(false);
        MainLoginCanvas.SetActive(true);
    }

   public void loginSlideIn()
    {
        iTween.MoveTo(LoginSlide, iTween.Hash("x", -73f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
        iTween.MoveTo(SignUpSlide, iTween.Hash("x", 71.8f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
    }

    public void SignUpSlideIn()
    {
        iTween.MoveTo(SignUpSlide, iTween.Hash("x", -73f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
        iTween.MoveTo(LoginSlide, iTween.Hash("x", -175f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
    }

    public void NextSlide()
    {   if(slidecount<2)
        {
            iTween.MoveTo(ImageScreens[slidecount+1].gameObject, iTween.Hash("position", SlideInDisplay.transform.position, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
            iTween.MoveTo(ImageScreens[slidecount].gameObject, iTween.Hash("x", -1100f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
            slidecount++;
        }
    }
    public void PreviousSlide()
    {
        if(slidecount>0)
        {
            iTween.MoveTo(ImageScreens[slidecount-1].gameObject, iTween.Hash("position", SlideInDisplay.transform.position, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
            iTween.MoveTo(ImageScreens[slidecount].gameObject, iTween.Hash("x", 1100f, "islocal", true, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0));
            slidecount--;
        }
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



